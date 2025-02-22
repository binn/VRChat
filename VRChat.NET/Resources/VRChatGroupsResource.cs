
using VRChat.NET.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Net.Http;
using VRChat.NET.Models.Authentication;

namespace VRChat.NET.Resources
{
    public class VRChatGroupsResource
    {
        private readonly VRChatClient _client;

        public VRChatGroupsResource(VRChatClient client)
        {
            _client = client;
        }

        public async Task<VRChatResponse<VRChatCurrentUserResponse>> GetSelfAsync(CancellationToken ct = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.vrchat.cloud/api/1/auth/user");
            var res = await _client.SendRequestAsync<VRChatCurrentUserResponse>(req, ct);
            
            return res;
        }
    }
}