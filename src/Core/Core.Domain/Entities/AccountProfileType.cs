﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account_profile_type")]
  public class AccountProfileType {
    [Key]
    public int id { get; set; }
    [Required]
    public string title { get; set; }
    public string desc { get; set; }
  }
}
