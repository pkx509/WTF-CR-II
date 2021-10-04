using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.ReceiveService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.ReceiveAPIs.Controllers
{
    [Authorize]
    public partial class ReceiveDetailController : BaseApiController
    {
        private readonly IReceiveDetailService _Service;
        private readonly IMessageService _MessageService;

        public ReceiveDetailController(IReceiveDetailService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpDelete]
        [Route("api/receivedetail/cancel")]
        public async Task<ApiResponseMessage> Cancel(Guid id)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    return _Service.Cancel(id);
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