using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.StoredProcedure.Result {
    public class ClipboardResult: PagingResult {
        public string Content { get; set; }
        public int TypeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
