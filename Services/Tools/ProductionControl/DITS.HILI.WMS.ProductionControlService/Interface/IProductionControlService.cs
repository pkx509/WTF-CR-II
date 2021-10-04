using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.ProductionControlService
{
    public interface IProductionControlService : IRepository<ProductionControl>
    {
        List<PC_PackingModel> GetAllPacking(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<PC_PackedModel> GetAllPacked(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<PC_PackedModel> GetRePrintList(Guid controlID, bool isProduction);
        List<PC_PackedModel> GetRePrintOutboundList(DateTime? MFGDate, string productName, string PONo, out int totalRecords, int? pageIndex, int? pageSize);
        PalletInfoModel GetPalletInfo(string palletCode, string oldPalletCode, Guid oldProductID, decimal orderQTY);
        PC_PackingModel GetByID(Guid controlID);
        PC_PackingModel PrintPalletTag(PrintPalletModel model);
        CancelPalletModel CancelPallet(CancelPalletModel model);

        void ModifyProductionDetail(ProductionControlDetail entity);
        List<PC_PackedModel> GetPalletList(Guid receiveDetailId);
    }
}
