using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ClipboardService: GenericService<Clipboard>, IClipboardService {
        #region
        private readonly MsSQLDbContext _msSQLDbContext;

        public ClipboardService(MsSQLDbContext msSQLDbContext) {
            _msSQLDbContext = msSQLDbContext;
        }
        #endregion
    }
}
