using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account_profile")]
  public partial class AccountProfile: BaseEntity {
    [Key]
    public int id { get; set; }
    [Required]
    public int account_id { get; set; }
    [Required]
    public string profile_type { get; set; }
    [MaxLength(64)]
    public string linked_key { get; set; }
    public int status_id { get; set; }
    public DateTime inserted_at { get; set; }
  }

  public partial class AccountProfile {
    [ForeignKey(nameof(account_id))]
    public Account Account { get; set; }
  }
}
