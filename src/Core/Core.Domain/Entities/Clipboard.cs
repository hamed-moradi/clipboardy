using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("clipboard")]
  public partial class Clipboard: BaseEntity {
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
    public int status_id { get; set; }
    public DateTime inserted_at { get; set; }
  }

  public partial class Clipboard {
    [ForeignKey(nameof(account_id))]
    public Account Account { get; set; }

    [ForeignKey(nameof(device_id))]
    public AccountDevice AccountDevice { get; set; }

    [ForeignKey(nameof(type_id))]
    public ContentType ContentType { get; set; }
  }
}
