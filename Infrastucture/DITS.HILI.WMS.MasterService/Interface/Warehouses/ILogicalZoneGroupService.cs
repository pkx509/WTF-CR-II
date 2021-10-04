using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface ILogicalZoneGroupService : IRepository<LogicalZoneGroup>
    {
        LogicalZoneGroupModel GetById(Guid id);
        List<LogicalZoneGroupModel> Get(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        void AddLogicalZoneGroup(LogicalZoneGroup entity);
        void ModifyLogicalZoneGroup(LogicalZoneGroup entity);
        void RemoveLogicalZoneGroup(Guid id);


    }
}
