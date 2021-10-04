using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.InventoryToolsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    public class InspectionDamageController : BaseApiController
    {
        private readonly IInspectionDamageService _Service;
        private readonly IMessageService _MessageService;
        public InspectionDamageController(IInspectionDamageService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/InspectionDamage/GetInspectionDamage")]
        public async Task<ApiResponseMessage> GetInspectionDamage(DateTime sdte, DateTime edte, Guid lineId, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<Changestatus> result = await Task.Run(() =>
                {
                    return _Service.GetInspectionDamage(sdte, edte, lineId, status, search == null ? "" : search, out _totalRecord, pageIndex, pageSize);
                });

                ApiResponseMessage apiResp = Succeed(result, _totalRecord);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/InspectionDamage/SaveInspectionDamage")]
        public async Task<ApiResponseMessage> SaveInspectionDamage(Guid damageID, decimal rejectQty, decimal reprocessQty)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                Changestatus result = await Task.Run(() =>
                {
                    return _Service.SaveInspectionDamage(damageID, rejectQty, reprocessQty);
                });

                ApiResponseMessage apiResp = Succeed(result);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/InspectionDamage/ApproveInspectionDamage")]
        public async Task<ApiResponseMessage> ApproveInspectionDamage(Guid damageID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.ApproveInspectionDamage(damageID);
                });

                ApiResponseMessage apiResp = Succeed(result);

                return apiResp;

            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }



        [HttpPost]
        [Route("api/InspectionDamage/SendtoReprocess")]
        public async Task<ApiResponseMessage> SendtoReprocess(List<Changestatus> changes)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.SendtoReprocess(changes);
                });

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/InspectionDamage/SendtoDamage")]
        public async Task<ApiResponseMessage> SendtoDamage(List<Changestatus> changes)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.SendtoDamage(changes);
                });

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", Language));
            }
        }

    }
}