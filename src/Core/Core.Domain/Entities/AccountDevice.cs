using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("AccountDevice")]
  public partial class AccountDevice: BaseEntity {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required, MaxLength(64)]
    public string DeviceId { get; set; }
    [Required, MaxLength(128)]
    public string DeviceName { get; set; }
    [Required, MaxLength(64)]
    public string DeviceType { get; set; }
    public int StatusId { get; set; }
    public DateTime InsertedAt { get; set; }
  }

  public partial class AccountDevice {
    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; }
  }
}
