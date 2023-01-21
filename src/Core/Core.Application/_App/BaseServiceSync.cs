using Core.Domain._App;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Application._App {
  public partial class BaseService<TEntity> {
    public TEntity First(Expression<Func<TEntity, bool>> predicate) =>
      Entity.FirstOrDefault(predicate);

    public TEntity Add(TEntity entity, bool save = true) {
      Entity.Add(entity);
      if(save)
        PostgresContext.SaveChanges();
      return entity;
    }

    public TEntity Update(TEntity entity, bool save = true) {
      Entity.Update(entity);
      if(save)
        PostgresContext.SaveChanges();
      return entity;
    }

    public TEntity Remove(TEntity entity, bool save = true) {
      Entity.Remove(entity);
      if(save)
        PostgresContext.SaveChanges();
      return entity;
    }

    public int Save() =>
      PostgresContext.SaveChanges();
  }
}
