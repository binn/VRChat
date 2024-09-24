namespace VRChat.Environments
{
    public class VRChatCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Auth { get; set; }
        public string TwoFactorAuth { get; set; }
        public string ApiKey { get; set; } = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26";
        // TODO: In the future, move to using /config for retrieving the APIKey
    }
}
