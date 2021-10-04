using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.TransferWarehouseService;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace DITS.HILI.WMS.TransferWarehouseAPIs
{
    /// <summary>
    /// LoadingInController
    /// </summary>
    [Authorize]
    public partial class TransferWarehouseController : BaseApiController
    {
        private readonly ITransferWarehouseService _Service;
        private readonly IMessageService _MessageService;

        /// <summary>
        /// LoadingInController
        /// </summary>
        /// <param name="service"></param>
        /// <param name="messageService"></param>
        public TransferWarehouseController(ITransferWarehouseService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        /// <summary>
        /// GetPalletTagData
        /// </summary>
        /// <param name="palletCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/GetPalletTagData")]
        public ApiResponseMessage GetReceivingByPalletCode(string palletCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                List<ReceivingStatusEnum> status = new List<ReceivingStatusEnum>
                {
                    ReceivingStatusEnum.WaitApprove,
                    ReceivingStatusEnum.Complete
                };
                PalletTagModel result = _Service.GetPalletCode(palletCode, status);

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

        /// <summary>
        /// PutToTruck
        /// </summary>
        /// <param name="palletCode"></param>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/PutToTruck")]
        public ApiResponseMessage PutToTruck(string palletCode, Guid ticketCode, string location)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                string result = _Service.PutToTruck(palletCode, ticketCode, location).ToString();

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

        /// <summary>
        /// TransferDeletePallet
        /// </summary>
        /// <param name="TransDetailID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/TransferDeletePallet")]
        public ApiResponseMessage TransferDeletePallet(Guid TransDetailID)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = _Service.DeleteTransferDetail(TransDetailID);

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

        /// <summary>
        /// CheckExistJobTransferB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/CheckExistJobTransfer")]
        public ApiResponseMessage CheckExistJobTransfer()
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;



                List<JobTransferWarehouse> result = _Service.CheckExistJobTransfer();

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

        /// <summary>
        /// CloseJobTransfer
        /// </summary> 
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/CloseJobTransfer")]
        public ApiResponseMessage CloseJobTransfer(Guid ticketCode, string truckNo, Guid warehouseId)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;



                bool result = _Service.CloseJobTransfer(ticketCode, truckNo, warehouseId);

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



        /// <summary>
        /// GetTransferWarehouseDetail
        /// </summary> 
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TransferWarehouse/GetTransferWarehouseDetail")]
        public ApiResponseMessage GetTransferWarehouseDetail(Guid ticketCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                List<PalletTagModel> result = _Service.GetTransferWarehouseDetail(ticketCode);

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