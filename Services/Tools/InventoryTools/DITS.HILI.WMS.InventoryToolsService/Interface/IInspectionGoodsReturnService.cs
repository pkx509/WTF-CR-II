using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using System;
using System.Collections.Generic;


namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IInspectionGoodsReturnService : IRepository<GoodsReturn>
    {
        List<GoodsReturn> GetInspectionGoodsReturn(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize);
        GoodsReturn GetInspectionGoodsReturnByID(Guid id);
        bool SaveInspectionGoodsReturn(List<ItemGoodsReturn> _return, bool isApprove);
        bool SendtoDamage(List<ItemGoodsReturn> changes);
        bool SendtoReprocess(List<ItemGoodsReturn> changes);
    }
}
