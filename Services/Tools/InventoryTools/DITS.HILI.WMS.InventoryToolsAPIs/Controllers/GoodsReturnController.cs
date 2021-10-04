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
    public class GoodsReturnController : BaseApiController
    {
        IGoodsReturnService _Service;
        IMessageService _MessageService;
        public GoodsReturnController(IGoodsReturnService service, IMessageService messageService)
        {
            _Service = service;
            _MessageService = messageService;
        }

        [HttpGet]
        [Route("api/goodsreturn/getgoodsreturn")]
        public async Task<ApiResponseMessage> GetGoodsReturn(DateTime sdte, DateTime edte, GoodsReturnStatusEnum status, int pageIndex = 0, int pageSize = 20)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                int _totalRecord = 0;
                var _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                var result = await Task.Run(() =>
                {
                    return _Service.GetGoodsReturn(sdte, edte, status, out _totalRecord, pageIndex, pageSize);
                });

                var apiResp = Succeed(result, _totalRecord);

                return apiResp;

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
        [Route("api/goodsreturn/getgoodsreturnbyid")]
        public async Task<ApiResponseMessage> GetGoodsReturnByID( Guid GoodsReturnId)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            try
            { 
                var _header = ApiHelpers.Response(Request);
                _Service.UserID = _header.UserID.Value;
                var result = await Task.Run(() =>
                {
                    return _Service.GetGoodsReturnByID(GoodsReturnId);
                });

                var apiResp = Succeed(result);

                return apiResp;

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


        [HttpPost]
        [Route("api/goodsreturn/modifygoodsreturn")]
        public async Task<ApiResponseMessage> ModifyGoodsReturn(GoodsReturn entity)
        {
            try
            {
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ModifyGoodsReturn(entity);
                });

                return Succeed();
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

        [HttpPost]
        [Route("api/goodsreturn/approvegoodsreturn")]
        public async Task<ApiResponseMessage> ApproveGoodsReturn(GoodsReturn entity)
        {
            try
            {
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.ApproveGoodsReturn(entity);
                });

                return Succeed();
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


        [HttpPost]
        [Route("api/goodsreturn/dispatchgoodsreturn")]
        public async Task<ApiResponseMessage> DispatchGoodsReturn(GoodsReturn entity)
        {
            try
            {
                var _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.DispatchGoodsReturn(entity);
                });

                return Succeed();
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


    }
}