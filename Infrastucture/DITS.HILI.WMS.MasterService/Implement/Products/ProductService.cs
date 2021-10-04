using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Products
{
    public class ProductService : Repository<Product>, IProductService
    {
        #region [ Property ] 
        private readonly IRepository<ProductCodes> ProductCodeService;
        private readonly IRepository<ProductTemplateUomDetail> ProductTemplateUomDetailService;
        private readonly IRepository<ProductGroupLevel3> ProductGroupLevel3Service;
        private readonly IRepository<ProductStatus> ProductStatusService;
        private readonly IRepository<ProductSubStatus> ProductSubStatusService;
        private readonly IRepository<ProductUnit> ProductUnitService;
        private readonly IRepository<ProductBrand> ProductBrandService;
        private readonly IRepository<ProductShape> ProductShapeService;
        private readonly IRepository<Dispatch> DispatchService;
        private readonly IRepository<DispatchDetail> DispatchDetailService;
        private readonly IRepository<DispatchBooking> DispatchBookingService;
        private readonly IRepository<ItfInterfaceMapping> ItfInterfaceMappingService;
        #endregion

        #region [ Constructor ]


        public ProductService(IUnitOfWork context) : base(context)
        {
            ProductCodeService = context.Repository<ProductCodes>();
            ProductTemplateUomDetailService = context.Repository<ProductTemplateUomDetail>();
            ProductGroupLevel3Service = context.Repository<ProductGroupLevel3>();
            ProductStatusService = context.Repository<ProductStatus>();
            ProductSubStatusService = context.Repository<ProductSubStatus>();
            ProductUnitService = context.Repository<ProductUnit>();
            ProductBrandService = context.Repository<ProductBrand>();
            ProductShapeService = context.Repository<ProductShape>();
            DispatchService = context.Repository<Dispatch>();
            DispatchDetailService = context.Repository<DispatchDetail>();
            DispatchBookingService = context.Repository<DispatchBooking>();
            ItfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
        }

        #endregion

        #region [ Method ]


        public Product Get(Guid? productOwnerID, Guid id)
        {
            try
            {
                Product _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.IsActive && x.ProductID == id)
                                  .Include(x => x.CodeCollection.Where(s => s.IsActive))
                                  .Include(x => x.ColorCollection)
                                  .Include(x => x.UnitCollection)
                                  .Include(x => x.SafetyStockCollection)
                                  .Get()
                                  .Where(x => (productOwnerID != null ? x.ProductOfProductOwnerCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true) &&
                                              (productOwnerID != null ? x.SafetyStockCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true))
                                  .FirstOrDefault();

                return _current;
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public Product GetAllByID(Guid? productOwnerID, Guid id)
        {
            try
            {
                Product _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ProductID == id)
                                  .Include(x => x.CodeCollection)
                                  .Include(x => x.ColorCollection)
                                  .Include(x => x.UnitCollection)
                                  .Include(x => x.SafetyStockCollection)
                                  .Get()
                                  .Where(x => (productOwnerID != null ? x.ProductOfProductOwnerCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true) &&
                                              (productOwnerID != null ? x.SafetyStockCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true))
                                  .FirstOrDefault();

                return _current;
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public List<Product> Get(Guid? productOwnerID, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Product> result = Query().Include(x => x.ProductGroupLevel3)
                                    //.Include(x => x.ProductBrand)
                                    //.Include(x => x.ProductShape)
                                    .Include(x => x.CodeCollection)
                                    .Include(x => x.UnitCollection)
                                    .Include(x => x.ProductOfProductOwnerCollection)
                                    .Filter(x => x.IsActive == true &&
                                                 (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.CodeCollection.Any(s => s.Code.Contains(keyword))) &&
                                                 (brandId != null ? x.ProductBrandID == brandId : true) &&
                                                 (shapeId != null ? x.ProductShapeID == shapeId : true) &&
                                                 (groupLV3Id != null ? x.ProductGroupLevel3ID == groupLV3Id : true) &&
                                                 (productOwnerID != null ? x.ProductOfProductOwnerCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true))
                                                 .OrderBy(x => x.OrderBy(s => s.Name))
                                                 .Get(out totalRecords, pageIndex, pageSize);

                return result.ToList();
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }

        }

        public List<Product> GetByStockCode(Guid? productOwnerID, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Product> result = Query().Include(x => x.ProductGroupLevel3)
                                    .Include(x => x.CodeCollection)
                                    .Include(x => x.UnitCollection)
                                    .Include(x => x.ProductOfProductOwnerCollection)
                                    .Filter(x => x.IsActive == true && x.CodeCollection.Any(s => s.CodeType == ProductCodeTypeEnum.Stock) &&
                                                 (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.CodeCollection.Any(s => s.Code.Contains(keyword))) &&
                                                 (brandId != null ? x.ProductBrandID == brandId : true) &&
                                                 (shapeId != null ? x.ProductShapeID == shapeId : true) &&
                                                 (groupLV3Id != null ? x.ProductGroupLevel3ID == groupLV3Id : true) &&
                                                 (productOwnerID != null ? x.ProductOfProductOwnerCollection.All(s => s.ProductOwnerID == productOwnerID.Value) : true))
                                                 .OrderBy(x => x.OrderBy(s => s.Name))
                                                 .Get(out totalRecords, pageIndex, pageSize);


                List<Product> resultList = result.ToList();
                return resultList;
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }

        }

        public List<ProductModel> GetAll(Guid? productOwnerID, Guid? productId, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from _product in Query().Include(x => x.CodeCollection).Get()
                              join _productcode in ProductCodeService.Query().Filter(x => x.IsActive && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                               on _product.ProductID equals _productcode.ProductID
                              join _pg3 in ProductGroupLevel3Service.Query().Filter(x => x.IsActive == true).Get()
                               on _product.ProductGroupLevel3ID equals _pg3.ProductGroupLevel3ID into pg3
                              from _productGroupLevel3 in pg3.DefaultIfEmpty()
                              join _pb in ProductBrandService.Query().Filter(x => x.IsActive == true).Get()
                                on _product.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Filter(x => x.IsActive == true).Get()
                                on _product.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _pu in ProductUnitService.Query().Filter(x => x.IsActive == true && x.IsBaseUOM == true).Get()
                                on _product.ProductID equals _pu.ProductID into pu
                              from _unit in pu.DefaultIfEmpty()
                              where (_product.Name.Contains(keyword) ||
                                     _productcode.Code.Contains(keyword))
                              select new
                              {
                                  ProductID = _product.ProductID,
                                  ProductGroupLevel3ID = _product?.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = _productGroupLevel3?.Name,
                                  ProductCodeModel = _product?.CodeCollection.ToList(),
                                  ProductName = _product.Name,
                                  ProductCode = _productcode.Code,
                                  Description = _product.Description,
                                  ProductBrandID = _prodcutbrand?.ProductBrandID,
                                  ProductBrandName = _prodcutbrand?.Name,
                                  ProductShapeID = _prodcutshap?.ProductShapeID,
                                  ProductShapeName = _prodcutshap?.Name,
                                  Age = _product.Age,
                                  SafetyStockQTY = _product.SafetyStockQTY,
                                  IsActive = _product.IsActive,
                                  ProductUnitID = _unit?.ProductUnitID,
                                  ProductUnitName = _unit?.Name,
                                  ProductModelName = _product.ProductModel
                              } into g
                              group g by new
                              {
                                  g.ProductID,
                                  g.ProductGroupLevel3ID,
                                  g.ProductGroupLevel3Name,
                                  g.ProductCodeModel,
                                  g.ProductName,
                                  g.ProductCode,
                                  g.Description,
                                  g.ProductBrandID,
                                  g.ProductBrandName,
                                  g.ProductShapeID,
                                  g.ProductShapeName,
                                  g.Age,
                                  g.SafetyStockQTY,
                                  g.IsActive,
                                  g.ProductUnitID,
                                  g.ProductUnitName,
                                  g.ProductModelName
                              } into g2
                              select new
                              {
                                  ProductID = g2.Key.ProductID,
                                  ProductGroupLevel3ID = g2.Key.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = g2.Key.ProductGroupLevel3Name,
                                  ProductCodeModel = g2.Key.ProductCodeModel,
                                  ProductName = g2.Key.ProductName,
                                  ProductCode = g2.Key.ProductCode,
                                  Description = g2.Key.Description,
                                  ProductBrandID = g2.Key.ProductBrandID,
                                  ProductBrandName = g2.Key.ProductBrandName,
                                  ProductShapeID = g2.Key.ProductShapeID,
                                  ProductShapeName = g2.Key.ProductShapeName,
                                  Age = g2.Key.Age,
                                  SafetyStockQTY = g2.Key.SafetyStockQTY,
                                  IsActive = g2.Key.IsActive,
                                  ProductUnitID = g2.Key.ProductUnitID,
                                  ProductUnitName = g2.Key.ProductUnitName,
                                  ProductModelName = g2.Key.ProductModelName,
                              }).Where(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) &&
                                            (productId != null ? x.ProductID == productId : true) &&
                                            (brandId != null ? x.ProductBrandID == brandId : true) &&
                                            (shapeId != null ? x.ProductShapeID == shapeId : true) &&
                                            (groupLV3Id != null ? x.ProductGroupLevel3ID == groupLV3Id : true));



                if (result == null || result.Count() == 0)
                {
                    return new List<ProductModel>();
                }

                totalRecords = result.Count();

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ProductName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<ProductModel> productResult = result.Select(n => new ProductModel
                {
                    ProductID = n.ProductID,
                    ProductGroupLevel3ID = n.ProductGroupLevel3ID,
                    ProductGroupLevel3Name = n.ProductGroupLevel3Name,
                    ProductCode = n.ProductCode,
                    ProductName = n.ProductName,
                    Description = n.Description,
                    ProductBrandID = n.ProductBrandID,
                    ProductBrandName = n.ProductBrandName,
                    ProductShapeID = n.ProductShapeID,
                    ProductShapeName = n.ProductShapeName,
                    Age = n.Age,
                    SafetyStockQTY = n.SafetyStockQTY,
                    IsActive = n.IsActive,
                    ProductCodeModel = n.ProductCodeModel.Where(x => x.IsActive).ToList(),
                    ProductUnitID = n.ProductUnitID,
                    ProductUnitName = n.ProductUnitName,
                    ProductModelName = n.ProductModelName
                }).ToList();

                return productResult.ToList();
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }

        }
        public List<ProductCustomModel> GetProductSelectAll(string productCode, string productName, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                IEnumerable<Product> product = Query().Filter(x => x.IsActive).Get();
                IEnumerable<ProductCodes> pCode = ProductCodeService.Query().Filter(x => x.IsActive).Get();

                IEnumerable<ProductCustomModel> results = from p in product
                                                          join c in pCode on p.ProductID equals c.ProductID
                                                          where (productCode != null ? c.Code.Contains(productCode) : true)
                                                                    && (productName != null ? p.Name.Contains(productName) : true)
                                                          select new ProductCustomModel()
                                                          {
                                                              ProductID = p.ProductID,
                                                              ProductCode = c.Code,
                                                              ProductName = p.Name
                                                          };

                if (results == null || results.Count() == 0)
                {
                    return new List<ProductCustomModel>();
                }

                #region Paging

                totalRecords = results.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderBy(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                #endregion

                return results.ToList();
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<ProductCustomModel> GetProductForInternalRec(string PONo, string ProductCode, string ProductName
                                                                    , bool IsCreditNote, bool IsNormal, bool ToReprocess
                                                                    , bool FromReprocess, bool IsItemChange
                                                                    , bool IsWithoutGoods, Guid? ReferenceDispatchTypeID
                                                                    , out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                IQueryable<ProductCustomModel> results = null;
                IQueryable<Product> products = Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductCodes> pCodes = ProductCodeService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Dispatch> dispatchs = DispatchService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<DispatchDetail> dispatchDetails = DispatchDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<DispatchBooking> dispatchBooking = DispatchBookingService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ItfInterfaceMapping> interfaceMappings = ItfInterfaceMappingService.Query().Filter(x => x.IsActive).GetQueryable();

                if ((ToReprocess == true && IsNormal == false && FromReprocess == false) || IsWithoutGoods == true)
                {
                    // CASE II : Internal Receive to Reprocess

                    var tmp = from d in dispatchs
                              join dd in dispatchDetails on d.DispatchId equals dd.DispatchId
                              join bk in dispatchBooking on dd.DispatchDetailId equals bk.DispatchDetailId
                              join p in products on dd.ProductId equals p.ProductID
                              join c in pCodes on p.ProductID equals c.ProductID
                              join itf in interfaceMappings on d.DocumentId equals itf.DocumentId
                              where (ProductCode != null ? c.Code.Contains(ProductCode) : true)
                                        && (ProductName != null ? p.Name.Contains(ProductName) : true)
                                        && (PONo != null ? d.Pono.Contains(PONo) : true)
                                        && ((itf.ToReprocess ?? false) == false)
                                        && c.CodeType == ProductCodeTypeEnum.Stock
                                        && d.DispatchStatus ==DispatchStatusEnum.Complete
                              select new
                              {
                                  ProductID = p.ProductID,
                                  ProductCode = c.Code,
                                  ProductName = p.Name,
                                  PONo = d.Pono,
                                  MFGDate = bk.Mfgdate,
                              };

                    results = tmp.GroupBy(x => new
                    {
                        x.ProductID,
                        x.ProductCode,
                        x.ProductName,
                        x.PONo,
                        x.MFGDate
                    }).Select(y => new ProductCustomModel()
                    {
                        ProductID = y.Key.ProductID,
                        ProductCode = y.Key.ProductCode,
                        ProductName = y.Key.ProductName,
                        PONo = y.Key.PONo,
                        MFGDate = y.Key.MFGDate,
                    });

                }
                else if (ToReprocess == false && IsNormal == false && FromReprocess == true && IsItemChange == false)
                {
                    // CASE III : Internal Receive from Reprocess

                    results = from d in dispatchs
                              join dd in dispatchDetails on d.DispatchId equals dd.DispatchId
                              join p in products on dd.ProductId equals p.ProductID
                              join c in pCodes on p.ProductID equals c.ProductID
                              join itf in interfaceMappings on d.DocumentId equals itf.DocumentId
                              where (ProductCode != null ? c.Code.Contains(ProductCode) : true)
                                        && (ProductName != null ? p.Name.Contains(ProductName) : true)
                                        && (PONo != null ? d.Pono.Contains(PONo) : true)
                                        && ((itf.ToReprocess ?? false) == true)
                                        && c.CodeType == ProductCodeTypeEnum.Stock
                              select new ProductCustomModel()
                              {
                                  ProductID = p.ProductID,
                                  ProductCode = c.Code,
                                  ProductName = p.Name,
                                  PONo = d.Pono
                              };

                }
                else if ((ToReprocess == false && IsNormal == false && FromReprocess == false) || IsItemChange == true)
                {
                    // CASE I : Internal Receive QA Inspection && Repack

                    results = from p in products
                              join c in pCodes on p.ProductID equals c.ProductID
                              where (ProductCode != null ? c.Code.Contains(ProductCode) : true)
                                        && (ProductName != null ? p.Name.Contains(ProductName) : true)
                                        && c.CodeType == ProductCodeTypeEnum.Stock
                              select new ProductCustomModel()
                              {
                                  ProductID = p.ProductID,
                                  ProductCode = c.Code,
                                  ProductName = p.Name
                              };
                }
                else
                {
                    throw new HILIException("MSG00069");
                }

                if (results == null || results.Count() == 0)
                {
                    return new List<ProductCustomModel>();
                }

                #region Paging

                totalRecords = results.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderBy(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                #endregion

                return results.ToList();
            }
            catch (HILIException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Warning, MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public ApiResponseMessage AddProduct(Product entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.Name == entity.Name && x.IsActive == true);

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.ProdCode = entity.Code;
                    entity.ProductID = Guid.NewGuid();
                    entity.URLImage = "---";
                    entity.Description = "---";
                    entity.IsActive = true;
                    entity.UserCreated = UserID;
                    entity.UserModified = UserID;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    entity.CodeCollection.ToList().ForEach(data =>
                    {
                        data.UserCreated = UserID;
                        data.UserModified = UserID;
                        data.DateCreated = DateTime.Now;
                        data.DateModified = DateTime.Now;
                    });

                    Product result = base.Add(entity);

                    List<ProductTemplateUomDetail> _templateUomDetail = ProductTemplateUomDetailService.Query().Filter(x => x.IsActive == true && x.Product_UOM_Template_ID == entity.ProductUOMTemplateID).Get().ToList();

                    if (_templateUomDetail.Count() > 0)
                    {
                        _templateUomDetail.ForEach(item =>
                        {
                            ProductUnit productUnit = new ProductUnit();

                            productUnit = new ProductUnit()
                            {
                                ProductUnitID = Guid.NewGuid(),
                                ProductID = entity.ProductID,
                                Code = item.Product_UOM_Template_Detail_Name,
                                Barcode = item.Product_UOM_Template_Detail_Barcode == null ? "*" + entity.ProdCode + "*" : item.Product_UOM_Template_Detail_Barcode,
                                Name = item.Product_UOM_Template_Detail_Name,
                                Description = "---",
                                Quantity = Convert.ToDecimal(item.Product_UOM_Template_Detail_Quantity),
                                PalletQTY = 0,
                                IsBaseUOM = Convert.ToBoolean(item.Product_UOM_Template_Detail_SKU),
                                Width = item.Product_UOM_Template_Detail_Package_Width.Value,
                                Height = item.Product_UOM_Template_Detail_Package_Height.Value,
                                Length = item.Product_UOM_Template_Detail_Package_Length.Value,
                                Cubicmeters = 1,
                                ProductWeight = 1,
                                PackageWeight = item.Product_UOM_Template_Detail_Package_Weight.Value,
                                URLImage = "---",
                                ConversionMark = 1,
                                Remark = item.Remark,
                                IsActive = item.IsActive,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };

                            ProductUnitService.Add(productUnit);

                        });

                    }

                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ProductID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }

            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public ApiResponseMessage ModifyProduct(Product entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Product _current = Query().Filter(x => x.ProductID == entity.ProductID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.Name = entity.Name;
                    _current.ProductBrandID = entity.ProductBrandID;
                    _current.Remark = entity.Remark;
                    _current.SafetyStockQTY = entity.SafetyStockQTY;
                    _current.Age = entity.Age;
                    _current.BaseUnitName = entity.BaseUnitName;
                    _current.Description = entity.Description;
                    _current.ProductGroupLevel3ID = entity.ProductGroupLevel3ID;
                    _current.ProductShapeID = entity.ProductShapeID;
                    _current.Description = entity.Description;
                    _current.IsActive = entity.IsActive;
                    _current.UserModified = UserID;
                    _current.DateModified = DateTime.Now;
                    base.Modify(_current);

                    List<ProductCodes> itemCurrent = ProductCodeService.Query().Filter(x => x.IsActive).Include(x => x.Product).Filter(x => x.ProductID == entity.ProductID).Get().ToList();//&& x.Code == entity.Code
                    if (itemCurrent.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    IEnumerable<string> exceptCodes = itemCurrent.Select(x => x.Code);
                    List<string> inActiveCodes = exceptCodes.Except(entity.CodeCollection.Select(x => x.Code)).ToList();

                    entity.CodeCollection.ToList().ForEach(item =>
                    {
                        bool ok = itemCurrent.Any(x => x.IsActive == true
                                                   && x.Code == item.Code);
                        if (ok)
                        {
                            IRepository<ProductCodes> productCodes = Context.Repository<ProductCodes>();
                            ProductCodes _productCodes = productCodes.Query().Filter(x => x.IsActive == true && x.ProductID == entity.ProductID && x.Code == item.Code).Get().SingleOrDefault();

                            _productCodes.Code = item.Code;
                            _productCodes.IsDefault = item.IsDefault;
                            _productCodes.IsActive = true;
                            _productCodes.CodeType = item.CodeType;
                            _productCodes.Description = item.Description;
                            _productCodes.Product = null;
                            _productCodes.DateModified = DateTime.Now;
                            _productCodes.UserModified = UserID;
                            productCodes.Modify(_productCodes);
                        }
                        else
                        {
                            item.UserCreated = UserID;
                            item.UserModified = UserID;
                            item.DateCreated = DateTime.Now;
                            item.DateModified = DateTime.Now;
                            ProductCodeService.Add(item);
                        }
                    });


                    foreach (string item in inActiveCodes)
                    {
                        IRepository<ProductCodes> productCodes = Context.Repository<ProductCodes>();
                        ProductCodes _productCodes = productCodes.Query().Filter(x => x.IsActive == true && x.ProductID == entity.ProductID && x.Code == item).Get().SingleOrDefault();

                        if (_productCodes != null)
                        {
                            _productCodes.IsActive = false;
                            _productCodes.DateModified = DateTime.Now;
                            _productCodes.UserModified = UserID;
                            productCodes.Modify(_productCodes);
                        }
                    }

                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ProductID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }

            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void RemoveProduct(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Product _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

                    scope.Complete();
                }

            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        #endregion
    }
}
