﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("general_status")]
  public class AccountProvider {
    [Key]
    public int id { get; set; }
    [Required]
    public string title { get; set; }
    public string desc { get; set; }
  }
}
