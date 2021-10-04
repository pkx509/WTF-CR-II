using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.InventoryToolsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Tools
{
    public class InventoryToolsClient
    {
        #region Cycle Count

        public static async Task<ApiResponseMessage> GetAll(DateTime sdte, DateTime edte, int State, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("State",State.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("CycleCount/GetCycleCountlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetCycleCountDetail(string keyword)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("CycleCount/GetCycleCountDetail", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        #region Stock
        public static async Task<ApiResponseMessage> GetProductStock(Guid? WarehouseID, Guid? ZoneID, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("WarehouseID",WarehouseID.ToString()),
                                  new KeyValuePair<string,string>("ZoneID",ZoneID.ToString()),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("CycleCount/getcycyclecountstock", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion

        public static async Task<ApiResponseMessage> Add(CycleCountModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("CycleCount/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Modify(CycleCountModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("CycleCount/ModifyCycleCount", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Approve(CycleCountModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("CycleCount/Approve", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Remove(string id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("CycleCount/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> getCycleCountstatus()
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>()
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("CycleCount/getCycleCountstatus", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        #endregion

        #region Adjust

        public static async Task<ApiResponseMessage> GetAdjustStockCycleCount(string state, Guid? WarehouseID, string product, string pallet, string Lot, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("state",state),
                                  new KeyValuePair<string,string>("WarehouseID",WarehouseID.ToString()),
                                  new KeyValuePair<string,string>("product",product),
                                  new KeyValuePair<string,string>("pallet",pallet),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("Adjust/GetAdjustStockCycleCount", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetAdjustStockOther(string state, Guid? WarehouseID, string product, string pallet, string Lot, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("state",state),
                                  new KeyValuePair<string,string>("WarehouseID",WarehouseID.ToString()),
                                  new KeyValuePair<string,string>("product",product),
                                  new KeyValuePair<string,string>("pallet",pallet),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("Adjust/GetAdjustStockOther", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetAdjustDetail(string keyword)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Adjust/GetAdjustDetail", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        #region Stock
        public static async Task<ApiResponseMessage> GetAdjustAll(string state, string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("state",state),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Adjust/GetAdjustlist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion

        public static async Task<ApiResponseMessage> Add(AdjustModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Adjust/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Modify(AdjustModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Adjust/ModifyAdjust", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> Approve(AdjustModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Adjust/Approve", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveAdjust(string id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("Adjust/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        //public static async Task<ApiResponseMessage> getCycleCountstatus()
        //{
        //    var parameters = new List<KeyValuePair<String, String>>
        //                     {
        //                          new KeyValuePair<string,string>()
        //                     };


        //    var resp = await HttpService.GetAsync("CycleCount/getCycleCountstatus", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

        //    return resp;
        //}

        #endregion

        #region Inspection Damage

        public static async Task<ApiResponseMessage> GetInspectionDamage(DateTime sdte, DateTime edte, Guid lineId, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("lineId",lineId.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("search",search),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionDamage/GetInspectionDamage", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> SaveInspectionDamage(Guid damageID, decimal rejectQty, decimal reprocessQty)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("damageID",damageID.ToString()),
                                  new KeyValuePair<string,string>("rejectQty",rejectQty.ToString()),
                                  new KeyValuePair<string,string>("reprocessQty",reprocessQty.ToString()),
                             };
            return await HttpService.GetAsync("InspectionDamage/SaveInspectionDamage", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }


        public static async Task<ApiResponseMessage> ApproveInspectionDamage(Guid damageID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("damageID",damageID.ToString()),
                             };

            return await HttpService.GetAsync("InspectionDamage/ApproveInspectionDamage", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendtoReprocess(List<Changestatus> entity)
        {
            return await HttpService.SendAsync("InspectionDamage/SendtoReprocess", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendtoDamage(List<Changestatus> entity)
        {
            return await HttpService.SendAsync("InspectionDamage/SendtoDamage", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        #endregion

        #region Inspection Reclassified

        public static async Task<ApiResponseMessage> GetInspectionReclassified(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("search",search.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionReclassified/GetInspectionReclassified", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetInspectionReclassified(Guid reclassifiedID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("reclassifiedID",reclassifiedID.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionReclassified/GetInspectionReclassifiedByID", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> AddInspectionReclassified(List<ItemReclassified> list)
        {
            return await HttpService.SendAsync("InspectionReclassified/AddInspectionReclassified", HttpMethodType.Post, list, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }


        public static async Task<ApiResponseMessage> SaveInspectionReclassified(Reclassified entity)
        {
            return await HttpService.SendAsync("InspectionReclassified/SaveInspectionReclassified", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }


        public static async Task<ApiResponseMessage> ApproveInspectionReclassified(Reclassified entity)
        {
            return await HttpService.SendAsync("InspectionReclassified/ApproveInspectionReclassified", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);


        }

        public static async Task<ApiResponseMessage> GetPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, Guid producttsatusId, int? pageIndex, int? pageSize, string whRefCode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("palletNo",palletNo),
                                  new KeyValuePair<string,string>("productCode",productCode),
                                  new KeyValuePair<string,string>("productName",productName),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("Line",Line),
                                  new KeyValuePair<string,string>("mfgDate",mfgDate.ToString()),
                                  new KeyValuePair<string,string>("producttsatusId",producttsatusId.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                                  new KeyValuePair<string,string>("WHReferenceCode",whRefCode),

                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionReclassified/GetPalletTag", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion

        #region Inspection Dispatch
        public static async Task<ApiResponseMessage> GetInspectionDispatch(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("search",search.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionReclassified/GetInspectionDispatch", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetInspectionDispatch(Guid reclassifiedID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("reclassifiedID",reclassifiedID.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionReclassified/GetInspectionDispatchByID", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> ApproveInspectionDispatch(Reclassified entity)
        {
            return await HttpService.SendAsync("InspectionReclassified/ApproveInspectionDispatch", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }


        public static async Task<ApiResponseMessage> SendDispatchtoReprocess(List<ItemReclassified> entity)
        {
            return await HttpService.SendAsync("InspectionReclassified/SendtoReprocess", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendDispatchtoDamage(List<ItemReclassified> entity)
        {
            return await HttpService.SendAsync("InspectionReclassified/SendtoDamage", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        #endregion

        #region Inspection  Goods Return
        public static async Task<ApiResponseMessage> GetInspectionGoodsReturn(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("search",search.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionGoodsReturn/GetInspectionGoodsReturn", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetInspectionGoodsReturn(Guid goodsReturnID)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("reclassifiedID",goodsReturnID.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("InspectionGoodsReturn/GetGetInspectionGoodsReturnByID", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> SaveInspectionGoodsReturn(List<ItemGoodsReturn> entity)
        {
            return await HttpService.SendAsync("InspectionGoodsReturn/SaveInspectionGoodsReturn", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }


        public static async Task<ApiResponseMessage> ApproveInspectionGoodsReturn(List<ItemGoodsReturn> entity)
        {
            return await HttpService.SendAsync("InspectionGoodsReturn/ApproveInspectionGoodsReturn", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }



        public static async Task<ApiResponseMessage> SendGoodsReturntoReprocess(List<ItemGoodsReturn> entity)
        {
            return await HttpService.SendAsync("InspectionGoodsReturn/SendtoReprocess", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }

        public static async Task<ApiResponseMessage> SendGoodsReturntoDamage(List<ItemGoodsReturn> entity)
        {
            return await HttpService.SendAsync("InspectionGoodsReturn/SendtoDamage", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
        }
        #endregion

        #region TransferMargeting

        public static async Task<ApiResponseMessage> GetTransferMargetingAll(DateTime sdte, DateTime edte, string status, string search, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("sdte",sdte.ToString()),
                                  new KeyValuePair<string,string>("edte",edte.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("search",search),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("TransferMargeting/GetTransferMargetinglist", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetTransferMargetingDetail(Guid? TrmId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("TrmId",TrmId.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("TransferMargeting/GetTransferMargetingDetail", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetTransferMargetingDetailByPallet(Guid? TrmProductId)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("TrmProductId",TrmProductId.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("TransferMargeting/GetTransferMargetingDetailByPallet", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductStockByCode(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("palletNo",palletNo),
                                  new KeyValuePair<string,string>("productCode",productCode),
                                  new KeyValuePair<string,string>("productName",productName),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("Line",Line),
                                  new KeyValuePair<string,string>("mfgDate",mfgDate.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("TransferMargeting/getproductstockbycode", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetProductStock(string keyword, string orderno, string productstatuscode, string refcode, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("orderno",orderno),
                                  new KeyValuePair<string,string>("productstatuscode",productstatuscode.ToString()),
                                  new KeyValuePair<string,string>("refcode",refcode),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("TransferMargeting/getproductstock", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #region Stock
        public static async Task<ApiResponseMessage> GetTransferPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("palletNo",palletNo),
                                  new KeyValuePair<string,string>("productCode",productCode),
                                  new KeyValuePair<string,string>("productName",productName),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("Line",Line),
                                  new KeyValuePair<string,string>("mfgDate",mfgDate.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Transfer/GetTransferPalletTag", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion

        public static async Task<ApiResponseMessage> AddTransfer(TRMTransferMarketing entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyByProduct(TRMTransferMarketing entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/ModifyByProduct", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> AddTransferByPallet(TRMTransferMarketingProduct entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/addByPallet", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> OnAssignPick(List<TRMTransferMarketingProduct> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/OnAssignPick", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnApprove(List<TRMTransferMarketingProduct> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/OnApprove", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveTransfer(string id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveTransferProduct(string id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/RemoveTransferProduct", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveTransferProductDetail(string id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/RemoveTransferProductDetail", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyTransferByPallet(TRMTransferMarketingProduct entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("TransferMargeting/ModifyByPallet", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyTransfer(CycleCountModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Transfer/ModifyTransfer", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        public static async Task<ApiResponseMessage> ApproveTransfer(CycleCountModel entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("Transfer/Approve", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> getTransferstatus()
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>()
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("Transfer/getTransferstatus", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        #endregion TransferMargeting
    }
}
