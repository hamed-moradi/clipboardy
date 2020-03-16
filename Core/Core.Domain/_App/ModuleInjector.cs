using Assets.Model.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain {
    public class ModuleInjector {
        public static void Inject(IServiceCollection services, AppSetting appSetting = null) {
            services.AddSingleton<ConnectionPool>();
            services.AddDbContext<MsSqlDbContext>(options => {
                options.UseSqlServer(appSetting.ConnectionStrings.MsSql);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}
