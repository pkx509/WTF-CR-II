using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DITS.FoodTruckQueue.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string _baseUrl = ConfigurationManager.AppSettings["Service"].ToString();
            string _shareSecret = ConfigurationManager.AppSettings["sharedSecret"].ToString();
            string _clientId = ConfigurationManager.AppSettings["clientId"].ToString();
            string _clietnSecret = ConfigurationManager.AppSettings["clientSecret"].ToString();
            HILI.HttpClientService.HttpClientServiceHelper.BaseUrl = _baseUrl;
            HILI.HttpClientService.HttpClientServiceHelper.ShareSecret = _shareSecret;
            HILI.HttpClientService.HttpClientServiceHelper.ClientId = _clientId;
            HILI.HttpClientService.HttpClientServiceHelper.ClientSecret = _clietnSecret;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
        protected void Session_Start()
        {
            //AutoLogon();
        }
        
    }
}