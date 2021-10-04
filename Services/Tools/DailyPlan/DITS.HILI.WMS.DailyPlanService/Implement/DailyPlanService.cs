using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Infrastructure.Engine;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace DITS.HILI.WMS.DailyPlanService
{
    public class DailyPlanService : Repository<ProductionPlan>, IDailyPlanService
    {
        #region [ Property ] 
        private readonly IRepository<ProductionPlanDetail> _ProductionDetailService;
        private readonly IRepository<Product> _ProductService;
        private readonly IRepository<ProductUnit> _ProductUnitService;
        private readonly IRepository<Warehouse> _WarehouseService;
        private readonly IRepository<Period> _PeriodService;
        private readonly IRepository<Line> _LineService;
        private readonly IRepository<Zone> _ZoneService;
        private readonly IRepository<Location> _LocationService;
        private readonly IRepository<Receive> _ReceiveService;
        private readonly IRepository<ReceiveDetail> _ReceiveDetailService;
        private readonly IRepository<ReceivePrefix> _ReceivePrefixService;
        private readonly IRepository<DocumentType> _DocumentTypeService;
        private readonly IRepository<ProductOwner> _ProductOwnerService;
        private readonly IRepository<Contact> _ContactService;
        private readonly IRepository<ProductStatus> _ProductStatusService;
        private readonly IRepository<ProductSubStatus> _ProductSubStatusService;
        #endregion

        #region [ Constructor ]

        public DailyPlanService(IUnitOfWork context,
                                    IRepository<ProductionPlanDetail> _productionDetail,
                                    IRepository<Product> _product,
                                    IRepository<ProductUnit> _productUnit,
                                    IRepository<Warehouse> _warehouse,
                                    IRepository<Period> _period,
                                    IRepository<Line> _line,
                                    IRepository<ReceivePrefix> _receivePrefix,
                                    IRepository<DocumentType> _documentType,
                                    IRepository<ProductOwner> _productOwner,
                                    IRepository<ReceiveDetail> _receiveDetail) : base(context)
        {
            _ProductionDetailService = _productionDetail;
            _ProductService = _product;
            _ProductUnitService = _productUnit;
            _WarehouseService = _warehouse;
            _PeriodService = _period;
            _LineService = _line;
            _ReceivePrefixService = _receivePrefix;
            _DocumentTypeService = _documentType;
            _ProductOwnerService = _productOwner;
            _ReceiveService = context.Repository<Receive>();
            _ContactService = context.Repository<Contact>();
            _ReceiveDetailService = _receiveDetail;
            _LocationService = context.Repository<Location>();
            _ZoneService = context.Repository<Zone>();
            _ProductStatusService = context.Repository<ProductStatus>();
            _ProductSubStatusService = context.Repository<ProductSubStatus>();
        }

        #endregion

        public bool SaveData(ProductionPlanCustomModel data)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IEnumerable<ProductionPlanDetail> pDetails = _ProductionDetailService.Where(x => x.IsActive).ToList();

                    if (data.ProductionDetailID == Guid.Empty || data.ProductionDetailID == null)
                    {
                        #region Validate Seq

                        IEnumerable<ProductionPlan> xxx = Where(x => x.LineID == data.LineId
                                        && x.ProductionDate == data.ProductionDate).ToList();

                        bool yyy = (from pp in xxx
                                    join ppd in pDetails on pp.ProductionID equals ppd.ProductionID
                                    where ppd.Seq == data.Seq && ppd.ProductID == data.ProductID && ppd.IsActive
                                    select ppd).Any();

                        if (yyy)
                        {
                            //Data is duplicate
                            throw new HILIException("MSG00009");
                        }

                        #endregion

                        Add(FillData(data));
                    }
                    else
                    {
                        ProductionPlanDetail planDetail = _ProductionDetailService.FirstOrDefault(x => x.ProductionDetailID == data.ProductionDetailID);
                                            //.Include(x => x.ProductionPlan)
                                           // .Get().SingleOrDefault();

                        if (planDetail == null)
                        {
                            throw new HILIException("MSG00004");
                        }
                         
                        planDetail.Box = data.Box;
                        planDetail.DeliveryDate = data.DeliveryDate == DateTime.MinValue ? null : data.DeliveryDate;
                        planDetail.FD = data.FD;
                        planDetail.Film = data.Film;
                        planDetail.Formula = data.Formula;
                        planDetail.Mark = data.Mark;
                        planDetail.Oil = data.Oil;
                        planDetail.OilType = data.OilType;
                        planDetail.PalletQty = data.PalletQty;
                        planDetail.Powder = data.Powder;
                        planDetail.ProductionQty = data.ProductionQty;
                        planDetail.Remark = data.Remark;
                        planDetail.Seq = data.Seq;
                        planDetail.Stamp = data.Stamp;
                        planDetail.Weight_G = data.Weight_G;
                        planDetail.Sticker = data.Sticker;
                        planDetail.ProductID = data.ProductID;
                        planDetail.ProductUnitID = data.ProductUnitID;
                        planDetail.WorkingTime = data.WorkingTime;
                        planDetail.DateModified = DateTime.Now;
                        planDetail.UserModified = UserID;
                        _ProductionDetailService.Modify(planDetail);

                        ProductionPlan plan = FirstOrDefault(x => x.ProductionID == planDetail.ProductionID);
                        plan.UserModified = UserID;
                        plan.DateModified = DateTime.Now;
                        plan.LineID = data.LineId;
                        plan.ProductionDate = data.ProductionDate;
                        plan.OrderNo = data.OrderNo;
                        plan.OrderType = data.OrderType;
                        plan.DateModified = DateTime.Now;
                        plan.UserModified = UserID;                       
                        base.Modify(plan);

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
            //return true;
        }

        public bool ImportDailyPlan(List<ProductionPlanCustomModel> items)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (ProductionPlanCustomModel item in items)
                    {
                        Line line = _LineService.Query().Get().Where(x => x.LineCode == item.LineCode.Trim() && x.IsActive == true).FirstOrDefault();

                        ProductionPlan plan = new ProductionPlan
                        {
                            DailyPlanStatus = (int)DailyPlanStatusEnum.DailyPlan,
                            OrderNo = item.OrderNo,
                            LineID = line.LineID,
                            OrderType = item.OrderType,
                            ProductionDate = new DateTime(item.ProductionDate.Year, item.ProductionDate.Month, item.ProductionDate.Day),
                            Remark = item.Remark,
                        };

                        #region [ Check Dupplicate Product Code ] 
                        //DateTime _date = DateTime.Now.Date;
                        DateTime _date = new DateTime(item.ProductionDate.Year, item.ProductionDate.Month, item.ProductionDate.Day);

                        bool chkDup = Is_Dupplicate_Plan(_date, line.LineID, item.Seq);
                        if (chkDup)
                        {
                            throw new ArgumentNullException("ProductionPlan", "Order Seq Dupplicate.");
                        }
                        #endregion

                        //var product = _ProductService.Query().Include(x => x.CodeCollection).Get().Where(x => x.ProductCode == item.ProductCode).FirstOrDefault();
                        Product product = _ProductService.Query().Include(x => x.CodeCollection).Get().Where(x => x.CodeCollection.Any(y => y.Code == item.ProductCode)).FirstOrDefault();
                        ProductUnit unit = _ProductUnitService.Query().Get().Where(w => w.ProductID == product.ProductID && w.Name == item.ProductUnitName).FirstOrDefault();
                        int tempPalletQTY = (int)unit.PalletQTY;

                        if (item.PalletQty != null && item.PalletQty > 0)
                        {
                            tempPalletQTY = item.PalletQty.Value;
                        }

                        plan.ProductionPlanDetail.Add(new ProductionPlanDetail
                        {
                            Box = item.Box,
                            DeliveryDate = item.DeliveryDate,
                            FD = item.FD,
                            Film = item.Film,
                            Formula = item.Formula,
                            Mark = item.Mark,
                            Oil = item.Oil,
                            OilType = item.OilType,
                            PalletQty = tempPalletQTY,
                            Powder = item.Powder,
                            ProductionQty = item.ProductionQty,
                            Remark = item.Remark,
                            Seq = item.Seq,
                            Stamp = item.Stamp,
                            Weight_G = item.Weight_G,
                            Sticker = item.Sticker,
                            ProductID = product.ProductID,
                            ProductUnitID = unit.ProductUnitID,

                        });

                        Add(plan);
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

        public bool DeletePlan(List<Guid> items)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<Guid> _parents = new List<Guid>();
                    items.ForEach(id =>
                    {
                        ProductionPlanDetail _current = _ProductionDetailService.FindByID(id);
                        _current.IsActive = false;
                        _current.DateModified = DateTime.Now;
                        _current.UserModified = UserID;
                        _ProductionDetailService.Modify(_current);

                        _parents.Add(_current.ProductionID);
                    });

                    IEnumerable<ProductionPlan> plan = Query().Get().Where(x => _parents.Contains(x.ProductionID));

                    plan.ToList().ForEach(item =>
                    {
                        if (item.ProductionPlanDetail.Where(x => x.IsActive == true).Count() == 0)
                        {
                            item.IsActive = false;
                            base.Modify(item);
                        }
                    });
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

        public bool SendToReceive(List<ProductionPlanCustomModel> items)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    foreach (ProductionPlanCustomModel item in items)
                    {
                        ProductionPlan production = FirstOrDefault(y => y.ProductionID == item.ProductionID);

                        if (production.DailyPlanStatus == (int)DailyPlanStatusEnum.Receive)
                        {
                            throw new HILIException("MSG00086"); // Cannot sent to Receive this plan already send
                        }

                        production.DailyPlanStatus = (int)DailyPlanStatusEnum.Receive;
                        base.Modify(production);

                        Receive receive = FillReceiveData(item);
                        List<ProductionPlanCustomModel> productionDetails = new List<ProductionPlanCustomModel>
                        {
                            item
                        }; 
                        List<ReceiveDetail> receiveDetails = FillReceiveDetailData(productionDetails);
                        receive.ReceiveDetailCollection = receiveDetails;
                        _ReceiveService.Add(receive);
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

        private bool Is_Dupplicate_Plan(DateTime _datePlan, Guid? line, decimal? seq)
        {


            DateTime _date = new DateTime(_datePlan.Year, _datePlan.Month, _datePlan.Day);

            return (from plan in Query().Get().Where(x => x.ProductionDate == _date && x.LineID == line)
                    join plan_detail in _ProductionDetailService.Query().Get().Where(x => x.Seq == seq) on plan.ProductionID equals plan_detail.ProductionID
                    select new { plan_detail }).Any();



        }

        public override ProductionPlan Add(ProductionPlan entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.ProductionPlanDetail.ToList().ForEach(item =>
                    {
                        item.ProductionID = entity.ProductionID;
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                        item.UserCreated = UserID;
                        item.UserModified = UserID;
                    });

                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserCreated = UserID;
                    entity.UserModified = UserID;

                    ProductionPlan result = base.Add(entity);

                    scope.Complete();
                    return result;
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

        public override void Modify(ProductionPlan entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductionPlan _current = Query().Filter(x => x.ProductionID == entity.ProductionID)
                                          .Include(x => x.ProductionPlanDetail).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    ///Remove Item 
                    _current.ProductionPlanDetail.Where(x => x.IsActive).ToList()
                        .ForEach(item =>
                        {
                            bool exist = entity.ProductionPlanDetail.Any(x => x.ProductionDetailID == item.ProductionDetailID);
                            if (!exist)
                            {
                                item.IsActive = false;
                                item.DateModified = DateTime.Now;
                                item.UserModified = UserID;
                                _ProductionDetailService.Modify(item);
                            }
                        });

                    //Add new Item 
                    entity.ProductionPlanDetail.ToList()
                       .ForEach(item =>
                       {
                           Product product = _ProductService.Query().Filter(x => x.ProductID == item.ProductID).Get().FirstOrDefault();
                           if (product == null)
                           {
                               throw new HILIException("MSG00006");
                           }

                           if (item.ProductionDetailID == null || Utilities.IsZeroGuid(item.ProductionDetailID))
                           {
                               item.ProductionID = entity.ProductionID;
                               item.IsActive = true;
                               item.DateCreated = DateTime.Now;
                               item.DateModified = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               item.UserModified = entity.UserModified;
                               _ProductionDetailService.Add(item);
                           }
                           else
                           {
                               ProductionPlanDetail itemCurrent = _ProductionDetailService.Query().Filter(x => x.ProductionDetailID == item.ProductionDetailID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   throw new HILIException("MSG00006");
                               }

                               item.IsActive = true;
                               item.DateModified = DateTime.Now;
                               item.UserModified = entity.UserModified;
                               _ProductionDetailService.Modify(item);

                           }
                       });


                    entity.DateModified = DateTime.Now;

                    base.Modify(entity);
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

        public override void Remove(object id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductionPlan _current = FindByID(id);
                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.ProductionPlanDetail.ToList().ForEach(item =>
                    {
                        item.IsActive = false;
                        item.DateModified = DateTime.Now;
                        item.UserModified = UserID;
                        _ProductionDetailService.Modify(item);
                    });
                    _current.IsActive = _current.ProductionPlanDetail.Where(x => x.IsActive == true).Count() > 0;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

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

        public List<Location> GetLocationByLine(Guid lineID, LocationTypeEnum? locationType, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<Zone> zone = _ZoneService.Query().Get();
                IEnumerable<Location> location = _LocationService.Query().Get();

                keyword = (string.IsNullOrEmpty(keyword) ? string.Empty : keyword);

                IEnumerable<Location> result = from pl in _LineService.Query().Filter(x => x.LineID == lineID).Get()
                                               join z in zone on pl.WarehouseID equals z.WarehouseID
                                               join l in location on z.ZoneID equals l.ZoneID
                                               where l.LocationType == locationType && (keyword == string.Empty ? true : l.Code.Contains(keyword))
                                               select new Location()
                                               {
                                                   LocationID = l.LocationID,
                                                   Code = l.Code
                                               };

                totalRecords = result.Count();

                if (totalRecords == 0)
                {
                    return new List<Location>();
                }

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<ValidationImportFileResult> ValidateImportDailyPlan(List<ProductionPlanCustomModel> items)
        {
            try
            {
                List<ValidationImportFileResult> _results = new List<ValidationImportFileResult>();
                using (TransactionScope scope = new TransactionScope())
                {//var xx = 0;
                    foreach (ProductionPlanCustomModel item in items)
                    {
                        //xx +=1; 
                        #region [ Get Product ]
                        Product _getProduct = _ProductService.Query().Include(x => x.CodeCollection).Get().Where(x => x.IsActive && x.CodeCollection.Any(y => y.Code == item.ProductCode)).FirstOrDefault();
                        //strerror += "=>1." + xx.ToString();
                        #endregion [ Get Product ]

                        #region [ Seq ]

                        if (item.Seq != null)
                        {
                            if (item.Seq.Value < 1)
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Order Seq less than 1.", "Seq", 2));

                            }
                        }
                        //strerror += "=>2.";
                        #endregion [ Line ]

                        #region [ Line ]

                        if (string.IsNullOrWhiteSpace(item.LineCode))
                        {
                            _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Line not found.", "LineCode", 1));
                        }
                        else
                        {
                            Line line = _LineService.Query().Get().Where(x => x.LineCode == item.LineCode.Trim() && x.IsActive == true).FirstOrDefault();
                            if (line == null)
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Line not found.", "LineCode", 1));
                            }
                            else
                            {
                                item.LineId = line.LineID;
                            }
                        }
                        //strerror += "=>2.";
                        #endregion [ Line ]

                        #region [ Product Code ]
                        if (string.IsNullOrWhiteSpace(item.ProductCode))
                        {
                            _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Product Code not found.", "ProductCode", 3));
                        }
                        else
                        {
                            if (_getProduct == null)
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Product Code not found.", "ProductCode", 3));
                            }
                            else
                            {
                                #region [ Check Dupplicate Product Code ] 
                                //DateTime _date = DateTime.Now.Date;
                                DateTime _date = new DateTime(item.ProductionDate.Year, item.ProductionDate.Month, item.ProductionDate.Day);

                                bool chkDup = Is_Dupplicate_Plan(_date, item.LineId, item.Seq);
                                if (chkDup)
                                {
                                    _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Order Seq Dupplicate.", "Seq", 2));
                                }
                                #endregion
                            }
                        }
                        //strerror += "=>3.";
                        #endregion [ Product Code ]

                        #region [ Qty and Weight ]

                        if (Convert.ToInt32(item.ProductionQty) == 0)
                        {
                            _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Qty not found.", "ProductionQty", 5));
                        }
                        //if (Convert.ToInt32(item.Weight_G) == 0)
                        //{
                        //    _results.Add(this.AddValidationDetailResult(item.RowIndex.Value, false, "Order Seq not found.", "Seq", 5));
                        //}  
                        #endregion [ Qty and Weight ]

                        #region [ Unit ]

                        if (string.IsNullOrWhiteSpace(item.ProductUnitName))
                        {
                            _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Unit not found. ", "ProductUnitName", 6));
                        }
                        else
                        {
                            if (_getProduct == null)
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Product Code not map Unit.", "ProductUnitName", 6));
                            }
                            else if (_ProductUnitService.Query().Get().Where(w => w.IsActive && w.ProductID == _getProduct.ProductID && w.Name == item.ProductUnitName.ToUpper()).FirstOrDefault() == null)
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Product Code not map Unit.", "ProductUnitName", 6));
                            }
                        }
                        //strerror += "=>6.";
                        #endregion [ Unit ]

                        #region [ Order Type ]

                        if (string.IsNullOrWhiteSpace(item.OrderType))
                        {
                            _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Order Type not found.", "OrderType", 9));
                        }
                        else
                        {
                            if (!Enum.TryParse(item.OrderType, out OrderTypeStatusEnum type))
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Order Type not found.", "OrderType", 9));
                            }

                        }
                        #endregion

                        #region [OrderNo]

                        if (item.OrderType == "EXPORT")
                        {
                            if (string.IsNullOrWhiteSpace(item.OrderNo))
                            {
                                _results.Add(AddValidationDetailResult(item.RowIndex.Value, false, "Require Order no in Case Export", "OrderNo", 8));
                            }
                        }

                        #endregion

                    }
                    return _results;
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

        public ProductionPlanCustomModel GetByID(Guid id)
        {
            try
            {
                ProductionPlanCustomModel planResult = (from plan in Query().Include(x => x.ProductionPlanDetail).Get()
                                                        join plan_detail in _ProductionDetailService.Query().Get().Where(x => x.ProductionDetailID == id) on plan.ProductionID equals plan_detail.ProductionID
                                                        join line in _LineService.Query().Get() on plan.LineID equals line.LineID
                                                        join product in _ProductService.Query().Include(x => x.CodeCollection).Get() on plan_detail.ProductID equals product.ProductID
                                                        join unit in _ProductUnitService.Query().Get() on plan_detail.ProductUnitID equals unit.ProductUnitID

                                                        select new { plan, plan_detail, product, unit, line })
                                 .Select(n => new ProductionPlanCustomModel
                                 {
                                     DailyPlanStatus = n.plan.DailyPlanStatus,
                                     OrderNo = n.plan.OrderNo,
                                     OrderType = n.plan.OrderType,
                                     PeriodID = n.plan.PeriodID,
                                     //PeriodStartTime = n.period.P_StartTime,
                                     //PeriodEndTime = n.period.P_EndTime,
                                     ProductionDate = n.plan.ProductionDate,
                                     ProductionID = n.plan.ProductionID,
                                     Remark = n.plan_detail.Remark,
                                     Box = n.plan_detail.Box,
                                     DeliveryDate = n.plan_detail.DeliveryDate,
                                     FD = n.plan_detail.FD,
                                     Film = n.plan_detail.Film,
                                     Formula = n.plan_detail.Formula,
                                     Mark = n.plan_detail.Mark,
                                     Oil = n.plan_detail.Oil,
                                     OilType = n.plan_detail.OilType,
                                     PalletQty = n.plan_detail.PalletQty,
                                     Powder = n.plan_detail.Powder,
                                     ProductID = n.plan_detail.ProductID,
                                     ProductionDetailID = n.plan_detail.ProductionDetailID,
                                     ProductionQty = n.plan_detail.ProductionQty,
                                     ProductUnitID = n.plan_detail.ProductUnitID,
                                     Seq = n.plan_detail.Seq,
                                     Stamp = n.plan_detail.Stamp,
                                     Sticker = n.plan_detail.Sticker,
                                     Weight_G = n.plan_detail.Weight_G,
                                     WorkingTime = n.plan_detail.WorkingTime,
                                     LineId = n.plan.LineID,
                                     LineCode = n.line.LineCode,
                                     ProductCode = n.product.CodeCollection.FirstOrDefault(x => x.IsDefault).Code ?? "",
                                     ProductName = n.product.Name,
                                     ProductUnitName = n.unit.Name

                                 }).SingleOrDefault();
                return planResult;
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

        public List<ProductionPlanCustomModel> GetAll(DateTime? sdte, DateTime? edte, Guid? lineId, LineTypeEnum lineType, bool isReceive, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                DateTime?[] d = Utilities.GetTerm(sdte, edte);

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from plan in Query().Get().Where(x => x.DailyPlanStatus == Convert.ToInt32(isReceive))
                              join plan_detail in _ProductionDetailService.Query().Get() on plan.ProductionID equals plan_detail.ProductionID
                              join line in _LineService.Query().Get().Where(x => x.LineType == lineType.ToString()) on plan.LineID equals line.LineID
                              join product in _ProductService.Query().Include(x => x.CodeCollection).Get() on plan_detail.ProductID equals product.ProductID
                              join unit in _ProductUnitService.Query().Get() on plan_detail.ProductUnitID equals unit.ProductUnitID
                              where plan_detail.IsActive
                              select new { plan, plan_detail, product, unit, line });


                if (lineId != null && lineId != Guid.Empty)
                {
                    result = result.Where(x => x.plan.LineID == lineId);
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    result = result.Where(x => (keyword != null ? x.product.Name.Contains(keyword) : true)
                                                || (keyword != null ? x.product.CodeCollection.Any(y => y.Code.Contains(keyword)) : true));
                }

                if (sdte != null && edte != null)
                {
                    result = result.Where(x => x.plan.ProductionDate >= sdte && x.plan.ProductionDate <= edte);
                }

                if (result == null || result.Count() == 0)
                {
                    return new List<ProductionPlanCustomModel>();
                }

                totalRecords = result.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.plan.ProductionDate).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<ProductionPlanCustomModel> planResult = result.Select(n => new ProductionPlanCustomModel
                {
                    DailyPlanStatus = n.plan.DailyPlanStatus,
                    OrderNo = n.plan.OrderNo,
                    OrderType = n.plan.OrderType,
                    PeriodID = n.plan.PeriodID,
                    //PeriodStartTime = n.period.P_StartTime,
                    //PeriodEndTime = n.period.P_EndTime,
                    ProductionDate = n.plan.ProductionDate,
                    ProductionID = n.plan.ProductionID,
                    Remark = n.plan_detail.Remark,
                    Box = n.plan_detail.Box,
                    DeliveryDate = n.plan_detail.DeliveryDate,
                    FD = n.plan_detail.FD,
                    Film = n.plan_detail.Film,
                    Formula = n.plan_detail.Formula,
                    Mark = n.plan_detail.Mark,
                    Oil = n.plan_detail.Oil,
                    OilType = n.plan_detail.OilType,
                    PalletQty = n.plan_detail.PalletQty,
                    Powder = n.plan_detail.Powder,
                    ProductID = n.plan_detail.ProductID,
                    ProductionDetailID = n.plan_detail.ProductionDetailID,
                    ProductionQty = n.plan_detail.ProductionQty,
                    ProductUnitID = n.plan_detail.ProductUnitID,
                    Seq = n.plan_detail.Seq,
                    Stamp = n.plan_detail.Stamp,
                    Sticker = n.plan_detail.Sticker,
                    Weight_G = n.plan_detail.Weight_G,
                    WorkingTime = n.plan_detail.WorkingTime,
                    LineId = n.plan.LineID,
                    LineCode = n.line.LineCode,
                    ProductCode = n.product.ProductCode,
                    ProductName = n.product.Name,
                    ProductUnitName = n.unit.Name,
                    IsActive = n.plan_detail.IsActive,
                    IsReceive = n.plan.DailyPlanStatus.Value == 1
                }).ToList();

                return planResult;
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

        private ValidationImportFileResult AddValidationDetailResult(int RowIndex, bool IsWarning, string message, string field, int col)
        {
            ValidationImportFileResult resultItem = new ValidationImportFileResult
            {
                TypeOfFile = "Details",
                ColumnIndex = col,
                RowIndex = RowIndex,
                Message = message,
                IsWarning = IsWarning,
                Field = field
            };
            return resultItem;
        }

        private ProductionPlan FillData(ProductionPlanCustomModel data)
        {
            ProductionPlan plan = new ProductionPlan
            {
                DailyPlanStatus = (int)DailyPlanStatusEnum.DailyPlan,
                OrderNo = data.OrderNo,
                LineID = data.LineId,
                OrderType = data.OrderType,
                ProductionDate = data.ProductionDate,
                Remark = data.Remark,
                UserCreated = UserID,
                UserModified = UserID,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            plan.ProductionPlanDetail.Add(new ProductionPlanDetail
            {
                Box = data.Box,
                DeliveryDate = data.DeliveryDate == DateTime.MinValue ? null : data.DeliveryDate,
                FD = data.FD,
                Film = data.Film,
                Formula = data.Formula,
                Mark = data.Mark,
                Oil = data.Oil,
                OilType = data.OilType,
                PalletQty = data.PalletQty,
                Powder = data.Powder,
                ProductionQty = data.ProductionQty,
                Remark = data.Remark,
                Seq = data.Seq,
                Stamp = data.Stamp,
                Weight_G = data.Weight_G,
                Sticker = data.Sticker,
                ProductID = data.ProductID,
                ProductUnitID = data.ProductUnitID,
                WorkingTime = data.WorkingTime,
                UserCreated = UserID,
                UserModified = UserID,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            });

            return plan;
        }

        private Receive FillReceiveData(ProductionPlanCustomModel data)
        {
            if (data.LineId == null || data.LineId == Guid.Empty)
            {
                throw new HILIException("MSG00006"); // data not found
            }

            Guid? locationLoadingInID = (from pl in _LineService.Query().Filter(x => x.IsActive).Get()
                                         join z in _ZoneService.Query().Filter(x => x.IsActive).Get() on pl.WarehouseID equals z.WarehouseID
                                         join l in _LocationService.Query().Filter(x => x.IsActive).Get() on z.ZoneID equals l.ZoneID
                                         where l.LocationType == LocationTypeEnum.LoadingIN
                                         select l).FirstOrDefault()?.LocationID;

            IQueryable<DocumentType> receiveTypes = _DocumentTypeService.Query().Filter(x => x.IsActive && x.DocType == DocumentTypeEnum.Receive).GetQueryable();
            Guid? receiveTypeID = receiveTypes.FirstOrDefault(x => (x.IsDefault ?? false))?.DocumentTypeID;
            if (receiveTypeID == null)
            {
                receiveTypeID = receiveTypes.FirstOrDefault()?.DocumentTypeID;
            }

            IQueryable<Contact> suppliers = _ContactService.Query().GetQueryable();
            Guid? supplierID = suppliers.FirstOrDefault(x => x.Code == "20004431")?.ContactID;
            if (supplierID == null)
            {
                supplierID = suppliers.FirstOrDefault()?.ContactID;
            }

            ReceivePrefix prefix = _ReceivePrefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
            if (prefix == null)
            {
                throw new HILIException("REC10012");
            }
            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10012));
            ReceivePrefix tPrefix = _ReceivePrefixService.FindByID(prefix.PrefixID);

            string receiveCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
            tPrefix.IsLastest = false;

            ReceivePrefix newPrefix = new ReceivePrefix()
            {
                IsLastest = true,
                LastedKey = receiveCode,
                PrefixKey = prefix.PrefixKey,
                FormatKey = prefix.FormatKey,
                LengthKey = prefix.LengthKey
            };

            _ReceivePrefixService.Add(newPrefix);
            _ReceivePrefixService.Modify(tPrefix);


            Receive receive = new Receive()
            {
                ReceiveCode = receiveCode,
                IsActive = true,
                IsUrgent = false,
                IsCrossDock = false,
                ReceiveStatus = ReceiveStatusEnum.New,
                Reference1 = data.OrderNo,
                EstimateDate = data.ProductionDate,
                ActualDate = data.ProductionDate,
                LineID = data.LineId,
                Remark = data.Remark,

                ReceiveTypeID = receiveTypeID.Value,
                SupplierID = supplierID.Value,
                ProductOwnerID = _ProductOwnerService.Query().GetQueryable().FirstOrDefault()?.ProductOwnerID ?? Guid.Empty,
                LocationID = locationLoadingInID,
                ReferenceID = data.ProductionID,

                UserCreated = UserID,
                DateCreated = DateTime.Now,
                UserModified = UserID,
                DateModified = DateTime.Now,
            };

            return receive;

        }

        private List<ReceiveDetail> FillReceiveDetailData(List<ProductionPlanCustomModel> data)
        {
            List<ReceiveDetail> receiveDetails = new List<ReceiveDetail>();
            Guid? pStatusID = _ProductStatusService.Query().Filter(x => x.Code == "NORMAL").Get().FirstOrDefault()?.ProductStatusID;
            Guid? pSubStatusID = _ProductSubStatusService.Query().Filter(x => x.Code == "SS000").Get().FirstOrDefault()?.ProductSubStatusID;

            int i = 1;

            foreach (ProductionPlanCustomModel item in data)
            {
                Product product = _ProductService.Query().Filter(x => x.ProductID == item.ProductID
                                && x.IsActive).Include(x => x.UnitCollection).GetQueryable().FirstOrDefault();

                ProductUnit unit = _ProductUnitService.Query().Filter(x => x.ProductUnitID == item.ProductUnitID
                                 && x.IsActive).GetQueryable().FirstOrDefault();

                decimal baseQty = 0;

                if (product == null || unit == null)
                {
                    throw new HILIException("MSG00006");
                }

                #region Unit Conversion

                if (unit.ConversionMark == null || unit.ConversionMark != 2)
                {
                    baseQty = Math.Round(item.ProductionQty * unit.Quantity, 2);
                }
                else
                {
                    baseQty = Math.Round(item.ProductionQty / unit.Quantity, 2);
                }

                #endregion

                ReceiveDetail receiveDetail = new ReceiveDetail()
                {
                    Sequence = i,
                    IsActive = true,
                    ReceiveDetailStatus = ReceiveDetailStatusEnum.New,

                    StockUnitID = item.ProductUnitID.Value,
                    BaseUnitID = product.UnitCollection.Where(x => x.IsBaseUOM).FirstOrDefault().ProductUnitID,

                    Remark = item.Remark,
                    ProductID = item.ProductID,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,

                    ProductStatusID = pStatusID.Value,
                    ProductSubStatusID = pSubStatusID,

                    ManufacturingDate = item.ProductionDate,
                    ExpirationDate = item.ProductionDate.AddDays(product.Age),

                    Quantity = item.ProductionQty,
                    ProductWeight = (double)item.Weight_G,
                    BaseQuantity = baseQty,
                    ConversionQty = unit.Quantity,
                    ProductWidth = unit.Width,
                    ProductLength = unit.Length,
                    ProductHeight = unit.Height,
                    PackageWeight = unit.PackageWeight,

                    UserCreated = UserID,
                    DateCreated = DateTime.Now,
                    UserModified = UserID,
                    DateModified = DateTime.Now
                };


                receiveDetails.Add(receiveDetail);
                i++;
            }

            return receiveDetails;
        }

        public async void SedToReceive(List<Guid> ids)
        {
            bool ok = await OnSendData(ids);
        }

        private async Task<bool> OnSendData(List<Guid> ids)
        {
            try
            {
                List<ProductionPlan> receiving = Query().Filter(x => x.IsActive && ids.Contains(x.ProductionID)).Get().ToList();

                List<DataTransfer> dataTrans = receiving.GroupBy(g => new
                {
                    ProductionID = g.ProductionID
                }).Select(n => new DataTransfer
                {
                    PackagePrevID = InstanceID,
                    InstanceID = InstanceID,
                    ReferenceBaseID = n.Key.ProductionID,
                    Start = true
                }).ToList();

                Engine engine = new Engine();
                bool result = await engine.Transmit(dataTrans);
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

    }
}
