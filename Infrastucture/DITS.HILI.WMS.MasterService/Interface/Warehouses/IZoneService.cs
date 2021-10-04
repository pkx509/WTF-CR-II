using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IZoneService : IRepository<Zone>
    {
        Zone Get(Guid id);
        List<Zone> Get(Guid warehouseId, Guid? zoneTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Zone> GetAll(Guid? warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ZoneModel> Getlist(Guid? warehouseId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<ZoneModel> GetZoneCombo(Guid? warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ZoneType> GetZoneType(Guid? zoneTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        void AddZoneType(ZoneType entity);
        void ModifyZoneType(ZoneType entity);
        void RemoveZoneType(Guid id);
    }
}
