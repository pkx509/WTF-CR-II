using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DailyPlanService;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.DailyPlanAPIs.Controllers
{
    [Authorize]
    public partial class DailyPlanController : BaseApiController
    {
        private readonly IDailyPlanService service;
        private readonly IMessageService _messageService;

        public DailyPlanController(IDailyPlanService _service,
                              IMessageService messageService)
        {
            service = _service;
            _messageService = messageService;
        }

        [HttpPost]
        [Route("api/dailyplan/importdailyplan")]
        //[ResponseType(typeof(bool))]
        public async Task<ApiResponseMessage> ImportDailyPlan(List<ProductionPlanCustomModel> entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                service.UserID = UserId;
                bool _result = await Task.Run(() =>
                {

                    return service.ImportDailyPlan(entity);
                });

                return Succeed(_result);

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
        [Route("api/dailyplan/validateimportdailyplan")]
        //[ResponseType(typeof(ValidationImportFileResult))]
        public async Task<ApiResponseMessage> ValidateImportDailyPlan(List<ProductionPlanCustomModel> entity)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                service.UserID = UserId;
                List<ValidationImportFileResult> _result = await Task.Run(() =>
                {
                    return service.ValidateImportDailyPlan(entity);
                });

                return Succeed(_result);

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
        [Route("api/dailyplan/getbyid")]
        // [ResponseType(typeof(ProductionPlanCustomModel))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ProductionPlanCustomModel result = await Task.Run(() =>
                {
                    return service.GetByID(id);
                });

                if (result == null)
                {
                    return NotSucceed(_messageService.GetMessage("MSG00006", Language));
                }

                return Succeed(result);

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
        [Route("api/dailyplan/get")]
        //[ResponseType(typeof(List<ProductionPlanCustomModel>))]
        public async Task<ApiResponseMessage> get(DateTime sdte, DateTime edte, Guid? lineId, LineTypeEnum lineType, bool isReceive, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<ProductionPlanCustomModel> result = await Task.Run(() =>
                {
                    return service.GetAll(sdte, edte, lineId, lineType, isReceive, keyword, out _totalRecord, pageIndex, pageSize);
                });

                ApiResponseMessage apiResp = Succeed(result, _totalRecord);

                return apiResp;

                //var responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                //responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                //IHttpActionResult response = ResponseMessage(responseMsg);
                //return response;
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
        [Route("api/dailyplan/sendtoreceive")]
        //[ResponseType(typeof(bool))]
        public async Task<ApiResponseMessage> SendtoReceive(List<ProductionPlanCustomModel> data)
        {
            try
            {
                bool _result = await Task.Run(() =>
                {
                    service.UserID = UserId;
                    return service.SendToReceive(data);
                });
                return Succeed(_result);

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
        [Route("api/dailyplan/saveplan")]
        //[ResponseType(typeof(bool))]
        public async Task<ApiResponseMessage> saveplan(ProductionPlanCustomModel data)
        {
            try
            {
                bool _result = await Task.Run(() =>
                {
                    service.UserID = UserId;
                    return service.SaveData(data);
                });
                return Succeed(_result);

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
        [Route("api/dailyplan/deleteplan")]
        //[ResponseType(typeof(bool))]
        public async Task<ApiResponseMessage> DeletePlan(List<Guid> planDetailIds)
        {
            try
            {
                bool _result = await Task.Run(() =>
                {
                    return service.DeletePlan(planDetailIds);
                });
                return Succeed(_result);

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
        [Route("api/dailyplan/getlocationbyline")]
        public async Task<ApiResponseMessage> GetLocationByLine(Guid lineID, LocationTypeEnum? locationType, string keyword, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;
                List<Location> result = await Task.Run(() =>
                {
                    return service.GetLocationByLine(lineID, locationType, keyword, out _totalRecord, pageIndex, pageSize);
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
    }
}
