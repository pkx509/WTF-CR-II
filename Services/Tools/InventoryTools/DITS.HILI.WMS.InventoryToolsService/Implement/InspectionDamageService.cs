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
    public class InspectionDamageService : Repository<Changestatus>, IInspectionDamageService
    {
        #region [ Property ] 
        private readonly IRepository<Product> productService;
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
        private readonly IRepository<Reclassified> reclassifiedService;
        private readonly IRepository<ReclassifiedDetail> reclassifiedDetailService;
        private readonly IStockService stockService;
        private readonly IRepository<ShippingTo> shiptoService;
        private readonly IRepository<ItfInterfaceMapping> itfInterfaceMappingService;
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


        private readonly IRepository<StockInfo> stockInfoService;
        private readonly IRepository<StockBalance> stockBalanceService;
        private readonly IRepository<StockTransaction> stockTransService;
        private readonly IRepository<StockLocationBalance> stockLocationService;

        #endregion
        public InspectionDamageService(IUnitOfWork context,
                                 IRepository<ProductionControl> _productionControlService,
                                 IRepository<Product> _product,
                                 IRepository<Line> _line,
                                 IRepository<ProductOwner> _productOwner,
                                 IRepository<Location> _location,
                                 IRepository<ProductUnit> _productUnit,
                                 IRepository<ProductStatus> _productStatus,
                                 IRepository<Warehouse> _warehouse,
                                 IRepository<ProductionControlDetail> _pcDetail,
                                 IRepository<ReclassifiedPrefix> _prefix,
                                 IRepository<Receiving> _receiving,
                                 IRepository<Reason> _reason,
                                 //IRepository<Reclassified> _reclassified,
                                 //IRepository<ReclassifiedDetail> _reclassifiedDetail,
                                 IStockService _stockServicee,
                              IDispatchDetailService tmpDispatchDetailService)
            : base(context)
        {
            reasonService = _reason;
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
            reclassifiedService = Context.Repository<Reclassified>();
            reclassifiedDetailService = Context.Repository<ReclassifiedDetail>();
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
        }


        public List<Changestatus> GetInspectionDamage(DateTime sdte, DateTime edte, Guid lineId, string status, string search, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                InspectionStatus statusE = InspectionStatus.QA_Approve;
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    statusE = (InspectionStatus)Enum.Parse(typeof(InspectionStatus), status);
                }


                IEnumerable<Changestatus> list = from ch in Query().Get()
                                                 join re in reasonService.Query().Get() on ch.ReasonID equals re.ReasonID
                                                 join li in lineService.Query().Get() on ch.LineID equals li.LineID
                                                 join l in locationService.Query().Include(x => x.Zone).Get() on ch.LocationID equals l.LocationID
                                                 join w in warehouseService.Query().Get() on l.Zone.WarehouseID equals w.WarehouseID
                                                 join p in productService.Query().Include(x => x.CodeCollection).Get() on ch.ProductID equals p.ProductID
                                                 join uom in productUnitService.Query().Get() on ch.StockUnitID equals uom.ProductUnitID
                                                 where ch.IsActive == true && ch.DamageDate.Date >= sdte && ch.DamageDate.Date <= edte
                                                 && ((!string.IsNullOrEmpty(status) && status != "All" ? ch.InspectionStatus == statusE : "" == ""))
                                                 && (lineId == Guid.Empty ? "" == "" : li.LineID == lineId)
                                                 && (p.Name.ToLower().Contains(search.ToLower()) || p.Code.ToLower().Contains(search.ToLower()) || ch.PalletCode.ToLower().Contains(search.ToLower()))

                                                 select (new Changestatus()
                                                 {
                                                     DamageID = ch.DamageID,
                                                     DamageCode = ch.DamageCode,
                                                     PalletCode = ch.PalletCode,
                                                     Lot = ch.Lot,
                                                     ApproveDate = ch.ApproveDate,
                                                     MFGDate = ch.MFGDate,
                                                     ProductID = p.ProductID,
                                                     ProductCode = p.Code,
                                                     ProductName = p.Name,
                                                     LineCode = li.LineCode,
                                                     LocationNo = l.Code,
                                                     WarehouseName = w.Name,
                                                     UnitName = uom.Name,
                                                     InspectionStatus = ch.InspectionStatus,
                                                     StatusName = ch.InspectionStatus.Description(),
                                                     ProductStatusID = ch.ProductStatusID,
                                                     DamageQty = ch.DamageQty,
                                                     ReprocessQty = ch.ReprocessQty,
                                                     RejectQty = ch.RejectQty,
                                                     DamageDate = ch.DamageDate,
                                                     RejectPalletCode = ch.RejectPalletCode,
                                                     ReprocessPalletCode = ch.ReprocessPalletCode,
                                                     RejectBaseQty = ch.RejectBaseQty,
                                                     ReprocessBaseQty = ch.ReprocessBaseQty,
                                                     ConversionQty = ch.ConversionQty,
                                                     BaseUnitID = ch.BaseUnitID,
                                                     DispatchRejectStatus = ch.DispatchRejectStatus,
                                                     DispatchReprocessStatus = ch.DispatchReprocessStatus,
                                                     ReasonName = re.ReasonName


                                                 });


                totalRecords = list.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    list = list.OrderByDescending(x => x.DamageDate).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public Changestatus SaveInspectionDamage(Guid damageID, decimal rejectQty, decimal reprocessQty)
        {
            try
            {
                Changestatus ch = Query().Filter(x => x.DamageID == damageID).Get().SingleOrDefault();

                //ch.DamageQty = damageQty;
                //ch.DamageBaseQty = damageQty * ch.ConversionQty;

                ch.RejectQty = rejectQty;
                ch.RejectBaseQty = rejectQty * ch.ConversionQty;

                ch.ReprocessQty = reprocessQty;
                ch.ReprocessBaseQty = reprocessQty * ch.ConversionQty;

                ch.DateModified = DateTime.Now;
                ch.UserModified = UserID;
                Modify(ch);

                return ch;
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

        public bool ApproveInspectionDamage(Guid damageID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    Changestatus ch = Query().Filter(x => x.DamageID == damageID).Get().SingleOrDefault();
                    decimal inspectQty = ch.DamageQty;//Inspection
                    decimal? normalQty = ch.DamageQty - (ch.ReprocessQty + ch.RejectQty);
                    decimal? holdQty = ch.ReprocessQty;//Hold
                    decimal? rejectQty = ch.RejectQty;//Reject

                    ch.InspectionStatus = InspectionStatus.QA_Approve;
                    ch.DateModified = DateTime.Now;
                    ch.ApproveDate = DateTime.Now;
                    ch.UserModified = UserID;
                    ch.ApproveBy = UserID;
                    ch.RejectLot = ch.Lot + "R";
                    ch.RejectPalletCode = ch.PalletCode + "R";

                    ch.DispatchRejectStatus = rejectQty == 0;
                    ch.DispatchReprocessStatus = holdQty == 0;
                    Modify(ch);


                    ProductionControlDetail normalPallet = pcDetailService.Query().Filter(x => x.PalletCode == ch.PalletCode).Get().SingleOrDefault();
                    ProductionControlDetail holdPallet = pcDetailService.Query().Filter(x => x.PalletCode == ch.ReprocessPalletCode).Get().SingleOrDefault();//pallet + H

                    Location normal_location = locationService.Query().Filter(x => x.LocationID == normalPallet.LocationID).Include(x => x.Zone).Get().SingleOrDefault();

                    if (normal_location == null)
                    {
                        normal_location = locationService.Query().Include(x => x.Zone).Filter(x => x.LocationType == LocationTypeEnum.Dummy && x.Zone.WarehouseID == normalPallet.WarehouseID && (x.PalletCapacity - x.LocationReserveQty) > 0).Include(x => x.Zone).Get().FirstOrDefault();

                    }

                    Location hold_location = locationService.Query().Filter(x => x.LocationID == holdPallet.LocationID).Include(x => x.Zone).Get().SingleOrDefault();

                    Receiving receiving = receivingService.Query().Filter(x => x.PalletCode == normalPallet.PalletCode).Get().SingleOrDefault();


                    //Normal pallet
                    normalPallet.RemainQTY += normalQty;
                    normalPallet.RemainBaseQTY = normalPallet.RemainQTY * normalPallet.ConversionQty;
                    normalPallet.DateModified = DateTime.Now;
                    normalPallet.UserModified = UserID;
                    if (normalPallet.PackingStatus == PackingStatusEnum.Delivery)
                    {
                        normalPallet.PackingStatus = PackingStatusEnum.PutAway;
                    }

                    if (normalPallet.RemainQTY != 0)
                    {
                        normalPallet.LocationID = normal_location.LocationID;
                    }

                    normalPallet.WarehouseID = normal_location.Zone.WarehouseID;
                    pcDetailService.Modify(normalPallet);

                    //Hold Pallet
                    holdPallet.RemainQTY = holdQty;
                    holdPallet.RemainBaseQTY = holdQty * holdPallet.ConversionQty;
                    holdPallet.DateModified = DateTime.Now;
                    holdPallet.UserModified = UserID;
                    pcDetailService.Modify(holdPallet);


                    if (normalQty != 0)
                    {
                        #region Out Hold - In Good


                        List<StockInOutModel> stockOut = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = ch.ProductID,
                                StockUnitID = ch.StockUnitID,
                                BaseUnitID = ch.BaseUnitID,
                                Lot = ch.ReprocessLot,
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
                                Quantity = normalQty.Value,//out normal
                                ConversionQty = holdPallet.ConversionQty.Value,
                                PalletCode = holdPallet.PalletCode,
                                LocationCode = hold_location.Code,
                                DocumentCode = ch.DamageCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = ch.DamageID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusOut,
                                Remark = "Inspection Damage"
                            }
                        };
                        stockService.UserID = UserID;
                        stockService.Outgoing(stockOut, StockTransactionTypeEnum.ChangeStatusOut);

                        List<StockInOutModel> stockIn = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = ch.ProductID,
                                StockUnitID = ch.StockUnitID,
                                BaseUnitID = ch.BaseUnitID,
                                Lot = normalPallet.LotNo,
                                ProductOwnerID = receiving.ProductOwnerID.Value,
                                SupplierID = receiving.SupplierID.Value,
                                ManufacturingDate = normalPallet.MFGDate.Value,
                                ExpirationDate = receiving.ExpirationDate.Value,
                                ProductWidth = receiving.ProductWidth,
                                ProductLength = receiving.ProductLength,
                                ProductHeight = receiving.ProductHeight,
                                ProductWeight = receiving.ProductWeight,
                                PackageWeight = receiving.PackageWeight,
                                Price = receiving.Price,
                                ProductUnitPriceID = receiving.ProductUnitPriceID,
                                ProductStatusID = normalPallet.ProductStatusID.Value,//hold pallet
                                ProductSubStatusID = normalPallet.ProductSubStatusID.Value,
                                Quantity = normalQty.Value,//in normal
                                ConversionQty = normalPallet.ConversionQty.Value,
                                PalletCode = normalPallet.PalletCode,
                                LocationCode = normal_location.Code,
                                DocumentCode = ch.DamageCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = ch.DamageID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusIn,
                                Remark = "Inspection Damage"
                            }
                        };
                        stockService.Incomming2(stockIn, Context);
                        #endregion
                    }
                    if (rejectQty != 0)
                    {
                        #region Out Hold -In Reject
                        ProductionControlDetail pcI = pcDetailService.Query().Filter(x => x.PalletCode == ch.PalletCode + "R").Get().SingleOrDefault();

                        #region Gen pallet

                        if (pcI == null)
                        {
                            ProductStatus status = statusService.Query().Filter(x => x.IsChangeStatus == true).Get().SingleOrDefault();
                            int? seq = pcDetailService.Query().Filter(x => x.ControlID == normalPallet.ControlID).Get().Max(x => x.Sequence) + 1;
                            pcI = new ProductionControlDetail
                            {
                                PackingID = Guid.NewGuid(),
                                ControlID = normalPallet.ControlID,
                                PalletCode = ch.RejectPalletCode,
                                Sequence = seq,
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
                                RemainQTY = rejectQty,
                                RemainBaseQTY = rejectQty * normalPallet.ConversionQty,
                                LotNo = ch.RejectLot,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                ReceiveDetailID = normalPallet.ReceiveDetailID,
                                ReserveBaseQTY = 0,
                                ReserveQTY = 0
                            };
                            pcDetailService.Add(pcI);

                            Receiving rcv = new Receiving
                            {
                                Sequence = seq.Value,
                                GRNCode = receiving.GRNCode.Substring(0, receiving.GRNCode.Length - 1) + seq.Value,
                                Quantity = rejectQty.Value,
                                BaseQuantity = rejectQty.Value * pcI.ConversionQty.Value,
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

                                Remark = "Change status",
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
                            pcI.RemainQTY += rejectQty;
                            pcI.RemainBaseQTY = pcI.RemainQTY * pcI.ConversionQty;
                            pcDetailService.Modify(pcI);
                        }
                        #endregion

                        ProductionControlDetail rejectPallet = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == ch.PalletCode + "R").Get().SingleOrDefault();

                        List<StockInOutModel> stockOut = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = ch.ProductID,
                                StockUnitID = ch.StockUnitID,
                                BaseUnitID = ch.BaseUnitID,
                                Lot = ch.ReprocessLot,
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
                                Quantity = rejectQty.Value,//out normal
                                ConversionQty = holdPallet.ConversionQty.Value,
                                PalletCode = ch.ReprocessPalletCode,
                                LocationCode = hold_location.Code,
                                DocumentCode = ch.DamageCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = ch.DamageID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusOut,
                                Remark = "Inspection Damage"
                            }
                        };
                        stockService.UserID = UserID;
                        stockService.Outgoing2(stockOut, Context);

                        List<StockInOutModel> stockIn = new List<StockInOutModel>
                        {
                            new StockInOutModel
                            {
                                ProductID = ch.ProductID,
                                StockUnitID = ch.StockUnitID,
                                BaseUnitID = ch.BaseUnitID,
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
                                Quantity = rejectQty.Value,//in normal
                                ConversionQty = rejectPallet.ConversionQty.Value,
                                PalletCode = rejectPallet.PalletCode,
                                LocationCode = hold_location.Code,
                                DocumentCode = ch.DamageCode,
                                // DocumentTypeID = detail.putaway.DocumentTypeID.Value,
                                DocumentID = ch.DamageID,
                                StockTransTypeEnum = StockTransactionTypeEnum.ChangeStatusIn,
                                Remark = "Inspection Damage"
                            }
                        };
                        stockService.Incomming2(stockIn, Context);

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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool SendtoReprocess(List<Changestatus> list)
        {
            try
            {

                if (list.Count == 0)
                {
                    throw new HILIException("MSG00006");
                }

                ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                        .Filter(x => x.IsActive
                                            && x.IsQAReprocessFromDamage == true).Get().FirstOrDefault();

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

        public bool SendtoDamage(List<Changestatus> list)
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

        public void SendToDispatch(List<Changestatus> list, ItfInterfaceMapping dispatchType, bool isReject)
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

                #region Find Warehouse

                //var tmpWarehouse = (from w in warehouse
                //                    join z in zone on w.WarehouseID equals z.WarehouseID
                //                    join l in location on z.ZoneID equals l.ZoneID
                //                    where l.LocationID == changeStatus.LocationID
                //                    select w).FirstOrDefault();

                //if (tmpWarehouse == null)
                //    throw new HILIException("MSG00006");

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

                DispatchPrefix prefixPO = dispatchPrefixService.Query().Filter(x => x.PrefixType == DispatchPreFixTypeEnum.PONO_INTERNAL).Get().SingleOrDefault();
                if (prefixPO == null)
                {
                    throw new HILIException("MSG00006");
                }

                string PO = Prefix.OnCreatePrefixed(prefixPO.LastedKey, prefixPO.PrefixKey, prefixPO.FormatKey, prefixPO.LengthKey);

                prefixPO.LastedKey = PO;

                dispatchPrefixService.Modify(prefixPO);

                #endregion

                #region Insert Dispatch

                Dispatch dispatchModel = new Dispatch()
                {
                    Pono = PO,

                    DispatchStatus = DispatchStatusEnum.InprogressConfirmQA,
                    DocumentId = dispatchType.DocumentId,
                    DispatchCode = DispatchCode,
                    DeliveryDate = dateNow,
                    DocumentDate = dateNow,
                    OrderDate = dateNow,
                    ShipptoId = shipto.ShipToId,
                    IsUrgent = false,
                    IsBackOrder = false,
                    IsActive = true,
                    Remark = "Inspection Damage",
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
                foreach (Changestatus itm in list)
                {
                    Changestatus ch = Query().Filter(x => x.DamageID == itm.DamageID).Get().SingleOrDefault();

                    string palletCode = isReject ? ch.RejectPalletCode : ch.ReprocessPalletCode;

                    ProductionControlDetail pc = pcDetailService.Query().Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();
                    Receiving rcv = receivingService.Query().Filter(x => x.ReceiveDetailID == pc.ReceiveDetailID && x.PalletCode == palletCode).Get().SingleOrDefault();
                    Receive rv = receiveService.Query().Filter(x => x.ReceiveID == rcv.ReceiveID).Get().SingleOrDefault();

                    if (isReject)
                    {
                        if (ch.DispatchRejectStatus)
                        {
                            throw new HILIException("MSG00089");
                        }
                    }
                    else
                    {
                        if (ch.DispatchReprocessStatus)
                        {
                            throw new HILIException("MSG00089");
                        }
                    }

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
                        ProductId = ch.ProductID,
                        StockUnitId = ch.StockUnitID,
                        Quantity = isReject ? ch.RejectQty : ch.ReprocessQty,
                        BaseQuantity = isReject ? ch.RejectBaseQty : ch.ReprocessBaseQty,
                        BaseUnitId = ch.BaseUnitID,
                        ConversionQty = ch.ConversionQty,
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
                        Remark = "Inspection Damage",
                        PalletCode = palletCode
                    });

                    #endregion

                    #region Reserve Stock

                    StockSearch stockModel = new StockSearch()
                    {
                        Lot = pc.LotNo,
                        LocationID = pc.LocationID,
                        ManufacturingDate = pc.MFGDate.Value,
                        ExpirationDate = rcv.ExpirationDate.Value,
                        ProductID = ch.ProductID,
                        ConversionQty = ch.ConversionQty,
                        ProductOwnerID = rcv.ProductOwnerID.Value,
                        QTY = ch.ReprocessQty.Value,
                        SupplierID = rv.SupplierID,
                        StockUnitID = ch.StockUnitID,
                        BaseUnitID = ch.BaseUnitID,
                        ProductStatusID = pc.ProductStatusID.Value,
                    };

                    if (ch.InspectionStatus != InspectionStatus.SendtoReprocess)
                    {
                        stockListModel.Add(stockModel);
                    }

                    #endregion

                    if (isReject)
                    {
                        ch.DispatchRejectStatus = isReject;
                    }
                    else
                    {
                        ch.DispatchReprocessStatus = !isReject;
                    }

                    ch.InspectionStatus = InspectionStatus.SendtoReprocess;
                    ch.DateModified = DateTime.Now;
                    ch.UserModified = UserID;
                    Modify(ch);
                    seq++;
                }

                _DispatchDetailService.UserID = UserID;
                _DispatchDetailService.AddList(dispatchDetailCustoms);
                // stockService.UserID = UserID;
                //stockService.AdjustReserve_(stockListModel, StockReserveTypeEnum.Reserve);

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
                //var disp_detail = dispatchDetailService.Query().Filter(a => a.DispatchId == dispatchModel.DispatchId).Get().Select(X => X.DispatchDetailId);



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
                            AssignStatus =PickingStatusEnum.Pick,
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
    }
}
