using Assets.Utility;
using Assets.Utility.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Utility {
    [TestClass]
    public class CompressionTest {
        #region ctor
        private readonly CompressionHandler _compressionHandler;

        public CompressionTest() {
            _compressionHandler = ServiceLocator.Current.GetInstance<CompressionHandler>();
        }
        #endregion

        [TestMethod, TestCategory("Compression"), TestCategory("RoundTrip")]
        public void RoundTrip() {
            var key = "Q1Tch3JCS9inoQ8brOW9UA";
            var comped = _compressionHandler.Compress(key);
            var decomped = _compressionHandler.Decompress(comped);

            Assert.AreEqual(key, decomped);
        }
    }
}
