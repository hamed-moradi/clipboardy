using Assets.Utility;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Common.Units {
    [TestClass]
    public class AccountProfileServiceUnitTest {
        #region ctor
        private readonly IAccountProfileService _accountProfileService;

        public AccountProfileServiceUnitTest() {
            _accountProfileService = ServiceLocator.Current.GetInstance<IAccountProfileService>();
        }
        #endregion

        [TestMethod, TestCategory("AccountProfileService"), TestCategory("CleanForgotPasswordTokensAsync")]
        public void CleanForgotPasswordTokensAsync() {
            var result = _accountProfileService.CleanForgotPasswordTokensAsync(5).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
    }
}
