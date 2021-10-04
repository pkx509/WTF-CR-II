using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Master
{
    public class LocationClient
    {

        public static async Task<List<Location>> GetLoadingInLocation()
        {
            return await HttpService.Get<List<Location>>("location/getloadingin", Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Location>> GetLocation(Guid? warehouseId, Guid? zoneId, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<Location>>("location/getlocation?warehouseId=" + warehouseId + "&zoneId=" + zoneId, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<Location> GetLocation(string locationNo)
        {
            return await HttpService.Get<Location>("location/getlocation?locationNo=" + locationNo, Common.User.UserID, Common.Language, Common.AccessToken);
        }
    }
}
