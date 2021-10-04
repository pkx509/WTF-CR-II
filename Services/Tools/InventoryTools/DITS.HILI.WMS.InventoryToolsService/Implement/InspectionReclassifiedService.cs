using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.DispatchService;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.WMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class InspectionReclassifiedService : Repository<Reclassified>, IInspectionReclassifiedService
    {
        #region [ Property ]
        private readonly IRepository<Contact> contactService;
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductCodes> productCodeService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<ReclassifiedPrefix> prefixService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<ProductStatus> statusService;
        private readonly IRepository<Line> lineService;
        private readonly IRepository<Changestatus> changestatusService;
        private readonly IRepository<ReclassifiedDetail> reclassifiedDetailService;
        private readonly IRepository<ChangestatusPrefix> changestatusPrefixService;
        private readonly IStockService stockService;
        private readonly IRepository<ItfInterfaceMapping> itfInterfaceMappingService;



        private readonly IRepository<StockInfo> stockInfoService;
        private readonly IRepository<StockBalance> stockBalanceService;
        private readonly IRepository<StockTransaction> stockTransService;
        private readonly IRepository<StockLocationBalance> stockLocationService;


        private readonly IRepository<Zone> zoneService;
        private readonly IRepository<Dispatch> dispatchService;
        private readonly IRepository<DispatchDetail> dispatchDetailService;
        private readonly IRepository<DispatchBooking> dispatchBookingService;
        private readonly IRepository<DispatchPrefix> dispatchPrefixService;
        private readonly IRepository<ReceiveDetail> receiveDetailService;
        private readonly IRepository<Receive> receiveService;
        private readonly IDispatchDetailService _DispatchDetailService;
        private readonly IRepository<PickingPrefix> pickingPrefixService;
        private readonly IRepository<Picking> pickingService;
        private readonly IRepository<PickingAssign> pickingAssignService;
        private readonly IRepository<Reason> reasonService;
        private readonly IRepository<ShippingTo> shiptoService;

        #endregion
        public InspectionReclassifiedService(IUnitOfWork context,
                                 IRepository<ProductionControl> _productionControlService,
                                 IRepository<Product> _product,
                                 IRepository<ProductCodes> _productCode,
                                 IRepository<Line> _line,
                                 IRepository<ProductOwner> _productOwner,
                                 IRepository<Location> _location,
                                 IRepository<ProductUnit> _productUnit,
                                 IRepository<ProductStatus> _productStatus,
                                 IRepository<Warehouse> _warehouse,
                                 IRepository<ProductionControlDetail> _pcDetail,
                                 IRepository<ReclassifiedPrefix> _prefix,
                                 //IRepository<Changestatus> _changestatus,
                                 IRepository<ChangestatusPrefix> _changestatusPrefix,
                                 IRepository<Receiving> _receiving,
                                 IRepository<ReclassifiedDetail> _reclassifiedDetail,
                                 IStockService _stockServicee,
                              IDispatchDetailService tmpDispatchDetailService)
            : base(context)
        {
            _DispatchDetailService = tmpDispatchDetailService;
            pcDetailService = _pcDetail;
            productService = _product;
            locationService = _location;
            productUnitService = _productUnit;
            productOwnerService = _productOwner;
            lineService = _line;
            productionControlService = _productionControlService;
            warehouseService = _warehouse;
            stockService = _stockServicee;
            prefixService = _prefix;
            receivingService = _receiving;
            statusService = _productStatus;
            changestatusService = Context.Repository<Changestatus>();// _changestatus;
            changestatusPrefixService = _changestatusPrefix;
            reclassifiedDetailService = _reclassifiedDetail;
            productCodeService = _productCode;


            shiptoService = context.Repository<ShippingTo>();
            itfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
            zoneService = context.Repository<Zone>();
            dispatchService = context.Repository<Dispatch>();
            dispatchDetailService = context.Repository<DispatchDetail>();
            dispatchBookingService = context.Repository<DispatchBooking>();
            dispatchPrefixService = context.Repository<DispatchPrefix>();
            receiveService = context.Repository<Receive>();
            receiveDetailService = context.Repository<ReceiveDetail>();
            receiveDetailService = context.Repository<ReceiveDetail>();

            pickingPrefixService = context.Repository<PickingPrefix>();
            pickingService = context.Repository<Picking>();
            pickingAssignService = context.Repository<PickingAssign>();



            stockInfoService = context.Repository<StockInfo>();
            stockBalanceService = context.Repository<StockBalance>();
            stockTransService = context.Repository<StockTransaction>();
            stockLocationService = context.Repository<StockLocationBalance>();
            contactService = context.Repository<Contact>();
        }

        #region Reclassified
        public List<Reclassified> GetInspectionReclassified(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }

                totalRecords = 0;

                ReclassifiedStatus statusE = ReclassifiedStatus.Approve;
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    statusE = (ReclassifiedStatus)Enum.Parse(typeof(ReclassifiedStatus), status);
                }
                IEnumerable<Reclassified> list = from ch in Query().Get()
                                                 join p in productService.Query().Include(x => x.CodeCollection).Get() on ch.ProductID equals p.ProductID
                                                 where ch.IsActive == true && ch.DateCreated.Date >= sdte && ch.DateCreated.Date <= edte
                                                 && ((!string.IsNullOrEmpty(status) && status != "All" ? ch.ReclassStatus == statusE : "" == ""))
                                                 && (p.Name.ToLower().Contains(search.ToLower()) || p.Code.ToLower().Contains(search.ToLower()))

                                                 select (new Reclassified()
                                                 {
                                                     ReclassifiedID = ch.ReclassifiedID,
                                                     ReclassifiedCode = ch.ReclassifiedCode,
                                                     Remark = ch.Remark,
                                                     ReclassStatus = ch.ReclassStatus,
                                                     ReclassStatusName = ch.ReclassStatus.Description(),
                                                     Description = ch.Description,
                                                     ApproveDate = ch.ApproveDate,
                                                     EXPDate = ch.EXPDate,
                                                     LineID = ch.LineID,
                                                     MFGDate = ch.MFGDate,
                                                     MFGTimeEnd = ch.MFGTimeEnd,
                                                     MFGTimeFrom = ch.MFGTimeFrom,
                                                     ProductID = ch.ProductID,
                                                     ProductStatusID = ch.ProductStatusID,
                                                     FromProductStatusID = ch.FromProductStatusID,
                                                     ReclassFromLot = ch.ReclassFromLot,
                                                     ReclassToLot = ch.ReclassToLot,
                                                     DateCreated = ch.DateCreated,
                                                     IsApprove = (ch.ReclassStatus == ReclassifiedStatus.Approve || ch.ReclassStatus == ReclassifiedStatus.ApproveDispatch || ch.ReclassStatus == ReclassifiedStatus.SendtoReprocess),
                                                     ProductCode = p.Code,
                                                     ProductName = p.Name,
                                                 });


                totalRecords = list.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    list = list.OrderByDescending(x => x.ApproveDate).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public Reclassified GetInspectionReclassifiedByID(Guid id)
        {
            try
            {
                Reclassified result = Query().Filter(x => x.IsActive == true && x.ReclassifiedID == id).Get().FirstOrDefault();

                var result_detail = (from reclass in Query().Get()
                                     join reclass_detail in reclassifiedDetailService.Query().Get() on reclass.ReclassifiedID equals reclass_detail.ReclassifiedID
                                     join pallet in pcDetailService.Query().Get() on reclass_detail.FromPalletCode equals pallet.PalletCode
                                     join line in lineService.Query().Get() on reclass.LineID equals line.LineID
                                     join location in locationService.Query().Get() on reclass_detail.ReFromLoction equals location.LocationID into ps
                                     from lo in ps.DefaultIfEmpty()
                                     join product in productService.Query().Include(x => x.CodeCollection).Get() on reclass.ProductID equals product.ProductID
                                     join priceunit in productUnitService.Query().Get() on reclass_detail.ReclassifiedUnitID equals priceunit.ProductUnitID
                                     where reclass_detail.IsActive == true && reclass_detail.ReclassifiedID == id
                                     select new { reclass, reclass_detail, pallet, lo, product, priceunit, line });


                List<ItemReclassified> _detail = result_detail.Select(n => new ItemReclassified
                {
                    Location = n.lo == null ? "" : n.lo.Code,
                    PalletCode = n.pallet.PalletCode,
                    ProductName = n.product.Name,
                    PalletQty = n.pallet.RemainQTY.Value - n.pallet.ReserveQTY.Value,
                    ReclassifiedQty = n.reclass_detail.ReclassifiedQty,
                    UnitName = n.priceunit.Name,
                    ProductCode = n.product.Code,
                    Lot = n.pallet.LotNo,
                    MFGDate = n.pallet.MFGDate,
                    LineCode = n.line.LineCode,
                    ProductStatusID = n.pallet.ProductStatusID,
                    ProductID = n.reclass.ProductID,
                    ReclassifiedDetailID = n.reclass_detail.ReclassifiedDetailID,

                }).ToList();

                List<ProductStatus> status = statusService.Query().Filter(x => x.IsActive == true).Get().ToList();

                if (result.FromProductStatusID != null)
                {
                    result.FromProductStatus = status.Where(x => x.ProductStatusID == result.FromProductStatusID).FirstOrDefault().Description;
                }

                result.ProductStatus = status.Where(x => x.ProductStatusID == result.ProductStatusID).FirstOrDefault().Description;

                result.ReclassifiedItem = _detail;

                return result;
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

        public bool AddInspectionReclassified(List<ItemReclassified> _reclassList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var groups = _reclassList.GroupBy(g => new
                    {
                        g.ProductID,
                        g.MFGDate,
                        g.LineCode,
                        g.ProductStatusID,
                        g.FromProductStatusID,
                        g.Lot,
                        g.Description,
                        g.ApproveDate
                    }).Select(g => new
                    {
                        ProductID = g.Key.ProductID,
                        MFGDate = g.Key.MFGDate,
                        LineCode = g.Key.LineCode,
                        ProductStatusID = g.Key.ProductStatusID,
                        Lot = g.Key.Lot,
                        Description = g.Key.Description,
                        ApproveDate = g.Key.ApproveDate,
                        FromProductStatusID = g.Key.FromProductStatusID
                    }).ToList();

                    groups.ForEach(item =>
                    {
                        #region [ PreFix ]

                        ReclassifiedPrefix prefix = prefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                        if (prefix == null)
                        {
                            throw new HILIException("REG10012");
                        }

                        ReclassifiedPrefix tPrefix = prefixService.FindByID(prefix.PrefixID);

                        string reclassifiedCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);

                        tPrefix.IsLastest = false;

                        ReclassifiedPrefix newPrefix = new ReclassifiedPrefix()
                        {
                            IsLastest = true,
                            LastedKey = reclassifiedCode,
                            PrefixKey = prefix.PrefixKey,
                            FormatKey = prefix.FormatKey,
                            LengthKey = prefix.LengthKey
                        };

                        prefixService.Add(newPrefix);

                        #endregion [ PreFix ] 

                        ProductStatus product_status = statusService.Query().Filter(x => x.IsDefault == true).Get().FirstOrDefault();

                        if (product_status == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        Line line = lineService.Query().Filter(x => x.LineCode == item.LineCode).Get().First();
                        Reclassified reclass = new Reclassified
                        {
                            ReclassifiedID = Guid.NewGuid(),
                            ReclassifiedCode = reclassifiedCode,
                            ReclassFromLot = item.Lot,
                            ReclassToLot = product_status.ProductStatusID == item.FromProductStatusID ? item.Lot + "H" : item.Lot.Replace("H", ""),
                            ReclassStatus = ReclassifiedStatus.Reclassified,
                            Description = item.Description,
                            ProductID = item.ProductID,
                            LineID = line.LineID,
                            ProductStatusID = item.ProductStatusID,
                            MFGDate = item.MFGDate,
                            DateCreated = DateTime.Now,
                            UserCreated = UserID,
                            DateModified = DateTime.Now,
                            UserModified = UserID,
                            IsActive = true,
                            FromProductStatusID = item.FromProductStatusID,
                            IsApprove = false,

                        };

                        if (item.ApproveDate != DateTime.MinValue)
                        {
                            reclass.ApproveDate = item.ApproveDate;

                        }
                        Add(reclass);


                        List<ItemReclassified> reclass_detail = _reclassList.Where(x => x.ProductID == item.ProductID && x.MFGDate == item.MFGDate
                                               && x.LineCode == item.LineCode && x.ProductStatusID == item.ProductStatusID
                                               && x.Lot == item.Lot && x.Description == item.Description).ToList(); ;

                        reclass_detail.ForEach(detail =>
                        {

                            ProductionControlDetail pc = pcDetailService.Query().Filter(x => x.PalletCode == detail.PalletCode).Get().FirstOrDefault();

                            Location lo = locationService.Query().Filter(x => x.LocationID == pc.LocationID).Include(x => x.Zone).Get().FirstOrDefault();


                            ReclassifiedDetail reclassDetail = new ReclassifiedDetail
                            {
                                ReclassifiedID = reclass.ReclassifiedID,
                                ReclassifiedDetailID = Guid.NewGuid(),
                                PalletCode = product_status.ProductStatusID == item.FromProductStatusID ? detail.PalletCode + "H" : detail.PalletCode.Replace("H", ""),
                                ReclassifiedQty = detail.ReclassifiedQty,
                                ReclassifiedBaseQty = detail.ReclassifiedQty * pc.ConversionQty.Value,
                                ReclassifiedUnitID = pc.StockUnitID.Value,
                                ReclassifiedBaseUnitID = pc.BaseUnitID.Value,
                                ConversionQty = pc.ConversionQty.Value,
                                //ReclassifiedDetailStatus = ReclassifiedDetailStatus.Reclassified,
                                ReFromWarehouse = lo.Zone.WarehouseID,
                                ReFromLoction = pc.LocationID,
                                DateCreated = DateTime.Now,
                                UserCreated = UserID,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                IsActive = true,
                                //ReprocessQty = 0,
                                TotalReprocessBaseQty = 0,
                                TotalReprocessQty = 0,
                                //RejectQty = 0,
                                TotalRejectQty = 0,
                                TotalRejectBaseQty = 0,
                                FromPalletCode = detail.PalletCode,
                                ReprocessPalletCode = detail.PalletCode + "H",
                                ReprocessLot = detail.Lot + "H",
                                RejectLot = detail.Lot + "R",
                                RejectPalletCode = detail.PalletCode + "R",
                            };

                            reclassifiedDetailService.Add(reclassDetail);


                            pc.ReserveQTY += detail.ReclassifiedQty;
                            pc.ReserveBaseQTY += pc.ConversionQty.Value * detail.ReclassifiedQty;
                            pc.DateModified = DateTime.Now;
                            pc.UserModified = UserID;
                            pcDetailService.Modify(pc);
                        });
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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool SaveInspectionReclassified(Reclassified _reclass, bool isApprove = false)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    Reclassified reclass = Query().Filter(x => x.ReclassifiedID == _reclass.ReclassifiedID).Get().SingleOrDefault();
                    if (_reclass.ApproveDate != DateTime.MinValue)
                    {
                        reclass.ApproveDate = _reclass.ApproveDate;

                    }
                    reclass.Description = _reclass.Description;
                    reclass.UserModified = UserID;
                    reclass.DateModified = DateTime.Now;
                    if (isApprove)
                    {
                        reclass.ReclassStatus = ReclassifiedStatus.Approve;
                    }
                    Modify(reclass);

                    SaveData(_reclass);

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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool ApproveInspectionReclassified(Reclassified _reclass)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductStatus product_status = statusService.Query().Filter(x => x.IsDefault == true).Get().FirstOrDefault();
                    Reclassified reclass = Query().Filter(x => x.ReclassifiedID == _reclass.ReclassifiedID).Get().SingleOrDefault();
                    if (_reclass.ApproveDate != DateTime.MinValue)
                    {
                        reclass.ApproveDate = _reclass.ApproveDate;

                    }
                    reclass.ApproveBy = UserID;
                    reclass.Description = _reclass.Description;
                    reclass.UserModified = UserID;
                    reclass.DateModified = DateTime.Now;
                    reclass.ReclassStatus = ReclassifiedStatus.Approve;
                    Modify(reclass);


                    SaveData(_reclass);



                    List<ReclassifiedDetail> _detail_list = reclassifiedDetailService.Query().Filter(x => x.ReclassifiedID == reclass.ReclassifiedID && x.IsActive == true).Get().ToList();

                    _detail_list.ForEach(item =>
                    {

                        ProductionControlDetail _pc = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == item.FromPalletCode).Get().SingleOrDefault();
                        ProductionControlDetail pc = pcDetailService.FindByID(_pc.PackingID);


                        Location location = locationService.Query().Filter(x => x.LocationID == pc.LocationID).Include(x => x.Zone).Get().SingleOrDefault();

                        pc.RemainQTY -= item.ReclassifiedQty;
                        pc.RemainBaseQTY -= pc.ConversionQty.Value * item.ReclassifiedQty;


                        pc.ReserveQTY -= item.ReclassifiedQty;
                        pc.ReserveBaseQTY -= pc.ConversionQty.Value * item.ReclassifiedQty;

                        pc.PackingStatus = PackingStatusEnum.QAInspection;
                        pc.UserModified = UserID;
                        pc.DateModified = DateTime.Now;


                        if (pc.RemainQTY == 0)
                        {
                            pc.LocationID = null;
                            pc.SugguestLocationID = null;
                        }

                        pcDetailService.Modify(pc);



                        ProductionControlDetail pcI = pcDetailService.Query().Filter(x => x.PalletCode == item.PalletCode).Get().SingleOrDefault();


                        if (pcI == null)
                        {
                            Receiving _receiving = receivingService.Query().Filter(x => x.PalletCode == item.FromPalletCode).Get().SingleOrDefault();


                            int? seq = pcDetailService.Query().Filter(x => x.ControlID == pc.ControlID).Get().Max(x => x.Sequence) + 1;

                            pcI = new ProductionControlDetail
                            {
                                PackingID = Guid.NewGuid(),
                                ControlID = pc.ControlID,
                                PalletCode = item.PalletCode,
                                Sequence = seq,
                                StockQuantity = pc.StockQuantity,
                                BaseQuantity = pc.BaseQuantity,
                                ConversionQty = pc.ConversionQty,
                                StockUnitID = pc.StockUnitID,
                                BaseUnitID = pc.BaseUnitID,
                                ProductStatusID = reclass.ProductStatusID,
                                ProductSubStatusID = pc.ProductSubStatusID,
                                MFGDate = pc.MFGDate,
                                MFGTimeStart = pc.MFGTimeStart,
                                MFGTimeEnd = pc.MFGTimeEnd,
                                LocationID = location.LocationID,
                                WarehouseID = location.Zone.WarehouseID,
                                PackingStatus = PackingStatusEnum.QAInspection,
                                RemainStockUnitID = pc.RemainStockUnitID,
                                RemainBaseUnitID = pc.RemainBaseUnitID,
                                RemainQTY = item.ReclassifiedQty,
                                RemainBaseQTY = item.ReclassifiedQty * pc.ConversionQty,
                                LotNo = product_status.ProductStatusID == reclass.FromProductStatusID ? pc.LotNo + "H" : pc.LotNo.Replace("H", ""),
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                ReserveBaseQTY = item.ReclassifiedQty * pc.ConversionQty,
                                ReserveQTY = item.ReclassifiedQty,
                                ReceiveDetailID = pc.ReceiveDetailID
                            };


                            pcDetailService.Add(pcI);

                            Receiving rcv = new Receiving
                            {
                                Sequence = seq.Value,
                                GRNCode = _receiving.GRNCode.Substring(0, _receiving.GRNCode.Length - 1) + seq.Value,
                                Quantity = item.ReclassifiedQty,
                                BaseQuantity = item.ReclassifiedQty * pc.ConversionQty.Value,
                                PalletCode = pcI.PalletCode,

                                ReceiveID = _receiving.ReceiveID,
                                ProductOwnerID = _receiving.ProductOwnerID,
                                SupplierID = _receiving.SupplierID,
                                LocationID = pcI.LocationID, // DummyLocation

                                ReceiveDetailID = _receiving.ReceiveDetailID,
                                ProductID = _receiving.ProductID,
                                Lot = pcI.LotNo,
                                ManufacturingDate = _receiving.ManufacturingDate,
                                ExpirationDate = _receiving.ExpirationDate,
                                ConversionQty = _receiving.ConversionQty,
                                StockUnitID = _receiving.StockUnitID,
                                BaseUnitID = _receiving.BaseUnitID,
                                ProductStatusID = pcI.ProductStatusID.Value,
                                ProductSubStatusID = pcI.ProductSubStatusID,

                                IsDraft = false,
                                IsSentInterface = false,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,

                                Remark = "Inspection Reclassified",
                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,

                            };

                            receivingService.Add(rcv);

                        }
                        else
                        {
                            pcI.UserModified = UserID;
                            pcI.DateModified = DateTime.Now;

                            pcI.RemainQTY += item.ReclassifiedQty;
                            pcI.RemainBaseQTY += pcI.RemainQTY * pcI.ConversionQty;
                            if (pcI.LocationID == null)
                            {
                                pcI.LocationID = location.LocationID;
                            }
                            if (pcI.ProductStatusID != product_status.ProductStatusID)
                            {
                                pcI.ReserveBaseQTY += item.ReclassifiedQty * pcI.ConversionQty;
                                pcI.ReserveQTY += item.ReclassifiedQty;
                            }
                            else
                            {
                                pcI.PackingStatus = PackingStatusEnum.PutAway;
                            }
                            pcDetailService.Modify(pcI);
                        }

                        pcI = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == item.PalletCode).Get().SingleOrDefault();
                        Receiving receiving = receivingService.Query().Filter(x => x.PalletCode == item.PalletCode).Get().SingleOrDefault();

                        pc = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == item.FromPalletCode).Get().SingleOrDefault();

                        List<StockInOutModel> stockout = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = pc.ProductionControl.ProductID,
                                StockUnitID = pc.StockUnitID.Value,
                                BaseUnitID = pc.BaseUnitID.Value,
                                Lot = pc.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.Value,
                                SupplierID = receiving.SupplierID.Value,
                                ManufacturingDate = pc.MFGDate.Value,
                                ExpirationDate = receiving.ExpirationDate.Value,
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = reclass.FromProductStatusID.Value,
                                ProductSubStatusID = pc.ProductSubStatusID.Value,
                                Quantity = item.ReclassifiedQty,
                                ConversionQty = pc.ProductionControl.ConversionQty.Value,
                                PalletCode = pc.PalletCode,
                                LocationCode = location.Code,
                                DocumentCode = reclass.ReclassifiedCode,
                                DocumentTypeID = null,
                                DocumentID = item.ReclassifiedDetailID
                            }
                        };
                        stockService.UserID = UserID;



                        List<StockInOutModel> stockChange = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = pcI.ProductionControl.ProductID,
                                StockUnitID = pcI.StockUnitID.Value,
                                BaseUnitID = pcI.BaseUnitID.Value,
                                Lot = pcI.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.Value,
                                SupplierID = receiving.SupplierID.Value,
                                ManufacturingDate = pcI.MFGDate.Value,
                                ExpirationDate = receiving.ExpirationDate.Value,
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = reclass.ProductStatusID.Value,
                                ProductSubStatusID = pcI.ProductSubStatusID.Value,
                                Quantity = item.ReclassifiedQty,
                                ConversionQty = pcI.ProductionControl.ConversionQty.Value,
                                PalletCode = pcI.PalletCode,
                                LocationCode = location.Code,
                                DocumentCode = reclass.ReclassifiedCode,
                                DocumentTypeID = null,
                                DocumentID = item.ReclassifiedDetailID
                            }
                        };
                        stockService.InspectionReclassify(stockout, stockChange);

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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public void SaveData(Reclassified _reclass)
        {
            ProductStatus product_status = statusService.Query().Filter(x => x.IsDefault == true).Get().FirstOrDefault();
            List<ReclassifiedDetail> _reclass_detail = reclassifiedDetailService.Query().Filter(x => x.ReclassifiedID == _reclass.ReclassifiedID && x.IsActive == true).Get().ToList();

            List<Guid> ids = _reclass.ReclassifiedItem.Select(x => x.ReclassifiedDetailID).ToList();


            _reclass.ReclassifiedItem.ForEach(detail =>
            {
                ReclassifiedDetail _detail = _reclass_detail.Where(x => x.ReclassifiedDetailID == detail.ReclassifiedDetailID).FirstOrDefault();

                ProductionControlDetail pc = pcDetailService.Query().Filter(x => x.PalletCode == detail.PalletCode).Get().FirstOrDefault();
                Location lo = locationService.Query().Filter(x => x.LocationID == pc.LocationID).Include(x => x.Zone).Get().FirstOrDefault();

                if (_detail == null)
                {
                    #region Add detail
                    ReclassifiedDetail reclassDetail = new ReclassifiedDetail
                    {
                        ReclassifiedID = _reclass.ReclassifiedID,
                        ReclassifiedDetailID = Guid.NewGuid(),
                        PalletCode = product_status.ProductStatusID == detail.FromProductStatusID ? detail.PalletCode + "H" : detail.PalletCode.Replace("H", ""),
                        ReclassifiedQty = detail.ReclassifiedQty,
                        ReclassifiedBaseQty = detail.ReclassifiedQty * pc.ConversionQty.Value,
                        ReclassifiedUnitID = pc.StockUnitID.Value,
                        ReclassifiedBaseUnitID = pc.BaseUnitID.Value,
                        ConversionQty = pc.ConversionQty.Value,
                        //ReclassifiedDetailStatus = ReclassifiedDetailStatus.Reclassified,
                        ReFromWarehouse = lo.Zone.WarehouseID,
                        ReFromLoction = pc.LocationID,
                        DateCreated = DateTime.Now,
                        UserCreated = UserID,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        IsActive = true,
                        //ReprocessQty = 0,
                        TotalReprocessBaseQty = 0,
                        TotalReprocessQty = 0,
                        //RejectQty = 0,
                        TotalRejectQty = 0,
                        TotalRejectBaseQty = 0,
                        FromPalletCode = detail.PalletCode,
                        ReprocessPalletCode = detail.PalletCode + "H",
                        ReprocessLot = detail.Lot + "H",
                        RejectLot = detail.Lot + "R",
                        RejectPalletCode = detail.PalletCode + "R",


                    };

                    reclassifiedDetailService.Add(reclassDetail);
                    #endregion

                    pc.ReserveQTY += detail.ReclassifiedQty;
                    pc.ReserveBaseQTY += pc.ConversionQty.Value * detail.ReclassifiedQty;
                    pc.DateModified = DateTime.Now;
                    pc.UserModified = UserID;
                    pcDetailService.Modify(pc);
                }
            });

            _reclass_detail.Where(x => !ids.Contains(x.ReclassifiedDetailID)).ToList().ForEach(itm =>
            {
                itm.UserModified = UserID;
                itm.DateModified = DateTime.Now;
                itm.IsActive = false;
                reclassifiedDetailService.Modify(itm);

                ProductionControlDetail pc = pcDetailService.Query().Filter(x => x.PalletCode == itm.FromPalletCode).Get().FirstOrDefault();
                pc.ReserveQTY -= itm.ReclassifiedQty;
                pc.ReserveBaseQTY -= pc.ConversionQty.Value * itm.ReclassifiedQty;
                pc.DateModified = DateTime.Now;
                pc.UserModified = UserID;
                pcDetailService.Modify(pc);
            });
        }

        public List<PalletTagModel> GetPalletTag(string palletNo, string productCode, string productName, string Lot, string Line, string mfgDate, Guid producttsatusId, out int totalRecords, int? pageIndex, int? pageSize, string WHReferenceCode)
        {
            try
            {
                palletNo = (string.IsNullOrEmpty(palletNo) ? "" : palletNo);
                productCode = (string.IsNullOrEmpty(productCode) ? "" : productCode);
                productName = (string.IsNullOrEmpty(productName) ? "" : productName);
                Lot = (string.IsNullOrEmpty(Lot) ? "" : Lot);
                Line = (string.IsNullOrEmpty(Line) ? "" : Line);
                mfgDate = (string.IsNullOrEmpty(mfgDate) ? "" : mfgDate);


                var result = (from pc in productionControlService.Query().Get()
                              join pc_detail in pcDetailService.Query().Get() on pc.ControlID equals pc_detail.ControlID
                              join line in lineService.Query().Get() on pc.LineID equals line.LineID
                              join product in productService.Query().Include(x => x.CodeCollection).Get() on pc.ProductID equals product.ProductID

                              join priceunit in productUnitService.Query().Get() on pc_detail.StockUnitID equals priceunit.ProductUnitID
                              join location in locationService.Query().Include(x => x.Zone).Get() on pc_detail.LocationID equals location.LocationID
                              join wh in warehouseService.Query().Get() on location.Zone.WarehouseID equals wh.WarehouseID
                              where (pc_detail.PackingStatus != PackingStatusEnum.Waiting_Receive && pc_detail.PackingStatus != PackingStatusEnum.In_Progress
                              && pc_detail.PackingStatus != PackingStatusEnum.Loading_In && pc_detail.PackingStatus != PackingStatusEnum.Cancel)
                              && wh.ReferenceCode == WHReferenceCode
                              && (pc_detail.RemainQTY - pc_detail.ReserveQTY) > 0
                              && pc_detail.ProductStatusID == producttsatusId
                              select new { pc, pc_detail, product, priceunit, location, line });

                if (!string.IsNullOrWhiteSpace(palletNo))
                {
                    result = result.Where(x => x.pc_detail.PalletCode.ToLower().Contains(palletNo.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    result = result.Where(x => x.product.ProductCode.ToLower().Contains(productCode.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(Lot))
                {
                    result = result.Where(x => x.pc_detail.LotNo.ToLower().Contains(Lot.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(productName))
                {
                    result = result.Where(x => x.product.Name.ToLower().Contains(productName.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(Line))
                {
                    result = result.Where(x => x.line.LineCode.ToLower().Contains(Line.ToLower()));
                }

                totalRecords = result.Count();

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.product.ProductCode).ThenBy(x => x.pc_detail.PalletCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<PalletTagModel> ret = result.Select(n => new PalletTagModel
                {
                    Location = n.location.Code,
                    PalletCode = n.pc_detail.PalletCode,
                    ProductName = n.product.Name,
                    Qty = n.pc_detail.RemainQTY - n.pc_detail.ReserveQTY,
                    UnitName = n.priceunit.Name,
                    ProductCode = n.product.Code,
                    LotNo = n.pc_detail.LotNo,
                    MFGDate = n.pc_detail.MFGDate,
                    LineCode = n.line.LineCode,
                    ProductStatusID = n.pc_detail.ProductStatusID,
                    ProductID = n.pc.ProductID,

                }).ToList();



                return ret;
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

        #region Dispatch 

        public List<Reclassified> GetInspectionDispatch(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }

                totalRecords = 0;

                ReclassifiedStatus statusE = ReclassifiedStatus.Approve;
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    statusE = (ReclassifiedStatus)Enum.Parse(typeof(ReclassifiedStatus), status);
                }
                IEnumerable<Reclassified> list = from ch in Query().Get()
                                                 join p in productService.Query().Include(x => x.CodeCollection).Get() on ch.ProductID equals p.ProductID
                                                 join s in statusService.Query().Get() on ch.ProductStatusID equals s.ProductStatusID
                                                 where ch.IsActive == true && s.IsDefault == false && ch.DateCreated.Date >= sdte && ch.DateCreated.Date <= edte
                                                 && ((!string.IsNullOrEmpty(status) && status != "All" ? ch.ReclassStatus == statusE : "" == ""))
                                                 && (p.Name.ToLower().Contains(search.ToLower()) || p.Code.ToLower().Contains(search.ToLower()))

                                                 select (new Reclassified()
                                                 {
                                                     ReclassifiedID = ch.ReclassifiedID,
                                                     ReclassifiedCode = ch.ReclassifiedCode,
                                                     Remark = ch.Remark,
                                                     ReclassStatus = ch.ReclassStatus,
                                                     ReclassStatusName = ch.ReclassStatus.Description(),
                                                     Description = ch.Description,
                                                     ApproveDate = ch.ApproveDate,
                                                     EXPDate = ch.EXPDate,
                                                     LineID = ch.LineID,
                                                     MFGDate = ch.MFGDate,
                                                     MFGTimeEnd = ch.MFGTimeEnd,
                                                     MFGTimeFrom = ch.MFGTimeFrom,
                                                     ProductID = ch.ProductID,
                                                     ProductStatusID = ch.ProductStatusID,
                                                     FromProductStatusID = ch.FromProductStatusID,
                                                     ReclassFromLot = ch.ReclassFromLot,
                                                     ReclassToLot = ch.ReclassToLot,
                                                     DateCreated = ch.DateCreated,
                                                     IsApprove = ch.ApproveDispatchDate != null,
                                                     ProductCode = p.Code,
                                                     ProductName = p.Name,
                                                 });


                totalRecords = list.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    list = list.OrderByDescending(x => x.ReclassifiedCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public bool ApproveInspectionDispatch(Reclassified _reclass)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductStatus product_status = statusService.Query().Filter(x => x.IsDefault == true).Get().FirstOrDefault();
                    Reclassified reclass = FindByID(_reclass.ReclassifiedID);
                    reclass.ApproveDispatchDate = _reclass.ApproveDispatchDate;
                    reclass.Description = _reclass.Description;
                    reclass.UserModified = UserID;
                    reclass.ApproveDispatchBy = UserID;
                    reclass.DateModified = DateTime.Now;
                    reclass.ReclassStatus = ReclassifiedStatus.ApproveDispatch;

                    Modify(reclass);


                    List<ItemReclassified> reject = _reclass.ReclassifiedItem.Where(x => x.RejectQty > 0).ToList();
                    if (reject.Count > 0)
                    {
                        ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                          .Filter(x => x.IsActive
                                              && x.IsQADamage == true).Get().FirstOrDefault();


                        if (dispatchType == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        SendToDispatch(reject, dispatchType, true);
                    }
                    List<ItemReclassified> reprocess = _reclass.ReclassifiedItem.Where(x => x.ReprocessQty > 0).ToList();
                    if (reprocess.Count > 0)
                    {
                        ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                          .Filter(x => x.IsActive
                                              && x.IsQAReprocessFromHold == true).Get().FirstOrDefault();

                        if (dispatchType == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        SendToDispatch(reprocess, dispatchType, false);
                    }

                    _reclass.ReclassifiedItem.ForEach(detail =>
                    {
                        ReclassifiedDetail reclass_detail = reclassifiedDetailService.Query().Filter(x => x.ReclassifiedDetailID == detail.ReclassifiedDetailID).Get().SingleOrDefault();

                        ProductionControlDetail _pallet = pcDetailService.Query().Filter(x => x.PalletCode == reclass_detail.ReprocessPalletCode).Get().SingleOrDefault();//pallet + H
                        ProductionControlDetail holdPallet = pcDetailService.FindByID(_pallet.PackingID);

                        holdPallet.ReserveQTY -= reclass_detail.ReclassifiedQty - reclass_detail.TotalReprocessQty - reclass_detail.TotalRejectQty;
                        holdPallet.ReserveBaseQTY -= (reclass_detail.ReclassifiedQty - reclass_detail.TotalReprocessQty - reclass_detail.TotalRejectQty) * reclass_detail.ConversionQty;

                        holdPallet.UserModified = UserID;
                        holdPallet.DateModified = DateTime.Now;

                        pcDetailService.Modify(holdPallet);
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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public Reclassified GetInspectionDispatchByID(Guid id)
        {
            try
            {
                Reclassified result = Query().Filter(x => x.IsActive == true && x.ReclassifiedID == id).Get().FirstOrDefault();

                var result_detail = (from reclass in Query().Get()
                                     join reclass_detail in reclassifiedDetailService.Query().Get() on reclass.ReclassifiedID equals reclass_detail.ReclassifiedID
                                     join pallet in pcDetailService.Query().Get() on reclass_detail.PalletCode equals pallet.PalletCode
                                     join line in lineService.Query().Get() on reclass.LineID equals line.LineID
                                     join location in locationService.Query().Get() on pallet.LocationID equals location.LocationID
                                     join product in productService.Query().Include(x => x.CodeCollection).Get() on reclass.ProductID equals product.ProductID
                                     join priceunit in productUnitService.Query().Get() on reclass_detail.ReclassifiedUnitID equals priceunit.ProductUnitID
                                     where reclass_detail.IsActive == true && reclass_detail.ReclassifiedID == id
                                     select new { reclass, reclass_detail, pallet, location, product, priceunit, line });


                List<ItemReclassified> _detail = result_detail.Select(n => new ItemReclassified
                {
                    Location = n.location.Code,
                    PalletCode = n.pallet.PalletCode,
                    ProductName = n.product.Name,
                    PalletQty = n.pallet.RemainQTY.Value,
                    ReclassifiedQty = n.reclass_detail.ReclassifiedQty - n.reclass_detail.TotalRejectQty - n.reclass_detail.TotalReprocessQty,
                    UnitName = n.priceunit.Name,
                    ProductCode = n.product.Code,
                    Lot = n.pallet.LotNo,
                    MFGDate = n.pallet.MFGDate,
                    LineCode = n.line.LineCode,
                    ProductStatusID = n.pallet.ProductStatusID,
                    ProductID = n.reclass.ProductID,
                    ReclassifiedDetailID = n.reclass_detail.ReclassifiedDetailID,
                    ReclassifiedCode = n.reclass.ReclassifiedCode,
                    ReclassifiedID = n.reclass.ReclassifiedID,

                }).ToList();

                List<ProductStatus> status = statusService.Query().Filter(x => x.IsActive == true).Get().ToList();

                if (result.FromProductStatusID != null)
                {
                    result.FromProductStatus = status.Where(x => x.ProductStatusID == result.FromProductStatusID).FirstOrDefault().Description;
                }

                result.ProductStatus = status.Where(x => x.ProductStatusID == result.ProductStatusID).FirstOrDefault().Description;

                result.ReclassifiedItem = _detail;

                return result;
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

        public bool SendtoReprocess(List<ItemReclassified> list)
        {
            try
            {

                if (list.Count == 0)
                {
                    throw new HILIException("MSG00006");
                }

                ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                        .Filter(x => x.IsActive
                                            && x.IsQAReprocessFromHold == true).Get().FirstOrDefault();

                if (dispatchType == null)
                {
                    throw new HILIException("MSG00006");
                }

                SendToDispatch(list, dispatchType, false);



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

        public bool SendtoDamage(List<ItemReclassified> list)
        {
            try
            {
                if (list.Count == 0)
                {
                    throw new HILIException("MSG00006");
                }

                ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                        .Filter(x => x.IsActive
                                            && x.IsQADamage == true).Get().FirstOrDefault();


                if (dispatchType == null)
                {
                    throw new HILIException("MSG00006");
                }

                SendToDispatch(list, dispatchType, true);


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

        public void SendToDispatch(List<ItemReclassified> list, ItfInterfaceMapping dispatchType, bool isReject)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                DateTime dateNow = DateTime.Now;
                IEnumerable<Warehouse> warehouse = warehouseService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Zone> zone = zoneService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Location> location = locationService.Query().Filter(x => x.IsActive).Get();

                #region Find Shipto [Hard Code Fix later]

                ShippingTo shipto = shiptoService.Query().Filter(x => x.IsActive && x.Description.Contains("Others")).Get().FirstOrDefault();
                if (shipto == null)
                {
                    shipto = shiptoService.Query().Filter(x => x.IsActive).Get().FirstOrDefault();
                    if (shipto == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                }

                #endregion

                #region Find DispatchType


                if (dispatchType == null)
                {
                    throw new HILIException("MSG00064");
                }

                #endregion

                #region Find Dispatch Prefix

                DispatchPrefix prefix = dispatchPrefixService.Query().Filter(x => x.PrefixType == DispatchPreFixTypeEnum.DISPATHCODE).Get().FirstOrDefault();
                if (prefix == null)
                {
                    throw new HILIException("MSG00063");
                }

                DispatchPrefix tPrefix = dispatchPrefixService.FindByID(prefix.PrefixId);
                string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                tPrefix.LastedKey = DispatchCode;
                dispatchPrefixService.Modify(tPrefix);

                #endregion

                #region Find Dispatch Prefix

                DispatchPrefix _prefixPO = dispatchPrefixService.Query().Filter(x => x.PrefixType == DispatchPreFixTypeEnum.PONO_INTERNAL).Get().SingleOrDefault();
                if (_prefixPO == null)
                {
                    throw new HILIException("MSG00006");
                }

                DispatchPrefix prefixPO = dispatchPrefixService.FindByID(_prefixPO.PrefixId);


                string PO = Prefix.OnCreatePrefixed(prefixPO.LastedKey, prefixPO.PrefixKey, prefixPO.FormatKey, prefixPO.LengthKey);

                prefixPO.LastedKey = PO;

                dispatchPrefixService.Modify(prefixPO);

                #endregion

                #region Insert Dispatch

                Guid? supplierID = contactService.Query().Filter(x => x.IsActive && x.Code == "20004431").Get().FirstOrDefault()?.ContactID;
                Dispatch dispatchModel = new Dispatch()
                {
                    Pono = PO,

                    DispatchStatus = DispatchStatusEnum.InprogressConfirmQA,
                    DocumentId = dispatchType.DocumentId,
                    DispatchCode = DispatchCode,
                    DeliveryDate = dateNow,
                    DocumentDate = dateNow,
                    OrderDate = dateNow,
                    CustomerId = supplierID,
                    ShipptoId = shipto.ShipToId,
                    IsUrgent = false,
                    IsBackOrder = false,
                    IsActive = true,
                    Remark = "Inspection Dispatch",
                    UserCreated = UserID,
                    DateCreated = dateNow,
                    UserModified = UserID,
                    DateModified = dateNow
                };

                dispatchService.Add(dispatchModel);

                #endregion

                List<StockSearch> stockListModel = new List<StockSearch>();
                List<DispatchDetailCustom> dispatchDetailCustoms = new List<DispatchDetailCustom>();

                int seq = 1;
                foreach (ItemReclassified itm in list)
                {
                    ReclassifiedDetail _reclass_detail = reclassifiedDetailService.Query().Filter(x => x.ReclassifiedDetailID == itm.ReclassifiedDetailID).Get().SingleOrDefault();
                    ReclassifiedDetail reclass_detail = reclassifiedDetailService.FindByID(_reclass_detail.ReclassifiedDetailID);
                    Reclassified _reclass = Query().Filter(x => x.ReclassifiedID == reclass_detail.ReclassifiedID).Get().SingleOrDefault();
                    Reclassified reclass = FindByID(_reclass.ReclassifiedID);

                    if (reclass.ReclassStatus != ReclassifiedStatus.SendtoReprocess && reclass.ReclassStatus != ReclassifiedStatus.ApproveDispatch)
                    {
                        reclass.ReclassStatus = ReclassifiedStatus.SendtoReprocess;
                        reclass.UserModified = UserID;
                        reclass.DateModified = DateTime.Now;
                        Modify(reclass);
                    }

                    #region Reject
                    if (isReject)
                    {
                        reclass_detail.TotalRejectQty += itm.RejectQty;
                        reclass_detail.TotalRejectBaseQty += itm.RejectQty * reclass_detail.ConversionQty;


                        ProductionControlDetail normalPallet = pcDetailService.Query().Filter(x => x.PalletCode == itm.PalletCode).Get().SingleOrDefault();
                        ProductionControlDetail holdPallet = pcDetailService.Query().Filter(x => x.PalletCode == reclass_detail.ReprocessPalletCode).Get().SingleOrDefault();//pallet + H
                        Location hold_location = locationService.Query().Filter(x => x.LocationID == holdPallet.LocationID).Get().SingleOrDefault();

                        Receiving receiving = receivingService.Query().Filter(x => x.PalletCode == normalPallet.PalletCode).Get().SingleOrDefault();

                        #region Out Hold -In Reject
                        ProductionControlDetail pcI = pcDetailService.Query().Filter(x => x.PalletCode == reclass_detail.RejectPalletCode).Get().SingleOrDefault();

                        #region Gen pallet

                        if (pcI == null)
                        {
                            ProductStatus status = statusService.Query().Filter(x => x.IsInspectionReclassify == true && x.IsDefault == false).Get().SingleOrDefault();
                            int? r_seq = pcDetailService.Query().Filter(x => x.ControlID == normalPallet.ControlID).Get().Max(x => x.Sequence) + 1;
                            pcI = new ProductionControlDetail
                            {
                                PackingID = Guid.NewGuid(),
                                ControlID = normalPallet.ControlID,
                                PalletCode = reclass_detail.RejectPalletCode,
                                Sequence = r_seq,
                                StockQuantity = normalPallet.StockQuantity,
                                BaseQuantity = normalPallet.BaseQuantity,
                                ConversionQty = normalPallet.ConversionQty,
                                StockUnitID = normalPallet.StockUnitID,
                                BaseUnitID = normalPallet.BaseUnitID,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = normalPallet.ProductSubStatusID,
                                MFGDate = normalPallet.MFGDate,
                                MFGTimeStart = normalPallet.MFGTimeStart,
                                MFGTimeEnd = normalPallet.MFGTimeEnd,
                                LocationID = hold_location.LocationID,
                                WarehouseID = normalPallet.WarehouseID,
                                PackingStatus = PackingStatusEnum.QAInspection,
                                RemainStockUnitID = normalPallet.RemainStockUnitID,
                                RemainBaseUnitID = normalPallet.RemainBaseUnitID,
                                RemainQTY = itm.RejectQty,
                                RemainBaseQTY = itm.RejectQty * normalPallet.ConversionQty,
                                LotNo = reclass_detail.RejectLot,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                ReceiveDetailID = normalPallet.ReceiveDetailID,
                                ReserveBaseQTY = itm.RejectQty * normalPallet.ConversionQty,
                                ReserveQTY = itm.RejectQty
                            };
                            pcDetailService.Add(pcI);

                            Receiving _receiving = new Receiving
                            {
                                Sequence = r_seq.Value,
                                GRNCode = receiving.GRNCode.Substring(0, receiving.GRNCode.Length - 1) + r_seq.Value,
                                Quantity = itm.RejectQty,
                                BaseQuantity = itm.RejectQty * pcI.ConversionQty.Value,
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
                                ProductStatusID = pcI.ProductStatusID.Value,
                                ProductSubStatusID = pcI.ProductSubStatusID,

                                IsDraft = false,
                                IsSentInterface = false,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,

                                Remark = "Inspection Dispatch",
                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                            };

                            receivingService.Add(_receiving);
                        }
                        else
                        {
                            pcI.UserModified = UserID;
                            pcI.DateModified = DateTime.Now;
                            pcI.RemainQTY += itm.RejectQty;
                            pcI.RemainBaseQTY += itm.RejectQty * pcI.ConversionQty;
                            pcI.ReserveQTY += itm.RejectQty;
                            pcI.ReserveBaseQTY += itm.RejectQty * pcI.ConversionQty;
                            pcDetailService.Modify(pcI);
                        }
                        #endregion

                        ProductionControlDetail rejectPallet = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == reclass_detail.RejectPalletCode).Get().SingleOrDefault();

                        List<StockInOutModel> stockOut = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = receiving.ProductID,
                                StockUnitID = receiving.StockUnitID,
                                BaseUnitID = receiving.BaseUnitID,
                                Lot = holdPallet.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.Value,
                                SupplierID = receiving.SupplierID.Value,
                                ManufacturingDate = holdPallet.MFGDate.Value,
                                ExpirationDate = receiving.ExpirationDate.Value,
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = holdPallet.ProductStatusID.Value,//hold pallet
                                ProductSubStatusID = holdPallet.ProductSubStatusID.Value,
                                Quantity = itm.RejectQty,//out normal
                                ConversionQty = holdPallet.ConversionQty.Value,
                                PalletCode = holdPallet.PalletCode,
                                LocationCode = hold_location.Code,
                                DocumentCode = itm.ReclassifiedCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = itm.ReclassifiedDetailID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusOut,
                                Remark = "Inspection Dispatch"
                            }
                        };
                        stockService.UserID = UserID;
                        stockService.Outgoing2(stockOut, Context);

                        List<StockInOutModel> stockIn = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = receiving.ProductID,
                                StockUnitID = receiving.StockUnitID,
                                BaseUnitID = receiving.BaseUnitID,
                                Lot = rejectPallet.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.Value,
                                SupplierID = receiving.SupplierID.Value,
                                ManufacturingDate = rejectPallet.MFGDate.Value,
                                ExpirationDate = receiving.ExpirationDate.Value,
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = rejectPallet.ProductStatusID.Value,//hold pallet
                                ProductSubStatusID = rejectPallet.ProductSubStatusID.Value,
                                Quantity = itm.RejectQty,//in normal
                                ConversionQty = rejectPallet.ConversionQty.Value,
                                PalletCode = rejectPallet.PalletCode,
                                LocationCode = hold_location.Code,
                                DocumentCode = itm.ReclassifiedCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = itm.ReclassifiedDetailID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusIn,
                                Remark = "Inspection Dispatch"
                            }
                        };
                        stockService.Incomming2(stockIn, Context);


                        holdPallet.RemainQTY -= itm.RejectQty;
                        holdPallet.RemainBaseQTY -= holdPallet.ConversionQty.Value * itm.RejectQty;
                        holdPallet.ReserveQTY -= itm.RejectQty;
                        holdPallet.ReserveBaseQTY -= holdPallet.ConversionQty.Value * itm.RejectQty;

                        holdPallet.UserModified = UserID;
                        holdPallet.DateModified = DateTime.Now;
                        pcDetailService.Modify(holdPallet);

                        #endregion
                    }
                    else
                    {
                        reclass_detail.TotalReprocessQty += itm.ReprocessQty;
                        reclass_detail.TotalReprocessBaseQty += itm.ReprocessQty * reclass_detail.ConversionQty;
                    }


                    if (reclass_detail.ReclassifiedQty < (reclass_detail.TotalReprocessQty + reclass_detail.TotalRejectQty))
                    {
                        throw new HILIException("MSG00089");
                    }
                    #endregion

                    string palletCode = isReject ? reclass_detail.RejectPalletCode : reclass_detail.ReprocessPalletCode;

                    ProductionControlDetail pc = pcDetailService.Query().Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();
                    Receiving rcv = receivingService.Query().Filter(x => x.ReceiveDetailID == pc.ReceiveDetailID && x.PalletCode == palletCode).Get().SingleOrDefault();
                    Receive rv = receiveService.Query().Filter(x => x.ReceiveID == rcv.ReceiveID).Get().SingleOrDefault();

                    dispatchModel.SupplierId = rv.SupplierID;
                    dispatchModel.CustomerId = rv.SupplierID;


                    #region Insert Dispatch Details Customs

                    dispatchDetailCustoms.Add(new DispatchDetailCustom()
                    {
                        DispatchID = dispatchModel.DispatchId,
                        RuleID = shipto.RuleId,
                        DispatchDetailStatus = (int)DispatchDetailStatusEnum.InprogressConfirmQA,
                        DispatchDetailProductWidth = 0,
                        DispatchDetailProductLength = 0,
                        DispatchDetailProductHeight = 0,
                        DispatchPrice = 0,
                        IsBackOrder = false,

                        Sequence = seq,
                        ProductId = reclass.ProductID.Value,
                        StockUnitId = reclass_detail.ReclassifiedUnitID,
                        Quantity = isReject ? itm.RejectQty : itm.ReprocessQty,
                        BaseQuantity = (isReject ? itm.RejectQty : itm.ReprocessQty) * reclass_detail.ConversionQty,
                        BaseUnitId = reclass_detail.ReclassifiedBaseUnitID,
                        ConversionQty = reclass_detail.ConversionQty,
                        ProductOwnerId = rcv.ProductOwnerID.Value,
                        ProductStatusId = pc.ProductStatusID,
                        ProductSubStatusId = pc.ProductSubStatusID,

                        BookingStatus = (int)BookingStatusEnum.Complete,
                        LocationId = pc.LocationID.Value,
                        ProductLot = pc.LotNo,
                        Mfgdate = pc.MFGDate.Value,
                        ExpirationDate = rcv.ExpirationDate,

                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = dateNow,
                        UserModified = UserID,
                        DateModified = dateNow,
                        Remark = "Inspection Dispatch",
                        PalletCode = palletCode
                    });

                    #endregion

                    reclass_detail.UserModified = UserID;
                    reclass_detail.DateModified = DateTime.Now;
                    reclassifiedDetailService.Modify(reclass_detail);


                    #region Reserve Stock

                    StockSearch stockModel = new StockSearch()
                    {
                        Lot = pc.LotNo,
                        LocationID = pc.LocationID,
                        ManufacturingDate = pc.MFGDate.Value,
                        ExpirationDate = rcv.ExpirationDate.Value,
                        ProductID = reclass.ProductID.Value,
                        ConversionQty = reclass_detail.ConversionQty,
                        ProductOwnerID = rcv.ProductOwnerID.Value,
                        QTY = isReject ? itm.RejectQty : itm.ReprocessQty,
                        SupplierID = rv.SupplierID,
                        StockUnitID = reclass_detail.ReclassifiedUnitID,
                        BaseUnitID = reclass_detail.ReclassifiedBaseUnitID,
                        ProductStatusID = pc.ProductStatusID.Value,
                    };
                    stockListModel.Add(stockModel);

                    #endregion

                    seq++;
                }

                _DispatchDetailService.UserID = UserID;
                _DispatchDetailService.AddList(dispatchDetailCustoms);


                #region Stock Reserve 
                stockListModel.ForEach(stock =>
                {
                    Location location_bal = locationService.Query().Filter(x => x.LocationID == stock.LocationID).Include(x => x.Zone).Get().SingleOrDefault();

                    if (location_bal == null)
                    {
                        throw new HILIException("MSG00055");
                    }

                    StockInfo stockInfo = stockInfoService.Query().Filter(x => x.ProductID == stock.ProductID
                                                   && x.StockUnitID == stock.StockUnitID
                                                   && x.BaseUnitID == stock.BaseUnitID
                                                   && x.ConversionQty == stock.ConversionQty
                                                   && x.Lot == stock.Lot
                                                   && x.ProductStatusID == stock.ProductStatusID
                                                   && x.ManufacturingDate == stock.ManufacturingDate
                                                   && x.ExpirationDate == stock.ExpirationDate
                                                   //&& x.SupplierID == stock.SupplierID
                                                   //&& x.ProductOwnerID == stock.ProductOwnerID
                                                   ).Get().SingleOrDefault();

                    if (stockInfo == null)
                    {
                        throw new HILIException("MSG00037");
                    }

                    StockBalance bal_stock = stockBalanceService.Query().Filter(x => x.StockInfoID == stockInfo.StockInfoID).Get().SingleOrDefault();

                    StockBalance balStock = stockBalanceService.FindByID(bal_stock.StockBalanceID);

                    if (bal_stock == null)
                    {
                        throw new HILIException("MSG00038");
                    }

                    StockLocationBalance bal_stock_location = stockLocationService.Query().Filter(x => x.StockBalanceID == bal_stock.StockBalanceID
                                                  && x.ZoneID == location_bal.ZoneID
                                                  && x.WarehouseID == location_bal.Zone.WarehouseID).Get().SingleOrDefault();

                    StockLocationBalance balStockLocation = stockLocationService.FindByID(bal_stock_location.StockLocationID);

                    if (bal_stock_location == null)
                    {
                        throw new HILIException("MSG00042");
                    }

                    balStock.ReserveQuantity += stock.QTY;
                    balStock.UserModified = UserID;
                    balStock.DateModified = DateTime.Now;
                    stockBalanceService.Modify(balStock);

                    balStockLocation.ReserveQuantity += stock.QTY;
                    balStockLocation.UserModified = UserID;
                    balStockLocation.DateModified = DateTime.Now;
                    stockLocationService.Modify(balStockLocation);
                });
                #endregion

                #region Picking

                List<DispatchAllModel> _picking = (from _dispatch in dispatchService.Query().Filter(x => x.IsActive == true && x.DispatchId == dispatchModel.DispatchId).Get()
                                                   join disp_detail in dispatchDetailService.Query().Get() on _dispatch.DispatchId equals disp_detail.DispatchId
                                                   join booking in dispatchBookingService.Query().Get() on disp_detail.DispatchDetailId equals booking.DispatchDetailId
                                                   join _location in locationService.Query().Filter(x => x.IsActive == true).Get() on booking.LocationId equals _location.LocationID
                                                   join _zone in zoneService.Query().Filter(x => x.IsActive == true).Get() on _location.ZoneID equals _zone.ZoneID
                                                   join _warehouse in warehouseService.Query().Filter(x => x.IsActive == true).Get() on _zone.WarehouseID equals _warehouse.WarehouseID
                                                   select new
                                                   {
                                                       _dispatch,
                                                       disp_detail,
                                                       booking,
                                                       _location,
                                                       _zone,
                                                       _warehouse
                                                   }).GroupBy(g => new
                                                   {
                                                       ZoneID = g._location.ZoneID,
                                                       WarehouseID = g._warehouse.WarehouseID,
                                                       DispatchCode = g._dispatch.DispatchCode,
                                                       OrderNo = g._dispatch.OrderNo,
                                                       PoNo = g._dispatch.Pono,
                                                   }).Select(n => new DispatchAllModel
                                                   {
                                                       OrderNo = n.Key.OrderNo,
                                                       PoNo = n.Key.PoNo,
                                                       ZoneID = n.Key.ZoneID,
                                                       WarehouseID = n.Key.WarehouseID,
                                                       DispatchCode = n.Key.DispatchCode,
                                                       BookingBaseQty = n.Sum(x => x.booking.BookingBaseQty),
                                                       BookingQty = n.Sum(x => x.booking.BookingBaseQty),
                                                   }).ToList();


                _picking.ForEach(item =>
                {
                    #region [ PreFix ]

                    PickingPrefix pickingprefix = pickingPrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (pickingprefix == null)
                    {
                        throw new HILIException("PK10012");
                    }

                    PickingPrefix pickingPrefix = pickingPrefixService.FindByID(pickingprefix.PrefixID);

                    string PickingCode = Prefix.OnCreatePrefixed(pickingprefix.LastedKey, pickingprefix.PrefixKey, pickingprefix.FormatKey, pickingprefix.LengthKey);

                    pickingPrefix.IsLastest = false;

                    PickingPrefix newPrefix = new PickingPrefix()
                    {
                        IsLastest = true,
                        LastedKey = PickingCode,
                        PrefixKey = pickingPrefix.PrefixKey,
                        FormatKey = pickingPrefix.FormatKey,
                        LengthKey = pickingPrefix.LengthKey
                    };

                    pickingPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]

                    Picking _pickModel = new Picking
                    {
                        PickingID = Guid.NewGuid(),
                        PickingCode = PickingCode,
                        PickingStartDate = DateTime.Now,
                        PickingCompleteDate = null,
                        PickingEntryDate = DateTime.Now,
                        PickingStatus = PickingStatusEnum.Pick,
                        ShippingCode = "",
                        DispatchCode = item.DispatchCode,
                        DocumentNo = item.DocumentNo,
                        OrderNo = item.OrderNo,
                        PONo = item.PoNo,
                        EmployeeAssignID = UserID,
                        PickingCloseReason = "",
                        DateCreated = DateTime.Now,
                        UserCreated = UserID,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        IsActive = true
                    };

                    pickingService.Add(_pickModel);


                    List<DispatchAllModel> _pickingAssign = (from _dispatch in dispatchService.Query().Filter(x => x.IsActive == true && x.DispatchCode == _pickModel.DispatchCode).Get()
                                                             join disp_detail in dispatchDetailService.Query().Get() on _dispatch.DispatchId equals disp_detail.DispatchId
                                                             join booking in dispatchBookingService.Query().Get() on disp_detail.DispatchDetailId equals booking.DispatchDetailId
                                                             join _location in locationService.Query().Filter(x => x.IsActive == true).Get() on booking.LocationId equals _location.LocationID
                                                             join _zone in zoneService.Query().Filter(x => x.IsActive == true).Get() on item.ZoneID equals _zone.ZoneID
                                                             join _warehouse in warehouseService.Query().Filter(x => x.IsActive == true).Get() on item.WarehouseID equals _warehouse.WarehouseID
                                                             // where _warehouse.WarehouseID == item.WarehouseID && _zone.WarehouseID == item.ZoneID
                                                             select new
                                                             {
                                                                 _dispatch,
                                                                 disp_detail,
                                                                 booking,
                                                                 _location,
                                                                 _zone,
                                                                 _warehouse
                                                             }).GroupBy(g => new
                                                             {
                                                                 ShippingDetailID = Guid.Empty,
                                                                 PickingID = _pickModel.PickingID,
                                                                 ProductID = g.booking.ProductId,
                                                                 ConfirmQuantity = g.booking.BookingQty,
                                                                 ConfirmBasicQuantity = g.booking.BookingBaseQty,
                                                                 ConfirmBasicUnitID = g.booking.BookingBaseUnitId,
                                                                 ConfirmUnitID = g.disp_detail.StockUnitId,
                                                                 BookingId = g.booking.BookingId,
                                                                 LocationId = g.booking.LocationId,
                                                                 ProductLot = g.booking.ProductLot,
                                                                 PalletCode = g.booking.PalletCode,
                                                                 RemainQTY = g.booking.BookingQty,
                                                                 RemainStockUnitID = g.disp_detail.StockUnitId,
                                                                 RemainBaseQTY = g.booking.BookingBaseQty,
                                                                 RemainBaseUnitID = g.booking.BookingBaseUnitId,
                                                             }).Select(n => new DispatchAllModel
                                                             {
                                                                 ShippingDetailID = n.Key.ShippingDetailID,
                                                                 PickingID = n.Key.PickingID,
                                                                 ProductID = n.Key.ProductID,
                                                                 ConfirmQuantity = n.Key.ConfirmQuantity,
                                                                 ConfirmBasicQuantity = n.Key.ConfirmBasicQuantity,
                                                                 ConfirmBasicUnitID = n.Key.ConfirmBasicUnitID,
                                                                 ConfirmUnitID = n.Key.ConfirmUnitID,
                                                                 BookingId = n.Key.BookingId,
                                                                 LocationId = n.Key.LocationId,
                                                                 ProductLot = n.Key.ProductLot,
                                                                 PalletCode = n.Key.PalletCode,
                                                                 RemainQTY = n.Key.RemainQTY,
                                                                 RemainStockUnitID = n.Key.RemainStockUnitID,
                                                                 RemainBaseQTY = n.Key.RemainBaseQTY,
                                                                 RemainBaseUnitID = n.Key.RemainBaseUnitID,
                                                             }).ToList();
                    seq = 0;

                    _pickingAssign.ForEach(itm =>
                    {
                        PickingAssign _detail = new PickingAssign
                        {
                            AssignID = Guid.NewGuid(),
                            OrderPick = seq,
                            ShippingDetailID = itm.ShippingDetailID,
                            BookingID = itm.BookingId,
                            PickingID = itm.PickingID,
                            ProductID = itm.ProductID,
                            BaseQuantity = itm.ConfirmBasicQuantity,
                            BaseUnitID = itm.ConfirmBasicUnitID,
                            Barcode = itm.PalletCode,
                            StockUnitID = itm.ConfirmUnitID,
                            StockQuantity = itm.ConfirmQuantity,
                            SuggestionLocationID = itm.LocationId,
                            RefLocationID = itm.LocationId,
                            PalletCode = itm.PalletCode,
                            RefPalletCode = itm.PalletCode,
                            PalletQty = itm.RemainQTY,
                            PalletUnitID = itm.ConfirmBasicUnitID,
                            PickingLot = itm.ProductLot,
                            AssignStatus = PickingStatusEnum.Pick,
                            DateCreated = DateTime.Now,
                            UserCreated = UserID,
                            DateModified = DateTime.Now,
                            UserModified = UserID,
                            IsActive = true

                        };

                        pickingAssignService.Add(_detail);

                        seq++;
                    });
                });

                #endregion

                scope.Complete();
            }

        }
        #endregion
    }
}
