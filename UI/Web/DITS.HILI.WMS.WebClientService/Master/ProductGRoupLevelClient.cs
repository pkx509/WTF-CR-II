using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ProductGroupLevelClient
    {

        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("productbrand/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("productbrand/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductGroupLevel(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductGroupLevel/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductGroupLevel2(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductGroupLevel/getProductGroupLevel2", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductGroupLevelAll(Guid? groupLV2Id, string keyword, bool IsActive, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("groupLV2Id",groupLV2Id.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                   new KeyValuePair<string,string>("IsActive",IsActive.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductGroupLevel/getall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Add(ProductGroupLevel3 entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductGroupLevel/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Modify(ProductGroupLevel3 entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductGroupLevel/modify", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("ProductGroupLevel/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        //static Ref<int> _total = new Ref<int>();
        //public static async Task<ProductGroupLevel1> GetByID(Guid id)
        //{
        //    return await HttpService.Get<ProductGroupLevel1>("productbrand/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<ProductGroupLevel1>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<ProductGroupLevel1>>("productbrand/get?keyword=" + keyword, total, pageIndex, pageSize,  Common.User.UserID, Common.Language, Common.AccessToken);
        //}
    }
}
