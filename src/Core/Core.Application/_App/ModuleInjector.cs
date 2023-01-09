using Assets.Model.Common;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application {
    public static class ModuleInjector {
        public static void AddApplications(
            this IServiceCollection services, 
            AppSetting appSetting = null) {

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountDeviceService, AccountDeviceService>();
            services.AddScoped<IAccountProfileService, AccountProfileService>();
            services.AddScoped<IClipboardService, ClipboardService>();
            //services.AddScoped<IContentTypeService, ContentTypeService>();
        }
    }
}
