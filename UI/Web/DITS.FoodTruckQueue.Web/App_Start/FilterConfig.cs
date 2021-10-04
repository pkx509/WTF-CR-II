using System.Web;
using System.Web.Mvc;

namespace DITS.FoodTruckQueue.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
