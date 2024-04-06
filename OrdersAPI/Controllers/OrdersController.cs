using App.Data;
using App.Data.Models;
using App.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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

        // GET api/<OrdersController>/6610c2d857538fcb2f444f47
        [HttpGet("{orderId}")]
        [Authorize]
        public GetOrderDto Get(string orderId)
        {
            // We could do more here with validation, I just checked to see if it could be formatted as an Object Id to handle an exception
            // down the line if it fails to convert.  If it is not a valid object id, they didnt get it from us, so return null seems good
            // enough for my time constraints.  Could result in a "false" 200 response, but need requirements to do anything useful with this scenario.
            bool idIsValid = ObjectId.TryParse(orderId, out var _);

            if(!idIsValid)
            {
                return null;
            }

            //Pull user from auth layer, and never the request data
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Order orderToReturn = _orderRepository.GetOrderById(orderId, userId);
            GetOrderDto dtoToReturn = _mapper.Map<Order, GetOrderDto>(orderToReturn);
            return dtoToReturn;

        }

        // POST api/<OrdersController>
        [HttpPost]
        [Authorize]
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
        [HttpPut("{orderId}")]
        [Authorize]
        public void Put(string orderId, [FromBody] IEnumerable<OrderItem> items)
        {
            if (!items.Any()) {
                throw new InvalidDataException("Items cannot be empty for an update, delete the order instead.");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _orderRepository.Update(orderId, userId, items);
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
