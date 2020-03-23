using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public class RandomMaker {
        #region Constructor
        private readonly Random _random;

        public RandomMaker() {
            _random = new Random();
        }
        #endregion

        public string NewNumber(int min = 100000000, int max = 999999999) {
            var number = _random.Next(min, max);
            return number.ToString();
        }

        public string NewToken() {
            var guid = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            return Convert.ToBase64String(guid);
        }

        public int Create(string patern) {
            var start = "0";
            var end = "9";
            if(patern.Length > 1) {
                start = "1";
                for(var index = 1; index < patern.Length; index++) {
                    start += "0";
                    end += "9";
                }
            }
            return _random.Next(int.Parse(start), int.Parse(end));
        }
    }
}
