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
    public class UserAccountController : BaseApiController
    {
        private readonly IUserAccountService _Service;
        private readonly IMessageService _messageService;

        public UserAccountController(IUserAccountService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/UserAccounts/getbyid")]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                _Service.UserID = UserId;
                UserAccounts result = await Task.Run(() =>
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
        [Route("api/UserAccounts/get")]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = UserId;
                List<UserAccounts> result = await Task.Run(() =>
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

        [HttpGet]
        [Route("api/UserAccounts/getuser")]
        public async Task<ApiResponseMessage> getuser(string username)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                UserAccounts result = await Task.Run(() =>
                {
                    return _Service.GetUserByName(username);
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
        [Route("api/UserAccounts/adduser")]
        public async Task<ApiResponseMessage> AddUser(UserAccounts entity)
        {
            try
            {
                _Service.UserID = UserId;
                UserAccounts result = await Task.Run(() =>
                {
                    return _Service.Add(entity);
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
        [Route("api/UserAccounts/modifyuser")]
        public async Task<ApiResponseMessage> ModifyUser(UserAccounts entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.Modify(entity);
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
        [Route("api/UserAccounts/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.Delete(id);
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
        [Route("api/UserAccounts/GetRoles")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetRoles(Guid userId)
        {
            try
            {
                _Service.UserID = UserId;
                List<UserInRole> result = await Task.Run(() =>
                {
                    return _Service.GetRoles(userId);
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
        [Route("api/UserAccounts/saveRoles")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> SaveRoles(List<UserInRole> entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.SaveRoles(entity);
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
        [Route("api/UserAccounts/GetGroups")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetGroups(Guid userId)
        {
            try
            {
                _Service.UserID = UserId;
                List<UserInGroup> result = await Task.Run(() =>
                {
                    return _Service.GetGroups(userId);
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
        [Route("api/UserAccounts/SaveGroups")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> SaveGroups(List<UserInGroup> entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.SaveGroups(entity);
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
