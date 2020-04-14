using Assets.Utility;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
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
            var account = _accountService.FirstAsync(new AccountGetFirstSchema { Id = 3 }).GetAwaiter().GetResult();
            _accountService.UpdateAsync(new AccountUpdateSchema {
                Id = account.Id.Value,
                LastSignedinAt = DateTime.Now
            });
        }
    }
}
