using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Model.Base {
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
}
