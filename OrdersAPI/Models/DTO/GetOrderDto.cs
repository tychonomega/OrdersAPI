using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using App.Data.Models;

namespace App.Models.DTO
{
    public class GetOrderDto
    {
        public string Id { get; set; } = "";
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
        // Can certainly be argued that this is not necessary to return for single-user use cases, but including it would allow master/admin bulk operations to re-use this DTO.
        // if there were sensitive fields, we would split that into an different DTO.
        public string User { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
