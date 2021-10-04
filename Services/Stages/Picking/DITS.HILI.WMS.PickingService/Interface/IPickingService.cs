using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.PickingModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PickingService
{
    public interface IPickingService : IRepository<Picking>
    {
        bool Approve(AssignJobModel model);
        bool Save(AssignJobModel model);
        PickingListHHModel GetPickingHH(string pickingID, string productID);
        string ConfirmPickHH(Guid pickingID, Guid productID, string palletCode, string refPalletCode, decimal confirmQTY, decimal consolidateQTY, decimal orderQTY, string orderUnit, string reason);
        AssignJobModel GetAssignJob(Guid pickingID);
        AssignJobModel GetAssignJobByPO(string PONo);
        PickingListHHModel CheckPallet(Guid pickingID, string palletCode, bool isReprocess);
        List<PickingListHHModel> GetPickingListHH(string keyword);
        List<UserGroups> GetUserWHGroup(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<AssignJobModel> GetAllAssignJob(PickingStatusEnum pickingStatus, DateTime? startDate, DateTime? endDate, string DocNo, string PONo, out int totalRecords, int? pageIndex, int? pageSize);
        List<DispatchforAssignJobModel> GetDispatchforAssignJob(string searchPO, out int totalRecords, int? pageIndex, int? pageSize);
        bool RemovePickingAssign(Guid id);
        PrintPalletTagModel GetPrintPallet(Guid pickingID, string palletCode);
    }
}
