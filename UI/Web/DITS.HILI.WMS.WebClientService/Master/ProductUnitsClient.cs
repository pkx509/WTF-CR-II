using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ProductUnitsClient
    {
        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("units/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

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


            ApiResponseMessage resp = await HttpService.GetAsync("units/Get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductUnits(Guid? productUnitId, Guid? productId, string keyword, int? pageIndex = 0, int? pageSize = 20)
        {
            string _ProductUnitId = null;
            string _ProductId = null;

            if (productUnitId != null && productUnitId != Guid.Empty)
            {
                _ProductUnitId = productUnitId.ToString();
            }

            if (productId != null && productId != Guid.Empty)
            {
                _ProductId = productId.ToString();
            }

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("productUnitId",_ProductUnitId),
                                  new KeyValuePair<string,string>("productId",_ProductId),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("ProductUnit/GetProductUnitAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> AddProductUnits(ProductUnit entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductUnit/AddUnit", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyProductUnits(ProductUnit entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("ProductUnit/ModifyUnit", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveProductUnits(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("ProductUnit/RemoveUnit", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
    }
}
