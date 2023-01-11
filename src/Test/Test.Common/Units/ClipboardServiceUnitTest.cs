using Assets.Utility;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Units {
  [TestClass]
  public class ClipboardServiceUnitTest {
    #region ctor
    private readonly IClipboardService _clipboardService;

    public ClipboardServiceUnitTest() {
      _clipboardService = ServiceLocator.Current.GetInstance<IClipboardService>();
    }
    #endregion

    [TestMethod, TestCategory("ClipboardService"), TestCategory("GetPagingByModel")]
    public void GetPagingByModel() {
      //var query = new ClipboardGetPagingSchema { Order = "CreatedAt", OrderBy = "ASC", Skip = 1, Take = 1000 };
      //var result = _clipboardService.PagingAsync(query).GetAwaiter().GetResult();
      //Assert.IsTrue(result.Any());
      //Assert.IsTrue(query.TotalCount > 0);
      //Assert.IsTrue(query.TotalPages > 0);
    }

    [TestMethod, TestCategory("ClipboardService"), TestCategory("GetPagingByPredicate")]
    public void GetPagingByPredicate() {
      //var query = new ClipboardGetPagingSchema { };
      //var result = _clipboardService.PagingAsync(query);
      //Assert.IsTrue(result.Result.Any());
      //Assert.IsTrue(query.TotalCount > 0);
      //Assert.IsTrue(query.TotalPages > 0);
    }

    [TestMethod, TestCategory("ClipboardService"), TestCategory("First")]
    public void First() {
      //var query = new ClipboardGetFirstSchema { Id = 1 };
      //var clipboard = _clipboardService.FirstAsync(query).GetAwaiter().GetResult();
      //Assert.IsTrue(query.StatusCode == 200);
      //Assert.IsTrue(clipboard != null);
    }

    [TestMethod, TestCategory("ClipboardService"), TestCategory("Add")]
    public void Add() {
      //var clipboard = new ClipboardAddSchema {
      //  AccountId = 21,
      //  DeviceId = 17,
      //  TypeId = ContentType.txt.ToInt(),
      //  Content = Convert.ToBase64String(Encoding.UTF8.GetBytes("Content")),
      //  CreatedAt = DateTime.Now,
      //  StatusId = Status.Active.ToInt()
      //};
      //var clipboardId = _clipboardService.AddAsync(clipboard).GetAwaiter().GetResult();
      //Assert.IsTrue(clipboard.StatusCode == 200);
      //Assert.IsTrue(clipboardId > 0);
    }
  }
}
