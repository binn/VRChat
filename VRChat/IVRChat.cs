using VRChat.Client;
using VRChat.Environments;
using VRChat.Models.Authentication;
using VRChat.Resources;

namespace VRChat
{
    public interface IVRChat
    {
        IVRChatRestClient RestClient { get; }
        VRChatCredentials Credentials { get; }
        VRCAuthenticatedUser CurrentUser { get; }

        IAuthenticationResource Authentication { get; }
    }
}
