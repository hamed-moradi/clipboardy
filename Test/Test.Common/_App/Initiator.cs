using Assets.Model.Settings;
using AutoMapper;
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

            // automapper
            services.AddAutoMapper(typeof(MapperProfile));

            // custome services
            Assets.Utility.ModuleInjector.Inject(services);
            Core.Domain.ModuleInjector.Inject(services, appSettings);
            Core.Application.ModuleInjector.Inject(services);
            Presentation.WebApi.ModuleInjector.Inject(services, appSettings);

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
