using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public class RandomMaker {
        public string NewNumber(int min = 100000000, int max = 999999999) {
            var rnd = new Random();
            var no = rnd.Next(min, max);
            return no.ToString();
        }

        public string NewToken() {
            var guid = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            return Convert.ToBase64String(guid);
        }
    }
}
