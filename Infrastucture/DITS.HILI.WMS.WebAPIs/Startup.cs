using Autofac;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using DITS.HILI.WMS.Core.Infrastructure.Engine.Service;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.MasterService.Secure;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(DITS.HILI.WMS.WebAPIs.Startup))]
namespace DITS.HILI.WMS.WebAPIs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Load Configuration
            AssembliesFactory.LoadPackages();

            HttpConfiguration config = new HttpConfiguration();
            //config.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());

            IContainer depen = DependencyRegistration.RegisterComponents(config);

            WebApiConfig.Register(config);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            IWorkflowService work = depen.Resolve<IWorkflowService>();
            work.Load();

            IUserAccountService uService = depen.Resolve<IUserAccountService>();
            IMessageService mService = depen.Resolve<IMessageService>();
            ConfigureOAuth(app, uService, mService);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }


        public void ConfigureOAuth(IAppBuilder app, IUserAccountService u, IMessageService m)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(Configure.TokenLifeTime),
                Provider = new WMSAuthorizationServerProvider(u, m)
            };

            // Token Generation 
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}