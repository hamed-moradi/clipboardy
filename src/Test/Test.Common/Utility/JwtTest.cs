using Assets.Model.Common;
using Assets.Utility;
using Assets.Utility.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Test.Common.Utility {
    [TestClass]
    public class JwtTest {
        #region ctor
        private readonly JwtHandler _jwtHandler;

        public JwtTest() {
            _jwtHandler = ServiceLocator.Current.GetInstance<JwtHandler>();
        }
        #endregion

        [TestMethod, TestCategory("Jwt"), TestCategory("Generate")]
        public void Generate() {
            var token = _jwtHandler.Create(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, "hamèd"),
                    new Claim(ClaimTypes.Name, "User")
                }));
            
            var claims = _jwtHandler.Validate(token);
        }
    }
}
