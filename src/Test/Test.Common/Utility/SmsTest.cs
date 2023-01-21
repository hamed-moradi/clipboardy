using Assets.Model.Common;
using Assets.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Utility {
  [TestClass]
  public class SmsTest {
    #region ctor
    private readonly ISMSService _smsService;

    public SmsTest() {
      _smsService = ServiceLocator.Current.GetInstance<ISMSService>();
    }
    #endregion

    [TestMethod, TestCategory("Sms"), TestCategory("Send")]
    public void Send() {
      var result = _smsService.SendAsync(new SMSModel {
        PhoneNo = "09356817681",
        TextBody = "Test.Common.Utility.SmsTest.Send"
      }).GetAwaiter().GetResult();
      //Assert.IsTrue();
    }
  }
}
