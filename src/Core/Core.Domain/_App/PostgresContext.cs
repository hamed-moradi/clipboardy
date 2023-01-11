using Core.Domain.Configurations;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain._App {
  public class PostgresContext: DbContext {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      AccountConfiguration.OnModelCreating(modelBuilder);
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
