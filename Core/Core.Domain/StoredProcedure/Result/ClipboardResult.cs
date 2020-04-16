using Assets.Model.Base;
using System;

namespace Core.Domain.StoredProcedure.Result {
    public class ClipboardResult: PagingResult {
        public string Content { get; set; }
        public int TypeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
