using VRChat.Environments;
using VRChat.Models.Authentication;
using VRChat.Resources;

namespace VRChat.Client
{
    public class VRChat : IVRChat
    {
        private IVRChatRestClient _client;
        private VRChatCredentials _credentials;

        private IAuthenticationResource _authentication;

        internal VRChat(VRChatCredentials creds, IVRChatRestClient client)
        {
            _client = client;
            _credentials = creds;
            _client.SetAuth(creds.Auth, creds.ApiKey);
            
            _authentication = new AuthenticationResource(this, client);
        }

        public IAuthenticationResource Authentication => _authentication;

        public IVRChatRestClient RestClient => _client;
        public VRChatCredentials Credentials => _credentials;
        public VRCAuthenticatedUser CurrentUser => throw new System.NotImplementedException();
    }
}
