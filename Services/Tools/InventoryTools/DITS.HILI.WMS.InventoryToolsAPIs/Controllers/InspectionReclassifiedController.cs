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
    public class InspectionReclassifiedController : BaseApiController
    {
        private readonly IInspectionReclassifiedService _Service;
        private readonly IMessageService _MessageService;
        public InspectionReclassifiedController(IInspectionReclassifiedService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/InspectionReclassified/GetInspectionReclassified")]
        public async Task<ApiResponseMessage> GetInspectionReclassified(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<Reclassified> result = await Task.Run(() =>
                {
                    return _Service.GetInspectionReclassified(sdte, edte, status, search, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/InspectionReclassified/GetInspectionReclassifiedByID")]
        public async Task<ApiResponseMessage> GetInspectionReclassifiedByID(Guid reclassifiedID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                Reclassified result = await Task.Run(() =>
                {
                    return _Service.GetInspectionReclassifiedByID(reclassifiedID);
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
        [Route("api/InspectionReclassified/AddInspectionReclassified")]
        public async Task<ApiResponseMessage> AddInspectionReclassified(List<ItemReclassified> _reclassList)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.AddInspectionReclassified(_reclassList);
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
        [Route("api/InspectionReclassified/SaveInspectionReclassified")]
        public async Task<ApiResponseMessage> SaveInspectionReclassified(Reclassified entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.SaveInspectionReclassified(entity);
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
        [Route("api/InspectionReclassified/ApproveInspectionReclassified")]
        public async Task<ApiResponseMessage> ApproveInspectionReclassified(Reclassified entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.ApproveInspectionReclassified(entity);
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
        [Route("api/InspectionReclassified/GetPalletTag")]
        public async Task<ApiResponseMessage> GetPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, Guid producttsatusId, int pageIndex = 0, int pageSize = 0, string WHReferenceCode = "111")
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<ReceiveModel.PalletTagModel> result = await Task.Run(() =>
                {
                    return _Service.GetPalletTag(palletNo, productCode, productName, Lot, Line, mfgDate, producttsatusId, out _totalRecord, pageIndex, pageSize, WHReferenceCode);
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
        [Route("api/InspectionReclassified/GetInspectionDispatch")]
        public async Task<ApiResponseMessage> GetInspectionDispatch(DateTime sdte, DateTime edte, string status, string search, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                List<Reclassified> result = await Task.Run(() =>
                {
                    return _Service.GetInspectionDispatch(sdte, edte, status, search, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/InspectionReclassified/GetInspectionDispatchByID")]
        public async Task<ApiResponseMessage> GetInspectionDispatchByID(Guid reclassifiedID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                Reclassified result = await Task.Run(() =>
                {
                    return _Service.GetInspectionDispatchByID(reclassifiedID);
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
        [Route("api/InspectionReclassified/ApproveInspectionDispatch")]
        public async Task<ApiResponseMessage> ApproveInspectionDispatch(Reclassified entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                bool result = await Task.Run(() =>
                {
                    return _Service.ApproveInspectionDispatch(entity);
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
        [Route("api/InspectionReclassified/SendtoReprocess")]
        public async Task<ApiResponseMessage> SendtoReprocess(List<ItemReclassified> changes)
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
        [Route("api/InspectionReclassified/SendtoDamage")]
        public async Task<ApiResponseMessage> SendtoDamage(List<ItemReclassified> changes)
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