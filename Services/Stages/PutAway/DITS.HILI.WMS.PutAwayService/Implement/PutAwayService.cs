using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.PackagesModel;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.PutAwayModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.TransferWarehouseModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.PutAwayService
{
    public class PutAwayService : Repository<PutAway>, IPutAwayService
    {
        #region [ Property ]
        private readonly IRepository<PutAwayItem> itemService;
        private readonly IRepository<PutAwayConfirm> putawayConfirmService;
        private readonly IRepository<PutAwayDetail> putAwayDetailService;
        private readonly IRepository<PutAwayPrefix> prefixService;
        private readonly IRepository<PutAwayMap> putawayMapService;
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductStatus> productStatusService;
        private readonly IRepository<ProductSubStatus> productSubStatusService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<Package> packageService;
        private readonly IRepository<DocumentType> documentTypeService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IStockService stockService;
        private readonly IRepository<TransferWarehouseDetail> transferWarehouseDetail;
        private readonly IRepository<TransferWarehouse> transferWarehouseService;

        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<ProductionControl> pcService;
        private readonly IRepository<ReceiveDetail> receiveDetailService;
        private readonly IRepository<Receive> receiveService;
        private readonly IRepository<Zone> zoneService;
        private readonly IRepository<Warehouse> whereHouseService;

        #endregion

        #region [ Contructor ]

        public PutAwayService(IUnitOfWork context,
                              IRepository<PutAwayItem> _itemService,
                              IRepository<PutAwayConfirm> _putawayConfirmService,
                              IRepository<PutAwayDetail> _putAwayDetailService,
                              IRepository<Product> _productService,
                              IRepository<ProductUnit> _productUnitService,
                              IRepository<ProductStatus> _productStatusService,
                              IRepository<ProductSubStatus> _productSubStatusService,
                              IRepository<Location> _locationService,
                              IRepository<Package> _packageService,
                              IRepository<DocumentType> _documentTypeService,
                              IRepository<PutAwayPrefix> _prefix,
                              IRepository<PutAwayMap> _putawayMapService,
                              IRepository<ProductionControlDetail> _pcDetailService,
                              IRepository<ProductionControl> _pcService,
                              IRepository<Receiving> _receiving,
                              IRepository<Zone> _zoneService,
                              IRepository<Warehouse> _whereHouseService,
                              //IRepository<TransferWarehouseDetail> _transferWarehouseDetail,
                              //IRepository<TransferWarehouse> _transferWarehouse,
                              IStockService _stockService)
            : base(context)
        {
            itemService = _itemService;
            productService = _productService;
            productUnitService = _productUnitService;
            productStatusService = _productStatusService;
            productSubStatusService = _productSubStatusService;
            locationService = _locationService;
            packageService = _packageService;
            putawayConfirmService = _putawayConfirmService;
            putAwayDetailService = _putAwayDetailService;
            documentTypeService = _documentTypeService;
            stockService = _stockService;
            prefixService = _prefix;
            putawayMapService = _putawayMapService;
            pcDetailService = _pcDetailService;
            receivingService = _receiving;
            transferWarehouseDetail = context.Repository<TransferWarehouseDetail>(); ;
            transferWarehouseService = context.Repository<TransferWarehouse>(); ;
            pcService = _pcService;

            receiveDetailService = context.Repository<ReceiveDetail>();
            receiveService = context.Repository<Receive>();
            zoneService = _zoneService;
            whereHouseService = _whereHouseService;
            base.GetProductInfo<PutAwayService>(System.Reflection.Assembly.GetExecutingAssembly());
        }

        #endregion


        public void CreateJobPutAway(Guid transID, List<PutAwayItem> putawayItem)
        {
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                putawayItem.ForEach(item =>
                {
                    item.PutAwayItemID = Guid.NewGuid();
                    itemService.Add(item);
                });
                var pwi = (from ts in transferWarehouseDetail.Where(x => x.TranID == transID)
                           join pw in itemService.Where() on ts.TranDetailID equals pw.ReferenceBaseID
                           select new { pw }).ToList();
                List<PutAwayItem> result = new List<PutAwayItem>();
                foreach (var pwy in pwi)
                {
                    var temps = (from putaway in itemService.Where(x => x.IsActive && !x.IsComplete && x.PutAwayItemID == pwy.pw.PutAwayItemID)
                                 join product in productService.Where(x => x.IsActive) on putaway.ProductID equals product.ProductID
                                 join location in locationService.Where(x => x.IsActive) on putaway.SuggestionLocationID equals location.LocationID
                                 join zone in zoneService.Where(x => x.IsActive) on location.ZoneID equals zone.ZoneID
                                 join whereHouse in whereHouseService.Where(x => x.IsActive) on zone.WarehouseID equals whereHouse.WarehouseID
                                 select new { putaway, product, location, zone, whereHouse }).ToList();
                    foreach (var pw in temps)
                    {
                        var item = new PutAwayItem()
                        {
                            PutAwayItemID = pw.putaway.PutAwayItemID,
                            ProductOwnerID = pw.putaway.ProductOwnerID,
                            SupplierID = pw.putaway.SupplierID,
                            FromLocationID = pw.putaway.FromLocationID,
                            SuggestionLocationID = pw.putaway.SuggestionLocationID,
                            ProductID = pw.putaway.ProductID,
                            Lot = pw.putaway.Lot,
                            PalletCode = pw.putaway.PalletCode,
                            ManufacturingDate = pw.putaway.ManufacturingDate,
                            ExpirationDate = pw.putaway.ExpirationDate,
                            ProductHeight = pw.putaway.ProductHeight,
                            ProductLength = pw.putaway.ProductLength,
                            ProductWidth = pw.putaway.ProductWidth,
                            ProductWeight = pw.putaway.ProductWeight,
                            PackageWeight = pw.putaway.PackageWeight,
                            ProductStatusID = pw.putaway.ProductStatusID,
                            ProductSubStatusID = pw.putaway.ProductSubStatusID,
                            ProductUnitPriceID = pw.putaway.ProductUnitPriceID,
                            Price = pw.putaway.Price,
                            StockUnitID = pw.putaway.StockUnitID,
                            BaseUnitID = pw.putaway.BaseUnitID,
                            Quantity = pw.putaway.Quantity,
                            BaseQuantity = pw.putaway.Quantity * pw.putaway.ConversionQty,
                            ConversionQty = pw.putaway.ConversionQty,
                            Location = pw.location,
                            DocumentTypeID = pw.putaway.DocumentTypeID
                        };
                        item.Location.Zone = pw.zone;
                        item.Location.Zone.Warehouse = pw.whereHouse;
                        result.Add(item);
                    }  
                }

                var jobs = result.GroupBy(a => new
                {
                    ZoneId = a.Location.ZoneID,
                    WhareHouse = a.Location.Zone.WarehouseID
                })
                .Select(g => new
                 {
                        WhareHouse = g.Key.WhareHouse,
                        ZoneId = g.Key.ZoneId,
                        Quantity = g.Sum(x => x.Quantity),
                }).ToList();

                PutAwayPrefix prefix = prefixService.FirstOrDefault();

                jobs.ForEach(item =>
                {
                    string code = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    prefix.LastedKey = code;
                    prefixService.Modify(prefix);
                    List<PutAwayItem> jobItems = result.Where(x => x.Location.ZoneID == item.ZoneId && x.Location.Zone.WarehouseID == item.WhareHouse).ToList();

                    foreach (PutAwayItem job in jobItems)
                    {
                        PutAway putAway = new PutAway
                        {
                            PutAwayID = Guid.NewGuid(),
                            PutAwayJobCode = code,
                            StartDate = DateTime.Now,
                            FromLocationID = job.FromLocationID,
                            SuggestionLocationID = job.SuggestionLocationID,
                            PutAwayStatus = PutAwayStatusEnum.Trans,
                            ProductID = job.ProductID,
                            Lot = job.Lot,
                            ManufacturingDate = job.ManufacturingDate,
                            ExpirationDate = job.ExpirationDate,
                            ProductStatusID = job.ProductStatusID,
                            ProductSubStatusID = job.ProductSubStatusID,
                            PackageWeight = job.PackageWeight,
                            ProductWeight = job.ProductWeight,
                            ProductWidth = job.ProductWidth,
                            ProductLength = job.ProductLength,
                            ProductHeight = job.ProductHeight,
                            PalletCode = job.PalletCode,
                            IsActive = true,
                            ProductOwnerID = job.ProductOwnerID,
                            PutAwayDate = DateTime.Now,
                            SupplierID = job.SupplierID,
                            Price = 1,
                            ZoneID = item.ZoneId,
                            WarehouseID = item.WhareHouse,
                            DocumentTypeID = job.DocumentTypeID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };
                        Add(putAway);

                        PutAwayDetail detail = new PutAwayDetail
                        {
                            BaseUnitID = job.BaseUnitID,
                            StockUnitID = job.StockUnitID,
                            ConversionQty = job.ConversionQty,
                            BaseQuantity = job.Quantity * job.ConversionQty,
                            Quantity = job.Quantity,
                            PutAwayID = putAway.PutAwayID,
                            PutAwayDetailID = Guid.NewGuid(),
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true
                        };

                        putAwayDetailService.Add(detail);
                        PutAwayMap map = new PutAwayMap
                        {
                            PutAwayItemID = job.PutAwayItemID,
                            PutAwayID = putAway.PutAwayID,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true,

                        };

                        putawayMapService.Add(map);
                    }
                });
                //    scope.Complete();
                //}

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
        public PalletTagModel GetPalletCode_Old(string palletCode)
        {
            try
            {

                var detail = (from putaway in Query().Get()
                              join putaway_detail in putAwayDetailService.Query().Get() on putaway.PutAwayID equals putaway_detail.PutAwayID
                              join product in productService.Query().Include(x => x.CodeCollection).Get() on putaway.ProductID equals product.ProductID
                              join priceunit in productUnitService.Query().Get() on putaway_detail.StockUnitID equals priceunit.ProductUnitID
                              join fromlocation in locationService.Query().Get() on putaway.FromLocationID equals fromlocation.LocationID
                              join tolocation in locationService.Query().Get() on putaway.SuggestionLocationID equals tolocation.LocationID
                              where putaway.PalletCode == palletCode && (putaway.PutAwayStatus == PutAwayStatusEnum.Trans || putaway.PutAwayStatus == PutAwayStatusEnum.ReceiveTrans) && putaway_detail.IsActive == true
                              && putaway.IsActive == true
                              select new { putaway, product, priceunit, putaway_detail, fromlocation, tolocation })
                               .Select(n => new PalletTagModel
                               {
                                   Location = n.tolocation.Code,
                                   PalletCode = n.putaway.PalletCode,
                                   ProductName = n.product.Name,
                                   Qty = n.putaway_detail.Quantity,
                                   UnitName = n.priceunit.Name,
                                   WarehouseID = n.putaway.WarehouseID,
                                   LotNo = n.putaway.Lot,
                                   SuggestLocation = n.tolocation.Code,
                                   IsPutAway = n.putaway_detail.ConfirmQty != 0,
                                   ConfirmQty = n.putaway_detail.ConfirmQty

                               }).FirstOrDefault();

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
        public PalletTagModel GetPalletCode(string palletCode)
        {
            try
            {

                var tmp = (from putaway in Where(e => e.IsActive)
                           join putaway_detail in putAwayDetailService.Where(e => e.IsActive) on putaway.PutAwayID equals putaway_detail.PutAwayID
                           join product in productService.Where(e => e.IsActive) on putaway.ProductID equals product.ProductID
                           join priceunit in productUnitService.Where(e => e.IsActive) on putaway_detail.StockUnitID equals priceunit.ProductUnitID
                           join fromlocation in locationService.Where(e => e.IsActive) on putaway.FromLocationID equals fromlocation.LocationID
                           join tolocation in locationService.Where(e => e.IsActive) on putaway.SuggestionLocationID equals tolocation.LocationID
                           where putaway.PalletCode == palletCode
                           && (putaway.PutAwayStatus == PutAwayStatusEnum.Trans || putaway.PutAwayStatus == PutAwayStatusEnum.ReceiveTrans)
                           && putaway_detail.IsActive
                           && putaway.IsActive
                           select new { putaway, product, priceunit, putaway_detail, fromlocation, tolocation }).ToList();

                PalletTagModel detail = (from n in tmp
                                         select new PalletTagModel
                                         {
                                             Location = n.tolocation.Code,
                                             PalletCode = n.putaway.PalletCode,
                                             ProductName = n.product.Name,
                                             Qty = n.putaway_detail.Quantity,
                                             UnitName = n.priceunit.Name,
                                             WarehouseID = n.putaway.WarehouseID,
                                             LotNo = n.putaway.Lot,
                                             SuggestLocation = n.tolocation.Code,
                                             IsPutAway = n.putaway_detail.ConfirmQty != 0,
                                             ConfirmQty = n.putaway_detail.ConfirmQty
                                         }).FirstOrDefault();
                if (detail == null)
                {

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


        public void ConfirmReceive(string palletCode, decimal qty)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductionControlDetail packing_detail = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                    if (packing_detail == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    var detail = (from putaway_detail in putAwayDetailService.Where(e => e.IsActive)
                                  join putaway in Where(e => e.IsActive)  on putaway_detail.PutAwayID equals putaway.PutAwayID
                                  join putaway_map in putawayMapService.Where(e => e.IsActive) on putaway.PutAwayID equals putaway_map.PutAwayID
                                  join putaway_item in itemService.Where(e => e.IsActive) on putaway_map.PutAwayItemID equals putaway_item.PutAwayItemID
                                  where putaway.PalletCode == palletCode 
                                  && putaway.PutAwayStatus == PutAwayStatusEnum.Trans 
                                  && putaway_detail.IsActive
                                  && putaway.IsActive
                                  select new { putaway, putaway_detail, putaway_item }).FirstOrDefault();
                    if (detail == null)
                    {
                        throw new HILIException("MSG00090");
                    }
                    PutAwayDetail putawayDetail = putAwayDetailService.FirstOrDefault(x => x.PutAwayDetailID == detail.putaway_detail.PutAwayDetailID);
                    putawayDetail.ConfirmQty = qty;
                    putawayDetail.BaseQuantity = qty * putawayDetail.ConversionQty;
                    putawayDetail.UserModified = UserID;
                    putawayDetail.DateModified = DateTime.Now;
                    putAwayDetailService.Modify(putawayDetail);

                    PutAway _putaway = FirstOrDefault(x => x.PalletCode == palletCode && x.PutAwayStatus == PutAwayStatusEnum.Trans && x.IsActive);
                    _putaway.PutAwayStatus = PutAwayStatusEnum.ReceiveTrans;
                    _putaway.UserModified = UserID;
                    _putaway.DateModified = DateTime.Now;

                    Modify(_putaway);

                    var tran_detail = (from t_detail in transferWarehouseDetail.Where(e => e.IsActive)
                                       join tran in transferWarehouseService.Where(e => e.IsActive) on t_detail.TranID  equals tran.TranID
                                       where t_detail.PalletCode == palletCode && tran.TransferWarehouseStatus == TransferWarehouseEnum.Inprogress
                                       && t_detail.IsActive
                                       select new { t_detail }).FirstOrDefault();
                    if (tran_detail == null)
                    {
                        throw new HILIException("MSG00090");
                    }
                    TransferWarehouseDetail transDetail = tran_detail.t_detail;


                    var detail_count = (from t_detail in transferWarehouseDetail.Where(e => e.IsActive)
                                        join tran in transferWarehouseService.Where(e => e.IsActive) on t_detail.TranID  equals tran.TranID
                                        where tran.TransferWarehouseStatus == TransferWarehouseEnum.Inprogress && t_detail.FinishDT == null
                                        && t_detail.IsActive
                                        select t_detail).Any();

                    if (!detail_count)
                    {
                        TransferWarehouse trans = transferWarehouseService.FirstOrDefault(x => x.TranID == transDetail.TranID);
                        trans.TransferWarehouseStatus = TransferWarehouseEnum.Complete;
                        trans.DateModified = DateTime.Now;
                        trans.UserModified = UserID;
                        transferWarehouseService.Modify(trans);
                    }

                    transDetail.FinishDT = DateTime.Now;
                    transDetail.UserModified = UserID;
                    transDetail.DateModified = DateTime.Now;
                    transferWarehouseDetail.Modify(transDetail);


                    Location form_location = locationService.FirstOrDefault(x => x.LocationID == detail.putaway.FromLocationID);
                    var form_zone = zoneService.FindByID(form_location.ZoneID);

                    packing_detail.PackingStatus = PackingStatusEnum.In_Progress;
                    packing_detail.UserModified = UserID;
                    packing_detail.DateModified = DateTime.Now;
                    pcDetailService.Modify(packing_detail);

                    List<StockInOutModel> stockOut = new List<StockInOutModel>
                    {
                        new StockInOutModel
                        {
                            DocumentID = detail.putaway_item.ReferenceBaseID,
                            PalletCode = detail.putaway.PalletCode,
                            DocumentCode = detail.putaway.PutAwayJobCode,
                            DocumentTypeID = detail.putaway.DocumentTypeID.GetValueOrDefault(),
                            ProductOwnerID = detail.putaway.ProductOwnerID,
                            SupplierID = detail.putaway.SupplierID.GetValueOrDefault(),
                            ProductID = detail.putaway.ProductID.GetValueOrDefault(),
                            Lot = detail.putaway.Lot,
                            ExpirationDate = detail.putaway.ExpirationDate.GetValueOrDefault(),
                            ManufacturingDate = detail.putaway.ManufacturingDate.GetValueOrDefault(),
                            ProductWidth = detail.putaway.ProductWidth,
                            ProductLength = detail.putaway.ProductLength,
                            ProductHeight = detail.putaway.ProductHeight,
                            ProductWeight = detail.putaway.ProductWeight,
                            PackageWeight = detail.putaway.PackageWeight,
                            Price = detail.putaway.Price,
                            StockUnitID = detail.putaway_detail.StockUnitID,
                            BaseUnitID = detail.putaway_detail.BaseUnitID,
                            ProductUnitPriceID = detail.putaway.ProductUnitPriceID,
                            ConversionQty = detail.putaway_detail.ConversionQty,
                            ProductStatusID = detail.putaway.ProductStatusID,
                            ProductSubStatusID = detail.putaway.ProductSubStatusID,
                            LocationCode = form_location.Code,
                            Quantity = qty
                        }
                    };
                    stockService.UserID = UserID;
                    stockService.Outgoing(stockOut);
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
        
        public void ConfirmKeep(string palletCode, decimal qty, string locationCode)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Receiving _reciving = receivingService.FirstOrDefault(x => x.PalletCode == palletCode);
                    Receiving reciving = receivingService.FindByID(_reciving.ReceivingID);
                    if (reciving == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    var detail = (from putaway in Where(e=>e.IsActive)
                                  join putaway_detail in putAwayDetailService.Where(e => e.IsActive) on putaway.PutAwayID equals putaway_detail.PutAwayID
                                  where putaway.PalletCode == palletCode 
                                  && putaway.PutAwayStatus == PutAwayStatusEnum.ReceiveTrans 
                                  && putaway_detail.IsActive 
                                  && putaway.IsActive
                                  select new { putaway, putaway_detail }).FirstOrDefault();


                    if (detail == null)
                    {
                        throw new HILIException("MSG00090");
                    }

                    Location confirm_location = locationService.FirstOrDefault(x => x.Code == locationCode);
                    var confrim_zone = zoneService.FindByID(confirm_location.ZoneID);

                    if (confirm_location == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    if (confirm_location.LocationReserveQty >= confirm_location.PalletCapacity)
                    {
                        throw new HILIException("MSG00078");
                    }


                    confirm_location.LocationReserveQty = (confirm_location.LocationReserveQty == null ? 0 : confirm_location.LocationReserveQty.GetValueOrDefault()) + 1;
                    locationService.Modify(confirm_location);

                    Location _form_location = locationService.FirstOrDefault(x => x.LocationID == detail.putaway.FromLocationID);
                    var _from_zone = zoneService.FindByID(_form_location.ZoneID);

                    if (_form_location == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    Location form_location = locationService.FindByID(_form_location.LocationID);
                    form_location.LocationReserveQty = (form_location.LocationReserveQty == null ? 0 : form_location.LocationReserveQty.GetValueOrDefault()) - 1;
                    if (form_location.LocationReserveQty < 0)
                    {
                        form_location.LocationReserveQty = 0;
                    }

                    locationService.Modify(form_location); 


                    PutAwayConfirm confirm = new PutAwayConfirm
                    {
                        PutAwayConfirmID = Guid.NewGuid(),
                        PutAwayDetailID = detail.putaway_detail.PutAwayDetailID,
                        Quantity = qty,
                        BaseQuantity = qty * detail.putaway_detail.ConversionQty,
                        ConversionQty = detail.putaway_detail.ConversionQty,
                        StockUnitID = detail.putaway_detail.StockUnitID,
                        BaseUnitID = detail.putaway_detail.BaseUnitID,
                        ConfirmLocationID = confirm_location.LocationID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                    };

                    putawayConfirmService.Add(confirm);

                    PutAway _putaway = FirstOrDefault(x => x.PalletCode == palletCode && x.PutAwayStatus == PutAwayStatusEnum.ReceiveTrans && x.IsActive);
                    _putaway.PutAwayStatus = PutAwayStatusEnum.PutAway;
                    _putaway.UserModified = UserID;
                    _putaway.DateModified = DateTime.Now;

                    Modify(_putaway);

                    List<StockInOutModel> stockIn = new List<StockInOutModel>
                    {
                        new StockInOutModel
                        {
                            ProductID = detail.putaway.ProductID.Value,
                            StockUnitID = detail.putaway_detail.StockUnitID,
                            BaseUnitID = detail.putaway_detail.BaseUnitID,
                            Lot = detail.putaway.Lot,
                            ProductOwnerID = detail.putaway.ProductOwnerID,
                            SupplierID = detail.putaway.SupplierID.Value,
                            ManufacturingDate = detail.putaway.ManufacturingDate.Value,
                            ExpirationDate = detail.putaway.ExpirationDate.Value,
                            ProductWidth = detail.putaway.ProductWidth,
                            ProductLength = detail.putaway.ProductLength,
                            ProductHeight = detail.putaway.ProductHeight,
                            ProductWeight = detail.putaway.ProductWeight,
                            PackageWeight = detail.putaway.PackageWeight,
                            Price = detail.putaway.Price,
                            ProductUnitPriceID = detail.putaway.ProductUnitPriceID,
                            ProductStatusID = detail.putaway.ProductStatusID,
                            ProductSubStatusID = detail.putaway.ProductSubStatusID,
                            Quantity = qty,
                            ConversionQty = detail.putaway_detail.ConversionQty,
                            PalletCode = detail.putaway.PalletCode,
                            LocationCode = locationCode,
                            DocumentCode = detail.putaway.PutAwayJobCode,
                            DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                            DocumentID = confirm.PutAwayConfirmID
                        }
                    };
                    stockService.UserID = UserID;
                    stockService.Incomming(stockIn);


                    ProductionControlDetail pc = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                    pc.LocationID = confirm.ConfirmLocationID;
                    pc.SugguestLocationID = null;
                    pc.PackingStatus = PackingStatusEnum.PutAway;
                    pc.UserModified = UserID;
                    pc.DateModified = DateTime.Now;
                    pc.RemainQTY = qty;
                    pc.RemainBaseQTY = qty * detail.putaway_detail.ConversionQty;
                    pc.RemainBaseUnitID = confirm.BaseUnitID;
                    pc.RemainStockUnitID = confirm.StockUnitID;
                    pc.WarehouseID = confirm_location.Zone.WarehouseID;
                    pcDetailService.Modify(pc);



                    reciving.ReceivingStatus = ReceivingStatusEnum.Complete;
                    //reciving.Quantity = qty;
                    //reciving.BaseQuantity = detail.putaway_detail.ConversionQty * qty;
                    reciving.UserModified = UserID;
                    reciving.DateModified = DateTime.Now;
                    receivingService.Modify(reciving);


                    var countNotComplete = receivingService.Any(x => x.ReceiveDetailID == reciving.ReceiveDetailID
                    && (x.ReceivingStatus != ReceivingStatusEnum.Complete && x.ReceivingStatus != ReceivingStatusEnum.Reject) && x.IsActive);

                    if (!countNotComplete)
                    {
                        ReceiveDetail receive_detail = receiveDetailService.FindByID(reciving.ReceiveDetailID);
                        receive_detail.ReceiveDetailStatus = ReceiveDetailStatusEnum.Complete;
                        receive_detail.UserModified = UserID;
                        receive_detail.DateModified = DateTime.Now;

                        receiveDetailService.Modify(receive_detail);

                        countNotComplete = receiveDetailService.Any(x => x.ReceiveDetailID == reciving.ReceiveDetailID
                         && (x.ReceiveDetailStatus != ReceiveDetailStatusEnum.Complete && x.ReceiveDetailStatus != ReceiveDetailStatusEnum.Close
                         && x.ReceiveDetailStatus != ReceiveDetailStatusEnum.Cancel) && x.IsActive == true);
                        if (!countNotComplete)
                        {
                            Receive receive = receiveService.FindByID(reciving.ReceiveID);
                            receive.ReceiveStatus = ReceiveStatusEnum.Complete;
                            receive.UserModified = UserID;
                            receive.DateModified = DateTime.Now;

                            receiveService.Modify(receive);
                        }
                    } 
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

        public void CreateAndConfirmKeep(List<PutAwayItem> putawayItem, string palletCode, decimal qty, string locationCode)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region Create 
                    putawayItem.ForEach(item =>
                    {
                        item.PutAwayItemID = Guid.NewGuid();
                        itemService.Add(item);
                    });

                    List<PutAwayItem> result = new List<PutAwayItem>();
                    foreach (PutAwayItem pwy in putawayItem)
                    {

                        var temps = (from putaway in itemService.Where(x => x.IsActive && !x.IsComplete && x.PutAwayItemID == pwy.PutAwayItemID)
                                     join product in productService.Where(x => x.IsActive) on putaway.ProductID equals product.ProductID
                                     join location in locationService.Where(x => x.IsActive) on putaway.SuggestionLocationID equals location.LocationID
                                     //join zone in zoneService.Where(x => x.IsActive) on location.ZoneID equals zone.ZoneID
                                     //join wherehouse in whereHouseService.Where(x => x.IsActive) on zone.WarehouseID equals wherehouse.WarehouseID
                                     // join location in locationService.Where(x => x.IsActive).Include(x => x.Zone.Warehouse) on putaway.SuggestionLocationID equals location.LocationID
                                     select new { putaway, product, location } into pw
                                     select new PutAwayItem
                                     {
                                         PutAwayItemID = pw.putaway.PutAwayItemID,
                                         ProductOwnerID = pw.putaway.ProductOwnerID,
                                         SupplierID = pw.putaway.SupplierID,
                                         FromLocationID = pw.putaway.FromLocationID,
                                         SuggestionLocationID = pw.putaway.SuggestionLocationID,
                                         ProductID = pw.putaway.ProductID,
                                         Lot = pw.putaway.Lot,
                                         PalletCode = pw.putaway.PalletCode,
                                         ManufacturingDate = pw.putaway.ManufacturingDate,
                                         ExpirationDate = pw.putaway.ExpirationDate,
                                         ProductHeight = pw.putaway.ProductHeight,
                                         ProductLength = pw.putaway.ProductLength,
                                         ProductWidth = pw.putaway.ProductWidth,
                                         ProductWeight = pw.putaway.ProductWeight,
                                         PackageWeight = pw.putaway.PackageWeight,
                                         ProductStatusID = pw.putaway.ProductStatusID,
                                         ProductSubStatusID = pw.putaway.ProductSubStatusID,
                                         ProductUnitPriceID = pw.putaway.ProductUnitPriceID,
                                         Price = pw.putaway.Price,
                                         StockUnitID = pw.putaway.StockUnitID,
                                         BaseUnitID = pw.putaway.BaseUnitID,
                                         Quantity = pw.putaway.Quantity,
                                         BaseQuantity = pw.putaway.BaseQuantity,
                                         ConversionQty = pw.putaway.ConversionQty,
                                         Location = pw.location,
                                         DocumentTypeID = pw.putaway.DocumentTypeID
                                     }).ToList();
                        foreach (var item in temps)
                        {
                            var zone = zoneService.FindByID(item.Location.ZoneID);
                            var wherehouse = whereHouseService.FindByID(zone.WarehouseID);
                            item.Location.Zone = zone;
                            item.Location.Zone.Warehouse = wherehouse;
                        }
                        result.AddRange(temps);
                    }

                    var jobs = result.GroupBy(a => new { ZoneId = a.Location.ZoneID, WhareHouse = a.Location.Zone.WarehouseID })
                        .Select(g => new
                        {
                            WhareHouse = g.Key.WhareHouse,
                            ZoneId = g.Key.ZoneId,
                            Quantity = g.Sum(x => x.Quantity),
                        }).ToList();

                    PutAwayPrefix prefix = prefixService.FirstOrDefault() ;

                    jobs.ForEach(item =>
                    {
                        string code = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                        prefix.LastedKey = code;
                        prefixService.Modify(prefix);
                        List<PutAwayItem> jobItems = result.Where(x => x.Location.ZoneID == item.ZoneId && x.Location.Zone.WarehouseID == item.WhareHouse).ToList();
                        foreach (PutAwayItem job in jobItems)
                        {
                            PutAway putAway = new PutAway
                            {
                                PutAwayID = Guid.NewGuid(),
                                PutAwayJobCode = code,
                                StartDate = DateTime.Now,
                                FromLocationID = job.FromLocationID,
                                SuggestionLocationID = job.SuggestionLocationID,
                                PutAwayStatus = PutAwayStatusEnum.PutAway,
                                ProductID = job.ProductID,
                                Lot = job.Lot,
                                ManufacturingDate = job.ManufacturingDate,
                                ExpirationDate = job.ExpirationDate,
                                ProductStatusID = job.ProductStatusID,
                                ProductSubStatusID = job.ProductSubStatusID,
                                PackageWeight = job.PackageWeight,
                                ProductWeight = job.ProductWeight,
                                ProductWidth = job.ProductWidth,
                                ProductLength = job.ProductLength,
                                ProductHeight = job.ProductHeight,
                                PalletCode = job.PalletCode,
                                IsActive = true,
                                ProductOwnerID = job.ProductOwnerID,
                                PutAwayDate = DateTime.Now,
                                SupplierID = job.SupplierID,
                                Price = 1,
                                ZoneID = item.ZoneId,
                                WarehouseID = item.WhareHouse,
                                DocumentTypeID = job.DocumentTypeID,
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                            };

                            Add(putAway);

                            PutAwayDetail _detail = new PutAwayDetail
                            {
                                BaseUnitID = job.BaseUnitID,
                                StockUnitID = job.StockUnitID,
                                ConversionQty = job.ConversionQty,
                                BaseQuantity = job.Quantity * job.ConversionQty,
                                Quantity = job.Quantity,
                                PutAwayID = putAway.PutAwayID,
                                PutAwayDetailID = Guid.NewGuid(),
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                IsActive = true
                            };

                            putAwayDetailService.Add(_detail);
                            PutAwayMap map = new PutAwayMap
                            {
                                PutAwayItemID = job.PutAwayItemID,
                                PutAwayID = putAway.PutAwayID,
                                UserCreated = UserID,
                                UserModified = UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                IsActive = true,

                            };

                            putawayMapService.Add(map);
                        }
                    });
                    #endregion

                    #region Confirm
                    Receiving _reciving = receivingService.FirstOrDefault(x => x.PalletCode == palletCode);
                    Receiving reciving = receivingService.FindByID(_reciving.ReceivingID);
                    if (reciving == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    var detail = (from putaway in Where(e=>e.IsActive)
                                  join putaway_detail in putAwayDetailService.Where(e => e.IsActive) on putaway.PutAwayID equals putaway_detail.PutAwayID
                                  where putaway.PalletCode == palletCode && putaway.PutAwayStatus == PutAwayStatusEnum.PutAway
                                  select new { putaway, putaway_detail }).FirstOrDefault();



                    Location confirm_location = locationService.FirstOrDefault(x => x.Code == locationCode);
                    

                    if (confirm_location == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    if (confirm_location.LocationReserveQty >= confirm_location.PalletCapacity)
                    {
                        throw new HILIException("MSG00006");
                    }
                    var confirm_zone = zoneService.FindByID(confirm_location.ZoneID);
                    confirm_location.LocationReserveQty = (confirm_location.LocationReserveQty == null ? 0 : confirm_location.LocationReserveQty.GetValueOrDefault()) + 1;
                    locationService.Modify(confirm_location);

                    Location form_location = locationService.FirstOrDefault(x => x.LocationID == detail.putaway.FromLocationID);

                    if (form_location == null)
                    {
                        throw new HILIException("MSG00040");
                    }
                    var from_zone = zoneService.FindByID(form_location.ZoneID);
                    form_location.LocationReserveQty = (form_location.LocationReserveQty == null ? 0 : form_location.LocationReserveQty.GetValueOrDefault()) - 1;

                    if (form_location.LocationReserveQty < 0)
                    {
                        form_location.LocationReserveQty = 0;
                    }

                    locationService.Modify(form_location); 
                    PutAwayConfirm confirm = new PutAwayConfirm
                    {
                        PutAwayConfirmID = Guid.NewGuid(),
                        PutAwayDetailID = detail.putaway_detail.PutAwayDetailID,
                        Quantity = qty,
                        BaseQuantity = qty * detail.putaway_detail.ConversionQty,
                        ConversionQty = detail.putaway_detail.ConversionQty,
                        StockUnitID = detail.putaway_detail.StockUnitID,
                        BaseUnitID = detail.putaway_detail.BaseUnitID,
                        ConfirmLocationID = confirm_location.LocationID,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                    };

                    putawayConfirmService.Add(confirm); 
                    List<StockInOutModel> stockIn = new List<StockInOutModel>
                    {
                        new StockInOutModel
                        {
                            ProductID = detail.putaway.ProductID.GetValueOrDefault(),
                            StockUnitID = detail.putaway_detail.StockUnitID,
                            BaseUnitID = detail.putaway_detail.BaseUnitID,
                            Lot = detail.putaway.Lot,
                            ProductOwnerID = detail.putaway.ProductOwnerID,
                            SupplierID = detail.putaway.SupplierID.GetValueOrDefault(),
                            ManufacturingDate = detail.putaway.ManufacturingDate.GetValueOrDefault(),
                            ExpirationDate = detail.putaway.ExpirationDate.GetValueOrDefault(),
                            ProductWidth = detail.putaway.ProductWidth,
                            ProductLength = detail.putaway.ProductLength,
                            ProductHeight = detail.putaway.ProductHeight,
                            ProductWeight = detail.putaway.ProductWeight,
                            PackageWeight = detail.putaway.PackageWeight,
                            Price = detail.putaway.Price,
                            ProductUnitPriceID = detail.putaway.ProductUnitPriceID,
                            ProductStatusID = detail.putaway.ProductStatusID,
                            ProductSubStatusID = detail.putaway.ProductSubStatusID,
                            Quantity = qty,
                            ConversionQty = detail.putaway_detail.ConversionQty,
                            PalletCode = detail.putaway.PalletCode,
                            LocationCode = locationCode,
                            DocumentCode = detail.putaway.PutAwayJobCode,
                            DocumentTypeID = detail.putaway.DocumentTypeID.GetValueOrDefault(),
                            DocumentID = confirm.PutAwayConfirmID,
                            FromLocationCode = form_location.Code,
                        }
                    };
                    stockService.UserID = UserID;
                    stockService.OutgoingAndIncomming(stockIn);


                    ProductionControlDetail pc = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                    pc.LocationID = confirm.ConfirmLocationID;
                    pc.SugguestLocationID = null;
                    pc.PackingStatus = PackingStatusEnum.PutAway;
                    pc.UserModified = UserID;
                    pc.DateModified = DateTime.Now;
                    pc.RemainQTY = qty;
                    pc.RemainBaseQTY = qty * detail.putaway_detail.ConversionQty;
                    pc.RemainBaseUnitID = confirm.BaseUnitID;
                    pc.RemainStockUnitID = confirm.StockUnitID;
                    pc.WarehouseID = confirm_zone.WarehouseID;
                    pcDetailService.Modify(pc); 

                    reciving.ReceivingStatus = ReceivingStatusEnum.Complete;
                    reciving.Quantity = qty;
                    reciving.BaseQuantity = detail.putaway_detail.ConversionQty * qty;
                    reciving.UserModified = UserID;
                    reciving.DateModified = DateTime.Now;
                    receivingService.Modify(reciving);
                    #endregion
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
    }
}
