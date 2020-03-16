using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain {
    public class MsSqlDbContext: DbContext {
        #region ctor
        public MsSqlDbContext(DbContextOptions contextOptions) : base(contextOptions) { }
        #endregion

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountDevice> AccountDevices { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<Clipboard> Clipboard { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
    }
}
