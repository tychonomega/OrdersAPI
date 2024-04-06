using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using App.Data.Models;

namespace App.Models.DTO
{
    public class GetOrderDto
    {
        public string Id { get; set; } = "";
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
        public string User { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
