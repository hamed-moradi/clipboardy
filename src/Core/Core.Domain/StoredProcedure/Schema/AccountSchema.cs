using Assets.Model;
using Assets.Model.Base;
using Assets.Utility.Extension;
using System;
using System.Net;

namespace Core.Domain.StoredProcedure.Schema {
    [StoredProcedure("dbo", "webapi_account_authenticate")]
    public class AccountAuthenticateSchema: BaseSchema {
        [InputParameter]
        public string @Token { get; set; }
    }

    [StoredProcedure("dbo", "webapi_account_getFirst")]
    public class AccountGetFirstSchema: BaseSchema {
        [InputParameter(Name = "Id")]
        public int? @Id { get; set; }

        [InputParameter]
        public string @Username { get; set; }

        [InputParameter]
        public string @Password { get; set; }

        [InputParameter]
        public int? @ProviderId { get; set; }

        [InputParameter]
        public DateTime? LastSignedinAt { get; set; }

        [InputParameter]
        public int? @StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_account_add")]
    public class AccountAddSchema: BaseSchema {
        [InputParameter]
        public string @Username { get; set; }

        [InputParameter]
        public string @Password { get; set; }

        [InputParameter]
        public int @ProviderId { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [InputParameter]
        public int @StatusId { get; set; } = Status.Pending.ToInt();
    }

    [StoredProcedure("dbo", "webapi_account_update")]
    public class AccountUpdateSchema: BaseSchema {
        [InputParameter]
        public int @Id { get; set; }

        [InputParameter]
        public string @Password { get; set; }

        [InputParameter]
        public DateTime? LastSignedinAt { get; set; }

        [InputParameter]
        public int? @StatusId { get; set; }
    }
}
