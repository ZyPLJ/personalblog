namespace Personalblog.Models.Config
{
    public class SecuritySetting
    {
        public Token Token { get; set; }
    }
    public class Token
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
