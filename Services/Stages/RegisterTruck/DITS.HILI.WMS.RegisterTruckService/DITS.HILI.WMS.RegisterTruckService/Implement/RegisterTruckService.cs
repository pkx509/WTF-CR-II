using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.RegisterTruckService
{
    public class RegisterTruckService : Repository<RegisterTruck>, IRegisterTruckService
    {
        #region Property
        // private readonly IUnitOfWork unitofwork;
        private readonly IRepository<Dispatch> DispatchService;
        private readonly IRepository<DispatchDetail> DispatchDetailService;
        private readonly IRepository<DispatchBooking> DispatchBookingService;
        private readonly IRepository<DocumentType> DocumentTypeService;
        private readonly IRepository<RegisterTruckPrefix> RegTruckPrefixService;
        private readonly IRepository<DocumentNoPrefix> DocNoService;
        private readonly IRepository<PickingPrefix> PickingPrefixService;
        private readonly IRepository<Picking> PickingService;
        private readonly IRepository<PickingDetail> PickingDetailService;
        private readonly IRepository<PickingAssign> PickingAssignService;
        private readonly IRepository<Product> ProductService;
        private readonly IRepository<ProductCodes> ProductCodeService;
        private readonly IRepository<ProductUnit> ProductUnitService;
        private readonly IRepository<ShippingTo> ShiptoService;
        private readonly IRepository<RegisterTruckDetail> regisTruckDetailService;
        private readonly IRepository<RegisterTruckConsolidate> regisTruckConsolidateService;
        private readonly IRepository<ProductionControlDetail> packingDetailService;
        private readonly IRepository<TruckType> truckTypeService;
        private readonly IRepository<DockConfig> dockConfigService;
        private readonly IRepository<ItfInterfaceMapping> ItfInterfaceMappingService;
        private readonly IRepository<Contact> contactService;
        private readonly IRepository<Location> locationService;
        private readonly IRepository<PhysicalZone> physicalZoneService;
        private readonly IRepository<Zone> ZoneService;
        private readonly IRepository<Warehouse> WarehouseService; 
        #endregion

        #region Constructor

        public RegisterTruckService(IUnitOfWork dbContext,
                                    IRepository<Dispatch> _dispatch,
                                    IRepository<DispatchDetail> _dispatchDetail,
                                    IRepository<DispatchBooking> _dispatchBookingDetail,
                                    IRepository<DocumentType> _documentType,
                                    IRepository<RegisterTruckPrefix> _regTruckPrefix,
                                    IRepository<DocumentNoPrefix> _docNoPrefix,
                                    IRepository<Picking> _picking,
                                    IRepository<PickingPrefix> _pickingPrefix,
                                    IRepository<PickingDetail> _pickingDetail,
                                    IRepository<PickingAssign> _pickingAssign,
                                    IRepository<Product> _product,
                                    IRepository<ProductCodes> _productCode,
                                    IRepository<ProductUnit> _productUnit,
                                    IRepository<ShippingTo> _shipto,
                                    IRepository<RegisterTruckDetail> _regisTruck,
                                    IRepository<RegisterTruckConsolidate> _regisConsolidateTruck,
                                    IRepository<ProductionControlDetail> _packingDetailService,
                                    IRepository<TruckType> _truckType,
                                    IRepository<DockConfig> _dockConfig,
                                    IRepository<ItfInterfaceMapping> _iptfInterfaceMappingService,
                                    IRepository<Contact> _contact,
                                    IRepository<Location> _location,
                                    IRepository<PhysicalZone> _physicalZone,
                                    IRepository<Zone> _Zone,
                                    IRepository<Warehouse> _Warehouse,
                                    IRepository<PickingAssign> _PickingAssign)
            : base(dbContext)
        {
            // unitofwork = dbContext;
            DispatchService = _dispatch;
            DispatchDetailService = _dispatchDetail;
            DispatchBookingService = _dispatchBookingDetail;
            DocumentTypeService = _documentType;
            RegTruckPrefixService = _regTruckPrefix;
            PickingPrefixService = _pickingPrefix;
            PickingDetailService = _pickingDetail;
            PickingAssignService = _pickingAssign;
            ProductService = _product;
            ProductCodeService = _productCode;
            ProductUnitService = _productUnit;
            ShiptoService = _shipto;
            regisTruckDetailService = _regisTruck;
            DocNoService = _docNoPrefix;
            regisTruckConsolidateService = _regisConsolidateTruck;
            packingDetailService = _packingDetailService;
            truckTypeService = _truckType;
            dockConfigService = _dockConfig;
            ItfInterfaceMappingService = _iptfInterfaceMappingService;
            contactService = _contact;
            locationService = _location;
            physicalZoneService = _physicalZone;
            ZoneService = _Zone;
            WarehouseService = _Warehouse;
            PickingAssignService = _PickingAssign;
            PickingService = _picking;
        }

        #endregion
        public RegisterTruck Get(Guid id)
        {
            try
            {
                RegisterTruck _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ShippingID == id)
                                  .Get().FirstOrDefault();

                return _current;
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

        public RegisTruckModel GetbyDetailId(Guid? ShippingID)
        {
            try
            {
                var result = (from _rTruck in Query().Get()
                              join _truckType in truckTypeService.Query().Get() on _rTruck.TruckTypeID equals _truckType.TruckTypeID
                              join _dockConfig in dockConfigService.Query().Include(x => x.Warehouse).Get() on _rTruck.DockTypeID equals _dockConfig.DockConfigID
                              join _shipto in ShiptoService.Query().Get() on _rTruck.ShiptoID equals _shipto.ShipToId
                              where ((ShippingID != null ? _rTruck.ShippingID == ShippingID.Value : true))
                              select new { _rTruck, _truckType, _dockConfig, _shipto });
                RegisTruckModel _rTruckResult = result.Select(n => new RegisTruckModel
                {
                    ShippingID = n._rTruck.ShippingID,
                    ShippingCode = n._rTruck.ShippingCode,
                    DocumentDate = n._rTruck.DocumentDate,
                    RegisterTypeID = n._rTruck.RegisterTypeID,
                    RegisterType = n._rTruck.RegisterTypeID == 1 ? RegisterTruckEnum.Local.ToString() : RegisterTruckEnum.Export.ToString(),
                    TruckTypeID = n._truckType.TruckTypeID,
                    TruckTypeName = n._truckType.TypeName,
                    ShippingTruckNo = n._rTruck.ShippingTruckNo,
                    DockTypeID = n._rTruck.DockTypeID,
                    DockTypeName = n._dockConfig.DockName,
                    DriverName = n._rTruck.DriverName,
                    WarehouseID = n._rTruck.WarehouseID,
                    WarehouseName = n._dockConfig?.Warehouse.Name,
                    LogisticCompany = n._rTruck.LogisticCompany,
                    OrderNo = n._rTruck.OrderNo,
                    Container_No = n._rTruck.Container_No,
                    SealNo = n._rTruck.SealNo,
                    BookingNo = n._rTruck.BookingNo,
                    PoNo = n._rTruck.PoNo,
                    ShippingStatus = (int)n._rTruck.ShippingStatus,
                    DispatchCode = n._rTruck.Dispatchcode,
                    ShipptoCode = n._rTruck.ShipptoCode,
                    ShipptoName = n._shipto.Name,
                    DocumentNo = n._rTruck.DocumentNo,
                    CompleteDate = n._rTruck.CompleteDate,
                    CancelDate = n._rTruck.CancelDate,
                    IsActive = n._rTruck.IsActive,
                    Remark = n._rTruck.Remark
                }).FirstOrDefault();


                var _regDetail = (from _regTruck in Query().Filter(x => x.IsActive == true &&
                                                                        x.ShippingID == _rTruckResult.ShippingID &&
                                                                        (_rTruckResult.WarehouseID != null ? x.WarehouseID == _rTruckResult.WarehouseID.Value : true))
                                                                        .Get()
                                  join _regTruckdetails in regisTruckDetailService.Query().Filter(x => x.IsActive == true).Get()
                                    on _regTruck.ShippingID equals _regTruckdetails.ShippingID
                                  join _product in ProductService.Query().Filter(x => x.IsActive == true).Include(x => x.CodeCollection).Get()
                                    on _regTruckdetails.ProductID equals _product.ProductID
                                  join _productUnit in ProductUnitService.Query().Filter(x => x.IsActive == true).Get()
                                    on _regTruckdetails.ShippingUnitID equals _productUnit.ProductUnitID
                                  select new { _regTruck, _regTruckdetails, _product, _productUnit });
                _rTruckResult.RegisTruckDetail = _regDetail.Select(n => new RegisterTruckDetailModel()
                {
                    ReferenceID = n._regTruckdetails.ReferenceID,
                    BookingID = n._regTruckdetails.BookingID,
                    ProductID = n._product.ProductID,
                    ProductCode = n._product.CodeCollection.Where(x => x.CodeType == 0).FirstOrDefault()?.Code,
                    ShippingQuantity = n._regTruckdetails.ShippingQuantity,
                    ShippingUnitID = n._regTruckdetails.ShippingUnitID,
                    BookingQty = n._regTruckdetails.ShippingQuantity,
                    BookingStockUnitId = n._regTruckdetails.BasicUnitID,
                    BookingBaseQty = n._regTruckdetails.BasicQuantity,
                    BookingBaseUnitId = n._regTruckdetails.BasicUnitID,
                    ConversionQty = n._regTruckdetails.ConversionQty,
                    ShippingStatus = n._regTruck.ShippingStatus,
                    TransactionTypeID = 10,
                    ProductUnitID = n._regTruckdetails.ShippingUnitID,
                    ProductUnitName = n._productUnit.Name,
                    ProductName = n._product.Name,
                    ConfirmQuantity = n._regTruckdetails.ConfirmQuantity,
                    Remain_Qty = n._regTruckdetails.ShippingQuantity - n._regTruckdetails.ConfirmQuantity
                }).ToList();

                return _rTruckResult;
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

        public List<DispatchAllModel> GetAll(Guid? ShippingID, string Po, string Doc, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                Po = (string.IsNullOrEmpty(Po) ? "" : Po);
                Doc = (string.IsNullOrEmpty(Doc) ? "" : Doc);

                IEnumerable<DispatchAllModel> result = (from _rTruck in Where(x => x.IsActive)
                                                        join _rTruckDetail in regisTruckDetailService.Where(x => x.IsActive) on _rTruck.ShippingID equals _rTruckDetail.ShippingID
                                                        join _truckType in truckTypeService.Where(x => x.IsActive) on _rTruck.TruckTypeID equals _truckType.TruckTypeID
                                                        join _dockConfig in dockConfigService.Where(x => x.IsActive ) on _rTruck.DockTypeID equals _dockConfig.DockConfigID

                                                        where (ShippingID.HasValue? _rTruck.ShippingID == ShippingID.Value : true)
                                                        group new { _rTruck, _rTruckDetail, _truckType, _dockConfig }
                                                           by new
                                                           {
                                                               ShippingID = _rTruck.ShippingID,
                                                               ShippingCode = _rTruck.ShippingCode,
                                                               DocumentDate = _rTruck.DocumentDate,
                                                               RegisterTypeID = _rTruck.RegisterTypeID,
                                                               RegisterType = _rTruck.RegisterTypeID,
                                                               TruckTypeName = _truckType.TypeName,
                                                               ShippingTruckNo = _rTruck.ShippingTruckNo,
                                                               DockTypeID = _rTruck.DockTypeID,
                                                               DockTypeName = _dockConfig.DockName,
                                                               DriverName = _rTruck.DriverName,
                                                               LogisticCompany = _rTruck.LogisticCompany,
                                                               OrderNo = _rTruck.OrderNo,
                                                               Container_No = _rTruck.Container_No,
                                                               SealNo = _rTruck.SealNo,
                                                               BookingNo = _rTruck.BookingNo,
                                                               PoNo = _rTruck.PoNo,
                                                               ShippingStatus = (int)_rTruck.ShippingStatus,
                                                               Dispatchcode = _rTruck.Dispatchcode,
                                                               ShipptoCode = _rTruck.ShipptoCode,
                                                               DocumentNo = _rTruck.DocumentNo,
                                                               CompleteDate = _rTruck.CompleteDate,
                                                               CancelDate = _rTruck.CancelDate,
                                                               IsActive = _rTruck.IsActive
                                                           }
                                 into s
                                                        select new DispatchAllModel()
                                                        {
                                                            ShippingID = s.Key.ShippingID,
                                                            ShippingCode = s.Key.ShippingCode,
                                                            DocumentDate = s.Key.DocumentDate,
                                                            RegisterTypeID = s.Key.RegisterTypeID,
                                                            RegisterType = s.Key.RegisterTypeID == 1 ? RegisterTruckEnum.Local.ToString() : RegisterTruckEnum.Export.ToString(),
                                                            TruckTypeName = s.Key.TruckTypeName,
                                                            ShippingTruckNo = s.Key.ShippingTruckNo,
                                                            DockTypeID = s.Key.DockTypeID,
                                                            DockTypeName = s.Key.DockTypeName,
                                                            DriverName = s.Key.DriverName,
                                                            LogisticCompany = s.Key.LogisticCompany,
                                                            OrderNo = s.Key.OrderNo,
                                                            Container_No = s.Key.Container_No,
                                                            SealNo = s.Key.SealNo,
                                                            BookingNo = s.Key.BookingNo,
                                                            PoNo = s.Key.PoNo,
                                                            ShippingStatus = s.Key.ShippingStatus,
                                                            DispatchCode = s.Key.Dispatchcode,
                                                            ShipptoCode = s.Key.ShipptoCode,
                                                            DocumentNo = s.Key.DocumentNo,
                                                            CompleteDate = s.Key.CompleteDate,
                                                            CancelDate = s.Key.CancelDate,
                                                            IsActive = s.Key.IsActive,
                                                            IsApprove = (s.Key.ShippingStatus == (int)ShippingStatusEnum.Assign ||
                                                                         s.Key.ShippingStatus == (int)ShippingStatusEnum.Complete ?
                                                                         true : false)
                                                        });

                if (!string.IsNullOrWhiteSpace(Po))
                {
                    result = result.Where(x => x.PoNo.Contains(Po));
                }

                if (!string.IsNullOrWhiteSpace(Doc))
                {
                    result = result.Where(x => x.DocumentNo.Contains(Doc));
                }

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.OrderByDescending(x => x.ShippingCode).ToList();

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

        public List<DispatchAllModel> GetDispatchForRegisTrucklistAll(Guid? WarehouseID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                int[] Status = new int[]
                 {
                    (int)DispatchStatusEnum.WaitingConfirmDispatch,
                    (int)DispatchStatusEnum.InprogressConfirm,
                    (int)DispatchStatusEnum.InBackOrder,
                    (int)DispatchStatusEnum.Register,
                 };

                var tmp = (from _dispatch in DispatchService.Where(x => x.IsActive == true && Status.Contains((int)x.DispatchStatus))
                           join _dispatchDetail in DispatchDetailService.Where(x => x.IsActive == true && Status.Contains((int)x.DispatchDetailStatus)) on _dispatch.DispatchId equals _dispatchDetail.DispatchId
                           join _dispatchBooking in DispatchBookingService.Where(x => x.IsActive == true && x.BookingStatus == BookingStatusEnum.InprogressConfirm) on _dispatchDetail.DispatchDetailId equals _dispatchBooking.DispatchDetailId
                           join _location in locationService.Where(x => x.IsActive) on _dispatchBooking.LocationId equals _location.LocationID
                           join _zone in ZoneService.Where(x => x.IsActive) on _location.ZoneID equals _zone.ZoneID
                           join _warehouse in WarehouseService.Where(x => x.IsActive == true && x.ReferenceCode == "111") on _zone.WarehouseID equals _warehouse.WarehouseID
                           join _documentType in DocumentTypeService.Where(x => x.IsActive == true) on _dispatch.DocumentId equals _documentType.DocumentTypeID
                           join _marketting in ItfInterfaceMappingService.Where(x => x.IsActive == true && x.IsRegistTruck == true) on _dispatch.DocumentId equals _marketting.DocumentId
                           join _shipto in ShiptoService.Where(x => x.IsActive == true) on _dispatch.ShipptoId equals _shipto.ShipToId
                           join _contact in contactService.Where(x => x.IsActive == true) on _dispatch.CustomerId equals _contact.ContactID
                           where ((keyword != "" ? _dispatch.Pono.ToLower().Trim() == keyword.ToLower().Trim() : true) &&
                                 (WarehouseID.HasValue? _warehouse.WarehouseID == WarehouseID.Value : true))
                           select new { _dispatch, _dispatchDetail, _dispatchBooking, _location, _warehouse, _documentType, _shipto, _contact }).ToList();

                       var result= from t in tmp
                                   group new { t._dispatch, t._dispatchDetail, t._dispatchBooking, t._location, t._warehouse, t._documentType, t._shipto, t._contact }
                             by new
                             {
                                 WarehouseID = t._warehouse.WarehouseID,
                                 WarehouseName = t._warehouse.Name,
                                 OrderNo = t._dispatch.OrderNo,
                                 PoNo = t._dispatch.Pono,
                                 DeliveryDate = t._dispatch.DeliveryDate.Value,
                                 DispatchCode = t._dispatch.DispatchCode,
                                 ShipToId = t._shipto.ShipToId,
                                 ShiptoName = t._shipto.Name,
                                 DocumentCode = t._documentType.Code,
                                 DocumentName = t._documentType.Name,
                                 CustomerName = t._contact.Name
                             }
                              into s
                             select new
                             {
                                 WarehouseID = s.Key.WarehouseID,
                                 WarehouseName = s.Key.WarehouseName,
                                 OrderNo = s.Key.OrderNo,
                                 PoNo = s.Key.PoNo,
                                 DeliveryDate = s.Key.DeliveryDate,
                                 DispatchCode = s.Key.DispatchCode,
                                 ShipToId = s.Key.ShipToId,
                                 ShiptoName = s.Key.ShiptoName,
                                 DocumentCode = s.Key.DocumentCode,
                                 DocumentName = s.Key.DocumentName,
                                 CustomerName = s.Key.CustomerName,
                             };

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null && totalRecords>1)
                {
                    result = result.OrderByDescending(x => x.PoNo).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var tmps = result.ToList();
                List<DispatchAllModel> _result = tmps.Select(n => new DispatchAllModel
                {
                    WarehouseID = n.WarehouseID,
                    WarehouseName = n.WarehouseName,
                    OrderNo = n.OrderNo,
                    PoNo = n.PoNo,
                    DeliveryDate = n.DeliveryDate,
                    DispatchCode = n.DispatchCode,
                    ShipToId = n.ShipToId,
                    ShipptoName = n.ShiptoName,
                    DocumentCode = n.ShiptoName,
                    DocumentName = n.DocumentName,
                    CustomerName = n.CustomerName
                }).ToList();

                return _result;

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

        public DispatchAllModel GetDispatchForRegisTruckById(Guid? WarehouseID, string keyword)
        {
            try
            {
                SqlParameter param = new SqlParameter("@WarehouseID", SqlDbType.UniqueIdentifier) { Value = WarehouseID == null ? Guid.Empty : WarehouseID };
                SqlParameter param2 = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = keyword };
                DispatchAllModel _result = Context.SQLQuery<DispatchAllModel>("exec GetDispatchForRegisTruckById @WarehouseID, @PoNo", param, param2).FirstOrDefault();
                if (_result == null)
                {
                    throw new HILIException("MSG00006");
                }

                param = new SqlParameter("@WarehouseID", SqlDbType.UniqueIdentifier) { Value = WarehouseID == null ? Guid.Empty : WarehouseID };
                param2 = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = keyword };

                List<RegisterTruckDetailModel> resultDetail = Context.SQLQuery<RegisterTruckDetailModel>("exec GetDispatchDetailForRegisTruckById @WarehouseID, @PoNo", param, param2).ToList();
                _result.DispatchDetails = resultDetail;
                return _result;
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

        public List<RegisterTruckDetailModel> GetConfirmRegisTruck(Guid? ShippingID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    List<RegisterTruckDetailModel> result = (from _rTruck in Where(x => x.IsActive == true && (ShippingID != null ? x.ShippingID == ShippingID.Value : true) &&
                                                                  x.ShippingStatus == (int)ShippingStatusEnum.New).DefaultIfEmpty()
                                                             join _rTruckdetails in regisTruckDetailService.Where(x => x.IsActive == true) on _rTruck.ShippingID equals _rTruckdetails.ShippingID
                                                             join tmpRegisTruckDetail in
                                                             (
                                                                 from _regd in regisTruckDetailService.Where(x => x.IsActive).DefaultIfEmpty()
                                                                 join _reg in Where(x => x.IsActive && x.ShippingStatus == (int)ShippingStatusEnum.New) on _regd.ShippingID equals _reg.ShippingID
                                                                 where _regd.IsActive
                                                                 group new { _reg, _regd } by new
                                                                 {
                                                                     _reg.ShippingID,
                                                                     _regd.ProductID,
                                                                     _reg.PoNo,
                                                                     _regd.ReferenceID,
                                                                     _regd.BookingID
                                                                 }
                                                                 into _sumQty
                                                                 select new
                                                                 {
                                                                     ShippingID = _sumQty.Key.ShippingID,
                                                                     ProductID = _sumQty.Key.ProductID,
                                                                     PoNo = _sumQty.Key.PoNo,
                                                                     ReferenceID = _sumQty.Key.ReferenceID,
                                                                     BookingID = _sumQty.Key.BookingID,
                                                                     ConfirmQuantity = _sumQty.Sum(x => x._regd.ConfirmQuantity),
                                                                     ConfirmBasicQuantity = _sumQty.Sum(x => x._regd.ConfirmBasicQuantity)
                                                                 }
                                                             )
                                                             on
                                                             new { _rTruck.ShippingID, _rTruckdetails.BookingID }
                                                             equals
                                                             new { tmpRegisTruckDetail.ShippingID, tmpRegisTruckDetail.BookingID }
                                                             select new RegisterTruckDetailModel()
                                                             {
                                                                 ShippingDetailID = _rTruckdetails.ShippingDetailID,
                                                                 ShippingID = _rTruck.ShippingID,
                                                                 ShippingQuantity = _rTruckdetails.ShippingQuantity,
                                                                 ShippingUnitID = _rTruckdetails.ShippingUnitID,
                                                                 BasicQuantity = _rTruckdetails.BasicQuantity,
                                                                 BasicUnitID = _rTruckdetails.BasicUnitID,
                                                                 DispatchCode = _rTruck.Dispatchcode,
                                                                 ReferenceID = _rTruckdetails.ReferenceID,
                                                                 BookingID = _rTruckdetails.BookingID,
                                                                 ConfirmBasicQuantity = tmpRegisTruckDetail.ConfirmBasicQuantity,
                                                                 ConfirmQuantity = tmpRegisTruckDetail.ConfirmQuantity,
                                                                 ConfirmBasicUnitID = _rTruckdetails.ConfirmBasicUnitID,
                                                                 ConfirmUnitID = _rTruckdetails.ConfirmUnitID
                                                             }).ToList();

                    return result;
                };
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

        public List<DispatchAllModel> GetAssignPick(string keyword)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    var temp = (from _dispatchBooking in DispatchBookingService.Where(x => x.IsActive)
                                join _rTruckdetails in regisTruckDetailService.Where(x => x.IsActive) on _dispatchBooking.BookingId equals _rTruckdetails.BookingID
                                join _rTruck in Where(x => x.IsActive) on _rTruckdetails.ShippingID equals _rTruck.ShippingID
                                join _dispatchDetail in DispatchDetailService.Where(x => x.IsActive) on _rTruckdetails.ReferenceID equals _dispatchDetail.DispatchDetailId
                                join _dispatch in DispatchService.Where(x => x.IsActive) on _dispatchDetail.DispatchId equals _dispatch.DispatchId
                                join _location in locationService.Where(x => x.IsActive) on _dispatchBooking.LocationId equals _location.LocationID
                                join _zone in ZoneService.Where(x => x.IsActive) on _location.ZoneID equals _zone.ZoneID
                                join _warehouse in WarehouseService.Where(x => x.IsActive) on _zone.WarehouseID equals _warehouse.WarehouseID
                                where (_rTruck.ShippingCode.Contains(keyword))
                                select new
                                {
                                    _dispatch,
                                    _dispatchDetail,
                                    _dispatchBooking,
                                    _rTruckdetails,
                                    _rTruck,
                                    _location,
                                    _zone,
                                    _warehouse
                                }).ToList();
                    List<DispatchAllModel> result = temp.GroupBy(g => new
                    {
                        WarehouseID = g._warehouse.WarehouseID,
                        ShippingID = g._rTruckdetails.ShippingID,
                        DispatchCode = g._dispatch.DispatchCode,
                        DocumentNo = g._rTruck.DocumentNo,
                        OrderNo = g._rTruck.OrderNo,
                        PoNo = g._rTruck.PoNo,
                    }).Select(n => new DispatchAllModel
                    {
                        OrderNo = n.Key.OrderNo,
                        PoNo = n.Key.PoNo,
                        WarehouseID = n.Key.WarehouseID,
                        ShippingID = n.Key.ShippingID,
                        DispatchCode = n.Key.DispatchCode,
                        DocumentNo = n.Key.DocumentNo,
                        BookingBaseQty = n.Sum(x => x._dispatchBooking.BookingBaseQty),
                        BookingQty = n.Sum(x => x._dispatchBooking.BookingBaseQty),
                    }).ToList();
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

        public List<DispatchAllModel> GetAssignPickingJob(string keyword)
        {
            try
            {

                int[] packingStatus = new int[]
                {
                    (int)PackingStatusEnum.Loading_In,
                    (int)PackingStatusEnum.In_Progress,
                    (int)PackingStatusEnum.Transfer,
                    (int)PackingStatusEnum.PutAway
                };

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                List<DispatchAllModel> result = (from _picking in PickingService.Where(x => x.IsActive == true && x.ShippingCode == keyword)
                                                 join _rTruck in Where(x => x.IsActive)  on _picking.ShippingCode equals _rTruck.ShippingCode
                                                 join _rTruckdetails in regisTruckDetailService.Where(x => x.IsActive)  on _rTruck.ShippingID equals _rTruckdetails.ShippingID
                                                 join _dispatchBooking in DispatchBookingService.Where(x => x.IsActive) on _rTruckdetails.BookingID equals _dispatchBooking.BookingId
                                                 join _packingDetail in packingDetailService.Where(x => x.IsActive == true && packingStatus.Contains((int)x.PackingStatus))
                                                   on _dispatchBooking.PalletCode equals _packingDetail.PalletCode
                                                 select new
                                                 {
                                                     _picking,
                                                     _rTruck,
                                                     _dispatchBooking,
                                                     _rTruckdetails,
                                                     _packingDetail
                                                 }).GroupBy(g => new
                                                 {
                                                     ShippingDetailID = g._rTruckdetails.ShippingDetailID,
                                                     PickingID = g._picking.PickingID,
                                                     ProductID = g._rTruckdetails.ProductID,
                                                     ConfirmQuantity = g._rTruckdetails.ConfirmQuantity,
                                                     ConfirmBasicQuantity = g._rTruckdetails.ConfirmBasicQuantity,
                                                     ConfirmBasicUnitID = g._rTruckdetails.ConfirmBasicUnitID,
                                                     ConfirmUnitID = g._rTruckdetails.ConfirmUnitID,
                                                     ShippingCode = g._picking.ShippingCode,
                                                     BookingId = g._dispatchBooking.BookingId,
                                                     LocationId = g._dispatchBooking.LocationId,
                                                     ProductLot = g._dispatchBooking.ProductLot,
                                                     PalletCode = g._packingDetail.PalletCode,
                                                     RemainQTY = g._packingDetail.RemainQTY,
                                                     RemainStockUnitID = g._packingDetail.RemainStockUnitID,
                                                     RemainBaseQTY = g._packingDetail.RemainBaseQTY,
                                                     RemainBaseUnitID = g._packingDetail.RemainBaseUnitID,
                                                 }).Select(n => new DispatchAllModel
                                                 {
                                                     ShippingDetailID = n.Key.ShippingDetailID,
                                                     PickingID = n.Key.PickingID,
                                                     ProductID = n.Key.ProductID,
                                                     ConfirmQuantity = n.Key.ConfirmQuantity,
                                                     ConfirmBasicQuantity = n.Key.ConfirmBasicQuantity,
                                                     ConfirmBasicUnitID = n.Key.ConfirmBasicUnitID,
                                                     ConfirmUnitID = n.Key.ConfirmUnitID,
                                                     ShippingCode = n.Key.ShippingCode,
                                                     BookingId = n.Key.BookingId,
                                                     LocationId = n.Key.LocationId,
                                                     ProductLot = n.Key.ProductLot,
                                                     PalletCode = n.Key.PalletCode,
                                                     RemainQTY = n.Key.RemainQTY,
                                                     RemainStockUnitID = n.Key.RemainStockUnitID,
                                                     RemainBaseQTY = n.Key.RemainBaseQTY,
                                                     RemainBaseUnitID = n.Key.RemainBaseUnitID,
                                                 }).ToList();

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

        public bool AddRegisTruck(RegisTruckModel entity)
        {

            try
            {
                if (entity == null)
                {
                    throw new HILIException("MSG00006");
                }

                DispatchAllModel checkCreateRegisterTruck = GetDispatchForRegisTruckById(entity.WarehouseID, entity.PoNo);
                if (checkCreateRegisterTruck.DispatchDetails.Count() == 0)
                {
                    throw new HILIException("MSG00006");
                }

                entity.RegisTruckDetail.ToList().ForEach(item =>
                {
                    RegisterTruckDetailModel book = checkCreateRegisterTruck.DispatchDetails.FirstOrDefault(x => x.BookingID == item.BookingID);
                    if (item.ConfirmQuantity > book.Remain_Qty)
                    {
                        throw new HILIException("MSG00102");
                    }
                });
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region [ PreFix ]

                    RegisterTruckPrefix prefix = RegTruckPrefixService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                    if (prefix == null)
                    {
                        throw new HILIException("REG10012");
                    }

                    RegisterTruckPrefix tPrefix = RegTruckPrefixService.FindByID(prefix.PrefixID);

                    string ShippingCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    entity.ShippingCode = ShippingCode;
                    tPrefix.IsLastest = false;

                    RegisterTruckPrefix newPrefix = new RegisterTruckPrefix()
                    {
                        IsLastest = true,
                        LastedKey = ShippingCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    RegTruckPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]

                    #region [ PreFix ]

                    DocumentNoPrefix DocNoprefix = DocNoService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                    if (DocNoprefix == null)
                    {
                        throw new HILIException("REG10012");
                    }

                    DocumentNoPrefix DocPrefix = DocNoService.FindByID(DocNoprefix.PrefixID);

                    string DocumentNo = Prefix.OnCreatePrefixed(DocNoprefix.LastedKey, DocNoprefix.PrefixKey, DocNoprefix.FormatKey, DocNoprefix.LengthKey);
                    entity.DocumentNo = DocumentNo;
                    DocPrefix.IsLastest = false;

                    DocumentNoPrefix newDocPrefix = new DocumentNoPrefix()
                    {
                        IsLastest = true,
                        LastedKey = DocumentNo,
                        PrefixKey = DocNoprefix.PrefixKey,
                        FormatKey = DocNoprefix.FormatKey,
                        LengthKey = DocNoprefix.LengthKey
                    };

                    DocNoService.Add(newDocPrefix);

                    #endregion [ PreFix ] 

                    bool docNoduplicate = Any(x => x.DocumentNo.ToLower() == entity.DocumentNo.ToLower());
                    if (docNoduplicate)
                    {
                        throw new HILIException("MSG00009");
                    }

                    ShippingTo shipto = ShiptoService.FirstOrDefault(x => x.IsActive == true && x.Name.Contains(entity.ShipptoName));
                    Warehouse warehouse = WarehouseService.FirstOrDefault(x => x.IsActive == true && x.Name.Contains(entity.WarehouseName));
                    if (warehouse == null)
                    {
                        warehouse = new Warehouse
                        {
                            //warehouse.WarehouseID = WarehouseService.Query().Filter(x => x.IsActive == true && x.Name.Contains(entity.WarehouseID)).Get().FirstOrDefault();
                            WarehouseID = Guid.Parse(entity.WarehouseName)
                        };
                    }

                    RegisterTruck _regisTruck = new RegisterTruck();

                    _regisTruck = new RegisterTruck
                    {
                        ShippingID = Guid.NewGuid(),
                        ShippingCode = ShippingCode,
                        DocumentDate = entity.DocumentDate,
                        RegisterTypeID = entity.RegisterTypeID,
                        DockTypeID = entity.DockTypeID,
                        TruckTypeID = entity.TruckTypeID,
                        WarehouseID = warehouse.WarehouseID,
                        ShippingTruckNo = entity.ShippingTruckNo,
                        DriverName = entity.DriverName,
                        LogisticCompany = entity.LogisticCompany,
                        OrderNo = entity.OrderNo,
                        Container_No = entity.Container_No,
                        SealNo = entity.SealNo,
                        BookingNo = entity.BookingNo,
                        PoNo = entity.PoNo,
                        ShippingStatus = (int)ShippingStatusEnum.New,
                        Dispatchcode = entity.DispatchCode,
                        ShiptoID = shipto.ShipToId,
                        ShipptoCode = entity.ShipptoCode,
                        DocumentNo = entity.DocumentNo,
                        CompleteDate = entity.CompleteDate,
                        CancelDate = entity.CancelDate,
                        Remark = entity.Remark,
                        UserCreated = UserID,
                        UserModified = UserID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    entity.RegisTruckDetail.ToList().ForEach(item =>
                    {
                        RegisterTruckDetail _regisTruckDetail = new RegisterTruckDetail();

                        _regisTruckDetail = new RegisterTruckDetail
                        {
                            ShippingDetailID = Guid.NewGuid(),
                            ShippingID = _regisTruck.ShippingID,
                            ProductID = item.ProductID.Value,
                            ShippingQuantity = item.BookingQty.Value,
                            ShippingUnitID = item.BookingStockUnitId.Value,
                            BasicQuantity = item.BookingBaseQty.Value,
                            BasicUnitID = item.BookingBaseUnitId.Value,
                            ConversionQty = item.ConversionQty.Value,
                            ReferenceID = item.ReferenceID.Value,
                            BookingID = item.BookingID.Value,
                            TransactionTypeID = item.TransactionTypeID.Value,
                            Shipping_DT = DateTime.Now,
                            ConfirmQuantity = item.ConfirmQuantity,
                            ConfirmUnitID = item.BookingStockUnitId,
                            ConfirmBasicQuantity = item.ConfirmQuantity * item.ConversionQty,
                            ConfirmBasicUnitID = item.BookingBaseUnitId,
                            UserCreated = UserID,
                            UserModified = UserID,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };
                        _regisTruck.RegisterTruckDetail.Add(_regisTruckDetail);
                    });


                    base.Add(_regisTruck);

                    entity.RegisTruckDetail.ToList().ForEach(items =>
                    {
                        IRepository<DispatchDetail> _disDetail = Context.Repository<DispatchDetail>();
                        DispatchDetail dispatchDetail = _disDetail.FindByID(items.ReferenceID);

                        if (dispatchDetail == null)
                        {
                            throw new HILIException("MSG00006");
                        }
                        decimal _sumBoookingQty = DispatchBookingService.Where(x => x.IsActive == true && x.DispatchDetailId == items.ReferenceID).Sum(x => x.BookingQty);
                        decimal? _sumConfirmQty = regisTruckDetailService.Where(x => x.IsActive == true && x.ReferenceID == items.ReferenceID).Sum(x => x.ConfirmQuantity);

                        if (dispatchDetail.IsBackOrder.HasValue && dispatchDetail.IsBackOrder.Value)
                        {
                            dispatchDetail.DispatchDetailStatus = DispatchDetailStatusEnum.InBackOrder;
                            dispatchDetail.UserModified = UserID;
                            dispatchDetail.DateModified = DateTime.Now;
                            dispatchDetail.IsActive = true;
                            _disDetail.Modify(dispatchDetail);
                        }
                        else
                        {
                            if (_sumBoookingQty == _sumConfirmQty)
                            {
                                dispatchDetail.DispatchDetailStatus =DispatchDetailStatusEnum.Register;
                                dispatchDetail.UserModified = UserID;
                                dispatchDetail.DateModified = DateTime.Now;
                                dispatchDetail.IsActive = true;
                                _disDetail.Modify(dispatchDetail);
                            }
                        }
                    });

                    var id = entity.RegisTruckDetail.FirstOrDefault().DispatchId;
                    //DispatchStatusEnum[] dispatchStatus = new DispatchStatusEnum[] {DispatchStatusEnum.InBackOrder };
                    var dispatch = DispatchService.FindByID(id);
                    var countdetail = DispatchDetailService.Query().Filter(x => x.IsActive == true && DispatchDetailStatusEnum.InBackOrder == x.DispatchDetailStatus &&  x.DispatchId == id).Get().Count();
                    if (countdetail == 0)
                    {
                        if (dispatch != null)
                        {
                            dispatch.DispatchStatus = DispatchStatusEnum.Register;
                            dispatch.UserModified = this.UserID;
                            dispatch.DateModified = DateTime.Now;
                            DispatchService.Modify(dispatch);
                        }
                    }
                    else
                    {
                        if (dispatch != null)
                        {
                            dispatch.DispatchStatus = DispatchStatusEnum.InBackOrder;
                            dispatch.UserModified = this.UserID;
                            dispatch.DateModified = DateTime.Now;
                            DispatchService.Modify(dispatch);
                        }
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

        public bool ModifyRegisTruck(RegisTruckModel entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                        
                    #region [ Update Register Truck ]

                    RegisterTruck _truck_do = FirstOrDefault(s => s.ShippingCode == entity.ShippingCode);

                    _truck_do.RegisterTypeID = entity.RegisterTypeID;
                    _truck_do.TruckType = entity.TruckType;
                    _truck_do.DockTypeID = entity.DockTypeID;
                    _truck_do.ShippingTruckNo = entity.ShippingTruckNo;
                    _truck_do.DriverName = entity.DriverName;
                    _truck_do.LogisticCompany = entity.LogisticCompany;

                    _truck_do.DocumentDate = entity.DocumentDate;
                    _truck_do.PoNo = entity.PoNo;
                    _truck_do.OrderNo = entity.OrderNo;
                    _truck_do.Container_No = entity.Container_No;
                    _truck_do.SealNo = entity.SealNo;
                    _truck_do.BookingNo = entity.BookingNo;
                    //_truck_do.ShippingStatus = ShippingStatus;
                    _truck_do.Remark = entity.Remark;
                    _truck_do.DocumentNo = entity.DocumentNo;
                    _truck_do.ShippingCode = entity.ShippingCode;
                    _truck_do.Dispatchcode = entity.DispatchCode;

                    _truck_do.UserModified = UserID;
                    _truck_do.DateModified = DateTime.Now;
                    base.Modify(_truck_do);

                    #endregion [ Update Register Truck ]

                    List<RegisterTruckDetail> _truck_detail = regisTruckDetailService.Where(d => d.ShippingID == entity.ShippingID && d.IsActive == true).ToList();

                    #region [ Delete Detail ]

                    IEnumerable<Guid?> intGridID = entity.RegisTruckDetail.Select(g => g.ShippingDetailID);

                    IEnumerable<RegisterTruckDetail> _delete_detail = _truck_detail.Where(w => !intGridID.Contains(w.ShippingDetailID));

                    foreach (RegisterTruckDetail _regis_item in _delete_detail)
                    {
                        _regis_item.IsActive = false;

                        _regis_item.UserModified = UserID;
                        _regis_item.DateModified = DateTime.Now;

                        IEnumerable<DispatchBooking> booking = DispatchBookingService.Where(x => x.BookingId == _regis_item.BookingID).ToList();

                        foreach (DispatchBooking b in booking)
                        {
                            b.IsActive = false;
                        }
                    }

                    //e.SaveChanges();
                    #endregion [ Delete Detail ]


                    #region [ Insert Detail ]

                    List<RegisterTruckDetailModel> insertDetail = entity.RegisTruckDetail.Where(w => w.ShippingDetailID == Guid.Empty).ToList();
                    AutoMapper.Mapper.CreateMap<RegisterTruckDetailModel, RegisterTruckDetail>();
                    List<RegisterTruckDetail> _regisTruckDetail = AutoMapper.Mapper.Map<List<RegisterTruckDetailModel>, List<RegisterTruckDetail>>(insertDetail);

                    foreach (RegisterTruckDetail insertItem in _regisTruckDetail)
                    {
                        //var booking = e.op_dispatch_booking.Where(x => x.DispatchDetail_ID == insertItem.DispatchDetail_ID);
                        //foreach (var b in booking)
                        //{
                        //    if (!b.RegisterTruck.Value)
                        //        b.RegisterTruck = true;
                        //    else
                        //        return false;
                        //}

                        insertItem.IsActive = true;
                        insertItem.DateCreated = DateTime.Now;
                        insertItem.UserCreated = UserID;
                        insertItem.DateModified = DateTime.Now;
                        insertItem.UserModified = UserID;
                        insertItem.ShippingID = entity.ShippingID;
                        //insertItem.ShippingCode = entity.ShippingCode;
                        regisTruckDetailService.Modify(insertItem);
                        //e.op_truck_consolidate_do_detail.Add(insertItem);
                    }

                    //e.SaveChanges();

                    #endregion [ Insert Detail ]

                    #region [ Update Detail ]

                    //var _updateDetail = entity.RegisTruckDetail.Where(w => w.ShippingDetailID > 0).ToList();
                    List<RegisterTruckDetailModel> _updateDetail = entity.RegisTruckDetail.Where(w => w.ShippingDetailID != Guid.Empty).ToList();

                    foreach (RegisterTruckDetailModel iUpdate in _updateDetail)
                    {
                        RegisterTruckDetail _getUpdate = regisTruckDetailService.FirstOrDefault(p => p.ShippingDetailID == iUpdate.ShippingDetailID);

                        //_getUpdate.Delivery_Qty = iUpdate.Delivery_Qty.ToInt();
                        //_getUpdate.Remark = iUpdate.Remark;

                        _getUpdate.DateModified = DateTime.Now;
                        _getUpdate.UserModified = UserID;
                    }

                    //e.SaveChanges();
                    #endregion

                    if (_truck_do.ShippingStatus == (int)ShippingStatusEnum.Assign)//_truck_do.IsApprove.Value
                    {
                        CreatePickingJob(_truck_do);
                    }

                    //var _current = Query().Filter(x => x.ZoneID == entity.ZoneID).Include(x => x.ZoneType).Get().FirstOrDefault();

                    //if (_current == null)
                    //    throw new Exception("Not found Zone");

                    //entity.UserModified = this.UserID;
                    //entity.DateModified = DateTime.Now;

                    //base.Modify(entity);
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

        public bool AssignModify(RegisTruckModel entity)
        {
            try
            {
                List<RegisterTruckDetailModel> _regisDetail = GetConfirmRegisTruck(entity.ShippingID);
                if (_regisDetail == null)
                {
                    throw new HILIException("MSG00006");
                }

                List<DispatchAllModel> _picking = GetAssignPick(entity.ShippingCode);
                if (!_picking.Any())
                {
                    throw new HILIException("MSG00006");
                }
                var dupicatePicking = PickingService.Where(e => e.ShippingCode == entity.ShippingCode && e.PONo == entity.PoNo
                                                            && e.DispatchCode == entity.DispatchCode && e.DocumentNo == entity.DocumentNo && e.IsActive).ToList();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    dupicatePicking.ForEach(e =>
                    {
                        if (e.PickingStatus == PickingStatusEnum.WaitingPick)
                        {
                            e.PickingStatus = PickingStatusEnum.Cancel;
                            e.UserModified = UserID;
                            e.DateModified = DateTime.Now;
                            PickingService.Modify(e);
                        }
                    });
                    if(!dupicatePicking.Any(e=>e.PickingStatus== PickingStatusEnum.Complete))
                    {
                        _regisDetail.ToList().ForEach(item =>
                        {
                            RegisterTruckDetail _rTruckDetail = regisTruckDetailService.FindByID(item.ShippingDetailID);
                            if (_regisDetail == null)
                            {
                                throw new HILIException("MSG00006");
                            }
                            _rTruckDetail.ConfirmQuantity = item.ConfirmQuantity;//--Modifiy
                            _rTruckDetail.ConfirmUnitID = item.ConfirmUnitID;//--Modifiy
                            _rTruckDetail.ConfirmBasicQuantity = item.ConfirmBasicQuantity;//--Modifiy
                            _rTruckDetail.ConfirmBasicUnitID = item.ConfirmBasicUnitID;//--Modifiy
                            _rTruckDetail.UserModified = UserID;
                            _rTruckDetail.DateModified = DateTime.Now;
                            regisTruckDetailService.Modify(_rTruckDetail);
                            //var _disBooking = unitofwork.Repository<DispatchBooking>();
                            DispatchBooking Booking = DispatchBookingService.FindByID(item.BookingID);
                            if (Booking == null)
                            {
                                throw new HILIException("MSG00006");
                            }
                            decimal _sumBookingQty = DispatchBookingService.Where(x => x.IsActive == true && x.BookingId == item.BookingID).Sum(x => x.BookingQty);
                            decimal? _sumConfirmQty = regisTruckDetailService.Where(x => x.IsActive == true && x.BookingID == item.BookingID).Sum(x => x.ConfirmQuantity);
                            if (_sumBookingQty == _sumConfirmQty)
                            {
                                Booking.BookingStatus = BookingStatusEnum.Complete;
                                Booking.UserModified = UserID;
                                Booking.DateModified = DateTime.Now;
                                Booking.IsActive = true;
                                DispatchBookingService.Modify(Booking);
                            }
                        });

                        _picking.ForEach(item =>
                        {
                            #region [ PreFix ]

                            PickingPrefix prefix = PickingPrefixService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                            if (prefix == null)
                            {
                                throw new HILIException("PK10012");
                            }

                            PickingPrefix tPrefix = PickingPrefixService.FindByID(prefix.PrefixID);

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

                            PickingPrefixService.Add(newPrefix);

                            #endregion [ PreFix ]

                            Picking _pickModel = new Picking
                            {
                                PickingID = Guid.NewGuid(),
                                PickingCode = PickingCode,
                                PickingStartDate = DateTime.Now,
                                PickingCompleteDate = null,
                                PickingEntryDate = DateTime.Now,
                                PickingStatus = PickingStatusEnum.WaitingPick,
                                ShippingCode = entity.ShippingCode,
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
                            PickingService.Add(_pickModel);
                        });
                        List<DispatchAllModel> _pickingAssign = GetAssignPickingJob(entity.ShippingCode);
                        if (_pickingAssign.Count() == 0)
                        {
                            throw new HILIException("PK10012");
                        }
                        int seq = 1;
                        _pickingAssign.ForEach(item =>
                        {
                            PickingAssign _detail = new PickingAssign
                            {
                                AssignID = Guid.NewGuid(),
                                OrderPick = seq,
                                ShippingDetailID = item.ShippingDetailID,
                                PickingID = item.PickingID,
                                ProductID = item.ProductID,
                                BaseQuantity = item.ConfirmBasicQuantity,
                                BaseUnitID = item.ConfirmBasicUnitID,
                                Barcode = item.PalletCode,
                                StockUnitID = item.ConfirmUnitID,
                                StockQuantity = item.ConfirmQuantity,
                                SuggestionLocationID = item.LocationId,
                                RefLocationID = item.LocationId,
                                PalletCode = item.PalletCode,
                                RefPalletCode = item.PalletCode,
                                PalletQty = item.RemainQTY,
                                PalletUnitID = item.ConfirmUnitID,
                                PickingLot = item.ProductLot,
                                AssignStatus = PickingStatusEnum.WaitingPick,
                                BookingID = item.BookingId,
                                DateCreated = DateTime.Now,
                                UserCreated = UserID,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                IsActive = true
                            };
                            PickingAssignService.Add(_detail);
                            seq++;
                        });
                    }                 
                    RegisterTruck _regisTruck = FirstOrDefault(x => x.IsActive == true && x.ShippingCode == entity.ShippingCode);
                    if (_regisTruck == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    RegisterTruck regisTruck = FindByID(_regisTruck.ShippingID);
                    regisTruck.ShippingTruckNo = entity.ShippingTruckNo;
                    regisTruck.ShippingStatus = (int)ShippingStatusEnum.Assign;
                    regisTruck.CompleteDate = DateTime.Now;
                    base.Modify(regisTruck);
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

        public bool RemoveRegisTruck(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    RegisterTruck _regTruck = FirstOrDefault(x => x.IsActive == true &&
                                    x.ShippingID == id &&
                                    x.ShippingStatus == (int)ShippingStatusEnum.New);

                    if (_regTruck == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _regTruck.IsActive = false;
                    _regTruck.DateModified = DateTime.Now;
                    _regTruck.UserModified = UserID;
                    base.Modify(_regTruck);

                    List<RegisterTruckDetail> _regTruckDetail = regisTruckDetailService.Where(x => x.IsActive == true &&
                                                                                      x.ShippingID == id).ToList();
                    if (_regTruckDetail.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _regTruckDetail.ForEach(items =>
                    {
                        items.IsActive = false;
                        items.DateModified = DateTime.Now;
                        items.UserModified = UserID;
                        regisTruckDetailService.Modify(items);


                        IRepository<DispatchDetail> _disDetail = Context.Repository<DispatchDetail>();
                        DispatchDetail _dispatchDetail = _disDetail.FindByID(items.ReferenceID);

                        //var _dispatchDetail = DispatchDetailService.Query().Filter(x => x.IsActive == true &&
                        //                                                          x.DispatchDetailId == items.ReferenceID)
                        //                                                          .Get().FirstOrDefault();
                        if (_dispatchDetail == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        _dispatchDetail.DispatchDetailStatus = DispatchDetailStatusEnum.InprogressConfirm;
                        _dispatchDetail.IsActive = true;
                        _dispatchDetail.DateModified = DateTime.Now;
                        _dispatchDetail.UserModified = UserID;
                        DispatchDetailService.Modify(_dispatchDetail);
                    });


                    DispatchDetailStatusEnum[] dispatchStatus = new DispatchDetailStatusEnum[] { DispatchDetailStatusEnum.Register,DispatchDetailStatusEnum.InBackOrder };
                    Dispatch _dispatch = DispatchService.FirstOrDefault(x => x.IsActive == true &&
                                                          x.DispatchCode == _regTruck.Dispatchcode);

                    List<DispatchDetail> _details = DispatchDetailService.Where(x => x.IsActive == true &&
                                                          x.DispatchId == _dispatch.DispatchId &&
                                                          dispatchStatus.Contains(x.DispatchDetailStatus)).ToList();

                    if (_details.Count() == 0)
                    {
                        IRepository<Dispatch> dpatch = Context.Repository<Dispatch>();
                        Dispatch dispatch = DispatchService.FirstOrDefault(x => x.IsActive == true &&
                                                          x.DispatchCode == _regTruck.Dispatchcode);

                        if (dispatch == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        dispatch.DispatchStatus =DispatchStatusEnum.InprogressConfirm;
                        dispatch.DateModified = DateTime.Now;
                        dispatch.UserModified = UserID;
                        dpatch.Modify(dispatch);
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

        public void CreatePickingJob(RegisterTruck regisTruck)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {

                    var dType = from _dispatchType in DocumentTypeService.Where(x=>x.IsActive)
                                join dispatch in DispatchService.Where(x => x.IsActive == true)on _dispatchType.DocumentTypeID equals dispatch.DocumentId
                                where dispatch.DispatchCode == regisTruck.Dispatchcode
                                select new { dispatchType = _dispatchType.Name };

                    //var pickingStatus = e.sys_status.Where(x => (x.KeyType == "PK" || x.KeyType == "AA")
                    //                                        && x.IsDefault == true).FirstOrDefault()?.Status_Code;

                    string reason = "เบิกจากการ : " + dType.FirstOrDefault()?.dispatchType;

                    Picking _picking = new Picking
                    {
                        PickingStatus = PickingStatusEnum.WaitingPick,
                        ShippingCode = regisTruck.ShippingCode,
                        DispatchCode = regisTruck.Dispatchcode,
                        PickingCloseReason = reason,
                        DateCreated = DateTime.Now,
                        UserCreated = UserID,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        IsActive = true
                    };


                    //var programCode = ModuleManage.GetProgramCode(ProgramCode.P_0041_PickingList);
                    //var _sysPrefix = e.sys_prefix.FirstOrDefault(x => x.ModuleKey == programCode);
                    //string pickingCode = Prefix.OnCreatePrefixed(_sysPrefix.LastedKey, _sysPrefix.PrefixKey, _sysPrefix.FormatKey, _sysPrefix.LengthKey);
                    //_picking.Picking_Code = pickingCode;
                    //_sysPrefix.LastedKey = pickingCode;

                    //e.op_picking.Add(_picking);

                    #region [ PreFix ]

                    PickingPrefix prefix = PickingPrefixService.FirstOrDefault(x => x.IsLastest.HasValue && x.IsLastest.Value);
                    if (prefix == null)
                    {
                        throw new HILIException("PK10012");
                    }

                    PickingPrefix tPrefix = PickingPrefixService.FindByID(prefix.PrefixID);

                    string PickingCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                    _picking.PickingCode = PickingCode;
                    tPrefix.IsLastest = false;

                    PickingPrefix newPrefix = new PickingPrefix()
                    {
                        IsLastest = true,
                        LastedKey = PickingCode,
                        PrefixKey = prefix.PrefixKey,
                        FormatKey = prefix.FormatKey,
                        LengthKey = prefix.LengthKey
                    };

                    PickingPrefixService.Add(newPrefix);

                    #endregion [ PreFix ]


                    List<RegisterTruckDetail> _truck_do_detail = regisTruckDetailService.Where(x => x.ShippingID == regisTruck.ShippingID && x.IsActive == true).ToList();

                    foreach (RegisterTruckDetail registruckDetail in _truck_do_detail)
                    {
                        DispatchDetail _dispatch_detail = DispatchDetailService.FirstOrDefault(x => x.DispatchDetailId == registruckDetail.ReferenceID && x.IsActive == true);//.Get().FirstOrDefault(); //e.op_dispatch_detail.Where(d => d.DispatchDetail_ID == registruckDetail.DispatchDetail_ID && d.IsActive == true).FirstOrDefault();

                        if (_dispatch_detail != null)
                        {
                           var _dispatch_book = DispatchBookingService.Where(x => x.BookingId == registruckDetail.BookingID && x.IsActive == true).ToList(); //e.op_dispatch_booking.Where(d => d.DispatchDetail_ID == registruckDetail.DispatchDetail_ID && d.IsActive == true);

                            foreach (DispatchBooking book in _dispatch_book)
                            {

                                string _wherehouseCode = (from l in locationService.Where(x=>x.IsActive)
                                                          join z in physicalZoneService.Where(x => x.IsActive) on l.ZoneID equals z.Physicalzone_Id
                                                          select z.Warehouse_Code).FirstOrDefault();

                                decimal actual = book.BookingQty * (ProductUnitService.FirstOrDefault(x => x.ProductUnitID == book.BookingStockUnitId)?.Quantity ?? 0);

                                PickingDetail _detail = new PickingDetail
                                {
                                    PickingDetailID = Guid.NewGuid(),
                                    AssignID = Guid.NewGuid(),
                                    PickStockUnitID = book.BookingStockUnitId,
                                    PickStockQty = book.BookingQty,
                                    PickBaseUnitID = book.BookingBaseUnitId,
                                    PickBaseQty = book.BookingBaseQty,
                                    ConversionQty = book.ConversionQty,
                                    PickingStatus = (int)ShippingStatusEnum.Assign,
                                    PickingReason = reason,
                                    LocationID = book.LocationId,
                                    PalletCode = book.ProductLot,
                                    DateCreated = DateTime.Now,
                                    UserCreated = UserID,
                                    DateModified = DateTime.Now,
                                    UserModified = UserID,
                                    IsActive = true

                                    //Dispatch_code = registruckDetail.Dispatch_Code,
                                    //Job_Picking_CBM = 0,
                                    //Job_Picking_Confirm = 0,
                                    //DispatchDetail_ID = _dispatch_detail.DispatchDetail_ID,
                                    //Job_Picking_Confirm_To_Location = "",//_trans.Stock_Location_No,
                                    //Job_Picking_Detail_Reason = reason,
                                    //Job_Picking_Detail_Status = StatusCode.ASSIGN.GetStatus(),
                                    //Job_Picking_Package_Actual = actual,
                                    //Job_Picking_Quantity = book.BookingQty,
                                    //Job_Picking_Weight = _dispatch_detail.DispatchDetail_Product_Weight,
                                    //Picking_Code = _picking.Picking_Code,
                                    //Picking_Product_Lot = book.Product_Booking_Lot,
                                    //Product_System_Code = _dispatch_detail.Product_System_Code,
                                    //Product_UOM_ID = _dispatch_detail.Product_UOM_ID,

                                    //IsActive = true,
                                    //Shipping_Detail_ID = registruckDetail.Shipping_Detail_ID,
                                    //Job_Picking_Suggest_To_Location = book.Location_No,
                                    //Warehouse_Code = _wherehouseCode,
                                    //Pallet_Code = book.Product_Booking_Lot,
                                    //Damage_Qty = 0,

                                    //Booking_ID = book.Booking_ID,

                                    //Create_Date = DateTime.Now,
                                    //Create_User = this._userID,
                                    //Update_Date = DateTime.Now,
                                    //Update_User = this._userID,


                                };
                                PickingDetailService.Add(_detail);

                                // book.RegisterTruck = true;
                            }

                            //var _dispatch = e.op_dispatch.Where(d => d.Dispatch_Code == _dispatch_detail.Dispatch_Code && d.IsActive == true).FirstOrDefault();

                            //_dispatch.Dispatch_Status_Dispatch = StatusCode.ASSIGN.GetStatus();
                            //_dispatch_detail.DispatchDetail_Status = StatusCode.ASSIGN.GetStatus();
                        }
                    }

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

        #region Consolidate
        public RegisterTruckConsolidateHeaderModel GetConsolidateByPO(string pono, string documentNo)
        {
            try
            {
                SqlParameter param = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param2 = new SqlParameter("@DocumentNo", SqlDbType.NVarChar) { Value = documentNo };

                RegisterTruckConsolidateHeaderModel result = Context.SQLQuery<RegisterTruckConsolidateHeaderModel>("exec SP_GetConsolidateByPO @PoNo, @DocumentNo", param, param2).FirstOrDefault();


                if (result == null)
                {
                    throw new HILIException("MSG00047");
                }

                result.RegisterTypeName = GetRegisterTypeEnumDescription((RegisterTruckEnum)Enum.Parse(typeof(RegisterTruckEnum), result.RegisterTypeID.ToString()));
                result.ShippingStatusName = GetShippingStatusEnumDescription((ShippingStatusEnum)Enum.Parse(typeof(ShippingStatusEnum), result.ShippingStatus.Value.ToString()));

                SqlParameter param11 = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param22 = new SqlParameter("@DocumentNo", SqlDbType.NVarChar) { Value = documentNo };

                List<RegisterTruckConsolidateDeatilModel> result_detail = Context.SQLQuery<RegisterTruckConsolidateDeatilModel>("exec SP_GetConsolidateDetailByPO @PoNo, @DocumentNo", param11, param22).ToList();


                result.RegisterTruckConsolidateDeatilModels = result_detail;
                return result;


                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                //{
                //    var result = (from _regtruck in Query().Filter(x => x.PoNo == pono && x.DocumentNo == documentNo).Get()
                //                  join _trucktype in truckTypeService.Query().Get() on _regtruck.TruckTypeID equals _trucktype.TruckTypeID
                //                  join _warehouse in WarehouseService.Query().Get() on _regtruck.WarehouseID equals _warehouse.WarehouseID
                //                  join _docktype in dockConfigService.Query().Get() on _regtruck.DockTypeID equals _docktype.DockConfigID
                //                  join _shipto in ShiptoService.Query().Get() on _regtruck.ShiptoID equals _shipto.ShipToId
                //                  select new RegisterTruckConsolidateHeaderModel
                //                  {
                //                      ShippingID = _regtruck.ShippingID,
                //                      ShippingCode = _regtruck.ShippingCode,
                //                      DocumentDate = _regtruck.DocumentDate,
                //                      RegisterTypeID = _regtruck.RegisterTypeID,
                //                      RegisterTypeName = GetRegisterTypeEnumDescription((RegisterTruckEnum)Enum.Parse(typeof(RegisterTruckEnum), _regtruck.RegisterTypeID.ToString())),
                //                      TruckTypeName = _trucktype.TypeName,
                //                      DockTypeID = _regtruck.DockTypeID,
                //                      DockTypeName = _docktype.DockName,
                //                      TruckTypeID = _regtruck.TruckTypeID,
                //                      WarehouseID = _regtruck.WarehouseID,
                //                      WarehouseName = _warehouse.Name,
                //                      ShippingTruckNo = _regtruck.ShippingTruckNo,
                //                      DriverName = _regtruck.DriverName,
                //                      LogisticCompany = _regtruck.LogisticCompany,
                //                      OrderNo = _regtruck.OrderNo,
                //                      Container_No = _regtruck.Container_No,
                //                      ShippingStatus = _regtruck.ShippingStatus,
                //                      ShippingStatusName = GetShippingStatusEnumDescription((ShippingStatusEnum)Enum.Parse(typeof(ShippingStatusEnum), _regtruck.ShippingStatus.Value.ToString())),
                //                      SealNo = _regtruck.SealNo,
                //                      BookingNo = _regtruck.BookingNo,
                //                      PoNo = _regtruck.PoNo,
                //                      Dispatchcode = _regtruck.Dispatchcode,
                //                      ShiptoID = _regtruck.ShiptoID,
                //                      ShiptoName = _shipto.Name,
                //                      ShipptoCode = _regtruck.ShipptoCode,
                //                      DocumentNo = _regtruck.DocumentNo,
                //                      CompleteDate = _regtruck.DocumentDate,
                //                      CancelDate = _regtruck.CancelDate,
                //                      Remark = _regtruck.Remark
                //                  }
                //                 ).FirstOrDefault();

                //    if (result == null)
                //        throw new HILIException("MSG00047");


                //    var result_detail = (from _consolidate in regisTruckConsolidateService.Query().Filter(x => x.IsActive).Get()
                //                         join _truckdetail in regisTruckDetailService.Query().Filter(x => x.IsActive).Get() on _consolidate.ShippingDetailID equals _truckdetail.ShippingDetailID
                //                         join _truck in Query().Filter(x => x.IsActive && x.PoNo == pono && x.DocumentNo == documentNo).Get() on _truckdetail.ShippingID equals _truck.ShippingID
                //                         join _pick in PickingService.Query().Filter(x => x.IsActive && x.PONo == pono).Get() on _truck.ShippingCode equals _pick.ShippingCode
                //                         join _pickassign in PickingAssignService.Query().Filter(x => x.IsActive).Get() on _pick.PickingID equals _pickassign.PickingID
                //                         join _pickdetail in PickingDetailService.Query().Filter(x => x.IsActive).Get() on _pickassign.AssignID equals _pickdetail.AssignID
                //                         where _truckdetail.ProductID == _pickassign.ProductID
                //                         && _consolidate.PalletCode == _pickdetail.PalletCode
                //                         && _consolidate.StockUnitID == _pickdetail.PickStockUnitID
                //                         && _consolidate.BaseUnitID == _pickdetail.PickBaseUnitID
                //                         join _product in ProductService.Query().Get() on _pickassign.ProductID equals _product.ProductID
                //                         join _productcode in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get() on _product.ProductID equals _productcode.ProductID
                //                         join _location in locationService.Query().Get() on _pickdetail.LocationID equals _location.LocationID
                //                         join _locationsuggest in locationService.Query().Get() on _pickassign.SuggestionLocationID equals _locationsuggest.LocationID
                //                         join _uomstock in ProductUnitService.Query().Get() on _pickassign.StockUnitID equals _uomstock.ProductUnitID
                //                         join _uompallet in ProductUnitService.Query().Get() on _pickassign.PalletUnitID equals _uompallet.ProductUnitID
                //                         join _uomconso in ProductUnitService.Query().Get() on _consolidate.StockUnitID equals _uomconso.ProductUnitID
                //                         select new RegisterTruckConsolidateDeatilModel
                //                         {
                //                             DeliveryID = _consolidate.DeliveryID,
                //                             OrderPick = _pickassign.OrderPick.Value,
                //                             ProductId = _pickassign.ProductID,
                //                             ProductName = _product.Name,
                //                             ProductCode = _productcode.Code,
                //                             LocationId = _pickdetail.LocationID,
                //                             LocationCode = _location.Code,
                //                             LocationSuggestId = _pickassign.SuggestionLocationID,
                //                             LocationSuggestCode = _locationsuggest.Code,
                //                             PickStockUnitId = _pickdetail.PickStockUnitID,
                //                             PickStockUnitName = _uomstock.Name,
                //                             PalletCode = _pickassign.PalletCode,
                //                             PalletQty = _pickassign.PalletQty.Value,
                //                             PalletUnitId = _pickassign.PalletUnitID,
                //                             PalletUnitName = _uompallet.Name,
                //                             PickStockQty = _pickdetail.PickStockQty.Value,
                //                             PickBaseQty = _pickdetail.PickBaseQty.Value,
                //                             DispatchCode = _truck.Dispatchcode,
                //                             ConsolidateQty = _consolidate.StockQuantity,
                //                             ConsolidateUnitId = _consolidate.StockUnitID,
                //                             ConsolidateUnitName = _uomconso.Name,
                //                             AssignID = _pickassign.AssignID,
                //                             Pono = _truck.PoNo
                //                         }
                //                  ).OrderBy(o => o.OrderPick).ToList();

                //    result.RegisterTruckConsolidateDeatilModels = result_detail;
                //    return result;
                //}
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

        public List<RegisterTruckConsolidateListModel> GetConsolidateAll(string pono, string documentno, int? status, DateTime? datafrom, DateTime? datato, string licenseplate, out int totalRecords, int? pageIndex, int? pageSize)
        {
            DateTime startDate = datafrom.HasValue ? DateTime.Parse(datafrom.Value.ToString("yyyy/MM/dd 00:00:00")) : DateTime.MinValue;
            DateTime endDate = datato.HasValue ? DateTime.Parse(datato.Value.ToString("yyyy/MM/dd 23:59:59")) : DateTime.MaxValue;

            var tmp = (from _consolidate in regisTruckConsolidateService.Where(e => e.IsActive)
                          join _truckdetail in regisTruckDetailService.Where(e => e.IsActive) on _consolidate.ShippingDetailID equals _truckdetail.ShippingDetailID
                          join _truck in Where(e => e.IsActive) on _truckdetail.ShippingID equals _truck.ShippingID
                          where (!String.IsNullOrEmpty(licenseplate) ? _truck.ShippingTruckNo.Contains(licenseplate) : true)
                          && (!String.IsNullOrEmpty(pono) ? _truck.PoNo.Contains(pono) : true) &&
                                       (!String.IsNullOrEmpty(documentno) ? _truck.DocumentNo.Contains(documentno) : true) &&
                                       (status != null ? _consolidate.ConsolidateStatus == status : true) &&
                                       (datafrom != null ? _consolidate.DateCreated >= startDate : true) &&
                                       (datato != null ? _consolidate.DateCreated <= endDate : true)
                          select new { _truck, _consolidate }).ToList();

            var result = (from t in tmp
                          select new
                          {
                              PoNo = t._truck.PoNo,
                              DocumentNo = t._truck.DocumentNo,
                              DateCreated = t._consolidate.DateCreated,
                              ConsolidateStatusId = t._truck.ShippingStatus.GetValueOrDefault(),
                              ConsolidateStatusName = GetConsolidateTypeEnumDescription((ConsolidateStatusEnum)Enum.Parse(typeof(ConsolidateStatusEnum), t._truck.ShippingStatus.ToString())),
                          } into g
                          group g by new
                          {
                              g.PoNo,
                              g.DocumentNo,
                          } into x
                          select new RegisterTruckConsolidateListModel
                          {
                              PoNo = x.Key.PoNo,
                              DocumentNo = x.Key.DocumentNo,
                              DateCreated = x.Min(s => s.DateCreated),
                              ConsolidateStatusId = x.Min(s => s.ConsolidateStatusId),
                              ConsolidateStatusName = GetConsolidateTypeEnumDescription((ConsolidateStatusEnum)Enum.Parse(typeof(ConsolidateStatusEnum), x.Min(s => s.ConsolidateStatusId).ToString())),
                          }).ToList(); 
            totalRecords = result.Count();

            if (pageIndex != null && pageSize != null)
            {
                result = result.OrderBy(x => x.PoNo).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }
            return result;

            /*

           var tmps = regisTruckConsolidateService.Where(e => (status.HasValue&& status.Value>0 ? e.ConsolidateStatus == status : true)
               && (!string.IsNullOrEmpty(licenseplate) ? e.RegisterTruckDetail.RegisterTruck.ShippingTruckNo.Contains(licenseplate) : true)
               && (!string.IsNullOrEmpty(pono) ? e.RegisterTruckDetail.RegisterTruck.PoNo.Contains(pono) : true)
               && (!string.IsNullOrEmpty(documentno) ? e.RegisterTruckDetail.RegisterTruck.DocumentNo.Contains(documentno) : true) 
               && (datafrom.HasValue ? e.DateCreated >= startDate : true)
               && (datato.HasValue ? e.DateCreated <= endDate : true))
               .Select(e => new
               {
                   RegisterTruckConsolidate = e,
                   RegisterTruck = e.RegisterTruckDetail.RegisterTruck,
                   RegisterTruckDetail = e.RegisterTruckDetail
               }).ToList();

           var result = (from t in tmps
                         where t.RegisterTruckDetail != null
                         select new
                         {
                             PoNo = t.RegisterTruck.PoNo,
                             DocumentNo = t.RegisterTruck.DocumentNo,
                             DateCreated = t.RegisterTruckConsolidate.DateCreated,
                             ConsolidateStatusId = t.RegisterTruckConsolidate.ConsolidateStatus,
                             ConsolidateStatusName = GetConsolidateTypeEnumDescription((ConsolidateStatusEnum)Enum.Parse(typeof(ConsolidateStatusEnum),
                             t.RegisterTruckConsolidate.ConsolidateStatus.ToString())),
                         }).ToList();

           List<RegisterTruckConsolidateListModel> results = (from t in result
                                                              select t
                             into g
                                                              group g by new
                                                              {
                                                                  g.PoNo,
                                                                  g.DocumentNo,
                                                              } into x
                                                              select new RegisterTruckConsolidateListModel
                                                              {
                                                                  PoNo = x.Key.PoNo,
                                                                  DocumentNo = x.Key.DocumentNo,
                                                                  DateCreated = x.Min(s => s.DateCreated),
                                                                  ConsolidateStatusId = x.Min(s => s.ConsolidateStatusId),
                                                                  ConsolidateStatusName = GetConsolidateTypeEnumDescription((ConsolidateStatusEnum)Enum.Parse(typeof(ConsolidateStatusEnum), x.Min(s => s.ConsolidateStatusId).ToString())),
                                                              }).ToList();

           totalRecords = results.Count();

           if (pageIndex != null && pageSize != null)
           {
               return results.OrderBy(x => x.PoNo).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
           }
           return results.ToList();*/
        }

        public bool ModifyConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {


                    entity.ForEach(item =>
                    {
                        RegisterTruckConsolidate _updatecon = regisTruckConsolidateService.FirstOrDefault(x => x.DeliveryID == item.DeliveryID);

                        if (_updatecon == null)
                        {
                            throw new HILIException("MSG00060");
                        }

                        if (item.ConsolidateQty > item.PickStockQty)
                        {
                            throw new HILIException("MSG00062");
                        }

                        RegisterTruckDetail _regdetail = regisTruckDetailService.FirstOrDefault(x => x.ShippingDetailID == _updatecon.ShippingDetailID);
                        if (_regdetail != null)
                        {
                            _updatecon.BaseQuantity = item.ConsolidateQty * _regdetail.ConversionQty;
                        }
                        _updatecon.StockQuantity = item.ConsolidateQty;
                        _updatecon.UserCreated = UserID;
                        _updatecon.DateModified = DateTime.Now;

                        regisTruckConsolidateService.Modify(_updatecon);


                        PickingDetail _updatepickdt = PickingDetailService.FirstOrDefault(x => x.AssignID == item.AssignID);

                        if (_updatepickdt == null)
                        {
                            throw new HILIException("MSG00061");
                        }

                        _updatepickdt.PalletCode = item.PalletCode;
                        _updatepickdt.UserCreated = UserID;
                        _updatepickdt.DateModified = DateTime.Now;

                        PickingDetailService.Modify(_updatepickdt);
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

        public bool ApproveConsolidate(List<RegisterTruckConsolidateDeatilModel> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    List<Guid> ShippingDetailIDs = new List<Guid>();
                    entity.ForEach(item =>
                    {
                        RegisterTruckConsolidate _updatecon = regisTruckConsolidateService.FirstOrDefault(x => x.DeliveryID == item.DeliveryID);
                        _updatecon.ConsolidateStatus = (int)ConsolidateStatusEnum.Complete;
                        _updatecon.UserModified = UserID;
                        _updatecon.DateModified = DateTime.Now;
                        regisTruckConsolidateService.Modify(_updatecon);
                        ShippingDetailIDs.Add(_updatecon.ShippingDetailID.GetValueOrDefault());
                    });
                    RegisterTruckConsolidateDeatilModel _dataw = entity.FirstOrDefault();
                    var regTrucks = (from _truck in Where(x => x.IsActive && x.PoNo == _dataw.Pono && _dataw.DispatchCode == x.Dispatchcode)
                                     join _truckdetail in regisTruckDetailService.Where(x => x.IsActive) on _truck.ShippingID equals _truckdetail.ShippingID
                                     where ShippingDetailIDs.Contains(_truckdetail.ShippingDetailID)
                                     select _truck).Distinct().ToList();
                    regTrucks.ForEach(item =>
                    {
                        RegisterTruck reg = FirstOrDefault(x => x.ShippingID == item.ShippingID);
                        reg.ShippingStatus = (int)ConsolidateStatusEnum.Complete;
                        reg.UserModified = UserID;
                        reg.DateModified = DateTime.Now;
                        this.Modify(reg);
                    });
                    //Update Dispatch  Status                      
                    string _dispatchcode = _dataw.DispatchCode;
                    string _pono = _dataw.Pono;
                    Dispatch _dispatchhd = DispatchService.FirstOrDefault(x => x.IsActive && x.DispatchCode == _dispatchcode && x.Pono==_dataw.Pono);
                    //Check Booking Status
                    var regTruck = Where(e => e.IsActive && e.Dispatchcode == _dataw.DispatchCode  && e.PoNo==_dataw.Pono && (e.ShippingStatus != (int)ShippingStatusEnum.Complete) && (e.ShippingStatus != (int)ShippingStatusEnum.Cancel));    
                    _dispatchhd.DispatchDetailCollection = null;
                    if (!regTruck.Any())
                    {
                        _dispatchhd.DispatchStatus = DispatchStatusEnum.WaitingConfirmDispatch;
                    }
                    _dispatchhd.DateModified = DateTime.Now;
                    _dispatchhd.UserModified = UserID;
                    DispatchService.Modify(_dispatchhd); 
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
        #endregion

        #region Function
        public static string GetRegisterTypeEnumDescription(RegisterTruckEnum status)
        {

            Type type = typeof(RegisterTruckEnum);
            MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;

            return description;
        }
        public static string GetShippingStatusEnumDescription(ShippingStatusEnum status)
        {

            Type type = typeof(ShippingStatusEnum);
            MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;

            return description;
        }
        public static string GetConsolidateTypeEnumDescription(ConsolidateStatusEnum status)
        {

            Type type = typeof(ConsolidateStatusEnum);
            MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;

            return description;
        }
        #endregion

        #region [Consolidate HandHeld]
        public List<RegisterTruckConsolidateDeatilModel> GetConsolidateData(string DocNo)
        {
            try
            {

                SqlParameter param = new SqlParameter("@DocNo", SqlDbType.NVarChar) { Value = DocNo };

                List<RegisterTruckConsolidateDeatilModel> resultDetail = Context.SQLQuery<RegisterTruckConsolidateDeatilModel>("exec SP_GetConsolidateData @DocNo ", param).ToList();

                return resultDetail;
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

        public RegisterTruckConsolidateDeatilModel GetDetail(string DocumentNo, string Pallet)
        {
            try
            {

                SqlParameter param = new SqlParameter("@DocumentNo", SqlDbType.NVarChar) { Value = DocumentNo };
                SqlParameter param2 = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = Pallet };

                RegisterTruckConsolidateDeatilModel resultDetail = Context.SQLQuery<RegisterTruckConsolidateDeatilModel>("exec GetConsoByPalletHH @DocumentNo, @PalletCode ", param, param2).SingleOrDefault();

                return resultDetail;
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

        public RegisterTruckConsolidateDeatilModel GetDetailByPallet(string DocumentNo, string ScanPallet)
        {
            try
            {

                SqlParameter param = new SqlParameter("@DocumentNo", SqlDbType.NVarChar) { Value = DocumentNo };
                SqlParameter param2 = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = ScanPallet };

                RegisterTruckConsolidateDeatilModel resultDetail = Context.SQLQuery<RegisterTruckConsolidateDeatilModel>("exec GetPalletConfirmConsoHH @DocumentNo, @PalletCode ", param, param2).SingleOrDefault();



                return resultDetail;
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

        public List<RegisterTruckConsolidateDeatilModel> GetCheckDocData(Guid ShippingID, string DockNo)
        {
            try
            {
                List<RegisterTruckConsolidateDeatilModel> result = (from _regtruck in Where(x => x.IsActive)
                                                                    join _dockConfig in dockConfigService.Where(x => x.IsActive == true)
                                                                       on _regtruck.DockTypeID equals _dockConfig.DockConfigID
                                                                    where (_dockConfig.Barcode.Contains(DockNo)) &&
                                                                            (ShippingID != null ? _regtruck.ShippingID == ShippingID : true)
                                                                    select new { _regtruck, _dockConfig })
                              .Select(n => new RegisterTruckConsolidateDeatilModel()
                              {
                                  ShippingID = n._regtruck.ShippingID,
                                  DockTypeID = n._regtruck.DockTypeID,
                                  DockName = n._dockConfig.DockName
                              }).ToList();

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

        public bool ConfirmConsolidate(string DocumentNo, Guid ShippingID, string ScanPallet, decimal ConfirmQty)
        {

            try
            {
                if (ScanPallet == null || ConfirmQty == 0)
                {
                    throw new HILIException("MSG00006");
                }

                RegisterTruckConsolidateDeatilModel getConfirm = GetDetailByPallet(DocumentNo, ScanPallet);

                if (getConfirm == null)
                {
                    throw new HILIException("MSG00006");
                }

                RegisterTruckConsolidate ConsoWaitingConfirm = regisTruckConsolidateService.SingleOrDefault(x => x.IsActive &&
                                                                                      x.ConsolidateStatus == (int)ConsolidateStatusEnum.WaitingConfirm &&
                                                                                      x.ShippingDetailID == getConfirm.ShippingDetailID &&
                                                                                      x.PalletCode == ScanPallet);
                if (ConsoWaitingConfirm != null)
                {
                    throw new HILIException("MSG00006");
                }

                SqlParameter param = new SqlParameter("@DocumentNo", SqlDbType.NVarChar) { Value = DocumentNo };
                SqlParameter param2 = new SqlParameter("@ShippingID", SqlDbType.UniqueIdentifier) { Value = ShippingID };
                SqlParameter param3 = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = ScanPallet };
                SqlParameter param4 = new SqlParameter("@ConfirmQty", SqlDbType.Decimal) { Value = ConfirmQty };
                SqlParameter param5 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };

                bool resultDetail = Context.SQLQuery<bool>("exec SP_ConfirmConsolidate @DocumentNo,@ShippingID, @PalletCode, @ConfirmQty,  @UserID", param, param2, param3, param4, param5).SingleOrDefault();

                return resultDetail;
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

        public RegisterTruckConsolidateDeatilModel JobConsoComplete(Guid ShippingID, Guid ShippingDetailID, string PalletCode)
        {
            try
            {

                SqlParameter param = new SqlParameter("@ShippingID", SqlDbType.UniqueIdentifier) { Value = ShippingID };
                SqlParameter param2 = new SqlParameter("@ShippingDetailID", SqlDbType.UniqueIdentifier) { Value = ShippingDetailID };
                SqlParameter param3 = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(PalletCode) ? "" : PalletCode };

                RegisterTruckConsolidateDeatilModel dataModel = Context.SQLQuery<RegisterTruckConsolidateDeatilModel>("exec SP_CheckJobConsoComplete @ShippingID,@ShippingDetailID, @PalletCode ", param, param2, param3).SingleOrDefault();


                return dataModel;
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


        #endregion [Consolidate HandHeld]

    }
}
