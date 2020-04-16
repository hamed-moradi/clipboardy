using Core.Application.Infrastructure;
using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountDeviceService: IAccountDeviceService {
        #region
        private readonly IStoredProcedureService _storedProcedure;

        public AccountDeviceService(
            IStoredProcedureService storedProcedure) {

            _storedProcedure = storedProcedure;
        }
        #endregion

        public async Task<AccountDeviceResult> FirstAsync(AccountDeviceGetFirstSchema accountDevice) {
            var result = await _storedProcedure.QueryFirstAsync<AccountDeviceGetFirstSchema, AccountDeviceResult>(accountDevice);
            return result;
        }

        public async Task<int> AddAsync(AccountDeviceAddSchema accountDevice) {
            var result = await _storedProcedure.ExecuteScalarAsync<AccountDeviceAddSchema, int>(accountDevice);
            return result;
        }

        public async Task UpdateAsync(AccountDeviceUpdateSchema accountDevice) {
            await _storedProcedure.ExecuteAsync(accountDevice);
        }
    }
}
