using Core.Domain;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountProfileService: GenericService<AccountProfile>, IAccountProfileService {
        #region
        private readonly MsSqlDbContext _msSqlDbContext;

        public AccountProfileService(MsSqlDbContext msSqlDbContext) {
            _msSqlDbContext = msSqlDbContext;
        }
        #endregion
    }
}
