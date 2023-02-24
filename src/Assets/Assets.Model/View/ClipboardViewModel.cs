using Assets.Model.Base;
using System;

namespace Assets.Model.View {
  public class ClipboardViewModel: IBaseViewModel {
    public int? Id { get; set; }
    public int? TypeId { get; set; }
    public string TypeName { get; set; }
    public string Icon { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
  }
}
