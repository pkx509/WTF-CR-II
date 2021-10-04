using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterService.Secure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class RoleController : BaseApiController
    {
        private readonly IRoleService service;
        private readonly IMessageService _messageService;

        public RoleController(IRoleService _service,
                              IMessageService messageService)
        {
            service = _service;
            _messageService = messageService;
        }


        [HttpGet]
        [Route("api/role/get")]
        public async Task<ApiResponseMessage> get(Guid id)
        {
            try
            {
                service.UserID = UserId;
                Roles result = await Task.Run(() =>
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
        [Route("api/role/getall")]
        public async Task<ApiResponseMessage> getAll(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                service.UserID = UserId;
                List<Roles> result = await Task.Run(() =>
                {
                    return service.GetAll(keyword, out _totalRecord, pageIndex, pageSize);
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


        [HttpPost]
        [Route("api/role/add")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> add(Roles entity)
        {
            try
            {
                service.UserID = UserId;


                await Task.Run(() =>
                {
                    service.Add(entity);
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
        [Route("api/role/modify")]
        public async Task<ApiResponseMessage> modify(Roles entity)
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

        [HttpDelete]
        [Route("api/role/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                service.UserID = UserId;
                await Task.Run(() =>
                {
                    service.Delete(id);
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


        [HttpPost]
        [Route("api/role/savepermission")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> SavePermission(List<PermissionInRole> entity)
        {
            try
            {
                service.UserID = UserId;

                await Task.Run(() =>
                {
                    service.SavePermission(entity);
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
        [Route("api/role/getpermission")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetPermission(Guid ruleID)
        {
            try
            {
                service.UserID = UserId;
                List<PermissionInRole> result = await Task.Run(() =>
                {
                    return service.GetPermission(ruleID);
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
    }
}