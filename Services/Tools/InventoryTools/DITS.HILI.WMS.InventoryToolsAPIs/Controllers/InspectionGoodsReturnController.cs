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
    public class InspectionGoodsReturnController : BaseApiController
    {
        private readonly IInspectionGoodsReturnService _Service;
        private readonly IMessageService _MessageService;
        public InspectionGoodsReturnController(IInspectionGoodsReturnService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/InspectionGoodsReturn/GetInspectionGoodsReturn")]
        public async Task<ApiResponseMessage> GetInspectionGoodsReturn(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<GoodsReturn> result = await Task.Run(() =>
                {
                    return _Service.GetInspectionGoodsReturn(sdte, edte, status, search, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/InspectionGoodsReturn/GetGetInspectionGoodsReturnByID")]
        public async Task<ApiResponseMessage> GetInspectionReclassifiedByID(Guid reclassifiedID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                GoodsReturn result = await Task.Run(() =>
                {
                    return _Service.GetInspectionGoodsReturnByID(reclassifiedID);
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
        [Route("api/InspectionGoodsReturn/SaveInspectionGoodsReturn")]
        public async Task<ApiResponseMessage> AddInspectionReclassified(List<ItemGoodsReturn> _return)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.SaveInspectionGoodsReturn(_return, false);
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
        [Route("api/InspectionGoodsReturn/ApproveInspectionGoodsReturn")]
        public async Task<ApiResponseMessage> ApproveInspectionGoodsReturn(List<ItemGoodsReturn> _return)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.SaveInspectionGoodsReturn(_return, true);
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
        [Route("api/InspectionGoodsReturn/SendtoReprocess")]
        public async Task<ApiResponseMessage> SendtoReprocess(List<ItemGoodsReturn> changes)
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
        [Route("api/InspectionGoodsReturn/SendtoDamage")]
        public async Task<ApiResponseMessage> SendtoDamage(List<ItemGoodsReturn> changes)
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