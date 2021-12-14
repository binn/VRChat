using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public class VRCAuthenticatedUser : VRCUser
    {
        //public new Task BlockAsync() => InvalidForThisUser();
        public new Task FriendAsync() => InvalidForThisUser();
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
