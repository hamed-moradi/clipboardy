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

    public class BaseEntity: IBaseEntity {
        [Key]
        public virtual int Id { get; set; }

        [NotMapped]
        public virtual byte Status { get; set; }

        [NotMapped]
        public string OrderBy { get; set; } = "Id";

        [NotMapped]
        public bool OrderAscending { get; set; } = false;

        [NotMapped]
        public int Skip { get; set; } = 0;

        [NotMapped]
        public int Take { get; set; } = 10;

        [NotMapped]
        public long TotalCount { get; set; } = 0;
    }

    public class BaseModel: IBaseModel {
        public virtual int Id { get; set; }
    }

    public class BaseViewModel {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;
        public string Message { get; set; }
        public object Data { get; set; }
        public int? TotalPages { get; set; }
        public static string Version { get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); } }
    }

    public class HeaderBindingModel: IBaseBindingModel {
        public string Token { get; set; }
        public int? AccountId { get; set; }
    }

}
