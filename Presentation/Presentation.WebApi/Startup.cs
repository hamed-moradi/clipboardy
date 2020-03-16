using System.Reflection;
using Assets.Model.Settings;
using Assets.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Presentation.WebApi.Infrastructure;
using Presentation.WebApi.Services;
using Serilog;
using AutoMapper;
using System;

namespace Presentation.WebApi {
    public class Startup {
        #region ctor
        private readonly IConfiguration _configuration;
        private readonly string _appVersion;
        private readonly string _allowedSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration) {
            _configuration = configuration;
            _appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        public void ConfigureServices(IServiceCollection services) {
            Log.Debug($"SendReceiveWebApi {_appVersion} getting ready...");

            // app setting
            services.Configure<AppSetting>(config => _configuration.Bind(config));
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
            services.AddSingleton(new ServiceLocator(services));

            // add mvc routing
            services.AddControllers(configure => {
                configure.AllowEmptyInputInBodyModelBinding = true;
            });
            services.AddCors(options => {
                options.AddPolicy(_allowedSpecificOrigins,
                builder => {
                    builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
                });
            });

            services.AddAuthentication()
                .AddGoogle(options => {
                    options.ClientId = appSettings.Authentication.Google.ClientId;
                    options.ClientSecret = appSettings.Authentication.Google.ClientSecret;
                });
        }

        public void Configure(
            IApplicationBuilder appBuilder,
            IWebHostEnvironment webHostEnv,
            IHealthCkeckService healthCheck,
            IHostApplicationLifetime appLifetime) {

            if(webHostEnv.IsDevelopment()) {
                appBuilder.UseDeveloperExceptionPage();
            }
            else {
                //appBuilder.UseExceptionHandler("/Home/ErrorHandler");
            }

            if(healthCheck.Analyze()) {
                appBuilder.UseCors(_allowedSpecificOrigins);
                appBuilder.UseHttpsRedirection();
                appBuilder.UseRouting();
                appBuilder.UseEndpoints(endpoints => {
                    endpoints.MapControllers();
                });
            }
            else {
                Log.Warning("The Service doesn't start, check health logs for more info.");
                appLifetime.StopApplication();
            }
        }
    }
}
