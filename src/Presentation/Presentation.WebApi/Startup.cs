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
using Assets.Model.Base;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Core.Domain;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Presentation.WebApi {
    public class Startup {
        #region ctor
        private readonly IConfiguration _configuration;
        private readonly string _allowedSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }
        #endregion

        public void ConfigureServices(
            IServiceCollection services) {

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

            // memory cache
            services.AddMemoryCache();

            // automapper
            services.AddAutoMapper(typeof(MapperProfile));

            // custom services
            services.AddUtilities();
            services.AddDomains();
            services.AddApplications();
            services.AddModules();

            // add mvc
            services.AddMvc();

            // add mvc routing
            services.AddControllers(configure => {
                configure.AllowEmptyInputInBodyModelBinding = true;
            });

            // add cors
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

            // identity config
            services.AddAuthentication(
                options => {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg => {
                    cfg.TokenValidationParameters = new TokenValidationParameters() {
                        ValidIssuer = appSettings.Authentication.Issuer,
                        ValidAudience = appSettings.Authentication.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Authentication.SecurityKey))
                    };
                });

            // service locator
            services.AddSingleton(new ServiceLocator(services));
        }

        public void Configure(
            IApplicationBuilder appBuilder,
            IWebHostEnvironment webHostEnv,
            IHealthCkeckService healthCheck,
            IHostApplicationLifetime appLifetime,
            ILogger<Startup> logger) {

            logger.LogInformation("Web api about to start");

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

                appBuilder.UseHttpsRedirection();

                appBuilder.UseStaticFiles();

                appBuilder.UseRouting();

                appBuilder.UseAuthentication();
                appBuilder.UseAuthorization();

                //appBuilder.UseFileServer(false);
                //appBuilder.UseStatusCodePages();

                appBuilder.UseEndpoints(endpoints => {
                    //endpoints.MapControllers();
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }
            else {
                Log.Warning("The Service doesn't start, check health logs for more info.");
                appLifetime.StopApplication();
            }
        }
    }
}
