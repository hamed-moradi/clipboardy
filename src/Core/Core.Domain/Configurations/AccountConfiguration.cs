﻿using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Account>()
        .HasKey(k => k.Id);

      modelBuilder.Entity<Account>()
        .Property(p => p.StatusId)
        .HasDefaultValue(Status.Active);

      modelBuilder.Entity<Account>()
        .Property(p => p.InsertedAt)
        .HasDefaultValueSql("now()");
    }
  }
}
