using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using static DITS.HILI.WMS.DispatchModel.CustomModel.DispatchOtherModel;

namespace DITS.HILI.WMS.DispatchService
{
    public interface IDispatchService : IRepository<Dispatch>
    {
        #region Import
        List<ObjectPropertyValidatorException> ImportPreDispatch(List<PreDispatchesImportModel> entity);
        void SaveImportPreDispatch(List<PreDispatchesImportModel> entity);
        #endregion

        #region Dispatch
        List<POListModels> GetPOList(Guid documentTypeID, int? dispatchType, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        DispatchModels GetById(Guid id);
        List<DispatchModels> Get(Guid? documenttypeid, DateTime? deliverlydate, int? status, string pono, string orderno, out int totalRecords, int? pageIndex, int? pageSize);
        string GetDispatchPreFixEnum(DispatchPreFixTypeEnum prefixtype);
        void AddAll(Dispatch entity);
        void ModifyAll(Dispatch entity);
        void ModifyHeader(Dispatch entity);
        void ModifyDetail(List<DispatchDetail> entity);
        void RemoveDispatch(Guid id); 
        bool OnApproveDispatch(string dispatchcode, string pono, DateTime approvedispatchdate);
        bool OnApproveDispatchInternal(string dispatchcode, string pono, string refcode, DateTime approvedispatchdate);
        bool OnApproveDispatchPicking(string dispatchcode, string pono, DateTime approvedispatchdate);
        bool OnCancelDispatch(string dispatchcode, string pono);
        bool OnCancelDispatchInternal(string dispatchcode, string pono);
        bool OnCancelDispatchPicking(string dispatchcode, string pono);
        bool OnCancelAll(string dispatchcode, string pono, string revisereason);
        bool OnCancelDispatchComplete(string pono, string type);
        #endregion

        #region Stock
        List<ProductModel> GetProductStock(string keyword, string orderno, Guid producttsatusId, string refcode, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductModel> GetProductNoneStock(string keyword, Guid producttsatusId, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductModel> GetProductStockByCode(string productcode, string orderno, Guid producttsatusId, string refcode, out int totalRecords);
        List<ProductModel> GetProductNoneStockByCode(string productcode, out int totalRecords);
        List<ProductModel> GetProductStockAllByCode(string productcode, string orderno, Guid producttsatusId, string refcode);
        #endregion

        #region Booking
        bool OnBookingByRule(string dispatchcode, string pono, string refcode);
        List<DPDetailItemBackOrder> OnValidateBookingByRule(string dispatchcode, string pono, string refcode);
        bool ManualBooking(Guid DispatchID, string pallets);
        List<PalletModel> GetPalletBooking(Guid DispatchID, Guid? WarehouseID, string product, string pallet, string Lot, string OrderNo, out int totalRecords, int? pageIndex, int? pageSize);
        bool OnCancel(string dispatchcode, string pono);
        bool OnApproveBooking(string dispatchcode, string pono, string refcode);
        DispatchModels GetBookingById(Guid id);
        DispatchModels GetConsolidateById(Guid id);
        DispatchModels GetPackingById(Guid id);
        DispatchModels GetDispatchCompleteById(Guid id);
        bool RemoveBooking(Guid id);
        bool RemoveBookingAdjustToBackOrder(Guid id);
        bool RemoveBookingToReCalculateBooking(Guid id);
        #endregion

        #region BackOrder
        List<BackOrderModel> GetViewBackOrder(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        bool OnBookingBackOrder(string dispatchcode, string pono, Guid? bookingid);
        bool CheckBackOrder(string pono);
        #endregion

        #region Revise Pono
        DispatchModels GetByPoNo(string pono);
        #endregion

    }
}
