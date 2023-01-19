using Assets.Utility;
using Core.Domain._App;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Application._App {
  public class BaseService {
    public readonly PostgresContext PostgresContext;

    public BaseService() {
      PostgresContext = ServiceLocator.Current.GetInstance<PostgresContext>();
    }
  }

  public class BaseService<TEntity>: BaseService, IBaseService<TEntity>
    where TEntity : BaseEntity {

    #region ctors
    public BaseService() {

    }
    #endregion

    public DbSet<TEntity> Entity => PostgresContext.Set<TEntity>();

    public TEntity Add(TEntity entity, bool save = true) {
      Entity.Add(entity);
      if(save)
        PostgresContext.SaveChanges();
      return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, bool save = true) {
      await Entity.AddAsync(entity);
      if(save)
        await PostgresContext.SaveChangesAsync();
      return entity;
    }
  }
}
