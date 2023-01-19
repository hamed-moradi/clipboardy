using Core.Application._App;
using Core.Domain.Entities;
using System.Threading.Tasks;

namespace Core.Application.Services {
  public class AccountProfileService: BaseService<AccountProfile>, IAccountProfileService {
    #region

    public AccountProfileService() {
    }
    #endregion

    public async Task CleanForgotPasswordTokensAsync(int accountId) {
    }
  }
}
