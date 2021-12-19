using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public class VRCUserCore : IVRCUserCore
    {
        internal IVRChat _caller;

        public string Id { get; init; }
        public string Username { get; init; }
        public string DisplayName { get; init; }
        public string Bio { get; init; }
        public bool IsFriend { get; init; }

        [JsonPropertyName("last_platform")]
        public string LastPlatform { get; init; }
        public string ProfilePicOverride { get; init; }
        public string StatusDescription { get; init; }
        public string[] Tags { get; init; }
        public string UserIcon { get; init; }
        public string Location { get; init; }
        public string FriendKey { get; init; }
        public string FallbackAvatar { get; init; }
        public string CurrentAvatarImageURL { get; init; }
        public string CurrentAvatarThumbnailImageURL { get; init; }
        public DeveloperType DeveloperType { get; init; }

        public Task FriendAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnfriendAsync()
        {
            throw new NotImplementedException();
        }

        internal void SetCaller(IVRChat caller) =>
            _caller = caller;
    }
}
