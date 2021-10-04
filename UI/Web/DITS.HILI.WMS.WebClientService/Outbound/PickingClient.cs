using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.PickingModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Outbound
{
    public class PickingClient
    {
        public static async Task<ApiResponseMessage> Approve(AssignJobModel model)
        {
            return await HttpService.SendAsync("picking/approve", HttpMethodType.Post, model, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> Save(AssignJobModel model)
        {
            return await HttpService.SendAsync("picking/save", HttpMethodType.Post, model, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetAllAssignJob(PickingStatusEnum pickingStatus, DateTime? startDate, DateTime? endDate, string docNo, string PONo, int pageIndex, int pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pickingStatus",pickingStatus.ToString()),
                                  new KeyValuePair<string,string>("startDate",startDate.ToString()),
                                  new KeyValuePair<string,string>("endDate",endDate.ToString()),
                                  new KeyValuePair<string,string>("docNo",docNo),
                                  new KeyValuePair<string,string>("PONo",PONo),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("picking/getallassignjob", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetAssignJob(Guid pickingID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pickingID",pickingID.ToString()),
                             };

            return await HttpService.GetAsync("picking/getassignjob", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetAssignJobByPO(string PONo)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("PONo", PONo),
                             };

            return await HttpService.GetAsync("picking/getassignjobbypo", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetUserWHGroup(string keyword, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword", keyword),
                                  new KeyValuePair<string,string>("pageIndex", pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize", pageSize.ToString()),
                             };

            return await HttpService.GetAsync("picking/getuserwhgroup", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetDispatchforAssignJob(string PONo, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("PONo",PONo),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("picking/getdispatchforassignjob", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("picking/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
