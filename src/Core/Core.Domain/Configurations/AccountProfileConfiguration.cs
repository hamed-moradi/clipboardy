using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountProfileConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<AccountProfile>()
        .HasOne(p => p.Account)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<AccountProfile>()
        .Property(p => p.status)
        .HasDefaultValue(Status.ACTIVE);

      modelBuilder.Entity<AccountProfile>()
        .Property(p => p.inserted_at)
        .HasDefaultValueSql("now()");

      modelBuilder.Entity<AccountProfile>()
        .HasIndex(p => new { p.account_id, p.linked_key })
        .IsUnique();
    }
  }
}
