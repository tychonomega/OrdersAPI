using System.Text.Json.Serialization;

namespace App.Data.Models
{
    public class OrderItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
