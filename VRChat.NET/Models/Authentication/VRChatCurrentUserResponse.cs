namespace VRChat.NET.Models.Authentication
{
    public class VRChatCurrentUserResponse
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        public VRChatPresenceResponse Presence { get; set; }
    }

    public class VRChatPresenceResponse
    {
        public string Instance { get; set; }
    }
}