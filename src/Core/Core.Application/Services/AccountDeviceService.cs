using System.Threading.Tasks;

namespace Core.Application.Services {
  public class AccountDeviceService: IAccountDeviceService {
    #region

    public AccountDeviceService() {

    }
    #endregion

    public async Task<AccountDeviceResult> FirstAsync(AccountDeviceGetFirstSchema accountDevice) {
      return null;
    }

    public async Task<int> AddAsync(AccountDeviceAddSchema accountDevice) {
      return 0;
    }

    public async Task UpdateAsync(AccountDeviceUpdateSchema accountDevice) {
    }
  }
}
