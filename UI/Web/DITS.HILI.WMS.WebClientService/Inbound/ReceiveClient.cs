using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Inbound
{
    public class ReceiveClient
    {
        /// <summary>
        /// เพิ่มข้อมูล Receive Order
        /// </summary>
        /// <param name="entity">Receive Order Model</param>
        /// <returns>New Order Receive</returns>
        public static async System.Threading.Tasks.Task<Receive> Add(Receive entity)
        {
            Receive result = await HttpService.Post<Receive>("receive/add", entity, Common.User.UserID, Common.Language, Common.AccessToken);
            return result;
        }

        /// <summary>
        /// ค้นหาด้วยรหัส Receive Order 
        /// </summary>
        /// <param name="id">ReceiveID (Guid)</param>
        /// <returns>Receive</returns>
        public static async System.Threading.Tasks.Task<Receive> GetByID(Guid id)
        {
            Receive result = await HttpService.Get<Receive>("receive/getbyid?id=" + id, Common.User.UserID, Common.Language, Common.AccessToken);
            return result;
        }

        /// <summary>
        /// ค้นหาด้วย
        /// </summary>
        /// <param name="keyword">คำที่ต้องการค้นหา</param>
        /// <returns>Collection of Receive</returns>
        public static async System.Threading.Tasks.Task<List<Receive>> GetAll(ReceiveStatusEnum? status, string keyword, DateTime? sdte, DateTime? edte, Ref<int> total, int? pageIndex, int? pageSize)
        {
            List<Receive> result = await HttpService.Get<List<Receive>>("receive/get?receivestatusenum=" + status + "&keyword=" + keyword + "&sdte=" + sdte + "&edte=" + edte,
                total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
            return result;
        }

        public static async System.Threading.Tasks.Task<List<Receiving>> GetReceivingNo(Guid receiveid)
        {
            return await HttpService.Get<List<Receiving>>("receiving/getgrnlist?receiveid=" + receiveid, Common.User.UserID, Common.Language, Common.AccessToken);
        }
        public static async System.Threading.Tasks.Task<List<Receiving>> GetReceivingList(string grnNo)
        {
            return await HttpService.Get<List<Receiving>>("receiving/getgrnlist?grnNo=" + grnNo, Common.User.UserID, Common.Language, Common.AccessToken);
        }

        /// <summary>
        /// ยกเลิก Receive Order
        /// </summary>
        /// <param name="id">ReceiveID (Guid) </param>
        /// <returns>True/False</returns>
        public static async Task<ApiResponseMessage> Cancel(Guid id)
        {
            //var result = await HttpService.Put("receive/cancel?id=" + id, new object(), Common.User.UserID, Common.Language, Common.AccessToken);
            //return result;

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id", id.ToString())
                             };

            return await HttpService.SendAsync("receive/cancel", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> CancelReceiveDetail(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id", id.ToString())
                             };

            ApiResponseMessage resp = await HttpService.SendAsync("receivedetail/cancel", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Save(ReceiveHeaderModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("receive/save", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> SaveInternalReceive(ReceiveHeaderModel entity)
        {
            return await HttpService.SendAsync("receive/SaveInternalReceive", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> ConfirmInternalReceive(ReceiveHeaderModel entity)
        {
            return await HttpService.SendAsync("receive/ConfirmInternalReceive", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        public static async Task<ApiResponseMessage> GenerateDispatch(Guid receiveID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("receiveID", receiveID.ToString())
                             };

            return await HttpService.GetAsync("receive/GenerateDispatch", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendtoProductionControl(List<Guid> receiveIDs)
        {
            return await HttpService.SendAsync("receive/sendtoproductioncontrol", HttpMethodType.Post, receiveIDs, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetReceiveByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id", id.ToString())
                             };

            return await HttpService.GetAsync("receive/getreceivebyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

        }
        public static async Task<ApiResponseMessage> GetAll(DateTime? estDate, ReceiveStatusEnum status, Guid lineID, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("lineID",lineID.ToString()),
                                  new KeyValuePair<string,string>("estDate",estDate.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("receive/getlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> GetAllInternalReceive(DateTime? estDate, ReceiveStatusEnum status, Guid receiveTypeID, string receiveCode, string orderNo, string PONo, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("estDate",estDate.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("receiveTypeID",receiveTypeID.ToString()),
                                  new KeyValuePair<string,string>("receiveCode",receiveCode),
                                  new KeyValuePair<string,string>("orderNo",orderNo),
                                  new KeyValuePair<string,string>("PONo",PONo),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("receive/getallinternalreceive", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
    }
}
