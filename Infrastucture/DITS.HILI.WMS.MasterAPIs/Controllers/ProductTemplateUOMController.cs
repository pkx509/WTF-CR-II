using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterService.Products;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ProductTemplateUOMController : BaseApiController
    {
        private readonly IProductTemplateUOMService _Service;
        private readonly IMessageService _messageService;

        public ProductTemplateUOMController(IProductTemplateUOMService service, IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/producttemplateuom/getbyid")]
        // [ResponseType(typeof(Units))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ProductTemplateUom result = await Task.Run(() =>
                {
                    return _Service.GetById(id);
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
        [Route("api/producttemplateuom/getproducttemplateuom")]
        //[ResponseType(typeof(List<Units>))]
        public async Task<ApiResponseMessage> GetProductTemplateUom(string keyword, bool IsActive, int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ProductTemplateUom> result = await Task.Run(() =>
                {
                    return _Service.GetProductTemplateUom(keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/producttemplateuom/getproducttemplateuomdetail")]
        //[ResponseType(typeof(List<Units>))]
        public async Task<ApiResponseMessage> GetProductTemplateUomDetail(Guid productuomtemplateid)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ProductTemplateUomDetail> result = await Task.Run(() =>
                {
                    return _Service.GetProductTemplateUomDetail(productuomtemplateid);
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
        [Route("api/producttemplateuom/add")]
        // [ResponseType(typeof(Units))]
        public async Task<ApiResponseMessage> add(ProductTemplateUom entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.AddProductTemplateUom(entity);
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
        [Route("api/producttemplateuom/modify")]
        public async Task<ApiResponseMessage> modify(ProductTemplateUom entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _Service.ModifyProductTemplateUom(entity);
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
        [Route("api/producttemplateuom/remove")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveProductTemplateUom(id);
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

    }
}