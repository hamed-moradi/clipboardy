using Core.Domain._App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application._App {
  public partial class BaseService<TEntity> {
    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate) =>
      await Entity.FirstOrDefaultAsync(predicate);

    public async Task<TEntity> AddAsync(TEntity entity, bool save = true) {
      await Entity.AddAsync(entity);
      if(save)
        await PostgresContext.SaveChangesAsync();
      return entity;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
               return await PostgresContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    
  }
}
