using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MobileService.Inbound
{
    public class ReceiveClient
    {
        public static async Task<List<CustomEnumerable>> GetReceiveStatus()
        {
            return await HttpService.Get<List<CustomEnumerable>>("receive/getreceivestatus", 0, 0, 0, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        public static async Task<List<CustomEnumerable>> GetReceiveDetailStatus()
        {
            return await HttpService.Get<List<CustomEnumerable>>("receive/getreceivedetailstatus", 0, 0, 0, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        public static async Task<List<CustomEnumerable>> GetReceivingStatus()
        {
            return await HttpService.Get<List<CustomEnumerable>>("receive/getreceivingstatus", 0, 0, 0, Common.User.UserID, Common.Language, Common.AccessToken);
        }
         
        public static async Task<List<Receive>> GetReceiveList(ReceiveStatusEnum? status, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        {
            var result = await HttpService.Get<List<Receive>>("receive/get?receivestatusenum=" + status + "&keyword=" + keyword + "&sdte=&edte=",
                                                                total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
            return result.Where(x => (status == null ?
                                        (x.ReceiveStatus != ReceiveStatusEnum.Cancel ||
                                         x.ReceiveStatus != ReceiveStatusEnum.Close ||
                                         x.ReceiveStatus != ReceiveStatusEnum.Complete) : x.ReceiveStatus == status)).ToList();
        }

        public static async Task<Receive> GetReceive(string receiveCode)
        {
            var result = await HttpService.Get<Receive>("receive/getbyreceivecode?code=" + receiveCode, Common.User.UserID, Common.Language, Common.AccessToken);


            List<ReceiveDetail> receiveDetail = result.ReceiveDetailCollection.Where(x => x.IsActive &&
                                                            (x.ReceiveDetailStatus == ReceiveDetailStatusEnum.Inprogress ||
                                                             x.ReceiveDetailStatus == ReceiveDetailStatusEnum.New ||
                                                             x.ReceiveDetailStatus == ReceiveDetailStatusEnum.Partial)).ToList();

            List<Receiving> receiving = new List<Receiving>();
            receiveDetail.ToList().ForEach(item =>
            {
                receiving.AddRange(item.ReceivingCollection.Where(x => x.IsActive && (x.ReceivingStatus == ReceivingStatusEnum.Inprogress)).ToList());
                item.ReceivingCollection = receiving;
            });

            result.ReceiveDetailCollection = receiveDetail;
            return result;
        }

        public static async Task<ReceiveDetail> GetReceiveDetail(Guid receiveDetailID)
        {
            ReceiveDetail result = await HttpService.Get<ReceiveDetail>("receive/getreceivedetail?receiveDetailID=" + receiveDetailID, Common.User.UserID, Common.Language, Common.AccessToken);
            return result;
        }

        public static async Task<bool> ConfirmReceive(Receiving entity)
        {
            return await HttpService.Put("receiving/receiving", entity, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        public static async Task<bool> Finish(Guid receiveID)
        {
            return await HttpService.Put("receiving/finishreceiving?id=" + receiveID, new object(), Common.User.UserID, Common.Language, Common.AccessToken);
        }
    }
}
