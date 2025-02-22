using System.Text.Json.Serialization;

namespace VRChat.NET.Models
{
    public class VRChatErrorResponse
    {
        public VRChatError Error { get; set; }
    }

    public class VRChatError
    {
        public string Message { get; set; }

        [JsonPropertyName("waf_code")]
        public int WafCode { get; set; }

        [JsonPropertyName("waf_list")]
        public string WafList { get; set; }
    }
}