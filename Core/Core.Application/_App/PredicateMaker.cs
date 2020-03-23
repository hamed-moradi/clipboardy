using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using Core.Domain.Entities;
using FastMember;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.Application {
    public interface IPredicateMaker<TEntity> where TEntity : IBaseEntity {
        IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true);
        IQueryable<TEntity> GenerateQuery(out long totalCount, out long totalPage, QuerySetting querysettings, Expression<Func<TEntity, bool>> predicate = null, bool tracking = true);
        IQueryable<TModel> GenerateQuery<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true) where TModel : BaseModel;
        IQueryable<TModel> GenerateQuery<TModel>(out long totalCount, out long totalPage, QuerySetting querysettings, Expression<Func<TEntity, bool>> predicate = null, bool tracking = true) where TModel : BaseModel;
        IQueryable<TEntity> GenerateQuery(TEntity model, bool tracking = true);
        IQueryable<TEntity> GenerateQuery(out long totalCount, out long totalPage, QuerySetting querysettings, TEntity model, bool tracking = true);
        IQueryable<TModel> GenerateQuery<TModel>(TEntity model, bool tracking = true) where TModel : BaseModel;
        IQueryable<TModel> GenerateQuery<TModel>(out long totalCount, out long totalPage, QuerySetting querysettings, TEntity model, bool tracking = true) where TModel : BaseModel;
    }

    public class PredicateMaker<TEntity>: IPredicateMaker<TEntity> where TEntity : BaseEntity {
        #region ctor
        private readonly IMapper _mapper;
        private readonly MsSQLDbContext _dbContext;
        public DbSet<TEntity> Entity { get { return _dbContext.Set<TEntity>(); } }

        protected internal PredicateMaker(MsSQLDbContext dbContext = null, IMapper mapper = null) {
            _dbContext = dbContext ?? ServiceLocator.Current.GetInstance<MsSQLDbContext>();
            _mapper = mapper ?? ServiceLocator.Current.GetInstance<IMapper>();
        }
        #endregion

        public IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true) {
            var entity = tracking ? Entity : Entity.AsNoTracking();
            var query = predicate is null ? entity : entity.Where(predicate);
            return query;
        }

        public IQueryable<TEntity> GenerateQuery(out long totalCount, out long totalPage, QuerySetting querysettings, Expression<Func<TEntity, bool>> predicate = null, bool tracking = true) {
            var entity = tracking ? Entity : Entity.AsNoTracking();
            var query = predicate is null ? entity : entity.Where(predicate);
            totalCount = query.Count();
            totalPage = totalCount > 0 ? totalCount >= querysettings.Take ? (long)Math.Ceiling((decimal)(totalCount / querysettings.Take)) : 1 : 0;
            query = query.OrderByField(querysettings.OrderBy, querysettings.OrderAscending).Skip(querysettings.Skip).Take(querysettings.Take);
            return query;
        }

        public IQueryable<TModel> GenerateQuery<TModel>(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true)
            where TModel : BaseModel {
            return GenerateQuery(predicate, tracking).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }

        public IQueryable<TModel> GenerateQuery<TModel>(out long totalCount, out long totalPage, QuerySetting querysettings, Expression<Func<TEntity, bool>> predicate = null, bool tracking = true)
            where TModel : BaseModel {
            return GenerateQuery(out totalCount, out totalPage, querysettings, predicate, tracking).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }

        public IQueryable<TEntity> GenerateQuery(TEntity model, bool tracking = true) {
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
            return query;
        }

        public IQueryable<TEntity> GenerateQuery(out long totalCount, out long totalPage, QuerySetting querysettings, TEntity model, bool tracking = true) {
            var query = GenerateQuery(model, tracking: tracking);
            totalCount = query.Count();
            totalPage = totalCount > 0 ? totalCount >= querysettings.Take ? (long)Math.Ceiling((decimal)(totalCount / querysettings.Take)) : 1 : 0;
            query = query.OrderByField(querysettings.OrderBy, querysettings.OrderAscending)
                .Skip(querysettings.Skip).Take(querysettings.Take);
            return query;
        }

        public IQueryable<TModel> GenerateQuery<TModel>(TEntity model, bool tracking = true)
            where TModel : BaseModel {
            return GenerateQuery(model, tracking).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }

        public IQueryable<TModel> GenerateQuery<TModel>(out long totalCount, out long totalPage, QuerySetting querysettings, TEntity model, bool tracking = true)
            where TModel : BaseModel {
            return GenerateQuery(out totalCount, out totalPage, querysettings, model, tracking).ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }
    }
}
