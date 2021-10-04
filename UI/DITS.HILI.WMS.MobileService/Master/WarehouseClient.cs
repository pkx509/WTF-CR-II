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
    public class WarehouseClient
    {
        static Ref<int> _total = new Ref<int>();

        #region [ Warehouse ]
        public static async Task<Warehouse> GetWarehouseByID(Guid id)
        {
            return await HttpService.Get<Warehouse>("warehouse/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Warehouse>> GetWarehouse(WarehouseType warehouseType, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<Warehouse>>("warehouse/get?warehouseTypeId=" + warehouseType + "&keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        #endregion

        #region [ Zone ]
        public static async Task<Zone> GetZoneByID(Guid id)
        {
            return await HttpService.Get<Zone>("zone/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Zone>> GetZone(Guid warehouseId, ZoneType zoneType, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<Zone>>("zone/get?warehouseId=" + warehouseId + "&zoneTypeId=" + zoneType + "&keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        #endregion

        #region [ Location ]
        public static async Task<Location> GetLocationByID(Guid id)
        {
            return await HttpService.Get<Location>("location/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Location>> GetLocation(Guid? zoneId, LocationTypeEnum? locationType, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<Location>>("location/get?locationType=" + locationType + "&keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<List<Location>> GetLoadingINLocation()
        {
            return await HttpService.Get<List<Location>>("location/getloadingin", _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        }



        #endregion
    }
}
