using System.Text.Json;
using VRChat.NET;

namespace VRChat.ImplTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var username = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_USERNAME");
            var password = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_PASSWORD");
            var twoFactorSecret = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_2FASECRET");

            VRC.SetAccount(username, password, twoFactorSecret, null);
            var vrchat = new VRChatClient(VRC.Account, "VRChat.NET/1.0");

            var loginRes = await vrchat.LoginAsync();
            var res = await vrchat.Friends.GetSelfAsync();
        }

        static async Task OldTest(string[] args)
        {
            var username = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_USERNAME");
            var password = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_PASSWORD");
            var twoFactorSecret = Environment.GetEnvironmentVariable("VRCAPI_VRCHAT_2FASECRET");
            
            IVRChat vrchat = new VRChatConnectionBuilder()
                .AsSDKContext()
                .WithCredentials(username, password)
                .Build();

            var user = await vrchat.Authentication.LoginAsync();
            Console.WriteLine(JsonSerializer.Serialize(user));
            Console.WriteLine("Testing GetCurrentUser");

            var u2 = await vrchat.Authentication.GetCurrentUserAsync();
            Console.WriteLine(u2.UserIcon);
            Console.ReadLine();
        }
    }
}
