using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class GeneralStatusConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<GeneralStatus>()
        .HasIndex(p => p.title)
        .IsUnique();
    }
  }
}
