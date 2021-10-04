using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Rule;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllPutAwayRule : BaseUIPage
    {

        // string AutoCompleteService = "../../Common/DataClients/DataOfMaster.ashx";



        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!IsPostBack && DynamicMenu.ToolbarControlOperation(ProgramCode, null, null, null, null, null, null, null, null, null, null))
            //{
            //    this.PagingToolbar1.MoveFirst();
            //    BindData();
            //}

        }



        protected void Store_Refresh(object sender, EventArgs e)
        {
            try
            {
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        //protected void CommandClick(object sender, DirectEventArgs e)
        //{
        //    string command = e.ExtraParams["command"];
        //    string oDataKeyId = e.ExtraParams["oDataKeyId"];

        //    if (command == "delete")
        //    {
        //Delete

        //DataServiceModel dataService = new DataServiceModel();
        //dataService.Add<string>("Special_Rul_Code", oDataKeyId);

        //Results data = WebServiceHelper.Post<Results>("DeletePutAwayRule", dataService.GetObject());

        //if (data.result == true)
        //{
        //    NotificationExt.Show(Resources.Langauge.Delete, MyToolkit.CustomMessage(data.message));

        //    Clear_Data();
        //    BindData();
        //}
        //else
        //{
        //    MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
        //}

        // }
        //}

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            //if (command.ToLower() == "edit")
            //{
            //    GetAddEditForm(oDataKeyId);
            //}
            Core.Domain.ApiResponseMessage ok = ClientService.Master.RulesClient.Remove(Id).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                PagingToolbar1.MoveFirst();
            }
            else
            {
                NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
            }

        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            try
            {
                cmbConditionAdd.SelectedItem.Value = "=";

                ddlUnitLot.SelectedItem.Index = 0;

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);

                int total = 0;
                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                Core.Domain.ApiResponseMessage apiResp = RulesClient.GetPutawayRules(null, txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
                List<SpecialPutawayRule> data = new List<SpecialPutawayRule>();
                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<SpecialPutawayRule>>();
                }

                return new { data, total };

                //DataServiceModel dataService = new DataServiceModel();

                //dataService.Add<string>("SearchText", this.txtSearch.Text);

                //List<PutAwayRuleModel> _itemDetail = WebServiceHelper.Post<List<PutAwayRuleModel>>("GetPutAwayRules", dataService.GetObject());

                //if (_itemDetail.Count() > 0)
                //{
                //    this.StoreOfDataList.Data = _itemDetail;
                //    this.StoreOfDataList.DataBind();
                //}
                //else
                //{
                //    this.StoreOfDataList.RemoveAll();
                //    this.StoreOfDataList.DataBind();
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [DirectMethod(Timeout=180000)]
        public void Edit(string id, string field, string oldValue, string newValue, object gridJson)
        {

            //DataServiceModel dataService = new DataServiceModel();
            //PutAwayRuleModel _search = JSON.Deserialize<PutAwayRuleModel>(gridJson.ToString());

            //_search.search = field;

            //dataService.Add<PutAwayRuleModel>("PutAwayRuleModel", _search);

            //Results res = WebServiceHelper.Post<Results>("UpdatePutAwayRule", dataService.GetObject());

            //if (res.result)
            //{
            //    NotificationExt.Show("Update", "Update Complete");
            //    this.grdDataList.GetStore().GetById(id).Commit();
            //    BindData();
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(res.message);
            //}

        }

        #region [ Add Data ]


        protected void btnAddPutAwayRule_Click(object sender, DirectEventArgs e)
        {
            txtSearch.Text = "";
            if (!Validate_Save())
            {
                return;
            }

            SpecialPutawayRule _getrule = GetDataDispatchRule();
            bool isSuccess = true;
            Core.Domain.ApiResponseMessage datasave = ClientService.Master.RulesClient.Add(_getrule).Result;
            if (datasave.ResponseCode != "0")
            {
                isSuccess = false;
                MessageBoxExt.ShowError(datasave.ResponseMessage);
            }

            if (isSuccess)
            {
                NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                PagingToolbar1.MoveFirst();
                Clear_Data();
            }
            //DataServiceModel dataService = new DataServiceModel();
            //PutAwayRuleModel _getrule = GetDataDispatchRule();

            //dataService.Add<PutAwayRuleModel>("PutAwayRuleModel", _getrule);

            //Results res = WebServiceHelper.Post<Results>("InsertPutAwayRule", dataService.GetObject());

            //if (res.result)
            //{
            //    NotificationExt.Show("Save", "Save Complete");

            //    Clear_Data();
            //    BindData();
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(res.message);
            //}
        }

        private SpecialPutawayRule GetDataDispatchRule()
        {
            SpecialPutawayRule itemRule = new SpecialPutawayRule
            {
                LogicalZoneID = new Guid(cmbLogicalzone.Text),
                //itemRule.Product_System_Code = this.hidAddProduct_System_Code.Text;
                //itemRule.OrderNo = txtOrderNo.Text;
                PeriodLot = Convert.ToInt16(txtPeriodLot.Text),
                //itemRule.Unit_Lot = ddlUnitLot.SelectedItem.Value;
                Condition = cmbConditionAdd.Text
            };

            return itemRule;
        }

        private void Clear_Data()
        {

            txtProductCode.Reset();
            txtAddProduct_Name_Full.Reset();
            txtPeriodLot.Reset();
            txtOrderNo.Reset();
            cmbConditionAdd.Reset();

        }

        private bool Validate_Save()
        {
            if (string.IsNullOrWhiteSpace(txtOrderNo.Text))
            {
                MessageBoxExt.Warning("Please enter Order No.");
                txtOrderNo.Focus(true, 300);
                return false;
            }
            //if (string.IsNullOrWhiteSpace(this.txtAddProduct_Name_Full.Text))
            //{
            //    MessageBoxExt.Warning("Please enter Product Code.");
            //    this.txtProductCode.Focus(true, 300);
            //    return false;
            //}

            if (string.IsNullOrWhiteSpace(cmbLogicalzone.Text))//this.cmbLogicalzone.GetValue()
            {
                MessageBoxExt.Warning("Please select Group Product.");
                cmbConditionAdd.Focus(true, 300);
                return false;
            }

            if (Convert.ToInt16(txtPeriodLot.Text) < 1)
            {
                MessageBoxExt.Warning("Please enter Period Lot days.");
                txtPeriodLot.Focus(true, 300);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbConditionAdd.Text))
            {
                MessageBoxExt.Warning("Please select Condition.");
                cmbConditionAdd.Focus(true, 300);
                return false;
            }

            return true;
        }

        #endregion [ Add Data ]

        #region [UC Product]
        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code)
        {

            //StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = ProductClient.GetAll(txtSearch.Text, false, null, null, null, null, null, null).Result;
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>();
            }

            ProductModel dataModel = data.FirstOrDefault();
            if (data.Count() == 1)
            {
                txtProductCode.Text = dataModel.ProductCode;
                hidAddProduct_System_Code.Text = dataModel.ProductID.ToString();
                txtAddProduct_Name_Full.Text = dataModel.ProductName;
                //this.hidAddUomID.Text = dataModel.ProductUomID.ToString();
            }
            else
            {
                // this.ucProductSelect.Passvalue("all", "");
                //this.ucProductSelect.Show(_product_code);
            }

            //DataServiceModel dataService = new DataServiceModel();
            //List<SearchModel> _searchModel1 = new List<SearchModel>();

            //if (_product_code != "")
            //{
            //    _searchModel1.Add(new SearchModel()
            //    {
            //        Group = 1,
            //        Key = "sys_product_code.Product_Code",
            //        Operation = "=",
            //        Value = _product_code,
            //        IsCondition = ConditionWhere.AND,
            //    });
            //    dataService.Add<List<SearchModel>>("SearchModel", _searchModel1);
            //}




            //List<ProductModel> data_receive = WebServiceHelper.Post<List<ProductModel>>("SYS_Get_Product_Receive", dataService.GetObject());

            //var data = data_receive.FirstOrDefault();
            //if (data_receive.Count == 1)
            //{
            //    this.txtProductCode.Text = data.Product_Code;
            //    this.hidAddProduct_System_Code.Text = data.Product_System_Code;
            //    this.txtAddProduct_Name_Full.Text = data.Product_Name_Full;
            //    this.hidAddUomID.Text = data.Product_UOM_ID.ToString();
            //}
            //else
            //{
            //    this.ucProductSelect.Passvalue("all","");
            //    this.ucProductSelect.Show(_product_code);
            //}

        }

        //        [DirectMethod(Timeout=180000)]
        //public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        //{
        //    return ucProductSelect.ProductSelectBindData(action, extraParams);
        //}

        [DirectMethod(Timeout=180000)]
        public void ucProductCode_Select(string record)
        {
            ProductModel data = JSON.Deserialize<ProductModel>(record);

            txtProductCode.Text = data.ProductCode;
            //this.hidAddProduct_System_Code.Text = data.Product_System_Code;
            txtAddProduct_Name_Full.Text = data.ProductName;
            //this.hidAddUomID.Text = data.Product_UOM_ID.ToString();

            //this.ucProductSelect.Close();
        }

        #endregion

        #region [ Load Combo ]

        private void LoadLogicalzone()
        {
            //DataServiceModel dataService = new DataServiceModel();

            //List<SearchModel> _searchModel = new List<SearchModel>();

            //_searchModel.Add(new SearchModel()
            //{
            //    Group = 2,
            //    Key = "sys_receive_type.IsActive",
            //    Operation = "=",
            //    Value = "1",
            //    IsCondition = ConditionWhere.AND,
            //});
            //_searchModel.Add(new SearchModel()
            //{
            //    Group = 2,
            //    Key = "sys_receive_type.IsReprocess",
            //    Operation = "=",
            //    Value = "1",
            //    IsCondition = ConditionWhere.NONE,
            //});
            //dataService.Add<List<SearchModel>>("SearchModel", _searchModel);

            //List<LogicalzoneGroupModel> data = WebServiceHelper.Post<List<LogicalzoneGroupModel>>("Get_LogicalGroup", dataService.GetObject());

            //StoreLogicalzone.Data = data;
            //StoreLogicalzone.DataBind();

        }
        #endregion


    }
}