using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("Clipboard")]
  public class Clipboard: BaseEntity {
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int DeviceId { get; set; }
    [Required]
    public int TypeId { get; set; }
    [Required]
    public string Content { get; set; }
    public int StatusId { get; set; }
    public DateTime InsertedAt { get; set; }
  }
}
