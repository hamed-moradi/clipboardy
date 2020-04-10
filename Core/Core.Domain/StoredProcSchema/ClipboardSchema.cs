using Assets.Model;
using Assets.Model.Base;
using System;
using System.Net;

namespace Core.Domain.StoredProcSchema {
    [StoredProcedure("dbo", "webapi_clipboard_getPaging")]
    public class ClipboardGetPagingSchema: PagingSchema {
        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? DeviceId { get; set; }

        [InputParameter]
        public int? TypeId { get; set; }

        [InputParameter]
        public DateTime? CreatedAt { get; set; }

        [InputParameter]
        public Status? StatusId { get; set; }

        [ReturnParameter]
        public HttpStatusCode StatusCode { get; set; }
    }
}
