using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.View;
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

        List<TEntity> GetPaging(TEntity entity, bool tracking = true);
        List<TEntity> GetPaging(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
        List<TModel> GetPaging<TModel>(TEntity entity, bool tracking = true) where TModel : BaseModel;
        List<TModel> GetPaging<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel;

        Task<List<TEntity>> GetPagingAsync(TEntity entity, bool tracking = true);
        Task<List<TEntity>> GetPagingAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true);
        Task<List<TModel>> GetPagingAsync<TModel>(TEntity entity, bool tracking = true) where TModel : BaseModel;
        Task<List<TModel>> GetPagingAsync<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel;

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);

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

        int Save();
        Task<int> SaveAsync();
    }

    public interface IContentTypeService: IGenericService<ContentType> { }
}
