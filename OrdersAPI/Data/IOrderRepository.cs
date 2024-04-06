using App.Data.Models;
using MongoDB.Bson;

namespace App.Data
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(string id);
        void Create(Order order);
        bool Update(Order order);
        bool Delete(string id);
    }
}
