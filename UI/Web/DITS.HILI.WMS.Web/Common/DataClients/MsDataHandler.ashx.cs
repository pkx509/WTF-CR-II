using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.TruckQueueModel;
using DITS.WMS.Data.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace DITS.HILI.WMS.Web.Common.DataClients
{
    /// <summary>
    /// Summary description for DataClientHandler
    /// </summary>
    public class DataClientHandler : HttpTaskAsyncHandler, IRequiresSessionState
    {
        public override async System.Threading.Tasks.Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            switch (context.Request.QueryString["Method"])
            {
                case "product":
                    await getProduct(context);
                    break;
                case "productbystockcode":
                    await getProductByStockCode(context);
                    break;
                case "Unit":
                    await getUnit(context);
                    break;
                case "location":
                    await getLocation(context);
                    break;
                case "Warehouse":
                    await getWarehouse(context);
                    break;
                case "WarehouseMk":
                    await getWarehouseMk(context);
                    break;
                case "WarehouseType":
                    await getWarehouseType(context);
                    break;
                case "Zone":
                    await getZone(context);
                    break;
                case "ZoneType":
                    await getZoneType(context);
                    break;
                case "LogicalZone":
                    await getLogicalZone(context);
                    break;
                case "Site":
                    await getSite(context);
                    break;
                case "Province":
                    await getProvince(context);
                    break;
                case "ProvinceWithoutpage":
                    await getProvinceWithoutpage(context);
                    break;
                case "District":
                    await getDistict(context);
                    break;
                case "SubDistrict":
                    await getSupDistict(context);
                    break;
                case "Line":
                    await getLine(context);
                    break;
                case "TruckType":
                    await GetDock(context);
                    break;
                case "TruckTypeOnly":
                    await getTruckTypeOnly(context);
                    break;
                case "PhysicalZone":
                    await getPhysicalZone(context);
                    break;
                case "ZoneCombo":
                    await getZoneCombo(context);
                    break;
                case "ProductGroupLevel":
                    await getProductGroupLevel(context);
                    break;
                case "ProductGroupLevel2":
                    await getProductGroupLevel2(context);
                    break;
                case "ProductBrand":
                    await getProductBrand(context);
                    break;
                case "ProductShape":
                    await getProductShape(context);
                    break;
                case "UnitCombo":
                    await getUnitCombo(context);
                    break;
                case "ProductTemplateUom":
                    await getProductTemplateUom(context);
                    break;
                case "ReceiveType":
                    await getReceiveType(context);
                    break;
                case "InternalReceiveType":
                    await getInternalReceiveType(context);
                    break;
                case "ProductOwner":
                    await getProductOwner(context);
                    break;
                case "Printer":
                    await getPrinter(context);
                    break;
                case "PrinterMachine":
                    await getPrinterMachine(context);
                    break;
                case "PrinterLocation":
                    await getPrinterLocation(context);
                    break;
                case "ProductStatusByDocType":
                    await getProductStatusByDocType(context);
                    break;
                case "ProductUnit":
                    await getProductUnit(context);
                    break;
                case "LocationByLine":
                    await getLocationByLine(context);
                    break;
                case "LocationByZoneWarehouse":
                    await getLocationByZoneWarehouse(context);
                    break;
                case "Conditionconfig":
                    await getConditionConfig(context);
                    break;
                case "Customer":
                    await getCustomer(context);
                    break;
                case "Supplier":
                    await getSupplier(context);
                    break;
                case "DispatchType":
                    await getDispatchType(context);
                    break;
                case "DispatchTypeWithAll":
                    await getDispatchTypeWithAll(context);
                    break;
                case "ShipTo":
                    await getShipTo(context);
                    break;
                case "ShipToWithOutPage":
                    await getShipToWithOutPage(context);
                    break;
                case "ShipFrom":
                    await getShippingFromList(context);
                    break;
                case "EmployeeAssign":
                    await EmployeeAssign(context);
                    break;
                case "CycleCountStatus":
                    await CycleCountStatus(context);
                    break;
                case "BookingRule":
                    await BookingRule(context);
                    break;
                case "SpecialBookingRule":
                    await SpecialBookingRule(context);
                    break;
                case "POList":
                    await getPOList(context);
                    break;
                case "QueueDock":
                    await getQueueDockList(context);
                    break;
                case "QueueStatus":
                    await getQueueStatusList(context);
                    break;
                case "RegisterType":
                    await getRegisterTypeList(context);
                    break;
                default: break;
            }

        }

        private async System.Threading.Tasks.Task getQueueDockList(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Queue.QueueClient.GetDockAll(1, string.Empty);
            if (api.IsSuccess)
            {
                List<QueueDock> data = api.Get<List<QueueDock>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        private async System.Threading.Tasks.Task getRegisterTypeList(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Queue.QueueClient.GetQueueTypeAll(1, string.Empty);
            if (api.IsSuccess)
            {
                List<QueueRegisterType> data = api.Get<List<QueueRegisterType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }

        private async System.Threading.Tasks.Task getQueueStatusList(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Queue.QueueClient.GetQueueStatusAll(1, string.Empty);
            if (api.IsSuccess)
            {
                List<QueueStatus> data = api.Get<List<QueueStatus>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }

        private async System.Threading.Tasks.Task getShippingFromList(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Queue.QueueClient.GetShippingFromAll(1, string.Empty, string.Empty);
            if (api.IsSuccess)
            {
                List<ShippingFrom> data = api.Get<List<ShippingFrom>>().OrderBy(e => e.Name).ToList();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
         
        public async System.Threading.Tasks.Task getProduct(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductClient.Get(_query, null, null, null, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Product> data = api.Get<List<Product>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }

        }
        public async System.Threading.Tasks.Task getProductByStockCode(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ProductClient.GetByStockCode(_query, null, null, null, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Product> data = api.Get<List<Product>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }

        }
        public async System.Threading.Tasks.Task getLocation(HttpContext context)
        {
            string _query = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                _query = context.Request["query"];
            }

            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            //var data = await DITS.HILI.WMS.ClientService.Master.WarehouseClient.GetLocationAll(_query, total, null, null);
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetLocationAll(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Location> data = api.Get<List<Location>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }


        }
        public async System.Threading.Tasks.Task getWarehouse(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetWarehouse(null, search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Warehouse> data = api.Get<List<Warehouse>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }

        }
        public async System.Threading.Tasks.Task getWarehouseMk(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetWarehouseMk(null, search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Warehouse> data = api.Get<List<Warehouse>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }

        }
        public async System.Threading.Tasks.Task getWarehouseType(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetCmbWarehouseType(null, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<WarehouseType> data = api.Get<List<WarehouseType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }

        }
        public async System.Threading.Tasks.Task getZoneType(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetZoneType(null, prms.Query, false, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Zone> data = api.Get<List<Zone>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getZone(HttpContext context)
        {
            System.Guid id = new Guid();
            string search = context.Request.QueryString["query"];
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = context.Request.QueryString["query"].ToString().Replace(",", "");
            }
            string WhKey = context.Request.QueryString["WhKey"];
            if (!string.IsNullOrEmpty(WhKey))
            {
                id = new Guid(WhKey);
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetZone(id, search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Zone> data = api.Get<List<Zone>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getZoneCombo(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            //var WhKey = context.Request.QueryString["WhKey"];
            //if (!string.IsNullOrEmpty(WhKey))
            //{
            //    id = new Guid(WhKey);
            //}
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetZoneCombo(null, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ZoneModel> data = api.Get<List<ZoneModel>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getUnitCombo(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            //var WhKey = context.Request.QueryString["WhKey"];
            //if (!string.IsNullOrEmpty(WhKey))
            //{
            //    id = new Guid(WhKey);
            //}
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.UnitsClient.GetUnitCombo(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Units> data = api.Get<List<Units>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getPrinter(HttpContext context)
        {
            Guid lineID = Guid.Empty; // Optional
            bool isGroupName = false;

            if (!string.IsNullOrEmpty(context.Request["lineID"]))
            {
                Guid.TryParse(context.Request["lineID"], out lineID);
            }

            if (!string.IsNullOrEmpty(context.Request["isGroupName"]))
            {
                bool.TryParse(context.Request["isGroupName"], out isGroupName);
            }

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.PrinterClient.Get(prms.Query, prms.Page, prms.Limit, lineID);

            if (api.IsSuccess)
            {
                List<PrinterModel> data = api.Get<List<PrinterModel>>();

                if (isGroupName)
                {
                    var groupData = data.GroupBy(g => new
                    {
                        g.PrinterName
                    }).Select(x => new
                    {
                        x.Key.PrinterName
                    });

                    total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(groupData), total.Value));
                }
                else
                {
                    total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
                }
            }
        }
        public async System.Threading.Tasks.Task getPrinterMachine(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            //var WhKey = context.Request.QueryString["WhKey"];
            //if (!string.IsNullOrEmpty(WhKey))
            //{
            //    id = new Guid(WhKey);
            //}
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.PrinterClient.GetPrinterMachine(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<BasePrinter> data = api.Get<List<BasePrinter>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getPrinterLocation(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            //var WhKey = context.Request.QueryString["WhKey"];
            //if (!string.IsNullOrEmpty(WhKey))
            //{
            //    id = new Guid(WhKey);
            //}
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.PrinterClient.GetPrinterLocation(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Line> data = api.Get<List<Line>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task GetDock(HttpContext context)
        {
            DockConfigModel _Truck = new DockConfigModel();
            string search = context.Request.QueryString["query"];
            string WhKey = context.Request.QueryString["WhKey"];
            if (!string.IsNullOrEmpty(WhKey))
            {
                _Truck.TruckTypeID = new Guid(WhKey);
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await WarehouseClient.GetDockAll(_Truck.TruckTypeID, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<DockConfigModel> data = api.Get<List<DockConfigModel>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getTruckTypeOnly(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await WarehouseClient.GetTruckType(prms.Query, false, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<TruckType> data = api.Get<List<TruckType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getPhysicalZone(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetPhysicalZoneCombo(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<PhysicalZone> data = api.Get<List<PhysicalZone>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getSite(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.SiteClient.GetCmbSite(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Site> data = api.Get<List<Site>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getLine(HttpContext context)
        {

            LineTypeEnum lineType = LineTypeEnum.NP;

            if (!string.IsNullOrEmpty(context.Request["LineType"]))
            {
                lineType = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), context.Request["LineType"]);
            }
            else
            {
                lineType = LineTypeEnum.All;
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.DailyPlan.ProductionLineClient.GetLine(prms.Query, false, lineType, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Line> data = api.Get<List<Line>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProvince(HttpContext context)
        {
            System.Guid? id = new Guid();
            string search = context.Request.QueryString["query"];
            string provinceid = context.Request.QueryString["provinceid"];
            if (!string.IsNullOrEmpty(provinceid))
            {
                id = new Guid(provinceid);
            }
            else
            {
                id = null;
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ContactClient.GetProvince(id, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Province> data = api.Get<List<Province>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProvinceWithoutpage(HttpContext context)
        {
            System.Guid? id = new Guid();
            string search = context.Request.QueryString["query"];
            string provinceid = context.Request.QueryString["provinceid"];
            if (!string.IsNullOrEmpty(provinceid))
            {
                id = new Guid(provinceid);
            }
            else
            {
                id = null;
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ContactClient.GetProvince(id, prms.Query, null, null);
            if (api.IsSuccess)
            {
                List<Province> data = api.Get<List<Province>>().OrderBy(e => e.Name).ToList();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getDistict(HttpContext context)
        {
            System.Guid id = new Guid();
            string search = context.Request.QueryString["query"];
            string provinceid = context.Request.QueryString["provinceid"];
            if (!string.IsNullOrEmpty(provinceid))
            {
                id = new Guid(provinceid);
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ContactClient.GetDistrict(id, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<District> data = api.Get<List<District>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getSupDistict(HttpContext context)
        {
            System.Guid id = new Guid();
            string search = context.Request.QueryString["query"];
            string districtid = context.Request.QueryString["districtid"];
            if (!string.IsNullOrEmpty(districtid))
            {
                id = new Guid(districtid);
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ContactClient.GetSubDistrict(id, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<SubDistrict> data = api.Get<List<SubDistrict>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getUnit(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await UnitsClient.GetUnit(search, false, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Units> data = api.Get<List<Units>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductGroupLevel(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductGroupLevelClient.GetProductGroupLevel(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ProductGroupLevel3> data = api.Get<List<ProductGroupLevel3>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductGroupLevel2(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductGroupLevelClient.GetProductGroupLevel2(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ProductGroupLevel2> data = api.Get<List<ProductGroupLevel2>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductBrand(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductGroupLevelClient.Get(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ProductBrand> data = api.Get<List<ProductBrand>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductShape(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductShapeClient.Get(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ProductShape> data = api.Get<List<ProductShape>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductUnit(HttpContext context)
        {
            StoreRequestParameters prms = new StoreRequestParameters(context);

            Guid productId = Guid.Empty; // Optional
            Guid productUnitId = Guid.Empty;  // Optional
            bool isQuery = false;
            string query = prms.Query;

            if (!string.IsNullOrEmpty(context.Request["productUnitID"]))
            {
                Guid.TryParse(context.Request["productUnitID"], out productUnitId);
            }
            if (!string.IsNullOrEmpty(context.Request["productID"]))
            {
                Guid.TryParse(context.Request["productID"], out productId);
            }

            if (!string.IsNullOrEmpty(context.Request["isQuery"]))
            {
                bool.TryParse(context.Request["isQuery"], out isQuery);
                if (isQuery)
                {
                    query = string.Empty;
                }
            }

            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductUnitsClient.GetProductUnits(productUnitId, productId, query, prms.Page, prms.Limit);

            if (api.IsSuccess)
            {
                List<ProductUnit> data = api.Get<List<ProductUnit>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductTemplateUom(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ProductTemplateUOMClient.GetProductTemplateUom(prms.Query, false, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ProductTemplateUom> data = api.Get<List<ProductTemplateUom>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getReceiveType(HttpContext context)
        {
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.DocumentTypeClient.GetReceiveType(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<DocumentType> data = api.Get<List<DocumentType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getInternalReceiveType(HttpContext context)
        {
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.DocumentTypeClient.GetForInternalReceive(prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<DocumentTypeCustomModel> data = api.Get<List<DocumentTypeCustomModel>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductOwner(HttpContext context)
        {
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ProductOwnerClient.Get(prms.Query, prms.Page, prms.Limit);

            if (api.IsSuccess)
            {
                List<ProductOwner> data = api.Get<List<ProductOwner>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getProductStatusByDocType(HttpContext context)
        {
            Guid documentTypeID = Guid.Empty; // Optional
            if (!string.IsNullOrEmpty(context.Request["DocumentTypeID"]))
            {
                Guid.TryParse(context.Request["DocumentTypeID"], out documentTypeID);
            }
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ProductStatusClient.Get(prms.Query, prms.Page, prms.Limit, documentTypeID);

            if (api.IsSuccess)
            {
                List<ProductStatus> data = api.Get<List<ProductStatus>>();

                if (!string.IsNullOrEmpty(context.Request["IsInspectionReclassify"]))
                {
                    data = data.Where(x => x.IsInspectionReclassify == true).ToList();
                }
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getLocationByLine(HttpContext context)
        {
            Guid lineGUID = new Guid();
            string search = context.Request.QueryString["query"];
            string lineID = context.Request.QueryString["lineID"];

            if (Guid.TryParse(lineID, out lineGUID))
            {
                StoreRequestParameters prms = new StoreRequestParameters(context);
                Ref<int> total = new Ref<int>();
                Core.Domain.ApiResponseMessage api = await ClientService.DailyPlan.ImportProductionClient.GetLocationByLine(lineGUID, LocationTypeEnum.LoadingIN, prms.Query, prms.Page, prms.Limit);
                if (api.IsSuccess)
                {
                    List<LocationModel> data = api.Get<List<LocationModel>>();
                    total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
                }
            }
        }
        public async System.Threading.Tasks.Task getLocationByZoneWarehouse(HttpContext context)
        {
            System.Guid _zoneId = new Guid();
            System.Guid _warehouseId = new Guid();
            string zoneId = context.Request.QueryString["zoneId"];
            string warehouseId = context.Request.QueryString["warehouseId"];
            string search = string.Empty;
            string replacesearch = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                search = context.Request["query"];
                replacesearch = search.Replace(",", "");
            }
            if (!string.IsNullOrEmpty(zoneId))
            {
                _zoneId = new Guid(zoneId);
            }
            if (!string.IsNullOrEmpty(warehouseId))
            {
                _warehouseId = new Guid(warehouseId);
            }
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetLocation(_zoneId, _warehouseId, replacesearch, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<LocationModel> data = api.Get<List<LocationModel>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getConditionConfig(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            string modulename = "logicalzoneconfig";
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.LogicalZoneClient.GetConditionConfig(modulename, prms.Query, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ConditionConfigModel> data = api.Get<List<ConditionConfigModel>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getLogicalZone(HttpContext context)
        {

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.WarehouseClient.GetLogicalZone(prms.Query, prms.Page, prms.Limit);

            if (api.IsSuccess)
            {
                List<LogicalZoneGroup> data = api.Get<List<LogicalZoneGroup>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getCustomer(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ContactClient.GetCustomer(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Contact> data = api.Get<List<Contact>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getSupplier(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.ContactClient.GetSupplier(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<Contact> data = api.Get<List<Contact>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getDispatchType(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.DocumentTypeClient.GetDispatchType(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<DocumentType> data = api.Get<List<DocumentType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getDispatchTypeWithAll(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ClientService.Master.DocumentTypeClient.GetDispatchTypeWithAll(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<DocumentType> data = api.Get<List<DocumentType>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getShipTo(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ShipToClient.Get(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<ShippingTo> data = api.Get<List<ShippingTo>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task getShipToWithOutPage(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await ShipToClient.Get(search, null, null);
            if (api.IsSuccess)
            {
                List<ShippingTo> data = api.Get<List<ShippingTo>>().OrderBy(e => e.Name).ToList();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task EmployeeAssign(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await PickingClient.GetUserWHGroup(search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<UserGroups> data = api.Get<List<UserGroups>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }

        public async System.Threading.Tasks.Task CycleCountStatus(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await InventoryToolsClient.getCycleCountstatus();
            if (api.IsSuccess)
            {
                List<CustomEnumerable> data = api.Get<List<CustomEnumerable>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }

        public async System.Threading.Tasks.Task BookingRule(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await WarehouseClient.GetBookingRule(search, false, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<SpecialBookingRule> data = api.Get<List<SpecialBookingRule>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }
        public async System.Threading.Tasks.Task SpecialBookingRule(HttpContext context)
        {
            string search = context.Request.QueryString["query"];
            string Active = context.Request.QueryString["Active"];
            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await WarehouseClient.GetBookingRule(search, Convert.ToBoolean(Active), prms.Page, prms.Limit);
            List<SpecialBookingRule> data = new List<SpecialBookingRule>();
            if (api.ResponseCode == "0")
            {
                total = api.Totals;
                data = api.Get<List<SpecialBookingRule>>();
            }
            context.Response.Write(string.Format("{{'data':{0},total:{1}}}", JSON.Serialize(data), total));
        }

        public async System.Threading.Tasks.Task getPOList(HttpContext context)
        {
            Guid docTypeGUID = Guid.Empty;
            string search = context.Request.QueryString["query"];
            string docTypeID = context.Request.QueryString["documentTypeID"];
            string dispatchStatus = context.Request.QueryString["dispatchStatus"];

            Guid.TryParse(docTypeID, out docTypeGUID);

            StoreRequestParameters prms = new StoreRequestParameters(context);
            Ref<int> total = new Ref<int>();
            Core.Domain.ApiResponseMessage api = await DispatchClient.GetPOList(docTypeGUID, dispatchStatus, search, prms.Page, prms.Limit);
            if (api.IsSuccess)
            {
                List<POListModels> data = api.Get<List<POListModels>>();
                total = api.Totals; context.Response.Write(string.Format("{{total:{1},'plants':{0}}}", JSON.Serialize(data), total.Value));
            }
        }

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
    }
}