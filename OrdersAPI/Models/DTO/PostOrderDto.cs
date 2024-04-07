using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using App.Data.Models;
using System.Text.Json.Serialization;

namespace App.Models.DTO
{
    public class PostOrderDto
    {
        [JsonPropertyName("items")]
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
    }
}
