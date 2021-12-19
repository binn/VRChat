using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using VRChat.Environments;

namespace VRChat.Client
{
    public interface IVRChatRestClient
    {
        HttpClient HttpClient { get; }
        HttpClientHandler HttpClientHandler { get; }
        VRChatEnvironment Environment { get; }
        NotFoundBehavior NotFoundBehavior { get; }
        JsonSerializerOptions Json { get; }

        // Should look into what this class does more

        void SetCookie(string cookie);
        void SetAuth(string auth, string apiKey);
        string GetCookie(string name);
        DateTime CookieExpiry(string name);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<HttpResponseMessage> GetAsync(string endpoint, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default);
        Task<HttpResponseMessage> DeleteAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default);
    }

    public class VRChatRestClient : IVRChatRestClient
    {
        private readonly NotFoundBehavior _notFoundBehavior;
        private readonly JsonSerializerOptions _options;
        private readonly VRChatEnvironment _environment;
        private readonly HttpClientHandler _handler;
        private readonly HttpClient _client;
        private string _apiKey;

        public VRChatRestClient(NotFoundBehavior behavior, VRChatEnvironment env, HttpClient client, HttpClientHandler handler)
        {
            _notFoundBehavior = behavior;
            _environment = env;
            _handler = handler;
            _client = client;

            _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _options.Converters.Add(new JsonStringEnumMemberConverter());
        }

        public HttpClient HttpClient => _client;
        public HttpClientHandler HttpClientHandler => _handler;
        public VRChatEnvironment Environment => _environment;
        public NotFoundBehavior NotFoundBehavior => _notFoundBehavior;
        public JsonSerializerOptions Json => _options;

        public Task<HttpResponseMessage> DeleteAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default) =>
            SendAsync(endpoint, HttpMethod.Get, data, expectedStatusCode, behavior, ct);

        public Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default) =>
            SendAsync(endpoint, HttpMethod.Get, data, expectedStatusCode, behavior, ct);

        public Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default) =>
            SendAsync(endpoint, HttpMethod.Get, data, expectedStatusCode, behavior, ct);

        public Task<HttpResponseMessage> GetAsync(string endpoint, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default) =>
            SendAsync<object>(endpoint, HttpMethod.Get, null, expectedStatusCode, behavior, ct);

        private async Task<HttpResponseMessage> SendAsync<T>(string endpoint, HttpMethod method, T data, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default)
        {
            using (var req = new HttpRequestMessage(method, endpoint))
            {
                if (data != null)
                    req.Content = JsonContent.Create(data);

                return await this.SendAsync(req, expectedStatusCode, behavior, ct).ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, int expectedStatusCode, NotFoundBehavior? behavior = null, CancellationToken ct = default)
        {
            behavior ??= _notFoundBehavior;
            
            //todo: i fucking forgot we had access to the cookiecontainer
            //so next time we're just going to have it grab the new auth from the cookiecontainer
            //as well as the apiKey
            //but we really should just have an InitializeAsync or something like that method
            //that calls /config anonymously
            //and populates the apikey from there
            //as well as remove all public calls to apiKey (unless we want users to be able to control that)

            if (_apiKey != null) // MOVE THIS INTO AN EXTENSION METHOD
            {
                UriBuilder builder = new UriBuilder(request.RequestUri.ToAbsolute("https://vrc.com"));
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["apiKey"] = _apiKey;
                builder.Query = query.ToString();

                Uri.TryCreate(builder.Uri.ToRelative(), UriKind.Relative, out Uri req);
                request.RequestUri = req; // fix this fucked impl later
            }

            var res = await _client.SendAsync(request, ct).ConfigureAwait(false);
            if (expectedStatusCode != (int)res.StatusCode)
            {
                if (res.StatusCode == HttpStatusCode.NotFound)
                    if (behavior == NotFoundBehavior.Ignore)
                        return res; // so i resorted to this lol
                
                throw HandleError(res);
            }
            
            return res;
        }

        private Exception HandleError(HttpResponseMessage res)
        {
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new Exception("Unauthorized");
            }
            else if (res.StatusCode == HttpStatusCode.Forbidden)
            {
                return new Exception();
            }
            else if (res.StatusCode == HttpStatusCode.NotFound)
            {
                return new Exception();
            }

            return new Exception();
        }

        [Obsolete("Should not be used over the CookieContainer")]
        public void SetCookie(string cookie)
        {
            _client.DefaultRequestHeaders.Remove("Cookie");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", cookie);
        }

        public void SetAuth(string auth, string apiKey)
        {
            SetApiKey(apiKey);
            string cookie = $"apiKey={apiKey};";

            if (auth != null)
                cookie += $" auth={auth};";

            _handler.CookieContainer.SetCookies(new Uri(_environment.Endpoint), cookie);
        }

        public void SetApiKey(string apiKey) =>
            _apiKey = apiKey;

        public string GetCookie(string name)
        {
            CookieCollection cookies = _handler.CookieContainer.GetCookies(_environment.BaseAddress);
         
            foreach(var ck in cookies)
            {
                var cookie = (Cookie)ck;
                if (cookie.Name == name)
                    return cookie.Value;
            }

            return null;
        }

        public DateTime CookieExpiry(string name)
        {
            CookieCollection cookies = _handler.CookieContainer.GetCookies(_environment.BaseAddress);

            foreach (var ck in cookies)
            {
                var cookie = (Cookie)ck;
                if (cookie.Name == name)
                    return cookie.Expires;
            }

            return default;
        }
    }

    public static class UriExtensions
    {
        public static string ToRelative(this Uri uri)
        {
            return uri.IsAbsoluteUri ? uri.PathAndQuery.TrimStart('/') : uri.ToString();
        }

        public static Uri ToAbsolute(this Uri uri, string @b)
        {
            var baseUri = new Uri(@b);
            var relative = uri.ToRelative();

            if (Uri.TryCreate(baseUri, relative, out var absolute))
                return absolute;

            return uri.IsAbsoluteUri ? uri : null;
        }
    }
}