using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utility
{
    public static class JwtHelper
    {
        private static JwtSecurityToken DecodeToken(string token)
        {
            if (token is not null)
            {
                var stream = token;
                var handler = new JwtSecurityTokenHandler();

                var jsonToken = handler.ReadToken(stream);

                var tokenS = jsonToken as JwtSecurityToken;
                return tokenS;
            }
            return null;
        }

        public static string GetRememberMe(string token)
        {
            var tokenS = DecodeToken(token);

            var rememberMe=tokenS.Claims.FirstOrDefault(x=>x.Type== "rememberMe")?.Value;

            return  rememberMe;
        }
    }
}
