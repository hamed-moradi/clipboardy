using System;

namespace Assets.Model {
    public class UpdatableAttribute: Attribute { }
    public class HelperParameterAttribute: Attribute { }
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
}
