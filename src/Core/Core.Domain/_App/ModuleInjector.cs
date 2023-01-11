using Assets.Model.Common;
using Core.Domain._App;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain {
  public static class ModuleInjector {
    public static void AddDomains(this IServiceCollection services, AppSetting appSetting = null) {
      services.AddSingleton<PostgresContext>();
    }
  }
}
