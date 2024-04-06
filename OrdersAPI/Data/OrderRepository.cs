using App.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;

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

        public Order GetOrderById(string orderId, string userId)
        {
            // ID is coming from the database in this instance, and is a required field, o.Id cannot be null
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return _orderContext.Orders.Find(o => o.User.Equals(userId) && o.Id.Equals(orderId)).SingleOrDefault();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public bool Update(string orderId, string userId, IEnumerable<OrderItem> items)
        {
            // Creates a filter for the Order to update by id
            var filter = Builders<Order>.Filter
                .Eq(o => o.Id, orderId);
            // what to update
            var update = Builders<Order>.Update
                .Set(o => o.Items, items)
                .Set(o => o.Updated, DateTime.Now);


            UpdateResult updateResult = _orderContext.Orders.UpdateOne(filter, update);
            if(updateResult != null && updateResult.IsAcknowledged)
            {
                return true;
            }


            return false;
        }
    }
}
