using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterService.Core;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{

    public class LanguageController : BaseApiController
    {
        private readonly ILanguageService _Service;
        private readonly IMessageService _messageService;

        public LanguageController(ILanguageService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }
        [HttpGet]
        [Route("api/language/custommessage")]
        //[ResponseType(typeof(Company))]
        public async Task<ApiResponseMessage> GetMessage(string key)
        {
            try
            {
                MasterModel.Core.CustomMessage result = await Task.Run(() =>
                {
                    return _Service.GetMessage(key, Language);
                });

                return Succeed(result);
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
        [Route("api/language/customresource")]
        //[ResponseType(typeof(Company))]
        public async Task<ApiResponseMessage> GetResource(string key)
        {
            try
            {
                MasterModel.Core.CustomResource result = await Task.Run(() =>
                {
                    return _Service.GetResource(key, Language);
                });

                return Succeed(result);
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
        [Route("api/language/language")]
        public async Task<ApiResponseMessage> GetLlanguage()
        {
            try
            {
                System.Collections.Generic.List<MasterModel.Core.Language> result = await Task.Run(() =>
                {
                    return _Service.GetAll();
                });

                return Succeed(result);
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
        [Route("api/ping")]
        //[ResponseType(typeof(List<Program>))]
        public async Task<ApiResponseMessage> ping()
        {
            try
            {
                return Succeed();
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