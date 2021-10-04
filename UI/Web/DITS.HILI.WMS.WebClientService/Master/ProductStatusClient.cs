using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ProductStatusClient
    {
        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductStatus/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(string keyword, int? pageIndex = 0, int? pageSize = 20, Guid? documentTypeID = null)
        {
            string documentType = null;
            if (documentTypeID != null && documentTypeID != Guid.Empty)
            {
                documentTypeID.ToString();
            }

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                                  new KeyValuePair<string,string>("documentTypeID",documentType)
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("productstatus/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetByDocumentTypeID(Guid documentTypeId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("documentTypeid",documentTypeId.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("productstatus/getbydocumenttypeid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        //static Ref<int> _total = new Ref<int>();
        //public static async Task<ProductStatus> GetByID(Guid id)
        //{
        //    return await HttpService.Get<ProductStatus>("ProductStatus/getbyid?id=" + id, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        //}
        //public static async Task<List<ProductStatus>> GetByDocumentTypeID(Guid documentTypeId)
        //{
        //    return await HttpService.Get<List<ProductStatus>>("ProductStatus/getbydocumenttypeid?documentTypeid=" + documentTypeId, _total, null, null, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<List<ProductStatus>> Get(string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<ProductStatus>>("ProductStatus/get?keyword=" + keyword, total, pageIndex, pageSize,  Common.User.UserID, Common.Language, Common.AccessToken);
        //}
    }
}
