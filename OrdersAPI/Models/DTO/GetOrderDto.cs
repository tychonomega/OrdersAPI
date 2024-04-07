using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using App.Data.Models;
using System.Text.Json.Serialization;

namespace App.Models.DTO
{
    public class GetOrderDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("items")]
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
        // Can certainly be argued that this is not necessary to return for single-user use cases, but including it would allow master/admin bulk operations to re-use this DTO.
        // if there were sensitive fields, we would split that into an different DTO.
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }
    }
}
