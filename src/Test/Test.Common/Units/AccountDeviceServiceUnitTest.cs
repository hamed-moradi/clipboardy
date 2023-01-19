using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Units {
  [TestClass]
  public class AccountDeviceServiceUnitTest {
    #region ctor
    private readonly IAccountDeviceService _accountDeviceService;
    private readonly RandomMaker _randomMaker;

    public AccountDeviceServiceUnitTest() {
      _accountDeviceService = ServiceLocator.Current.GetInstance<IAccountDeviceService>();
      _randomMaker = ServiceLocator.Current.GetInstance<RandomMaker>();
    }
    #endregion

    [TestMethod, TestCategory("AccountDeviceService"), TestCategory("First")]
    public void First() {
      //var query = new AccountDeviceGetFirstSchema { Id = 5 };
      //var account = _accountDeviceService.FirstAsync(query).GetAwaiter().GetResult();
      //Assert.IsTrue(query.StatusCode == 200);
      //Assert.IsTrue(account != null);
    }

    [TestMethod, TestCategory("AccountDeviceService"), TestCategory("Add")]
    public void Add() {
      var accountDevice = new AccountDevice {
        AccountId = 1,
        DeviceId = "DeviceId",
        DeviceName = "DeviceName",
        DeviceType = "DeviceType"
      };
      var result = _accountDeviceService.AddAsync(accountDevice).GetAwaiter().GetResult();
      Assert.IsNotNull(result);
    }

    [TestMethod, TestCategory("AccountDeviceService"), TestCategory("Update")]
    public void Update() {
      //var accountDevice = new AccountDeviceUpdateSchema {
      //  Id = 15,
      //  StatusId = Status.Active.ToInt()
      //};
      //_accountDeviceService.UpdateAsync(accountDevice).GetAwaiter().GetResult();
      //Assert.IsTrue(accountDevice.StatusCode == 200);
    }
  }
}
