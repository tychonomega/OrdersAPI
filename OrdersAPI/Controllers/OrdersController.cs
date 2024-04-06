using App.Data;
using App.Data.Models;
using App.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

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
            IEnumerable<Order> ordersToReturn = _orderRepository.GetAllOrders();

            
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
