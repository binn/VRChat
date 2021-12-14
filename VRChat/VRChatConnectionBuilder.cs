using VRChat.Builders;
using VRChat.Environments;

namespace VRChat
{
    public class VRChatConnectionBuilder
    {
        public VRChatContextConnectionBuilder AsCustomContext()
        {
            return new VRChatContextConnectionBuilder()
                .WithNotFoundBehavior(NotFoundBehavior.Ignore)
                .WithEnvironment(VRChatEnvironment.VRChatGlobalCloud);
        }

        public VRChatContextConnectionBuilder AsWebContext()
        {
            return new VRChatContextConnectionBuilder()
                .WithNotFoundBehavior(NotFoundBehavior.Ignore)
                .WithUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.93 Safari/537.36")
                .WithEnvironment(VRChatEnvironment.VRChatWebProxy);
        }

        public VRChatContextConnectionBuilder AsSDKContext() =>
            AsSDKContext(VRChatPlatform.Windows);

        public VRChatContextConnectionBuilder AsSDKContext(VRChatPlatform platform)
        {
            return new VRChatContextConnectionBuilder()
                .WithRequester()
                .WithPlatform(platform)
                .WithUserAgent("VRC.Core.BestHTTP")
                .WithNotFoundBehavior(NotFoundBehavior.Ignore)
                .WithSDKVersion(VRChatPlatform.KnownSDKVersion)
                .WithEnvironment(VRChatEnvironment.VRChatGlobalCloud);
        }

        public VRChatContextConnectionBuilder AsClientContext() =>
            AsClientContext(VRChatPlatform.Windows);

        public VRChatContextConnectionBuilder AsClientContext(VRChatPlatform platform)
        {
            return new VRChatContextConnectionBuilder()
                .WithRequester()
                .WithPlatform(platform)
                .WithUserAgent("VRC.Core.BestHTTP")
                .WithNotFoundBehavior(NotFoundBehavior.Ignore)
                .WithClientVersion(VRChatPlatform.KnownClientVersion)
                .WithEnvironment(VRChatEnvironment.VRChatGlobalCloud);
        }
    }
}
