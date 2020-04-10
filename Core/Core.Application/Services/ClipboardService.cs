using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ClipboardService: GenericService<Clipboard>, IClipboardService {
        #region

        public ClipboardService(
            MsSQLDbContext msSQLDbContext) : base(dbContext: msSQLDbContext) {

        }
        #endregion
    }
}
