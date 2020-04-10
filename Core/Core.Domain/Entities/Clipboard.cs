using Assets.Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {

    [Table("Clipboard")]
    public partial class Clipboard: BaseEntity {
        [Required]
        public int? AccountId { get; set; }

        [Required]
        public int? DeviceId { get; set; }

        [Required]
        public int? TypeId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime? Priority { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public Status? StatusId { get; set; }
    }

    public partial class Clipboard {
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        [ForeignKey(nameof(DeviceId))]
        public AccountDevice AccountDevice { get; set; }

        [ForeignKey(nameof(TypeId))]
        public ContentType ContentType { get; set; }
    }
}
