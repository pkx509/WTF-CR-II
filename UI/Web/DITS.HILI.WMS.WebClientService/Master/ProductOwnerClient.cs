using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ProductOwnerClient
    {
        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("productowner/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

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


            ApiResponseMessage resp = await HttpService.GetAsync("productowner/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        //static Ref<int> _total = new Ref<int>();
        //public static async Task<ProductOwner> GetByID(Guid id)
        //{
        //    return await HttpService.Get<ProductOwner>("productowner/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<ProductOwner>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<ProductOwner>>("productowner/get?keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}
    }
}
