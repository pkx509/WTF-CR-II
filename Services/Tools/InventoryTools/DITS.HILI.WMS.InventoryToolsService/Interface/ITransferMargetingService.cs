using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface ITransferMargetingService : IRepository<TRMTransferMarketing>
    {
        List<TRMTransferMarketing> GetTransferMargetinglist(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize);
        TRMTransferMarketing GetTransferMargetingDetail(Guid? TrmId);
        TRMTransferMarketingProduct GetTransferMargetingDetailByPallet(Guid? TrmProductId);
        bool Add(TRMTransferMarketing entity);
        bool ModifyByProduct(TRMTransferMarketing entity);
        bool AddByPallet(TRMTransferMarketingProduct entity);
        bool ModifyByPallet(TRMTransferMarketingProduct entity);
        List<TRMTransferMarketingProduct> OnAssignPick(List<TRMTransferMarketingProduct> entity);
        bool OnApprove(List<TRMTransferMarketingProduct> entity);
        List<ProductModel> GetProductStockByCode(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductModel> GetProductStock(string keyword, string orderno, string productstatuscode, string refcode, out int totalRecords, int? pageIndex, int? pageSize);
        bool RemoveTransfer(Guid id);

        bool RemoveTransferProduct(Guid id);
        bool RemoveTransferProductDetail(Guid id);
        List<TRMTransferMarketingProduct> GetTransferMargetingProductHandheld(string keyword);
        TRMTransferMarketingProductDetail GetTransferMargetingDetailByPalletHandheld(Guid TrmProductDetailID, string Pallet, string Location);
        bool ConfirmPickTransfer(Guid TrmProductID, string Pallet, string Location, decimal ConfirmQty, decimal sumPickQTY);
    }

}
