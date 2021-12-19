using System;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public class VRCAuthenticatedUser : VRCUser
    {
        public string[] Friends { get; init; }
        public string[] FriendGroupNames { get; init; }
        public string[] PastDisplayNames { get; init; }
        public string[] StatusHistory { get; init; }

        public static new Task BlockAsync() => InvalidForThisUser();
        public static new Task FriendAsync() => InvalidForThisUser();
        public static new Task HideAvatarAsync() => InvalidForThisUser();
        public static new Task MuteAsync() => InvalidForThisUser();
        public static new Task ShowAvatarAsync() => InvalidForThisUser();
        public static new Task UnblockAsync() => InvalidForThisUser();
        public static new Task UnfriendAsync() => InvalidForThisUser();
        public static new Task UnmuteAsync() => InvalidForThisUser();

        public Task DisableCloningAsync()
        {
            throw new InvalidOperationException();
        }

        public Task EnableCloningAsync()
        {
            throw new InvalidOperationException();
        }

        private static Task InvalidForThisUser() =>
            throw new InvalidOperationException("Cannot perform this operation on the current user.");
    }
}
