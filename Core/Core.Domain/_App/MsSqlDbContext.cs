using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain {
    public class MsSqlDbContext: DbContext {
        #region ctor
        public MsSqlDbContext(DbContextOptions contextOptions) : base(contextOptions) { }
        #endregion

        public DbSet<ContentType> ContentTypes { get; set; }
    }
}
