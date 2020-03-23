using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Model.Common {
    public class EmailModel {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string AttachmentPath { get; set; }
    }
}
