using System.Threading.Tasks;

namespace Core.Application.Services {
  public class AccountProfileService: IAccountProfileService {
    #region

    public AccountProfileService() {
    }
    #endregion

    public async Task CleanForgotPasswordTokensAsync(int accountId) {
    }
  }
}
