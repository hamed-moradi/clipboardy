using Core.Domain.Configurations;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain._App {
  public class PostgresContext: DbContext {
    #region ctors
    public PostgresContext(DbContextOptions options) : base(options) { }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      AccountConfiguration.OnModelCreating(modelBuilder);
      AccountDeviceConfiguration.OnModelCreating(modelBuilder);
      AccountProfileConfiguration.OnModelCreating(modelBuilder);
      ClipboardConfiguration.OnModelCreating(modelBuilder);
      ContentTypeConfiguration.OnModelCreating(modelBuilder);
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountDevice> AccountDevices { get; set; }
    public DbSet<AccountProfile> AccountProfiles { get; set; }
    public DbSet<Clipboard> Clipboards { get; set; }
    public DbSet<ContentType> ContentTypes { get; set; }
  }
}
