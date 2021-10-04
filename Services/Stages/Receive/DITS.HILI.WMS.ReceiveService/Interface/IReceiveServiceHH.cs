using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.ReceiveModel;
using System.Collections.Generic;

namespace DITS.HILI.WMS.ReceiveService
{
    public interface IReceiveServiceHH : IRepository<Receive>
    {
        bool ReceivePallet(string palletCode, decimal receiveQty, string suggestLocation);
        bool ConfirmKeep(string palletCode, decimal receiveQty, string locationCod);
        PalletTagModel GetReceivingByPalletCode(string palletCode, List<ReceivingStatusEnum> status);
    }
}
