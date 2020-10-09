using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            var query = new AccountDeviceGetFirstSchema { Id = 5 };
            var account = _accountDeviceService.FirstAsync(query).GetAwaiter().GetResult();
            Assert.IsTrue(query.StatusCode == 200);
            Assert.IsTrue(account != null);
        }

        [TestMethod, TestCategory("AccountDeviceService"), TestCategory("Add")]
        public void Add() {
            var accountDevice = new AccountDeviceAddSchema {
                AccountId = 8,
                Token = _randomMaker.NewToken(),
                DeviceId = Guid.NewGuid().ToString(),
                DeviceName = "DeviceName",
                DeviceType = "DeviceType",
                CreatedAt = DateTime.Now,
                StatusId = Status.Active.ToInt()
            };
            var accountDeviceId = _accountDeviceService.AddAsync(accountDevice).GetAwaiter().GetResult();
            Assert.IsTrue(accountDevice.StatusCode == 200);
            Assert.IsTrue(accountDeviceId > 0);
        }

        [TestMethod, TestCategory("AccountDeviceService"), TestCategory("Update")]
        public void Update() {
            var accountDevice = new AccountDeviceUpdateSchema {
                Id = 15,
                Token = _randomMaker.NewToken(),
                StatusId = Status.Active.ToInt()
            };
            _accountDeviceService.UpdateAsync(accountDevice).GetAwaiter().GetResult();
            Assert.IsTrue(accountDevice.StatusCode == 200);
        }
    }
}
