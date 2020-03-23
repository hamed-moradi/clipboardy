using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain {
    public class MsSQLDbContext: DbContext {
        #region ctor
        public MsSQLDbContext(DbContextOptions contextOptions) : base(contextOptions) { }
        #endregion

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountDevice> AccountDevices { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<Clipboard> Clipboard { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //modelBuilder.Entity<AccountDevice>().HasOne(o => o.Account);
            //modelBuilder.Entity<AccountProfile>().HasOne(o => o.Account);
            //modelBuilder.Entity<Clipboard>().HasOne(o => o.AccountDevice);
            base.OnModelCreating(modelBuilder);
        }
    }
}
