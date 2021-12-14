using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public interface IVRCUserCore
    {
        string Id { get; }
        string Username { get; }
        string DisplayName { get; }
        string Bio { get; }
        bool IsFriend { get; }

        string LastPlatform { get; }
        string ProfilePicOverride { get; }
        string StatusDescription { get; }
        string[] Tags { get; }

        string UserIcon { get; }
        string Location { get; }
        string FriendKey { get; }

        string FallbackAvatar { get; }
        string CurrentAvatarImageURL { get; }
        string CurrentAvatarThumbnailImageURL { get; }

        DeveloperType DeveloperType { get; }

        Task FriendAsync();
        Task UnfriendAsync();
    }
}
