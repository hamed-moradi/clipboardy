using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Model.Binding {
    public class ClipboardGetBindingModel: BaseBindingModel {
        public int? TypeId { get; set; } = 34;
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class ClipboardAddBindingModel: BaseBindingModel {
        public int? TypeId { get; set; }
        public string Content { get; set; }
    }
}
