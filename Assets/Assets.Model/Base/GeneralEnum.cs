namespace Assets.Model.Base {

    public enum Status {
        Inactive = 0,
        Active = 10,
        Pending = 20,
        Deleted = 30
    }

    public enum AccountProvider {
        Clipboard = 1,
        Google = 2,
        Microsoft = 3,
        Facebook = 4,
        Twitter = 5
    }

    public enum SendSMSStatus {
        Sent = 1,
        Failed = 2
    }
}
