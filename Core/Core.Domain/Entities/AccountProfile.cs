using Assets.Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {

    [Table("AccountProfile")]
    public partial class AccountProfile: BaseEntity {

        [Required]
        public int? AccountId { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }

        [Required]
        public bool? ConfirmedEmail { get; set; }

        [Required, MaxLength(16)]
        public string Phone { get; set; }

        [Required]
        public bool? ConfirmedPhone { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public Status? StatusId { get; set; }
    }

    public partial class AccountProfile {
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
