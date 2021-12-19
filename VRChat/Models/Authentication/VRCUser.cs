using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VRChat.Models.Authentication
{
    public class VRCUser : VRCUserCore, IVRCUser
    {
        public string InstanceId { get; init; }
        public bool AllowAvatarCopying { get; init; }
        public string[] BioLinks { get; init; }

        [JsonPropertyName("date_joined")] // Possibly make internal methods for getters/setters so that this doesn't get reserialized into date_joined
        public DateTime DateJoined { get; init; }

        [JsonPropertyName("last_login")]
        public DateTime? LastLogin { get; init; }
        public string WorldId { get; init; }
        public VRCUserState State { get; init; }
        public VRCUserStatus Status { get; init; }

        public Task BlockAsync()
        {
            Console.WriteLine("Auth: " + _caller.Credentials.Auth);
            Console.WriteLine("Block called");
            return Task.CompletedTask;
        }

        public Task HideAvatarAsync()
        {
            throw new NotImplementedException();
        }

        public Task MuteAsync()
        {
            throw new NotImplementedException();
        }

        public Task ShowAvatarAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnblockAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnmuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
