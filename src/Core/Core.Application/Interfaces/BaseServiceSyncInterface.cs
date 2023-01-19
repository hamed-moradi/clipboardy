using Core.Domain._App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Core.Application.Interfaces {
  public partial interface IBaseService<TEntity> where TEntity : BaseEntity {
    DbSet<TEntity> Entity { get; }
    TEntity First(Expression<Func<TEntity, bool>> expression);
    TEntity Add(TEntity entity, bool save = true);
    TEntity Update(TEntity entity, bool save = true);
    TEntity Remove(TEntity entity, bool save = true);
    int Save();
  }
}
