using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterService.Contacts;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ContactController : BaseApiController
    {
        private readonly IContactService _Service;
        private readonly IMessageService _messageService;

        public ContactController(IContactService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/Contact/getbyid")]
        //[ResponseType(typeof(Contact))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Contact result = await Task.Run(() =>
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
        [Route("api/Contact/get")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> get(ContactType contactType, string keyword, bool IsActive, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Contact> result = await Task.Run(() =>
                {
                    return _Service.Get(contactType, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Contact/getAll")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getAll(ContactType contactType, Guid? contactID, string keyword, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Contact> result = await Task.Run(() =>
                {
                    return _Service.GetAll(contactType, contactID, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Contact/add")]
        public async Task<ApiResponseMessage> add(Contact entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.Add(entity);
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
        [Route("api/Contact/modify")]
        public async Task<ApiResponseMessage> modify(Contact entity)
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
        [Route("api/Contact/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.Remove(id);
                });

                return Succeed();
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/Contact/getcustomer")]
        // [ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getcustomer(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Contact> result = await Task.Run(() =>
                {
                    return _Service.GetCustomer(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
        [Route("api/Contact/getsupplier")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getsupplier(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Contact> result = await Task.Run(() =>
                {
                    return _Service.GetSupplier(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
        [Route("api/Contact/getsubdistrict")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getSubDistrict(Guid? districtId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<SubDistrict> result = await Task.Run(() =>
                {
                    return _Service.GetSubDistrict(districtId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Contact/getdistrict")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getDistrict(Guid? provinceId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<District> result = await Task.Run(() =>
                {
                    return _Service.GetDistrict(provinceId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Contact/getprovince")]
        //[ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> getProvince(Guid? provinceId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Province> result = await Task.Run(() =>
                {
                    return _Service.GetProvince(provinceId, keyword, out _totalRecord, pageIndex, pageSize);
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