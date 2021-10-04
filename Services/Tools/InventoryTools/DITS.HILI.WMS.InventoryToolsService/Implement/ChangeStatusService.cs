using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class ChangeStatusService : Repository<Changestatus>, IChangeStatusService
    {
        #region [ Property ] 
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<ChangestatusPrefix> changestatusPrefixService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<ProductStatus> statusService;
        private readonly IRepository<Line> lineService;
        private readonly IRepository<Reclassified> reclassifiedService;
        private readonly IRepository<ReclassifiedDetail> reclassifiedDetailService;
        private readonly IStockService stockService;
        #endregion
        public ChangeStatusService(IUnitOfWork context,
                                 IRepository<ProductionControl> _productionControlService,
                                 IRepository<Product> _product,
                                 IRepository<Line> _line,
                                 IRepository<ProductOwner> _productOwner,
                                 IRepository<Location> _location,
                                 IRepository<ProductUnit> _productUnit,
                                 IRepository<ProductStatus> _productStatus,
                                 IRepository<Warehouse> _warehouse,
                                 IRepository<ProductionControlDetail> _pcDetail,
                                 IRepository<ChangestatusPrefix> _changestatus,
                                 IRepository<Receiving> _receiving,
                                 IRepository<Reclassified> _reclassified,
                                 IRepository<ReclassifiedDetail> _reclassifiedDetail,
                                 IStockService _stockServicee)
            : base(context)
        {
            pcDetailService = _pcDetail;
            productService = _product;
            locationService = _location;
            productUnitService = _productUnit;
            productOwnerService = _productOwner;
            lineService = _line;
            productionControlService = _productionControlService;
            warehouseService = _warehouse;
            stockService = _stockServicee;
            changestatusPrefixService = _changestatus;
            receivingService = _receiving;
            statusService = _productStatus;
            reclassifiedService = _reclassified;
            reclassifiedDetailService = _reclassifiedDetail;
            //base.GetProductInfo<ReceiveService>(Assembly.GetExecutingAssembly());
        }

        public PalletTagModel GetPalletCode(string palletCode)
        {
            try
            {

                PalletTagModel detail = (from pc in pcDetailService.Query().Get()
                                         join pc_head in productionControlService.Query().Get() on pc.ControlID equals pc_head.ControlID
                                         join product in productService.Query().Include(x => x.CodeCollection).Get() on pc_head.ProductID equals product.ProductID
                                         join priceunit in productUnitService.Query().Get() on pc_head.StockUnitID equals priceunit.ProductUnitID
                                         join l in locationService.Query().Include(x => x.Zone).Get() on pc.LocationID equals l.LocationID into g
                                         from lo in g.DefaultIfEmpty()
                                         where pc.PalletCode == palletCode && (pc.PackingStatus != PackingStatusEnum.Waiting_Receive && pc.PackingStatus != PackingStatusEnum.In_Progress
                                         && pc.PackingStatus != PackingStatusEnum.Loading_In && pc.PackingStatus != PackingStatusEnum.Cancel)
                                         && (pc.ReserveQTY == 0 || pc.ReserveQTY == null)
                                         select new { pc, product, priceunit, lo })
                               .Select(n => new PalletTagModel
                               {
                                   ConfirmQty = (n.pc.RemainQTY == null ? 0 : n.pc.RemainQTY) - (n.pc.ReserveQTY == null ? 0 : n.pc.ReserveQTY),
                                   Location = n.lo.Code,
                                   PalletCode = n.pc.PalletCode,
                                   ProductName = n.product.Name,
                                   Qty = (n.pc.RemainQTY == null ? 0 : n.pc.RemainQTY) - (n.pc.ReserveQTY == null ? 0 : n.pc.ReserveQTY),
                                   UnitName = n.priceunit.Name,
                                   PackingStatus = (int)n.pc.PackingStatus,
                                   ReceivingQty = n.pc.RemainQTY,
                                   WarehouseID = n.lo.Zone.WarehouseID,
                                   LotNo = n.pc.LotNo,
                                   ReceiveDetailId = Guid.Empty,
                                   ReceivingID = Guid.Empty,
                                   SuggestLocation = n.lo.Code,
                                   IsTransfer = false
                               }).FirstOrDefault();
                if (detail == null)
                {
                    throw new HILIException("MSG00072");
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

        public bool UpdateChangestatus(string palletCode, decimal qty, Guid reasonId, Guid productStatusId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductStatus status = statusService.Query().Filter(x => x.IsChangeStatus == true).Get().SingleOrDefault();
                    if (status == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    ProductionControlDetail pc = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();

                    Guid? oldProductstatus = pc.ProductStatusID;


                    if (productStatusId == oldProductstatus)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Receiving receiving = receivingService.Query().Filter(x => x.PalletCode == palletCode).Get().SingleOrDefault();



                    if (pc == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    decimal? remain_qty = pc.RemainQTY - qty;

                    pc.UserModified = UserID;
                    pc.DateModified = DateTime.Now;
                    pc.RemainQTY = remain_qty;
                    pc.RemainBaseQTY = (remain_qty) * pc.ConversionQty;



                    Location _location_inspec = locationService.Query().Include(x => x.Zone).Filter(x => x.LocationType == LocationTypeEnum.Inspection && x.Zone.WarehouseID == pc.WarehouseID).Get().SingleOrDefault();
                    if (_location_inspec == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    Location location_inspec = locationService.FindByID(_location_inspec.LocationID);

                    if (location_inspec == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    Location location = locationService.Query().Filter(x => x.LocationID == pc.LocationID).Get().SingleOrDefault();

                    if (remain_qty == 0)
                    {
                        pc.LocationID = null;
                        pc.SugguestLocationID = null;
                    }

                    pcDetailService.Modify(pc);

                    ProductionControlDetail pcI = pcDetailService.Query().Filter(x => x.PalletCode == palletCode + "H").Get().SingleOrDefault();

                    if (pcI == null)
                    {
                        int? seq = pcDetailService.Query().Filter(x => x.ControlID == pc.ControlID).Get().Max(x => x.Sequence) + 1;

                        pcI = new ProductionControlDetail
                        {
                            PackingID = Guid.NewGuid(),
                            ControlID = pc.ControlID,
                            PalletCode = pc.PalletCode + "H",
                            Sequence = seq,
                            StockQuantity = pc.StockQuantity,
                            BaseQuantity = pc.BaseQuantity,
                            ConversionQty = pc.ConversionQty,
                            StockUnitID = pc.StockUnitID,
                            BaseUnitID = pc.BaseUnitID,
                            ProductStatusID = status.ProductStatusID,
                            ProductSubStatusID = pc.ProductSubStatusID,
                            MFGDate = pc.MFGDate,
                            MFGTimeStart = pc.MFGTimeStart,
                            MFGTimeEnd = pc.MFGTimeEnd,
                            LocationID = location_inspec.LocationID,
                            WarehouseID = _location_inspec.Zone.WarehouseID,
                            PackingStatus = PackingStatusEnum.QAInspection,
                            RemainStockUnitID = pc.RemainStockUnitID,
                            RemainBaseUnitID = pc.RemainBaseUnitID,
                            RemainQTY = qty,
                            RemainBaseQTY = qty * pc.ConversionQty,
                            LotNo = pc.LotNo + "H",
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            ReserveBaseQTY = 0,
                            ReserveQTY = 0,
                            ReceiveDetailID = pc.ReceiveDetailID,

                        };
                        pcDetailService.Add(pcI);

                        Receiving rcv = new Receiving
                        {
                            Sequence = seq.Value,
                            GRNCode = receiving.GRNCode.Substring(0, receiving.GRNCode.Length - 1) + seq.Value,
                            Quantity = qty,
                            BaseQuantity = qty * pcI.ConversionQty.Value,
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
                        pcI.RemainQTY += pcI.RemainQTY + qty;
                        pcI.RemainBaseQTY += pcI.RemainQTY * pcI.ConversionQty;
                        pcDetailService.Modify(pcI);
                    }

                    pcI = pcDetailService.Query().Include(x => x.ProductionControl).Filter(x => x.PalletCode == palletCode + "H").Get().SingleOrDefault();

                    #region [ PreFix ]

                    ChangestatusPrefix prefix = changestatusPrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("REG10012");
                    }

                    ChangestatusPrefix tPrefix = changestatusPrefixService.FindByID(prefix.PrefixID);

                    string changestatusCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);

                    tPrefix.IsLastest = false;

                    ChangestatusPrefix newPrefix = new ChangestatusPrefix()
                    {
                        IsLastest = true,
                        LastedKey = changestatusCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    changestatusPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]

                    Changestatus ch = new Changestatus
                    {
                        DamageID = Guid.NewGuid(),
                        DamageCode = changestatusCode,
                        ProductID = pcI.ProductionControl.ProductID,
                        PalletCode = pc.PalletCode,
                        ReprocessPalletCode = pcI.PalletCode,
                        ReprocessLot = pcI.LotNo,
                        StockUnitID = pcI.StockUnitID.Value,
                        BaseUnitID = pcI.BaseUnitID.Value,
                        ReprocessQty = 0,
                        ReprocessBaseQty = 0,
                        RejectQty = 0,
                        RejectBaseQty = 0,
                        DocumentID = pcI.ControlID,
                        InspectionStatus = InspectionStatus.QA_Inspection,
                        LineID = pcI.ProductionControl.LineID,
                        ReasonID = reasonId,
                        ConversionQty = pcI.ProductionControl.ConversionQty.Value,
                        Lot = pc.LotNo,
                        LocationID = pcI.LocationID,
                        DMFromWarehouse = pcI.WarehouseID,
                        WorkerID = UserID,
                        ProductStatusID = productStatusId,
                        DamageDate = DateTime.Now,
                        DamageQty = qty,
                        DamageBaseQty = qty * pcI.ConversionQty.Value,
                        MFGDate = pcI.MFGDate,
                        DispatchReprocessStatus = false,
                        DispatchRejectStatus = false,
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        UserCreated = UserID,
                        UserModified = UserID
                    };
                    Add(ch);



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
                            ProductStatusID = oldProductstatus.Value,//pc.ProductStatusID.Value,
                            ProductSubStatusID = pc.ProductSubStatusID.Value,
                            Quantity = qty,
                            ConversionQty = pc.ProductionControl.ConversionQty.Value,
                            PalletCode = pc.PalletCode,
                            LocationCode = location.Code,
                            DocumentCode = ch.DamageCode,
                            DocumentTypeID = null,
                            DocumentID = ch.DamageID
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
                            ProductStatusID = productStatusId,
                            ProductSubStatusID = pcI.ProductSubStatusID.Value,
                            Quantity = qty,
                            ConversionQty = pcI.ProductionControl.ConversionQty.Value,
                            PalletCode = pcI.PalletCode,
                            LocationCode = location_inspec.Code,
                            DocumentCode = ch.DamageCode,
                            DocumentTypeID = null,
                            DocumentID = ch.DamageID
                        }
                    };
                    stockService.ChangeStatus(stockout, stockChange);

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


    }
}
