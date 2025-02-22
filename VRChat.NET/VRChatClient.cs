using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VRChat.NET.Models;
using OtpNet;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using VRChat.NET.Models.Authentication;
using VRChat.NET.Resources;

namespace VRChat.NET
{
    public class VRChatClient
    {
        public VRChatAccount Account;
        public CookieContainer Cookies;
        public HttpClient HttpClient;
        private HttpClientHandler _handler;

        public VRChatUsersResource Users { get; private set; }
        public VRChatGroupsResource Groups { get; private set; }
        public VRChatWorldsResource Worlds { get; private set; }
        public VRChatAvatarsResource Avatars { get; private set; }
        public VRChatFriendsResource Friends { get; private set; }
        public VRChatInvitesResource Invites { get; private set; }
        public VRChatFavoritesResource Favorites { get; private set; }
        public VRChatInstancesResource Instances { get; private set; }
        public VRChatModerationsResource Moderations { get; private set; }
        public VRChatNotificationsResource Notifications { get; private set; }
        public VRChatAuthenticationResource Authentication { get; private set; }

        public VRChatClient(VRChatAccount account, string userAgent)
        {
            Account = account;
            SetupClient(userAgent, false);
        }

        public void SetupClient(string userAgent, bool enableProxy = true)
        {
            _handler = new HttpClientHandler();
            _handler.UseCookies = true;
            Cookies = _handler.CookieContainer = new CookieContainer();

            HttpClient = new HttpClient(_handler);
            HttpClient.Timeout = TimeSpan.FromSeconds(30);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);

            Users = new VRChatUsersResource(this);
            Groups = new VRChatGroupsResource(this);
            Worlds = new VRChatWorldsResource(this);
            Avatars = new VRChatAvatarsResource(this);
            Friends = new VRChatFriendsResource(this);
            Invites = new VRChatInvitesResource(this);
            Favorites = new VRChatFavoritesResource(this);
            Instances = new VRChatInstancesResource(this);
            Moderations = new VRChatModerationsResource(this);
            Notifications = new VRChatNotificationsResource(this);
            Authentication = new VRChatAuthenticationResource(this);
        }

        public async Task<VRChatResponse<T>> SendRequestAsync<T>(HttpRequestMessage req, CancellationToken ct = default, bool withCookie = true)
        {
            var res = new VRChatResponse<T>();

            if(withCookie)
                req.Headers.TryAddWithoutValidation("Cookie", Account.Cookie);

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(req, ct);
            }
            catch (Exception exception)
            {
                res.ErrorMessage = exception.Message;
                res.Exception = exception;
                res.StatusCode = -1;

                return res;
            }

            res.StatusCode = (int)response.StatusCode;
            res.RawResponse = response;

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<VRChatErrorResponse>();

                res.ErrorMessage = error.Error.Message;
                res.WafCode = error.Error.WafCode;
                res.WafList = error.Error.WafList;

                return res;
            }

            return res;
        }

        public Task<VRChatResponse<VRChatLoginResponse>> LoginAsync(CancellationToken ct = default) =>
            LoginAsync(Account.Email, Account.Password, Account.TwoFactorSecret, ct);

        public async Task<VRChatResponse<VRChatLoginResponse>> LoginAsync(string email, string password, string twoFactorSecret, CancellationToken ct = default)
        {
            var res = await BeginLoginAsync(email, password, ct);
            if (!res.Success)
                return res;

            if (!res.Value.RequiresTwoFactorAuth.Contains("totp"))
                throw new Exception("TOTP is not enabled on this account.");

            Totp totp = new Totp(Base32Encoding.ToBytes(twoFactorSecret));
            string code = totp.ComputeTotp();

            res = await CompleteLoginAsync(res.Value.Cookie, code, otpMode: "totp", ct);
            return res;
        }

        public async Task<VRChatResponse<VRChatLoginResponse>> BeginLoginAsync(string email, string password, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, "https://api.vrchat.cloud/api/1/auth/user");
            req.Headers.TryAddWithoutValidation("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{email}:{password}")));

            var res = await SendRequestAsync<VRChatLoginResponse>(req, ct, withCookie: false);

            var cookieList = Cookies.GetCookies(new Uri("https://api.vrchat.cloud"));
            var cookies = new Cookie[cookieList.Count];
            cookieList.CopyTo(cookies, 0);

            string cookie = cookies
                .Select(x => $"{x.Name}={x.Value}")
                .Aggregate((x, y) => $"{x}; {y}");

            res.Value.Cookie = cookie;
            return res;
        }

        public async Task<VRChatResponse<VRChatLoginResponse>> CompleteLoginAsync(string tempCookie, string code, string otpMode = "emailotp", CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"https://api.vrchat.cloud/api/1/auth/twofactorauth/{otpMode}/verify");
            req.Headers.TryAddWithoutValidation("Cookie", tempCookie);
            req.Content = JsonContent.Create(new { code });

            var res = await SendRequestAsync<VRChatLoginResponse>(req, ct, withCookie: false);

            var cookieList = Cookies.GetCookies(new Uri("https://api.vrchat.cloud"));
            var cookies = new Cookie[cookieList.Count];
            cookieList.CopyTo(cookies, 0);

            string cookie = cookies
                .Select(x => $"{x.Name}={x.Value}")
                .Aggregate((x, y) => $"{x}; {y}");

            var currentUser = await res.RawResponse.Content.ReadFromJsonAsync<VRChatCurrentUserResponse>(ct);

            Account.Cookie = cookie;
            res.Value.Cookie = cookie;
            res.Value.CurrentUser = currentUser;

            return res;
        }
    }
}