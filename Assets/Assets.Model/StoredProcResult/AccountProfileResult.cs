using Assets.Model.Base;
using System;

namespace Assets.Model.StoredProcResult {
    public class AccountProfileResult: IStoredProcResult {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public AccountProfileType? TypeId { get; set; }
        public string LinkedId { get; set; }
        public string ForgotPasswordToken { get; set; }
        public Status? StatusId { get; set; }
    }
}
