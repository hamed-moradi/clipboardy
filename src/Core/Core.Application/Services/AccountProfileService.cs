using System.Threading.Tasks;
using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;

namespace Core.Application.Services {
    public class AccountProfileService: IAccountProfileService {
        #region

        public AccountProfileService() {
        }
        #endregion

        public async Task<AccountProfileResult> FirstAsync(AccountProfileGetFirstSchema accountProfile) {
            return null;
        }

        public async Task<int> AddAsync(AccountProfileAddSchema accountProfile) {
            return 0;
        }

        public async Task UpdateAsync(AccountProfileUpdateSchema accountProfile) {
        }

        public async Task CleanForgotPasswordTokensAsync(int accountId) {
        }
    }
}
