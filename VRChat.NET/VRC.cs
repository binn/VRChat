namespace VRChat.NET
{
    public class VRC
    {
        public static VRChatAccount Account { get; set; } = new VRChatAccount();

        public static void SetAccount(string email, string password, string twoFactorSecret, string cookie)
        {
            Account.Email = email;
            Account.Password = password;
            Account.TwoFactorSecret = twoFactorSecret;
            Account.Cookie = cookie;
        }
    }
}
