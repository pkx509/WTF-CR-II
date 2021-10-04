using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IInspectionReclassifiedService : IRepository<Reclassified>
    {
        List<Reclassified> GetInspectionReclassified(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize);
        bool AddInspectionReclassified(List<ItemReclassified> _reclassList);
        bool SaveInspectionReclassified(Reclassified _reclass, bool isApprove = false);
        bool ApproveInspectionReclassified(Reclassified _reclass);
        List<PalletTagModel> GetPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, Guid producttsatusId, out int totalRecords, int? pageIndex, int? pageSize, string WHReferenceCode);
        Reclassified GetInspectionReclassifiedByID(Guid id);
        List<Reclassified> GetInspectionDispatch(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize);
        Reclassified GetInspectionDispatchByID(Guid id);
        bool ApproveInspectionDispatch(Reclassified _reclass);
        bool SendtoDamage(List<ItemReclassified> list);
        bool SendtoReprocess(List<ItemReclassified> list);
    }
}
