using Assets.Utility;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Common.Units {
    [TestClass]
    public class AccountServiceUnitTest {
        #region ctor
        private readonly IAccountService _accountService;

        public AccountServiceUnitTest() {
            _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
        }
        #endregion

        [TestMethod, TestCategory("AccountService"), TestCategory("ChangeLastSignedinAt")]
        public void ChangeLastSignedinAt() {
            var account = _accountService.First(5);
            account.LastSignedinAt = DateTime.Now;
            _accountService.Update(account, needToFetch: false);
        }
    }
}
