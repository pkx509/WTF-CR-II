using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Helpers;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Interface;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Stock;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.RegisterTruckModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using static DITS.HILI.WMS.DispatchModel.CustomModel.DispatchOtherModel;

namespace DITS.HILI.WMS.DispatchService
{
    public class DispatchService : Repository<Dispatch>, IDispatchService
    {
        #region Property
        private readonly IStockService stockService;
        private readonly IRepository<DispatchDetail> DispatchDetailService;
        private readonly IRepository<Contact> ContactService;
        private readonly IRepository<ContactInType> ContactInTypeService;
        private readonly IRepository<DocumentType> DocumentTypeService;
        private readonly IRepository<Product> ProductService;
        private readonly IRepository<ProductCodes> ProductCodeService;
        private readonly IRepository<ProductUnit> ProductUnitService;
        private readonly IRepository<ShippingTo> ShiptoService;
        private readonly IRepository<DispatchPrefix> DispatchPrefixService;
        private readonly IRepository<Warehouse> WarehouseService;
        private readonly IRepository<SpecialBookingRule> SpecialBookingRuleService;
        private readonly IRepository<ProductStatus> ProductStatusService;
        private readonly IRepository<ProductSubStatus> ProductSubStatusService;
        private readonly IRepository<StockInfo> StockInfoService;
        private readonly IRepository<StockBalance> StockBalanceService;
        private readonly IRepository<StockTransaction> StockTransactionService;
        private readonly IRepository<ProductBrand> ProductBrandService;
        private readonly IRepository<ProductShape> ProductShapeService;
        private readonly IRepository<ProductOwner> ProductOwnerService;
        private readonly IRepository<Location> LocationService;
        private readonly IRepository<StockLocationBalance> StockLocationBalanceService;
        private readonly IRepository<ItfInterfaceMapping> ItfInterfaceMappingService;
        private readonly IRepository<DispatchBooking> DispatchBookingService;
        private readonly IRepository<Zone> ZoneService;
        private readonly IRepository<ProductionControlDetail> ProductionControlDetailService;
        private readonly IRepository<ProductionControl> ProductionControlService;
        private readonly IRepository<Picking> PickingService;
        private readonly IRepository<PickingAssign> PickingAssignService;
        private readonly IRepository<PickingDetail> PickingDetailService;
        private readonly IRepository<RegisterTruck> RegisterTruckService;
        private readonly IRepository<RegisterTruckDetail> RegisterTruckDetailService;
        private readonly IRepository<RegisterTruckConsolidate> RegisterTruckConsolidateService;
        private readonly IRepository<ProductStatusMapDocument> ProductStatusMapDocService;
        private readonly IRepository<itf_temp_in_dispatch_log> itf_temp_in_dispatch_logService;
        private readonly IRepository<ItfTransactionType> ItfTransactionTypeService;
        private readonly IRepository<UserAccounts> UserAccoutService;
        private readonly IRepository<Receiving> receivingService;
        private readonly IRepository<Receive> receiveService;
        private readonly IRepository<ReceiveDetail> receiveDetailService;
        private readonly IUnitOfWork unitofwork;
        #endregion

        #region Constructor

        public DispatchService(IUnitOfWork context, IStockService _stockServicee) : base(context)
        {
            unitofwork = context;
            stockService = _stockServicee;
            DispatchDetailService = context.Repository<DispatchDetail>();
            ContactService = context.Repository<Contact>();
            ContactInTypeService = context.Repository<ContactInType>();
            DocumentTypeService = context.Repository<DocumentType>();
            ProductService = context.Repository<Product>();
            ProductCodeService = context.Repository<ProductCodes>();
            ProductUnitService = context.Repository<ProductUnit>();
            ShiptoService = context.Repository<ShippingTo>();
            DispatchPrefixService = context.Repository<DispatchPrefix>();
            WarehouseService = context.Repository<Warehouse>();
            SpecialBookingRuleService = context.Repository<SpecialBookingRule>();
            ProductStatusService = context.Repository<ProductStatus>();
            ProductSubStatusService = context.Repository<ProductSubStatus>();
            StockInfoService = context.Repository<StockInfo>();
            StockBalanceService = context.Repository<StockBalance>();
            StockTransactionService = context.Repository<StockTransaction>();
            ProductBrandService = context.Repository<ProductBrand>();
            ProductShapeService = context.Repository<ProductShape>();
            ProductOwnerService = context.Repository<ProductOwner>();
            LocationService = context.Repository<Location>();
            StockLocationBalanceService = context.Repository<StockLocationBalance>();
            ItfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
            DispatchBookingService = context.Repository<DispatchBooking>();
            ZoneService = context.Repository<Zone>();
            ProductionControlDetailService = context.Repository<ProductionControlDetail>();
            ProductionControlService = context.Repository<ProductionControl>();
            PickingService = context.Repository<Picking>();
            PickingAssignService = context.Repository<PickingAssign>();
            PickingDetailService = context.Repository<PickingDetail>();
            RegisterTruckService = context.Repository<RegisterTruck>();
            RegisterTruckDetailService = context.Repository<RegisterTruckDetail>();
            RegisterTruckConsolidateService = context.Repository<RegisterTruckConsolidate>();
            ProductStatusMapDocService = context.Repository<ProductStatusMapDocument>();
            itf_temp_in_dispatch_logService = context.Repository<itf_temp_in_dispatch_log>();
            ItfTransactionTypeService = context.Repository<ItfTransactionType>();
            UserAccoutService = context.Repository<UserAccounts>();
            receivingService = context.Repository<Receiving>();
            receiveService = context.Repository<Receive>();
            receiveDetailService = context.Repository<ReceiveDetail>();
        }

        #endregion

