using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Stock;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.PutAwayModel;
using DITS.HILI.WMS.PutAwayService;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;


namespace DITS.HILI.WMS.ReceiveService
{
    public class ReceiveServiceHH : Repository<Receive>, IReceiveServiceHH
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
        private readonly IRepository<ReceiveAssignJob> assignjob;
        private readonly IRepository<ReceivePrefix> prefixService;
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
        private readonly IStockService stockService;
        private readonly IPutAwayService putawayService;
        private readonly IPutAwayService putawayServiceII;
        #endregion

        #region [ Constructor ]

        public ReceiveServiceHH(IUnitOfWork context,
                              IRepository<ReceiveDetail> _receiveDetail,
                              IRepository<Product> _product,
                              IRepository<Contact> _contact,
                              IRepository<ProductOwner> _productOwner,
                              IRepository<DocumentType> _documenType,
                              IRepository<Location> _location,
                              IRepository<ProductUnit> _productUnit,
                              IRepository<ProductStatus> _productStatus,
                              IRepository<ProductSubStatus> _productSubStatus,
                              IRepository<Receiving> _receiving,
                              IRepository<ProductCodes> _productCode,
                              IRepository<ReceiveAssignJob> _assign,
                              IRepository<ReceivePrefix> _prefix,
                              IRepository<Employee> _emp,
                              IRepository<Line> _line,
                              //IRepository<ProductionPlan> _productionPlan,
                              IRepository<ProductionPlanDetail> _productionPlanDetail,
                              IRepository<ProductionControl> _productionControl,
                              IStockService _stockService,
                              IPutAwayService _putawayService,
                              IPutAwayService _putawayServiceII)
            : base(context)
        {
            unitofwork = context;
            receiveDetailService = _receiveDetail;
            productService = _product;
            documentTypeService = _documenType;
            contactService = _contact;
            locationService = _location;
            productUnitService = _productUnit;
            productStatusService = _productStatus;
            productSubStatusService = _productSubStatus;
            receivingService = _receiving;
            productCodeService = _productCode;
            assignjob = _assign;
            prefixService = _prefix;
            productOwnerService = _productOwner;
            employeeService = _emp;
            lineService = _line;
            productionPlanDetailService = _productionPlanDetail;
            productionControlService = _productionControl;
            stockService = _stockService;
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
            putawayService = _putawayService;
            putawayServiceII = _putawayServiceII;
        }

