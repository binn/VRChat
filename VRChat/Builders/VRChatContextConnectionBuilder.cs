using System;
using System.Net;
using VRChat.Client;
using VRChat.Environments;

namespace VRChat.Builders
{
    public class VRChatContextConnectionBuilder
    {
        private readonly VRChatRestClientBuilder _builder;
        private readonly VRChatCredentials _credentials;

        public VRChatContextConnectionBuilder()
        {
            _credentials = new VRChatCredentials();
            _builder = new VRChatRestClientBuilder();
            this.WithApiKey(_credentials.ApiKey);
        }

        public IVRChat Build() => 
            new Client.VRChat(_credentials, _builder.Build());

        public VRChatContextConnectionBuilder WithEnvironment(VRChatEnvironment environment) =>
            ConfigureRestClient(builder => builder.WithEnvironment(environment));

        public VRChatContextConnectionBuilder ConfigureRestClient(Action<VRChatRestClientBuilder> builder)
        {
            builder?.Invoke(_builder);
            return this;
        }

        public VRChatContextConnectionBuilder WithCredentials(string auth)
        {
            _credentials.Auth = auth;
            return this;
        }

        public VRChatContextConnectionBuilder WithCredentials(string username, string password) => this
            .WithUsername(username)
            .WithPassword(password);

        public VRChatContextConnectionBuilder WithUsername(string username)
        {
            _credentials.Username = username;
            return this;
        }

        public VRChatContextConnectionBuilder WithPassword(string password)
        {
            _credentials.Password = password;
            return this;
        }

        public VRChatContextConnectionBuilder WithApiKey(string apiKey) =>
            ConfigureRestClient(builder => builder.WithApiKey(_credentials.ApiKey = apiKey));

        public VRChatContextConnectionBuilder WithNotFoundBehavior(NotFoundBehavior behavior) =>
            ConfigureRestClient(builder => builder.WithNotFoundBehavior(behavior));

        public VRChatContextConnectionBuilder WithHwid(string hwid) =>
            ConfigureRestClient(builder => builder.AddHeader("X-MacAddress", hwid));

        public VRChatContextConnectionBuilder WithSDKVersion(string sdkVersion) =>
            ConfigureRestClient(builder => builder.AddHeader("X-SDK-Version", sdkVersion));

        public VRChatContextConnectionBuilder WithClientVersion(string clientVersion) =>
            ConfigureRestClient(builder => builder.AddHeader("X-Client-Version", clientVersion));

        public VRChatContextConnectionBuilder WithUserAgent(string userAgent) =>
            ConfigureRestClient(builder => builder.WithUserAgent(userAgent));

        public VRChatContextConnectionBuilder WithRequester(string requester = "XMLHttpRequest") =>
            ConfigureRestClient(builder => builder.AddHeader("X-Requested-With", requester));

        public VRChatContextConnectionBuilder WithPlatform(VRChatPlatform platform) =>
            ConfigureRestClient(builder => builder.AddHeader("X-Platform", platform.ToString()));

        public VRChatContextConnectionBuilder WithProxy(WebProxy proxy) =>
            ConfigureRestClient(builder => builder.WithProxy(proxy));

        public VRChatContextConnectionBuilder WithTimeout(TimeSpan timeout) =>
            ConfigureRestClient(builder => builder.WithTimeout(timeout));
    }
}
