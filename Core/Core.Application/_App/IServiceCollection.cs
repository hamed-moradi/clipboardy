using Assets.Model.Base;
using Assets.Model.Binding;
using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application {
    public interface IAccountService {
        Task<AccountAuthenticateResult> AuthenticateAsync(AccountAuthenticateSchema schema);
        Task<AccountResult> FirstAsync(AccountGetFirstSchema account);
        Task<int> AddAsync(AccountAddSchema account);
        Task UpdateAsync(AccountUpdateSchema account);
        Task<IServiceResult> SignupAsync(SignupBindingModel signupModel);
        Task<IServiceResult> ExternalSignupAsync(ExternalUserBindingModel externalUser);
        Task<IServiceResult> SigninAsync(SigninBindingModel signinModel);
        Task<IServiceResult> ExternalSigninAsync(ExternalUserBindingModel externalUser, AccountProfileResult accountProfile);
    }

    public interface IAccountDeviceService {
        Task<AccountDeviceResult> FirstAsync(AccountDeviceGetFirstSchema accountDevice);
        Task<int> AddAsync(AccountDeviceAddSchema accountDevice);
        Task UpdateAsync(AccountDeviceUpdateSchema accountDevice);
    }

    public interface IAccountProfileService {
        Task<AccountProfileResult> FirstAsync(AccountProfileGetFirstSchema accountProfile);
        Task<int> AddAsync(AccountProfileAddSchema accountProfile);
        Task UpdateAsync(AccountProfileUpdateSchema accountProfile);
        Task CleanForgotPasswordTokensAsync(AccountProfileCleanTokensSchema accountProfile);
    }

    public interface IClipboardService {
        Task<ClipboardResult> FirstAsync(ClipboardGetFirstSchema clipboard);
        Task<IEnumerable<ClipboardResult>> PagingAsync(ClipboardGetPagingSchema clipboard);
        Task AddAsync(ClipboardAddSchema clipboard);
    }

    //public interface IContentTypeService: IGenericService<ContentType> {
    //    Task BulkInsertAsync(List<ContentType> contentTypes);
    //}
}
