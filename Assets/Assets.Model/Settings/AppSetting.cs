
namespace Assets.Model.Settings {
    public class AppSetting {
        public Authentication Authentication { get; set; }
        public Encryption Custom { get; set; }
        public SignalR SignalR { get; set; }
        public ConnectionString ConnectionStrings { get; set; }
    }

    public class Authentication {
        public Google Google { get; set; }
    }

    public class Google {
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
}
