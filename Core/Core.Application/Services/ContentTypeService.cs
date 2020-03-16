using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ContentTypeService: GenericService<ContentType>, IContentTypeService {
        #region
        private readonly MsSqlDbContext _msSqlDbContext;

        public ContentTypeService(MsSqlDbContext msSqlDbContext) {
            _msSqlDbContext = msSqlDbContext;
        }
        #endregion

        public async Task BulkInsertAsync(List<ContentType> contentTypes) {
            contentTypes.ForEach(item => {
                _msSqlDbContext.Add(item);
            });
            await _msSqlDbContext.SaveChangesAsync();
        }
    }
}
