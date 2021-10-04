using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.RegisterTruckModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.PickingService
{
    public class PickingService : Repository<Picking>, IPickingService
    {
        #region Property 
        private readonly IRepository<Picking> _PickingService;
        private readonly IRepository<PickingAssign> _PickingAssignService;
        private readonly IRepository<PickingDetail> _PickingDetailService;
        private readonly IRepository<RegisterTruck> _RegisterTruckService;
        private readonly IRepository<RegisterTruckDetail> _RegisterTruckDetailService;
        private readonly IRepository<Dispatch> _DispatchService;
        private readonly IRepository<DispatchDetail> _DispatchDetailService;
        private readonly IRepository<DispatchBooking> _DispatchBookingService;
        private readonly IRepository<Product> _ProductService;
        private readonly IRepository<ProductCodes> _ProductCodeService;
        private readonly IRepository<ProductUnit> _ProductUnitService;
        private readonly IRepository<DockConfig> _DockConfigService;
        private readonly IRepository<Location> _LocationService;
        private readonly IRepository<ShippingTo> _ShiptoService;
        private readonly IRepository<Contact> _ContactService;
        private readonly IRepository<ProductionControl> _ProductionControlService;
        private readonly IRepository<ProductionControlDetail> _PCDetailService;
        private readonly IRepository<UserGroups> _UserGroupsService;
        private readonly IRepository<UserInGroup> _UserInGroupService;
        private readonly IRepository<PickingPrefix> _PickingPrefixService;       
        private readonly IRepository<ItfInterfaceMapping> _ItfInterfaceMappingService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<Line> lineService; 
        private readonly IStockService _StockService;
        private readonly IRepository<Zone> _ZoneService;  
        private readonly IRepository<StockInfo> _StockInfoService;
        private readonly IRepository<StockBalance> _StockBalanceService;
        private readonly IRepository<StockLocationBalance> _StockLocationBalanceService;
        private readonly IRepository<StockTransaction> _StockTransService;
        private readonly IRepository<PickingProductAssign> _PickingProductAssign;

        #endregion

        public PickingService(IUnitOfWork context, IStockService stockService) : base(context)
        {
            _PickingService = context.Repository<Picking>(); 
            _PickingAssignService = context.Repository<PickingAssign>();
            _PickingDetailService = context.Repository<PickingDetail>();
            _RegisterTruckService = context.Repository<RegisterTruck>();
            _RegisterTruckDetailService = context.Repository<RegisterTruckDetail>();
            _DispatchService = context.Repository<Dispatch>();
            _DispatchDetailService = context.Repository<DispatchDetail>();
            _DispatchBookingService = context.Repository<DispatchBooking>();
            _ProductService = context.Repository<Product>();
            _ProductCodeService = context.Repository<ProductCodes>();
            _ProductUnitService = context.Repository<ProductUnit>();
            _DockConfigService = context.Repository<DockConfig>();
            _LocationService = context.Repository<Location>();
            _ShiptoService = context.Repository<ShippingTo>();
            _ContactService = context.Repository<Contact>();
            _ProductionControlService = context.Repository<ProductionControl>();
            _PCDetailService = context.Repository<ProductionControlDetail>();
            _UserGroupsService = context.Repository<UserGroups>();
            _UserInGroupService = context.Repository<UserInGroup>();
            _PickingPrefixService = context.Repository<PickingPrefix>();
            _StockInfoService = context.Repository<StockInfo>();
            _ItfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
            receivingService = context.Repository<Receiving>();
            lineService = context.Repository<Line>();
            _StockService = stockService;
            _ZoneService = context.Repository<Zone>(); 
            _StockBalanceService = context.Repository<StockBalance>();
            _StockLocationBalanceService = context.Repository<StockLocationBalance>();
            _PickingProductAssign = context.Repository<PickingProductAssign>();
        }

        public bool Approve(AssignJobModel model)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (model.PickingID == null || model.PickingID == Guid.Empty)
                    {
                        throw new HILIException("MSG00048");
                    }

                    Picking picking = FindByID(model.PickingID);
                    picking.PickingStatus = PickingStatusEnum.Pick;
                    picking.UserModified = UserID;
                    picking.DateModified = DateTime.Now;
                    base.Modify(picking);
                    List<PickingAssign> assigns = _PickingAssignService.Query().Filter(x => x.IsActive && x.PickingID == model.PickingID).Get().ToList();                    
                    assigns.ForEach(x =>
                    {
                        x.AssignStatus = PickingStatusEnum.Pick;
                        x.UserModified = UserID;
                        x.DateModified = DateTime.Now;
                        _PickingAssignService.Modify(x);
                        if (x.RefPalletCode != x.PalletCode)
                        {
                            Guid refLocationID = Guid.Empty;
                            Guid locationID = Guid.Empty;
                            AdjustReserve(x, out refLocationID, out locationID);
                            StockSearch stockUnReserve = GetStockReserveModel(x, x.StockQuantity.Value, true, refLocationID);
                            StockSearch stockReserve = GetStockReserveModel(x, x.StockQuantity.Value, false, locationID);
                            _StockService.UserID = UserID;
                            _StockService.AdjustReserve(stockUnReserve, StockReserveTypeEnum.UnReserve);
                            _StockService.AdjustReserve(stockReserve, StockReserveTypeEnum.Reserve);
                        }
                    });
                    var assignByProduct = assigns.GroupBy(e => new { e.ProductID, e.StockUnitID, e.BaseUnitID }).ToList();
                    assignByProduct.ForEach(adp =>
                    {
                        var productAssign = _PickingProductAssign.FirstOrDefault(e => e.IsActive
                                                                 && e.PickingID == model.PickingID
                                                                 && e.ProductID == adp.Key.ProductID
                                                                 && e.StockUnitID == adp.Key.StockUnitID
                                                                 && e.BaseUnitID == adp.Key.BaseUnitID);
                        if (productAssign != null)
                        {
                            productAssign.IsActive = false;
                            _PickingProductAssign.Modify(productAssign);
                        }
                        var productAssignHeadCount = assigns.Where(e =>
                                                       e.BaseUnitID == adp.Key.BaseUnitID
                                                       && e.StockUnitID == adp.Key.StockUnitID
                                                       && e.ProductID == adp.Key.ProductID
                                                       && e.AssignStatus == PickingStatusEnum.Pick
                                                       && e.IsActive).Sum(e => e.StockQuantity.GetValueOrDefault());
                        productAssign = new PickingProductAssign()
                        {
                            BaseQuantity = productAssignHeadCount * (adp.FirstOrDefault().BaseQuantity.GetValueOrDefault() / adp.FirstOrDefault().StockQuantity.GetValueOrDefault()),
                            BaseUnitID = adp.Key.BaseUnitID.GetValueOrDefault(),
                            IsActive = true,
                            PickingID = model.PickingID.GetValueOrDefault(),
                            ProductAssignID = Guid.NewGuid(),
                            ProductID = adp.Key.ProductID.GetValueOrDefault(),
                            StockQuantity = productAssignHeadCount,
                            StockUnitID = adp.Key.StockUnitID.GetValueOrDefault(),
                            UserCreated = UserID,
                            UserModified = UserID,
                        };
                        _PickingProductAssign.Add(productAssign);
                    }); 
                    List<Guid?> bookingID = assigns.Select(x => x.BookingID).ToList();
                    List<DispatchBooking> bookings = _DispatchBookingService.Query().Filter(x => bookingID.Contains(x.BookingId)).Get().ToList();

                    bookings.ForEach(x =>
                    {
                        decimal _sumBookingQty = _DispatchBookingService.Query().Filter(s => s.IsActive == true && s.BookingId == x.BookingId).Get().Sum(s => s.BookingQty);
                        decimal? _sumConfirmQty = _RegisterTruckDetailService.Query().Filter(s => s.IsActive == true && s.BookingID == x.BookingId).Get().Sum(s => s.ConfirmQuantity);
                        if (_sumBookingQty == _sumConfirmQty)
                        {
                            x.BookingStatus = BookingStatusEnum.Complete;
                            x.UserModified = UserID;
                            x.DateModified = DateTime.Now;
                            _DispatchBookingService.Modify(x);
                        }
                    });
                    scope.Complete();
                }
                return true;
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

        public bool Save(AssignJobModel model)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (model == null)
                    {
                        throw new HILIException("MSG00006"); // data not found
                    }

                    if (model.PickingID != Guid.Empty)
                    {
                        // Modified
                        #region Update Picking Assign

                        if (model.AssignJobDetailCollection.Count() > 0)
                        {
                            List<AssignJobDetailModel> assignModel = model.AssignJobDetailCollection.ToList();
                            List<Guid?> assignIDs = assignModel.Select(x => x.AssignID).ToList();

                            List<PickingAssign> assigns = _PickingAssignService.Query().Filter(x => x.PickingID == model.PickingID).Get().ToList();
                            IEnumerable<PickingAssign> selects = assigns.Where(x => assignIDs.Contains(x.AssignID));
                            List<PickingAssign> excepts = assigns.Except(selects).ToList();

                            assignModel.ForEach(x =>
                            {
                                PickingAssign assign = _PickingAssignService.FindByID(x.AssignID);

                                assign.PickingID = model.PickingID;
                                assign.AssignStatus = model.PickingStatusEnums;

                                assign.BookingID = x.BookingID;
                                assign.OrderPick = x.OrderPick;
                                assign.StockQuantity = x.OrderQTY;
                                assign.BaseQuantity = x.OrderBaseQTY;
                                assign.StockUnitID = x.OrderUnitID;
                                assign.BaseUnitID = x.OrderBaseUnitID;
                                assign.Barcode = x.PalletNo;
                                assign.ProductID = x.ProductID;
                                assign.SuggestionLocationID = x.SGLocationID;
                                assign.RefLocationID = x.OldSGLocationID;
                                assign.PalletUnitID = x.PalletUnitID;
                                assign.PalletCode = x.PalletNo;
                                assign.RefPalletCode = x.OldPalletNo;
                                assign.PalletQty = x.PalletQTY;
                                assign.PickingLot = x.PickingLot;

                                assign.IsActive = true;
                                assign.PickingUserID = UserID;
                                assign.PickingDate = DateTime.Now;
                                assign.UserModified = UserID;
                                assign.DateModified = DateTime.Now;
                                _PickingAssignService.Modify(assign);
                            });

                            excepts.ForEach(x =>
                            {
                                x.IsActive = false;
                                x.UserModified = UserID;
                                x.DateModified = DateTime.Now;
                                _PickingAssignService.Modify(x);
                            });
                        }

                        #endregion

                        #region Update Picking

                        Picking picking = FindByID(model.PickingID);
                        if (picking == null)
                        {
                            throw new HILIException("MSG00004");
                        }

                        picking.PONo = model.PONo;
                        picking.DocumentNo = model.DocNo;
                        picking.OrderNo = model.OrderNo;
                        picking.EmployeeAssignID = model.EmployeeAssignID;
                        picking.ShippingCode = model.ShippingCode;
                        picking.PickingStatus = model.PickingStatusEnums;

                        picking.IsActive = true;
                        picking.UserModified = UserID;
                        picking.DateModified = DateTime.Now;
                        picking.Remark = model.Remark;
                        base.Modify(picking);

                        #endregion
                    }
                    else
                    {
                        #region PreFix 

                        PickingPrefix prefix = _PickingPrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                        if (prefix == null)
                        {
                            throw new HILIException("PK10012");
                        }

                        PickingPrefix tPrefix = _PickingPrefixService.FindByID(prefix.PrefixID);

                        string PickingCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);

                        tPrefix.IsLastest = false;

                        PickingPrefix newPrefix = new PickingPrefix()
                        {
                            IsLastest = true,
                            LastedKey = PickingCode,
                            PrefixKey = prefix.PrefixKey,
                            FormatKey = prefix.FormatKey,
                            LengthKey = prefix.LengthKey
                        };

                        _PickingPrefixService.Add(newPrefix);

                        #endregion [ PreFix ]

                        // Add New
                        #region Insert Picking

                        Picking newPicking = new Picking()
                        {
                            PickingCode = newPrefix.LastedKey,
                            PickingStatus = PickingStatusEnum.WaitingPick,
                            ShippingCode = model.ShippingCode,
                            DispatchCode = model.DispatchCode,
                            DocumentNo = model.DocNo,
                            PONo = model.PONo,
                            OrderNo = model.OrderNo,
                            PickingEntryDate = model.PickingDate,
                            EmployeeAssignID = model.EmployeeAssignID,
                            Remark = model.Remark,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now
                        };
                        base.Add(newPicking);

                        #endregion

                        #region Insert Picking Assign

                        if (model.AssignJobDetailCollection.Count() > 0)
                        {
                            List<PickingAssign> assigns = new List<PickingAssign>();

                            foreach (AssignJobDetailModel item in model.AssignJobDetailCollection)
                            {
                                decimal? baseQTY = 0;
                                ProductUnit baseUnit = _ProductUnitService.Query().Filter(x => x.ProductID == item.ProductID
                                                && x.IsBaseUOM && x.IsActive).Get().FirstOrDefault();

                                ProductUnit orderUnit = _ProductUnitService.Query().Filter(x => x.ProductUnitID == item.OrderUnitID && x.IsActive).Get().FirstOrDefault();

                                if (baseUnit == null || orderUnit == null)
                                {
                                    throw new HILIException("MSG00015");
                                }

                                if (item.OrderUnitID != baseUnit.ProductUnitID)
                                {
                                    baseQTY = item.OrderQTY * orderUnit.Quantity;
                                }
                                else
                                {
                                    baseQTY = item.OrderQTY;
                                }

                                PickingAssign assign = new PickingAssign()
                                {
                                    BookingID = item.BookingID,
                                    OrderPick = item.OrderPick,
                                    BaseUnitID = baseUnit.ProductUnitID,
                                    BaseQuantity = baseQTY,
                                    StockUnitID = item.OrderUnitID,
                                    StockQuantity = item.OrderQTY,
                                    Barcode = item.PalletNo,
                                    PickingID = newPicking.PickingID,
                                    ProductID = item.ProductID,
                                    SuggestionLocationID = item.SGLocationID,
                                    RefLocationID = item.OldSGLocationID,
                                    PalletUnitID = item.PalletUnitID,
                                    PalletQty = item.PalletQTY,
                                    PalletCode = item.PalletNo,
                                    RefPalletCode = item.OldPalletNo,
                                    PickingLot = item.PickingLot,
                                    AssignStatus = PickingStatusEnum.WaitingPick,

                                    IsActive = true,
                                    UserCreated = UserID,
                                    DateCreated = DateTime.Now,
                                    UserModified = UserID,
                                    DateModified = DateTime.Now,
                                };
                                assigns.Add(assign);
                            }
                            _PickingAssignService.AddRange(assigns);

                        }

                        #endregion
                    }

                    scope.Complete();
                }

                return true;
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

        private void AdjustReserve(PickingAssign item, out Guid refLocationId, out Guid locationId)
        {
            IQueryable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Filter(x => x.IsActive).GetQueryable();
            ProductionControlDetail unReservePallet = pcDetail.Where(x => x.PalletCode == item.RefPalletCode).FirstOrDefault();
            ProductionControlDetail reservePallet = pcDetail.Where(x => x.PalletCode == item.PalletCode).FirstOrDefault();


            refLocationId = unReservePallet.LocationID.Value;
            locationId = reservePallet.LocationID.Value;

            ProductionControlDetail tmpUnReservePallet = _PCDetailService.FindByID(unReservePallet.PackingID);
            tmpUnReservePallet.ReserveQTY -= item.StockQuantity;
            tmpUnReservePallet.ReserveBaseQTY -= item.StockQuantity * tmpUnReservePallet.ConversionQty;
            tmpUnReservePallet.UserModified = UserID;
            tmpUnReservePallet.DateModified = DateTime.Now;

            if (tmpUnReservePallet.ReserveQTY < 0)
            {
                throw new HILIException("MSG00004"); // Save Incomplete
            }

            _PCDetailService.Modify(tmpUnReservePallet);

            ProductionControlDetail tmpReservePallet = _PCDetailService.FindByID(reservePallet.PackingID);
            tmpReservePallet.ReserveQTY += item.StockQuantity;// item.StockQuantity;
            tmpReservePallet.ReserveBaseQTY += item.StockQuantity * tmpUnReservePallet.ConversionQty;
            tmpReservePallet.UserModified = UserID;
            tmpReservePallet.DateModified = DateTime.Now;

            if (tmpReservePallet.ReserveQTY > tmpReservePallet.RemainQTY)
            {
                throw new HILIException("MSG00004"); // Save Incomplete
            }

            _PCDetailService.Modify(tmpReservePallet);             
        }

        private void AdjustReserveX(PickingAssign item)
        {
            IQueryable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Filter(x => x.IsActive).GetQueryable();
            ProductionControlDetail unReservePallet = pcDetail.Where(x => x.PalletCode == item.RefPalletCode).FirstOrDefault();
            ProductionControlDetail reservePallet = pcDetail.Where(x => x.PalletCode == item.PalletCode).FirstOrDefault();

            ProductionControlDetail tmpUnReservePallet = _PCDetailService.FindByID(unReservePallet.PackingID);
            tmpUnReservePallet.ReserveQTY -= item.PalletQty;// item.StockQuantity;
            tmpUnReservePallet.ReserveBaseQTY -= item.PalletQty * tmpUnReservePallet.ConversionQty;
            tmpUnReservePallet.UserModified = UserID;
            tmpUnReservePallet.DateModified = DateTime.Now;

            if (tmpUnReservePallet.ReserveQTY < 0)
            {
                throw new HILIException("MSG00004"); // Save Incomplete
            }

            _PCDetailService.Modify(tmpUnReservePallet);

            ProductionControlDetail tmpReservePallet = _PCDetailService.FindByID(reservePallet.PackingID);
            tmpReservePallet.ReserveQTY += item.PalletQty;// item.StockQuantity;
            tmpReservePallet.ReserveBaseQTY += item.PalletQty * reservePallet.ConversionQty;
            tmpReservePallet.UserModified = UserID;
            tmpReservePallet.DateModified = DateTime.Now;

            if (tmpReservePallet.ReserveQTY > tmpReservePallet.RemainQTY)
            {
                throw new HILIException("MSG00004"); // Save Incomplete
            }

            _PCDetailService.Modify(tmpReservePallet);             
        }

        private StockSearch GetStockReserveModel(PickingAssign assign, decimal QTY, bool unReserve, Guid? locationID = null, string locationCode = null)
        {
            if (locationID == null && locationCode == null)
            {
                throw new HILIException("MSG00006");
            }

            string tmpPalletCode = assign.PalletCode;

            if (unReserve)
            {
                tmpPalletCode = assign.RefPalletCode;
            }

            Product product = _ProductService.Query().Filter(y => y.IsActive && y.ProductID == assign.ProductID).Get().FirstOrDefault();
            IEnumerable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Filter(y => y.IsActive).Get();
            IEnumerable<ProductionControl> pControl = _ProductionControlService.Query().Filter(y => y.IsActive).Get();

            StockSearch reserve = (from pcd in pcDetail
                                   join pc in pControl on pcd.ControlID equals pc.ControlID
                                   where pcd.PalletCode == tmpPalletCode
                                   select new StockSearch()
                                   {
                                       QTY = QTY,
                                       LocationID = locationID,
                                       LocationCode = locationCode,

                                       ProductID = pc.ProductID,

                                       Lot = pcd.LotNo,
                                       StockUnitID = pcd.StockUnitID.Value,
                                       BaseUnitID = pcd.BaseUnitID.Value,
                                       ProductStatusID = pcd.ProductStatusID.Value,
                                       ConversionQty = pcd.ConversionQty ?? 0,
                                       ManufacturingDate = pcd.MFGDate.Value,
                                       ExpirationDate = pcd.MFGDate.Value.AddDays(product.Age),
                                   }).FirstOrDefault();

            return reserve;
        }

        public string ConfirmPickHH(Guid pickingID, Guid productID, string palletCode, string refPalletCode, decimal confirmQTY, decimal consolidateQTY, decimal orderQTY, string orderUnit, string reason)
        {
            try
            {
                Picking pick = FirstOrDefault(x => x.PickingID == pickingID);
                var isRegisTruck = !string.IsNullOrEmpty(pick.ShippingCode);

                string split_pallet = "";
                palletCode = palletCode.ToUpper(); 
                var pickingAssignData = new PickingAssign();                 
                var _pickPallet_ = _PCDetailService.Query().Filter(x => x.IsActive && x.PalletCode == palletCode &&
                                        x.PackingStatus != PackingStatusEnum.Waiting_Receive &&
                                        x.PackingStatus != PackingStatusEnum.Loading_In &&
                                        x.PackingStatus != PackingStatusEnum.In_Progress &&
                                        x.PackingStatus != PackingStatusEnum.Transfer &&
                                        x.PackingStatus != PackingStatusEnum.Cancel)
                                    .Include(x => x.ProductionControl).Get().FirstOrDefault();

                Guid _location_pick = _pickPallet_.LocationID.GetValueOrDefault();

                Guid _suggust_location_pick = Guid.Empty;

                var _ref_pallet = string.IsNullOrEmpty(refPalletCode) ? "" : refPalletCode;

                var _pickRefPallet = _PCDetailService.Query().Filter(x => x.IsActive && 
                                       x.PalletCode == _ref_pallet &&
                                       x.PackingStatus != PackingStatusEnum.Waiting_Receive &&
                                       x.PackingStatus != PackingStatusEnum.Loading_In &&
                                       x.PackingStatus != PackingStatusEnum.In_Progress &&
                                       x.PackingStatus != PackingStatusEnum.Transfer &&
                                       x.PackingStatus != PackingStatusEnum.Cancel)
                                   .Include(x => x.ProductionControl).Get().FirstOrDefault();

                if (_pickPallet_ == null)
                    throw new HILIException("MSG00072"); // Pallet not found

                var _pickPallet = _PCDetailService.FindByID(_pickPallet_.PackingID);
                var pick_remainQty = _pickPallet.RemainQTY; 
                var locationExist = _LocationService.Query().Include(z => z.Zone).Filter(x => x.LocationID == _pickPallet.LocationID &&  x.IsActive).Get().FirstOrDefault();
                
                if (locationExist == null)
                    throw new HILIException("MSG00055"); // Location Data not found
                
                var zoneExist = _ZoneService.FindByID(locationExist.ZoneID); 

                var locationOld = new Location();
                if (!string.IsNullOrEmpty(refPalletCode))
                {
                    locationOld = _LocationService.FirstOrDefault(x => x.LocationID == _pickRefPallet.LocationID && x.IsActive);
                    if (locationOld == null)
                    {
                        throw new HILIException("MSG00055"); // Location Data not found
                    }
                    var zone = _ZoneService.FindByID(locationOld.ZoneID);
                    if (zone.WarehouseID != zoneExist.WarehouseID)
                    {
                        throw new HILIException("MSG00073"); // Pallet not exist in location. Please Re-location your pallet first
                    }
                }
                var P = Query().Filter(x => x.PickingID == pickingID && x.IsActive).Include(x => x.PickingAssignCollection).Get().FirstOrDefault();
                var ptemp = P.PickingAssignCollection.Where(e=>e.IsActive).FirstOrDefault(x => x.PalletCode == (string.IsNullOrEmpty(refPalletCode) ? palletCode : refPalletCode));
                if(ptemp== null)
                {
                    ptemp = P.PickingAssignCollection.Where(e => e.IsActive).FirstOrDefault(x => x.PalletCode == (string.IsNullOrEmpty(refPalletCode) ? palletCode : refPalletCode));
                } 
                decimal pick_reserve = ptemp != null ? ptemp.StockQuantity.GetValueOrDefault() : 0;
                var dispatch = _DispatchService.FirstOrDefault(x => x.IsActive && x.DispatchCode == P.DispatchCode && x.Pono == P.PONo);//.Include(x => x.DispatchDetailCollection).Get().FirstOrDefault(); 
                var receiving = receivingService.FirstOrDefault(x => x.ReceiveDetailID == _pickPallet.ReceiveDetailID && x.IsActive);
                var productAssign = _PickingProductAssign.FirstOrDefault(e => e.IsActive && e.PickingID == pickingID && e.ProductID == receiving.ProductID && e.StockUnitID == receiving.StockUnitID  && e.BaseUnitID == receiving.BaseUnitID);

                using (TransactionScope scope = new TransactionScope())
                {
                    //********************* cancel reserve stock -> split pallet --> rolo --> reserve new location ***********************************
                    #region un-reserve location

                    if (!string.IsNullOrEmpty(refPalletCode))
                    {
                        if (locationOld.LocationID != locationExist.LocationID)
                        {
                            var lo = _LocationService.FindByID(locationOld.LocationID);
                            lo.LocationReserveQty -= 1;
                            lo.UserModified = UserID;
                            lo.DateModified = DateTime.Now;
                            _LocationService.Modify(lo);
                        }
                    }

                    var l = _LocationService.FindByID(locationExist.LocationID);
                    l.LocationReserveQty -= 1;
                    if (l.LocationReserveQty <= 0)
                    {
                        l.LocationReserveQty = 0;
                    }
                    l.UserModified = UserID;
                    l.DateModified = DateTime.Now;
                    _LocationService.Modify(l);
                    #endregion

                    #region restore Reserve
                    List<StockInOutModel> stockRestore = new List<StockInOutModel>();

                    //check swap pallet

                    var booksList = _DispatchBookingService.Where(x => x.PalletCode == _pickPallet.PalletCode && x.IsActive && x.BookingStatus ==  BookingStatusEnum.Inprogress).ToList();
                    var bookQty = booksList.Sum(x => x.RequestQty);

                    stockRestore.Add(new StockInOutModel
                    {
                        ProductID = receiving.ProductID,
                        StockUnitID = _pickPallet.StockUnitID.GetValueOrDefault(),
                        BaseUnitID = _pickPallet.BaseUnitID.GetValueOrDefault(),
                        Lot = _pickPallet.LotNo,
                        ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                        SupplierID = receiving.SupplierID.GetValueOrDefault(),
                        ManufacturingDate = receiving.ManufacturingDate.GetValueOrDefault(),
                        ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                        ProductWidth = receiving.ProductWidth,
                        ProductLength = receiving.ProductLength,
                        ProductHeight = receiving.ProductHeight,
                        ProductWeight = receiving.ProductWeight,
                        PackageWeight = receiving.PackageWeight,
                        Price = receiving.Price,
                        ProductUnitPriceID = receiving.ProductUnitPriceID,
                        ProductStatusID = receiving.ProductStatusID,//hold pallet
                        ProductSubStatusID = receiving.ProductSubStatusID.GetValueOrDefault(),
                        Quantity = string.IsNullOrEmpty(refPalletCode) ? _pickPallet.ReserveQTY.GetValueOrDefault() - bookQty : pick_reserve,//out normal
                        ConversionQty = receiving.ConversionQty,
                        PalletCode = string.IsNullOrEmpty(refPalletCode) ? _pickPallet.PalletCode : _pickRefPallet.PalletCode,
                        LocationCode = string.IsNullOrEmpty(refPalletCode) ? locationExist.Code : locationOld.Code,
                        DocumentCode = dispatch.DispatchCode,
                        DocumentTypeID = dispatch.DocumentId,
                        DocumentID = dispatch.DispatchId,
                        StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                        Remark = "Picking Relocation (Cancel Reserve)",
                        ReserveQuantity = string.IsNullOrEmpty(refPalletCode) ? _pickPallet.ReserveQTY - bookQty : pick_reserve
                    });

                    if (!string.IsNullOrEmpty(refPalletCode))
                    {
                        if (_pickPallet.ReserveQTY > 0)
                            stockRestore.Add(new StockInOutModel
                            {
                                ProductID = receiving.ProductID,
                                StockUnitID = _pickPallet.StockUnitID.GetValueOrDefault(),
                                BaseUnitID = _pickPallet.BaseUnitID.GetValueOrDefault(),
                                Lot = _pickPallet.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                                SupplierID = receiving.SupplierID.GetValueOrDefault(),
                                ManufacturingDate = receiving.ManufacturingDate.GetValueOrDefault(),
                                ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = receiving.ProductStatusID,//hold pallet
                                ProductSubStatusID = receiving.ProductSubStatusID.GetValueOrDefault(),
                                Quantity = _pickPallet.ReserveQTY.GetValueOrDefault() - bookQty,//out normal
                                ConversionQty = receiving.ConversionQty,
                                PalletCode = _pickPallet.PalletCode,
                                LocationCode = locationExist.Code,
                                DocumentCode = dispatch.DispatchCode,
                                DocumentTypeID = dispatch.DocumentId,
                                DocumentID = dispatch.DispatchId,
                                StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                                Remark = "Picking Relocation (Cancel Reserve)",
                                ReserveQuantity = _pickPallet.ReserveQTY - bookQty
                            });
                    }
                    _StockService.UserID = UserID;
                    _StockService.RestoreReserve(stockRestore, Context);

                    #endregion

                    #region Find-location 
                    var dummylocation = _LocationService.Query().Include(z => z.Zone)
                                        .Filter(x => x.Zone.WarehouseID == locationExist.Zone.WarehouseID && x.LocationType == LocationTypeEnum.Dummy && x.IsActive)
                                        .Get().FirstOrDefault();
                    var loadingOutlocation = _LocationService.Query().Include(z => z.Zone)
                                            .Filter(x => x.Zone.WarehouseID == locationExist.Zone.WarehouseID && x.LocationType == LocationTypeEnum.LoadingOut && x.IsActive)
                                            .Get().FirstOrDefault();


                    if (dummylocation == null)
                        throw new HILIException("MSG00066"); // DummyLocation not found

                    if (loadingOutlocation == null)
                        throw new HILIException("MSG00103"); // Location Location not found

                    List<StockInOutModel> stockRelo = new List<StockInOutModel>();

                    var NewLocation = new Location();
                    if (confirmQTY != _pickPallet.RemainQTY)
                    {
                        NewLocation = dummylocation;
                    }
                    else
                    {
                        NewLocation = loadingOutlocation;
                    }

                    #endregion

                    #region split pallet 
                    var pcI = new ProductionControlDetail();
                    if (confirmQTY != pick_remainQty)
                    {
                        CultureInfo cultureinfo = new CultureInfo("en-US");
                        var timeStamp = DateTime.Now.TimeOfDay;
                        var pcline = _ProductionControlService.Query().Filter(x => x.IsActive && x.ControlID == _pickPallet.ControlID).Include(x => x.PCDetailCollection).Get().FirstOrDefault();
                        var line = lineService.FirstOrDefault(x => x.IsActive && x.LineID == pcline.LineID);
                        string newPallet = _pickPallet.LotNo
                                           + line.LineCode
                                           + ((pcline.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1).ToString("000")
                                           + timeStamp.ToString("hhmmss");
                        split_pallet = newPallet;
                        pcI = new ProductionControlDetail
                        {
                            PackingID = Guid.NewGuid(),
                            ControlID = _pickPallet.ControlID,
                            PalletCode = newPallet,
                            Sequence = (pcline.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1,
                            StockQuantity = 0,//confirmQTY,
                            BaseQuantity = 0,//confirmQTY * _pickPallet.ConversionQty,
                            ConversionQty = _pickPallet.ConversionQty,
                            StockUnitID = _pickPallet.StockUnitID,
                            BaseUnitID = _pickPallet.BaseUnitID,
                            ProductStatusID = _pickPallet.ProductStatusID,
                            ProductSubStatusID = _pickPallet.ProductSubStatusID,
                            MFGDate = _pickPallet.MFGDate,
                            MFGTimeStart = _pickPallet.MFGTimeStart,
                            MFGTimeEnd = _pickPallet.MFGTimeEnd,
                            LocationID = loadingOutlocation.LocationID,
                            WarehouseID = loadingOutlocation.Zone.WarehouseID,
                            PackingStatus = PackingStatusEnum.PutAway,
                            RemainStockUnitID = _pickPallet.RemainStockUnitID,
                            RemainBaseUnitID = _pickPallet.RemainBaseUnitID,
                            RemainQTY = confirmQTY,
                            RemainBaseQTY = confirmQTY * _pickPallet.ConversionQty,
                            LotNo = _pickPallet.LotNo,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            ReserveBaseQTY = confirmQTY * _pickPallet.ConversionQty,
                            ReserveQTY = confirmQTY,
                            ReceiveDetailID = _pickPallet.ReceiveDetailID,
                            IsNonProduction = true,
                            RefPalletCode = _pickPallet.PalletCode,
                            IsNormal = true,
                        };
                        _PCDetailService.Add(pcI);
                        var rcv = new Receiving
                        {
                            Sequence = (pcline.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1,
                            GRNCode = receiving.GRNCode.Substring(0, receiving.GRNCode.Length - 1) + (pcline.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1,
                            Quantity = 0,
                            BaseQuantity = 0,
                            PalletCode = pcI.PalletCode,
                            ReceiveID = receiving.ReceiveID,
                            ProductOwnerID = receiving.ProductOwnerID,
                            SupplierID = receiving.SupplierID,
                            LocationID = pcI.LocationID, // DummyLocation 
                            ReceiveDetailID = receiving.ReceiveDetailID,
                            ProductID = receiving.ProductID,
                            Lot = pcI.LotNo,
                            ManufacturingDate = receiving.ManufacturingDate,
                            ExpirationDate = receiving.ExpirationDate,
                            ConversionQty = receiving.ConversionQty,
                            StockUnitID = receiving.StockUnitID,
                            BaseUnitID = receiving.BaseUnitID,
                            ProductStatusID = pcI.ProductStatusID.GetValueOrDefault(),
                            ProductSubStatusID = pcI.ProductSubStatusID,
                            IsDraft = false,
                            IsSentInterface = true,
                            ReceivingStatus = ReceivingStatusEnum.Complete,
                            PackageWeight = 1,
                            ProductWeight = 1,
                            ProductWidth = 1,
                            ProductLength = 1,
                            ProductHeight = 1,
                            Remark = "Pick",
                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now
                        };
                        receivingService.Add(rcv);
                        if (string.IsNullOrEmpty(refPalletCode))
                        {
                            _pickPallet.ReserveQTY -= confirmQTY;
                            _pickPallet.ReserveBaseQTY -= confirmQTY * _pickPallet.ConversionQty;
                        }
                        _pickPallet.RemainQTY -= confirmQTY;
                        _pickPallet.RemainBaseQTY -= confirmQTY * _pickPallet.ConversionQty;
                        _pickPallet.UserModified = UserID;
                        _pickPallet.DateModified = DateTime.Now;
                        _pickPallet.LocationID = dummylocation.LocationID;
                        _PCDetailService.Modify(_pickPallet);
                        if (!string.IsNullOrEmpty(refPalletCode))
                        {
                            var ref_pallet = _PCDetailService.FindByID(_pickRefPallet.PackingID);
                            ref_pallet.ReserveQTY -= pick_reserve;
                            ref_pallet.ReserveBaseQTY -= pick_reserve * ref_pallet.ConversionQty;
                            ref_pallet.UserModified = UserID;
                            ref_pallet.DateModified = DateTime.Now;
                            _PCDetailService.Modify(ref_pallet);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(refPalletCode) && (pick_reserve - confirmQTY) > 0)
                        {

                            var ref_pallet = _PCDetailService.FindByID(_pickRefPallet.PackingID);
                            ref_pallet.ReserveQTY -= (pick_reserve - confirmQTY);
                            ref_pallet.ReserveBaseQTY -= (pick_reserve - confirmQTY) * ref_pallet.ConversionQty;
                            ref_pallet.UserModified = UserID;
                            ref_pallet.DateModified = DateTime.Now;
                            _PCDetailService.Modify(ref_pallet);
                        }
                        _pickPallet.UserModified = UserID;
                        _pickPallet.DateModified = DateTime.Now;
                        _pickPallet.LocationID = NewLocation.LocationID;
                        _PCDetailService.Modify(_pickPallet);
                    }
                    #endregion 

                    #region Relo-location  
                    if (confirmQTY == pick_remainQty)
                    {
                        if (locationExist.Code != NewLocation.Code)
                        {
                            stockRelo.Add(new StockInOutModel
                            {
                                DocumentID = _pickPallet.PackingID,
                                PalletCode = _pickPallet.PalletCode,
                                DocumentCode = dispatch.DispatchCode,
                                ProductID = receiving.ProductID,
                                Lot = _pickPallet.LotNo,
                                ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                                ManufacturingDate = _pickPallet.MFGDate.GetValueOrDefault(),
                                StockUnitID = _pickPallet.RemainStockUnitID.GetValueOrDefault(),
                                BaseUnitID = _pickPallet.RemainBaseUnitID.GetValueOrDefault(),
                                ConversionQty = _pickPallet.ConversionQty.GetValueOrDefault(),
                                ProductStatusID = _pickPallet.ProductStatusID.GetValueOrDefault(),
                                FromLocationCode = locationExist.Code,
                                LocationCode = NewLocation.Code,
                                Quantity = _pickPallet.RemainQTY.GetValueOrDefault(),
                                ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                                SupplierID = receiving.SupplierID.GetValueOrDefault()
                            });
                        }
                    }
                    else
                    {
                        if (locationExist.Code != NewLocation.Code)
                        {
                            stockRelo.Add(new StockInOutModel
                            {
                                DocumentID = _pickPallet.PackingID,
                                PalletCode = _pickPallet.PalletCode,
                                DocumentCode = dispatch.DispatchCode,
                                ProductID = receiving.ProductID,
                                Lot = _pickPallet.LotNo,
                                ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                                ManufacturingDate = _pickPallet.MFGDate.GetValueOrDefault(),
                                StockUnitID = _pickPallet.RemainStockUnitID.GetValueOrDefault(),
                                BaseUnitID = _pickPallet.RemainBaseUnitID.GetValueOrDefault(),
                                ConversionQty = _pickPallet.ConversionQty.GetValueOrDefault(),
                                ProductStatusID = _pickPallet.ProductStatusID.GetValueOrDefault(),
                                FromLocationCode = locationExist.Code,
                                LocationCode = dummylocation.Code,
                                Quantity = _pickPallet.RemainQTY.GetValueOrDefault(),
                                ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                                SupplierID = receiving.SupplierID.GetValueOrDefault()
                            });
                        }
                        stockRelo.Add(new StockInOutModel
                        {
                            DocumentID = pcI.PackingID,
                            PalletCode = pcI.PalletCode,
                            DocumentCode = dispatch.DispatchCode,
                            ProductID = receiving.ProductID,
                            Lot = pcI.LotNo,
                            ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = pcI.MFGDate.GetValueOrDefault(),
                            StockUnitID = pcI.RemainStockUnitID.GetValueOrDefault(),
                            BaseUnitID = pcI.RemainBaseUnitID.GetValueOrDefault(),
                            ConversionQty = pcI.ConversionQty.GetValueOrDefault(),
                            ProductStatusID = pcI.ProductStatusID.GetValueOrDefault(),
                            FromLocationCode = locationExist.Code,
                            LocationCode = loadingOutlocation.Code,
                            Quantity = confirmQTY,
                            ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                            SupplierID = receiving.SupplierID.GetValueOrDefault()
                        });
                    }
                    _StockService.UserID = UserID;
                    _StockService.UpdateLocationOutgoingAndIncomming(stockRelo);

                    #endregion

                    #region Reserve

                    List<StockInOutModel> stockReServe = new List<StockInOutModel>();

                    if (confirmQTY != pick_remainQty)//splite pallet
                    {
                        if (_pickPallet.ReserveQTY - bookQty != 0)
                        {
                            stockReServe.Add(new StockInOutModel
                            {
                                DocumentID = _pickPallet.PackingID,
                                PalletCode = _pickPallet.PalletCode,
                                DocumentCode = dispatch.DispatchCode,
                                ProductID = receiving.ProductID,
                                Lot = _pickPallet.LotNo,
                                ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                                ManufacturingDate = _pickPallet.MFGDate.GetValueOrDefault(),
                                StockUnitID = _pickPallet.RemainStockUnitID.GetValueOrDefault(),
                                BaseUnitID = _pickPallet.RemainBaseUnitID.GetValueOrDefault(),
                                ConversionQty = _pickPallet.ConversionQty.GetValueOrDefault(),
                                ProductStatusID = _pickPallet.ProductStatusID.GetValueOrDefault(),
                                LocationCode = dummylocation.Code,
                                Quantity = _pickPallet.ReserveQTY.GetValueOrDefault() - bookQty - (pick_reserve == confirmQTY ? 0 : (pick_reserve - confirmQTY)),
                                ReserveQuantity = _pickPallet.ReserveQTY.GetValueOrDefault() - bookQty - (pick_reserve == confirmQTY ? 0 : (pick_reserve - confirmQTY)),
                                ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                                SupplierID = receiving.SupplierID.GetValueOrDefault(),
                                StockTransTypeEnum = StockTransactionTypeEnum.Reserve
                            });
                        }
                        stockReServe.Add(new StockInOutModel
                        {
                            DocumentID = pcI.PackingID,
                            PalletCode = pcI.PalletCode,
                            DocumentCode = dispatch.DispatchCode,
                            ProductID = receiving.ProductID,
                            Lot = pcI.LotNo,
                            ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = pcI.MFGDate.GetValueOrDefault(),
                            StockUnitID = pcI.RemainStockUnitID.GetValueOrDefault(),
                            BaseUnitID = pcI.RemainBaseUnitID.GetValueOrDefault(),
                            ConversionQty = pcI.ConversionQty.GetValueOrDefault(),
                            ProductStatusID = pcI.ProductStatusID.GetValueOrDefault(),
                            LocationCode = loadingOutlocation.Code,
                            Quantity = confirmQTY,
                            ReserveQuantity = confirmQTY,
                            ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                            SupplierID = receiving.SupplierID.GetValueOrDefault(),
                            StockTransTypeEnum = StockTransactionTypeEnum.Reserve
                        });
                        _pickPallet = _PCDetailService.FindByID(pcI.PackingID);
                    }
                    else
                    {
                        stockReServe.Add(new StockInOutModel
                        {
                            DocumentID = _pickPallet.PackingID,
                            PalletCode = _pickPallet.PalletCode,
                            DocumentCode = dispatch.DispatchCode,
                            ProductID = receiving.ProductID,
                            Lot = _pickPallet.LotNo,
                            ExpirationDate = receiving.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = _pickPallet.MFGDate.GetValueOrDefault(),
                            StockUnitID = _pickPallet.RemainStockUnitID.GetValueOrDefault(),
                            BaseUnitID = _pickPallet.RemainBaseUnitID.GetValueOrDefault(),
                            ConversionQty = _pickPallet.ConversionQty.GetValueOrDefault(),
                            ProductStatusID = _pickPallet.ProductStatusID.GetValueOrDefault(),
                            LocationCode = NewLocation.Code,
                            Quantity = string.IsNullOrEmpty(refPalletCode) ? _pickPallet.ReserveQTY.GetValueOrDefault() - bookQty : (_pickPallet.ReserveQTY.GetValueOrDefault() + confirmQTY),
                            ReserveQuantity = string.IsNullOrEmpty(refPalletCode) ? _pickPallet.ReserveQTY - bookQty : (_pickPallet.ReserveQTY + confirmQTY),
                            ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                            SupplierID = receiving.SupplierID.GetValueOrDefault(),
                            StockTransTypeEnum = StockTransactionTypeEnum.Reserve
                        });
                    }
                    _StockService.UserID = UserID;
                    _StockService.StockReserve(stockReServe, null);
                    #endregion
                    //********************************************************
                    var pickingAssign = _PickingAssignService.Query().Filter(x => x.IsActive && x.PickingID == pickingID).Get();
                    var assignExists = from pa in pickingAssign where pa.PalletCode == palletCode select pa;
                    if (assignExists != null && assignExists.Count() > 0 && confirmQTY == pick_remainQty)
                    {
                        pickingAssignData = assignExists.FirstOrDefault();
                        _suggust_location_pick = pickingAssignData.SuggestionLocationID.Value;

                        #region Check การยิง pallet ซ้ำ , QTY ต้องไม่เกินที่สั่งไว้ ตามแต่ pallet นั้นๆ 

                        ValidateOverPicking(confirmQTY, pickingAssignData);

                        #endregion

                        pickingAssignData.AssignStatus =PickingStatusEnum.LoadingOut;
                        pickingAssignData.PickingUserID = UserID;
                        pickingAssignData.UserModified = UserID;
                        pickingAssignData.PickingDate = DateTime.Now;
                        pickingAssignData.DateModified = DateTime.Now;
                        _PickingAssignService.Modify(pickingAssignData);
                    }
                    else if (confirmQTY != pick_remainQty)
                    {
                        #region Assign not Exist

                        var unAssign = (from pa in pickingAssign
                                        where pa.ProductID == productID
                                                   && pa.PalletCode == palletCode
                                        select pa).FirstOrDefault();

                        if (unAssign == null)
                        {
                            unAssign = (from pa in pickingAssign
                                        where pa.ProductID == productID
                                                   && pa.PalletCode == refPalletCode
                                        select pa).FirstOrDefault();


                        }
                        if (unAssign != null)
                        {
                            #region Check การยิง pallet ซ้ำ , QTY ต้องไม่เกินที่สั่งไว้ ตามแต่ pallet นั้นๆ 

                            ValidateOverPicking(confirmQTY, unAssign);

                            #endregion

                            unAssign.IsActive = false;
                            unAssign.UserModified = UserID;
                            unAssign.DateModified = DateTime.Now;
                            _PickingAssignService.Modify(unAssign);
                        }
                        else
                        {
                            throw new HILIException("MSG00006"); // Data not found
                        }
                        _suggust_location_pick = unAssign.SuggestionLocationID.Value;

                        #region find Base & Stock Unit

                        decimal? baseQTY = 0;
                        var baseUnit = _ProductUnitService.Query().Filter(x => x.ProductID == productID
                                        && x.IsBaseUOM && x.IsActive).Get().FirstOrDefault();
                        var stockUnit = _ProductUnitService.Query().Filter(x => x.ProductID == productID
                                        && x.Name == orderUnit).Get().FirstOrDefault();

                        if (baseUnit == null || stockUnit == null)
                            throw new HILIException("MSG00015"); // Data is null

                        if (stockUnit.ProductUnitID != baseUnit.ProductUnitID)
                            baseQTY = unAssign.StockQuantity * stockUnit.Quantity;
                        else
                            baseQTY = unAssign.StockQuantity;

                        #endregion

                        pickingAssignData = new PickingAssign()
                        {
                            PickingID = pickingID,
                            Barcode = pcI.PalletCode,
                            PalletCode = pcI.PalletCode,
                            PickingUserID = UserID,
                            PickingDate = DateTime.Now,
                            OrderPick = (pickingAssign.Max(x => x.OrderPick) ?? 0) + 1,
                            AssignStatus = PickingStatusEnum.LoadingOut,
                            ProductID = productID,

                            StockQuantity = confirmQTY,//unAssign.StockQuantity,
                            StockUnitID = stockUnit?.ProductUnitID,
                            BaseQuantity = confirmQTY * stockUnit.Quantity,
                            BaseUnitID = baseUnit?.ProductUnitID,

                            PalletQty = _pickPallet?.RemainQTY,
                            PalletUnitID = _pickPallet?.RemainStockUnitID,
                            PickingLot = _pickPallet?.LotNo,
                            SuggestionLocationID = _suggust_location_pick,//_pickPallet?.LocationID,

                            ShippingDetailID = unAssign.ShippingDetailID,
                            RefLocationID = unAssign.SuggestionLocationID,
                            RefPalletCode = unAssign.PalletCode,
                            BookingID = unAssign.BookingID,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                        };
                        _PickingAssignService.Add(pickingAssignData);

                        if (confirmQTY < unAssign.StockQuantity && unAssign.Remark == "Override Booking Rule")
                        {
                            RegisterTruckDetail _regisTruckDetail = new RegisterTruckDetail() { ShippingDetailID = Guid.Empty };

                            var remainValue = (unAssign.StockQuantity - confirmQTY).GetValueOrDefault();
                            if (isRegisTruck)
                            {
                                var regtruck = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingDetailID == unAssign.ShippingDetailID);
                                regtruck.ShippingQuantity = confirmQTY;
                                regtruck.BasicQuantity = confirmQTY * regtruck.ConversionQty;
                                regtruck.ConfirmQuantity = confirmQTY;
                                regtruck.ConfirmBasicQuantity = confirmQTY * regtruck.ConversionQty;
                                regtruck.Remark = "Override Booking Rule";

                                _regisTruckDetail = new RegisterTruckDetail
                                {
                                    ShippingDetailID = Guid.NewGuid(),
                                    ShippingID = regtruck.ShippingID,
                                    ProductID = regtruck.ProductID,
                                    ShippingQuantity = remainValue,
                                    ShippingUnitID = regtruck.ShippingUnitID,
                                    BasicQuantity = remainValue * regtruck.ConversionQty,
                                    BasicUnitID = regtruck.BasicUnitID,
                                    ConversionQty = regtruck.ConversionQty,
                                    ReferenceID = regtruck.ReferenceID,
                                    BookingID = regtruck.BookingID,
                                    TransactionTypeID = regtruck.TransactionTypeID,
                                    Shipping_DT = DateTime.Now,
                                    ConfirmQuantity = remainValue,
                                    ConfirmUnitID = regtruck.ConfirmUnitID,
                                    ConfirmBasicQuantity = remainValue * regtruck.ConversionQty,
                                    ConfirmBasicUnitID = regtruck.ConfirmBasicUnitID,
                                    UserCreated = unAssign.UserCreated,
                                    UserModified = unAssign.UserCreated,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now,
                                    IsActive = true,
                                    Remark = "Override Booking Rule",
                                };
                                _RegisterTruckDetailService.Modify(regtruck);
                                _RegisterTruckDetailService.Add(_regisTruckDetail);
                            }
                            var pickingAssignData2 = new PickingAssign()
                            {
                                PickingID = pickingID,
                                Barcode = unAssign.PalletCode,
                                PalletCode = unAssign.PalletCode,
                                PickingUserID = UserID,
                                PickingDate = DateTime.Now,
                                OrderPick = (pickingAssign.Max(x => x.OrderPick) ?? 0) + 1,
                                AssignStatus = PickingStatusEnum.Pick,
                                ProductID = productID,

                                StockQuantity = remainValue,
                                StockUnitID = stockUnit?.ProductUnitID,
                                BaseQuantity = remainValue * stockUnit.Quantity,
                                BaseUnitID = baseUnit?.ProductUnitID,

                                PalletQty = _pickPallet?.RemainQTY,
                                PalletUnitID = _pickPallet?.RemainStockUnitID,
                                PickingLot = _pickPallet?.LotNo,
                                SuggestionLocationID = _suggust_location_pick,//_pickPallet?.LocationID,

                                ShippingDetailID = _regisTruckDetail.ShippingDetailID,
                                RefLocationID = unAssign.SuggestionLocationID,
                                RefPalletCode = unAssign.PalletCode,
                                BookingID = unAssign.BookingID,

                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                Remark = "Override Booking Rule"
                            };                 
                            _PickingAssignService.Add(pickingAssignData2);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Assign not Exist

                        var unAssign = (from pa in pickingAssign
                                        where pa.ProductID == productID
                                                   && pa.PalletCode == refPalletCode
                                        select pa).FirstOrDefault();

                        if (unAssign != null)
                        {
                            #region Check การยิง pallet ซ้ำ , QTY ต้องไม่เกินที่สั่งไว้ ตามแต่ pallet นั้นๆ 

                            ValidateOverPicking(confirmQTY, unAssign);

                            #endregion

                            unAssign.IsActive = false;
                            unAssign.UserModified = UserID;
                            unAssign.DateModified = DateTime.Now;
                            _PickingAssignService.Modify(unAssign);
                        }
                        else
                        {
                            throw new HILIException("MSG00006"); // Data not found
                        }
                        _suggust_location_pick = unAssign.SuggestionLocationID.Value;

                        #region find Base & Stock Unit

                        decimal? baseQTY = 0;
                        var baseUnit = _ProductUnitService.Query().Filter(x => x.ProductID == productID
                                        && x.IsBaseUOM && x.IsActive).Get().FirstOrDefault();
                        var stockUnit = _ProductUnitService.Query().Filter(x => x.ProductID == productID
                                        && x.Name == orderUnit).Get().FirstOrDefault();

                        if (baseUnit == null || stockUnit == null)
                            throw new HILIException("MSG00015"); // Data is null

                        if (stockUnit.ProductUnitID != baseUnit.ProductUnitID)
                            baseQTY = unAssign.StockQuantity * stockUnit.Quantity;
                        else
                            baseQTY = unAssign.StockQuantity;

                        #endregion

                        pickingAssignData = new PickingAssign()
                        {
                            PickingID = pickingID,
                            Barcode = palletCode,
                            PalletCode = palletCode,
                            PickingUserID = UserID,
                            PickingDate = DateTime.Now,
                            OrderPick = (pickingAssign.Max(x => x.OrderPick) ?? 0) + 1,
                            AssignStatus = PickingStatusEnum.LoadingOut,
                            ProductID = productID,

                            StockQuantity = unAssign.StockQuantity,
                            StockUnitID = stockUnit?.ProductUnitID,
                            BaseQuantity = baseQTY,
                            BaseUnitID = baseUnit?.ProductUnitID,

                            PalletQty = _pickPallet?.RemainQTY,
                            PalletUnitID = _pickPallet?.RemainStockUnitID,
                            PickingLot = _pickPallet?.LotNo,
                            SuggestionLocationID = _suggust_location_pick,//_pickPallet?.LocationID,

                            ShippingDetailID = unAssign.ShippingDetailID,
                            RefLocationID = unAssign.SuggestionLocationID,
                            RefPalletCode = unAssign.PalletCode,
                            BookingID = unAssign.BookingID,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                        };

                        _PickingAssignService.Add(pickingAssignData);

                        AdjustReserveX(pickingAssignData);

                        #endregion
                    }

                    if (pickingAssignData != null)
                    {
                        #region ADD PickingDetail

                        var conversionQTY = (from pu in _ProductUnitService.Query().Filter(x => x.IsActive).Get()
                                             where pu.ProductUnitID == pickingAssignData.StockUnitID
                                             select pu).FirstOrDefault()?.Quantity ?? 1;

                        var pickingDetailModel = _PickingDetailService.Query().Filter(x => x.IsActive && x.AssignID == pickingAssignData.AssignID).Get().FirstOrDefault();

                        if (pickingDetailModel != null)
                        {
                            var sumPick = pickingDetailModel.PickStockQty + confirmQTY;

                            pickingDetailModel.PickStockQty = sumPick;
                            pickingDetailModel.PickBaseQty = sumPick * conversionQTY;
                            pickingDetailModel.PickingStatus = (int)PickingStatusEnum.PickPartial;
                            pickingDetailModel.UserModified = UserID;
                            pickingDetailModel.DateModified = DateTime.Now;
                            _PickingDetailService.Modify(pickingDetailModel);
                        }
                        else
                        {
                            var pickingDetail = new PickingDetail()
                            {
                                AssignID = pickingAssignData.AssignID,
                                PickingStatus = (int)PickingStatusEnum.PickPartial,
                                PalletCode = string.IsNullOrEmpty(pcI.PalletCode) ? palletCode : pcI.PalletCode,
                                PickStockQty = confirmQTY,
                                PickingReason = reason,
                                PickBaseQty = (confirmQTY * conversionQTY),
                                LocationID = _location_pick,//NewLocation.LocationID,
                                PickStockUnitID = pickingAssignData.StockUnitID,
                                PickBaseUnitID = pickingAssignData.BaseUnitID,
                                ConversionQty = conversionQTY,

                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                            };
                            _PickingDetailService.Add(pickingDetail);
                        }

                        #endregion
                    }
                    
                    #region Update Status

                    CheckSumProductAssign(pickingID, productAssign,receiving.ConversionQty);
                    var tmpPickingAssignService = Context.Repository<PickingAssign>();
                    var tmpPickingDetailService = Context.Repository<PickingDetail>();
                    var tmpBookingService = Context.Repository<DispatchBooking>();
                    //var PA = _PickingAssignService.Where(x => x.PickingID == pickingID && x.IsActive).ToList();
                    //var PD = (from pd in _PickingDetailService.Where(x => x.IsActive)
                    //          join pa in _PickingAssignService.Where(x => x.PickingID == pickingID && x.IsActive) on pd.AssignID equals pa.AssignID
                    //          select pd).ToList();
                    var PA = _PickingAssignService.Where(e => e.PickingID == pickingID && e.ProductID == pickingAssignData.ProductID && e.StockUnitID == pickingAssignData.StockUnitID && e.BaseUnitID == productAssign.BaseUnitID && e.IsActive).ToList();
                    var assignIds = PA.Select(e => e.AssignID).ToList();
                    //.Query().Filter(x => x.AssignID == pickingAssignData.AssignID && x.IsActive).Get().ToList();
                    var PD = _PickingDetailService.Where(e =>  assignIds.Contains(e.AssignID.Value)&& e.PickStockUnitID == pickingAssignData.StockUnitID && e.PickBaseUnitID == productAssign.BaseUnitID && e.IsActive).ToList();
                    //.Query().Filter(x => x.AssignID == pickingAssignData.AssignID && x.IsActive).Get().ToList();
                    var dispatchCount = PA.Sum(e => e.StockQuantity);
                    var pickingCount = PD.Sum(e => e.PickStockQty);
                    if (productAssign != null && productAssign.StockQuantity > 0)
                    {
                        dispatchCount = productAssign.StockQuantity;
                    }
                    if (dispatchCount == pickingCount) //PD.Sum(e => e.PickStockQty) == PA.Sum(e => e.StockQuantity))
                    {
                        PA.ForEach(x =>
                        {
                            var temp = tmpPickingAssignService.FindByID(x.AssignID);
                            temp.AssignStatus = PickingStatusEnum.Complete;
                            temp.UserModified = UserID;
                            temp.DateModified = DateTime.Now;
                            tmpPickingAssignService.Modify(temp);

                            var booking = tmpBookingService.FindByID(temp.BookingID);
                            booking.BookingStatus = BookingStatusEnum.Complete;
                            booking.UserModified = UserID;
                            booking.DateModified = DateTime.Now;
                            tmpBookingService.Modify(booking);
                        });

                        PD.ForEach(x =>
                        {
                            var tempp = tmpPickingDetailService.FindByID(x.PickingDetailID);
                            tempp.PickingStatus = (int)PickingStatusEnum.Complete;
                            tempp.UserModified = UserID;
                            tempp.DateModified = DateTime.Now;
                            tmpPickingDetailService.Modify(tempp);
                        });

                        P = Query().Filter(x => x.PickingID == pickingID).Include(x => x.PickingAssignCollection).Get().FirstOrDefault();
                        if (!P.PickingAssignCollection.Any(x => x.IsActive && ((x.AssignStatus == PickingStatusEnum.Pick) || (x.AssignStatus == PickingStatusEnum.LoadingOut))))
                        {
                            var temppp = FindByID(P.PickingID);
                            temppp.PickingStatus = PickingStatusEnum.Complete;
                            temppp.UserModified = UserID;
                            temppp.DateModified = DateTime.Now;
                            base.Modify(temppp);
                        }
                    }

                    #region Picking for non-register truck

                    if (string.IsNullOrWhiteSpace(P.ShippingCode) && !string.IsNullOrWhiteSpace(P.DispatchCode))
                    {
                        DispatchDetailStatusEnum tmpStatus = DispatchDetailStatusEnum.WaitingConfirmDispatchNoneRegister;
                        DispatchStatusEnum distmpStatus = DispatchStatusEnum.WaitingConfirmDispatchNoneRegister;

                        if (dispatch == null)
                            throw new HILIException("MSG00006"); // data not found

                        if (dispatch.IsBackOrder ?? false)
                        {
                            tmpStatus = DispatchDetailStatusEnum.InBackOrder;
                            distmpStatus = DispatchStatusEnum.InBackOrder;
                        }
                        var list = _DispatchDetailService.Where(e => e.DispatchId == dispatch.DispatchId && e.IsActive).ToList(); // dispatch.DispatchDetailCollection.Where(x => x.IsActive).ToList();
                        list.ForEach(x =>
                        {
                            var item = _DispatchDetailService.FirstOrDefault(e => e.DispatchDetailId == x.DispatchDetailId);
                            item.UserModified = UserID;
                            item.DateModified = DateTime.Now;
                            item.DispatchDetailStatus = tmpStatus;
                            _DispatchDetailService.Modify(item);
                        });
                        //dispatch.DispatchDetailCollection.Where(x => x.IsActive).ToList().ForEach(x =>
                        //{
                        //    x.UserModified = UserID;
                        //    x.DateModified = DateTime.Now;
                        //    x.DispatchDetailStatus = tmpStatus;
                        //    _DispatchDetailService.Modify(x);
                        //});
                        dispatch.IsActive = true;
                        dispatch.UserModified = UserID;
                        dispatch.DateModified = DateTime.Now;
                        dispatch.DispatchStatus = distmpStatus;
                        _DispatchService.Modify(dispatch);
                    }

                    #endregion

                    #endregion

                    scope.Complete();
                }

                return split_pallet;
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

        private void CheckSumProductAssign(Guid pickingID, PickingProductAssign productAssign,decimal ConversionQty)
        {
            try
            {
                Picking pick = FirstOrDefault(x => x.PickingID == pickingID);
                var isRegisTruck = !string.IsNullOrEmpty(pick.ShippingCode);
                var PA = _PickingAssignService.Where(x => x.PickingID == pickingID
                                               && x.IsActive
                                               && x.ProductID == productAssign.ProductID
                                               && x.StockUnitID == productAssign.StockUnitID
                                               && x.BaseUnitID == productAssign.BaseUnitID).ToList();
                decimal sumStockPick = new decimal(0);
                foreach (var item in PA)
                {
                    if (item.AssignStatus == PickingStatusEnum.Complete || item.AssignStatus == PickingStatusEnum.PickPartial)
                    {
                        sumStockPick += _PickingDetailService.Where(e => e.IsActive && e.AssignID == item.AssignID).Sum(e => e.PickStockQty.HasValue ? e.PickStockQty.Value : 0);
                    }
                    else
                    {
                        sumStockPick += item.StockQuantity.GetValueOrDefault();
                    }
                }
                var shotQty = productAssign.StockQuantity - sumStockPick;
                var palletcodes = PA.Select(e => e.PalletCode);
                var bookingId = PA.Select(e => e.BookingID).FirstOrDefault();
                RegisterTruckDetail regdetail = new RegisterTruckDetail()
                {
                    ShippingDetailID = Guid.Empty
                };
                RegisterTruck registerTruck = null;
                RegisterTruckDetail refregdetail = null;
                if (isRegisTruck)
                {
                    var shippingDetailID = PA.Select(e => e.ShippingDetailID).FirstOrDefault();
                    var shippingDetailsAssigns = PA.Select(e => e.ShippingDetailID);
                    var shippingId = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingDetailID == shippingDetailID && e.IsActive).ShippingID;
                    registerTruck = _RegisterTruckService.FirstOrDefault(e => e.ShippingID == shippingId && e.IsActive);
                    regdetail = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingID == shippingId && !e.IsActive && !shippingDetailsAssigns.Contains(e.ShippingDetailID) && e.ConfirmQuantity >= shotQty);
                    refregdetail = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingID == shippingId && e.ProductID == productAssign.ProductID && e.ShippingUnitID == productAssign.StockUnitID && e.BasicUnitID == productAssign.BaseUnitID);
                }
                var booking = _DispatchBookingService.FirstOrDefault(e => e.BookingId == bookingId && e.IsActive);
                var dispatchDetail = _DispatchDetailService.FirstOrDefault(e => e.DispatchDetailId == booking.DispatchDetailId && e.IsActive);
                var newbooking = _DispatchBookingService.FirstOrDefault(e => e.DispatchDetailId == dispatchDetail.DispatchDetailId && e.BookingQty >= shotQty && !palletcodes.Contains(e.PalletCode));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {                   
                    if (sumStockPick < productAssign.StockQuantity)
                    {                       
                        newbooking.IsActive = true;
                        newbooking.RequestQty = shotQty;
                        newbooking.RequestBaseQty = shotQty * ConversionQty;
                        newbooking.BookingQty = shotQty;
                        newbooking.BookingBaseQty = shotQty * ConversionQty;
                        newbooking.BookingStatus = BookingStatusEnum.Complete;
                        _DispatchBookingService.Modify(newbooking);

                        if (isRegisTruck)
                        {
                            if (regdetail == null)
                            {                                
                                regdetail = new RegisterTruckDetail()
                                {
                                    BasicQuantity = shotQty * ConversionQty,
                                    BasicUnitID = productAssign.BaseUnitID,
                                    BookingID = newbooking.BookingId,
                                    ConfirmBasicQuantity = shotQty * ConversionQty,
                                    ConfirmQuantity = shotQty,
                                    ConfirmBasicUnitID = productAssign.BaseUnitID,
                                    ConfirmUnitID = productAssign.StockUnitID,
                                    ConversionQty = ConversionQty,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now,
                                    IsActive = true,
                                    ProductID = productAssign.ProductID,
                                    RegisterTruck = registerTruck,
                                    Remark = "Override Booking Rule",
                                    ShippingDetailID = Guid.NewGuid(),
                                    ShippingID = registerTruck.ShippingID,
                                    ShippingQuantity = shotQty,
                                    ShippingUnitID = productAssign.StockUnitID,
                                    TransactionTypeID = refregdetail.TransactionTypeID,
                                    ReferenceID = refregdetail.ReferenceID,
                                    Shipping_DT = refregdetail.Shipping_DT,
                                    UserCreated = refregdetail.UserCreated,
                                    UserModified = refregdetail.UserModified
                                };
                                _RegisterTruckDetailService.Add(regdetail);
                            }
                            else
                            {
                                regdetail.IsActive = true;
                                regdetail.BasicQuantity = shotQty * ConversionQty;
                                regdetail.ConfirmBasicQuantity = shotQty * ConversionQty;
                                regdetail.ConfirmQuantity = shotQty;
                                regdetail.ShippingQuantity = shotQty;
                                regdetail.Remark = "Override Booking Rule";
                                _RegisterTruckDetailService.Modify(regdetail);
                            }
                        }
                        var pickingAssign = _PickingAssignService.Where(x =>
                                        x.PickingID == pickingID
                                        && !x.IsActive
                                        && x.StockQuantity >= shotQty
                                        && x.ProductID == productAssign.ProductID
                                        && x.StockUnitID == productAssign.StockUnitID
                                        && !palletcodes.Contains(x.PalletCode)
                                        && x.BaseUnitID == productAssign.BaseUnitID)
                                       .OrderBy(e => e.StockQuantity).FirstOrDefault();
                        if (pickingAssign == null)
                        {
                            pickingAssign = new PickingAssign()
                            {
                                IsActive = true,
                                AssignID = Guid.NewGuid(),
                                AssignStatus = PickingStatusEnum.Pick,
                                Barcode = newbooking.PalletCode,
                                DateCreated = DateTime.Now,
                                PalletCode = newbooking.PalletCode,
                                BookingID = newbooking.BookingId,
                                BaseQuantity = shotQty * ConversionQty,
                                BaseUnit = PA.FirstOrDefault().BaseUnit,
                                BaseUnitID = PA.FirstOrDefault().BaseUnitID,
                                DateModified = DateTime.Now,
                                OrderPick = PA.Max(e => e.OrderPick) + 1,
                                PalletQty = shotQty,
                                PalletUnit = PA.FirstOrDefault().PalletUnit,
                                PalletUnitID = PA.FirstOrDefault().PalletUnitID,
                                Picking = PA.FirstOrDefault().Picking,
                                PickingDate = PA.FirstOrDefault().PickingDate,
                                PickingID = PA.FirstOrDefault().PickingID,
                                PickingLot = newbooking.ProductLot,
                                PickingUserID = PA.FirstOrDefault().PickingUserID,
                                Product = PA.FirstOrDefault().Product,
                                ProductID = PA.FirstOrDefault().ProductID,
                                RefLocationID = PA.FirstOrDefault().RefLocationID,
                                Remark = "Override Booking Rule",
                                ShippingDetailID = regdetail.ShippingDetailID,
                                StockQuantity = shotQty,
                                StockUnit = PA.FirstOrDefault().StockUnit,
                                StockUnitID = PA.FirstOrDefault().StockUnitID,
                                SuggestionLocation = PA.FirstOrDefault().SuggestionLocation,
                                SuggestionLocationID = PA.FirstOrDefault().SuggestionLocationID,
                                UserCreated = PA.FirstOrDefault().UserCreated,
                                UserModified = PA.FirstOrDefault().UserModified
                            };
                            _PickingAssignService.Add(pickingAssign);
                        }
                        else
                        {
                            pickingAssign.IsActive = true;
                            pickingAssign.AssignStatus = PickingStatusEnum.Pick;
                            pickingAssign.Barcode = newbooking.PalletCode;
                            pickingAssign.PalletCode = newbooking.PalletCode;
                            pickingAssign.BookingID = newbooking.BookingId;
                            pickingAssign.BaseQuantity = shotQty * ConversionQty;
                            pickingAssign.PalletQty = shotQty;
                            pickingAssign.PickingLot = newbooking.ProductLot;
                            pickingAssign.Remark = "Override Booking Rule";
                            pickingAssign.ShippingDetailID = regdetail.ShippingDetailID;
                            pickingAssign.StockQuantity = shotQty;
                            _PickingAssignService.Modify(pickingAssign);
                        }
                    }
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
            }
        }
        private void ValidateOverPicking(decimal confirmQTY, PickingAssign pickingAssignData)
        {
            decimal sumPickDetail = _PickingDetailService.Where(x => x.AssignID == pickingAssignData.AssignID).ToList().Sum(x => x.PickStockQty.HasValue ? x.PickStockQty.Value : 0);
            decimal sumPDPlusComfirmPick = sumPickDetail + confirmQTY;
            if (pickingAssignData.StockQuantity < sumPDPlusComfirmPick)
            {
                throw new HILIException("MSG00085"); // cannot picking QTY exceed maximum of order QTY
            }
        }

        public AssignJobModel GetAssignJob(Guid pickingID)
        {
            try
            {
                var pkInfo = _PickingService.FindByID(pickingID);

                IQueryable<PickingAssign> pickingAssign = _PickingAssignService.Query().Filter(x => x.IsActive && x.PickingID == pickingID).GetQueryable();

                IQueryable<PickingDetail> pickingDetail = _PickingDetailService.Query().Filter(x => x.IsActive).GetQueryable();

                IQueryable<RegisterTruck> regisTruck = _RegisterTruckService.Query().Filter(x => x.IsActive && x.PoNo== pkInfo.PONo).GetQueryable();
                
                IQueryable<Dispatch> dispatch = _DispatchService.Query().Filter(x => x.IsActive && x.Pono == pkInfo.PONo).GetQueryable();
                
                IQueryable<DispatchDetail> dispatchDetail = _DispatchDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<DispatchBooking> dispatchBooking = _DispatchBookingService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<Product> product = _ProductService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<ProductCodes> productCode = _ProductCodeService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<ProductUnit> productUnit = _ProductUnitService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<DockConfig> dock = _DockConfigService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<Location> location = _LocationService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<Zone> zone = _ZoneService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<ShippingTo> shipto = _ShiptoService.Query().Filter(x => x.IsActive).GetQueryable();
                
                IQueryable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Filter(x => x.IsActive).GetQueryable();

                AssignJobModel picking = (from pk in Query().Get()
                                          join rt in _RegisterTruckService.Query().Get() on pk.ShippingCode equals rt.ShippingCode into _rt
                                          from rt in _rt.DefaultIfEmpty()
                                          join dp in _DispatchService.Query().Filter(x => x.IsActive).Get() on pk.DispatchCode equals dp.DispatchCode into _dp
                                          from dp in _dp.DefaultIfEmpty()
                                          join st in shipto on dp?.ShipptoId ?? Guid.Empty equals st.ShipToId into _st
                                          from st in _st.DefaultIfEmpty()
                                          where pk.PickingID == pickingID
                                          select new AssignJobModel()
                                          {
                                              PickingID = pk.PickingID,
                                              PONo = pk.PONo,
                                              OrderNo = pk.OrderNo,
                                              DocNo = pk.DocumentNo,
                                              PickingDate = pk.PickingEntryDate,
                                              PickingCode = pk.PickingCode,
                                              Remark = pk.Remark,
                                              PickingStatusEnums = pk.PickingStatus,
                                              ShippingCode = pk.ShippingCode,
                                              ShippingTruckNo = rt?.ShippingTruckNo ?? "",
                                              ShipTo = st?.Name ?? "",
                                              DispatchCode = pk.DispatchCode,
                                              EmployeeAssignID = pk.EmployeeAssignID,
                                              EmployeeAssign = _UserGroupsService.Query().Filter(x => x.GroupID == pk.EmployeeAssignID).Get().FirstOrDefault()?.GroupName,
                                              PickingStatus = ((DescriptionAttribute)typeof(PickingStatusEnum).GetMember(pk.PickingStatus.ToString())[0].GetCustomAttribute(typeof(DescriptionAttribute), false)).Description
                                          }).FirstOrDefault();

                if (picking == null)
                {
                    throw new HILIException("MSG00006");
                }

                List<AssignJobDetailModel> details = (from pk in Query().GetQueryable()
                                                      join rt in regisTruck on pk.ShippingCode equals rt.ShippingCode into _rt
                                                      from rt in _rt.DefaultIfEmpty()
                                                      join pa in pickingAssign on pk.PickingID equals pa.PickingID 
                                                      join pd in pickingDetail on pa.AssignID equals pd.AssignID into _pd
                                                      from pd in _pd.DefaultIfEmpty()
                                                      join pcd in pcDetail on pa.PalletCode equals pcd.PalletCode
                                                      join pc in productCode on pa.ProductID equals pc.ProductID
                                                      join p in product on pc.ProductID equals p.ProductID
                                                      join d in dock on rt.DockTypeID equals d.DockConfigID into _d
                                                      from d in _d.DefaultIfEmpty()
                                                      join sl in location on pa.SuggestionLocationID equals sl.LocationID into _sl
                                                      from sl in _sl.DefaultIfEmpty()
                                                      join pl in location on pd.LocationID equals pl.LocationID into _pl
                                                      from pl in _pl.DefaultIfEmpty()
                                                      join ou in productUnit on pa.StockUnitID equals ou.ProductUnitID into _ou
                                                      from ou in _ou.DefaultIfEmpty()
                                                      join pku in productUnit on pd.PickStockUnitID equals pku.ProductUnitID into _pku
                                                      from pku in _pku.DefaultIfEmpty()
                                                      join plu in productUnit on pa.PalletUnitID equals plu.ProductUnitID into _plu
                                                      from plu in _plu.DefaultIfEmpty()
                                                      where pk.PickingID == pickingID && pc.CodeType == ProductCodeTypeEnum.Stock
                                                      //&& pa.IsActive
                                                      //&& pk.IsActive
                                                      //&& rt.IsActive
                                                      //&& pd.IsActive
                                                      //&& pcd.IsActive
                                                      //&& pc.IsActive
                                                      //&& p.IsActive
                                                      //&& pcd.RemainQTY != null
                                                      select new AssignJobDetailModel()
                                                      {
                                                          AssignID = pa.AssignID,
                                                          BookingID = pa.BookingID,
                                                          OrderPick = pa.OrderPick,
                                                          ProductID = p.ProductID,
                                                          ProductCode = pc.Code,
                                                          ProductName = p.Name,
                                                          PickingLot = pcd.LotNo,
                                                          OldSGLocationID = pa.RefLocationID,
                                                          SGLocationID = pa.SuggestionLocationID,
                                                          SGLocation = sl != null ? sl.Code : string.Empty,
                                                          PickLocationID = pd.LocationID,
                                                          PickLocation = pl != null ? pl.Code : string.Empty,
                                                          OrderBaseQTY = pa.BaseQuantity,
                                                          OrderBaseUnitID = pa.BaseUnitID,
                                                          OrderQTY = pa.StockQuantity ?? 0,
                                                          OrderUnitID = pa.StockUnitID,
                                                          OrderUnit = ou != null ? ou.Name : string.Empty,
                                                          PickQTY = pd.PickStockQty ?? 0,
                                                          PickUnitID = pd.PickStockUnitID,
                                                          PickUnit = pku != null ? pku.Name : string.Empty,
                                                          OldPalletNo = pa.RefPalletCode,
                                                          PalletNo = pa.PalletCode,
                                                          PalletQTY = pa.PalletQty ?? 0,
                                                          PalletUnitID = pa.PalletUnitID,
                                                          PalletUnit = plu != null ? plu.Name : string.Empty,
                                                          Dock = d != null ? d.DockName : string.Empty
                                                      }).ToList();

                if (details.Count() != 0)
                {
                    picking.AssignJobDetailCollection = details;
                }

                return picking;
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

        public AssignJobModel GetAssignJobByPO(string PONo)
        {
            try
            {
                IQueryable<DispatchDetail> dispatchDetail = _DispatchDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<DispatchBooking> dispatchBooking = _DispatchBookingService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Product> product = _ProductService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductCodes> productCode = _ProductCodeService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductUnit> productUnit = _ProductUnitService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Location> location = _LocationService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ShippingTo> shipto = _ShiptoService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Contact> contact = _ContactService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductionControlDetail> PCDetail = _PCDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ItfInterfaceMapping> interfaceMapping = _ItfInterfaceMappingService.Query().Filter(x => x.IsActive).GetQueryable();



                List<Guid?> _pickingAssign = _PickingAssignService.Query().Include(x => x.Picking).Filter(x => x.IsActive && x.Picking.PONo == PONo).Get().Select(x => x.BookingID).ToList();

                IQueryable<Dispatch> dispatch = from dp in _DispatchService.Query().GetQueryable()
                                                join itf in interfaceMapping on dp.DocumentId equals itf.DocumentId
                                                where (dp.IsActive && dp.Pono == PONo)
                                                     && ((dp.DispatchStatus == DispatchStatusEnum.InprogressConfirm)
                                                             || (dp.DispatchStatus == DispatchStatusEnum.InprogressConfirmQA)
                                                             || (dp.DispatchStatus == DispatchStatusEnum.InBackOrder))
                                                     && (itf.IsAssign ?? false) && (itf.IsRegistTruck != true)
                                                select dp;

                AssignJobModel picking = (from d in dispatch
                                          join st in shipto on d.ShipptoId equals st.ShipToId into _st
                                          from st in _st.DefaultIfEmpty()
                                          select new AssignJobModel()
                                          {
                                              PickingCode = "New",
                                              PONo = d.Pono,
                                              OrderNo = d.OrderNo,
                                              DispatchCode = d.DispatchCode,
                                              PickingDate = DateTime.Now,
                                              PickingStatusEnums = PickingStatusEnum.WaitingPick,
                                              ShipTo = st != null ? st.Name : string.Empty,
                                              PickingStatus = PickingStatusEnum.WaitingPick.ToString()
                                          }).FirstOrDefault();

                if (picking == null)
                {
                    throw new HILIException("MSG00006");
                }

                IEnumerable<AssignJobDetailModel> tmp = (from d in dispatch
                                                         join dd in dispatchDetail on d.DispatchId equals dd.DispatchId
                                                         join bk in dispatchBooking on dd.DispatchDetailId equals bk.DispatchDetailId
                                                         join pcd in PCDetail on bk.PalletCode equals pcd.PalletCode
                                                         join p in product on bk.ProductId equals p.ProductID
                                                         join pc in productCode on p.ProductID equals pc.ProductID
                                                         join sl in location on bk.LocationId equals sl.LocationID into _sl
                                                         from sl in _sl.DefaultIfEmpty()
                                                         join ou in productUnit on bk.BookingStockUnitId equals ou.ProductUnitID into _ou
                                                         from ou in _ou.DefaultIfEmpty()
                                                         join plu in productUnit on pcd.RemainStockUnitID equals plu.ProductUnitID into _plu
                                                         from plu in _plu.DefaultIfEmpty()
                                                         where pc.CodeType == ProductCodeTypeEnum.Stock
                                                                  && pcd.RemainQTY != null
                                                                  && bk.BookingStatus != BookingStatusEnum.Complete
                                                                  && !_pickingAssign.Contains(bk.BookingId)
                                                                  //&& pcd.IsActive
                                                                  //&& pc.IsActive
                                                                  //&& p.IsActive                                                               
                                                         select new AssignJobDetailModel()
                                                         {
                                                             ProductID = p.ProductID,
                                                             ProductCode = pc.Code,
                                                             ProductName = p.Name,
                                                             BookingID = bk.BookingId,
                                                             OldSGLocationID = bk.LocationId,
                                                             SGLocationID = bk.LocationId,
                                                             SGLocation = sl.Code,
                                                             OrderQTY = bk.BookingQty,
                                                             OrderUnitID = bk.BookingStockUnitId,
                                                             OrderUnit = ou.Name,
                                                             PickQTY = 0,
                                                             PickUnitID = bk.BookingStockUnitId,
                                                             PickUnit = ou.Name,
                                                             OldPalletNo = pcd.PalletCode,
                                                             PalletNo = pcd.PalletCode,
                                                             PalletQTY = pcd.RemainQTY,
                                                             PalletUnitID = pcd.RemainStockUnitID,
                                                             PalletUnit = plu.Name,
                                                             PickingLot = pcd.LotNo,
                                                         }).AsEnumerable();

                List<AssignJobDetailModel> details = tmp.Select((x, index) => new AssignJobDetailModel()
                {
                    OrderPick = index + 1,
                    ProductID = x.ProductID,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    BookingID = x.BookingID,
                    OldSGLocationID = x.OldSGLocationID,
                    SGLocationID = x.SGLocationID,
                    SGLocation = x.SGLocation,
                    OrderQTY = x.OrderQTY,
                    OrderUnitID = x.OrderUnitID,
                    OrderUnit = x.OrderUnit,
                    PickQTY = x.PickQTY,
                    PickUnitID = x.PickUnitID,
                    PickUnit = x.PickUnit,
                    OldPalletNo = x.OldPalletNo,
                    PalletNo = x.PalletNo,
                    PalletQTY = x.PalletQTY,
                    PalletUnitID = x.PalletUnitID,
                    PalletUnit = x.PalletUnit,
                    PickingLot = x.PickingLot,
                }).ToList();

                if (details.Count() != 0)
                {
                    picking.AssignJobDetailCollection = details;
                }

                return picking;
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

        public PickingListHHModel CheckPallet(Guid pickingID, string palletCode, bool isReprocess)
        {
            try
            {
                if(pickingID == Guid.Empty)
                {
                    throw new HILIException("MSG00072"); // Pallet not found
                }
                PickingListHHModel result = new PickingListHHModel
                {
                    IsShowReason = false
                };
                Picking pick = FirstOrDefault(x => x.PickingID == pickingID);
                result.IsRegisTruck = !string.IsNullOrEmpty(pick.ShippingCode); 
               
                List<PickingAssign> assigns = _PickingAssignService.Query()
                                    .Filter(x => x.PickingID == pickingID && x.IsActive)
                                    .Include(x => x.PickingDetailCollection).Get().ToList();

                ProductionControlDetail pcDetail = _PCDetailService.Query().Filter(x => x.PalletCode == palletCode && x.IsActive).Include(x => x.ProductionControl).Get().FirstOrDefault();
                if (pcDetail == null)
                {
                    throw new HILIException("MSG00072"); // Pallet not found
                }               

                PickingAssign assign = assigns.FirstOrDefault(x => x.PalletCode == palletCode && x.IsActive);
                if (assign == null)
                {    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        CheckOverrideBookingRule(pickingID, palletCode); 
                        scope.Complete();
                    }
                    assigns = _PickingAssignService.Query()
                                    .Filter(x => x.PickingID == pickingID && x.IsActive)
                                    .Include(x => x.PickingDetailCollection).Get().ToList();
                    assign = assigns.FirstOrDefault(x => x.PalletCode == palletCode);
                    
                }             
                if (assign == null)
                {
                    if (isReprocess)
                    {
                        throw new HILIException("MSG00082"); // Pallet not exists (in case reprocess)
                    }
                    result.IsShowReason = true;
                    List<KeyValuePair<string, decimal?>> refPalletCodes = assigns.Where(x => x.PickingLot == pcDetail.LotNo
                                            && x.ProductID == pcDetail.ProductionControl.ProductID
                                            && !x.PickingDetailCollection.Any())
                        .Select(t => new KeyValuePair<string, decimal?>(t.PalletCode, t.StockQuantity)).ToList();
                    if (refPalletCodes != null && refPalletCodes.Count() > 0)
                    {
                        result.RefPalletNo = refPalletCodes;
                    }
                    else
                    {
                        throw new HILIException("MSG00074"); // Please select pallet with same lot according to this Assign Job
                    }
                    result.PalletQTY = pcDetail.RemainQTY - pcDetail.ReserveQTY;
                }
                else
                {
                    result.PalletQTY = assign.StockQuantity;// pcDetail.RemainQTY - (pcDetail.ReserveQTY - assign.StockQuantity);
                }
                result.ProductID = pcDetail.ProductionControl.ProductID;
                result.UnitID = pcDetail.RemainStockUnitID;
                result.ProductName = _ProductService.Where(x => x.ProductID == pcDetail.ProductionControl.ProductID).Select(x => x.Name).FirstOrDefault();
                result.Unit = _ProductUnitService.Where(x => x.ProductUnitID == pcDetail.RemainStockUnitID).Select(x => x.Name).FirstOrDefault();
                return result;
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

        private void CheckOverrideBookingRule(Guid pickingID, string palletCode)
        {
            Picking pick = FirstOrDefault(x => x.PickingID == pickingID);
            var isRegisTruck = !string.IsNullOrEmpty(pick.ShippingCode);

            var productionControlDetail = _PCDetailService.FirstOrDefault(x => x.IsActive &&
                                        x.PalletCode == palletCode &&
                                        x.PackingStatus != PackingStatusEnum.Waiting_Receive &&
                                        x.PackingStatus != PackingStatusEnum.Loading_In &&
                                        x.PackingStatus != PackingStatusEnum.In_Progress &&
                                        x.PackingStatus != PackingStatusEnum.Transfer &&
                                        x.PackingStatus != PackingStatusEnum.Cancel);
            if (productionControlDetail == null)
            {
                throw new HILIException("MSG00072");
            }
            if (productionControlDetail.RemainQTY - productionControlDetail.ReserveQTY <= 0)
            {
                throw new HILIException("MSG00098");
            }
            var productionControl = _ProductionControlService.FirstOrDefault(e => e.ControlID == productionControlDetail.ControlID && e.IsActive);
            var pickingAssigns = _PickingAssignService.Where(e =>
                                   e.BaseUnitID == productionControlDetail.BaseUnitID
                                && e.StockUnitID == productionControlDetail.StockUnitID
                                && e.ProductID == productionControl.ProductID
                                && e.Picking.PickingID == pickingID
                                && e.PalletCode != palletCode
                                && e.AssignStatus != PickingStatusEnum.Cancel
                                && e.AssignStatus != PickingStatusEnum.Complete
                                && e.AssignStatus != PickingStatusEnum.All
                                && e.IsActive).OrderBy(e => e.StockQuantity).ToList();
            var productRemainQty = (productionControlDetail.RemainQTY - productionControlDetail.ReserveQTY).GetValueOrDefault();
            Dictionary<Guid, decimal> pickingList = new Dictionary<Guid, decimal>();

            foreach (var assign in pickingAssigns.Where(e => e.StockQuantity.GetValueOrDefault() > 0))
            {
                var pickingAssignDetail = _PickingDetailService.Where(e => e.AssignID == assign.AssignID && e.IsActive).ToList();
                var reserveQty = assign.StockQuantity.GetValueOrDefault();
                if (pickingAssignDetail.Any())
                {
                    reserveQty -= pickingAssignDetail.Sum(e => e.PickStockQty.GetValueOrDefault());
                }
                if (reserveQty > 0)
                {
                    if (productRemainQty <= reserveQty)
                    {
                        pickingList.Add(assign.AssignID, productRemainQty);
                        productRemainQty = 0;
                    }
                    else
                    {
                        pickingList.Add(assign.AssignID, reserveQty);
                        productRemainQty -= reserveQty;
                    }
                }
                if (productRemainQty <= 0) { break; }
            }
            if (!pickingList.Any())
            {
                throw new HILIException("MSG00090");
            }

            try
            {
                foreach (var item in pickingList)
                {
                    ReverseOldBooking(productionControlDetail, item);
                }
                #region --- new stock reseve ---  
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                //{
                var oldassign = pickingAssigns.FirstOrDefault();
                var oldBooking = _DispatchBookingService.FirstOrDefault(e => e.BookingId == oldassign.BookingID);
                var dispatachdetail = _DispatchDetailService.FirstOrDefault(e => e.DispatchDetailId == oldBooking.DispatchDetailId);
                var receive = receivingService.FirstOrDefault(x => x.IsActive && x.PalletCode == palletCode);
                var regtruck = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingDetailID == oldassign.ShippingDetailID);
                var stockInfo = _StockInfoService.FirstOrDefault(e =>
                          e.ProductID == oldassign.ProductID &&
                          e.Lot == productionControlDetail.LotNo &&
                          e.StockUnitID == productionControlDetail.StockUnitID &&
                          e.BaseUnitID == productionControlDetail.BaseUnitID &&
                          e.ProductStatusID == productionControlDetail.ProductStatusID &&
                          e.ProductSubStatusID == productionControlDetail.ProductSubStatusID &&
                          e.ConversionQty == productionControlDetail.ConversionQty &&
                          e.SupplierID == receive.SupplierID &&
                          e.ProductOwnerID == receive.ProductOwnerID &&
                          e.IsActive);
                var stockBalance = _StockBalanceService.FirstOrDefault(e => e.StockInfoID == stockInfo.StockInfoID && e.IsActive);
                var stockLoca = _StockLocationBalanceService.FirstOrDefault(e => e.StockBalanceID == stockBalance.StockBalanceID && e.IsActive);
                var pickValue = pickingList.Sum(e => e.Value);

                var newBooking = new DispatchBooking()
                {
                    UserModified = UserID,
                    UserCreated = UserID,
                    BookingBaseQty = pickValue * productionControlDetail.ConversionQty,
                    BookingBaseUnitId = productionControlDetail.BaseUnitID,
                    BookingQty = pickValue,
                    BookingStatus = BookingStatusEnum.Complete,
                    BookingStockUnitId = productionControlDetail.StockUnitID,
                    ConversionQty = productionControlDetail.ConversionQty,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    DispatchDetailId = dispatachdetail.DispatchDetailId,
                    ExpirationDate = stockInfo.ExpirationDate,
                    IsActive = true,
                    IsBackOrder = false,
                    LocationId = productionControlDetail.LocationID.GetValueOrDefault(),
                    Mfgdate = productionControlDetail.MFGDate.GetValueOrDefault(),
                    PalletCode = palletCode,
                    ProductId = productionControlDetail.ProductionControl.ProductID,
                    ProductLot = productionControlDetail.LotNo,
                    Remark = "Override Booking Rule",
                    RequestBaseQty = pickValue * productionControlDetail.ConversionQty,
                    RequestBaseUnitId = productionControlDetail.BaseUnitID,
                    RequestQty = pickValue,
                    RequestStockUnitId = productionControlDetail.StockUnitID,
                    Sequence = oldBooking.Sequence,
                    BookingId = Guid.NewGuid()
                };
                _DispatchBookingService.Add(newBooking);
                var newstockInfo = _StockInfoService.FirstOrDefault(e => e.ProductID == productionControl.ProductID && e.Lot == productionControlDetail.LotNo
                && e.StockUnitID == productionControlDetail.StockUnitID && e.BaseUnitID == productionControlDetail.BaseUnitID && e.IsActive);
                RegisterTruckDetail _regisTruckDetail = new RegisterTruckDetail() { ShippingDetailID = Guid.Empty };
                if (isRegisTruck)
                {
                    _regisTruckDetail = new RegisterTruckDetail
                    {
                        ShippingDetailID = Guid.NewGuid(),
                        ShippingID = regtruck.ShippingID,
                        ProductID = productionControl.ProductID,  //regtruck.ProductID,
                        ShippingQuantity = pickValue,
                        ShippingUnitID = productionControl.StockUnitID.GetValueOrDefault(), //regtruck.ShippingUnitID,
                        BasicQuantity = pickValue * regtruck.ConversionQty,
                        BasicUnitID = productionControl.BaseUnitID.GetValueOrDefault(), //regtruck.BasicUnitID,
                        ConversionQty = productionControl.ConversionQty.GetValueOrDefault(),
                        ReferenceID = regtruck.ReferenceID,
                        BookingID = newBooking.BookingId,
                        TransactionTypeID = regtruck.TransactionTypeID,
                        Shipping_DT = DateTime.Now,
                        ConfirmQuantity = pickValue,
                        ConfirmUnitID = productionControl.StockUnitID.GetValueOrDefault(),
                        ConfirmBasicQuantity = pickValue * productionControl.ConversionQty.GetValueOrDefault(),
                        ConfirmBasicUnitID = productionControl.BaseUnitID.GetValueOrDefault(),
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsActive = true,
                        Remark = "Override Booking Rule",
                        UserCreated = UserID,
                        UserModified = UserID,
                    };
                }
                PickingAssign newpickingAssign = new PickingAssign()
                {
                    AssignStatus = PickingStatusEnum.Pick,
                    Barcode = palletCode,
                    BaseQuantity = pickValue * regtruck.ConversionQty,
                    BaseUnit = oldassign.BaseUnit,
                    BaseUnitID = oldassign.BaseUnitID,
                    BookingID = newBooking.BookingId,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true,
                    OrderPick = oldassign.OrderPick,
                    PalletCode = palletCode,
                    PalletQty = pickValue,
                    PalletUnit = oldassign.PalletUnit,
                    PalletUnitID = oldassign.PalletUnitID,
                    Picking = _PickingService.FirstOrDefault(e => e.PickingID == pickingID),
                    PickingID = pickingID,
                    PickingDate = oldassign.PickingDate,
                    PickingLot = productionControlDetail.LotNo,
                    PickingUserID = oldassign.PickingUserID,
                    ProductID = productionControlDetail.ProductionControl.ProductID,
                    ShippingDetailID = _regisTruckDetail.ShippingDetailID,
                    StockQuantity = pickValue,
                    Remark = "Override Booking Rule",
                    StockUnitID = productionControlDetail.StockUnitID,
                    SuggestionLocationID = oldassign.SuggestionLocationID,
                    RefPalletCode = palletCode,
                    RefLocationID = productionControlDetail.LocationID,
                    AssignID = Guid.NewGuid(),
                    StockUnit = oldassign.StockUnit,
                    Product = oldassign.Product,
                    UserCreated = UserID,
                    UserModified = UserID
                };
                if (isRegisTruck)
                {
                    _RegisterTruckDetailService.Add(_regisTruckDetail);
                }
                _PickingAssignService.Add(newpickingAssign);
                var newstockBalance = _StockBalanceService.FirstOrDefault(e => e.StockInfoID == newstockInfo.StockInfoID && e.IsActive);
                var newstockLoca = _StockLocationBalanceService.FirstOrDefault(e => e.StockBalanceID == stockBalance.StockBalanceID && e.IsActive);
                newstockBalance.ReserveQuantity += pickValue;
                _StockLocationBalanceService.Modify(stockLoca);

                newstockLoca.ReserveQuantity += pickValue;
                _StockBalanceService.Modify(stockBalance);

                productionControlDetail.ReserveQTY += pickValue;
                productionControlDetail.ReserveBaseQTY = productionControlDetail.ReserveQTY * productionControlDetail.ConversionQty;
                _PCDetailService.Modify(productionControlDetail);
                //    scope.Complete();
                //}
                #endregion
            }
            catch
            {
                throw new HILIException("MSG00072");
            }
        }

        public PrintPalletTagModel GetPrintPallet(Guid pickingID, string palletCode)
        {
            string dockName = "";
            try
            {
                dockName = (from pick in _PickingAssignService.Where(e => e.IsActive && e.PickingID == pickingID && e.PalletCode == palletCode)
                            join booking in _DispatchBookingService.Where(e => e.IsActive) on pick.BookingID equals booking.BookingId
                            join dispatchdetail in _DispatchDetailService.Where(e => e.IsActive) on booking.DispatchDetailId equals dispatchdetail.DispatchDetailId
                            join dispatch in _DispatchService.Where(e => e.IsActive) on dispatchdetail.DispatchId equals dispatch.DispatchId
                            join registerTruck in _RegisterTruckService.Where(e => e.IsActive) on dispatch.DispatchCode equals registerTruck.Dispatchcode
                            join dock in _DockConfigService.Where(e => e.IsActive) on registerTruck.DockTypeID equals dock.DockConfigID
                            select dock.DockName).FirstOrDefault();
                if (string.IsNullOrEmpty(dockName))
                {
                    dockName = "";
                }
            }
            catch
            {
                dockName = "";
            }
            try
            {
                var productionControlDetail = _PCDetailService.FirstOrDefault(e => e.IsActive && e.PalletCode == palletCode);
                var productionControl = _ProductionControlService.FirstOrDefault(e => e.ControlID == productionControlDetail.ControlID && e.IsActive);
                var product = _ProductService.FirstOrDefault(e => e.ProductID == productionControl.ProductID);
                var productCode = _ProductCodeService.FirstOrDefault(e => e.ProductID == product.ProductID);
                var line = lineService.FindByID(productionControl.LineID);
                var unit = _ProductUnitService.FindByID(productionControlDetail.StockUnitID);
                return new PrintPalletTagModel()
                {
                    Line = line != null ? line.LineCode : "",
                    LotNo = productionControlDetail != null ? productionControlDetail.LotNo : "",
                    OrderNo = productionControl != null ? productionControl.OrderNo : "",
                    ProductCode = productCode.Code,
                    ProductionDate = productionControl != null ? string.Format("{0:dd/MM/yyyy}", productionControlDetail.MFGDate) : "",
                    ProductName = product.Name,
                    ProductTime = productionControl != null && productionControl.ProductionTime != null ? productionControlDetail.MFGTimeStart.ToString() : "",
                    Quantity = productionControlDetail != null ? string.Format("{0:N0}", productionControlDetail.RemainQTY) :"0.00",
                    UnitName = unit != null ? unit.Name : "",
                    PalletTag = productionControlDetail != null ? productionControlDetail.PalletCode : "",
                    DockLoad = dockName
                };
            }
            catch (Exception ex)
            {
                throw new HILIException("MSG00006");
            }
        }

        private void ReverseOldBooking(ProductionControlDetail productionControlDetail, KeyValuePair<Guid, decimal> item)
        {
            var assign = _PickingAssignService.FindByID(item.Key);
            var booking = _DispatchBookingService.FirstOrDefault(e => e.BookingId == assign.BookingID && e.IsActive);
            var regtruck = _RegisterTruckDetailService.FirstOrDefault(e => e.ShippingDetailID == assign.ShippingDetailID);
            var receive = receivingService.FirstOrDefault(x => x.IsActive && x.PalletCode == booking.PalletCode);
            var reserveQty = assign.StockQuantity.GetValueOrDefault();
            var assignPcControl = _PCDetailService.FirstOrDefault(e => e.IsActive && e.PalletCode == assign.PalletCode);
            var pickingAssignDetail = _PickingDetailService.Where(e => e.AssignID == assign.AssignID && e.IsActive).ToList();
            Picking pick = FirstOrDefault(x => x.PickingID == assign.PickingID);
            var isRegisTruck = !string.IsNullOrEmpty(pick.ShippingCode);

            if (pickingAssignDetail.Any())
            {
                reserveQty -= pickingAssignDetail.Sum(e => e.PickStockQty.GetValueOrDefault());
            }
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
            if (reserveQty == item.Value || reserveQty < item.Value)
            {
                if ((assign.AssignStatus == PickingStatusEnum.Pick || assign.AssignStatus == PickingStatusEnum.PickPartial))
                {
                    #region---Disable Old Booking --
                    if (booking.BookingQty == reserveQty)
                    {
                        booking.IsActive = false;
                        booking.BookingStatus = BookingStatusEnum.Close;
                    }
                    else if (booking.BookingQty > reserveQty)
                    {
                        booking.BookingQty = booking.BookingQty - reserveQty;
                        booking.BookingBaseQty = booking.BookingQty * booking.ConversionQty;
                        booking.RequestQty = booking.BookingQty;
                        booking.RequestBaseQty = booking.BookingBaseQty * booking.ConversionQty;
                    }
                    else
                    {
                        var shotqty = reserveQty - booking.BookingQty;
                        booking.BookingQty = reserveQty;
                        booking.BookingBaseQty = reserveQty * booking.ConversionQty;
                        booking.RequestQty = reserveQty;
                        booking.RequestBaseQty = reserveQty * booking.ConversionQty;
                        booking.IsActive = false;
                        booking.BookingStatus = BookingStatusEnum.Close;

                        var newBooking = new DispatchBooking()
                        {
                            UserModified = UserID,
                            UserCreated = UserID,
                            BookingBaseQty = shotqty * productionControlDetail.ConversionQty,
                            BookingBaseUnitId = productionControlDetail.BaseUnitID,
                            BookingQty = shotqty,
                            BookingStatus = BookingStatusEnum.Complete,
                            BookingStockUnitId = productionControlDetail.StockUnitID,
                            ConversionQty = productionControlDetail.ConversionQty,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            DispatchDetailId = booking.DispatchDetailId,
                            ExpirationDate = booking.ExpirationDate,
                            IsActive = true,
                            IsBackOrder = false,
                            LocationId = productionControlDetail.LocationID.GetValueOrDefault(),
                            Mfgdate = productionControlDetail.MFGDate.GetValueOrDefault(),
                            PalletCode = booking.PalletCode,
                            ProductId = productionControlDetail.ProductionControl.ProductID,
                            ProductLot = productionControlDetail.LotNo,
                            Remark = "Override Booking Rule",
                            RequestBaseQty = shotqty * productionControlDetail.ConversionQty,
                            RequestBaseUnitId = productionControlDetail.BaseUnitID,
                            RequestQty = shotqty,
                            RequestStockUnitId = productionControlDetail.StockUnitID,
                            Sequence = booking.Sequence,
                            BookingId = Guid.NewGuid()
                        };

                        _DispatchBookingService.Add(newBooking);
                    }
                    _DispatchBookingService.Modify(booking);
                    #endregion

                    #region---Disable Old Assign --

                    if (assign.StockQuantity == reserveQty)
                    {
                        assign.IsActive = false;
                        assign.AssignStatus = PickingStatusEnum.Cancel;
                        if (isRegisTruck)
                        {
                            regtruck.IsActive = false;
                        }
                    }
                    else
                    {
                        assign.StockQuantity = reserveQty;
                        assign.BaseQuantity = reserveQty * booking.ConversionQty;
                        assign.PalletQty = reserveQty;
                        if (isRegisTruck)
                        {
                            regtruck.ConfirmQuantity = reserveQty;
                            regtruck.ConfirmBasicQuantity = reserveQty * booking.ConversionQty;
                            regtruck.ShippingQuantity = reserveQty;
                            regtruck.BasicQuantity = reserveQty * booking.ConversionQty.GetValueOrDefault();
                        }
                    }
                    _PickingAssignService.Modify(assign);
                    if (isRegisTruck)
                    {
                        _RegisterTruckDetailService.Modify(regtruck);
                    }
                    #endregion

                    assignPcControl.ReserveQTY -= reserveQty;
                    assignPcControl.ReserveBaseQTY = assignPcControl.ReserveQTY * booking.ConversionQty.GetValueOrDefault();
                    _PCDetailService.Modify(assignPcControl);

                    if (reserveQty < regtruck.ShippingQuantity)
                    {
                        regtruck.ShippingQuantity = regtruck.ShippingQuantity - reserveQty;
                        regtruck.ConfirmQuantity = regtruck.ConfirmQuantity - reserveQty;
                        regtruck.ConfirmBasicQuantity = regtruck.ConfirmQuantity * regtruck.ConversionQty;
                        regtruck.BasicQuantity = regtruck.ShippingQuantity * regtruck.ConversionQty;
                        _RegisterTruckDetailService.Modify(regtruck);
                    }
                }
                else
                {
                    booking.BookingQty = item.Value;
                    booking.RequestQty = item.Value;
                    booking.RequestBaseQty = item.Value * booking.ConversionQty;
                    booking.BookingBaseQty = item.Value * booking.ConversionQty;
                    booking.BookingStatus = BookingStatusEnum.Complete;

                    assign.StockQuantity = item.Value;
                    assign.PalletQty = item.Value;
                    assign.BaseQuantity = item.Value * booking.ConversionQty;
                    assign.AssignStatus = PickingStatusEnum.LoadingOut;
                    assignPcControl.ReserveQTY -= item.Value;
                    assignPcControl.ReserveBaseQTY = (assignPcControl.ReserveQTY) * assignPcControl.ConversionQty;
                    if (isRegisTruck)
                    {
                        regtruck.ShippingQuantity = item.Value;
                        regtruck.BasicQuantity = item.Value * booking.ConversionQty.GetValueOrDefault();
                        regtruck.ConfirmBasicQuantity = item.Value * booking.ConversionQty.GetValueOrDefault();
                        regtruck.ConfirmQuantity = item.Value;
                        _RegisterTruckDetailService.Modify(regtruck);
                    }
                    _PickingAssignService.Modify(assign);
                    _DispatchBookingService.Modify(booking);
                    _PCDetailService.Modify(assignPcControl);
                }
            }
            else
            {
                assignPcControl.ReserveQTY -= item.Value;
                assignPcControl.ReserveBaseQTY = (assignPcControl.ReserveQTY) * assignPcControl.ConversionQty;

                booking.BookingQty -= item.Value;
                booking.BookingBaseQty = booking.BookingQty * booking.ConversionQty;
                booking.RequestQty -= item.Value;
                booking.RequestBaseQty = booking.RequestQty * booking.ConversionQty;

                assign.StockQuantity -= item.Value;
                assign.BaseQuantity = assign.StockQuantity * booking.ConversionQty;

                _DispatchBookingService.Modify(booking);
                _PickingAssignService.Modify(assign);
                _PCDetailService.Modify(assignPcControl);
                if (isRegisTruck)
                {
                    regtruck.ShippingQuantity -= item.Value;
                    regtruck.BasicQuantity = regtruck.ShippingQuantity * booking.ConversionQty.GetValueOrDefault();
                    regtruck.ConfirmQuantity -= item.Value;
                    regtruck.ConfirmBasicQuantity = regtruck.ConfirmQuantity * booking.ConversionQty.GetValueOrDefault();
                    _RegisterTruckDetailService.Modify(regtruck);
                }
            }
                #region --- Update Stock Reserve---
                var stockInfo = _StockInfoService.FirstOrDefault(e =>
                                    e.ProductID == assign.ProductID &&
                                    e.Lot == assign.PickingLot &&
                                    e.StockUnitID == assign.StockUnitID &&
                                    e.BaseUnitID == assign.BaseUnitID &&
                                    e.ProductStatusID == productionControlDetail.ProductStatusID &&
                                    e.ProductSubStatusID == productionControlDetail.ProductSubStatusID &&
                                    e.ConversionQty == booking.ConversionQty &&
                                    e.SupplierID == receive.SupplierID &&
                                    e.ProductOwnerID == receive.ProductOwnerID &&
                                    e.IsActive);
                var stockBalance = _StockBalanceService.FirstOrDefault(e => e.StockInfoID == stockInfo.StockInfoID);
                var stockLoca = _StockLocationBalanceService.FirstOrDefault(e => e.StockBalanceID == stockBalance.StockBalanceID);
                stockLoca.ReserveQuantity -= item.Value;
                _StockLocationBalanceService.Modify(stockLoca);

                stockBalance.ReserveQuantity -= item.Value;
                _StockBalanceService.Modify(stockBalance);
            //    scope.Complete();
            //}
            #endregion
        }

        public List<PickingListHHModel> GetPickingListHH(string keyword)
        {
            try
            {  
                List<PickingListHHModel> results = new List<PickingListHHModel>();
                if (keyword != null && !string.IsNullOrEmpty(keyword))
                {
                    results = Context.SQLQuery<PickingListHHModel>($"exec GetPickingListHH '{keyword}'").ToList();
                }
                if (results == null || results.Count() == 0)
                {
                    return new List<PickingListHHModel>();
                }
                return results;
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

        public PickingListHHModel GetPickingHH(string pickingID, string productID)
        {
            try
            {

                PickingListHHModel results = new PickingListHHModel();

                results = Context.SQLQuery<PickingListHHModel>($"exec GetPickingHH '{pickingID}' ,'{productID}'").FirstOrDefault();

                if (results == null)
                {
                    return new PickingListHHModel();
                }

                return results;
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

        public List<AssignJobModel> GetAllAssignJob(PickingStatusEnum pickingStatus, DateTime? startDate, DateTime? endDate, string DocNo, string PONo, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                DateTime?[] d = Utilities.GetTerm(startDate, endDate);

                IEnumerable<AssignJobModel> results = from p in Query().Get()
                                                      join rt in _RegisterTruckService.Query().Get() on p.ShippingCode equals rt.ShippingCode into _rt
                                                      from rt in _rt.DefaultIfEmpty()
                                                      where p.IsActive == true
                                                      select new AssignJobModel()
                                                      {
                                                          PickingID = p.PickingID,
                                                          PONo = p.PONo,
                                                          DocNo = p.DocumentNo,
                                                          PickingDate = p.PickingEntryDate,
                                                          PickingStatus = ((DescriptionAttribute)typeof(PickingStatusEnum).GetMember(p.PickingStatus.ToString())[0].GetCustomAttribute(typeof(DescriptionAttribute), false)).Description,
                                                          PickingStatusEnums = p.PickingStatus,
                                                          ShippingCode = p.ShippingCode,
                                                          ShippingTruckNo = rt != null ? rt.ShippingTruckNo : ""
                                                      };

                #region Filtering

                if (d[0] != null && d[1] != null)
                {
                    results = results.Where(x => x.PickingDate >= d[0] && x.PickingDate <= d[1]);
                }

                if (pickingStatus != PickingStatusEnum.All)
                {
                    results = results.Where(x => x.PickingStatusEnums == pickingStatus);
                }

                if (!string.IsNullOrWhiteSpace(DocNo))
                {
                    results = results.Where(x => (x.DocNo != null) && (x.DocNo.Contains(DocNo)));
                }

                if (!string.IsNullOrWhiteSpace(PONo))
                {
                    results = results.Where(x => (x.PONo != null) && (x.PONo.Contains(PONo)));
                }

                if (results == null)
                {
                    return new List<AssignJobModel>();
                }
                #endregion

                #region Paging

                totalRecords = results.Count();


                if (totalRecords == 0)
                {
                    return new List<AssignJobModel>();
                }

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderByDescending(x => x.PONo).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<DispatchforAssignJobModel> GetDispatchforAssignJob(string searchPO, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                searchPO = searchPO ?? "";
                IEnumerable<Picking> picking = Query().Filter(x => x.IsActive && x.PickingStatus != PickingStatusEnum.Cancel).Get();
                IEnumerable<Dispatch> dispatch = _DispatchService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ShippingTo> shipto = _ShiptoService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Contact> contact = _ContactService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ItfInterfaceMapping> itfmap = _ItfInterfaceMappingService.Query().Filter(x => x.IsActive).Get();

                IEnumerable<DispatchforAssignJobModel> results = from d in dispatch
                                                                 join s in shipto on d.ShipptoId equals s.ShipToId
                                                                 join c in contact on d.CustomerId equals c.ContactID
                                                                 join i in itfmap on d.DocumentId equals i.DocumentId
                                                                 where (d.Pono != null && d.Pono.Contains(""))
                                                                       && ((d.DispatchStatus == DispatchStatusEnum.InprogressConfirm)
                                                                               || (d.DispatchStatus == DispatchStatusEnum.InprogressConfirmQA)
                                                                               || (d.DispatchStatus == DispatchStatusEnum.InBackOrder))
                                                                       && d.Pono.Contains(searchPO)
                                                                       && (i.IsAssign ?? false) && i.IsRegistTruck != true
                                                                 select new DispatchforAssignJobModel()
                                                                 {
                                                                     SearchPO = searchPO,
                                                                     DispatchID = d.DispatchId,
                                                                     PONo = d.Pono,
                                                                     CustomerName = c.Name,
                                                                     ShiptoName = s.Name,
                                                                     OrderNo = d.OrderNo,
                                                                     DeliveryDate = d.DeliveryDate
                                                                 };

                if (results == null || results.Count() == 0)
                {
                    return new List<DispatchforAssignJobModel>();
                }

                #region Paging

                totalRecords = results.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderByDescending(x => x.PONo).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<UserGroups> GetUserWHGroup(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                IEnumerable<UserGroups> userGroups = _UserGroupsService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<UserInGroup> userInGroups = _UserInGroupService.Query().Filter(x => x.IsActive).Get();

                IEnumerable<UserGroups> results = from ug in userGroups
                                                  join uig in userInGroups on ug.GroupID equals uig.GroupID
                                                  where uig.UserID == UserID
                                                  select ug;

                if (results == null || results.Count() == 0)
                {
                    return new List<UserGroups>();
                }

                #region Paging

                totalRecords = results.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderBy(x => x.GroupName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public bool RemovePickingAssign(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Picking _picking = Query().Filter(x => x.IsActive == true &&
                                                       x.PickingID == id &&
                                                       x.PickingStatus == PickingStatusEnum.WaitingPick)
                                                       .Get().FirstOrDefault();

                    if (_picking == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _picking.IsActive = false;
                    _picking.DateModified = DateTime.Now;
                    _picking.UserModified = UserID;
                    base.Modify(_picking);


                    List<PickingAssign> _pickingAssign = _PickingAssignService.Query().Filter(x => x.IsActive == true &&
                                                                                   x.PickingID == id)
                                                                                   .Get().ToList();
                    if (_pickingAssign.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _pickingAssign.ForEach(items =>
                    {
                        items.IsActive = false;
                        items.DateModified = DateTime.Now;
                        items.UserModified = UserID;
                        _PickingAssignService.Modify(items);
                    });


                    RegisterTruck _registerTruck = _RegisterTruckService.Query().Filter(x => x.IsActive == true &&
                                                                                   x.ShippingCode == _picking.ShippingCode)
                                                                                   .Get().FirstOrDefault();

                    IRepository<RegisterTruck> _regisTruck = Context.Repository<RegisterTruck>();
                    RegisterTruck regisTruck = _regisTruck.Query().Filter(x => x.IsActive == true &&
                                                                     x.ShippingCode == _picking.ShippingCode)
                                                                     .Get().FirstOrDefault();

                    if (regisTruck != null)
                    {
                        regisTruck.ShippingStatus = (int)ShippingStatusEnum.New;
                        regisTruck.DateModified = DateTime.Now;
                        regisTruck.UserModified = UserID;
                        _regisTruck.Modify(regisTruck);
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
    }
}