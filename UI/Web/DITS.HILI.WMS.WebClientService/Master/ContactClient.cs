using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ContactClient
    {
        //public static async task<contact> getbyid(guid id)
        //{
        //    return await httpservice.get<contact>("contact/getbyid?id=" + id, common.user.userid, common.language, common.accesstoken);
        //}

        //public static async Task<List<Contact>> Get(ContactType contactType, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<Contact>>("contact/get?contactType=" + contactType + "&keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<Contact>> GetCustomer(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<Contact>>("contact/getcustomer?keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<Contact>> GetSupplier(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<Contact>>("contact/getsupplier?keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<Contact>> GetProductOwner(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<Contact>>("productowner/get?keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(ContactType contactType, string keyword, bool IsActive, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("contactType",contactType.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("IsActive",IsActive.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetAll(ContactType contactType, Guid? contactID, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("contactID",contactID.ToString()),
                                  new KeyValuePair<string,string>("contactType",contactType.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Add(Contact entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Contact/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Modify(Contact entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Contact/modify", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("Contact/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> GetCustomer(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getcustomer", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetSupplier(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getsupplier", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductOwner(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getsupplier", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetSubDistrict(Guid districtId, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                new KeyValuePair<string,string>("districtId",districtId.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getsubdistrict", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetDistrict(Guid provinceId, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                new KeyValuePair<string,string>("provinceId",provinceId.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getdistrict", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProvince(Guid? provinceId, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                new KeyValuePair<string,string>("provinceId",provinceId.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("contact/getprovince", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
    }
}
