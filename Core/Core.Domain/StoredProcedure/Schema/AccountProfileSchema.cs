using Assets.Model;
using Assets.Model.Base;
using Assets.Utility.Extension;
using System;
using System.Net;

namespace Core.Domain.StoredProcedure.Schema {
    [StoredProcedure("dbo", "webapi_accountProfile_getFirst")]
    public class AccountProfileGetFirstSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int? Id { get; set; }

        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? TypeId { get; set; }

        [InputParameter]
        public string LinkedId { get; set; }

        [InputParameter]
        public int? StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_accountProfile_add")]
    public class AccountProfileAddSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int AccountId { get; set; }

        [InputParameter]
        public int TypeId { get; set; }

        [InputParameter]
        public string LinkedId { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [InputParameter]
        public int? StatusId { get; set; } = Status.Pending.ToInt();
    }

    [StoredProcedure("dbo", "webapi_accountProfile_update")]
    public class AccountProfileUpdateSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int Id { get; set; }

        [InputParameter]
        public string ForgotPasswordToken { get; set; }

        [InputParameter]
        public int? StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_accountProfile_cleanTokens")]
    public class AccountProfileCleanTokensSchema: BaseSchema, IStoredProcSchema {
        [InputParameter]
        public int AccountId { get; set; }
    }
}
