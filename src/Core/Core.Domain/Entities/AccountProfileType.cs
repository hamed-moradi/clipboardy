using Core.Domain._App;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("AccountProfileType")]
  public class AccountProfileType: BaseEntity {
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
  }
}
