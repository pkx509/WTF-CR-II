using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.PutAwayModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PutAwayService
{
    public interface IPutAwayService : IRepository<PutAway>
    {
        void CreateJobPutAway(Guid transID, List<PutAwayItem> putawayItem);
        PalletTagModel GetPalletCode(string palletCode);
        void ConfirmReceive(string palletCode, decimal qty);
        void ConfirmKeep(string palletCode, decimal qty, string locationCode);
        void CreateAndConfirmKeep(List<PutAwayItem> putawayItem, string palletCode, decimal qty, string locationCode);
    }
}
