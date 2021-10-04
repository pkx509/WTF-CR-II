using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.MasterService.Warehouses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class LocationController : BaseApiController
    {
        private readonly ILocationService _Service;
        private readonly IMessageService _messageService;

        public LocationController(ILocationService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/Location/getbyid")]
        //[ResponseType(typeof(Location))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                LocationModel result = await Task.Run(() =>
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
        [Route("api/Location/getbyzone")] 
        public async Task<ApiResponseMessage> get(Guid zoneId, LocationTypeEnum? locationType, string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Location> result = await Task.Run(() =>
                {
                    return _Service.Get(zoneId, locationType, keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
        [Route("api/Location/get")]
        //[ResponseType(typeof(List<Location>))]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Location> result = await Task.Run(() =>
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
        [Route("api/Location/getbywarehouse")]
        //[ResponseType(typeof(List<Units>))]
        public async Task<ApiResponseMessage> Get(Guid zoneId, Guid warehouseId, string keyword, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<LocationModel> result = await Task.Run(() =>
                {
                    return _Service.Get(zoneId, warehouseId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Location/getAll")]
        //[ResponseType(typeof(List<Location>))]
        public async Task<ApiResponseMessage> getAll(string keyword, bool IsActive = false, int? pageIndex = 0, int? pageSize = int.MaxValue)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<LocationModel> result = await Task.Run(() =>
                {
                    return _Service.GetAll(keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/location/getloadingin")]
        //[ResponseType(typeof(List<Location>))]
        public async Task<ApiResponseMessage> getloadingin()
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Location> result = await Task.Run(() =>
                {
                    return _Service.GetLoading();
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
        [Route("api/location/getloadingout")]
        //[ResponseType(typeof(List<Location>))]
        public async Task<ApiResponseMessage> getloadingout()
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Location> result = await Task.Run(() =>
                {
                    return _Service.GetLoadingOut();
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
        [Route("api/Location/getlocationbetweenlist")]
        //[ResponseType(typeof(List<Location>))]
        public async Task<ApiResponseMessage> getLocationbetweenlist(string location_from, string location_to, Guid zoneid)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<LogicalZoneDetailModel> result = await Task.Run(() =>
                {
                    return _Service.GetLocationBetweenList(location_from, location_to, zoneid);
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
        [Route("api/Location/CheckLocation")]
        public async Task<ApiResponseMessage> CheckLocation(List<LocationModel> modellist)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ApiResponseMessage result = await Task.Run(() =>
                {
                    return _Service.CheckLocation(modellist);
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
        [Route("api/Location/addLocation")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> add(List<Location> entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.AddLocation(entity);
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
        [Route("api/Location/modifyLocation")]
        public async Task<ApiResponseMessage> modify(Location entity)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.ModifyLocation(entity);
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
        [Route("api/Location/removeLocation")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                _Service.UserID = UserId;
                await Task.Run(() =>
                {
                    _Service.RemoveLocation(id);
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