
namespace Assets.Model.Common {
    public class AppSetting {
        public Authentication Authentication { get; set; }
        public Encryption Encryption { get; set; }
        public SignalR SignalR { get; set; }
        public ConnectionString ConnectionStrings { get; set; }
        public SmtpConfig SmtpConfig { get; set; }
        public SMSConfig SMSConfig { get; set; }
    }

    public class Authentication {
        public Google Google { get; set; }
    }

    public class Google {
        public string APIKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class Encryption {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public class SignalR {
        public string ServerPattern { get; set; }
        public int MaximumMessageSize { get; set; }
    }

    public class ConnectionString {
        public string MsSql { get; set; }
    }

    public class SmtpConfig {
        public string MailboxName { get; set; }
        public string MailboxAddress { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SMSConfig {
        public string Number { get; set; }
        public string Signature { get; set; }
    }

    public class MailChimpConfig { }
}
