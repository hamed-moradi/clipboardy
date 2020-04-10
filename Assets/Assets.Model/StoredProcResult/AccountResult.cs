using Assets.Model.Base;
using System;

namespace Assets.Model.StoredProcResult {
    public class AccountAuthenticateResult: IStoredProcResult {
        public int? Id { get; set; }
        public string Username { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        public string DeviceId { get; set; }
    }
}
