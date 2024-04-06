using App.Data.Models;
using MongoDB.Bson;

namespace App.Data
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders(string userId);
        Order GetOrderById(string id, string userId);
        void Create(Order order);
        bool Update(Order order);
        bool Delete(string id);
    }
}
