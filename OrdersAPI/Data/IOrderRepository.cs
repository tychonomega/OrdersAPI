using App.Data.Models;
using MongoDB.Bson;

namespace App.Data
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders(string userId);
        Order GetOrderById(string orderId, string userId);
        void Create(Order order);
        bool Update(string orderId, string userId, IEnumerable<OrderItem> items);
        bool Delete(string orderId, string userId);
    }
}
