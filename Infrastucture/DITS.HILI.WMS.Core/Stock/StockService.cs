using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.Core.Stock
{
    public class StockService : Repository<StockInfo>, IStockService
    {
        private readonly IRepository<StockInfo> _StockInfoService;
        private readonly IRepository<StockBalance> _StockBalanceService;
        private readonly IRepository<StockTransaction> _StockTransService;
        private readonly IRepository<StockLocationBalance> _StockLocationService;
        private readonly IRepository<Location> _LocationService;
        private readonly IRepository<ProductionControlDetail> ProductionControlDetailService;
        private readonly IRepository<Zone> _ZoneService;
        public StockService(IUnitOfWork context,
                            IRepository<StockInfo> _stockInfo,
                            IRepository<StockBalance> _stockBalance,
                            IRepository<StockTransaction> _stockTrans,
                            IRepository<StockLocationBalance> _stockLocation,
                            IRepository<Location> _locationService,
                            IRepository<Zone> _zoneService,
                            IRepository<ProductionControlDetail> _productionControlDetail) : base(context)
        {
            _StockInfoService = _stockInfo;
            _StockBalanceService = _stockBalance;
            _StockTransService = _stockTrans;
            _StockLocationService = _stockLocation;
            _LocationService = _locationService;
            ProductionControlDetailService = _productionControlDetail;
            _ZoneService = _zoneService;
        }
        public bool Incomming(List<StockInOutModel> stockIn, StockTransactionTypeEnum transType = StockTransactionTypeEnum.Incomming)
        {
            try
            {
                stockIn.ForEach(item =>
                {
                    Location location = _LocationService.FirstOrDefault(x => x.Code == item.LocationCode && x.IsActive);//.Include(x => x.Zone).SingleOrDefault();

                    StockInfo _stockInfo = _StockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );

                    StockInfo stockInfo = _stockInfo == null ? null : _StockInfoService.FindByID(_stockInfo.StockInfoID);

                    if (stockInfo == null)
                    {
                        #region insert StockInfo
                        stockInfo = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            Remark = item.Remark,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockInfo);
                        #endregion
                    }

                    IRepository<StockBalance> stockBalanceService = Context.Repository<StockBalance>();
                    StockBalance _balance = stockBalanceService.SingleOrDefault(x => x.StockInfoID == stockInfo.StockInfoID);//.Get().SingleOrDefault();
                   
                    StockBalance balance = _balance == null ? null : stockBalanceService.FindByID(_balance.StockBalanceID);
                    if (balance == null)
                    {
                        #region  not found StockBalance
                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockInfo.StockInfoID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            Remark = item.Remark,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        stockBalanceService.Add(balance);

                        #endregion
                    }
                    else
                    {
                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += (item.Quantity * item.ConversionQty);
                        balance.Remark = item.Remark;
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        stockBalanceService.Modify(balance);
                    }
                    var zone = _ZoneService.FirstOrDefault(e => e.ZoneID == location.ZoneID);
                    StockLocationBalance _location_balance_in = _StockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location.ZoneID
                                                                                  && x.WarehouseID == zone.WarehouseID);//.Get().SingleOrDefault();

                    StockLocationBalance location_balance_in = _location_balance_in == null ? null : _StockLocationService.FindByID(_location_balance_in.StockLocationID);

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            Remark = item.Remark,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    {
                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.Remark = item.Remark;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = item.StockTransTypeEnum == 0 ? transType : item.StockTransTypeEnum,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);
                });

                return true;
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

        public bool Incomming2(List<StockInOutModel> stockIn, IUnitOfWork uow)
        {
            try
            {
                IRepository<Location> locationService = uow.Repository<Location>();
                IRepository<StockInfo> stockInfoService = uow.Repository<StockInfo>();
                IRepository<StockBalance> stockBalanceService = uow.Repository<StockBalance>();
                IRepository<StockLocationBalance> stockLocationService = uow.Repository<StockLocationBalance>();
                IRepository<StockTransaction> stockTransService = uow.Repository<StockTransaction>();
                IRepository<Zone> zoneTransService = uow.Repository<Zone>();
                stockIn.ForEach(item =>
                {
                    Location location = locationService.FirstOrDefault(x => x.Code == item.LocationCode);//.Include(x => x.Zone).Get().SingleOrDefault();

                    StockInfo stockInfo = stockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );//.Get().SingleOrDefault();

                    if (stockInfo == null)
                    {
                        #region insert StockInfo
                        stockInfo = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            Remark = item.Remark,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        stockInfoService.Add(stockInfo);
                        #endregion
                    }

                    StockBalance balance = stockBalanceService.SingleOrDefault(x => x.StockInfoID == stockInfo.StockInfoID);//.Get().SingleOrDefault();

                    if (balance == null)
                    {
                        #region  not found StockBalance
                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockInfo.StockInfoID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            Remark = item.Remark,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        stockBalanceService.Add(balance);

                        #endregion
                    }
                    else
                    {
                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += (item.Quantity * item.ConversionQty);
                        balance.Remark = item.Remark;
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        stockBalanceService.Modify(balance);
                    }
                    var zone = zoneTransService.FindByID(location.ZoneID);
                    StockLocationBalance location_balance_in = stockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location.ZoneID
                                                                                  && x.WarehouseID == zone.WarehouseID);//.Get().SingleOrDefault();

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            Remark = item.Remark,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };
                        stockLocationService.Add(location_balance_in);
                        #endregion
                    }
                    else
                    {
                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.Remark = item.Remark;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        stockLocationService.Modify(location_balance_in);
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = item.StockTransTypeEnum == 0 ? StockTransactionTypeEnum.Incomming : item.StockTransTypeEnum,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    stockTransService.Add(trans_in);
                });

                return true;
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

        public bool AdjustReserve(StockSearch stock, StockReserveTypeEnum reserveType)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    StockBalance balance = new StockBalance();
                    StockLocationBalance location_balance_in = new StockLocationBalance();

                    SearchStock(stock, ref balance, ref location_balance_in);

                    StockBalance tmpBalance = _StockBalanceService.FindByID(balance.StockBalanceID);
                    StockLocationBalance tmpLocationBalance = _StockLocationService.FindByID(location_balance_in.StockLocationID);

                    if (reserveType == StockReserveTypeEnum.Reserve)
                    {
                        tmpBalance.ReserveQuantity += stock.QTY;
                        tmpLocationBalance.ReserveQuantity += stock.QTY;
                    }
                    else if (reserveType == StockReserveTypeEnum.UnReserve)
                    {
                        tmpBalance.ReserveQuantity -= stock.QTY;
                        tmpLocationBalance.ReserveQuantity -= stock.QTY;

                        if (tmpBalance.ReserveQuantity < 0 || tmpLocationBalance.ReserveQuantity < 0)
                        {
                            throw new HILIException("MSG00039");
                        }
                    }

                    tmpBalance.UserModified = UserID;
                    tmpBalance.DateModified = DateTime.Now;
                    _StockBalanceService.Modify(tmpBalance);

                    tmpLocationBalance.UserModified = UserID;
                    tmpLocationBalance.DateModified = DateTime.Now;
                    _StockLocationService.Modify(tmpLocationBalance);

                    scope.Complete();
                }

                return true;
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

        public bool AdjustReserve_(List<StockSearch> stockList, StockReserveTypeEnum reserveType)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    StockBalance balance = new StockBalance();
                    StockLocationBalance location_balance_in = new StockLocationBalance();

                    foreach (StockSearch stock in stockList)
                    {
                        SearchStock(stock, ref balance, ref location_balance_in);

                        if (reserveType == StockReserveTypeEnum.Reserve)
                        {
                            balance.ReserveQuantity += stock.QTY;
                            location_balance_in.ReserveQuantity += stock.QTY;
                        }
                        else if (reserveType == StockReserveTypeEnum.UnReserve)
                        {
                            balance.ReserveQuantity -= stock.QTY;
                            location_balance_in.ReserveQuantity -= stock.QTY;

                            if (balance.ReserveQuantity < 0 || location_balance_in.ReserveQuantity < 0)
                            {
                                throw new HILIException("MSG00039");
                            }
                        }

                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);

                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    scope.Complete();
                }

                return true;
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

        public bool AdjustStockTrans(StockInOutModel stock)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    StockBalance balance = new StockBalance();
                    StockLocationBalance location_balance_in = new StockLocationBalance();

                    StockSearch stockSearch = new StockSearch()
                    {
                        SupplierID = stock.SupplierID,
                        Lot = stock.Lot,
                        LocationID = stock.LocationID,
                        ManufacturingDate = stock.ManufacturingDate,
                        ExpirationDate = stock.ExpirationDate,
                        ProductID = stock.ProductID,
                        ProductOwnerID = stock.ProductOwnerID,
                        StockUnitID = stock.StockUnitID,
                        BaseUnitID = stock.BaseUnitID,
                        ProductStatusID = stock.ProductStatusID,
                        ConversionQty = stock.ConversionQty,
                    };

                    SearchStock(stockSearch, ref balance, ref location_balance_in);

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = stock.StockTransTypeEnum,
                        LocationID = stock.LocationID.Value,
                        PalletCode = stock.PalletCode,
                        BaseQuantity = stock.ConversionQty * stock.Quantity,
                        ConversionQty = stock.ConversionQty,
                        DocumentCode = stock.DocumentCode,
                        DocumentTypeID = stock.DocumentTypeID,
                        DocumentID = stock.DocumentID,

                        StockLocationID = location_balance_in.StockLocationID,

                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                    };

                    _StockTransService.Add(trans_in);
                    scope.Complete();
                }

                return true;
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

        private void SearchStock(StockSearch stock, ref StockBalance balance, ref StockLocationBalance location_balance_in)
        {
            Location location = new Location();
            IQueryable<Location> locationExist = _LocationService.Where(x => x.IsActive);

            if (stock.LocationID != null)
            {
                location = locationExist.FirstOrDefault(x => x.LocationID == stock.LocationID);//.Include(x => x.Zone).Get().SingleOrDefault();
            }
            else
            {
                location = locationExist.FirstOrDefault(x => x.Code == stock.LocationCode);//.Include(x => x.Zone).Get().SingleOrDefault();
            }

            if (location == null)
            {
                throw new HILIException("MSG00055");
            }

            StockInfo stockInfo = _StockInfoService.FirstOrDefault(x => x.ProductID == stock.ProductID
                                                        && x.StockUnitID == stock.StockUnitID
                                                        && x.BaseUnitID == stock.BaseUnitID
                                                        && x.ConversionQty == stock.ConversionQty
                                                        && x.Lot == stock.Lot
                                                        && x.ProductStatusID == stock.ProductStatusID
                                                        && x.ManufacturingDate == stock.ManufacturingDate
                                                        && x.ExpirationDate == stock.ExpirationDate
                                                        //&& x.SupplierID == stock.SupplierID
                                                        //&& x.ProductOwnerID == stock.ProductOwnerID
                                                        );//.Get().SingleOrDefault();

            if (stockInfo == null)
            {
                throw new HILIException("MSG00037");
            }

            StockBalance tmpBalance;
            tmpBalance = balance = _StockBalanceService.FirstOrDefault(x => x.StockInfoID == stockInfo.StockInfoID);

            if (balance == null || tmpBalance == null)
            {
                throw new HILIException("MSG00038");
            }
            var zone = _ZoneService.FindByID(location.ZoneID);
            location_balance_in = _StockLocationService.FirstOrDefault(x => x.StockBalanceID == tmpBalance.StockBalanceID
                                                      && x.ZoneID == location.ZoneID
                                                      && x.WarehouseID == zone.WarehouseID);

            if (location_balance_in == null)
            {
                throw new HILIException("MSG00042");
            }
        }

        public bool Outgoing(List<StockInOutModel> stockOut, StockTransactionTypeEnum transType = StockTransactionTypeEnum.Outgoing)
        {
            try
            {
                stockOut.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    StockLocationBalance location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    if (location_balance == null)
                    {
                        #region no found location balance
                        location_balance = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance);

                        #endregion
                    }



                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = transType,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);


                    IRepository<StockLocationBalance> tmpStockLocationService = Context.Repository<StockLocationBalance>();
                    StockLocationBalance tmp_location_balance = tmpStockLocationService.SingleOrDefault(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID);//.Get().SingleOrDefault();

                    tmp_location_balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    tmp_location_balance.StockQuantity -= item.Quantity;
                    tmp_location_balance.UserModified = UserID;
                    tmp_location_balance.DateModified = DateTime.Now;
                    tmpStockLocationService.Modify(tmp_location_balance);

                    IRepository<StockBalance> tmpStockBalanceService = Context.Repository<StockBalance>();
                    StockBalance tmpBalance = tmpStockBalanceService.SingleOrDefault(x => x.StockBalanceID == tmp_location_balance.StockBalanceID);//.Get().SingleOrDefault();

                    tmpBalance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    tmpBalance.StockQuantity -= item.Quantity;
                    tmpBalance.UserModified = UserID;
                    tmpBalance.DateModified = DateTime.Now;
                    tmpStockBalanceService.Modify(tmpBalance);
                });

                return true;
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

        public bool Outgoing2(List<StockInOutModel> stockOut, IUnitOfWork uow)
        {
            try
            {
                IRepository<Location> locationService = uow.Repository<Location>();
                IRepository<StockInfo> stockInfoService = uow.Repository<StockInfo>();
                IRepository<StockBalance> stockBalanceService = uow.Repository<StockBalance>();
                IRepository<StockLocationBalance> stockLocationService = uow.Repository<StockLocationBalance>();
                IRepository<StockTransaction> stockTransService = uow.Repository<StockTransaction>();
                IRepository<Zone> zontTransService = uow.Repository<Zone>();
                stockOut.ForEach(item =>
                {
                    Location location = locationService.FirstOrDefault(x => x.Code == item.LocationCode && x.IsActive);
                    var zone = zontTransService.FirstOrDefault(x=>x.ZoneID== location.ZoneID);
                    StockInfo stockINFO = stockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        throw new HILIException("MSG00037");
                    }
                    else
                    {
                        balance = stockBalanceService.FirstOrDefault(x => x.StockInfoID == stockINFO.StockInfoID);
                    }

                    if (balance == null)
                    {
                        throw new HILIException("MSG00098");
                    }
                    StockLocationBalance location_balance = stockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID);
                    if (location_balance == null)
                    {
                        throw new HILIException("MSG00099");
                    }
                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = item.StockTransTypeEnum == 0 ? StockTransactionTypeEnum.Outgoing : item.StockTransTypeEnum,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        Remark = item.Remark

                    };

                    stockTransService.Add(trans_in);


                    IRepository<StockLocationBalance> tmpStockLocationService = uow.Repository<StockLocationBalance>();
                    StockLocationBalance tmp_location_balance = tmpStockLocationService.FindByID(location_balance.StockLocationID);

                    tmp_location_balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    tmp_location_balance.StockQuantity -= item.Quantity;
                    tmp_location_balance.UserModified = UserID;
                    tmp_location_balance.DateModified = DateTime.Now;
                    if (item.ReserveQuantity.HasValue)
                    {
                        tmp_location_balance.ReserveQuantity -= item.ReserveQuantity;
                    }

                    tmpStockLocationService.Modify(tmp_location_balance);

                    IRepository<StockBalance> tmpStockBalanceService = uow.Repository<StockBalance>();
                    StockBalance tmpBalance = tmpStockBalanceService.FindByID(tmp_location_balance.StockBalanceID);

                    tmpBalance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    tmpBalance.StockQuantity -= item.Quantity;
                    tmpBalance.UserModified = UserID;
                    tmpBalance.DateModified = DateTime.Now;
                    if (item.ReserveQuantity.HasValue)
                    {
                        tmpBalance.ReserveQuantity -= item.ReserveQuantity.Value;
                    }

                    tmpStockBalanceService.Modify(tmpBalance);


                    if (tmp_location_balance.StockQuantity < 0)
                    {
                        tmp_location_balance.StockQuantity = 0;
                        tmp_location_balance.BaseQuantity = 0;
                        tmp_location_balance.ReserveQuantity = 0;
                    }


                    if (tmpBalance.StockQuantity < 0)
                    {
                        tmpBalance.StockQuantity = 0;
                        tmpBalance.BaseQuantity = 0;
                        tmpBalance.ReserveQuantity = 0;
                    }

                    //if (tmp_location_balance.StockQuantity < 0)
                    //  throw new HILIException("MSG00095");


                    //if (tmpBalance.StockQuantity < 0)
                    //  throw new HILIException("MSG00096");
                });

                return true;
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

        public bool ReceivetoReprocess(StockInternalRecModel stock, IUnitOfWork uow)
        {
            try
            {
                #region Declare Repository

                IRepository<Location> locationService = uow.Repository<Location>();
                IRepository<StockInfo> stockInfoService = uow.Repository<StockInfo>();
                IRepository<StockBalance> stockBalanceService = uow.Repository<StockBalance>();
                IRepository<StockLocationBalance> stockLocationService = uow.Repository<StockLocationBalance>();
                IRepository<StockTransaction> stockTranService = uow.Repository<StockTransaction>();
                IRepository<Zone> zoneTranService = uow.Repository<Zone>();
                #endregion

                StockInfo stockInfo = new StockInfo();
                StockBalance balance = new StockBalance();
                StockLocationBalance locationBalance = new StockLocationBalance();
                Location location = new Location();

                #region Search Location

                RepositoryQuery<Location> locationExist = locationService.Query().Filter(x => x.IsActive);

                if (stock.LocationID != null)
                {
                    location = locationExist.Filter(x => x.LocationID == stock.LocationID).Include(x => x.Zone).Get().SingleOrDefault();
                }
                else
                {
                    location = locationExist.Filter(x => x.Code == stock.LocationCode).Include(x => x.Zone).Get().SingleOrDefault();
                }

                if (location == null)
                {
                    throw new HILIException("MSG00055");
                }

                #endregion
                var zone = zoneTranService.FindByID(location.ZoneID);
                #region Search & Add Stock Info

                Guid? stockInfoID = stockInfoService.Query().Filter(x => x.ProductID == stock.ProductID
                                                                && x.StockUnitID == stock.StockUnitID
                                                                && x.BaseUnitID == stock.BaseUnitID
                                                                && x.ConversionQty == stock.ConversionQty
                                                                && x.Lot == stock.Lot
                                                                && x.ProductOwnerID == stock.ProductOwnerID
                                                                && x.ProductStatusID == stock.ProductStatusID
                                                                && x.SupplierID == stock.SupplierID
                                                                && x.ManufacturingDate == stock.ManufacturingDate
                                                                && x.ExpirationDate == stock.ExpirationDate
                                                                ).Get().SingleOrDefault()?.StockInfoID;

                if (stockInfoID == null)
                {
                    stockInfoID = Guid.NewGuid();

                    stockInfo = new StockInfo
                    {
                        StockInfoID = stockInfoID.Value,
                        ProductOwnerID = stock.ProductOwnerID,
                        SupplierID = stock.SupplierID,
                        ProductID = stock.ProductID,
                        Lot = stock.Lot,
                        ExpirationDate = stock.ExpirationDate,
                        ManufacturingDate = stock.ManufacturingDate,
                        ProductWidth = stock.ProductWidth,
                        ProductLength = stock.ProductLength,
                        ProductHeight = stock.ProductHeight,
                        ProductWeight = stock.ProductWeight,
                        PackageWeight = stock.PackageWeight,
                        Price = stock.Price,
                        StockUnitID = stock.StockUnitID,
                        BaseUnitID = stock.BaseUnitID,
                        ProductUnitPriceID = stock.ProductUnitPriceID,
                        ConversionQty = stock.ConversionQty,
                        ProductStatusID = stock.ProductStatusID,
                        ProductSubStatusID = stock.ProductSubStatusID,
                        Remark = stock.Remark,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        IsActive = true
                    };
                    stockInfoService.Add(stockInfo);
                }

                #endregion

                #region  Search & Add Stock Balance

                Guid? balanceID = stockBalanceService.Query().Filter(x => x.StockInfoID == stockInfoID).Get().SingleOrDefault()?.StockBalanceID;

                if (balanceID == null)
                {
                    balanceID = Guid.NewGuid();

                    balance = new StockBalance
                    {
                        StockBalanceID = balanceID.Value,
                        StockInfoID = stockInfoID.Value,
                        BaseQuantity = stock.Quantity * stock.ConversionQty,
                        ConversionQty = stock.ConversionQty,
                        StockQuantity = stock.Quantity,
                        ReserveQuantity = 0,
                        Remark = stock.Remark,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                    };
                    stockBalanceService.Add(balance);
                }
                else
                {
                    balance = stockBalanceService.FindByID(balanceID);

                    balance.StockQuantity += stock.Quantity;
                    balance.BaseQuantity += (stock.Quantity * stock.ConversionQty);
                    balance.IsActive = true;
                    balance.Remark = stock.Remark;
                    balance.UserModified = UserID;
                    balance.DateModified = DateTime.Now;
                    stockBalanceService.Modify(balance);
                }

                #endregion

                #region Search & Add Stock Location Balance

                Guid? locationBalanceID = stockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location.ZoneID
                                                                                  && x.WarehouseID == zone.WarehouseID)
                                                                                  .Get().SingleOrDefault()?.StockLocationID;

                if (locationBalanceID == null)
                {
                    locationBalanceID = Guid.NewGuid();

                    locationBalance = new StockLocationBalance
                    {
                        StockLocationID = locationBalanceID.Value,
                        StockBalanceID = balanceID.Value,
                        WarehouseID = zone.WarehouseID,
                        BaseQuantity = stock.Quantity * stock.ConversionQty,
                        StockQuantity = stock.Quantity,
                        ReserveQuantity = 0,
                        ZoneID = location.ZoneID,
                        IsActive = true,
                        Remark = stock.Remark,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        UserCreated = UserID,
                        UserModified = UserID
                    };

                    stockLocationService.Add(locationBalance);
                }
                else
                {
                    locationBalance = stockLocationService.FindByID(locationBalanceID);

                    locationBalance.BaseQuantity += stock.Quantity * stock.ConversionQty;
                    locationBalance.StockQuantity += stock.Quantity;
                    locationBalance.IsActive = true;
                    locationBalance.Remark = stock.Remark;
                    locationBalance.UserModified = UserID;
                    locationBalance.DateModified = DateTime.Now;
                    stockLocationService.Modify(locationBalance);
                }

                #endregion

                #region Add 3 Transaction

                List<StockTransaction> stockTransList = new List<StockTransaction>();

                for (int i = 0; i < 3; i++)
                {
                    StockTransactionTypeEnum stockTransType = StockTransactionTypeEnum.Incomming;
                    string tmpPalletCode = stock.PalletCode;

                    if (i == 0)
                    {
                        // Insert Transaction In for Pallet
                        stockTransType = StockTransactionTypeEnum.Incomming;
                        tmpPalletCode = stock.PalletCode;
                    }
                    else if (i == 1)
                    {
                        // Insert Transaction Out for Pallet
                        stockTransType = StockTransactionTypeEnum.Outgoing;
                        tmpPalletCode = stock.PalletCode;
                    }
                    else if (i == 2)
                    {
                        // Insert Transaction In for Pallet_I
                        stockTransType = StockTransactionTypeEnum.Incomming;
                        tmpPalletCode = stock.Pallet_I_Code;
                    }

                    stockTransList.Add(new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = stockTransType,
                        LocationID = location.LocationID,
                        PalletCode = tmpPalletCode,
                        BaseQuantity = stock.ConversionQty * stock.Quantity,
                        ConversionQty = stock.ConversionQty,
                        StockLocationID = locationBalanceID.Value,
                        DocumentCode = stock.DocumentCode,
                        DocumentTypeID = stock.DocumentTypeID,
                        DocumentID = stock.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    });
                }

                stockTranService.AddRange(stockTransList);

                #endregion

                return true;
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

        public bool OutgoingAndIncomming(List<StockInOutModel> stockInOut)
        {
            try
            {
                stockInOut.ForEach(item =>
                {
                    Location location_out = _LocationService.Query().Filter(x => x.Code == item.FromLocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone_out = _ZoneService.FindByID(location_out.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    StockLocationBalance location_balance_out = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location_out.ZoneID
                                                                                   && x.WarehouseID == zone_out.WarehouseID).Get().SingleOrDefault();

                    if (location_balance_out == null)
                    {
                        #region no found location balance
                        location_balance_out = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone_out.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_out.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_out);

                        #endregion
                    }



                    StockTransaction trans_out = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Outgoing,
                        LocationID = location_out.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_out.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_out);


                    location_balance_out = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location_out.ZoneID
                                                                                   && x.WarehouseID == zone_out.WarehouseID).Get().SingleOrDefault();

                    location_balance_out.BaseQuantity -= item.ConversionQty * item.Quantity;
                    location_balance_out.StockQuantity -= item.Quantity;
                    location_balance_out.UserModified = UserID;
                    location_balance_out.DateModified = DateTime.Now;
                    _StockLocationService.Modify(location_balance_out);




                    Location location_in = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();

                    var zone_in = _ZoneService.FindByID(location_in.ZoneID);

                    StockLocationBalance location_balance_in = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location_in.ZoneID
                                                                                  && x.WarehouseID == zone_in.WarehouseID).Get().SingleOrDefault();

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone_in.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_in.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    {

                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }


                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Incomming,
                        LocationID = location_in.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);

                });

                return true;
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

        public bool ChangeStatus(List<StockInOutModel> stockOut, List<StockInOutModel> stockChange)
        {
            try
            {
                stockOut.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);
                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    StockLocationBalance location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    if (location_balance == null)
                    {
                        #region no found location balance
                        location_balance = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance);

                        #endregion
                    }



                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.ChangeStatusOut,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);

                    location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID ==zone.WarehouseID).Get().SingleOrDefault();

                    location_balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    location_balance.StockQuantity -= item.Quantity;
                    location_balance.UserModified = UserID;
                    location_balance.DateModified = DateTime.Now;
                    _StockLocationService.Modify(location_balance);

                    balance = _StockBalanceService.Query().Filter(x => x.StockBalanceID == location_balance.StockBalanceID).Get().SingleOrDefault();

                    balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    balance.StockQuantity -= item.Quantity;
                    balance.UserModified = UserID;
                    balance.DateModified = DateTime.Now;
                    _StockBalanceService.Modify(balance);
                });

                stockChange.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();

                    var zone = _ZoneService.FindByID(location.ZoneID);
                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);
                        #endregion
                    }

                    StockBalance balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();

                    if (balance == null)
                    {
                        #region  not found StockBalance
                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += (item.Quantity * item.ConversionQty);
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);
                    }

                    StockLocationBalance location_balance_in = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location.ZoneID
                                                                                  && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();


                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    {

                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.ChangeStatusIn,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);
                });


                return true;
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

        public bool InspectionReclassify(List<StockInOutModel> stockOut, List<StockInOutModel> stockChange)
        {
            try
            {
                StockBalance balance = new StockBalance();
                stockOut.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();


                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        StockBalance _balanceTmp = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                        balance = _StockBalanceService.FindByID(_balanceTmp.StockBalanceID);
                    }

                    StockLocationBalance _location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    StockLocationBalance location_balance = _StockLocationService.FindByID(_location_balance.StockLocationID);

                    if (location_balance == null)
                    {
                        #region no found location balance
                        location_balance = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance);

                        #endregion
                    }



                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.QAInspectionOut,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);

                    _location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();
                    location_balance = _StockLocationService.FindByID(_location_balance.StockLocationID);

                    location_balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    location_balance.StockQuantity -= item.Quantity;
                    location_balance.UserModified = UserID;
                    location_balance.DateModified = DateTime.Now;
                    _StockLocationService.Modify(location_balance);

                    StockBalance _balance = _StockBalanceService.Query().Filter(x => x.StockBalanceID == location_balance.StockBalanceID).Get().SingleOrDefault();
                    balance = _StockBalanceService.FindByID(_balance.StockBalanceID);

                    balance.BaseQuantity -= item.ConversionQty * item.Quantity;
                    balance.StockQuantity -= item.Quantity;
                    balance.UserModified = UserID;
                    balance.DateModified = DateTime.Now;
                    _StockBalanceService.Modify(balance);
                });

                stockChange.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);
                        #endregion
                    }

                    StockBalance _balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();

                    if (_balance == null)
                    {
                        #region  not found StockBalance
                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.FindByID(_balance.StockBalanceID);
                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += (item.Quantity * item.ConversionQty);
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);
                    }

                    StockLocationBalance _location_balance_in = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location.ZoneID
                                                                                  && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    StockLocationBalance location_balance_in = new StockLocationBalance();

                    if (_location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID =zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    {
                        location_balance_in = _StockLocationService.FindByID(_location_balance_in.StockLocationID);

                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.QAInspectionIn,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);
                });


                return true;
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

        public bool RestoreReserve(List<StockInOutModel> stockRestore, IUnitOfWork uow)
        {
            try
            {
                IRepository<Location> locationService = uow.Repository<Location>();
                IRepository<StockInfo> stockInfoService = uow.Repository<StockInfo>();
                IRepository<StockBalance> stockBalanceService = uow.Repository<StockBalance>();
                IRepository<StockLocationBalance> stockLocationService = uow.Repository<StockLocationBalance>();
                IRepository<StockTransaction> stockTransService = uow.Repository<StockTransaction>();
                IRepository<Zone> zoneService = uow.Repository<Zone>();

                stockRestore.ForEach(item =>
                {
                    Location location = locationService.Query().Filter(x => x.Code == item.LocationCode).Include(x => x.Zone).Get().SingleOrDefault();

                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = stockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );
                    if (stockINFO == null)
                    {
                        stockINFO = stockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate);
                    }


                    if (stockINFO == null)
                    {
                        throw new HILIException("MSG00037");
                    }
                    //throw new Exception("Stock Info not found.!!");

                    StockBalance balance = stockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();


                    if (balance == null)
                    {
                        throw new HILIException("MSG00038");
                    }
                    // throw new Exception("Stock Balance not found.!!");

                    StockLocationBalance location_balance = stockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();
                    if (location_balance == null)
                    {
                        throw new HILIException("MSG00042");
                    }
                    // throw new Exception("Stock Balance Location not found.!!");


                    StockLocationBalance tmp_location_balance = stockLocationService.FindByID(location_balance.StockLocationID);


                    tmp_location_balance.UserModified = UserID;
                    tmp_location_balance.DateModified = DateTime.Now;
                    tmp_location_balance.ReserveQuantity -= item.ReserveQuantity;

                    if (tmp_location_balance.ReserveQuantity < 0)
                    {
                        tmp_location_balance.ReserveQuantity = 0;
                    }

                    stockLocationService.Modify(tmp_location_balance);


                    //if (tmp_location_balance.ReserveQuantity < 0)
                    //    throw new HILIException("MSG00095");

                    StockBalance tmpBalance = stockBalanceService.FindByID(tmp_location_balance.StockBalanceID);


                    tmpBalance.UserModified = UserID;
                    tmpBalance.DateModified = DateTime.Now;
                    tmpBalance.ReserveQuantity -= item.ReserveQuantity.Value;
                    if (tmpBalance.ReserveQuantity < 0)
                    {
                        tmpBalance.ReserveQuantity = 0;
                    }

                    stockBalanceService.Modify(tmpBalance); 
                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = item.StockTransTypeEnum,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);


                    //if (tmpBalance.ReserveQuantity < 0)
                    //    throw new HILIException("MSG00096");
                });

                return true;
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

        public bool StockReserve(List<StockInOutModel> stockRestore, IUnitOfWork uow = null)
        {
            try
            {
                IRepository<Location> locationService = uow == null ? _LocationService : uow.Repository<Location>();
                IRepository<StockInfo> stockInfoService = uow == null ? _StockInfoService : uow.Repository<StockInfo>();
                IRepository<StockBalance> stockBalanceService = uow == null ? _StockBalanceService : uow.Repository<StockBalance>();
                IRepository<StockLocationBalance> stockLocationService = uow == null ? _StockLocationService : uow.Repository<StockLocationBalance>();
                IRepository<StockTransaction> stockTransService = uow == null ? _StockTransService : uow.Repository<StockTransaction>();
                IRepository<Zone> zoneService = uow == null ? _ZoneService : uow.Repository<Zone>();

                stockRestore.ForEach(item =>
                {
                    Location location = locationService.FirstOrDefault(x => x.Code == item.LocationCode && x.IsActive);//.Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = zoneService.FindByID(location.ZoneID);
                    StockInfo stockINFO = stockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );


                    if (stockINFO == null)
                    {
                        throw new HILIException("MSG00037");
                    }
                    //throw new Exception("Stock Info not found.!!");

                    StockBalance balance = stockBalanceService.FirstOrDefault(x => x.StockInfoID == stockINFO.StockInfoID);


                    if (balance == null)
                    {
                        throw new HILIException("MSG00038");
                    }
                    // throw new Exception("Stock Balance not found.!!");

                    StockLocationBalance location_balance = stockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID
                                                                && x.ZoneID == location.ZoneID
                                                                && x.WarehouseID == zone.WarehouseID
                                                                && x.IsActive);
                    if (location_balance == null)
                    {
                        throw new HILIException("MSG00042");
                    }
                    // throw new Exception("Stock Balance Location not found.!!"); 
                    StockLocationBalance tmp_location_balance = stockLocationService.FirstOrDefault(x => x.StockLocationID == location_balance.StockLocationID && x.IsActive);
                    tmp_location_balance.UserModified = UserID;
                    tmp_location_balance.DateModified = DateTime.Now;
                    tmp_location_balance.ReserveQuantity += item.ReserveQuantity;

                    if (tmp_location_balance.ReserveQuantity < 0)
                    {
                        tmp_location_balance.ReserveQuantity = 0;
                    }
                    stockLocationService.Modify(tmp_location_balance);                    
                    //////if (tmp_location_balance.ReserveQuantity < 0)
                    //////    throw new HILIException("MSG00095");
                    //  StockBalance tmpBalance = stockBalanceService.FindByID(tmp_location_balance.StockBalanceID);
                    StockBalance tmpBalance = stockBalanceService.FirstOrDefault(x => x.StockBalanceID == tmp_location_balance.StockBalanceID && x.IsActive);
                    tmpBalance.UserModified = UserID;
                    tmpBalance.DateModified = DateTime.Now;
                    tmpBalance.ReserveQuantity += item.ReserveQuantity.GetValueOrDefault();

                    if (tmpBalance.ReserveQuantity < 0)
                    {
                        tmpBalance.ReserveQuantity = 0;
                    }

                    stockBalanceService.Modify(tmpBalance);
                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = item.StockTransTypeEnum,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now
                    };
                    _StockTransService.Add(trans_in);
                });
                return true;
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

        #region [StockIn Adjust]

        public bool AdjustIncomming(List<StockInOutModel> stockIn)
        {
            try
            {
                stockIn.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);
                        #endregion
                    }

                    StockBalance balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();

                    if (balance == null)
                    {
                        #region  not found StockBalance
                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += (item.Quantity * item.ConversionQty);
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);
                    }

                    StockLocationBalance location_balance_in = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID &&
                                                                                x.ZoneID == location.ZoneID &&
                                                                                x.WarehouseID == zone.WarehouseID)
                                                                                .Get().SingleOrDefault();

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    {
                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.AdjustIn,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                    };
                    _StockTransService.Add(trans_in);

                    ProductionControlDetail modifypackingDetail = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == item.PalletCode).Get().SingleOrDefault();
                    if (modifypackingDetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    modifypackingDetail.LocationID = location.LocationID;
                    modifypackingDetail.WarehouseID =zone.WarehouseID;
                    modifypackingDetail.PackingStatus = PackingStatusEnum.PutAway;
                    modifypackingDetail.StockQuantity += item.Quantity;
                    modifypackingDetail.BaseQuantity += item.Quantity * modifypackingDetail.ConversionQty;
                    modifypackingDetail.RemainQTY += item.Quantity;
                    modifypackingDetail.RemainBaseQTY = modifypackingDetail.RemainQTY * modifypackingDetail.ConversionQty;
                    ProductionControlDetailService.Modify(modifypackingDetail);
                });

                return true;
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
        public bool AdjustOutgoing(List<StockInOutModel> stockOut)
        {
            try
            {
                stockOut.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    StockLocationBalance location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    if (location_balance == null)
                    {
                        #region no found location balance
                        location_balance = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * (item.Quantity * (-1)),
                            StockQuantity = (item.Quantity * (-1)),
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance);

                        #endregion
                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.AdjustOut,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * (item.Quantity * (-1)),
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);

                    location_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    location_balance.BaseQuantity -= item.ConversionQty * (item.Quantity * (-1));
                    location_balance.StockQuantity -= (item.Quantity * (-1));
                    location_balance.UserModified = UserID;
                    location_balance.DateModified = DateTime.Now;
                    _StockLocationService.Modify(location_balance);

                    balance = _StockBalanceService.Query().Filter(x => x.StockBalanceID == location_balance.StockBalanceID).Get().SingleOrDefault();

                    balance.BaseQuantity -= item.ConversionQty * (item.Quantity * (-1));
                    balance.StockQuantity -= (item.Quantity * (-1));
                    balance.UserModified = UserID;
                    balance.DateModified = DateTime.Now;
                    _StockBalanceService.Modify(balance);

                    ProductionControlDetail modifypackingDetail = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == item.PalletCode).Get().SingleOrDefault();
                    if (modifypackingDetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    modifypackingDetail.StockQuantity -= (item.Quantity * (-1));
                    modifypackingDetail.BaseQuantity -= (item.Quantity * (-1)) * modifypackingDetail.ConversionQty;
                    modifypackingDetail.RemainQTY -= (item.Quantity * (-1));
                    modifypackingDetail.RemainBaseQTY = modifypackingDetail.RemainQTY * modifypackingDetail.ConversionQty;
                    modifypackingDetail.LocationID = modifypackingDetail.StockQuantity == 0 ? null : modifypackingDetail.LocationID;
                    ProductionControlDetailService.Modify(modifypackingDetail);

                    if (modifypackingDetail.StockQuantity == 0)
                    {
                        location.LocationReserveQty -= 1;
                        _LocationService.Modify(location);
                    }
                });

                return true;
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

        public bool AdjustOthergoing(List<StockInOutModel> stockOut)
        {
            try
            {
                StockTransactionTypeEnum stockTransType = StockTransactionTypeEnum.AdjustIn;

                stockOut.ForEach(item =>
                {
                    Location location = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();
                    var zone = _ZoneService.FindByID(location.ZoneID);

                    StockInfo stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    StockBalance balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * (item.Quantity < 0 ? Math.Abs(item.Quantity) : item.Quantity),
                            ConversionQty = item.ConversionQty,
                            StockQuantity = (item.Quantity < 0 ? Math.Abs(item.Quantity) : item.Quantity),
                            ReserveQuantity = 0,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    StockLocationBalance tmpLocation_balance = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location.ZoneID
                                                                                   && x.WarehouseID == zone.WarehouseID).Get().SingleOrDefault();

                    if (tmpLocation_balance == null)
                    {
                        #region no found location balance
                        tmpLocation_balance = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * (item.Quantity < 0 ? Math.Abs(item.Quantity) : item.Quantity),
                            StockQuantity = (item.Quantity < 0 ? Math.Abs(item.Quantity) : item.Quantity),
                            ReserveQuantity = 0,
                            ZoneID = location.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(tmpLocation_balance);

                        #endregion
                    }

                    StockLocationBalance location_balance = _StockLocationService.FindByID(tmpLocation_balance.StockLocationID);

                    if (item.Quantity > 0)
                    {
                        location_balance.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance.StockQuantity += item.Quantity;
                        location_balance.UserModified = UserID;
                        location_balance.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance);
                    }

                    if (item.Quantity < 0)
                    {
                        location_balance.BaseQuantity -= item.ConversionQty * Math.Abs(item.Quantity);
                        location_balance.StockQuantity -= Math.Abs(item.Quantity);
                        location_balance.UserModified = UserID;
                        location_balance.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance);
                    }

                    balance = _StockBalanceService.FindByID(location_balance.StockBalanceID);
                    //balance = _StockBalanceService.Query().Filter(x => x.StockBalanceID == location_balance.StockBalanceID).Get().SingleOrDefault();

                    if (item.Quantity > 0)
                    {
                        stockTransType = StockTransactionTypeEnum.AdjustIn;

                        balance.StockQuantity += item.Quantity;
                        balance.BaseQuantity += item.Quantity * item.ConversionQty;
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);
                    }
                    if (item.Quantity < 0)
                    {
                        stockTransType = StockTransactionTypeEnum.AdjustOut;

                        balance.BaseQuantity -= item.ConversionQty * Math.Abs(item.Quantity);
                        balance.StockQuantity -= Math.Abs(item.Quantity);
                        balance.UserModified = UserID;
                        balance.DateModified = DateTime.Now;
                        _StockBalanceService.Modify(balance);
                    }

                    ProductionControlDetail modifypackingDetail = ProductionControlDetailService.Query().Filter(x => x.IsActive && x.PalletCode == item.PalletCode).Get().SingleOrDefault();
                    if (modifypackingDetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    if (item.Quantity > 0)
                    {
                        modifypackingDetail.LocationID = location.LocationID;
                        modifypackingDetail.WarehouseID =zone.WarehouseID;
                        modifypackingDetail.PackingStatus = PackingStatusEnum.PutAway;
                        modifypackingDetail.StockQuantity += item.Quantity;
                        modifypackingDetail.BaseQuantity += item.Quantity * modifypackingDetail.ConversionQty;
                        modifypackingDetail.RemainQTY += item.Quantity;
                        modifypackingDetail.RemainBaseQTY = modifypackingDetail.RemainQTY * modifypackingDetail.ConversionQty;
                        ProductionControlDetailService.Modify(modifypackingDetail);
                    }
                    if (item.Quantity < 0)
                    {

                        modifypackingDetail.StockQuantity -= Math.Abs(item.Quantity);
                        modifypackingDetail.BaseQuantity -= Math.Abs(item.Quantity) * modifypackingDetail.ConversionQty;
                        modifypackingDetail.RemainQTY -= Math.Abs(item.Quantity);
                        modifypackingDetail.RemainBaseQTY = modifypackingDetail.RemainQTY * modifypackingDetail.ConversionQty;
                        modifypackingDetail.LocationID = modifypackingDetail.StockQuantity == 0 ? null : modifypackingDetail.LocationID;
                        ProductionControlDetailService.Modify(modifypackingDetail);

                        if (modifypackingDetail.StockQuantity == 0)
                        {

                            Location tempLocation = _LocationService.FindByID(location.LocationID);
                            tempLocation.LocationReserveQty -= 1;
                            _LocationService.Modify(tempLocation);
                        }

                    }

                    StockTransaction trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = stockTransType,
                        LocationID = location.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * (item.Quantity < 0 ? Math.Abs(item.Quantity) : item.Quantity),
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };

                    _StockTransService.Add(trans_in);
                });

                return true;
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

        #endregion [StockIn Adjust]

        #region [ReLocation]

        public bool UpdateLocationOutgoingAndIncomming(List<StockInOutModel> stockInOut)
        {
            try
            {
                stockInOut.ForEach(item =>
                {
                    var location_out = _LocationService.Query().Filter(x => x.Code == item.FromLocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();


                    var stockINFO = _StockInfoService.Query().Filter(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                ).Get().SingleOrDefault();

                    var balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = this.UserID,
                            UserModified = this.UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = this.UserID,
                            UserModified = this.UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    var location_balance_out = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                   && x.ZoneID == location_out.ZoneID
                                                                                   && x.WarehouseID == location_out.Zone.WarehouseID).Get().SingleOrDefault();

                    if (location_balance_out == null)
                    {
                        #region no found location balance
                        location_balance_out = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = location_out.Zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_out.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_out);

                        #endregion
                    }

                    var trans_out = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Outgoing,
                        LocationID = location_out.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_out.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                    };

                    _StockTransService.Add(trans_out);

                    location_balance_out = _StockLocationService.FindByID(location_balance_out.StockLocationID);

                    location_balance_out.BaseQuantity -= item.ConversionQty * item.Quantity;
                    location_balance_out.StockQuantity -= item.Quantity;
                    location_balance_out.UserModified = UserID;
                    location_balance_out.DateModified = DateTime.Now;
                    _StockLocationService.Modify(location_balance_out);

                    var location_in = _LocationService.Query().Filter(x => x.Code == item.LocationCode && x.IsActive).Include(x => x.Zone).Get().SingleOrDefault();

                    var location_balance_in = _StockLocationService.Query().Filter(x => x.StockBalanceID == balance.StockBalanceID
                                                                                  && x.ZoneID == location_in.ZoneID
                                                                                  && x.WarehouseID == location_in.Zone.WarehouseID).Get().SingleOrDefault();

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = location_in.Zone.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_in.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID
                        };

                        _StockLocationService.Add(location_balance_in);

                        #endregion
                    }
                    else
                    { 
                        location_balance_in = _StockLocationService.FindByID(location_balance_in.StockLocationID);
                        location_balance_in.BaseQuantity += item.Quantity * item.ConversionQty;
                        location_balance_in.StockQuantity += item.Quantity;
                        location_balance_in.UserModified = UserID;
                        location_balance_in.DateModified = DateTime.Now;
                        _StockLocationService.Modify(location_balance_in);
                    }

                    var trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Incomming,
                        LocationID = location_in.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);
                });

                return true;
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

        public bool TransferOutgoingAndIncomming(List<StockInOutModel> stockInOut)
        {
            try
            {
                stockInOut.ForEach(item =>
                {
                    var location_out = _LocationService.FirstOrDefault(x => x.Code == item.FromLocationCode && x.IsActive);
                    var zone_out = _ZoneService.FindByID(location_out.ZoneID);
                    var stockINFO = _StockInfoService.FirstOrDefault(x => x.ProductID == item.ProductID
                                                                && x.StockUnitID == item.StockUnitID
                                                                && x.BaseUnitID == item.BaseUnitID
                                                                && x.ConversionQty == item.ConversionQty
                                                                && x.Lot == item.Lot
                                                                && x.ProductOwnerID == item.ProductOwnerID
                                                                && x.ProductStatusID == item.ProductStatusID
                                                                && x.SupplierID == item.SupplierID
                                                                && x.ManufacturingDate == item.ManufacturingDate
                                                                && x.ExpirationDate == item.ExpirationDate
                                                                );

                    var balance = new StockBalance();

                    if (stockINFO == null)
                    {
                        #region insert StockInfo
                        stockINFO = new StockInfo
                        {
                            StockInfoID = Guid.NewGuid(),
                            ProductOwnerID = item.ProductOwnerID,
                            SupplierID = item.SupplierID,
                            ProductID = item.ProductID,
                            Lot = item.Lot,
                            ExpirationDate = item.ExpirationDate,
                            ManufacturingDate = item.ManufacturingDate,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            StockUnitID = item.StockUnitID,
                            BaseUnitID = item.BaseUnitID,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ConversionQty = item.ConversionQty,
                            ProductStatusID = item.ProductStatusID,
                            ProductSubStatusID = item.ProductSubStatusID,
                            UserCreated = this.UserID,
                            UserModified = this.UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true
                        };
                        _StockInfoService.Add(stockINFO);

                        balance = new StockBalance
                        {
                            StockBalanceID = Guid.NewGuid(),
                            StockInfoID = stockINFO.StockInfoID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            ConversionQty = item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            UserCreated = this.UserID,
                            UserModified = this.UserID,
                            DateModified = DateTime.Now,
                            DateCreated = DateTime.Now,
                            IsActive = true

                        };
                        _StockBalanceService.Add(balance);
                        #endregion
                    }
                    else
                    {
                        balance = _StockBalanceService.Query().Filter(x => x.StockInfoID == stockINFO.StockInfoID).Get().SingleOrDefault();
                    }

                    var location_balance_out = _StockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID && x.ZoneID == location_out.ZoneID && x.WarehouseID == zone_out.WarehouseID);

                    if (location_balance_out == null)
                    {
                        #region no found location balance
                        location_balance_out = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = location_out.Zone.WarehouseID,
                            BaseQuantity = item.ConversionQty * item.Quantity,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_out.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = this.UserID,
                            UserModified = this.UserID
                        };

                        _StockLocationService.Add(location_balance_out);

                        #endregion
                    }

                    var trans_out = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Transfer412Out,
                        LocationID = location_out.LocationID,
                        PalletCode = item.PalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_out.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = this.UserID,
                        DateCreated = DateTime.Now,
                        UserModified = this.UserID,
                        DateModified = DateTime.Now,
                    };

                    _StockTransService.Add(trans_out);
                    location_balance_out = _StockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID && x.ZoneID == location_out.ZoneID && x.WarehouseID == zone_out.WarehouseID);                    
                    
                    var _sb = Context.Repository<StockLocationBalance>();
                    var locationbalanceOut = _sb.FindByID(location_balance_out.StockLocationID);

                    locationbalanceOut.BaseQuantity -= item.ConversionQty * item.Quantity;
                    locationbalanceOut.StockQuantity -= item.Quantity;
                    locationbalanceOut.UserModified = this.UserID;
                    locationbalanceOut.DateModified = DateTime.Now;

                    _sb.Modify(locationbalanceOut);

                    var location_in = _LocationService.FirstOrDefault(x => x.IsActive == true && x.Code == item.LocationCode);
                    var zone_in = _ZoneService.FindByID(location_in.ZoneID);
                    var location_balance_in = _StockLocationService.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID && x.ZoneID == location_in.ZoneID && x.WarehouseID == zone_in.WarehouseID);

                    if (location_balance_in == null)
                    {
                        #region not found location balance -in
                        location_balance_in = new StockLocationBalance
                        {
                            StockLocationID = Guid.NewGuid(),
                            StockBalanceID = balance.StockBalanceID,
                            WarehouseID = zone_in.WarehouseID,
                            BaseQuantity = item.Quantity * item.ConversionQty,
                            StockQuantity = item.Quantity,
                            ReserveQuantity = 0,
                            ZoneID = location_in.ZoneID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = this.UserID,
                            UserModified = this.UserID
                        };
                        _StockLocationService.Add(location_balance_in);
                        #endregion
                    }
                    else
                    {
                        var _stockLocationBalance = Context.Repository<StockLocationBalance>();
                        var stockLocationBalance = _stockLocationBalance.FirstOrDefault(x => x.StockBalanceID == balance.StockBalanceID && x.ZoneID == location_in.ZoneID && x.WarehouseID == zone_in.WarehouseID);
                        var SLB_In = _stockLocationBalance.FindByID(stockLocationBalance.StockLocationID);
                        SLB_In.BaseQuantity += item.Quantity * item.ConversionQty;
                        SLB_In.StockQuantity += item.Quantity;
                        SLB_In.UserModified = this.UserID;
                        SLB_In.DateModified = DateTime.Now;
                        _stockLocationBalance.Modify(SLB_In);
                    }

                    var trans_in = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        StockTransType = StockTransactionTypeEnum.Transfer412In,
                        LocationID = location_in.LocationID,
                        PalletCode = item.NewPalletCode,
                        BaseQuantity = item.ConversionQty * item.Quantity,
                        ConversionQty = item.ConversionQty,
                        StockLocationID = location_balance_in.StockLocationID,
                        DocumentCode = item.DocumentCode,
                        DocumentTypeID = item.DocumentTypeID,
                        DocumentID = item.DocumentID,
                        IsActive = true,
                        UserCreated = this.UserID,
                        DateCreated = DateTime.Now,
                        UserModified = this.UserID,
                        DateModified = DateTime.Now,

                    };
                    _StockTransService.Add(trans_in);
                });

                return true;
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

        #endregion [ReLocation]
    }
}
