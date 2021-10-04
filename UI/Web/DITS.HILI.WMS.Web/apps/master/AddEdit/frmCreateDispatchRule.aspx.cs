using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateDispatchRule : BaseUIPage
    {
        private readonly string AutoCompleteService = "~/Common/DataClients/DataOfMaster.ashx";
        private static readonly int _Special_Rul_ID;



        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    SetParameterProxy();

            //    BindData(Request.QueryString["oDataKeyId"]);

            //    DynamicMenu.ToolbarControl(ProgramCode, null, btnSave, null);
            //}

        }

        private void BindData(string oDataKeyId)
        {
            //txtRule_Code.Text = oDataKeyId;
            //// ID = 0 : Add new
            //if (oDataKeyId == "new")
            //{
            //    return;
            //}

            //btnClear.Disable();

            //DataServiceModel dataService = new DataServiceModel();
            //List<SearchModel> _searchModel = new List<SearchModel>();

            //if (int.TryParse(oDataKeyId, out _Special_Rul_ID) == false)
            //{
            //    MessageBoxExt.Warning("special_Rul_ID invalid");
            //}

            //dataService.Add<string>("SearchText", "");
            //dataService.Add<int>("special_Rul_ID", _Special_Rul_ID);

            //_searchModel.Add(new SearchModel()
            //{
            //    Group = 1,
            //    Key = "dispatch_rule.[Special_Rul_ID]",
            //    Operation = "=",
            //    Value = _Special_Rul_ID.ToString(),
            //    IsCondition = ConditionWhere.NONE,
            //});
            //dataService.Add<List<SearchModel>>("SearchModel", _searchModel);

            //List<DispatchRuleModel> data = WebServiceHelper.Post<List<DispatchRuleModel>>("Get_DispatchRules", dataService.GetObject());

            //if (data.Count > 0)
            //{
            //    var dataItem = data.FirstOrDefault();

            //    txtRule_Code.Text = dataItem.Special_Rul_Code;
            //    txtRuleName.Text = dataItem.Rule_name;

            //    cmbShippingTo.SelectedItem.Value = dataItem.Shippto_Code;

            //    txtLotNo.Text = dataItem.LotNo;
            //    nbLotAging.Value = dataItem.Aging;
            //    nbLotDuration.Value = dataItem.PeriodLot;
            //    nbLotLessThan.Value = dataItem.LotLessThan;

            //    hidAddProduct_System_Code.Text = dataItem.Product_System_Code;
            //    txtProductCode.Text = dataItem.Product_Code;
            //    txtAddProduct_Name_Full.Text = dataItem.Product_Name;

            //    chkIsActive.Checked = dataItem.IsActive;
            //    chkIsDefault.Checked = dataItem.IsDefault ?? false;

            //}
        }

        [DirectMethod(Timeout=180000)]
        public void UpdateDispatchRule()
        {
            try
            {
                //Results data = new Results() { result = false };

                //#region [ Get Data to Model ]

                //DispatchRuleModel dispatchRuleModel = Get_DispatchRule_Model();
                //DataServiceModel dataService = new DataServiceModel();
                //dataService.Add<DispatchRuleModel>("DispatchRuleModel", dispatchRuleModel);

                //#endregion

                //if (txtRule_Code.Text == "new")
                //{
                //    data = WebServiceHelper.Post<Results>("Insert_DispatchRule", dataService.GetObject());
                //}
                //else
                //{

                //    data = WebServiceHelper.Post<Results>("Update_DispatchRule", dataService.GetObject());
                //}


                //if (data.result)
                //{
                //    X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage(data.message));
                //    X.Call("parentAutoLoadControl.close()");
                //}
                //else
                //{
                //    MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
                //}

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }

        //private DispatchRuleModel Get_DispatchRule_Model()
        // {
        //DispatchRuleModel get_data = new DispatchRuleModel()
        //{
        //    Special_Rul_ID = _Special_Rul_ID,
        //    Special_Rul_Code = txtRule_Code.Text,
        //    Special_Rul_Name = txtRuleName.Text,
        //    Rule_name = txtRuleName.Text,
        //    Shippto_Code = cmbShippingTo.SelectedItem.Value,
        //    Shippto_Name = cmbShippingTo.SelectedItem.Text,

        //    LotNo = txtLotNo.Text,
        //    Aging = nbLotAging.Value.ToInt(),
        //    PeriodLot = nbLotDuration.Value.ToInt(),
        //    LotLessThan = nbLotLessThan.Value.ToInt(),

        //    Product_System_Code = hidAddProduct_System_Code.Text,
        //    Product_Code = txtProductCode.Text,
        //    Product_Name = txtAddProduct_Name_Full.Text,

        //    IsActive = chkIsActive.Checked,
        //    IsDefault = chkIsDefault.Checked
        //};

        //return get_data;
        // }

        //private void SetParameterProxy()
        //{
        //    StoreShipTo.AutoCompleteProxy(AutoCompleteService,
        //                                 new string[] { "Methode" },
        //                                 new string[] { "shipto" });
        //    StoreShipTo.LoadProxy();


        //}

        //#region [UC Product]
        //[DirectMethod]
        //public void GetProduct(string _product_code)
        //{

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
        //    txtProductCode.Text = data.Product_Code;
        //    hidAddProduct_System_Code.Text = data.Product_System_Code;
        //    txtAddProduct_Name_Full.Text = data.Product_Name_Full;
        //    hidAddUomID.Text = data.Product_UOM_ID.ToString();
        //}
        //else
        //{
        //    ucProductSelect.Passvalue("all", "");
        //    ucProductSelect.Show(_product_code);
        //}

        //}

        //[DirectMethod]
        //public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        //{
        //    return ucProductSelect.ProductSelectBindData(action, extraParams);
        //}

        //[DirectMethod]
        //public void ucProductCode_Select(string record)
        //{
        //    ProductModel data = JSON.Deserialize<ProductModel>(record);

        //    txtProductCode.Text = data.Product_Code;
        //    hidAddProduct_System_Code.Text = data.Product_System_Code;
        //    txtAddProduct_Name_Full.Text = data.Product_Name_Full;
        //    hidAddUomID.Text = data.Product_UOM_ID.ToString();

        //    ucProductSelect.Close();
        //}

        //#endregion
    }
}