using System;
using System.Globalization;

namespace Assets.Resource {
    public class SupportedCulture {
        public static CultureInfo[] List {
            get {
                return new[] {
                    new CultureInfo("en-US"),
                    new CultureInfo("fa")
                };
            }
        }
    }
}
