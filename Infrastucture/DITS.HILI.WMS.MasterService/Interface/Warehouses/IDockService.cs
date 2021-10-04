using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IDockService : IRepository<DockConfig>
    {
        DockConfig Get(Guid id);
        List<DockConfigModel> Get(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<DockConfigModel> GetAll(Guid? warehouseID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
