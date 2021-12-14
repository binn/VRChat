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
            new Uri(this.ToString());

        public override string ToString() =>
            $"{_endpoint}/{_path}";
    }

    public partial class VRChatEnvironment
    {
        public static VRChatEnvironment VRChatGlobalCloud = 
            new VRChatEnvironment("https://api.vrchat.cloud/", "/api/1/");

        public static VRChatEnvironment VRChatWebProxy =
            new VRChatEnvironment("https://vrchat.com/", "/api/1/");
    }
}
