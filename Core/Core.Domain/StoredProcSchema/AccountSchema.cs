using Assets.Model;
using Assets.Model.Base;
using System;
using System.Net;

namespace Core.Domain.StoredProcSchema {
    [StoredProcedure("dbo", "webapi_account_authenticate")]
    public class AccountAuthenticateSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public string Token { get; set; }
    }

    [StoredProcedure("dbo", "webapi_account_getFirst")]
    public class AccountGetFirstSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int? Id { get; set; }

        [InputParameter]
        public string Username { get; set; }

        [InputParameter]
        public string Password { get; set; }

        [InputParameter]
        public AccountProvider? ProviderId { get; set; }

        [InputParameter]
        public DateTime? LastSignedinAt { get; set; }

        [InputParameter]
        public Status? StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_account_add")]
    public class AccountAddSchema: BaseSchema, IStoredProcSchema {
        [OutputParameter]
        public int? Id { get; set; }

        [InputParameter]
        public string Username { get; set; }

        [InputParameter]
        public string Password { get; set; }

        [InputParameter]
        public AccountProvider? ProviderId { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [InputParameter]
        public Status? StatusId { get; set; } = Status.Pending;
    }

    [StoredProcedure("dbo", "webapi_account_update")]
    public class AccountUpdateSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int Id { get; set; }

        [InputParameter]
        public string Password { get; set; }

        [InputParameter]
        public DateTime? LastSignedinAt { get; set; }

        [InputParameter]
        public Status? StatusId { get; set; }
    }
}
