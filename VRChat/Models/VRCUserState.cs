using System.Text.Json.Serialization;

namespace VRChat.Models
{
    public enum VRCUserState
    {
        [JsonPropertyName("offline")]
        Offline,
        
        [JsonPropertyName("active")]
        Active,

        [JsonPropertyName("online")]
        Online
    }
}
