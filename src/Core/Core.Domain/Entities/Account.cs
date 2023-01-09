using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities {
    public class Account {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(32)]
        public string Username { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [Required]
        public int ProviderId { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
    }
}
