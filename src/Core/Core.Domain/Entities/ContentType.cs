using Core.Domain._App;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("content_type")]
  public class ContentType: BaseEntity {
    [Key]
    public int id { get; set; }
    [Required, MaxLength(64)]
    public string name { get; set; }
    [Required, MaxLength(16)]
    public string ext { get; set; }
    [Required, MaxLength(128)]
    public string mime_type { get; set; }
  }
}
