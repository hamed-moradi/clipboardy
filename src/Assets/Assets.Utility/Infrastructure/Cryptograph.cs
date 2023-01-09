using Assets.Model.Common;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public class Cryptograph {
        #region ctor
        private readonly AppSetting _appSetting;

        public Cryptograph(AppSetting appSetting) {
            _appSetting = appSetting;
        }
        #endregion

        public string Md5(string plainText) {
            if(string.IsNullOrWhiteSpace(plainText))
                return null;
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(plainText);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach(var t in hash) {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }

        public string RNG(string password) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool IsEqual(string sentpass, string dbpass) {
            byte[] hashBytes = Convert.FromBase64String(dbpass);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(sentpass, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for(int i = 0; i < 20; i++)
                if(hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }
    }
}