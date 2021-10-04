using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface ILocationService : IRepository<Location>
    {
        LocationModel Get(Guid id);
        List<Location> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Location> Get(Guid zoneId, LocationTypeEnum? locationType, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<LocationModel> Get(Guid zoneId, Guid warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Location> GetLoading();
        List<Location> GetLoadingOut();
        List<LocationModel> GetAll(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<LogicalZoneDetailModel> GetLocationBetweenList(string location_from, string location_to, Guid _zoneid);
        ApiResponseMessage CheckLocation(List<LocationModel> entity);
        void AddLocation(List<Location> entity);
        void ModifyLocation(Location entity);
        void RemoveLocation(Guid id);
    }
}
