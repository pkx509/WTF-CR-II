using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class UserAccountClient
    {

        public static async Task<ApiResponseMessage> GetAll(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("useraccounts/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("useraccounts/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Add(UserAccounts entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("useraccounts/adduser", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Modify(UserAccounts entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("useraccounts/modifyuser", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("useraccounts/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }


        public static async Task<ApiResponseMessage> getRole(Guid userId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("userId",userId.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("useraccounts/GetRoles", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> SaveRole(List<UserInRole> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("useraccounts/saveroles", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> getGroup(Guid userId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("userId",userId.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("useraccounts/getgroups", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> SaveGroup(List<UserInGroup> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("useraccounts/SaveGroups", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
