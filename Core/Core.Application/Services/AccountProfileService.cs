using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountProfileService: GenericService<AccountProfile>, IAccountProfileService {
        #region
        private readonly MsSQLDbContext _msSQLDbContext;

        public AccountProfileService(
            MsSQLDbContext msSQLDbContext) {

            _msSQLDbContext = msSQLDbContext;
        }
        #endregion

        public async Task<bool> CleanForgotPasswordTokensAsync(int accountId) {
            var query = $"UPDATE dbo.AccountProfile SET ForgotPasswordToken = NULL WHERE AccountId = {accountId};";
            var result = await _msSQLDbContext.Database.ExecuteSqlRawAsync(query);
            if(result > 0)
                return true;
            return false;
        }
    }
}
