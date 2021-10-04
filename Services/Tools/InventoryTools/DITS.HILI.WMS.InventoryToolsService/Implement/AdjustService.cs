using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{

    public class AdjustService : Repository<Adjust>, IAdjustService
    {
        #region Property
        private readonly string _locationCode = ConfigurationManager.AppSettings["LocationCode"].ToString();
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> packingDetailService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<CycleCountDetail> cycleCountDetailService;
        private readonly IRepository<CycleCount> cycleCountService;
        private readonly IRepository<CycleCountAssign> cycleCountAssignService;
        private readonly IRepository<AdjustDetail> adjustDetailService;
        private readonly IRepository<AdjustType> adjustTypeService;
        private readonly IRepository<AdjustPrefix> AdjustPrefixService;
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
        private readonly IRepository<DocumentType> DocumentTypeService;
        private readonly IStockService stockService;
        #endregion

        #region Constructor

        public AdjustService(IUnitOfWork dbContext,
                                    IRepository<ProductionControl> _productionControl,
                                    IRepository<ProductionControlDetail> _packingDetailService,
                                    IRepository<CycleCount> _cycleCountService,
                                    IRepository<CycleCountDetail> _cycleCountDetailService,
                                    IRepository<CycleCountAssign> _cycleCountAssignService,
                                    IRepository<AdjustDetail> _adjustDetailService,
                                    IRepository<AdjustType> _adjustTypeService,
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
                                    IRepository<AdjustPrefix> _AdjustPrefix,
                                    IRepository<TruckType> _truckType,
                                    IRepository<DockConfig> _dockConfig,
                                    IRepository<Location> _location,
                                    IRepository<PhysicalZone> _physicalZone,
                                    IRepository<Zone> _Zone,
                                    IRepository<Warehouse> _Warehouse,
                                    IRepository<DocumentType> _DocumentType,
                                    IRepository<Receiving> _Receiving,
                                    IStockService _stockService)
            : base(dbContext)
        {
            productionControlService = _productionControl;
            cycleCountService = _cycleCountService;
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
            AdjustPrefixService = _AdjustPrefix;
            cycleCountDetailService = _cycleCountDetailService;
            cycleCountAssignService = _cycleCountAssignService;
            adjustDetailService = _adjustDetailService;
            adjustTypeService = _adjustTypeService;
            receivingService = _Receiving;
            locationService = _location;
            physicalZoneService = _physicalZone;
            ZoneService = _Zone;
            WarehouseService = _Warehouse;
            DocumentTypeService = _DocumentType;
            stockService = _stockService;
        }

        #endregion

        public Adjust Get(Guid id)
        {
            try
            {
                Adjust _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.AdjustID == id)
                                  .Include(x => x.AdjustDetail)
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
        public AdjustModel GetAll(string keyword)
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
                    (int)PackingStatusEnum.Damage
                };

                AdjustModel adjust = (from _adjust in Where(x => x.IsActive == true)
                                      join _adjustType in adjustTypeService.Where(x => x.IsActive == true)
                                        on _adjust.AdjustTypeID equals _adjustType.AdjustTypeID
                                      where (_adjust.AdjustCode.Contains(keyword))
                                      select new AdjustModel()
                                      {
                                          AdjustID = _adjust.AdjustID,
                                          AdjustCode = _adjust.AdjustCode,
                                          AdjustTypeID = _adjust.AdjustTypeID,
                                          AdjustTypeName = _adjustType.Name,
                                          AdjustStatus = (int)_adjust.AdjustStatus,
                                          AdjustStartDate = _adjust.AdjustStartDate,
                                          AdjustCompleteDate = _adjust.AdjustCompleteDate,
                                          IsActive = _adjust.IsActive,
                                          DateCreated = _adjust.DateCreated,
                                          DateModified = _adjust.DateModified,
                                          Remark = _adjust.Remark,
                                          UserCreated = _adjust.UserCreated,
                                          UserModified = _adjust.UserModified,
                                          ReferenceDoc = _adjust.ReferenceDoc

                                      }).FirstOrDefault();

                IEnumerable<AdjustModelDetails> details = (from _adjust in Where(x => x.IsActive == true && x.AdjustCode == keyword)
                                                           join _adjustDetail in adjustDetailService.Where(x => x.IsActive == true) on _adjust.AdjustID equals _adjustDetail.AdjustID
                                                           join _packDetail in packingDetailService.Where(x => x.IsActive == true) on _adjustDetail.PalletCode equals _packDetail.PalletCode
                                                           join _a in locationService.Where(x => x.IsActive == true) on _adjustDetail.LocationID equals _a.LocationID into a
                                                           from _location in a.DefaultIfEmpty()
                                                           join _z in ZoneService.Where(x => x.IsActive == true) on _location.ZoneID equals _z.ZoneID into z
                                                           from _zone in z.DefaultIfEmpty()
                                                           join _w in WarehouseService.Where(x => x.IsActive == true) on _zone.WarehouseID equals _w.WarehouseID into w
                                                           from _warehouse in w.DefaultIfEmpty()
                                                           join _product in ProductService.Where(x => x.IsActive == true) on _adjustDetail.ProductID equals _product.ProductID
                                                           join _productCode in ProductCodeService.Where(x => x.IsActive == true) on _product.ProductID equals _productCode.ProductID
                                                           join _productUnit in ProductUnitService.Where(x => x.IsActive == true) on _adjustDetail.ProductUnitID equals _productUnit.ProductUnitID
                                                           where _productCode.CodeType == ProductCodeTypeEnum.Stock
                                                           select new AdjustModelDetails
                                                           {
                                                               AdjustDetailID = _adjustDetail.AdjustDetailID,
                                                               AdjustID = _adjust.AdjustID,
                                                               AdjustStockQty = _adjustDetail.AdjustStockQty,
                                                               AdjustStockUnitID = _adjustDetail.AdjustStockUnitID,
                                                               Barcode = _adjustDetail.Barcode,
                                                               ConversionQty = _adjustDetail.ConversionQty,
                                                               AdjustBaseQty = _adjustDetail.AdjustBaseQty,
                                                               AdjustBaseUnitID = _adjustDetail.AdjustBaseUnitID,
                                                               PalletCode = _adjustDetail.PalletCode,
                                                               ProductName = _product.Name,
                                                               ProductCode = _productCode.Code,
                                                               ProductID = _product.ProductID,
                                                               ProductStatusID = _packDetail.ProductStatusID,
                                                               ProductUnitName = _productUnit.Name,
                                                               ProductLot = _adjustDetail.ProductLot,
                                                               LocationID = _adjustDetail.LocationID,
                                                               MFGDate = _adjustDetail.MFGDate,
                                                               LocationNo = _location.Code,
                                                               ZoneID = _zone.ZoneID,
                                                               ZoneName = _zone.Name,
                                                               WarehouseID = _warehouse.WarehouseID,
                                                               WarehouseName = _warehouse.Name,
                                                               AdjustStatus = _adjustDetail.AdjustStatus,
                                                               Remark = _adjustDetail.Remark,
                                                               DateCreated = _adjustDetail.DateCreated,
                                                               DateModified = _adjustDetail.DateModified,
                                                               UserCreated = _adjustDetail.UserCreated,
                                                               UserModified = _adjustDetail.UserModified,
                                                           });

                adjust.AdjustModelDetails = details.ToList();
                return adjust;

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

        public List<AdjustModel> GetlistAll(string state, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                if (!Enum.TryParse(state, out AdjustStatusEnum AdjustStatus))
                {
                    AdjustStatus = AdjustStatusEnum.New;
                }
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<AdjustModel> result = (from _adjust in Where(x => x.IsActive == true && (x.AdjustStatus == AdjustStatus))
                                                   join _adjustType in adjustTypeService.Where(x => x.IsActive == true) on _adjust.AdjustTypeID equals _adjustType.AdjustTypeID
                                                   where string.IsNullOrEmpty(keyword) ? true : (_adjust.AdjustCode.Contains(keyword))
                                                   select new AdjustModel()
                                                   {
                                                       AdjustID = _adjust.AdjustID,
                                                       AdjustCode = _adjust.AdjustCode,
                                                       AdjustTypeID = _adjust.AdjustTypeID,
                                                       AdjustTypeName = _adjustType.Value,
                                                       AdjustStatusName = _adjust.AdjustStatus.ToString(),
                                                       AdjustStatus = (int)_adjust.AdjustStatus,
                                                       AdjustStartDate = _adjust.AdjustStartDate,
                                                       AdjustCompleteDate = _adjust.AdjustCompleteDate,
                                                       IsEffect = _adjust.IsEffect,
                                                       ReferenceDoc = _adjust.ReferenceDoc,
                                                   }).ToList();
                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.AdjustCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<AdjustModel> GetAdjustStockOther(string state, Guid? WarehouseID, string product, string pallet, string Lot, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                if ((!WarehouseID.HasValue || WarehouseID.Value == Guid.Empty) && string.IsNullOrEmpty(product) && string.IsNullOrEmpty(pallet) && string.IsNullOrEmpty(Lot))
                {
                    throw new HILIException("ADJ0000001");
                    //return new List<AdjustModel>();
                }
                product = (string.IsNullOrEmpty(product) ? "" : product);
                pallet = (string.IsNullOrEmpty(pallet) ? "" : pallet);
                Lot = (string.IsNullOrEmpty(Lot) ? "" : Lot); 
                object[] parameters = new object[6];
                parameters[0] = new SqlParameter("WarehouseID", SqlDbType.UniqueIdentifier) { Value = WarehouseID ?? Guid.Empty };
                parameters[1] = new SqlParameter("productCode", SqlDbType.NVarChar, 100) { Value = product };
                parameters[2] = new SqlParameter("palletCode", SqlDbType.NVarChar, 100) { Value = pallet };
                parameters[3] = new SqlParameter("lot", SqlDbType.NVarChar, 100) { Value = Lot };
                parameters[4] = new SqlParameter("PageNum", pageIndex) { Value = pageIndex != null ? pageIndex : 0 };
                parameters[5] = new SqlParameter("PageSize", pageSize) { Value = pageSize != null ? pageSize : 0 }; 
                var results = Context.SQLQuery<AdjustModel>("exec usp_getAdjustStockOther @WarehouseID,@productCode,@palletCode,@lot,@PageNum,@PageSize", parameters).ToList();

                using (var connection = Context.ContextScope.Database.Connection)
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_getAdjustStockOtherCount";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.UniqueIdentifier) { Value = WarehouseID ?? Guid.Empty });
                    command.Parameters.Add(new SqlParameter("@productCode", SqlDbType.NVarChar, 100) { Value = product });
                    command.Parameters.Add(new SqlParameter("@palletCode", SqlDbType.NVarChar, 100) { Value = pallet });
                    command.Parameters.Add(new SqlParameter("@lot", SqlDbType.NVarChar, 100) { Value = Lot });
                    totalRecords = (int)command.ExecuteScalar();
                    connection.Close();
                }
                return results;
                //int[] packingStatus = new int[]
                //{
                //    (int)PackingStatusEnum.PutAway,
                //    (int)PackingStatusEnum.Delivery
                //};
                //var result = (from _packingDetail in packingDetailService.Where(x => x.IsActive)
                //             join _productionControl in productionControlService.Where(x => x.IsActive) on _packingDetail.ControlID equals _productionControl.ControlID
                //             join _product in ProductService.Where(x => x.IsActive) on _productionControl.ProductID equals _product.ProductID
                //             join _productCode in ProductCodeService.Where(x => x.IsActive) on _productionControl.ProductID equals _productCode.ProductID
                //             join _productUnit in ProductUnitService.Where(x => x.IsActive) on _productionControl.ProductUnitID equals _productUnit.ProductUnitID
                //             join _productstatus in ProductStatusService.Where(x => x.IsActive) on _packingDetail.ProductStatusID equals _productstatus.ProductStatusID
                //             join _productsubstatus in ProductSubStatusService.Where(x => x.IsActive) on _packingDetail.ProductSubStatusID equals _productsubstatus.ProductSubStatusID
                //             join _location in locationService.Where(x => x.IsActive) on _packingDetail.LocationID equals _location.LocationID
                //             join _zone in ZoneService.Where(x => x.IsActive) on _location.ZoneID equals _zone.ZoneID
                //             join _warehouse in WarehouseService.Where(x => x.IsActive) on _zone.WarehouseID equals _warehouse.WarehouseID
                //             where (_warehouse.WarehouseID == WarehouseID)
                //             && (_packingDetail.PackingStatus == PackingStatusEnum.PutAway || _packingDetail.PackingStatus == PackingStatusEnum.Delivery)
                //             && _warehouse.ReferenceCode == "111"
                //             && (_productCode.CodeType == ProductCodeTypeEnum.Stock)
                //             && product == "" ? true : _productCode.Code.Contains(product)
                //             && pallet == "" ? true : _packingDetail.PalletCode.Contains(pallet)
                //             && Lot == "" ? true : _productionControl.Lot.Contains(pallet)
                //             group new { _productionControl, _packingDetail, _location, _zone, _warehouse, _product, _productCode, _productUnit }
                //             by new
                //             {
                //                 ControlID = _productionControl.ControlID,
                //                 ProductionDate = _productionControl.ProductionDate,
                //                 Lot = _productionControl.Lot,
                //                 LineID = _productionControl.LineID,
                //                 ProductID = _productionControl.ProductID,
                //                 ProductCode = _productCode.Code,
                //                 ProductName = _product.Name,
                //                 ProductUnitID = _productionControl.ProductUnitID,
                //                 ProductUnitName = _productUnit.Name,
                //                 ProductStatusName = _productstatus.Name,
                //                 ProductSubStatusName = _productsubstatus.Name,
                //                 OrderType = _productionControl.OrderType,
                //                 OrderNo = _productionControl.OrderNo,
                //                 PalletCode = _packingDetail.PalletCode,
                //                 Sequence = _packingDetail.Sequence,
                //                 StockQuantity = (_packingDetail.RemainQTY.HasValue ? _packingDetail.RemainQTY.Value : 0) - (_packingDetail.ReserveQTY.HasValue ? _packingDetail.ReserveQTY.Value : 0),
                //                 BaseQuantity = _packingDetail.BaseQuantity,
                //                 ConversionQty = _packingDetail.ConversionQty,
                //                 ProductStatusID = _packingDetail.ProductStatusID,
                //                 ProductSubStatusID = _packingDetail.ProductSubStatusID,
                //                 MFGDate = _packingDetail.MFGDate,
                //                 MFGTimeStart = _packingDetail.MFGTimeStart,
                //                 MFGTimeEnd = _packingDetail.MFGTimeEnd,
                //                 LocationID = _packingDetail.LocationID,
                //                 LocationNo = _location.Code,
                //                 WarehouseID = _packingDetail.WarehouseID,
                //                 PackingStatus = (int)_packingDetail.PackingStatus,
                //                 RemainQTY = _packingDetail.RemainQTY,
                //                 RemainStockUnitID = _packingDetail.RemainStockUnitID,
                //                 RemainBaseQty = _packingDetail.RemainBaseQTY,
                //                 RemainBaseUnitID = _packingDetail.RemainBaseUnitID,
                //                 ZoneName = _zone.Name,
                //                 ZoneID = _zone.ZoneID,
                //                 WarehouseName = _warehouse.Name
                //             } into s
                //             select new AdjustModel()
                //             {
                //                 ControlID = s.Key.ControlID,
                //                 ProductionDate = s.Key.ProductionDate,
                //                 Lot = s.Key.Lot,
                //                 LineID = s.Key.LineID,
                //                 ProductID = s.Key.ProductID,
                //                 ProductCode = s.Key.ProductCode,
                //                 ProductName = s.Key.ProductName,
                //                 ProductUnitID = s.Key.ProductUnitID,
                //                 ProductUnitName = s.Key.ProductUnitName,
                //                 ProductStatusName = s.Key.ProductStatusName,
                //                 ProductSubStatusName = s.Key.ProductSubStatusName,
                //                 OrderType = s.Key.OrderType,
                //                 OrderNo = s.Key.OrderNo,
                //                 PalletCode = s.Key.PalletCode,
                //                 Sequence = s.Key.Sequence,
                //                 StockQuantity = s.Key.StockQuantity,
                //                 BaseQuantity = s.Key.BaseQuantity,
                //                 ConversionQty = s.Key.ConversionQty,
                //                 ProductStatusID = s.Key.ProductStatusID,
                //                 ProductSubStatusID = s.Key.ProductSubStatusID,
                //                 MFGDate = s.Key.MFGDate,
                //                 MFGTimeStart = s.Key.MFGTimeStart,
                //                 MFGTimeEnd = s.Key.MFGTimeEnd,
                //                 LocationID = s.Key.LocationID,
                //                 LocationNo = s.Key.LocationNo,
                //                 WarehouseID = s.Key.WarehouseID,
                //                 PackingStatus = s.Key.PackingStatus,
                //                 RemainQTY = s.Key.RemainQTY,
                //                 AdjustStockUnitID = s.Key.RemainStockUnitID,
                //                 RemainBaseQTY = s.Key.RemainBaseQty,
                //                 AdjustBaseUnitID = s.Key.RemainBaseUnitID,
                //                 ZoneID = s.Key.ZoneID,
                //                 ZoneName = s.Key.ZoneName,
                //                 WarehouseName = s.Key.WarehouseName
                //             }).ToList();
                //totalRecords = result.Count();
                //if (pageIndex != null && pageSize != null)
                //{
                //    return result.OrderByDescending(x => x.AdjustCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
                //}
                //return result.ToList();
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
        public List<AdjustModel> GetAdjustStockCycleCount(string state, Guid? WarehouseID, string product, string pallet, string Lot, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                product = (string.IsNullOrWhiteSpace(product) ? "" : product);
                pallet = (string.IsNullOrWhiteSpace(pallet) ? "" : pallet);
                Lot = (string.IsNullOrWhiteSpace(Lot) ? "" : Lot);
                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.PutAway,
                    (int)PackingStatusEnum.Delivery
                };

                IEnumerable<AdjustModel> result = (from _productionControl in productionControlService.Where(x => x.IsActive == true)
                                                   join _packingDetail in packingDetailService.Where(x => x.IsActive == true && packingStatus.Contains((int)x.PackingStatus)) on _productionControl.ControlID equals _packingDetail.ControlID
                                                   join _location in locationService.Where(x => x.IsActive == true) on _packingDetail.LocationID equals _location.LocationID
                                                   join _zone in ZoneService.Where(x => x.IsActive == true) on _location.ZoneID equals _zone.ZoneID
                                                   join _ccount in cycleCountService.Where(x => x.IsActive == true) on _zone.WarehouseID equals _ccount.WarehounseID
                                                   join _ccountDetail in cycleCountDetailService.Where(x => x.IsActive == true)
                                                   on new { CCD1 = _ccount.CycleCountID, CCD2 = _packingDetail.LocationID, CCD3 = _packingDetail.PalletCode }
                                                   equals new { CCD1 = _ccountDetail.CycleCountID.Value, CCD2 = _ccountDetail.LocationID, CCD3 = _ccountDetail.PalletCode }
                                                   join _warehouse in WarehouseService.Where(x => x.IsActive == true) on _ccount.WarehounseID equals _warehouse.WarehouseID
                                                   join _productstatus in ProductStatusService.Where(x => x.IsActive == true) on _packingDetail.ProductStatusID equals _productstatus.ProductStatusID
                                                   join _productsubstatus in ProductSubStatusService.Where(x => x.IsActive == true) on _packingDetail.ProductSubStatusID equals _productsubstatus.ProductSubStatusID
                                                   join _product in ProductService.Where(x => x.IsActive == true) on _productionControl.ProductID equals _product.ProductID
                                                   join _productCode in ProductCodeService.Where(x => x.IsActive == true && x.CodeType == ProductCodeTypeEnum.Stock) on _product.ProductID equals _productCode.ProductID
                                                   join _productUnit in ProductUnitService.Where(x => x.IsActive == true) on _productionControl.ProductUnitID equals _productUnit.ProductUnitID
                                                   where 
                                                   (product !=""? _productCode.Code.Contains(product):true)
                                                   && (pallet!="" ? _packingDetail.PalletCode.Contains(pallet) : true)
                                                   && (Lot!="" ? _packingDetail.LotNo.Contains(Lot) : true)
                                                   && (_ccountDetail.DiffQty.HasValue && _ccountDetail.DiffQty != 0)
                                                   group new { _productionControl, _packingDetail, _ccount, _ccountDetail, _location, _zone, _warehouse, _product, _productCode, _productUnit }
                                                   by new
                                                   {
                                                       ControlID = _productionControl.ControlID,
                                                       ProductionDate = _productionControl.ProductionDate,
                                                       Lot = _productionControl.Lot,
                                                       LineID = _productionControl.LineID,
                                                       ProductID = _productionControl.ProductID,
                                                       ProductCode = _productCode.Code,
                                                       ProductName = _product.Name,
                                                       ProductStatusName = _productstatus.Name,
                                                       ProductSubStatusName = _productsubstatus.Name,
                                                       ProductUnitID = _productionControl.ProductUnitID,
                                                       ProductUnitName = _productUnit.Name,
                                                       OrderType = _productionControl.OrderType,
                                                       OrderNo = _productionControl.OrderNo,
                                                       PalletCode = _packingDetail.PalletCode,
                                                       Sequence = _packingDetail.Sequence,
                                                       StockQuantity = (_packingDetail.RemainQTY.HasValue ? _packingDetail.RemainQTY.Value : 0) - (_packingDetail.ReserveQTY.HasValue ? _packingDetail.ReserveQTY.Value : 0),
                                                       BaseQuantity = _packingDetail.BaseQuantity,
                                                       CountingStockQty = _ccountDetail.CountingStockQty,
                                                       DiffQty = _ccountDetail.DiffQty,
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
                                                   } into s
                                                   select new AdjustModel()
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
                                                       ProductStatusName = s.Key.ProductStatusName,
                                                       ProductSubStatusName = s.Key.ProductSubStatusName,
                                                       OrderType = s.Key.OrderType,
                                                       OrderNo = s.Key.OrderNo,
                                                       PalletCode = s.Key.PalletCode,
                                                       Sequence = s.Key.Sequence,
                                                       StockQuantity = s.Key.StockQuantity,
                                                       DiffQty = s.Key.DiffQty,
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
                                                       AdjustStockUnitID = s.Key.RemainStockUnitID,
                                                       RemainBaseQTY = s.Key.RemainBaseQty,
                                                       AdjustBaseUnitID = s.Key.RemainBaseUnitID,
                                                       ZoneID = s.Key.ZoneID,
                                                       ZoneName = s.Key.ZoneName,
                                                       WarehouseName = s.Key.WarehouseName
                                                   }).ToList();

                if (AdjustTypeStatusEnum.AddStock.ToString() == state)
                {
                    result = result.Where(x => x.DiffQty < 0).ToList();
                }

                if (AdjustTypeStatusEnum.ReduceStock.ToString() == state)
                {
                    result = result.Where(x => x.DiffQty > 0).ToList();
                }
                //if (AdjustTypeStatusEnum.AddOther.ToString() == state || AdjustTypeStatusEnum.ReduceOther.ToString() == state)
                //{
                //    result = result.Where(x => x.DiffQty == 0).ToList();
                //} 
                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Where(x => x.StockQuantity > 0).OrderByDescending(x => x.AdjustCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
                }

                return result.ToList();
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
        public bool AddAdjust(AdjustModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region [ PreFix ]

                    AdjustPrefix prefix = AdjustPrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("AJ10012");
                    }

                    AdjustPrefix tPrefix = AdjustPrefixService.FindByID(prefix.PrefixID);

                    string AdjustCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    entity.AdjustCode = AdjustCode;
                    tPrefix.IsLastest = false;

                    AdjustPrefix newPrefix = new AdjustPrefix()
                    {
                        IsLastest = true,
                        LastedKey = AdjustCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    AdjustPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]

                    AdjustType _adjustType = new AdjustType();

                    _adjustType = new AdjustType
                    {
                        AdjustTypeID = Guid.NewGuid(),
                        Name = entity.AdjustTypeName,
                        Value = entity.AdjustTypeName,
                        Remark = entity.Remark,
                        IsActive = true,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    adjustTypeService.Add(_adjustType);

                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Adjust _adjust = new Adjust();

                    _adjust = new Adjust
                    {
                        AdjustID = Guid.NewGuid(),
                        AdjustCode = AdjustCode,
                        AdjustTypeID = _adjustType.AdjustTypeID,
                        AdjustStatus = AdjustStatusEnum.New,
                        AdjustStartDate = DateTime.Now,
                        AdjustCompleteDate = null,
                        Remark = entity.Remark,
                        IsEffect = entity.IsEffect,
                        ReferenceDoc = entity.ReferenceDoc,
                        IsActive = true,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    base.Add(_adjust);

                    entity.AdjustModelDetails.ToList().ForEach(item =>
                    {
                        AdjustDetail _adjustModelDetails = new AdjustDetail();

                        _adjustModelDetails = new AdjustDetail
                        {
                            AdjustDetailID = Guid.NewGuid(),
                            AdjustID = _adjust.AdjustID,
                            ProductID = item.ProductID,
                            Barcode = "*" + item.PalletCode + "*",
                            PalletCode = item.PalletCode,
                            AdjustStockQty = item.AdjustStockQty.Value,
                            ProductLot = item.ProductLot,
                            AdjustBaseQty = item.AdjustStockQty * item.ConversionQty,
                            ConversionQty = item.ConversionQty.Value,
                            AdjustBaseUnitID = item.AdjustBaseUnitID,
                            AdjustStockUnitID = item.AdjustStockUnitID,
                            LocationID = item.LocationID,
                            AdjustStatus = (int)AdjustStatusEnum.New,
                            ProductUnitID = item.ProductUnitID,
                            ReferenceID = item.ReferenceID,
                            MFGDate = item.MFGDate,
                            AdjustTransactionType = item.AdjustStockQty.Value > 0 ? "R03" : "I03",
                            Remark = item.Remark,
                            IsActive = true,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };

                        adjustDetailService.Add(_adjustModelDetails);

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
        public bool ModifyAdjust(AdjustModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Adjust _adjust = Query().Filter(x => x.IsActive == true && x.AdjustID == entity.AdjustID).Get().FirstOrDefault();
                    if (_adjust == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _adjust.ReferenceDoc = entity.ReferenceDoc;
                    _adjust.Remark = entity.Remark;
                    _adjust.IsActive = true;
                    _adjust.UserModified = UserID;
                    _adjust.AdjustStartDate = entity.AdjustStartDate;
                    _adjust.DateModified = DateTime.Now;
                    base.Modify(_adjust);

                    entity.AdjustModelDetails.ToList().ForEach(item =>
                    {
                        AdjustDetail _adjustDetails = adjustDetailService.Query().Filter(x => x.IsActive == true && x.AdjustDetailID == item.AdjustDetailID).Get().FirstOrDefault();
                        if (_adjustDetails == null)
                        {
                            AdjustDetail _adjustModelDetails = new AdjustDetail();

                            _adjustModelDetails = new AdjustDetail
                            {
                                AdjustDetailID = Guid.NewGuid(),
                                AdjustID = _adjust.AdjustID,
                                ProductID = item.ProductID,
                                Barcode = "*" + item.PalletCode + "*",
                                PalletCode = item.PalletCode,
                                AdjustStockQty = item.AdjustStockQty.Value,
                                ProductLot = item.ProductLot,
                                AdjustBaseQty = item.AdjustStockQty * item.ConversionQty,
                                ConversionQty = item.ConversionQty.Value,
                                AdjustBaseUnitID = item.AdjustBaseUnitID,
                                AdjustStockUnitID = item.AdjustStockUnitID,
                                LocationID = item.LocationID,
                                AdjustStatus = (int)AdjustStatusEnum.New,
                                ProductUnitID = item.ProductUnitID,
                                ReferenceID = item.ReferenceID,
                                MFGDate = item.MFGDate,
                                AdjustTransactionType = item.AdjustStockQty.Value > 0 ? "R03" : "I03",
                                Remark = item.Remark,
                                IsActive = true,
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                            };

                            adjustDetailService.Add(_adjustModelDetails);
                        }
                        else
                        {
                            _adjustDetails.AdjustStockQty = item.AdjustStockQty;
                            _adjustDetails.Remark = item.Remark;
                            _adjustDetails.UserModified = UserID;
                            _adjustDetails.DateModified = DateTime.Now;
                            adjustDetailService.Modify(_adjustDetails);
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
        public bool Approve(AdjustModel entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Guid? productOwnerID = ProductOwnerService.Query().Filter(x => x.IsActive).Get().FirstOrDefault()?.ProductOwnerID;
                    Guid? productSubStatusID = ProductSubStatusService.Query().Filter(x => x.IsActive && x.Code == "SS000").Get().FirstOrDefault()?.ProductSubStatusID;
                    Adjust _adjust = Query().Filter(x => x.IsActive == true && x.AdjustID == entity.AdjustID).Get().FirstOrDefault();
                    if (_adjust == null || productOwnerID == null || productSubStatusID == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _adjust.AdjustStatus = AdjustStatusEnum.Complete;
                    _adjust.AdjustCompleteDate = DateTime.Now;
                    _adjust.Remark = entity.Remark;
                    _adjust.UserModified = UserID;
                    _adjust.DateModified = DateTime.Now;
                    _adjust.IsActive = entity.IsHideItem == false;
                    base.Modify(_adjust);

                    entity.AdjustModelDetails.ToList().ForEach(item =>
                    {
                        Product product = ProductService.Query().Filter(x => x.IsActive && x.ProductID == item.ProductID).Get().FirstOrDefault();
                        AdjustDetail _adjustDetails = adjustDetailService.Query().Filter(x => x.IsActive == true && x.AdjustDetailID == item.AdjustDetailID).Get().FirstOrDefault();
                        DocumentType _doc = DocumentTypeService.Query().Filter(x => x.Code == (item.AdjustStockQty.Value > 0 ? "RE0005" : "DI0024")).Get().FirstOrDefault();
                        Receiving _receiving = receivingService.Query().Filter(x => x.IsActive && x.PalletCode == item.PalletCode).Get().FirstOrDefault();

                        if (_adjustDetails == null || product == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        if (_receiving == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        _adjustDetails.AdjustStockQty = item.AdjustStockQty;
                        //_adjustDetails.AdjustTransactionType = item.AdjustStockQty.Value > 0 ? "R03" : "I03";
                        _adjustDetails.AdjustTransactionType = item.AdjustStockQty.Value > 0 ? "R08" : "A03";
                        _adjustDetails.AdjustStatus = (int)AdjustStatusEnum.Complete;
                        _adjustDetails.Remark = item.Remark;
                        _adjustDetails.UserModified = UserID;
                        _adjustDetails.DateModified = DateTime.Now;
                        _adjustDetails.IsSentInterface = entity.IsSentInterface;
                        adjustDetailService.Modify(_adjustDetails);


                        ProductionControlDetail _packing = packingDetailService.Query().Filter(x => x.IsActive && x.PalletCode == item.PalletCode && x.PackingStatus == PackingStatusEnum.Delivery).Get().SingleOrDefault();

                        List<StockInOutModel> stockOut = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                DocumentID = item.AdjustDetailID,
                                DocumentTypeID = _doc.DocumentTypeID,
                                PalletCode = item.PalletCode,
                                DocumentCode = _adjust.AdjustCode,
                                ProductID = item.ProductID.Value,
                                Lot = item.ProductLot,
                                ProductOwnerID = productOwnerID.Value,
                                ProductSubStatusID = productSubStatusID.Value,
                                ExpirationDate = _receiving.ExpirationDate.Value,
                                ManufacturingDate = item.MFGDate.Value,
                                StockUnitID = item.AdjustStockUnitID.Value,
                                BaseUnitID = item.AdjustBaseUnitID.Value,
                                ConversionQty = item.ConversionQty.Value,
                                ProductStatusID = item.ProductStatusID.Value,
                                LocationCode = _packing == null ? item.LocationNo : _locationCode,
                                Quantity = item.AdjustStockQty.Value,
                                PackingStatus = (int)PackingStatusEnum.PutAway,
                            }
                        };

                        stockService.UserID = UserID;
                        if (entity.AdjustTypeName == AdjustTypeStatusEnum.AddStock.ToString())
                        {
                            stockService.AdjustIncomming(stockOut);
                        }

                        if (entity.AdjustTypeName == AdjustTypeStatusEnum.ReduceStock.ToString())
                        {
                            stockService.AdjustOutgoing(stockOut);
                        }

                        if (entity.AdjustTypeName == AdjustTypeStatusEnum.AddOther.ToString() || entity.AdjustTypeName == AdjustTypeStatusEnum.ReduceOther.ToString())
                        {
                            stockService.AdjustOthergoing(stockOut);
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
        public bool Remove(string id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Adjust _current = Query().Filter(x => x.IsActive == true && x.AdjustCode == id).Get().SingleOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Remove(_current);

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
    }
}