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
    public class WarehouseController : BaseApiController
    {
        private readonly IWarehouseService _Service;
        private readonly IMessageService _messageService;

        public WarehouseController(IWarehouseService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/Warehouse/getbyid")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Warehouse result = await Task.Run(() =>
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
        [Route("api/Warehouse/get")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> get(Guid? warehouseTypeId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Warehouse> result = await Task.Run(() =>
                {
                    return _Service.Get(warehouseTypeId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Warehouse/getAll")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> getAll(Guid? warehouseTypeId, string keyword, bool IsActive = false, int? pageIndex = 0, int? pageSize = int.MaxValue)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<WarehouseModel> result = await Task.Run(() =>
                {
                    return _Service.GetAll(warehouseTypeId, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Warehouse/getMk")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> getMk(Guid? warehouseTypeId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Warehouse> result = await Task.Run(() =>
                {
                    return _Service.GetMk(warehouseTypeId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Warehouse/add")]
        //[ResponseType(typeof(Warehouse))]
        public async Task<ApiResponseMessage> add(Warehouse entity)
        {
            try
            {
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
        [Route("api/Warehouse/modify")]
        public async Task<ApiResponseMessage> modify(Warehouse entity)
        {
            try
            {
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
        [Route("api/Warehouse/remove")]
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
        [Route("api/Warehouse/GetWarehouseType")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetWarehouseType(Guid? warehouseTypeId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<WarehouseType> result = await Task.Run(() =>
                {
                    return _Service.GetWarehouseType(warehouseTypeId, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Warehouse/getWarehouseTypeAll")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> getWarehouseTypeAll(Guid? warehouseTypeId, string keyword, bool IsActive, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<WarehouseModel> result = await Task.Run(() =>
                {
                    return _Service.GetWarehouseTypeAll(warehouseTypeId, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Warehouse/GetWarehouseTypByID")]
        //[ResponseType(typeof(List<Warehouse>))]
        public async Task<ApiResponseMessage> GetWarehouseTypByID(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                WarehouseType result = await Task.Run(() =>
                {
                    return _Service.GetWarehouseTypByID(id);
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
        [Route("api/Warehouse/addWarehouseType")]
        //[ResponseType(typeof(WarehouseType))]
        public async Task<ApiResponseMessage> AddWarehouseType(WarehouseType entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddWarehouseType(entity);
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
        [Route("api/Warehouse/modifyWarehouseType")]
        public async Task<ApiResponseMessage> WarehouseTypemodify(WarehouseType entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _Service.WarehouseTypeModify(entity);
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
        [Route("api/Warehouse/removeWarehouseType")]
        public async Task<ApiResponseMessage> WarehouseTyperemove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.WarehouseTypeRemove(id);
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