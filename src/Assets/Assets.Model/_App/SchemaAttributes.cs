using Assets.Model.Base;
using System;

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

  public class CustomParameterAttribute: Attribute {
    public string Name { get; set; }
    public SQLPropType Type { get; set; }
    public int Length { get; set; }
    public bool IsNullable { get; set; }

    public CustomParameterAttribute(string name = null, SQLPropType type = SQLPropType.INT, int length = 0, bool isnull = true) {
      Name = name;
      Type = type;
      Length = length;
      IsNullable = isnull;
    }
  }

  public class InputParameterAttribute: CustomParameterAttribute {
    public InputParameterAttribute(string name = null, SQLPropType type = SQLPropType.INT, int length = 0, bool isnull = true)
        : base(name, type, length, isnull) {
    }
  }

  public class OutputParameterAttribute: CustomParameterAttribute {
    public OutputParameterAttribute(string name = null, SQLPropType type = SQLPropType.INT, int length = 0, bool isnull = true)
        : base(name, type, length, isnull) {
    }
  }

  public class ReturnParameterAttribute: Attribute { }

  public class HelperParameterAttribute: Attribute { }
}
