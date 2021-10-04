using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterService.Secure;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ProgramController : BaseApiController
    {
        private readonly IProgramService service;
        private readonly IMessageService _messageService;

        public ProgramController(IProgramService _service,
                              IMessageService messageService)
        {
            service = _service;
            _messageService = messageService;
        }


        [HttpGet]
        [Route("api/program/get")]
        //[ResponseType(typeof(List<Program>))]
        public async Task<ApiResponseMessage> get(Guid appId)
        {
            try
            {
                service.UserID = UserId;
                System.Collections.Generic.List<Program> result = await Task.Run(() =>
                {
                    return service.GetAll(appId, UserId, Language);
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
        [Route("api/program/getbyid")]
        //[ResponseType(typeof(List<Program>))]
        public async Task<ApiResponseMessage> getById(Guid id)
        {
            try
            {
                service.UserID = UserId;
                Program result = await Task.Run(() =>
                {
                    return service.Get(id);
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
        [Route("api/program/getAll")]
        public async Task<ApiResponseMessage> getAll(int programType, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                service.UserID = UserId;
                System.Collections.Generic.List<Program> result = await Task.Run(() =>
                {
                    return service.GetAll((ProgramType)programType, Language, out _totalRecord, pageIndex, pageSize);
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


        [HttpPut]
        [Route("api/program/modify")]
        public async Task<ApiResponseMessage> modify(Program entity)
        {
            try
            {
                service.UserID = UserId;
                await Task.Run(() =>
                {
                    service.Modify(entity);
                });

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