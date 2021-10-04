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
    public class UserGroupController : BaseApiController
    {
        private readonly IUserGroupService _Service;
        private readonly IMessageService _messageService;

        public UserGroupController(IUserGroupService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/UserGroup/getbyid")]
        //[ResponseType(typeof(UserAccounts))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {

                _Service.UserID = UserId;
                UserGroups result = await Task.Run(() =>
                {
                    return _Service.Get(id);
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
        [Route("api/UserGroup/get")]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                int _totalRecord = 0;
                _Service.UserID = UserId;
                List<UserGroups> result = await Task.Run(() =>
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


        [HttpPost]
        [Route("api/UserGroup/add")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> add(UserGroups entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.AddUserGroup(entity);
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
        [Route("api/UserGroup/modify")]
        public async Task<ApiResponseMessage> modify(UserGroups entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.ModifyUserGroup(entity);
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
        [Route("api/UserGroup/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.DeleteUserGroup(id);
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
        [Route("api/usergroup/getprogram")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetPermission(Guid groupId)
        {
            try
            {
                _Service.UserID = UserId;
                List<ProgramInGroup> result = await Task.Run(() =>
                {
                    return _Service.GetProgram(groupId, Language);
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


        [HttpPost]
        [Route("api/usergroup/saveprogram")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> SaveProgram(List<ProgramInGroup> entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.SaveProgram(entity);
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
