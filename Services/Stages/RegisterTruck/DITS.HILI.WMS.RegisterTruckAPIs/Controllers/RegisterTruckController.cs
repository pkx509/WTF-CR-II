using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using DITS.HILI.WMS.RegisterTruckService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.RegisterTruckAPIs.Controllers
{

    //[Authorize]
    public class RegisterTruckController : BaseApiController
    {
        // GET: RegisterTruck
        private readonly IRegisterTruckService _service;
        private readonly IMessageService _messageService;
        public RegisterTruckController(IRegisterTruckService service, IMessageService messageService)
        {
            _service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/RegisterTruck/getbyid")]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                RegisterTruck result = await Task.Run(() =>
                {
                    return _service.Get(id);
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
        [Route("api/RegisterTruck/getbyDetailid")]
        public async Task<ApiResponseMessage> getbyDetailid(Guid? id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                RegisTruckModel result = await Task.Run(() =>
                {
                    return _service.GetbyDetailId(id);
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
        [Route("api/RegisterTruck/get")]
        public async Task<ApiResponseMessage> get(Guid? shippingID, string Po, string Doc, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<DispatchAllModel> result = await Task.Run(() =>
                {
                    return _service.GetAll(shippingID, Po, Doc, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/RegisterTruck/getdispatchForRegisTrucklistAll")]
        public async Task<ApiResponseMessage> GetDispatchForRegisTrucklistAll(Guid? warehouseID, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<DispatchAllModel> result = await Task.Run(() =>
                {
                    return _service.GetDispatchForRegisTrucklistAll(warehouseID, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/RegisterTruck/getdispatchForRegisTruckById")]
        public async Task<ApiResponseMessage> GetDispatchForRegisTruckById(Guid? warehouseID, string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DispatchAllModel result = await Task.Run(() =>
                {
                    return _service.GetDispatchForRegisTruckById(warehouseID, keyword);
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
        [Route("api/RegisterTruck/add")]
        public async Task<ApiResponseMessage> add(RegisTruckModel entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.AddRegisTruck(entity);
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
        [Route("api/RegisterTruck/modify")]
        public async Task<ApiResponseMessage> modify(RegisTruckModel entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.ModifyRegisTruck(entity);
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
        [Route("api/RegisterTruck/AssignModify")]
        public async Task<ApiResponseMessage> AssignModify(RegisTruckModel entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.AssignModify(entity);
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
        [Route("api/RegisterTruck/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.RemoveRegisTruck(id);
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

        #region Consolidate

        [HttpGet]
        [Route("api/RegisterTruck/getconsolidatebypo")]
        public async Task<ApiResponseMessage> GetConsolidateByPo(string pono, string documentNo)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                RegisterTruckConsolidateHeaderModel result = await Task.Run(() =>
                {
                    return _service.GetConsolidateByPO(pono, documentNo);
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
        [Route("api/RegisterTruck/getconsolidateall")]
        public async Task<ApiResponseMessage> GetConsolidateAll(string pono, string documentno, int? status, DateTime? datafrom, DateTime? datato, string licenseplate, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<RegisterTruckConsolidateListModel> result = await Task.Run(() =>
                {
                    return _service.GetConsolidateAll(pono, documentno, status, datafrom, datato, licenseplate, out _totalRecord, pageIndex, pageSize);
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
        [HttpPut]
        [Route("api/RegisterTruck/modifyconsolidate")]
        public async Task<ApiResponseMessage> ModifyConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.ModifyConsolidate(entity);
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
        [Route("api/RegisterTruck/approveconsolidate")]
        public async Task<ApiResponseMessage> ApproveConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            try
            {
                _service.UserID = UserId;
                await Task.Run(() =>
                {
                    _service.ApproveConsolidate(entity);
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

        [HttpGet]
        [Route("api/RegisterTruck/GetConsolidateData")]
        public async Task<ApiResponseMessage> GetConsolidateData(string DocNo)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<RegisterTruckConsolidateDeatilModel> result = await Task.Run(() =>
                {
                    return _service.GetConsolidateData(DocNo);
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
        [Route("api/RegisterTruck/GetCheckDocData")]
        public async Task<ApiResponseMessage> GetCheckDocData(string ShippingID, string DockNo)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<RegisterTruckConsolidateDeatilModel> result = await Task.Run(() =>
                {
                    return _service.GetCheckDocData(Guid.Parse(ShippingID), DockNo);
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
        [Route("api/RegisterTruck/GetDetail")]
        public async Task<ApiResponseMessage> GetDetail(string DocumentNo, string lotNumber)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                RegisterTruckConsolidateDeatilModel result = await Task.Run(() =>
                {
                    return _service.GetDetail(DocumentNo, lotNumber);
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
        [Route("api/RegisterTruck/Confirm")]
        public async Task<ApiResponseMessage> Confirm(string DocumentNo, Guid ShippingID, string lotNumber, decimal ConfirmQty)
        {
            try
            {
                _service.UserID = UserId;
                bool result = await Task.Run(() =>
                {
                    return _service.ConfirmConsolidate(DocumentNo, ShippingID, lotNumber, ConfirmQty);
                });


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
        [Route("api/RegisterTruck/JobConsoComplete")]
        public async Task<ApiResponseMessage> JobConsoComplete(Guid ShippingID, Guid ShippingDetailID, string PalletCode)
        {
            try
            {
                int _totalRecord = 0;
                _service.UserID = UserId;
                RegisterTruckConsolidateDeatilModel result = await Task.Run(() =>
                {
                    return _service.JobConsoComplete(ShippingID, ShippingDetailID, PalletCode);
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
