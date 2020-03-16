using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountDeviceService: GenericService<AccountDevice>, IAccountDeviceService {
        #region
        private readonly MsSqlDbContext _msSqlDbContext;

        public AccountDeviceService(MsSqlDbContext msSqlDbContext) {
            _msSqlDbContext = msSqlDbContext;
        }
        #endregion
    }
}
