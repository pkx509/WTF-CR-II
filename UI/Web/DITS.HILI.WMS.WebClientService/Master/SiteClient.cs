using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class SiteClient
    {
        public static async Task<ApiResponseMessage> GetSiteByID(Guid id, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Site/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> GetCmbSite(string keyword, int? pageIndex, int? pageSize)
        {
            if (pageIndex == null)
            {
                pageIndex = 0;
            }

            if (pageSize == null)
            {
                pageSize = 0;
            }

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Site/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> GetSite(Guid? id, string keyword, bool IsActive, int? pageIndex, int? pageSize)
        {
            if (pageIndex == null)
            {
                pageIndex = 0;
            }

            if (pageSize == null)
            {
                pageSize = 0;
            }

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("IsActive",IsActive.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Site/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Add(Site entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Site/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Modify(Site entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Site/modify", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("Site/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
