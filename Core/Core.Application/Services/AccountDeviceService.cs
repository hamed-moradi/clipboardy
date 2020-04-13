using Assets.Model.StoredProcResult;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.StoredProcSchema;
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

        public async Task AddAsync(AccountDeviceAddSchema accountDevice) {
            await _storedProcedure.ExecuteAsync(accountDevice);
        }

        public async Task UpdateAsync(AccountDeviceUpdateSchema accountDevice) {
            await _storedProcedure.ExecuteAsync(accountDevice);
        }
    }
}
