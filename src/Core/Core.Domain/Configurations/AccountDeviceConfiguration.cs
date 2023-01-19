using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountDeviceConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<AccountDevice>()
        .HasKey(k => k.Id);

      modelBuilder.Entity<AccountDevice>()
        .HasOne(p => p.Account)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<AccountDevice>()
        .Property(p => p.StatusId)
        .HasDefaultValue(Status.Active);

      modelBuilder.Entity<AccountDevice>()
        .Property(p => p.InsertedAt)
        .HasDefaultValueSql("now()");
    }
  }
}
