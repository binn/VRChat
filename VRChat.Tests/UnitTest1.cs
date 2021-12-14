using System;
using System.Threading.Tasks;
using Xunit;

namespace VRChat.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Login_Test()
        {
            string username = Environment.GetEnvironmentVariable("VRCHAT_USERNAME");
            string password = Environment.GetEnvironmentVariable("VRCHAT_PASSWORD");

            IVRChat vrc = new VRChatConnectionBuilder()
                .AsWebContext()
                .WithCredentials(username, password)
                .Build();

            var user = await vrc.Authentication.LoginAsync();
            Assert.Equal(username, user.Username);
        }
    }
}
