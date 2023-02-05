﻿using Assets.Utility;
using AutoMapper;
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
    protected readonly IMapper _mapper;

    public BaseService() {
      _mapper = ServiceLocator.Current.GetInstance<IMapper>();
    }
    #endregion

    public DbSet<TEntity> Entity => PostgresContext.Set<TEntity>();
  }
}
