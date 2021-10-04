using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.InventoryToolsService;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DITS.HILI.WMS.Core.CustomException;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.InventoryToolsAPIs.Controllers
{
    public class TransferController : BaseApiController
    {
        ICycleCountService _Service;
        IMessageService _messageService;
        IMessageService _MessageService;
        public TransferController(ICycleCountService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/Transfer/GetTransferlist")]
        public ApiResponseMessage GetTransferlist(int State, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                var _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                 
                var result = _Service.GetlistAll(State, keyword, out _totalRecord, pageIndex, pageSize);

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
            }
        }
        
        [HttpGet]
        [Route("api/Transfer/GetTransferDetail")]
        public ApiResponseMessage GetTransferDetail(string keyword)
        {
            try
            {
                var _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                var result = _Service.GetAll(keyword);

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
            }
        }

        [HttpGet]
        [Route("api/Transfer/getTransferstatus")]
        public ApiResponseMessage getTransferstatus()
        {
            try
            {
                var _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;

                var result = _Service.GetCycleCountStatus();

                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _MessageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _MessageService.GetMessage("SYS99999", this.Language));
            }
        }

        #region Stock

        //[HttpGet]
        //[Route("api/Transfer/GetTransferPalletTag")]
        //public async Task<ApiResponseMessage> GetPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, Guid producttsatusId, int pageIndex = 0, int pageSize = 0)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        _Service.UserID = _header.UserID.Value;
        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.GetTransferPalletTag(palletNo, productCode, productName, Lot, Line, mfgDate, producttsatusId, out _totalRecord, pageIndex, pageSize);
        //        });

        //        return Succeed(result, _totalRecord);
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


        [HttpPost]
        [Route("api/Transfer/add")]
        public async Task<ApiResponseMessage> add(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = this.UserId;
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddCycleCount(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
            }
        }

        [HttpPut]
        [Route("api/Transfer/ModifyTransfer")]
        public async Task<ApiResponseMessage> ModifyTransfer(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = this.UserId;
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyCycleCount(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
            }
        }

        [HttpPut]
        [Route("api/Transfer/Approve")]
        public async Task<ApiResponseMessage> Approve(CycleCountModel entity)
        {
            try
            {
                _Service.UserID = this.UserId;
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.Approve(entity);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
            }
        }

        [HttpDelete]
        [Route("api/Transfer/remove")]
        public async Task<ApiResponseMessage> remove(string id)
        {
            try
            {
                _Service.UserID = this.UserId;
                await Task.Run(() =>
                {
                    _Service.Remove(id);
                });

                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
            }
            catch (Exception ex)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
            }
        }
        #endregion
    }
}