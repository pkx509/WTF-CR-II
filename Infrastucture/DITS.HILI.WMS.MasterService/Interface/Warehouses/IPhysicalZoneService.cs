using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IPhysicalZoneService : IRepository<PhysicalZone>
    {
        PhysicalZone Get(Guid id);
        List<PhysicalZone> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<PhysicalZone> GetPhysicalCombo(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void AddPhysicalZone(PhysicalZone entity);
        void ModifyPhysicalZone(PhysicalZone entity);
        void RemovePhysicalZone(Guid id);
    }
}
