using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class AccountProfileTypeConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<AccountProfileType>()
        .HasIndex(p => p.title)
        .IsUnique();
    }
  }
}
