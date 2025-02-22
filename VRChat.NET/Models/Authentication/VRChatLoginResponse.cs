namespace VRChat.NET.Models.Authentication
{
    public class VRChatLoginResponse
    {
        public string[] RequiresTwoFactorAuth { get; set; }
        public string Cookie { get; set; }
        public VRChatCurrentUserResponse CurrentUser { get; internal set; }
    }
}