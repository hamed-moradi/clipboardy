using Assets.Model.Common;
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
using Microsoft.AspNetCore.Localization;
using Assets.Resource;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Assets.Model.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Presentation.WebApi {
    public class Startup {
        #region ctor
        private readonly IConfiguration _configuration;
        private readonly string _allowedSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }
        #endregion

        public void ConfigureServices(IServiceCollection services) {
            Log.Debug($"SendReceiveWebApi {BaseViewModel.Version} getting ready...");

            // config localization
            services.AddLocalization(options => options.ResourcesPath = "Assets/Assets.Resource/Tables");
            services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = SupportedCulture.List;
                options.SupportedUICultures = SupportedCulture.List;
            });

            // app settings
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

            // add external authentication
            services
                .AddAuthentication(options => {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
                    options.ClientId = appSettings.Authentication.Google.ClientId;
                    options.ClientSecret = appSettings.Authentication.Google.ClientSecret;
                })
                .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options => {
                    options.ClientId = appSettings.Authentication.Microsoft.ClientId;
                    options.ClientSecret = appSettings.Authentication.Microsoft.ClientSecret;
                });

            // service locator
            services.AddSingleton(new ServiceLocator(services));
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
                appBuilder.UseHsts();
            }

            if(healthCheck.Analyze()) {
                //appBuilder.UseRequestLocalization();
                appBuilder.UseCors(_allowedSpecificOrigins);

                appBuilder.UseHttpsRedirection();
                //appBuilder.UseStaticFiles();
                appBuilder.UseCookiePolicy(new CookiePolicyOptions {
                    //MinimumSameSitePolicy = SameSiteMode.Strict,
                });

                appBuilder.UseRouting();

                appBuilder.UseAuthentication();
                appBuilder.UseAuthorization();

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
