using Assets.Model.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace Assets.Model.Base {
    public interface IStoredProcSchema { }
    public interface IStoredProcResult { }
    public interface IBaseBindingModel { }
    public interface IBaseViewModel { }

    public class BaseSchema: IStoredProcSchema {
        [ReturnParameter]
        public int StatusCode { get; set; }
    }

    public class PagingResult: IStoredProcResult {
        [HelperParameter]
        public long TotalCount { get; set; }
    }

    public class PagingSchema: BaseSchema {
        [InputParameter]
        public string @OrderBy { get; set; } = "Id";

        [InputParameter]
        public string @Order { get; set; } = "DESC";

        [InputParameter]
        public int? @Skip { get; set; } = 0;

        [InputParameter]
        public int? @Take { get; set; } = 10;

        [HelperParameter]
        public long TotalCount { get; set; }

        [HelperParameter]
        public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalCount / Take.Value); } }
    }

    public class PagingOption: IBaseBindingModel {
        public string OrderBy { get; set; } = "Id";
        public string Order { get; set; } = "DESC";
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
