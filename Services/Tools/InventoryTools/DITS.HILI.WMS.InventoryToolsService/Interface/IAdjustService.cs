using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IAdjustService : IRepository<Adjust>
    {
        Adjust Get(Guid id);
        AdjustModel GetAll(string keyword);
        List<AdjustModel> GetlistAll(string State, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<AdjustModel> GetAdjustStockCycleCount(string state, Guid? WarehouseID, string product, string pallet, string Lot, out int totalRecords, int? pageIndex, int? pageSize);
        List<AdjustModel> GetAdjustStockOther(string state, Guid? WarehouseID, string product, string pallet, string Lot, out int totalRecords, int? pageIndex, int? pageSize);
        bool AddAdjust(AdjustModel entity);
        bool Approve(AdjustModel entity);
        bool ModifyAdjust(AdjustModel entity);
        bool Remove(string id);
    }
}
