using System;
using System.Configuration;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace DITS.HILI.WMS.Web
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string _baseUrl = ConfigurationManager.AppSettings["Service"].ToString();
            string _shareSecret = ConfigurationManager.AppSettings["sharedSecret"].ToString();
            string _clientId = ConfigurationManager.AppSettings["clientId"].ToString();
            string _clietnSecret = ConfigurationManager.AppSettings["clientSecret"].ToString();

            HttpClientService.HttpClientServiceHelper.BaseUrl = _baseUrl;
            HttpClientService.HttpClientServiceHelper.ShareSecret = _shareSecret;
            HttpClientService.HttpClientServiceHelper.ClientId = _clientId;
            HttpClientService.HttpClientServiceHelper.ClientSecret = _clietnSecret;

        }
    }
}