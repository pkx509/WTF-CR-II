using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.ReceiveService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DITS.HILI.WMS.ReceiveAPIs.Controllers
{
    /// <summary>
    /// Receive API Service
    /// </summary>
    [Authorize]
    public partial class ReceiveController : BaseApiController
    {
        private readonly IReceiveService _Service;
        private readonly IMessageService _MessageService;

        public ReceiveController(IReceiveService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        #region [ Receive ]
        /// <summary>
        /// Save Receive Order
        /// </summary>
        /// <param name="entity">Receive Model</param>
        /// <returns>New receive data model</returns>
        [HttpPost]
        [Route("api/receive/add")]
        [ResponseType(typeof(Receive))]
        public async Task<IHttpActionResult> add(Receive entity)
        {
            try
            {
                Receive _result = await Task.Run(() =>
                {
                    return _Service.Add(entity);
                });
                return Ok(_result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/receive/cancel")]
        public async Task<ApiResponseMessage> Cancel(Guid id)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.Cancel(id);
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

        /// <summary>
        /// Filter Receive Order By ReceiveID
        /// </summary>
        /// <param name="id">ReceiveID (Guid)</param>
        /// <returns>Receive order data model</returns>
        [HttpGet]
        [Route("api/receive/getbyid")]
        [ResponseType(typeof(Receive))]
        public async Task<IHttpActionResult> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Receive result = await Task.Run(() =>
                {
                    return _Service.GetByID(id);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/receive/getbyreceivecode")]
        [ResponseType(typeof(Receive))]
        public async Task<IHttpActionResult> getbyreceivecode(string code)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Receive result = await Task.Run(() =>
                {
                    return _Service.GetByReceiveCode(code);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Search Receive Order
        /// </summary>
        /// <param name="receivestatusenum">Receive Status Enumerable </param>
        /// <param name="keyword">Receive Code/PO/Invoice/ Supp. Name/Owner Name</param>
        /// <param name="sdte">Start date search (Optional, Not require)</param>
        /// <param name="edte">End date search(Optional, Not require)</param>
        /// <returns>Receive Model Object</returns>
        [HttpGet]
        [Route("api/receive/get")]
        [ResponseType(typeof(List<Receive>))]
        public async Task<IHttpActionResult> get(ReceiveStatusEnum? receivestatusenum = null, string keyword = "", DateTime? sdte = null, DateTime? edte = null)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Receive> result = await Task.Run(() =>
                {
                    return _Service.GetAll(_header.ProductOwnerID, receivestatusenum, keyword, sdte, edte, out _totalRecord, _header.PageIndex, _header.PageSize);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/receive/getlist")]
        public async Task<ApiResponseMessage> getList(ReceiveStatusEnum status, Guid lineID, DateTime? estDate = null, string keyword = "", int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<ReceiveListModel> result = await Task.Run(() =>
                {
                    return _Service.GetAll(estDate, lineID, status, keyword, out _totalRecord, pageIndex, pageSize);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="estDate"></param>
        /// <param name="status"></param>
        /// <param name="receiveTypeID"></param>
        /// <param name="receiveCode"></param>
        /// <param name="orderNo"></param>
        /// <param name="PONo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/receive/getallinternalreceive")]
        public async Task<ApiResponseMessage> GetAllInternalReceive(DateTime? estDate, ReceiveStatusEnum status, Guid receiveTypeID, string receiveCode, string orderNo, string PONo, int pageIndex = 0, int pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<ReceiveHeaderModel> result = await Task.Run(() =>
                {
                    return _Service.GetAllInternalReceive(estDate, status, receiveTypeID, receiveCode, orderNo, PONo, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/receive/getreceivebyid")]
        public async Task<ApiResponseMessage> getReceiveByID(Guid id)
        {
            try
            {
                ReceiveHeaderModel result = await Task.Run(() =>
                {
                    return _Service.GetReceiveByID(id);
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
        [Route("api/receive/save")]
        public async Task<ApiResponseMessage> save(ReceiveHeaderModel entity)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.Save(entity);
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
        [Route("api/receive/SaveInternalReceive")]
        public async Task<ApiResponseMessage> SaveInternalReceive(ReceiveHeaderModel entity)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.SaveInternalReceive(entity);
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
        [Route("api/receive/ConfirmInternalReceive")]
        public async Task<ApiResponseMessage> ConfirmInternalReceive(ReceiveHeaderModel entity)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.ConfirmInternalReceive(entity);
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
        [Route("api/receive/GenerateDispatch")]
        public async Task<ApiResponseMessage> GenerateDispatch(Guid receiveID)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.GenerateDispatch(receiveID);
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

        /// <summary> 
        /// Get Receive Detail by product code
        /// </summary>
        /// <param name="productCode">Product Code</param>
        /// <returns>Collection of Receive Detail</returns>
        [HttpGet]
        [Route("api/receive/getbyproductcode")]
        [ResponseType(typeof(List<ReceiveDetail>))]
        public async Task<IHttpActionResult> getbyproductcode(Guid receiveID, string productCode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ReceiveDetail result = await Task.Run(() =>
                {
                    return _Service.GetReceiveDetailByProductCode(receiveID, productCode);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Get Receive Detail by pallet code
        /// </summary>
        /// <param name="palletCode">Pallet Code</param>
        /// <returns>Collection of Receive Detail</returns>
        [HttpGet]
        [Route("api/receive/getbypallet")]
        [ResponseType(typeof(List<ReceiveDetail>))]
        public async Task<IHttpActionResult> getbypallet(Guid receiveID, string palletCode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ReceiveDetail result = await Task.Run(() =>
                {
                    return _Service.GetReceiveDetailByPallet(receiveID, palletCode);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/receive/getreceivedetail")]
        [ResponseType(typeof(List<ReceiveDetail>))]
        public async Task<IHttpActionResult> getreceivedetail(Guid receiveDetailID)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ReceiveDetail result = await Task.Run(() =>
                {
                    return _Service.GetReceiveDetail(receiveDetailID);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region [ Receiving]
        /// <summary>
        /// Receiving Product
        /// </summary>
        /// <param name="entity">Receiving Model</param>
        /// <returns>Ok</returns>
        [HttpPut]
        [Route("api/receiving/receiving")]
        public async Task<IHttpActionResult> receiving(Receiving entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    _Service.Receiving(entity);
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Finish Receiving list
        /// </summary>
        /// <param name="id">GRN ID</param>
        /// <returns>Ok</returns>
        [HttpPut]
        [Route("api/receiving/finishreceiving")]
        public async Task<IHttpActionResult> finishreceiving(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.UserID = _header.UserID.Value;
                    _Service.FinishReceiving(id);
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get GRN No List of receive
        /// </summary>
        /// <param name="receiveid">Receive ID</param>
        /// <returns>Collection of GRN No</returns>
        [HttpGet]
        [Route("api/receiving/getgrnlist")]
        [ResponseType(typeof(List<Receiving>))]
        public async Task<IHttpActionResult> getgrnlist(Guid receiveid)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Receiving> result = await Task.Run(() =>
                {
                    return _Service.GetReceivingNo(receiveid);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Receiving list by GRN No
        /// </summary>
        /// <param name="grnno">GRN No</param>
        /// <returns>Collection of receiving</returns>
        [HttpGet]
        [Route("api/receiving/getreceivingbygrnno")]
        [ResponseType(typeof(List<Receiving>))]
        public async Task<IHttpActionResult> GetReceivingByGRNNo(string grnno)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Receiving> result = await Task.Run(() =>
                {
                    return _Service.GetReceivingList(grnno);
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/receiving/GetReceivingWaitFinish")]
        [ResponseType(typeof(List<Receiving>))]
        public async Task<IHttpActionResult> GetReceivingWaitFinish(string receiveCode)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<Receiving> result = await Task.Run(() =>
                {
                    List<Receiving> receivingList = new List<Receiving>();
                    _Service.GetByReceiveCode(receiveCode)
                                    .ReceiveDetailCollection.ToList()
                                    .ForEach(item =>
                                    {
                                        receivingList.AddRange(item.ReceivingCollection.Where(x => x.IsActive && x.ReceivingStatus == ReceivingStatusEnum.Inprogress).ToList());
                                    });

                    return receivingList;
                });

                HttpResponseMessage responseMsg = Request.CreateResponse(HttpStatusCode.OK, result);
                responseMsg.Headers.Add("X-TotalRecords", _totalRecord.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Send to Production Control

        [HttpPost]
        [Route("api/receive/sendtoproductioncontrol")]
        public async Task<ApiResponseMessage> SendtoProductionControl(List<Guid> receiveIDs)
        {
            try
            {
                bool result = await Task.Run(() =>
                {
                    _Service.UserID = UserId;
                    return _Service.SendtoProductionControl(receiveIDs);
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

        #endregion



    }
}
