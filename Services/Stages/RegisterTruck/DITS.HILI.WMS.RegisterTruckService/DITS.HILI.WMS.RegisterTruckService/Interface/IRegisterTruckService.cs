using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.RegisterTruckService
{
    public interface IRegisterTruckService : IRepository<RegisterTruck>
    {
        RegisterTruck Get(Guid id);
        RegisTruckModel GetbyDetailId(Guid? ShippingID);
        List<DispatchAllModel> GetAll(Guid? ShippingID, string Po, string Doc, out int totalRecords, int? pageIndex, int? pageSize);
        List<DispatchAllModel> GetDispatchForRegisTrucklistAll(Guid? WarehouseID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        DispatchAllModel GetDispatchForRegisTruckById(Guid? WarehouseID, string keyword);
        bool AddRegisTruck(RegisTruckModel entity);
        bool ModifyRegisTruck(RegisTruckModel entity);
        bool AssignModify(RegisTruckModel entity);
        bool RemoveRegisTruck(Guid id);

        #region Consolidate
        RegisterTruckConsolidateHeaderModel GetConsolidateByPO(string pono, string documentNo);
        List<RegisterTruckConsolidateListModel> GetConsolidateAll(string pono, string documentno, int? status, DateTime? datafrom, DateTime? datato, string licenseplate, out int totalRecords, int? pageIndex, int? pageSize);
        bool ModifyConsolidate(List<RegisterTruckConsolidateDeatilModel> entity);
        bool ApproveConsolidate(List<RegisterTruckConsolidateDeatilModel> entity);
        #endregion

        List<RegisterTruckConsolidateDeatilModel> GetConsolidateData(string DocNo);
        RegisterTruckConsolidateDeatilModel GetDetail(string DocumentNo, string pallet);
        List<RegisterTruckConsolidateDeatilModel> GetCheckDocData(Guid ShippingID, string DockNo);
        bool ConfirmConsolidate(string DocumentNo, Guid ShippingID, string pallet, decimal ConfirmQty);
        RegisterTruckConsolidateDeatilModel JobConsoComplete(Guid ShippingID, Guid ShippingDetailID, string PalletCode);
    }
}
