using Assets.Model;
using Assets.Model.Base;
using System.Net;

namespace Core.Domain.StoredProcSchema {
    [StoredProcedure("dbo", "webapi_account_authenticate")]
    public class AccountAuthenticateSchema: IStoredProcSchema {
        [InputParameter]
        public string Token { get; set; }

        [ReturnParameter]
        public HttpStatusCode StatusCode { get; set; }
    }
}
