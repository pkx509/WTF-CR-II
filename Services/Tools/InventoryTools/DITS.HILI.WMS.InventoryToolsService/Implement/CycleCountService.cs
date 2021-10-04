using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class CycleCountService : Repository<CycleCount>, ICycleCountService
    {
        #region Property 
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> packingDetailService;
        private readonly IRepository<CycleCountDetail> cycleCountDetailService;
        private readonly IRepository<CycleCountAssign> cycleCountAssignService;
        private readonly IRepository<CycleCountPrefix> cycleCountPrefixService;
        private readonly IRepository<Product> ProductService;
        private readonly IRepository<ProductCodes> ProductCodeService;
        private readonly IRepository<ProductStatus> ProductStatusService;
        private readonly IRepository<ProductSubStatus> ProductSubStatusService;
        private readonly IRepository<ProductUnit> ProductUnitService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<PhysicalZone> physicalZoneService;
        private readonly IRepository<Zone> ZoneService;
        private readonly IRepository<Warehouse> WarehouseService;
        private readonly IRepository<StockInfo> StockInfoService;
        private readonly IRepository<StockBalance> StockBalanceService;
        private readonly IRepository<StockTransaction> StockTransactionService;
        private readonly IRepository<ProductOwner> ProductOwnerService;
        private readonly IRepository<ProductBrand> ProductBrandService;
        private readonly IRepository<ProductShape> ProductShapeService;
        #endregion

        #region Constructor

        public CycleCountService(IUnitOfWork dbContext,
                                    IRepository<ProductionControl> _productionControl,
                                    IRepository<ProductionControlDetail> _packingDetailService,
                                    IRepository<CycleCountDetail> _cycleCountDetailService,
                                    IRepository<CycleCountAssign> _cycleCountAssignService,
                                    IRepository<Product> _product,
                                    IRepository<ProductCodes> _productCode,
                                    IRepository<ProductStatus> _ProductStatus,
                                    IRepository<ProductSubStatus> _ProductSubStatus,
                                    IRepository<ProductUnit> _productUnit,
                                        IRepository<StockInfo> _StockInfo,
                                        IRepository<StockBalance> _StockBalance,
                                        IRepository<StockTransaction> _StockTransaction,
                                        IRepository<ProductBrand> _ProductBrand,
                                        IRepository<ProductShape> _ProductShape,
                                        IRepository<ProductOwner> _ProductOwner,
                                        IRepository<CycleCountPrefix> _cycleCountPrefix,
                                    IRepository<TruckType> _truckType,
                                    IRepository<DockConfig> _dockConfig,
                                    IRepository<Location> _location,
                                    IRepository<PhysicalZone> _physicalZone,
                                    IRepository<Zone> _Zone,
                                    IRepository<Warehouse> _Warehouse)
            : base(dbContext)
        {
            productionControlService = _productionControl;
            packingDetailService = _packingDetailService;
            ProductService = _product;
            ProductCodeService = _productCode;
            ProductStatusService = _ProductStatus;
            ProductSubStatusService = _ProductSubStatus;
            StockInfoService = _StockInfo;
            StockBalanceService = _StockBalance;
            StockTransactionService = _StockTransaction;
            ProductBrandService = _ProductBrand;
            ProductShapeService = _ProductShape;
            ProductOwnerService = _ProductOwner;
            ProductUnitService = _productUnit;
            cycleCountPrefixService = _cycleCountPrefix;
            cycleCountDetailService = _cycleCountDetailService;
            cycleCountAssignService = _cycleCountAssignService;
            locationService = _location;
            physicalZoneService = _physicalZone;
            ZoneService = _Zone;
            WarehouseService = _Warehouse;

        }

        #endregion

        public CycleCount Get(Guid id)
        {
            try
            {
                CycleCount _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.CycleCountID == id)
                                  .Include(x => x.CycCountDetail)
                                  .Get().FirstOrDefault();

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
        public List<CycleCountModel> GetAll(int state, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

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

                IEnumerable<CycleCountModel> result = (from _productionControl in productionControlService.Query().Filter(x => x.IsActive == true).Get()
                                                       join _packingDetail in packingDetailService.Query().Filter(x => x.IsActive == true && packingStatus.Contains((int)x.PackingStatus)).Get()
                                                         on _productionControl.ControlID equals _packingDetail.ControlID
                                                       join _location in locationService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _packingDetail.LocationID equals _location.LocationID
                                                       join _ccount in Query().Filter(x => x.IsActive == true && x.CycleCountStatus == state).Get()
                                                         on new { CC1 = _packingDetail.WarehouseID, CC2 = _location.ZoneID } equals new { CC1 = _ccount.WarehounseID, CC2 = _ccount.ZoneID.Value }
                                                       join _zone in ZoneService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _ccount.ZoneID equals _zone.ZoneID
                                                       join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _zone.WarehouseID equals _warehouse.WarehouseID
                                                       where (_ccount.CycleCountCode.Contains(keyword)) //&& (ShippingID != null ? _rTruck.ShippingID == ShippingID.Value : true))
                                                       group new { _productionControl, _packingDetail, _location, _ccount, _zone, _warehouse }
                                                          by new
                                                          {
                                                              ControlID = _productionControl.ControlID,
                                                              ProductionDate = _productionControl.ProductionDate,
                                                              Lot = _packingDetail.LotNo,
                                                              LineID = _productionControl.LineID,
                                                              ProductID = _productionControl.ProductID,
                                                              ProductUnitID = _productionControl.ProductUnitID,
                                                              OrderType = _productionControl.OrderType,
                                                              OrderNo = _productionControl.OrderNo,
                                                              PalletCode = _packingDetail.PalletCode,
                                                              Sequence = _packingDetail.Sequence,
                                                              StockQuantity = _packingDetail.StockQuantity,
                                                              BaseQuantity = _packingDetail.BaseQuantity,
                                                              ConversionQty = _packingDetail.ConversionQty,
                                                              ProductStatusID = _packingDetail.ProductStatusID,
                                                              ProductSubStatusID = _packingDetail.ProductSubStatusID,
                                                              MFGDate = _packingDetail.MFGDate,
                                                              MFGTimeStart = _packingDetail.MFGTimeStart,
                                                              MFGTimeEnd = _packingDetail.MFGTimeEnd,
                                                              LocationID = _packingDetail.LocationID,
                                                              WarehouseID = _packingDetail.WarehouseID,
                                                              WarehouseName = _warehouse.Name,
                                                              PackingStatus = (int)_packingDetail.PackingStatus,
                                                              RemainQTY = _packingDetail.RemainQTY,
                                                              RemainStockUnitID = _packingDetail.RemainStockUnitID,
                                                              RemainBaseQty = _packingDetail.RemainBaseQTY,
                                                              RemainBaseUnitID = _packingDetail.RemainBaseUnitID,
                                                              ZoneID = _location.ZoneID,
                                                              ZoneName = _zone.Name,
                                                              CycleCountID = _ccount.CycleCountID,
                                                              CycleCountCode = _ccount.CycleCountCode,
                                                              CycleCountStatus = _ccount.CycleCountStatus,
                                                              CycleCountStartDate = _ccount.CycleCountStartDate,
                                                              CycleCountCompleteDate = _ccount.CycleCountCompleteDate,
                                                              CycleCountAssignDate = _ccount.CycleCountAssignDate
                                                          }
                                 into s
                                                       select new CycleCountModel()
                                                       {
                                                           ControlID = s.Key.ControlID,
                                                           ProductionDate = s.Key.ProductionDate,
                                                           Lot = s.Key.Lot,
                                                           LineID = s.Key.LineID,
                                                           ProductID = s.Key.ProductID,
                                                           ProductUnitID = s.Key.ProductUnitID,
                                                           OrderType = s.Key.OrderType,
                                                           OrderNo = s.Key.OrderNo,
                                                           PalletCode = s.Key.PalletCode,
                                                           Sequence = s.Key.Sequence,
                                                           StockQuantity = s.Key.StockQuantity,
                                                           BaseQuantity = s.Key.BaseQuantity,
                                                           ConversionQty = s.Key.ConversionQty,
                                                           ProductStatusID = s.Key.ProductStatusID,
                                                           ProductSubStatusID = s.Key.ProductSubStatusID,
                                                           MFGDate = s.Key.MFGDate,
                                                           MFGTimeStart = s.Key.MFGTimeStart,
                                                           MFGTimeEnd = s.Key.MFGTimeEnd,
                                                           LocationID = s.Key.LocationID,
                                                           WarehouseID = s.Key.WarehouseID,
                                                           WarehouseName = s.Key.WarehouseName,
                                                           PackingStatus = s.Key.PackingStatus,
                                                           RemainQTY = s.Key.RemainQTY,
                                                           RemainStockUnitID = s.Key.RemainStockUnitID,
                                                           RemainBaseQTY = s.Key.RemainBaseQty,
                                                           RemainBaseUnitID = s.Key.RemainBaseUnitID,
                                                           ZoneID = s.Key.ZoneID,
                                                           ZoneName = s.Key.ZoneName,
                                                           CycleCountID = s.Key.CycleCountID,
                                                           CycleCountCode = s.Key.CycleCountCode,
                                                           CycleCountStatus = s.Key.CycleCountStatus,
                                                           CycleCountStartDate = s.Key.CycleCountStartDate,
                                                           CycleCountCompleteDate = s.Key.CycleCountCompleteDate,
                                                           CycleCountAssignDate = s.Key.CycleCountAssignDate
                                                       });

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.CycleCountCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

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
        public List<CycleCountModel> GetlistAll(DateTime? sdte, DateTime? edte, int state, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                DateTime?[] d = Utilities.GetTerm(sdte, edte);
                IEnumerable<CycleCountModel> result = (from _ccount in Query().Filter(x => x.IsActive == true && x.CycleCountStatus == state).Get().DefaultIfEmpty()
                                                       join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _ccount.WarehounseID equals _warehouse.WarehouseID into tempwarehouse
                                                       from warehouse in tempwarehouse.DefaultIfEmpty()
                                                       where (_ccount.CycleCountCode.Contains(keyword))
                                                       group new { _ccount, warehouse }
                                                       by new
                                                       {
                                                           WarehouseID = _ccount.WarehounseID,
                                                           WarehouseName = warehouse.Name,
                                                           CycleCountID = _ccount.CycleCountID,
                                                           CycleCountCode = _ccount.CycleCountCode,
                                                           Status = (CycleCountStatusEnum)_ccount.CycleCountStatus,
                                                           CycleCountStatus = (CycleCountStatusEnum)_ccount.CycleCountStatus,
                                                           CycleCountStartDate = _ccount.CycleCountStartDate,
                                                           CycleCountCompleteDate = _ccount.CycleCountCompleteDate,
                                                           CycleCountAssignDate = _ccount.CycleCountAssignDate
                                                       } into s
                                                       select new CycleCountModel()
                                                       {
                                                           WarehouseID = s.Key.WarehouseID,
                                                           WarehouseName = s.Key.WarehouseName,
                                                           CycleCountID = s.Key.CycleCountID,
                                                           CycleCountCode = s.Key.CycleCountCode,
                                                           Status = s.Key.Status,
                                                           CycleCountStatus = (int)s.Key.CycleCountStatus,
                                                           CycleCountStartDate = s.Key.CycleCountStartDate,
                                                           CycleCountCompleteDate = s.Key.CycleCountCompleteDate,
                                                           CycleCountAssignDate = s.Key.CycleCountAssignDate
                                                       });

                if (sdte != null && edte != null)
                {
                    result = result.Where(x => x.CycleCountAssignDate.Value.Date >= sdte.Value.Date && x.CycleCountAssignDate.Value.Date <= edte.Value.Date);
                }

                if (result != null)
                {
                    totalRecords = result.Count();
                }
                else
                {
                    totalRecords = 0;
                }

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.CycleCountCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

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
        public CycleCountModel GetAll(string keyword)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
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

                CycleCountModel cyclecount = (from _ccount in Query().Filter(x => x.IsActive == true).Get()
                                              where (_ccount.CycleCountCode.Contains(keyword))
                                              select new CycleCountModel()
                                              {
                                                  CycleCountID = _ccount.CycleCountID,
                                                  CycleCountCode = _ccount.CycleCountCode,
                                                  CycleCountAssignDate = _ccount.CycleCountAssignDate,
                                                  CycleCountCompleteDate = _ccount.CycleCountCompleteDate,
                                                  CycleCountStartDate = _ccount.CycleCountStartDate,
                                                  CycleCountStatus = _ccount.CycleCountStatus,
                                                  IsActive = _ccount.IsActive,
                                                  DateCreated = _ccount.DateCreated,
                                                  DateModified = _ccount.DateModified,
                                                  Remark = _ccount.Remark,
                                                  UserCreated = _ccount.UserCreated,
                                                  UserModified = _ccount.UserModified,
                                                  ZoneID = _ccount.ZoneID,
                                                  WarehouseID = _ccount.WarehounseID,

                                              }).FirstOrDefault();

                IEnumerable<CycleCountDetails> details = (from _ccount in Query().Filter(x => x.IsActive == true && x.CycleCountCode == keyword).Get()
                                                          join _ccountDetail in cycleCountDetailService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccount.CycleCountID equals _ccountDetail.CycleCountID
                                                          join _location in locationService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccountDetail.LocationID equals _location.LocationID
                                                          join _zone in ZoneService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _location.ZoneID equals _zone.ZoneID
                                                          join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _zone.WarehouseID equals _warehouse.WarehouseID
                                                          join _product in ProductService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccountDetail.ProductID equals _product.ProductID
                                                          join _productCode in ProductCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                            on _product.ProductID equals _productCode.ProductID
                                                          join _productUnit in ProductUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccountDetail.ProductUnitID equals _productUnit.ProductUnitID
                                                          select new CycleCountDetails
                                                          {
                                                              CyclecountDetailID = _ccountDetail.CyclecountDetailID,
                                                              CycleCountID = _ccount.CycleCountID,
                                                              CycleCountCode = _ccount.CycleCountCode,
                                                              DetailStatus = _ccountDetail.DetailStatus,
                                                              CountingStockQty = _ccountDetail.CountingStockQty,
                                                              ConversionQty = _ccountDetail.ConversionQty,
                                                              BaseQuantity = _ccountDetail.BaseQuantity,
                                                              Barcode = _ccountDetail.Barcode,
                                                              DiffQty = _ccountDetail.DiffQty,
                                                              PalletCode = _ccountDetail.PalletCode,
                                                              ProductName = _product.Name,
                                                              ProductCode = _productCode.Code,
                                                              ProductID = _product.ProductID,
                                                              ProductUnitName = _productUnit.Name,
                                                              Lot = _ccountDetail.Product_Lot,
                                                              LocationID = _ccountDetail.LocationID,
                                                              LocationNo = _location.Code,
                                                              ZoneID = _ccount.ZoneID,
                                                              ZoneName = _zone.Name,
                                                              WarehouseID = _ccount.WarehounseID,
                                                              WarehouseName = _warehouse.Name,
                                                              Remark = _ccountDetail.Remark,
                                                              RemainQTY = _ccountDetail.StockQuantity,
                                                              DateCreated = _ccountDetail.DateCreated,
                                                              DateModified = _ccountDetail.DateModified,
                                                              UserCreated = _ccountDetail.UserCreated,
                                                              UserModified = _ccountDetail.UserModified,
                                                          });

                cyclecount.CycleCountDetails = details.ToList();
                return cyclecount;

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
        public bool AddCycleCount(CycleCountModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region [ PreFix ]

                    CycleCountPrefix prefix = cycleCountPrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("CC10012");
                    }

                    CycleCountPrefix tPrefix = cycleCountPrefixService.FindByID(prefix.PrefixID);

                    string CycleCountCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    entity.CycleCountCode = CycleCountCode;
                    tPrefix.IsLastest = false;

                    CycleCountPrefix newPrefix = new CycleCountPrefix()
                    {
                        IsLastest = true,
                        LastedKey = CycleCountCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    cycleCountPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    CycleCount _cyclecount = new CycleCount();

                    _cyclecount = new CycleCount
                    {
                        CycleCountID = Guid.NewGuid(),
                        CycleCountCode = CycleCountCode,
                        ZoneID = entity.ZoneID == null ? (Guid?)null : entity.ZoneID.Value,
                        WarehounseID = entity.WarehouseID,
                        CycleCountAssignDate = DateTime.Now,
                        CycleCountStartDate = entity.CycleCountStartDate,
                        CycleCountCompleteDate = entity.CycleCountCompleteDate,
                        CycleCountStatus = 10,
                        Remark = entity.Remark,
                        IsActive = true,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    base.Add(_cyclecount);

                    entity.CycleCountDetails.ToList().ForEach(item =>
                    {
                        CycleCountDetail _cyclecountDetails = new CycleCountDetail();

                        _cyclecountDetails = new CycleCountDetail
                        {
                            CyclecountDetailID = Guid.NewGuid(),
                            CycleCountID = _cyclecount.CycleCountID,
                            ProductID = item.ProductID,
                            Barcode = "*" + item.PalletCode + "*",
                            PalletCode = item.PalletCode,
                            StockQuantity = item.RemainQTY.Value,
                            CountingStockQty = 0,
                            DiffQty = item.DiffQty.Value,
                            ConversionQty = item.ConversionQty.Value,
                            BaseQuantity = item.RemainQTY * item.ConversionQty,
                            Product_Lot = item.Lot,
                            LocationID = item.LocationID,
                            DetailStatus = 10,
                            ProductUnitID = item.ProductUnitID,
                            Remark = item.Remark,
                            IsActive = true,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };

                        cycleCountDetailService.Add(_cyclecountDetails);

                    });

                    //var DispatchId = entity.RegisTruckDetail.FirstOrDefault().DispatchId;


                    //foreach (var _dispatchDetail in entity.RegisTruckDetail.ToList())
                    //{
                    //    var dispatchDetail = DispatchDetailService.Query().Filter(x => x.IsActive == true &&
                    //                                                               x.DispatchDetailId == _dispatchDetail.ReferenceID)
                    //                                                               .Get().FirstOrDefault();
                    //    if (dispatchDetail == null)
                    //        throw new HILIException("MSG00006");
                    //    if (dispatchDetail != null)
                    //    {
                    //        dispatchDetail.DispatchDetailStatus = 40;
                    //        dispatchDetail.UserModified = this.UserID;
                    //        dispatchDetail.DateModified = DateTime.Now;
                    //        dispatchDetail.IsActive = true;
                    //        DispatchDetailService.Modify(dispatchDetail);
                    //    }
                    //}

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
        public bool ModifyCycleCount(CycleCountModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    CycleCount _cyclecount = Query().Filter(x => x.IsActive == true && x.CycleCountID == entity.CycleCountID).Get().FirstOrDefault();
                    if (_cyclecount == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _cyclecount.CycleCountStatus = (int)CycleCountStatusEnum.Counting;
                    _cyclecount.CycleCountStartDate = entity.CycleCountStartDate;
                    _cyclecount.CycleCountAssignDate = entity.CycleCountAssignDate;
                    _cyclecount.Remark = entity.Remark;
                    _cyclecount.IsActive = true;
                    _cyclecount.UserModified = UserID;
                    _cyclecount.DateModified = DateTime.Now;
                    base.Modify(_cyclecount);

                    entity.CycleCountDetails.Where(x => x.CountingStockQty > 0).ToList().ForEach(item =>
                    {
                        CycleCountDetail _cyclecountDetails = cycleCountDetailService.FindByID(item.CyclecountDetailID);
                        //var _cyclecountDetails = cycleCountDetailService.Query().Filter(x => x.IsActive == true && x.CyclecountDetailID == item.CyclecountDetailID).Get().FirstOrDefault();
                        if (_cyclecountDetails == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        _cyclecountDetails.DetailStatus = (int)CycleCountStatusEnum.Counting;
                        _cyclecountDetails.CountingStockQty = item.CountingStockQty;
                        _cyclecountDetails.Remark = item.Remark;
                        _cyclecountDetails.UserModified = UserID;
                        _cyclecountDetails.DateModified = DateTime.Now;
                        cycleCountDetailService.Modify(_cyclecountDetails);
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
        public bool Approve(CycleCountModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    CycleCount _cyclecount = Query().Filter(x => x.IsActive == true && x.CycleCountID == entity.CycleCountID).Get().FirstOrDefault();
                    if (_cyclecount == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _cyclecount.CycleCountCompleteDate = DateTime.Now;
                    _cyclecount.CycleCountStatus = (int)CycleCountStatusEnum.Approve;
                    _cyclecount.Remark = entity.Remark;
                    _cyclecount.UserModified = UserID;
                    _cyclecount.DateModified = DateTime.Now;
                    base.Modify(_cyclecount);

                    entity.CycleCountDetails.ToList().ForEach(item =>
                    {
                        CycleCountDetail _cyclecountDetails = cycleCountDetailService.Query().Filter(x => x.IsActive == true && x.CyclecountDetailID == item.CyclecountDetailID).Get().FirstOrDefault();
                        if (_cyclecountDetails == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        _cyclecountDetails.CountingStockQty = item.CountingStockQty;
                        _cyclecountDetails.DiffQty = item.RemainQTY - item.CountingStockQty;
                        _cyclecountDetails.DetailStatus = (int)CycleCountStatusEnum.Approve;
                        _cyclecountDetails.Remark = item.Remark;
                        _cyclecountDetails.UserModified = UserID;
                        _cyclecountDetails.DateModified = DateTime.Now;
                        cycleCountDetailService.Modify(_cyclecountDetails);

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
        public bool Remove(string id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    CycleCount _current = Query().Filter(x => x.IsActive == true && x.CycleCountCode == id).Get().SingleOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.CycleCountStatus = (int)CycleCountStatusEnum.Cancel;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

                    cycleCountDetailService.Query().Filter(x => x.IsActive == true && x.CycleCountID == _current.CycleCountID).Get()
                        .ToList().ForEach(x =>
                        {
                            x.DetailStatus = (int)CycleCountStatusEnum.Cancel;
                            x.DateModified = DateTime.Now;
                            x.UserModified = UserID;
                            cycleCountDetailService.Modify(x);
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

        #region Stock
        public List<ProductModel> GetProductStock(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from _stockinfo in StockInfoService.Query().Filter(x => x.IsActive).Get()
                              join _stoclbalance in StockBalanceService.Query().Filter(x => x.StockQuantity > 0).Get()
                                on _stockinfo.StockInfoID equals _stoclbalance.StockInfoID
                              join _productowner in ProductOwnerService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductOwnerID equals _productowner.ProductOwnerID
                              join _proudct in ProductService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductID equals _proudct.ProductID
                              join _productcode in ProductCodeService.Query().Filter(x => x.IsActive && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                on _proudct.ProductID equals _productcode.ProductID
                              join _pb in ProductBrandService.Query().Filter(x => x.IsActive).Get()
                                on _proudct.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Filter(x => x.IsActive).Get()
                                on _proudct.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _unit in ProductUnitService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.StockUnitID equals _unit.ProductUnitID
                              join _a in ProductUnitService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductUnitPriceID equals _a.ProductUnitID into a
                              from _unit2 in a.DefaultIfEmpty()
                              join _status in ProductStatusService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductStatusID equals _status.ProductStatusID
                              join _sub_status in ProductSubStatusService.Query().Filter(x => x.IsActive).Get()
                                on _stockinfo.ProductSubStatusID equals _sub_status.ProductSubStatusID
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
                                  Price = (_stockinfo.Price != null ? _stockinfo.Price : null),
                                  Quantity = _stoclbalance.StockQuantity - _stoclbalance.ReserveQuantity,
                                  BaseQuantity = _stoclbalance.StockQuantity,
                                  BaseUnitId = _stockinfo.BaseUnitID,
                                  ConversionQty = _stoclbalance.ConversionQty,
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
                              }).Where(x => (x.ProductName.Contains(keyword) || x.ProductCode.Contains(keyword) || x.ProductUnitName.Contains(keyword)));

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
        public List<CycleCountModel> GetCycleCountStock(Guid? WarehouseID, Guid? ZoneID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.PutAway
                };

                IEnumerable<CycleCountModel> result = (from _productionControl in productionControlService.Query().Filter(x => x.IsActive == true).Get()
                                                       join _packingDetail in packingDetailService.Query().Filter(x => x.IsActive == true && packingStatus.Contains((int)x.PackingStatus)).Get()
                                                         on _productionControl.ControlID equals _packingDetail.ControlID
                                                       join _location in locationService.Query().Filter(x => x.IsActive == true).Get()
                                                        on _packingDetail.LocationID equals _location.LocationID
                                                       join _zone in ZoneService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _location.ZoneID equals _zone.ZoneID
                                                       join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true && x.ReferenceCode != "412").Get()
                                                         on _zone.WarehouseID equals _warehouse.WarehouseID
                                                       join _product in ProductService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _productionControl.ProductID equals _product.ProductID
                                                       join _productCode in ProductCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                         on _product.ProductID equals _productCode.ProductID
                                                       join _productUnit in ProductUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                       on _productionControl.ProductUnitID equals _productUnit.ProductUnitID

                                                       where (WarehouseID != Guid.Empty ? _warehouse.WarehouseID == WarehouseID.Value : true) &&
                                                             (ZoneID != Guid.Empty ? _zone.ZoneID == ZoneID.Value : true)
                                                       group new { _productionControl, _packingDetail, _location, _zone, _warehouse, _product, _productCode, _productUnit }
                                                          by new
                                                          {
                                                              ControlID = _productionControl.ControlID,
                                                              ProductionDate = _productionControl.ProductionDate,
                                                              Lot = _packingDetail.LotNo,
                                                              LineID = _productionControl.LineID,
                                                              ProductID = _productionControl.ProductID,
                                                              ProductCode = _productCode.Code,
                                                              ProductName = _product.Name,
                                                              ProductUnitID = _productionControl.ProductUnitID,
                                                              ProductUnitName = _productUnit.Name,
                                                              OrderType = _productionControl.OrderType,
                                                              OrderNo = _productionControl.OrderNo,
                                                              PalletCode = _packingDetail.PalletCode,
                                                              Sequence = _packingDetail.Sequence,
                                                              StockQuantity = (_packingDetail.RemainQTY != null ? _packingDetail.RemainQTY.Value : 0) - (_packingDetail.ReserveQTY != null ? _packingDetail.ReserveQTY.Value : 0),//_packingDetail.StockQuantity,
                                                              BaseQuantity = _packingDetail.BaseQuantity,
                                                              ConversionQty = _packingDetail.ConversionQty,
                                                              ProductStatusID = _packingDetail.ProductStatusID,
                                                              ProductSubStatusID = _packingDetail.ProductSubStatusID,
                                                              MFGDate = _packingDetail.MFGDate,
                                                              MFGTimeStart = _packingDetail.MFGTimeStart,
                                                              MFGTimeEnd = _packingDetail.MFGTimeEnd,
                                                              LocationID = _packingDetail.LocationID,
                                                              LocationNo = _location.Code,
                                                              WarehouseID = _packingDetail.WarehouseID,
                                                              PackingStatus = (int)_packingDetail.PackingStatus,
                                                              RemainQTY = _packingDetail.RemainQTY,
                                                              RemainStockUnitID = _packingDetail.RemainStockUnitID,
                                                              RemainBaseQty = _packingDetail.RemainBaseQTY,
                                                              RemainBaseUnitID = _packingDetail.RemainBaseUnitID,
                                                              ZoneName = _zone.Name,
                                                              ZoneID = _zone.ZoneID,
                                                              WarehouseName = _warehouse.Name
                                                          }
                                 into s
                                                       select new CycleCountModel()
                                                       {
                                                           ControlID = s.Key.ControlID,
                                                           ProductionDate = s.Key.ProductionDate,
                                                           Lot = s.Key.Lot,
                                                           LineID = s.Key.LineID,
                                                           ProductID = s.Key.ProductID,
                                                           ProductCode = s.Key.ProductCode,
                                                           ProductName = s.Key.ProductName,
                                                           ProductUnitID = s.Key.ProductUnitID,
                                                           ProductUnitName = s.Key.ProductUnitName,
                                                           OrderType = s.Key.OrderType,
                                                           OrderNo = s.Key.OrderNo,
                                                           PalletCode = s.Key.PalletCode,
                                                           Sequence = s.Key.Sequence,
                                                           StockQuantity = s.Key.StockQuantity,
                                                           BaseQuantity = s.Key.BaseQuantity,
                                                           ConversionQty = s.Key.ConversionQty,
                                                           ProductStatusID = s.Key.ProductStatusID,
                                                           ProductSubStatusID = s.Key.ProductSubStatusID,
                                                           MFGDate = s.Key.MFGDate,
                                                           MFGTimeStart = s.Key.MFGTimeStart,
                                                           MFGTimeEnd = s.Key.MFGTimeEnd,
                                                           LocationID = s.Key.LocationID,
                                                           LocationNo = s.Key.LocationNo,
                                                           WarehouseID = s.Key.WarehouseID,
                                                           PackingStatus = s.Key.PackingStatus,
                                                           RemainQTY = s.Key.RemainQTY,
                                                           RemainStockUnitID = s.Key.RemainStockUnitID,
                                                           RemainBaseQTY = s.Key.RemainBaseQty,
                                                           RemainBaseUnitID = s.Key.RemainBaseUnitID,
                                                           ZoneID = s.Key.ZoneID,
                                                           ZoneName = s.Key.ZoneName,
                                                           WarehouseName = s.Key.WarehouseName
                                                       });

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    result = result.Where(x => x.WarehouseName.Trim().Contains(keyword) ||
                                               x.LocationNo.Trim().Contains(keyword) ||
                                               x.ProductName.Trim().Contains(keyword) ||
                                               x.ProductCode.Trim().Contains(keyword) ||
                                               (x.OrderNo != null && x.OrderNo.Contains(keyword)));
                }

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Where(x => x.RemainQTY > 0).OrderByDescending(x => x.CycleCountCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

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
        public List<CustomEnumerable> GetCycleCountStatus()
        {
            try
            {
                return Utilities.GetEnumDescription<CycleCountStatusEnum>().Select(n => new CustomEnumerable { ID = (int)n.Key, Value = n.Value }).ToList();
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        #endregion

        #region [CycleCount HandHeld]
        public List<CycleCountModel> GetCycleCountData(Guid? warehouseID)
        {
            try
            {
                int[] packingStatus = new int[] { (int)CycleCountStatusEnum.New, (int)CycleCountStatusEnum.Counting };
                IEnumerable<CycleCountModel> result = (from _ccount in Query().Filter(x => x.IsActive == true && (warehouseID != null ? x.WarehounseID == warehouseID.Value : true) && packingStatus.Contains((int)x.CycleCountStatus)).Get()
                                                       join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true).Get()
                                                         on _ccount.WarehounseID equals _warehouse.WarehouseID
                                                       select new CycleCountModel
                                                       {
                                                           CycleCountID = _ccount.CycleCountID,
                                                           CycleCountCode = _ccount.CycleCountCode,
                                                           WarehouseID = _ccount.WarehounseID,
                                                           WarehouseName = _warehouse.Name,
                                                           Remark = _ccount.Remark,
                                                           DateCreated = _ccount.DateCreated,
                                                           DateModified = _ccount.DateModified,
                                                           UserCreated = _ccount.UserCreated,
                                                           UserModified = _ccount.UserModified,
                                                           CycleCountStartDate = _ccount.CycleCountStartDate,
                                                           CycleCountCompleteDate = _ccount.CycleCountCompleteDate,
                                                           CycleCountAssignDate = _ccount.CycleCountAssignDate
                                                       });

                return result.OrderByDescending(x => x.CycleCountCode).ToList();

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
        public CycleCountDetails GetCycleCountDataDetail(string CycleCountCode, Guid? warehouseID, string ScanPallet)
        {
            try
            {
                IEnumerable<CycleCountDetails> details = (from _ccount in Query().Filter(x => x.IsActive == true && x.CycleCountCode == CycleCountCode && (warehouseID != null ? x.WarehounseID == warehouseID.Value : true)).Get()
                                                          join _ccountDetail in cycleCountDetailService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccount.CycleCountID equals _ccountDetail.CycleCountID
                                                          join _location in locationService.Query().Filter(x => x.IsActive == true).Get()
                                                           on _ccountDetail.LocationID equals _location.LocationID
                                                          join _zone in ZoneService.Query().Filter(x => x.IsActive == true).Get()
                                                           on _location.ZoneID equals _zone.ZoneID
                                                          join _warehouse in WarehouseService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccount.WarehounseID equals _warehouse.WarehouseID
                                                          join _product in ProductService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccountDetail.ProductID equals _product.ProductID
                                                          join _productCode in ProductCodeService.Query().Filter(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock).Get()
                                                            on _product.ProductID equals _productCode.ProductID
                                                          join _productUnit in ProductUnitService.Query().Filter(x => x.IsActive == true).Get()
                                                            on _ccountDetail.ProductUnitID equals _productUnit.ProductUnitID
                                                          where (_ccountDetail.PalletCode.Contains(ScanPallet))
                                                          select new CycleCountDetails
                                                          {
                                                              CyclecountDetailID = _ccountDetail.CyclecountDetailID,
                                                              CycleCountID = _ccount.CycleCountID,
                                                              CycleCountCode = _ccount.CycleCountCode,
                                                              DetailStatus = _ccountDetail.DetailStatus,
                                                              CountingStockQty = _ccountDetail.CountingStockQty,
                                                              ConversionQty = _ccountDetail.ConversionQty,
                                                              BaseQuantity = _ccountDetail.BaseQuantity,
                                                              Barcode = _ccountDetail.Barcode,
                                                              DiffQty = _ccountDetail.DiffQty,
                                                              PalletCode = _ccountDetail.PalletCode,
                                                              ProductName = _product.Name,
                                                              ProductCode = _productCode.Code,
                                                              ProductID = _product.ProductID,
                                                              ProductUnitName = _productUnit.Name,
                                                              Lot = _ccountDetail.Product_Lot,
                                                              LocationID = _ccountDetail.LocationID,
                                                              ZoneID = _zone.ZoneID,
                                                              ZoneName = _zone.Name,
                                                              WarehouseID = _ccount.WarehounseID,
                                                              WarehouseName = _warehouse.Name,
                                                              Remark = _ccountDetail.Remark,
                                                              StockQuantity = _ccountDetail.StockQuantity,
                                                              DateCreated = _ccountDetail.DateCreated,
                                                              DateModified = _ccountDetail.DateModified,
                                                              UserCreated = _ccountDetail.UserCreated,
                                                              UserModified = _ccountDetail.UserModified,
                                                          });
                return details.FirstOrDefault();

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
        public bool ConfirmCounting(Guid CyclecountDetailID, string CycleCountCode, decimal CountingQty, decimal DiffQty, string LotNumber)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    CycleCountDetail _cyclecountDetails = cycleCountDetailService.Query().Filter(x => x.IsActive && x.CyclecountDetailID == CyclecountDetailID).Get().FirstOrDefault();

                    if (_cyclecountDetails == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _cyclecountDetails.CountingStockQty = CountingQty;
                    _cyclecountDetails.DiffQty = DiffQty;
                    _cyclecountDetails.DetailStatus = (int)CycleCountStatusEnum.Counting;
                    _cyclecountDetails.UserModified = UserID;
                    _cyclecountDetails.DateModified = DateTime.Now;
                    cycleCountDetailService.Modify(_cyclecountDetails);

                    int _countDetails = cycleCountDetailService.Query().Filter(x => x.IsActive && x.CycleCountID == _cyclecountDetails.CycleCountID && x.DetailStatus == (int)CycleCountStatusEnum.Counting).Get().Count();
                    bool ok = cycleCountDetailService.Query().Get().Any(x => x.IsActive && x.CycleCountID == _cyclecountDetails.CycleCountID && x.DetailStatus == (int)CycleCountStatusEnum.New);

                    if (_countDetails == 1 && ok)
                    {
                        CycleCount _cyclecount = Query().Filter(x => x.IsActive && x.CycleCountID == _cyclecountDetails.CycleCountID).Get().FirstOrDefault();
                        _cyclecount.CycleCountStatus = (int)CycleCountStatusEnum.Counting;
                        _cyclecount.CycleCountStartDate = DateTime.Now;
                        _cyclecount.UserModified = UserID;
                        _cyclecount.DateModified = DateTime.Now;
                        base.Modify(_cyclecount);
                    }
                    if (_countDetails == 1 && !ok)
                    {
                        CycleCount _cyclecount = Query().Filter(x => x.IsActive && x.CycleCountID == _cyclecountDetails.CycleCountID).Get().FirstOrDefault();
                        _cyclecount.CycleCountStartDate = DateTime.Now;
                        _cyclecount.CycleCountCompleteDate = DateTime.Now;
                        _cyclecount.CycleCountStatus = (int)CycleCountStatusEnum.Complete;
                        _cyclecount.UserModified = UserID;
                        _cyclecount.DateModified = DateTime.Now;
                        base.Modify(_cyclecount);
                    }
                    if (_countDetails > 1 && !ok)
                    {
                        CycleCount _cyclecount = Query().Filter(x => x.IsActive && x.CycleCountID == _cyclecountDetails.CycleCountID).Get().FirstOrDefault();
                        _cyclecount.CycleCountCompleteDate = DateTime.Now;
                        _cyclecount.CycleCountStatus = (int)CycleCountStatusEnum.Complete;
                        _cyclecount.UserModified = UserID;
                        _cyclecount.DateModified = DateTime.Now;
                        base.Modify(_cyclecount);
                    }

                    CycleCountAssign _ccAssign = new CycleCountAssign();

                    _ccAssign = new CycleCountAssign
                    {
                        CyclecountAssignID = Guid.NewGuid(),
                        CyclecountDetailID = CyclecountDetailID,
                        PalletCode = LotNumber,
                        StockQuantity = CountingQty,
                        StockUnitID = _cyclecountDetails.ProductUnitID,
                        BasickQuantity = CountingQty,
                        BasicUnit = _cyclecountDetails.ProductUnitID,
                    };

                    cycleCountAssignService.Add(_ccAssign);

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
        public CycleCountModel GetJobComplete(string CycleCountCode)
        {
            try
            {
                IEnumerable<CycleCountModel> result = (from _ccount in Query().Filter(x => x.IsActive == true && x.CycleCountCode == CycleCountCode).Get()
                                                       select new CycleCountModel
                                                       {
                                                           CycleCountID = _ccount.CycleCountID,
                                                           CycleCountCode = _ccount.CycleCountCode,
                                                           Status = (CycleCountStatusEnum)_ccount.CycleCountStatus,
                                                           WarehouseID = _ccount.WarehounseID,
                                                           Remark = _ccount.Remark,
                                                           DateCreated = _ccount.DateCreated,
                                                           DateModified = _ccount.DateModified,
                                                           UserCreated = _ccount.UserCreated,
                                                           UserModified = _ccount.UserModified,
                                                           CycleCountStartDate = _ccount.CycleCountStartDate,
                                                           CycleCountCompleteDate = _ccount.CycleCountCompleteDate,
                                                           CycleCountAssignDate = _ccount.CycleCountAssignDate
                                                       });

                return result.FirstOrDefault();

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
        #endregion [CycleCount HandHeld]
    }
}
