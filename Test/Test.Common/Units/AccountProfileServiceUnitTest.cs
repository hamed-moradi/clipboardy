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
    public class AccountProfileServiceUnitTest {
        #region ctor
        private readonly IAccountProfileService _accountProfileService;
        private readonly RandomMaker _randomMaker;

        public AccountProfileServiceUnitTest() {
            _accountProfileService = ServiceLocator.Current.GetInstance<IAccountProfileService>();
            _randomMaker = ServiceLocator.Current.GetInstance<RandomMaker>();
        }
        #endregion

        [TestMethod, TestCategory("AccountProfileService"), TestCategory("First")]
        public void First() {
            var query = new AccountProfileGetFirstSchema { Id = 1 };
            var account = _accountProfileService.FirstAsync(query).GetAwaiter().GetResult();
            Assert.IsTrue(query.StatusCode == 200);
            Assert.IsTrue(account != null);
        }

        [TestMethod, TestCategory("AccountProfileService"), TestCategory("Add")]
        public void Add() {
            var accountProfile = new AccountProfileAddSchema {
                AccountId = 8,
                TypeId = AccountProfileType.Email.ToInt(),
                LinkedId = "test_1@clipboardy.com",
                CreatedAt = DateTime.Now,
                StatusId = Status.Active.ToInt()
            };
            var accountProfileId = _accountProfileService.AddAsync(accountProfile).GetAwaiter().GetResult();
            Assert.IsTrue(accountProfile.StatusCode == 200);
            Assert.IsTrue(accountProfileId > 0);
        }

        [TestMethod, TestCategory("AccountProfileService"), TestCategory("Update")]
        public void Update() {
            var accountProfile = new AccountProfileUpdateSchema {
                Id = 1,
                ForgotPasswordToken = _randomMaker.NewToken(),
                StatusId = Status.Active.ToInt()
            };
            _accountProfileService.UpdateAsync(accountProfile).GetAwaiter().GetResult();
            Assert.IsTrue(accountProfile.StatusCode == 200);
        }

        [TestMethod, TestCategory("AccountProfileService"), TestCategory("CleanForgotPasswordTokens")]
        public void CleanForgotPasswordTokens() {
            var accountProfile = new AccountProfileCleanTokensSchema { AccountId = 8 };
            _accountProfileService.CleanForgotPasswordTokensAsync(accountProfile).GetAwaiter().GetResult();
            Assert.IsTrue(accountProfile.StatusCode == 200);
        }
    }
}
