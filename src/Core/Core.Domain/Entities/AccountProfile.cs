using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("AccountProfile")]
  public partial class AccountProfile: BaseEntity {
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int TypeId { get; set; }
    [MaxLength(64)]
    public string LinkedId { get; set; }
    public int StatusId { get; set; }
    public DateTime InsertedAt { get; set; }
  }

  public partial class AccountProfile {
    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; }
  }
}
