using Assets.Utility;
using Assets.Utility.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Claims;

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
                }), DateTime.UtcNow.AddMonths(1),false);

      var claims = _jwtHandler.Validate(token);
    }
  }
}
