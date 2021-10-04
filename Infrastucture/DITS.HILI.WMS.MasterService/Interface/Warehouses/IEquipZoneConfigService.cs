using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IEquipZoneConfigService : IRepository<EquipZoneConfig>
    {
        EquipZoneConfig GetById(Guid id);
        List<EquipZoneConfigModel> Get(string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize);
        void AddEquip(EquipZoneConfig entity);
        void ModifyEquip(EquipZoneConfig entity);
        void RemoveEquip(Guid id);
    }
}
