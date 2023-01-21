using Assets.Utility;
using Core.Application.Interfaces;
using Core.Domain._App;
using Microsoft.EntityFrameworkCore;

namespace Core.Application._App {
  public class BaseService {
    public readonly PostgresContext PostgresContext;

    public BaseService() {
      PostgresContext = ServiceLocator.Current.GetInstance<PostgresContext>();
    }
  }

  public partial class BaseService<TEntity>: BaseService, IBaseService<TEntity>
    where TEntity : BaseEntity {

    #region ctors
    public BaseService() {

    }
    #endregion

    public DbSet<TEntity> Entity => PostgresContext.Set<TEntity>();
  }
}
