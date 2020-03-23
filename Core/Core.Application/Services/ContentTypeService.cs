using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ContentTypeService: GenericService<ContentType>, IContentTypeService {
        #region
        private readonly MsSQLDbContext _msSQLDbContext;

        public ContentTypeService(MsSQLDbContext msSQLDbContext) {
            _msSQLDbContext = msSQLDbContext;
        }
        #endregion

        public async Task BulkInsertAsync(List<ContentType> contentTypes) {
            contentTypes.ForEach(item => {
                _msSQLDbContext.Add(item);
            });
            await _msSQLDbContext.SaveChangesAsync();
        }
    }
}
