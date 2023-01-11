using Assets.Model.Common;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WebApi.Services;

namespace Presentation.WebApi {
  public static class ModuleInjector {
    public static void AddModules(
        this IServiceCollection services,
        AppSetting appSetting = null) {

      services.AddTransient<IHealthCkeckService, HealthCkeckService>();
    }
  }
}
