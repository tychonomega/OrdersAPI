using App.Data.Models;
using MongoDB.Driver;

namespace App.Data
{
    public interface IOrderContext
    {
        public IMongoCollection<Order> Orders { get; }
    }
}
