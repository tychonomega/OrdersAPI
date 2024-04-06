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
        public string ItemName { get; set; } = "";
        public int quantity { get; set; }
    }
}
