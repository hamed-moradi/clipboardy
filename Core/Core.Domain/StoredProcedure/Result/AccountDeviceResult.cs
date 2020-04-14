using Assets.Model.Base;
using System;

namespace Core.Domain.StoredProcedure.Result {
    public class AccountDeviceResult: IStoredProcResult {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public Status? StatusId { get; set; }
    }
}
