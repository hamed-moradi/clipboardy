using Assets.Utility;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Common.Units {
    [TestClass]
    public class ClientConnectionServiceUnitTest {
        #region ctor
        private readonly ICompressionHandler _compressionHandler;

        public ClientConnectionServiceUnitTest() {
            _compressionHandler = ServiceLocator.Current.GetInstance<ICompressionHandler>();
        }
        #endregion

        [TestMethod, TestCategory("ClientConnection"), TestCategory("GetAllTokens")]
        public void GetAllTokens() {
            
        }

        [TestMethod, TestCategory("ClientConnection"), TestCategory("SetStatus")]
        public void SetStatus() {
            
        }

        [TestMethod, TestCategory("ClientConnection"), TestCategory("GetStatus")]
        public void GetStatus() {
            
        }
    }
}
