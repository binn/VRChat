using System;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public interface IVRCUser : IVRCUserCore
    {
        string InstanceId { get; }
        bool AllowAvatarCopying { get; }
        string[] BioLinks { get; }
        DateTime DateJoined { get; }
        DateTime? LastLogin { get; }
        string WorldId { get; }
        VRCUserState State { get; }
        VRCUserStatus Status { get; }

        Task BlockAsync();
        Task UnblockAsync();
        Task MuteAsync();
        Task UnmuteAsync();
        Task HideAvatarAsync();
        Task ShowAvatarAsync();
    }
}
