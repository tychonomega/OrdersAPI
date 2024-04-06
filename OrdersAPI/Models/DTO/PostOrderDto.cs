using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using App.Data.Models;

namespace App.Models.DTO
{
    public class PostOrderDto
    {
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
    }
}
