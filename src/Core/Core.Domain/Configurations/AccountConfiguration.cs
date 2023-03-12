using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Clipboard>()
        .Property(p => p.status)
        .HasDefaultValue(Status.ACTIVE);

      modelBuilder.Entity<Account>()
        .Property(p => p.inserted_at)
        .HasDefaultValueSql("now()");
    }
  }
}
