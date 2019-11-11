using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using MMK.SmartSystem.Authentication.JwtBearer;
using MMK.SmartSystem.Configuration;
using MMK.SmartSystem.EntityFrameworkCore;
using MMK.CNC.Application;
using MMK.SmartSystem.RealTime;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;

namespace MMK.SmartSystem
{
    [DependsOn(
         typeof(SmartSystemApplicationModule),
         typeof(SmartSystemRealTimeModule),
         typeof(CNCApplicationModule),
         typeof(SmartSystemEntityFrameworkModule),
         typeof(AbpAspNetCoreModule),
        typeof(AbpHangfireAspNetCoreModule)
        , typeof(AbpAspNetCoreSignalRModule)
     )]
    public class SmartSystemWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SmartSystemWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SmartSystemConsts.ConnectionStringName
            );
            Configuration.BackgroundJobs.UseHangfire();

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(SmartSystemApplicationModule).GetAssembly()
                 );
            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(
                   typeof(CNCApplicationModule).GetAssembly()
               );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemWebCoreModule).GetAssembly());
        }
    }
}
