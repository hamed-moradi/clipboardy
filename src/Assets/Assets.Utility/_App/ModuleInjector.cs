using Assets.Model.Common;
using Assets.Utility.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Assets.Utility {
  public static class ModuleInjector {
    public static void AddUtilities(this IServiceCollection services, AppSetting appSetting = null) {
      services.AddTransient<Cryptograph>();
      services.AddTransient<RandomMaker>();
      services.AddScoped<CompressionHandler>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<ISMSService, ParsGreenSMSService>();
      services.AddSingleton<StoredProcedureHelper>();
      services.AddScoped<JwtHandler>();
    }
  }
}
