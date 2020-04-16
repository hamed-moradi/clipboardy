using Assets.Model;
using Assets.Model.Base;
using Assets.Utility.Extension;
using System;
using System.Net;

namespace Core.Domain.StoredProcedure.Schema {
    [StoredProcedure("dbo", "webapi_accountDevice_getFirst")]
    public class AccountDeviceGetFirstSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int? @Id { get; set; }

        [InputParameter]
        public int? @AccountId { get; set; }

        [InputParameter]
        public string @DeviceId { get; set; }

        [InputParameter]
        public string @DeviceName { get; set; }

        [InputParameter]
        public string @DeviceType { get; set; }

        [InputParameter]
        public int? @StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_accountDevice_get")]
    public class AccountDeviceGetSchema: AccountDeviceGetFirstSchema { }

    [StoredProcedure("dbo", "webapi_accountDevice_add")]
    public class AccountDeviceAddSchema: BaseSchema, IStoredProcSchema {[InputParameter]
        public int? @AccountId { get; set; }

        [InputParameter]
        public string @Token { get; set; }

        [InputParameter]
        public string @DeviceId { get; set; }

        [InputParameter]
        public string @DeviceName { get; set; }

        [InputParameter]
        public string @DeviceType { get; set; }

        [InputParameter]
        public DateTime @CreatedAt { get; set; } = DateTime.Now;

        [InputParameter]
        public int? @StatusId { get; set; } = Status.Pending.ToInt();
    }

    [StoredProcedure("dbo", "webapi_accountDevice_update")]
    public class AccountDeviceUpdateSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int @Id { get; set; }

        [InputParameter]
        public string @Token { get; set; }

        [InputParameter]
        public int? @StatusId { get; set; }
    }
}
