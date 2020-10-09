using Assets.Model.Common;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Services;

namespace Presentation.WebApi {
    public class ModuleInjector {
        public static void Inject(IServiceCollection services, AppSetting appSetting = null) {
            services.AddTransient<IHealthCkeckService, HealthCkeckService>();
        }
    }
}
