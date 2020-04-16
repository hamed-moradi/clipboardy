using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Model.View {
    public class ClipboardViewModel: IBaseViewModel {
        public int? TypeId { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
