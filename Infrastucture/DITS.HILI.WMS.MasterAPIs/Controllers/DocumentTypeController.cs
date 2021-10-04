using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterService.Utility;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class DocumentTypeController : BaseApiController
    {
        private readonly IDocumentTypeService _Service;
        private readonly IMessageService _messageService;

        public DocumentTypeController(IDocumentTypeService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/DocumentType/getbyid")]
        //[ResponseType(typeof(DocumentType))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                DocumentType result = await Task.Run(() =>
                {
                    return _Service.Get(id);
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
        [Route("api/DocumentType/get")]
        //[ResponseType(typeof(List<DocumentType>))]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<DocumentType> result = await Task.Run(() =>
                {
                    return _Service.Get(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
        [Route("api/DocumentType/GetByDocTypeEnum")]
        //[ResponseType(typeof(List<DocumentType>))]
        public async Task<ApiResponseMessage> GetByDocTypeEnum(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DocumentType> result = await Task.Run(() =>
                {
                    return _Service.GetByDocTypeEnum(DocumentTypeEnum.Dispatch, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/DocumentType/GetByDocTypeEnumWithAll")]
        //[ResponseType(typeof(List<DocumentType>))]
        public async Task<ApiResponseMessage> GetByDocTypeEnumWithAll(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DocumentType> result = await Task.Run(() =>
                {
                    return _Service.GetByDocTypeEnumWithAll(DocumentTypeEnum.Dispatch, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/DocumentType/GetReceiveType")]
        public async Task<ApiResponseMessage> GetReceiveType(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DocumentType> result = await Task.Run(() =>
                {
                    return _Service.GetReceiveType(DocumentTypeEnum.Receive, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/DocumentType/GetForInternalReceive")]
        public async Task<ApiResponseMessage> GetForInternalReceive(string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<MasterModel.CustomModel.DocumentTypeCustomModel> result = await Task.Run(() =>
                {
                    return _Service.GetForInternalReceive(keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/DocumentType/GetRefDispatchType")]
        public async Task<ApiResponseMessage> GetRefDispatchType(Guid documentID, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DocumentType> result = await Task.Run(() =>
                {
                    return _Service.GetRefDispatchType(documentID, out _totalRecord, pageIndex, pageSize);
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