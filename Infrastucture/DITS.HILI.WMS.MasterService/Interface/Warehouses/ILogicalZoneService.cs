using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface ILogicalZoneService : IRepository<LogicalZone>
    {
        LogicalZoneModel GetById(Guid id);
        List<LogicalZoneModel> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ConditionConfigModel> GetConditionConfig(string modulename, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void AddLogicalZone(LogicalZone entity);
        void ModifyLogicalZone(LogicalZone entity);
        void RemoveLogicalZone(Guid id);
        List<LogicalZoneGroup> GetLogicalZoneGroup(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DataKeyValueString> GetConditionConfigBy_Configvaliable(string configvaliable, string keyword, out int totalRecords, int? pageIndex, int? pageSize);

    }
}
