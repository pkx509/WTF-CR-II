using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.MasterService.Secure;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.ProductionControlService
{
    public class ProductionControlService : Repository<ProductionControl>, IProductionControlService
    {
        private readonly string _powerUser = ConfigurationManager.AppSettings["powerUser"].ToString();
        private readonly string _powerUserNP = ConfigurationManager.AppSettings["powerUserNP"].ToString();
        private readonly string _powerUserSP = ConfigurationManager.AppSettings["powerUserSP"].ToString();

        private readonly IRepository<Line> _LineService;
        private readonly IRepository<ProductUnit> _UnitService;
        private readonly IRepository<Product> _ProductService;
        private readonly IRepository<ProductCodes> _ProductCodesService;
        private readonly IRepository<ProductionControlDetail> _PCDetailService;
        private readonly IRepository<Receiving> _ReceivingService;
        private readonly IRepository<ReceiveDetail> _ReceiveDetailService;
        private readonly IRepository<Zone> _ZoneService;
        private readonly IRepository<Location> _LocationService;
        private readonly IRepository<ProductSubStatus> _PSubStatusService;
        private readonly IRepository<ProductStatus> _PStatusService;
        private readonly IRepository<Picking> _PickingService;
        private readonly IRepository<PickingAssign> _PickingAssignService;
        private readonly IRepository<PickingDetail> _PickingDetailService;
        private readonly IRepository<UserAccounts> _UserAccountService;

        private readonly IUserAccountService _IUserAccountService;

        public ProductionControlService(IUnitOfWork context, IUserAccountService userAccountService) : base(context)
        {
            _LineService = context.Repository<Line>();
            _UnitService = context.Repository<ProductUnit>();
            _ProductService = context.Repository<Product>();
            _ProductCodesService = context.Repository<ProductCodes>();
            _PCDetailService = context.Repository<ProductionControlDetail>();
            _ReceivingService = context.Repository<Receiving>();
            _ReceiveDetailService = context.Repository<ReceiveDetail>();
            _ZoneService = context.Repository<Zone>();
            _LocationService = context.Repository<Location>();
            _PSubStatusService = context.Repository<ProductSubStatus>();
            _PickingService = context.Repository<Picking>();
            _PickingAssignService = context.Repository<PickingAssign>();
            _PickingDetailService = context.Repository<PickingDetail>();
            _PStatusService = context.Repository<ProductStatus>();
            _UserAccountService = context.Repository<UserAccounts>();

            _IUserAccountService = userAccountService;
        }

        public List<PC_PackingModel> GetAllPacking(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                DateTime tmpDate = DateTime.Now;

                IEnumerable<Line> line = _LineService.Query().Get();
                IEnumerable<ProductUnit> unit = _UnitService.Query().Get();
                IEnumerable<Product> product = _ProductService.Query().Include(x => x.CodeCollection).Get();

                if (planDate != null)
                {
                    if (!DateTime.TryParse(planDate.Value.ToString("yyyy/MM/dd 00:00:00"), out tmpDate))
                    {
                        throw new HILIException("MSG00005");
                    }
                }

                IEnumerable<PC_PackingModel> result = from pc in Query().Filter(x => x.IsActive
                                             && (planDate != null ? x.ProductionDate == tmpDate : true)
                                             && (lineID != null ? x.LineID == lineID : true)).Get()
                                                      join p in product on pc.ProductID equals p.ProductID into _p
                                                      from p in _p.DefaultIfEmpty()
                                                      join l in line on pc.LineID equals l.LineID into _l
                                                      from l in _l.DefaultIfEmpty()
                                                      join u in unit on pc.StockUnitID equals u.ProductUnitID into _u
                                                      from u in _u.DefaultIfEmpty()
                                                      where l.LineType == lineType.ToString() && pc.PcControlStatus != (int)PCControlStatusEnum.Complete
                                                      select (new PC_PackingModel()
                                                      {
                                                          ControlID = pc.ControlID,
                                                          ProductCode = p?.CodeCollection.Where(x => x.CodeType == ProductCodeTypeEnum.Stock).FirstOrDefault()?.Code,
                                                          ProductID = p?.ProductID,
                                                          ProductName = p?.Name,
                                                          LineID = l?.LineID,
                                                          LineCode = l?.LineCode,
                                                          OrderNo = pc.OrderNo,
                                                          QTY = pc.Quantity,
                                                          UnitID = u?.ProductUnitID,
                                                          Unit = u?.Name
                                                      });

                totalRecords = 0;

                totalRecords = result.Count();


                if (totalRecords == 0)
                {
                    return new List<PC_PackingModel>();
                }

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.ToList();
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

        public List<PC_PackedModel> GetAllPacked(LineTypeEnum lineType, DateTime? planDate, Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                DateTime tmpDate = DateTime.Now;

                IEnumerable<Line> line = _LineService.Query().Get();
                IEnumerable<ProductUnit> unit = _UnitService.Query().Get();
                IEnumerable<Product> product = _ProductService.Query().Include(x => x.CodeCollection).Get();

                if (planDate != null)
                {
                    if (!DateTime.TryParse(planDate.Value.ToString("yyyy/MM/dd 00:00:00"), out tmpDate))
                    {
                        throw new HILIException("MSG00005");
                    }
                }

                IEnumerable<PC_PackedModel> result = from pc in Query().Filter(x => x.IsActive
                                           && (planDate != null ? x.ProductionDate == tmpDate : true)
                                           && (lineID != null ? x.LineID == lineID : true)).Include(x => x.PCDetailCollection).Get()
                                                     join p in product on pc.ProductID equals p.ProductID into _p
                                                     from p in _p.DefaultIfEmpty()
                                                     join l in line on pc.LineID equals l.LineID into _l
                                                     from l in _l.DefaultIfEmpty()
                                                     join u in unit on pc.StockUnitID equals u.ProductUnitID into _u
                                                     from u in _u.DefaultIfEmpty()
                                                     where (pc.PCDetailCollection != null && pc.PCDetailCollection.Count() > 0) && l.LineType == lineType.ToString()
                                                     && true == (string.IsNullOrEmpty(keyword) ? true : pc.PCDetailCollection.Any(d => d.PalletCode == keyword))
                                                     select new PC_PackedModel()
                                                     {
                                                         ControlID = pc.ControlID,
                                                         ProductCode = p?.CodeCollection.Where(x => x.CodeType == ProductCodeTypeEnum.Stock).FirstOrDefault()?.Code,
                                                         ProductID = p?.ProductID,
                                                         ProductName = p?.Name,
                                                         LineID = l?.LineID,
                                                         LineCode = l?.LineCode,
                                                         OrderNo = pc.OrderNo,
                                                         QTY = pc.Quantity,
                                                         UnitID = u?.ProductUnitID,
                                                         Unit = u?.Name,
                                                         CompleteQTY = pc.PCDetailCollection.Sum(x => x.StockQuantity),
                                                         StartTime = pc.PCDetailCollection.Min(x => x.MFGTimeStart),
                                                         EndTime = pc.PCDetailCollection.Max(x => x.MFGTimeEnd)
                                                     };

                totalRecords = 0;

                totalRecords = result.Count();


                if (totalRecords == 0)
                {
                    return new List<PC_PackedModel>();
                }

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.ToList();
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

        public List<PC_PackedModel> GetRePrintList(Guid controlID, bool isProduction)
        {
            try
            {
                IEnumerable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<Line> line = _LineService.Query().Get();
                IEnumerable<ProductUnit> unit = _UnitService.Query().Get();
                IEnumerable<Product> product = _ProductService.Query().Include(x => x.CodeCollection).Get();

                IEnumerable<PC_PackedModel> result = from pc in Query().Filter(x => x.IsActive).Get()
                                                     join pcd in pcDetail on pc.ControlID equals pcd.ControlID
                                                     join p in product on pc.ProductID equals p.ProductID into _p
                                                     from p in _p.DefaultIfEmpty()
                                                     join l in line on pc.LineID equals l.LineID into _l
                                                     from l in _l.DefaultIfEmpty()
                                                     join u in unit on pc.StockUnitID equals u.ProductUnitID into _u
                                                     from u in _u.DefaultIfEmpty()
                                                     where pc.ControlID == controlID && (true == (isProduction ? (!isProduction == (pcd.IsNonProduction ?? false)) : true))
                                                     orderby pcd.Sequence
                                                     select new PC_PackedModel()
                                                     {
                                                         PackingID = pcd.PackingID,
                                                         ControlID = pcd.ControlID,
                                                         PalletCode = pcd.PalletCode,
                                                         Sequence = pcd.Sequence,
                                                         LineID = l?.LineID,
                                                         LineCode = l?.LineCode,
                                                         ProductID = p?.ProductID,
                                                         ProductCode = p?.Code,
                                                         ProductName = p?.Name,
                                                         UnitID = u?.ProductUnitID,
                                                         Unit = u?.Name,
                                                         RemainQTY = (pcd.RemainQTY ?? 0) - (pcd.ReserveQTY ?? 0),
                                                         CompleteQTY = pcd.StockQuantity,
                                                         MFGTime = pcd.MFGTimeStart.Value.ToString(@"hh\:mm") + "-" + pcd.MFGTimeEnd.Value.ToString(@"hh\:mm")
                                                     };

                if (result.Count() == 0)
                {
                    return new List<PC_PackedModel>();
                }

                return result.ToList();
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

        public PalletInfoModel GetPalletInfo(string palletCode, string oldPalletCode, Guid oldProductID, decimal orderQTY)
        {
            if (palletCode == null)
            {
                throw new HILIException("MSG00006"); // Data Not Found
            }

            ProductionControlDetail oldPCDetail = _PCDetailService.Query().Filter(x => oldPalletCode != null ? x.PalletCode == oldPalletCode : true).Include(x => x.ProductionControl).Get().FirstOrDefault();
            IEnumerable<ProductionControlDetail> pcDetails = _PCDetailService.Query().Filter(x => (palletCode != null ? x.PalletCode == palletCode : true) &&
                                    x.PackingStatus != PackingStatusEnum.Waiting_Receive &&
                                    x.PackingStatus != PackingStatusEnum.Loading_In &&
                                    x.PackingStatus != PackingStatusEnum.In_Progress &&
                                    x.PackingStatus != PackingStatusEnum.Transfer &&
                                    x.PackingStatus != PackingStatusEnum.Cancel).Include(x => x.ProductionControl).Get();
            IEnumerable<Location> location = _LocationService.Query().Filter(x => x.IsActive).Get();
            IEnumerable<ProductUnit> unit = _UnitService.Query().Filter(x => x.IsActive).Get();

            if (pcDetails == null || pcDetails.Count() == 0)
            {
                throw new HILIException("MSG00006"); // Data Not Found
            }
            //return new PalletInfoModel();

            ProductionControlDetail pcDetail = pcDetails.FirstOrDefault();
            Guid? productID = pcDetail.ProductionControl?.ProductID;
            decimal remainQTY = (pcDetail.RemainQTY ?? 0) - (pcDetail.ReserveQTY ?? 0);



            Guid? unitID = pcDetail.ProductionControl?.StockUnitID;
            Guid? oldUnitID = oldPCDetail.ProductionControl?.StockUnitID;


            Guid whID = _LocationService.Query().Filter(x => x.IsActive && x.LocationID == pcDetail.LocationID).Include(x => x.Zone).Get().FirstOrDefault().Zone.WarehouseID;
            Guid oldWhID = _LocationService.Query().Filter(x => x.IsActive && x.LocationID == oldPCDetail.LocationID).Include(x => x.Zone).Get().FirstOrDefault().Zone.WarehouseID;

            if (pcDetail.ProductStatusID != oldPCDetail.ProductStatusID)
            {
                throw new HILIException("MSG00071"); // Should not select different Product Status
            }

            if (productID == null)
            {
                throw new HILIException("MSG00006"); // Data Not Found
            }

            if (productID.Value != oldProductID)
            {
                throw new HILIException("MSG00068"); // Should not select different Product
            }

            if (unitID.Value != oldUnitID)
            {
                throw new HILIException("MSG00105"); // Should not select different Unit
            }

            if (whID != oldWhID)
            {
                throw new HILIException("MSG00106"); // Should not select different warehouse
            }

            if (orderQTY > remainQTY)
            {
                throw new HILIException("MSG00039"); // Quanlity Over Stock
            }

            PalletInfoModel result = (from pcd in pcDetails
                                      join l in location on pcd.LocationID equals l.LocationID
                                      join u in unit on pcd.RemainStockUnitID equals u.ProductUnitID into _u
                                      from u in _u.DefaultIfEmpty()
                                      select new PalletInfoModel()
                                      {
                                          PackingID = pcd.PackingID,
                                          Lot = pcd.LotNo,
                                          LocationID = l.LocationID,
                                          Location = l.Code,
                                          RemainStockQTY = pcd.RemainQTY ?? 0,
                                          RemainStockUnitID = pcd.RemainStockUnitID,
                                          RemainStockUnit = u.Name
                                      }).FirstOrDefault();

            if (result == null)
            {
                return new PalletInfoModel();
            }

            return result;
        }

        public PC_PackingModel GetByID(Guid controlID)
        {
            try
            {
                IEnumerable<Line> line = _LineService.Where(e => e.IsActive).ToList();
                IEnumerable<ProductUnit> unit = _UnitService.Where(e => e.IsActive).ToList();
                IEnumerable<Product> product = _ProductService.Query().Include(x => x.CodeCollection).Get();
                var tmps = (from pc in Query().Filter(x => x.ControlID == controlID).Include(x => x.PCDetailCollection).Get()
                            join p in product on pc.ProductID equals p.ProductID into _p
                            from p in _p.DefaultIfEmpty()
                            join l in line on pc.LineID equals l.LineID into _l
                            from l in _l.DefaultIfEmpty()
                            join bu in unit on pc.BaseUnitID equals bu.ProductUnitID into _bu
                            from bu in _bu.DefaultIfEmpty()
                            join u in unit on pc.StockUnitID equals u.ProductUnitID into _u
                            from u in _u.DefaultIfEmpty()
                            select new { pc, p, l, bu, u }).ToList();

                IEnumerable<PC_PackingModel> result = (from ti in tmps
                                                      select new PC_PackingModel()
                                                      {
                                                          ControlID = ti.pc.ControlID,
                                                          ProductCode = ti.p?.CodeCollection.Where(x => x.CodeType == ProductCodeTypeEnum.Stock).FirstOrDefault()?.Code,
                                                          ProductID = ti.p?.ProductID,
                                                          ProductName = ti.p?.Name,
                                                          LineID = ti.l?.LineID,
                                                          LineCode = ti.l?.LineCode,
                                                          OrderNo = ti.pc.OrderNo,
                                                          OrderType = ti.pc.OrderType,
                                                          QTY = ti.pc.Quantity,
                                                          StdPalletQTY = ti.pc.StandardPalletQty == 0 ? 1.0M : ti.pc.StandardPalletQty,
                                                          ConversionQTY = ti.pc.ConversionQty,
                                                          PalletCount = ti.pc.PCDetailCollection.Where(x => (x.IsNonProduction ?? false) == false)?.Count(),
                                                          UnitID = ti.u?.ProductUnitID,
                                                          Unit = ti.u?.Name,
                                                          BaseUnitID = ti.bu?.ProductUnitID,
                                                          BaseUnit = ti.bu?.Name,
                                                          StartDate = ti.pc.ProductionDate.Date,
                                                          PackingID = ti.pc.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.PackingID,
                                                          StartTime = ti.pc.PCDetailCollection.Where(x => (x.IsNonProduction ?? false) == false).OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? DateTime.Now.TimeOfDay,
                                                          IsLastestPallet = (Math.Ceiling((ti.pc.Quantity / (ti.pc.StandardPalletQty == 0 ? 1.0M : ti.pc.StandardPalletQty))) <= ti.pc.PCDetailCollection.Where(x => (x.IsNonProduction ?? false) == false)?.Count() + 1) ? true : false
                                                      });
                if (result == null || !result.Any())
                {
                    throw new HILIException("MSG00006");
                }

                return result.FirstOrDefault();
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

        public PC_PackingModel PrintPalletTag(PrintPalletModel model)
        {
            try
            {

                Guid controlID = model.ControlID;


                ProductionControl tmpPC = base.FindByID(controlID);

                if (tmpPC == null || tmpPC.PcControlStatus == (int)PCControlStatusEnum.Complete)
                {
                    throw new HILIException("MSG00097");
                }

                SqlParameter param = new SqlParameter("@controlID", SqlDbType.UniqueIdentifier) { Value = model.ControlID };
                SqlParameter param2 = new SqlParameter("@lineCode", SqlDbType.NVarChar) { Value = model.LineCode };
                SqlParameter param3 = new SqlParameter("@Suffix", SqlDbType.NVarChar) { Value = model.Suffix ?? "" };
                SqlParameter param4 = new SqlParameter("@OptionalSuffix", SqlDbType.NVarChar) { Value = model.OptionalSuffix ?? "" };
                SqlParameter param5 = new SqlParameter("@ProductStatusID", SqlDbType.UniqueIdentifier) { Value = model.ProductStatusID ?? Guid.Empty };
                SqlParameter param6 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };


                bool isSuccess = Context.SQLQuery<bool>("exec SP_PrintPalletTag @controlID,@lineCode,@Suffix,@OptionalSuffix,@ProductStatusID,@UserID", param, param2, param3, param4, param5, param6).SingleOrDefault();

                PC_PackingModel result = null;
                if (isSuccess)
                {
                    result = GetByID(controlID);
                }
                else
                {
                    throw new HILIException("MSG00006");
                }

                return result;
            }
            catch (HILIException ex)
            {
                //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
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
        
        public PC_PackingModel PrintPalletTagXX(PrintPalletModel model)
        {
            try
            {
                CultureInfo cultureinfo = new CultureInfo("en-US");
                //DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd", cultureinfo));

                Guid controlID = model.ControlID;
                string lineCode = model.LineCode;
                decimal QTYperPallet = 0.0M;

                using (TransactionScope scope = new TransactionScope())
                {

                    ProductionControl tmpPC = base.FindByID(controlID);
                    decimal sumStockQTY = _PCDetailService.Query().Filter(x => x.IsActive && (x.IsNormal ?? false) && x.ControlID == controlID).Get().Sum(x => x.StockQuantity) ?? 0;

                    if (tmpPC != null && tmpPC.PcControlStatus != (int)PCControlStatusEnum.Complete)
                    {
                        if ((tmpPC.StandardPalletQty + sumStockQTY) > tmpPC.Quantity)
                        {
                            QTYperPallet = tmpPC.Quantity - sumStockQTY;
                        }
                        else
                        {
                            QTYperPallet = tmpPC.StandardPalletQty;
                        }

                        // print all pallet completely
                        if ((sumStockQTY + QTYperPallet) == tmpPC.Quantity)
                        {
                            tmpPC.PcControlStatus = (int)PCControlStatusEnum.Complete;
                            tmpPC.UserModified = UserID;
                            tmpPC.DateModified = DateTime.Now;
                            base.Modify(tmpPC);
                        }
                    }
                    else
                    {
                        throw new HILIException("MSG00097"); // Data Not Found
                    }

                    IEnumerable<ProductUnit> unit = _UnitService.Query().Get();
                    IEnumerable<Line> line = _LineService.Query().Get();
                    IEnumerable<ReceiveDetail> rcvDetail = _ReceiveDetailService.Query().Include(x => x.Receive).Get();
                    Guid? pSubStatusID = _PSubStatusService.Query().Filter(x => x.Code == "SS000").Get().FirstOrDefault()?.ProductSubStatusID;
                    TimeSpan timeStamp = DateTime.Now.TimeOfDay;
                    List<Guid> PCDetailID = new List<Guid>();

                    IEnumerable<ProductionControlDetail> PCDetail = from pc in Query().Filter(x => x.ControlID == controlID).Include(x => x.PCDetailCollection).Get()
                                                                    join l in line on pc.LineID equals l.LineID into _l
                                                                    from l in _l.DefaultIfEmpty()
                                                                    join rd in rcvDetail on pc.ReferenceID equals rd.ReceiveDetailID into _rd
                                                                    from rd in _rd.DefaultIfEmpty()
                                                                    select new ProductionControlDetail()
                                                                    {
                                                                        ControlID = pc.ControlID,
                                                                        PalletCode = pc.ProductionDate.ToString("yyyyMMdd", cultureinfo)
                                                                                         + lineCode
                                                                                         + ((pc.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1).ToString("000")
                                                                                         + timeStamp.ToString("hhmmss") + model.Suffix,
                                                                        LotNo = pc.ProductionDate.ToString("yyyyMMdd", cultureinfo) + model.Suffix,
                                                                        Sequence = ((pc.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1),
                                                                        OptionalSuffix = l.LineType == "SP" ? model.OptionalSuffix : ((pc.PCDetailCollection.Max(x => x.Sequence) ?? 0) + 1).ToString(),
                                                                        StockQuantity = QTYperPallet,
                                                                        BaseQuantity = (QTYperPallet * pc.ConversionQty),
                                                                        ConversionQty = pc.ConversionQty,
                                                                        StockUnitID = pc.StockUnitID,
                                                                        BaseUnitID = pc.BaseUnitID,
                                                                        ProductStatusID = model.ProductStatusID ?? rd.ProductStatusID,
                                                                        ProductSubStatusID = pSubStatusID,
                                                                        MFGDate = pc.ProductionDate,
                                                                        MFGTimeStart = pc.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                                                        MFGTimeEnd = timeStamp,
                                                                        PackingStatus = PackingStatusEnum.Waiting_Receive,
                                                                        WarehouseID = l.WarehouseID,
                                                                        ReceiveDetailID = rd.ReceiveDetailID,
                                                                        LocationID = rd.Receive.LocationID,
                                                                        ReserveQTY = 0,
                                                                        ReserveBaseQTY = 0,

                                                                        IsNormal = string.IsNullOrWhiteSpace(model.Suffix),
                                                                        IsActive = true,
                                                                        UserCreated = UserID,
                                                                        DateCreated = DateTime.Now,
                                                                        UserModified = UserID,
                                                                        DateModified = DateTime.Now
                                                                    };

                    if (PCDetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    if (string.IsNullOrWhiteSpace(model.Suffix))
                    {
                        ReceiveDetailEdit(PCDetail);
                    }

                    ReceivingAdd(controlID, PCDetail);
                    _PCDetailService.AddRange(PCDetail);

                    scope.Complete();
                }

                PC_PackingModel result = GetByID(controlID);
                return result;
            }
            catch (HILIException ex)
            {
                //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
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

        public CancelPalletModel CancelPallet(CancelPalletModel model)
        {
            try
            {
                CultureInfo cultureinfo = new CultureInfo("en-US");

                using (TransactionScope scope = new TransactionScope())
                {
                    _IUserAccountService.UserID = UserID;
                    if (_IUserAccountService.Login(model.UserName, model.Password) && (model.UserName == _powerUser || model.UserName == _powerUserNP || model.UserName == _powerUserSP))
                    {
                        ProductionControlDetail packingService = _PCDetailService.Query().Filter(x => x.IsActive && (x.IsNormal ?? false) && x.PackingID == model.PackingID).Get().FirstOrDefault();
                        if (packingService == null)
                        {
                            throw new HILIException("MSG00011"); // The user name or password is incorrect.
                        }

                        if (_ReceivingService.Query().Filter(x => x.IsActive && x.PalletCode == packingService.PalletCode && x.ReceivingStatus != ReceivingStatusEnum.Inprogress).Get().Any())
                        {
                            throw new HILIException("MSG00093"); // cant cancel. pallet is in the putaway process..
                        }

                        if (Query().Filter(x => x.ControlID == packingService.ControlID && x.PcControlStatus != (int)PCControlStatusEnum.Complete).Get().Count() > 0)
                        {
                            throw new HILIException("MSG00087"); // Please complete receive first
                        }

                        Guid? userID = _UserAccountService.Query().Filter(x => x.UserName == model.UserName).Get().FirstOrDefault()?.UserID;

                        packingService.IsActive = false;
                        packingService.PackingStatus = PackingStatusEnum.Cancel;
                        packingService.UserModified = userID ?? Guid.Empty;
                        packingService.DateModified = DateTime.Now;
                        _PCDetailService.Modify(packingService);

                        model.ControlID = packingService.ControlID;
                    }
                    else
                    {
                        throw new HILIException("MSG00011"); // The user name or password is incorrect.
                    }

                    scope.Complete();
                }

                return model;
            }
            catch (HILIException ex)
            {
                //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
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

        private void ReceiveDetailEdit(IEnumerable<ProductionControlDetail> PCDetail)
        {
            try
            {
                var groupIDs = PCDetail.GroupBy(g => new { g.ReceiveDetailID }).Select(n => new
                {
                    n.Key.ReceiveDetailID
                });

                foreach (var groupID in groupIDs)
                {
                    ReceiveDetail rDetail = _ReceiveDetailService.FindByID(groupID.ReceiveDetailID);
                    rDetail.Lot = PCDetail.FirstOrDefault()?.LotNo;
                    rDetail.UserModified = UserID;
                    rDetail.DateModified = DateTime.Now;
                    _ReceiveDetailService.Modify(rDetail);
                }
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

        private void ReceivingAdd(Guid controlID, IEnumerable<ProductionControlDetail> pcds)
        {
            try
            {
                IEnumerable<Line> line = _LineService.Query().Get();
                IEnumerable<ReceiveDetail> rcvDetail = _ReceiveDetailService.Query().Include(x => x.Receive).Get();
                IEnumerable<Zone> zone = _ZoneService.Query().Get();
                IEnumerable<Location> location = _LocationService.Query().Get();

                foreach (ProductionControlDetail pcd in pcds)
                {
                    IEnumerable<Receiving> receiving = from pc in Query().Filter(x => x.ControlID == controlID).Get()
                                                       join rcd in rcvDetail on pc.ReferenceID equals rcd.ReceiveDetailID into _rcd
                                                       from rcd in _rcd.DefaultIfEmpty()
                                                       select new Receiving()
                                                       {
                                                           GRNCode = rcd.Receive.ReceiveCode + pcd.Sequence.ToString(),
                                                           ReceiveID = rcd.ReceiveID,
                                                           Sequence = pcd.Sequence ?? 1,
                                                           ReceiveDetailID = rcd.ReceiveDetailID,
                                                           IsDraft = true,
                                                           ReceivingStatus = ReceivingStatusEnum.Inprogress,
                                                           ProductID = rcd.ProductID,
                                                           Lot = rcd.Lot,
                                                           ManufacturingDate = rcd.ManufacturingDate,
                                                           ExpirationDate = rcd.ExpirationDate,
                                                           Quantity = pcd.StockQuantity ?? 0,
                                                           BaseQuantity = pcd.BaseQuantity ?? 0,
                                                           ConversionQty = pcd.ConversionQty ?? 1,
                                                           StockUnitID = pcd.StockUnitID.Value,
                                                           BaseUnitID = pcd.BaseUnitID.Value,
                                                           ProductStatusID = pcd.ProductStatusID.Value,
                                                           ProductSubStatusID = pcd.ProductSubStatusID,
                                                           PackageWeight = 1,
                                                           ProductWeight = 1,
                                                           ProductWidth = 1,
                                                           ProductLength = 1,
                                                           ProductHeight = 1,
                                                           PalletCode = pcd.PalletCode,
                                                           //LocationID = (from l in line
                                                           //              join z in zone on l.WarehouseID equals z.WarehouseID
                                                           //              join loc in location on z.ZoneID equals loc.ZoneID
                                                           //              where l.LineID == pc.LineID && loc.LocationType == LocationTypeEnum.LoadingIN
                                                           //              select loc.LocationID).FirstOrDefault(),
                                                           LocationID = rcd.Receive.LocationID,
                                                           ProductOwnerID = rcd.Receive.ProductOwnerID,
                                                           SupplierID = rcd.Receive.SupplierID,

                                                           IsActive = true,
                                                           IsSentInterface = false,
                                                           UserCreated = UserID,
                                                           DateCreated = DateTime.Now,
                                                           UserModified = UserID,
                                                           DateModified = DateTime.Now
                                                       };

                    _ReceivingService.AddRange(receiving);
                }

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

        public void ModifyProductionDetail(ProductionControlDetail entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    _PCDetailService.Modify(entity);

                    scope.Complete();
                }

            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<PC_PackedModel> GetRePrintOutboundList(DateTime? MFGDate, string productName, string PONo, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                DateTime tmpDate = DateTime.Now;

                IQueryable<ProductionControl> controls = Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Picking> pickings = _PickingService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<PickingAssign> assigns = _PickingAssignService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<PickingDetail> pDetails = _PickingDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductionControlDetail> pcDetails = _PCDetailService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Product> products = _ProductService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductCodes> productCodes = _ProductCodesService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<ProductUnit> units = _UnitService.Query().Filter(x => x.IsActive).GetQueryable();
                IQueryable<Line> lines = _LineService.Query().Filter(x => x.IsActive).GetQueryable();

                if (MFGDate != null)
                {
                    if (!DateTime.TryParse(MFGDate.Value.ToString("yyyy/MM/dd 00:00:00"), out tmpDate))
                    {
                        throw new HILIException("MSG00005");
                    }
                }

                IQueryable<PC_PackedModel> result = from pk in pickings
                                                    join pa in assigns on pk.PickingID equals pa.PickingID
                                                    join pd in pDetails on pa.AssignID equals pd.AssignID
                                                    join pcd in pcDetails on pa.PalletCode equals pcd.PalletCode
                                                    join pc in controls on pcd.ControlID equals pc.ControlID
                                                    join l in lines on pc.LineID equals l.LineID
                                                    join p in products on pa.ProductID equals p.ProductID
                                                    join c in productCodes on p.ProductID equals c.ProductID
                                                    join u in units on pa.PalletUnitID equals u.ProductUnitID
                                                    where pk.PONo == PONo && c.CodeType == ProductCodeTypeEnum.Stock
                                                              && (MFGDate != null ? pcd.MFGDate == tmpDate : true)
                                                              && (productName != null ? p.Name.Contains(productName) : true)
                                                    select new PC_PackedModel()
                                                    {
                                                        PalletCode = pa.PalletCode,
                                                        ProductCode = c.Code,
                                                        ProductID = p.ProductID,
                                                        ProductName = p.Name,
                                                        UnitID = u.ProductUnitID,
                                                        Unit = u.Name,
                                                        PickingDetailID = pd.PickingDetailID,
                                                        DOQty = pd.PickStockQty,
                                                        RemainQTY = (pcd.RemainQTY - pcd.ReserveQTY),
                                                        MFGDate = pcd.MFGDate,
                                                        PackingID = pcd.PackingID,
                                                        LineID = l.LineID,
                                                        LineCode = l.LineCode,
                                                    };

                if (result.Count() == 0)
                {
                    return new List<PC_PackedModel>();
                }

                totalRecords = result.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.PalletCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.ToList();

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

        public List<PC_PackedModel> GetPalletList(Guid receiveDetailId)
        {
            try
            {
                IEnumerable<ProductionControlDetail> pcDetail = _PCDetailService.Query().Get();
                IEnumerable<Line> line = _LineService.Query().Get();
                IEnumerable<ProductUnit> unit = _UnitService.Query().Get();
                IEnumerable<Product> product = _ProductService.Query().Include(x => x.CodeCollection).Get();

                IEnumerable<PC_PackedModel> result = from pc in Query().Filter(x => x.IsActive).Get()
                                                     join pcd in pcDetail on pc.ControlID equals pcd.ControlID
                                                     join s in _PStatusService.Query().Get() on pcd.ProductStatusID equals s.ProductStatusID
                                                     join p in product on pc.ProductID equals p.ProductID into _p
                                                     from p in _p.DefaultIfEmpty()
                                                     join l in line on pc.LineID equals l.LineID into _l
                                                     from l in _l.DefaultIfEmpty()
                                                     join u in unit on pc.StockUnitID equals u.ProductUnitID into _u
                                                     from u in _u.DefaultIfEmpty()
                                                     where pc.ReferenceID == receiveDetailId
                                                     orderby pcd.Sequence
                                                     select new PC_PackedModel()
                                                     {
                                                         PackingID = pcd.PackingID,
                                                         ControlID = pcd.ControlID,
                                                         PalletCode = pcd.PalletCode,
                                                         Sequence = pcd.Sequence,
                                                         LineID = l?.LineID,
                                                         LineCode = l?.LineCode,
                                                         ProductID = p?.ProductID,
                                                         ProductCode = p?.Code,
                                                         ProductName = p?.Name,
                                                         UnitID = u?.ProductUnitID,
                                                         Unit = u?.Name,
                                                         Lot = pcd.LotNo,
                                                         CompleteQTY = pcd.StockQuantity,
                                                         MFGTime = pcd.MFGTimeStart.Value.ToString(@"hh\:mm") + "-" + pcd.MFGTimeEnd.Value.ToString(@"hh\:mm"),
                                                         MFGDate = pcd.MFGDate,
                                                         ProductStatusName = s.Name
                                                     };


                if (result.Count() == 0)
                {
                    return new List<PC_PackedModel>();
                }

                return result.ToList();
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
    }
}
