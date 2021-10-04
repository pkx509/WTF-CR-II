using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DailyPlanModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.DailyPlan
{
    public class ProductionLineClient
    {
        public static async Task<ApiResponseMessage> GetLineByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductionLine/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> GetLine(string keyword, bool Active, LineTypeEnum? lineType, int? pageIndex = 0, int? pageSize = 0)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                   new KeyValuePair<string,string>("Active",Active.ToString()),
                                  new KeyValuePair<string,string>("lineType",lineType.ToString()),
                                  new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("PageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("ProductionLine/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Add(Line entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductionLine/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Modify(Line entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductionLine/modify", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("ProductionLine/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
