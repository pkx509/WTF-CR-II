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
    public class ItfInterfaceMappingController : BaseApiController
    {
        private readonly IItfInterfaceMappingService _Service;
        private readonly IMessageService _messageService;

        public ItfInterfaceMappingController(IItfInterfaceMappingService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/InterfaceMapping/getbydocument")]
        //[ResponseType(typeof(DocumentType))]
        public async Task<ApiResponseMessage> getbydocument(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<MasterModel.Utility.ItfInterfaceMapping> result = await Task.Run(() =>
                {
                    return _Service.GetByDocument(id);
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