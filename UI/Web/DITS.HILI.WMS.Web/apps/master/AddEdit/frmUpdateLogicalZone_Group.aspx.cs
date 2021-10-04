
using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmUpdateLogicalZone_Group : BaseUIPage
    {
        private readonly string AutoCompleteService = "../../../Common/DataClients/MsDataHandler.ashx";

        public string ProgramCode = frmAllLogicalZone_Group.ProgramCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetParameterProxy();
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void BindData(string oDataKeyId)
        {
            try
            {
                if (oDataKeyId == "new")
                {
                    txtGroup_Code.Text = "new";
                    return;
                }

                Core.Domain.ApiResponseMessage apiResp = LogicalZoneGroupClient.GetByID(new Guid(oDataKeyId)).Result;
                LogicalZoneGroupModel data = new LogicalZoneGroupModel();
                if (apiResp.ResponseCode == "0")
                {
                    data = apiResp.Get<LogicalZoneGroupModel>();

                    txtGroup_Code.Text = data.LogicalZoneGroupId.ToString();
                    txtGroup_Name.Text = data.LogicalZoneGroupName;
                    txtIsActive.Checked = data.IsActive.Value;

                    if (data.LogicalZoneGroupDetailCollection.Count() > 0)
                    {
                        LogicalZoneGroupDetailModel _datadt = data.LogicalZoneGroupDetailCollection.FirstOrDefault();

                        if (_datadt != null)
                        {
                            if (_datadt.LogicalZoneGroupLevel3Id != null)
                            {
                                cmbProductGroupLevel3.SetAutoCompleteValue(
                                    new List<ProductGroupLevel3>
                                    {
                                                new ProductGroupLevel3
                                    {
                                        Name = _datadt.LogicalZoneGroupLevel3Name,
                                        ProductGroupLevel3ID = _datadt.LogicalZoneGroupLevel3Id.Value
                                    }
                                    }
                                    , _datadt.LogicalZoneGroupLevel3Id.ToString()
                                    );
                            }

                        }
                        StoreOfDataList.Data = data.LogicalZoneGroupDetailCollection;
                        StoreOfDataList.DataBind();
                    }
                    else
                    {
                        StoreOfDataList.RemoveAll();
                        StoreOfDataList.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private bool Validate_Save(List<LogicalZoneGroupDetailModel> gridData)
        {

            if (string.IsNullOrEmpty(txtGroup_Name.Text.Trim()))
            {

                MessageBoxExt.Warning("Please Insert Logicalzone Group Name");
                txtGroup_Name.Focus();
                return false;
            }

            if (gridData.Count == 0)
            {
                MessageBoxExt.ShowError("No Item");
                return false;
            }


            //if (cmbLogicalZone.SelectedItem.Value == null)
            //{

            //    MessageBoxExt.Warning("Please Insert Logicalzone Group Level 3");
            //    cmbLogicalZone.Focus();
            //    return false;
            //}


            return true;
        }

        protected void btnSaveAll_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<LogicalZoneGroupDetailModel> gridDataList = JSON.Deserialize<List<LogicalZoneGroupDetailModel>>(gridJson);

            if (Validate_Save(gridDataList))
            {

                LogicalZoneGroup savedata = new LogicalZoneGroup();
                string id = Request.QueryString["oDataKeyId"];
                if (id != "new")
                {
                    savedata.LogicalZoneGroupID = new Guid(txtGroup_Code.Text);
                }
                else
                {
                    savedata.LogicalZoneGroupID = Guid.NewGuid();
                }
                savedata.LogicalZoneGroupName = txtGroup_Name.Text;
                savedata.IsActive = txtIsActive.Checked;

                savedata.LogicalZoneGroupDetail = new List<LogicalZoneGroupDetail>();
                LogicalZoneGroupDetail itemdetailModel;

                foreach (LogicalZoneGroupDetailModel item in gridDataList)
                {

                    itemdetailModel = new LogicalZoneGroupDetail();
                    if (Guid.Empty == item.LogicalZoneGroupDetailId)
                    {
                        itemdetailModel.LogicalGroupDetailID = Guid.NewGuid();
                    }
                    else
                    {
                        itemdetailModel.LogicalGroupDetailID = item.LogicalZoneGroupDetailId;
                    }

                    if (cmbProductGroupLevel3.SelectedItem.Value != null)
                    {
                        itemdetailModel.ProductGroupLevel3ID = new Guid(cmbProductGroupLevel3.SelectedItem.Value);
                    }
                    itemdetailModel.LogicalZoneGroupID = savedata.LogicalZoneGroupID;
                    itemdetailModel.ProductID = item.ProductId;
                    itemdetailModel.IsActive = item.IsActive;
                    savedata.LogicalZoneGroupDetail.Add(itemdetailModel);
                }


                bool isSuccess = true;
                if (id == "new")
                {
                    Core.Domain.ApiResponseMessage datasave = ClientService.Master.LogicalZoneGroupClient.Add(savedata).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    Core.Domain.ApiResponseMessage datamodify = ClientService.Master.LogicalZoneGroupClient.Modify(savedata).Result;
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

        }

        protected void btnClose_Click(object sender, DirectEventArgs e)
        {
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            //string oDataKeyId = e.ExtraParams["oDataKeyId"];
            //string strProduct_System_Code = e.ExtraParams["oProductId"];

            //string gridJson = e.ExtraParams["ParamStorePages"];
            //List<LogicalZoneGroupDetailModel> gridDataList = JSON.Deserialize<List<LogicalZoneGroupDetailModel>>(gridJson);



            //// data exist in database
            //if (oDataKeyId != null && oDataKeyId != "0")
            //{
            //    DataServiceModel dataService = new DataServiceModel();
            //    dataService.Add<int>("Mapping_Group_ID", oDataKeyId.ToInt());

            //    data = WebServiceHelper.Post<Results>("Delete_LogicalZoneList_By_ID", dataService.GetObject());
            //}
            //else
            //{
            //    data.result = true;
            //}

            //if (data.result == true)
            //{
            //    var _data = gridDataList.Where(w => w.Product_System_Code != strProduct_System_Code).ToList();

            //    if (_data.Count > 0)
            //    {
            //        this.StoreOfDataList.Data = _data;
            //        this.StoreOfDataList.DataBind();
            //    }
            //    else
            //    {
            //        this.StoreOfDataList.RemoveAll();
            //        this.StoreOfDataList.DataBind();
            //    }
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
            //}
        }

        #region [UC Product]

        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code, List<LogicalZoneGroupDetailModel> dataExist)
        {
            ucProductMultiSelect.Show(_product_code, dataExist);
        }

        [DirectMethod(Timeout=180000)]
        public object ProductMultiSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucProductMultiSelect.BindData(action, extraParams);
        }


        [DirectMethod(Timeout=180000)]
        public void ucProductCode_MultiSelect(string record)
        {
            List<LogicalZoneGroupDetailModel> listdataExist = Session["dataExistPg"] as List<LogicalZoneGroupDetailModel>;

            List<ProductModel> products = JSON.Deserialize<List<ProductModel>>(record);

            List<LogicalZoneGroupDetailModel> listData = new List<LogicalZoneGroupDetailModel>();

            foreach (ProductModel product in products)
            {
                LogicalZoneGroupDetailModel ret = listdataExist.Where(x => x.ProductCode == product.ProductCode).FirstOrDefault();
                if (ret == null)
                {
                    listData.Add(new LogicalZoneGroupDetailModel()
                    {
                        ProductId = product.ProductID,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        IsActive = true
                    });
                }

            }


            StoreOfDataList.Add(listData);
            StoreOfDataList.CommitChanges();

            ucProductMultiSelect.Close();
            Session["dataExistPg"] = null;
        }

        #endregion

        protected void chkIsActive_Change(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<LogicalZoneGroupDetailModel> gridDataList = JSON.Deserialize<List<LogicalZoneGroupDetailModel>>(gridJson);
            foreach (LogicalZoneGroupDetailModel item in gridDataList)
            {
                item.IsActive = txtIsActive.Checked;
            }
            StoreOfDataList.Data = gridDataList;
            StoreOfDataList.DataBind();
        }

        private void SetParameterProxy()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "ProductGroupLevel" },
                { "query", cmbProductGroupLevel3.SelectedItem.Text }
            };
            StoreLogicalZone.PageSize = 20;
            StoreLogicalZone.AutoCompleteProxy(AutoCompleteService, param);
            StoreLogicalZone.LoadProxy();
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {

            string gridJson = e.ExtraParams["ParamStorePages"];
            List<LogicalZoneGroupDetailModel> gridDataList = JSON.Deserialize<List<LogicalZoneGroupDetailModel>>(gridJson);

            GetProduct("", gridDataList);

            ucProductMultiSelect.ShowOnly();
        }

    }
}