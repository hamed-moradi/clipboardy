using Assets.Model;
using Assets.Model.Base;
using System.Linq;

namespace Assets.Utility.Infrastructure {
    public class PropertyMapper {
        public void Bind<T>(T source, T destination)
            where T : IBaseEntity {

            var srcprops = source.GetType().GetProperties();
            var destprops = destination.GetType().GetProperties();

            foreach(var srcprop in srcprops) {
                var srcattrs = srcprop.GetCustomAttributes(true);
                foreach(var srcattr in srcattrs) {
                    var trygetattr = srcattr as UpdatableAttribute;
                    if(trygetattr != null) {
                        var destprop = destprops.FirstOrDefault(f => f.Name.Equals(srcprop.Name));
                        if(destprop != null) {
                            var srcval = srcprop.GetValue(source, null);
                            destprop.SetValue(destination, srcval);
                        }
                    }
                }

            }
        }
    }
}
