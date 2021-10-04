using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.MasterService.Warehouses;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ZoneController : BaseApiController
    {
        private readonly IZoneService _Service;
        private readonly IMessageService _messageService;

        public ZoneController(IZoneService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/Zone/getbyid")]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Zone result = await Task.Run(() =>
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
        [Route("api/Zone/getbywarehouse")]
        public async Task<ApiResponseMessage> get(Guid warehouseId, Guid? zoneTypeId, string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Zone> result = await Task.Run(() =>
                {
                    return _Service.Get(warehouseId, zoneTypeId, keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
        [Route("api/Zone/get")]
        public async Task<ApiResponseMessage> get(Guid? id, string keyword, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                //var _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Zone> result = await Task.Run(() =>
                {
                    return _Service.GetAll(id, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Zone/getlist")]
        public async Task<ApiResponseMessage> getlist(Guid? id, string keyword, bool IsActive, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                //var _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ZoneModel> result = await Task.Run(() =>
                {
                    return _Service.Getlist(id, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Zone/getzonecombo")]
        public async Task<ApiResponseMessage> getzonecombo(Guid? id, string keyword, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                //var _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ZoneModel> result = await Task.Run(() =>
                {
                    return _Service.GetZoneCombo(id, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Zone/add")]
        public async Task<ApiResponseMessage> add(Zone entity)
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
        [Route("api/Zone/modify")]
        public async Task<ApiResponseMessage> modify(Zone entity)
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
        [Route("api/Zone/remove")]
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
        [Route("api/Zone/getZoneType")]
        public async Task<ApiResponseMessage> getZoneType(Guid? id, string keyword, bool IsActive, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                //var _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ZoneType> result = await Task.Run(() =>
                {
                    return _Service.GetZoneType(id, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Zone/addZoneType")]
        public async Task<ApiResponseMessage> addZoneType(ZoneType entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddZoneType(entity);
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
        [Route("api/Zone/modifyZoneType")]
        public async Task<ApiResponseMessage> modifyZoneType(ZoneType entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _Service.ModifyZoneType(entity);
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
        [Route("api/Zone/removeZoneType")]
        public async Task<ApiResponseMessage> removeZoneType(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveZoneType(id);
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