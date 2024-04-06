using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace App.Data.Models
{
    public class Order : BaseDataModel
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public IEnumerable<OrderItem>? Items { get; set; }
        public string User { get; set; } = string.Empty;
      
    }
}
