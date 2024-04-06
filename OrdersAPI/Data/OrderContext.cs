using App.Configuration;
using App.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace App.Data
{
    public class OrderContext : IOrderContext
    {
        private readonly IMongoDatabase _db;

        public OrderContext(IOptions<MongoConnectionSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }


        public IMongoCollection<Order> Orders => _db.GetCollection<Order>("Orders");
    }
}
