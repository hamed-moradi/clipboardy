using Assets.Model.Common;
using Assets.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Utility {
    [TestClass]
    public class EmailServiceTest {
        #region ctor
        private readonly IEmailService _emailService;

        public EmailServiceTest() {
            _emailService = ServiceLocator.Current.GetInstance<IEmailService>();
        }
        #endregion

        [TestMethod, TestCategory("EmailService"), TestCategory("Send")]
        public void Send() {
            var result = _emailService.SendAsync(new EmailModel {
                Address = "hamed.international@gmail.com",
                Subject = "sent from unit test",
                Body = "hi there!"
            }).GetAwaiter().GetResult();
            Assert.IsTrue(result.Code == 200);
        }
    }
}
