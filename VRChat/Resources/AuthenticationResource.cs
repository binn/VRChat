using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VRChat.Client;
using VRChat.Environments;
using VRChat.Models;
using VRChat.Models.Authentication;

namespace VRChat.Resources
{
    public interface IAuthenticationResource
    {
        Task<VRCAuthenticatedUser> LoginAsync(NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<VRCAuthenticatedUser> GetCurrentUserAsync(NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<bool> IsValidAuthCookieAsync(string authCookie);
    }

    public class AuthenticationResource : IAuthenticationResource
    {
        private readonly IVRChatRestClient _client;
        private readonly IVRChat _caller;

        public AuthenticationResource(IVRChat caller, IVRChatRestClient client)
        {
            _client = client;
            _caller = caller;
        }

        public async Task<VRCAuthenticatedUser> GetCurrentUserAsync(NotFoundBehavior? behavior = null, CancellationToken ct = default)
        {
            using var res = await _client.GetAsync("auth/user", 200, behavior, ct).ConfigureAwait(false);
            return await res.Content.ReadFromJsonAsync<VRCAuthenticatedUser>(_client.Json, ct).ConfigureAwait(false);
        }

        public Task<bool> IsValidAuthCookieAsync(string authCookie)
        {
            throw new System.NotImplementedException();
        }

        public async Task<VRCAuthenticatedUser> LoginAsync(NotFoundBehavior? behavior = null, CancellationToken ct = default)
        {
            // move this to Endpoints.Authentication.GetCurrentUser in the future
            using(var req = new HttpRequestMessage(HttpMethod.Get, "auth/user"))
            {
                // move this code somewhere else
                string header = WebUtility.HtmlEncode(_caller.Credentials.Username) + ":" + WebUtility.HtmlEncode(_caller.Credentials.Password);
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(header)));

                using (var res = await _client.SendAsync(req, 200, behavior, ct).ConfigureAwait(false))
                {
                    //foreach(var cookie in res.Headers.GetValues("Set-Cookie")) // MOVE THIS CODE SOMEWHERE ELSE
                    //{
                    //    var cookieItems = cookie.Split(';');
                    //    var auth = cookieItems.FirstOrDefault(x => x.StartsWith("auth"));
                    //    var apiKey = cookieItems.FirstOrDefault(x => x.StartsWith("apiKey"));

                    //    if (auth != null)
                    //        _caller.Credentials.Auth = auth.Split('=').LastOrDefault();

                    //    if(apiKey != null)
                    //        _caller.Credentials.ApiKey = apiKey.Split('=').LastOrDefault();

                    //    _client.SetAuth(_caller.Credentials.Auth, _caller.Credentials.ApiKey);
                    //}

                    var auth = _client.GetCookie("auth");
                    var apiKey = _client.GetCookie("apiKey");

                    if (auth != null)
                        _caller.Credentials.Auth = auth;

                    if (apiKey != null)
                        _caller.Credentials.ApiKey = apiKey;

                    _client.SetAuth(_caller.Credentials.Auth, _caller.Credentials.ApiKey);

                    var user = await res.Content.ReadFromJsonAsync<VRCAuthenticatedUser>(_client.Json, ct)
                        .ConfigureAwait(false);

                    user.SetCaller(_caller);
                    return user;
                }
            }
        }
    }
}
