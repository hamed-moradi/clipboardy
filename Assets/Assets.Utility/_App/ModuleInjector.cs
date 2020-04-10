using Microsoft.Extensions.DependencyInjection;
using Assets.Model.Common;
using Assets.Utility.Infrastructure;

namespace Assets.Utility {
    public class ModuleInjector {
        public static void Inject(IServiceCollection services, AppSetting appSetting = null) {
            services.AddTransient<Cryptograph>();
            services.AddTransient<RandomMaker>();
            services.AddScoped<CompressionHandler>();
            services.AddScoped<ContentBodyMaker>();
            services.AddScoped<IEmailService, MailKitService>();
            services.AddScoped<ISMSService, ParsGreenSMSService>();
            services.AddSingleton<PropertyMapper>();
            services.AddSingleton<IParameterHandler, ParameterHandler>();
            services.AddSingleton(typeof(IParameterHandler<>), typeof(ParameterHandler<>));
        }
    }
}
