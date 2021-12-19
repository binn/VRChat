using System.Text.Json.Serialization;

namespace VRChat.Models
{
    public class VRCError
    {
        public string Message { get; init; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; init; }
    }

    public class VRCErrorWrapper
    {
        public VRCError Error { get; init; }
    }
}
