using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ContentTypeService: GenericService<ContentType> {
        #region

        public ContentTypeService(
            MsSQLDbContext msSQLDbContext) : base(dbContext: msSQLDbContext) {

        }
        #endregion

        public async Task BulkInsertAsync(List<ContentType> contentTypes) {
            contentTypes.ForEach(item => {
                GetMsSQLDbContext().Add(item);
            });
            await GetMsSQLDbContext().SaveChangesAsync();
        }
    }
}