        #region Method
        #region Import
        public List<ObjectPropertyValidatorException> ImportPreDispatch(List<PreDispatchesImportModel> entity)
        {
            return Validate(entity);
        }
        public void SaveImportPreDispatch(List<PreDispatchesImportModel> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    List<DispatchDetail> dispatchdetail;

                    var _detail = entity.GroupBy(g => new
                    {
                        index = g.DispatchCode,
                        DispatchCode = g.DispatchCode,
                        CustomerCode = g.CustomerCode,
                        DispatchType = g.DispatchType,
                        Dispatch_Type_ID = g.Dispatch_Type_ID,
                        Dispatch_Type_Code = g.Dispatch_Type_Code,
                        EstDispatchDate = g.EstDispatchDate,
                        PONumber = g.PONumber,
                        OrderNumber = g.OrderNumber,
                        IsBackOrder = g.IsBackOrder,
                        IsUrgent = g.IsUrgent,
                        ProductCode = g.ProductCode,
                        UOM = g.UOM,
                        Remark = g.Remark,
                        SubStatus = g.SubStatus,
                        Price = g.Price,
                        UnitPrice = g.UnitPrice,
                        Remark2 = g.Remark2,
                        DocumentDate = g.DocumentDate,
                        ShippingTo = g.ShippingTo,
                        DeliveryDate = g.DeliveryDate,

                    }).Select(n => new
                    {
                        Index = n.Key.index,
                        DispatchCode = n.Key.DispatchCode,
                        CustomerCode = n.Key.CustomerCode,
                        DispatchType = n.Key.DispatchType,
                        Dispatch_Type_ID = n.Key.Dispatch_Type_ID,
                        Dispatch_Type_Code = n.Key.Dispatch_Type_Code,
                        EstDispatchDate = n.Key.EstDispatchDate,
                        PONumber = n.Key.PONumber,
                        OrderNumber = n.Key.OrderNumber,
                        IsBackOrder = n.Key.IsBackOrder,
                        IsUrgent = n.Key.IsUrgent,
                        ProductCode = n.Key.ProductCode,
                        Quantity = n.Sum(x => x.Quantity),
                        UOM = n.Key.UOM,
                        Remark = n.Key.Remark,
                        SubStatus = n.Key.SubStatus,
                        Price = n.Key.Price,
                        UnitPrice = n.Key.UnitPrice,
                        Remark2 = n.Key.Remark2,
                        DocumentDate = n.Key.DocumentDate,
                        ShippingTo = n.Key.ShippingTo,
                        DeliveryDate = n.Key.DeliveryDate,
                    }).ToList();


                    var _header = _detail.GroupBy(g => new
                    {
                        index = g.DispatchCode,
                        CustomerCode = g.CustomerCode,
                        DispatchTypeCode = g.Dispatch_Type_Code,
                        EstDispatchDate = g.EstDispatchDate,
                        DocumentDate = g.DocumentDate,
                        ShippingTo = g.ShippingTo,
                        DeliveryDate = g.DeliveryDate,
                        PONumber = g.PONumber,
                        OrderNumber = g.OrderNumber,
                        IsUrgent = g.IsUrgent,
                        IsBackOrder = g.IsBackOrder,
                        Remark = g.Remark

                    }).Select(n => new
                    {
                        Index = n.Key.index,
                        CustomerCode = n.Key.CustomerCode,
                        DispatchTypeCode = n.Key.DispatchTypeCode,
                        EstDispatchDate = n.Key.EstDispatchDate,
                        DocumentDate = n.Key.DocumentDate,
                        ShippingTo = n.Key.ShippingTo,
                        DeliveryDate = n.Key.DeliveryDate,
                        PONumber = n.Key.PONumber,
                        OrderNumber = n.Key.OrderNumber,
                        IsUrgent = n.Key.IsUrgent,
                        IsBackOrder = n.Key.IsBackOrder,
                        Remark = n.Key.Remark
                    }).ToList();


                    string dispatchCode = string.Empty;
                    string customer_Code = string.Empty;
                    string dock = string.Empty;
                    string route = string.Empty;
                    string poNumber = string.Empty;
                    string dispatchRefered = string.Empty;
                    string remark = string.Empty;

                    Guid? dispatch_Type_ID = null;
                    bool? isBackOrder = null;
                    bool? isUrgent = null;

                    int sequence = 1;

                    foreach (var _item in _header)
                    {
                        if (string.IsNullOrEmpty(_item.IsBackOrder))
                        {
                            isBackOrder = false;
                        }
                        else
                        {
                            isBackOrder = (_item.IsBackOrder == "Y" ? true : false);
                        }

                        if (string.IsNullOrEmpty(_item.IsUrgent))
                        {
                            isUrgent = false;
                        }
                        else
                        {
                            isUrgent = (_item.IsUrgent == "Y" ? true : false);
                        }

                        dispatch_Type_ID = DocumentTypeService.FirstOrDefault(x => x.Code == _item.DispatchTypeCode).DocumentTypeID;
                        //var reson = this.reasonObj.FirstOrDefault(x => x.IsDefault != null && x.IsDefault == true).Reason_Code;
                        ShippingTo shipto = ShiptoService.FirstOrDefault(x => x.Name == _item.ShippingTo);

                        Contact contact = ContactService.FirstOrDefault(x => x.Code == _item.CustomerCode);

                        Dispatch dispatch = new Dispatch()
                        {
                            DispatchId = Guid.NewGuid(),
                            DispatchCode = _item.Index,
                            DocumentId = dispatch_Type_ID.Value,
                            OrderDate = _item.EstDispatchDate.Value,
                            DeliveryDate = null,//_item.DeliveryDate.Value,
                            DocumentDate = DateTime.Now,
                            ShipptoId = shipto?.ShipToId ?? Guid.Empty,
                            Pono = _item.PONumber,
                            OrderNo = _item.OrderNumber,
                            IsBackOrder = isBackOrder,
                            IsUrgent = isUrgent.Value,
                            DispatchStatus = DispatchStatusEnum.New,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            UserCreated = UserID,
                            UserModified = UserID,
                            CustomerId = contact.ContactID
                        };

                        Guid _ruleid = shipto.RuleId;

                        // var _productstatusid = ProductStatusService.GetByDocuemtnType(dispatch_Type_ID.Value);
                        ProductStatusMapDocument _productstatusdata = ProductStatusMapDocService.FirstOrDefault(x => x.DocumentTypeID == dispatch_Type_ID.Value);
                        Guid _productstatusid = Guid.Empty;
                        if (_productstatusdata != null)
                        {
                            _productstatusid = _productstatusdata.ProductStatusID;
                        }

                        Guid _productownerid = ProductOwnerService.FirstOrDefault(x => x.IsActive).ProductOwnerID;



                        DispatchPrefix prefix = DispatchPrefixService.SingleOrDefault(x => x.PrefixType == DispatchPreFixTypeEnum.DISPATHCODE);
                        if (prefix == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        IRepository<DispatchPrefix> _dispatchprefix = Context.Repository<DispatchPrefix>();
                        DispatchPrefix updatdispatchprefix = _dispatchprefix.FindByID(prefix.PrefixId);

                        string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                        dispatch.DispatchCode = DispatchCode;
                        updatdispatchprefix.LastedKey = DispatchCode;
                        DispatchPrefixService.Modify(updatdispatchprefix);


                        dispatchdetail = new List<DispatchDetail>();

                        _detail.Where(x => x.DispatchCode == _item.Index &&
                                            x.CustomerCode == _item.CustomerCode &&
                                            x.Dispatch_Type_Code == _item.DispatchTypeCode &&
                                            x.EstDispatchDate == _item.EstDispatchDate &&
                                            x.DocumentDate == _item.DocumentDate &&
                                            x.ShippingTo == _item.ShippingTo &&
                                            x.DeliveryDate == _item.DeliveryDate &&
                                            x.PONumber == _item.PONumber &&
                                            x.OrderNumber == _item.OrderNumber &&
                                            x.IsBackOrder == _item.IsBackOrder &&
                                            x.IsUrgent == _item.IsUrgent &&
                                            x.Remark == _item.Remark
                                        ).ToList().ForEach(_itemDispatch =>
                                        {


                                            ProductCodes product_code = ProductCodeService.FirstOrDefault(x => x.IsActive && x.Code == _itemDispatch.ProductCode);//.Get().FirstOrDefault();//this.getProductsByProductCode(_item.CustomerCode, _itemDispatch.ProductCode);
                                            Product product = ProductService.FirstOrDefault(x => x.IsActive && x.ProductID == product_code.ProductID);//.Get().FirstOrDefault();//this.getProductsByProductCode(_item.CustomerCode, _itemDispatch.ProductCode);
                                            if (product == null)
                                            {
                                                throw new Exception("Not found Product ");
                                            }

                                            ProductUnit uom = ProductUnitService.FirstOrDefault(x => x.IsActive && x.ProductID == product.ProductID && x.Name == _itemDispatch.UOM);//.Get().FirstOrDefault();
                                            if (uom == null)
                                            {
                                                throw new Exception("Not found Product Unit Price");
                                            }

                                            ProductUnit uomPrice = ProductUnitService.FirstOrDefault(x => x.IsActive && x.ProductID == product.ProductID && x.Name == _itemDispatch.UnitPrice);//.Get().FirstOrDefault();
                                            //if (uomPrice == null)
                                            //    throw new Exception("Not found Product Unit Price");


                                            ProductUnit sku = ProductUnitService.FirstOrDefault(x => x.IsActive && x.ProductID == product.ProductID && x.IsBaseUOM == true);//.Get().FirstOrDefault();
                                            if (sku == null)
                                            {
                                                throw new Exception("Not found Product UOM SKU.");
                                            }

                                            Guid? _product_PriceUnitId = null;
                                            if (uomPrice != null)
                                            {
                                                _product_PriceUnitId = uomPrice.ProductUnitID;
                                            }
                                            else
                                            {
                                                _product_PriceUnitId = Guid.Empty; //uom.ProductUnitID;
                                            }

                                            decimal? _Price = null;
                                            if (_itemDispatch.Price != null)
                                            {
                                                _Price = decimal.Parse(_itemDispatch.Price.ToString());
                                            } 
                                            DispatchDetail detail = new DispatchDetail()
                                            {
                                                DispatchId = dispatch.DispatchId,
                                                ProductId = product.ProductID,
                                                StockUnitId = uom.ProductUnitID,
                                                DispatchDetailProductHeight = decimal.Parse(uom.Height.ToString()),
                                                DispatchDetailProductLength = decimal.Parse(uom.Length.ToString()),
                                                DispatchDetailProductWidth = decimal.Parse(uom.Width.ToString()),
                                                Sequence = sequence,
                                                Quantity = decimal.Parse(_itemDispatch.Quantity.ToString()),
                                                DispatchDetailStatus = DispatchDetailStatusEnum.New,
                                                DispatchPrice = _Price,
                                                DispatchPriceUnitId = _product_PriceUnitId,//uomPrice.ProductUnitID,
                                                BaseUnitId = sku.ProductUnitID,
                                                BaseQuantity = decimal.Parse(_itemDispatch.Quantity.ToString()) * uom.Quantity,
                                                ConversionQty = uom.Quantity,
                                                IsActive = true,
                                                DateCreated = DateTime.Now,
                                                DateModified = DateTime.Now,
                                                UserCreated = UserID,
                                                UserModified = UserID,
                                                RuleId = _ruleid,
                                                ProductStatusId = _productstatusid,
                                                ProductOwnerId = _productownerid
                                            };
                                            dispatchdetail.Add(detail);
                                            sequence = sequence + 1;
                                        });
                        dispatch.DispatchDetailCollection = dispatchdetail;




                        base.Add(dispatch);
                        sequence = 1;
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
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<ObjectPropertyValidatorException> Validate(List<PreDispatchesImportModel> entities)
        {
            List<ObjectPropertyValidatorException> Errors = new List<ObjectPropertyValidatorException>();

            try
            {
                int _rowIndex = 0;

                foreach (PreDispatchesImportModel _item in entities)
                {
                    //Contact
                    int _contact_col = 1;
                    if (!string.IsNullOrWhiteSpace(_item.CustomerCode))
                    {
                        Contact _contact = ContactService.FirstOrDefault(x => x.Code == _item.CustomerCode);//.Get().FirstOrDefault();
                        if (_contact == null)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _contact_col, "Not found customer.", "CustomerCode"));
                        }
                        //else {
                        //    var _contacttype = ContactInTypeService.Query().Filter(x => x.ContactID == _contact.ContactID && x.ContactType == ContactType.Customer).Get().FirstOrDefault();
                        //    if (_contacttype == null)
                        //        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _contact_col, "Not found customer.", "CustomerCode"));
                        //}
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _contact_col, "Customer is empty.", "CustomerCode"));
                    }
                    //##


                    //Dispatch Type
                    int _dispatchtype_col = 2;
                    if (!string.IsNullOrEmpty(_item.Dispatch_Type_Code))
                    {
                        DocumentType _dispatchtype = DocumentTypeService.FirstOrDefault(x => x.Code == _item.Dispatch_Type_Code && x.DocType == DocumentTypeEnum.Dispatch);//.Get().FirstOrDefault();
                        if (_dispatchtype == null)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _dispatchtype_col, "Not found dispatch type.", "DispatchType"));
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _dispatchtype_col, "Dispatch Type is empty.", "DispatchType"));
                    }
                    //##

                    //Est Dispatch Date
                    int _estdispatchdate_col = 3;
                    if (_item.EstDispatchDate != null && _item.EstDispatchDate != DateTime.MinValue)
                    {

                        bool result = DateTime.TryParse(_item.EstDispatchDate.ToString(), out DateTime estDate);
                        if (!result)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _estdispatchdate_col, "Est.Dispatch Date format invalid.", "EstDispatchDate"));
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _estdispatchdate_col, "Est.Dispatch Date is empty.", "EstDispatchDate"));
                    }
                    //##

                    //Shipto
                    int _shopto_col = 5;
                    if (!string.IsNullOrWhiteSpace(_item.CustomerCode))
                    {
                        ShippingTo _result_shipto = ShiptoService.FirstOrDefault(a => a.Name == _item.ShippingTo);//.Get().FirstOrDefault();
                        if (_result_shipto == null)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _shopto_col, "Not found ship to.", "ShipTo"));
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _shopto_col, "Ship to is empty.", "ShipTo"));
                    }
                    //##

                    //PO
                    int _pono_col = 7;
                    if (string.IsNullOrEmpty(_item.PONumber))
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _pono_col, "PO Number is empty.", "PONumber"));
                    }
                    //##

                    //Order No
                    //int _orderno_col = 8;
                    //if (string.IsNullOrEmpty(_item.OrderNumber))

                    //    Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _orderno_col, "Order Number is empty.", "OrderNumber"));
                    //##


                    //BackOrder
                    string[] backorder = new string[] { "Y", "N" };
                    int _backorder_col = 9;
                    if (!string.IsNullOrWhiteSpace(_item.IsBackOrder))
                    {
                        bool _ok = backorder.Contains(_item.IsBackOrder);
                        if (!_ok)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _backorder_col, "Invalid back order formart.[Y/N]", "Back Order"));
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _backorder_col, "Invalid back order formart.[Y/N]", "Back Order"));
                    }
                    //##


                    //Urgent
                    string[] urgent = new string[] { "Y", "N" };
                    int _urgent_col = 11;
                    if (!string.IsNullOrWhiteSpace(_item.IsUrgent))
                    {
                        bool _ok = urgent.Contains(_item.IsUrgent);
                        if (!_ok)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _urgent_col, "Invalid urgent formart.[Y/N]", "Urgent"));
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _urgent_col, "Invalid urgent formart.[Y/N]", "Urgent"));
                    }
                    //##


                    //Product
                    int _productcode_col = 12;
                    if (string.IsNullOrWhiteSpace(_item.ProductCode))
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _productcode_col, "Product Code is empty.", "ProductCode"));
                    }
                    else
                    {
                        ProductCodes result_productcode = ProductCodeService.FirstOrDefault(x => x.Code == _item.ProductCode);//.Get().FirstOrDefault();
                        if (result_productcode == null)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _productcode_col, "Not found product.", "ProductCode"));
                        }
                        else
                        {
                            result_productcode = ProductCodeService.FirstOrDefault(x => x.Code == _item.ProductCode && x.IsActive);//.Get().FirstOrDefault();
                            if (result_productcode != null)
                            {
                                Product result_product = ProductService.FirstOrDefault(a => a.ProductID.Equals(result_productcode.ProductID) && a.IsActive);
                                if (result_product == null)
                                {
                                    Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _productcode_col, "Not found product.", "ProductCode"));
                                }
                            }
                        }
                    }
                    //##

                    //Quantity
                    int _quantity_col = 13;
                    if (_item.Quantity == null || _item.Quantity == 0)
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _quantity_col, "Quantity not is null.", "Quantity"));
                    }
                    //##


                    //Product UOM
                    int _uom_col = 14;
                    if (!string.IsNullOrEmpty(_item.UOM.Trim()))
                    {
                        ProductCodes _product_System_Code = ProductCodeService.FirstOrDefault(x => x.Code == _item.ProductCode);//.Get().FirstOrDefault();
                        if (_product_System_Code == null)
                        {
                            Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _uom_col, "Not found Product Code.", "UOM"));
                        }
                        else
                        {
                            ProductUnit _productunit = ProductUnitService.FirstOrDefault(a => a.Name == _item.UOM && a.ProductID == _product_System_Code.ProductID);//.Get().FirstOrDefault();
                            if (_productunit == null)
                            {
                                Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _uom_col, "Not found product UOM.", "UOM"));
                            }
                        }
                    }
                    else
                    {
                        Errors.Add(new ObjectPropertyValidatorException(_rowIndex, _uom_col, "UOM is empty.", "UOM"));
                    }

                    //##



                    _rowIndex += 1;
                }




                return Errors;


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Customer validation. {0}", ex.Message.ToString()));
            }
        }
        #endregion

        #region Dispatch

        public List<POListModels> GetPOList(Guid documentTypeID, int? dispatchStatus, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                Guid? dispatchDocTypeID = ItfInterfaceMappingService.FirstOrDefault(x => x.IsActive && x.ReferenceDocumentID == documentTypeID)?.DocumentId;

                IQueryable<POListModels> poList = (from dp in Where(x => x.IsActive)
                                                   join itm in ItfInterfaceMappingService.Where(x => x.IsActive) on dp.DocumentId equals itm.DocumentId
                                                   where (dispatchStatus.HasValue ? dp.DispatchStatus == (DispatchStatusEnum)dispatchStatus : true)
                                                             && (dispatchDocTypeID.HasValue ? itm.DocumentId == dispatchDocTypeID.Value : true)
                                                   select new POListModels()
                                                   {
                                                       PONo = dp.Pono,
                                                   });

                if (poList == null || poList.Count() == 0)
                {
                    return new List<POListModels>();
                }

                if (keyword != null && !string.IsNullOrWhiteSpace(keyword))
                {
                    poList = poList.Where(x => x.PONo.Contains(keyword));
                }

                totalRecords = poList.Count();
                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    poList = poList.OrderByDescending(x => x.PONo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault());
                }

                return poList.ToList();

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

        public List<DispatchModels> Get(Guid? documenttypeid, DateTime? deliverlydate, int? status, string pono, string orderno, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            { 
                //AddInterfaceDispatch(result_test);
                ////#####
                var result = (from _dispatch in Where(x => x.IsActive) 
                              join _dispatchtype in DocumentTypeService.Where(x => x.IsActive) on _dispatch.DocumentId equals _dispatchtype.DocumentTypeID
                              join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                              where (documenttypeid != null ? _dispatch.DocumentId == documenttypeid : true)
                              && (deliverlydate.HasValue ? _dispatch.OrderDate == deliverlydate : true)
                              && (status.HasValue && status.Value>0? 
                              (_dispatch.IsBackOrder == false &&  _dispatch.DispatchStatus == DispatchStatusEnum.InBackOrder ?DispatchStatusEnum.InprogressConfirm : _dispatch.DispatchStatus) == (DispatchStatusEnum)status : true)
                              && (!string.IsNullOrEmpty(pono) ? _dispatch.Pono.Contains(pono) : true)
                              && (!string.IsNullOrEmpty(orderno) ? _dispatch.OrderNo.Contains(orderno) : true)
                              select new
                              {
                                  DispatchId = _dispatch.DispatchId,
                                  DispatchCode = _dispatch.DispatchCode,
                                  IsUrgent = _dispatch.IsUrgent,
                                  IsBackOrder = _dispatch.IsBackOrder,
                                  OrderNo = _dispatch.OrderNo,
                                  OrderDate = _dispatch.OrderDate,
                                  Pono = _dispatch.Pono,
                                  DocumentId = _dispatch.DocumentId,
                                  Name = _dispatchtype.Name,
                                  DocumentApproveDate = _dispatch.DocumentApproveDate,
                                  DispatchStatus = _dispatch.DispatchStatus,
                                  DeliveryDate = _dispatch.DeliveryDate,
                                  Quantity = _dispatchdetail.Quantity
                              } into x
                              group x by new
                              {
                                  x.DispatchId,
                                  x.DispatchCode,
                                  x.IsUrgent,
                                  x.IsBackOrder,
                                  x.OrderNo,
                                  x.OrderDate,
                                  x.Pono,
                                  x.DocumentId,
                                  x.Name,
                                  x.DocumentApproveDate,
                                  x.DeliveryDate,
                                  x.DispatchStatus
                              } into g
                              select new
                              {
                                  DispatchID = g.Key.DispatchId,
                                  DispatchCode = g.Key.DispatchCode,
                                  IsUrgent = g.Key.IsUrgent,
                                  IsBackOrder = g.Key.IsBackOrder,
                                  OrderNo = g.Key.OrderNo,
                                  OrderDate = g.Key.OrderDate,
                                  Pono = g.Key.Pono,
                                  DocumentId = g.Key.DocumentId,
                                  DocumentTypeName = g.Key.Name,
                                  DocumentApproveDate = g.Key.DocumentApproveDate,
                                  DeliveryDate = g.Key.DeliveryDate,
                                  DispatchStatus = g.Key.DispatchStatus,
                                  TotalDispatchQty = g.Sum(i => i.Quantity),
                              });

                totalRecords = result.Count();

                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    result = result.OrderByDescending(x => x.IsUrgent).ThenByDescending(y => y.DispatchCode)
                        .Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault())
                        .Take(pageSize.GetValueOrDefault());
                }

                var result2 = result.ToList();
                List<DispatchModels> resultsmodel = result2.Select(n => new DispatchModels
                {
                    DispatchId = n.DispatchID,
                    DispatchCode = n.DispatchCode,
                    IsUrgent = n.IsUrgent.Value,
                    IsBackOrder = n.IsBackOrder.GetValueOrDefault(),
                    Pono = n.Pono,
                    OrderNo = n.OrderNo,
                    DocumentId = n.DocumentId,
                    DocumentName = n.DocumentTypeName,
                    OrderDate = n.OrderDate,
                    DocumentApproveDate = n.DocumentApproveDate,
                    DeliveryDate = n.DeliveryDate,
                    TotalDispatchQty = n.TotalDispatchQty.GetValueOrDefault(),
                    DispatchStatusId = (int?)n.DispatchStatus,
                    DispatchStatusName = (!n.IsBackOrder.HasValue && n.DispatchStatus == DispatchStatusEnum.InBackOrder ?
                    GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), DispatchStatusEnum.InprogressConfirm.ToString())) 
                    : GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n.DispatchStatus.ToString())))
                }).ToList(); 
                return resultsmodel;
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

        public DispatchModels GetById(Guid id)
        {
            try
            {

                Dispatch _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var hd = (from _dispatch in Where(x => x.IsActive && x.DispatchId == id)//.Get()
                          join _document in DocumentTypeService.Where(x => x.IsActive) on _dispatch.DocumentId equals _document.DocumentTypeID
                          join _shipto in ShiptoService.Where(x => x.IsActive) on _dispatch.ShipptoId equals _shipto.ShipToId
                          join _a in WarehouseService.Where(x => x.IsActive) on _dispatch.FromwarehouseId equals _a.WarehouseID into a
                          from _warehouse in a.DefaultIfEmpty()
                          join _x in WarehouseService.Where(x => x.IsActive) on _dispatch.TowarehouseId equals _x.WarehouseID into x
                          from _warehouse2 in x.DefaultIfEmpty()
                          join _customer in ContactService.Where(x => x.IsActive) on _dispatch.CustomerId equals _customer.ContactID
                          join _marketting in ItfInterfaceMappingService.Where(x => x.IsActive) on _dispatch.DocumentId equals _marketting.DocumentId
                          select new { _dispatch, _document, _shipto, _warehouse, _warehouse2, _customer, _marketting }
                           ).FirstOrDefault();
              DispatchModels resultsmodel = new DispatchModels
                {
                    DispatchId = hd._dispatch.DispatchId,
                    DispatchCode = hd._dispatch.DispatchCode,
                    IsUrgent = hd._dispatch.IsUrgent.Value,
                    IsBackOrder = hd._dispatch.IsBackOrder.GetValueOrDefault(),
                    Pono = hd._dispatch.Pono,
                    OrderNo = hd._dispatch.OrderNo,
                    DocumentId = hd._dispatch.DocumentId,
                    DocumentName = hd._document.Name,
                    OrderDate = hd._dispatch.OrderDate,
                    DocumentApproveDate = hd._dispatch.DocumentApproveDate,
                    DocumentDate = hd._dispatch.DocumentDate,
                    DeliveryDate = hd._dispatch.DeliveryDate,
                    DispatchStatusId = (int?)hd._dispatch.DispatchStatus,
                    DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), 
                    hd._dispatch.DispatchStatus.ToString())),
                    FromwarehouseId = (hd._warehouse != null ? hd._dispatch.FromwarehouseId : null),
                    FromwarehouseName = (hd._warehouse != null ? hd._warehouse.Name : null),
                    TowarehouseId = (hd._warehouse2 != null ? hd._dispatch.TowarehouseId : null),
                    TowarehouseName = (hd._warehouse2 != null ? hd._warehouse2.Name : null),
                    CustomerId = hd._dispatch.CustomerId.GetValueOrDefault(),
                    CustomerName = hd._customer.Name,
                    CustomerCode = hd._customer.Code,
                    ShipptoId = hd._dispatch.ShipptoId,
                    ShipptoName = hd._shipto.Name,
                    Remark = hd._dispatch.Remark,
                    IsActive = hd._dispatch.IsActive,
                    IsMarketing = (hd._marketting.IsMarketing.HasValue ? hd._marketting.IsMarketing.Value : false),
                    Reason = hd._dispatch.ReviseReason,
                    ReferenceId = hd._dispatch.ReferenceId
                };
                var _dt = (from _dispatchdetail in DispatchDetailService.Where(x => x.IsActive && x.DispatchId == id)
                           join _proudct in ProductService.Where(x => x.IsActive) on _dispatchdetail.ProductId equals _proudct.ProductID
                           join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                           join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.StockUnitId equals _unit.ProductUnitID
                           join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                           from _unit2 in a.DefaultIfEmpty()
                           join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                           join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                           from _status in s.DefaultIfEmpty()
                           join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                           from _sub_status in ss.DefaultIfEmpty()
                           select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status }
                 ).ToList();

                List<DispatchDetailModels> resultsdetailmodel = _dt.Select(n => new DispatchDetailModels
                {
                    DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                    DispatchId = n._dispatchdetail.DispatchId,
                    Sequence = n._dispatchdetail.Sequence,
                    ProductId = n._dispatchdetail.ProductId,
                    ProductName = n._proudct.Name,
                    ProductCode = n._productcode.Code,
                    StockUnitId = n._dispatchdetail.StockUnitId,
                    StockUnitName = n._unit.Name,
                    Quantity = n._dispatchdetail.Quantity,
                    BaseQuantity = n._dispatchdetail.BaseQuantity,
                    ConversionQty = n._dispatchdetail.ConversionQty,
                    ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                    DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                    DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                    DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                    BaseUnitId = n._dispatchdetail.BaseUnitId,
                    BaseUnitName = n._unit.Name,
                    DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                    DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                    DispatchPrice = n._dispatchdetail.DispatchPrice,
                    Remark = n._dispatchdetail.Remark,
                    IsActive = n._dispatchdetail.IsActive,
                    DispatchDetailStatusId =(int) n._dispatchdetail.DispatchDetailStatus,
                    DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                    RuleId = n._rule.RuleId,
                    RuleName = n._rule.RuleName,
                    ProductStatusId = n._status.ProductStatusID,
                    ProductStatusName = n._status.Name,
                    ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                    ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),

                }).ToList();

                resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;
                resultsmodel.TypeTotal = 1;
                return resultsmodel;

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

        public DispatchModels GetBookingById(Guid id)
        {
            try
            {

                Dispatch _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var _hd = (from _dispatch in Where(x => x.IsActive && x.DispatchId == id)
                           join _document in DocumentTypeService.Where(x => x.IsActive) on _dispatch.DocumentId equals _document.DocumentTypeID
                           join _shipto in ShiptoService.Where(x => x.IsActive) on _dispatch.ShipptoId equals _shipto.ShipToId
                           join _a in WarehouseService.Where(x => x.IsActive) on _dispatch.FromwarehouseId equals _a.WarehouseID into a
                           from _warehouse in a.DefaultIfEmpty()
                           join _x in WarehouseService.Where(x => x.IsActive) on _dispatch.TowarehouseId equals _x.WarehouseID into x
                           from _warehouse2 in x.DefaultIfEmpty()
                           join _customer in ContactService.Where(x => x.IsActive) on _dispatch.CustomerId equals _customer.ContactID
                           join _marketting in ItfInterfaceMappingService.Where(x => x.IsActive) on _dispatch.DocumentId equals _marketting.DocumentId
                           select new { _dispatch, _document, _shipto, _warehouse, _warehouse2, _customer, _marketting }
                           ).FirstOrDefault();

                DispatchModels resultsmodel = new DispatchModels
                {
                    DispatchId = _hd._dispatch.DispatchId,
                    DispatchCode = _hd._dispatch.DispatchCode,
                    IsUrgent = _hd._dispatch.IsUrgent.Value,
                    IsBackOrder = _hd._dispatch.IsBackOrder.Value,
                    Pono = _hd._dispatch.Pono,
                    OrderNo = _hd._dispatch.OrderNo,
                    DocumentId = _hd._dispatch.DocumentId,
                    DocumentName = _hd._document.Name,
                    OrderDate = _hd._dispatch.OrderDate,
                    DocumentApproveDate = _hd._dispatch.DocumentApproveDate,
                    DocumentDate = _hd._dispatch.DocumentDate,
                    DeliveryDate = _hd._dispatch.DeliveryDate,
                    DispatchStatusId =(int?) _hd._dispatch.DispatchStatus,
                    DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), _hd._dispatch.DispatchStatus.ToString())),
                    FromwarehouseId = (_hd._warehouse != null ? _hd._dispatch.FromwarehouseId : null),
                    FromwarehouseName = (_hd._warehouse != null ? _hd._warehouse.Name : null),
                    TowarehouseId = (_hd._warehouse2 != null ? _hd._dispatch.TowarehouseId : null),
                    TowarehouseName = (_hd._warehouse2 != null ? _hd._warehouse2.Name : null),
                    CustomerId = _hd._dispatch.CustomerId.Value,
                    CustomerName = _hd._customer.Name,
                    CustomerCode = _hd._customer.Code,
                    ShipptoId = _hd._dispatch.ShipptoId,
                    ShipptoName = _hd._shipto.Name,
                    Remark = _hd._dispatch.Remark,
                    IsActive = _hd._dispatch.IsActive,
                    IsMarketing = (_hd._marketting.IsMarketing != null ? _hd._marketting.IsMarketing.Value : false),
                    Reason = _hd._dispatch.ReviseReason,
                    ReferenceId = _hd._dispatch.ReferenceId
                };

                var _dt = (from _dispatchbooking in DispatchBookingService.Where(x => x.IsActive)
                           join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive && x.DispatchId == id) on _dispatchbooking.DispatchDetailId equals _dispatchdetail.DispatchDetailId
                           join _proudct in ProductService.Where(x => x.IsActive) on _dispatchbooking.ProductId equals _proudct.ProductID
                           join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                           join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchbooking.RequestStockUnitId equals _unit.ProductUnitID
                           join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                           from _unit2 in a.DefaultIfEmpty()
                           join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                           join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                           from _status in s.DefaultIfEmpty()
                           join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                           from _sub_status in ss.DefaultIfEmpty()
                           join _l in LocationService.Where(x => x.IsActive) on _dispatchbooking.LocationId equals _l.LocationID into l
                           from _location in l.DefaultIfEmpty()
                           join _p in ProductionControlDetailService.Where(x => x.IsActive) on _dispatchbooking.PalletCode equals _p.PalletCode into p
                           from _pallet in p.DefaultIfEmpty()
                           join _pc in ProductionControlService.Where(x => x.IsActive) on _pallet.ControlID equals _pc.ControlID into gpc
                           from _product in gpc.DefaultIfEmpty()
                           select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status, _dispatchbooking, _location, _pallet, _product }
                 ).ToList();

                List<DispatchDetailModels> resultsdetailmodel = new List<DispatchDetailModels>(); 
                resultsdetailmodel = _dt.Select(n => new DispatchDetailModels
                {
                    DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                    DispatchId = n._dispatchdetail.DispatchId,
                    Sequence = n._dispatchdetail.Sequence,
                    ProductId = n._dispatchdetail.ProductId,
                    ProductName = n._proudct.Name,
                    ProductCode = n._productcode.Code,
                    StockUnitId = n._dispatchdetail.StockUnitId,
                    StockUnitName = n._unit.Name,
                    Quantity = n._dispatchbooking.BookingQty>0? n._dispatchbooking.BookingQty : n._dispatchbooking.RequestQty,
                    ConversionQty = n._dispatchbooking.ConversionQty,
                    BaseQuantity =n._dispatchbooking.BookingQty > 0 ? n._dispatchbooking.BookingBaseQty : n._dispatchbooking.BookingBaseQty,
                    ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                    DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                    DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                    DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                    BaseUnitId = n._dispatchdetail.BaseUnitId,
                    BaseUnitName = n._unit.Name,
                    DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                    DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                    DispatchPrice = n._dispatchdetail.DispatchPrice,
                    Remark = n._dispatchdetail.Remark,
                    IsActive = n._dispatchdetail.IsActive,
                    DispatchDetailStatusId = (int)n._dispatchdetail.DispatchDetailStatus,
                    DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                    RuleId = n._rule.RuleId,
                    RuleName = n._rule.RuleName,
                    ProductStatusId = n._status.ProductStatusID,
                    ProductStatusName = n._status.Name,
                    ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                    ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),
                    ProductLot = n._dispatchbooking.ProductLot,
                    LocationCode = (n._location?.Code),
                    PalletCode = n._dispatchbooking.PalletCode,
                    IsBackOrder = n._dispatchbooking.IsBackOrder,
                    BookingId = n._dispatchbooking.BookingId,
                    OrderNo = n._product == null ? "" : n._product.OrderNo
                }).ToList();
                resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;
                resultsmodel.TypeTotal = 2;
                return resultsmodel;
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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public DispatchModels GetConsolidateById(Guid id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@DispatchId", SqlDbType.UniqueIdentifier) { Value = id };

                DispatchModels resultsmodel = Context.SQLQuery<DispatchModels>("exec SP_GetPackingByDispatchId @DispatchId ", param).FirstOrDefault();


                if (resultsmodel == null)
                {
                    throw new HILIException("MSG00047");
                }

                resultsmodel.DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), resultsmodel.DispatchStatusId.ToString()));



                SqlParameter param1 = new SqlParameter("@DispatchId", SqlDbType.UniqueIdentifier) { Value = id };


                List<DispatchDetailModels> result_detail = Context.SQLQuery<DispatchDetailModels>("exec SP_GetConsolidateDetailByDispatchId @DispatchId", param1).ToList();

                result_detail.ForEach(item =>
                {
                    item.DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), item.DispatchDetailStatusId.ToString()));
                });
                resultsmodel.DispatchDetailModelsCollection = result_detail;

                resultsmodel.TypeTotal = 3;
                return resultsmodel; 
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

        public DispatchModels GetDispatchCompleteById(Guid id)
        {
            try
            {

                Dispatch _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var _hd = (from _dispatch in Where(x => x.IsActive && x.DispatchId == id)
                           join _document in DocumentTypeService.Where(x => x.IsActive) on _dispatch.DocumentId equals _document.DocumentTypeID
                           join _shipto in ShiptoService.Where(x => x.IsActive) on _dispatch.ShipptoId equals _shipto.ShipToId
                           join _a in WarehouseService.Where(x => x.IsActive) on _dispatch.FromwarehouseId equals _a.WarehouseID into a
                           from _warehouse in a.DefaultIfEmpty()
                           join _x in WarehouseService.Where(x => x.IsActive) on _dispatch.TowarehouseId equals _x.WarehouseID into x
                           from _warehouse2 in x.DefaultIfEmpty()
                           join _customer in ContactService.Where(x => x.IsActive) on _dispatch.CustomerId equals _customer.ContactID
                           join _marketting in ItfInterfaceMappingService.Where(x => x.IsActive) on _dispatch.DocumentId equals _marketting.DocumentId
                           select new { _dispatch, _document, _shipto, _warehouse, _warehouse2, _customer, _marketting }
                           ).ToList();

                DispatchModels _hdmodel = new DispatchModels();


                DispatchModels resultsmodel = _hd.Select(n => new DispatchModels
                {
                    DispatchId = n._dispatch.DispatchId,
                    DispatchCode = n._dispatch.DispatchCode,
                    IsUrgent = n._dispatch.IsUrgent.Value,
                    IsBackOrder = n._dispatch.IsBackOrder.Value,
                    Pono = n._dispatch.Pono,
                    OrderNo = n._dispatch.OrderNo,
                    DocumentId = n._dispatch.DocumentId,
                    DocumentName = n._document.Name,
                    OrderDate = n._dispatch.OrderDate,
                    DocumentApproveDate = n._dispatch.DocumentApproveDate,
                    DocumentDate = n._dispatch.DocumentDate,
                    DeliveryDate = n._dispatch.DeliveryDate,
                    DispatchStatusId = (int?)n._dispatch.DispatchStatus,
                    DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatch.DispatchStatus.ToString())),
                    FromwarehouseId = (n._warehouse != null ? n._dispatch.FromwarehouseId : null),
                    FromwarehouseName = (n._warehouse != null ? n._warehouse.Name : null),
                    TowarehouseId = (n._warehouse2 != null ? n._dispatch.TowarehouseId : null),
                    TowarehouseName = (n._warehouse2 != null ? n._warehouse2.Name : null),
                    CustomerId = n._dispatch.CustomerId.Value,
                    CustomerName = n._customer.Name,
                    CustomerCode = n._customer.Code,
                    ShipptoId = n._dispatch.ShipptoId,
                    ShipptoName = n._shipto.Name,
                    Remark = n._dispatch.Remark,
                    IsActive = n._dispatch.IsActive,
                    IsMarketing = (n._marketting.IsMarketing != null && n._marketting.IsMarketing.Value),
                    IsAssign = (n._marketting.IsAssign.HasValue && n._marketting.IsAssign.Value),
                    Reason = n._dispatch.ReviseReason,
                    ReferenceId = n._dispatch.ReferenceId
                }).FirstOrDefault();

                var _dt = (from _dispatchbooking in DispatchBookingService.Where(x => x.IsActive)
                           join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive && x.DispatchId == id) on _dispatchbooking.DispatchDetailId equals _dispatchdetail.DispatchDetailId
                           join _tuckdetail in RegisterTruckDetailService.Where(x => x.IsActive) on _dispatchbooking.BookingId equals _tuckdetail.BookingID
                           join _pickassign in PickingAssignService.Where(x => x.IsActive) on _tuckdetail.ShippingDetailID equals _pickassign.ShippingDetailID
                           join _pickdetail in PickingDetailService.Where(x => x.IsActive) on _pickassign.AssignID equals _pickdetail.AssignID
                           join _consolidate in RegisterTruckConsolidateService.Where(x => x.IsActive) on _tuckdetail.ShippingDetailID equals _consolidate.ShippingDetailID
                           join _proudct in ProductService.Where(x => x.IsActive) on _dispatchbooking.ProductId equals _proudct.ProductID
                           join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                           join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchbooking.RequestStockUnitId equals _unit.ProductUnitID
                           join _unitcon in ProductUnitService.Where(x => x.IsActive) on _consolidate.StockUnitID equals _unitcon.ProductUnitID
                           join _unitpick in ProductUnitService.Where(x => x.IsActive) on _pickdetail.PickStockUnitID equals _unitpick.ProductUnitID
                           join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                           from _unit2 in a.DefaultIfEmpty()
                           join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                           join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                           from _status in s.DefaultIfEmpty()
                           join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                           from _sub_status in ss.DefaultIfEmpty()
                           join _l in LocationService.Where(x => x.IsActive) on _dispatchbooking.LocationId equals _l.LocationID into l
                           from _location in l.DefaultIfEmpty()
                           join _l2 in LocationService.Where(x => x.IsActive) on _pickdetail.LocationID equals _l2.LocationID into l2
                           from _location2 in l2.DefaultIfEmpty()
                           select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status, _dispatchbooking, _location, _pickdetail, _pickassign, _consolidate, _location2, _unitcon, _unitpick }
                 ).ToList();

                List<DispatchDetailModels> resultsdetailmodel = new List<DispatchDetailModels>();
                resultsdetailmodel = _dt.Select(n => new DispatchDetailModels
                {
                    DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                    DispatchId = n._dispatchdetail.DispatchId,
                    Sequence = n._dispatchdetail.Sequence,
                    ProductId = n._dispatchdetail.ProductId,
                    ProductName = n._proudct.Name,
                    ProductCode = n._productcode.Code,
                    StockUnitId = n._dispatchdetail.StockUnitId,
                    StockUnitName = n._unit.Name,
                    PalletCode = n._dispatchbooking.PalletCode,
                    Quantity = n._dispatchbooking.RequestQty,
                    ConversionQty = n._dispatchbooking.ConversionQty,
                    BaseQuantity = n._dispatchbooking.RequestBaseQty,
                    ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                    DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                    DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                    DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                    BaseUnitId = n._dispatchdetail.BaseUnitId,
                    BaseUnitName = n._unit.Name,
                    DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                    DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                    DispatchPrice = n._dispatchdetail.DispatchPrice,
                    Remark = n._dispatchdetail.Remark,
                    IsActive = n._dispatchdetail.IsActive,
                    DispatchDetailStatusId =(int) n._dispatchdetail.DispatchDetailStatus,
                    DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                    RuleId = n._rule.RuleId,
                    RuleName = n._rule.RuleName,
                    ProductStatusId = n._status.ProductStatusID,
                    ProductStatusName = n._status.Name,
                    ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                    ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),
                    ProductLot = n._dispatchbooking.ProductLot,
                    LocationCode = (n._location != null ? n._location.Code : null),
                    IsBackOrder = n._dispatchbooking.IsBackOrder,
                    PickLocationId = n._pickdetail.LocationID,
                    PickLocationCode = n._location2.Code,
                    PickQTY = n._pickdetail.PickStockQty,
                    PickQTYUnitId = n._pickdetail.PickStockUnitID,
                    PickQTYUnitName = n._unitpick.Name,
                    PickBaseQTY = n._pickdetail.PickBaseQty,
                    PickPalletCode = n._pickdetail.PalletCode,
                    PickProductLot = n._pickassign.PickingLot,
                    ConsolidateQTY = n._consolidate.StockQuantity,
                    ConsolidateBaseQTY = n._consolidate.BaseQuantity,
                    ConsolidateQTYUnitId = n._consolidate.StockUnitID,
                    ConsolidateQTYUnitName = n._unitcon.Name,
                    DeliveryId = n._consolidate.DeliveryID,
                    BookingId = n._dispatchbooking.BookingId
                }).ToList();


                if (resultsdetailmodel.Count > 0)
                {
                    resultsmodel.TypeTotal = 3;
                    resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;

                }
                else
                {
                    var _dt2 = (from _dispatch in Where(x => x.IsActive && x.DispatchId == id)
                                join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                join _dispatchbooking in DispatchBookingService.Where(x => x.IsActive) on _dispatchdetail.DispatchDetailId equals _dispatchbooking.DispatchDetailId
                                join _proudct in ProductService.Where(x => x.IsActive) on _dispatchdetail.ProductId equals _proudct.ProductID
                                join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                                join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.StockUnitId equals _unit.ProductUnitID
                                join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                                from _unit2 in a.DefaultIfEmpty()
                                join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                                join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                                from _status in s.DefaultIfEmpty()
                                join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                                from _sub_status in ss.DefaultIfEmpty()
                                join _pickdetailtemp in (
                                        from _pick in PickingService.Where(x => x.IsActive)
                                        where _pick.DispatchCode == resultsmodel.DispatchCode
                                        join _pickassign in PickingAssignService.Where(x => x.IsActive) on _pick.PickingID equals _pickassign.PickingID
                                        join _pickdetail in PickingDetailService.Where(x => x.IsActive) on _pickassign.AssignID equals _pickdetail.AssignID
                                        join _unitpick2 in ProductUnitService.Where(x => x.IsActive) on _pickdetail.PickStockUnitID equals _unitpick2.ProductUnitID
                                        join _lp in LocationService.Where(x => x.IsActive) on _pickdetail.LocationID equals _lp.LocationID
                                        select new
                                        {
                                            LocationId = _pickdetail.LocationID,
                                            LocationCode = _lp.Code,
                                            ProductId = _pickassign.ProductID,
                                            PickQTYUnitId = _pickdetail.PickStockUnitID,
                                            PickBaseUnitID = _pickdetail.PickBaseUnitID,
                                            PalletCode = _pickdetail.PalletCode,
                                            PickQTYUnitName = _unitpick2.Name,
                                            PickQTY = _pickdetail.PickStockQty,
                                            PickBaseQTY = _pickdetail.PickBaseQty,
                                            PickProductLot = _pickassign.PickingLot,
                                        }
                                ) on _dispatchbooking.ProductId equals _pickdetailtemp.ProductId
                                where _dispatchbooking.BookingStockUnitId == _pickdetailtemp.PickQTYUnitId
                                select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status, _pickdetailtemp }
                            ).ToList();


                    resultsdetailmodel = _dt2.Select(n => new DispatchDetailModels
                    {
                        DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                        DispatchId = n._dispatchdetail.DispatchId,
                        Sequence = n._dispatchdetail.Sequence,
                        ProductId = n._dispatchdetail.ProductId,
                        ProductName = n._proudct.Name,
                        ProductCode = n._productcode.Code,
                        StockUnitId = n._dispatchdetail.StockUnitId,
                        StockUnitName = n._unit.Name,
                        ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                        DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                        DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                        DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                        BaseUnitId = n._dispatchdetail.BaseUnitId,
                        BaseUnitName = n._unit.Name,
                        DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                        DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                        DispatchPrice = n._dispatchdetail.DispatchPrice,
                        Remark = n._dispatchdetail.Remark,
                        IsActive = n._dispatchdetail.IsActive,
                        DispatchDetailStatusId =(int) n._dispatchdetail.DispatchDetailStatus,
                        DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                        RuleId = n._rule.RuleId,
                        RuleName = n._rule.RuleName,
                        ProductStatusId = n._status.ProductStatusID,
                        ProductStatusName = n._status.Name,
                        ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                        ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),
                        PickLocationId = n._pickdetailtemp.LocationId,
                        PickLocationCode = n._pickdetailtemp.LocationCode,
                        PickProductLot = n._pickdetailtemp.PickProductLot,
                        PickQTY = n._pickdetailtemp.PickQTY,
                        PickQTYUnitId = n._pickdetailtemp.PickQTYUnitId,
                        PickQTYUnitName = n._pickdetailtemp.PickQTYUnitName,
                        PickBaseQTY = n._pickdetailtemp.PickBaseQTY,
                        PickPalletCode = n._pickdetailtemp.PalletCode,
                    }).ToList();


                    List<DispatchDetailModels> resultsdetailmodel_g = (from n in resultsdetailmodel
                                                                       select new
                                                                       {
                                                                           DispatchDetailId = n.DispatchDetailId,
                                                                           DispatchId = n.DispatchId,
                                                                           Sequence = n.Sequence,
                                                                           ProductId = n.ProductId,
                                                                           ProductName = n.ProductName,
                                                                           ProductCode = n.ProductCode,
                                                                           StockUnitId = n.StockUnitId,
                                                                           StockUnitName = n.StockUnitName,
                                                                           ProductOwnerId = n.ProductOwnerId,
                                                                           DispatchDetailProductWidth = n.DispatchDetailProductWidth,
                                                                           DispatchDetailProductLength = n.DispatchDetailProductLength,
                                                                           DispatchDetailProductHeight = n.DispatchDetailProductHeight,
                                                                           BaseUnitId = n.BaseUnitId,
                                                                           BaseUnitName = n.BaseUnitName,
                                                                           DispatchPriceUnitId = n.DispatchPriceUnitId,
                                                                           DispatchPriceUnitName = n.DispatchPriceUnitName,
                                                                           DispatchPrice = n.DispatchPrice,
                                                                           Remark = n.Remark,
                                                                           IsActive = n.IsActive,
                                                                           DispatchDetailStatusId = n.DispatchDetailStatusId,
                                                                           DispatchDetailStatusName = n.DispatchDetailStatusName,
                                                                           RuleId = n.RuleId,
                                                                           RuleName = n.RuleName,
                                                                           ProductStatusId = n.ProductStatusId,
                                                                           ProductStatusName = n.ProductStatusName,
                                                                           ProductSubStatusId = n.ProductSubStatusId,
                                                                           ProductSubStatusName = n.ProductSubStatusName,
                                                                           PickLocationId = n.PickLocationId,
                                                                           PickLocationCode = n.PickLocationCode,
                                                                           PickProductLot = n.PickProductLot,
                                                                           PickQTY = n.PickQTY,
                                                                           PickQTYUnitId = n.PickQTYUnitId,
                                                                           PickQTYUnitName = n.PickQTYUnitName,
                                                                           PickBaseQTY = n.PickBaseQTY,
                                                                           PickPalletCode = n.PickPalletCode,
                                                                       } into g
                                                                       group g by new
                                                                       {
                                                                           g.DispatchDetailId,
                                                                           g.DispatchId,
                                                                           g.Sequence,
                                                                           g.ProductId,
                                                                           g.ProductName,
                                                                           g.ProductCode,
                                                                           g.StockUnitId,
                                                                           g.StockUnitName,
                                                                           g.ProductOwnerId,
                                                                           g.DispatchDetailProductWidth,
                                                                           g.DispatchDetailProductLength,
                                                                           g.DispatchDetailProductHeight,
                                                                           g.BaseUnitId,
                                                                           g.BaseUnitName,
                                                                           g.DispatchPriceUnitId,
                                                                           g.DispatchPriceUnitName,
                                                                           g.DispatchPrice,
                                                                           g.Remark,
                                                                           g.IsActive,
                                                                           g.DispatchDetailStatusId,
                                                                           g.DispatchDetailStatusName,
                                                                           g.RuleId,
                                                                           g.RuleName,
                                                                           g.ProductStatusId,
                                                                           g.ProductStatusName,
                                                                           g.ProductSubStatusId,
                                                                           g.ProductSubStatusName,
                                                                           g.PickLocationId,
                                                                           g.PickLocationCode,
                                                                           g.PickProductLot,
                                                                           g.PickQTY,
                                                                           g.PickQTYUnitId,
                                                                           g.PickQTYUnitName,
                                                                           g.PickBaseQTY,
                                                                           g.PickPalletCode,
                                                                       } into x
                                                                       select new DispatchDetailModels
                                                                       {
                                                                           DispatchDetailId = x.Key.DispatchDetailId,
                                                                           DispatchId = x.Key.DispatchId,
                                                                           Sequence = x.Key.Sequence,
                                                                           ProductId = x.Key.ProductId,
                                                                           ProductName = x.Key.ProductName,
                                                                           ProductCode = x.Key.ProductCode,
                                                                           StockUnitId = x.Key.StockUnitId,
                                                                           StockUnitName = x.Key.StockUnitName,
                                                                           ProductOwnerId = x.Key.ProductOwnerId,
                                                                           DispatchDetailProductWidth = x.Key.DispatchDetailProductWidth,
                                                                           DispatchDetailProductLength = x.Key.DispatchDetailProductLength,
                                                                           DispatchDetailProductHeight = x.Key.DispatchDetailProductHeight,
                                                                           BaseUnitId = x.Key.BaseUnitId,
                                                                           BaseUnitName = x.Key.BaseUnitName,
                                                                           DispatchPriceUnitId = x.Key.DispatchPriceUnitId,
                                                                           DispatchPriceUnitName = x.Key.DispatchPriceUnitName,
                                                                           DispatchPrice = x.Key.DispatchPrice,
                                                                           Remark = x.Key.Remark,
                                                                           IsActive = x.Key.IsActive,
                                                                           DispatchDetailStatusId = x.Key.DispatchDetailStatusId,
                                                                           DispatchDetailStatusName = x.Key.DispatchDetailStatusName,
                                                                           RuleId = x.Key.RuleId,
                                                                           RuleName = x.Key.RuleName,
                                                                           ProductStatusId = x.Key.ProductStatusId,
                                                                           ProductStatusName = x.Key.ProductStatusName,
                                                                           ProductSubStatusId = x.Key.ProductSubStatusId,
                                                                           ProductSubStatusName = x.Key.ProductSubStatusName,
                                                                           PickLocationId = x.Key.PickLocationId,
                                                                           PickLocationCode = x.Key.PickLocationCode,
                                                                           PickProductLot = x.Key.PickProductLot,
                                                                           PickQTY = x.Key.PickQTY,
                                                                           PickQTYUnitId = x.Key.PickQTYUnitId,
                                                                           PickQTYUnitName = x.Key.PickQTYUnitName,
                                                                           PickBaseQTY = x.Key.PickBaseQTY,
                                                                           PickPalletCode = x.Key.PickPalletCode,
                                                                       }
                                    ).ToList();


                    if (resultsdetailmodel.Count > 0)
                    {
                        resultsmodel.TypeTotal = 4;
                        resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel_g;
                    }
                    else
                    {
                        var _dt3 = (from _dispatchbooking in DispatchBookingService.Where(x => x.IsActive)
                                    join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive && x.DispatchId == id) on _dispatchbooking.DispatchDetailId equals _dispatchdetail.DispatchDetailId
                                    join _proudct in ProductService.Where(x => x.IsActive) on _dispatchbooking.ProductId equals _proudct.ProductID
                                    join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                                    join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchbooking.RequestStockUnitId equals _unit.ProductUnitID
                                    join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                                    from _unit2 in a.DefaultIfEmpty()
                                    join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                                    join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                                    from _status in s.DefaultIfEmpty()
                                    join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                                    from _sub_status in ss.DefaultIfEmpty()
                                    join _l in LocationService.Where(x => x.IsActive) on _dispatchbooking.LocationId equals _l.LocationID into l
                                    from _location in l.DefaultIfEmpty()
                                    select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status, _dispatchbooking, _location }
                        ).ToList();

                        resultsdetailmodel = _dt3.Select(n => new DispatchDetailModels
                        {
                            DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                            DispatchId = n._dispatchdetail.DispatchId,
                            Sequence = n._dispatchdetail.Sequence,
                            ProductId = n._dispatchdetail.ProductId,
                            ProductName = n._proudct.Name,
                            ProductCode = n._productcode.Code,
                            StockUnitId = n._dispatchdetail.StockUnitId,
                            StockUnitName = n._unit.Name,
                            Quantity = n._dispatchbooking.RequestQty,
                            ConversionQty = n._dispatchbooking.ConversionQty,
                            BaseQuantity = n._dispatchbooking.RequestBaseQty,
                            ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                            DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                            DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                            DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                            BaseUnitId = n._dispatchdetail.BaseUnitId,
                            BaseUnitName = n._unit.Name,
                            DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                            DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                            DispatchPrice = n._dispatchdetail.DispatchPrice,
                            Remark = n._dispatchdetail.Remark,
                            IsActive = n._dispatchdetail.IsActive,
                            DispatchDetailStatusId = (int)n._dispatchdetail.DispatchDetailStatus,
                            DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                            RuleId = n._rule.RuleId,
                            RuleName = n._rule.RuleName,
                            ProductStatusId = n._status.ProductStatusID,
                            ProductStatusName = n._status.Name,
                            ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                            ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),
                            ProductLot = n._dispatchbooking.ProductLot,
                            LocationCode = (n._location != null ? n._location.Code : null),
                            PalletCode = n._dispatchbooking.PalletCode,
                            IsBackOrder = n._dispatchbooking.IsBackOrder
                        }).ToList();

                        if (resultsdetailmodel.Count > 0)
                        {
                            resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;
                            resultsmodel.TypeTotal = 2;
                        }
                        else
                        {

                            var _dt4 = (from _dispatchdetail in DispatchDetailService.Where(x => x.IsActive && x.DispatchId == id)
                                        join _proudct in ProductService.Where(x => x.IsActive) on _dispatchdetail.ProductId equals _proudct.ProductID
                                        join _productcode in ProductCodeService.Where(x => x.CodeType == ProductCodeTypeEnum.Stock) on _proudct.ProductID equals _productcode.ProductID
                                        join _unit in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.StockUnitId equals _unit.ProductUnitID
                                        join _a in ProductUnitService.Where(x => x.IsActive) on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                                        from _unit2 in a.DefaultIfEmpty()
                                        join _rule in SpecialBookingRuleService.Where(x => x.IsActive) on _dispatchdetail.RuleId equals _rule.RuleId
                                        join _s in ProductStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                                        from _status in s.DefaultIfEmpty()
                                        join _ss in ProductSubStatusService.Where(x => x.IsActive) on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                                        from _sub_status in ss.DefaultIfEmpty()
                                        select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status }
                                        ).ToList();

                            resultsdetailmodel = _dt4.Select(n => new DispatchDetailModels
                            {
                                DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                                DispatchId = n._dispatchdetail.DispatchId,
                                Sequence = n._dispatchdetail.Sequence,
                                ProductId = n._dispatchdetail.ProductId,
                                ProductName = n._proudct.Name,
                                ProductCode = n._productcode.Code,
                                StockUnitId = n._dispatchdetail.StockUnitId,
                                StockUnitName = n._unit.Name,
                                Quantity = n._dispatchdetail.Quantity,
                                ConversionQty = n._dispatchdetail.ConversionQty,
                                BaseQuantity = n._dispatchdetail.BaseQuantity,
                                ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                                DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                                DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                                DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                                BaseUnitId = n._dispatchdetail.BaseUnitId,
                                BaseUnitName = n._unit.Name,
                                DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                                DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                                DispatchPrice = n._dispatchdetail.DispatchPrice,
                                Remark = n._dispatchdetail.Remark,
                                IsActive = n._dispatchdetail.IsActive,
                                DispatchDetailStatusId = (int)n._dispatchdetail.DispatchDetailStatus,
                                DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                                RuleId = n._rule.RuleId,
                                RuleName = n._rule.RuleName,
                                ProductStatusId = n._status.ProductStatusID,
                                ProductStatusName = n._status.Name,
                                ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                                ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),
                                ProductLot = "",
                                LocationCode = "",
                                PalletCode = "",
                                IsBackOrder = null
                            }).ToList();

                            resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;
                            resultsmodel.TypeTotal = 1;

                        }

                    }
                }

                return resultsmodel;


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

        public DispatchModels GetPackingById(Guid id)
        {
            try
            {

                SqlParameter param = new SqlParameter("@DispatchId", SqlDbType.UniqueIdentifier) { Value = id };

                DispatchModels resultsmodel = Context.SQLQuery<DispatchModels>("exec SP_GetPackingByDispatchId @DispatchId ", param).FirstOrDefault();


                if (resultsmodel == null)
                {
                    throw new HILIException("MSG00047");
                }

                resultsmodel.DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), resultsmodel.DispatchStatusId.ToString()));



                SqlParameter param1 = new SqlParameter("@DispatchId", SqlDbType.UniqueIdentifier) { Value = id };


                List<DispatchDetailModels> result_detail = Context.SQLQuery<DispatchDetailModels>("exec SP_GetPackingDetailByDispatchId @DispatchId", param1).ToList();

                result_detail.ForEach(item =>
                {
                    item.DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), item.DispatchDetailStatusId.ToString()));
                });
                resultsmodel.DispatchDetailModelsCollection = result_detail;

                resultsmodel.TypeTotal = 4;
                return resultsmodel; 
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

        public string GetDispatchPreFixEnum(DispatchPreFixTypeEnum prefixtype)
        {
            DispatchPrefix prefix = DispatchPrefixService.SingleOrDefault(x => x.PrefixType == prefixtype);
            if (prefix == null)
            {
                throw new HILIException("MSG00006");
            }

            string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
            return DispatchCode;
        }

        public void AddAll(Dispatch entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Any(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close 
                    && x.Pono.Replace(" ", string.Empty).ToLower().Equals(entity.Pono.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00033");
                    }

                    if (entity.ReferenceId == null)
                    {
                        //Dispatch Code
                        DispatchPrefix prefix = DispatchPrefixService.SingleOrDefault(x => x.PrefixType == DispatchPreFixTypeEnum.DISPATHCODE);
                        if (prefix == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                        entity.DispatchCode = DispatchCode;
                        prefix.LastedKey = DispatchCode;

                        DispatchPrefixService.Modify(prefix);

                        //PO No
                        ItfInterfaceMapping _checkinternal = ItfInterfaceMappingService.SingleOrDefault(x => x.DocumentId == entity.DocumentId);
                        if (_checkinternal.IsMarketing.Value)
                        {
                            DispatchPrefix poprefix = DispatchPrefixService.SingleOrDefault(x => x.PrefixType == DispatchPreFixTypeEnum.PONO_INTERNAL);
                            if (poprefix == null)
                            {
                                throw new HILIException("MSG00006");
                            }

                            poprefix.LastedKey = entity.Pono;
                            DispatchPrefixService.Modify(poprefix);
                        }
                    }
                    entity.UserCreated = UserID;
                    entity.UserModified = UserID;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    foreach (DispatchDetail item in entity.DispatchDetailCollection)
                    {
                        item.UserCreated = UserID;
                        item.UserModified = UserID;
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                    }
                    Dispatch result = base.Add(entity);

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

        public void ModifyAll(Dispatch entity)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    Dispatch _current = FirstOrDefault(x => x.DispatchId == entity.DispatchId);//.Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool dupicate = Any(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close
                                                 && x.DispatchId != entity.DispatchId
                                                 && x.Pono.Replace(" ", string.Empty)
                                 .ToLower()
                                 .Equals(entity.Pono.Replace(" ", string.Empty).ToLower()));

                    if (dupicate)
                    {
                        throw new HILIException("MSG00033");
                    }

                    entity.UserCreated = _current.UserCreated;
                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    List<DispatchDetail> _currentdetail = DispatchDetailService.Where(x => x.DispatchId == entity.DispatchId && x.IsActive).ToList();
                    _currentdetail.ForEach(item =>
                    {
                        item.IsActive = false;
                        item.DateModified = DateTime.Now;
                        item.UserModified = UserID;
                        DispatchDetailService.Modify(item);
                    });

                    if (entity.DispatchDetailCollection != null)
                    {
                        //Add Edit Item 
                        entity.DispatchDetailCollection.ToList().ForEach(item =>
                           {
                               if (item.DispatchDetailId == Guid.Empty || Utilities.IsZeroGuid(item.DispatchDetailId))
                               {
                                   item.DateCreated = DateTime.Now;
                                   item.UserCreated = UserID;
                                   item.DateModified = DateTime.Now;
                                   item.UserModified = UserID;
                                   DispatchDetailService.Add(item);
                               }
                               else
                               {
                                   DispatchDetail itemCurrent = DispatchDetailService.FirstOrDefault(x => x.DispatchDetailId == item.DispatchDetailId);
                                   if (itemCurrent == null)
                                   {
                                       throw new HILIException("REC10001");
                                   }
                                   itemCurrent.UserCreated = _current.UserCreated;
                                   itemCurrent.DateModified = DateTime.Now;
                                   itemCurrent.UserModified = UserID;
                                   itemCurrent.BackOrderQuantity = item.BackOrderQuantity;
                                   itemCurrent.BaseQuantity = item.BaseQuantity;
                                   itemCurrent.BaseUnitId = item.BaseUnitId;
                                   itemCurrent.ConversionQty = item.ConversionQty;
                                   itemCurrent.DateCreated = item.DateCreated;
                                   itemCurrent.DispatchDetailProductHeight = item.DispatchDetailProductHeight;
                                   itemCurrent.DispatchDetailProductLength = item.DispatchDetailProductLength;
                                   itemCurrent.DispatchDetailProductWidth = item.DispatchDetailProductWidth;
                                   itemCurrent.DispatchDetailId = item.DispatchDetailId;
                                   itemCurrent.DispatchDetailStatus = item.DispatchDetailStatus;
                                   itemCurrent.DispatchId = item.DispatchId;
                                   itemCurrent.DispatchPrice = item.DispatchPrice;
                                   itemCurrent.DispatchPriceUnitId = item.DispatchPriceUnitId;
                                   itemCurrent.IsActive = true;
                                   itemCurrent.IsBackOrder = item.IsBackOrder;
                                   itemCurrent.IsSentInterface = item.IsSentInterface;
                                   itemCurrent.ProductId = item.ProductId;
                                   itemCurrent.ProductOwnerId = item.ProductOwnerId;
                                   itemCurrent.ProductStatusId = item.ProductStatusId;
                                   itemCurrent.ProductSubStatusId = item.ProductSubStatusId;
                                   itemCurrent.Quantity = item.Quantity;
                                   itemCurrent.Remark = item.Remark;
                                   itemCurrent.ReviseDateTime = item.ReviseDateTime;
                                   itemCurrent.ReviseReason = item.ReviseReason;
                                   itemCurrent.RuleId = item.RuleId;
                                   itemCurrent.Sequence = item.Sequence;
                                   itemCurrent.StockUnitId = item.StockUnitId;
                                   DispatchDetailService.Modify(itemCurrent);
                               }
                           });
                    }
                    _current.CustomerId = entity.CustomerId;
                    _current.DeliveryDate = entity.DeliveryDate;
                    _current.DispatchCode = entity.DispatchCode;
                    _current.DispatchId = entity.DispatchId;
                    _current.DispatchStatus = entity.DispatchStatus;
                    _current.DocumentApproveDate = entity.DocumentApproveDate;
                    _current.DocumentDate = entity.DocumentDate;
                    _current.DocumentId = entity.DocumentId;
                    _current.FromwarehouseId = entity.FromwarehouseId;
                    _current.IsActive = entity.IsActive;
                    _current.IsBackOrder = entity.IsBackOrder;
                    _current.IsUrgent = entity.IsUrgent;
                    _current.OrderDate = entity.OrderDate;
                    _current.OrderNo = entity.OrderNo;
                    _current.Pono = entity.Pono;
                    _current.ReferenceId = entity.ReferenceId;
                    _current.Remark = entity.Remark;
                    _current.ReviseDateTime = entity.ReviseDateTime;
                    _current.ReviseReason = entity.ReviseReason;
                    _current.ShipptoId = entity.ShipptoId;
                    _current.SupplierId = entity.SupplierId;
                    _current.TowarehouseId = entity.TowarehouseId;
                    _current.UserModified = entity.UserModified;
                    _current.DateModified = entity.DateModified;
                    base.Modify(_current);

                    scope.Complete();

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

        public void ModifyHeader(Dispatch entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Dispatch _current = FirstOrDefault(x => x.DispatchId == entity.DispatchId);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    //var ok = Query().Filter(x => x.IsActive && x.DispatchId != entity.DispatchId && x.Pono.Replace(" ", string.Empty).ToLower().Equals(entity.Pono.Replace(" ", string.Empty).ToLower())).Get().FirstOrDefault();

                    //if (ok != null)
                    //    throw new HILIException("MSG00033");
                    _current.DocumentApproveDate = entity.DocumentApproveDate;
                    _current.DispatchStatus = entity.DispatchStatus;
                    _current.DeliveryDate = entity.DeliveryDate;
                    _current.DocumentDate = entity.DocumentDate;
                    _current.OrderDate = entity.OrderDate;
                    _current.UserModified = UserID;
                    _current.DateModified = DateTime.Now;
                    _current.CustomerId = entity.CustomerId;
                    _current.DateCreated = entity.DateCreated;
                    _current.DispatchCode = entity.DispatchCode;
                    _current.DispatchDetailCollection = entity.DispatchDetailCollection;
                    _current.DocumentId = entity.DocumentId;
                    _current.FromwarehouseId = entity.FromwarehouseId;
                    _current.IsActive = entity.IsActive;
                    _current.IsBackOrder = entity.IsBackOrder;
                    _current.IsUrgent = entity.IsUrgent;
                    _current.OrderNo = entity.OrderNo;
                    _current.Pono = entity.Pono;
                    _current.ReferenceId = entity.ReferenceId;
                    _current.Remark = entity.Remark;
                    _current.ReviseDateTime = entity.ReviseDateTime;
                    _current.ReviseReason = entity.ReviseReason;
                    _current.ShipptoId = entity.ShipptoId;
                    _current.SupplierId = entity.SupplierId;
                    _current.TowarehouseId = entity.TowarehouseId;
                    _current.UserCreated = entity.UserCreated;
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

        public void ModifyDetail(List<DispatchDetail> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {

                    entity.ForEach(item =>
                    {
                        item.UserModified = UserID;
                        item.DateModified = DateTime.Now;
                        DispatchDetailService.Modify(item);
                    });
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

        public void RemoveDispatch(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Dispatch _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
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

        public bool OnApproveDispatch(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            try
            {
                List<StockInOutModel> stockOut = new List<StockInOutModel>();
                Guid? _documenttypeid = null;
                Guid? _dispatchid = null;

                //validation
                if (string.IsNullOrEmpty(dispatchcode))
                {
                    throw new HILIException("MSG00006");
                }

                if (string.IsNullOrEmpty(pono))
                {
                    throw new HILIException("MSG00006");
                }

                Dispatch _checkdispatch = FirstOrDefault(x => x.DispatchStatus != DispatchStatusEnum.Close && x.DispatchCode == dispatchcode && x.Pono == pono);
                if (_checkdispatch != null)
                {
                    if (_checkdispatch.DispatchStatus ==DispatchStatusEnum.Complete)
                    {
                        throw new HILIException("Data Is Completed!!");
                    }
                }
                var _datadisptch = (from _dispatch in Where(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close && x.DispatchCode == dispatchcode && x.Pono == pono && x.Pono.ToLower() == pono.ToLower())
                                    join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive)
                                    on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                    select new
                                    {
                                        ProductStatusId = _dispatchdetail.ProductStatusId,
                                        dispatchid = _dispatch.DispatchId,
                                        dispatchcode = _dispatch.DispatchCode,
                                        documenttypeid = _dispatch.DocumentId
                                    }
                              ).FirstOrDefault();

                Guid? _productstatusid = _datadisptch.ProductStatusId;
                string _dispatchcode = _datadisptch.dispatchcode;
                _dispatchid = _datadisptch.dispatchid;
                _documenttypeid = _datadisptch.documenttypeid;

                SqlParameter param = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param2 = new SqlParameter("@Dispatchcode", SqlDbType.NVarChar) { Value = _dispatchcode };

                List<DispatchApproveModels> result = Context.SQLQuery<DispatchApproveModels>("exec SP_CheckApproveDispatch @PoNo, @Dispatchcode", param, param2).ToList(); 
                if (result.Count == 0)
                {
                    throw new HILIException("MSG00052");
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    var _result = result.GroupBy(g => new
                    {
                        g.ProductId,
                        g.StockUnitID,
                        g.BaseUnitID,
                        g.ProductLot,
                        g.ProductOwnerID,
                        g.SupplierID,
                        g.MFGDate,
                        g.ExpirationDate,
                        g.ProductWidth,
                        g.ProductLength,
                        g.ProductHeight,
                        g.ProductWeight,
                        g.PackageWeight,
                        g.Price,
                        g.ProductUnitPriceID,
                        g.ProductStatusID,
                        g.ProductSubStatusID,
                        g.ConversionQty,
                        g.PalletCode,
                        g.LocationCode,
                        g.DispatchCode,
                    }).Select(n => new
                    {
                        ProductId = n.Key.ProductId,
                        StockUnitID = n.Key.StockUnitID,
                        BaseUnitID = n.Key.BaseUnitID,
                        ProductLot = n.Key.ProductLot,
                        ProductOwnerID = n.Key.ProductOwnerID,
                        SupplierID = n.Key.SupplierID,
                        MFGDate = n.Key.MFGDate,
                        ExpirationDate = n.Key.ExpirationDate,
                        ProductWidth = n.Key.ProductWidth,
                        ProductLength = n.Key.ProductLength,
                        ProductHeight = n.Key.ProductHeight,
                        ProductWeight = n.Key.ProductWeight,
                        PackageWeight = n.Key.PackageWeight,
                        Price = n.Key.Price,
                        ProductUnitPriceID = n.Key.ProductUnitPriceID,
                        ProductStatusID = n.Key.ProductStatusID,
                        ProductSubStatusID = n.Key.ProductSubStatusID,
                        ConversionQty = n.Key.ConversionQty,
                        PalletCode = n.Key.PalletCode,
                        LocationCode = n.Key.LocationCode,
                        DispatchCode = n.Key.DispatchCode,
                        ReserveQty = n.Sum(s => s.ReserveQty),
                        StockQuantity = n.Sum(s => s.StockQuantity)
                    }).ToList();

                    _result.ForEach(item =>
                    {
                        stockOut.Add(new StockInOutModel
                        {
                            ProductID = item.ProductId,
                            StockUnitID = item.StockUnitID.Value,
                            BaseUnitID = item.BaseUnitID.Value,
                            Lot = item.ProductLot,
                            ProductOwnerID = item.ProductOwnerID.Value,
                            SupplierID = item.SupplierID.Value,
                            ManufacturingDate = item.MFGDate.Value,
                            ExpirationDate = item.ExpirationDate.Value,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ProductStatusID = item.ProductStatusID.Value,
                            ProductSubStatusID = item.ProductSubStatusID.Value,
                            Quantity = item.StockQuantity.Value,
                            ConversionQty = item.ConversionQty.Value,
                            PalletCode = item.PalletCode,
                            LocationCode = item.LocationCode,
                            DocumentCode = item.DispatchCode,
                            DocumentTypeID = _documenttypeid,
                            DocumentID = _dispatchid.Value,
                            StockTransTypeEnum = StockTransactionTypeEnum.Outgoing,
                            Remark = "Approve Dispatch",
                            ReserveQuantity = item.ReserveQty
                        });

                        ProductionControlDetail _pcdt = ProductionControlDetailService.Query().Filter(x => x.PalletCode == item.PalletCode).Get().FirstOrDefault();
                        if (_pcdt == null)
                        {
                            throw new HILIException("MSG00038");
                        }

                        ProductionControlDetail pcdt = ProductionControlDetailService.FindByID(_pcdt.PackingID);

                        pcdt.ReserveQTY -= item.ReserveQty;
                        pcdt.ReserveBaseQTY -= item.ReserveQty * item.ConversionQty;
                        pcdt.RemainQTY -= item.StockQuantity;
                        pcdt.RemainBaseQTY -= item.StockQuantity * item.ConversionQty;
                        pcdt.DateModified = DateTime.Now;
                        pcdt.UserModified = UserID;

                        if (pcdt.RemainQTY == 0)
                        {
                            pcdt.PackingStatus = PackingStatusEnum.Delivery;
                        }
                        ProductionControlDetailService.Modify(pcdt);

                    });

                    stockService.UserID = UserID;
                    stockService.Outgoing2(stockOut, Context);
                    Dispatch _updatedbhd = FirstOrDefault(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close && x.Pono == pono);
                    if (_updatedbhd == null)
                    {
                        throw new HILIException("MSG00053");
                    }
                    List<DispatchDetail> updatedbdt = DispatchDetailService.Where(x => x.IsActive && x.DispatchId == _updatedbhd.DispatchId).ToList();

                    updatedbdt.ForEach(itemdt =>
                    {
                        DispatchDetail _dt = DispatchDetailService.FindByID(itemdt.DispatchDetailId); 
                        _dt.DispatchDetailStatus = DispatchDetailStatusEnum.Complete;
                        _dt.DateModified = DateTime.Now;
                        _dt.UserModified = UserID;
                        DispatchDetailService.Modify(_dt);
                    });
                    Dispatch updatedbhd = FindByID(_updatedbhd.DispatchId);
                    //this.ModifyDetail(updatedbdt);
                    TimeSpan ts = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);
                    updatedbhd.DocumentApproveDate = approvedispatchdate.Add(ts);
                    updatedbhd.DispatchStatus = DispatchStatusEnum.Complete;
                    base.Modify(updatedbhd);
                    //this.ModifyHeader(updatedbhd);
                    RegisterTruck _updatetruck = RegisterTruckService.FirstOrDefault(x => x.ShippingStatus != (int)ShippingStatusEnum.Cancel && x.PoNo == pono);
                    if (_updatetruck == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    RegisterTruck updatetruck = RegisterTruckService.FindByID(_updatetruck.ShippingID);

                    updatetruck.DateModified = DateTime.Now;
                    updatetruck.ShippingStatus = (int)ShippingStatusEnum.Complete;
                    RegisterTruckService.Modify(updatetruck);


                    //##### Interface Log ######
                    List<InterfaceDispatchModel> itfmodel = (from s in result
                                                             select new
                                                             {
                                                                 ProductId = s.ProductId,
                                                                 DispatchCode = s.DispatchCode,
                                                                 PONo = s.PONo,
                                                                 Quantity = s.StockQuantity.Value,
                                                                 UnitId = s.StockUnitID.Value,
                                                                 BaseQuantity = s.StockQuantity.Value * s.ConversionQty.Value,
                                                                 BaseUnitId = s.BaseUnitID.Value,
                                                                 Lot = s.ProductLot
                                                             } into g
                                                             group g by new
                                                             {
                                                                 g.ProductId,
                                                                 g.DispatchCode,
                                                                 g.PONo,
                                                                 g.UnitId,
                                                                 g.BaseUnitId,
                                                                 g.Lot
                                                             } into x
                                                             select new InterfaceDispatchModel
                                                             {
                                                                 ProductId = x.Key.ProductId,
                                                                 DispatchCode = x.Key.DispatchCode,
                                                                 PONo = x.Key.PONo,
                                                                 Quantity = x.Sum(s => s.Quantity),
                                                                 UnitId = x.Key.UnitId,
                                                                 BaseQuantity = x.Sum(s => s.BaseQuantity),
                                                                 BaseUnitId = x.Key.BaseUnitId,
                                                                 Lot = x.Key.Lot
                                                             }
                                    ).ToList();

                    AddInterfaceDispatch(itfmodel);
                    //#####  Interface Log ######

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

        public bool OnApproveDispatchPicking(string dispatchcode, string pono, DateTime approvedispatchdate)
        {
            try
            {
                Guid? _documenttypeid = null;
                Guid? _dispatchid = null;


                //validation
                if (string.IsNullOrEmpty(dispatchcode))
                {
                    throw new HILIException("MSG00006");
                }

                if (string.IsNullOrEmpty(pono))
                {
                    throw new HILIException("MSG00006");
                }

                Dispatch _checkdispatch = FirstOrDefault(x => x.DispatchStatus != DispatchStatusEnum.Close && x.DispatchCode == dispatchcode && x.Pono == pono);
                if (_checkdispatch != null)
                {
                    if (_checkdispatch.DispatchStatus == DispatchStatusEnum.Complete)
                    {
                        throw new HILIException("Data Is Completed!!");
                    }
                }

                var _datadisptch = (from _dispatch in Where(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close 
                                    && x.DispatchCode == dispatchcode && x.Pono == pono && x.Pono.ToLower() == pono.ToLower())
                                    join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive)
                                    on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                    select new
                                    {
                                        ProductStatusId = _dispatchdetail.ProductStatusId,
                                        dispatchid = _dispatch.DispatchId,
                                        dispatchcode = _dispatch.DispatchCode,
                                        documenttypeid = _dispatch.DocumentId
                                    }
                              ).FirstOrDefault();

                Guid? _productstatusid = _datadisptch.ProductStatusId;
                string _dispatchcode = _datadisptch.dispatchcode;
                _dispatchid = _datadisptch.dispatchid;
                _documenttypeid = _datadisptch.documenttypeid;

                SqlParameter param = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param2 = new SqlParameter("@Dispatchcode", SqlDbType.NVarChar) { Value = dispatchcode };

                List<DispatchApproveModels> result = Context.SQLQuery<DispatchApproveModels>("exec SP_CheckApproveDispatchPicking @PoNo, @Dispatchcode", param, param2).ToList();




                //var param11 = new SqlParameter("@PoNo", SqlDbType.NVarChar) { Value = pono };
                //var param22 = new SqlParameter("@Dispatchcode", SqlDbType.NVarChar) { Value = dispatchcode };

                //var restore_result = unitofwork.SQLQuery<DispatchApproveModels>("exec SP_CheckRestoreReservePicking @PoNo, @Dispatchcode", param11, param22).ToList();


                if (result.Count == 0)
                {
                    throw new HILIException("MSG00052");
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {

                    List<StockInOutModel> stockOut = new List<StockInOutModel>();
                    result.ForEach(item =>
                    {
                        stockOut.Add(new StockInOutModel
                        {
                            ProductID = item.ProductId,
                            StockUnitID = item.StockUnitID.Value,
                            BaseUnitID = item.BaseUnitID.Value,
                            Lot = item.ProductLot,
                            ProductOwnerID = item.ProductOwnerID.Value,
                            SupplierID = item.SupplierID.Value,
                            ManufacturingDate = item.MFGDate.Value,
                            ExpirationDate = item.ExpirationDate.Value,
                            ProductWidth = item.ProductWidth,
                            ProductLength = item.ProductLength,
                            ProductHeight = item.ProductHeight,
                            ProductWeight = item.ProductWeight,
                            PackageWeight = item.PackageWeight,
                            Price = item.Price,
                            ProductUnitPriceID = item.ProductUnitPriceID,
                            ProductStatusID = item.ProductStatusID.Value,//hold pallet
                            ProductSubStatusID = item.ProductSubStatusID.Value,
                            Quantity = item.StockQuantity.Value,//out normal
                            ConversionQty = item.ConversionQty.Value,
                            PalletCode = item.PalletCode,
                            LocationCode = item.LocationCode,
                            DocumentCode = item.DispatchCode,
                            DocumentTypeID = _documenttypeid,
                            DocumentID = _dispatchid.Value,
                            StockTransTypeEnum = StockTransactionTypeEnum.Outgoing,
                            Remark = "Approve Dispatch",
                            ReserveQuantity = item.ReserveQty
                        });



                        ProductionControlDetail pcdt = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == item.PalletCode);
                        if (pcdt == null)
                        {
                            throw new HILIException("MSG00038");
                        }

                        //var pcdt = ProductionControlDetailService.FindByID(_pcdt.PackingID);

                        pcdt.ReserveQTY -= item.ReserveQty;
                        pcdt.ReserveBaseQTY -= item.ReserveQty * item.ConversionQty;
                        pcdt.RemainQTY -= item.StockQuantity;
                        pcdt.RemainBaseQTY -= item.StockQuantity * item.ConversionQty;
                        pcdt.DateModified = DateTime.Now;
                        pcdt.UserModified = UserID;

                        if (pcdt.RemainQTY == 0)
                        {
                            pcdt.PackingStatus = PackingStatusEnum.Delivery;
                        }
                        ProductionControlDetailService.Modify(pcdt);

                    });


                    stockService.UserID = UserID;
                    stockService.Outgoing2(stockOut, Context);


                    //List<StockInOutModel> stockRestore = new List<StockInOutModel>();
                    //restore_result.ForEach(item =>
                    //{

                    //    stockRestore.Add(new StockInOutModel
                    //    {
                    //        ProductID = item.ProductId,
                    //        StockUnitID = item.StockUnitID.Value,
                    //        BaseUnitID = item.BaseUnitID.Value,
                    //        Lot = item.ProductLot,
                    //        ProductOwnerID = item.ProductOwnerID.Value,
                    //        SupplierID = item.SupplierID.Value,
                    //        ManufacturingDate = item.MFGDate.Value,
                    //        ExpirationDate = item.ExpirationDate.Value,
                    //        ProductWidth = item.ProductWidth,
                    //        ProductLength = item.ProductLength,
                    //        ProductHeight = item.ProductHeight,
                    //        ProductWeight = item.ProductWeight,
                    //        PackageWeight = item.PackageWeight,
                    //        Price = item.Price,
                    //        ProductUnitPriceID = item.ProductUnitPriceID,
                    //        ProductStatusID = item.ProductStatusID.Value,//hold pallet
                    //        ProductSubStatusID = item.ProductSubStatusID.Value, 
                    //        ConversionQty = item.ConversionQty.Value,
                    //        PalletCode = item.PalletCode,
                    //        LocationCode = item.LocationCode,
                    //        DocumentCode = item.DispatchCode,
                    //        DocumentTypeID = _documenttypeid,
                    //        DocumentID = _dispatchid.Value,
                    //        StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                    //        Remark = "Cancel Dispatch",
                    //        ReserveQuantity = item.StockQuantity
                    //    });


                    //    var _pc_detail = ProductionControlDetailService.Query().Filter(x => x.PalletCode == item.PalletCode).Get().FirstOrDefault();
                    //    var pc_detail = ProductionControlDetailService.FindByID(_pc_detail.PackingID);

                    //    pc_detail.ReserveQTY -= item.StockQuantity;
                    //    pc_detail.ReserveBaseQTY -= item.StockQuantity * pc_detail.ConversionQty;
                    //    pc_detail.DateModified = DateTime.Now;
                    //    pc_detail.UserModified = this.UserID;
                    //    ProductionControlDetailService.Modify(pc_detail);


                    //}); 

                    //stockService.UserID = UserID;
                    //stockService.RestoreReserve(stockRestore, unitofwork);


                    List<Picking> _pick = PickingService.Where(x => x.DispatchCode == dispatchcode && x.PONo == @pono).ToList();

                    _pick.ForEach(item_pick =>
                   {


                       if (item_pick.PickingStatus != PickingStatusEnum.Complete)
                       {
                           Picking pick = PickingService.FindByID(item_pick.PickingID);
                           pick.PickingStatus = PickingStatusEnum.Complete;
                           pick.DateModified = DateTime.Now;
                           pick.UserModified = UserID;
                           PickingService.Modify(pick); 
                       }
                       List<PickingAssign> _pick_assign = PickingAssignService.Where(x => x.PickingID == item_pick.PickingID && x.IsActive).ToList();
                       _pick_assign.ForEach(item =>
                       {
                           PickingAssign pick_assign = PickingAssignService.FindByID(item.AssignID);
                           pick_assign.AssignStatus =  PickingStatusEnum.Complete;
                           pick_assign.DateModified = DateTime.Now;
                           pick_assign.UserModified = UserID;
                           PickingAssignService.Modify(pick_assign);
                           List<PickingDetail> _detail = PickingDetailService.Where(x => x.AssignID == pick_assign.AssignID && x.IsActive).ToList();
                           _detail.ForEach(d =>
                          {
                              PickingDetail detail = PickingDetailService.FindByID(d.PickingDetailID);
                              detail.PickingStatus = 100;
                              detail.DateModified = DateTime.Now;
                              detail.UserModified = UserID;
                              PickingDetailService.Modify(detail);
                          });
                       });
                   });
                    Dispatch _updatedbhd = FirstOrDefault(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close && x.Pono == pono);
                    if (_updatedbhd == null)
                    {
                        throw new HILIException("MSG00053");
                    }
                    List<DispatchDetail> updatedbdt = DispatchDetailService.Where(x => x.IsActive && x.DispatchId == _updatedbhd.DispatchId).ToList();
                    updatedbdt.ForEach(itemdt =>
                    {
                        DispatchDetail _updatedbdt = DispatchDetailService.FindByID(itemdt.DispatchDetailId);
                        _updatedbdt.DispatchDetailStatus = DispatchDetailStatusEnum.Complete;
                        _updatedbdt.DateModified = DateTime.Now;
                        _updatedbdt.UserModified = UserID;
                        DispatchDetailService.Modify(_updatedbdt);
                    });
                    Dispatch updatedbhd = FindByID(_updatedbhd.DispatchId);

                    TimeSpan ts = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);
                    updatedbhd.DocumentApproveDate = approvedispatchdate.Add(ts);
                    updatedbhd.DispatchStatus = DispatchStatusEnum.Complete;
                    Modify(updatedbhd);
                    //##### Interface Log ######
                    List<InterfaceDispatchModel> itfmodel = (from s in result
                                                             select new
                                                             {
                                                                 ProductId = s.ProductId,
                                                                 DispatchCode = s.DispatchCode,
                                                                 PONo = s.PONo,
                                                                 Quantity = s.StockQuantity.Value,
                                                                 UnitId = s.StockUnitID.Value,
                                                                 BaseQuantity = s.StockQuantity.Value * s.ConversionQty.Value,
                                                                 BaseUnitId = s.BaseUnitID.Value,
                                                                 Lot = s.ProductLot
                                                             } into g
                                                             group g by new
                                                             {
                                                                 g.ProductId,
                                                                 g.DispatchCode,
                                                                 g.PONo,
                                                                 g.UnitId,
                                                                 g.BaseUnitId,
                                                                 g.Lot
                                                             } into x
                                                             select new InterfaceDispatchModel
                                                             {
                                                                 ProductId = x.Key.ProductId,
                                                                 DispatchCode = x.Key.DispatchCode,
                                                                 PONo = x.Key.PONo,
                                                                 Quantity = x.Sum(s => s.Quantity),
                                                                 UnitId = x.Key.UnitId,
                                                                 BaseQuantity = x.Sum(s => s.BaseQuantity),
                                                                 BaseUnitId = x.Key.BaseUnitId,
                                                                 Lot = x.Key.Lot
                                                             }
                                    ).ToList();

                    AddInterfaceDispatch(itfmodel);
                    //#####  Interface Log ######


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

        public bool OnApproveDispatchInternal(string dispatchcode, string pono, string refcode, DateTime approvedispatchdate)
        {
            //validation
            Dispatch _checkdispatch = FirstOrDefault(x => x.DispatchStatus != DispatchStatusEnum.Close && x.DispatchCode == dispatchcode && x.Pono == pono);
            if (_checkdispatch != null)
            {
                if (_checkdispatch.DispatchStatus == DispatchStatusEnum.Complete)
                {
                    throw new HILIException("Data Is Completed!!");
                }
            }
            var _datadisptch = (from _dispatch in Where(x => x.IsActive)
                                where _dispatch.DispatchStatus != DispatchStatusEnum.Close
                                && _dispatch.DispatchCode == dispatchcode
                                && _dispatch.Pono.ToLower() == pono.ToLower()
                                select new
                                {
                                    dispatchid = _dispatch.DispatchId,
                                    dispatchcode = _dispatch.DispatchCode,
                                    documenttypeid = _dispatch.DocumentId
                                }
                    ).FirstOrDefault();

            Guid _dispatchid = _datadisptch.dispatchid;
            string _dispatchcode = _datadisptch.dispatchcode;
            Guid? _documenttypeid = _datadisptch.documenttypeid;

            var booking_result = (from _dispatch in Where(x => x.IsActive)
                                  join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                  join _booking in DispatchBookingService.Where(x => x.BookingStatus == BookingStatusEnum.InternalReceive) on _dispatchdetail.DispatchDetailId equals _booking.DispatchDetailId
                                  join _stock_info in StockInfoService.Where(x => x.IsActive) on _booking.ProductId equals _stock_info.ProductID
                                  join _stock_balance in StockBalanceService.Where(x => x.IsActive) on _stock_info.StockInfoID equals _stock_balance.StockInfoID
                                  where _stock_info.ProductStatusID == _dispatchdetail.ProductStatusId
                                  && _stock_info.Lot == _booking.ProductLot
                                  && _stock_info.StockUnitID == _booking.BookingStockUnitId
                                  && _stock_info.BaseUnitID == _booking.BookingBaseUnitId
                                  && _stock_info.ManufacturingDate == _booking.Mfgdate
                                  && _stock_info.ExpirationDate == _booking.ExpirationDate
                                  && _dispatch.DispatchStatus !=DispatchStatusEnum.Close
                                  && _dispatch.DispatchCode == dispatchcode && _dispatch.Pono.ToLower() == pono.ToLower()
                                  select new
                                  {
                                      StockInfoId = _stock_info.StockInfoID,
                                      StockBalanceId = _stock_balance.StockBalanceID,
                                      LocationId = _booking.LocationId,
                                      BookingId = _booking.BookingId,
                                      BookStockQuantity = _booking.BookingQty,
                                      BookBaseQuantity = _booking.BookingBaseQty,
                                      BookStockUnitId = _booking.BookingStockUnitId,
                                      BookBaseUnitId = _booking.BookingBaseUnitId,
                                      BookConversionQty = _booking.ConversionQty,
                                      DocumentTypeId = _dispatch.DocumentId,
                                      PalletCode = _booking.PalletCode
                                  }
                    ).ToList();
            List<DPStockBalance> _stockbalance = new List<DPStockBalance>();
            List<DPStockBalanceLocation> _stocklocation = new List<DPStockBalanceLocation>();
            List<PCPackingDetail> _pcdetail = new List<PCPackingDetail>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {         
                foreach (var item in booking_result)
                { 
                    Location datalocation = LocationService.FindByID(item.LocationId);
                    if (datalocation == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    Zone datazone = ZoneService.FindByID(datalocation.ZoneID);
                    if (datazone == null)
                    {
                        throw new HILIException("MSG00041");
                    }

                    StockLocationBalance datastocklocationbalance = StockLocationBalanceService.FirstOrDefault(x => x.ZoneID == datazone.ZoneID
                    && x.WarehouseID == datazone.WarehouseID
                    && x.StockBalanceID == item.StockBalanceId);
                    if (datastocklocationbalance == null)
                    {
                        throw new HILIException("MSG00042");
                    }

                    DPStockBalance _returnst = new DPStockBalance
                    {
                        StockInfoId = item.StockInfoId,
                        BookingId = item.BookingId,
                        BookStockQuantity = item.BookStockQuantity,
                        BookBaseQuantity = item.BookStockQuantity * item.BookBaseQuantity.GetValueOrDefault(),
                        BookReserveQuantity = item.BookStockQuantity
                    };
                    _stockbalance.Add(_returnst); 

                    DPStockBalanceLocation _returnstl = new DPStockBalanceLocation
                    {
                        StockLocationId = datastocklocationbalance.StockLocationID,
                        BookingId = item.BookingId,
                        BookStockQuantity = item.BookStockQuantity,
                        BookBaseQuantity = item.BookBaseQuantity.Value,
                        BookReserveQuantity = item.BookStockQuantity
                    };
                    _stocklocation.Add(_returnstl);

                    PCPackingDetail _returnpcdt = new PCPackingDetail
                    {
                        PalletCode = item.PalletCode,
                        BookStockQuantity = item.BookStockQuantity,
                        BookBaseQuantity = item.BookBaseQuantity.Value,
                        BookReserveQuantity = item.BookStockQuantity
                    };
                    _pcdetail.Add(_returnpcdt);


                    //Stock Trans
                    //Insert Stock Transaction
                    StockTransaction _stocktrans = new StockTransaction
                    {
                        StockTransactionID = Guid.NewGuid(),
                        DocumentCode = _dispatchcode,
                        DocumentID = _dispatchid,
                        PackageID = null,
                        StockTransType = StockTransactionTypeEnum.Outgoing,
                        LocationID = item.LocationId,
                        BaseQuantity = item.BookBaseQuantity.Value,
                        ConversionQty = item.BookConversionQty.Value,
                        Remark = "ApproveDispatchInternal",
                        StockLocationID = datastocklocationbalance.StockLocationID,
                        DocumentTypeID = _documenttypeid,
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        UserCreated = UserID,
                        DateModified = DateTime.Now,
                        UserModified = UserID
                    };
                    StockTransactionService.Add(_stocktrans);
                }

                var res = (from s in _stockbalance
                           select new
                           {
                               StockInfoId = s.StockInfoId,
                               BookStockQuantity = s.BookStockQuantity,
                               BookBaseQuantity = s.BookBaseQuantity,
                               BookReserveQuantity = s.BookReserveQuantity
                           }).ToList();
                var stockbalance = (from t in res
                                    group t by new
                                    {
                                        t.StockInfoId
                                    } into x
                                    select new
                                    {
                                        StockInfoId = x.Key.StockInfoId,
                                        BookStockQuantity = x.Sum(s => s.BookStockQuantity),
                                        BookBaseQuantity = x.Sum(s => s.BookBaseQuantity),
                                        BookReserveQuantity = x.Sum(s => s.BookReserveQuantity)
                                    } ).ToList();

                stockbalance.ForEach(itemst =>
                {
                    StockBalance usb = StockBalanceService.FirstOrDefault(x => x.StockInfoID == itemst.StockInfoId);
                    if (usb == null)
                    {
                        throw new HILIException("MSG00038");
                    }

                    usb.StockQuantity = (usb.StockQuantity - itemst.BookStockQuantity >= 0 ? usb.StockQuantity - itemst.BookStockQuantity : 0);
                    usb.BaseQuantity = (usb.BaseQuantity - itemst.BookBaseQuantity >= 0 ? usb.BaseQuantity - itemst.BookBaseQuantity : 0);
                    usb.ReserveQuantity = (usb.ReserveQuantity - itemst.BookReserveQuantity >= 0 ? usb.ReserveQuantity - itemst.BookReserveQuantity : 0);
                    usb.DateModified = DateTime.Now;
                    usb.UserModified = UserID;
                    StockBalanceService.Modify(usb);
                });

                //StockLocation
                var lo_res = (from s in _stocklocation
                              select new
                              {
                                  StockLocationId = s.StockLocationId,
                                  BookStockQuantity = s.BookStockQuantity,
                                  BookBaseQuantity = s.BookBaseQuantity,
                                  BookReserveQuantity = s.BookReserveQuantity
                              }).ToList();
                var stocklocation = (from t in lo_res
                                     group t by new
                                     {
                                         t.StockLocationId
                                     } into x
                                     select new
                                     {
                                         StockLocationId = x.Key.StockLocationId,
                                         BookStockQuantity = x.Sum(s => s.BookStockQuantity),
                                         BookBaseQuantity = x.Sum(s => s.BookBaseQuantity),
                                         BookReserveQuantity = x.Sum(s => s.BookReserveQuantity)
                                     }).ToList();

                stocklocation.ForEach(itemst =>
                {
                    StockLocationBalance usl = StockLocationBalanceService.FirstOrDefault(x => x.StockLocationID == itemst.StockLocationId);
                    if (usl == null)
                    {
                        throw new HILIException("MSG00042");
                    }
                    usl.StockQuantity = (usl.StockQuantity - itemst.BookStockQuantity >= 0 ? usl.StockQuantity - itemst.BookStockQuantity : 0);
                    usl.BaseQuantity = (usl.BaseQuantity - itemst.BookBaseQuantity >= 0 ? usl.BaseQuantity - itemst.BookBaseQuantity : 0);
                    usl.ReserveQuantity = (usl.ReserveQuantity - itemst.BookReserveQuantity >= 0 ? usl.ReserveQuantity - itemst.BookReserveQuantity : 0);
                    usl.DateModified = DateTime.Now;
                    usl.UserModified = UserID;
                    StockLocationBalanceService.Modify(usl);
                });

                if (refcode == "412")// (RefCode=111):Case จาก Internal Receive จะไม่มีไป Reserve ค่าที่ Production Control Detail , (RefCode=412):Case Marketing มีไป Reserve
                {
                    var prod_res = (from s in _pcdetail
                                    select new
                                    {
                                        PalletCode = s.PalletCode,
                                        StockQuantity = s.StockQuantity,
                                        BaseQuantity = s.BaseQuantity,
                                        ReserveQuantity = s.ReserveQuantity,
                                        BookStockQuantity = s.BookStockQuantity,
                                        BookBaseQuantity = s.BookBaseQuantity,
                                        BookReserveQuantity = s.BookReserveQuantity
                                    }).ToList();

                    var pccontroldetail = (from t in prod_res
                                           group t by new
                                           {
                                               t.PalletCode
                                           } into x
                                           select new
                                           {
                                               PalletCode = x.Key.PalletCode,
                                               StockQuantity = x.Sum(s => s.StockQuantity),
                                               BaseQuantity = x.Sum(s => s.BaseQuantity),
                                               ReserveQuantity = x.Sum(s => s.ReserveQuantity),
                                               BookStockQuantity = x.Sum(s => s.BookStockQuantity),
                                               BookBaseQuantity = x.Sum(s => s.BookBaseQuantity),
                                               BookReserveQuantity = x.Sum(s => s.BookReserveQuantity)
                                           }).ToList();

                    pccontroldetail.ForEach(itemst =>
                    {
                        ProductionControlDetail pcdt = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == itemst.PalletCode);
                        if (pcdt == null)
                        {
                            throw new HILIException("MSG00038");
                        }

                        pcdt.ReserveQTY = (pcdt.ReserveQTY - itemst.BookStockQuantity >= 0 ? pcdt.ReserveQTY - itemst.BookStockQuantity : 0);
                        pcdt.ReserveBaseQTY = (pcdt.ReserveBaseQTY - itemst.BookBaseQuantity >= 0 ? pcdt.ReserveBaseQTY - itemst.BookBaseQuantity : 0);
                        pcdt.RemainQTY = (pcdt.RemainQTY - itemst.BookStockQuantity >= 0 ? pcdt.RemainQTY - itemst.BookStockQuantity : 0);
                        pcdt.RemainBaseQTY = (pcdt.RemainBaseQTY - itemst.BookBaseQuantity >= 0 ? pcdt.RemainBaseQTY - itemst.BookBaseQuantity : 0);
                        pcdt.DateModified = DateTime.Now;
                        pcdt.UserModified = UserID;

                        if (pcdt.RemainQTY == 0)
                        {
                            pcdt.PackingStatus = PackingStatusEnum.Delivery;
                        }
                        ProductionControlDetailService.Modify(pcdt);
                    });
                }


                Dispatch updatedbhd = FirstOrDefault(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close && x.Pono == pono);
                if (updatedbhd == null)
                {
                    throw new HILIException("MSG00053");
                }

                List<DispatchDetail> updatedbdt = DispatchDetailService.Where(x => x.IsActive && x.DispatchId == updatedbhd.DispatchId).ToList();

                updatedbdt.ForEach(itemdt =>
                {
                    itemdt.DispatchDetailStatus = DispatchDetailStatusEnum.Complete;
                    itemdt.DateModified = DateTime.Now;
                    itemdt.UserModified = UserID;
                    DispatchDetailService.Modify(itemdt);
                }); 
                TimeSpan ts = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);
                updatedbhd.DocumentApproveDate = approvedispatchdate.Add(ts);
                updatedbhd.DocumentApproveDate = approvedispatchdate;
                updatedbhd.DispatchStatus =DispatchStatusEnum.Complete;
                base.Modify(updatedbhd);
                //##### Interface Log ######
                var temp = (from s in booking_result
                            join bk in DispatchBookingService.Where(x => x.IsActive) on s.BookingId equals bk.BookingId
                            join dpdt in DispatchDetailService.Where(x => x.IsActive) on bk.DispatchDetailId equals dpdt.DispatchDetailId
                            join dthd in Where(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close) on dpdt.DispatchId equals dthd.DispatchId
                            where dpdt.DispatchId == updatedbhd.DispatchId
                            select new { bk, dthd, book = s, }).ToList();
                List<InterfaceDispatchModel> itfmodel = (from t in temp
                                                         select new
                                                         {
                                                             ProductId = t.bk.ProductId,
                                                             DispatchCode = t.dthd.DispatchCode,
                                                             PONo = t.dthd.Pono,
                                                             Quantity = t.bk.BookingQty,
                                                             UnitId = t.book.BookStockUnitId.GetValueOrDefault(),
                                                             BaseQuantity = t.book.BookBaseQuantity.GetValueOrDefault(),
                                                             BaseUnitId = t.book.BookBaseUnitId.GetValueOrDefault(),
                                                             Lot = t.bk.ProductLot
                                                         } into g
                                                         group g by new
                                                         {
                                                             g.ProductId,
                                                             g.DispatchCode,
                                                             g.PONo,
                                                             g.UnitId,
                                                             g.BaseUnitId,
                                                             g.Lot
                                                         } into x
                                                         select new InterfaceDispatchModel
                                                         {
                                                             ProductId = x.Key.ProductId,
                                                             DispatchCode = x.Key.DispatchCode,
                                                             PONo = x.Key.PONo,
                                                             Quantity = x.Sum(s => s.Quantity),
                                                             UnitId = x.Key.UnitId,
                                                             BaseQuantity = x.Sum(s => s.BaseQuantity),
                                                             BaseUnitId = x.Key.BaseUnitId,
                                                             Lot = x.Key.Lot
                                                         }
                                ).ToList();
                AddInterfaceDispatch(itfmodel);
                //#####  Interface Log ######
                scope.Complete();
                return true;
            }
        }
         
        public bool OnCancelAll(string dispatchcode, string pono, string revisereason)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(30)))
                {
                    Dispatch dispatch = Query().Filter(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower()).Get().FirstOrDefault();
                    if (dispatch == null)
                    {
                        throw new HILIException("MSG00053");
                    }
                    if (dispatch.DispatchStatus ==DispatchStatusEnum.Complete)
                    {
                        throw new HILIException("MSG00094");
                    }
                    if (dispatch.DispatchStatus == DispatchStatusEnum.Close)
                    {
                        throw new HILIException("MSG00094");
                    }
                    List<StockInOutModel> stockRestore = new List<StockInOutModel>();

                    List<DispatchBooking> bookings = DispatchBookingService.Where(x => x.IsActive && x.DispatchDetails.DispatchId == dispatch.DispatchId && x.IsBackOrder == false).ToList();
                    var approve_pick = PickingService.Any(x => x.IsActive && x.PONo.ToLower() == pono.ToLower() && x.PickingStatus == PickingStatusEnum.Pick);
                    //จอง,ยืนยันจอง,ยืนยันจอง QA,ยืนยันการจ่ายแบบรับภายใน
                    if ((dispatch.DispatchStatus == DispatchStatusEnum.Inprogress
                        || dispatch.DispatchStatus == DispatchStatusEnum.InprogressConfirmQA
                        || dispatch.DispatchStatus == DispatchStatusEnum.InternalReceive)
                        || (!approve_pick && dispatch.DispatchStatus == DispatchStatusEnum.InprogressConfirm))
                    {
                        Guid receiveID = Guid.Empty;
                        //ยืนยันการจ่ายแบบรับภายใน ปรับให้ receive gen dispatch ได้
                        if (dispatch.DispatchStatus ==DispatchStatusEnum.InternalReceive)
                        {
                            Receive receive = receiveService.FirstOrDefault(x => x.IsActive && x.ReceiveCode == dispatch.Pono.Replace("RCV", ""));
                            if (receive != null)
                            {
                                var reciveModify = receiveService.FindByID(receive.ReceiveID);
                                receiveID = receive.ReceiveID;
                                reciveModify.ReceiveStatus = ReceiveStatusEnum.Complete;
                                reciveModify.DateModified = DateTime.Now;
                                reciveModify.UserModified = UserID;
                                receiveService.Modify(reciveModify);
                            }
                        }

                        bookings.ForEach(booking =>
                        {
                            Receiving receiving = null;
                            if (!string.IsNullOrEmpty(booking.PalletCode))
                            {
                                receiving = receivingService.FirstOrDefault(x => x.PalletCode == booking.PalletCode);
                            }
                            else
                            {
                                ReceiveDetail receive_detail = receiveDetailService.FirstOrDefault(x => x.IsActive && x.ReceiveID == receiveID 
                                && x.ProductID == booking.ProductId
                                && x.StockUnitID == booking.BookingStockUnitId);
                                if (receive_detail != null)
                                {
                                    receiving = receivingService.FirstOrDefault(x => x.ReceiveDetailID == receive_detail.ReceiveDetailID);
                                }
                            }

                            ProductionControlDetail pc_detail = new ProductionControlDetail();
                            ProductionControlDetail tmp_pc_detail = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == booking.PalletCode);
                            if (tmp_pc_detail != null)
                            {
                                pc_detail = ProductionControlDetailService.FindByID(tmp_pc_detail.PackingID);
                                pc_detail.ReserveQTY -= booking.BookingQty;
                                pc_detail.ReserveBaseQTY -= booking.BookingQty * pc_detail.ConversionQty;
                                pc_detail.DateModified = DateTime.Now;
                                pc_detail.UserModified = UserID;
                                ProductionControlDetailService.Modify(pc_detail);
                            }
                            Guid? _locationId = string.IsNullOrEmpty(booking.PalletCode) ? booking.LocationId : pc_detail.LocationID;
                            Location location = LocationService.FirstOrDefault(x => x.LocationID == _locationId);
                            //จองไม่ต้องคืน stock
                            if (dispatch.DispatchStatus != DispatchStatusEnum.Inprogress && receiving != null)
                            {
                                var strockOut = new StockInOutModel
                                {
                                    ProductID = booking.ProductId,
                                    StockUnitID = booking.BookingStockUnitId.Value,
                                    BaseUnitID = booking.BookingBaseUnitId.Value,
                                    Lot = booking.ProductLot,
                                    ProductOwnerID = receiving.ProductOwnerID.Value,
                                    SupplierID = receiving.SupplierID.Value,
                                    ManufacturingDate = booking.Mfgdate,
                                    ExpirationDate = booking.ExpirationDate.Value,
                                    ProductWidth = receiving.ProductWidth,
                                    ProductLength = receiving.ProductLength,
                                    ProductHeight = receiving.ProductHeight,
                                    ProductWeight = receiving.ProductWeight,
                                    PackageWeight = receiving.PackageWeight,
                                    Price = receiving.Price,
                                    ProductUnitPriceID = receiving.ProductUnitPriceID,
                                    ProductStatusID = receiving.ProductStatusID,//hold pallet
                                    ProductSubStatusID = receiving.ProductSubStatusID.Value,
                                    //Quantity = booking.StockQuantity.Value,//out normal
                                    ConversionQty = booking.ConversionQty.Value,
                                    PalletCode = booking.PalletCode,
                                    LocationCode = location.Code,
                                    DocumentCode = dispatch.DispatchCode,
                                    DocumentTypeID = dispatch.DocumentId,
                                    DocumentID = dispatch.DispatchId,
                                    StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                                    Remark = "Cancel Dispatch",
                                    ReserveQuantity = booking.BookingQty
                                };
                                stockRestore.Add(strockOut);
                            }
                        });
                    }
                    else
                    {
                        //check assing ว่า status 20 และ สลับ pallet มั๊ย และ ต้องดูค่า diff ของ booking กับ assign
                        List<PickSwap> picks_swap = GetPickSwapPallet(pono);
                        IEnumerable<string> pallets = picks_swap.Select(x => x.RefPalletCode);
                        RestoreBookingNotSwapPallet(dispatch, stockRestore, bookings, pallets);
                        RestoreBookingSwapPallet(dispatch, stockRestore, bookings, picks_swap, pallets);
                    }
                    stockService.UserID = UserID;
                    stockService.RestoreReserve(stockRestore, Context);
                    CloseDispatch(revisereason, dispatch);
                    CancelRegistruck(pono);
                    CancelPicking(pono);

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

        private void CloseDispatch(string revisereason, Dispatch dispatch)
        {
            #region [Close Dispatch]
            List<DispatchDetail> updatedbdt = DispatchDetailService.Where(x => x.DispatchId == dispatch.DispatchId && x.IsActive == true).ToList();
            updatedbdt.ForEach(itemdt =>
            {
                itemdt.DispatchDetailStatus = DispatchDetailStatusEnum.Close;
                itemdt.DateModified = DateTime.Now;
                itemdt.UserModified = UserID;
                DispatchDetailService.Modify(itemdt);
            });
            //this.ModifyDetail(updatedbdt);
            dispatch.DispatchDetailCollection = null;
            dispatch.DispatchStatus = DispatchStatusEnum.Close;
            dispatch.ReviseReason = revisereason;
            dispatch.ReviseDateTime = DateTime.Now;
            Modify(dispatch);
            #endregion  [Close Dispatch]
        }

        private void CancelRegistruck(string pono)
        {
            #region [Close Registruck]
            List<RegisterTruck> cancelregistruck = RegisterTruckService.Where(x => x.IsActive && x.PoNo == pono).ToList();

            if (cancelregistruck.Any())
            {
                cancelregistruck.ForEach(item =>
                {
                    item.ShippingStatus = (int)ShippingStatusEnum.Cancel;
                    item.DateModified = DateTime.Now;
                    item.UserModified = UserID;
                    RegisterTruckService.Modify(item);
                    List<RegisterTruckDetail> cancelregistruckDetail = RegisterTruckDetailService.Where(x => x.IsActive && x.ShippingID == item.ShippingID).ToList();

                    cancelregistruckDetail.ForEach(itemDetail =>
                    {
                        itemDetail.IsActive = false;
                        itemDetail.DateModified = DateTime.Now;
                        itemDetail.UserModified = UserID;
                        RegisterTruckDetailService.Modify(itemDetail);

                        RegisterTruckConsolidate cancelregistruckCosolidate = RegisterTruckConsolidateService.FirstOrDefault(x => x.IsActive && x.ShippingDetailID == itemDetail.ShippingDetailID);
                        if (cancelregistruckCosolidate != null)
                        {
                            cancelregistruckCosolidate.ConsolidateStatus = (int)ShippingStatusEnum.Cancel;
                            cancelregistruckCosolidate.DateModified = DateTime.Now;
                            cancelregistruckCosolidate.UserModified = UserID;
                            RegisterTruckConsolidateService.Modify(cancelregistruckCosolidate);
                        }
                    });
                });
            }
            #endregion  [Close Registruck]
        }

        private void CancelPicking(string pono)
        {
            #region [Close Pick]
            List<Picking> cancelpick = PickingService.Where(x => x.IsActive && x.PONo == pono).ToList();
            if (cancelpick.Any())
            {
                cancelpick.ForEach(item =>
                {
                    item.PickingStatus = PickingStatusEnum.Cancel;
                    item.DateModified = DateTime.Now;
                    item.UserModified = UserID;
                    PickingService.Modify(item);

                    List<PickingAssign> cancelpickassign = PickingAssignService.Where(x => x.IsActive && x.PickingID == item.PickingID).ToList();
                    cancelpickassign.ForEach(itemassign =>
                    {
                        itemassign.AssignStatus = PickingStatusEnum.Cancel;
                        itemassign.DateModified = DateTime.Now;
                        itemassign.UserModified = UserID;
                        PickingAssignService.Modify(itemassign);

                        List<PickingDetail> cancelpickDetail = PickingDetailService.Where(x => x.IsActive && x.AssignID == itemassign.AssignID).ToList();
                        cancelpickDetail.ForEach(itemDetail =>
                        {
                            itemDetail.PickingStatus = (int)PickingStatusEnum.Cancel;
                            itemDetail.DateModified = DateTime.Now;
                            itemDetail.UserModified = UserID;
                            PickingDetailService.Modify(itemDetail);
                        });
                    });
                });
            }
            #endregion  [Close Pick]
        }

        /// <summary>
        /// ต้องการ pallet ที่สลับกันเพื่อคืนให้ตรง Pallet
        /// </summary>
        /// <param name="dispatch"></param>
        /// <param name="stockRestore"></param>
        /// <param name="bookings"></param>
        /// <param name="picks_swap"></param>
        /// <param name="pallets"></param>
        private void RestoreBookingSwapPallet(Dispatch dispatch, List<StockInOutModel> stockRestore, List<DispatchBooking> bookings, List<PickSwap> picks_swap, IEnumerable<string> pallets)
        {
            List<DispatchBooking> booking_swap = bookings.Where(x => pallets.Contains(x.PalletCode)).ToList();

            #region หา pallet ที่สลับ
            booking_swap.ForEach(book =>
            {
                var pallet_swap = picks_swap.Where(x => x.RefPalletCode == book.PalletCode).ToList();
                decimal restore_book_qty = 0;

                pallet_swap.ForEach(pick_pallet =>
                {
                    restore_book_qty += pick_pallet.StockQuantity;
                    #region คืนตัวสลับ
                    ProductionControlDetail _pc_detail = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == pick_pallet.PalletCode);
                    ProductionControlDetail pc_detail = ProductionControlDetailService.FindByID(_pc_detail.PackingID);

                    pc_detail.ReserveQTY -= pick_pallet.StockQuantity;
                    pc_detail.ReserveBaseQTY -= pick_pallet.StockQuantity * pc_detail.ConversionQty;
                    pc_detail.DateModified = DateTime.Now;
                    pc_detail.UserModified = UserID;
                    ProductionControlDetailService.Modify(pc_detail);


                    Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == pick_pallet.PalletCode);
                    Location location = LocationService.FirstOrDefault(x => x.LocationID == pc_detail.LocationID);

                    stockRestore.Add(new StockInOutModel
                    {
                        ProductID = receiving.ProductID,
                        StockUnitID = receiving.StockUnitID,
                        BaseUnitID = receiving.BaseUnitID,
                        Lot = receiving.Lot,
                        ProductOwnerID = receiving.ProductOwnerID.Value,
                        SupplierID = receiving.SupplierID.Value,
                        ManufacturingDate = receiving.ManufacturingDate.Value,
                        ExpirationDate = receiving.ExpirationDate.Value,
                        ProductWidth = receiving.ProductWidth,
                        ProductLength = receiving.ProductLength,
                        ProductHeight = receiving.ProductHeight,
                        ProductWeight = receiving.ProductWeight,
                        PackageWeight = receiving.PackageWeight,
                        Price = receiving.Price,
                        ProductUnitPriceID = receiving.ProductUnitPriceID,
                        ProductStatusID = receiving.ProductStatusID,//hold pallet
                        ProductSubStatusID = receiving.ProductSubStatusID.Value,
                        //Quantity = booking.StockQuantity.Value,//out normal
                        ConversionQty = receiving.ConversionQty,
                        PalletCode = pick_pallet.PalletCode,
                        LocationCode = location.Code,
                        DocumentCode = dispatch.DispatchCode,
                        DocumentTypeID = dispatch.DocumentId,
                        DocumentID = dispatch.DispatchId,
                        StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                        Remark = "Cancel Dispatch",
                        ReserveQuantity = pick_pallet.StockQuantity
                    });

                    #endregion
                });


                if (book.BookingQty > restore_book_qty)
                {
                    #region คืนตัว book
                    decimal qty = book.BookingQty - restore_book_qty;
                    ProductionControlDetail _pc_detail = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == book.PalletCode);
                    ProductionControlDetail pc_detail = ProductionControlDetailService.FindByID(_pc_detail.PackingID);

                    pc_detail.ReserveQTY -= qty;
                    pc_detail.ReserveBaseQTY -= qty * pc_detail.ConversionQty;
                    pc_detail.DateModified = DateTime.Now;
                    pc_detail.UserModified = UserID;
                    ProductionControlDetailService.Modify(pc_detail);


                    Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == book.PalletCode);
                    Location location = LocationService.FirstOrDefault(x => x.LocationID == pc_detail.LocationID);

                    stockRestore.Add(new StockInOutModel
                    {
                        ProductID = book.ProductId,
                        StockUnitID = book.BookingStockUnitId.Value,
                        BaseUnitID = book.BookingBaseUnitId.Value,
                        Lot = book.ProductLot,
                        ProductOwnerID = receiving.ProductOwnerID.Value,
                        SupplierID = receiving.SupplierID.Value,
                        ManufacturingDate = book.Mfgdate,
                        ExpirationDate = book.ExpirationDate.Value,
                        ProductWidth = receiving.ProductWidth,
                        ProductLength = receiving.ProductLength,
                        ProductHeight = receiving.ProductHeight,
                        ProductWeight = receiving.ProductWeight,
                        PackageWeight = receiving.PackageWeight,
                        Price = receiving.Price,
                        ProductUnitPriceID = receiving.ProductUnitPriceID,
                        ProductStatusID = receiving.ProductStatusID,//hold pallet
                        ProductSubStatusID = receiving.ProductSubStatusID.Value,
                        //Quantity = booking.StockQuantity.Value,//out normal
                        ConversionQty = book.ConversionQty.Value,
                        PalletCode = book.PalletCode,
                        LocationCode = location.Code,
                        DocumentCode = dispatch.DispatchCode,
                        DocumentTypeID = dispatch.DocumentId,
                        DocumentID = dispatch.DispatchId,
                        StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                        Remark = "Cancel Dispatch",
                        ReserveQuantity = qty
                    });
                    #endregion
                }
            });
            #endregion
        }
        /// <summary>
        /// สามารถคืน Palletได้ตรงๆ
        /// </summary>
        /// <param name="dispatch"></param>
        /// <param name="stockRestore"></param>
        /// <param name="bookings"></param>
        /// <param name="pallets"></param>
        private void RestoreBookingNotSwapPallet(Dispatch dispatch, List<StockInOutModel> stockRestore, List<DispatchBooking> bookings, IEnumerable<string> pallets)
        {
            List<DispatchBooking> booking_not_swap = bookings.Where(x => !pallets.Contains(x.PalletCode)).ToList();

            #region คืน reserve ได้ตรงๆ 
            booking_not_swap.ForEach(not_swap =>
            {
                ProductionControlDetail _pc_detail = ProductionControlDetailService.FirstOrDefault(x => x.PalletCode == not_swap.PalletCode);
                ProductionControlDetail pc_detail = ProductionControlDetailService.FindByID(_pc_detail.PackingID);

                pc_detail.ReserveQTY = pc_detail.ReserveQTY - not_swap.BookingQty;
                pc_detail.ReserveBaseQTY = pc_detail.ReserveBaseQTY - (not_swap.BookingQty * pc_detail.ConversionQty);
                pc_detail.DateModified = DateTime.Now;
                pc_detail.UserModified = UserID;
                ProductionControlDetailService.Modify(pc_detail);

                Receiving receiving = receivingService.FirstOrDefault(x => x.PalletCode == not_swap.PalletCode);
                Location location = LocationService.FirstOrDefault(x => x.LocationID == pc_detail.LocationID);

                stockRestore.Add(new StockInOutModel
                {
                    ProductID = not_swap.ProductId,
                    StockUnitID = not_swap.BookingStockUnitId.GetValueOrDefault(),
                    BaseUnitID = not_swap.BookingBaseUnitId.GetValueOrDefault(),
                    Lot = not_swap.ProductLot,
                    ProductOwnerID = receiving.ProductOwnerID.GetValueOrDefault(),
                    SupplierID = receiving.SupplierID.GetValueOrDefault(),
                    ManufacturingDate = not_swap.Mfgdate,
                    ExpirationDate = not_swap.ExpirationDate.GetValueOrDefault(),
                    ProductWidth = receiving.ProductWidth,
                    ProductLength = receiving.ProductLength,
                    ProductHeight = receiving.ProductHeight,
                    ProductWeight = receiving.ProductWeight,
                    PackageWeight = receiving.PackageWeight,
                    Price = receiving.Price,
                    ProductUnitPriceID = receiving.ProductUnitPriceID,
                    ProductStatusID = receiving.ProductStatusID,//hold pallet
                    ProductSubStatusID = receiving.ProductSubStatusID.Value,
                    //Quantity = booking.StockQuantity.Value,//out normal
                    ConversionQty = not_swap.ConversionQty.GetValueOrDefault(),
                    PalletCode = not_swap.PalletCode,
                    LocationCode = location.Code,
                    DocumentCode = dispatch.DispatchCode,
                    DocumentTypeID = dispatch.DocumentId,
                    DocumentID = dispatch.DispatchId,
                    StockTransTypeEnum = StockTransactionTypeEnum.CancelReserve,
                    Remark = "Cancel Dispatch",
                    ReserveQuantity = not_swap.BookingQty
                });
            });
            #endregion
        }

        private List<PickSwap> GetPickSwapPallet(string pono)
        {
            var tmps = (from _pick in PickingService.Where(x => x.IsActive && x.PONo.ToLower() == pono.ToLower())
                        join _pick_assign in PickingAssignService.Where(x => x.IsActive && (x.AssignStatus !=  PickingStatusEnum.WaitingPick || x.AssignStatus !=  PickingStatusEnum.Cancel)
                                     && x.PalletCode != x.RefPalletCode) on _pick.PickingID equals _pick_assign.PickingID
                        select _pick_assign).ToList();

            var picks_swap = (from tmp in tmps
                              select new
                              {
                                  PalletCode = tmp.PalletCode,
                                  RefPalletCode = tmp.RefPalletCode,
                                  StockQuantity = tmp.StockQuantity,
                                  LocationID = tmp.SuggestionLocationID
                              } into g
                              group g by new
                              {
                                  g.PalletCode,
                                  g.RefPalletCode,
                                  g.LocationID
                              } into x
                              select new PickSwap
                              {
                                  PalletCode = x.Key.PalletCode,
                                  RefPalletCode = x.Key.RefPalletCode,
                                  LocationID = x.Key.LocationID.GetValueOrDefault(),
                                  StockQuantity = x.Sum(s => s.StockQuantity.GetValueOrDefault())
                              }).ToList();
            return picks_swap;
        }
        public bool OnCancelDispatchComplete(string pono, string type)
        {
            try
            {
                SqlParameter param = new SqlParameter("@pono", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param2 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };

                bool isSuccess = true;

                if (type == "0")
                {
                    isSuccess = Context.SQLQuery<bool>("exec SP_OnCancelDispatchComplete @pono, @UserID", param, param2).SingleOrDefault();
                }
                else
                {
                    isSuccess = Context.SQLQuery<bool>("exec SP_OnCancelTransferMK @pono, @UserID", param, param2).SingleOrDefault();
                }

                if (!isSuccess)
                {
                    throw new HILIException("MSG00006");
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
        #endregion

        #region Stock
        public List<ProductModel> GetProductStock(string keyword, string orderno, Guid producttsatusId, string refcode, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                var param1 = new SqlParameter("@OrderNo", SqlDbType.NVarChar) { Value = orderno == null ? "" : orderno };
                var param2 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode == null ? "" : refcode };
                var param3 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = keyword == null ? "" : keyword };
                var param4 = new SqlParameter("@producttsatusId", SqlDbType.UniqueIdentifier) { Value = producttsatusId };
                var param5 = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageIndex };
                var param6 = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                var productResult = unitofwork.SQLQuery<ProductModel>("exec SP_GetProductStock @OrderNo , @refcode, @keyword, @producttsatusId, @PageNumber ,@PageSize ", param1, param2, param3, param4, param5, param6).ToList();
                var param11 = new SqlParameter("@OrderNo", SqlDbType.NVarChar) { Value = orderno == null ? "" : orderno };
                var param22 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode == null ? "" : refcode };
                var param33 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = keyword == null ? "" : keyword };
                var param44 = new SqlParameter("@producttsatusId", SqlDbType.UniqueIdentifier) { Value = producttsatusId };
                var param55 = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageIndex };
                var param66 = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                totalRecords = unitofwork.SQLQuery<int>("exec SP_GetProductStockCount @OrderNo , @refcode, @keyword, @producttsatusId, @PageNumber ,@PageSize ", param11, param22, param33, param44, param55, param66).FirstOrDefault();
                return productResult;
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
        public List<ProductModel> GetProductNoneStock(string keyword, Guid producttsatusId, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                Guid _productownerid = ProductOwnerService.Query().Get().FirstOrDefault().ProductOwnerID;

                var result = (from _proudct in ProductService.Query().Filter(x => x.IsActive).Get()
                              join _productcode in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get() on _proudct.ProductID equals _productcode.ProductID
                              join _pc2 in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Commercial).Get() on _proudct.ProductID equals _pc2.ProductID into pc2
                              from _productcode2 in pc2.DefaultIfEmpty()
                              join _pb in ProductBrandService.Query().Get() on _proudct.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Get() on _proudct.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _unit in ProductUnitService.Query().Get() on _proudct.ProductID equals _unit.ProductID
                              join _unitbase in ProductUnitService.Query().Filter(x => x.IsBaseUOM).Get() on _proudct.ProductID equals _unitbase.ProductID
                              where (_proudct.Name.Contains(keyword) || _productcode.Code.Contains(keyword) || _unit.Name.Contains(keyword) || (_productcode2 != null ? _productcode2.Code.Contains(keyword) : _productcode.Code.Contains(keyword)))
                              select new
                              {
                                  ProductID = _proudct.ProductID,
                                  ProductGroupLevel3ID = _proudct.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = _proudct.ProductGroupLevel3?.Name,
                                  ProductCode = _productcode.Code,
                                  ProductName = _proudct.Name,
                                  Description = _proudct.Description,
                                  ProductBrandID = _prodcutbrand?.ProductBrandID,
                                  ProductBrandName = _prodcutbrand?.Name,
                                  ProductShapeID = _prodcutshap?.ProductShapeID,
                                  ProductShapeName = _prodcutshap?.Name,
                                  Age = _proudct.Age,
                                  IsActive = _proudct.IsActive,
                                  ProductUnitID = _unit.ProductUnitID,
                                  ProductUnitName = _unit.Name,
                                  PriceUnitId = Guid.Empty,
                                  PriceUnitName = "",
                                  Price = 0,
                                  Quantity = 0,
                                  BaseQuantity = 0,
                                  BaseUnitId = _unitbase.ProductUnitID,
                                  ConversionQty = _unit.Quantity,
                                  ProductOwnerId = _productownerid,
                                  ProductOwnerName = "",
                                  ProductWidth = 0,
                                  ProductLength = 0,
                                  ProductHeight = 0,
                                  ProductWeight = 0,
                                  PackageWeight = 0,
                                  ProductStatusId = Guid.Empty,
                                  ProductStatusName = "",
                                  ProductSubStatusId = Guid.Empty,
                                  ProductSubStatusName = ""
                              } into g
                              group g by new
                              {
                                  g.ProductID,
                                  g.ProductGroupLevel3ID,
                                  g.ProductGroupLevel3Name,
                                  g.ProductCode,
                                  g.ProductName,
                                  g.Description,
                                  g.ProductBrandID,
                                  g.ProductBrandName,
                                  g.ProductShapeID,
                                  g.ProductShapeName,
                                  g.Age,
                                  g.IsActive,
                                  g.ProductUnitID,
                                  g.ProductUnitName,
                                  g.PriceUnitId,
                                  g.PriceUnitName,
                                  g.Price,
                                  g.BaseUnitId,
                                  g.ConversionQty,
                                  g.ProductOwnerId,
                                  g.ProductOwnerName,
                                  g.ProductWidth,
                                  g.ProductLength,
                                  g.ProductHeight,
                                  g.ProductWeight,
                                  g.PackageWeight,
                                  g.ProductStatusId,
                                  g.ProductStatusName,
                                  g.ProductSubStatusId,
                                  g.ProductSubStatusName
                              } into g2
                              select new
                              {
                                  ProductID = g2.Key.ProductID,
                                  ProductGroupLevel3ID = g2.Key.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = g2.Key.ProductGroupLevel3Name,
                                  ProductCode = g2.Key.ProductCode,
                                  ProductName = g2.Key.ProductName,
                                  Description = g2.Key.Description,
                                  ProductBrandID = g2.Key.ProductBrandID,
                                  ProductBrandName = g2.Key.ProductBrandName,
                                  ProductShapeID = g2.Key.ProductShapeID,
                                  ProductShapeName = g2.Key.ProductShapeName,
                                  Age = g2.Key.Age,
                                  IsActive = g2.Key.IsActive,
                                  ProductUnitID = g2.Key.ProductUnitID,
                                  ProductUnitName = g2.Key.ProductUnitName,
                                  PriceUnitId = g2.Key.PriceUnitId,
                                  PriceUnitName = g2.Key.PriceUnitName,
                                  Price = g2.Key.Price,
                                  BaseUnitId = g2.Key.BaseUnitId,
                                  ConversionQty = g2.Key.ConversionQty,
                                  ProductOwnerId = g2.Key.ProductOwnerId,
                                  ProductOwnerName = g2.Key.ProductOwnerName,
                                  ProductWidth = g2.Key.ProductWidth,
                                  ProductLength = g2.Key.ProductLength,
                                  ProductHeight = g2.Key.ProductHeight,
                                  ProductWeight = g2.Key.ProductWeight,
                                  PackageWeight = g2.Key.PackageWeight,
                                  ProductStatusId = g2.Key.ProductStatusId,
                                  ProductStatusName = g2.Key.ProductStatusName,
                                  ProductSubStatusId = g2.Key.ProductSubStatusId,
                                  ProductSubStatusName = g2.Key.ProductSubStatusName,
                                  Quantity = g2.Sum(s => s.Quantity),
                                  BaseQuantity = g2.Sum(s => s.BaseQuantity)
                              });
                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.ProductCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var productResult = result.Select(n => new ProductModel
                {
                    ProductID = n.ProductID,
                    ProductGroupLevel3ID = n.ProductGroupLevel3ID,
                    ProductGroupLevel3Name = n.ProductGroupLevel3Name,
                    ProductCode = n.ProductCode,
                    ProductName = n.ProductName,
                    Description = n.Description,
                    ProductBrandID = n.ProductBrandID,
                    ProductBrandName = n.ProductBrandName,
                    ProductShapeID = n.ProductShapeID,
                    ProductShapeName = n.ProductShapeName,
                    Age = n.Age,
                    IsActive = n.IsActive,
                    ProductUnitID = n.ProductUnitID,
                    ProductUnitName = n.ProductUnitName,
                    PriceUnitId = n.PriceUnitId,
                    PriceUnitName = n.PriceUnitName,
                    Price = n.Price,
                    BaseUnitId = n.BaseUnitId,
                    ConversionQty = n.ConversionQty,
                    ProductOwnerId = n.ProductOwnerId,
                    ProductOwnerName = n.ProductOwnerName,
                    ProductWidth = n.ProductWidth,
                    ProductLength = n.ProductLength,
                    ProductHeight = n.ProductHeight,
                    ProductWeight = n.ProductWeight,
                    PackageWeight = n.PackageWeight,
                    ProductStatusId = n.ProductStatusId,
                    ProductStatusName = n.ProductStatusName,
                    ProductSubStatusId = n.ProductSubStatusId,
                    ProductSubStatusName = n.ProductSubStatusName,
                    Quantity = n.Quantity,
                    BaseQuantity = n.BaseQuantity
                }).ToList();


                return productResult;
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
        public List<ProductModel> GetProductStockByCode(string productcode, string orderno, Guid producttsatusId, string refcode, out int totalRecords)
        {
            try
            {

                productcode = (string.IsNullOrEmpty(productcode) ? "" : productcode);
                var param11 = new SqlParameter("@OrderNo", SqlDbType.NVarChar) { Value = orderno == null ? "" : orderno };
                var param22 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode == null ? "" : refcode };
                var param33 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = productcode == null ? "" : productcode };
                var param44 = new SqlParameter("@producttsatusId", SqlDbType.UniqueIdentifier) { Value = producttsatusId };
                var param55 = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 };
                var param66 = new SqlParameter("@PageSize", SqlDbType.Int) { Value = 20 };
                totalRecords = unitofwork.SQLQuery<int>("exec SP_GetProductStockCount @OrderNo , @refcode, @keyword, @producttsatusId, @PageNumber ,@PageSize ", param11, param22, param33, param44, param55, param66).FirstOrDefault();
                if (totalRecords > 0)
                {
                    var param1 = new SqlParameter("@OrderNo", SqlDbType.NVarChar) { Value = orderno == null ? "" : orderno };
                    var param2 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode == null ? "" : refcode };
                    var param3 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = productcode == null ? "" : productcode };
                    var param4 = new SqlParameter("@producttsatusId", SqlDbType.UniqueIdentifier) { Value = producttsatusId };
                    var param5 = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 };
                    var param6 = new SqlParameter("@PageSize", SqlDbType.Int) { Value = 20 };
                    var productResult = unitofwork.SQLQuery<ProductModel>("exec SP_GetProductStock @OrderNo , @refcode, @keyword, @producttsatusId, @PageNumber ,@PageSize ", param1, param2, param3, param4, param5, param6).ToList();
                    return productResult;
                }
                else
                {
                    return new List<ProductModel>();
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
        public List<ProductModel> GetProductNoneStockByCode(string productcode, out int totalRecords)
        {
            try
            {

                productcode = (string.IsNullOrEmpty(productcode) ? "" : productcode);

                Guid _productownerid = ProductOwnerService.Query().Get().FirstOrDefault().ProductOwnerID;

                var result = (from _proudct in ProductService.Query().Filter(x => x.IsActive).Get()
                              join _productcode in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get() on _proudct.ProductID equals _productcode.ProductID
                              join _pc2 in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Commercial).Get() on _proudct.ProductID equals _pc2.ProductID into pc2
                              from _productcode2 in pc2.DefaultIfEmpty()
                              join _pb in ProductBrandService.Query().Get() on _proudct.ProductBrandID equals _pb.ProductBrandID into pb
                              from _prodcutbrand in pb.DefaultIfEmpty()
                              join _ps in ProductShapeService.Query().Get() on _proudct.ProductShapeID equals _ps.ProductShapeID into ps
                              from _prodcutshap in ps.DefaultIfEmpty()
                              join _unit in ProductUnitService.Query().Get() on _proudct.ProductID equals _unit.ProductID
                              join _unitbase in ProductUnitService.Query().Filter(x => x.IsBaseUOM).Get() on _proudct.ProductID equals _unitbase.ProductID
                              where _productcode.Code.Contains(productcode) || (_productcode2 != null ? _productcode2.Code.Contains(productcode) : _productcode.Code.Contains(productcode))
                              select new
                              {
                                  ProductID = _proudct.ProductID,
                                  ProductGroupLevel3ID = _proudct.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = _proudct.ProductGroupLevel3?.Name,
                                  ProductCode = _productcode.Code,
                                  ProductName = _proudct.Name,
                                  Description = _proudct.Description,
                                  ProductBrandID = _prodcutbrand?.ProductBrandID,
                                  ProductBrandName = _prodcutbrand?.Name,
                                  ProductShapeID = _prodcutshap?.ProductShapeID,
                                  ProductShapeName = _prodcutshap?.Name,
                                  Age = _proudct.Age,
                                  IsActive = _proudct.IsActive,
                                  ProductUnitID = _unit.ProductUnitID,
                                  ProductUnitName = _unit.Name,
                                  PriceUnitId = Guid.Empty,
                                  PriceUnitName = "",
                                  Price = 0,
                                  Quantity = 0,
                                  BaseQuantity = 0,
                                  BaseUnitId = _unitbase.ProductUnitID,
                                  ConversionQty = _unit.Quantity,
                                  ProductOwnerId = _productownerid,
                                  ProductOwnerName = "",
                                  ProductWidth = 0,
                                  ProductLength = 0,
                                  ProductHeight = 0,
                                  ProductWeight = 0,
                                  PackageWeight = 0,
                                  ProductStatusId = Guid.Empty,
                                  ProductStatusName = "",
                                  ProductSubStatusId = Guid.Empty,
                                  ProductSubStatusName = ""
                              } into g
                              group g by new
                              {
                                  g.ProductID,
                                  g.ProductGroupLevel3ID,
                                  g.ProductGroupLevel3Name,
                                  g.ProductCode,
                                  g.ProductName,
                                  g.Description,
                                  g.ProductBrandID,
                                  g.ProductBrandName,
                                  g.ProductShapeID,
                                  g.ProductShapeName,
                                  g.Age,
                                  g.IsActive,
                                  g.ProductUnitID,
                                  g.ProductUnitName,
                                  g.PriceUnitId,
                                  g.PriceUnitName,
                                  g.Price,
                                  g.BaseUnitId,
                                  g.ConversionQty,
                                  g.ProductOwnerId,
                                  g.ProductOwnerName,
                                  g.ProductWidth,
                                  g.ProductLength,
                                  g.ProductHeight,
                                  g.ProductWeight,
                                  g.PackageWeight,
                                  g.ProductStatusId,
                                  g.ProductStatusName,
                                  g.ProductSubStatusId,
                                  g.ProductSubStatusName
                              } into g2
                              select new
                              {
                                  ProductID = g2.Key.ProductID,
                                  ProductGroupLevel3ID = g2.Key.ProductGroupLevel3ID,
                                  ProductGroupLevel3Name = g2.Key.ProductGroupLevel3Name,
                                  ProductCode = g2.Key.ProductCode,
                                  ProductName = g2.Key.ProductName,
                                  Description = g2.Key.Description,
                                  ProductBrandID = g2.Key.ProductBrandID,
                                  ProductBrandName = g2.Key.ProductBrandName,
                                  ProductShapeID = g2.Key.ProductShapeID,
                                  ProductShapeName = g2.Key.ProductShapeName,
                                  Age = g2.Key.Age,
                                  IsActive = g2.Key.IsActive,
                                  ProductUnitID = g2.Key.ProductUnitID,
                                  ProductUnitName = g2.Key.ProductUnitName,
                                  PriceUnitId = g2.Key.PriceUnitId,
                                  PriceUnitName = g2.Key.PriceUnitName,
                                  Price = g2.Key.Price,
                                  BaseUnitId = g2.Key.BaseUnitId,
                                  ConversionQty = g2.Key.ConversionQty,
                                  ProductOwnerId = g2.Key.ProductOwnerId,
                                  ProductOwnerName = g2.Key.ProductOwnerName,
                                  ProductWidth = g2.Key.ProductWidth,
                                  ProductLength = g2.Key.ProductLength,
                                  ProductHeight = g2.Key.ProductHeight,
                                  ProductWeight = g2.Key.ProductWeight,
                                  PackageWeight = g2.Key.PackageWeight,
                                  ProductStatusId = g2.Key.ProductStatusId,
                                  ProductStatusName = g2.Key.ProductStatusName,
                                  ProductSubStatusId = g2.Key.ProductSubStatusId,
                                  ProductSubStatusName = g2.Key.ProductSubStatusName,
                                  Quantity = g2.Sum(s => s.Quantity),
                                  BaseQuantity = g2.Sum(s => s.BaseQuantity)
                              });

                totalRecords = result.Count();
                var productResult = result.Select(n => new ProductModel
                {
                    ProductID = n.ProductID,
                    ProductGroupLevel3ID = n.ProductGroupLevel3ID,
                    ProductGroupLevel3Name = n.ProductGroupLevel3Name,
                    ProductCode = n.ProductCode,
                    ProductName = n.ProductName,
                    Description = n.Description,
                    ProductBrandID = n.ProductBrandID,
                    ProductBrandName = n.ProductBrandName,
                    ProductShapeID = n.ProductShapeID,
                    ProductShapeName = n.ProductShapeName,
                    Age = n.Age,
                    IsActive = n.IsActive,
                    ProductUnitID = n.ProductUnitID,
                    ProductUnitName = n.ProductUnitName,
                    PriceUnitId = n.PriceUnitId,
                    PriceUnitName = n.PriceUnitName,
                    Price = n.Price,
                    BaseUnitId = n.BaseUnitId,
                    ConversionQty = n.ConversionQty,
                    ProductOwnerId = n.ProductOwnerId,
                    ProductOwnerName = n.ProductOwnerName,
                    ProductWidth = n.ProductWidth,
                    ProductLength = n.ProductLength,
                    ProductHeight = n.ProductHeight,
                    ProductWeight = n.ProductWeight,
                    PackageWeight = n.PackageWeight,
                    ProductStatusId = n.ProductStatusId,
                    ProductStatusName = n.ProductStatusName,
                    ProductSubStatusId = n.ProductSubStatusId,
                    ProductSubStatusName = n.ProductSubStatusName,
                    Quantity = n.Quantity,
                    BaseQuantity = n.BaseQuantity
                }).ToList();


                return productResult;
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
        public List<ProductModel> GetProductStockAllByCode(string productcode, string orderno, Guid producttsatusId, string refcode)
        {
            try
            {
                productcode = (string.IsNullOrEmpty(productcode) ? "" : productcode);

                List<ProductModel> _ret = new List<ProductModel>();
                int totalRecords;
                _ret = GetProductStockByCode(productcode, orderno, producttsatusId, refcode, out totalRecords);
                totalRecords = _ret.Count();

                if (_ret.Count > 0)
                {
                    return _ret;
                }
                else
                {
                    _ret = GetProductNoneStockByCode(productcode, out totalRecords);
                    totalRecords = _ret.Count();
                    return _ret;
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

        #region Booking

        public List<DPDetailItemBackOrder> OnValidateBookingByRule(string dispatchcode, string pono, string refcode)
        {
            //validation
            Dispatch _checkdispatch = FirstOrDefault(x => x.DispatchCode == dispatchcode && x.Pono == pono);
            if (_checkdispatch != null)
            {
                if (_checkdispatch.DispatchStatus ==DispatchStatusEnum.Inprogress)
                {
                    throw new HILIException("Data Is Booked!!");
                }
            }
            try
            {
                SqlParameter param = new SqlParameter("@dispatchcode", SqlDbType.NVarChar) { Value = dispatchcode };
                SqlParameter param2 = new SqlParameter("@pono", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param3 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode };
                SqlParameter param4 = new SqlParameter("@isValidate", SqlDbType.Bit) { Value = 1 };
                SqlParameter param5 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };
                List<DPDetailItemBackOrder> results = Context.SQLQuery<DPDetailItemBackOrder>("exec SP_CheckBookingRule @dispatchcode, @pono, @refcode, @isValidate, @UserID", param, param2, param3, param4, param5).ToList();
                    


                return results;

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

        public bool OnBookingByRule(string dispatchcode, string pono, string refcode)
        {
            try
            {
                Dispatch _checkdispatch = FirstOrDefault(x => x.DispatchCode == dispatchcode && x.Pono == pono);
                if (_checkdispatch != null && _checkdispatch.DispatchStatus == DispatchStatusEnum.Inprogress)
                {
                    throw new HILIException("Data Is Booked!!");
                }               

                SqlParameter param = new SqlParameter("@dispatchcode", SqlDbType.NVarChar) { Value = dispatchcode };
                SqlParameter param2 = new SqlParameter("@pono", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param3 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode };
                SqlParameter param4 = new SqlParameter("@isValidate", SqlDbType.Bit) { Value = 0 };
                SqlParameter param5 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };
                List<string> ret = Context.SQLQuery<string>("exec SP_CheckBookingRule @dispatchcode, @pono, @refcode, @isValidate, @UserID", param, param2, param3, param4, param5).ToList();

                if (ret[0] != "")
                {
                    throw new Exception(ret[0]);
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

        public List<PalletModel> GetPalletBooking(Guid DispatchID, Guid? WarehouseID, string product, string pallet, string Lot, string OrderNo, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                SqlParameter param1 = new SqlParameter("@DispatchID", SqlDbType.UniqueIdentifier) { Value = DispatchID };
                SqlParameter param2 = new SqlParameter("@warehouseId", SqlDbType.UniqueIdentifier) { Value = WarehouseID };
                SqlParameter param3 = new SqlParameter("@productCode", SqlDbType.NVarChar) { Value = product ?? "" };
                SqlParameter param4 = new SqlParameter("@palletCode", SqlDbType.NVarChar) { Value = pallet ?? "" };
                SqlParameter param5 = new SqlParameter("@Lot", SqlDbType.NVarChar) { Value = Lot ?? "" };
                SqlParameter param6 = new SqlParameter("@pageIndex", SqlDbType.Int) { Value = pageIndex.Value };
                SqlParameter param7 = new SqlParameter("@pageSize", SqlDbType.Int) { Value = pageSize.Value };
                SqlParameter param8 = new SqlParameter("@OrderNo", SqlDbType.NVarChar) { Value = OrderNo ?? "" };
                //var results = new List<DPDetailItemBackOrder>();

                List<PalletModel> ret = Context.SQLQuery<PalletModel>("exec SP_GetPalletBooking @DispatchID, @warehouseId, @productCode, @palletCode, @Lot, @OrderNo, @pageIndex, @pageSize", param1, param2, param3, param4, param5, param8, param6, param7).ToList();


                totalRecords = 0;

                if (ret.Count > 0)
                {
                    totalRecords = ret[0].TotalRecords;
                }

                return ret;

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
        
        public bool ManualBooking(Guid DispatchID, string pallets)
        {
            try
            {

                SqlParameter param = new SqlParameter("@DispatchID", SqlDbType.UniqueIdentifier) { Value = DispatchID };
                SqlParameter param2 = new SqlParameter("@pallets", SqlDbType.NVarChar) { Value = pallets };
                SqlParameter param3 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };
                //var results = new List<DPDetailItemBackOrder>();

                List<string> ret = Context.SQLQuery<string>("exec SP_BookingManual @DispatchID, @pallets , @UserID", param, param2, param3).ToList();

                if (ret[0] != "")
                {
                    throw new Exception(ret[0]);
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
         
        public bool OnCancel(string dispatchcode, string pono)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(30)))
                {
                    var result = (from _dispatch in Where(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower())
                                  join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                  join _booking in DispatchBookingService.Where(x => x.IsActive) on _dispatchdetail.DispatchDetailId equals _booking.DispatchDetailId
                                  join _pcd_ in ProductionControlDetailService.Where(x => x.IsActive) on _booking.PalletCode equals _pcd_.PalletCode into a
                                  from _pcd in a.DefaultIfEmpty()
                                  select new { _dispatch, _dispatchdetail, _booking, _pcd }
                              ).ToList();

                    result.ForEach(item =>
                    {
                        DispatchBooking booking = DispatchBookingService.FindByID(item._booking.BookingId);

                        if (booking == null)
                        {
                            throw new HILIException("MSG00006");
                        }
                        booking.IsActive = false;
                        booking.UserModified = UserID;
                        booking.DateModified = DateTime.Now;
                        DispatchBookingService.Modify(booking);

                        if (booking.IsBackOrder == false)
                        {
                            ProductionControlDetail updatepcdt = ProductionControlDetailService.FindByID(item._pcd.PackingID);

                            decimal? qty = updatepcdt.ReserveQTY - booking.RequestQty;
                            if (updatepcdt.ReserveQTY > 0)
                            {
                                if (updatepcdt.ReserveQTY >= qty)
                                {
                                    updatepcdt.ReserveQTY = qty;
                                }
                                else
                                {
                                    updatepcdt.ReserveQTY = 0;
                                }

                                updatepcdt.ReserveBaseQTY = updatepcdt.ReserveBaseQTY - booking.RequestBaseQty; updatepcdt.UserModified = UserID;
                                updatepcdt.DateModified = DateTime.Now;
                            }
                            else
                            {
                                updatepcdt.ReserveQTY = 0;
                            }

                            ProductionControlDetailService.Modify(updatepcdt);
                        }

                    }); 
                    List<Dispatch> dispatch = Where(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower()).ToList();// result.Select(x => x._dispatch).Distinct().ToList();
                    dispatch.ForEach(x =>
                    {
                        Dispatch dispath = FindByID(x.DispatchId); 
                        if (dispath == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        dispath.IsBackOrder = false;
                        dispath.DispatchStatus =  DispatchStatusEnum.New;
                        dispath.UserModified = UserID;
                        dispath.DateModified = DateTime.Now;
                        base.Modify(dispath);


                        List<DispatchDetail> dispatchdetail = DispatchDetailService.Where(e => x.IsActive && e.DispatchId == x.DispatchId).ToList();
                        dispatchdetail.ForEach(dpd =>
                        {
                            DispatchDetail dispathdetail = DispatchDetailService.FindByID(dpd.DispatchDetailId);

                            if (dispathdetail == null)
                            {
                                throw new HILIException("MSG00006");
                            }

                            dispathdetail.IsBackOrder = false;
                            dispathdetail.BackOrderQuantity = 0;
                            dispathdetail.DispatchDetailStatus = DispatchDetailStatusEnum.New;
                            dispathdetail.UserModified = UserID;
                            dispathdetail.DateModified = DateTime.Now;
                            DispatchDetailService.Modify(dispathdetail);
                        }); 
                    }); 

                    //bool isupdateheader = false;

                    //var result_status = (from _dispatch in Query().Filter(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower()).Get()
                    //                     join _dispatchdetail in DispatchDetailService.Query().Filter(x => x.IsActive).Get() on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                    //                     select new { _dispatch, _dispatchdetail }
                    //    ).ToList();

                    //result_status.ForEach(item =>
                    //{
                    //    var dispathdetail = DispatchDetailService.FindByID(item._dispatchdetail.DispatchDetailId);

                    //    if (dispathdetail == null)
                    //        throw new HILIException("MSG00006");

                    //    dispathdetail.IsBackOrder = false;
                    //    dispathdetail.BackOrderQuantity = 0;
                    //    dispathdetail.DispatchDetailStatus = (int)DispatchDetailStatusEnum.New;
                    //    dispathdetail.UserModified = this.UserID;
                    //    dispathdetail.DateModified = DateTime.Now;
                    //    DispatchDetailService.Modify(dispathdetail);


                    //    if (isupdateheader == false)
                    //    {
                    //        var dispath = FindByID(item._dispatch.DispatchId);

                    //        if (dispath == null)
                    //            throw new HILIException("MSG00006");

                    //        dispath.IsBackOrder = false;
                    //        dispath.DispatchStatus = (int)DispatchStatusEnum.New;
                    //        dispath.UserModified = this.UserID;
                    //        dispath.DateModified = DateTime.Now;
                    //        base.Modify(dispath);

                    //        isupdateheader = true;
                    //    }
                    //});


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
        public bool OnApproveBooking(string dispatchcode, string pono, string refcode)
        {
            try
            {
                var result = new[]
                {
                    new { _dispatch = new Dispatch(), _dispatchdetail = new DispatchDetail(), _booking = new DispatchBooking(),_rcv = new Receiving() ,_pc = new ProductionControlDetail()}
                }.ToList();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    //validation
                    var _checkdispatch = FirstOrDefault(x => x.DispatchCode == dispatchcode && x.Pono == pono);
                    if (_checkdispatch != null && _checkdispatch.DispatchStatus == (refcode == "111" ? DispatchStatusEnum.InprogressConfirm : DispatchStatusEnum.InternalReceive))
                    {
                        throw new HILIException("Data Is Approve Booked!!");
                    }
                    result = (from _dispatch in Where(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower())
                              join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                              join _booking in DispatchBookingService.Where(x => x.IsActive && x.BookingStatus == BookingStatusEnum.Inprogress && x.IsBackOrder == false)  on _dispatchdetail.DispatchDetailId equals _booking.DispatchDetailId
                              join _rcv in receivingService.Where(x => x.IsActive) on new { PalletCode = _booking.PalletCode , ProductId =_booking.ProductId} equals new { PalletCode = _rcv.PalletCode, ProductId = _rcv.ProductID}
                              join _pc in ProductionControlDetailService.Where(x => x.IsActive) on _booking.PalletCode equals _pc.PalletCode
                             // where _pc.RemainQTY - (_pc.ReserveQTY<0?0:_pc.ReserveQTY) >0
                              select new { _dispatch, _dispatchdetail, _booking, _rcv, _pc}
                        ).ToList(); 
                };
                var _datastockbalance = result.Where(e=>e._pc.RemainQTY - (e._pc.ReserveQTY < 0 ? 0 : e._pc.ReserveQTY) > 0).GroupBy(g => new
                {
                    ProductId = g._booking.ProductId,
                    ProductLot = g._booking.ProductLot,
                    RequestStockUnitId = g._booking.RequestStockUnitId,
                    RequestBaseUnitId = g._booking.RequestBaseUnitId,
                    ConversionQty = g._booking.ConversionQty,
                    ProductStatusId = g._dispatchdetail.ProductStatusId,
                    ProductSubStatusId = g._dispatchdetail.ProductSubStatusId,
                    DocumentId = g._dispatchdetail.DispatchDetailId,
                    DocumentTypeId = g._dispatch.DocumentId,
                    DocumentCode = g._dispatch.DispatchCode,
                    LocationId = g._pc.LocationID,
                    ManufacturingDate = g._booking.Mfgdate,
                    ExpirationDate = g._booking.ExpirationDate,
                    SupplierID = g._rcv.SupplierID,
                    ProductOwnerID = g._rcv.ProductOwnerID,
                    PalletCode = g._rcv.PalletCode
                }).Select(n => new
                {
                    ProductId = n.Key.ProductId,
                    ProductLot = n.Key.ProductLot,
                    RequestQty = n.Sum(x => x._booking.RequestQty),
                    RequestStockUnitId = n.Key.RequestStockUnitId,
                    RequestBaseUnitId = n.Key.RequestBaseUnitId,
                    ConversionQty = n.Key.ConversionQty,
                    ProductStatusId = n.Key.ProductStatusId,
                    ProductSubStatusId = n.Key.ProductSubStatusId,
                    DocumentId = n.Key.DocumentId,
                    DocumentTypeId = n.Key.DocumentTypeId,
                    DocumentCode = n.Key.DocumentCode,
                    LocationId = n.Key.LocationId,
                    ManufacturingDate = n.Key.ManufacturingDate.Date,
                    ExpirationDate = n.Key.ExpirationDate.Value.Date,
                    SupplierID = n.Key.SupplierID,
                    ProductOwnerID = n.Key.ProductOwnerID,
                    PalletCode = n.Key.PalletCode
                }).OrderBy(e=>e.ProductId).ToList();
                 

                if (_datastockbalance == null)
                {
                    throw new HILIException("MSG00036");
                }
                List<DPLocationBalance> _locationbalance = new List<DPLocationBalance>();
                List<DPBalanceInfo> _balanceInfo = new List<DPBalanceInfo>();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                { 
                    _datastockbalance.ForEach(item =>
                    {
                        var tmp = (from location in StockLocationBalanceService.Where(e => e.IsActive && (e.StockQuantity - e.ReserveQuantity) > 0)
                                       join balance in StockBalanceService.Where(e => e.IsActive && (e.StockQuantity-e.ReserveQuantity)>0) on location.StockBalanceID equals balance.StockBalanceID
                                       join stockinfo in StockInfoService.Where(e => e.IsActive) on balance.StockInfoID equals stockinfo.StockInfoID
                                       where stockinfo.ProductID == item.ProductId
                                       && stockinfo.Lot == item.ProductLot
                                       && stockinfo.StockUnitID == item.RequestStockUnitId
                                       && stockinfo.BaseUnitID == item.RequestBaseUnitId
                                       && stockinfo.ConversionQty == item.ConversionQty
                                       && stockinfo.ManufacturingDate == item.ManufacturingDate
                                       && stockinfo.ExpirationDate == item.ExpirationDate
                                       && stockinfo.SupplierID == item.SupplierID
                                       && stockinfo.ProductOwnerID == item.ProductOwnerID      
                                       select location.StockLocationID
                                    ).Distinct().ToList();

                        var _stocks = (from location in StockLocationBalanceService.Where(e => e.IsActive)
                                       join balance in StockBalanceService.Where(e => e.IsActive) on location.StockBalanceID equals balance.StockBalanceID
                                       join stockinfo in StockInfoService.Where(e => e.IsActive) on balance.StockInfoID equals stockinfo.StockInfoID
                                       where tmp.Contains(location.StockLocationID)
                                       select new
                                       {
                                           stockinfo,
                                           balance,
                                           location
                                       }).ToList(); 
                        if (_stocks == null || _stocks.Count <= 0)
                        {
                            throw new HILIException("MSG00037");
                        }
                        var requestQty = item.RequestQty;
                        _stocks.ForEach(_stock =>
                        {
                            if (requestQty > 0)
                            {
                                var _stockbalance = _stock.balance;
                                if (_stockbalance == null)
                                {
                                    throw new HILIException("MSG00038");
                                }
                                //if (item.RequestQty > (_stockbalance.StockQuantity - _stockbalance.ReserveQuantity))
                                if ((_stockbalance.StockQuantity - _stockbalance.ReserveQuantity) <= 0)
                                {
                                    throw new HILIException("MSG00039");
                                }
                                var datalocation = LocationService.FirstOrDefault(x => x.LocationID == item.LocationId);
                                if (datalocation == null)
                                    throw new HILIException("MSG00040");

                                var datazone = ZoneService.FirstOrDefault(x => x.ZoneID == datalocation.ZoneID);
                                if (datazone == null)
                                {
                                    throw new HILIException("MSG00041");
                                }
                                var datastocklocationbalance = StockLocationBalanceService.FirstOrDefault(x => x.IsActive && x.ZoneID == datazone.ZoneID && x.WarehouseID == datazone.WarehouseID && x.StockBalanceID == _stockbalance.StockBalanceID);
                                if (datastocklocationbalance == null)
                                {
                                    throw new HILIException("MSG00042");
                                }

                                var newRequest = ((datastocklocationbalance.StockQuantity - datastocklocationbalance.ReserveQuantity) > requestQty ? requestQty : (datastocklocationbalance.StockQuantity - datastocklocationbalance.ReserveQuantity)).GetValueOrDefault();

                                var _datalocation = new DPLocationBalance
                                {
                                    LocationId = datastocklocationbalance.StockLocationID,
                                    StockQuantity = newRequest
                                };
                                _locationbalance.Add(_datalocation);
                                var _datainfo = new DPBalanceInfo
                                {
                                    StockInfoId = _stock.stockinfo.StockInfoID,
                                    StockQuantity = newRequest
                                };
                                _balanceInfo.Add(_datainfo);

                                //Insert Stock Transaction
                                var _stocktrans = new StockTransaction
                                {
                                    StockTransactionID = Guid.NewGuid(),
                                    DocumentID = item.DocumentId,
                                    DocumentCode = item.DocumentCode,
                                    PackageID = null,
                                    StockTransType = StockTransactionTypeEnum.Reserve,
                                    LocationID = item.LocationId.GetValueOrDefault(),
                                    PalletCode = item.PalletCode,
                                    BaseQuantity = newRequest * item.ConversionQty.GetValueOrDefault(),
                                    ConversionQty = item.ConversionQty.GetValueOrDefault(),
                                    StockLocationID = datastocklocationbalance.StockLocationID,
                                    DocumentTypeID = item.DocumentTypeId,
                                    IsActive = true,
                                    DateCreated = DateTime.Now,
                                    UserCreated = this.UserID,
                                    DateModified = DateTime.Now,
                                    UserModified = this.UserID
                                };
                                StockTransactionService.Add(_stocktrans);
                                requestQty = requestQty - newRequest;
                            }
                        });
                    });

                    //StockLocationBalance
                    var stock_location = (from s in _locationbalance
                                          select new
                                          {
                                              LocationId = s.LocationId,
                                              StockQuantity = s.StockQuantity
                                          } into g
                                          group g by new
                                          {
                                              g.LocationId
                                          } into x
                                          select new
                                          {
                                              LocationId = x.Key.LocationId,
                                              StockQuantity = x.Sum(s => s.StockQuantity)
                                          }).ToList();

                    stock_location.ForEach(itemst =>
                    {
                        var usl = StockLocationBalanceService.FirstOrDefault(x => x.StockLocationID == itemst.LocationId);
                        if (usl == null)
                        {
                            throw new HILIException("MSG00042");
                        }

                        if (usl.StockQuantity - (usl.ReserveQuantity + itemst.StockQuantity) < 0)
                        {
                            throw new HILIException("MSG00039");
                        }
                        usl.ReserveQuantity += itemst.StockQuantity;
                        usl.DateModified = DateTime.Now;
                        usl.UserModified = this.UserID;
                        StockLocationBalanceService.Modify(usl);
                    }); 
                    //Stock Balnce 
                    var stock_balnce = (from s in _balanceInfo
                                        select new
                                        {
                                            StockInfoId = s.StockInfoId,
                                            StockQuantity = s.StockQuantity
                                        } into g
                                        group g by new
                                        {
                                            g.StockInfoId
                                        } into x
                                        select new
                                        {
                                            StockInfoId = x.Key.StockInfoId,
                                            StockQuantity = x.Sum(s => s.StockQuantity)
                                        }
                                         ).ToList();

                    stock_balnce.ForEach(itemsb =>
                    {
                        var _stockbalance = StockBalanceService.FirstOrDefault(x => x.StockInfoID == itemsb.StockInfoId);
                        if (_stockbalance == null)
                        {
                            throw new HILIException("MSG00038");
                        }
                        if (_stockbalance.StockQuantity - (_stockbalance.ReserveQuantity + itemsb.StockQuantity) < 0)
                        {
                            throw new HILIException("MSG00039");
                        }
                        _stockbalance.ReserveQuantity += itemsb.StockQuantity;
                        _stockbalance.DateModified = DateTime.Now;
                        _stockbalance.UserModified = this.UserID;
                        StockBalanceService.Modify(_stockbalance);
                    });


                    result.ForEach(item =>
                    {
                        //Update Dispatch Booking Status
                        var up = DispatchBookingService.FindByID(item._booking.BookingId);
                        if (up == null)
                        {
                            throw new HILIException("MSG00044");
                        }
                        up.BookingQty = item._booking.RequestQty;
                        up.BookingStockUnitId = item._booking.RequestStockUnitId;
                        up.BookingBaseQty = item._booking.RequestBaseQty;
                        up.BookingBaseUnitId = item._booking.RequestBaseUnitId;
                        up.BookingStatus = (refcode == "111" ? BookingStatusEnum.InprogressConfirm : BookingStatusEnum.InternalReceive);
                        up.DateModified = DateTime.Now;
                        up.UserModified = this.UserID;
                        DispatchBookingService.Modify(up);
                    });
                    //Update Dispatch Status
                    var _updatedispatch_ = FirstOrDefault(x => x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower() && x.IsActive); 
                    var _updatedispatch = FindByID(_updatedispatch_.DispatchId); 
                    //Update Dispatch Detail Status
                    var _updatedispatchdetail = DispatchDetailService.Where(x => x.IsActive && x.DispatchId == _updatedispatch.DispatchId).ToList();

                    _updatedispatchdetail.ForEach(_item =>
                    {
                        var item = DispatchDetailService.FindByID(_item.DispatchDetailId);
                        item.DispatchDetailStatus = (refcode == "111" ? DispatchDetailStatusEnum.InprogressConfirm :DispatchDetailStatusEnum.InternalReceive);
                        item.DateModified = DateTime.Now;
                        item.UserModified = this.UserID;
                        DispatchDetailService.Modify(item);
                    });
                    _updatedispatch.DispatchStatus = (refcode == "111" ? DispatchStatusEnum.InprogressConfirm : DispatchStatusEnum.InternalReceive);
                    _updatedispatch.DateModified = DateTime.Now;
                    _updatedispatch.UserModified = this.UserID;
                    base.Modify(_updatedispatch);
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

        public bool RemoveBooking(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DispatchBooking booking = DispatchBookingService.SingleOrDefault(x => x.IsActive && x.BookingId == id);

                    if (booking == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    //Booking
                    booking.BookingStatus = BookingStatusEnum.Close;
                    booking.IsActive = false;
                    booking.UserModified = UserID;
                    booking.DateModified = DateTime.Now;
                    DispatchBookingService.Modify(booking); 
                    //Dispatch Detail
                    DispatchDetail dispathdetail = DispatchDetailService.FindByID(booking.DispatchDetailId);

                    if (dispathdetail == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    dispathdetail.BackOrderQuantity = dispathdetail.BackOrderQuantity - booking.RequestQty;
                    dispathdetail.UserModified = UserID;
                    dispathdetail.DateModified = DateTime.Now;
                    if (dispathdetail.BackOrderQuantity >= 0)
                    {
                        dispathdetail.IsBackOrder = false;
                    }
                    DispatchDetailService.Modify(dispathdetail);


                    //Dispatch
                    DispatchDetail dispathdt = DispatchDetailService.FirstOrDefault(x => x.DispatchDetailId == booking.DispatchDetailId);
                    Dispatch dispathhd = FirstOrDefault(x => x.DispatchId == dispathdt.DispatchId);


                    //Update status back order
                    bool isupdateheader = false;
                    var result = (from _dispatch in Where(x => x.IsActive && x.DispatchCode == dispathhd.DispatchCode && x.Pono.ToLower() == dispathhd.Pono.ToLower())
                                  join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                  join _booking in DispatchBookingService.Where(x => x.IsActive) on _dispatchdetail.DispatchDetailId equals _booking.DispatchDetailId
                                  where _booking.IsBackOrder == true
                                  select new { _dispatch, _dispatchdetail, _booking }
                              ).ToList();


                    if (result.Count > 0)
                    {
                        isupdateheader = true;
                    }

                    Dispatch dispath = FindByID(dispathhd.DispatchId);
                    if (dispath == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    dispath.IsBackOrder = isupdateheader;
                    dispath.UserModified = UserID;
                    dispath.DateModified = DateTime.Now;
                    base.Modify(dispath);


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

        public bool RemoveBookingAdjustToBackOrder(Guid id)
        {
            try
            {
                DispatchBooking booking = DispatchBookingService.FirstOrDefault(x => x.IsActive && x.BookingId == id);
                if (booking == null)
                {
                    throw new HILIException("MSG00006");
                }
                using (TransactionScope scope = new TransactionScope())
                {                  
                    //Booking
                    booking.IsActive = false;
                    booking.UserModified = UserID;
                    booking.DateModified = DateTime.Now;
                    DispatchBookingService.Modify(booking);

                    if (booking.IsBackOrder == false)
                    {
                        //return reserver qty 
                        IRepository<ProductionControlDetail> _getpcdt = Context.Repository<ProductionControlDetail>();
                        ProductionControlDetail updatepcdt = _getpcdt.FirstOrDefault(x => x.IsActive && x.PalletCode == booking.PalletCode);
                        if (updatepcdt.ReserveQTY > 0)
                        {
                            updatepcdt.ReserveQTY -= booking.RequestQty;
                            updatepcdt.ReserveBaseQTY -= booking.RequestBaseQty;
                            updatepcdt.UserModified = UserID;
                            updatepcdt.DateModified = DateTime.Now;
                            _getpcdt.Modify(updatepcdt);
                        }

                        //adjust back order qty
                        DispatchBooking bookingaddjust = DispatchBookingService.SingleOrDefault(x => x.DispatchDetailId == booking.DispatchDetailId && x.IsBackOrder == true && x.IsActive == true && x.ProductId == booking.ProductId && x.BookingStockUnitId == booking.BookingStockUnitId);
                        if (bookingaddjust != null)
                        {
                            bookingaddjust.RequestQty += booking.RequestQty;
                            bookingaddjust.RequestBaseQty += booking.RequestBaseQty;
                            bookingaddjust.BookingQty += booking.BookingQty;
                            bookingaddjust.BookingBaseQty += booking.BookingBaseQty;
                            DispatchBookingService.Modify(bookingaddjust);


                            DispatchDetail dispathdetail = DispatchDetailService.FindByID(booking.DispatchDetailId);

                            if (dispathdetail == null)
                            {
                                throw new HILIException("MSG00006");
                            }

                            dispathdetail.IsBackOrder = true;
                            dispathdetail.BackOrderQuantity += booking.RequestQty;
                            dispathdetail.UserModified = UserID;
                            dispathdetail.DateModified = DateTime.Now;
                            DispatchDetailService.Modify(dispathdetail);

                        }
                        else
                        {
                            DispatchBooking savebooking = new DispatchBooking
                            {
                                BookingId = Guid.NewGuid(),
                                DispatchDetailId = booking.DispatchDetailId,
                                Sequence = booking.Sequence,
                                ProductId = booking.ProductId,
                                ProductLot = "",
                                RequestQty = booking.RequestQty,
                                BookingQty = 0,
                                BookingStatus = BookingStatusEnum.Inprogress,
                                LocationId = Guid.Empty,
                                Mfgdate = DateTime.Now,
                                ExpirationDate = DateTime.Now,
                                RequestStockUnitId = booking.RequestStockUnitId,
                                RequestBaseQty = booking.RequestBaseQty,
                                RequestBaseUnitId = booking.RequestBaseUnitId,
                                BookingStockUnitId = booking.BookingStockUnitId,
                                BookingBaseQty = booking.BookingBaseQty,
                                BookingBaseUnitId = booking.BookingBaseUnitId,
                                ConversionQty = booking.ConversionQty,
                                DateCreated = DateTime.Now,
                                UserCreated = UserID,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                IsActive = true,
                                IsBackOrder = true
                            };
                            DispatchBookingService.Add(savebooking);

                            DispatchDetail dispathdetail = DispatchDetailService.FindByID(booking.DispatchDetailId);

                            if (dispathdetail == null)
                            {
                                throw new HILIException("MSG00006");
                            }

                            dispathdetail.IsBackOrder = true;
                            dispathdetail.BackOrderQuantity = dispathdetail.BackOrderQuantity + booking.RequestQty;
                            dispathdetail.UserModified = UserID;
                            dispathdetail.DateModified = DateTime.Now;
                            DispatchDetailService.Modify(dispathdetail);

                        }

                    }
                    else
                    {
                        //reduce back order qty 
                        DispatchDetail dispathdetail = DispatchDetailService.FindByID(booking.DispatchDetailId);

                        if (dispathdetail == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        dispathdetail.BackOrderQuantity -= booking.RequestQty;
                        dispathdetail.UserModified = UserID;
                        dispathdetail.DateModified = DateTime.Now;
                        if (dispathdetail.BackOrderQuantity >= 0)
                        {
                            dispathdetail.IsBackOrder = false;
                        }
                        DispatchDetailService.Modify(dispathdetail);
                    }


                    //Update status back order
                    DispatchDetail dispathdt = DispatchDetailService.FirstOrDefault(x => x.DispatchDetailId == booking.DispatchDetailId);
                    Dispatch dispathhd = FirstOrDefault(x => x.DispatchId == dispathdt.DispatchId);


                    bool isupdateheader = false;
                    var result = (from _dispatch in Where(x => x.IsActive && x.DispatchCode == dispathhd.DispatchCode && x.Pono.ToLower() == dispathhd.Pono.ToLower())
                                  join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _dispatch.DispatchId equals _dispatchdetail.DispatchId
                                  join _booking in DispatchBookingService.Where(x => x.IsActive) on _dispatchdetail.DispatchDetailId equals _booking.DispatchDetailId
                                  where _booking.IsBackOrder == true
                                  select new { _dispatch, _dispatchdetail, _booking }
                              ).ToList();


                    if (result.Count > 0)
                    {
                        isupdateheader = true;
                    }

                    Dispatch dispath = FindByID(dispathhd.DispatchId);
                    if (dispath == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    dispath.IsBackOrder = isupdateheader;
                    dispath.UserModified = UserID;
                    dispath.DateModified = DateTime.Now;
                    base.Modify(dispath);

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

        public bool RemoveBookingToReCalculateBooking(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DispatchBooking booking = DispatchBookingService.SingleOrDefault(x => x.IsActive && x.BookingId == id);

                    if (booking == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    DispatchDetail dispatchdt = DispatchDetailService.SingleOrDefault(x => x.DispatchDetailId == booking.DispatchDetailId);

                    dispatchdt.Quantity = dispatchdt.Quantity - booking.RequestQty;
                    dispatchdt.BaseQuantity = dispatchdt.BaseQuantity - booking.RequestBaseQty;
                    dispatchdt.UserModified = UserID;
                    dispatchdt.DateModified = DateTime.Now;
                    DispatchDetailService.Modify(dispatchdt);


                    Dispatch dispatchhd = SingleOrDefault(x => x.DispatchId == dispatchdt.DispatchId);

                    bool ret = OnBookingBackOrder(dispatchhd.DispatchCode, dispatchhd.Pono, null);

                    if (ret)
                    {
                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
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

        #endregion

        #region BackOrder
        public List<BackOrderModel> GetViewBackOrder(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {


                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword.ToLower());

                SqlParameter param1 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = keyword };
                SqlParameter param2 = new SqlParameter("@pageIndex", SqlDbType.Int) { Value = pageIndex };
                SqlParameter param3 = new SqlParameter("@pageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter param4 = new SqlParameter("@chkCount", SqlDbType.Bit) { Value = 0 };

                List<BackOrderModel> result = Context.SQLQuery<BackOrderModel>("exec SP_GetViewBackOrder @keyword, @pageIndex, @pageSize, @chkCount", param1, param2, param3, param4).ToList();



                SqlParameter param11 = new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = keyword };
                SqlParameter param22 = new SqlParameter("@pageIndex", SqlDbType.Int) { Value = pageIndex };
                SqlParameter param33 = new SqlParameter("@pageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter param44 = new SqlParameter("@chkCount", SqlDbType.Bit) { Value = 1 };

                int result_count = Context.SQLQuery<int>("exec SP_GetViewBackOrder @keyword, @pageIndex, @pageSize, @chkCount", param11, param22, param33, param44).SingleOrDefault();




                totalRecords = result_count;
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
         
        public bool OnBookingBackOrder(string dispatchcode, string pono, Guid? bookingid)
        {
            try
            {
                string refcode = "111";
                var dispatch = (from _dispatch in Where(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower())
                                join _itfmap in ItfInterfaceMappingService.Where(x => x.IsActive) on _dispatch.DocumentId equals _itfmap.DocumentId
                                select new { IsMarketing = _itfmap.IsMarketing }
                                ).SingleOrDefault();
                if (dispatch != null)
                {
                    if (dispatch.IsMarketing != null)
                    {
                        refcode = (dispatch.IsMarketing == true ? "412" : "111");
                    }

                }

                SqlParameter param1 = new SqlParameter("@dispatchcode", SqlDbType.NVarChar) { Value = dispatchcode };
                SqlParameter param2 = new SqlParameter("@pono", SqlDbType.NVarChar) { Value = pono };
                SqlParameter param3 = new SqlParameter("@bookingid", SqlDbType.UniqueIdentifier) { Value = bookingid };
                SqlParameter param4 = new SqlParameter("@refcode", SqlDbType.NVarChar) { Value = refcode };
                SqlParameter param5 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };

                List<Guid> results = Context.SQLQuery<Guid>("exec SP_OnBookingBackOrder @dispatchcode, @pono, @bookingid, @refcode,@UserID", param1, param2, param3, param4, param5).ToList();
                               
                var result = (from _booking in DispatchBookingService.Where(x => x.IsActive && results.Contains(x.BookingId))
                              join _rcv in receivingService.Where(x => x.IsActive) on _booking.PalletCode equals _rcv.PalletCode
                              join _pc in ProductionControlDetailService.Where(x => x.IsActive) on _rcv.PalletCode equals _pc.PalletCode
                              join _dispatchdetail in DispatchDetailService.Where(x => x.IsActive) on _booking.DispatchDetailId equals _dispatchdetail.DispatchDetailId
                              join _dispatch in Where(x => x.IsActive && x.DispatchCode == dispatchcode && x.Pono.ToLower() == pono.ToLower()) on _dispatchdetail.DispatchId equals _dispatch.DispatchId
                              select new { _dispatch, _dispatchdetail, _booking, _rcv, _pc }
                  ).ToList();


                Dispatch _updatedispatch = FirstOrDefault(x => x.DispatchCode == dispatchcode && x.Pono == pono);

                using (TransactionScope scope = new TransactionScope())
                {
                    if (_updatedispatch.DispatchStatus >= DispatchStatusEnum.InprogressConfirm)
                    {
                        var _datastockbalance = result.GroupBy(g => new
                        {
                            ProductId = g._booking.ProductId,
                            ProductLot = g._booking.ProductLot,
                            RequestStockUnitId = g._booking.RequestStockUnitId,
                            RequestBaseUnitId = g._booking.RequestBaseUnitId,
                            ConversionQty = g._booking.ConversionQty,
                            ProductStatusId = g._dispatchdetail.ProductStatusId,
                            ProductSubStatusId = g._dispatchdetail.ProductSubStatusId,
                            DocumentId = g._dispatchdetail.DispatchDetailId,
                            DocumentTypeId = g._dispatch.DocumentId,
                            DocumentCode = g._dispatch.DispatchCode,
                            LocationId = g._pc.LocationID.Value,
                            ManufacturingDate = g._booking.Mfgdate,
                            ExpirationDate = g._booking.ExpirationDate,
                            SupplierID = g._rcv.SupplierID,
                            ProductOwnerID = g._rcv.ProductOwnerID,
                            PalletCode = g._rcv.PalletCode
                        }).Select(n => new
                        {
                            ProductId = n.Key.ProductId,
                            ProductLot = n.Key.ProductLot,
                            RequestQty = n.Sum(x => x._booking.RequestQty),
                            RequestStockUnitId = n.Key.RequestStockUnitId,
                            RequestBaseUnitId = n.Key.RequestBaseUnitId,
                            ConversionQty = n.Key.ConversionQty,
                            ProductStatusId = n.Key.ProductStatusId,
                            ProductSubStatusId = n.Key.ProductSubStatusId,
                            DocumentId = n.Key.DocumentId,
                            DocumentTypeId = n.Key.DocumentTypeId,
                            DocumentCode = n.Key.DocumentCode,
                            LocationId = n.Key.LocationId,
                            ManufacturingDate = n.Key.ManufacturingDate.Date,
                            ExpirationDate = n.Key.ExpirationDate.Value.Date,
                            SupplierID = n.Key.SupplierID,
                            ProductOwnerID = n.Key.ProductOwnerID,
                            PalletCode = n.Key.PalletCode

                        }).ToList();

                        if (_datastockbalance == null)
                        {
                            throw new HILIException("MSG00036");
                        }

                        List<DPLocationBalance> _locationbalance = new List<DPLocationBalance>();
                        List<DPBalanceInfo> _balanceInfo = new List<DPBalanceInfo>();
                        _datastockbalance.ForEach(item =>
                        {
                            StockInfo _stock = StockInfoService.FirstOrDefault(
                                x => x.ProductID == item.ProductId
                                && x.Lot == item.ProductLot
                                && x.StockUnitID == item.RequestStockUnitId
                                && x.BaseUnitID == item.RequestBaseUnitId
                                && x.ConversionQty == item.ConversionQty
                                && x.ManufacturingDate == item.ManufacturingDate
                                && x.ExpirationDate == item.ExpirationDate
                                && x.SupplierID == item.SupplierID
                                && x.ProductOwnerID == item.ProductOwnerID
                            ///&& x.Price == item.Price
                            );

                            if (_stock == null)
                            {
                                throw new HILIException("MSG00037");
                            }

                            StockBalance _stockbalance = StockBalanceService.FirstOrDefault(x => x.StockInfoID == _stock.StockInfoID);
                            if (_stockbalance == null)
                            {
                                throw new HILIException("MSG00038");
                            }

                            if (item.RequestQty > _stockbalance.StockQuantity - _stockbalance.ReserveQuantity)
                            {
                                throw new HILIException("MSG00039");
                            }

                            Location datalocation = LocationService.FirstOrDefault(x => x.LocationID == item.LocationId);
                            if (datalocation == null)
                            {
                                throw new HILIException("MSG00040");
                            }

                            Zone datazone = ZoneService.FirstOrDefault(x => x.ZoneID == datalocation.ZoneID);
                            if (datazone == null)
                            {
                                throw new HILIException("MSG00041");
                            }
                            StockLocationBalance datastocklocationbalance = StockLocationBalanceService.FirstOrDefault(x => x.ZoneID == datazone.ZoneID && x.WarehouseID == datazone.WarehouseID && x.StockBalanceID == _stockbalance.StockBalanceID);
                            if (datastocklocationbalance == null)
                            {
                                throw new HILIException("MSG00042");
                            }

                            DPLocationBalance _datalocation = new DPLocationBalance
                            {
                                LocationId = datastocklocationbalance.StockLocationID,
                                StockQuantity = item.RequestQty
                            };
                            _locationbalance.Add(_datalocation);


                            DPBalanceInfo _datainfo = new DPBalanceInfo
                            {
                                StockInfoId = _stock.StockInfoID,
                                StockQuantity = item.RequestQty
                            };
                            _balanceInfo.Add(_datainfo);


                            //Insert Stock Transaction
                            StockTransaction _stocktrans = new StockTransaction
                            {
                                StockTransactionID = Guid.NewGuid(),
                                DocumentID = item.DocumentId,
                                DocumentCode = item.DocumentCode,
                                PackageID = null,
                                StockTransType = StockTransactionTypeEnum.Reserve,
                                LocationID = item.LocationId,
                                PalletCode = item.PalletCode,
                                BaseQuantity = item.RequestQty * item.ConversionQty.Value,
                                ConversionQty = item.ConversionQty.Value,
                                StockLocationID = datastocklocationbalance.StockLocationID,
                                DocumentTypeID = item.DocumentTypeId,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                UserCreated = UserID,
                                DateModified = DateTime.Now,
                                UserModified = UserID
                            };

                            StockTransactionService.Add(_stocktrans);
                        });

                        //StockLocationBalance
                        var stock_location = (from s in _locationbalance
                                              select new
                                              {
                                                  LocationId = s.LocationId,
                                                  StockQuantity = s.StockQuantity
                                              } into g
                                              group g by new
                                              {
                                                  g.LocationId
                                              } into x
                                              select new
                                              {
                                                  LocationId = x.Key.LocationId,
                                                  StockQuantity = x.Sum(s => s.StockQuantity)
                                              }
                                              ).ToList();

                        stock_location.ForEach(itemst =>
                        {
                            StockLocationBalance usl = StockLocationBalanceService.FirstOrDefault(x => x.StockLocationID == itemst.LocationId);
                            if (usl == null)
                            {
                                throw new HILIException("MSG00042");
                            }

                            usl.ReserveQuantity = usl.ReserveQuantity + itemst.StockQuantity;
                            usl.DateModified = DateTime.Now;
                            usl.UserModified = UserID;
                            StockLocationBalanceService.Modify(usl);
                        });
                        //Stock Balnce
                        var stock_balnce = (from s in _balanceInfo
                                            select new
                                            {
                                                StockInfoId = s.StockInfoId,
                                                StockQuantity = s.StockQuantity
                                            } into g
                                            group g by new
                                            {
                                                g.StockInfoId
                                            } into x
                                            select new
                                            {
                                                StockInfoId = x.Key.StockInfoId,
                                                StockQuantity = x.Sum(s => s.StockQuantity)
                                            }
                                             ).ToList();

                        stock_balnce.ForEach(itemsb =>
                        {
                            StockBalance _stockbalance = StockBalanceService.FirstOrDefault(x => x.StockInfoID == itemsb.StockInfoId);
                            if (_stockbalance == null)
                            {
                                throw new HILIException("MSG00038");
                            }

                            _stockbalance.ReserveQuantity = _stockbalance.ReserveQuantity + itemsb.StockQuantity;
                            _stockbalance.DateModified = DateTime.Now;
                            _stockbalance.UserModified = UserID;
                            StockBalanceService.Modify(_stockbalance);
                        });


                        result.ForEach(item =>
                        {
                            //Update Dispatch Booking Status
                            DispatchBooking up = DispatchBookingService.FindByID(item._booking.BookingId);
                            if (up == null)
                            {
                                throw new HILIException("MSG00044");
                            }

                            up.BookingQty = item._booking.RequestQty;
                            up.BookingStockUnitId = item._booking.RequestStockUnitId;
                            up.BookingBaseQty = item._booking.RequestBaseQty;
                            up.BookingBaseUnitId = item._booking.RequestBaseUnitId;
                            up.BookingStatus = (refcode == "111" ? BookingStatusEnum.InprogressConfirm :BookingStatusEnum.InternalReceive);
                            up.DateModified = DateTime.Now;
                            up.UserModified = UserID;
                            DispatchBookingService.Modify(up);


                        });
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
        
        public bool CheckBackOrder(string pono)
        {
            bool _hdBackorder = false;
            Dispatch _checkbackorder = SingleOrDefault(x => x.IsActive && x.Pono == pono && x.DispatchStatus != DispatchStatusEnum.Close);
            if (_checkbackorder != null)
            {
                _hdBackorder = _checkbackorder.IsBackOrder.Value;
            }
            return _hdBackorder;
        }
        #endregion

        #region Revise Pono
        public DispatchModels GetByPoNo(string pono)
        {
            try
            {

                Dispatch _current = FirstOrDefault(x => x.Pono.ToLower() == pono.ToLower() && x.DispatchStatus != DispatchStatusEnum.Close);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                Dispatch _dispatchdata = SingleOrDefault(x => x.IsActive && x.Pono == pono && x.DispatchStatus !=DispatchStatusEnum.Close);

                int _distpatchcount = Where(x => x.IsActive && x.DispatchCode == _dispatchdata.DispatchCode).Count();

                string _ponorevise = _dispatchdata.Pono + "/" + _distpatchcount.ToString();

                var _hd = (from _dispatch in Where(x => x.IsActive && x.Pono == pono)
                           join _document in DocumentTypeService.Where(x => x.IsActive) on _dispatch.DocumentId equals _document.DocumentTypeID
                           join _shipto in ShiptoService.Where(x => x.IsActive) on _dispatch.ShipptoId equals _shipto.ShipToId
                           join _a in WarehouseService.Where(x => x.IsActive) on _dispatch.FromwarehouseId equals _a.WarehouseID into a
                           from _warehouse in a.DefaultIfEmpty()
                           join _x in WarehouseService.Where(x => x.IsActive) on _dispatch.TowarehouseId equals _x.WarehouseID into x
                           from _warehouse2 in x.DefaultIfEmpty()
                           join _customer in ContactService.Where(x => x.IsActive) on _dispatch.CustomerId equals _customer.ContactID
                           join _marketting in ItfInterfaceMappingService.Where(x => x.IsActive) on _dispatch.DocumentId equals _marketting.DocumentId
                           orderby _dispatch.DispatchStatus
                           select new { _dispatch, _document, _shipto, _warehouse, _warehouse2, _customer, _marketting }
                           ).FirstOrDefault();

                return new DispatchModels()
                {
                    DispatchId = _hd._dispatch.DispatchId,
                    DispatchCode = _hd._dispatch.DispatchCode,
                    IsUrgent = _hd._dispatch.IsUrgent.Value,
                    IsBackOrder = _hd._dispatch.IsBackOrder.Value,
                    Pono = _ponorevise,
                    OrderNo = _hd._dispatch.OrderNo,
                    DocumentId = _hd._dispatch.DocumentId,
                    DocumentName = _hd._document.Name,
                    OrderDate = _hd._dispatch.OrderDate,
                    DocumentApproveDate = _hd._dispatch.DocumentApproveDate,
                    DocumentDate = _hd._dispatch.DocumentDate,
                    DeliveryDate = _hd._dispatch.DeliveryDate,
                    DispatchStatusId = (int?)_hd._dispatch.DispatchStatus,
                    DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), _hd._dispatch.DispatchStatus.ToString())),
                    FromwarehouseId = (_hd._warehouse != null ? _hd._dispatch.FromwarehouseId : null),
                    FromwarehouseName = (_hd._warehouse != null ? _hd._warehouse.Name : null),
                    TowarehouseId = (_hd._warehouse2 != null ? _hd._dispatch.TowarehouseId : null),
                    TowarehouseName = (_hd._warehouse2 != null ? _hd._warehouse2.Name : null),
                    CustomerId = _hd._dispatch.CustomerId.Value,
                    CustomerName = _hd._customer.Name,
                    CustomerCode = _hd._customer.Code,
                    ShipptoId = _hd._dispatch.ShipptoId,
                    ShipptoName = _hd._shipto.Name,
                    Remark = _hd._dispatch.Remark,
                    IsActive = _hd._dispatch.IsActive,
                    IsMarketing = (_hd._marketting.IsMarketing != null ? _hd._marketting.IsMarketing.Value : false)
                };
                //var resultsmodel = _hd.OrderBy(x => x._dispatch.DispatchStatus).Select(n => new DispatchModels
                //{
                //    DispatchId = n._dispatch.DispatchId,
                //    DispatchCode = n._dispatch.DispatchCode,
                //    IsUrgent = n._dispatch.IsUrgent.Value,
                //    IsBackOrder = n._dispatch.IsBackOrder.Value,
                //    Pono = _ponorevise,
                //    OrderNo = n._dispatch.OrderNo,
                //    DocumentId = n._dispatch.DocumentId,
                //    DocumentName = n._document.Name,
                //    OrderDate = n._dispatch.OrderDate,
                //    DocumentApproveDate = n._dispatch.DocumentApproveDate,
                //    DocumentDate = n._dispatch.DocumentDate,
                //    DeliveryDate = n._dispatch.DeliveryDate,
                //    DispatchStatusId = n._dispatch.DispatchStatus,
                //    DispatchStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatch.DispatchStatus.Value.ToString())),
                //    FromwarehouseId = (n._warehouse != null ? n._dispatch.FromwarehouseId : null),
                //    FromwarehouseName = (n._warehouse != null ? n._warehouse.Name : null),
                //    TowarehouseId = (n._warehouse2 != null ? n._dispatch.TowarehouseId : null),
                //    TowarehouseName = (n._warehouse2 != null ? n._warehouse2.Name : null),
                //    CustomerId = n._dispatch.CustomerId.Value,
                //    CustomerName = n._customer.Name,
                //    CustomerCode = n._customer.Code,
                //    ShipptoId = n._dispatch.ShipptoId,
                //    ShipptoName = n._shipto.Name,
                //    Remark = n._dispatch.Remark,
                //    IsActive = n._dispatch.IsActive,
                //    IsMarketing = (n._marketting.IsMarketing != null ? n._marketting.IsMarketing.Value : false)
                //}).FirstOrDefault();


                //var _dt = (from _dispatchdetail in DispatchDetailService.Query().Filter(x => x.IsActive).Get()
                //           join _dispatch in Query().Filter(x=>x.Pono== pono).Get() on _dispatchdetail.DispatchId equals _dispatch.DispatchId
                //           join _proudct in ProductService.Query().Get() on _dispatchdetail.ProductId equals _proudct.ProductID
                //           join _productcode in ProductCodeService.Query().Get() on _proudct.ProductID equals _productcode.ProductID
                //           join _unit in ProductUnitService.Query().Get() on _dispatchdetail.StockUnitId equals _unit.ProductUnitID
                //           join _a in ProductUnitService.Query().Get() on _dispatchdetail.DispatchPriceUnitId equals _a.ProductUnitID into a
                //           from _unit2 in a.DefaultIfEmpty()
                //           join _rule in SpecialBookingRuleService.Query().Get() on _dispatchdetail.RuleId equals _rule.RuleId
                //           join _s in ProductStatusService.Query().Get() on _dispatchdetail.ProductStatusId equals _s.ProductStatusID into s
                //           from _status in s.DefaultIfEmpty()
                //           join _ss in ProductSubStatusService.Query().Get() on _dispatchdetail.ProductSubStatusId equals _ss.ProductSubStatusID into ss
                //           from _sub_status in ss.DefaultIfEmpty()
                //           select new { _dispatchdetail, _proudct, _productcode, _unit, _unit2, _rule, _status, _sub_status }

                // );



                //var resultsdetailmodel = _dt.Select(n => new DispatchDetailModels
                //{
                //    DispatchDetailId = n._dispatchdetail.DispatchDetailId,
                //    DispatchId = n._dispatchdetail.DispatchId,
                //    Sequence = n._dispatchdetail.Sequence,
                //    ProductId = n._dispatchdetail.ProductId,
                //    ProductName = n._proudct.Name,
                //    ProductCode = n._productcode.Code,
                //    StockUnitId = n._dispatchdetail.StockUnitId,
                //    StockUnitName = n._unit.Name,
                //    Quantity = n._dispatchdetail.Quantity,
                //    ConversionQty = n._dispatchdetail.ConversionQty,
                //    ProductOwnerId = n._dispatchdetail.ProductOwnerId,
                //    DispatchDetailProductWidth = n._dispatchdetail.DispatchDetailProductWidth,
                //    DispatchDetailProductLength = n._dispatchdetail.DispatchDetailProductLength,
                //    DispatchDetailProductHeight = n._dispatchdetail.DispatchDetailProductHeight,
                //    BaseUnitId = n._dispatchdetail.BaseUnitId,
                //    BaseUnitName = n._unit.Name,
                //    DispatchPriceUnitId = (n._dispatchdetail.DispatchPriceUnitId != null ? n._dispatchdetail.DispatchPriceUnitId : null),
                //    DispatchPriceUnitName = (n._unit2 != null ? n._unit2.Name : null),
                //    DispatchPrice = n._dispatchdetail.DispatchPrice,
                //    Remark = n._dispatchdetail.Remark,
                //    IsActive = n._dispatchdetail.IsActive,
                //    DispatchDetailStatusId = n._dispatchdetail.DispatchDetailStatus,
                //    DispatchDetailStatusName = GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), n._dispatchdetail.DispatchDetailStatus.ToString())),
                //    RuleId = n._rule.RuleId,
                //    RuleName = n._rule.RuleName,
                //    ProductStatusId = n._status.ProductStatusID,
                //    ProductStatusName = n._status.Name,
                //    ProductSubStatusId = n._dispatchdetail.ProductSubStatusId,// n._sub_status.ProductSubStatusID,
                //    ProductSubStatusName = (n._dispatchdetail.ProductSubStatusId != null ? n._sub_status.Name : ""),

                //}).ToList();


                //resultsmodel.DispatchDetailModelsCollection = resultsdetailmodel;


                // return resultsmodel;

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

        #region QA
        public void QAAddAllConfrimBook(Dispatch entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Any(x => x.IsActive && x.Pono.Replace(" ", string.Empty).ToLower().Equals(entity.Pono.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00033");
                    }

                    if (entity.ReferenceId == null)
                    {
                        DispatchPrefix prefix = DispatchPrefixService.SingleOrDefault(x => x.PrefixType == DispatchPreFixTypeEnum.DISPATHCODE);
                        if (prefix == null)
                        {
                            throw new HILIException("MSG00006");
                        }

                        string DispatchCode = Prefix.OnCreatePrefixed(prefix.LastedKey, prefix.PrefixKey, prefix.FormatKey, prefix.LengthKey);
                        entity.DispatchCode = DispatchCode;
                        prefix.LastedKey = DispatchCode;

                        DispatchPrefixService.Modify(prefix);
                    }

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    foreach (DispatchDetail item in entity.DispatchDetailCollection)
                    {
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                    }
                    Dispatch result = base.Add(entity);

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
        #endregion

        #region Interface Log Dispatch
        public void AddInterfaceDispatch(List<InterfaceDispatchModel> data)
        {
            try
            {
                var interfaceDate = DateTime.Now;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (data == null)
                        throw new HILIException("MSG00006");

                    var ret = (from _data in data
                               join _dp in Query().Filter(x => x.IsActive && x.DispatchStatus != DispatchStatusEnum.Close).Get() on _data.DispatchCode equals _dp.DispatchCode                               
                               join _pdc in ProductCodeService.Query().Filter(x => x.CodeType == ProductCodeTypeEnum.Stock).Get() on _data.ProductId equals _pdc.ProductID
                               join _pd in ProductService.Query().Filter(x => x.IsActive).Get() on _data.ProductId equals _pd.ProductID
                               join _unit in ProductUnitService.Query().Filter(x => x.IsActive).Get() on _data.UnitId equals _unit.ProductUnitID
                               join _unitbase in ProductUnitService.Query().Filter(x => x.IsActive).Get()on _data.BaseUnitId equals _unitbase.ProductUnitID
                               join _itfmap in ItfInterfaceMappingService.Query().Get() on _dp.DocumentId equals _itfmap.DocumentId
                               join _itftype in ItfTransactionTypeService.Query().Get() on _itfmap.InterfaceTypeId equals _itftype.InterfaceTypeId
                               join _cus in ContactService.Query().Get() on _dp.CustomerId equals _cus.ContactID
                               where _data.PONo == _dp.Pono && _dp.DispatchStatus ==  DispatchStatusEnum.Complete
                               select new itf_temp_in_dispatch_log
                               {
                                   TransactionId = Guid.NewGuid(),
                                   Cono = 101,
                                   Orno = "",
                                   Cuno = _cus.Code,
                                   Faci = "F11",
                                   Whlo = (_itfmap.IsMarketing.Value ? "412" : "111"),
                                   Itno = _pdc.Code,
                                   Itds = _pd.Name,
                                   Dwdt = DateFuntion.dateInterface_us(_dp.DocumentApproveDate.Value),//20171116
                                   Orqt = _data.Quantity,
                                   Alun = _unit.Name,
                                   Sapr = 0,
                                   Spun = _unit.Name,
                                   Dia1 = 0,
                                   Dia2 = 0,
                                   Dia3 = 0,
                                   Dia4 = 0,
                                   Dia5 = 0,
                                   Dia6 = 0,
                                   Dip1 = 0,
                                   Dip2 = 0,
                                   Dip3 = 0,
                                   Dip4 = 0,
                                   Dip5 = 0,
                                   Dip6 = 0,
                                   Cuor = _data.PONo,
                                   Dwdz = DateFuntion.dateInterface_us(_dp.DocumentApproveDate.Value),//20171116
                                   Dwhz = interfaceDate.ToString("HHmm"),//1230
                                   Rscd = "",
                                   Bano = _data.Lot,
                                   Whsl = "A01",
                                   Ortp = _itftype.Ortp,
                                   Rldt = DateFuntion.dateInterface_us(_dp.DocumentApproveDate.Value),//20171116
                                   Modl = "",
                                   Tedl = "",
                                   Yref = "",
                                   Tepy = "",
                                   Pyno = "",
                                   Adid = "",
                                   Oref = "",
                                   Cudt = DateFuntion.dateInterface_us(_dp.DocumentDate.Value),//20171116
                                   Ordt = DateFuntion.dateInterface_us(_dp.OrderDate.Value),//20171116
                                   Rldz = DateFuntion.dateInterface_us(_dp.DocumentApproveDate.Value),//20171116
                                   Rlhz = interfaceDate.ToString("HHmm"),//1230
                                   Ctst = "",
                                   Emsg = "",
                                   Wmsorn = _data.DispatchCode,
                                   Gdate = DateFuntion.dateInterface_us(interfaceDate),//20171116
                                   Gtime = interfaceDate.ToString("HHmm"),//1230
                                   Gstt = "S",
                                   Rnom3 = "",
                                   Fdate = "",
                                   Ftime = "",
                                   Fstt = "",
                                   SyncUnsuccessNo = 0,
                                   SyncFlag = "",
                                   SyncDate = "",
                                   ProductSystemCode = _pdc.Code,
                                   ProductNameFull = _pd.Name,
                                   DispatchDateDelivery = _dp.DeliveryDate.Value,
                                   DispatchDetailProductQuantity = _data.Quantity,
                                   ProductPriceUomId = null,
                                   DispatchDetailProductPrice = 0,
                                   DocumentNo = _data.PONo,
                                   DispatchDateOrder = _dp.OrderDate.Value,
                                   SubCustCode = _cus.Code,
                                   DispatchCode = _data.DispatchCode,
                                   Gedt = DateFuntion.dateInterface_us(interfaceDate),//20171116
                                   Getm = interfaceDate.ToString("HHmm"),//1230
                                   Twhl = (_itfmap.IsMarketing.Value ? "412" : "111"),
                                   Twsl = "A01",
                                   Dlqt = _data.BaseQuantity,
                                   Alqt = _data.BaseQuantity,
                                   Ridn = "",
                                   Ridl = 0,
                                   Ridx = 0,
                                   Ridi = 0,
                                   Plsx = 0,
                                   Dlix = 0,
                                   Trtp = _itftype.Ortp,
                                   Tofp = "",
                                   Resp =  _dp.UserCreated.ToString(),//"WTFWH001",
                                   Rpdt = DateFuntion.dateInterface_us(_dp.DocumentApproveDate.Value),//20171116
                                   Rptm = _dp.DocumentApproveDate.Value.ToString("HHmm"),//1230
                                   Ituom = _unitbase.Name,
                                   Dispatchtypeid = _dp.DocumentId,
                                   IsSentInterface = false
                               }
                               ).ToList();

                     
                    ret.ForEach(item =>
                    {
                        var useraccount = UserAccoutService.Query().Filter(x => x.UserID == new Guid(item.Resp)).Get().FirstOrDefault();//"WTFWH001",
                        if (useraccount != null)
                        {
                            item.Resp = useraccount.UserName;
                        }
                        if (item.Resp.Length > 10)
                        {
                            item.Resp = item.Resp.Substring(0, 10);
                        }
                        itf_temp_in_dispatch_logService.Add(item);
                    });
                    //var result = itf_temp_in_dispatch_logService.AddRange(ret);

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
        #endregion

        #endregion

        #region Function
        public static string GetDispatchEnumDescription(DispatchStatusEnum status)
        {

            Type type = typeof(DispatchStatusEnum);
            MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;

            return description;
        }

        public static List<BookingDispatchModel> GetLotAndDurationByQty_Fix(List<BookingDispatchModel> _listdata, decimal _qty, int lot, int duration)
        {
            //Group Lot
            IEnumerable<BookingDispatchModel> _lotall = (from _data in _listdata
                                                         select new { GroupLot = _data.GroupLot, ManufacturingDate = _data.ManufacturingDate, StockQuantity = _data.StockQuantity } into g
                                                         group g by new { GroupLot = g.GroupLot } into s
                                                         select new BookingDispatchModel { GroupLot = s.Key.GroupLot, ManufacturingDate = s.Min(x => x.ManufacturingDate), StockQuantity = s.Sum(x => x.StockQuantity) }
                           );
            //Map Group
            List<GroupLotModel> _mainlist = new List<GroupLotModel>();
            string _maplist = "";
            List<int> _tempmaplist = new List<int>();
            List<BookingDispatchModel> _lottempLoop1 = _lotall.OrderBy(x => x.GroupLot).ToList();
            List<BookingDispatchModel> _lottempLoop2 = _lottempLoop1; //_lotall.Where(x => x.GroupLot != 1).OrderBy(x => x.GroupLot).ToList();
            List<BookingDispatchModel> _lottempLoop3 = _lottempLoop1; //_lotall.Where(x => x.GroupLot != 1 && x.GroupLot != 2).OrderBy(x => x.GroupLot).ToList();
            List<BookingDispatchModel> _lottempLoop4 = _lottempLoop1; //_lotall.Where(x => x.GroupLot != 1 && x.GroupLot != 2 && x.GroupLot != 3).OrderBy(x => x.GroupLot).ToList();
            List<BookingDispatchModel> _lottempLoop5 = _lottempLoop1; //_lotall.Where(x => x.GroupLot != 1 && x.GroupLot != 2 && x.GroupLot != 3 && x.GroupLot != 4).OrderBy(x => x.GroupLot).ToList();

            foreach (BookingDispatchModel a in _lottempLoop1)
            {

                if (lot == 1)// 1 Lot Only
                {
                    decimal _sum = _listdata.Where(x => x.GroupLot == a.GroupLot).Sum(s => s.StockQuantity);
                    if (_sum >= _qty)
                    {
                        return _listdata.Where(x => x.GroupLot == a.GroupLot).OrderBy(o => o.GroupLot).ToList();
                    }
                    continue;
                }

                //More than 1 Lot 

                if (_tempmaplist.Count < lot)
                {
                    _tempmaplist.Add(a.GroupLot);
                }
                else
                {
                    _tempmaplist = new List<int>
                    {
                        a.GroupLot
                    };
                }


                if (lot > 1)
                {//Loop Lot 2
                    foreach (BookingDispatchModel b in _lottempLoop2)
                    {
                        if (a.GroupLot == b.GroupLot || _tempmaplist.Where(x => x.ToString().Contains(b.GroupLot.ToString())).Count() > 0)
                        {
                            continue;
                        }

                        if (_tempmaplist.Count < lot)
                        {

                            if (duration == 0)//No Check Duration
                            {
                                _tempmaplist.Add(b.GroupLot);
                            }
                            else// Check Duration
                            {
                                int _day = Math.Abs((int)(a.ManufacturingDate - b.ManufacturingDate).TotalDays);
                                if (_day <= duration)
                                {
                                    _tempmaplist.Add(b.GroupLot);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            _tempmaplist = new List<int>
                            {
                                a.GroupLot,
                                b.GroupLot
                            };
                        }


                        if (lot > 2)//Loop Lot 3
                        {
                            foreach (BookingDispatchModel c in _lottempLoop3)
                            {
                                if (b.GroupLot == c.GroupLot || _tempmaplist.Where(x => x.ToString().Contains(c.GroupLot.ToString())).Count() > 0)
                                {
                                    continue;
                                }
                                if (_tempmaplist.Count < lot)
                                {
                                    if (duration == 0)
                                    {
                                        _tempmaplist.Add(c.GroupLot);
                                    }
                                    else
                                    {
                                        int _day = Math.Abs((int)(a.ManufacturingDate - c.ManufacturingDate).TotalDays);
                                        int _day2 = Math.Abs((int)(b.ManufacturingDate - c.ManufacturingDate).TotalDays);
                                        if (_day <= duration && _day2 <= duration)
                                        {
                                            _tempmaplist.Add(c.GroupLot);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    _tempmaplist = new List<int>
                                    {
                                        a.GroupLot,
                                        b.GroupLot,
                                        c.GroupLot
                                    };
                                }

                                if (lot > 3)//Loop Lot 3
                                {
                                    foreach (BookingDispatchModel d in _lottempLoop4)
                                    {
                                        if (c.GroupLot == d.GroupLot || _tempmaplist.Where(x => x.ToString().Contains(d.GroupLot.ToString())).Count() > 0)
                                        {
                                            continue;
                                        }
                                        if (_tempmaplist.Count < lot)
                                        {
                                            if (duration == 0)
                                            {
                                                _tempmaplist.Add(d.GroupLot);
                                            }
                                            else
                                            {
                                                int _day = Math.Abs((int)(a.ManufacturingDate - d.ManufacturingDate).TotalDays);
                                                int _day2 = Math.Abs((int)(b.ManufacturingDate - d.ManufacturingDate).TotalDays);
                                                int _day3 = Math.Abs((int)(c.ManufacturingDate - d.ManufacturingDate).TotalDays);

                                                if (_day <= duration && _day2 <= duration && _day3 <= duration)
                                                {
                                                    _tempmaplist.Add(d.GroupLot);
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _tempmaplist = new List<int>
                                            {
                                                a.GroupLot,
                                                b.GroupLot,
                                                c.GroupLot,
                                                d.GroupLot
                                            };
                                        }

                                        if (lot > 4)//Loop Lot 4
                                        {

                                        }
                                        else
                                        {
                                            _maplist = string.Join(",", _tempmaplist.OrderBy(o => o));
                                            string[] _fillterlot = _maplist.Split(',');
                                            int checkduplicate = _mainlist.Where(x => x.GroupLotList == _maplist).Count();
                                            if (checkduplicate == 0)
                                            {
                                                _mainlist.Add(new GroupLotModel { GroupLotList = _maplist });

                                                decimal _sum = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);

                                                if (_sum >= _qty)
                                                {
                                                    return _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
                                                }

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    _maplist = string.Join(",", _tempmaplist.OrderBy(o => o));
                                    string[] _fillterlot = _maplist.Split(',');
                                    int checkduplicate = _mainlist.Where(x => x.GroupLotList == _maplist).Count();
                                    if (checkduplicate == 0)
                                    {
                                        _mainlist.Add(new GroupLotModel { GroupLotList = _maplist });

                                        decimal _sum = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);

                                        if (_sum >= _qty)
                                        {
                                            return _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
                                        }

                                    }

                                }
                            }
                        }
                        else
                        {
                            _maplist = string.Join(",", _tempmaplist.OrderBy(o => o));
                            string[] _fillterlot = _maplist.Split(',');
                            int checkduplicate = _mainlist.Where(x => x.GroupLotList == _maplist).Count();
                            if (checkduplicate == 0)
                            {
                                _mainlist.Add(new GroupLotModel { GroupLotList = _maplist });

                                decimal _sum = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);

                                if (_sum >= _qty)
                                {
                                    return _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
                                }

                            }

                        }
                    }
                }

            }

            List<GroupLotModel> ret = _mainlist;

            return new List<BookingDispatchModel>();

        }

        public static List<BookingDispatchModel> GetLotAndDurationByQty_Dynamic(List<BookingDispatchModel> _listdata, decimal _qty, int lot, int duration)
        {
            //Group Lot
            IEnumerable<BookingDispatchModel> _lotall = (from _data in _listdata
                                                         select new { GroupLot = _data.GroupLot, ManufacturingDate = _data.ManufacturingDate, StockQuantity = _data.StockQuantity } into g
                                                         group g by new { GroupLot = g.GroupLot } into s
                                                         select new BookingDispatchModel { GroupLot = s.Key.GroupLot, ManufacturingDate = s.Min(x => x.ManufacturingDate), StockQuantity = s.Sum(x => x.StockQuantity) }
                           );
            //Map Group Lot
            List<GroupLotModel> _mainlist = new List<GroupLotModel>();
            List<BookingDispatchModel> _maxqtylist = new List<BookingDispatchModel>();
            string _maplist = "";
            List<int> _tempmaplist = new List<int>();
            List<decimal> _tempsumlist = new List<decimal>();

            List<List<int>> _collections = new List<List<int>>();
            List<int> _group = new List<int>();
            foreach (BookingDispatchModel item in _lotall)
            {
                _group.Add(item.GroupLot);
            }

            for (int i = 1; i <= lot; i++)
            {
                _collections.Add(_group);
            }
            //Use Cartesian Product Fomula
            IEnumerable<IEnumerable<int>> result = _collections
            .Select(list => list.AsEnumerable())
            .CartesianProduct();

            //Fillter Group Lot Step 1
            string[] _fillterlot;
            foreach (IEnumerable<int> a in result)
            {
                int[] _item = a.ToArray();
                foreach (int b in _item)
                {
                    _tempmaplist.Add(b);
                }

                _maplist = string.Join(",", _tempmaplist.OrderBy(o => o).Distinct());
                _fillterlot = _maplist.Split(',');
                int checkduplicate = _mainlist.Where(x => x.GroupLotList == _maplist).Count();
                if (checkduplicate == 0)
                {
                    _mainlist.Add(new GroupLotModel { GroupLotList = _maplist, GroupIndex = _fillterlot.Count() });
                }
                _tempmaplist = new List<int>();
            }

            //Fillter Group Lot Step 1 and Sort GroupLot Index
            IOrderedEnumerable<GroupLotModel> _ItemList = _mainlist.OrderBy(o => o.GroupIndex);//(lot == 1 ? _mainlist.OrderBy(o => o.GroupIndex) : _mainlist.Where(x => x.GroupIndex != 1).OrderBy(o => o.GroupIndex));

            //Sum Group Lot 

            foreach (GroupLotModel item in _ItemList)
            {
                _fillterlot = item.GroupLotList.Split(',');
                if (duration == 0)// No Check Dulation 
                {
                    item.GroupSumQTY = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);
                }
                else
                {//  Check Dulation 
                    List<BookingDispatchModel> _compare = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
                    bool checkdate = true;
                    foreach (BookingDispatchModel a in _compare)
                    {
                        foreach (BookingDispatchModel b in _compare)
                        {
                            if (a.GroupLot != b.GroupLot)
                            {
                                int _day = Math.Abs((int)(a.ManufacturingDate - b.ManufacturingDate).TotalDays);
                                if (_day > duration)
                                {
                                    checkdate = false;
                                    break;
                                }

                            }
                        }
                        if (!checkdate)
                        {
                            break;
                        }
                    }

                    if (checkdate)
                    {
                        item.GroupSumQTY = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);
                    }

                }
            }
            //หา Group Lot ที่มี Qty มากที่สุด
            // decimal _maxqty = _ItemList.Max(m => m.GroupSumQTY );
            // var _tempitem = _ItemList.Where(x => x.GroupSumQTY == _maxqty).FirstOrDefault();
            GroupLotModel _tempitem = new GroupLotModel();
            GroupLotModel _ck = _ItemList.Where(x => x.GroupSumQTY >= _qty).FirstOrDefault();
            if (_ck != null)
            {
                _tempitem = _ItemList.Where(x => x.GroupSumQTY >= _qty).FirstOrDefault();
            }
            else
            {
                _tempitem.GroupSumQTY = 0;
            }


            // var _tempitem_list = _ItemList.Where(x => x.GroupSumQTY == _maxqty).ToList();
            // var _tempitem = _ItemList.Where(x => x.GroupSumQTY == _maxqty).SingleOrDefault();

            if (_tempitem.GroupSumQTY == 0)
            {
                return new List<BookingDispatchModel>();
            }
            else
            {
                _fillterlot = _tempitem.GroupLotList.Split(',');
                return _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
            }


        }

        public static List<BookingDispatchModel> GetLotAndDurationByQty_Dynamic_WithOldData(List<BookingDispatchModel> _listdata, decimal _qty, int lot, int duration)
        {
            //Group Lot

            List<BookingDispatchModel> _lotall_old = (from _data in _listdata
                                                      where _data.PalletCode == null
                                                      select new { GroupLot = _data.GroupLot, ManufacturingDate = _data.ManufacturingDate, StockQuantity = _data.StockQuantity } into g
                                                      group g by new { GroupLot = g.GroupLot } into s
                                                      select new BookingDispatchModel { GroupLot = s.Key.GroupLot, ManufacturingDate = s.Min(x => x.ManufacturingDate), StockQuantity = s.Sum(x => x.StockQuantity) }
               ).ToList();




            List<BookingDispatchModel> _lotall = (from _data in _listdata
                                                  select new { GroupLot = _data.GroupLot, ManufacturingDate = _data.ManufacturingDate, StockQuantity = _data.StockQuantity } into g
                                                  group g by new { GroupLot = g.GroupLot } into s
                                                  select new BookingDispatchModel { GroupLot = s.Key.GroupLot, ManufacturingDate = s.Min(x => x.ManufacturingDate), StockQuantity = s.Sum(x => x.StockQuantity) }
                           ).ToList();

            //Map Group Lot
            List<GroupLotModel> _mainlist = new List<GroupLotModel>();
            List<BookingDispatchModel> _maxqtylist = new List<BookingDispatchModel>();
            string _maplist = "";
            List<int> _tempmaplist = new List<int>();
            List<decimal> _tempsumlist = new List<decimal>();

            List<List<int>> _collections = new List<List<int>>();
            List<int> _group = new List<int>();
            List<int> _group_old = new List<int>();

            //foreach (var item in _lotall_old)
            //{
            //    _group_old.Add(item.GroupLot);
            //}

            //var maplistold = String.Join(",", _group_old.OrderBy(o => o).Distinct());

            foreach (BookingDispatchModel item in _lotall)
            {
                _group.Add(item.GroupLot);
            }

            for (int i = 1; i <= lot; i++)
            {
                _collections.Add(_group);
            }
            //Use Cartesian Product Fomula
            IEnumerable<IEnumerable<int>> result = _collections
            .Select(list => list.AsEnumerable())
            .CartesianProduct();

            //Fillter Group Lot Step 1
            bool _break = true;
            string[] _fillterlot;
            foreach (IEnumerable<int> a in result)
            {
                int[] _item = a.ToArray();
                foreach (int b in _item)
                {
                    _tempmaplist.Add(b);
                }

                if (_lotall_old.Count() > 0)
                {

                    foreach (BookingDispatchModel item in _lotall_old)
                    {
                        List<int> checkoldlot = _tempmaplist.Where(x => x.ToString().Contains(item.GroupLot.ToString())).ToList();
                        if (checkoldlot.Count() == 0)
                        {
                            _break = false;
                            break;
                        }
                    }

                }
                else
                {
                    _break = true;
                }

                if (_break)
                {
                    _maplist = string.Join(",", _tempmaplist.OrderBy(o => o).Distinct());
                    _fillterlot = _maplist.Split(',');
                    int checkduplicate = _mainlist.Where(x => x.GroupLotList == _maplist).Count();
                    if (checkduplicate == 0)
                    {
                        _mainlist.Add(new GroupLotModel { GroupLotList = _maplist, GroupIndex = _fillterlot.Count() });
                    }
                }

                _break = true;
                _tempmaplist = new List<int>();


            }

            //Fillter Group Lot Step 1 and Sort GroupLot Index
            IOrderedEnumerable<GroupLotModel> _ItemList = _mainlist.OrderBy(o => o.GroupIndex);//(lot == 1 ? _mainlist.OrderBy(o => o.GroupIndex) : _mainlist.Where(x => x.GroupIndex != 1).OrderBy(o => o.GroupIndex));

            //Sum Group Lot 

            foreach (GroupLotModel item in _ItemList)
            {
                _fillterlot = item.GroupLotList.Split(',');
                if (duration == 0)// No Check Dulation 
                {
                    item.GroupSumQTY = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);
                }
                else
                {//  Check Dulation 
                    List<BookingDispatchModel> _compare = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
                    bool checkdate = true;
                    foreach (BookingDispatchModel a in _compare)
                    {
                        foreach (BookingDispatchModel b in _compare)
                        {
                            if (a.GroupLot != b.GroupLot)
                            {
                                int _day = Math.Abs((int)(a.ManufacturingDate - b.ManufacturingDate).TotalDays);
                                if (_day > duration)
                                {
                                    checkdate = false;
                                    break;
                                }

                            }
                        }
                        if (!checkdate)
                        {
                            break;
                        }
                    }

                    if (checkdate)
                    {
                        item.GroupSumQTY = _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).Sum(s => s.StockQuantity);
                    }

                }
            }

            if (_ItemList.Count() == 0)
            {
                return new List<BookingDispatchModel>();
            }

            //หา Group Lot ที่มี Qty มากที่สุด
            decimal _maxqty = _ItemList.Max(m => m.GroupSumQTY);
            GroupLotModel _tempitem = _ItemList.Where(x => x.GroupSumQTY == _maxqty).SingleOrDefault();
            _fillterlot = _tempitem.GroupLotList.Split(',');

            if (_tempitem.GroupSumQTY == 0)
            {
                return new List<BookingDispatchModel>();
            }
            else
            {
                return _listdata.Where(x => _fillterlot.Contains(x.GroupLot.ToString())).OrderBy(o => o.GroupLot).ToList();
            }


        }
         
        public bool OnCancelDispatch(string dispatchcode, string pono)
        {
            throw new NotImplementedException();
        }

        public bool OnCancelDispatchInternal(string dispatchcode, string pono)
        {
            throw new NotImplementedException();
        }

        public bool OnCancelDispatchPicking(string dispatchcode, string pono)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
    public class PickSwap
    {
        public string PalletCode { get; set; }
        public string RefPalletCode { get; set; }
        public Guid LocationID { get; set; }
        public decimal StockQuantity { get; set; }
    }
}