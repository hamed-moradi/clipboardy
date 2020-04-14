using Assets.Model;
using Assets.Model.Base;
using System;
using System.Net;

namespace Core.Domain.StoredProcedure.Schema {
    [StoredProcedure("dbo", "webapi_clipboard_getFirst")]
    public class ClipboardGetFirstSchema: BaseSchema {
        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? DeviceId { get; set; }

        [InputParameter]
        public ContentType? TypeId { get; set; }

        [InputParameter]
        public string Content { get; set; }

        [InputParameter]
        public DateTime Priority { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; }

        [InputParameter]
        public Status StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_clipboard_getPaging")]
    public class ClipboardGetPagingSchema: PagingSchema {
        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? DeviceId { get; set; }

        [InputParameter]
        public int? TypeId { get; set; }

        [InputParameter]
        public string Content { get; set; }

        [InputParameter]
        public DateTime? CreatedAt { get; set; }

        [InputParameter]
        public Status? StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_clipboard_add")]
    public class ClipboardAddSchema: BaseSchema {
        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? DeviceId { get; set; }

        [InputParameter]
        public ContentType? TypeId { get; set; }

        [InputParameter]
        public string Content { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; }

        [InputParameter]
        public Status StatusId { get; set; }
    }

    [StoredProcedure("dbo", "webapi_clipboard_update")]
    public class ClipboardUpdateSchema: BaseSchema {
        [InputParameter]
        public int? AccountId { get; set; }

        [InputParameter]
        public int? DeviceId { get; set; }

        [InputParameter]
        public ContentType? TypeId { get; set; }

        [InputParameter]
        public string Content { get; set; }

        [InputParameter]
        public DateTime CreatedAt { get; set; }

        [InputParameter]
        public Status StatusId { get; set; }
    }
}
