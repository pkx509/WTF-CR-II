using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.TransferWarehouseModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.TransferWarehouseService
{
    public interface ITransferWarehouseService : IRepository<TransferWarehouse>
    {
        PalletTagModel GetPalletCode(string palletCode, List<ReceivingStatusEnum> status);
        Guid PutToTruck(string palletCode, Guid ticketCode, string location);
        List<JobTransferWarehouse> CheckExistJobTransfer();
        bool CloseJobTransfer(Guid ticketCode, string truckNo, Guid warehouseI);
        List<PalletTagModel> GetTransferWarehouseDetail(Guid ticketCode);
        bool DeleteTransferDetail(Guid transferDetaiId);
    }
}
