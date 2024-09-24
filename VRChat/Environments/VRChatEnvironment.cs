using System;

namespace VRChat.Environments
{
    public partial class VRChatEnvironment
    {
        private readonly string _path;
        private readonly string _endpoint;

        public string Path => _path;
        public string Endpoint => _endpoint;
        public Uri BaseAddress => this.ToUri();

        public VRChatEnvironment(string endpoint, string path)
        {
            _path = path.TrimStart('/');
            _endpoint = endpoint.TrimEnd('/');
        }

        public Uri ToUri() =>
            new(this.ToString());

        public override string ToString() =>
            $"{_endpoint}/{_path}";
    }

    public partial class VRChatEnvironment
    {
        public readonly static VRChatEnvironment VRChatGlobalCloud = 
            new("https://api.vrchat.cloud/", "/api/1/");

        public readonly static VRChatEnvironment VRChatWebProxy =
            new("https://vrchat.com/", "/api/1/");
    }
}
