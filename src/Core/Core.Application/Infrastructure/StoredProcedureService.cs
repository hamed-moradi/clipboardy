using Assets.Model.Base;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Application.Infrastructure {
    public class StoredProcedureService: IStoredProcedureService {
        #region ctor
        private readonly IDbConnection _dbconn;
        private readonly IParameterHandler _parameterHandler;

        public StoredProcedureService(
            ConnectionPool connpool,
            IParameterHandler parameterHandler) {

            _dbconn = connpool.MsSqlConnection;
            _parameterHandler = parameterHandler;
        }
        #endregion

        //Sync
        public void Execute(string procedure) {
            _dbconn.Execute(procedure, commandType: CommandType.StoredProcedure);
        }

        public void Execute<Schema>(Schema model)
            where Schema : IStoredProcSchema {

            var parameters = _parameterHandler.MakeParameters(model);
            _dbconn.Execute(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
        }

        public Result ExecuteScalar<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = _dbconn.ExecuteScalar<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Result ExecuteScalar<Schema, Result>(Schema model)
            where Schema : IStoredProcSchema {

            if(typeof(Result) != typeof(bool) && typeof(Result) != typeof(int) && typeof(Result) != typeof(string)) {
                throw new Exception("ExecuteScalarAsync is called with invalid Result data type.");
            }
            var parameters = _parameterHandler.MakeParameters(model);
            var result = _dbconn.ExecuteScalar<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            return result;
        }

        public IEnumerable<Result> Query<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = _dbconn.Query<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public IEnumerable<Result> Query<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameters = _parameterHandler.MakeParameters(model);
            var result = _dbconn.Query<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            _parameterHandler.SetTotalCount(model, result);
            return result;
        }

        public Result QueryFirst<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = _dbconn.QueryFirstOrDefault<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Result QueryFirst<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameters = _parameterHandler.MakeParameters(model);
            var result = _dbconn.QueryFirstOrDefault<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            return result;
        }

        //Async
        public async Task ExecuteAsync(string procedure) {
            await _dbconn.ExecuteAsync(procedure, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteAsync<Schema>(Schema model)
            where Schema : IStoredProcSchema {

            var parameters = _parameterHandler.MakeParameters(model);
            await _dbconn.ExecuteAsync(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
        }

        public async Task<Result> ExecuteScalarAsync<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = await _dbconn.ExecuteScalarAsync<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Result> ExecuteScalarAsync<Schema, Result>(Schema model)
            where Schema : IStoredProcSchema {

            if(typeof(Result) != typeof(bool) && typeof(Result) != typeof(int) && typeof(Result) != typeof(string)) {
                throw new Exception("ExecuteScalarAsync is called with invalid Result data type.");
            }
            var parameters = _parameterHandler.MakeParameters(model);
            var result = await _dbconn.ExecuteScalarAsync<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            return result;
        }

        public async Task<IEnumerable<Result>> QueryAsync<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = await _dbconn.QueryAsync<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Result>> QueryAsync<Schema, Result>(Schema model)
            where Schema : IStoredProcSchema
            where Result : IStoredProcResult {

            var parameters = _parameterHandler.MakeParameters(model);
            var result = await _dbconn.QueryAsync<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            _parameterHandler.SetTotalCount(model, result);
            return result;
        }

        public async Task<Result> QueryFirstAsync<Result>(string procedure)
            where Result : IStoredProcResult {

            var result = await _dbconn.QueryFirstOrDefaultAsync<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Result> QueryFirstAsync<Schema, Result>(Schema model)
            where Result : IStoredProcResult
            where Schema : IStoredProcSchema {

            var parameters = _parameterHandler.MakeParameters(model);
            var result = await _dbconn.QueryFirstOrDefaultAsync<Result>(model.GetStoredProcedureName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(model, parameters);
            _parameterHandler.SetReturnValue(model, parameters);
            return result;
        }
    }
}
