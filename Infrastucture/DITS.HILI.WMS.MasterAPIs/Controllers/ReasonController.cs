using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterService.Utility;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ReasonController : BaseApiController
    {
        private readonly IReasonService _Service;
        private readonly IMessageService _messageService;

        public ReasonController(IReasonService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }


        [HttpGet]
        [Route("api/reason/get")]
        public async Task<ApiResponseMessage> get(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                System.Collections.Generic.List<MasterModel.Utility.Reason> result = await Task.Run(() =>
                {
                    return _Service.GetReasons(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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

    }
}