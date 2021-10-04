using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin; 
[assembly: OwinStartup(typeof(DITS.FoodTruckQueue.Web.Startup))]

namespace DITS.FoodTruckQueue.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            // app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
