using Assets.Model.Base;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Domain;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Application {
    public class StoredProcedureService<Result, Schema>: IStoredProcedureService<Result, Schema>
        where Result : IStoredProcResult
        where Schema : IStoredProcSchema {
        #region ctor
        private readonly IDbConnection _dbconn;
        private readonly IParameterHandler<Schema> _parameterHandler;

        public StoredProcedureService(ConnectionPool connpool, IParameterHandler<Schema> parameterHandler) {
            _dbconn = connpool.DbConnection;
            _parameterHandler = parameterHandler;
        }
        #endregion

        //Sync
        public void ExecuteReturnLess(string procedure) {
            _dbconn.Execute(procedure, commandType: CommandType.StoredProcedure);
        }
        public void ExecuteReturnLess(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            _dbconn.Execute(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
            _parameterHandler.SetReturnValue(parameters);
        }
        public IEnumerable<Result> Execute(string procedure) {
            var result = _dbconn.Query<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<Result> Execute(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            var result = _dbconn.Query<Result>(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
            _parameterHandler.SetReturnValue(parameters);
            return result;
        }
        public Result ExecuteFirstOrDefault(string procedure) {
            var result = _dbconn.QueryFirstOrDefault(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }
        public Result ExecuteFirstOrDefault(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            var result = _dbconn.QueryFirstOrDefault(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
            _parameterHandler.SetReturnValue(parameters);
            return result;
        }

        //Async
        public async Task ExecuteReturnLessAsync(string procedure) {
            await _dbconn.ExecuteAsync(procedure, commandType: CommandType.StoredProcedure);
        }
        public async Task ExecuteReturnLessAsync(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            await _dbconn.ExecuteAsync(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
        }
        public async Task<IEnumerable<Result>> ExecuteAsync(string procedure) {
            var result = await _dbconn.QueryAsync<Result>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Result>> ExecuteAsync(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            var result = await _dbconn.QueryAsync<Result>(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
            _parameterHandler.SetReturnValue(parameters);
            return result;
        }
        public async Task<Result> ExecuteFirstOrDefaultAsync(string procedure) {
            var result = await _dbconn.QueryFirstOrDefaultAsync(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<Result> ExecuteFirstOrDefaultAsync(Schema model) {
            var parameters = _parameterHandler.MakeParameters();
            var result = await _dbconn.QueryFirstOrDefaultAsync(model.GetSchemaName(), parameters, commandType: CommandType.StoredProcedure);
            _parameterHandler.SetOutputValues(parameters);
            _parameterHandler.SetReturnValue(parameters);
            return result;
        }
    }
}
