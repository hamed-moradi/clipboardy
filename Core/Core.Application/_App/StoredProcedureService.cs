using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using AutoMapper;
using Core.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Application {
    public class StoredProcedureService: IStoredProcedureService {
        #region ctor
        private readonly IDbConnection _dbconn;

        public StoredProcedureService(ConnectionPool connpool) {
            _dbconn = connpool.DbConnection;
        }
        #endregion

        //Sync
        public void Execute(string procedure) {
            _dbconn.Execute(procedure, commandType: CommandType.StoredProcedure);
        }

        public void Execute<Schema>(Schema model) where Schema : IStoredProcSchema {
            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            _dbconn.Execute($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
            parameterHandler.SetReturnValue(parameters);
        }

        public IEnumerable<Result> Query<Result>(string procedure) where Result : IStoredProcResult {
            var result = _dbconn.Query<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public IEnumerable<Result> Query<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            var result = _dbconn.Query<Result>($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
            parameterHandler.SetReturnValue(parameters);
            return result;
        }

        public Result QueryFirst<Result>(string procedure) where Result : IStoredProcResult {
            var result = _dbconn.QueryFirstOrDefault(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Result QueryFirst<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            var result = _dbconn.QueryFirstOrDefault($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
            parameterHandler.SetReturnValue(parameters);
            return result;
        }

        //Async
        public async Task ExecuteAsync(string procedure) {
            await _dbconn.ExecuteAsync(procedure, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteAsync<Schema>(Schema model) where Schema : IStoredProcSchema {
            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            await _dbconn.ExecuteAsync($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
        }

        public async Task<IEnumerable<Result>> QueryAsync<Result>(string procedure) where Result : IStoredProcResult {
            var result = await _dbconn.QueryAsync<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Result>> QueryAsync<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            var result = await _dbconn.QueryAsync<Result>($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
            parameterHandler.SetReturnValue(parameters);
            return result;
        }

        public async Task<Result> QueryFirstAsync<Result>(string procedure) where Result : IStoredProcResult {
            var result = await _dbconn.QueryFirstOrDefaultAsync(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Result> QueryFirstAsync<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameterHandler = ServiceLocator.Current.GetInstance<IParameterHandler<Schema>>();
            var parameters = parameterHandler.MakeParameters();
            var (schema, name) = model.GetStoredProcedureAttribute();
            var result = await _dbconn.QueryFirstOrDefaultAsync($"{schema}.{name}", parameters, commandType: CommandType.StoredProcedure);
            parameterHandler.SetOutputValues(parameters);
            parameterHandler.SetReturnValue(parameters);
            return result;
        }
    }
}
