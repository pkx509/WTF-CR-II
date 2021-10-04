using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.PutAwayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Inbound
{
    public class PutAwayClient
    {
        private static string prefix = "putaway/";

        //public async static Task<List<PutAwayReason>> GetPutawayReason()
        //{
        //    return await HttpService.Get<List<PutAwayReason>>(prefix + "getputawayreason", Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        public async static Task<List<PutAway>> GetPutAway(Guid? warehouseId, Ref<int> total, int? pageIndex, int? pageSize)
        {
            return await HttpService.Get<List<PutAway>>(prefix + "getmobilejobputawaylist?warehouseId=" + warehouseId, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        //public async static Task<bool> ConfirmPutAway(PutAwayConfirm putawayConfirm)
        //{
        //    try
        //    {

        //        return await HttpService.Put(prefix + "confirmputaway", putawayConfirm, Common.User.UserID, Common.Language, Common.AccessToken);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
