using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface ICycleCountService : IRepository<CycleCount>
    {
        CycleCount Get(Guid id);
        List<CycleCountModel> GetAll(int State, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<CycleCountModel> GetlistAll(DateTime? sdte, DateTime? edte, int State, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        CycleCountModel GetAll(string keyword);
        #region Stock
        List<ProductModel> GetProductStock(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<CycleCountModel> GetCycleCountStock(Guid? WarehouseID, Guid? ZoneID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        bool AddCycleCount(CycleCountModel entity);
        bool Approve(CycleCountModel entity);
        bool ModifyCycleCount(CycleCountModel entity);
        bool Remove(string id);
        List<CustomEnumerable> GetCycleCountStatus();
        #endregion

        List<CycleCountModel> GetCycleCountData(Guid? warehouseID);
        CycleCountDetails GetCycleCountDataDetail(string CycleCountCode, Guid? warehouseID, string ScanPallet);
        CycleCountModel GetJobComplete(string CycleCountCode);
        bool ConfirmCounting(Guid CyclecountDetailID, string CycleCountCode, decimal CountingQty, decimal DiffQty, string LotNumber);
    }
}
