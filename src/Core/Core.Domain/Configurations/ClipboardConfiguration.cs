using Assets.Model.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class ClipboardConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Clipboard>()
        .HasOne(p => p.Account)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Clipboard>()
        .HasOne(p => p.AccountDevice)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Clipboard>()
        .HasOne(p => p.ContentType)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Clipboard>()
        .Property(p => p.status_id)
        .HasDefaultValue(Status.Active);

      modelBuilder.Entity<Clipboard>()
        .Property(p => p.inserted_at)
        .HasDefaultValueSql("now()");
    }
  }
}
