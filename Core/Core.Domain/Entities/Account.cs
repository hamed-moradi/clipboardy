using Assets.Model.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {

    [Table("Account")]
    public partial class Account: BaseEntity {

        [Required, MaxLength(32)]
        public string Username { get; set; }

        [Required, MaxLength(512)]
        public string Password { get; set; }

        [Required]
        public AccountProvider? ProviderId { get; set; }

        public DateTime? LastSignedinAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public Status? StatusId { get; set; }
    }

    public partial class Account { }
}
