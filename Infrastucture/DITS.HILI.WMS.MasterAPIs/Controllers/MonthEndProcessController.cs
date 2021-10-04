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
    public class MonthEndProcessController : BaseApiController
    {

        private readonly IMonthEndService _service;
        private readonly IMessageService _messageService;

        public MonthEndProcessController(IMonthEndService service,
                              IMessageService messageService)
        {
            _service = service;
            _messageService = messageService;
        }


        [HttpGet]
        [Route("api/monthendprocess/getbyid")]
        //[ResponseType(typeof(List<Program>))]
        public async Task<ApiResponseMessage> getById(Guid id)
        {
            try
            {
                _service.UserID = UserId;
                System.Collections.Generic.List<Monthend> result = await Task.Run(() =>
                {
                    return _service.GetAll(id);
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
        [Route("api/monthendprocess/getAll")]
        public async Task<ApiResponseMessage> getAll(int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                System.Collections.Generic.List<Monthend> result = await Task.Run(() =>
                {
                    return _service.GetAll(out _totalRecord, pageIndex, pageSize);
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
        [Route("api/monthendprocess/createorupdate")]
        public async Task<ApiResponseMessage> CreateOrUpdate(Monthend entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.CreateOrUpdate(entity);
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

        [HttpPut]
        [Route("api/monthendprocess/delete")]
        public async Task<ApiResponseMessage> Delete(Monthend entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.Remove(entity);
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
        [HttpPut]
        [Route("api/monthendprocess/InActive")]
        public async Task<ApiResponseMessage> InActive(Monthend entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.Active(entity);
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
        [HttpPut]
        [Route("api/monthendprocess/Active")]
        public async Task<ApiResponseMessage> Active(Monthend entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.Active(entity);
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

        [HttpGet]
        [Route("api/monthendprocess/validatecutoffdate")]
        public async Task<ApiResponseMessage> CheckCutoffDate(DateTime date)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.CheckCutoffDate(date);
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