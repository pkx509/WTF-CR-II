using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsService;
using System;
using System.Web.Http;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    public class ChangeStatusController : BaseApiController
    {
        private readonly IChangeStatusService _Service;
        private readonly IMessageService _MessageService;
        public ChangeStatusController(IChangeStatusService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }
        [HttpGet]
        [Route("api/ChangeStatus/GetPalletTag")]
        public ApiResponseMessage GetReceivingByPalletCode(string palletCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                ReceiveModel.PalletTagModel result = _Service.GetPalletCode(palletCode);

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


        [HttpGet]
        [Route("api/ChangeStatus/UpdateStatus")]
        public ApiResponseMessage UpdateStatus(string palletCode, decimal qty, Guid reasonId, Guid productStatusId)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = _Service.UpdateChangestatus(palletCode, qty, reasonId, productStatusId);

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