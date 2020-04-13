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
        IEnumerable<Result> Query<Result>(string procedure) where Result : IStoredProcResult;
        IEnumerable<Result> Query<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
        Result QueryFirst<Result>(string procedure) where Result : IStoredProcResult;
        Result QueryFirst<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;

        //Async
        Task ExecuteAsync(string procedure);
        Task ExecuteAsync<Schema>(Schema model) where Schema : IStoredProcSchema;
        Task<IEnumerable<Result>> QueryAsync<Result>(string procedure) where Result : IStoredProcResult;
        Task<IEnumerable<Result>> QueryAsync<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
        Task<Result> QueryFirstAsync<Result>(string procedure) where Result : IStoredProcResult;
        Task<Result> QueryFirstAsync<Schema, Result>(Schema model) where Result : IStoredProcResult where Schema : IStoredProcSchema;
    }

    public interface IStoredProcedureService<Result, Schema>
        where Result : IStoredProcResult
        where Schema : IStoredProcSchema {

        //Sync
        void Execute(string procedure);
        void Execute(Schema model);
        IEnumerable<Result> Query(string procedure);
        IEnumerable<Result> Query(Schema model);
        Result QueryFirst(string procedure);
        Result QueryFirst(Schema model);

        //Async
        Task ExecuteAsync(string procedure);
        Task ExecuteAsync(Schema model);
        Task<IEnumerable<Result>> QueryAsync(string procedure);
        Task<IEnumerable<Result>> QueryAsync(Schema model);
        Task<Result> QueryFirstAsync(string procedure);
        Task<Result> QueryFirstAsync(Schema model);
    }
}
