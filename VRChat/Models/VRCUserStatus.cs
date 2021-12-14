using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VRChat.Models
{
    public enum VRCUserStatus
    {
        [JsonPropertyName("offline")]
        Offline,

        [JsonPropertyName("active")]
        Active,
        
        [JsonPropertyName("join me")]
        [EnumMember(Value = "join me")]
        JoinMe,
        
        [JsonPropertyName("ask me")]
        [EnumMember(Value = "ask me")]
        AskMe,

        [JsonPropertyName("busy")]
        Busy
    }
}
