using Assets.Model.Common;
using Core.Application.Infrastructure;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application {
    public class ModuleInjector {
        public static void Inject(IServiceCollection services, AppSetting appSetting = null) {
            services.AddSingleton<IStoredProcedureService, StoredProcedureService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountDeviceService, AccountDeviceService>();
            services.AddScoped<IAccountProfileService, AccountProfileService>();
            services.AddScoped<IClipboardService, ClipboardService>();
            //services.AddScoped<IContentTypeService, ContentTypeService>();
        }
    }
}
