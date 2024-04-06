using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace App.Models.DTO
{
    public class GetOrderDto
    {
        public string Id { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int quantity { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
