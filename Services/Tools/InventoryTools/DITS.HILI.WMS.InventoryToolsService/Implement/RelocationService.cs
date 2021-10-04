using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class RelocationService : Repository<RelocationModel>, IRelocationService
    {
        #region Property 
        private readonly IRepository<ProductionControlDetail> packingDetailService; 
        private readonly IRepository<Location> locationService;
        private readonly IRepository<Zone> ZoneService; 
        private readonly IStockService stockService;
        #endregion

        #region Constructor

        public RelocationService(IUnitOfWork dbContext, 
                                    IRepository<ProductionControlDetail> _packingDetailService, 
                                    IRepository<Zone> _Zone, 
                                     IStockService _stockService,
                                    IRepository<Location> _location)
            : base(dbContext)
        { 
            ZoneService = _Zone;
            stockService = _stockService;
            packingDetailService = _packingDetailService;
            locationService = _location;
        }

        #endregion

        public RelocationModel GetAll(string keyword)
        {
            try
            {

                SqlParameter param = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = keyword ?? "" };
                //var results = new List<DPDetailItemBackOrder>();

                RelocationModel ret = Context.SQLQuery<RelocationModel>("exec SP_GetPalletRelocationHH @PalletCode", param).FirstOrDefault();


                return ret;
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

        public bool UpdateLocation(string NewLocation, string LotNumber)
        {
            try
            {
                var _relocationDetail = this.GetAll(LotNumber);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (_relocationDetail == null)
                        throw new HILIException("MSG00105");
                    var _location = locationService.Query().Filter(x => x.Code == NewLocation).Include(x => x.Zone).Get().SingleOrDefault(); 

                    if (_location.LocationReserveQty == null)
                        throw new HILIException("MSG00006");

                    if ((_location.PalletCapacity.GetValueOrDefault() - _location.LocationReserveQty.GetValueOrDefault()) == 0)
                        throw new HILIException("MSG00006");

                    List<StockInOutModel> stockOut = new List<StockInOutModel>
                    {
                        new StockInOutModel
                        {
                            DocumentID = _relocationDetail.PackingID,
                            PalletCode = _relocationDetail.PalletCode,
                            DocumentCode = "RL",
                            ProductID = _relocationDetail.ProductID,
                            Lot = _relocationDetail.Product_Lot,
                            ExpirationDate = _relocationDetail.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = _relocationDetail.MFGDate.GetValueOrDefault(),
                            StockUnitID = _relocationDetail.RemainStockUnitID.GetValueOrDefault(),
                            BaseUnitID = _relocationDetail.RemainBaseUnitID.GetValueOrDefault(),
                            ConversionQty = _relocationDetail.ConversionQty.GetValueOrDefault(),
                            ProductStatusID = _relocationDetail.ProductStatusID.GetValueOrDefault(),
                            ProductSubStatusID  = _relocationDetail.ProductSubStatusID.GetValueOrDefault(),
                            LocationID = _relocationDetail.LocationID.GetValueOrDefault(),
                            FromLocationCode = _relocationDetail.LocationNo,
                            LocationCode = NewLocation,
                            Quantity = _relocationDetail.StockQuantity.GetValueOrDefault(),
                            ProductOwnerID = _relocationDetail.ProductOwnerID.GetValueOrDefault(),
                            SupplierID = _relocationDetail.SupplierID.GetValueOrDefault()
                        }
                    };

                    stockService.UserID = UserID;
                    stockService.UpdateLocationOutgoingAndIncomming(stockOut);

                    var locationReserveQtyOut = locationService.Query().Filter(x => x.Code == _relocationDetail.LocationNo).Get().SingleOrDefault();

                    var _locationReserveQtyOut = locationService.FindByID(locationReserveQtyOut.LocationID);
                    if (_locationReserveQtyOut != null)
                    {
                        _locationReserveQtyOut.LocationReserveQty -= 1;
                        _locationReserveQtyOut.UserModified = UserID;
                        _locationReserveQtyOut.DateModified = DateTime.Now;
                        locationService.Modify(_locationReserveQtyOut);
                    }

                    var locationReserveQtyIn = locationService.Query().Filter(x => x.Code == NewLocation).Get().SingleOrDefault();
                    var _locationReserveQtyIn = locationService.FindByID(locationReserveQtyIn.LocationID);
                    if (_locationReserveQtyIn != null)
                    {
                        _locationReserveQtyIn.LocationReserveQty += 1;
                        _locationReserveQtyIn.UserModified = UserID;
                        _locationReserveQtyIn.DateModified = DateTime.Now;
                        locationService.Modify(_locationReserveQtyIn);
                    }

                    var _modifypackingDetail = packingDetailService.Query().Filter(x => x.IsActive && x.PalletCode == LotNumber).Get().SingleOrDefault();
                    var modifypackingDetail = packingDetailService.FindByID(_modifypackingDetail.PackingID);
                    if (modifypackingDetail == null)
                        throw new HILIException("MSG00006");

                    modifypackingDetail.LocationID = _location.LocationID;
                    modifypackingDetail.WarehouseID = _location.Zone.WarehouseID;
                    packingDetailService.Modify(modifypackingDetail); 
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
        public bool UpdateLocation_New(string NewLocation, string LotNumber)
        {
            try
            {
                RelocationModel _relocationDetail = GetAll(LotNumber);
                if (_relocationDetail == null)
                {
                    throw new HILIException("MSG00105");
                }

                Location _location = locationService.FirstOrDefault(x => x.Code == NewLocation);
                if (_location == null)
                {
                    throw new HILIException("MSG00042");
                }
                var _zone = ZoneService.FindByID(_location.ZoneID);

                if (!_location.LocationReserveQty.HasValue)
                {
                    throw new HILIException("MSG00006");
                }

                if ((_location.PalletCapacity.GetValueOrDefault() - _location.LocationReserveQty.GetValueOrDefault()) == 0)
                {
                    throw new HILIException("MSG00006");
                } 
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                { 
                    List<StockInOutModel> stockOut = new List<StockInOutModel>
                    {
                        new StockInOutModel
                        {
                            DocumentID = _relocationDetail.PackingID,
                            PalletCode = _relocationDetail.PalletCode,
                            DocumentCode = "RL",
                            ProductID = _relocationDetail.ProductID,
                            Lot = _relocationDetail.Product_Lot,
                            ExpirationDate = _relocationDetail.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = _relocationDetail.MFGDate.GetValueOrDefault(),
                            StockUnitID = _relocationDetail.RemainStockUnitID.GetValueOrDefault(),
                            BaseUnitID = _relocationDetail.RemainBaseUnitID.GetValueOrDefault(),
                            ConversionQty = _relocationDetail.ConversionQty.GetValueOrDefault(),
                            ProductStatusID = _relocationDetail.ProductStatusID.GetValueOrDefault(),
                            ProductSubStatusID  = _relocationDetail.ProductSubStatusID.GetValueOrDefault(),
                            LocationID = _relocationDetail.LocationID.GetValueOrDefault(),
                            FromLocationCode = _relocationDetail.LocationNo,
                            LocationCode = NewLocation,
                            Quantity = _relocationDetail.StockQuantity.GetValueOrDefault(),
                            ProductOwnerID = _relocationDetail.ProductOwnerID.GetValueOrDefault(),
                            SupplierID = _relocationDetail.SupplierID.Value
                        }
                    };

                    stockService.UserID = UserID;
                    stockService.UpdateLocationOutgoingAndIncomming(stockOut);

                    Location locationReserveQtyOut = locationService.FirstOrDefault(x => x.Code == _relocationDetail.LocationNo);

                    Location _locationReserveQtyOut = locationService.FindByID(locationReserveQtyOut.LocationID);
                    if (_locationReserveQtyOut != null)
                    {
                        _locationReserveQtyOut.LocationReserveQty -= 1;
                        _locationReserveQtyOut.UserModified = UserID;
                        _locationReserveQtyOut.DateModified = DateTime.Now;
                        locationService.Modify(_locationReserveQtyOut);
                    }

                    Location locationReserveQtyIn = locationService.FirstOrDefault(x => x.Code == NewLocation);
                    Location _locationReserveQtyIn = locationService.FindByID(locationReserveQtyIn.LocationID);
                    if (_locationReserveQtyIn != null)
                    {
                        _locationReserveQtyIn.LocationReserveQty += 1;
                        _locationReserveQtyIn.UserModified = UserID;
                        _locationReserveQtyIn.DateModified = DateTime.Now;
                        locationService.Modify(_locationReserveQtyIn);
                    }

                    ProductionControlDetail _modifypackingDetail = packingDetailService.FirstOrDefault(x => x.IsActive && x.PalletCode == LotNumber);
                    ProductionControlDetail modifypackingDetail = packingDetailService.FindByID(_modifypackingDetail.PackingID);
                    if (modifypackingDetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    modifypackingDetail.LocationID = _location.LocationID;
                    modifypackingDetail.WarehouseID = _zone.WarehouseID;
                    packingDetailService.Modify(modifypackingDetail);

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
        //    #endregion [CycleCount HandHeld]
    }
}

