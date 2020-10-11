using System.Threading.Tasks;
using Core.Application.Infrastructure;
using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;

namespace Core.Application.Services {
    public class AccountProfileService: IAccountProfileService {
        #region
        private readonly IStoredProcedureService _storedProcedure;

        public AccountProfileService(
            IStoredProcedureService storedProcedure) {

            _storedProcedure = storedProcedure;
        }
        #endregion

        public async Task<AccountProfileResult> FirstAsync(AccountProfileGetFirstSchema accountProfile) {
            var result = await _storedProcedure.QueryFirstAsync<AccountProfileGetFirstSchema, AccountProfileResult>(accountProfile);
            return result;
        }

        public async Task<int> AddAsync(AccountProfileAddSchema accountProfile) {
            var result = await _storedProcedure.ExecuteScalarAsync<AccountProfileAddSchema, int>(accountProfile);
            return result;
        }

        public async Task UpdateAsync(AccountProfileUpdateSchema accountProfile) {
            await _storedProcedure.ExecuteAsync(accountProfile);
        }

        public async Task CleanForgotPasswordTokensAsync(int accountId) {
        }
    }
}
