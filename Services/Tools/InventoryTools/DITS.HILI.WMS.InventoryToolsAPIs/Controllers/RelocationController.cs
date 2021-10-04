using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    public class RelocationController : BaseApiController
    {
        private readonly IRelocationService _Service;
        private readonly IMessageService _messageService;
        private readonly IMessageService _MessageService;
        public RelocationController(IRelocationService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/Relocation/GetPalletTag")]
        public ApiResponseMessage GetPalletTag(string barcode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                InventoryToolsModel.RelocationModel result = _Service.GetAll(barcode);

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
        [Route("api/Relocation/UpdateLocation")]
        public async Task<ApiResponseMessage> UpdateLocation(string NewLocation, string LotNumber)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = await Task.Run(() =>
                {
                    return _Service.UpdateLocation(NewLocation, LotNumber);
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