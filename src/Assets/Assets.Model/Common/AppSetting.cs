
using System.Reflection;

namespace Assets.Model.Common
{
    public class AppSetting
    {
        private static string _version;
        public static string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_version))
                {
                    _version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
                return _version;
            }
        }
        public Authentication Authentication { get; set; }
        public SignalR SignalR { get; set; }
        public ConnectionString ConnectionStrings { get; set; }
        public SmtpConfig SmtpConfig { get; set; }
        public SMSConfig SMSConfig { get; set; }
        public ForgetResetPasswordConfig ForgetResetPasswordConfig { get; set; }
    }

    public class Authentication
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public Google Google { get; set; }
        public Microsoft Microsoft { get; set; }
    }

    public class Google
    {
        public string APIKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class Microsoft
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class SignalR
    {
        public string ServerPattern { get; set; }
        public int MaximumMessageSize { get; set; }
    }

    public class ConnectionString
    {
        public string Postgres { get; set; }
    }

    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SMSConfig
    {
        public string Number { get; set; }
        public string Signature { get; set; }
    }

    public class MailChimpConfig { }
    public class ForgetResetPasswordConfig
    {
        public string ForgetBaseUrl { get; set; }
        public int ExpireDate { get; set; }
    }
}
