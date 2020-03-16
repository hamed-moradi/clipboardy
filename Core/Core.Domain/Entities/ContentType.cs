using Assets.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {

    [Table("ContentType")]
    public partial class ContentType: BaseEntity {

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(16)]
        public string Extension { get; set; }

        [Required, MaxLength(256)]
        public string MIMEType { get; set; }
    }

    public partial class ContentType { }
}
