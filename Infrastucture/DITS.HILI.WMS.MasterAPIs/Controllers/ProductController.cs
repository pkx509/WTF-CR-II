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
    public class ProductController : BaseApiController
    {
        private readonly IProductService _Service;
        private readonly IMessageService _messageService;

        public ProductController(IProductService service,
                              IMessageService messageService)
        {
            _Service = service;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("api/Product/getbyid")]
        //[ResponseType(typeof(Product))]
        public async Task<ApiResponseMessage> getbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Product result = await Task.Run(() =>
                {
                    return _Service.Get(_header.ProductOwnerID, id);
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
        [Route("api/Product/getallbyid")]
        //[ResponseType(typeof(Product))]
        public async Task<ApiResponseMessage> getallbyid(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                Product result = await Task.Run(() =>
                {
                    return _Service.GetAllByID(_header.ProductOwnerID, id);
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
        [Route("api/Product/get")]
        //[ResponseType(typeof(List<Product>))]
        public async Task<ApiResponseMessage> get(string keyword, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex = 0, int? pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Product> result = await Task.Run(() =>
                {
                    return _Service.Get(_header.ProductOwnerID, brandId, shapeId, groupLV3Id, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Product/GetByStockCode")]
        public async Task<ApiResponseMessage> GetByStockCode(string keyword, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex = 0, int? pageSize = 20)
        {
            try
            {
                int _totalRecord = 0;

                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<Product> result = await Task.Run(() =>
                {
                    return _Service.GetByStockCode(_header.ProductOwnerID, brandId, shapeId, groupLV3Id, keyword, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Product/getAll")]
        //[ResponseType(typeof(List<Product>))]
        public async Task<ApiResponseMessage> getAll(string keyword, bool IsActive, Guid? brandId, Guid? productId, Guid? shapeId, Guid? groupLV3Id, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;


                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ProductModel> result = await Task.Run(() =>
                {
                    return _Service.GetAll(_header.ProductOwnerID, productId, brandId, shapeId, groupLV3Id, keyword, IsActive, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Product/GetProductSelectAll")]
        public async Task<ApiResponseMessage> GetProductSelectAll(string productCode, string productName, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DITS.WMS.Data.CustomModel.ProductCustomModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductSelectAll(productCode, productName, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Product/GetProductForInternalRec")]
        public async Task<ApiResponseMessage> GetProductForInternalRec(string PONo, string ProductCode, string ProductName
                                                                        , bool IsCreditNote, bool IsNormal, bool ToReprocess
                                                                        , bool FromReprocess, bool IsItemChange
                                                                        , bool IsWithoutGoods, Guid? ReferenceDispatchTypeID
                                                                        , int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                System.Collections.Generic.List<DITS.WMS.Data.CustomModel.ProductCustomModel> result = await Task.Run(() =>
                {
                    return _Service.GetProductForInternalRec(PONo, ProductCode, ProductName
                                                                , IsCreditNote, IsNormal, ToReprocess
                                                                , FromReprocess, IsItemChange
                                                                , IsWithoutGoods, ReferenceDispatchTypeID
                                                                , out _totalRecord, pageIndex, pageSize);
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
        [Route("api/Product/AddProduct")]
        public async Task<ApiResponseMessage> add(Product entity)
        {
            try
            {
                _Service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _Service.AddProduct(entity);
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

        [HttpPut]
        [Route("api/Product/ModifyProduct")]
        public async Task<ApiResponseMessage> modify(Product entity)
        {
            try
            {
                //await Task.Run(() =>
                //{
                //    _Service.Modify(entity);
                //});

                //return Succeed();
                _Service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _Service.ModifyProduct(entity);
                });

                return Succeed(_result);
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpDelete]
        [Route("api/Product/RemoveProduct")]
        public async Task<ApiResponseMessage> remove(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _Service.RemoveProduct(id);
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