        #endregion
        public PalletTagModel GetReceivingByPalletCode(string palletCode, List<ReceivingStatusEnum> status)
        {
            try
            {
                SqlParameter param = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = palletCode };
                PalletTagModel detail = unitofwork.SQLQuery<PalletTagModel>("exec SP_GetReceivingByPalletCode @palletCode ", param).SingleOrDefault();
                return detail;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, Framework.LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, Framework.LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool ReceivePallet(string palletCode, decimal receiveQty, string suggestLocation)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Receiving reciving = receivingService.FirstOrDefault(x => x.PalletCode == palletCode);
                    if (reciving == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    ProductionControlDetail packing_detail = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                    if (packing_detail == null)
                    {
                        throw new HILIException("MSG00072");
                    }

                    if (packing_detail.PackingStatus != PackingStatusEnum.Waiting_Receive)
                    {
                        throw new HILIException("MSG00090");
                    }

                    Receive receive = FirstOrDefault(x => x.ReceiveID == reciving.ReceiveID);
                    if (reciving == null)
                    {
                        throw new HILIException("MSG00075");
                    }

                    ReceiveDetail receive_detail = receiveDetailService.FirstOrDefault(x => x.ReceiveDetailID == reciving.ReceiveDetailID);
                    if (receive_detail == null)
                    {
                        throw new HILIException("MSG00076");
                    }

                    receive.ReceiveStatus = ReceiveStatusEnum.Partial;
                    receive.UserModified = UserID;
                    receive.DateModified = DateTime.Now;

                    Modify(receive);

                    receive_detail.ReceiveDetailStatus = ReceiveDetailStatusEnum.Partial;
                    receive_detail.UserModified = UserID;
                    receive_detail.DateModified = DateTime.Now;

                    receiveDetailService.Modify(receive_detail);


                    Location location_loading_in = locationService.Query().Filter(x => x.LocationID == reciving.LocationID).Include(x => x.Zone).Get().FirstOrDefault();
                     

                    if (location_loading_in == null)
                    {
                        throw new HILIException("MSG00077");
                    }

                    reciving.ReceivingStatus = ReceivingStatusEnum.WaitApprove;
                    //reciving.Quantity = receiveQty;
                    //reciving.BaseQuantity = reciving.ConversionQty * receiveQty;
                    reciving.UserModified = UserID;
                    reciving.DateModified = DateTime.Now;

                    receivingService.Modify(reciving);
                    Location location_suggest = locationService.FirstOrDefault(x => x.Code == suggestLocation);

                    if (location_suggest == null)
                    {
                        throw new HILIException("MSG00040");
                    }

                    packing_detail.StockQuantity = receiveQty;
                    packing_detail.RemainBaseQTY = receiveQty * packing_detail.ConversionQty;
                    packing_detail.RemainBaseUnitID = reciving.BaseUnitID;
                    packing_detail.RemainStockUnitID = reciving.StockUnitID;
                    packing_detail.RemainQTY = receiveQty;
                    packing_detail.PackingStatus = PackingStatusEnum.Loading_In;
                    packing_detail.UserModified = UserID;
                    packing_detail.DateModified = DateTime.Now;
                    packing_detail.SugguestLocationID = location_suggest.LocationID;
                    pcDetailService.Modify(packing_detail);



                    List<StockInOutModel> stockIn = new List<StockInOutModel>
                    {
                        new StockInOutModel
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
                            Quantity = receiveQty,
                            PalletCode = reciving.PalletCode,
                            LocationCode = location_loading_in.Code,
                            DocumentCode = receive.ReceiveCode,//reciving.GRNCode,
                            DocumentTypeID = receive.ReceiveTypeID,
                            DocumentID = reciving.ReceivingID
                        }
                    };
                    stockService.UserID = UserID;
                    stockService.Incomming(stockIn);
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
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool ConfirmKeep(string palletCode, decimal receiveQty, string locationCode)
        {
            try
            {
                Receiving reciving = receivingService.FirstOrDefault(x => x.PalletCode == palletCode);

                if (reciving == null)
                {
                    throw new HILIException("MSG00072");
                }

                ProductionControlDetail pc = pcDetailService.FirstOrDefault(x => x.PalletCode == palletCode);
                if (pc == null)
                {
                    throw new HILIException("MSG00072");
                }

                Receive receive = FindByID(reciving.ReceiveID); //Query().Filter(x => x.ReceiveID == reciving.ReceiveID).Get().FirstOrDefault();
                if (receive == null)
                {
                    throw new HILIException("MSG00075");
                }

                ReceiveDetail receive_detail = receiveDetailService.FirstOrDefault(x => x.ReceiveDetailID == reciving.ReceiveDetailID);
                if (receive_detail == null)
                {
                    throw new HILIException("MSG00076");
                }

                Location confirm_location = locationService.FirstOrDefault(x => x.Code == locationCode);

                if (confirm_location == null)
                {
                    throw new HILIException("MSG00040");
                }

                if (confirm_location.LocationReserveQty >= confirm_location.PalletCapacity)
                {
                    throw new HILIException("MSG00078");
                }
                if (receive_detail.ReceiveDetailStatus == ReceiveDetailStatusEnum.Complete)
                {
                    throw new HILIException("MSG00090");
                }

                SqlParameter param = new SqlParameter("@PalletCode", SqlDbType.NVarChar) { Value = palletCode };
                SqlParameter param2 = new SqlParameter("@receiveQty", SqlDbType.Decimal) { Value = receiveQty };
                SqlParameter param3 = new SqlParameter("@locationCode", SqlDbType.NVarChar) { Value = locationCode };
                SqlParameter param4 = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = UserID };

                bool isSuccess = unitofwork.SQLQuery<bool>("exec SP_ConfirmKeep @palletCode, @receiveQty, @locationCode, @UserID ", param, param2, param3, param4).FirstOrDefault();

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
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

    }
}
