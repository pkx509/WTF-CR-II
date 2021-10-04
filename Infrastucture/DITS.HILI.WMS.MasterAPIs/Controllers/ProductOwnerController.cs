using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterService.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ProductOwnerController : BaseApiController
    {
        private readonly IProductOwnerService service;
        private readonly IMessageService _messageService;

        public ProductOwnerController(IProductOwnerService _service,
                              IMessageService messageService)
        {
            service = _service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/productowner/getbyid")]
        [ResponseType(typeof(ProductOwner))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ProductOwner result = await Task.Run(() =>
                {
                    return service.Get(id);
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
        [Route("api/productowner/get")]
        [ResponseType(typeof(List<Contact>))]
        public async Task<ApiResponseMessage> get(string keyword)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                List<ProductOwner> result = await Task.Run(() =>
                {
                    return service.Get(keyword, out _totalRecord, _header.PageIndex, _header.PageSize);
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
