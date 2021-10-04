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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.InventoryToolsService
{
    public class InspectionGoodsReturnService : Repository<GoodsReturn>, IInspectionGoodsReturnService
    {
        #region [ Property ] 
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductCodes> productCodeService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<ProductStatus> statusService;
        private readonly IRepository<Line> lineService;
        private readonly IRepository<GoodsReturnDetail> returnDetailService;
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
        private readonly IRepository<ShippingTo> shiptoService;

        #endregion

        public InspectionGoodsReturnService(IUnitOfWork context,
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
                                IRepository<Receiving> _receiving,
                                IRepository<GoodsReturnDetail> _returnDetail,
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
            receivingService = _receiving;
            statusService = _productStatus;
            returnDetailService = _returnDetail;
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

            pickingPrefixService = context.Repository<PickingPrefix>();
            pickingService = context.Repository<Picking>();
            pickingAssignService = context.Repository<PickingAssign>();



            stockInfoService = context.Repository<StockInfo>();
            stockBalanceService = context.Repository<StockBalance>();
            stockTransService = context.Repository<StockTransaction>();
            stockLocationService = context.Repository<StockLocationBalance>();
        }

        public List<GoodsReturn> GetInspectionGoodsReturn(DateTime sdte, DateTime edte, string status, string search, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }

                totalRecords = 0;

                GoodsReturnStatusEnum statusE = GoodsReturnStatusEnum.QA_Inspection;
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    statusE = (GoodsReturnStatusEnum)Enum.Parse(typeof(GoodsReturnStatusEnum), status);
                }
                IEnumerable<GoodsReturn> list = from gr in Query().Get()
                                                join rcv in receiveService.Query().Get() on gr.ReceiveID equals rcv.ReceiveID
                                                where gr.IsActive == true && gr.DateCreated.Date >= sdte && gr.DateCreated.Date <= edte
                                                && ((!string.IsNullOrEmpty(status) && status != "All" ? gr.GoodsReturnStatus == statusE : "" == ""))
                                                && rcv.PONumber.ToLower().Contains(search.ToLower())
                                                select (new GoodsReturn()
                                                {
                                                    GoodsReturnID = gr.GoodsReturnID,
                                                    ReceiveCode = rcv.ReceiveCode,
                                                    PONumber = rcv.PONumber,
                                                    IsApprove = gr.GoodsReturnStatus == GoodsReturnStatusEnum.QA_Approve || gr.GoodsReturnStatus == GoodsReturnStatusEnum.SendtoReprocess,
                                                    ApproveDate = gr.ApproveDate,
                                                    GoodsReturnStatusName = gr.GoodsReturnStatus.Description(),
                                                    Description = gr.Remark
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

        public GoodsReturn GetInspectionGoodsReturnByID(Guid id)
        {
            try
            {
                GoodsReturn result = Query().Filter(x => x.IsActive == true && x.GoodsReturnID == id).Get().FirstOrDefault();

                Receive receive = receiveService.Query().Filter(x => x.IsActive == true && x.ReceiveID == result.ReceiveID).Get().FirstOrDefault();

                result.PONumber = receive.PONumber;
                result.ReceiveCode = receive.ReceiveCode;
                result.ReceiveDate = receive.ActualDate != null ? receive.ActualDate.Value : receive.EstimateDate;

                var result_detail = (from returns in Query().Get()
                                     join returns_detail in returnDetailService.Query().Get() on returns.GoodsReturnID equals returns_detail.GoodsReturnID
                                     join product in productService.Query().Include(x => x.CodeCollection).Get() on returns_detail.ProductID equals product.ProductID
                                     join priceunit in productUnitService.Query().Get() on returns_detail.ReceiveUnitID equals priceunit.ProductUnitID
                                     join line in lineService.Query().Get() on returns_detail.LineID equals line.LineID
                                     where returns_detail.IsActive == true && returns_detail.GoodsReturnID == id
                                     select new { returns, returns_detail, product, priceunit, line });


                List<ItemGoodsReturn> _detail = result_detail.Select(n => new ItemGoodsReturn
                {
                    GoodsReturnId = n.returns.GoodsReturnID,
                    GoodsReturnDetailId = n.returns_detail.GoodsReturnDetailID,
                    ProductCode = n.product.Code,
                    ProductName = n.product.Name,
                    ReceiveQTY = n.returns_detail.ReceiveQTY,
                    ReprocessQty = n.returns_detail.ReprocessQty,
                    RejectQty = n.returns_detail.RejectQty,
                    UnitName = n.priceunit.Name,
                    ReceiveLot = n.returns_detail.ReceiveLot,
                    LineCode = n.line.LineCode,
                    ReceiveUnitID = n.returns_detail.ReceiveUnitID.Value,
                    MFGDate = n.returns_detail.MFGDate,
                    ConversionQty = n.returns_detail.ConversionQTY.Value,
                    LineID = n.returns_detail.LineID,
                    GoodsReturnStatus = n.returns.GoodsReturnStatus,
                    RejectStatus = n.returns_detail.RejectStatus,
                    ReprocessStatus = n.returns_detail.ReprocessStatus,
                    ReceiveDetailID = n.returns_detail.ReceiveDetailID

                }).ToList();

                List<ProductStatus> status = statusService.Query().Filter(x => x.IsActive == true).Get().ToList();



                result.ItemGoodsReturns = _detail;

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

        public bool SaveInspectionGoodsReturn(List<ItemGoodsReturn> _return, bool isApprove)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Guid id = _return.Select(x => x.GoodsReturnId).FirstOrDefault();
                    DateTime? approveDate = _return.Select(x => x.ApproveDate).FirstOrDefault();
                    string desc = _return.Select(x => x.Description).FirstOrDefault();
                    GoodsReturn goods = FindByID(id);

                    if (approveDate != DateTime.MinValue)
                    {
                        goods.ApproveDate = approveDate;

                    }
                    goods.Description = desc;
                    goods.UserModified = UserID;
                    goods.DateModified = DateTime.Now;
                    if (isApprove)
                    {
                        goods.ApproveBy = UserID;
                        goods.GoodsReturnStatus = GoodsReturnStatusEnum.QA_Approve;
                    }
                    Modify(goods);

                    SaveData(goods, _return, isApprove);

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

        public void SaveData(GoodsReturn goods, List<ItemGoodsReturn> _return, bool isApprove)
        {
            CultureInfo cultureinfo = new CultureInfo("en-US");
            Receive rvc = receiveService.FindByID(goods.ReceiveID);

            List<ReceiveDetail> rvc_detail = receiveDetailService.Query().Filter(x => x.ReceiveID == rvc.ReceiveID).Get().ToList();
            _return.ForEach(itm =>
            {

                GoodsReturnDetail _detail = returnDetailService.FindByID(itm.GoodsReturnDetailId);
                // To do check null
                ReceiveDetail _rvc_detai = rvc_detail.Where(x => x.ProductID == _detail.ProductID && x.StockUnitID == _detail.ReceiveUnitID && x.Quantity == _detail.ReceiveQTY).FirstOrDefault();
                // To do check null
                _detail.RejectQty = itm.RejectQty;
                _detail.ReprocessQty = itm.ReprocessQty;
                _detail.UserModified = UserID;
                _detail.DateModified = DateTime.Now;
                returnDetailService.Modify(_detail);

                if (isApprove)
                {
                    string lineCode = itm.LineCode;
                    TimeSpan timeStamp = DateTime.Now.TimeOfDay;
                    string suffix = "";
                    Line line = lineService.Query().Filter(x => x.LineCode == lineCode).Get().FirstOrDefault();
                    Location location = locationService.Query().Filter(x => x.Zone.WarehouseID == line.WarehouseID && x.LocationType == LocationTypeEnum.Inspection).Get().FirstOrDefault();
                    Warehouse wh = warehouseService.FindByID(line.WarehouseID);


                    decimal? rejectQty = _detail.RejectQty == null ? 0 : _detail.RejectQty;
                    decimal? reprocessQty = _detail.ReprocessQty == null ? 0 : _detail.ReprocessQty;
                    decimal? receiveQty = _detail.ReceiveQTY == null ? 0 : _detail.ReceiveQTY;
                    decimal? normalQty = receiveQty - rejectQty - reprocessQty;
                    ProductionControl pc = productionControlService.Query().Filter(x => x.ReferenceID == _rvc_detai.ReceiveDetailID).Get().FirstOrDefault();

                    ProductUnit uom = productUnitService.Query().Filter(x => x.ProductUnitID == pc.ProductUnitID).Get().FirstOrDefault();


                    List<StockInOutModel> stockIn = new List<StockInOutModel>();
                    // To do check null
                    //var pc_detail = pcDetailService.Query().Filter(x => x.IsActive && (x.IsNormal ?? false) && x.ControlID == pc.ControlID).Get().Sum(x => x.StockQuantity) ?? 0;
                    int iSeq = 1;
                    //normal
                    if (normalQty > 0)
                    {
                        #region normal
                        decimal iPallet = Math.Ceiling(normalQty.Value / uom.PalletQTY);
                        decimal sumQty = normalQty.Value % uom.PalletQTY;
                        sumQty = sumQty == 0 ? uom.PalletQTY : sumQty;
                        for (int i = 1; i <= iPallet; i++)
                        {
                            ProductStatus status = statusService.Query().Include(x => x.ProductStatusMapCollection).Filter(x => x.IsDefault == true).Get().SingleOrDefault();

                            ProductionControlDetail pc_detail = new ProductionControlDetail()
                            {
                                ControlID = pc.ControlID,
                                PalletCode = pc.ProductionDate.ToString("yyyyMMdd", cultureinfo)
                                                        + lineCode
                                                        + iSeq.ToString("000")
                                                        + timeStamp.ToString("hhmmss") + suffix,
                                LotNo = itm.ReceiveLot + suffix,
                                Sequence = iSeq,
                                StockQuantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                ConversionQty = pc.ConversionQty,
                                StockUnitID = pc.StockUnitID,
                                BaseUnitID = pc.BaseUnitID,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                MFGDate = pc.ProductionDate,
                                MFGTimeStart = pc.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                MFGTimeEnd = timeStamp,
                                PackingStatus = PackingStatusEnum.PutAway,
                                WarehouseID = line.WarehouseID,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                LocationID = location.LocationID,
                                ReserveQTY = 0,
                                ReserveBaseQTY = 0,
                                RemainBaseQTY = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                RemainQTY = (i == iPallet) ? sumQty : uom.PalletQTY,
                                RemainStockUnitID = pc.StockUnitID,
                                IsNormal = string.IsNullOrWhiteSpace(suffix),
                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                OptionalSuffix = line.LineType == "NP" ? iSeq.ToString() : "S01"
                            };

                            Receiving reciving = new Receiving()
                            {
                                GRNCode = rvc.ReceiveCode + iSeq.ToString(),
                                ReceiveID = rvc.ReceiveID,
                                Sequence = iSeq,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                IsDraft = true,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                ProductID = _rvc_detai.ProductID,
                                Lot = itm.ReceiveLot,
                                ManufacturingDate = _rvc_detai.ManufacturingDate,
                                ExpirationDate = _rvc_detai.ExpirationDate,
                                Quantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty.Value),
                                ConversionQty = pc.ConversionQty.Value,
                                StockUnitID = pc.StockUnitID.Value,
                                BaseUnitID = pc.BaseUnitID.Value,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,
                                PalletCode = pc_detail.PalletCode,
                                ProductOwnerID = rvc.ProductOwnerID,
                                SupplierID = rvc.SupplierID,

                                IsActive = true,
                                IsSentInterface = false,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };

                            receivingService.Add(reciving);
                            pcDetailService.Add(pc_detail);

                            stockIn.Add(new StockInOutModel
                            {
                                ProductID = reciving.ProductID,
                                StockUnitID = reciving.StockUnitID,
                                BaseUnitID = reciving.BaseUnitID,
                                ConversionQty = reciving.ConversionQty,
                                Lot = reciving.Lot,
                                ProductOwnerID = reciving.ProductOwnerID.Value,
                                SupplierID = reciving.SupplierID.Value,
                                ManufacturingDate = reciving.ManufacturingDate.Value,
                                ExpirationDate = reciving.ExpirationDate.Value,
                                ProductWidth = reciving.ProductWidth,
                                ProductLength = reciving.ProductLength,
                                ProductHeight = reciving.ProductHeight,
                                ProductWeight = reciving.ProductWeight,
                                PackageWeight = reciving.PackageWeight,
                                Price = reciving.Price,
                                ProductUnitPriceID = reciving.ProductUnitPriceID,
                                ProductStatusID = reciving.ProductStatusID,
                                ProductSubStatusID = reciving.ProductSubStatusID.Value,
                                Quantity = reciving.Quantity,
                                PalletCode = reciving.PalletCode,
                                LocationCode = location.Code,
                                DocumentCode = rvc.ReceiveCode,//reciving.GRNCode,
                                DocumentTypeID = rvc.ReceiveTypeID,
                                DocumentID = reciving.ReceivingID
                            });

                            iSeq++;
                        }
                        #endregion
                    }
                    if (_detail.RejectQty > 0)
                    {
                        #region Reject
                        suffix = "R";
                        decimal iPallet = Math.Ceiling(_detail.RejectQty.Value / uom.PalletQTY);
                        decimal sumQty = _detail.RejectQty.Value % uom.PalletQTY;
                        sumQty = sumQty == 0 ? uom.PalletQTY : sumQty;
                        for (int i = 1; i <= iPallet; i++)
                        {
                            ProductStatus status = statusService.Query().Include(x => x.ProductStatusMapCollection).Filter(x => x.IsInspectionReclassify == true && x.IsDefault == false).Get().SingleOrDefault();

                            ProductionControlDetail pc_detail = new ProductionControlDetail()
                            {
                                ControlID = pc.ControlID,
                                PalletCode = pc.ProductionDate.ToString("yyyyMMdd", cultureinfo)
                                                        + lineCode
                                                        + iSeq.ToString("000")
                                                        + timeStamp.ToString("hhmmss") + suffix,
                                LotNo = itm.ReceiveLot + suffix,
                                Sequence = iSeq,
                                StockQuantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                ConversionQty = pc.ConversionQty,
                                StockUnitID = pc.StockUnitID,
                                BaseUnitID = pc.BaseUnitID,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                MFGDate = pc.ProductionDate,
                                MFGTimeStart = pc.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                MFGTimeEnd = timeStamp,
                                PackingStatus = PackingStatusEnum.PutAway,
                                WarehouseID = line.WarehouseID,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                LocationID = location.LocationID,
                                ReserveQTY = (i == iPallet) ? sumQty : uom.PalletQTY,
                                ReserveBaseQTY = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),

                                IsNormal = string.IsNullOrWhiteSpace(suffix),
                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                RemainBaseQTY = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                RemainQTY = (i == iPallet) ? sumQty : uom.PalletQTY,
                                RemainStockUnitID = pc.StockUnitID,
                                OptionalSuffix = line.LineType == "NP" ? iSeq.ToString() : "S01"
                            };

                            Receiving reciving = new Receiving()
                            {
                                GRNCode = rvc.ReceiveCode + iSeq.ToString(),
                                ReceiveID = rvc.ReceiveID,
                                Sequence = iSeq,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                IsDraft = true,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                ProductID = _rvc_detai.ProductID,
                                Lot = itm.ReceiveLot + suffix,
                                ManufacturingDate = _rvc_detai.ManufacturingDate,
                                ExpirationDate = _rvc_detai.ExpirationDate,
                                Quantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty.Value),
                                ConversionQty = pc.ConversionQty.Value,
                                StockUnitID = pc.StockUnitID.Value,
                                BaseUnitID = pc.BaseUnitID.Value,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,
                                PalletCode = pc_detail.PalletCode,
                                ProductOwnerID = rvc.ProductOwnerID,
                                SupplierID = rvc.SupplierID,

                                IsActive = true,
                                IsSentInterface = false,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };


                            receivingService.Add(reciving);
                            pcDetailService.Add(pc_detail);

                            stockIn.Add(new StockInOutModel
                            {
                                ProductID = reciving.ProductID,
                                StockUnitID = reciving.StockUnitID,
                                BaseUnitID = reciving.BaseUnitID,
                                ConversionQty = reciving.ConversionQty,
                                Lot = reciving.Lot,
                                ProductOwnerID = reciving.ProductOwnerID.Value,
                                SupplierID = reciving.SupplierID.Value,
                                ManufacturingDate = reciving.ManufacturingDate.Value,
                                ExpirationDate = reciving.ExpirationDate.Value,
                                ProductWidth = reciving.ProductWidth,
                                ProductLength = reciving.ProductLength,
                                ProductHeight = reciving.ProductHeight,
                                ProductWeight = reciving.ProductWeight,
                                PackageWeight = reciving.PackageWeight,
                                Price = reciving.Price,
                                ProductUnitPriceID = reciving.ProductUnitPriceID,
                                ProductStatusID = reciving.ProductStatusID,
                                ProductSubStatusID = reciving.ProductSubStatusID.Value,
                                Quantity = reciving.Quantity,
                                PalletCode = reciving.PalletCode,
                                LocationCode = location.Code,
                                DocumentCode = rvc.ReceiveCode,//reciving.GRNCode,
                                DocumentTypeID = rvc.ReceiveTypeID,
                                DocumentID = reciving.ReceivingID
                            });

                            iSeq++;
                        }
                        #endregion
                    }
                    if (_detail.ReprocessQty > 0)
                    {
                        #region Reprocess
                        suffix = "H";
                        decimal iPallet = Math.Ceiling(_detail.ReprocessQty.Value / uom.PalletQTY);
                        decimal sumQty = _detail.ReprocessQty.Value % uom.PalletQTY;
                        sumQty = sumQty == 0 ? uom.PalletQTY : sumQty;
                        for (int i = 1; i <= iPallet; i++)
                        {
                            ProductStatus status = statusService.Query().Include(x => x.ProductStatusMapCollection).Filter(x => x.IsInspectionReclassify == true && x.IsDefault == false).Get().SingleOrDefault();

                            ProductionControlDetail pc_detail = new ProductionControlDetail()
                            {
                                ControlID = pc.ControlID,
                                PalletCode = pc.ProductionDate.ToString("yyyyMMdd", cultureinfo)
                                                        + lineCode
                                                        + iSeq.ToString("000")
                                                        + timeStamp.ToString("hhmmss") + suffix,
                                LotNo = itm.ReceiveLot + suffix,
                                Sequence = iSeq,
                                StockQuantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                ConversionQty = pc.ConversionQty,
                                StockUnitID = pc.StockUnitID,
                                BaseUnitID = pc.BaseUnitID,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                MFGDate = pc.ProductionDate,
                                MFGTimeStart = pc.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                MFGTimeEnd = timeStamp,
                                PackingStatus = PackingStatusEnum.PutAway,
                                WarehouseID = line.WarehouseID,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                LocationID = location.LocationID,
                                ReserveQTY = (i == iPallet) ? sumQty : uom.PalletQTY,
                                ReserveBaseQTY = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),

                                IsNormal = false,
                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,
                                RemainBaseQTY = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty),
                                RemainQTY = (i == iPallet) ? sumQty : uom.PalletQTY,
                                RemainStockUnitID = pc.StockUnitID,
                                OptionalSuffix = line.LineType == "NP" ? iSeq.ToString() : "S01"
                            };

                            Receiving reciving = new Receiving()
                            {
                                GRNCode = rvc.ReceiveCode + iSeq.ToString(),
                                ReceiveID = rvc.ReceiveID,
                                Sequence = iSeq,
                                ReceiveDetailID = _rvc_detai.ReceiveDetailID,
                                IsDraft = true,
                                ReceivingStatus = ReceivingStatusEnum.Complete,
                                ProductID = _rvc_detai.ProductID,
                                Lot = itm.ReceiveLot + suffix,
                                ManufacturingDate = _rvc_detai.ManufacturingDate,
                                ExpirationDate = _rvc_detai.ExpirationDate,
                                Quantity = (i == iPallet) ? sumQty : uom.PalletQTY,
                                BaseQuantity = (((i == iPallet) ? sumQty : uom.PalletQTY) * pc.ConversionQty.Value),
                                ConversionQty = pc.ConversionQty.Value,
                                StockUnitID = pc.StockUnitID.Value,
                                BaseUnitID = pc.BaseUnitID.Value,
                                ProductStatusID = status.ProductStatusID,
                                ProductSubStatusID = status.ProductStatusMapCollection.Where(x => x.IsDefault == true).FirstOrDefault().ProductSubStatusID,
                                PackageWeight = 1,
                                ProductWeight = 1,
                                ProductWidth = 1,
                                ProductLength = 1,
                                ProductHeight = 1,
                                PalletCode = pc_detail.PalletCode,
                                ProductOwnerID = rvc.ProductOwnerID,
                                SupplierID = rvc.SupplierID,

                                IsActive = true,
                                IsSentInterface = false,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };


                            receivingService.Add(reciving);
                            pcDetailService.Add(pc_detail);

                            stockIn.Add(new StockInOutModel
                            {
                                ProductID = reciving.ProductID,
                                StockUnitID = reciving.StockUnitID,
                                BaseUnitID = reciving.BaseUnitID,
                                ConversionQty = reciving.ConversionQty,
                                Lot = reciving.Lot,
                                ProductOwnerID = reciving.ProductOwnerID.Value,
                                SupplierID = reciving.SupplierID.Value,
                                ManufacturingDate = reciving.ManufacturingDate.Value,
                                ExpirationDate = reciving.ExpirationDate.Value,
                                ProductWidth = reciving.ProductWidth,
                                ProductLength = reciving.ProductLength,
                                ProductHeight = reciving.ProductHeight,
                                ProductWeight = reciving.ProductWeight,
                                PackageWeight = reciving.PackageWeight,
                                Price = reciving.Price,
                                ProductUnitPriceID = reciving.ProductUnitPriceID,
                                ProductStatusID = reciving.ProductStatusID,
                                ProductSubStatusID = reciving.ProductSubStatusID.Value,
                                Quantity = reciving.Quantity,
                                PalletCode = reciving.PalletCode,
                                LocationCode = location.Code,
                                DocumentCode = rvc.ReceiveCode,//reciving.GRNCode,
                                DocumentTypeID = rvc.ReceiveTypeID,
                                DocumentID = reciving.ReceivingID
                            });

                            iSeq++;
                        }
                        #endregion
                    }

                    stockService.UserID = UserID;
                    stockService.Incomming(stockIn);


                    ProductionControl pcHeader = productionControlService.FindByID(pc.ControlID);

                    pcHeader.PcControlStatus = (int)PCControlStatusEnum.Complete;
                    pcHeader.UserModified = UserID;
                    pcHeader.DateModified = DateTime.Now;
                    productionControlService.Modify(pcHeader);
                }
            });
        }

        public bool SendtoReprocess(List<ItemGoodsReturn> list)
        {
            try
            {

                if (list.Count == 0)
                {
                    throw new HILIException("MSG00006");
                }

                Guid goodsReturnID = list.Max(z => z.GoodsReturnId);
                GoodsReturn gReturn = FindByID(goodsReturnID);
                Receive receive = receiveService.FindByID(gReturn.ReceiveID);

                ItfInterfaceMapping _dispatchType = itfInterfaceMappingService.Query()
                                        .Filter(x => x.DocumentId == receive.ReceiveTypeID).Get().FirstOrDefault();

                ItfInterfaceMapping dispatchType = itfInterfaceMappingService.Query()
                                       .Filter(x => x.DocumentId == _dispatchType.ReferenceDocumentID).Get().FirstOrDefault();

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

        public bool SendtoDamage(List<ItemGoodsReturn> list)
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

        public void SendToDispatch(List<ItemGoodsReturn> list, ItfInterfaceMapping dispatchType, bool isReject)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DateTime dateNow = DateTime.Now;
                IEnumerable<Warehouse> warehouse = warehouseService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Zone> zone = zoneService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Location> location = locationService.Query().Filter(x => x.IsActive).Get();

                Guid goodsReturnID = list.Max(z => z.GoodsReturnId);
                GoodsReturn _return = Query().Filter(x => x.GoodsReturnID == goodsReturnID).Get().FirstOrDefault();
                GoodsReturn gReturn = FindByID(_return.GoodsReturnID);

                Receive receive = receiveService.FindByID(gReturn.ReceiveID);
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

                Dispatch dispatchModel = new Dispatch()
                {
                    Pono = PO,

                    DispatchStatus =DispatchStatusEnum.InprogressConfirmQA,
                    DocumentId = dispatchType.DocumentId,
                    DispatchCode = DispatchCode,
                    DeliveryDate = dateNow,
                    DocumentDate = dateNow,
                    OrderDate = dateNow,
                    ShipptoId = shipto.ShipToId,
                    CustomerId = receive.SupplierID,
                    //SupplierId = receive.SupplierID,
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


                if (gReturn.GoodsReturnStatus != GoodsReturnStatusEnum.SendtoReprocess)
                {
                    gReturn.GoodsReturnStatus = GoodsReturnStatusEnum.SendtoReprocess;
                    gReturn.UserModified = UserID;
                    gReturn.DateModified = DateTime.Now;
                    Modify(gReturn);
                }

                int seq = 1;
                foreach (ItemGoodsReturn itm in list)
                {
                    GoodsReturnDetail _return_detail = returnDetailService.Query().Filter(x => x.GoodsReturnDetailID == itm.GoodsReturnDetailId).Get().SingleOrDefault();

                    ReceiveDetail receiveDetail = receiveDetailService.Query().Filter(x => x.ReceiveDetailID == _return_detail.ReceiveDetailID).Get().FirstOrDefault();

                    List<ProductionControlDetail> pc = pcDetailService.Query().Filter(x => x.ReceiveDetailID == receiveDetail.ReceiveDetailID && x.LotNo == itm.ReceiveLot + (isReject ? "R" : "H")).Get().ToList();

                    List<DispatchDetailPalletCustom> pallets = new List<DispatchDetailPalletCustom>();
                    pc.ForEach(item =>
                    {
                        Receiving rcv = receivingService.Query().Filter(x => x.ReceiveDetailID == item.ReceiveDetailID && x.PalletCode == item.PalletCode).Get().SingleOrDefault();

                        pallets.Add(new DispatchDetailPalletCustom
                        {
                            PalletCode = item.PalletCode,
                            ExpirationDate = rcv.ExpirationDate,
                            Mfgdate = rcv.ManufacturingDate.Value,
                            RequestBaseQty = rcv.BaseQuantity,
                            RequestBaseUnitId = rcv.BaseUnitID,
                            RequestQty = rcv.Quantity,
                            RequestStockUnitId = rcv.StockUnitID
                        });
                    });
                    if (isReject)
                    {
                        if (_return_detail.RejectStatus)
                        {
                            throw new HILIException("MSG00089");
                        }
                    }
                    else
                    {
                        if (_return_detail.ReprocessStatus)
                        {
                            throw new HILIException("MSG00089");
                        }
                    }

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
                        ProductId = _return_detail.ProductID.Value,
                        StockUnitId = itm.ReceiveUnitID,
                        Quantity = isReject ? itm.RejectQty : itm.ReprocessQty,
                        BaseQuantity = (isReject ? itm.RejectQty : itm.ReprocessQty) * itm.ConversionQty,
                        BaseUnitId = pc[0].BaseUnitID,
                        ConversionQty = itm.ConversionQty,
                        ProductOwnerId = receiveDetail.ProductOwnerID.Value,
                        ProductStatusId = pc[0].ProductStatusID,
                        ProductSubStatusId = pc[0].ProductSubStatusID,

                        BookingStatus = (int)BookingStatusEnum.Complete,
                        LocationId = pc[0].LocationID.Value,
                        ProductLot = pc[0].LotNo,
                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = dateNow,
                        UserModified = UserID,
                        DateModified = dateNow,
                        Remark = "Inspection Goods Return",
                        PalletCodes = pallets
                    });

                    #endregion 

                    if (isReject)
                    {
                        _return_detail.RejectStatus = true;
                    }
                    else
                    {
                        _return_detail.ReprocessStatus = true;
                    }

                    _return_detail.UserModified = UserID;
                    _return_detail.DateModified = DateTime.Now;
                    returnDetailService.Modify(_return_detail);
                }
                AddList(dispatchDetailCustoms);

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

        public bool AddList(List<DispatchDetailCustom> dispatchDetails)
        {
            try
            {
                foreach (DispatchDetailCustom item in dispatchDetails)
                {
                    DispatchDetail dispatchDetailModel = new DispatchDetail
                    {
                        DispatchId = item.DispatchID,
                        RuleId = item.RuleID,
                        DispatchDetailStatus =(DispatchDetailStatusEnum) item.DispatchDetailStatus,
                        DispatchDetailProductWidth = 0,
                        DispatchDetailProductLength = 0,
                        DispatchDetailProductHeight = 0,
                        DispatchPrice = 0,
                        IsBackOrder = false,

                        Sequence = item.Sequence,
                        ProductId = item.ProductId,
                        StockUnitId = item.StockUnitId,
                        Quantity = item.Quantity,
                        BaseQuantity = item.BaseQuantity,
                        BaseUnitId = item.BaseUnitId,
                        ConversionQty = item.ConversionQty,
                        ProductOwnerId = item.ProductOwnerId,
                        ProductStatusId = item.ProductStatusId,
                        ProductSubStatusId = item.ProductSubStatusId,

                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        Remark = item.Remark
                    };

                    dispatchDetailService.Add(dispatchDetailModel);

                    int i = 1;
                    item.PalletCodes.ForEach(x =>
                    {
                        dispatchBookingService.Add(new DispatchBooking()
                        {
                            DispatchDetailId = dispatchDetailModel.DispatchDetailId,

                            Sequence = i,
                            ProductId = item.ProductId,
                            RequestQty = x.RequestQty,
                            RequestStockUnitId = x.RequestStockUnitId,
                            RequestBaseQty = x.RequestBaseQty,
                            RequestBaseUnitId = x.RequestBaseUnitId,
                            BookingQty = x.RequestQty,
                            BookingStockUnitId = x.RequestStockUnitId,
                            BookingBaseQty = x.RequestBaseQty,
                            BookingBaseUnitId = x.RequestBaseUnitId,

                            IsBackOrder = false,
                            BookingStatus = BookingStatusEnum.InternalReceive,
                            PalletCode = x.PalletCode,
                            LocationId = item.LocationId,
                            ProductLot = item.ProductLot,
                            ConversionQty = item.ConversionQty,
                            Mfgdate = x.Mfgdate,
                            ExpirationDate = x.ExpirationDate,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now,
                            Remark = item.Remark
                        });
                        i++;
                    });
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

    }
}
