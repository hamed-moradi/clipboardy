using Assets.Model.Base;
using System;

namespace Assets.Model.Binding {
  public class ClipboardGetBindingModel: PagingOption {
    public int? TypeId { get; set; } = 34;
    public string Content { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
  }

  public class ClipboardAddBindingModel: IBaseBindingModel {
    public int? TypeId { get; set; }
    public string Content { get; set; }
  }
}
