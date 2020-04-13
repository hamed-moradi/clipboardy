using Assets.Model.Base;
using System;

namespace Assets.Model.StoredProcResult {
    public class AccountResult: IStoredProcResult {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountProvider? ProviderId { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        public Status? StatusId { get; set; }
    }

    public class AccountAuthenticateResult: IStoredProcResult {
        public int? Id { get; set; }
        public string Username { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        public string DeviceId { get; set; }
    }
}
