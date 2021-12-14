using System.Text.Json.Serialization;

namespace VRChat.Models
{
    public enum DeveloperType
    {
        [JsonPropertyName("none")]
        None,

        [JsonPropertyName("trusted")]
        Trusted,
        
        [JsonPropertyName("internal")]
        Internal,

        [JsonPropertyName("moderator")]
        Moderator
    }
}
