using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DITS.HILI.WMS.Web.Common.DataClients
{
    /// <summary>
    /// Summary description for OptDataHandler
    /// </summary>
    public class OptDataHandler : HttpTaskAsyncHandler, IRequiresSessionState
    {

        public override async System.Threading.Tasks.Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            switch (context.Request.QueryString["Method"])
            {
                case "receivelist":
                    await ReceiveList(context);
                    break;
                case "receivedetaillist":
                    await ReceiveDetailList(context);
                    break;
                case "receivingdetaillist":
                    await ReceivingDetailList(context);
                    break;
                case "receivinggrnlist":
                    await ReceivingGRNList(context);
                    break;
                //case "Adjustment":
                //    await getAdjustment(context);
                //    break;
                //case "ProductOwner":
                //    await getProductOwner(context);
                //    break;
                case "AdjustStock":
                    await getStockBalanceAdjust(context);
                    break;
                case "PalletBooking":
                    await getPalletForBooking(context);
                    break;
                //case "CycleCountList":
                //    await getCycleCountList(context);
                //    break;
                case "CycleCountWithOutAdjust":
                    await getCycleCountNoAdjust(context);
                    break;
                case "ProductStock":
                    await getProductStock(context);
                    break;
                default: break;
            }
        }

        #region "Receive & Receiving"
        public async System.Threading.Tasks.Task ReceiveList(HttpContext context)
        {
            ReceiveStatusEnum? status = null;
            string query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }
            if (!string.IsNullOrEmpty(context.Request["status"]))
            {
                status = (ReceiveStatusEnum?)Enum.Parse(typeof(ReceiveStatusEnum), context.Request["status"]);
            }

            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            List<Receive> data = await DITS.HILI.WMS.ClientService.Inbound.ReceiveClient.GetAll(status, query, null, null, total, null, null);
            context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));

        }
        public async System.Threading.Tasks.Task ReceiveDetailList(HttpContext context)
        {
            System.Guid BaseID = new Guid();
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                BaseID = new Guid(context.Request["query"]);
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Receive _data = await DITS.HILI.WMS.ClientService.Inbound.ReceiveClient.GetByID(BaseID);
            List<ReceiveDetail> _receivedetail = _data.ReceiveDetailCollection.ToList();

            context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(_receivedetail), total.Value));

        }
        public async System.Threading.Tasks.Task ReceivingDetailList(HttpContext context)
        {
            string GRNNo = "";
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                GRNNo = context.Request["query"];
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            List<Receiving> _data = await DITS.HILI.WMS.ClientService.Inbound.ReceiveClient.GetReceivingList(GRNNo);
            context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(_data), total.Value));

        }
        public async System.Threading.Tasks.Task ReceivingGRNList(HttpContext context)
        {
            System.Guid ReceiveID = new Guid();
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                ReceiveID = new Guid(context.Request["query"]);
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            List<Receiving> _data = await DITS.HILI.WMS.ClientService.Inbound.ReceiveClient.GetReceivingNo(ReceiveID);
            context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(_data), total.Value));

        }
        #endregion

        #region [Adjust]
        public async System.Threading.Tasks.Task getCycleCountNoAdjust(HttpContext context)
        {
            string product = context.Request.QueryString["Product"];
            string pallet = context.Request.QueryString["Pallet"];
            string Lot = context.Request.QueryString["Lot"];
            string whID = context.Request.QueryString["WarehouseID"];
            string state = context.Request.QueryString["state"];
            Guid? WarehouseID = null;
            if (!string.IsNullOrEmpty(whID))
            {
                WarehouseID = new Guid(whID);
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            //var data = await HttpService.Get<List<StockActualBalance>>("cyclecount/GetWithoutAdjust?keyword=" + search + "&warehouseCode=" + whCode + "&customerCode=" + custCode + "&pageIndex=" + prms.Start + "&pageSize=" + prms.Limit, total, 0, 0);
            int total = 0;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustStockCycleCount(state, WarehouseID, product, pallet, Lot, prms.Page, prms.Limit).Result;
            List<AdjustModel> data = new List<AdjustModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<AdjustModel>>();
            }
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(data), total));

        }
        public async System.Threading.Tasks.Task getStockBalanceAdjust(HttpContext context)
        {
            string product = context.Request.QueryString["Product"];
            string pallet = context.Request.QueryString["Pallet"];
            string Lot = context.Request.QueryString["Lot"];
            string whID = context.Request.QueryString["WarehouseID"];
            string state = context.Request.QueryString["state"];
            Guid? WarehouseID = null;
            if (!string.IsNullOrEmpty(whID))
            {
                WarehouseID = new Guid(whID);
            }

            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            //var data = await HttpService.Get<List<StockActualBalance>>("cyclecount/GetWithoutAdjust?keyword=" + search + "&warehouseCode=" + whCode + "&customerCode=" + custCode + "&pageIndex=" + prms.Start + "&pageSize=" + prms.Limit, total, 0, 0);
            int total = 0;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustStockOther(state, WarehouseID, product, pallet, Lot, prms.Page, prms.Limit).Result;
            List<AdjustModel> data = new List<AdjustModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<AdjustModel>>();
            }
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(data), total));

        }
        #endregion [Adjust]

        #region Dispatch
        public async System.Threading.Tasks.Task getPalletForBooking(HttpContext context)
        {
            string product = context.Request.QueryString["Product"];
            string pallet = context.Request.QueryString["Pallet"];
            string Lot = context.Request.QueryString["Lot"];
            string OrderNo = context.Request.QueryString["OrderNo"];
            string whID = context.Request.QueryString["WarehouseID"];
            string dpID = context.Request.QueryString["DispatchID"];
            Guid? WarehouseID = Guid.Empty;
            if (!string.IsNullOrEmpty(whID))
            {
                WarehouseID = new Guid(whID);
            }
            Guid dispatchID = new Guid(dpID);
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            //var data = await HttpService.Get<List<StockActualBalance>>("cyclecount/GetWithoutAdjust?keyword=" + search + "&warehouseCode=" + whCode + "&customerCode=" + custCode + "&pageIndex=" + prms.Start + "&pageSize=" + prms.Limit, total, 0, 0);
            int total = 0;

            Core.Domain.ApiResponseMessage apiResp = DispatchClient.GetPalletBooking(dispatchID, WarehouseID, product, pallet, Lot, OrderNo, prms.Page, prms.Limit).Result;
            List<PalletModel> data = new List<PalletModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<PalletModel>>();
            }
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(data), total));

        }
        #endregion
        public async System.Threading.Tasks.Task getProductStock(HttpContext context)
        {
            string query = context.Request.QueryString["query"];
            string whID = context.Request.QueryString["WarehouseID"];
            string zID = context.Request.QueryString["ZoneID"];
            Guid? WarehouseID = null;
            if (!string.IsNullOrEmpty(whID))
            {
                WarehouseID = new Guid(whID);
            }

            Guid? ZoneID = null;
            if (!string.IsNullOrEmpty(zID))
            {
                ZoneID = new Guid(zID);
            }

            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            int total = 0;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetProductStock(WarehouseID, ZoneID, query, prms.Page, prms.Limit).Result;
            List<CycleCountModel> data = new List<CycleCountModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<CycleCountModel>>();
            }
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(data), total));

        }

    }
}