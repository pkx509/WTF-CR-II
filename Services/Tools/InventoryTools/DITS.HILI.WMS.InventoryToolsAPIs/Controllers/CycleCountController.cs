using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.InventoryToolsService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    public class CycleCountController : BaseApiController
    {
        private readonly ICycleCountService _Service;
        private readonly IMessageService _messageService;
        private readonly IMessageService _MessageService;
        public CycleCountController(ICycleCountService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/CycleCount/GetCycleCountlist")]
        public ApiResponseMessage GetCycleCountlist(DateTime? sdte, DateTime? edte, int State, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                System.Collections.Generic.List<CycleCountModel> result = _Service.GetlistAll(sdte, edte, State, keyword, out int _totalRecord, pageIndex, pageSize);

                return Succeed(result, _totalRecord);
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
        [Route("api/CycleCount/GetCycleCountDetail")]
        public ApiResponseMessage GetCycleCountDetail(string keyword)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                CycleCountModel result = _Service.GetAll(keyword);

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

        [HttpGet]
        [Route("api/CycleCount/getCycleCountstatus")]
        public ApiResponseMessage getCycleCountstatus()
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                System.Collections.Generic.List<MasterModel.CustomEnumerable> result = _Service.GetCycleCountStatus();

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

        #region Stock

        [HttpGet]
        [Route("api/CycleCount/getcycyclecountstock")]
        public async Task<ApiResponseMessage> getcycyclecountstock(Guid? WarehouseID, Guid? ZoneID, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<CycleCountModel> result = await Task.Run(() =>
                {
                    return _Service.GetCycleCountStock(WarehouseID, ZoneID, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/CycleCount/add")]
        public async Task<ApiResponseMessage> add(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddCycleCount(entity);
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
        [Route("api/CycleCount/ModifyCycleCount")]
        public async Task<ApiResponseMessage> ModifyCycleCount(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyCycleCount(entity);
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
        [Route("api/CycleCount/Approve")]
        public async Task<ApiResponseMessage> Approve(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.Approve(entity);
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
        [Route("api/CycleCount/remove")]
        public async Task<ApiResponseMessage> remove(string id)
        {
            try
            {
                _Service.UserID = UserId;
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
        #endregion

        #region [CycleCount HandHeld]
        [HttpGet]
        [Route("api/CycleCount/GetCycleCountData")]
        public ApiResponseMessage GetCycleCountData(Guid? warehouseID)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                System.Collections.Generic.List<CycleCountModel> result = _Service.GetCycleCountData(warehouseID);

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
        [HttpGet]
        [Route("api/CycleCount/GetCycleCountDataDetail")]
        public ApiResponseMessage GetCycleCountDataDetail(string CycleCountCode, Guid? warehouseID, string barcode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                CycleCountDetails result = _Service.GetCycleCountDataDetail(CycleCountCode, warehouseID, barcode);

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
        [HttpGet]
        [Route("api/CycleCount/GetJobComplete")]
        public ApiResponseMessage GetJobComplete(string CycleCountCode)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                CycleCountModel result = _Service.GetJobComplete(CycleCountCode);

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
        [HttpGet]
        [Route("api/CycleCount/ConfirmCount")]
        public async Task<ApiResponseMessage> ConfirmCount(Guid CyclecountDetailID, string CycleCountCode, decimal CountingQty, decimal DiffQty, string LotNumber)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;


                bool result = await Task.Run(() =>
                {
                    return _Service.ConfirmCounting(CyclecountDetailID, CycleCountCode, CountingQty, DiffQty, LotNumber);
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
        #endregion []
    }
}