using Core.Domain._App;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("ContentType")]
  public class ContentType: BaseEntity {
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(64)]
    public string Name { get; set; }
    [Required, MaxLength(16)]
    public string Extension { get; set; }
    [Required, MaxLength(128)]
    public string MIMEType { get; set; }
  }
}
