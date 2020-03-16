using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.Application {
    public interface IPredicateMaker<TEntity> where TEntity : IBaseEntity {
        IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, bool pagingSupport = false);
        IQueryable<TModel> GenerateQuery<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, bool pagingSupport = false) where TModel : BaseModel;
        IQueryable<TEntity> GenerateQuery(TEntity model, bool tracking = true, bool pagingSupport = false);
        IQueryable<TModel> GenerateQuery<TModel>(TEntity model, bool tracking = true, bool pagingSupport = false) where TModel : BaseModel;
    }

    public class PredicateMaker<TEntity>: IPredicateMaker<TEntity> where TEntity : BaseEntity {
        #region ctor
        private readonly IMapper _mapper;
        private readonly MsSqlDbContext _dbContext;
        public DbSet<TEntity> Entity { get { return _dbContext.Set<TEntity>(); } }

        protected internal PredicateMaker(MsSqlDbContext dbContext = null, IMapper mapper = null) {
            _dbContext = dbContext ?? ServiceLocator.Current.GetInstance<MsSqlDbContext>();
            _mapper = mapper ?? ServiceLocator.Current.GetInstance<IMapper>();
        }
        #endregion

        public IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, bool pagingSupport = false) {
            var entity = tracking ? Entity : Entity.AsNoTracking();
            var query = predicate is null ? entity : entity.Where(predicate);
            if(pagingSupport) {
                predicate.Body.GetType().GetProperty("TotalCount").SetValue(predicate, query.Count()); // it doesn't work
                var orderBy = (string)predicate.Body.GetType().GetProperty("OrderBy").GetValue(predicate);
                var orderAsc = (bool)predicate.Body.GetType().GetProperty("Order").GetValue(predicate);
                var skip = (int)predicate.Body.GetType().GetProperty("Skip").GetValue(predicate);
                var take = (int)predicate.Body.GetType().GetProperty("Take").GetValue(predicate);
                query = query.OrderByField(orderBy, orderAsc).Skip(skip).Take(take);
            }
            return query;
        }
        public IQueryable<TModel> GenerateQuery<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, bool pagingSupport = false)
            where TModel : BaseModel {
            return GenerateQuery(predicate, tracking, pagingSupport).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }
        public IQueryable<TEntity> GenerateQuery(TEntity model, bool tracking = true, bool pagingSupport = false) {
            var query = GenerateQuery(tracking: tracking);
            var properties = model.GetType().GetProperties().Where(item
                => !Attribute.IsDefined(item, typeof(NotMappedAttribute))
                && !Attribute.IsDefined(item, typeof(ForeignKeyAttribute)));
            foreach(var prp in properties) {
                var key = prp.Name;
                var value = prp.GetValue(model, null);
                if(value != null) {
                    query = query.Where(w => w.GetType().GetProperty(key).GetValue(model) == value);
                }
            }
            if(pagingSupport) {
                model.TotalCount = query.Count();
                query = query.OrderByField(model.OrderBy, model.OrderAscending)
                    .Skip(model.Skip).Take(model.Take);
            }
            return query;
        }
        public IQueryable<TModel> GenerateQuery<TModel>(TEntity model, bool tracking = true, bool pagingSupport = false)
            where TModel : BaseModel {
            return GenerateQuery(model, tracking, pagingSupport).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }
    }
}
