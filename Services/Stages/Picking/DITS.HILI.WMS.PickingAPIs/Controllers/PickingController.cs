using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.PickingService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.PickingAPIs.Controllers
{
    [Authorize]
    public partial class PickingController : BaseApiController
    {
        private readonly IPickingService _Service;
        private readonly IMessageService _MessageService;

        public PickingController(IPickingService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpPost]
        [Route("api/picking/approve")]
        public async Task<ApiResponseMessage> Approve(AssignJobModel model)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.Approve(model);
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
        [Route("api/picking/save")]
        public async Task<ApiResponseMessage> Save(AssignJobModel model)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.Save(model);
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
        [Route("api/picking/confirmpickhh")]
        public async Task<ApiResponseMessage> ConfirmPickHH(Guid pickingID, Guid productID, string palletCode, string refPalletCode, decimal confirmQTY, decimal consolidateQTY, decimal orderQTY, string orderUnit, string reason)
        {
            try
            {
                string result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.ConfirmPickHH(pickingID, productID, palletCode, refPalletCode, confirmQTY, consolidateQTY, orderQTY, orderUnit, reason);
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
        [Route("api/picking/getallassignjob")]
        public async Task<ApiResponseMessage> GetAllAssignJob(PickingStatusEnum pickingStatus, DateTime? startDate = null, DateTime? endDate = null, string docNo = null, string PONo = null, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<AssignJobModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetAllAssignJob(pickingStatus, startDate, endDate, docNo, PONo, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/picking/getassignjob")]
        public async Task<ApiResponseMessage> GetAssignJob(Guid pickingID)
        {
            try
            {
                AssignJobModel result = await Task.Run(() =>
                {
                    return _Service.GetAssignJob(pickingID);
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
        [Route("api/picking/getassignjobbypo")]
        public async Task<ApiResponseMessage> GetAssignJobByPO(string PONo)
        {
            try
            {
                AssignJobModel result = await Task.Run(() =>
                {
                    return _Service.GetAssignJobByPO(PONo);
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
        [Route("api/picking/getuserwhgroup")]
        public async Task<ApiResponseMessage> GetUserWHGroup(string keyword = null, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<MasterModel.Secure.UserGroups> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetUserWHGroup(keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/picking/getdispatchforassignjob")]
        public async Task<ApiResponseMessage> GetDispatchforAssignJob(string PONo = null, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                System.Collections.Generic.List<DispatchforAssignJobModel> result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GetDispatchforAssignJob(PONo, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/picking/getpickinglisthh")]
        public async Task<ApiResponseMessage> GetPickingListHH(string keyword)
        {
            try
            {
                System.Collections.Generic.List<PickingListHHModel> result = await Task.Run(() =>
                {
                    return _Service.GetPickingListHH(keyword);
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
        [Route("api/picking/getpickinghh")]
        public async Task<ApiResponseMessage> GetPickingHH(string pickingID, string productID)
        {
            try
            {
                PickingListHHModel result = await Task.Run(() =>
                {
                    return _Service.GetPickingHH(pickingID, productID);
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
        [Route("api/picking/checkpallet")]
        public async Task<ApiResponseMessage> CheckPallet(Guid pickingID, string palletCode, bool isReprocess)
        {
            try
            {
                PickingListHHModel result = await Task.Run(() =>
                {
                    return _Service.CheckPallet(pickingID, palletCode, isReprocess);
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

        [HttpDelete]
        [Route("api/picking/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.RemovePickingAssign(id);
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
        [Route("api/picking/getPrintPalletTag")]
        public async Task<ApiResponseMessage> GetPrintPalletTag(Guid pickingID, string barcode)
        {
            try
            {
                PrintPalletTagModel result = await Task.Run(() =>
                {
                    return _Service.GetPrintPallet(pickingID,barcode);
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