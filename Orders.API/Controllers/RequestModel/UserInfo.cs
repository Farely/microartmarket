using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Orders.API.Controllers.RequestModel
{
    public class OrderInfo
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("claims")] public Dictionary<string, string> Claims { get; set; }
    }
}