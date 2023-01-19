using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account_device")]
  public class AccountDevice: BaseEntity {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int account_id { get; set; }
    [Required, MaxLength(32)]
    public string device_id { get; set; }
    [Required, MaxLength(128)]
    public string device_name { get; set; }
    [Required, MaxLength(64)]
    public string device_type { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime inserted_at { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int status_id { get; set; }
  }
}
