using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.PutAwayService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.PutAwayAPIs.Controllers
{
    public class PutAwayController : BaseApiController
    {
        private readonly IPutAwayService service;
        private readonly IMessageService messageService;

        public PutAwayController(IPutAwayService _service, IMessageService _messageService)
        {
            service = _service;
            messageService = _messageService;
        }


        [HttpGet]
        [Route("api/PutAway/GetPalletTag")]
        public async Task<ApiResponseMessage> GetPalletTag(string palletCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                service.UserID = _header.UserID.Value;


                ReceiveModel.PalletTagModel result = service.GetPalletCode(palletCode);


                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/PutAway/ConfirmReceive")]
        public async Task<ApiResponseMessage> ConfirmReceive(string palletCode, decimal qty)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                service.UserID = _header.UserID.Value;


                service.ConfirmReceive(palletCode, qty);

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, messageService.GetMessage("SYS99999", Language));
            }
        }


        [HttpGet]
        [Route("api/PutAway/ConfirmKeep")]
        public async Task<ApiResponseMessage> ConfirmKeep(string palletCode, decimal qty, string locationCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                service.UserID = _header.UserID.Value;


                service.ConfirmKeep(palletCode, qty, locationCode);


                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, messageService.GetMessage("SYS99999", Language));
            }
        }
    }
}
