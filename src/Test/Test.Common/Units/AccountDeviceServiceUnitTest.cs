using Assets.Utility;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace Test.Common.Units
{
    [TestClass]
  public class AccountDeviceServiceUnitTest {
    #region ctor
    private readonly IAccountDeviceService _accountDeviceService;

    public AccountDeviceServiceUnitTest() {
      _accountDeviceService = ServiceLocator.Current.GetInstance<IAccountDeviceService>();
    }
    #endregion

    [TestMethod, TestCategory("AccountDeviceService"), TestCategory("First")]
    public void First() {
      var accountDevice = _accountDeviceService.First(p => p.Id == 1);
      Assert.IsNotNull(accountDevice);
      Console.WriteLine(JsonConvert.SerializeObject(accountDevice.Account));
    }

    [TestMethod, TestCategory("AccountDeviceService"), TestCategory("Add")]
    public void Add() {
      var accountDevice = new AccountDevice {
        AccountId = 1,
        DeviceId = "DeviceId",
        DeviceName = "DeviceName",
        DeviceType = "DeviceType"
      };
      var result = _accountDeviceService.Add(accountDevice);
      Assert.IsNotNull(result);
    }
  }
}
