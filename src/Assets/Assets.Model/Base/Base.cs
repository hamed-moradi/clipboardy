using System;

namespace Assets.Model.Base {
  public interface IStoredProcSchema { }
  public interface IStoredProcResult { }
  public interface IBaseBindingModel { }
  public interface IBaseViewModel { }

  public class BaseSchema: IStoredProcSchema {
    public int StatusCode { get; set; }
  }

  public class PagingResult: IStoredProcResult {
    public long TotalCount { get; set; }
  }

  public class PagingSchema: BaseSchema {
    public string @OrderBy { get; set; } = "Id";
    public string @Order { get; set; } = "DESC";
    public int? @Skip { get; set; } = 0;
    public int? @Take { get; set; } = 10;
    public long TotalCount { get; set; }
    public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalCount / Take.Value); } }
  }

  public class PagingOption: IBaseBindingModel {
    public string OrderBy { get; set; } = "Id";
    public string Order { get; set; } = "DESC";
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
  }
}
