using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.ReceiveService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;


namespace DITS.HILI.WMS.ReceiveAPIs.Controllers
{
    /// <summary>
    /// LoadingInController
    /// </summary>
    [Authorize]
    public partial class LoadingInController : BaseApiController
    {
        private readonly IReceiveServiceHH _Service;
        private readonly IMessageService _MessageService;

        /// <summary>
        /// LoadingInController
        /// </summary>
        /// <param name="service"></param>
        /// <param name="messageService"></param>
        public LoadingInController(IReceiveServiceHH service, IMessageService messageService)
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
        [Route("api/LoadingIn/GetPalletTagData")]
        public async Task<ApiResponseMessage> GetReceivingByPalletCode(string palletCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                List<ReceivingStatusEnum> status = new List<ReceivingStatusEnum>
                {
                    ReceivingStatusEnum.Inprogress,
                    ReceivingStatusEnum.WaitApprove
                };

                PalletTagModel result = _Service.GetReceivingByPalletCode(palletCode, status);


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
        /// ReceivePallet
        /// </summary>
        /// <param name="palletCode"></param>
        /// <param name="receiveQty"></param>
        /// <param name="suggestLocation"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/LoadingIn/ReceivePallet")]
        public async Task<ApiResponseMessage> ReceivePallet(string palletCode, decimal receiveQty, string suggestLocation)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = _Service.ReceivePallet(palletCode, receiveQty, suggestLocation);

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
        /// 
        /// </summary>
        /// <param name="palletCode"></param>
        /// <param name="qty"></param>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/LoadingIn/ConfirmKeep")]
        public async Task<ApiResponseMessage> ConfirmKeep(string palletCode, decimal qty, string locationCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = _Service.ConfirmKeep(palletCode, qty, locationCode);


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