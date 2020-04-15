using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Test.Common.Units {
    [TestClass]
    public class AccountServiceUnitTest {
        #region ctor
        private readonly IAccountService _accountService;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;

        public AccountServiceUnitTest() {
            _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
            _randomMaker = ServiceLocator.Current.GetInstance<RandomMaker>();
            _cryptograph = ServiceLocator.Current.GetInstance<Cryptograph>();
        }
        #endregion

        [TestMethod, TestCategory("AccountService"), TestCategory("Authenticate")]
        public void Authenticate() {
            var query = new AccountAuthenticateSchema { Token = "ZmI0NGE4NTAtYzIyMC00Y2I5LWFlNGItNDNkMTAxNjNmYjYz" };
            var account = _accountService.AuthenticateAsync(query).GetAwaiter().GetResult();
            Assert.IsTrue(query.StatusCode == 200);
            Assert.IsTrue(account != null);
        }

        [TestMethod, TestCategory("AccountService"), TestCategory("First")]
        public void First() {
            var query = new AccountGetFirstSchema { Id = 5 };
            var account = _accountService.FirstAsync(query).GetAwaiter().GetResult();
            Assert.IsTrue(query.StatusCode == 200);
            Assert.IsTrue(account != null);
        }

        [TestMethod, TestCategory("AccountService"), TestCategory("Add")]
        public void Add() {
            var username = _randomMaker.NewNumber();
            var duplicated = _accountService.FirstAsync(new AccountGetFirstSchema { Username = username }).GetAwaiter().GetResult();
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = _accountService.FirstAsync(new AccountGetFirstSchema { Username = username }).GetAwaiter().GetResult();
            }

            var account = new AccountAddSchema {
                Password = _cryptograph.RNG("1234"),
                ProviderId = AccountProvider.Clipboard.ToInt(),
                Username = username,
                CreatedAt = DateTime.Now,
                StatusId = Status.Active.ToInt()
            };
            var accountId = _accountService.AddAsync(account).GetAwaiter().GetResult();
            Assert.IsTrue(account.StatusCode == 200);
            Assert.IsTrue(accountId > 0);
        }

        [TestMethod, TestCategory("AccountService"), TestCategory("Update")]
        public void Update() {
            var account = new AccountUpdateSchema {
                Id = 8,
                Password = _cryptograph.RNG("4321"),
                LastSignedinAt = DateTime.Now,
                StatusId = Status.Active.ToInt()
            };
            _accountService.UpdateAsync(account).GetAwaiter().GetResult();
            Assert.IsTrue(account.StatusCode == 200);
        }

        [TestMethod, TestCategory("AccountService"), TestCategory("Signup")]
        public void Signup() {
            var user = new SignupBindingModel {
                Email = "hamed-moradi@live.com",
                Password = "1234",
                ConfirmPassword = "1234",
                DeviceId = Guid.NewGuid().ToString(),
                DeviceName = "DeviceName",
                DeviceType = "DeviceType"
            };
            var result = _accountService.SignupAsync(user).GetAwaiter().GetResult();

            Console.WriteLine(result.Code);
            Console.WriteLine(result.Data);

            Assert.IsTrue(result.Code == 200);
            Assert.IsTrue(result.Data != null);
        }

        [TestMethod, TestCategory("AccountService"), TestCategory("Signin")]
        public void Signin() {
            var user = new SigninBindingModel {
                Username = "hamed-moradi@live.com",
                Password = "1234"
            };
            var result = _accountService.SigninAsync(user).GetAwaiter().GetResult();

            Console.WriteLine(result.Code);
            Console.WriteLine(result.Data);

            Assert.IsTrue(result.Code == 200);
            Assert.IsTrue(result.Data != null);
        }
    }
}
