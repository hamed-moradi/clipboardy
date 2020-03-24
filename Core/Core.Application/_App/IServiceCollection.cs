using Assets.Model.Base;
using Assets.Model.Binding;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application {
    public interface IGenericService<TEntity>: IPredicateMaker<TEntity> where TEntity : BaseEntity {
        List<TEntity> All(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000);
        List<TModel> All<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel;
        List<TEntity> All(TEntity entity, bool tracking = true, int retrieveLimit = 1000);
        List<TModel> All<TModel>(TEntity entity, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel;

        Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000);
        Task<List<TModel>> AllAsync<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel;
        Task<List<TEntity>> AllAsync(TEntity entity, bool tracking = true, int retrieveLimit = 1000);
        Task<List<TModel>> AllAsync<TModel>(TEntity entity, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel;

        TEntity First(long id, bool tracking = true, bool force = false);
        TEntity First(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
        TModel First<TModel>(int id, bool tracking = true, bool force = false) where TModel : BaseModel;
        TModel First<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel;

        Task<TEntity> FirstAsync(long id, bool tracking = true, bool force = false);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
        Task<TModel> FirstAsync<TModel>(int id, bool tracking = true, bool force = false) where TModel : BaseModel;
        Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel;

        (List<TEntity> Result, long TotalCount, long TotalPage) GetPaging(TEntity entity, QuerySetting querysetting, bool tracking = true);
        (List<TEntity> Result, long TotalCount, long TotalPage) GetPaging(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true);
        (List<TModel> Result, long TotalCount, long TotalPage) GetPaging<TModel>(TEntity entity, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel;
        (List<TModel> Result, long TotalCount, long TotalPage) GetPaging<TModel>(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel;

        Task<(List<TEntity> Result, long TotalCount, long TotalPage)> GetPagingAsync(TEntity entity, QuerySetting querysetting, bool tracking = true);
        Task<(List<TEntity> Result, long TotalCount, long TotalPage)> GetPagingAsync(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true);
        Task<(List<TModel> Result, long TotalCount, long TotalPage)> GetPagingAsync<TModel>(TEntity entity, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel;
        Task<(List<TModel> Result, long TotalCount, long TotalPage)> GetPagingAsync<TModel>(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel;

        TEntity Add(TEntity entity);
        TModel Add<TModel>(TEntity entity) where TModel : BaseModel;
        Task<TEntity> AddAsync(TEntity entity);
        Task<TModel> AddAsync<TModel>(TEntity entity) where TModel : BaseModel;

        TEntity Update(TEntity entity, bool needToFetch = true);
        Task<TEntity> UpdateAsync(TEntity entity, bool needToFetch = true);

        bool Remove(long id);
        bool Remove(TEntity entity);
        bool Remove<TModel>(TModel viewModel) where TModel : BaseModel;
        bool Remove(Expression<Func<TEntity, bool>> predicate);

        Task<bool> RemoveAsync(long id);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveAsync<TModel>(TModel viewModel) where TModel : BaseModel;
        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);

        bool Delete(long id);
        bool Delete(TEntity entity);

        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteAsync(TEntity entity);

        int SaveAll();
        Task<int> SaveAllAsync();
    }

    public interface IAccountService: IGenericService<Account> {
        Task<BaseViewModel> Signup(SignupBindingModel signupModel);
        Task<BaseViewModel> Signin(SigninBindingModel signinModel);
    }

    public interface IAccountDeviceService: IGenericService<AccountDevice> { }

    public interface IAccountProfileService: IGenericService<AccountProfile> {
        Task<bool> CleanForgotPasswordTokensAsync(int accountId);
    }

    public interface IClipboardService: IGenericService<Clipboard> { }

    public interface IContentTypeService: IGenericService<ContentType> {
        Task BulkInsertAsync(List<ContentType> contentTypes);
    }
}
