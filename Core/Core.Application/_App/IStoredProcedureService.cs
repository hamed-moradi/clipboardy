using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application {
    public interface IStoredProcedureService {
        //Sync
        void Execute(string procedure);
        void Execute<Schema>(Schema model) where Schema : IStoredProcSchema;
        Result ExecuteScalar<Result>(string procedure) where Result : IStoredProcResult;
        Result ExecuteScalar<Schema, Result>(Schema model) where Schema : IStoredProcSchema;
        IEnumerable<Result> Query<Result>(string procedure) where Result : IStoredProcResult;
        IEnumerable<Result> Query<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
        Result QueryFirst<Result>(string procedure) where Result : IStoredProcResult;
        Result QueryFirst<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;

        //Async
        Task ExecuteAsync(string procedure);
        Task ExecuteAsync<Schema>(Schema model) where Schema : IStoredProcSchema;
        Task<Result> ExecuteScalarAsync<Result>(string procedure) where Result : IStoredProcResult;
        Task<Result> ExecuteScalarAsync<Schema, Result>(Schema model) where Schema : IStoredProcSchema;
        Task<IEnumerable<Result>> QueryAsync<Result>(string procedure) where Result : IStoredProcResult;
        Task<IEnumerable<Result>> QueryAsync<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
        Task<Result> QueryFirstAsync<Result>(string procedure) where Result : IStoredProcResult;
        Task<Result> QueryFirstAsync<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
    }
}
