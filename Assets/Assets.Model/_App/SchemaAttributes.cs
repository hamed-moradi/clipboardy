using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Model {
    public class SchemaAttribute: Attribute {
        public string Name { get; set; }
        public SchemaAttribute(string name = null) {
            Name = name;
        }
    }

    public class StoredProcedureAttribute: Attribute {
        public string Schema { get; set; }
        public string Name { get; set; }
        public StoredProcedureAttribute(string schema = null, string name = null) {
            Schema = schema;
            Name = name;
        }
    }

    public class InputParameterAttribute: Attribute {
        public string Name { get; set; }
        public InputParameterAttribute(string name = null) {
            Name = name;
        }
    }

    public class OutputParameterAttribute: Attribute {
        public string Name { get; set; }
        public OutputParameterAttribute(string name = null) {
            Name = name;
        }
    }
    public class ReturnParameterAttribute: Attribute { }

    public class ErrorAttribute: Attribute {
        public string Message { get; set; }

        public ErrorAttribute(string message) {
            Message = message;
        }
    }

    public class HelperParameterAttribute: Attribute { }
}
