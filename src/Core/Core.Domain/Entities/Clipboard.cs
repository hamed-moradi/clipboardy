using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
    [Table("clipboard")]
    public class Clipboard {
        [Key]
        public int id { get; set; }
        [Required]
        public int account_id { get; set; }
        [Required]
        public int device_id { get; set; }
        [Required]
        public int type_id { get; set; }
        [Required]
        public string content { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime inserted_at { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int status_id { get; set; }
    }
}
