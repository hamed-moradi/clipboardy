using Assets.Model.Base;
using System;

namespace Assets.Model.Binding
{
    public class ClipboardGetBindingModel : PagingOption
    {
        public int AccountId { get; set; }
        public int DeviceId { get; set; }
        public int? TypeId { get; set; }
        public string Content { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ClipboardAddBindingModel : IBaseBindingModel
    {
        public int? TypeId { get; set; }
        public string Content { get; set; }
    }

    public class ClipboardUpdateBindingModel : IBaseBindingModel
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string Content { get; set; }
    }
}
