using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.DispatchService;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.ReceiveService
{

    public class ReceiveService : Repository<Receive>, IReceiveService
    {
        #region [ Property ]
        private readonly IUnitOfWork unitofwork;
        private readonly IRepository<ReceiveDetail> receiveDetailService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductCodes> productCodeService;
        private readonly IRepository<Contact> contactService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<DocumentType> documentTypeService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<ProductUnit> productUnitService;
        private readonly IRepository<ProductStatus> productStatusService;
        private readonly IRepository<ProductSubStatus> productSubStatusService;
        private readonly IRepository<ProductStatusMapDocument> productStatusMapDocService;
        private readonly IRepository<ReceiveAssignJob> assignjob;
        private readonly IRepository<ReceivePrefix> prefixService;
        private readonly IRepository<GoodsReturnPrefix> goodsReturnPrefixService;
        private readonly IRepository<Employee> employeeService;
        private readonly IRepository<Line> lineService;
        //private readonly IRepository<ProductionPlan> productionPlanService;
        private readonly IRepository<ProductionPlanDetail> productionPlanDetailService;
        private readonly IRepository<ProductionControl> productionControlService;
        private readonly IRepository<ProductionControlDetail> pcDetailService;
        private readonly IRepository<ItfInterfaceMapping> itfInterfaceMappingService;
        private readonly IRepository<Dispatch> dispatchService;
        private readonly IRepository<DispatchDetail> dispatchDetailService;
        private readonly IRepository<DispatchBooking> dispatchBookingService;
        private readonly IRepository<DispatchPrefix> dispatchPrefixService;
        private readonly IRepository<ShippingTo> shiptoService;
        private readonly IRepository<Zone> zoneService;
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<ChangestatusPrefix> changestatusPrefixService;
        private readonly IRepository<Changestatus> changeStatusService;
        private readonly IRepository<Reason> ReasonService;
        private readonly IRepository<GoodsReturn> GoodsReturnService;
        private readonly IRepository<GoodsReturnDetail> GoodsReturnDetailService;
        private readonly IStockService stockService;
        private readonly IDispatchDetailService _DispatchDetailService;

        #endregion

        #region [ Constructor ]

        public ReceiveService(IUnitOfWork context,
                              IStockService _stockService,
                              IDispatchDetailService tmpDispatchDetailService)
            : base(context)
        {
            unitofwork = context;
            receiveDetailService = context.Repository<ReceiveDetail>();
            productService = context.Repository<Product>();
            documentTypeService = context.Repository<DocumentType>();
            contactService = context.Repository<Contact>();
            locationService = context.Repository<Location>();
            productUnitService = context.Repository<ProductUnit>();
            productStatusService = context.Repository<ProductStatus>();
            productSubStatusService = context.Repository<ProductSubStatus>();
            productStatusMapDocService = context.Repository<ProductStatusMapDocument>();
            receivingService = context.Repository<Receiving>();
            productCodeService = context.Repository<ProductCodes>();
            assignjob = context.Repository<ReceiveAssignJob>();
            prefixService = context.Repository<ReceivePrefix>();
            goodsReturnPrefixService = context.Repository<GoodsReturnPrefix>();
            productOwnerService = context.Repository<ProductOwner>(); ;
            employeeService = context.Repository<Employee>(); ;
            lineService = context.Repository<Line>(); ;
            productionPlanDetailService = context.Repository<ProductionPlanDetail>(); ;
            productionControlService = context.Repository<ProductionControl>(); ;
            pcDetailService = context.Repository<ProductionControlDetail>();
            itfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
            dispatchService = context.Repository<Dispatch>();
            dispatchDetailService = context.Repository<DispatchDetail>();
            dispatchBookingService = context.Repository<DispatchBooking>();
            dispatchPrefixService = context.Repository<DispatchPrefix>();
            shiptoService = context.Repository<ShippingTo>();
            zoneService = context.Repository<Zone>();
            warehouseService = context.Repository<Warehouse>();
            changestatusPrefixService = context.Repository<ChangestatusPrefix>();
            changeStatusService = context.Repository<Changestatus>();
            ReasonService = context.Repository<Reason>();
            GoodsReturnService = context.Repository<GoodsReturn>();
            GoodsReturnDetailService = context.Repository<GoodsReturnDetail>();
            stockService = _stockService;
            _DispatchDetailService = tmpDispatchDetailService;
        }

        #endregion

        #region [ Method ]

        public override Receive Add(Receive entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("REC10001");
                    }
                    //throw new ArgumentNullException("Receive", MessageManger.GetMessage(ReceiveLanguage.R10001));

                    ReceivePrefix prefix = prefixService.Query().Filter(x => x.IsLastest.HasValue && x.IsLastest.Value).Get().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("REC10012");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10012));
                    ReceivePrefix tPrefix = prefixService.FindByID(prefix.PrefixID);

                    string receiveCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    entity.ReceiveCode = receiveCode;
                    tPrefix.IsLastest = false;

                    ReceivePrefix newPrefix = new ReceivePrefix()
                    {
                        IsLastest = true,
                        LastedKey = receiveCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    prefixService.Add(newPrefix);
                    prefixService.Modify(tPrefix);

                    int nIndex = 1;
                    entity.ReceiveDetailCollection.ToList().ForEach(item =>
                    {
                        Product product = productService.Query().Filter(x => x.ProductID == item.ProductID).Get().FirstOrDefault();
                        if (product == null)
                        {
                            throw new HILIException("REC10001");
                        }
                        //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Product"));

                        ProductUnit baseUnit = productUnitService.Query().Filter(x => x.IsActive && x.ProductID == item.ProductID && x.IsBaseUOM).Get().FirstOrDefault();
                        if (baseUnit == null)
                        {
                            throw new HILIException("REC10001");
                        }
                        //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Base Unit"));

                        ProductUnit stockUnit = productUnitService.Query().Filter(x => x.IsActive && x.ProductID == item.ProductID && x.ProductUnitID == item.StockUnitID).Get().FirstOrDefault();
                        if (stockUnit == null)
                        {
                            throw new HILIException("REC10001");
                        }
                        //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Stock Unit"));

                        item.ProductWeight = baseUnit.ProductWeight;
                        item.ProductWidth = stockUnit.Width;
                        item.ProductLength = stockUnit.Length;
                        item.ProductHeight = stockUnit.Height;
                        item.PackageWeight = stockUnit.PackageWeight;

                        item.ConversionQty = stockUnit.Quantity;
                        item.BaseUnitID = baseUnit.ProductUnitID;
                        item.Sequence = nIndex;
                        item.BaseQuantity = item.Quantity * stockUnit.Quantity;
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                        item.UserCreated = entity.UserCreated;
                        item.UserModified = entity.UserModified;
                        nIndex++;
                    });

                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    entity.ActualDate = null;

                    Receive result = base.Add(entity);

                    scope.Complete();
                    return result;

                }
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

        public override void Modify(Receive entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Receive _current = Query().Filter(x => x.ReceiveID == entity.ReceiveID)
                                          .Include(x => x.ReceiveDetailCollection)
                                          .Include(x => x.ReceiveAssignJobCollection).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("REC10001");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));

                    if (_current.ReceiveStatus == ReceiveStatusEnum.Cancel)
                    {
                        throw new HILIException("REC10003");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10003));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Close)
                    {
                        throw new HILIException("REC10002");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10002));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Complete)
                    {

                        entity.ReceiveAssignJobCollection = null;
                        entity.ReceiveDetailCollection = null;
                        entity.DateModified = DateTime.Now;
                        base.Modify(entity);
                        scope.Complete();

                        return;
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10004));


                    ///Remove Item 
                    _current.ReceiveDetailCollection.Where(x => x.IsActive).ToList()
                        .ForEach(item =>
                        {
                            if (item.ReceiveDetailStatus == ReceiveDetailStatusEnum.Inprogress)
                            {
                                throw new HILIException("REC10007");
                            }
                            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10007));
                            else if (item.ReceiveDetailStatus == ReceiveDetailStatusEnum.Complete)
                            {
                                throw new HILIException("REC10011");
                            }
                            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10011));
                            else if (item.ReceiveDetailStatus == ReceiveDetailStatusEnum.Cancel)
                            {
                                throw new HILIException("REC10009");
                            }
                            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10009));
                            else if (_current.ReceiveStatus == ReceiveStatusEnum.Close)
                            {
                                throw new HILIException("REC10008");
                            }
                            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10008));

                            bool exist = entity.ReceiveDetailCollection.Any(x => x.ReceiveDetailID == item.ReceiveDetailID);
                            if (!exist)
                            {
                                item.IsActive = false;
                                item.DateModified = DateTime.Now;
                                item.UserModified = UserID;
                                receiveDetailService.Modify(item);
                            }
                        });

                    //Add new Item 
                    entity.ReceiveDetailCollection.ToList()
                       .ForEach(item =>
                       {
                           Product product = productService.Query().Filter(x => x.ProductID == item.ProductID).Get().FirstOrDefault();
                           if (product == null)
                           {
                               throw new HILIException("REC10001");
                           }
                           //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Product"));

                           ProductUnit baseUnit = productUnitService.Query().Filter(x => x.IsActive && x.ProductID == item.ProductID && x.IsBaseUOM).Get().FirstOrDefault();
                           if (baseUnit == null)
                           {
                               throw new HILIException("REC10001");
                           }
                           //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Base Unit"));

                           ProductUnit stockUnit = productUnitService.Query().Filter(x => x.IsActive && x.ProductID == item.ProductID && x.ProductUnitID == item.StockUnitID).Get().FirstOrDefault();
                           if (stockUnit == null)
                           {
                               throw new HILIException("REC10001");
                           }
                           //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Stock Unit"));

                           if (item.ReceiveDetailID == null || Utilities.IsZeroGuid(item.ReceiveDetailID))
                           {
                               item.ProductWeight = baseUnit.ProductWeight;
                               item.ProductWidth = stockUnit.Width;
                               item.ProductLength = stockUnit.Length;
                               item.ProductHeight = stockUnit.Height;
                               item.PackageWeight = stockUnit.PackageWeight;

                               item.ConversionQty = stockUnit.Quantity;
                               item.BaseUnitID = baseUnit.ProductUnitID;
                               item.BaseQuantity = item.Quantity * stockUnit.Quantity;
                               item.IsActive = true;
                               item.DateCreated = DateTime.Now;
                               item.DateModified = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               item.UserModified = entity.UserModified;
                               receiveDetailService.Add(item);
                           }
                           else
                           {
                               ReceiveDetail itemCurrent = receiveDetailService.Query().Filter(x => x.ReceiveDetailID == item.ReceiveDetailID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   throw new HILIException("REC10001");
                               }
                               //throw new Exception(MessageManger.GetMessage(Message.ReceiveLanguage.R10001, "Receive Detail"));


                               item.ConversionQty = (stockUnit.IsBaseUOM ? item.Quantity : stockUnit.Quantity);
                               item.BaseUnitID = baseUnit.ProductUnitID;
                               item.BaseQuantity = item.Quantity * stockUnit.Quantity;

                               item.IsActive = true;
                               item.DateModified = DateTime.Now;
                               item.UserModified = entity.UserModified;
                               receiveDetailService.Modify(item);

                           }
                       });


                    //Remove job 
                    _current.ReceiveAssignJobCollection.ToList()
                        .ForEach(item =>
                        {
                            bool has = entity.ReceiveAssignJobCollection.Any(x => x.ReferenceID == item.ReferenceID && x.EmployeeID == item.EmployeeID);
                            if (!has)
                            {
                                ReceiveAssignJob itm = assignjob.Query().Filter(x => x.ReferenceID == item.ReferenceID && x.EmployeeID == item.EmployeeID).Get().FirstOrDefault();
                                assignjob.Remove(itm);
                            }

                        });

                    //Add new job Item 
                    entity.ReceiveAssignJobCollection.ToList()
                       .ForEach(item =>
                       {
                           ReceiveAssignJob job = assignjob.Query().Filter(x => x.ReferenceID == item.ReferenceID && x.EmployeeID == item.EmployeeID).Get().FirstOrDefault();
                           if (job == null)
                           {
                               ReceiveAssignJob _job = new ReceiveAssignJob
                               {
                                   EmployeeID = item.EmployeeID,
                                   ReferenceID = item.ReferenceID

                               };
                               assignjob.Add(_job);
                           }
                       });



                    entity.ReceiveAssignJobCollection = null;
                    entity.ReceiveDetailCollection = null;
                    entity.DateModified = DateTime.Now;

                    base.Modify(entity);
                    scope.Complete();
                }
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
                    Receive _current = FindByID(id);
                    if (_current == null)
                    {
                        throw new HILIException("REC10001");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));

                    if (_current.ReceiveStatus == ReceiveStatusEnum.Cancel)
                    {
                        throw new HILIException("REC10003");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10003));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Close)
                    {
                        throw new HILIException("REC10002");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10002));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Complete)
                    {
                        throw new HILIException("REC10004");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10004));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Partial)
                    {
                        throw new HILIException("REC10006");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10006));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Inprogress)
                    {
                        throw new HILIException("REC10006");
                    }
                    // throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10006));


                    _current.ReceiveStatus = ReceiveStatusEnum.Cancel;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

                    scope.Complete();
                }

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

        public bool Cancel(Guid id)
        {
            try
            {
                Receive _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("SYS10001");
                }
                //throw new Exception(MessageManger.GetMessage(Message.Core.SYS10001));

                using (TransactionScope scope = new TransactionScope())
                {
                    if (_current.ReceiveStatus == ReceiveStatusEnum.Cancel)
                    {
                        throw new HILIException("REC10003");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10003));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Close)
                    {
                        throw new HILIException("REC10002");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10002));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Complete)
                    {
                        throw new HILIException("REC10004");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10004));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Partial)
                    {
                        throw new HILIException("REC10006");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10006));
                    else if (_current.ReceiveStatus == ReceiveStatusEnum.Inprogress)
                    {
                        throw new HILIException("REC10006");
                    }
                    // throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10006));

                    _current.ReceiveStatus = ReceiveStatusEnum.Cancel;
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

                    List<ReceiveDetail> rDetails = receiveDetailService.Query().Filter(x => x.ReceiveID == id).Get().ToList();
                    if (rDetails != null)
                    {
                        rDetails.ForEach(x =>
                        {
                            x.IsActive = false;
                            x.ReceiveDetailStatus = ReceiveDetailStatusEnum.Cancel;
                            x.UserModified = UserID;
                            x.DateModified = DateTime.Now;
                            receiveDetailService.Modify(x);
                        });
                    }

                    scope.Complete();
                }

                return true;
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

        public Receive GetByID(Guid id)
        {
            try
            {
                Receive _current = base.FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("REC10001");
                }
                //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));

                Receive receiveResult = (from receive in Query().Filter(x => x.ReceiveID == id)
                                                            .Include(x => x.ReceiveDetailCollection.Select(s => s.ReceivingCollection))
                                                            .Include(x => x.ReceiveAssignJobCollection).Get()
                                         join supplier in contactService.Query().Get() on receive.SupplierID equals supplier.ContactID
                                         join productOwner in productOwnerService.Query().Get() on receive.ProductOwnerID equals productOwner.ProductOwnerID
                                         join document in documentTypeService.Query().Get() on receive.ReceiveTypeID equals document.DocumentTypeID
                                         join location in locationService.Query().Include(x => x.Zone.Warehouse).Get() on receive.LocationID equals location.LocationID
                                         select new { receive, supplier, productOwner, document, location })
                                     .Select(n => new Receive
                                     {
                                         ReceiveDetailCollection = n.receive.ReceiveDetailCollection.Where(x => x.IsActive).ToList(),
                                         ReceiveAssignJobCollection = n.receive.ReceiveAssignJobCollection.ToList(),
                                         ReceiveID = n.receive.ReceiveID,
                                         ActualDate = n.receive.ActualDate,
                                         CloseJobReason = n.receive.CloseJobReason,
                                         ContainerNo = n.receive.ContainerNo,
                                         DateCreated = n.receive.DateCreated,
                                         DateModified = n.receive.DateModified,
                                         EstimateDate = n.receive.EstimateDate,
                                         InvoiceNo = n.receive.InvoiceNo,
                                         IsActive = n.receive.IsActive,
                                         IsUrgent = n.receive.IsUrgent,
                                         IsCrossDock = n.receive.IsCrossDock,
                                         LocationID = n.receive.LocationID,
                                         PONumber = n.receive.PONumber,
                                         ProductOwnerID = n.receive.ProductOwnerID,
                                         ReceiveCode = n.receive.ReceiveCode,
                                         ReceiveTypeID = n.receive.ReceiveTypeID,
                                         Reference1 = n.receive.Reference1,
                                         Reference2 = n.receive.Reference2,
                                         ReceiveStatus = n.receive.ReceiveStatus,
                                         Remark = n.receive.Remark,
                                         SupplierID = n.receive.SupplierID,
                                         UserCreated = n.receive.UserCreated,
                                         UserModified = n.receive.UserModified,
                                         DocumentType = n.document,
                                         Location = n.location,
                                         ProductOwner = n.productOwner,
                                         Supplier = n.supplier
                                     }).FirstOrDefault();


                List<ReceiveDetail> detail = (from receivedetail in receiveResult.ReceiveDetailCollection.Where(x => x.IsActive).ToList()
                                              join product in productService.Query()
                                                                            .Include(x => x.CodeCollection).Get() on receivedetail.ProductID equals product.ProductID
                                              join stockunit in productUnitService.Query().Get() on receivedetail.StockUnitID equals stockunit.ProductUnitID
                                              join inventoryunit in productUnitService.Query().Get() on receivedetail.BaseUnitID equals inventoryunit.ProductUnitID
                                              join priceunit in productUnitService.Query().Get() on receivedetail.ProductUnitPriceID equals priceunit.ProductUnitID into g
                                              from priceUOM in g.DefaultIfEmpty()
                                              join productstatus in productStatusService.Query().Get() on receivedetail.ProductStatusID equals productstatus.ProductStatusID into pStatus
                                              from prodStatus in pStatus
                                              join productsubstatus in productSubStatusService.Query().Get() on receivedetail.ProductSubStatusID equals productsubstatus.ProductSubStatusID into pSubStatus
                                              from prodSubStatus in pSubStatus
                                              select new { receivedetail, product, stockunit, inventoryunit, priceUOM, prodStatus, prodSubStatus })
                             .Select(n => new ReceiveDetail
                             {
                                 ReceivingCollection = n.receivedetail.ReceivingCollection.Where(x => x.IsActive).ToList(),
                                 ConversionQty = n.receivedetail.ConversionQty,
                                 DateCreated = n.receivedetail.DateCreated,
                                 DateModified = n.receivedetail.DateModified,
                                 ExpirationDate = n.receivedetail.ExpirationDate,
                                 ReceiveDetailID = n.receivedetail.ReceiveDetailID,
                                 BaseUnitID = n.receivedetail.BaseUnitID,
                                 IsActive = n.receivedetail.IsActive,
                                 Lot = n.receivedetail.Lot,
                                 ManufacturingDate = n.receivedetail.ManufacturingDate,
                                 BaseQuantity = n.receivedetail.BaseQuantity,
                                 PackageWeight = n.receivedetail.PackageWeight,
                                 Price = n.receivedetail.Price,
                                 ProductHeight = n.receivedetail.ProductHeight,
                                 ProductLength = n.receivedetail.ProductLength,
                                 ProductStatusID = n.receivedetail.ProductStatusID,
                                 ProductID = n.receivedetail.ProductID,
                                 ProductSubStatusID = n.receivedetail.ProductSubStatusID,
                                 ProductUnitPriceID = n.receivedetail.ProductUnitPriceID,
                                 ProductWeight = n.receivedetail.ProductWeight,
                                 ProductWidth = n.receivedetail.ProductWidth,
                                 Quantity = n.receivedetail.Quantity,
                                 ReceiveID = n.receivedetail.ReceiveID,
                                 Remark = n.receivedetail.Remark,
                                 StockUnitID = n.receivedetail.StockUnitID,
                                 UserCreated = n.receivedetail.UserCreated,
                                 UserModified = n.receivedetail.UserModified,
                                 ReceiveDetailStatus = n.receivedetail.ReceiveDetailStatus,
                                 ProductCode = n.product.ProductCode,
                                 ProductBaseUOM = n.inventoryunit,
                                 ProductPriceUOM = n.priceUOM,
                                 Product = n.product,
                                 ProductUOM = n.stockunit,
                                 ProductStatus = n.prodStatus,
                                 ProductSubStatus = n.prodSubStatus,
                                 ProductName = n.product.Name,
                             }).ToList();

                List<ReceiveAssignJob> job = (from jobassg in receiveResult.ReceiveAssignJobCollection.ToList()
                                              join emp in employeeService.Query().Get() on jobassg.EmployeeID equals emp.EmployeeID
                                              select new { jobassg, emp }).Select(g => new ReceiveAssignJob
                                              {
                                                  ReferenceID = g.jobassg.ReferenceID,
                                                  EmployeeID = g.jobassg.EmployeeID,
                                                  Employee = g.emp,
                                              }).ToList();

                receiveResult.ReceiveDetailCollection = detail;
                receiveResult.ReceiveAssignJobCollection = job;

                return receiveResult;
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

        public Receive GetByReceiveCode(string code)
        {
            try
            {
                Receive current = Query().Filter(s => s.ReceiveCode == code).Get().FirstOrDefault();
                if (current == null)
                {
                    throw new HILIException("REC10001");
                }
                //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001, code));

                return GetByID(current.ReceiveID);
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

        public List<Receive> GetAll(Guid? productOwnerID, ReceiveStatusEnum? status, string keyword, DateTime? sdte, DateTime? edte, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                DateTime?[] d = Utilities.GetTerm(sdte, edte);

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from receive in Query().Filter(x => (status != null ? x.ReceiveStatus == status.Value : true)).Include(x => x.ReceiveDetailCollection).Get()
                              join supplier in contactService.Query().Get() on receive.SupplierID equals supplier.ContactID
                              join productOwner in productOwnerService.Query().Get() on receive.ProductOwnerID equals productOwner.ProductOwnerID
                              join document in documentTypeService.Query().Get() on receive.ReceiveTypeID equals document.DocumentTypeID
                              join location in locationService.Query()
                                                                .Include(x => x.Zone.Warehouse).Get() on receive.LocationID equals location.LocationID
                              where ((receive.ReceiveCode.Contains(keyword) || receive.PONumber.Contains(keyword) || receive.InvoiceNo.Contains(keyword) ||
                                      supplier.Name.Contains(keyword) || productOwner.Name.Contains(keyword)) &&
                                    (productOwnerID != null ? receive.ProductOwnerID == productOwnerID.Value : true)) &&
                                    (d[0] != null ? (receive.DateCreated >= d[0] && receive.DateCreated <= d[1]) : true)
                              select new { receive, supplier, productOwner, document, location });

                totalRecords = result.Count();

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.receive.ReceiveCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<Receive> receiveResult = result.Select(n => new Receive
                {
                    TotalItems = n.receive.ReceiveDetailCollection.Where(x => x.IsActive).Count(),
                    ReceiveID = n.receive.ReceiveID,
                    ActualDate = n.receive.ActualDate,
                    CloseJobReason = n.receive.CloseJobReason,
                    ContainerNo = n.receive.ContainerNo,
                    DateCreated = n.receive.DateCreated,
                    DateModified = n.receive.DateModified,
                    EstimateDate = n.receive.EstimateDate,
                    InvoiceNo = n.receive.InvoiceNo,
                    IsActive = n.receive.IsActive,
                    IsUrgent = n.receive.IsUrgent,
                    LocationID = n.receive.LocationID,
                    PONumber = n.receive.PONumber,
                    ProductOwnerID = n.receive.ProductOwnerID,
                    ReceiveCode = n.receive.ReceiveCode,
                    ReceiveTypeID = n.receive.ReceiveTypeID,
                    Reference1 = n.receive.Reference1,
                    Reference2 = n.receive.Reference2,
                    ReceiveStatus = n.receive.ReceiveStatus,
                    Remark = n.receive.Remark,
                    SupplierID = n.receive.SupplierID,
                    UserCreated = n.receive.UserCreated,
                    UserModified = n.receive.UserModified,
                    DocumentType = n.document,
                    Location = n.location,
                    ProductOwner = n.productOwner,
                    Supplier = n.supplier

                }).ToList();

                return receiveResult;
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public ReceiveDetail GetReceiveDetailByProductCode(Guid receiveID, string productCode)
        {
            try
            {
                ReceiveDetail detail = (from receivedetail in receiveDetailService.Query().Filter(x => x.ReceiveID == receiveID).Include(s => s.ReceivingCollection).Get()
                                        join product in productService.Query()
                                                                      .Include(x => x.CodeCollection).Get() on receivedetail.ProductID equals product.ProductID
                                        join stockunit in productUnitService.Query().Get() on receivedetail.StockUnitID equals stockunit.ProductUnitID
                                        join inventoryunit in productUnitService.Query().Get() on receivedetail.BaseUnitID equals inventoryunit.ProductUnitID
                                        join priceunit in productUnitService.Query().Get() on receivedetail.ProductUnitPriceID equals priceunit.ProductUnitID into priceuom
                                        from priceUnit in priceuom.DefaultIfEmpty()
                                        join productstatus in productStatusService.Query().Get() on receivedetail.ProductStatusID equals productstatus.ProductStatusID
                                        join productsubstatus in productSubStatusService.Query().Get() on receivedetail.ProductSubStatusID equals productsubstatus.ProductSubStatusID
                                        join productcode in productCodeService.Query().Get() on receivedetail.ProductID equals productcode.ProductID
                                        where productcode.Code == productCode
                                        select new { receivedetail, product, stockunit, inventoryunit, priceUnit, productstatus, productsubstatus })
                             .Select(n => new ReceiveDetail
                             {
                                 ReceivingCollection = n.receivedetail.ReceivingCollection.Where(x => x.IsActive).ToList(),
                                 ConversionQty = n.receivedetail.ConversionQty,
                                 DateCreated = n.receivedetail.DateCreated,
                                 DateModified = n.receivedetail.DateModified,
                                 ExpirationDate = n.receivedetail.ExpirationDate,
                                 ReceiveDetailID = n.receivedetail.ReceiveDetailID,
                                 BaseUnitID = n.receivedetail.BaseUnitID,
                                 IsActive = n.receivedetail.IsActive,
                                 Lot = n.receivedetail.Lot,
                                 ManufacturingDate = n.receivedetail.ManufacturingDate,
                                 BaseQuantity = n.receivedetail.BaseQuantity,
                                 PackageWeight = n.receivedetail.PackageWeight,
                                 Price = n.receivedetail.Price,
                                 ProductHeight = n.receivedetail.ProductHeight,
                                 ProductLength = n.receivedetail.ProductLength,
                                 ProductStatusID = n.receivedetail.ProductStatusID,
                                 ProductID = n.receivedetail.ProductID,
                                 ProductSubStatusID = n.receivedetail.ProductSubStatusID,
                                 ProductUnitPriceID = n.receivedetail.ProductUnitPriceID,
                                 ProductWeight = n.receivedetail.ProductWeight,
                                 ProductWidth = n.receivedetail.ProductWidth,
                                 Quantity = n.receivedetail.Quantity,
                                 ReceiveID = n.receivedetail.ReceiveID,
                                 Remark = n.receivedetail.Remark,
                                 StockUnitID = n.receivedetail.StockUnitID,
                                 UserCreated = n.receivedetail.UserCreated,
                                 UserModified = n.receivedetail.UserModified,
                                 ReceiveDetailStatus = n.receivedetail.ReceiveDetailStatus,
                                 ProductBaseUOM = n.inventoryunit,
                                 ProductPriceUOM = n.priceUnit,
                                 Product = n.product,
                                 ProductCode = n.product.ProductCode,
                                 ProductUOM = n.stockunit,
                                 ProductStatus = n.productstatus,
                                 ProductSubStatus = n.productsubstatus,
                                 PalletCode = n.receivedetail.PalletCode,
                                 ProductName = n.product.Name,
                                 Sequence = n.receivedetail.Sequence
                             }).FirstOrDefault();

                return detail;
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

        public ReceiveDetail GetReceiveDetailByPallet(Guid ReceiveID, string palletCode)
        {
            try
            {
                ReceiveDetail detail = (from receivedetail in receiveDetailService.Query().Filter(x => x.ReceiveID == ReceiveID).Get()
                                        join receiving in receivingService.Query().Get() on receivedetail.ReceiveDetailID equals receiving.ReceiveDetailID
                                        join product in productService.Query()
                                                                      .Include(x => x.CodeCollection).Get() on receivedetail.ProductID equals product.ProductID
                                        join stockunit in productUnitService.Query().Get() on receivedetail.StockUnitID equals stockunit.ProductUnitID
                                        join inventoryunit in productUnitService.Query().Get() on receivedetail.BaseUnitID equals inventoryunit.ProductUnitID
                                        join priceunit in productUnitService.Query().Get() on receivedetail.ProductUnitPriceID equals priceunit.ProductUnitID into priceuom
                                        from priceUnit in priceuom.DefaultIfEmpty()
                                        join productstatus in productStatusService.Query().Get() on receivedetail.ProductStatusID equals productstatus.ProductStatusID
                                        join productsubstatus in productSubStatusService.Query().Get() on receivedetail.ProductSubStatusID equals productsubstatus.ProductSubStatusID
                                        where receiving.PalletCode == palletCode
                                        select new { receivedetail, product, stockunit, inventoryunit, priceUnit, productstatus, productsubstatus })
                               .Select(n => new ReceiveDetail
                               {
                                   ConversionQty = n.receivedetail.ConversionQty,
                                   DateCreated = n.receivedetail.DateCreated,
                                   DateModified = n.receivedetail.DateModified,
                                   ExpirationDate = n.receivedetail.ExpirationDate,
                                   ReceiveDetailID = n.receivedetail.ReceiveDetailID,
                                   BaseUnitID = n.receivedetail.BaseUnitID,
                                   IsActive = n.receivedetail.IsActive,
                                   Lot = n.receivedetail.Lot,
                                   ManufacturingDate = n.receivedetail.ManufacturingDate,
                                   BaseQuantity = n.receivedetail.BaseQuantity,
                                   PackageWeight = n.receivedetail.PackageWeight,
                                   Price = n.receivedetail.Price,
                                   ProductHeight = n.receivedetail.ProductHeight,
                                   ProductLength = n.receivedetail.ProductLength,
                                   ProductStatusID = n.receivedetail.ProductStatusID,
                                   ProductID = n.receivedetail.ProductID,
                                   ProductSubStatusID = n.receivedetail.ProductSubStatusID,
                                   ProductUnitPriceID = n.receivedetail.ProductUnitPriceID,
                                   ProductWeight = n.receivedetail.ProductWeight,
                                   ProductWidth = n.receivedetail.ProductWidth,
                                   Quantity = n.receivedetail.Quantity,
                                   ReceiveID = n.receivedetail.ReceiveID,
                                   Remark = n.receivedetail.Remark,
                                   StockUnitID = n.receivedetail.StockUnitID,
                                   UserCreated = n.receivedetail.UserCreated,
                                   UserModified = n.receivedetail.UserModified,
                                   ReceiveDetailStatus = n.receivedetail.ReceiveDetailStatus,
                                   ProductBaseUOM = n.inventoryunit,
                                   ProductPriceUOM = n.priceUnit,
                                   Product = n.product,
                                   ProductCode = n.product.ProductCode,
                                   ProductUOM = n.stockunit,
                                   ProductStatus = n.productstatus,
                                   ProductSubStatus = n.productsubstatus,
                                   PalletCode = n.receivedetail.PalletCode,
                                   ProductName = n.product.Name,
                                   Sequence = n.receivedetail.Sequence
                               }).FirstOrDefault();

                return detail;
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

        public ReceiveDetail GetReceiveDetail(Guid receiveDetailID)
        {
            try
            {
                ReceiveDetail detail = (from receivedetail in receiveDetailService.Query().Filter(x => x.ReceiveDetailID == receiveDetailID).Include(s => s.ReceivingCollection).Include(x => x.Receive).Get()
                                        join product in productService.Query()
                                                                      .Include(x => x.CodeCollection).Get() on receivedetail.ProductID equals product.ProductID
                                        join stockunit in productUnitService.Query().Get() on receivedetail.StockUnitID equals stockunit.ProductUnitID
                                        join inventoryunit in productUnitService.Query().Get() on receivedetail.BaseUnitID equals inventoryunit.ProductUnitID
                                        join priceunit in productUnitService.Query().Get() on receivedetail.ProductUnitPriceID equals priceunit.ProductUnitID into g
                                        from priceUOM in g.DefaultIfEmpty()
                                        join productstatus in productStatusService.Query().Get() on receivedetail.ProductStatusID equals productstatus.ProductStatusID into pStatus
                                        from prodStatus in pStatus
                                        join productsubstatus in productSubStatusService.Query().Get() on receivedetail.ProductSubStatusID equals productsubstatus.ProductSubStatusID into pSubStatus
                                        from prodSubStatus in pSubStatus
                                        select new { receivedetail, product, stockunit, inventoryunit, priceUOM, prodStatus, prodSubStatus })
                             .Select(n => new ReceiveDetail
                             {
                                 Receive = n.receivedetail.Receive,
                                 ReceivingCollection = n.receivedetail.ReceivingCollection.Where(x => x.IsActive).ToList(),
                                 ConversionQty = n.receivedetail.ConversionQty,
                                 DateCreated = n.receivedetail.DateCreated,
                                 DateModified = n.receivedetail.DateModified,
                                 ExpirationDate = n.receivedetail.ExpirationDate,
                                 ReceiveDetailID = n.receivedetail.ReceiveDetailID,
                                 BaseUnitID = n.receivedetail.BaseUnitID,
                                 IsActive = n.receivedetail.IsActive,
                                 Lot = n.receivedetail.Lot,
                                 ManufacturingDate = n.receivedetail.ManufacturingDate,
                                 BaseQuantity = n.receivedetail.BaseQuantity,
                                 PackageWeight = n.receivedetail.PackageWeight,
                                 Price = n.receivedetail.Price,
                                 ProductHeight = n.receivedetail.ProductHeight,
                                 ProductLength = n.receivedetail.ProductLength,
                                 ProductStatusID = n.receivedetail.ProductStatusID,
                                 ProductID = n.receivedetail.ProductID,
                                 ProductSubStatusID = n.receivedetail.ProductSubStatusID,
                                 ProductUnitPriceID = n.receivedetail.ProductUnitPriceID,
                                 ProductWeight = n.receivedetail.ProductWeight,
                                 ProductWidth = n.receivedetail.ProductWidth,
                                 Quantity = n.receivedetail.Quantity,
                                 ReceiveID = n.receivedetail.ReceiveID,
                                 Remark = n.receivedetail.Remark,
                                 StockUnitID = n.receivedetail.StockUnitID,
                                 UserCreated = n.receivedetail.UserCreated,
                                 UserModified = n.receivedetail.UserModified,
                                 ReceiveDetailStatus = n.receivedetail.ReceiveDetailStatus,
                                 ProductCode = n.product.ProductCode,
                                 ProductBaseUOM = n.inventoryunit,
                                 ProductPriceUOM = n.priceUOM,
                                 Product = n.product,
                                 ProductUOM = n.stockunit,
                                 ProductStatus = n.prodStatus,
                                 ProductSubStatus = n.prodSubStatus,
                                 ProductName = n.product.Name,
                             }).FirstOrDefault();

                return detail;
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

        #region  [ Receiving ] 

        public void Receiving(Receiving entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ReceiveDetail receiveDetail = receiveDetailService.FirstOrDefault(x => x.IsActive && x.ReceiveDetailID == entity.ReceiveDetailID);
                    if (receiveDetail == null)
                    {
                        throw new HILIException("REC10001");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));

                    if (receiveDetail.ReceiveDetailStatus == ReceiveDetailStatusEnum.Cancel)
                    {
                        throw new HILIException("REC10009");
                    }
                    // throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10009));
                    else if (receiveDetail.ReceiveDetailStatus == ReceiveDetailStatusEnum.Close)
                    {
                        throw new HILIException("REC10008");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10008));
                    else if (receiveDetail.ReceiveDetailStatus == ReceiveDetailStatusEnum.Complete)
                    {
                        throw new HILIException("REC10011");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10011));


                    if (!string.IsNullOrEmpty(entity.PalletCode))
                    {
                        Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == entity.PalletCode);
                        if (receiving == null)
                        {
                            throw new HILIException("REC10011");
                        }
                        //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));


                        if (receiving != null)
                        {
                            if (!receiving.IsDraft)
                            {
                                throw new HILIException("REC10007");
                            }
                            //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10007));

                            receiving.Lot = entity.Lot;
                            receiving.Quantity = entity.Quantity;
                            receiving.ManufacturingDate = entity.ManufacturingDate;
                            receiving.ExpirationDate = entity.ExpirationDate;
                            receiving.Remark = entity.Remark;
                            receiving.ProductStatusID = entity.ProductStatusID;
                            receiving.ProductSubStatusID = entity.ProductSubStatusID;
                            receiving.ConversionQty = entity.ConversionQty;
                            receiving.BaseQuantity = entity.Quantity * entity.ConversionQty;
                            receiving.StockUnitID = entity.StockUnitID;
                            receiving.BaseUnitID = entity.BaseUnitID;
                            receiving.PackageWeight = receiveDetail.PackageWeight;
                            receiving.ProductWidth = receiveDetail.ProductWidth;
                            receiving.ProductLength = receiveDetail.ProductLength;
                            receiving.ProductHeight = receiveDetail.ProductHeight;
                            receiving.ProductWeight = receiveDetail.ProductWeight;

                            receiving.IsDraft = false;
                            receiving.DateModified = DateTime.Now;
                            receiving.UserModified = UserID;
                            receivingService.Modify(receiving);
                        }
                    }
                    else
                    {
                        int seq = 0;
                        List<Receiving> rcv = receivingService.Where(x => x.ReceiveDetailID == receiveDetail.ReceiveDetailID).ToList();
                        seq = rcv.Count + 1;

                        entity.ReceiveID = receiveDetail.ReceiveID;
                        entity.Price = receiveDetail.Price;
                        entity.ProductUnitPriceID = receiveDetail.ProductUnitPriceID;
                        entity.ReceivingStatus = ReceivingStatusEnum.Inprogress;
                        entity.Sequence = seq;
                        entity.PackageWeight = receiveDetail.PackageWeight;
                        entity.ProductWidth = receiveDetail.ProductWidth;
                        entity.ProductLength = receiveDetail.ProductLength;
                        entity.ProductHeight = receiveDetail.ProductHeight;
                        entity.ProductWeight = receiveDetail.ProductWeight;
                        entity.ProductID = receiveDetail.ProductID;
                        entity.BaseUnitID = receiveDetail.BaseUnitID;

                        entity.UserCreated = UserID;
                        entity.UserModified = UserID;
                        entity.DateCreated = DateTime.Now;
                        entity.DateModified = DateTime.Now;
                        entity.IsActive = true;
                        entity.IsDraft = false;
                        entity.BaseQuantity = entity.Quantity * entity.ConversionQty;
                        receivingService.Add(entity);
                    }


                    Receive receive = base.FindByID(receiveDetail.ReceiveID);
                    if (receive == null)
                    {
                        throw new HILIException("REC10001");
                    }
                    //throw new Exception(MessageManger.GetMessage(ReceiveLanguage.R10001));

                    if (receive.ReceiveStatus == ReceiveStatusEnum.New)
                    {
                        receive.ActualDate = DateTime.Now;
                        receive.ReceiveStatus = ReceiveStatusEnum.Inprogress;
                        receive.DateModified = DateTime.Now;
                        receive.UserModified = UserID;
                        base.Modify(receive);
                    }

                    List<Receiving> receivings = receivingService.Where(x => x.ReceiveDetailID == receiveDetail.ReceiveDetailID).ToList();
                    if (receivings.Sum(x => x.Quantity) >= receiveDetail.Quantity)
                    {
                        receiveDetail.ReceiveDetailStatus = ReceiveDetailStatusEnum.Complete;
                        receiveDetailService.Modify(receiveDetail);
                    }
                    else
                    {
                        receiveDetail.ReceiveDetailStatus = ReceiveDetailStatusEnum.Partial;
                        receiveDetailService.Modify(receiveDetail);
                    }


                    scope.Complete();
                }
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

        public void FinishReceiving(Guid id)
        {
            try
            {
                Receive receive = Where(x => x.ReceiveID == id).Include(x => x.ReceiveDetailCollection.Select(s => s.ReceivingCollection)).FirstOrDefault();

                List<Receiving> receiving = receivingService.Where(x => x.ReceiveID == id).ToList();
                int lastSeq = (receiving.Count == 0 ? 0 : receiving.Max(x => x.Sequence));
                lastSeq++;

                using (TransactionScope scope = new TransactionScope())
                {
                    receive.ReceiveDetailCollection.Where(x => x.IsActive).ToList()
                         .ForEach(item =>
                         {
                             item.ReceivingCollection.Where(x => x.IsActive && x.ReceivingStatus == ReceivingStatusEnum.Inprogress).ToList()
                             .ForEach(rItem =>
                             {
                                 rItem.GRNCode = receive.ReceiveCode + lastSeq.ToString("00");
                                 rItem.ReceivingStatus = ReceivingStatusEnum.WaitApprove;
                                 rItem.UserModified = UserID;
                                 rItem.DateModified = DateTime.Now;
                                 receivingService.Modify(rItem);

                                 //Finish Receive Detail
                                 if (rItem.Quantity >= item.Quantity)
                                 {
                                     item.ReceiveDetailStatus = ReceiveDetailStatusEnum.Complete;
                                     item.UserModified = UserID;
                                     item.DateModified = DateTime.Now;
                                     receiveDetailService.Modify(item);
                                 }
                                 else
                                 {
                                     item.ReceiveDetailStatus = ReceiveDetailStatusEnum.Partial;
                                 }
                             });
                         });

                    bool ok = receive.ReceiveDetailCollection.Where(x => x.IsActive).All(x => x.ReceiveDetailStatus == ReceiveDetailStatusEnum.Complete);
                    if (ok)
                    {
                        receive = base.FindByID(id);
                        receive.ReceiveStatus = ReceiveStatusEnum.Complete;
                        receive.UserModified = UserID;
                        receive.DateModified = DateTime.Now;
                        base.Modify(receive);
                    }

                    scope.Complete();
                }
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

        public void RemoveReceiving(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Receiving receiving = receivingService.FirstOrDefault(x => x.ReceivingID == id);
                    receiving.IsActive = false;
                    receiving.DateModified = DateTime.Now;
                    receiving.UserModified = UserID;
                    receivingService.Modify(receiving);

                    scope.Complete();
                }
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

        public List<Receiving> GetReceivingNo(Guid receiveid)
        {
            try
            {
                List<Receiving> grn = new List<Receiving>();
                receivingService.Where(x => x.IsActive && x.ReceiveID == receiveid && x.GRNCode != null)
                   .ToList().GroupBy(g => new
                   {
                       g.GRNCode,
                       g.ReceivingStatus
                   }).Select(n => new
                   {
                       n.Key.GRNCode,
                       n.Key.ReceivingStatus
                   }).OrderBy(x => x.GRNCode)
                     .ToList().ForEach(item =>
                     {
                         grn.Add(new Receiving { GRNCode = item.GRNCode, ReceivingStatus = item.ReceivingStatus });
                     });

                return grn;
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }

        }

        public List<Receiving> GetReceivingList(string grnNo)
        {
            try
            {
                List<Receiving> detail = (from receivedetail in receiveDetailService.Where(x=>x.IsActive)
                                          join receiving in receivingService.Where(x => x.IsActive) on receivedetail.ReceiveDetailID equals receiving.ReceiveDetailID
                                          join product in productService.Where(x => x.IsActive).Include(x => x.CodeCollection) on receivedetail.ProductID equals product.ProductID
                                          join stockunit in productUnitService.Where(x => x.IsActive) on receivedetail.StockUnitID equals stockunit.ProductUnitID
                                          join inventoryunit in productUnitService.Where(x => x.IsActive) on receivedetail.BaseUnitID equals inventoryunit.ProductUnitID
                                          join priceunit in productUnitService.Where(x => x.IsActive) on receivedetail.ProductUnitPriceID equals priceunit.ProductUnitID into g
                                          from priceUOM in g.DefaultIfEmpty()
                                          join productstatus in productStatusService.Where(x => x.IsActive) on receivedetail.ProductStatusID equals productstatus.ProductStatusID
                                          join productsubstatus in productSubStatusService.Where(x => x.IsActive) on receivedetail.ProductSubStatusID equals productsubstatus.ProductSubStatusID
                                          where receiving.GRNCode == grnNo
                                          select new { receivedetail, receiving, product, stockunit, inventoryunit, priceUOM, productstatus, productsubstatus })
                               .Select(n => new Receiving
                               {

                                   GRNCode = n.receiving.GRNCode,
                                   IsDraft = n.receiving.IsDraft,
                                   LocationID = n.receiving.LocationID,
                                   PalletCode = n.receiving.PalletCode,
                                   ProductUOM = n.receiving.ProductUOM,
                                   ReceiveDetailID = n.receiving.ReceiveDetailID,
                                   Sequence = n.receiving.Sequence,
                                   ConversionQty = n.receiving.ConversionQty,
                                   DateCreated = n.receiving.DateCreated,
                                   DateModified = n.receiving.DateModified,
                                   ExpirationDate = n.receiving.ExpirationDate,
                                   ReceivingID = n.receiving.ReceivingID,
                                   BaseUnitID = n.receivedetail.BaseUnitID,
                                   IsActive = n.receiving.IsActive,
                                   Lot = n.receiving.Lot,
                                   ManufacturingDate = n.receiving.ManufacturingDate,
                                   Quantity = n.receiving.Quantity,
                                   PackageWeight = n.receiving.PackageWeight,
                                   Price = n.receiving.Price,
                                   ProductHeight = n.receiving.ProductHeight,
                                   ProductLength = n.receiving.ProductLength,
                                   ProductStatusID = n.receiving.ProductStatusID,
                                   ProductID = n.receiving.ProductID,
                                   ProductSubStatusID = n.receiving.ProductSubStatusID,
                                   ProductUnitPriceID = n.receiving.ProductUnitPriceID,
                                   ProductWeight = n.receiving.ProductWeight,
                                   ProductWidth = n.receiving.ProductWidth,
                                   BaseQuantity = n.receiving.BaseQuantity,
                                   ReceiveID = n.receiving.ReceiveID,
                                   Remark = n.receiving.Remark,
                                   StockUnitID = n.receiving.StockUnitID,
                                   UserCreated = n.receiving.UserCreated,
                                   UserModified = n.receiving.UserModified,
                                   ReceivingStatus = n.receiving.ReceivingStatus,
                                   ProductBaseUOM = n.inventoryunit,
                                   ProductPriceUOM = n.priceUOM,
                                   Product = n.product,
                                   ProductCode = n.product.ProductCode,
                                   ProductStatus = n.productstatus,
                                   ProductSubStatus = n.productsubstatus,
                               }).ToList();

                return detail;
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

        /// <summary>
        /// Outging data
        /// </summary>
        /// <param name="data"></param>

        //private async Task<bool> OnSendData(string grnNo)
        //{
        //    try
        //    {
        //        var receiving = receivingService.Query().Filter(x => x.IsActive && x.GRNCode == grnNo).Include(x => x.ReceiveDetail.Receive).Get().ToList();

        //        var dataTrans = receiving.GroupBy(g => new
        //        {
        //            ReceiveID = g.ReceiveID,
        //            SupplierID = g.ReceiveDetail.Receive.SupplierID,
        //            ProductOwnerID = g.ReceiveDetail.Receive.ProductOwnerID,
        //            DocumentTypeID = g.ReceiveDetail.Receive.ReceiveTypeID,
        //            ReferenceCode = g.ReceiveDetail.Receive.ReceiveCode,
        //            LocationID = g.ReceiveDetail.Receive.LocationID,
        //            ProductID = g.ProductID,
        //            Lot = g.Lot,
        //            PalletCode = g.PalletCode,
        //            ExpirationDate = g.ExpirationDate,
        //            ManufacturingDate = g.ManufacturingDate,
        //            ProductWidth = g.ProductWidth,
        //            ProductLength = g.ProductLength,
        //            ProductHeight = g.ProductHeight,
        //            ProductWeight = g.ProductWeight,
        //            PackageWeight = g.PackageWeight,
        //            Remark = g.Remark,
        //            Price = g.Price,
        //            UnitPriceID = g.ProductUnitPriceID,
        //            StockUnitID = g.StockUnitID,
        //            BaseUnitID = g.BaseUnitID,
        //            ConversionQty = g.ConversionQty,
        //            ProductStatusID = g.ProductStatusID,
        //            ProductSubStatusID = g.ProductSubStatusID
        //        }).Select(n => new DataTransfer
        //        {
        //            PackagePrevID = this.InstanceID,
        //            InstanceID = this.InstanceID,
        //            ReferenceBaseID = n.Key.ReceiveID,
        //            ReferenceCode = n.Key.ReferenceCode,
        //            DocumentTypeID = n.Key.DocumentTypeID,
        //            ProductOwnerID = n.Key.ProductOwnerID,
        //            SupplierID = n.Key.SupplierID,
        //            LocationID = n.Key.LocationID,
        //            ProductID = n.Key.ProductID,
        //            Lot = n.Key.Lot,
        //            PalletCode = n.Key.PalletCode,
        //            ExpirationDate = n.Key.ExpirationDate,
        //            ManufacturingDate = n.Key.ManufacturingDate,
        //            ProductWidth = n.Key.ProductWidth,
        //            ProductHeight = n.Key.ProductHeight,
        //            ProductLength = n.Key.ProductLength,
        //            ProductWeight = n.Key.ProductWeight,
        //            PackageWeight = n.Key.PackageWeight,
        //            ProductStatusID = n.Key.ProductStatusID,
        //            ProductSubStatusID = n.Key.ProductSubStatusID,
        //            Price = n.Key.Price,
        //            ProductUnitPriceID = n.Key.UnitPriceID,
        //            BaseUnitID = n.Key.BaseUnitID,
        //            StockUnitID = n.Key.StockUnitID,
        //            Remark = n.Key.Remark,
        //            ConversionQty = n.Key.ConversionQty,
        //            Quantity = n.Sum(s => s.Quantity),
        //            BaseQuantity = n.Sum(s => s.BaseQuantity),
        //            Start = true
        //        }).ToList();


        //        stockService.Incomming(dataTrans);

        //        Engine engine = new Engine();
        //        var result = await engine.Transmit(dataTrans);
        //        return result;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Framework.Logging.LoBg(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}

        public List<ReceiveListModel> GetAll(DateTime? estDate, Guid lineID, ReceiveStatusEnum status, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                if (estDate == null)
                {
                    throw new HILIException("MSG00007");
                }

                //var tmpDate = DateTime.Parse(estDate.Value.ToString("yyyy/MM/dd 00:00:00"));

                IQueryable<ReceiveListModel> result = from rec in Where(x => x.IsActive && (status != 0 ? x.ReceiveStatus == status : true)
                                  && (estDate != null ? DbFunctions.TruncateTime(x.EstimateDate) == estDate : true))
                                                      join recd in receiveDetailService.Where(x => x.IsActive) on rec.ReceiveID equals recd.ReceiveID
                                                      join product in productService.Where(x => x.IsActive) on recd.ProductID equals product.ProductID
                                                      join docType in documentTypeService.Where(x => x.IsActive) on rec.ReceiveTypeID equals docType.DocumentTypeID
                                                      join itf in itfInterfaceMappingService.Where(x => x.IsActive) on docType.DocumentTypeID equals itf.DocumentId
                                                      join line in lineService.Where(x => x.IsActive) on rec.LineID equals line.LineID
                                                      where (lineID != Guid.Empty ? rec.LineID == lineID : true) && itf.IsNormal == true
                                                                 && (keyword != null ? rec.ReceiveCode.Contains(keyword) || product.Description.Contains(keyword) : true)
                                                      select (new ReceiveListModel()
                                                      {
                                                          ReceiveID = rec.ReceiveID,
                                                          ReceiveCode = rec.ReceiveCode,
                                                          ReceiveTypeName = docType.Name,
                                                          ReceiveDate = rec.EstimateDate,
                                                          ReceiveStatus = rec.ReceiveStatus.ToString(),
                                                          ReceiveStatusDesc = rec.ReceiveStatus.ToString(),
                                                          OrderNo = rec.Reference1,
                                                          LineCode = line.LineCode,
                                                          LineType = line.LineType,
                                                          ProductName = product.Name,
                                                          IsProduction = rec.ReceiveStatus == ReceiveStatusEnum.LoadIn
                                                      });

                totalRecords = 0;
                if (result.Count() == 0)
                {
                    return new List<ReceiveListModel>();
                }

                totalRecords = result.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ReceiveCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<ReceiveHeaderModel> GetAllInternalReceive(DateTime? estDate, ReceiveStatusEnum status, Guid receiveTypeID, string receiveCode, string orderNo, string PONo, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                IEnumerable<ReceiveHeaderModel> results = (from rcv in Where(x => x.IsActive
                                                          && (!(status != 0) || x.ReceiveStatus == status)
                                                          && (!(estDate != DateTime.MinValue) || DbFunctions.TruncateTime(x.EstimateDate) == estDate))
                                                           join map in itfInterfaceMappingService.Where(x => x.IsActive) on rcv.ReceiveTypeID equals map.DocumentId
                                                           join dt in documentTypeService.Where(x => x.IsActive) on rcv.ReceiveTypeID equals dt.DocumentTypeID
                                                           where ((map.FromReprocess ?? false)
                                                           || (map.ToReprocess ?? false)
                                                           || (map.ReferenceDocumentID != null)
                                                           || (map.IsWithoutGoods ?? false))
                                                           && (receiveTypeID == Guid.Empty || rcv.ReceiveTypeID == receiveTypeID)
                                                           select new ReceiveHeaderModel()
                                                           {
                                                               ReceiveID = rcv.ReceiveID,
                                                               ReceiveCode = rcv.ReceiveCode,
                                                               PONo = rcv.PONumber,
                                                               OrderNo = rcv.Reference1,
                                                               ReceiveTypeID = rcv.ReceiveTypeID,
                                                               ReceiveType = dt.Name,
                                                               ESTReceiveDate = rcv.EstimateDate,
                                                               ReceiveStatus = rcv.ReceiveStatus
                                                           }).ToList();

                if (!string.IsNullOrWhiteSpace(receiveCode))
                {
                    results = results.Where(x => !string.IsNullOrWhiteSpace(x.ReceiveCode) && x.ReceiveCode.Contains(receiveCode)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(PONo))
                {
                    results = results.Where(x => !string.IsNullOrWhiteSpace(x.PONo) && x.PONo.Contains(PONo)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(orderNo))
                {
                    results = results.Where(x => !string.IsNullOrWhiteSpace(x.OrderNo) && x.OrderNo.Contains(orderNo)).ToList();
                }
                totalRecords = results.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    results = results.OrderByDescending(x => x.ReceiveCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

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

        public ReceiveHeaderModel GetReceiveByID(Guid id)
        {
            try
            {
                IEnumerable<ItfInterfaceMapping> interfaceMapping = itfInterfaceMappingService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<DocumentType> documentType = documentTypeService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ProductStatusMapDocument> statusMapDoc = productStatusMapDocService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ProductStatus> pStatus = productStatusService.Query().Filter(x => x.IsActive).Get();

                IEnumerable<ReceiveHeaderModel> result = from rec in Query().Filter(x => x.ReceiveID == id)
                                    .Include(x => x.ReceiveDetailCollection).Get()
                                                         join docType in documentType on rec.ReceiveTypeID equals docType.DocumentTypeID
                                                         join pOwner in productOwnerService.Query().Get() on rec.ProductOwnerID equals pOwner.ProductOwnerID
                                                         join itfMap in interfaceMapping on rec.ReceiveTypeID equals itfMap.DocumentId
                                                         join supplier in contactService.Query().Get() on rec.SupplierID equals supplier.ContactID into _supplier
                                                         from supplier in _supplier.DefaultIfEmpty()
                                                         join location in locationService.Query().Get() on rec.LocationID equals location.LocationID into _location
                                                         from location in _location.DefaultIfEmpty()
                                                         join line in lineService.Query().Get() on rec.LineID equals line.LineID into _line
                                                         from line in _line.DefaultIfEmpty()
                                                         select (new ReceiveHeaderModel()
                                                         {
                                                             ReceiveID = rec.ReceiveID,
                                                             ReceiveCode = rec.ReceiveCode,
                                                             ReceiveTypeID = rec.ReceiveTypeID,
                                                             LocationID = rec.LocationID,
                                                             ReceiveStatus = rec.ReceiveStatus,
                                                             ProductOwnerID = rec.ProductOwnerID,
                                                             ESTReceiveDate = rec.EstimateDate,
                                                             InvoiceNo = rec.InvoiceNo,
                                                             ContainerNo = rec.ContainerNo,
                                                             PONo = rec.PONumber,
                                                             OrderNo = rec.Reference1,
                                                             OrderType = rec.Reference2,
                                                             Remark = rec.Remark,
                                                             IsUrgent = rec.IsUrgent,
                                                             LineID = rec.LineID,

                                                             Location = location?.Code,
                                                             ReceiveType = docType.Name,
                                                             LineCode = line.LineCode,
                                                             ProductOwner = pOwner.Description,
                                                             SupplierName = supplier?.Name,

                                                             IsNormal = itfMap.IsNormal,
                                                             ToReprocess = itfMap.ToReprocess,
                                                             FromReprocess = itfMap.FromReprocess,
                                                             IsCreditNote = itfMap.IsCreditNote,
                                                             IsWithoutGoods = itfMap.IsWithoutGoods,
                                                             IsItemChange = itfMap.IsItemChange,

                                                             ProductStatus = (from smd in statusMapDoc
                                                                              join ps in pStatus on smd.ProductStatusID equals ps.ProductStatusID into _ps
                                                                              from ps in _ps.DefaultIfEmpty()
                                                                              where smd.DocumentTypeID == rec.ReceiveTypeID && smd.IsDefault
                                                                              select ps).FirstOrDefault(),

                                                             ReceiveDetails = from recDetail in rec.ReceiveDetailCollection
                                                                              join pStat in pStatus on recDetail.ProductStatusID equals pStat.ProductStatusID into _pStatus
                                                                              from pStat in _pStatus.DefaultIfEmpty()
                                                                              join product in productService.Query().Get() on recDetail.ProductID equals product.ProductID
                                                                              join pCode in productCodeService.Query().Get() on product.ProductID equals pCode.ProductID
                                                                              join unit in productUnitService.Query().Get() on recDetail.StockUnitID equals unit.ProductUnitID
                                                                              where pCode.CodeType == ProductCodeTypeEnum.Stock && recDetail.IsActive == true
                                                                              select (new ReceiveDetailModel()
                                                                              {
                                                                                  ReceiveDetailID = recDetail.ReceiveDetailID,
                                                                                  ProductID = recDetail.ProductID,
                                                                                  ProductName = product.Name,
                                                                                  ProductCode = pCode.Code,
                                                                                  LotNo = recDetail.Lot,
                                                                                  MFGDate = recDetail.ManufacturingDate,
                                                                                  EXPDate = recDetail.ExpirationDate,
                                                                                  QTY = recDetail.Quantity,
                                                                                  RemainQTY = (recDetail.Quantity - receivingService.Query().Filter(x => x.ReceiveDetailID == recDetail.ReceiveDetailID && x.IsDraft).Get()?.Sum(x => x.Quantity) ?? 0),
                                                                                  PackageQTY = unit.Quantity,
                                                                                  ConfirmQTY = receivingService.Query().Filter(x => x.ReceiveDetailID == recDetail.ReceiveDetailID && x.IsDraft).Get()?.Sum(x => x.Quantity) ?? 0,
                                                                                  ConversionQTY = recDetail.ConversionQty,
                                                                                  UnitID = recDetail.StockUnitID,
                                                                                  Unit = unit.Name,
                                                                                  StatusID = recDetail.ProductStatusID,
                                                                                  Status = pStat?.Description,
                                                                                  Width = (decimal?)recDetail.ProductWidth,
                                                                                  Length = (decimal?)recDetail.ProductLength,
                                                                                  Height = (decimal?)recDetail.ProductHeight,
                                                                                  Remark = recDetail.Remark
                                                                              })
                                                         });

                if (result == null)
                {
                    throw new HILIException("MSG00006");
                }

                ReceiveHeaderModel output = result.FirstOrDefault();

                #region Find DispatchType 

                if (output.ReceiveTypeID != null)
                {
                    var itfDocType = (from itf in interfaceMapping
                                      join dt in documentType on itf.ReferenceDocumentID equals dt.DocumentTypeID into _documentType
                                      from _dt in _documentType.DefaultIfEmpty()
                                      where itf.DocumentId == output.ReceiveTypeID
                                      select new
                                      {
                                          ID = _dt?.DocumentTypeID,
                                          Name = _dt?.Name,
                                          //IsNormal = itf.IsNormal ?? false,
                                          //IsCreditNote = itf.IsCreditNote ?? false,
                                          //FromReProcess = itf.FromReprocess ?? false,
                                          //ToReprocess = itf.ToReprocess ?? false,
                                          //IsItemChange = itf.IsItemChange ?? false,
                                          //IsWithoutGoods = itf.IsWithoutGoods ?? false,
                                          //ProductStatus = (from smd in statusMapDoc
                                          //                 join ps in pStatus on smd.ProductStatusID equals ps.ProductStatusID into _ps
                                          //                 from ps in _ps.DefaultIfEmpty()
                                          //                 where smd.DocumentTypeID == output.ReceiveTypeID && smd.IsDefault
                                          //                 select ps).FirstOrDefault()
                                      }).FirstOrDefault();

                    output.DispatchTypeID = itfDocType?.ID;
                    output.DispatchType = itfDocType?.Name;
                    //output.IsNormal = itfDocType?.IsNormal;
                    //output.IsCreditNote = itfDocType?.IsCreditNote;
                    //output.FromReprocess = itfDocType?.FromReProcess;
                    //output.ToReprocess = itfDocType?.ToReprocess;
                    //output.IsItemChange = itfDocType?.IsItemChange;
                    //output.IsWithoutGoods = itfDocType?.IsWithoutGoods;
                    //output.ProductStatus = itfDocType?.ProductStatus;
                }

                #endregion

                return output;

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

        public bool Save(ReceiveHeaderModel receiveHeader)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (receiveHeader.ReceiveID == null || receiveHeader.ReceiveID == Guid.Empty)
                    {
                        throw new HILIException("MSG00005");
                    }

                    Receive current = FindByID(receiveHeader.ReceiveID);

                    if (current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    if (receiveHeader.ReceiveTypeID == null
                        || receiveHeader.LocationID == null
                        || receiveHeader.ProductOwnerID == null
                        || receiveHeader.ESTReceiveDate == null
                        || (receiveHeader.ESTReceiveDate.HasValue && receiveHeader.ESTReceiveDate.Value == DateTime.MinValue))
                    {
                        throw new HILIException("MSG00004");
                    }

                    current.ReceiveTypeID = receiveHeader.ReceiveTypeID.Value;
                    current.LocationID = receiveHeader.LocationID.Value;
                    current.ProductOwnerID = receiveHeader.ProductOwnerID.Value;
                    current.EstimateDate = receiveHeader.ESTReceiveDate.Value;
                    current.InvoiceNo = receiveHeader.InvoiceNo;
                    current.ContainerNo = receiveHeader.ContainerNo;
                    current.PONumber = receiveHeader.PONo;
                    current.Reference1 = receiveHeader.OrderNo;
                    current.Remark = receiveHeader.Remark;
                    current.IsUrgent = receiveHeader.IsUrgent ?? false;
                    base.Modify(current);

                    if (receiveHeader.ReceiveDetails != null)
                    {
                        foreach (ReceiveDetailModel item in receiveHeader.ReceiveDetails.Where(x => x.ReceiveDetailID != null))
                        {

                            if (item.StatusID == null || item.UnitID == null)
                            {
                                throw new HILIException("MSG00004");
                            }

                            #region Production Control Update

                            ProductionControl pControl = productionControlService.Query().Filter(x => x.ReferenceID == item.ReceiveDetailID).Get().FirstOrDefault();

                            IEnumerable<ProductionControlDetail> pcDetails = pcDetailService.Query().Filter(x => x.ReceiveDetailID == item.ReceiveDetailID).Get();
                            if (pControl != null)
                            {
                                if (pControl.PcControlStatus == (int)PCControlStatusEnum.Complete)
                                {
                                    throw new HILIException("MSG00100");
                                }

                                if (pcDetails.Sum(x => x.StockQuantity) > item.QTY)
                                {
                                    throw new HILIException("MSG00028"); // QTY must greater than Confirm QTY
                                }
                                else if (pcDetails.Sum(x => x.StockQuantity) == item.QTY)
                                {
                                    pControl.PcControlStatus = (int)PCControlStatusEnum.Complete;
                                }
                                else
                                {
                                    pControl.PcControlStatus = (int)PCControlStatusEnum.InProgress;
                                }

                                pControl.Quantity = item.QTY ?? 0;
                                pControl.UserModified = UserID;
                                pControl.DateModified = DateTime.Now;
                                productionControlService.Modify(pControl);
                            }

                            #endregion

                            ReceiveDetail detailCurrent = receiveDetailService.FindByID(item.ReceiveDetailID);
                            detailCurrent.Quantity = item.QTY ?? 0;
                            detailCurrent.BaseQuantity = (item.QTY ?? 0) * (item.ConversionQTY ?? 0);
                            detailCurrent.Remark = item.Remark;
                            detailCurrent.Lot = item.LotNo;
                            detailCurrent.ManufacturingDate = item.MFGDate;
                            detailCurrent.ExpirationDate = item.EXPDate;
                            detailCurrent.ProductStatusID = item.StatusID ?? Guid.Parse("02321264-C9F4-E611-93FF-0050569135DB");
                            detailCurrent.StockUnitID = item.UnitID.Value;

                            detailCurrent.IsActive = true;
                            detailCurrent.UserModified = UserID;
                            detailCurrent.DateModified = DateTime.Now;

                            receiveDetailService.Modify(detailCurrent);

                        }
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

        public bool SaveInternalReceive(ReceiveHeaderModel receiveHeader)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(20)))
            {
                try
                {
                    SaveInternalReceive_(receiveHeader);
                    scope.Complete();
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

        private Guid SaveInternalReceive_(ReceiveHeaderModel receiveHeader)
        {
            /* try
             {*/
            int i = 1;
            Guid receiveID = receiveHeader.ReceiveID;

            /*using (TransactionScope scope = new TransactionScope())
            {*/
            Guid? productOwnerID = productOwnerService.FirstOrDefault(e => e.IsActive)?.ProductOwnerID;
            Guid? supplierID = contactService.FirstOrDefault(x => x.IsActive && x.Code == "20004431")?.ContactID;
            Guid? pSubStatusID = productSubStatusService.FirstOrDefault(x => x.Code == "SS000")?.ProductSubStatusID;
            Guid? dummyLocationID = (from pl in lineService.Where(x => x.LineID == receiveHeader.LineID)
                                     join z in zoneService.Where(x => x.IsActive) on pl.WarehouseID equals z.WarehouseID
                                     join l in locationService.Where(x => x.IsActive) on z.ZoneID equals l.ZoneID
                                     where l.LocationType == LocationTypeEnum.Dummy
                                     select l).FirstOrDefault()?.LocationID;

            if (dummyLocationID == null)
            {
                throw new HILIException("MSG00066"); // DummyLocation not found
            }

            if (productOwnerID == null || supplierID == null)
            {
                throw new HILIException("MSG00006"); // Data not found
            }

            if (receiveHeader.ReceiveID != null && receiveHeader.ReceiveID != Guid.Empty)
            {
                // Edit
                #region Receive Edit

                Receive receive = FindByID(receiveHeader.ReceiveID);
                receive.PONumber = receiveHeader.PONo;
                receive.EstimateDate = receiveHeader.ESTReceiveDate.Value;
                receive.Reference1 = receiveHeader.OrderNo;
                receive.Reference2 = receiveHeader.OrderType;
                receive.InvoiceNo = receiveHeader.InvoiceNo;
                receive.Remark = receiveHeader.Remark;

                receive.UserModified = UserID;
                receive.DateModified = DateTime.Now;

                base.Modify(receive);

                #endregion

                #region Receive Details Edit

                List<Guid> receiveDetailExist = new List<Guid>();

                if (receiveHeader.ReceiveDetails != null && receiveHeader.ReceiveDetails.Count() > 0)
                {
                    foreach (ReceiveDetailModel item in receiveHeader.ReceiveDetails)
                    {
                        Product product = productService.Where(x => x.ProductID == item.ProductID && x.IsActive).Include(x => x.UnitCollection).FirstOrDefault();
                        ProductUnit stockUnit = productUnitService.Where(x => x.ProductUnitID == item.UnitID && x.IsActive).FirstOrDefault();

                        decimal baseQty = 0;
                        if (product == null || stockUnit == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        #region Unit Conversion

                        if (stockUnit.ConversionMark == null || stockUnit.ConversionMark != 2)
                        {
                            baseQty = Math.Round((item.QTY ?? 1) * stockUnit.Quantity, 2);
                        }
                        else
                        {
                            baseQty = Math.Round((item.QTY ?? 1) / stockUnit.Quantity, 2);
                        }

                        #endregion

                        IEnumerable<ReceiveDetail> details = receiveDetailService.Where(x => x.ReceiveID == receiveHeader.ReceiveID);
                        int maxSeq = details.Max(x => x.Sequence);
                        ReceiveDetail detail = details.FirstOrDefault(x => x.ReceiveDetailID == item.ReceiveDetailID);
                        if (detail != null)
                        {
                            #region Edit if Exist in database 
                            detail.ManufacturingDate = item.MFGDate;
                            detail.ExpirationDate = item.MFGDate.Value.AddDays(product.Age);
                            detail.Lot = item.LotNo;
                            detail.Remark = item.Remark;

                            detail.ProductWidth = stockUnit.Width;
                            detail.PackageWeight = stockUnit.PackageWeight;
                            detail.ProductWeight = stockUnit.ProductWeight;
                            detail.ProductLength = stockUnit.Length;
                            detail.ProductHeight = stockUnit.Height;
                            detail.ConversionQty = stockUnit.Quantity;

                            detail.Quantity = item.QTY ?? 0;
                            detail.BaseQuantity = baseQty;

                            detail.UserModified = UserID;
                            detail.DateModified = DateTime.Now;

                            receiveDetailService.Modify(detail);

                            #endregion
                        }
                        else
                        {
                            #region Add if not Exist in database

                            detail = new ReceiveDetail()
                            {
                                Sequence = maxSeq + 1,
                                ProductWidth = stockUnit.Width,
                                PackageWeight = stockUnit.PackageWeight,
                                ProductWeight = stockUnit.ProductWeight,
                                ProductLength = stockUnit.Length,
                                ProductHeight = stockUnit.Height,
                                ConversionQty = stockUnit.Quantity,
                                BaseUnitID = product.UnitCollection.FirstOrDefault(x => x.IsBaseUOM).ProductUnitID,
                                BaseQuantity = baseQty,
                                SupplierID = supplierID,
                                ReceiveID = receive.ReceiveID,
                                ProductOwnerID = productOwnerID,
                                ProductSubStatusID = pSubStatusID,
                                ReceiveDetailStatus = ReceiveDetailStatusEnum.New,

                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now,

                                Lot = item.LotNo,
                                ProductID = item.ProductID.Value,
                                Quantity = item.QTY ?? 0,
                                ManufacturingDate = item.MFGDate,
                                Remark = item.Remark,
                                ProductStatusID = item.StatusID.Value,
                                StockUnitID = item.UnitID.Value,
                                ExpirationDate = item.MFGDate.Value.AddDays(product.Age),
                            };

                            receiveDetailService.Add(detail);

                            #endregion
                        }

                        receiveDetailExist.Add(detail.ReceiveDetailID);
                    }
                }

                #region Internal Receive Detail Delete

                List<Guid> recDetailExcept = receiveDetailService.Where(x => x.ReceiveID == receiveHeader.ReceiveID).Select(x => x.ReceiveDetailID).ToList();
                List<Guid> recDetailforDeletes = new List<Guid>();

                if (receiveDetailExist.Count() > 0)
                {
                    recDetailforDeletes = recDetailExcept.Except(receiveDetailExist).ToList();
                }
                else
                {
                    recDetailforDeletes = recDetailExcept.ToList();
                }

                if (recDetailforDeletes.Count() > 0)
                {
                    foreach (Guid recDetailforDelete in recDetailforDeletes)
                    {
                        ReceiveDetail detail = receiveDetailService.FirstOrDefault(x => x.ReceiveDetailID == recDetailforDelete);
                        detail.IsActive = false;
                        detail.UserModified = UserID;
                        detail.DateModified = DateTime.Now;
                        receiveDetailService.Modify(detail);
                    }
                }

                #endregion

                #endregion

            }
            else
            {
                // Add
                #region Receive Prefix

                ReceivePrefix prefix = prefixService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                if (prefix == null)
                {
                    throw new HILIException("REC10012");
                }

                ReceivePrefix tPrefix = prefixService.FindByID(prefix.PrefixID);
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

                prefixService.Add(newPrefix);
                prefixService.Modify(tPrefix);

                #endregion

                #region Receive Add

                Receive receive = new Receive()
                {
                    ReceiveCode = receiveCode,
                    ReceiveTypeID = receiveHeader.ReceiveTypeID.Value,
                    ReceiveStatus = receiveHeader.ReceiveStatus.Value,
                    EstimateDate = receiveHeader.ESTReceiveDate.Value,
                    Reference1 = receiveHeader.OrderNo,
                    Reference2 = receiveHeader.OrderType,
                    IsUrgent = receiveHeader.IsUrgent ?? false,
                    LineID = receiveHeader.LineID,
                    PONumber = receiveHeader.PONo,
                    InvoiceNo = receiveHeader.InvoiceNo,
                    ContainerNo = receiveHeader.ContainerNo,
                    Remark = receiveHeader.Remark,

                    IsActive = true,
                    UserCreated = UserID,
                    DateCreated = DateTime.Now,
                    UserModified = UserID,
                    DateModified = DateTime.Now,

                    IsCrossDock = false,
                    ProductOwnerID = productOwnerID.Value,
                    SupplierID = supplierID.Value,
                    LocationID = dummyLocationID,
                };

                base.Add(receive);

                #endregion

                #region Receive Detail ADD

                if (receiveHeader.ReceiveDetails != null && receiveHeader.ReceiveDetails.Count() > 0)
                {
                    List<ReceiveDetail> rDetails = new List<ReceiveDetail>();

                    foreach (ReceiveDetailModel item in receiveHeader.ReceiveDetails)
                    {
                        Product product = productService.Where(x => x.ProductID == item.ProductID && x.IsActive).Include(x => x.UnitCollection).FirstOrDefault();
                        ProductUnit stockUnit = productUnitService.Where(x => x.ProductUnitID == item.UnitID && x.IsActive).FirstOrDefault();
                        decimal baseQty = 0;
                        if (product == null || stockUnit == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        #region Unit Conversion

                        if (stockUnit.ConversionMark == null || stockUnit.ConversionMark != 2)
                        {
                            baseQty = Math.Round((item.QTY ?? 1) * stockUnit.Quantity, 2);
                        }
                        else
                        {
                            baseQty = Math.Round((item.QTY ?? 1) / stockUnit.Quantity, 2);
                        }

                        #endregion

                        rDetails.Add(new ReceiveDetail()
                        {
                            Sequence = i,
                            ProductWidth = stockUnit.Width,
                            PackageWeight = stockUnit.PackageWeight,
                            ProductWeight = stockUnit.ProductWeight,
                            ProductLength = stockUnit.Length,
                            ProductHeight = stockUnit.Height,
                            ConversionQty = stockUnit.Quantity,

                            BaseUnitID = product.UnitCollection.Where(x => x.IsBaseUOM).Select(x => x.ProductUnitID).FirstOrDefault(),

                            BaseQuantity = baseQty,
                            SupplierID = supplierID,
                            ReceiveID = receive.ReceiveID,
                            ProductOwnerID = productOwnerID,
                            ProductSubStatusID = pSubStatusID,
                            ReceiveDetailStatus = ReceiveDetailStatusEnum.New,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now,

                            Lot = item.LotNo,
                            ProductID = item.ProductID.Value,
                            Quantity = item.QTY ?? 0,
                            ManufacturingDate = item.MFGDate,
                            Remark = item.Remark,
                            ProductStatusID = item.StatusID.Value,
                            StockUnitID = item.UnitID.Value,
                            ExpirationDate = item.MFGDate.Value.AddDays(product.Age),
                        });

                        i++;
                    }

                    receiveDetailService.AddRange(rDetails);
                }

                #endregion

                receiveID = receive.ReceiveID;
            }

            // scope.Complete();
            //}

            return receiveID;
            /* }
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
             }*/
        }

        public bool ConfirmInternalReceive(ReceiveHeaderModel receiveHeader)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(20)))
                {
                    int seq = 1;
                    TimeSpan timeStamp = DateTime.Now.TimeOfDay;
                    CultureInfo cultureinfo = new CultureInfo("en-US");
                    ReceiveStatusEnum receiveStatus = ReceiveStatusEnum.Complete;
                    Guid receiveID = SaveInternalReceive_(receiveHeader);

                    List<Guid> rDetailGUIDs = new List<Guid>();
                   // IEnumerable<Location> locations = locationService.Where(x => x.IsActive).ToList();
                    IEnumerable<Receive> receives = Where(x => x.IsActive && x.ReceiveID == receiveID 
                                                                          && (x.ReceiveStatus != ReceiveStatusEnum.Check 
                                                                          && x.ReceiveStatus != ReceiveStatusEnum.Complete)).ToList();
                    Line line = lineService.FirstOrDefault(x => x.IsActive && x.LineID == receiveHeader.LineID);
                    Guid? warehouseID = line?.WarehouseID;
                    if (receives == null || receives.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }
                    if (warehouseID == null || line == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    ItfInterfaceMapping type = (from r in receives
                                                join itf in itfInterfaceMappingService.Where(x => x.IsActive) on r.ReceiveTypeID equals itf.DocumentId
                                                select itf).FirstOrDefault(); 
                    var models = from r in receives
                                 join l in locationService.Where(x => x.IsActive) on r.LocationID equals l.LocationID
                                 join rd in receiveDetailService.Where(x => x.IsActive) on r.ReceiveID equals rd.ReceiveID
                                 join unit in productUnitService.Where(x => x.IsActive) on rd.StockUnitID equals unit.ProductUnitID into Gunit
                                 select new { r, rd, l, unit = Gunit.FirstOrDefault() };

                    //var results = (from r in receives
                    //               join rd in receiveDetailService.Where(x => x.IsActive) on r.ReceiveID equals rd.ReceiveID
                    //               join pc in productionControlService.Where(x => x.IsActive) on rd.ProductID equals pc.ProductID
                    //               join pcd in pcDetailService.Where(x => x.IsActive) on new { pc.ControlID, pc.StockUnitID, pc.BaseUnitID }
                    //               equals new { pcd.ControlID, pcd.StockUnitID, pcd.BaseUnitID }
                    //               select new { pc = pc, pcd = pcd }).ToList();
                    //IEnumerable<ProductionControl> pControls = results.Select(e => e.pc).Distinct().ToList();
                    //IEnumerable<ProductionControlDetail> pcDetails = results.Select(e => e.pcd).Distinct().ToList();

                    IEnumerable<ProductionControl> pControls = (from r in receives
                                                                join rd in receiveDetailService.Where(x => x.IsActive) on r.ReceiveID equals rd.ReceiveID
                                                                join p in productionControlService.Where(x => x.IsActive) on rd.ProductID equals p.ProductID
                                                                select p).Distinct().ToList();

                    IEnumerable<ProductionControlDetail> pcDetails = (from pc in pControls
                                                                      join pcd in pcDetailService.Where(x => x.IsActive) on
                                                                      new { pc.ControlID, pc.StockUnitID, pc.BaseUnitID }
                                                                      equals new { pcd.ControlID, pcd.StockUnitID, pcd.BaseUnitID } 
                                                                      select pcd).Distinct().ToList();                    
                    List<GoodsReturnDetail> goodsReturnDetails = new List<GoodsReturnDetail>(); 

                    #region Handle Internal Receive Case I - III

                    if (!(type.IsWithoutGoods ?? false))
                    {
                        if ((type.IsNormal.Value == false) && (type.FromReprocess.Value == false) && (type.ToReprocess.Value == true))
                        {
                            #region CASE II : Internal Recieve to Reprocess

                            #region Goods Return

                            #region Goods Return Prefix

                            GoodsReturnPrefix prefix = goodsReturnPrefixService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                            if (prefix == null)
                            {
                                throw new HILIException("MSG00063"); // Prefix not found
                            }
                            GoodsReturnPrefix tPrefix = goodsReturnPrefixService.FindByID(prefix.PrefixID);

                            string goodsReturnCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                            tPrefix.IsLastest = false;

                            GoodsReturnPrefix newPrefix = new GoodsReturnPrefix()
                            {
                                IsLastest = true,
                                LastedKey = goodsReturnCode,
                                PrefixKey = prefix.PrefixKey,
                                FormatKey = prefix.FormatKey,
                                LengthKey = prefix.LengthKey
                            };
                            goodsReturnPrefixService.Add(newPrefix);
                            goodsReturnPrefixService.Modify(tPrefix);

                            #endregion

                            GoodsReturn goodsReturn = new GoodsReturn()
                            {
                                ReceiveID = receiveID,
                                GoodsReturnCode = goodsReturnCode,
                                GoodsReturnStatus = GoodsReturnStatusEnum.QA_Inspection,

                                IsActive = true,
                                UserCreated = UserID,
                                DateCreated = DateTime.Now,
                                UserModified = UserID,
                                DateModified = DateTime.Now
                            };
                            GoodsReturnService.Add(goodsReturn);

                            #endregion

                            foreach (var item in models)
                            {
                                decimal palletQTY = item.unit?.PalletQTY ?? 1; 
                                #region Production Control Add 
                                ProductionControl pControl = new ProductionControl
                                {
                                    LineID = item.r.LineID.Value,
                                    OrderNo = item.r.Reference1,
                                    ProductID = item.rd.ProductID,
                                    ProductUnitID = item.rd.StockUnitID,
                                    Quantity = item.rd.Quantity,
                                    StockUnitID = item.rd.StockUnitID,
                                    ConversionQty = item.rd.ConversionQty,
                                    BaseUnitID = item.rd.BaseUnitID,
                                    ProductStatusID = item.rd.ProductStatusID,
                                    ProductSubStatusID = item.rd.ProductSubStatusID,
                                    ReferenceID = item.rd.ReceiveDetailID,
                                    ProductionDate = item.rd.ManufacturingDate.Value.Date,
                                    ProductionTime = item.rd.ManufacturingDate.Value.TimeOfDay,
                                    Lot = item.rd.ManufacturingDate != null ? item.rd.ManufacturingDate.Value.ToString("yyyyMMdd", cultureinfo) : null,
                                    Remark = "Internal Receive : " + item.r.ReceiveCode,
                                    PcControlStatus = (int)PCControlStatusEnum.New,
                                    StandardPalletQty = palletQTY,
                                    OrderType = "LOCAL",
                                    IsActive = true,
                                    UserCreated = UserID,
                                    DateCreated = DateTime.Now,
                                    UserModified = UserID,
                                    DateModified = DateTime.Now
                                };

                                productionControlService.Add(pControl);

                                #endregion                                 

                                #region Goods Return Detail

                                goodsReturnDetails.Add(new GoodsReturnDetail()
                                {
                                    GoodsReturnID = goodsReturn.GoodsReturnID,
                                    ProductID = item.rd.ProductID,
                                    ReceiveQTY = item.rd.Quantity,
                                    ReceiveBaseQTY = item.rd.BaseQuantity,
                                    ConversionQTY = item.rd.ConversionQty,
                                    ReceiveUnitID = item.rd.StockUnitID,
                                    ReceiveBaseUnitID = item.rd.BaseUnitID,
                                    MFGDate = item.rd.ManufacturingDate,
                                    ReceiveLot = item.rd.Lot,
                                    ReceiveDetailID = item.rd.ReceiveDetailID,
                                    LineID = item.r.LineID,

                                    IsActive = true,
                                    UserCreated = UserID,
                                    DateCreated = DateTime.Now,
                                    UserModified = UserID,
                                    DateModified = DateTime.Now
                                });
                                #endregion
                            }

                            GoodsReturnDetailService.AddRange(goodsReturnDetails);
                            receiveStatus = ReceiveStatusEnum.Check;

                            #endregion
                        }
                        else if ((type.IsNormal.Value == false) && (type.FromReprocess.Value == true) && (type.ToReprocess.Value == false))
                        {
                            #region CASE III : Internal Recieve Repack_Reprocess

                            foreach (var item in models)
                            {
                                decimal palletQTY = item.unit?.PalletQTY ?? 1;
                                decimal stockQTY = item.rd.Quantity;
                                decimal remainQTY = stockQTY;
                                decimal palletCount = Math.Ceiling(stockQTY / palletQTY);

                                string tmpLot = item.rd.ManufacturingDate != null ? item.rd.ManufacturingDate.Value.ToString("yyyyMMdd", cultureinfo) : null;

                                #region Find Lastest Sequence if exist

                                IEnumerable<ProductionControlDetail> existPCDetail = from pc in pControls
                                                                                     join pcd in pcDetails on pc.ControlID equals pcd.ControlID
                                                                                     where (tmpLot != null ? pc.Lot == tmpLot : false)
                                                                                         && pc.ProductID == item.rd.ProductID
                                                                                         && pc.StockUnitID == item.rd.StockUnitID
                                                                                         && pc.BaseUnitID == item.rd.BaseUnitID
                                                                                     select pcd;

                                int lastestSequence = existPCDetail.Max(x => x.Sequence) ?? 0;

                                #endregion

                                #region Production Control Add

                                ProductionControl pControl = new ProductionControl
                                {
                                    LineID = item.r.LineID.Value,
                                    OrderNo = item.r.Reference1,
                                    ProductID = item.rd.ProductID,
                                    ProductUnitID = item.rd.StockUnitID,
                                    Quantity = item.rd.Quantity,
                                    StockUnitID = item.rd.StockUnitID,
                                    ConversionQty = item.rd.ConversionQty,
                                    BaseUnitID = item.rd.BaseUnitID,
                                    ProductStatusID = item.rd.ProductStatusID,
                                    ProductSubStatusID = item.rd.ProductSubStatusID,
                                    ReferenceID = item.rd.ReceiveDetailID,
                                    ProductionDate = item.rd.ManufacturingDate.Value.Date,
                                    ProductionTime = item.rd.ManufacturingDate.Value.TimeOfDay,
                                    Lot = tmpLot,
                                    Remark = "Internal Receive : " + item.r.ReceiveCode,
                                    PcControlStatus = (int)PCControlStatusEnum.Complete,
                                    StandardPalletQty = palletQTY,
                                    OrderType = item.r.Reference2,
                                    IsActive = true,
                                    UserCreated = UserID,
                                    DateCreated = DateTime.Now,
                                    UserModified = UserID,
                                    DateModified = DateTime.Now
                                };
                                productionControlService.Add(pControl);

                                #endregion

                                List<ProductionControlDetail> packingDetails = new List<ProductionControlDetail>();
                                List<StockInOutModel> stockList = new List<StockInOutModel>();

                                double timeSeq = 5;

                                for (int i = 1; i <= palletCount; i++)
                                {
                                    timeStamp = timeStamp.Add(TimeSpan.FromSeconds(timeSeq));

                                    #region Find Quantity Per Pallet

                                    decimal QTYperPallet;

                                    if ((remainQTY - palletQTY) >= 0)
                                    {
                                        remainQTY -= palletQTY;
                                        QTYperPallet = palletQTY;
                                    }
                                    else
                                    {
                                        QTYperPallet = remainQTY;
                                    }

                                    #endregion

                                    #region Add model Packing Detail 

                                    ProductionControlDetail packingDetail = new ProductionControlDetail()
                                    {
                                        ControlID = pControl.ControlID,
                                        PalletCode = pControl.ProductionDate.ToString("yyyyMMdd", cultureinfo)
                                                            + line.LineCode
                                                            + (lastestSequence + i).ToString("000")
                                                            + timeStamp.ToString("hhmmss"),
                                        LotNo = pControl.ProductionDate.ToString("yyyyMMdd", cultureinfo),
                                        Sequence = (lastestSequence + i),
                                        ConversionQty = pControl.ConversionQty,
                                        StockUnitID = pControl.StockUnitID,
                                        BaseUnitID = pControl.BaseUnitID,
                                        RemainStockUnitID = pControl.StockUnitID,
                                        RemainBaseUnitID = pControl.BaseUnitID,

                                        MFGDate = pControl.ProductionDate,
                                        MFGTimeStart = pControl.PCDetailCollection.OrderByDescending(x => x.Sequence).FirstOrDefault()?.MFGTimeEnd ?? timeStamp,
                                        MFGTimeEnd = timeStamp,
                                        PackingStatus = PackingStatusEnum.Waiting_Receive,
                                        WarehouseID = warehouseID,
                                        RemainQTY = 0,
                                        RemainBaseQTY = 0,
                                        ReserveQTY = 0,
                                        ReserveBaseQTY = 0,
                                        StockQuantity = QTYperPallet,
                                        BaseQuantity = (QTYperPallet * pControl.ConversionQty),

                                        ProductStatusID = item.rd.ProductStatusID,
                                        ProductSubStatusID = item.rd.ProductSubStatusID,
                                        ReceiveDetailID = item.rd.ReceiveDetailID,

                                        LocationID = item.r.LocationID,

                                        Remark = "Internal Receive",
                                        IsNormal = true,
                                        IsActive = true,
                                        UserCreated = UserID,
                                        DateCreated = DateTime.Now,
                                        UserModified = UserID,
                                        DateModified = DateTime.Now,
                                    };

                                    packingDetails.Add(packingDetail);

                                    #endregion

                                    #region Receiving

                                    Receive tmpR = item.r;
                                    ReceiveDetail tmpRD = item.rd;
                                    ReceivingAdd(packingDetail, tmpR, tmpRD);
                                    #endregion
                                }
                                pcDetailService.AddRange(packingDetails);
                            }

                            #endregion
                        }
                        else if ((type.IsNormal.Value == false) && (type.FromReprocess.Value == false) && (type.ToReprocess.Value == false))
                        {
                            #region CASE I : Internal Receive from QA Inspection

                            #region StockIn

                            List<StockInOutModel> stockInOutModel = (from model in models
                                                                     select new StockInOutModel()
                                                                     {
                                                                         LocationCode = model.l.Code,

                                                                         SupplierID = model.r.SupplierID,
                                                                         ProductOwnerID = model.r.ProductOwnerID,

                                                                         ProductID = model.rd.ProductID,
                                                                         StockUnitID = model.rd.StockUnitID,
                                                                         BaseUnitID = model.rd.BaseUnitID,
                                                                         Quantity = model.rd.Quantity,
                                                                         ConversionQty = model.rd.ConversionQty,
                                                                         ManufacturingDate = model.rd.ManufacturingDate.Value,
                                                                         ExpirationDate = model.rd.ExpirationDate.Value,
                                                                         ProductStatusID = model.rd.ProductStatusID,
                                                                         ProductSubStatusID = model.rd.ProductSubStatusID.Value,
                                                                         DocumentCode = model.r.ReceiveCode,
                                                                         DocumentID = model.rd.ReceiveDetailID,
                                                                         DocumentTypeID = model.r.ReceiveTypeID,
                                                                         ProductWidth = model.rd.ProductWidth,
                                                                         PackageWeight = model.rd.PackageWeight,
                                                                         ProductLength = model.rd.ProductLength,
                                                                         ProductHeight = model.rd.ProductHeight,
                                                                         ProductWeight = model.rd.ProductWeight,
                                                                         Lot = model.rd.Lot,
                                                                         Remark = "InternalReceive"
                                                                     }).ToList();

                            stockService.UserID = UserID;
                            stockService.Incomming2(stockInOutModel, unitofwork);

                            #endregion

                            #region Receiving

                            List<Receiving> receivings = new List<Receiving>();

                            seq = 1;

                            foreach (var item in models)
                            {
                                receivings.Add(new Receiving()
                                {
                                    Sequence = seq,
                                    GRNCode = item.r.ReceiveCode + item.rd.Sequence,
                                    ReceiveID = item.r.ReceiveID,
                                    ReceiveDetailID = item.rd.ReceiveDetailID,
                                    IsDraft = true,
                                    ReceivingStatus = ReceivingStatusEnum.Complete,
                                    ProductID = item.rd.ProductID,
                                    Lot = item.rd.Lot,
                                    ManufacturingDate = item.rd.ManufacturingDate,
                                    ExpirationDate = item.rd.ExpirationDate,
                                    Quantity = item.rd.Quantity,
                                    BaseQuantity = item.rd.BaseQuantity,
                                    ConversionQty = item.rd.ConversionQty,
                                    StockUnitID = item.rd.StockUnitID,
                                    BaseUnitID = item.rd.BaseUnitID,
                                    ProductStatusID = item.rd.ProductStatusID,
                                    ProductSubStatusID = item.rd.ProductSubStatusID,
                                    PackageWeight = 1,
                                    ProductWeight = 1,
                                    ProductWidth = 1,
                                    ProductLength = 1,
                                    ProductHeight = 1,
                                    PalletCode = null, // in-case not simulate pallet
                                    LocationID = item.r.LocationID, // DummyLocation
                                    Remark = "Recieve from QA",
                                    IsActive = true,
                                    IsSentInterface = false,
                                    UserCreated = UserID,
                                    DateCreated = DateTime.Now,
                                    UserModified = UserID,
                                    DateModified = DateTime.Now,
                                    ProductOwnerID = item.r.ProductOwnerID,
                                    SupplierID = item.r.SupplierID,
                                });

                                rDetailGUIDs.Add(item.rd.ReceiveDetailID);
                                seq++;
                            }

                            receivingService.AddRange(receivings);

                            #endregion

                            #endregion
                        }
                    }

                    #endregion

                    #region Update Status Receive & ReceiveDeatil

                    Receive recUpdate = FindByID(receiveID);
                    recUpdate.ReceiveStatus = receiveStatus;
                    recUpdate.ActualDate = DateTime.Now;
                    recUpdate.UserModified = UserID;
                    recUpdate.DateModified = DateTime.Now;
                    base.Modify(recUpdate);

                    List<ReceiveDetail> rDetailUpdate = receiveDetailService.Where(x => x.IsActive && rDetailGUIDs.Contains(x.ReceiveDetailID)).ToList();
                    var updateIds = rDetailUpdate.Select(e => e.ReceiveDetailID);
                    var details = receiveDetailService.Where(e => e.IsActive && updateIds.Contains(e.ReceiveDetailID)).ToList();

                    details.ForEach(x =>
                    {
                        ReceiveDetail ttt = receiveDetailService.FindByID(x.ReceiveDetailID);
                        ttt.ReceiveDetailStatus = ReceiveDetailStatusEnum.Complete;
                        ttt.UserModified = UserID;
                        ttt.DateModified = DateTime.Now;
                        receiveDetailService.Modify(ttt);
                    });

                    #endregion

                    scope.Complete();
                    return true;
                }
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

        private void ReceivingAdd(ProductionControlDetail packingDetail, Receive tmpR, ReceiveDetail tmpRD)
        {
            var tmpData = new
            {
                Sequence = tmpRD.Sequence,
                StockQuantity = tmpRD.Quantity,
                BaseQuantity = tmpRD.BaseQuantity,
                PalletCode = string.Empty,
            };

            if (packingDetail != null)
            {
                tmpData = new
                {
                    Sequence = packingDetail.Sequence ?? 1,
                    StockQuantity = packingDetail.StockQuantity ?? 0,
                    BaseQuantity = packingDetail.BaseQuantity ?? 0,
                    PalletCode = packingDetail.PalletCode,
                };
            }

            Receiving receiving = new Receiving()
            {
                Sequence = tmpData.Sequence,
                GRNCode = tmpR.ReceiveCode + tmpData.Sequence,
                Quantity = tmpData.StockQuantity,
                BaseQuantity = tmpData.BaseQuantity,
                PalletCode = tmpData.PalletCode,

                ReceiveID = tmpR.ReceiveID,
                ProductOwnerID = tmpR.ProductOwnerID,
                SupplierID = tmpR.SupplierID,
                LocationID = tmpR.LocationID, // DummyLocation

                ReceiveDetailID = tmpRD.ReceiveDetailID,
                ProductID = tmpRD.ProductID,
                Lot = tmpRD.Lot,
                ManufacturingDate = tmpRD.ManufacturingDate,
                ExpirationDate = tmpRD.ExpirationDate,
                ConversionQty = tmpRD.ConversionQty,
                StockUnitID = tmpRD.StockUnitID,
                BaseUnitID = tmpRD.BaseUnitID,
                ProductStatusID = tmpRD.ProductStatusID,
                ProductSubStatusID = tmpRD.ProductSubStatusID,

                IsDraft = true,
                IsSentInterface = false,
                ReceivingStatus = ReceivingStatusEnum.Inprogress,
                PackageWeight = 1,
                ProductWeight = 1,
                ProductWidth = 1,
                ProductLength = 1,
                ProductHeight = 1,

                Remark = "Internal Receive",
                IsActive = true,
                UserCreated = UserID,
                DateCreated = DateTime.Now,
                UserModified = UserID,
                DateModified = DateTime.Now,

            };

            receivingService.Add(receiving);
        }

        public bool GenerateDispatch(Guid receiveID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region DeclareService

                    DateTime dateNow = DateTime.Now.Date;
                    IEnumerable<Receive> receives = Where(x => x.IsActive && x.ReceiveID == receiveID).AsEnumerable();
                    IEnumerable<Warehouse> warehouse = warehouseService.Where(x => x.IsActive).AsEnumerable();
                    IEnumerable<Zone> zone = zoneService.Where(x => x.IsActive).AsEnumerable();
                    IEnumerable<Location> location = locationService.Where(x => x.IsActive).AsEnumerable();
                    IEnumerable<ItfInterfaceMapping> interfaceMap = itfInterfaceMappingService.Where(x => x.IsActive).AsEnumerable();
                    IEnumerable<Receiving> receving = receivingService.Where(x => x.IsActive).AsEnumerable();
                    IEnumerable<Contact> customers = contactService.Where(x => x.IsActive).AsEnumerable();

                    if (receives == null || receives.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    Receive receive = receives.FirstOrDefault();

                    #endregion

                    #region MyRegion

                    Contact customer = customers.FirstOrDefault(x => x.Code == "10000056" && x.IsActive);//.FirstOrDefault();
                    if (customer == null)
                    {
                        throw new HILIException("MSG00006"); // Data not found
                    }

                    #endregion

                    #region Find Shipto Other

                    ShippingTo shipto = shiptoService.FirstOrDefault(x => x.IsActive && x.Description.Contains("Others"));//.Get().FirstOrDefault();
                    if (shipto == null)
                    {
                        shipto = shiptoService.FirstOrDefault(x => x.IsActive);//.Get().FirstOrDefault();
                        if (shipto == null)
                        {
                            throw new HILIException("MSG00006"); // Data not found
                        }
                    }

                    #endregion

                    #region Find DispatchType

                    ItfInterfaceMapping dispatchType = itfInterfaceMappingService.FirstOrDefault(x => x.IsActive && x.DocumentId == receive.ReceiveTypeID && x.ReferenceDocumentID != null);

                    if (dispatchType == null)
                    {
                        throw new HILIException("MSG00064");
                    }

                    #endregion

                    #region Find Warehouse

                    Warehouse tmpWarehouse = (from w in warehouse
                                              join z in zone on w.WarehouseID equals z.WarehouseID
                                              join l in location on z.ZoneID equals l.ZoneID
                                              where l.LocationID == receive.LocationID
                                              select w).FirstOrDefault();

                    if (tmpWarehouse == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    #endregion

                    #region Find Dispatch Prefix

                    DispatchPrefix prefix = dispatchPrefixService.Where().FirstOrDefault();
                    if (prefix == null)
                    {
                        throw new HILIException("MSG00063");
                    }

                    DispatchPrefix tPrefix = dispatchPrefixService.FindByID(prefix.PrefixId);
                    string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    tPrefix.LastedKey = DispatchCode;
                    dispatchPrefixService.Modify(tPrefix);

                    #endregion

                    #region Insert Dispatch

                    Dispatch dispatchModel = new Dispatch()
                    {
                        Pono = "RCV" + receive.ReceiveCode,
                        OrderNo = receive.ReceiveCode,
                        ReferenceId = receive.ReceiveID,
                        SupplierId = receive.SupplierID,
                        CustomerId = customer.ContactID,

                        DispatchStatus =DispatchStatusEnum.InternalReceive,
                        DocumentId = dispatchType.ReferenceDocumentID,
                        FromwarehouseId = tmpWarehouse.WarehouseID,
                        TowarehouseId = tmpWarehouse.WarehouseID,
                        DispatchCode = DispatchCode,
                        DeliveryDate = dateNow,
                        DocumentDate = dateNow,
                        OrderDate = dateNow,
                        ShipptoId = shipto.ShipToId,
                        IsUrgent = false,
                        IsBackOrder = false,

                        IsActive = true,
                        Remark = "Internal Receive",
                        UserCreated = UserID,
                        DateCreated = dateNow,
                        UserModified = UserID,
                        DateModified = dateNow
                    };

                    dispatchService.Add(dispatchModel);

                    #endregion

                    #region Find Receiving

                    var recevingModels = from rcv in receving
                                         where rcv.ReceiveID == receiveID && rcv.ReceivingStatus == ReceivingStatusEnum.Complete
                                         select new
                                         {
                                             DispatchId = dispatchModel.DispatchId,
                                             RuleId = shipto.RuleId,
                                             DispatchDetailStatus = (int)DispatchDetailStatusEnum.InprogressConfirm,
                                             DispatchDetailProductWidth = 0,
                                             DispatchDetailProductLength = 0,
                                             DispatchDetailProductHeight = 0,
                                             DispatchPrice = 0,
                                             IsBackOrder = false,

                                             SupplierID = rcv.SupplierID,
                                             Sequence = rcv.Sequence,
                                             ProductId = rcv.ProductID,
                                             StockUnitId = rcv.StockUnitID,
                                             Quantity = rcv.Quantity,
                                             BaseQuantity = rcv.BaseQuantity,
                                             BaseUnitId = rcv.BaseUnitID,
                                             ConversionQty = rcv.ConversionQty,
                                             ProductOwnerId = rcv.ProductOwnerID.Value,
                                             ProductStatusId = rcv.ProductStatusID,
                                             ProductSubStatusId = rcv.ProductSubStatusID,
                                             Lot = rcv.Lot,
                                             MFGDate = rcv.ManufacturingDate,
                                             EXPDate = rcv.ExpirationDate,
                                             LocationId = rcv.LocationID,

                                             IsActive = true,
                                             UserCreated = UserID,
                                             DateCreated = dateNow,
                                             UserModified = UserID,
                                             DateModified = dateNow,
                                             Remark = "Internal Receive"
                                         };

                    #endregion

                    List<StockSearch> stockListModel = new List<StockSearch>();
                    List<DispatchDetailCustom> dispatchDetailCustoms = new List<DispatchDetailCustom>();

                    foreach (var item in recevingModels.ToList())
                    {
                        #region Insert Dispatch Details Customs

                        dispatchDetailCustoms.Add(new DispatchDetailCustom()
                        {
                            DispatchID = dispatchModel.DispatchId,
                            RuleID = shipto.RuleId,
                            DispatchDetailStatus = (int)DispatchDetailStatusEnum.InternalReceive,
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

                            BookingStatus = (int)BookingStatusEnum.InternalReceive,
                            LocationId = item.LocationId.Value,
                            ProductLot = item.Lot,
                            Mfgdate = item.MFGDate.Value,
                            ExpirationDate = item.EXPDate,

                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = dateNow,
                            UserModified = UserID,
                            DateModified = dateNow,
                            Remark = "Internal Receive"
                        });

                        #endregion

                        #region Reserve Stock

                        StockSearch stockModel = new StockSearch()
                        {
                            Lot = item.Lot,
                            LocationID = item.LocationId,
                            ManufacturingDate = item.MFGDate.Value,
                            ExpirationDate = item.EXPDate.Value,
                            ProductID = item.ProductId,
                            ConversionQty = item.ConversionQty,
                            ProductOwnerID = item.ProductOwnerId,
                            QTY = item.Quantity,
                            SupplierID = item.SupplierID,
                            StockUnitID = item.StockUnitId,
                            BaseUnitID = item.BaseUnitId,
                            ProductStatusID = item.ProductStatusId,
                        };

                        stockListModel.Add(stockModel);

                        #endregion
                    }

                    _DispatchDetailService.UserID = UserID;
                    _DispatchDetailService.AddList(dispatchDetailCustoms);
                    stockService.UserID = UserID;
                    stockService.AdjustReserve_(stockListModel, StockReserveTypeEnum.Reserve);

                    receive.UserModified = UserID;
                    receive.DateModified = dateNow;
                    receive.ReceiveStatus = ReceiveStatusEnum.GenDispatch;
                    base.Modify(receive);

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

        public bool SendtoProductionControl(List<Guid> receiveIDs)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    CultureInfo cultureinfo = new CultureInfo("en-US");

                    foreach (var receiveID in receiveIDs)
                    {
                        var productionControl = (from rc in Query().Filter(x => x.ReceiveID == receiveID && x.IsActive).Get()
                                                 join rcd in receiveDetailService.Query().Get() on rc.ReceiveID equals rcd.ReceiveID
                                                 join ppd in productionPlanDetailService.Query().Include(x => x.ProductionPlan).Get() on rc.ReferenceID equals ppd.ProductionID
                                                 join pl in lineService.Query().Get() on rc.LineID equals pl.LineID
                                                 join pu in productUnitService.Query().Get() on rcd.StockUnitID equals pu.ProductUnitID
                                                 where rcd.ManufacturingDate != null && rc.LineID != null
                                                     && rc.ReceiveStatus != ReceiveStatusEnum.LoadIn
                                                     && rcd.ReceiveDetailStatus != ReceiveDetailStatusEnum.LoadIn

                                                 select new ProductionControl()
                                                 {
                                                     ProductionDate = rcd.ManufacturingDate.Value.Date,
                                                     ProductionTime = rcd.ManufacturingDate.Value.TimeOfDay,
                                                     Lot = rcd.ManufacturingDate != null ? rcd.ManufacturingDate.Value.ToString("yyyyMMdd", cultureinfo) : null,
                                                     LineID = rc.LineID.Value,
                                                     ProductID = rcd.ProductID,
                                                     ProductUnitID = rcd.StockUnitID,
                                                     Quantity = rcd.Quantity,
                                                     BaseUnitID = rcd.BaseUnitID,
                                                     ConversionQty = rcd.ConversionQty,
                                                     StockUnitID = rcd.StockUnitID,
                                                     ProductStatusID = rcd.ProductStatusID,
                                                     ProductSubStatusID = rcd.ProductSubStatusID,
                                                     OrderNo = rc.Reference1,
                                                     OrderType = ppd.ProductionPlan.OrderType,
                                                     PcControlStatus = (int)PCControlStatusEnum.New,
                                                     ReferenceID = rcd.ReceiveDetailID,
                                                     StandardPalletQty = (ppd.PalletQty == null || ppd.PalletQty == 0) ? pu.PalletQTY : (decimal)ppd.PalletQty,
                                                     IsActive = true,
                                                     DateCreated = DateTime.Now,
                                                     DateModified = DateTime.Now,
                                                     UserCreated = UserID,
                                                     UserModified = UserID
                                                 }).ToList();

                        productionControlService.AddRange(productionControl);

                        var model = Query().Filter(x => x.ReceiveID == receiveID).Include(x => x.ReceiveDetailCollection).Get().FirstOrDefault();

                        foreach (var item in model.ReceiveDetailCollection)
                        {
                            var rDetail = receiveDetailService.FindByID(item.ReceiveDetailID);
                            rDetail.DateModified = DateTime.Now;
                            rDetail.UserModified = UserID;
                            rDetail.ReceiveDetailStatus = ReceiveDetailStatusEnum.LoadIn;
                            receiveDetailService.Modify(rDetail);
                        }

                        var receive = FindByID(model.ReceiveID);
                        receive.DateModified = DateTime.Now;
                        receive.UserModified = UserID;
                        receive.ReceiveStatus = ReceiveStatusEnum.LoadIn;
                        base.Modify(receive);
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
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

    }
}