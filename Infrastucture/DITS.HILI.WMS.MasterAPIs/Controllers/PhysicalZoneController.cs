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
    public class PhysicalZoneController : BaseApiController
    {
        private readonly IPhysicalZoneService _Service;
        private readonly IMessageService _messageService;

        public PhysicalZoneController(IPhysicalZoneService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/PhysicalZone/getbyid")]
        // [ResponseType(typeof(Units))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                PhysicalZone result = await Task.Run(() =>
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
        [Route("api/PhysicalZone/get")]
        //[ResponseType(typeof(List<Units>))]
        public async Task<ApiResponseMessage> Get(string keyword, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<PhysicalZone> result = await Task.Run(() =>
                {
                    return _Service.Get(keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/PhysicalZone/getphysicalcombo")]
        //[ResponseType(typeof(List<Units>))]
        public async Task<ApiResponseMessage> GetPhysicalCombo(string keyword, int pageIndex, int pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<PhysicalZone> result = await Task.Run(() =>
                {
                    return _Service.GetPhysicalCombo(keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/PhysicalZone/add")]
        // [ResponseType(typeof(Units))]
        public async Task<ApiResponseMessage> add(PhysicalZone entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddPhysicalZone(entity);
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
        [Route("api/PhysicalZone/modify")]
        public async Task<ApiResponseMessage> modify(PhysicalZone entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _Service.ModifyPhysicalZone(entity);
                });

                return Succeed();
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/PhysicalZone/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemovePhysicalZone(id);
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