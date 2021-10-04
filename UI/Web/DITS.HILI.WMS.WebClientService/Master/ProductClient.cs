using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Master
{
    public class ProductClient
    {

        #region [Product]
        public static async Task<List<Product>> Get(string keyword, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, Ref<int> total, int? pageIndex, int? pageSize)
        {
            ApiResponseMessage apiResp = Get(keyword, brandId, shapeId, groupLV3Id, pageIndex, pageSize).Result;
            List<Product> data = new List<Product>();

            if (apiResp.IsSuccess)
            {
                data = apiResp.Get<List<Product>>();
                total = apiResp.Totals;
            }
            return data;
        }

        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("product/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }


        public static async Task<ApiResponseMessage> GetAllByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("product/getallbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(string keyword, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("brandId",brandId.ToString()),
                                  new KeyValuePair<string,string>("shapeId",shapeId.ToString()),
                                  new KeyValuePair<string,string>("groupLV3Id",groupLV3Id.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("product/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetByStockCode(string keyword, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("brandId",brandId.ToString()),
                                  new KeyValuePair<string,string>("shapeId",shapeId.ToString()),
                                  new KeyValuePair<string,string>("groupLV3Id",groupLV3Id.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("product/GetByStockCode", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetAll(string keyword, bool IsActive, Guid? productId, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("IsActive",IsActive.ToString()),
                                  new KeyValuePair<string,string>("productId",productId.ToString()),
                                  new KeyValuePair<string,string>("brandId",brandId.ToString()),
                                  new KeyValuePair<string,string>("shapeId",shapeId.ToString()),
                                  new KeyValuePair<string,string>("groupLV3Id",groupLV3Id.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Product/getAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductSelectAll(string productCode, string productName = null, int? pageIndex = 0, int? pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("productCode",productCode),
                                  new KeyValuePair<string,string>("productName",productName),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("Product/GetProductSelectAll", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

        }
        public static async Task<ApiResponseMessage> GetProductForInternalRec(string PONo, string ProductCode, string ProductName
                                                                                , bool IsCreditNote, bool IsNormal, bool ToReprocess
                                                                                , bool FromReprocess, bool IsItemChange, bool IsWithoutGoods
                                                                                , Guid? ReferenceDispatchTypeID, int? pageIndex = 0, int? pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                new KeyValuePair<string,string>("PONo",PONo.ToString()),
                                new KeyValuePair<string,string>("ProductCode",ProductCode),
                                new KeyValuePair<string,string>("ProductName",ProductName),
                                new KeyValuePair<string,string>("IsCreditNote",IsCreditNote.ToString()),
                                new KeyValuePair<string,string>("IsNormal",IsNormal.ToString()),
                                new KeyValuePair<string,string>("ToReprocess",ToReprocess.ToString()),
                                new KeyValuePair<string,string>("FromReprocess",FromReprocess.ToString()),
                                new KeyValuePair<string,string>("IsItemChange",IsItemChange.ToString()),
                                new KeyValuePair<string,string>("IsWithoutGoods",IsWithoutGoods.ToString()),
                                new KeyValuePair<string,string>("ReferenceDispatchTypeID",ReferenceDispatchTypeID.ToString()),
                                new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            return await HttpService.GetAsync("Product/GetProductForInternalRec", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

        }


        public static async Task<ApiResponseMessage> AddProduct(Product entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Product/AddProduct", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyProduct(Product entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Product/ModifyProduct", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveProduct(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("Product/RemoveProduct", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        #endregion [Product]

    }
}
