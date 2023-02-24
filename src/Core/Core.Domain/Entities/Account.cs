﻿using Core.Domain._App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities {
  [Table("account")]
  public partial class Account: BaseEntity {
    [Key]
    public int id { get; set; }
    [Required, MaxLength(32)]
    public string username { get; set; }
    [MaxLength(256)]
    public string password { get; set; }
    public int status_id { get; set; }
    public DateTime? last_signedin_at { get; set; }
    public DateTime inserted_at { get; set; }
  }
}