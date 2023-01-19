﻿using Core.Domain._App;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Application {
  public interface IBaseService<TEntity> where TEntity : BaseEntity {
    DbSet<TEntity> Entity { get; }
    TEntity Add(TEntity entity, bool save = true);
    Task<TEntity> AddAsync(TEntity entity, bool save = true);
  }

  public interface IAccountService: IBaseService<Account> {
    //AccountResult GetById(int id);
    //Task<Account> AuthenticateAsync(string token);
    //AccountResult First(AccountGetFirstSchema account);
    //Task<AccountResult> FirstAsync(AccountGetFirstSchema account);
    //Task<int> AddAsync(AccountAddSchema account);
    //Task UpdateAsync(AccountUpdateSchema account);
    //Task<IServiceResult> SignupAsync(SignupBindingModel signupModel);
    //Task<IServiceResult> ExternalSignupAsync(ExternalUserBindingModel externalUser);
    //Task<IServiceResult> SigninAsync(SigninBindingModel signinModel);
    //Task<IServiceResult> ExternalSigninAsync(ExternalUserBindingModel externalUser, AccountProfileResult accountProfile);
  }

  public interface IAccountDeviceService: IBaseService<AccountDevice> {
    //Task<AccountDeviceResult> FirstAsync(AccountDeviceGetFirstSchema accountDevice);
    //Task<int> AddAsync(AccountDeviceAddSchema accountDevice);
    //Task UpdateAsync(AccountDeviceUpdateSchema accountDevice);
  }

  public interface IAccountProfileService: IBaseService<AccountProfile> {
    //Task<AccountProfileResult> FirstAsync(AccountProfileGetFirstSchema accountProfile);
    //Task<int> AddAsync(AccountProfileAddSchema accountProfile);
    //Task UpdateAsync(AccountProfileUpdateSchema accountProfile);
    Task CleanForgotPasswordTokensAsync(int accountId);
  }

  public interface IClipboardService: IBaseService<Clipboard> {
    //Task<ClipboardResult> FirstAsync(ClipboardGetFirstSchema clipboard);
    //Task<IEnumerable<ClipboardResult>> PagingAsync(ClipboardGetPagingSchema clipboard);
    //Task<int> AddAsync(ClipboardAddSchema clipboard);
  }

  //public interface IContentTypeService: IGenericService<ContentType> {
  //    Task BulkInsertAsync(List<ContentType> contentTypes);
  //}
}
