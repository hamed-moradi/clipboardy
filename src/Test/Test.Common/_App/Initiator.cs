using Assets.Model.Common;
using Core.Domain._App;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.WebApi.Infrastructure;
using Serilog;

namespace Test.Common {
  [TestClass]
  public class Startup {
    [AssemblyInitialize]
    public static void Init(TestContext testContext) {
      var configuration = new ConfigurationBuilder()
          .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();

      Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();

      var services = new ServiceCollection();
      Log.Debug($"SendReceiveWebApi 'Test_Project' getting ready...");

      // app setting
      services.Configure<AppSetting>(config => configuration.Bind(config));
      var appSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<AppSetting>>().Value;
      services.AddSingleton(sp => appSettings);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      // No database provider has been configured for this DbContext.
      // A provider can be configured by overriding the 'DbContext.OnConfiguring' method
      // or by using 'AddDbContext' on the application service provider.
      // If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object
      // in its constructor and passes it to the base constructor for DbContext
      services.AddDbContext<PostgresContext>(opt => {
        opt.UseNpgsql(appSettings.ConnectionStrings.Postgres);
      }, ServiceLifetime.Transient);

      // automapper
      services.AddAutoMapper(typeof(MapperProfile));

      // custome services
      Assets.Utility.ModuleInjector.AddUtilities(services);
      Core.Domain.ModuleInjector.AddDomains(services, appSettings);
      Core.Application.ModuleInjector.AddApplications(services);
      Presentation.WebApi.ModuleInjector.AddModules(services, appSettings);

      // service locator
      services.AddSingleton(new Assets.Utility.ServiceLocator(services));
    }
  }

  [TestClass]
  public class Cleanup {
    [AssemblyCleanup]
    public static void Init() {
    }
  }
}
