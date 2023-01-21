using Core.Domain._App;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Interfaces {
  public partial interface IBaseService<TEntity> where TEntity : BaseEntity {
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> AddAsync(TEntity entity, bool save = true);
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
  }
}
