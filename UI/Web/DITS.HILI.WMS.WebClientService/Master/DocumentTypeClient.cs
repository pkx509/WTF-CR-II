using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class DocumentTypeClient
    {

        public static async Task<ApiResponseMessage> Get(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("DocumentType/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(DocumentTypeEnum documentType)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("docType",documentType.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("DocumentType/GetByDocTypeEnum", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetReceiveType(string keyword, int? pageIndex = 0, int? pageSize = 0)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("keyword",keyword),
                                new KeyValuePair<string,string>("docType", DocumentTypeEnum.Receive.ToString()),
                                new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("PageSize",pageSize.ToString())
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("DocumentType/GetReceiveType", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetDispatchType(string keyword, int? pageIndex = 0, int? pageSize = 20)
        {

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("keyword",keyword),
                                new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("PageSize",pageSize.ToString())
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("DocumentType/GetByDocTypeEnum", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetDispatchTypeWithAll(string keyword, int? pageIndex = 0, int? pageSize = 20)
        {

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("keyword",keyword),
                                new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("PageSize",pageSize.ToString())
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("DocumentType/GetByDocTypeEnumWithAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetForInternalReceive(string keyword, int? pageIndex = 0, int? pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("keyword",keyword),
                                new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("PageSize",pageSize.ToString())
                             };

            return await HttpService.GetAsync("DocumentType/GetForInternalReceive", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> GetRefDispatchType(Guid documentID, int? pageIndex = 0, int? pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("documentID",documentID.ToString()),
                                new KeyValuePair<string,string>("PageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("PageSize",pageSize.ToString())
                             };

            return await HttpService.GetAsync("DocumentType/GetRefDispatchType", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
    }
}
