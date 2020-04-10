using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Domain._App {
    public class StoredProcedureGenerator {
        #region ctor
        public StoredProcedureGenerator(string @namespace) {
            var storedprocs = Assembly.GetExecutingAssembly().GetTypes()
                .Where(w => w.IsClass && w.Namespace == @namespace);

            foreach(var sp in storedprocs) {
                
            }
        }
        #endregion
    }
}
