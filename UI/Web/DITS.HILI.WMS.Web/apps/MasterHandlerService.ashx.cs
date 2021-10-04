using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace DITS.HILI.WMS.Web.apps
{
    /// <summary>
    /// Summary description for MasterHandlerService
    /// </summary>
    public class MasterHandlerService : HttpTaskAsyncHandler, IRequiresSessionState
    {
        public override async System.Threading.Tasks.Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            SearchMaster m = (SearchMaster)Enum.Parse(typeof(SearchMaster), context.Request.QueryString["Method"]);

            switch (m)
            {
                case SearchMaster.Product:
                    await getProducts(context);
                    break;
                default:
                    break;
            }
        }

        public async System.Threading.Tasks.Task getProducts(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            string brandId = context.Request.QueryString["brandId"];
            string shapeId = context.Request.QueryString["shapeId"];
            string groupLV3Id = context.Request.QueryString["groupLV3Id"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);



            List<Product> product = new List<Product>();

            Core.Domain.ApiResponseMessage apiResp = ClientService.Master.ProductClient.Get(search, (string.IsNullOrEmpty(brandId) ? Guid.Empty : Guid.Parse(brandId)),
                                                                                  (string.IsNullOrEmpty(shapeId) ? Guid.Empty : Guid.Parse(shapeId)),
                                                                                  (string.IsNullOrEmpty(groupLV3Id) ? Guid.Empty : Guid.Parse(groupLV3Id)), prms.Page, 20).Result;
            int total = 0;
            if (apiResp.IsSuccess)
            {
                product = apiResp.Get<List<Product>>();
                total = apiResp.Totals;
            }



            //Ref<int> total = new Ref<int>();
            //var product = await ClientService.Master.ProductClient.Get(search, (string.IsNullOrEmpty(brandId) ? Guid.Empty : Guid.Parse(brandId)),
            //(string.IsNullOrEmpty(shapeId) ? Guid.Empty : Guid.Parse(shapeId)),
            //(string.IsNullOrEmpty(groupLV3Id) ? Guid.Empty : Guid.Parse(groupLV3Id)), total, prms.Page, 20);
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(product), total));
        }

    }
}