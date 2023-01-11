using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Domain.Configurations {
  public class AccountConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Account>()
        .HasKey(k => k.id);

      modelBuilder.Entity<Account>()
        .Property(p => p.inserted_at).ValueGeneratedOnAdd()
        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
    }
  }
}
