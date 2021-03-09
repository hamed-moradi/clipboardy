using Assets.Model.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain {
    public static class ModuleInjector {
        public static void AddDomains(this IServiceCollection services, AppSetting appSetting = null) {
            services.AddSingleton<ConnectionPool>();
        }
    }
}
