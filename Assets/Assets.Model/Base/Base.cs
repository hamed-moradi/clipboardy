using Assets.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Reflection;
using System.Text;

namespace Assets.Model.Base {
    public interface IBaseEntity { }

    public interface IBaseModel { }

    public interface IBaseBindingModel: IBaseModel { }

    public interface IBaseViewModel: IBaseModel { }

    public class BaseEntity: IBaseEntity {
        [Key]
        public virtual int? Id { get; set; }

        [NotMapped]
        public virtual Status? StatusId { get; set; }
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

    public class HeaderBindingModel: IBaseBindingModel {
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }

        public AccountHeader Account { get; set; }
    }

    public class BaseBindingModel: HeaderBindingModel {
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
}
