using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.DispatchService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.DispatchAPIs.Controllers
{
    [Authorize]
    public class DispatchController : BaseApiController
    {
        private readonly IDispatchService _Service;
        private readonly IMessageService _messageService;

        public DispatchController(IDispatchService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }
        #region Import
        [HttpPost]
        [Route("api/dispatch/importdispatch")]
        public async Task<ApiResponseMessage> Get(List<PreDispatchesImportModel> entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<ObjectPropertyValidatorException> result = await Task.Run(() =>
                {
                    return _Service.ImportPreDispatch(entity);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/dispatch/saveimportdispatch")]
        public async Task<ApiResponseMessage> SaveImportPreDispatch(List<PreDispatchesImportModel> entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.SaveImportPreDispatch(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region Dispatch

        [HttpGet]
        [Route("api/dispatch/GetPOList")]
        public async Task<ApiResponseMessage> GetPOList(Guid documentTypeID, int? dispatchStatus, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                List<POListModels> result = await Task.Run(() =>
                {
                    return _Service.GetPOList(documentTypeID, dispatchStatus, keyword, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getbyid")]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetById(id);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/get")]
        public async Task<ApiResponseMessage> Get(Guid? documenttypeid, DateTime? deliverlydate, int? status, string pono, string orderno, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<DispatchModels> result = await Task.Run(() =>
                {
                    return _Service.Get(documenttypeid, deliverlydate, status, pono, orderno, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getdispatchprefixenum")]
        public async Task<ApiResponseMessage> GetDispatchPreFixEnum(DispatchPreFixTypeEnum prefixtype)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                string result = await Task.Run(() =>
                {
                    return _Service.GetDispatchPreFixEnum(prefixtype);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpPost]
        [Route("api/dispatch/add")]
        public async Task<ApiResponseMessage> add(Dispatch entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddAll(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPut]
        [Route("api/dispatch/modify")]
        public async Task<ApiResponseMessage> modify(Dispatch entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.ModifyAll(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPut]
        [Route("api/dispatch/modifyheader")]
        public async Task<ApiResponseMessage> modifyheader(Dispatch entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _Service.ModifyHeader(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/dispatch/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveDispatch(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/approvedispatch")]
        public async Task<ApiResponseMessage> OnApproveDispatch(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnApproveDispatch(dispatchcode, pono, approvedispatchdate);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/approvedispatchinternal")]
        public async Task<ApiResponseMessage> OnApproveDispatchInternal(string dispatchcode, string pono, string refcode, DateTime approvedispatchdate)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnApproveDispatchInternal(dispatchcode, pono, refcode, approvedispatchdate);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/approvedispatchpicking")]
        public async Task<ApiResponseMessage> OnApproveDispatchpicking(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnApproveDispatchPicking(dispatchcode, pono, approvedispatchdate);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/cancelall")]
        public async Task<ApiResponseMessage> cancelall(string dispatchcode, string pono, string revisereason)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancelAll(dispatchcode, pono, revisereason);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/canceldispatch")]
        public async Task<ApiResponseMessage> OnCancelDispatch(string dispatchcode, string pono)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancelDispatch(dispatchcode, pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/canceldispatchinternal")]
        public async Task<ApiResponseMessage> OnCancelDispatchInternal(string dispatchcode, string pono)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancelDispatchInternal(dispatchcode, pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/canceldispatchpicking")]
        public async Task<ApiResponseMessage> OnCancelDispatchPicking(string dispatchcode, string pono)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancelDispatchPicking(dispatchcode, pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/dispatch/OnCancelDispatchComplete")]
        public async Task<ApiResponseMessage> OnCancelDispatchComplete(string pono, string type)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancelDispatchComplete(pono, type);
                });

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        #endregion

        #region Stock
        [HttpGet]
        [Route("api/dispatch/getproductstock")]
        public async Task<ApiResponseMessage> GetProductStock(string keyword, string orderno, Guid productstatusId, string refcode, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductStock(keyword, orderno, productstatusId, refcode, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getproductnonestock")]
        public async Task<ApiResponseMessage> GetProductNoneStock(string keyword, Guid productstatusId, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductNoneStock(keyword, productstatusId, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/dispatch/getproductstockbycode")]
        public async Task<ApiResponseMessage> GetProductStockByCode(string productcode, string orderno, Guid productstatusId, string refcode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductStockByCode(productcode, orderno, productstatusId, refcode, out _totalRecord);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getproductnonestockbycode")]
        public async Task<ApiResponseMessage> GetProductNoneStockByCode(string productcode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductNoneStockByCode(productcode, out _totalRecord);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getproductstockallbycode")]
        public async Task<ApiResponseMessage> GetProductStockAllByCode(string productcode, string orderno, Guid productstatusId, string refcode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductStockAllByCode(productcode, orderno, productstatusId, refcode);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region Booking
        [HttpGet]
        [Route("api/dispatch/booking")]
        public async Task<ApiResponseMessage> OnBooking(string dispatchcode, string pono, string refcode)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    //return _Service.OnBooking(dispatchcode, pono);
                    return _Service.OnBookingByRule(dispatchcode, pono, refcode);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/validatebooking")]
        public async Task<ApiResponseMessage> OnValidateBooking(string dispatchcode, string pono, string refcode)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                List<DispatchOtherModel.DPDetailItemBackOrder> result = await Task.Run(() =>
                {
                    //return _Service.OnValidateBooking(dispatchcode, pono);
                    return _Service.OnValidateBookingByRule(dispatchcode, pono, refcode);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/manualbooking")]
        public async Task<ApiResponseMessage> GetPallevtBooking(Guid DispatchID, string pallets)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                bool result = await Task.Run(() =>
                {
                    return _Service.ManualBooking(DispatchID, pallets);
                });

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/dispatch/GetPalletBooking")]
        public async Task<ApiResponseMessage> GetPalletBooking(Guid DispatchID, Guid? WarehouseID, string product, string pallet, string Lot, string OrderNo, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<PalletModel> result = await Task.Run(() =>
                {
                    return _Service.GetPalletBooking(DispatchID, WarehouseID, product, pallet, Lot, OrderNo, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/dispatch/cancelbooking")]
        public async Task<ApiResponseMessage> OnCancel(string dispatchcode, string pono)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnCancel(dispatchcode, pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/approvebooking")]
        public async Task<ApiResponseMessage> OnApproveBooking(string dispatchcode, string pono, string refcode)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnApproveBooking(dispatchcode, pono, refcode);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getbookingbyid")]
        public async Task<ApiResponseMessage> getbookingbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetBookingById(id);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getconsolidatebyid")]
        public async Task<ApiResponseMessage> getconsolidatebyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetConsolidateById(id);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getdispatchcompletebyid")]
        public async Task<ApiResponseMessage> getdispatchcompletebyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetDispatchCompleteById(id);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/getpackingbyid")]
        public async Task<ApiResponseMessage> getpackingbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetPackingById(id);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/dispatch/removebooking")]
        public async Task<ApiResponseMessage> removebooking(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveBooking(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/dispatch/removebookingadjusttobackorder")]
        public async Task<ApiResponseMessage> removebookingadjusttobackorder(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveBookingAdjustToBackOrder(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/dispatch/removebookingtorecalculatebooking")]
        public async Task<ApiResponseMessage> removebookingtorecalculatebooking(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveBookingToReCalculateBooking(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        #endregion

        #region Backorder
        [HttpGet]
        [Route("api/dispatch/getviewbackorder")]
        public async Task<ApiResponseMessage> GetViewBackOrder(string keyword, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<BackOrderModel> result = await Task.Run(() =>
                {
                    return _Service.GetViewBackOrder(keyword, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/bookingbackorder")]
        public async Task<ApiResponseMessage> OnConfirmBookingBackOrder(string dispatchcode, string pono, Guid? bookingid)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.OnBookingBackOrder(dispatchcode, pono, bookingid);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/dispatch/checkbackorder")]
        public async Task<ApiResponseMessage> CheckBackOrder(string pono)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                int _totalRecord = 0;
                bool result = await Task.Run(() =>
                {
                    return _Service.CheckBackOrder(pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }


        #endregion

        #region Revise Pono
        [HttpGet]
        [Route("api/dispatch/getbypono")]
        public async Task<ApiResponseMessage> getbypono(string pono)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchModels result = await Task.Run(() =>
                {
                    return _Service.GetByPoNo(pono);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion
    }
}
