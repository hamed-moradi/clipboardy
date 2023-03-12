using Assets.Model.Base;
using Assets.Utility;
using AutoMapper;
using Core.Application.Interfaces;
using Core.Domain._App;
using Microsoft.EntityFrameworkCore;
using System;

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
    protected readonly IMapper _mapper;

    public BaseService() {
      _mapper = ServiceLocator.Current.GetInstance<IMapper>();
    }
    #endregion

    public DbSet<TEntity> Entity => PostgresContext.Set<TEntity>();

    public IServiceResult Ok(object data = null, int code = 200) {
      return new ServiceResult(code, null, data);
    }

    public IServiceResult BadRequest(string message = "Bad request", int code = 400) {
      return new ServiceResult(code, message);
    }

    public IServiceResult InternalError(string message = "Internal error", int code = 500) {
      return new ServiceResult(code, message);
    }

    public IServiceResult InternalError(Exception exception, int code = 500) {
      return new ServiceResult(code, exception.Message);
    }
  }
}
