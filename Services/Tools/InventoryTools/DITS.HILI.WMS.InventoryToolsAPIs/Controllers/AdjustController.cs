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
    public class AdjustController : BaseApiController
    {
        private readonly IAdjustService _Service;
        private readonly IMessageService _messageService;
        private readonly IMessageService _MessageService;
        public AdjustController(IAdjustService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/Adjust/GetAdjustlist")]
        public ApiResponseMessage GetAdjustlist(string State, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                System.Collections.Generic.List<AdjustModel> result = _Service.GetlistAll(State, keyword, out int _totalRecord, pageIndex, pageSize);

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
        [Route("api/Adjust/GetAdjustDetail")]
        public ApiResponseMessage GetCycleCountDetail(string keyword)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                AdjustModel result = _Service.GetAll(keyword);

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
        [Route("api/Adjust/GetAdjustStockOther")]
        public async Task<ApiResponseMessage> GetAdjustStockOther(string state, Guid? WarehouseID, string product, string pallet, string Lot, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<AdjustModel> result = await Task.Run(() =>
                {
                    return _Service.GetAdjustStockOther(state, WarehouseID, product, pallet, Lot, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Adjust/GetAdjustStockCycleCount")]
        public async Task<ApiResponseMessage> GetAdjustStockCycleCount(string state, Guid? WarehouseID, string product, string pallet, string Lot, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<AdjustModel> result = await Task.Run(() =>
                {
                    return _Service.GetAdjustStockCycleCount(state, WarehouseID, product, pallet, Lot, out _totalRecord, pageIndex, pageSize);
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

        //#region Stock


        [HttpPost]
        [Route("api/Adjust/add")]
        public async Task<ApiResponseMessage> add(AdjustModel entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddAdjust(entity);
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
        [Route("api/Adjust/ModifyAdjust")]
        public async Task<ApiResponseMessage> ModifyCycleCount(AdjustModel entity)
        {
            try
            {
                _Service.UserID = UserId;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyAdjust(entity);
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
        [Route("api/Adjust/Approve")]
        public async Task<ApiResponseMessage> Approve(AdjustModel entity)
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
        [Route("api/Adjust/remove")]
        public async Task<ApiResponseMessage> remove(string id)
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
        //#endregion

        //#region [CycleCount HandHeld]
        //[HttpGet]
        //[Route("api/CycleCount/GetCycleCountData")]
        //public ApiResponseMessage GetCycleCountData(Guid? warehouseID)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        _Service.UserID = _header.UserID.Value;

        //        var result = _Service.GetCycleCountData(warehouseID);

        //        return Succeed(result);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
        //    }
        //}
        //[HttpGet]
        //[Route("api/CycleCount/GetCycleCountDataDetail")]
        //public ApiResponseMessage GetCycleCountDataDetail(string CycleCountCode, Guid? warehouseID, string barcode)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        _Service.UserID = _header.UserID.Value;

        //        var result = _Service.GetCycleCountDataDetail(CycleCountCode,warehouseID, barcode);

        //        return Succeed(result);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
        //    }
        //}
        //[HttpGet]
        //[Route("api/CycleCount/ConfirmCount")]
        //public async Task<ApiResponseMessage> ConfirmCount(Guid CyclecountDetailID, string CycleCountCode, decimal CountingQty, decimal DiffQty, string LotNumber)
        //{
        //    try
        //    {
        //        var _header = ApiHelpers.Response(Request);
        //        _Service.UserID = _header.UserID.Value;


        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.ConfirmCounting(CyclecountDetailID, CycleCountCode, CountingQty, DiffQty, LotNumber);
        //        });


        //        return Succeed(result);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
        //    }
        //}
        //#endregion []
    }
}