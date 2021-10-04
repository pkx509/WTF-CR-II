using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.PutAwayModel;
using DITS.HILI.WMS.PutAwayService;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.TransferWarehouseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.TransferWarehouseService
{
    public class TransferWarehouseService : Repository<TransferWarehouse>, ITransferWarehouseService
    {
        #region [ Property ]
        private readonly IUnitOfWork unitofwork;
        private readonly IRepository<TransferWarehouseDetail> transferWarehouseDetail;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<Zone> zoneService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<Line> lineService;
        private readonly IRepository<Truck> truckService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<Receive> receiveService;
        private readonly IStockService stockService;
        private readonly IPutAwayService putawayService;
        #endregion

        public TransferWarehouseService(IUnitOfWork context,
                                IRepository<TransferWarehouseDetail> _transferWarehouseDetail,
                                IRepository<Product> _product,
                                IRepository<ProductOwner> _productOwner,
                                IRepository<Location> _location,
                                IRepository<Zone> _zone,
                                IRepository<ProductUnit> _productUnit,
                                IRepository<Receiving> _receiving,
                                IRepository<Line> _line,
                                IRepository<Truck> _truck,
                                IRepository<Warehouse> _warehouse,
                                IRepository<Receive> _receive,
                                IRepository<ProductionControl> _productionControlService,
                                IStockService _stockService,
                                IPutAwayService _putawayService)
            : base(context)
        {
            unitofwork = context;
            transferWarehouseDetail = _transferWarehouseDetail;
            productService = _product;
            locationService = _location;
            productUnitService = _productUnit;
            receivingService = _receiving;
            receiveService = _receive;
            productOwnerService = _productOwner;
            lineService = _line;
            stockService = _stockService;
            pcDetailService = context.Repository<ProductionControlDetail>();
            productionControlService = _productionControlService;
            warehouseService = _warehouse;
            putawayService = _putawayService;
            truckService = _truck;
            zoneService = _zone;  
        }

        public PalletTagModel GetPalletCodeOld(string palletCode, List<ReceivingStatusEnum> status)
        {
            try
            {
                var transCount = transferWarehouseDetail.Query().Filter(x => x.PalletCode == palletCode && x.IsActive == true && (x.TransferWarehouse.TransferWarehouseStatus == TransferWarehouseEnum.New || x.TransferWarehouse.TransferWarehouseStatus == TransferWarehouseEnum.Inprogress)).Include(x => x.TransferWarehouse).Get().Count();

                var detail = (from receiving in receivingService.Query().Get()
                              join pc in pcDetailService.Query().Get() on receiving.PalletCode equals pc.PalletCode
                              join pc_head in productionControlService.Query().Get() on pc.ControlID equals pc_head.ControlID
                              join line in lineService.Query().Get() on pc_head.LineID equals line.LineID
                              join product in productService.Query().Include(x => x.CodeCollection).Get() on receiving.ProductID equals product.ProductID
                              join priceunit in productUnitService.Query().Get() on receiving.StockUnitID equals priceunit.ProductUnitID
                              join l in locationService.Query().Get() on receiving.LocationID equals l.LocationID into g
                              from lo in g.DefaultIfEmpty()
                              join s in locationService.Query().Get() on pc.SugguestLocationID equals s.LocationID into q
                              from su in q.DefaultIfEmpty()
                              where receiving.PalletCode == palletCode && status.Contains(receiving.ReceivingStatus) && (pc.ReserveQTY == 0 || pc.ReserveQTY == null)
                              && (pc.RemainQTY - pc.ReserveQTY) > 0
                              select new { receiving, product, priceunit, pc, lo, line, su })
                               .Select(n => new PalletTagModel
                               {
                                   ConfirmQty = n.pc.RemainQTY,
                                   Location = "",
                                   PalletCode = n.receiving.PalletCode,
                                   ProductName = n.product.Name,
                                   Qty = n.pc.RemainQTY,
                                   ReceiveDetailId = n.receiving.ReceiveDetailID,
                                   ReceivingID = n.receiving.ReceivingID,
                                   UnitName = n.priceunit.Name,
                                   PackingStatus = (int)n.pc.PackingStatus,
                                   ReceivingQty = n.pc.RemainQTY,
                                   WarehouseID = n.line.WarehouseID,
                                   LotNo = n.pc.LotNo,
                                   SuggestLocation = n.su == null ? "" : n.su.Code,
                                   IsTransfer = transCount > 0

                               }).FirstOrDefault();

                if (detail == null)
                {
                    throw new HILIException("MSG00088");
                }
                if (detail != null && string.IsNullOrEmpty(detail.SuggestLocation))
                {
                    var param = new SqlParameter("@Pallet_Code", SqlDbType.NVarChar) { Value = palletCode };
                    var location = unitofwork.SQLQuery<string>("exec SP_SuggestLocation @Pallet_Code", param).SingleOrDefault();
                    detail.SuggestLocation = location;

                }
                return detail;
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

        public Guid PutToTruckOld(string palletCode, Guid ticketCode, string location)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    var transCount = transferWarehouseDetail.Query().Filter(x => x.PalletCode == palletCode && x.IsActive == true).Get().Count();

                    if (transCount > 0)
                        throw new HILIException("MSG00072");

                    if (ticketCode != Guid.Empty)
                    {
                        var transHD = Query().Filter(x => x.TranID == ticketCode).Get().FirstOrDefault();

                        if (transHD.CloseDTTrans != null || transHD.IsActive == false)
                            throw new HILIException("MSG00094");
                    }

                    transCount = transferWarehouseDetail.Query().Filter(x => x.TranID == ticketCode && x.IsActive == true).Get().Count();

                    if (transCount == 0 && ticketCode != Guid.Empty)
                        throw new HILIException("MSG00094");

                    var receiving = receivingService.Query().Filter(x => x.PalletCode == palletCode).Get().FirstOrDefault();

                    var packing_detail = pcDetailService.Query().Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();
                    if (packing_detail == null)
                        throw new HILIException("MSG00072");


                    if (packing_detail.SugguestLocationID == null)
                    {
                        var lo = locationService.Query().Filter(x => x.Code == location).Get().FirstOrDefault();
                        packing_detail.SugguestLocationID = lo.LocationID;
                    }

                    var toWarehouse = locationService.Query().Filter(x => x.LocationID == packing_detail.SugguestLocationID).Include(x => x.Zone).Get().SingleOrDefault();
                    var fromWarehouse = pcDetailService.Query().Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();

                    if (ticketCode == Guid.Empty)
                    {
                        var trans = new TransferWarehouse
                        {
                            TranID = Guid.NewGuid(),
                            FromWarehouseID = fromWarehouse.WarehouseID.Value,
                            ToWarehouseID = toWarehouse.Zone.WarehouseID,
                            TransferWarehouseStatus = TransferWarehouseEnum.New,
                            StartDTTrans = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true
                        };
                        ticketCode = trans.TranID;

                        this.Add(trans);
                    }

                    var trans_detail = new TransferWarehouseDetail
                    {
                        TranDetailID = Guid.NewGuid(),
                        TranID = ticketCode,
                        ProductID = receiving.ProductID,
                        PalletCode = receiving.PalletCode,
                        FromLocationID = fromWarehouse.LocationID.Value,
                        ToLocationID = toWarehouse.LocationID,
                        StockUnitID = receiving.StockUnitID,
                        BaseUnitID = receiving.BaseUnitID,
                        ConversionQty = receiving.ConversionQty,
                        IsActive = true,
                        ProductOwnerID = receiving.ProductOwnerID,
                        SupplierID = receiving.SupplierID,
                        ProductStatusID = receiving.ProductStatusID,
                        ProductSubStatusID = receiving.ProductSubStatusID,
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        UserCreated = UserID,
                        StartDT = DateTime.Now,
                        StockQuantity = packing_detail.RemainQTY,
                        BaseQuantity = packing_detail.RemainQTY * packing_detail.ConversionQty,
                    };

                    transferWarehouseDetail.Add(trans_detail);

                    packing_detail.PackingStatus = PackingStatusEnum.Transfer;
                    packing_detail.UserModified = UserID;
                    packing_detail.DateModified = DateTime.Now;


                    pcDetailService.Modify(packing_detail);


                    scope.Complete();

                }
                return ticketCode;
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

        public PalletTagModel GetPalletCode(string palletCode, List<ReceivingStatusEnum> status)
        {
            try
            {
                bool IsTransfer = transferWarehouseDetail.Any(x => x.PalletCode == palletCode && x.IsActive == true
                                                                && (x.TransferWarehouse.TransferWarehouseStatus == TransferWarehouseEnum.New
                                                                || x.TransferWarehouse.TransferWarehouseStatus == TransferWarehouseEnum.Inprogress));

                var result = (from receiving in receivingService.Where(e => e.IsActive)
                              join pc in pcDetailService.Where(e => e.IsActive) on receiving.PalletCode equals pc.PalletCode
                              join pc_head in productionControlService.Where(e => e.IsActive) on pc.ControlID equals pc_head.ControlID
                              join line in lineService.Where(e => e.IsActive) on pc_head.LineID equals line.LineID
                              join product in productService.Where(e => e.IsActive) on receiving.ProductID equals product.ProductID
                              join priceunit in productUnitService.Where(e => e.IsActive) on receiving.StockUnitID equals priceunit.ProductUnitID
                              join l in locationService.Where(e => e.IsActive) on receiving.LocationID equals l.LocationID into g
                              from lo in g.DefaultIfEmpty()
                              join s in locationService.Where(e => e.IsActive) on pc.SugguestLocationID equals s.LocationID into q
                              from su in q.DefaultIfEmpty()
                              where receiving.PalletCode == palletCode
                              && status.Contains(receiving.ReceivingStatus)
                              && (!pc.ReserveQTY.HasValue || pc.ReserveQTY.Value == 0)
                              && ((pc.RemainQTY.HasValue ? pc.RemainQTY.Value : 0) - (pc.ReserveQTY.HasValue ? pc.ReserveQTY.Value : 0)) > 0
                              select new { receiving, product, priceunit, pc, lo, line, su }).ToList();

                PalletTagModel detail = result.Select(n => new PalletTagModel
                {
                    ConfirmQty = n.pc.RemainQTY.HasValue ? n.pc.RemainQTY.Value : 0,
                    Location = "",
                    PalletCode = n.receiving.PalletCode,
                    ProductName = n.product.Name,
                    Qty = n.pc.RemainQTY,
                    ReceiveDetailId = n.receiving.ReceiveDetailID,
                    ReceivingID = n.receiving.ReceivingID,
                    UnitName = n.priceunit.Name,
                    PackingStatus = (int)n.pc.PackingStatus,
                    ReceivingQty = n.pc.RemainQTY.HasValue ? n.pc.RemainQTY.Value : 0,
                    WarehouseID = n.line.WarehouseID,
                    LotNo = n.pc.LotNo,
                    SuggestLocation = n.su == null ? "" : n.su.Code,
                    IsTransfer = IsTransfer
                }).FirstOrDefault();

                if (detail == null)
                {
                    throw new HILIException("MSG00088");
                }
                if (string.IsNullOrEmpty(detail.SuggestLocation))
                {
                    SqlParameter param = new SqlParameter("@Pallet_Code", SqlDbType.NVarChar) { Value = palletCode };
                    string location = unitofwork.SQLQuery<string>("exec SP_SuggestLocation @Pallet_Code", param).FirstOrDefault();
                    detail.SuggestLocation = location;
                }
                return detail;
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
        public Guid PutToTruck(string palletCode, Guid ticketCode, string location)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                { 
                    if (transferWarehouseDetail.Any(x => x.PalletCode == palletCode && x.IsActive == true))
                    {
                        throw new HILIException("MSG00072");
                    }

                    if (ticketCode != Guid.Empty)
                    {
                        TransferWarehouse transHD = FirstOrDefault(x => x.TranID == ticketCode);

                        if (transHD.CloseDTTrans != null || transHD.IsActive == false)
                        {
                            throw new HILIException("MSG00094");
                        }
                    } 

                    if ((!transferWarehouseDetail.Any(x => x.TranID == ticketCode && x.IsActive == true)) && ticketCode != Guid.Empty)
                    {
                        throw new HILIException("MSG00094");
                    }

                    Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == palletCode);

                    ProductionControlDetail packing_detail = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                    if (packing_detail == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    if (packing_detail.SugguestLocationID == null)
                    {
                        Location lo = locationService.FirstOrDefault(x => x.Code == location);
                        packing_detail.SugguestLocationID = lo.LocationID;
                    }

                    Location toWarehouse = locationService.FirstOrDefault(x => x.LocationID == packing_detail.SugguestLocationID);
                    var toZone = zoneService.FindByID(toWarehouse.ZoneID);
                    ProductionControlDetail fromWarehouse = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode); 

                    if (ticketCode == Guid.Empty)
                    {
                        TransferWarehouse trans = new TransferWarehouse
                        {
                            TranID = Guid.NewGuid(),
                            FromWarehouseID = fromWarehouse.WarehouseID.Value,
                            ToWarehouseID = toZone.WarehouseID,
                            TransferWarehouseStatus = TransferWarehouseEnum.New,
                            StartDTTrans = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true
                        };
                        ticketCode = trans.TranID;

                        Add(trans);
                    }

                    TransferWarehouseDetail trans_detail = new TransferWarehouseDetail
                    {
                        TranDetailID = Guid.NewGuid(),
                        TranID = ticketCode,
                        ProductID = receiving.ProductID,
                        PalletCode = receiving.PalletCode,
                        FromLocationID = fromWarehouse.LocationID.Value,
                        ToLocationID = toWarehouse.LocationID,
                        StockUnitID = receiving.StockUnitID,
                        BaseUnitID = receiving.BaseUnitID,
                        ConversionQty = receiving.ConversionQty,
                        IsActive = true,
                        ProductOwnerID = receiving.ProductOwnerID,
                        SupplierID = receiving.SupplierID,
                        ProductStatusID = receiving.ProductStatusID,
                        ProductSubStatusID = receiving.ProductSubStatusID,
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        UserCreated = UserID,
                        StartDT = DateTime.Now,
                        StockQuantity = packing_detail.RemainQTY,
                        BaseQuantity = packing_detail.RemainQTY * packing_detail.ConversionQty,
                    };

                    transferWarehouseDetail.Add(trans_detail);

                    packing_detail.PackingStatus = PackingStatusEnum.Transfer;
                    packing_detail.UserModified = UserID;
                    packing_detail.DateModified = DateTime.Now;
                    pcDetailService.Modify(packing_detail);
                    scope.Complete();

                }
                return ticketCode;
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

        public List<JobTransferWarehouse> CheckExistJobTransfer()
        {
            try
            {
                IEnumerable<JobTransferWarehouse> transCount = (from trans in Where(x => (x.TransferWarehouseStatus == TransferWarehouseEnum.New) && x.IsActive)
                                                                join whFrom in warehouseService.Where(e=>e.IsActive) on trans.FromWarehouseID equals whFrom.WarehouseID
                                                                join whTo in warehouseService.Where(e => e.IsActive) on trans.ToWarehouseID equals whTo.WarehouseID
                                                                select new { trans, whFrom, whTo })
                                  .Select(n => new JobTransferWarehouse
                                  {
                                      TicketCode = n.trans.TranID,
                                      FromWarehouseID = n.trans.FromWarehouseID,
                                      ToWarehouseID = n.trans.ToWarehouseID,
                                      FromWarehouse = n.whFrom.Name,
                                      ToWarehouse = n.whTo.Name,
                                      TransferStatus = n.trans.TransferWarehouseStatus.ToString(),
                                      StartDTTrans = n.trans.StartDTTrans.Value.ToString("dd/MM/yyyy")
                                  });

                return transCount.ToList();
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

        public bool CloseJobTransfer(Guid ticketCode, string truckNo, Guid warehouseId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TransferWarehouse trans = FirstOrDefault(x => x.TranID == ticketCode);
                    Truck truck = truckService.FirstOrDefault(x => x.TruckNo == truckNo);

                    if (truck == null)
                    {
                        throw new HILIException("MSG00083");
                    }

                    if (trans.TransferWarehouseStatus != TransferWarehouseEnum.New)
                    {
                        throw new HILIException("MSG00086");
                    }

                    List<PutAwayItem> putawayItem = new List<PutAwayItem>();

                    var tmp = (from ts in Where(e => e.IsActive)
                               join transfer in transferWarehouseDetail.Where(e => e.IsActive) on ts.TranID equals transfer.TranID
                               join rcv in receivingService.Where(e => e.IsActive) on transfer.PalletCode equals rcv.PalletCode
                               join rc in receiveService.Where(e => e.IsActive) on rcv.ReceiveID equals rc.ReceiveID
                               join pc in pcDetailService.Where(e => e.IsActive) on transfer.PalletCode equals pc.PalletCode
                               join pc_head in productionControlService.Where(e => e.IsActive) on pc.ControlID equals pc_head.ControlID
                               join line in lineService.Where(e => e.IsActive) on pc_head.LineID equals line.LineID
                               join product in productService.Where(e => e.IsActive) on transfer.ProductID equals product.ProductID
                               join priceunit in productUnitService.Where(e => e.IsActive) on transfer.StockUnitID equals priceunit.ProductUnitID
                               where transfer.TranID == ticketCode && transfer.IsActive == true && ts.IsActive == true && ts.TransferWarehouseStatus == TransferWarehouseEnum.New
                               select new { transfer, product, priceunit, pc, line, rcv, rc, ts }).ToList();
                    List<PutAwayItem> detail = (from n in tmp
                                                select new PutAwayItem
                                                {
                                                    PutAwayItemID = Guid.NewGuid(),
                                                    RemainQuantity = 0,
                                                    ProductID = n.transfer.ProductID.HasValue ? n.transfer.ProductID.Value : Guid.Empty,
                                                    Lot = n.pc.LotNo,
                                                    ManufacturingDate = n.rcv.ManufacturingDate,
                                                    ExpirationDate = n.rcv.ExpirationDate,
                                                    Quantity = n.transfer.StockQuantity.HasValue ? n.transfer.StockQuantity.Value : 0,
                                                    BaseQuantity = n.transfer.BaseQuantity.HasValue ? n.transfer.BaseQuantity.Value : 0,
                                                    ConversionQty = n.transfer.ConversionQty.HasValue ? n.transfer.ConversionQty.Value : 0,
                                                    StockUnitID = n.transfer.StockUnitID.HasValue ? n.transfer.StockUnitID.Value : Guid.Empty,
                                                    BaseUnitID = n.transfer.BaseUnitID.HasValue ? n.transfer.BaseUnitID.Value : Guid.Empty,
                                                    ProductStatusID = n.transfer.ProductStatusID.HasValue ? n.transfer.ProductStatusID.Value : Guid.Empty,
                                                    ProductSubStatusID = n.transfer.ProductSubStatusID.HasValue ? n.transfer.ProductSubStatusID.Value : Guid.Empty,
                                                    PackageWeight = 1,
                                                    ProductWeight = 1,
                                                    ProductWidth = 1,
                                                    ProductLength = 1,
                                                    ProductHeight = 1,
                                                    PalletCode = n.transfer.PalletCode,
                                                    Remark = "",
                                                    IsActive = true,
                                                    UserCreated = UserID,
                                                    DateCreated = DateTime.Now,
                                                    UserModified = UserID,
                                                    DateModified = DateTime.Now,
                                                    FromLocationID = n.transfer.FromLocationID,
                                                    SuggestionLocationID = n.transfer.ToLocationID,
                                                    Price = 0,
                                                    IsComplete = false,
                                                    DocumentTypeID = n.rc.ReceiveTypeID,
                                                    ReferenceBaseID = n.transfer.TranDetailID,
                                                    LineID = n.line.LineID,
                                                    SupplierID = n.transfer.SupplierID,
                                                    ProductOwnerID = n.transfer.ProductOwnerID.HasValue ? n.transfer.ProductOwnerID.Value : Guid.Empty,
                                                    ReferenceCode = truck.TruckNo
                                                }).ToList();
                    putawayService.UserID = UserID;
                    putawayService.CreateJobPutAway(ticketCode, detail);

                    trans.ToWarehouseID = warehouseId;
                    trans.TruckID = truck.TruckID;
                    trans.DateModified = DateTime.Now;
                    trans.UserModified = UserID;
                    trans.TransferWarehouseStatus = TransferWarehouseEnum.Inprogress;
                    trans.CloseDTTrans = DateTime.Now;
                    Modify(trans);
                    scope.Complete();
                }
                return true;
            }
            catch (HILIException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
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

        public bool DeleteTransferDetail(Guid transferDetaiId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TransferWarehouseDetail transDetail = transferWarehouseDetail.Query().Filter(x => x.TranDetailID == transferDetaiId).Get().SingleOrDefault();

                    transDetail.IsActive = false;
                    transDetail.DateModified = DateTime.Now;
                    transDetail.UserModified = UserID;

                    transferWarehouseDetail.Modify(transDetail);

                    ProductionControlDetail packing_detail = pcDetailService.Query().Filter(x => x.PalletCode == transDetail.PalletCode).Get().SingleOrDefault();
                    if (packing_detail == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    if (packing_detail.PackingStatus != PackingStatusEnum.PutAway)
                    {

                        int countPallet = putawayService.Query().Filter(x => x.PalletCode == transDetail.PalletCode && x.PutAwayStatus == PutAwayStatusEnum.PutAway).Get().Count();
                        if (countPallet == 0)
                        {
                            packing_detail.PackingStatus = PackingStatusEnum.Loading_In;
                        }
                        else
                        {
                            packing_detail.PackingStatus = PackingStatusEnum.PutAway;
                        }
                    }
                    packing_detail.UserModified = UserID;
                    packing_detail.DateModified = DateTime.Now;
                    pcDetailService.Modify(packing_detail);


                    int transCount = transferWarehouseDetail.Query().Filter(x => x.TranID == transDetail.TranID && x.IsActive == true).Get().Count();

                    if (transCount == 0)
                    {
                        TransferWarehouse trans = Query().Filter(x => x.TranID == transDetail.TranID).Get().SingleOrDefault();

                        trans.IsActive = false;
                        trans.DateModified = DateTime.Now;
                        trans.UserModified = UserID;
                        Modify(trans);
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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<PalletTagModel> GetTransferWarehouseDetail(Guid ticketCode)
        {
            try
            {

               var results=  (from transfer in transferWarehouseDetail.Where(x => x.IsActive == true)
                                               join pc in pcDetailService.Query().Filter(x => x.IsActive == true).Get() on transfer.PalletCode equals pc.PalletCode
                                               join pc_head in productionControlService.Query().Get() on pc.ControlID equals pc_head.ControlID
                                               join line in lineService.Query().Get() on pc_head.LineID equals line.LineID
                                               join product in productService.Query().Include(x => x.CodeCollection).Get() on transfer.ProductID equals product.ProductID
                                               join priceunit in productUnitService.Query().Get() on transfer.StockUnitID equals priceunit.ProductUnitID
                                               join s in locationService.Query().Get() on pc.SugguestLocationID equals s.LocationID into q
                                               from su in q.DefaultIfEmpty()
                                               join zo in zoneService.Query().Filter(x => x.IsActive == true).Get() on su.ZoneID equals zo.ZoneID into gz
                                               from zone in gz.DefaultIfEmpty()
                                               where transfer.TranID == ticketCode
                                               select new { transfer, product, priceunit, pc, line, su, zone }).ToList();
                List<PalletTagModel> detail = results.Select(n => new PalletTagModel
                               {
                                   ConfirmQty = n.transfer.StockQuantity,
                                   Location = "",
                                   PalletCode = n.transfer.PalletCode,
                                   ProductName = n.product.Name,
                                   Qty = n.transfer.StockQuantity, 
                                   UnitName = n.priceunit.Name,
                                   TransDetailID = n.transfer.TranDetailID,
                                   PackingStatus = (int)n.pc.PackingStatus,
                                   ReceivingQty = n.pc.RemainQTY,
                                   WarehouseID = n.zone == null ? Guid.Empty : n.zone.WarehouseID,
                                   LotNo = n.pc.LotNo,
                                   SuggestLocation = "",
                                   IsTransfer = true

                               }).ToList();

                return detail;
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
