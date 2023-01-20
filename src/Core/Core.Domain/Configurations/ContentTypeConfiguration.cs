using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class ContentTypeConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<ContentType>()
        .HasIndex(p => p.extension)
        .IsUnique();
    }
  }
}
