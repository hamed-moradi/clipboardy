using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountDeviceService: GenericService<AccountDevice>, IAccountDeviceService {
        #region

        public AccountDeviceService(
            MsSQLDbContext msSQLDbContext) : base(dbContext: msSQLDbContext) {

        }
        #endregion
    }
}
