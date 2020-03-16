using Assets.Model;
using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly MsSqlDbContext _dbContext;
        private readonly PropertyMapper _propertyMapper;

        public GenericService(
            MsSqlDbContext dbContext = null,
            IMapper mapper = null,
            PropertyMapper propertyMapper = null) : base() {

            var serviceLocator = ServiceLocator.Current;
            _dbContext = dbContext ?? serviceLocator.GetInstance<MsSqlDbContext>();
            _mapper = mapper ?? serviceLocator.GetInstance<IMapper>();
            _propertyMapper = propertyMapper ?? serviceLocator.GetInstance<PropertyMapper>();
        }
        #endregion

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

        /// <summary>
        /// Get paged result by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>Paged list of TEntity</returns>
        public List<TEntity> GetPaging(TEntity entity, bool tracking = true) {
            var query = GenerateQuery(entity, tracking: tracking, pagingSupport: true);
            return query.ToList();
        }

        /// <summary>
        /// Get paged result by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TEntity</returns>
        public List<TEntity> GetPaging(Expression<Func<TEntity, bool>> predicate, bool tracking = true) {
            var query = GenerateQuery(predicate, tracking: tracking, pagingSupport: true);
            return query.ToList();
        }

        /// <summary>
        /// Get paged result by TEntity
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Paged list of TModel</returns>
        public List<TModel> GetPaging<TModel>(TEntity model, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(model, tracking: tracking, pagingSupport: true);
            return query.ToList();
        }

        /// <summary>
        /// Get paged result by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TModel</returns>
        public List<TModel> GetPaging<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(predicate, tracking: tracking, pagingSupport: true);
            return query.ToList();
        }

        /// <summary>
        /// Get paged result asynchrony by TEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>Paged list of TEntity</returns>
        public async Task<List<TEntity>> GetPagingAsync(TEntity model, bool tracking = true) {
            var query = GenerateQuery(model, tracking: tracking, pagingSupport: true);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get paged result asynchrony by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TEntity</returns>
        public async Task<List<TEntity>> GetPagingAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true) {
            var query = GenerateQuery(predicate, tracking: tracking, pagingSupport: true);
            return await query.ToListAsync();
        }

        /// /// <summary>
        /// Get paged result asynchrony by TEntity
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Paged list of TModel</returns>
        public async Task<List<TModel>> GetPagingAsync<TModel>(TEntity model, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(model, tracking: tracking, pagingSupport: true);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get paged result asynchrony by predicate query
        /// </summary>
        /// <param name="predicate">The predicate query</param>
        /// <returns>Paged list of TModel</returns>
        public async Task<List<TModel>> GetPagingAsync<TModel>(Expression<Func<TEntity, bool>> predicate, bool tracking = true) where TModel : BaseModel {
            var query = GenerateQuery<TModel>(predicate, tracking: tracking, pagingSupport: true);
            return await query.ToListAsync();
        }


        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public TEntity Add(TEntity entity) {
            var newItem = Entity.Add(entity);
            Save();
            return newItem.Entity;
        }

        /// <summary>
        /// Insert new record asychrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The inserted record</returns>
        public async Task<TEntity> AddAsync(TEntity entity) {
            var newItem = await Entity.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return newItem.Entity;
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public TEntity Update(TEntity entity, bool needToFetch = true) {
            var tracked = entity;
            if(needToFetch) {
                tracked = First(entity.Id, force: true);
                _propertyMapper.Bind(entity, tracked);
            }

            while(true) {
                try {
                    var updatedItem = Entity.Update(tracked);
                    // Attempt to save changes to the database
                    Save();
                    return updatedItem.Entity;
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

        /// <summary>
        /// Update record asynchrony
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="needToFetch">Get entity before update</param>
        /// <returns>The updated record</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool needToFetch = true) {
            var tracked = entity;
            if(needToFetch) {
                tracked = await FirstAsync(entity.Id, force: true);
                _propertyMapper.Bind(entity, tracked);
            }
            
            while(true) {
                try {
                    var updatedItem = Entity.Update(tracked);
                    // Attempt to save changes to the database
                    await SaveAsync();
                    return updatedItem.Entity;
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

        public bool Remove(long id) {
            First(id, true);
            var entity = First(id, true);
            entity.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(entity);
            Save();
            return modifiedItem.State == EntityState.Modified;
        }

        public bool Remove(TEntity entity) {
            First(entity.Id, true);
            entity.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(entity);
            Save();
            return modifiedItem.State == EntityState.Modified;
        }

        public bool Remove<TModel>(TModel viewModel) where TModel : BaseModel {
            var model = First(viewModel.Id, true);
            model.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(model);
            Save();
            return modifiedItem.State == EntityState.Modified;
        }

        public bool Remove(Expression<Func<TEntity, bool>> predicate) {
            var entities = All(predicate);
            entities.ForEach(e => e.Status = Status.Deleted.ToByte());
            return Save() == 1;
        }

        public async Task<bool> RemoveAsync(long id) {
            First(id, true);
            var entity = First(id, true);
            entity.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(entity);
            await SaveAsync();
            return modifiedItem.State == EntityState.Modified;
        }

        public async Task<bool> RemoveAsync(TEntity entity) {
            await FirstAsync(entity.Id, true);
            entity.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(entity);
            await _dbContext.SaveChangesAsync();
            return modifiedItem.State == EntityState.Modified;
        }

        public async Task<bool> RemoveAsync<TModel>(TModel viewModel) where TModel : BaseModel {
            var model = await FirstAsync(viewModel.Id, true);
            model.Status = Status.Deleted.ToByte();
            var modifiedItem = Entity.Update(model);
            await SaveAsync();
            return modifiedItem.State == EntityState.Modified;
        }

        public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate) {
            var entities = All(predicate);
            entities.ForEach(e => e.Status = Status.Deleted.ToByte());
            return await SaveAsync() == 1;
        }

        public bool Delete(long id) {
            var entity = First(id, true);
            var result = Entity.Remove(entity);
            Save();
            return result.State == EntityState.Deleted;
        }

        public bool Delete(TEntity entity) {
            var result = Entity.Remove(entity);
            Save();
            return result.State == EntityState.Deleted;
        }

        public async Task<bool> DeleteAsync(long id) {
            var entity = First(id, true);
            var result = Entity.Remove(entity);
            await SaveAsync();
            return result.State == EntityState.Deleted;
        }

        public async Task<bool> DeleteAsync(TEntity entity) {
            var result = Entity.Remove(entity);
            await SaveAsync();
            return result.State == EntityState.Deleted;
        }

        public int Save() {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync() {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
