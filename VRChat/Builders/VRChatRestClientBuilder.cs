using System;
using System.Net;
using System.Net.Http;
using VRChat.Environments;

namespace VRChat.Client
{
    public class VRChatRestClientBuilder
    {
        private HttpClient _client;
        private NotFoundBehavior _notFoundBehavior;
        private VRChatEnvironment _environment;
        private HttpClientHandler _handler;
        private string _apiKey;

        public VRChatRestClientBuilder()
        {
            _handler = new HttpClientHandler();

            _client = new HttpClient(_handler);
            _client.DefaultRequestHeaders.Clear();

            _environment = VRChatEnvironment.VRChatGlobalCloud;
        }

        public IVRChatRestClient Build()
        {
            _client.BaseAddress = _environment.ToUri();
            var rest = new VRChatRestClient(_notFoundBehavior, _environment, _client, _handler);
            rest.SetApiKey(_apiKey);

            return rest;
        }

        public VRChatRestClientBuilder WithApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public VRChatRestClientBuilder WithHttpClientHandler(HttpClientHandler handler)
        {
            _handler = handler;
            return this;
        }

        public VRChatRestClientBuilder WithHttpClient(HttpClient client)
        {
            _client = client;
            return this;
        }

        public VRChatRestClientBuilder WithHttpClient(HttpClient client, HttpClientHandler handler) => this
            .WithHttpClientHandler(handler)
            .WithHttpClient(client);

        // all of these methods may probably be removed as it seems almost useless to have access to the httpclient

        public VRChatRestClientBuilder WithEnvironment(VRChatEnvironment environment)
        {
            _environment = environment;
            return this;
        }

        public VRChatRestClientBuilder WithNotFoundBehavior(NotFoundBehavior behavior)
        {
            _notFoundBehavior = behavior;
            return this;
        }

        public VRChatRestClientBuilder AddHeader(string headerName, string value)
        {
            _client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, value);
            return this;
        }

        public VRChatRestClientBuilder WithUserAgent(string userAgent)
        {
            // Not sure if this'll really work while also letting fully custom UAs 
            // although user agent's should be following the HTTP spec
            _client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
            return this;
        }

        public VRChatRestClientBuilder WithProxy(WebProxy proxy)
        {
            _handler.Proxy = proxy;
            _handler.UseProxy = true;

            return this;
        }

        public VRChatRestClientBuilder WithTimeout(TimeSpan timeout)
        {
            _client.Timeout = timeout;
            return this;
        }
    }
}
