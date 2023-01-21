using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Account>()
        .Property(p => p.status_id)
        .HasDefaultValue(Status.Active);

      modelBuilder.Entity<Account>()
        .Property(p => p.inserted_at)
        .HasDefaultValueSql("now()");
    }
  }
}
