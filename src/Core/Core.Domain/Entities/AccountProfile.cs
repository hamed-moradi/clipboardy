using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account_profile")]
  public class AccountProfile: BaseEntity {
    [Key]
    public int id { get; set; }
    [Required]
    public int account_id { get; set; }
    [Required]
    public int type_id { get; set; }
    [MaxLength(64)]
    public string linked_id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime inserted_at { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int status_id { get; set; }
  }
}
