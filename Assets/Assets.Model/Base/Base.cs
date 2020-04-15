using Assets.Model.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace Assets.Model.Base {
    public interface IStoredProcSchema { }
    public interface IStoredProcResult { }
    public interface IBaseBindingModel { }

    public class BaseSchema: IStoredProcSchema {
        [ReturnParameter]
        public int StatusCode { get; set; }
    }

    public class PagingSchema: BaseSchema {
        [InputParameter]
        public string @OrderBy { get; set; }

        [InputParameter]
        public string @Order { get; set; }

        [InputParameter]
        public int? @Skip { get; set; }

        [InputParameter]
        public int? @Take { get; set; }

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

    public class BaseViewModel: IBaseViewModel {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;
        public string Message { get; set; }
        public object Data { get; set; }
        public long? TotalPages { get; set; }

        private static string _version;
        public static string Version {
            get {
                if(string.IsNullOrWhiteSpace(_version)) {
                    _version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
                return _version;
            }
        }
    }

    public interface IServiceResult {
        int Code { get; set; }
        string Message { get; set; }
        object Data { get; set; }
    }

    public class ServiceResult: IServiceResult {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ServiceResult(int code, string message, object data = null) {
            Code = code;
            Message = message;
            Data = data;
        }
    }


    #region old
    public interface IBaseEntity { }
    public interface IBaseModel { }


    public interface IBaseViewModel: IBaseModel { }

    public class BaseEntity: IBaseEntity {
        [Key]
        public virtual int? Id { get; set; }
    }

    public class QuerySetting: IBaseEntity {
        public string OrderBy { get; set; } = "Id";
        public bool OrderAscending { get; set; } = false;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }

    public class BaseModel: IBaseModel {
        public virtual int? Id { get; set; }
        //public int? TotalPages { get; set; }
    }

    public class BaseBindingModel: IBaseBindingModel {
        public string OrderBy { get; set; } = "Id";
        public bool OrderAscending { get; set; } = false;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public QuerySetting QuerySetting {
            get {
                return new QuerySetting { OrderBy = OrderBy, OrderAscending = OrderAscending, Skip = Skip, Take = Take };
            }
        }
    }
    #endregion
}
