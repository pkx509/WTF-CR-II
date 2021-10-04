
using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateLogicalZone : BaseUIPage
    {
        public static string ProgramCode = frmAllLogicalZone.ProgramCode;
        private readonly string AutoCompleteService = "../../../Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["LogicalZone.Items"] = null;
                Session["LogicalZone.Configs"] = null;
                //AutoCompleteProxy_Warehouse();
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void AutoCompleteProxy_Warehouse()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Warehouse" },
                { "query", cmbWarehouseName.SelectedItem.Text }
            };
            StoreWarehouseName.PageSize = 20;
            StoreWarehouseName.AutoCompleteProxy(AutoCompleteService, param);
            StoreWarehouseName.LoadProxy();

        }
        private void AutoCompleteProxy_Zone(string warehouseid)
        {
            cmbPhysicalZone.Clear();
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Zone" },
                { "query", cmbPhysicalZone.SelectedItem.Text },
                { "WhKey", warehouseid }
            };
            StorePhysicalZone.PageSize = 20;
            StorePhysicalZone.AutoCompleteProxy(AutoCompleteService, param);
            StorePhysicalZone.LoadProxy();

            cmbPhysicalZone.Enable();
            cmbPhysicalZone.Focus();
        }


        private void BindGrid(List<LogicalZoneDetailModel> list)
        {
            StoreLogicalDetail.DataSource = list.OrderBy(x => x.Seq);
            StoreLogicalDetail.DataBind();
        }


        private void BindGridConfig(List<LogicalZoneConfigModel> configs)
        {
            StoreConfig.DataSource = configs.OrderBy(x => x.ConfigSeq);
            StoreConfig.DataBind();
        }

        private void BindData(string oDataKeyId)
        {
            if (oDataKeyId == "new")
            {
                cmbPhysicalZone.Disable();
                cmbLocationNo.Disable();
                cmbLocationNoTo.Disable();
                return;
            }
            // ID = 0 : Add new
            if (oDataKeyId != "new")
            {

                Core.Domain.ApiResponseMessage apiResp = LogicalZoneClient.GetByID(new Guid(oDataKeyId)).Result;
                LogicalZoneModel data = new LogicalZoneModel();
                if (apiResp.ResponseCode == "0")
                {
                    data = apiResp.Get<LogicalZoneModel>();
                    if (data != null)
                    {
                        txtLogicalZone_Code.Text = data.LogicalZoneID.ToString();
                        txtLogicalZone_Name.Text = data.LogicalZoneName;

                        cmbWarehouseName.ReadOnly = true;

                        //cmbWarehouseName.Value = data.WarehouseID;
                        //cmbWarehouseName.Text = data.WarehouseName;
                        hddwharehouse.Text = data.WarehouseID.ToString();
                        chkIsPallet.Checked = Convert.ToBoolean(data.IsPallet);

                        cmbWarehouseName.SetAutoCompleteValue(
                       new List<Warehouse>
                       {
                                                                new Warehouse
                                                                {
                                                                    WarehouseID = data.WarehouseID,
                                                                    Name = data.WarehouseName
                                                                }
                       }
                       , data.WarehouseID.ToString()
                   );

                        AutoCompleteProxy_Zone(data.WarehouseID.ToString());
                        hddphysicalzone.Text = data.ZoneID.ToString();
                        //this.cmbPhysicalZone.SetAutoCompleteValue(
                        //    new List<Zone>
                        //    {
                        //                            new Zone
                        //                            {
                        //                                ZoneID = data.ZoneID,
                        //                                Name = data.ZoneName
                        //                            }
                        //    }
                        //    , data.ZoneID.ToString()
                        //);
                    }
                }


                Session.Add("LogicalZone.Items", data.LogicalZoneDetailModelCollection.ToList());

                BindGrid(data.LogicalZoneDetailModelCollection.ToList());

                Session.Add("LogicalZone.Configs", data.LogicalZoneConfigModelCollection.ToList());

                BindGridConfig(data.LogicalZoneConfigModelCollection.ToList());


            }
        }

        private bool validate_SaveClick(List<LogicalZoneDetailModel> gridData)
        {



            if (gridData.Count == 0)
            {
                MessageBoxExt.ShowError("No Item");
                return false;
            }

            if (string.IsNullOrEmpty(cmbWarehouseName.SelectedItem.Value))
            {
                MessageBoxExt.ShowError("Please select warehouse.");
                return false;
            }

            //if ( string.IsNullOrEmpty(this.cmbPhysicalZone.SelectedItem.Value))
            //{
            //    MessageBoxExt.ShowError("Please select zone.");
            //    return false;
            //}

            return true;
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {

            if (string.IsNullOrEmpty(cmbWarehouseName.SelectedItem.Value))
            {
                MessageBoxExt.ShowError("Please select warehouse.");
                return;
            }

            if (string.IsNullOrEmpty(cmbPhysicalZone.SelectedItem.Value))
            {
                MessageBoxExt.ShowError("Please select zone.");
                return;
            }

            if (string.IsNullOrEmpty(cmbLocationNo.SelectedItem.Value))
            {
                MessageBoxExt.ShowError("Please select location from.");
                return;
            }

            if (string.IsNullOrEmpty(cmbLocationNoTo.SelectedItem.Value))
            {
                MessageBoxExt.ShowError("Please select location to.");
                return;
            }

            cmbWarehouseName.ReadOnly = true;
            List<LogicalZoneDetailModel> dataList = new List<LogicalZoneDetailModel>();
            if (Session["LogicalZone.Items"] != null)
            {
                dataList = (List<LogicalZoneDetailModel>)Session["LogicalZone.Items"];
            }

            Session.Add("LogicalZone.Items", dataList);
            //int id = 0;
            //if (dataList.Count > 0) id = dataList.Min(x => x.LogicalZoneDetailID);
            //if (id > 0) id = -1;

            Core.Domain.ApiResponseMessage apiResp = WarehouseClient.GetLocationBetweenList(cmbLocationNo.SelectedItem.Text, cmbLocationNoTo.SelectedItem.Text, new Guid(cmbPhysicalZone.SelectedItem.Value)).Result;
            List<LogicalZoneDetailModel> data = new List<LogicalZoneDetailModel>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<LogicalZoneDetailModel>>();
            }

            if (data.Count > 0)
            {
                data = data.OrderByDescending(x => x.LocationNo).ToList();
            }

            foreach (LogicalZoneDetailModel l in data)
            {
                if (dataList.Where(x => x.LocationNo == l.LocationNo).Count() == 0)
                {
                    LogicalZoneDetailModel dataadd = new LogicalZoneDetailModel
                    {
                        LogicalZoneDetailID = Guid.NewGuid(),
                        LocationNo = l.LocationNo,
                        LocationId = l.LocationId,
                        ZoneName = l.ZoneName,
                        Seq = dataList.Count + 1,
                        LocationCapacity = l.LocationCapacity
                    };
                    dataList.Add(dataadd);
                }
            }
            Session["LogicalZone.Items"] = dataList;
            BindGrid(dataList);
        }

        protected void btnAddItem_Click(object sender, DirectEventArgs e)
        {
            List<LogicalZoneConfigModel> dataList = new List<LogicalZoneConfigModel>();
            if (Session["LogicalZone.Configs"] != null)
            {
                dataList = (List<LogicalZoneConfigModel>)Session["LogicalZone.Configs"];
            }

            //int id = 0;
            //if (dataList.Count > 0)
            //{
            //    id = dataList.Min(x => x.LogicalZoneConfigID);
            //}
            //if (id > 0) id = 0;

            //id = id - 1;


            LogicalZoneConfigModel _datacount = dataList.Where(x => x.ConfigValue == null).FirstOrDefault();
            if (_datacount == null)
            {
                LogicalZoneConfigModel m = new LogicalZoneConfigModel
                {
                    ConfigSeq = dataList.Count + 1
                };
                dataList.Add(m);
                Session["LogicalZone.Configs"] = dataList;
            }
            BindGridConfig(dataList);
            X.Call("startEdit(" + (dataList.Count - 1) + ")");
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            List<LogicalZoneDetailModel> dataList = new List<LogicalZoneDetailModel>();
            if (Session["LogicalZone.Items"] != null)
            {
                dataList = (List<LogicalZoneDetailModel>)Session["LogicalZone.Items"];
            }

            if (command == "Delete")
            {
                LogicalZoneDetailModel data = dataList.Where(x => x.LogicalZoneDetailID == new Guid(oDataKeyId)).SingleOrDefault();
                int seq = data.Seq;
                dataList.Remove(data);

                foreach (LogicalZoneDetailModel l in dataList.Where(x => x.Seq > seq).ToList())
                {
                    l.Seq--;
                }
            }
            if (command == "Up")
            {
                LogicalZoneDetailModel dataUp = dataList.Where(x => x.LogicalZoneDetailID == new Guid(oDataKeyId)).SingleOrDefault();
                LogicalZoneDetailModel dataDown = dataList.Where(x => x.Seq == (dataUp.Seq - 1)).SingleOrDefault();
                if (dataUp != null && dataDown != null)
                {
                    dataUp.Seq--;
                    dataDown.Seq++;
                }
            }
            if (command == "Down")
            {
                LogicalZoneDetailModel dataDown = dataList.Where(x => x.LogicalZoneDetailID == new Guid(oDataKeyId)).SingleOrDefault();
                LogicalZoneDetailModel dataUp = dataList.Where(x => x.Seq == (dataDown.Seq + 1)).SingleOrDefault();
                if (dataUp != null && dataDown != null)
                {
                    dataUp.Seq--;
                    dataDown.Seq++;
                }
            }
            Session["LogicalZone.Items"] = dataList;
            BindGrid(dataList);
        }

        protected void CommandConfigClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            List<LogicalZoneConfigModel> dataList = new List<LogicalZoneConfigModel>();
            if (Session["LogicalZone.Configs"] != null)
            {
                dataList = (List<LogicalZoneConfigModel>)Session["LogicalZone.Configs"];
            }

            if (command == "Delete")
            {
                LogicalZoneConfigModel data = dataList.Where(x => x.LogicalConfigID == new Guid(oDataKeyId)).SingleOrDefault();
                int seq = data.ConfigSeq.Value;
                dataList.Remove(data);

                foreach (LogicalZoneConfigModel l in dataList.Where(x => x.ConfigSeq > seq).ToList())
                {
                    l.ConfigSeq--;
                }
            }
            Session["LogicalZone.Configs"] = dataList;

            BindGridConfig(dataList);
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            LogicalZone saveModel = new LogicalZone();
            List<LogicalZoneDetailModel> gridData = JSON.Deserialize<List<LogicalZoneDetailModel>>(gridJson);

            if (!validate_SaveClick(gridData))
            {
                return;
            }

            try
            {
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    saveModel.LogicalZoneID = new Guid(id);
                }
                else
                {
                    saveModel.LogicalZoneID = Guid.NewGuid();
                }


                saveModel.LogicalZoneName = txtLogicalZone_Name.Text;
                saveModel.ZoneID = new Guid(hddphysicalzone.Text);//new Guid(this.cmbPhysicalZone.SelectedItem.Value);
                saveModel.IsPallet = true;

                saveModel.LogicalZoneDetail = new List<LogicalZoneDetail>();

                LogicalZoneDetail itemModel;

                foreach (LogicalZoneDetailModel item in gridData)
                {
                    itemModel = new LogicalZoneDetail();
                    if (Guid.Empty == item.LogicalZoneDetailID)
                    {
                        itemModel.LogicalZoneDetailID = Guid.NewGuid();
                    }
                    else
                    {
                        itemModel.LogicalZoneDetailID = item.LogicalZoneDetailID;
                    }

                    // itemModel.LogicalZoneDetailID = item.LogicalZoneDetailID;
                    itemModel.LogicalZoneID = saveModel.LogicalZoneID;
                    itemModel.LocationID = item.LocationId;
                    itemModel.Seq = item.Seq;
                    itemModel.IsActive = true;
                    saveModel.LogicalZoneDetail.Add(itemModel);
                }

                List<LogicalZoneConfigModel> dataList = new List<LogicalZoneConfigModel>();
                if (Session["LogicalZone.Configs"] != null)
                {
                    dataList = (List<LogicalZoneConfigModel>)Session["LogicalZone.Configs"];
                }

                saveModel.LogicalZoneConfig = new List<LogicalZoneConfig>();
                LogicalZoneConfig itemConfigModel;

                foreach (LogicalZoneConfigModel item in dataList)
                {
                    itemConfigModel = new LogicalZoneConfig();
                    if (Guid.Empty == item.LogicalConfigID)
                    {
                        itemConfigModel.LogicalConfigID = Guid.NewGuid();
                    }
                    else
                    {
                        itemConfigModel.LogicalConfigID = item.LogicalConfigID;
                    }
                    //itemConfigModel.LogicalConfigID = item.LogicalConfigID;
                    itemConfigModel.LogicalZoneID = saveModel.LogicalZoneID;
                    if (item.ConfigValueId != null)
                    {
                        itemConfigModel.ConfigValueID = item.ConfigValueId;
                    }
                    itemConfigModel.ConfigID = item.ConfigID;
                    itemConfigModel.ConfigValue = item.ConfigValue;
                    itemConfigModel.PrioritySeq = item.ConfigSeq;
                    itemConfigModel.IsActive = true;
                    saveModel.LogicalZoneConfig.Add(itemConfigModel);
                }

                bool isSuccess = true;

                if (id == "new")
                {
                    Core.Domain.ApiResponseMessage datasave = ClientService.Master.LogicalZoneClient.Add(saveModel).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    Core.Domain.ApiResponseMessage datamodify = ClientService.Master.LogicalZoneClient.Modify(saveModel).Result;
                    if (datamodify.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datamodify.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }


            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnClear_Click(object sender, DirectEventArgs e)
        {

            txtLogicalZone_Name.Text = "";
            //this.cmbWarehouseName.SelectedItem.Index = 0;
            //this.cmbWarehouseName.UpdateSelectedItems();
            cmbWarehouseName.Clear();
            cmbPhysicalZone.Clear();
            cmbLocationNo.Clear();
            cmbLocationNoTo.Clear();

            Session["LogicalZone.Items"] = null;
            Session["LogicalZone.Configs"] = null;
            BindData(Request.QueryString["oDataKeyId"]);

        }

        protected void btnPrint_Click(object sender, DirectEventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        private void ReportSavePDF()
        {
            throw new NotImplementedException();
        }

        protected void btnSelectReceiveCode_Click(object sender, DirectEventArgs e)
        {

        }

        protected void GetParameterProxyByLogicalZoneConfig(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            BindData(param);
        }

        protected void cmbWarehouseName_Change(object sender, DirectEventArgs e)
        {
            Guid _ret = new Guid();
            bool valid = Guid.TryParse(cmbWarehouseName.SelectedItem.Value, out _ret);
            if (valid)
            {
                hddwharehouse.Text = cmbWarehouseName.SelectedItem.Value;
                AutoCompleteProxy_Zone(cmbWarehouseName.SelectedItem.Value);
            }

        }

        protected void cmbPhysicalZone_Change(object sender, DirectEventArgs e)
        {
            Guid _ret = new Guid();
            bool valid = Guid.TryParse(cmbPhysicalZone.SelectedItem.Value, out _ret);
            if (valid)
            {
                if (!string.IsNullOrEmpty(cmbPhysicalZone.SelectedItem.Value))
                {
                    hddphysicalzone.Text = cmbPhysicalZone.SelectedItem.Value;

                    cmbLocationNo.Clear();
                    Dictionary<string, object> param = new Dictionary<string, object>
                    {
                        { "Method", "LocationByZoneWarehouse" },
                        { "query", cmbLocationNo.SelectedItem.Text },
                        { "warehouseId", cmbWarehouseName.SelectedItem.Value },
                        { "zoneId", cmbPhysicalZone.SelectedItem.Value }
                    };
                    StoreLocation.PageSize = 20;
                    StoreLocation.AutoCompleteProxy(AutoCompleteService, param);
                    StoreLocation.LoadProxy();

                    cmbLocationNoTo.Clear();
                    Dictionary<string, object> param2 = new Dictionary<string, object>
                    {
                        { "Method", "LocationByZoneWarehouse" },
                        { "query", cmbLocationNoTo.SelectedItem.Text },
                        { "warehouseId", cmbWarehouseName.SelectedItem.Value },
                        { "zoneId", cmbPhysicalZone.SelectedItem.Value }
                    };
                    StoreLocationTo.PageSize = 20;
                    StoreLocationTo.AutoCompleteProxy(AutoCompleteService, param2);
                    StoreLocationTo.LoadProxy();

                    cmbLocationNo.Enable();
                    cmbLocationNo.Focus();
                    cmbLocationNoTo.Enable();
                }

            }

        }

        protected void cmbCustomer_Change(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod(Timeout=180000)]
        public void LoadComboValue(Guid id, string code)
        {
            if (code != "")
            {

                Core.Domain.ApiResponseMessage apiResp = LogicalZoneClient.GetConditionConfigBy_Configvaliable(code, "", 0, 0).Result;
                List<DataKeyValueString> data = new List<DataKeyValueString>();
                if (apiResp.ResponseCode == "0")
                {
                    data = apiResp.Get<List<DataKeyValueString>>();
                }

                if (data != null)
                {
                    StoreValue.Data = data;
                    StoreValue.DataBind();

                }
            }
        }

        [DirectMethod(Timeout=180000)]
        public void ValidateSave(JsonObject values, JsonObject record)
        {
            hddVariable.Text = "";
        }

        [DirectMethod(Timeout=180000)]
        public void CancelConfig(object gridJson)
        {
            List<LogicalZoneConfigModel> dataList = new List<LogicalZoneConfigModel>();
            if (Session["LogicalZone.Configs"] != null)
            {
                dataList = (List<LogicalZoneConfigModel>)Session["LogicalZone.Configs"];
            }

            LogicalZoneConfigModel model = JSON.Deserialize<LogicalZoneConfigModel>(gridJson.ToString());

            if (string.IsNullOrEmpty(model.ConfigCode))
            {


                dataList.Remove(dataList.Where(x => x.LogicalConfigID == model.LogicalConfigID).SingleOrDefault());
            }
            Session["LogicalZone.Configs"] = dataList;
            BindGridConfig(dataList);
        }

        [DirectMethod(Timeout=180000)]
        public void SaveConfig(object gridJson)
        {
            LogicalZoneConfigModel model = JSON.Deserialize<LogicalZoneConfigModel>(gridJson.ToString());
            List<LogicalZoneConfigModel> dataList = new List<LogicalZoneConfigModel>();
            if (Session["LogicalZone.Configs"] != null)
            {
                dataList = (List<LogicalZoneConfigModel>)Session["LogicalZone.Configs"];
            }

            string code = model.ConfigID == Guid.Empty ? model.ConfigName : model.ConfigID.ToString();
            //string code2 = string.IsNullOrEmpty(model.Config_Variable) ? model.Config_Value : model.Config_Variable;
            //dataService = new DataServiceModel();

            //dataService.Add<string>("ConfigCode", code);

            //ConfigConditionModel con_model = WebServiceHelper.Post<ConfigConditionModel>("GetConfigConditionByCode", dataService.GetObject());

            Core.Domain.ApiResponseMessage apiResp = LogicalZoneClient.GetConditionConfig("logicalzoneconfig", "", 0, 0).Result;

            List<ConditionConfig> data = new List<ConditionConfig>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<ConditionConfig>>();
            }

            ConditionConfig data2 = data.Where(x => x.ConfigId == new Guid(code)).FirstOrDefault();


            Core.Domain.ApiResponseMessage apiResp2 = LogicalZoneClient.GetConditionConfigBy_Configvaliable(code, "", 0, 0).Result;

            List<DataKeyValueString> data3 = new List<DataKeyValueString>();
            if (apiResp2.ResponseCode == "0")
            {
                data3 = apiResp2.Get<List<DataKeyValueString>>();
            }

            DataKeyValueString data4 = data3.Where(x => x.Key == model.ConfigValue || x.Value == model.ConfigValue).FirstOrDefault();

            model.ConfigCode = code;
            model.ConfigValue = data4.Key;

            Guid guidResult = new Guid();
            bool isValid = Guid.TryParse(data4.Value, out guidResult);
            if (isValid)
            {
                model.ConfigValueId = new Guid(data4.Value);
            }

            model.ConfigName = data2.ConfigName;
            model.ConfigID = new Guid(code);
            int seq = model.ConfigSeq.Value;

            dataList.Remove(dataList.Where(x => x.LogicalConfigID == model.LogicalConfigID).LastOrDefault());
            if (dataList.Where(x => x.ConfigCode == model.ConfigCode && x.ConfigValue == model.ConfigValue && x.LogicalConfigID != model.LogicalConfigID).Count() == 0)
            {
                dataList.Add(model);
            }
            else
            {
                foreach (LogicalZoneConfigModel l in dataList.Where(x => x.ConfigSeq > seq).ToList())
                {
                    l.ConfigSeq--;
                }
            }

            Session["LogicalZone.Configs"] = dataList;
            BindGridConfig(dataList);
        }


        protected void btnExit_Click(object sender, EventArgs e)
        {
            X.Call("parent.App.direct.Close");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

    }
}