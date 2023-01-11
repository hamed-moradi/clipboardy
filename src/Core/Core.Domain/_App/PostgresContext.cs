using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Domain._App {
  public class PostgresContext: DbContext {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Account>()
        .HasKey(k => k.id);

      modelBuilder.Entity<Account>()
       .Property(p => p.inserted_at).ValueGeneratedOnAdd()
       .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountDevice> AccountDevices { get; set; }
    public DbSet<AccountProfile> AccountProfiles { get; set; }
    public DbSet<AccountProfileType> AccountProfileTypes { get; set; }
    public DbSet<AccountProvider> AccountProviders { get; set; }
    public DbSet<Clipboard> Clipboards { get; set; }
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<GeneralStatus> GeneralStatuses { get; set; }
  }
}
