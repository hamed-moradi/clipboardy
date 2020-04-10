using Assets.Model;
using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application {
    public class GenericService<TEntity>: PredicateMaker<TEntity>, IGenericService<TEntity> where TEntity : BaseEntity {
        #region ctor
        private readonly IMapper _mapper;
        private readonly MsSQLDbContext _msSQLDbContext;
        private readonly PropertyMapper _propertyMapper;

        public GenericService(
            MsSQLDbContext dbContext = null,
            IMapper mapper = null,
            PropertyMapper propertyMapper = null) : base(dbContext, mapper) {

            var serviceLocator = ServiceLocator.Current;
            _msSQLDbContext = dbContext ?? serviceLocator.GetInstance<MsSQLDbContext>();
            _mapper = mapper ?? serviceLocator.GetInstance<IMapper>();
            _propertyMapper = propertyMapper ?? serviceLocator.GetInstance<PropertyMapper>();
        }
        #endregion

        public MsSQLDbContext GetMsSQLDbContext(IDbContextTransaction transaction = null) {
            if(transaction != null)
                _msSQLDbContext.Database.UseTransaction(transaction.GetDbTransaction());
            return _msSQLDbContext;
        }

        #region all
        /// <summary>
        /// Get top 1000 rows by predicate query
        /// </summary>
        /// <param name="predicate">The predicate (bypass by null)</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected entity</returns>
        public List<TEntity> All(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000) {
            var query = GenerateQuery(predicate, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() > retrieveLimit) {
                // Your retrieve limit has been reached
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return query.ToList();
        }

        /// <summary>
        /// Get asynchrony top 1000 rows by predicate query
        /// </summary>
        /// <param name="predicate">The predicate (bypass by null)</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected entity</returns>
        public async Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000) {
            var query = GenerateQuery(predicate, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() > retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get top 1000 rows as TModel by predicate query
        /// </summary>
        /// <param name="predicate">The predicate (bypass by null)</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected model</returns>
        public List<TModel> All<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000)
            where TModel : BaseModel {
            var query = GenerateQuery<TModel>(predicate, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() > retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return query.ToList();
        }

        /// <summary>
        /// Get asynchrony top 1000 rows as TModel by predicate query
        /// </summary>
        /// <param name="predicate">The predicate (bypass by null)</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected model</returns>
        public async Task<List<TModel>> AllAsync<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, int retrieveLimit = 1000)
            where TModel : BaseModel {
            var query = GenerateQuery<TModel>(predicate, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() > retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get top 1000 rows by TEntity
        /// </summary>
        /// <param name="entity">The entity (bypass by null)</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected entity</returns>
        public List<TEntity> All(TEntity entity, bool tracking = true, int retrieveLimit = 1000) {
            var query = GenerateQuery(entity, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() >= retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return query.ToList();
        }

        /// <summary>
        /// Get asynchrony top 1000 rows by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected entity</returns>
        public async Task<List<TEntity>> AllAsync(TEntity entity, bool tracking = true, int retrieveLimit = 1000) {
            var query = GenerateQuery(entity, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() >= retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get top 1000 rows as TModel by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected model</returns>
        public List<TModel> All<TModel>(TEntity entity, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(entity, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() >= retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return query.ToList();
        }

        /// <summary>
        /// Get asynchrony top 1000 rows as TModel by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="retrieveLimit">Pass "zero" for reitrieve all data</param>
        /// <returns>A list of selected model</returns>
        public async Task<List<TModel>> AllAsync<TModel>(TEntity entity, bool tracking = true, int retrieveLimit = 1000) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(entity, tracking: tracking);
            if(retrieveLimit != 0 && query.Count() >= retrieveLimit) {
                throw new Exception(InternalMessage.RetrieveLimit, new Exception(GlobalVariables.SystemGeneratedMessage));
            }
            return await query.ToListAsync();
        }
        #endregion

        #region first
        /// <summary>
        /// Get single record by id
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="force">Throw exception if nothing found</param>
        /// <returns>Selected entity</returns>
        public TEntity First(long id, bool tracking = true, bool force = false) {
            var selectedItem = GenerateQuery(s => s.Id == id, tracking: tracking).FirstOrDefault();
            if(selectedItem == null) {
                if(force) {
                    throw new Exception(InternalMessage.ObjectNotFound, new Exception(GlobalVariables.SystemGeneratedMessage));
                }
                else {
                    return null;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// Get single record by predicate query
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="force">Throw exception if nothing found</param>
        /// <returns>Selected entity</returns>
        public TEntity First(Expression<Func<TEntity, bool>> predicate, bool tracking = true) {
            return GenerateQuery(predicate, tracking: tracking).FirstOrDefault();
        }

        /// <summary>
        /// Get single record by id
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="force">Throw exception if nothing found</param>
        /// <returns>Selected model</returns>
        public TModel First<TModel>(int id, bool tracking = true, bool force = false) where TModel : BaseModel {
            var selectedItem = GenerateQuery<TModel>(q => q.Id == id, tracking: tracking).FirstOrDefault();
            if(selectedItem == null) {
                if(force) {
                    throw new Exception(InternalMessage.ObjectNotFound, new Exception(GlobalVariables.SystemGeneratedMessage));
                }
                else {
                    return null;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// Get single record by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Selected model</returns>
        public TModel First<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel {
            return GenerateQuery<TModel>(predicate, tracking: tracking).FirstOrDefault();
        }

        /// <summary>
        /// Get asynchrony single record by id
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="force">Throw exception if nothing found</param>
        /// <returns>Selected entity</returns>
        public async Task<TEntity> FirstAsync(long id, bool tracking = true, bool force = false) {
            var selectedItem = await GenerateQuery(q => q.Id == id, tracking: tracking).FirstOrDefaultAsync();
            if(selectedItem == null) {
                if(force) {
                    throw new Exception(InternalMessage.ObjectNotFound, new Exception(GlobalVariables.SystemGeneratedMessage));
                }
                else {
                    return null;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// Get asynchrony single record by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Selected entity</returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true) {
            return await GenerateQuery(predicate, tracking: tracking).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get asynchrony single record by id
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="force">Throw exception if nothing found</param>
        /// <returns>Selected model</returns>
        public async Task<TModel> FirstAsync<TModel>(int id, bool tracking = true, bool force = false) where TModel : BaseModel {
            var selectedItem = await GenerateQuery<TModel>(q => q.Id == id, tracking: tracking).FirstOrDefaultAsync();
            if(selectedItem == null) {
                if(force) {
                    throw new Exception(InternalMessage.ObjectNotFound, new Exception(GlobalVariables.SystemGeneratedMessage));
                }
                else {
                    return null;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// Get asynchrony single record by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Selected model</returns>
        public async Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel {
            return await GenerateQuery<TModel>(predicate, tracking: tracking).FirstOrDefaultAsync();
        }
        #endregion

        #region paging
        /// <summary>
        /// Get paged result by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>Paged list of TEntity</returns>
        public (List<TEntity> Result, long TotalCount, long TotalPage) GetPaging(TEntity entity, QuerySetting guerysetting, bool tracking = true) {
            var query = GenerateQuery(out var totalCount, out var totalPage, guerysetting, entity, tracking: tracking);
            return (query.ToList(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TEntity</returns>
        public (List<TEntity> Result, long TotalCount, long TotalPage) GetPaging(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) {
            var query = GenerateQuery(out var totalCount, out var totalPage, querysetting, predicate, tracking: tracking);
            return (query.ToList(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result by TEntity
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Paged list of TModel</returns>
        public (List<TModel> Result, long TotalCount, long TotalPage) GetPaging<TModel>(TEntity model, QuerySetting guerysetting, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(out var totalCount, out var totalPage, guerysetting, model, tracking: tracking);
            return (query.ToList(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TModel</returns>
        public (List<TModel> Result, long TotalCount, long TotalPage) GetPaging<TModel>(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(out var totalCount, out var totalPage, querysetting, predicate, tracking: tracking);
            return (query.ToList(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result asynchrony by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>Paged list of TEntity</returns>
        public async Task<(List<TEntity> Result, long TotalCount, long TotalPage)> GetPagingAsync(TEntity model, QuerySetting guerysetting, bool tracking = true) {
            var query = GenerateQuery(out var totalCount, out var totalPage, guerysetting, model, tracking: tracking);
            return (await query.ToListAsync(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result asynchrony by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TEntity</returns>
        public async Task<(List<TEntity> Result, long TotalCount, long TotalPage)> GetPagingAsync(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) {
            var query = GenerateQuery(out var totalCount, out var totalPage, querysetting, predicate, tracking: tracking);
            return (await query.ToListAsync(), totalCount, totalPage);
        }

        /// /// <summary>
        /// Get paged result asynchrony by TEntity
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Paged list of TModel</returns>
        public async Task<(List<TModel> Result, long TotalCount, long TotalPage)> GetPagingAsync<TModel>(TEntity model, QuerySetting guerysetting, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(out var totalCount, out var totalPage, guerysetting, model, tracking: tracking);
            return (await query.ToListAsync(), totalCount, totalPage);
        }

        /// <summary>
        /// Get paged result asynchrony by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TModel</returns>
        public async Task<(List<TModel> Result, long TotalCount, long TotalPage)> GetPagingAsync<TModel>(Expression<Func<TEntity, bool>> predicate, QuerySetting querysetting, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(out var totalCount, out var totalPage, querysetting, predicate, tracking: tracking);
            return (await query.ToListAsync(), totalCount, totalPage);
        }
        #endregion

        #region add
        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public TEntity Add(TEntity entity) {
            return SaveAndReturn(Entity.Add(entity));
        }

        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public TModel Add<TModel>(TEntity entity) where TModel : BaseModel {
            return _mapper.Map<TModel>(SaveAndReturn(Entity.Add(entity)));
        }

        /// <summary>
        /// Insert new record asychrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public async Task<TEntity> AddAsync(TEntity entity) {
            return await SaveAndReturnAsync(await Entity.AddAsync(entity));
        }

        /// <summary>
        /// Insert new record asychrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public async Task<TModel> AddAsync<TModel>(TEntity entity) where TModel : BaseModel {
            return _mapper.Map<TModel>(await SaveAndReturnAsync(await Entity.AddAsync(entity)));
        }
        #endregion

        #region update
        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public TEntity Update(TEntity entity, bool needToFetch = true) {
            var tracked = entity;
            if(needToFetch) {
                tracked = First(entity.Id.Value, force: true);
                _propertyMapper.Bind(entity, tracked);
            }
            return SaveAndReturn(Entity.Update(tracked));
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public TModel Update<TModel>(TEntity entity, bool needToFetch = true) where TModel : BaseModel {
            var tracked = entity;
            if(needToFetch) {
                tracked = First(entity.Id.Value, force: true);
                _propertyMapper.Bind(entity, tracked);
            }
            return _mapper.Map<TModel>(SaveAndReturn(Entity.Update(tracked)));
        }

        /// <summary>
        /// Update record asynchrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool needToFetch = true) {
            var tracked = entity;
            if(needToFetch) {
                tracked = await FirstAsync(entity.Id.Value, force: true);
                _propertyMapper.Bind(entity, tracked);
            }
            return await SaveAndReturnAsync(Entity.Update(tracked));
        }

        /// <summary>
        /// Update record asynchrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public async Task<TModel> UpdateAsync<TModel>(TEntity entity, bool needToFetch = true) where TModel : BaseModel {
            var tracked = entity;
            if(needToFetch) {
                tracked = await FirstAsync(entity.Id.Value, force: true);
                _propertyMapper.Bind(entity, tracked);
            }
            return _mapper.Map<TModel>(await SaveAndReturnAsync(Entity.Update(tracked)));
        }
        #endregion

        //public bool Remove(long id) {
        //    return Remove(First(id, force: true));
        //}

        //public bool Remove<TModel>(TModel viewModel) where TModel : BaseModel {
        //    return Remove(First(viewModel.Id, force: true));
        //}

        //public bool Remove(TEntity entity) {
        //    var wrapped = ObjectAccessor.Create(entity);
        //    if(wrapped["Status"] != null) {
        //        wrapped["Status"] = Status.Deleted;
        //        return SaveAndGetState(Entity.Update(entity)) == EntityState.Modified;
        //    }
        //    return false;
        //}

        //public bool Remove(Expression<Func<TEntity, bool>> predicate) {
        //    var entities = All(predicate);
        //    entities.ForEach(e => {
        //        var wrapped = ObjectAccessor.Create(e);
        //        if(wrapped["Status"] != null) {
        //            wrapped["Status"] = Status.Deleted;
        //        }
        //    });
        //    return SaveAll() == 1;
        //}

        //public async Task<bool> RemoveAsync(long id) {
        //    return await RemoveAsync(await FirstAsync(id, force: true));
        //}

        //public async Task<bool> RemoveAsync<TModel>(TModel viewModel) where TModel : BaseModel {
        //    return await RemoveAsync(await FirstAsync(viewModel.Id, force: true));

        //}

        //public async Task<bool> RemoveAsync(TEntity entity) {
        //    var wrapped = ObjectAccessor.Create(entity);
        //    if(wrapped["Status"] != null) {
        //        wrapped["Status"] = Status.Deleted;
        //        return await SaveAndGetStateAsync(Entity.Update(entity)) == EntityState.Modified;
        //    }
        //    return false;
        //}

        //public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate) {
        //    var entities = All(predicate);
        //    entities.ForEach(e => {
        //        var wrapped = ObjectAccessor.Create(e);
        //        if(wrapped["Status"] != null) {
        //            wrapped["Status"] = Status.Deleted;
        //        }
        //    });
        //    return await SaveAllAsync() == 1;
        //}

        #region delete
        public bool Delete(long id) {
            return Delete(First(id, force: true));
        }

        public bool Delete(TEntity entity) {
            return SaveAndGetState(Entity.Remove(entity)) == EntityState.Deleted;
        }

        public async Task<bool> DeleteAsync(long id) {
            return await DeleteAsync(First(id, force: true));
        }

        public async Task<bool> DeleteAsync(TEntity entity) {
            return await SaveAndGetStateAsync(Entity.Remove(entity)) == EntityState.Deleted;
        }
        #endregion

        #region save
        public int Save(EntityEntry entry) {
            return entry.Context.SaveChanges();
        }

        public EntityState SaveAndGetState(EntityEntry entry) {
            entry.Context.SaveChanges();
            return entry.State;
        }

        public TEntity SaveAndReturn(EntityEntry entry) {
            entry.Context.SaveChanges();
            return (TEntity)entry.Entity;
        }

        public int SaveAll(IDbContextTransaction transaction = null) {
            return GetMsSQLDbContext(transaction).SaveChanges();
        }

        public async Task<int> SaveAsync(EntityEntry entity) {
            while(true) {
                try {
                    // Attempt to save changes to the database
                    return await entity.Context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException ex) {
                    foreach(var entry in ex.Entries) {
                        if(entry.Entity is TEntity) {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach(var property in proposedValues.Properties) {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                proposedValues[property] = proposedValue;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public async Task<EntityState> SaveAndGetStateAsync(EntityEntry entity) {

            while(true) {
                try {
                    await entity.Context.SaveChangesAsync();
                    return entity.State;
                }
                catch(DbUpdateConcurrencyException ex) {
                    foreach(var entry in ex.Entries) {
                        if(entry.Entity is TEntity) {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach(var property in proposedValues.Properties) {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                proposedValues[property] = proposedValue;
                            }

                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public async Task<TEntity> SaveAndReturnAsync(EntityEntry entity) {
            while(true) {
                try {
                    await entity.Context.SaveChangesAsync();
                    return (TEntity)entity.Entity;
                }
                catch(DbUpdateConcurrencyException ex) {
                    foreach(var entry in ex.Entries) {
                        if(entry.Entity is TEntity) {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach(var property in proposedValues.Properties) {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                proposedValues[property] = proposedValue;
                            }

                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public async Task<int> SaveAllAsync(IDbContextTransaction transaction = null) {
            return await GetMsSQLDbContext(transaction).SaveChangesAsync();
        }
        #endregion
    }
}
