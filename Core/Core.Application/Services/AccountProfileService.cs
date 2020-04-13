using System.Threading.Tasks;
using Assets.Model.StoredProcResult;
using Core.Domain.StoredProcSchema;

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

        public async Task AddAsync(AccountProfileAddSchema accountProfile) {
            await _storedProcedure.ExecuteAsync(accountProfile);
        }

        public async Task UpdateAsync(AccountProfileUpdateSchema accountProfile) {
            await _storedProcedure.ExecuteAsync(accountProfile);
        }

        public async Task CleanForgotPasswordTokensAsync(AccountProfileCleanTokensSchema accountProfile) {
            await _storedProcedure.ExecuteAsync(accountProfile);
        }
    }
}
