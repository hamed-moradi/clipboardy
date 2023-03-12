using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account_device")]
  public partial class AccountDevice: BaseEntity {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required]
    public int account_id { get; set; }
    [Required, MaxLength(64)]
    public string device_key { get; set; }
    [Required, MaxLength(128)]
    public string device_name { get; set; }
    [Required, MaxLength(64)]
    public string device_type { get; set; }
    public string status { get; set; }
    public DateTime inserted_at { get; set; }
  }

  public partial class AccountDevice {
    [ForeignKey(nameof(account_id))]
    public Account Account { get; set; }
  }
}
