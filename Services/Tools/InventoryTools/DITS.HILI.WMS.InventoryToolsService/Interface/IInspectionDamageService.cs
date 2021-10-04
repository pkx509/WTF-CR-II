using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IInspectionDamageService : IRepository<Changestatus>
    {
        List<Changestatus> GetInspectionDamage(DateTime sdte, DateTime edte, Guid lineId, string status, string search, out int totalRecords, int? pageIndex, int? pageSize);
        bool ApproveInspectionDamage(Guid damageID);
        Changestatus SaveInspectionDamage(Guid damageID, decimal rejectQty, decimal reprocessQty);
        bool SendtoDamage(List<Changestatus> changes);
        bool SendtoReprocess(List<Changestatus> changes);
    }
}
