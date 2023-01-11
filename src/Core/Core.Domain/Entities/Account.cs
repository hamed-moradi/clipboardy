using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account")]
  public class Account {
    [Key]
    public int id { get; set; }
    [Required, MaxLength(32)]
    public string username { get; set; }
    [Required, MaxLength(256)]
    public string password { get; set; }
    [Required]
    public int provider_id { get; set; }
    public DateTime? last_signedin_at { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime inserted_at { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int status_id { get; set; }
  }
}
