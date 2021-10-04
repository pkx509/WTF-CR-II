using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ClientService.Outbound
{
    public class DispatchClient
    {
        #region Dispatch

        public static async Task<ApiResponseMessage> GetPOList(Guid documentTypeID, string dispatchStatus, string keyword, int? pageIndex = 0, int? pageSize = 0)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("documentTypeID",documentTypeID.ToString()),
                                  new KeyValuePair<string,string>("dispatchStatus",dispatchStatus),
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/GetPOList", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> GetByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> Get(Guid? documenttypeid, DateTime? deliverlydate, int? status, string pono, string orderno, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("documenttypeid",documenttypeid.ToString()),
                                  new KeyValuePair<string,string>("deliverlydate",deliverlydate.ToString()),
                                  new KeyValuePair<string,string>("status",status.ToString()),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("orderno",orderno),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/get", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetDispatchPreFixEnum(DispatchPreFixTypeEnum prefixtype)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("prefixtype",prefixtype.ToString())
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getdispatchprefixenum", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }


        public static async Task<ApiResponseMessage> Add(Dispatch entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/add", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Modify(Dispatch entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/modify", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> ModifyHeader(Dispatch entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/modifyheader", HttpMethodType.Put, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> Remove(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/remove", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> OnApproveDispatch(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                   new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                   new KeyValuePair<string,string>("pono",pono),
                                   new KeyValuePair<string,string>("approvedispatchdate",approvedispatchdate.ToString())
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/approvedispatch", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnApproveDispatchInternal(string dispatchcode, string pono, string refcode, DateTime approvedispatchdate)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                   new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                   new KeyValuePair<string,string>("pono",pono),
                                   new KeyValuePair<string,string>("refcode",refcode),
                                   new KeyValuePair<string,string>("approvedispatchdate",approvedispatchdate.ToString())
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/approvedispatchinternal", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnApproveDispatchPicking(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                    new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                    new KeyValuePair<string,string>("pono",pono),
                                    new KeyValuePair<string,string>("approvedispatchdate",approvedispatchdate.ToString())
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/approvedispatchpicking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnCancelDispatch(string dispatchcode, string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/canceldispatch", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnCancelDispatchInternal(string dispatchcode, string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/canceldispatchinternal", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnCancelDispatchPicking(string dispatchcode, string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/canceldispatchpicking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnCancelAll(string dispatchcode, string pono, string revisereason)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("revisereason",revisereason)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/cancelall", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnCancelDispatchComplete(string pono, string type)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pono",pono.ToString()),
                                  new KeyValuePair<string,string>("type",type.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/OnCancelDispatchComplete", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        #endregion

        #region ImportDispatch
        public static async Task<ApiResponseMessage> ImportDispatch(List<PreDispatchesImportModel> entity)
        {
            //var parameters = new List<KeyValuePair<String, String>>
            //                 {
            //                      new KeyValuePair<string,string>("PreDispatchesImportModel",PreDispatchesImportModel)
            //                 };


            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/importdispatch", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> SaveImportDispatch(List<PreDispatchesImportModel> entity)
        {
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/saveimportdispatch", HttpMethodType.Post, entity, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        #endregion

        #region Stock
        public static async Task<ApiResponseMessage> GetProductStock(string keyword, string orderno, Guid productstatusId, string refcode, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("orderno",orderno),
                                  new KeyValuePair<string,string>("productstatusId",productstatusId.ToString()),
                                  new KeyValuePair<string,string>("refcode",refcode),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getproductstock", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductNoneStock(string keyword, Guid productstatusId, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("productstatusId",productstatusId.ToString()),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getproductnonestock", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductStockByCode(string code, string orderno, Guid productstatusId, string refcode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("productcode",code),
                                  new KeyValuePair<string,string>("orderno",orderno),
                                  new KeyValuePair<string,string>("productstatusId",productstatusId.ToString()),
                                  new KeyValuePair<string,string>("refcode",refcode)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getproductstockbycode", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductNoneStockByCode(string code)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("productcode",code)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getproductnonestockbycode", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> GetProductStockAllByCode(string code, string orderno, Guid productstatusId, string refcode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("productcode",code),
                                  new KeyValuePair<string,string>("orderno",orderno),
                                  new KeyValuePair<string,string>("productstatusId",productstatusId.ToString()),
                                  new KeyValuePair<string,string>("refcode",refcode)
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getproductstockallbycode", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        #endregion

        #region Booking
        public static async Task<ApiResponseMessage> OnBooking(string dispatchcode, string pono, string refcode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("refcode",refcode)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/booking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnValidateBooking(string dispatchcode, string pono, string refcode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("refcode",refcode)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/validatebooking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetPalletBooking(Guid DispatchID, Guid? WarehouseID, string product, string pallet, string Lot, string OrderNo, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("DispatchID",DispatchID.ToString()),
                                  new KeyValuePair<string,string>("WarehouseID",WarehouseID.ToString()),
                                  new KeyValuePair<string,string>("product",product),
                                  new KeyValuePair<string,string>("pallet",pallet),
                                  new KeyValuePair<string,string>("Lot",Lot),
                                  new KeyValuePair<string,string>("OrderNo",OrderNo),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };

            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/GetPalletBooking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }


        public static async Task<ApiResponseMessage> ManualBooking(Guid dispatchId, string pallets)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("DispatchId",dispatchId.ToString()),
                                  new KeyValuePair<string,string>("pallets",pallets.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/manualbooking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> OnCancel(string dispatchcode, string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/cancelbooking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnApproveBooking(string dispatchcode, string pono, string refcode)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("refcode",refcode)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/approvebooking", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetBookingByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getbookingbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetConsolidateByID(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getconsolidatebyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetDispatchCompleteById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getdispatchcompletebyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> GetPackingById(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getpackingbyid", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveBooking(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/removebooking", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }

        public static async Task<ApiResponseMessage> RemoveBookingAdjustToBackOrder(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/removebookingadjusttobackorder", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }



        public static async Task<ApiResponseMessage> RemoveBookingToReCalculateBooking(Guid id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("id",id.ToString()),
                             };
            ApiResponseMessage resp = await HttpService.SendAsync("dispatch/removebookingtorecalculatebooking", HttpMethodType.Delete, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            return resp;
        }
        #endregion

        #region BackOrder
        public static async Task<ApiResponseMessage> GetViewBackOrder(string keyword, int? pageIndex, int? pageSize)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("keyword",keyword),
                                  new KeyValuePair<string,string>("pageIndex",pageIndex.ToString()),
                                  new KeyValuePair<string,string>("pageSize",pageSize.ToString()),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getviewbackorder", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }

        public static async Task<ApiResponseMessage> OnBookingBackOrder(string dispatchcode, string pono, Guid? bookingid)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("dispatchcode",dispatchcode),
                                  new KeyValuePair<string,string>("pono",pono),
                                  new KeyValuePair<string,string>("bookingid",bookingid.ToString())
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/bookingbackorder", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        public static async Task<ApiResponseMessage> CheckBackOrder(string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pono",pono)
                             };


            // var resp = await HttpService.SendAsync("dispatch/booking", HttpMethodType.Post, parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);
            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/checkbackorder", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion

        #region  Revise PONo
        public static async Task<ApiResponseMessage> GetByPoNo(string pono)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                             {
                                  new KeyValuePair<string,string>("pono",pono),
                             };


            ApiResponseMessage resp = await HttpService.GetAsync("dispatch/getbypono", parameters, Common.User.UserID, Common.Language, Common.AccessToken).ConfigureAwait(false);

            return resp;
        }
        #endregion
    }
}
