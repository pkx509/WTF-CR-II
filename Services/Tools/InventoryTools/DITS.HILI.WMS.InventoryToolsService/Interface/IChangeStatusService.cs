using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.ReceiveModel;
using System;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public interface IChangeStatusService : IRepository<Changestatus>
    {
        PalletTagModel GetPalletCode(string palletCode);
        bool UpdateChangestatus(string palletCode, decimal qty, Guid reasonId, Guid productStatusId);
    }
}
