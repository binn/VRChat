using System.Text.Json;

namespace VRChat.ImplTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IVRChat vrchat = new VRChatConnectionBuilder()
                .AsSDKContext()
                .WithCredentials("dot bin", "x")
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
