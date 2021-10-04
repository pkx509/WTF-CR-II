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
    public class PalletTypeController : BaseApiController
    {
        private readonly IPalletTypeService _Service;
        private readonly IMessageService _messageService;

        public PalletTypeController(IPalletTypeService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/PalletType/getbyid")]
        //[ResponseType(typeof(PalletType))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                MasterModel.Utility.PalletType result = await Task.Run(() =>
                {
                    return _Service.Get(id);
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
        [Route("api/PalletType/get")]
        //[ResponseType(typeof(List<PalletType>))]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<MasterModel.Utility.PalletType> result = await Task.Run(() =>
                {
                    return _Service.Get(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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