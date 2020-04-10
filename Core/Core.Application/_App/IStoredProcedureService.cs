using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application {
    public interface IStoredProcedureService<Result, Schema>
        where Result : IStoredProcResult
        where Schema : IStoredProcSchema {

        //Sync
        void ExecuteReturnLess(string procedure);
        void ExecuteReturnLess(Schema model);
        IEnumerable<Result> Execute(string procedure);
        IEnumerable<Result> Execute(Schema model);
        Result ExecuteFirstOrDefault(string procedure);
        Result ExecuteFirstOrDefault(Schema model);

        //Async
        Task ExecuteReturnLessAsync(string procedure);
        Task ExecuteReturnLessAsync(Schema model);
        Task<IEnumerable<Result>> ExecuteAsync(string procedure);
        Task<IEnumerable<Result>> ExecuteAsync(Schema model);
        Task<Result> ExecuteFirstOrDefaultAsync(string procedure);
        Task<Result> ExecuteFirstOrDefaultAsync(Schema model);
    }
}
