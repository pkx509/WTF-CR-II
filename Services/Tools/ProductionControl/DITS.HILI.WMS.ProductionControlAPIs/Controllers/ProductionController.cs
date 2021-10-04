using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ProductionControlService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.ProductionControlAPIs.Controllers
{
    [Authorize]
    public partial class ProductionController : BaseApiController
    {
        private readonly IProductionControlService _Service;
        private readonly IMessageService _MessageService;

        public ProductionController(IProductionControlService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/productioncontrol/getpackinglist")]
        public async Task<ApiResponseMessage> GetPackingList(LineTypeEnum lineType, DateTime? planDate = null, Guid? lineID = null, string keyword = "", int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<PC_PackingModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetAllPacking(lineType, planDate, lineID, keyword, out _totalRecord, pageIndex, pageSize);
                });

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
        [Route("api/productioncontrol/getpackedlist")]
        public async Task<ApiResponseMessage> GetPackedList(LineTypeEnum lineType, DateTime? planDate = null, Guid? lineID = null, string keyword = "", int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<PC_PackedModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetAllPacked(lineType, planDate, lineID, keyword, out _totalRecord, pageIndex, pageSize);
                });

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
        [Route("api/productioncontrol/GetRePrintOutboundList")]
        public async Task<ApiResponseMessage> GetRePrintOutboundList(DateTime? MFGDate, string productName, string PONo, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<PC_PackedModel> result = await Task.Run(() =>
                {
                    return _Service.GetRePrintOutboundList(MFGDate, productName, PONo, out _totalRecord, pageIndex, pageSize);
                });

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
        [Route("api/productioncontrol/getpalletinfo")]
        public async Task<ApiResponseMessage> GetPalletInfo(string palletCode, string oldPalletCode, Guid oldProductID, decimal orderQTY)
        {
            try
            {
                PalletInfoModel result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetPalletInfo(palletCode, oldPalletCode, oldProductID, orderQTY);
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

        [HttpGet]
        [Route("api/productioncontrol/getbyid")]
        public async Task<ApiResponseMessage> GetByID(Guid controlID)
        {
            try
            {
                PC_PackingModel result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetByID(controlID);
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

        [HttpGet]
        [Route("api/productioncontrol/getreprintlist")]
        public async Task<ApiResponseMessage> GetRePrintList(Guid controlID, bool isProduction)
        {
            try
            {
                System.Collections.Generic.List<PC_PackedModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetRePrintList(controlID, isProduction);
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
        [Route("api/productioncontrol/printpallettag")]
        public async Task<ApiResponseMessage> PrintPalletTag(PrintPalletModel model)
        {
            try
            {
                PC_PackingModel result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.PrintPalletTag(model);
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
        [Route("api/productioncontrol/CancelPallet")]
        public async Task<ApiResponseMessage> CancelPallet(CancelPalletModel model)
        {
            try
            {
                CancelPalletModel result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.CancelPallet(model);
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

        [HttpGet]
        [Route("api/productioncontrol/getpalletlist")]
        public async Task<ApiResponseMessage> GetPalletList(Guid receiveDetailId)
        {
            try
            {
                System.Collections.Generic.List<PC_PackedModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetPalletList(receiveDetailId);
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