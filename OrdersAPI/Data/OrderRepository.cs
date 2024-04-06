using App.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Data
{
    public class OrderRepository : IOrderRepository
    {
        private IOrderContext _orderContext;

        public OrderRepository(IOrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public void Create(Order order)
        {
           _orderContext.Orders.InsertOne(order);                                                      
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAllOrders(string userId)
        {
            return _orderContext.Orders.Find(o => o.User.Equals(userId)).ToList();
        }

        public Order GetOrderById(string id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
