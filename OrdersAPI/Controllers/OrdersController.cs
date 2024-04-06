using App.Data;
using App.Data.Models;
using App.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        [Authorize]
        public IEnumerable<GetOrderDto> Get()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<Order> ordersToReturn = _orderRepository.GetAllOrders(userId);
                        
            return _mapper.Map<IEnumerable<Order>, IEnumerable<GetOrderDto>>(ordersToReturn);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrdersController>
        [HttpPost]
        public void Post([FromBody][Required] PostOrderDto order)
        {
            Order orderToCreate = _mapper.Map<Order>(order);

            // This would be improved in a production with an entire model/persistance for User data.
            // For simplicity in this exercise, just scrape the claim and use that as the "unique" user identifier.
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            orderToCreate.User = userId;

            _orderRepository.Create(orderToCreate);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
