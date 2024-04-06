using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace App.Models.DTO
{
    public class PostOrderDto
    {

        public string ItemName { get; set; } = "";
        public int quantity { get; set; }
    }
}
