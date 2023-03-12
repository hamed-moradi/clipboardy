using Assets.Model.Common;
using Assets.Model.View;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Assets.Utility.Infrastructure {
  public class JwtHandler {
    #region ctor
    private readonly AppSetting _appSetting;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly byte[] _buffer;

    public JwtHandler(AppSetting appSetting) {
      _appSetting = appSetting;
      _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
      _buffer = Encoding.UTF8.GetBytes(_appSetting.Authentication.SecurityKey);
    }
    #endregion

    public string Create(ClaimsIdentity claims, DateTime expires) {
      var tokenDescriptor = new SecurityTokenDescriptor {
        Issuer = _appSetting.Authentication.Issuer,
        Audience = _appSetting.Authentication.Audience,
        Expires = expires,
        Subject = claims,
        SigningCredentials = new SigningCredentials(
              new SymmetricSecurityKey(_buffer),
              SecurityAlgorithms.HmacSha256Signature,
              SecurityAlgorithms.Sha512Digest)
      };

      var secretString = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
      var parts = _jwtSecurityTokenHandler.WriteToken(secretString).Split(".");
      var token = new List<string>();
      parts.ToList().ForEach(e => token.Add(WebUtility.UrlEncode(e)));
      return string.Join(".", token);
    }

    public SigninViewModel Bearer(ClaimsIdentity claims, DateTime? expires = null) {
      var expiresAt = expires ?? DateTime.UtcNow.AddMonths(1);
      return new SigninViewModel {
        Token = $"Bearer {Create(claims, expiresAt)}",
        ExpiresAt = expiresAt
      };
    }

    public ClaimsPrincipal Validate(string token) {
      token = token.StartsWith("Bearer ") ? token.Remove(0, 7) : token;

      var param = new TokenValidationParameters {
        ValidIssuer = _appSetting.Authentication.Issuer,
        ValidAudience = _appSetting.Authentication.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(_buffer),
      };

      var parts = token.Split(".");
      var securityToken = new List<string>();
      parts.ToList().ForEach(e => securityToken.Add(WebUtility.UrlDecode(e)));
      try {
        var claimPrincipal = _jwtSecurityTokenHandler.ValidateToken(string.Join(".", securityToken), param, out var validatedToken);
        return claimPrincipal;
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return null;
      }
    }
  }
}
