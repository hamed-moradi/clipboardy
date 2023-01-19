using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("Account")]
  public class Account: BaseEntity {
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(32)]
    public string Username { get; set; }
    [MaxLength(256)]
    public string Password { get; set; }
    [Required]
    public int ProviderId { get; set; }
    public int StatusId { get; set; }
    public DateTime? LastSignedinAt { get; set; }
    public DateTime InsertedAt { get; set; }
  }
}
