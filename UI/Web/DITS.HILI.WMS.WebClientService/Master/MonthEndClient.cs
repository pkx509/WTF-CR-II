using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class MonthEndClient
    {
        public static async Task<ApiResponseMessage> GetAll(int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("monthendprocess/getAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> GetById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString())
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("monthendprocess/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> CreateOrUpdate(Monthend entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("monthendprocess/createorupdate", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Delete(Monthend entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("monthendprocess/delete", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Get(Guid monthendId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("monthendId",monthendId.ToString())
            };
            ApiResponseMessage resp = await HttpService.GetAsync("monthendprocess/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> CheckCutoffDate(DateTime dateref)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("date",dateref.ToString("yyyy/MM/dd HH:mm:ss"))
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("monthendprocess/validatecutoffdate", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
