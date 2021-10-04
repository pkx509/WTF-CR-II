using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.InventoryToolsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    /// <summary>
    /// LoadingInController
    /// </summary>
    [Authorize]
    public partial class TransferMarketingController : BaseApiController
    {
        private readonly ITransferMargetingService _Service;
        private readonly IMessageService _MessageService;

        /// <summary>
        /// LoadingInController
        /// </summary>
        /// <param name="service"></param>
        /// <param name="messageService"></param>
        public TransferMarketingController(ITransferMargetingService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdte"></param>
        /// <param name="edte"></param>
        /// <param name="lineId"></param>
        /// <param name="status"></param>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferMargeting/GetTransferMargetinglist")]
        public async Task<ApiResponseMessage> GetTransferMargetinglist(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<TRMTransferMarketing> result = await Task.Run(() =>
                {
                    return _Service.GetTransferMargetinglist(sdte, edte, status, search == null ? "" : search, out _totalRecord, pageIndex, pageSize);
                });

                ApiResponseMessage apiResp = Succeed(result, _totalRecord);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TrmId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferMargeting/GetTransferMargetingDetail")]
        public async Task<ApiResponseMessage> GetTransferMargetingDetail(Guid? TrmId)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                TRMTransferMarketing result = await Task.Run(() =>
                {
                    return _Service.GetTransferMargetingDetail(TrmId);
                });

                ApiResponseMessage apiResp = Succeed(result, _totalRecord);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TransferMargeting/GetTransferMargetingDetailByPallet")]
        public async Task<ApiResponseMessage> GetTransferMargetingDetailByPallet(Guid? TrmProductId)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                TRMTransferMarketingProduct result = await Task.Run(() =>
                {
                    return _Service.GetTransferMargetingDetailByPallet(TrmProductId);
                });

                ApiResponseMessage apiResp = Succeed(result, _totalRecord);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productcode"></param>
        /// <param name="orderno"></param>
        /// <param name="productstatusId"></param>
        /// <param name="refcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferMargeting/getproductstockbycode")]
        public async Task<ApiResponseMessage> GetProductStockByCode(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductStockByCode(palletNo, productCode, productName, Lot, Line, mfgDate, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TransferMargeting/getproductstock")]
        public async Task<ApiResponseMessage> GetProductStock(string keyword, string orderno, string productstatuscode, string refcode, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<MasterModel.Products.ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductStock(keyword, orderno, productstatuscode, refcode, out _totalRecord, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/TransferMargeting/add")]
        public async Task<ApiResponseMessage> add(TRMTransferMarketing entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.Add(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPut]
        [Route("api/TransferMargeting/ModifyByProduct")]
        public async Task<ApiResponseMessage> ModifyByProduct(TRMTransferMarketing entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyByProduct(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TransferMargeting/addByPallet")]
        public async Task<ApiResponseMessage> addByPallet(TRMTransferMarketingProduct entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddByPallet(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPut]
        [Route("api/TransferMargeting/ModifyByPallet")]
        public async Task<ApiResponseMessage> ModifyByPallet(TRMTransferMarketingProduct entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyByPallet(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TransferMargeting/OnAssignPick")]
        public async Task<ApiResponseMessage> OnAssignPick(List<TRMTransferMarketingProduct> entity)
        {
            try
            {
                _Service.UserID = UserId;
                List<TRMTransferMarketingProduct> result = await Task.Run(() =>
                {
                    return _Service.OnAssignPick(entity);
                });

                return Succeed(result);
                //var _header = ApiHelpers.Response(Request);
                //await Task.Run(() =>
                //{
                //    _Service.OnAssignPick(entity);
                //});

                //return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TransferMargeting/OnApprove")]
        public async Task<ApiResponseMessage> OnApprove(List<TRMTransferMarketingProduct> entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                await Task.Run(() =>
                {
                    _Service.OnApprove(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/TransferMargeting/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.RemoveTransfer(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/TransferMargeting/removeTransferProduct")]
        public async Task<ApiResponseMessage> removeTransferProduct(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.RemoveTransferProduct(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/TransferMargeting/RemoveTransferProductDetail")]
        public async Task<ApiResponseMessage> RemoveTransferProductDetail(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.RemoveTransferProductDetail(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TransferMargeting/GetPickingTransferListHH")]
        public async Task<ApiResponseMessage> GetPickingTransferListHH(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<TRMTransferMarketingProduct> result = await Task.Run(() =>
                {
                    return _Service.GetTransferMargetingProductHandheld(keyword);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TransferMargeting/CheckTransferPallet")]
        public async Task<ApiResponseMessage> CheckTransferPallet(Guid TrmProductDetailID, string Pallet, string Location)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                TRMTransferMarketingProductDetail result = await Task.Run(() =>
                {
                    return _Service.GetTransferMargetingDetailByPalletHandheld(TrmProductDetailID, Pallet, Location);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/TransferMargeting/ConfirmPickTransferHH")]
        public async Task<ApiResponseMessage> Confirm(Guid TrmProductId, string palletCode, string location, decimal confirmQTY, decimal sumPickQTY)
        {
            try
            {
                _Service.UserID = UserId;
                bool result = await Task.Run(() =>
                {
                    return _Service.ConfirmPickTransfer(TrmProductId, palletCode, location, confirmQTY, sumPickQTY);
                });


                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }
    }
}