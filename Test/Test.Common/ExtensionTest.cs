using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Utility;
using Assets.Utility.Extension;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Test.Common {
    [TestClass]
    public class ExtensionTest {
        #region ctor

        public ExtensionTest() {
        }
        #endregion

        [TestMethod, TestCategory("Extension"), TestCategory("IsPhoneNumber")]
        public void IsPhoneNumberTest() {
            var number = "09356817681";
            Assert.IsTrue(number.IsPhoneNumber());
        }
    }
}
