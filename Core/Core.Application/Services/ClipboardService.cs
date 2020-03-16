using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ClipboardService: GenericService<Clipboard>, IClipboardService {
        #region
        private readonly MsSqlDbContext _msSqlDbContext;

        public ClipboardService(MsSqlDbContext msSqlDbContext) {
            _msSqlDbContext = msSqlDbContext;
        }
        #endregion
    }
}
