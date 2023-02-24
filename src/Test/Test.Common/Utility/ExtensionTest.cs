using Assets.Utility.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Utility {
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
