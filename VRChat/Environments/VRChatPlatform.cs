namespace VRChat.Environments
{
    public partial class VRChatPlatform
    {
        private readonly string _platform;

        public VRChatPlatform(string platformName) =>
            _platform = platformName;

        public string Platform => _platform;

        public override string ToString() =>
            Platform;
    }

    public partial class VRChatPlatform
    {
        public static VRChatPlatform Windows => new VRChatPlatform("standalonewindows");
        public static VRChatPlatform Quest => new VRChatPlatform("android");

        public const string KnownSDKVersion = "2021.11.08.14.28";
        public const string KnownClientVersion = "2021.4.2p2-1160--Release";
    }
}
