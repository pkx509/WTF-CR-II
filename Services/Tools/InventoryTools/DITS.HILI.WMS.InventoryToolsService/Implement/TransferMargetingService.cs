using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Interface;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class TransferMargetingService : Repository<TRMTransferMarketing>, ITransferMargetingService
    {
        #region [ Property ] 
        private readonly IRepository<TRMTransferMarketingProduct> trmtransferMarketingProduct;
        private readonly IRepository<TRMTransferMarketingProductDetail> trmtransferMarketingProductDetail;
        private readonly IRepository<TransferMargetingPrefix> trmtransferMarketingPrefix;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductCodes> productCodeService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<StockInfo> StockInfoService;
        private readonly IRepository<StockBalance> StockBalanceService;
        private readonly IRepository<StockLocationBalance> StockLocationBalanceService;
        private readonly IRepository<StockTransaction> StockTransactionService;
        private readonly IRepository<ProductionControlDetail> ProductionControlDetailService;
        private readonly IRepository<ProductionControl> ProductionControlService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductStatus> ProductStatusService;
        private readonly IRepository<ProductSubStatus> ProductSubStatusService;
        private readonly IRepository<Line> lineService;
        private readonly IRepository<Truck> truckService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<ProductBrand> ProductBrandService;
        private readonly IRepository<ProductShape> ProductShapeService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<Zone> zoneService;
        private readonly IRepository<Receive> receiveService;
        private readonly IRepository<ReceiveDetail> receiveDetailService;
        private readonly IRepository<UserAccounts> UserAccountsService;
        private readonly IStockService stockService;
        private readonly IRepository<itf_temp_in_dispatch_log> itf_temp_in_dispatch_logService;

        #endregion

        public TransferMargetingService(IUnitOfWork context,
                                IRepository<TRMTransferMarketingProduct> _trmtransferMarketingProduct,
                                IRepository<TRMTransferMarketingProductDetail> _trmtransferMarketingProductDetail,
                                IRepository<TransferMargetingPrefix> _trmtransferMarketingPrefix,
                                IRepository<Product> _product,
                                IRepository<ProductCodes> _productCode,
                                IRepository<ProductOwner> _productOwner,
                                IRepository<StockInfo> _StockInfoService,
                                IRepository<StockTransaction> _StockTransactionService,
                                IRepository<StockBalance> _StockBalanceService,
                                IRepository<StockLocationBalance> _StockLocationBalanceService,
                                IRepository<Location> _location,
                                IRepository<ProductionControlDetail> _ProductionControlDetail,
                                IRepository<ProductionControl> _ProductionControl,
                                IRepository<ProductBrand> _ProductBrand,
                                IRepository<ProductShape> _ProductShape,
                                IRepository<ProductUnit> _productUnit,
                                IRepository<ProductStatus> _ProductStatus,
                                IRepository<ProductSubStatus> _ProductSubStatus,
                                IRepository<Receiving> _receiving,
                                IRepository<Line> _line,
                                IRepository<Truck> _truck,
                                IRepository<Warehouse> _warehouse,
                                IRepository<Zone> _zone,
                                IRepository<Receive> _receive,
                                IRepository<ReceiveDetail> _receiveDetail,
                                IRepository<UserAccounts> _UserAccountsService,
                                IRepository<ProductionControl> _productionControlService,
                                IRepository<itf_temp_in_dispatch_log> _itf_temp_in_dispatch_logService,
                                IStockService _stockService)
            : base(context)
        {
            trmtransferMarketingProduct = _trmtransferMarketingProduct;
            trmtransferMarketingProductDetail = _trmtransferMarketingProductDetail;
            trmtransferMarketingPrefix = _trmtransferMarketingPrefix;
            productService = _product;
            productCodeService = _productCode;
            locationService = _location;
            StockInfoService = _StockInfoService;
            StockBalanceService = _StockBalanceService;
            StockTransactionService = _StockTransactionService;
            StockLocationBalanceService = _StockLocationBalanceService;
            ProductionControlDetailService = _ProductionControlDetail;
            ProductionControlService = _productionControlService;
            ProductBrandService = _ProductBrand;
            ProductShapeService = _ProductShape;
            productUnitService = _productUnit;
            ProductStatusService = _ProductStatus;
            ProductSubStatusService = _ProductSubStatus;
            receivingService = _receiving;
            receiveService = _receive;
            productOwnerService = _productOwner;
            lineService = _line;
            stockService = _stockService;
            pcDetailService = context.Repository<ProductionControlDetail>();
            productionControlService = _productionControlService;
            warehouseService = _warehouse;
            receiveDetailService = _receiveDetail;
            UserAccountsService = _UserAccountsService;
            zoneService = _zone;
            truckService = _truck;
            itf_temp_in_dispatch_logService = _itf_temp_in_dispatch_logService;
        }

        public List<TRMTransferMarketing> GetTransferMargetinglist(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                TranferMargetingStatus statusEmun = TranferMargetingStatus.Approve;
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    statusEmun = (TranferMargetingStatus)Enum.Parse(typeof(TranferMargetingStatus), status);
                }

                IEnumerable<TRMTransferMarketing> list = from _trans in Query().Get()
                                                         where _trans.IsActive == true
                                                             && _trans.TransferDate.Date >= sdte
                                                             && _trans.TransferDate.Date <= edte
                                                             && ((!string.IsNullOrEmpty(status)
                                                             && status != "All" ? _trans.TransferStatus == (int)statusEmun : "" == ""))
                                                             && (_trans.TRM_CODE.ToLower().Contains(search.ToLower()))

                                                         select (new TRMTransferMarketing()
                                                         {
                                                             TRM_CODE = _trans.TRM_CODE,
                                                             TRM_ID = _trans.TRM_ID,
                                                             TransferDate = _trans.TransferDate,
                                                             Description = _trans.Description,
                                                             ApproveDate = _trans.ApproveDate,
                                                             TransferStatus = _trans.TransferStatus,
                                                             Remark = _trans.Remark,
                                                             IsActive = _trans.IsActive,
                                                             UserCreated = _trans.UserCreated,
                                                             UserModified = _trans.UserModified,
                                                             DateCreated = _trans.DateCreated,
                                                             DateModified = _trans.DateModified
                                                         });


                totalRecords = list.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    list = list.OrderByDescending(x => x.TransferDate).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                return list.ToList();
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
        public TRMTransferMarketing GetTransferMargetingDetail(Guid? TrmId)
        {
            try
            {
                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.Waiting_Receive,
                    (int)PackingStatusEnum.Loading_In,
                    (int)PackingStatusEnum.In_Progress,
                    (int)PackingStatusEnum.Transfer,
                    (int)PackingStatusEnum.PutAway,
                    (int)PackingStatusEnum.Damage,
                    (int)PackingStatusEnum.QAInspection
                };

                TRMTransferMarketing transfer = (from _trans in Query().Filter(x => x.IsActive == true).Get()
                                                 where (TrmId != null ? _trans.TRM_ID == TrmId.Value : true)
                                                 select new TRMTransferMarketing()
                                                 {
                                                     TRM_CODE = _trans.TRM_CODE,
                                                     TRM_ID = _trans.TRM_ID,
                                                     TransferDate = _trans.TransferDate,
                                                     Description = _trans.Description,
                                                     ApproveDate = _trans.ApproveDate,
                                                     TransferStatus = _trans.TransferStatus,
                                                     Remark = _trans.Remark,
                                                     IsActive = _trans.IsActive,
                                                     UserCreated = _trans.UserCreated,
                                                     UserModified = _trans.UserModified,
                                                     DateCreated = _trans.DateCreated,
                                                     DateModified = _trans.DateModified

                                                 }).FirstOrDefault();


                IEnumerable<TRMTransferMarketingProduct> details = (from _trans in Query().Filter(x => x.IsActive == true && (TrmId != null ? x.TRM_ID == TrmId.Value : true)).Get()
                                                                    join _transproduct in trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _trans.TRM_ID equals _transproduct.TRM_ID
                                                                    join _product in productService.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _transproduct.Product_ID equals _product.ProductID
                                                                    join _productCode in productCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                                      on _product.ProductID equals _productCode.ProductID
                                                                    join _productUnit in productUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _transproduct.TransferUnitID equals _productUnit.ProductUnitID
                                                                    select new TRMTransferMarketingProduct
                                                                    {
                                                                        TRM_Product_ID = _transproduct.TRM_Product_ID,
                                                                        TRM_ID = _trans.TRM_ID,
                                                                        Product_ID = _transproduct.Product_ID,
                                                                        TransferQty = _transproduct.TransferQty,
                                                                        TotalPickQty = _transproduct.TotalPickQty,
                                                                        TransferUnitID = _transproduct.TransferUnitID,
                                                                        ProductStatusID = _transproduct.ProductStatusID,
                                                                        TransferBaseUnitID = _transproduct.TransferBaseUnitID,
                                                                        TransferBaseQty = _transproduct.TransferBaseQty,
                                                                        PickQty = _transproduct.PickQty,
                                                                        ConfirmQty = _transproduct.ConfirmQty,
                                                                        PickStatus = _transproduct.PickStatus,
                                                                        ProductName = _product.Name,
                                                                        ProductCode = _productCode.Code,
                                                                        ProductUnitName = _productUnit.Name,
                                                                        Remark = _transproduct.Remark,
                                                                        DateCreated = _transproduct.DateCreated,
                                                                        DateModified = _transproduct.DateModified,
                                                                        UserCreated = _transproduct.UserCreated,
                                                                        UserModified = _transproduct.UserModified,
                                                                    });


                transfer.TRMTransferMarketingProduct = details.ToList();
                return transfer;

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
        public TRMTransferMarketingProduct GetTransferMargetingDetailByPallet(Guid? TrmProductId)
        {
            try
            {
                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.In_Progress,
                    (int)PackingStatusEnum.PutAway,
                    (int)PackingStatusEnum.Damage
                };

                var tmps = (from _trans in Where(x => x.IsActive == true)
                            join _transproduct in trmtransferMarketingProduct.Where(x => x.IsActive == true && (TrmProductId != null ? x.TRM_Product_ID == TrmProductId.Value : true))
                              on _trans.TRM_ID equals _transproduct.TRM_ID
                            join _product in productService.Where(x => x.IsActive == true) on _transproduct.Product_ID equals _product.ProductID
                            join _productCode in productCodeService.Where(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock) on _product.ProductID equals _productCode.ProductID
                            join _productUnit in productUnitService.Where(x => x.IsActive == true) on _transproduct.TransferUnitID equals _productUnit.ProductUnitID
                            select new { _trans = _trans, _transproduct = _transproduct, _product = _product, _productCode = _productCode, _productUnit = _productUnit }).ToList();

                TRMTransferMarketingProduct transferProduct = (from t in tmps
                                                               select new TRMTransferMarketingProduct
                                                               {
                                                                   TRM_CODE = t._trans.TRM_CODE,
                                                                   TRM_Product_ID = t._transproduct.TRM_Product_ID,
                                                                   Product_ID = t._transproduct.Product_ID,
                                                                   TransferQty = t._transproduct.TransferQty,
                                                                   TotalPickQty = t._transproduct.TotalPickQty,
                                                                   TransferUnitID = t._transproduct.TransferUnitID,
                                                                   ProductStatusID = t._transproduct.ProductStatusID,
                                                                   TransferBaseUnitID = t._transproduct.TransferBaseUnitID,
                                                                   TransferBaseQty = t._transproduct.TransferBaseQty,
                                                                   PickQty = t._transproduct.PickQty,
                                                                   TotalConfirmQty = t._transproduct.TotalConfirmQty,
                                                                   PickStatus = t._transproduct.PickStatus,
                                                                   ProductName = t._product.Name,
                                                                   ProductCode = t._productCode.Code,
                                                                   ProductUnitName = t._productUnit.Name,
                                                                   Remark = t._transproduct.Remark,
                                                                   DateCreated = t._transproduct.DateCreated,
                                                                   DateModified = t._transproduct.DateModified,
                                                                   UserCreated = t._transproduct.UserCreated,
                                                                   UserModified = t._transproduct.UserModified,
                                                               }).FirstOrDefault();

                if (transferProduct == null)
                {
                    return transferProduct;
                }

                var tmps2 = (from _transproduct in trmtransferMarketingProduct.Where(x => x.IsActive == true && (TrmProductId != null ? x.TRM_Product_ID == TrmProductId.Value : true))
                             join _transproductDetail in trmtransferMarketingProductDetail.Where(x => x.IsActive == true) on _transproduct.TRM_Product_ID equals _transproductDetail.TRM_Product_ID
                             join _location in locationService.Where(x => x.IsActive == true) on _transproductDetail.LocationID equals _location.LocationID
                             join _product in productService.Where(x => x.IsActive == true) on _transproductDetail.ProductID equals _product.ProductID
                             join _productCode in productCodeService.Where(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock) on _product.ProductID equals _productCode.ProductID
                             join _productUnit in productUnitService.Where(x => x.IsActive == true) on _transproductDetail.PalletUnitID equals _productUnit.ProductUnitID
                             select new
                             {
                                 _transproduct = _transproduct,
                                 _transproductDetail = _transproductDetail,
                                 _location = _location,
                                 _product = _product,
                                 _productCode = _productCode,
                                 _productUnit = _productUnit
                             }).ToList();
                List<TRMTransferMarketingProductDetail> details = (from t in tmps2
                                                                   select new TRMTransferMarketingProductDetail
                                                                   {
                                                                       TRM_ID = t._transproduct.TRM_ID,
                                                                       TRM_Product_Detail_ID = t._transproductDetail.TRM_Product_Detail_ID,
                                                                       TRM_Product_ID = t._transproductDetail.TRM_Product_ID,
                                                                       ProductID = t._transproductDetail.ProductID,
                                                                       PalletQty = t._transproductDetail.PalletQty,
                                                                       PalletUnitID = t._transproductDetail.PalletUnitID,
                                                                       PalletBaseUnitID = t._transproductDetail.PalletBaseUnitID,
                                                                       PalletBaseQty = t._transproductDetail.PalletBaseQty,
                                                                       PickQty = t._transproductDetail.PickQty,
                                                                       ConfirmPickQty = t._transproductDetail.ConfirmPickQty,
                                                                       LotNo = t._transproductDetail.LotNo,
                                                                       Location = t._location.Code,
                                                                       PalletCode = t._transproductDetail.PalletCode,
                                                                       NewPalletCode = t._transproductDetail.NewPalletCode,
                                                                       PickStatus = t._transproductDetail.PickStatus,
                                                                       LocationID = t._transproductDetail.LocationID,
                                                                       ProductName = t._product.Name,
                                                                       ProductCode = t._productCode.Code,
                                                                       ProductUnitName = t._productUnit.Name,
                                                                       Remark = t._transproductDetail.Remark,
                                                                       DateCreated = t._transproductDetail.DateCreated,
                                                                       DateModified = t._transproductDetail.DateModified,
                                                                       UserCreated = t._transproductDetail.UserCreated,
                                                                       UserModified = t._transproductDetail.UserModified,
                                                                   }).ToList();

                transferProduct.TRMTransferMarketingProductDetail = details;
                return transferProduct;

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



        public bool Add(TRMTransferMarketing entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region [ PreFix ]

                    TransferMargetingPrefix prefix = trmtransferMarketingPrefix.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("TR10012");
                    }

                    TransferMargetingPrefix tPrefix = trmtransferMarketingPrefix.FindByID(prefix.PrefixID);

                    string TransferCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    entity.TRM_CODE = TransferCode;
                    tPrefix.IsLastest = false;

                    TransferMargetingPrefix newPrefix = new TransferMargetingPrefix()
                    {
                        IsLastest = true,
                        LastedKey = TransferCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    trmtransferMarketingPrefix.Add(newPrefix);

                    #endregion [ PreFix ]

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    TRMTransferMarketing _trans = new TRMTransferMarketing();

                    _trans = new TRMTransferMarketing
                    {
                        TRM_ID = Guid.NewGuid(),
                        TRM_CODE = TransferCode,
                        TransferDate = entity.TransferDate,
                        TransferStatus = (int)TranferMargetingStatus.New,
                        ApproveDate = null,
                        Description = entity.Description,
                        Remark = entity.Remark,
                        IsActive = true,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    base.Add(_trans);

                    entity.TRMTransferMarketingProduct.ToList().ForEach(item =>
                    {
                        TRMTransferMarketingProduct _transProduct = new TRMTransferMarketingProduct();

                        _transProduct = new TRMTransferMarketingProduct
                        {
                            TRM_Product_ID = Guid.NewGuid(),
                            TRM_ID = _trans.TRM_ID,
                            Product_ID = item.Product_ID,
                            TransferQty = item.TransferQty,
                            TransferUnitID = item.TransferUnitID,
                            ProductStatusID = item.ProductStatusID,
                            TransferBaseQty = item.TransferBaseQty,
                            TransferBaseUnitID = item.TransferBaseUnitID,
                            PickStatus = (int)TranferMargetingStatus.New,
                            PickQty = item.PickQty,
                            ConfirmQty = item.ConfirmQty,
                            Remark = entity.Remark,
                            IsActive = true,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };

                        trmtransferMarketingProduct.Add(_transProduct);
                    });

                    scope.Complete();
                    return true;
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

        public bool ModifyByProduct(TRMTransferMarketing entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.TRMTransferMarketingProduct.ToList().ForEach(item =>
                    {
                        Guid? TrmId = entity.TRMTransferMarketingProduct.FirstOrDefault().TRM_ID == null ? entity.TRM_ID : entity.TRMTransferMarketingProduct.FirstOrDefault().TRM_ID;
                        TRMTransferMarketingProduct _transProduct = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == item.TRM_Product_ID).Get().SingleOrDefault();
                        IRepository<TRMTransferMarketingProduct> _tpd = Context.Repository<TRMTransferMarketingProduct>();
                        TRMTransferMarketingProduct _tpditem = _tpd.FindByID(item.TRM_Product_ID);

                        if (_tpditem != null)
                        {
                            _tpditem.PickQty = item.PickQty;
                            _tpditem.IsActive = true;
                            _tpditem.UserCreated = UserID;
                            _tpditem.UserModified = UserID;
                            _tpditem.DateCreated = DateTime.Now;
                            _tpd.Modify(_tpditem);
                        }
                        else
                        {

                            TRMTransferMarketingProduct _transprod = new TRMTransferMarketingProduct();

                            _transprod = new TRMTransferMarketingProduct
                            {
                                TRM_Product_ID = Guid.NewGuid(),
                                TRM_ID = TrmId,
                                Product_ID = item.Product_ID,
                                TransferQty = item.TransferQty,
                                TransferUnitID = item.TransferUnitID,
                                ProductStatusID = item.ProductStatusID,
                                TransferBaseQty = item.TransferBaseQty,
                                TransferBaseUnitID = item.TransferBaseUnitID,
                                PickStatus = (int)TranferMargetingStatus.New,
                                PickQty = item.PickQty,
                                ConfirmQty = item.ConfirmQty,
                                Remark = entity.Remark,
                                IsActive = true,
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                            };

                            trmtransferMarketingProduct.Add(_transprod);
                        }
                    });

                    scope.Complete();
                    return true;
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
        public bool AddByPallet(TRMTransferMarketingProduct entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    TRMTransferMarketingProduct _transProduct = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == entity.TRM_Product_ID).Get().FirstOrDefault();

                    if (_transProduct == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.TRMTransferMarketingProductDetail.ToList().ForEach(item =>
                    {
                        TRMTransferMarketingProductDetail _Details = new TRMTransferMarketingProductDetail();

                        _Details = new TRMTransferMarketingProductDetail
                        {
                            TRM_Product_Detail_ID = Guid.NewGuid(),
                            TRM_Product_ID = entity.TRM_Product_ID,
                            ProductID = item.ProductID,
                            PalletQty = item.PalletQty,
                            PalletUnitID = item.PalletUnitID,
                            PalletBaseUnitID = item.PalletBaseUnitID,
                            PalletBaseQty = item.PalletBaseQty,
                            PickQty = item.PickQty,
                            ConfirmPickQty = item.ConfirmPickQty,
                            LotNo = item.LotNo,
                            PalletCode = item.PalletCode,
                            NewPalletCode = item.NewPalletCode,
                            PickStatus = item.PickStatus,
                            LocationID = item.LocationID,
                            Remark = item.Remark,
                            IsActive = true,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };

                        trmtransferMarketingProductDetail.Add(_Details);


                        List<ProductionControlDetail> _getpcdts = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == _Details.PalletCode).Get().ToList();

                        ProductionControlDetail _getpcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == _Details.PalletCode).Get().FirstOrDefault();

                        ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(_getpcdt.PackingID);

                        if (updatepcdt.RemainQTY >= updatepcdt.ReserveQTY + _Details.PickQty)
                        {
                            updatepcdt.ReserveBaseQTY = (_Details.PickQty + updatepcdt.ReserveQTY) * updatepcdt.ConversionQty;
                            updatepcdt.ReserveQTY = _Details.PickQty + updatepcdt.ReserveQTY;
                            updatepcdt.UserModified = UserID;
                            updatepcdt.DateModified = DateTime.Now;
                            ProductionControlDetailService.Modify(updatepcdt);
                        }
                        else
                        {
                            throw new HILIException("MSG00039");
                        }
                    });


                    decimal? _sumPickQty = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == entity.TRM_Product_ID).Get().Sum(x => x.PickQty);

                    _transProduct.PickQty = _sumPickQty;
                    _transProduct.IsActive = true;
                    _transProduct.UserCreated = UserID;
                    _transProduct.UserModified = UserID;
                    _transProduct.DateCreated = DateTime.Now;
                    _transProduct.DateModified = DateTime.Now;

                    trmtransferMarketingProduct.Modify(_transProduct);



                    scope.Complete();
                    return true;
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

        public bool ModifyByPallet(TRMTransferMarketingProduct entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.TRMTransferMarketingProductDetail.ToList().ForEach(item =>
                    {
                        TRMTransferMarketingProductDetail _transProductDetail = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.TRM_Product_Detail_ID == item.TRM_Product_Detail_ID).Get().FirstOrDefault();//.SingleOrDefault();

                        if (_transProductDetail != null)
                        {
                            decimal? old_reserve = _transProductDetail.PickQty;

                            _transProductDetail.PickQty = item.PickQty;
                            _transProductDetail.IsActive = true;
                            _transProductDetail.UserCreated = UserID;
                            _transProductDetail.UserModified = UserID;
                            _transProductDetail.DateCreated = DateTime.Now;
                            trmtransferMarketingProductDetail.Modify(_transProductDetail);

                            //var _getpcdts = ProductionControlDetailService.Query().Filter(x => x.IsActive &&
                            //x.PalletCode == item.PalletCode).Get().ToList();

                            ProductionControlDetail _getpcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive &&
                            x.PalletCode == item.PalletCode).Get().FirstOrDefault();//.SingleOrDefault();

                            ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(_getpcdt.PackingID);

                            if (updatepcdt.RemainQTY >= updatepcdt.ReserveQTY + item.PickQty - old_reserve)
                            {
                                updatepcdt.ReserveBaseQTY = (updatepcdt.ReserveQTY + item.PickQty - old_reserve) * updatepcdt.ConversionQty;
                                updatepcdt.ReserveQTY = updatepcdt.ReserveQTY + item.PickQty - old_reserve;
                                updatepcdt.UserModified = UserID;
                                updatepcdt.DateModified = DateTime.Now;
                                ProductionControlDetailService.Modify(updatepcdt);
                            }
                            else
                            {
                                throw new HILIException("MSG00039");
                            }

                        }
                        else
                        {
                            TRMTransferMarketingProductDetail _Details = new TRMTransferMarketingProductDetail();

                            _Details = new TRMTransferMarketingProductDetail
                            {
                                TRM_Product_Detail_ID = Guid.NewGuid(),
                                TRM_Product_ID = entity.TRM_Product_ID,
                                ProductID = item.ProductID,
                                PalletQty = item.PalletQty,
                                PalletUnitID = item.PalletUnitID,
                                PalletBaseUnitID = item.PalletBaseUnitID,
                                PalletBaseQty = item.PalletBaseQty,
                                PickQty = item.PickQty,
                                ConfirmPickQty = item.ConfirmPickQty,
                                LotNo = item.LotNo,
                                PalletCode = item.PalletCode,
                                NewPalletCode = item.NewPalletCode,
                                PickStatus = (int)TranferMargetingStatus.New,
                                LocationID = item.LocationID,
                                Remark = item.Remark,
                                IsActive = true,
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                            };

                            trmtransferMarketingProductDetail.Add(_Details);

                            ProductionControlDetail _getpcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == _Details.PalletCode).Get().FirstOrDefault();//.SingleOrDefault();

                            ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(_getpcdt.PackingID);

                            if (updatepcdt.RemainQTY >= updatepcdt.ReserveQTY + _Details.PickQty)
                            {
                                updatepcdt.ReserveBaseQTY = (_Details.PickQty + updatepcdt.ReserveQTY) * updatepcdt.ConversionQty;
                                updatepcdt.ReserveQTY = _Details.PickQty + updatepcdt.ReserveQTY;
                                updatepcdt.UserModified = UserID;
                                updatepcdt.DateModified = DateTime.Now;
                                ProductionControlDetailService.Modify(updatepcdt);
                            }
                            else
                            {
                                throw new HILIException("MSG00039");
                            }
                        }
                    });

                    TRMTransferMarketingProduct _transProduct = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == entity.TRM_Product_ID).Get().FirstOrDefault();//.SingleOrDefault();
                    decimal? _sumPickQty = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == entity.TRM_Product_ID).Get().Sum(x => x.PickQty);

                    _transProduct.PickQty = _sumPickQty;
                    _transProduct.IsActive = true;
                    _transProduct.UserCreated = UserID;
                    _transProduct.UserModified = UserID;
                    _transProduct.DateCreated = DateTime.Now;
                    _transProduct.DateModified = DateTime.Now;

                    trmtransferMarketingProduct.Modify(_transProduct);

                    scope.Complete();
                    return true;
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

        public List<ProductModel> GetProductStockByCode(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.In_Progress,
                    (int)PackingStatusEnum.PutAway,
                    (int)PackingStatusEnum.Damage
                };

                ProductStatus productstatusid = ProductStatusService.Query().Filter(x => x.IsActive && x.Code == "NORMAL").Get().FirstOrDefault();

                palletNo = (string.IsNullOrEmpty(palletNo) ? "" : palletNo);
                productCode = (string.IsNullOrEmpty(productCode) ? "" : productCode);
                productName = (string.IsNullOrEmpty(productName) ? "" : productName);
                Lot = (string.IsNullOrEmpty(Lot) ? "" : Lot);
                Line = (string.IsNullOrEmpty(Line) ? "" : Line);
                mfgDate = (string.IsNullOrEmpty(mfgDate) ? "" : mfgDate);

                var result = (from _stockinfo in StockInfoService.Query().Filter(x => x.IsActive).Get()
                              join _sbl in StockBalanceService.Query().Filter(x => x.IsActive).Get()
                              on _stockinfo.StockInfoID equals _sbl.StockInfoID
                              join _pc in ProductionControlService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductID equals _pc.ProductID
                              join _pcdt in ProductionControlDetailService.Query().Filter(x => x.IsActive && packingStatus.Contains((int)x.PackingStatus)).Get()
                                on _pc.ControlID equals _pcdt.ControlID
                              join line in lineService.Query().Filter(x => x.IsActive).Get()
                                on _pc.LineID equals line.LineID
                              where _stockinfo.ProductID == _pc.ProductID &&
                                    _stockinfo.Lot == _pcdt.LotNo &&
                                    _stockinfo.StockUnitID == _pcdt.StockUnitID &&
                                    _stockinfo.BaseUnitID == _pcdt.BaseUnitID &&
                                    _stockinfo.ProductStatusID == _pcdt.ProductStatusID &&
                                    _stockinfo.ConversionQty == _pcdt.ConversionQty
                              join _warehouse in warehouseService.Query().Filter(x => x.IsActive).Get()
                                on _pcdt.WarehouseID equals _warehouse.WarehouseID
                              join _location in locationService.Query().Filter(x => x.IsActive).Get()
                                on _pcdt.LocationID equals _location.LocationID
                              where _warehouse.ReferenceCode == "111"//กรอง Warehouse Code M3
                              join _productowner in productOwnerService.Query().Get()
                                on _stockinfo.ProductOwnerID equals _productowner.ProductOwnerID
                              join _proudct in productService.Query().Get()
                                on _stockinfo.ProductID equals _proudct.ProductID
                              join _productcode in productCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                on _proudct.ProductID equals _productcode.ProductID
                              join _pc2 in productCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Commercial).Get()
                                on _productcode.ProductID equals _pc2.ProductID into pc2
                              from _productcode2 in pc2.DefaultIfEmpty()
                              join _pb in ProductBrandService.Query().Get()
                                on _proudct.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Get()
                                on _proudct.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _unit in productUnitService.Query().Get()
                                on _stockinfo.StockUnitID equals _unit.ProductUnitID
                              join _a in productUnitService.Query().Get()
                                on _stockinfo.ProductUnitPriceID equals _a.ProductUnitID into a
                              from _unit2 in a.DefaultIfEmpty()
                              join _status in ProductStatusService.Query().Get()
                                on _stockinfo.ProductStatusID equals _status.ProductStatusID
                              join _sub_status in ProductSubStatusService.Query().Get()
                                on _stockinfo.ProductSubStatusID equals _sub_status.ProductSubStatusID
                              where (_pcdt.RemainQTY != null ? _pcdt.RemainQTY.Value : 0) - (_pcdt.ReserveQTY != null ? _pcdt.ReserveQTY.Value : 0) > 0 && _stockinfo.ProductStatusID == productstatusid.ProductStatusID
                              select new
                              {
                                  ProductID = _stockinfo.ProductID,
                                  ProductGroupLevel3ID = _proudct.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = _proudct.ProductGroupLevel3?.Name,
                                  ProductCode = _productcode.Code,
                                  ProductName = _proudct.Name,
                                  PalletCode = _pcdt.PalletCode,
                                  LocationID = _pcdt.LocationID,
                                  Location = _location.Code,
                                  ProductLot = _pcdt.LotNo,
                                  LineCode = line.LineCode,
                                  MFGDate = _pcdt.MFGDate,
                                  Description = _proudct.Description,
                                  ProductBrandID = _prodcutbrand?.ProductBrandID,
                                  ProductBrandName = _prodcutbrand?.Name,
                                  ProductShapeID = _prodcutshap?.ProductShapeID,
                                  ProductShapeName = _prodcutshap?.Name,
                                  Age = _proudct.Age,
                                  IsActive = _proudct.IsActive,
                                  ProductUnitID = _unit.ProductUnitID,
                                  ProductUnitName = _unit.Name,
                                  PriceUnitId = (_unit2 != null ? _unit2.ProductUnitID : Guid.Empty),
                                  PriceUnitName = (_unit2 != null ? _unit2.Name : null),
                                  Price = (_stockinfo.Price != null ? _stockinfo.Price : 0),
                                  Quantity = (_pcdt.RemainQTY != null ? _pcdt.RemainQTY.Value : 0) - (_pcdt.ReserveQTY != null ? _pcdt.ReserveQTY.Value : 0),
                                  BaseQuantity = _pcdt.RemainBaseQTY,
                                  BaseUnitId = _stockinfo.BaseUnitID,
                                  ConversionQty = _pcdt.ConversionQty,
                                  ProductOwnerId = _stockinfo.ProductOwnerID,
                                  ProductOwnerName = _productowner.Name,
                                  ProductWidth = (decimal)_stockinfo.ProductWidth,
                                  ProductLength = (decimal)_stockinfo.ProductLength,
                                  ProductHeight = (decimal)_stockinfo.ProductHeight,
                                  ProductWeight = (decimal)_stockinfo.ProductWeight,
                                  PackageWeight = (decimal)_stockinfo.PackageWeight,
                                  ProductStatusId = _status.ProductStatusID,
                                  ProductStatusName = _status.Name,
                                  ProductSubStatusId = _sub_status.ProductSubStatusID,
                                  ProductSubStatusName = _sub_status.Name
                              } into g
                              group g by new
                              {
                                  g.ProductID,
                                  g.ProductGroupLevel3ID,
                                  g.ProductGroupLevel3Name,
                                  g.ProductCode,
                                  g.ProductName,
                                  g.PalletCode,
                                  g.LocationID,
                                  g.Location,
                                  g.ProductLot,
                                  g.LineCode,
                                  g.MFGDate,
                                  g.Description,
                                  g.ProductBrandID,
                                  g.ProductBrandName,
                                  g.ProductShapeID,
                                  g.ProductShapeName,
                                  g.Age,
                                  g.IsActive,
                                  g.ProductUnitID,
                                  g.ProductUnitName,
                                  g.PriceUnitId,
                                  g.PriceUnitName,
                                  g.Price,
                                  g.BaseUnitId,
                                  g.ConversionQty,
                                  g.ProductOwnerId,
                                  g.ProductOwnerName,
                                  g.ProductWidth,
                                  g.ProductLength,
                                  g.ProductHeight,
                                  g.ProductWeight,
                                  g.PackageWeight,
                                  g.ProductStatusId,
                                  g.ProductStatusName,
                                  g.ProductSubStatusId,
                                  g.ProductSubStatusName
                              } into g2
                              select new
                              {
                                  ProductID = g2.Key.ProductID,
                                  ProductGroupLevel3ID = g2.Key.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = g2.Key.ProductGroupLevel3Name,
                                  ProductCode = g2.Key.ProductCode,
                                  ProductName = g2.Key.ProductName,
                                  PalletCode = g2.Key.PalletCode,
                                  LocationID = g2.Key.LocationID,
                                  Location = g2.Key.Location,
                                  ProductLot = g2.Key.ProductLot,
                                  LineCode = g2.Key.LineCode,
                                  MFGDate = g2.Key.MFGDate,
                                  Description = g2.Key.Description,
                                  ProductBrandID = g2.Key.ProductBrandID,
                                  ProductBrandName = g2.Key.ProductBrandName,
                                  ProductShapeID = g2.Key.ProductShapeID,
                                  ProductShapeName = g2.Key.ProductShapeName,
                                  Age = g2.Key.Age,
                                  IsActive = g2.Key.IsActive,
                                  ProductUnitID = g2.Key.ProductUnitID,
                                  ProductUnitName = g2.Key.ProductUnitName,
                                  PriceUnitId = g2.Key.PriceUnitId,
                                  PriceUnitName = g2.Key.PriceUnitName,
                                  Price = g2.Key.Price,
                                  BaseUnitId = g2.Key.BaseUnitId,
                                  ConversionQty = g2.Key.ConversionQty,
                                  ProductOwnerId = g2.Key.ProductOwnerId,
                                  ProductOwnerName = g2.Key.ProductOwnerName,
                                  ProductWidth = g2.Key.ProductWidth,
                                  ProductLength = g2.Key.ProductLength,
                                  ProductHeight = g2.Key.ProductHeight,
                                  ProductWeight = g2.Key.ProductWeight,
                                  PackageWeight = g2.Key.PackageWeight,
                                  ProductStatusId = g2.Key.ProductStatusId,
                                  ProductStatusName = g2.Key.ProductStatusName,
                                  ProductSubStatusId = g2.Key.ProductSubStatusId,
                                  ProductSubStatusName = g2.Key.ProductSubStatusName,
                                  Quantity = g2.Sum(s => s.Quantity),
                                  BaseQuantity = g2.Sum(s => s.BaseQuantity)
                              });

                if (!string.IsNullOrWhiteSpace(palletNo))
                {
                    result = result.Where(x => x.PalletCode.Contains(palletNo));
                }

                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    result = result.Where(x => x.ProductCode.Contains(productCode));
                }

                if (!string.IsNullOrWhiteSpace(Lot))
                {
                    result = result.Where(x => x.ProductLot.Contains(Lot));
                }

                if (!string.IsNullOrWhiteSpace(productName))
                {
                    result = result.Where(x => x.ProductName.Contains(productName));
                }

                if (!string.IsNullOrWhiteSpace(Line))
                {
                    result = result.Where(x => x.LineCode.Contains(Line));
                }

                totalRecords = result.Count();


                List<ProductModel> productResult = result.Select(n => new ProductModel
                {
                    ProductID = n.ProductID,
                    ProductGroupLevel3ID = n.ProductGroupLevel3ID,
                    ProductGroupLevel3Name = n.ProductGroupLevel3Name,
                    ProductCode = n.ProductCode,
                    ProductName = n.ProductName,
                    PalletCode = n.PalletCode,
                    LocationID = n.LocationID,
                    Location = n.Location,
                    ProductLot = n.ProductLot,
                    LineCode = n.LineCode,
                    MFGDate = n.MFGDate,
                    Description = n.Description,
                    ProductBrandID = n.ProductBrandID,
                    ProductBrandName = n.ProductBrandName,
                    ProductShapeID = n.ProductShapeID,
                    ProductShapeName = n.ProductShapeName,
                    Age = n.Age,
                    IsActive = n.IsActive,
                    ProductUnitID = n.ProductUnitID,
                    ProductUnitName = n.ProductUnitName,
                    PriceUnitId = n.PriceUnitId,
                    PriceUnitName = n.PriceUnitName,
                    Price = n.Price,
                    BaseUnitId = n.BaseUnitId,
                    ConversionQty = n.ConversionQty,
                    ProductOwnerId = n.ProductOwnerId,
                    ProductOwnerName = n.ProductOwnerName,
                    ProductWidth = n.ProductWidth,
                    ProductLength = n.ProductLength,
                    ProductHeight = n.ProductHeight,
                    ProductWeight = n.ProductWeight,
                    PackageWeight = n.PackageWeight,
                    ProductStatusId = n.ProductStatusId,
                    ProductStatusName = n.ProductStatusName,
                    ProductSubStatusId = n.ProductSubStatusId,
                    ProductSubStatusName = n.ProductSubStatusName,
                    Quantity = n.Quantity,
                    BaseQuantity = n.BaseQuantity
                }).ToList();


                return productResult;
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
        public List<ProductModel> GetProductStock(string keyword, string orderno, string productstatuscode, string refcode, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                int[] packingStatus = new int[]
{
                    (int)PackingStatusEnum.In_Progress,
                    (int)PackingStatusEnum.PutAway,
                    (int)PackingStatusEnum.Damage
};
                ProductStatus productstatusid = ProductStatusService.Query().Filter(x => x.IsActive && x.Code == productstatuscode).Get().FirstOrDefault();
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from _stockinfo in StockInfoService.Query().Filter(x => x.IsActive).Get()
                              join _pc in ProductionControlService.Query().Filter(x => x.IsActive).Get()
                              on _stockinfo.ProductID equals _pc.ProductID
                              join _pcdt in ProductionControlDetailService.Query().Filter(x => x.IsActive && packingStatus.Contains((int)x.PackingStatus)).Get()
                              on _pc.ControlID equals _pcdt.ControlID
                              where _stockinfo.ProductID == _pc.ProductID &&
                                    _stockinfo.Lot == _pcdt.LotNo &&
                                    _stockinfo.StockUnitID == _pcdt.StockUnitID &&
                                    _stockinfo.BaseUnitID == _pcdt.BaseUnitID &&
                                    _stockinfo.ProductStatusID == _pcdt.ProductStatusID &&
                                    _stockinfo.ConversionQty == _pcdt.ConversionQty
                              join _warehouse in warehouseService.Query().Filter(x => x.IsActive).Get() on _pcdt.WarehouseID equals _warehouse.WarehouseID
                              where _warehouse.ReferenceCode == "111"//กรอง Warehouse Code M3
                              join _productowner in productOwnerService.Query().Get() on _stockinfo.ProductOwnerID equals _productowner.ProductOwnerID
                              join _proudct in productService.Query().Get() on _stockinfo.ProductID equals _proudct.ProductID
                              join _productcode in productCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get() on _proudct.ProductID equals _productcode.ProductID
                              join _pc2 in productCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Commercial).Get() on _productcode.ProductID equals _pc2.ProductID into pc2
                              from _productcode2 in pc2.DefaultIfEmpty()
                              join _pb in ProductBrandService.Query().Get() on _proudct.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Get() on _proudct.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _unit in productUnitService.Query().Get() on _stockinfo.StockUnitID equals _unit.ProductUnitID
                              join _a in productUnitService.Query().Get() on _stockinfo.ProductUnitPriceID equals _a.ProductUnitID into a
                              from _unit2 in a.DefaultIfEmpty()
                              join _status in ProductStatusService.Query().Get() on _stockinfo.ProductStatusID equals _status.ProductStatusID
                              join _sub_status in ProductSubStatusService.Query().Get() on _stockinfo.ProductSubStatusID equals _sub_status.ProductSubStatusID
                              where (_pcdt.RemainQTY != null ? _pcdt.RemainQTY.Value : 0) - (_pcdt.ReserveQTY != null ? _pcdt.ReserveQTY.Value : 0) > 0 &&
                                    _stockinfo.ProductStatusID == productstatusid.ProductStatusID &&
                                    (keyword == "" ? true : (_proudct.Name.Contains(keyword) || _productcode.Code.Contains(keyword) || _unit.Name.Contains(keyword) || (_productcode2 != null ? _productcode2.Code.Contains(keyword) : _productcode.Code.Contains(keyword))))
                              select new
                              {
                                  ProductID = _stockinfo.ProductID,
                                  ProductGroupLevel3ID = _proudct.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = _proudct.ProductGroupLevel3?.Name,
                                  ProductCode = _productcode.Code,
                                  ProductName = _proudct.Name,
                                  Description = _proudct.Description,
                                  ProductBrandID = _prodcutbrand?.ProductBrandID,
                                  ProductBrandName = _prodcutbrand?.Name,
                                  ProductShapeID = _prodcutshap?.ProductShapeID,
                                  ProductShapeName = _prodcutshap?.Name,
                                  Age = _proudct.Age,
                                  IsActive = _proudct.IsActive,
                                  ProductUnitID = _unit.ProductUnitID,
                                  ProductUnitName = _unit.Name,
                                  PriceUnitId = (_unit2 != null ? _unit2.ProductUnitID : Guid.Empty),
                                  PriceUnitName = (_unit2 != null ? _unit2.Name : null),
                                  Price = (_stockinfo.Price != null ? _stockinfo.Price : 0),
                                  Quantity = (_pcdt.RemainQTY != null ? _pcdt.RemainQTY.Value : 0) - (_pcdt.ReserveQTY != null ? _pcdt.ReserveQTY.Value : 0),
                                  BaseQuantity = _pcdt.RemainBaseQTY,
                                  BaseUnitId = _stockinfo.BaseUnitID,
                                  ConversionQty = _pcdt.ConversionQty,
                                  ProductOwnerId = _stockinfo.ProductOwnerID,
                                  ProductOwnerName = _productowner.Name,
                                  ProductWidth = (decimal)_stockinfo.ProductWidth,
                                  ProductLength = (decimal)_stockinfo.ProductLength,
                                  ProductHeight = (decimal)_stockinfo.ProductHeight,
                                  ProductWeight = (decimal)_stockinfo.ProductWeight,
                                  PackageWeight = (decimal)_stockinfo.PackageWeight,
                                  ProductStatusId = _status.ProductStatusID,
                                  ProductStatusName = _status.Name,
                                  ProductSubStatusId = _sub_status.ProductSubStatusID,
                                  ProductSubStatusName = _sub_status.Name,
                                  OrderType = _pc.OrderType,
                                  OrderNo = (string.IsNullOrEmpty(_pc.OrderNo) ? "" : _pc.OrderNo)
                              } into g
                              group g by new
                              {
                                  g.ProductID,
                                  g.ProductGroupLevel3ID,
                                  g.ProductGroupLevel3Name,
                                  g.ProductCode,
                                  g.ProductName,
                                  g.Description,
                                  g.ProductBrandID,
                                  g.ProductBrandName,
                                  g.ProductShapeID,
                                  g.ProductShapeName,
                                  g.Age,
                                  g.IsActive,
                                  g.ProductUnitID,
                                  g.ProductUnitName,
                                  g.PriceUnitId,
                                  g.PriceUnitName,
                                  g.Price,
                                  g.BaseUnitId,
                                  g.ConversionQty,
                                  g.ProductOwnerId,
                                  g.ProductOwnerName,
                                  g.ProductWidth,
                                  g.ProductLength,
                                  g.ProductHeight,
                                  g.ProductWeight,
                                  g.PackageWeight,
                                  g.ProductStatusId,
                                  g.ProductStatusName,
                                  g.ProductSubStatusId,
                                  g.ProductSubStatusName,
                                  g.OrderType,
                                  g.OrderNo

                              } into g2
                              select new
                              {
                                  ProductID = g2.Key.ProductID,
                                  ProductGroupLevel3ID = g2.Key.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = g2.Key.ProductGroupLevel3Name,
                                  ProductCode = g2.Key.ProductCode,
                                  ProductName = g2.Key.ProductName,
                                  Description = g2.Key.Description,
                                  ProductBrandID = g2.Key.ProductBrandID,
                                  ProductBrandName = g2.Key.ProductBrandName,
                                  ProductShapeID = g2.Key.ProductShapeID,
                                  ProductShapeName = g2.Key.ProductShapeName,
                                  Age = g2.Key.Age,
                                  IsActive = g2.Key.IsActive,
                                  ProductUnitID = g2.Key.ProductUnitID,
                                  ProductUnitName = g2.Key.ProductUnitName,
                                  PriceUnitId = g2.Key.PriceUnitId,
                                  PriceUnitName = g2.Key.PriceUnitName,
                                  Price = g2.Key.Price,
                                  BaseUnitId = g2.Key.BaseUnitId,
                                  ConversionQty = g2.Key.ConversionQty,
                                  ProductOwnerId = g2.Key.ProductOwnerId,
                                  ProductOwnerName = g2.Key.ProductOwnerName,
                                  ProductWidth = g2.Key.ProductWidth,
                                  ProductLength = g2.Key.ProductLength,
                                  ProductHeight = g2.Key.ProductHeight,
                                  ProductWeight = g2.Key.ProductWeight,
                                  PackageWeight = g2.Key.PackageWeight,
                                  ProductStatusId = g2.Key.ProductStatusId,
                                  ProductStatusName = g2.Key.ProductStatusName,
                                  ProductSubStatusId = g2.Key.ProductSubStatusId,
                                  ProductSubStatusName = g2.Key.ProductSubStatusName,
                                  OrderType = g2.Key.OrderType,
                                  OrderNo = g2.Key.OrderNo,
                                  Quantity = g2.Sum(s => s.Quantity),
                                  BaseQuantity = g2.Sum(s => s.BaseQuantity)
                              });

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.OrderNo).ThenBy(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
                    IsActive = n.IsActive,
                    ProductUnitID = n.ProductUnitID,
                    ProductUnitName = n.ProductUnitName,
                    PriceUnitId = n.PriceUnitId,
                    PriceUnitName = n.PriceUnitName,
                    Price = n.Price,
                    BaseUnitId = n.BaseUnitId,
                    ConversionQty = n.ConversionQty,
                    ProductOwnerId = n.ProductOwnerId,
                    ProductOwnerName = n.ProductOwnerName,
                    ProductWidth = n.ProductWidth,
                    ProductLength = n.ProductLength,
                    ProductHeight = n.ProductHeight,
                    ProductWeight = n.ProductWeight,
                    PackageWeight = n.PackageWeight,
                    ProductStatusId = n.ProductStatusId,
                    ProductStatusName = n.ProductStatusName,
                    ProductSubStatusId = n.ProductSubStatusId,
                    ProductSubStatusName = n.ProductSubStatusName,
                    Quantity = n.Quantity,
                    BaseQuantity = n.BaseQuantity,
                    OrderType = n.OrderType,
                    OrderNo = n.OrderNo
                }).ToList();


                return productResult;
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
        public List<TRMTransferMarketingProduct> OnAssignPick(List<TRMTransferMarketingProduct> entity)
        {
            if (entity == null)
            {
                throw new HILIException("MSG00045");
            }

            try
            {
                var temp_result = (from _stockinfo in StockInfoService.Where(x => x.IsActive)
                                   join _pc in ProductionControlService.Where(x => x.IsActive) on _stockinfo.ProductID equals _pc.ProductID
                                   join _pcdt in ProductionControlDetailService.Where(x => x.PackingStatus != PackingStatusEnum.Waiting_Receive) on _pc.ControlID equals _pcdt.ControlID
                                   join _warehouse in warehouseService.Where(x => x.IsActive) on _pcdt.WarehouseID equals _warehouse.WarehouseID
                                   where _warehouse.ReferenceCode == "111"
                                   && _stockinfo.ProductID == _pc.ProductID
                                   && _stockinfo.Lot == _pcdt.LotNo
                                   && _stockinfo.StockUnitID == _pcdt.StockUnitID
                                   && _stockinfo.BaseUnitID == _pcdt.BaseUnitID
                                   && _stockinfo.ConversionQty == _pcdt.ConversionQty
                                   && (_pcdt.RemainQTY.HasValue ? _pcdt.RemainQTY.Value : 0) - (_pcdt.ReserveQTY.HasValue ? _pcdt.ReserveQTY.Value : 0) > 0
                                   select new
                                   {
                                       _stockinfo = _stockinfo,
                                       _pcdt = _pcdt,
                                       _pc = _pc

                                   }).ToList();

                using (TransactionScope scope = new TransactionScope())
                {
                    List<TRMTransferMarketingProduct> _itemOverlist = new List<TRMTransferMarketingProduct>();
                    entity.ForEach(item =>
                    {
                        List<BookingDispatchModel> stock_result = (from t in temp_result
                                                                   where
                                                                   item.Product_ID == t._stockinfo.ProductID
                                                                   && item.TransferBaseUnitID == t._stockinfo.BaseUnitID
                                                                   && item.ProductStatusID == t._stockinfo.ProductStatusID
                                                                   && item.ProductStatusID == t._pcdt.ProductStatusID
                                                                   select new BookingDispatchModel
                                                                   {
                                                                       ProductId = t._stockinfo.ProductID,
                                                                       StockInfoId = t._stockinfo.StockInfoID,
                                                                       StockUnitId = t._stockinfo.StockUnitID,
                                                                       LocationId = t._pcdt.LocationID.HasValue ? t._pcdt.LocationID.Value : default(Guid),
                                                                       StockQuantity = (t._pcdt.RemainQTY.HasValue ? t._pcdt.RemainQTY.Value : 0) - (t._pcdt.ReserveQTY.HasValue ? t._pcdt.ReserveQTY.Value : 0),
                                                                       ManufacturingDate = t._stockinfo.ManufacturingDate.HasValue ? t._stockinfo.ManufacturingDate.Value : default(DateTime),
                                                                       ExpirationDate = t._stockinfo.ExpirationDate.HasValue ? t._stockinfo.ExpirationDate.Value : default(DateTime),
                                                                       BaseUnitId = t._pcdt.RemainBaseUnitID.HasValue ? t._pcdt.RemainBaseUnitID.Value : default(Guid),
                                                                       BaseQuantity = (t._pcdt.RemainBaseQTY.HasValue ? t._pcdt.RemainBaseQTY.Value : 0) - (t._pcdt.ReserveBaseQTY.HasValue ? t._pcdt.ReserveBaseQTY.Value : 0),
                                                                       ConversionQty = t._pcdt.ConversionQty.HasValue ? t._pcdt.ConversionQty.Value : 0,
                                                                       Lot = t._pcdt.LotNo,
                                                                       PackingId = t._pcdt.PackingID,
                                                                       DayDiff = t._stockinfo.ManufacturingDate.HasValue ? (int)(DateTime.Now - t._stockinfo.ManufacturingDate.Value).TotalDays : 0,
                                                                       OrderNo = t._pc.OrderNo
                                                                   }).ToList();

                        // Sum Qty ปัจจุบัน
                        decimal sumquantity = stock_result.Sum(x => x.StockQuantity);

                        if (item.PickQty > sumquantity)
                        {
                            TRMTransferMarketingProduct _itemOver = new TRMTransferMarketingProduct
                            {
                                ProductCode = item.ProductCode,
                                ProductName = item.ProductName,
                                PickQty = item.PickQty.Value,
                                RemainQTY = sumquantity,
                                OverQTY = item.PickQty.Value - sumquantity,
                                ProductUnitName = item.ProductUnitName
                            };
                            _itemOverlist.Add(_itemOver);
                        }
                        else
                        {

                            TRMTransferMarketingProduct _transProduct = trmtransferMarketingProduct.FirstOrDefault(x => x.IsActive.HasValue && x.IsActive.Value && x.TRM_Product_ID == item.TRM_Product_ID);
                            decimal? _sumPickQty = trmtransferMarketingProductDetail.Where(x => x.IsActive && x.TRM_Product_ID == item.TRM_Product_ID).Sum(x => x.PickQty);

                            if (_transProduct != null)
                            {
                                _transProduct.PickQty = _sumPickQty;
                                _transProduct.PickStatus = (int)TranferMargetingStatus.Assign;
                                _transProduct.IsActive = true;
                                _transProduct.UserCreated = UserID;
                                _transProduct.UserModified = UserID;
                                _transProduct.DateCreated = DateTime.Now;
                                trmtransferMarketingProduct.Modify(_transProduct);
                            }

                            //###
                            decimal _transferPickQty = item.PickQty.Value;

                            List<TRMTransferMarketingProductDetail> _detail = trmtransferMarketingProductDetail.Where(x => x.IsActive && x.TRM_Product_ID == item.TRM_Product_ID).ToList();

                            _detail.ToList().ForEach(itemDtail =>
                            {

                                if (_transferPickQty == 0)
                                {
                                    return;
                                }

                                TRMTransferMarketingProductDetail _transProductDetail = trmtransferMarketingProductDetail.FirstOrDefault(x => x.IsActive && x.TRM_Product_Detail_ID == itemDtail.TRM_Product_Detail_ID);

                                if (_transProductDetail != null)
                                {
                                    _transProductDetail.PickQty = itemDtail.PickQty;
                                    _transProductDetail.PickStatus = (int)TranferMargetingStatus.Assign;
                                    _transProductDetail.IsActive = true;
                                    _transProductDetail.UserCreated = UserID;
                                    _transProductDetail.UserModified = UserID;
                                    _transProductDetail.DateCreated = DateTime.Now;
                                    trmtransferMarketingProductDetail.Modify(_transProductDetail);
                                }

                                //var _getpcdt = unitofwork.Repository<ProductionControlDetail>();
                                //var updatepcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == itemDtail.PalletCode).Get().SingleOrDefault();

                                //updatepcdt.ReserveQTY = updatepcdt.ReserveQTY + itemDtail.PickQty;
                                //updatepcdt.ReserveBaseQTY = updatepcdt.ReserveBaseQTY + (itemDtail.PickQty * updatepcdt.ConversionQty);
                                //updatepcdt.UserModified = this.UserID;
                                //updatepcdt.DateModified = DateTime.Now;
                                //ProductionControlDetailService.Modify(updatepcdt);
                            });

                            if (_itemOverlist.Count == 0)
                            {
                                Guid? TrmId = entity.FirstOrDefault().TRM_ID;
                                TRMTransferMarketing _trans = FirstOrDefault(x => x.IsActive == true && x.TRM_ID == TrmId);

                                if (_trans != null)
                                {
                                    _trans.TransferStatus = (int)TranferMargetingStatus.Assign;
                                    _trans.IsActive = true;
                                    _trans.UserCreated = UserID;
                                    _trans.UserModified = UserID;
                                    _trans.DateCreated = DateTime.Now;
                                    base.Modify(_trans);
                                }
                            }


                        }

                    });
                    scope.Complete();
                    return _itemOverlist;
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
         
        public bool OnApprove(List<TRMTransferMarketingProduct> entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new HILIException("MSG00045");
                }
                var firstEntity = entity.FirstOrDefault();
                Guid? TrmId = firstEntity.TRM_ID;
                TRMTransferMarketing _trans = FirstOrDefault(x => x.IsActive == true && x.TRM_ID == TrmId);
                CultureInfo cultureinfo = new CultureInfo("en-US");
                TimeSpan timeStamp = DateTime.Now.TimeOfDay;

                using (TransactionScope scope = new TransactionScope())
                { 
                    if (_trans != null)
                    {
                        _trans.ApproveDate = firstEntity.ApproveDate;
                        _trans.TransferStatus = (int)TranferMargetingStatus.Approve;
                        _trans.IsActive = true;
                        _trans.UserModified = UserID;
                        _trans.DateModified = DateTime.Now;
                        base.Modify(_trans);                    
                   }

                    List<StockInOutModel> stockOutIn = new List<StockInOutModel>();

                    entity.ForEach(item =>
                    {
                        List<TRMTransferMarketingProductDetail> _detail = trmtransferMarketingProductDetail.Where(x => x.IsActive == true && x.TRM_Product_ID == item.TRM_Product_ID).ToList();
                        TRMTransferMarketingProduct _transProduct = trmtransferMarketingProduct.FirstOrDefault(x => x.IsActive == true && x.TRM_Product_ID == item.TRM_Product_ID);
                        int iSeq = 1; 
                       _detail.ForEach(TransDetail =>
                        {
                            string NewPalletCode = string.Empty;
                            var tmp = (from pc in ProductionControlService.Where(x => x.IsActive)
                                       join _pcdt in ProductionControlDetailService.Where(x => x.PackingStatus != PackingStatusEnum.Waiting_Receive && x.PalletCode == TransDetail.PalletCode) on pc.ControlID equals _pcdt.ControlID
                                       join l in lineService.Where(x => x.IsActive) on pc.LineID equals l.LineID
                                       where pc.ProductID == TransDetail.ProductID && _pcdt.PalletCode == TransDetail.PalletCode
                                       select new { pc, _pcdt, l , PCDetailCollection = pc.PCDetailCollection.ToList() }).ToList(); 
                            ProductionControlDetail PCDetail =(from t in tmp
                                                                select new ProductionControlDetail()
                                                                {
                                                                    ControlID = t.pc.ControlID,
                                                                    LotNo = t._pcdt.LotNo,
                                                                    Sequence = ((t.pc.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1),
                                                                    StockQuantity = TransDetail.ConfirmPickQty.GetValueOrDefault(),
                                                                    BaseQuantity = (TransDetail.ConfirmPickQty.GetValueOrDefault() * t.pc.ConversionQty.GetValueOrDefault()),
                                                                    ConversionQty = t.pc.ConversionQty.GetValueOrDefault(),
                                                                    StockUnitID = t.pc.StockUnitID,
                                                                    BaseUnitID = t.pc.BaseUnitID,
                                                                    RemainBaseUnitID = t._pcdt.RemainBaseUnitID,
                                                                    RemainStockUnitID = t._pcdt.RemainStockUnitID,
                                                                    ProductStatusID = t._pcdt.ProductStatusID,
                                                                    ProductSubStatusID = t._pcdt.ProductSubStatusID,
                                                                    MFGDate = t.pc.ProductionDate,
                                                                    MFGTimeStart = t.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                                                    MFGTimeEnd = timeStamp,
                                                                    PackingStatus = PackingStatusEnum.Transfer,
                                                                    WarehouseID = t.l.WarehouseID,
                                                                    ReceiveDetailID = t._pcdt.ReceiveDetailID,
                                                                    LocationID = null,
                                                                    ReserveQTY = 0,
                                                                    ReserveBaseQTY = 0,
                                                                    IsActive = true,
                                                                    UserCreated = UserID,
                                                                    DateCreated = DateTime.Now,
                                                                    UserModified = UserID,
                                                                    DateModified = DateTime.Now
                                                                }).FirstOrDefault(); 

                            ProductionControl pcline = ProductionControlService.FirstOrDefault(x => x.IsActive && x.ControlID == PCDetail.ControlID);
                            Line line = lineService.FirstOrDefault(x => x.IsActive && x.LineID == pcline.LineID);
                            var tmpPc = (from pc in ProductionControlService.Where(x => x.IsActive && x.ControlID == PCDetail.ControlID)
                                         join _pcdt in ProductionControlDetailService.Where(x => x.PackingStatus != PackingStatusEnum.Waiting_Receive) on pc.ControlID equals _pcdt.ControlID
                                         join l in lineService.Where(x => x.IsActive) on pc.LineID equals l.LineID
                                         where pc.ProductID == TransDetail.ProductID && _pcdt.PalletCode == TransDetail.PalletCode
                                         select new { pc, _pcdt, l, PCDetailCollection = pc.PCDetailCollection.ToList() }).ToList();
                            ProductionControlDetail pccontrol = (from t  in tmpPc
                                                                 select new ProductionControlDetail()
                                                                 {
                                                                     ControlID = t.pc.ControlID,
                                                                     PalletCode = t._pcdt.LotNo
                                                                                      + t.l.LineCode
                                                                                      + ((t.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1).ToString("000")
                                                                                      + timeStamp.ToString("hhmmss") + "M",
                                                                     LotNo = t._pcdt.LotNo,
                                                                     Sequence = ((t.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1),
                                                                 }).FirstOrDefault();

                            TRMTransferMarketingProductDetail _transProductDetail = trmtransferMarketingProductDetail.FirstOrDefault(x => x.IsActive == true && x.TRM_Product_Detail_ID == TransDetail.TRM_Product_Detail_ID);

                            if (_transProductDetail != null)
                            {
                                _transProductDetail.NewPalletCode = pccontrol.PalletCode;
                                _transProductDetail.PickQty = TransDetail.PickQty;
                                _transProductDetail.PickStatus = (int)TranferMargetingStatus.Approve;
                                _transProductDetail.IsActive = true;
                                _transProductDetail.UserCreated = UserID;
                                _transProductDetail.UserModified = UserID;
                                _transProductDetail.DateCreated = DateTime.Now;
                                trmtransferMarketingProductDetail.Modify(_transProductDetail);
                            }
                            ProductionControlDetail updatepcdt = ProductionControlDetailService.FirstOrDefault(x => x.IsActive && x.PalletCode == TransDetail.PalletCode); 
                            updatepcdt.RemainQTY -= TransDetail.ConfirmPickQty;
                            updatepcdt.RemainBaseQTY -= (TransDetail.ConfirmPickQty * PCDetail.ConversionQty);                          
                            updatepcdt.ReserveQTY -= TransDetail.ConfirmPickQty;                           
                            updatepcdt.ReserveBaseQTY -= (TransDetail.ConfirmPickQty * PCDetail.ConversionQty);

                            if (updatepcdt.ReserveQTY < 0)
                            {
                                updatepcdt.ReserveQTY = 0;
                            }
                            if (updatepcdt.RemainQTY < 0)
                            {
                                updatepcdt.RemainQTY = 0;
                            }
                            if (updatepcdt.RemainBaseQTY < 0)
                            {
                                updatepcdt.RemainBaseQTY = 0;
                            }
                            if (updatepcdt.ReserveBaseQTY < 0)
                            {
                                updatepcdt.ReserveBaseQTY = 0;
                            }
                            updatepcdt.UserModified = UserID;
                            updatepcdt.DateModified = DateTime.Now;
                            ProductionControlDetailService.Modify(updatepcdt);

                            Location location = locationService.FirstOrDefault(x => x.IsActive && x.LocationID == TransDetail.LocationID);
                            Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == TransDetail.PalletCode); 
                            Receive rcv = receiveService.FindByID(receiving.ReceiveID);
                            ReceiveDetail rcvDetail = receiveDetailService.FindByID(receiving.ReceiveDetailID);
                            LocationModel locationMarketing = (from w in warehouseService.Where(x => x.IsActive && x.ReferenceCode == "412")
                                                               join z in zoneService.Where(x => x.IsActive) on w.WarehouseID equals z.WarehouseID
                                                               join l in locationService.Where(x => x.IsActive && x.LocationType == LocationTypeEnum.Dummy) on z.ZoneID equals l.ZoneID
                                                               select new LocationModel
                                                               {
                                                                   WarehouseID = w.WarehouseID,
                                                                   ZoneID = z.ZoneID,
                                                                   LocationID = l.LocationID,
                                                                   Code = l.Code
                                                               }).FirstOrDefault();

                            #region Create ProductionControlDetail
                            ProductionControlDetail _pcl = new ProductionControlDetail
                            {
                                PackingID = Guid.NewGuid(),
                                ControlID = PCDetail.ControlID,
                                PalletCode = pccontrol.PalletCode, // New Pallet
                                Sequence = pccontrol.Sequence,
                                StockQuantity = 0,// TransDetail.ConfirmPickQty,
                                BaseQuantity = 0,//(TransDetail.ConfirmPickQty * PCDetail.ConversionQty),
                                ConversionQty = PCDetail.ConversionQty,
                                StockUnitID = TransDetail.PalletUnitID,
                                BaseUnitID = TransDetail.PalletBaseUnitID,
                                ProductStatusID = PCDetail.ProductStatusID,
                                ProductSubStatusID = PCDetail.ProductSubStatusID,
                                MFGDate = PCDetail.MFGDate,
                                MFGTimeStart = PCDetail.MFGTimeStart,
                                MFGTimeEnd = PCDetail.MFGTimeEnd,
                                LocationID = locationMarketing.LocationID,
                                WarehouseID = locationMarketing.WarehouseID,
                                PackingStatus = PackingStatusEnum.PutAway,
                                RemainStockUnitID = PCDetail.RemainStockUnitID,
                                RemainBaseUnitID = PCDetail.RemainBaseUnitID,
                                RemainQTY = TransDetail.ConfirmPickQty,
                                RemainBaseQTY = (TransDetail.ConfirmPickQty * PCDetail.ConversionQty),
                                LotNo = PCDetail.LotNo,
                                Remark = "Transfer to Marketing",
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                OptionalSuffix = line.LineType == "NP" ? iSeq.ToString() : "S01",
                                ReceiveDetailID = PCDetail.ReceiveDetailID,
                                ReserveBaseQTY = 0,
                                ReserveQTY = 0,
                                IsNormal = true,
                                IsNonProduction = true,
                                RefPalletCode = updatepcdt.PalletCode
                            };

                            Receiving reciving = new Receiving()
                            {
                                GRNCode = rcv.ReceiveCode + iSeq.ToString(),
                                ReceiveID = rcv.ReceiveID,
                                Sequence = iSeq,
                                ReceiveDetailID = receiving.ReceiveDetailID,
                                IsDraft = true,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                LocationID = locationMarketing.LocationID,
                                ProductID = receiving.ProductID,
                                Lot = PCDetail.LotNo,
                                ManufacturingDate = rcvDetail.ManufacturingDate,
                                ExpirationDate = rcvDetail.ExpirationDate,
                                Quantity = 0,//TransDetail.ConfirmPickQty.Value,
                                BaseQuantity = 0,// (TransDetail.ConfirmPickQty * PCDetail.ConversionQty).Value,
                                ConversionQty = _pcl.ConversionQty.GetValueOrDefault(),
                                StockUnitID = _pcl.StockUnitID.GetValueOrDefault(),
                                BaseUnitID = _pcl.BaseUnitID.GetValueOrDefault(),
                                ProductStatusID = PCDetail.ProductStatusID.GetValueOrDefault(),
                                ProductSubStatusID = PCDetail.ProductSubStatusID,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,
                                PalletCode = _pcl.PalletCode,
                                ProductOwnerID = rcv.ProductOwnerID,
                                SupplierID = rcv.SupplierID,

                                IsActive = true,
                                IsSentInterface = false,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };

                            receivingService.Add(reciving);

                            ProductionControlDetailService.Add(_pcl);
                            #endregion

                            stockOutIn.Add(
                                //  List<StockInOutModel> stockOutIn = new List<StockInOutModel>
                                //  {
                                new StockInOutModel
                                {
                                    ProductID = TransDetail.ProductID.GetValueOrDefault(),
                                    StockUnitID = PCDetail.StockUnitID.GetValueOrDefault(),
                                    BaseUnitID = PCDetail.BaseUnitID.GetValueOrDefault(),
                                    Lot = PCDetail.LotNo,
                                    ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                                    SupplierID = receiving.SupplierID.GetValueOrDefault(),
                                    ManufacturingDate = PCDetail.MFGDate.GetValueOrDefault(),
                                    ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                                    ProductWidth = receiving.ProductWidth,
                                    ProductLength = receiving.ProductLength,
                                    ProductHeight = receiving.ProductHeight,
                                    ProductWeight = receiving.ProductWeight,
                                    PackageWeight = receiving.PackageWeight,
                                    Price = receiving.Price,
                                    ProductUnitPriceID = receiving.ProductUnitPriceID,
                                    ProductStatusID = PCDetail.ProductStatusID.GetValueOrDefault(),
                                    ProductSubStatusID = PCDetail.ProductSubStatusID.GetValueOrDefault(),
                                    Quantity = TransDetail.ConfirmPickQty.GetValueOrDefault(),
                                    ConversionQty = PCDetail.ConversionQty.GetValueOrDefault(),
                                    PalletCode = TransDetail.PalletCode,
                                    NewPalletCode = _pcl.PalletCode,
                                    FromLocationCode = location.Code,
                                    LocationCode = locationMarketing.Code,
                                    DocumentCode = _trans.TRM_CODE,
                                    DocumentID = TransDetail.TRM_Product_Detail_ID.GetValueOrDefault(),
                                    StockTransTypeEnum = StockTransactionTypeEnum.Transfer412In,
                                    Remark = "Transfer to Marketing"
                                });
                          //  };
                            //stockService.TransferOutgoingAndIncomming(stockOutIn);
                            iSeq++;
                        });
                        
                        if (_transProduct != null)
                        {
                            _transProduct.PickQty = item.PickQty;
                            _transProduct.PickStatus = (int)TranferMargetingStatus.Approve;
                            _transProduct.IsActive = true;
                            _transProduct.UserModified = UserID;
                            _transProduct.DateModified = DateTime.Now;
                            trmtransferMarketingProduct.Modify(_transProduct);
                        }
                    });
                    stockService.TransferOutgoingAndIncomming(stockOutIn);

                    var tmpresult = (from _transfer in Where(x => x.IsActive == true && x.TransferStatus == (int)TranferMargetingStatus.Approve && x.TRM_CODE == _trans.TRM_CODE)
                                  join _transferProduct in trmtransferMarketingProduct.Where(x => x.IsActive == true) on _transfer.TRM_ID equals _transferProduct.TRM_ID
                                  join _transferDetail in trmtransferMarketingProductDetail.Where(x => x.IsActive) on _transferProduct.TRM_Product_ID equals _transferDetail.TRM_Product_ID
                                  join _stock_info in StockInfoService.Where(x => x.IsActive) on _transferDetail.ProductID equals _stock_info.ProductID
                                  join _stock_balance in StockBalanceService.Where(x => x.IsActive) on _stock_info.StockInfoID equals _stock_balance.StockInfoID
                                  join _pcdt in ProductionControlDetailService.Where(x => x.IsActive) on _transferDetail.PalletCode equals _pcdt.PalletCode
                                  join _user in ProductionControlDetailService.Where(x => x.IsActive) on _transferDetail.PalletCode equals _user.PalletCode
                                  where _stock_info.Lot == _pcdt.LotNo &&
                                       _stock_info.ProductStatusID == _transferProduct.ProductStatusID &&
                                       _stock_info.StockUnitID == _transferProduct.TransferUnitID &&
                                       _stock_info.BaseUnitID == _transferProduct.TransferBaseUnitID &&
                                       _stock_info.ProductID == _transferProduct.Product_ID &&
                                       _stock_info.Lot == _transferDetail.LotNo
                                  select new { _transfer, _transferProduct, _transferDetail, _pcdt, _stock_info, _stock_balance }).ToList();

                    var result = (from t in tmpresult
                                  select new
                                  {
                                      TRMID = t._transfer.TRM_ID,
                                      TRMCODE = t._transfer.TRM_CODE,
                                      TRMProductID = t._transferProduct.TRM_Product_ID,
                                      TRMProductDetailID = t._transferDetail.TRM_Product_Detail_ID,
                                      BaseQuantity = t._transferDetail.ConfirmPickQty * t._pcdt.ConversionQty,
                                      BaseUnitId = t._transferDetail.PalletBaseUnitID,
                                      PalletCode = t._transferDetail.PalletCode,
                                      StockQuantity = t._transferDetail.ConfirmPickQty,
                                      PickStockQty = t._transferDetail.PickQty,
                                      StockUnitId = t._transferDetail.PalletUnitID,
                                      ProductId = t._transferDetail.ProductID,
                                      ConversionQty = t._transferDetail.ConversionQty,
                                      ProductLot = t._stock_info.Lot,
                                      ManufacturingDate = t._stock_info.ManufacturingDate,
                                      ExpirationDate = t._stock_info.ExpirationDate,
                                      DocumentCode = t._transfer.TRM_CODE,
                                      StockBalanceId = t._stock_balance.StockBalanceID,
                                      StockInfoId = t._stock_info.StockInfoID,
                                      LocationId = t._transferDetail.LocationID,
                                      DocumentId = t._transferProduct.TRM_Product_ID
                                  }
                           ).ToList();

                    //##### Interface Log ######
                    List<InterfaceDispatchModel> itfmodel = (from s in result
                                                             select new
                                                             {
                                                                 ProductId = s.ProductId,
                                                                 TRMCODE = s.TRMCODE,
                                                                 Quantity = s.StockQuantity.Value,
                                                                 UnitId = s.StockUnitId.Value,
                                                                 BaseQuantity = s.BaseQuantity.Value,
                                                                 BaseUnitId = s.BaseUnitId.Value,
                                                                 Lot = s.ProductLot
                                                             } into g
                                                             group g by new
                                                             {
                                                                 g.ProductId,
                                                                 g.TRMCODE,
                                                                 g.UnitId,
                                                                 g.BaseUnitId,
                                                                 g.Lot
                                                             } into x
                                                             select new InterfaceDispatchModel
                                                             {
                                                                 ProductId = x.Key.ProductId.Value,
                                                                 DispatchCode = x.Key.TRMCODE,
                                                                 Quantity = x.Sum(s => s.Quantity),
                                                                 UnitId = x.Key.UnitId,
                                                                 BaseQuantity = x.Sum(s => s.BaseQuantity),
                                                                 BaseUnitId = x.Key.BaseUnitId,
                                                                 Lot = x.Key.Lot
                                                             }).ToList();
                    AddInterfaceDispatch(itfmodel);
                    scope.Complete();                 
                    //#####  Interface Log ######                   
                    return true;
                }
            }
            catch (HILIException ex)
            {
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

        #region Interface Log Dispatch
        public void AddInterfaceDispatch(List<InterfaceDispatchModel> data)
        {
            if (data == null)
            {
                throw new HILIException("MSG00006");
            }
            CultureInfo cultureinfo = new CultureInfo("en-US");
            try
            {

                var tmp = (from _data in data
                           join _tfm in Where(x => x.IsActive == true) on _data.DispatchCode equals _tfm.TRM_CODE
                           join _pdc in productCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _data.ProductId equals _pdc.ProductID
                           join _pd in productService.Where(x => x.IsActive == true) on _data.ProductId equals _pd.ProductID
                           join _unit in productUnitService.Where(x => x.IsActive == true) on _data.UnitId equals _unit.ProductUnitID
                           join _unitbase in productUnitService.Where(x => x.IsActive == true) on _data.BaseUnitId equals _unitbase.ProductUnitID
                           where _tfm.TransferStatus == (int)TranferMargetingStatus.Approve
                           select new { _data, _tfm, _pdc, _pd, _unit, _unitbase, }).ToList();
                List<itf_temp_in_dispatch_log> ret = (from t in tmp
                                                      select new itf_temp_in_dispatch_log
                                                      {
                                                          TransactionId = Guid.NewGuid(),
                                                          Cono = 101,
                                                          Orno = "",
                                                          Cuno = "",
                                                          Faci = "F11",
                                                          Whlo = "111",
                                                          Itno = t._pdc.Code,
                                                          Itds = t._pd.Name,
                                                          Dwdt = t._tfm.ApproveDate.Value.ToString("yyyyMMdd", new CultureInfo("en-US")),//20171116
                                                          Orqt = t._data.Quantity,
                                                          Alun = t._unit.Name,
                                                          Sapr = 0,
                                                          Spun = t._unit.Name,
                                                          Dia1 = 0,
                                                          Dia2 = 0,
                                                          Dia3 = 0,
                                                          Dia4 = 0,
                                                          Dia5 = 0,
                                                          Dia6 = 0,
                                                          Dip1 = 0,
                                                          Dip2 = 0,
                                                          Dip3 = 0,
                                                          Dip4 = 0,
                                                          Dip5 = 0,
                                                          Dip6 = 0,
                                                          Cuor = t._data.PONo,
                                                          Dwdz = t._tfm.ApproveDate.Value.ToString("yyyyMMdd", new CultureInfo("en-US")),//20171116
                                                          Dwhz = DateTime.Now.ToString("HHmm"),//1230
                                                          Rscd = "",
                                                          Bano = t._data.Lot,
                                                          Whsl = "A01",
                                                          Ortp = "",
                                                          Rldt = "0",
                                                          Modl = "",
                                                          Tedl = "",
                                                          Yref = "",
                                                          Tepy = "",
                                                          Pyno = "",
                                                          Adid = "",
                                                          Oref = "",
                                                          Cudt = "",//20171116
                                                          Ordt = "",//20171116
                                                          Rldz = "",//20171116
                                                          Rlhz = DateTime.Now.ToString("HHmm"),//1230
                                                          Ctst = "",
                                                          Emsg = "",
                                                          Wmsorn = t._data.DispatchCode,
                                                          Gdate = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("en-US")),//20171116
                                                          Gtime = DateTime.Now.ToString("HHmm"),//1230
                                                          Gstt = "S",
                                                          Rnom3 = "",
                                                          Fdate = "",
                                                          Ftime = "",
                                                          Fstt = "",
                                                          SyncUnsuccessNo = 0,
                                                          SyncFlag = "",
                                                          SyncDate = "",
                                                          ProductSystemCode = t._pdc.Code,
                                                          ProductNameFull = t._pd.Name,
                                                          DispatchDateDelivery = null,
                                                          DispatchDetailProductQuantity = t._data.Quantity,
                                                          ProductPriceUomId = null,
                                                          DispatchDetailProductPrice = 0,
                                                          DocumentNo = t._data.PONo,
                                                          DispatchDateOrder = null,
                                                          SubCustCode = "",
                                                          DispatchCode = t._data.DispatchCode,
                                                          Gedt = DateTime.Now.ToString("yyyyMMdd", new CultureInfo("en-US")),//20171116
                                                          Getm = DateTime.Now.ToString("HHmm"),//1230
                                                          Twhl = "412",
                                                          Twsl = "O10",
                                                          Dlqt = t._data.BaseQuantity,
                                                          Alqt = t._data.BaseQuantity,
                                                          Ridn = "",
                                                          Ridl = 0,
                                                          Ridx = 0,
                                                          Ridi = 0,
                                                          Plsx = 0,
                                                          Dlix = 0,
                                                          Trtp = "T01",
                                                          Tofp = "",
                                                          Resp = UserAccountsService.Query().Filter(x => x.IsActive && x.UserID == t._tfm.UserCreated).Get().FirstOrDefault().UserName,
                                                          Rpdt = t._tfm.ApproveDate.Value.ToString("yyyyMMdd", new CultureInfo("en-US")),//20171116
                                                          Rptm = t._tfm.ApproveDate.Value.ToString("HHmm"),//1230
                                                          Ituom = t._unitbase.Name,
                                                          Dispatchtypeid = null,
                                                          IsSentInterface = false
                                                      }).ToList();
                IEnumerable<itf_temp_in_dispatch_log> result = itf_temp_in_dispatch_logService.AddRange(ret);
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

        public bool RemoveTransfer(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TRMTransferMarketing _trans = Query().Filter(x => x.IsActive == true &&
                                                x.TRM_ID == id)
                                        .Get().FirstOrDefault();

                    if (_trans == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _trans.TransferStatus = (int)TranferMargetingStatus.Cancel;
                    _trans.DateModified = DateTime.Now;
                    _trans.UserModified = UserID;
                    base.Modify(_trans);

                    List<TRMTransferMarketingProduct> _transferProduct = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true &&
                                                                                          x.TRM_ID == _trans.TRM_ID)
                                                                                          .Get().ToList();
                    //if (_transferProduct.Count() == 0)
                    //    throw new HILIException("MSG00006");

                    _transferProduct.ForEach(items =>
                    {
                        items.PickStatus = (int)TranferMargetingStatus.Cancel;
                        items.DateModified = DateTime.Now;
                        items.UserModified = UserID;
                        trmtransferMarketingProduct.Modify(items);

                        List<TRMTransferMarketingProductDetail> _transferProductDetail = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true &&
                                                                                          x.TRM_Product_ID == items.TRM_Product_ID)
                                                                                          .Get().ToList();
                        if (_transferProductDetail.Count() > 0)
                        {
                            _transferProductDetail.ForEach(detail =>
                            {
                                detail.PickStatus = (int)TranferMargetingStatus.Cancel;
                                detail.DateModified = DateTime.Now;
                                detail.UserModified = UserID;
                                trmtransferMarketingProductDetail.Modify(detail);

                                //var _getpcdt = unitofwork.Repository<ProductionControlDetail>();
                                //var updatepcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == detail.PalletCode).Get().SingleOrDefault();

                                //updatepcdt.ReserveQTY = (updatepcdt.ReserveQTY - detail.PickQty >= 0 ? updatepcdt.ReserveQTY - detail.PickQty : 0);
                                //updatepcdt.ReserveBaseQTY = (updatepcdt.ReserveBaseQTY - (detail.PickQty * updatepcdt.ConversionQty) >= 0 ? updatepcdt.ReserveBaseQTY - (detail.PickQty * updatepcdt.ConversionQty) : 0);
                                //updatepcdt.UserModified = this.UserID;
                                //updatepcdt.DateModified = DateTime.Now;
                                //ProductionControlDetailService.Modify(updatepcdt);
                            });
                        }
                    });

                    scope.Complete();
                    return true;
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

        public bool RemoveTransferProduct(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    TRMTransferMarketingProduct _transferProduct = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true &&
                                                                                          x.TRM_Product_ID == id)
                                                                                          .Get().SingleOrDefault();
                    if (_transferProduct == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _transferProduct.IsActive = false;
                    _transferProduct.DateModified = DateTime.Now;
                    _transferProduct.UserModified = UserID;
                    trmtransferMarketingProduct.Modify(_transferProduct);

                    List<TRMTransferMarketingProductDetail> _transferProductDetail = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true &&
                                                                                                       x.TRM_Product_ID == _transferProduct.TRM_Product_ID)
                                                                                                       .Get().ToList();
                    if (_transferProductDetail.Count() > 0)
                    {
                        _transferProductDetail.ForEach(detail =>
                        {
                            detail.IsActive = false;
                            detail.DateModified = DateTime.Now;
                            detail.UserModified = UserID;
                            trmtransferMarketingProductDetail.Modify(detail);

                            ProductionControlDetail _getpcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == detail.PalletCode).Get().SingleOrDefault();

                            ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(_getpcdt.PackingID);

                            decimal? qty = updatepcdt.ReserveQTY - detail.PickQty;

                            if (qty < 0)
                            {
                                qty = 0;
                            }

                            updatepcdt.ReserveBaseQTY = qty * updatepcdt.ConversionQty;
                            updatepcdt.ReserveQTY = qty;
                            updatepcdt.UserModified = UserID;
                            updatepcdt.DateModified = DateTime.Now;
                            ProductionControlDetailService.Modify(updatepcdt);
                        });
                    }

                    scope.Complete();
                    return true;
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

        public bool RemoveTransferProductDetail(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    TRMTransferMarketingProductDetail detail = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true &&
                                                                                       x.TRM_Product_Detail_ID == id)
                                                                                       .Get().SingleOrDefault();
                    if (detail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    detail.IsActive = false;
                    detail.DateModified = DateTime.Now;
                    detail.UserModified = UserID;
                    trmtransferMarketingProductDetail.Modify(detail);


                    ProductionControlDetail _getpcdt = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == detail.PalletCode).Get().SingleOrDefault();

                    ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(_getpcdt.PackingID);

                    decimal? qty = updatepcdt.ReserveQTY - detail.PickQty;

                    if (qty < 0)
                    {
                        qty = 0;
                    }

                    updatepcdt.ReserveBaseQTY = qty * updatepcdt.ConversionQty;
                    updatepcdt.ReserveQTY = qty;
                    updatepcdt.UserModified = UserID;
                    updatepcdt.DateModified = DateTime.Now;
                    ProductionControlDetailService.Modify(updatepcdt);

                    scope.Complete();
                    return true;
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

        #region [Handheld]
        public List<TRMTransferMarketingProduct> GetTransferMargetingProductHandheld(string keyword)
        {
            try
            {
                int[] Status = new int[]
                {
                    (int)TranferMargetingStatus.Assign,
                    (int)TranferMargetingStatus.Confirm
                 };

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<TRMTransferMarketingProduct> details = (from _trans in Query().Filter(x => x.IsActive == true && Status.Contains((int)x.TransferStatus) && (keyword != "" ? x.TRM_CODE.Contains(keyword) : true)).Get()
                                                                    join _transproduct in trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _trans.TRM_ID equals _transproduct.TRM_ID
                                                                    join _product in productService.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _transproduct.Product_ID equals _product.ProductID
                                                                    join _productCode in productCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                                      on _product.ProductID equals _productCode.ProductID
                                                                    join _productUnit in productUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                                      on _transproduct.TransferUnitID equals _productUnit.ProductUnitID
                                                                    select new TRMTransferMarketingProduct
                                                                    {
                                                                        TRM_Product_ID = _transproduct.TRM_Product_ID,
                                                                        TRM_ID = _trans.TRM_ID,
                                                                        TransferStatus = _trans.TransferStatus,
                                                                        Product_ID = _transproduct.Product_ID,
                                                                        TransferQty = _transproduct.TransferQty,
                                                                        TotalPickQty = _transproduct.TotalPickQty,
                                                                        TransferUnitID = _transproduct.TransferUnitID,
                                                                        TransferBaseUnitID = _transproduct.TransferBaseUnitID,
                                                                        TransferBaseQty = _transproduct.TransferBaseQty,
                                                                        PickQty = _transproduct.PickQty,
                                                                        ConfirmQty = _transproduct.ConfirmQty,
                                                                        PickStatus = _transproduct.PickStatus,
                                                                        ProductName = _product.Name,
                                                                        ProductCode = _productCode.Code,
                                                                        ProductUnitName = _productUnit.Name,
                                                                        Remark = _transproduct.Remark,
                                                                        DateCreated = _transproduct.DateCreated,
                                                                        DateModified = _transproduct.DateModified,
                                                                        UserCreated = _transproduct.UserCreated,
                                                                        UserModified = _transproduct.UserModified,
                                                                    });


                return details.ToList();

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

        public TRMTransferMarketingProductDetail GetTransferMargetingDetailByPalletHandheld(Guid TrmProductDetailID, string Pallet, string Location)
        {
            try
            {
                Pallet = (string.IsNullOrEmpty(Pallet) ? "" : Pallet);
                Location = (string.IsNullOrEmpty(Location) ? "" : Location);

                IEnumerable<TRMTransferMarketingProductDetail> details = (from _transproduct in trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == TrmProductDetailID).Get()
                                                                          join _transproductDetail in trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true).Get()
                                                                            on _transproduct.TRM_Product_ID equals _transproductDetail.TRM_Product_ID
                                                                          join tmpsumTransDetail in
                                                                          (
                                                                            from _trmd in trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.PickStatus == (int)TranferMargetingStatus.Confirm).Get().DefaultIfEmpty()
                                                                            group new { _trmd } by new { _trmd?.TRM_Product_ID }
                                                                            into _sumQty
                                                                            select new
                                                                            {
                                                                                TRM_Product_ID = _sumQty.Key.TRM_Product_ID,
                                                                                SumPickConfQty = _sumQty.Sum(x => x._trmd?.ConfirmPickQty)
                                                                            }
                                                                          ) on _transproduct.TRM_Product_ID equals tmpsumTransDetail?.TRM_Product_ID into _tmpsumTransDetail
                                                                          from sumTransDetail in _tmpsumTransDetail.DefaultIfEmpty()
                                                                          join _location in locationService.Query().Filter(x => x.IsActive == true).Get()
                                                                            on _transproductDetail.LocationID equals _location.LocationID
                                                                          join _product in productService.Query().Filter(x => x.IsActive == true).Get()
                                                                            on _transproductDetail.ProductID equals _product.ProductID
                                                                          join _productCode in productCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                                            on _product.ProductID equals _productCode.ProductID
                                                                          join _productUnit in productUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                                            on _transproductDetail.PalletUnitID equals _productUnit.ProductUnitID
                                                                          where (Pallet != "" ? _transproductDetail.PalletCode.Contains(Pallet) : true) &&
                                                                                (Location != "" ? _location.Code.Contains(Location) : true)
                                                                          select new TRMTransferMarketingProductDetail
                                                                          {
                                                                              TRM_ID = _transproduct.TRM_ID,
                                                                              TRM_Product_Detail_ID = _transproductDetail.TRM_Product_Detail_ID,
                                                                              TRM_Product_ID = _transproductDetail.TRM_Product_ID,
                                                                              ProductID = _transproductDetail.ProductID,
                                                                              PalletQty = _transproductDetail.PalletQty,
                                                                              PalletUnitID = _transproductDetail.PalletUnitID,
                                                                              PalletBaseUnitID = _transproductDetail.PalletBaseUnitID,
                                                                              PalletBaseQty = _transproductDetail.PalletBaseQty,
                                                                              PickQty = _transproductDetail.PickQty,
                                                                              OrderPickQty = _transproduct.PickQty,
                                                                              ConfirmPickQty = _transproductDetail.ConfirmPickQty,
                                                                              LotNo = _transproductDetail.LotNo,
                                                                              PalletCode = _transproductDetail.PalletCode,
                                                                              NewPalletCode = _transproductDetail.NewPalletCode,
                                                                              PickStatus = _transproductDetail.PickStatus,
                                                                              LocationID = _transproductDetail.LocationID,
                                                                              ProductName = _product.Name,
                                                                              ProductCode = _productCode.Code,
                                                                              ProductUnitName = _productUnit.Name,
                                                                              Remark = _transproductDetail.Remark,
                                                                              SumPickConfQty = sumTransDetail?.SumPickConfQty,
                                                                              DateCreated = _transproductDetail.DateCreated,
                                                                              DateModified = _transproductDetail.DateModified,
                                                                              UserCreated = _transproductDetail.UserCreated,
                                                                              UserModified = _transproductDetail.UserModified,
                                                                          });

                return details.SingleOrDefault();

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

        public bool ConfirmPickTransfer(Guid TrmProductID, string Pallet, string Location, decimal ConfirmQty, decimal sumPickQTY)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (Pallet == null || ConfirmQty == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Location _location = locationService.Query().Filter(x => x.IsActive == true && x.Code == Location).Get().SingleOrDefault();
                    if (_location == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    TRMTransferMarketingProductDetail _transferProductDetail = trmtransferMarketingProductDetail.Query()
                                                .Filter(x => x.IsActive == true &&
                                                             x.TRM_Product_ID == TrmProductID &&
                                                             x.PalletCode == Pallet &&
                                                             x.LocationID == _location.LocationID)
                                                .Get().SingleOrDefault();

                    _transferProductDetail.ConfirmPickQty = ConfirmQty;
                    _transferProductDetail.PickStatus = (int)TranferMargetingStatus.Confirm;
                    _transferProductDetail.IsActive = true;
                    _transferProductDetail.UserModified = UserID;
                    _transferProductDetail.DateModified = DateTime.Now;
                    trmtransferMarketingProductDetail.Modify(_transferProductDetail);

                    decimal? sumPickQty = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == TrmProductID).Get().Sum(x => x.PickQty);

                    if (sumPickQty == (sumPickQTY + ConfirmQty))
                    {
                        TRMTransferMarketingProduct _modify = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == TrmProductID && x.PickStatus == (int)TranferMargetingStatus.Assign).Get().SingleOrDefault();
                        if (_modify == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        decimal? _sumPickQty = trmtransferMarketingProductDetail.Query().Filter(x => x.IsActive == true && x.TRM_Product_ID == TrmProductID).Get().Sum(x => x.PickQty);

                        _modify.ConfirmQty = _sumPickQty;
                        _modify.PickStatus = (int)TranferMargetingStatus.Confirm;
                        _modify.UserModified = UserID;
                        _modify.DateModified = DateTime.Now;
                        trmtransferMarketingProduct.Modify(_modify);

                        int _count = trmtransferMarketingProduct.Query().Filter(x => x.IsActive == true && x.TRM_ID == _modify.TRM_ID && x.PickStatus == (int)TranferMargetingStatus.Assign).Get().Count();
                        if (_count == 0)
                        {
                            TRMTransferMarketing _modifyTrans = Query().Filter(x => x.IsActive == true && x.TRM_ID == _modify.TRM_ID && x.TransferStatus == (int)TranferMargetingStatus.Assign).Get().SingleOrDefault();
                            if (_modifyTrans == null)
                            {
                                throw new HILIException("MSG00006");
                            }

                            _modifyTrans.TransferStatus = (int)TranferMargetingStatus.Confirm;
                            _modifyTrans.UserModified = UserID;
                            _modifyTrans.DateModified = DateTime.Now;
                            base.Modify(_modifyTrans);
                        }
                    }

                    scope.Complete();
                    return true;
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
        #endregion [Handheld]

    }
}
