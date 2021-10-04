
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateLogicalZone_Config : System.Web.UI.Page
    {
        string AutoCompleteService = "../../../Common/DataClients/DataOfMaster.ashx";

        DataServiceModel dataService = new DataServiceModel();

        List<SearchModel> _searchModel = new List<SearchModel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void BindData(string oDataKeyId)
        {
            SetParameterProxy();

            this.txtLogicalZoneCode.Text = oDataKeyId;

            dataService = new DataServiceModel();

            _searchModel = new List<SearchModel>();

            _searchModel.Add(new SearchModel()
            {
                Group = 1,
                Key = "sys_logicalzone_config.LogicalZone_Code",
                Operation = "=",
                Value = oDataKeyId,
                IsCondition = ConditionWhere.NONE,
            });
            dataService.Add<List<SearchModel>>("SearchModel", _searchModel);

            List<LogicalZoneConfigModel> data = WebServiceHelper.Post<List<LogicalZoneConfigModel>>("SYS_Get_LogicalZoneConfig", dataService.GetObject());

            if(data.Count()>0)
            {
                LogicalZoneConfigModel databind = data.FirstOrDefault();

                if (!IsPostBack)
                {
                    this.cmbSupplier.SetAutoCompleteValue(
                          new List<SupplierModel> 
                            { 
                                new SupplierModel
                                { 
                                    Supplier_Code =databind.Supplier_Code, 
                                    Supplier_NameTH =databind.SupplierName,
                                } 
                            }
                      );

                    this.cmbProductGroup3.SetAutoCompleteValue(
                         new List<ProductGroup_Level3Model> 
                            { 
                                new ProductGroup_Level3Model
                                { 
                                    ProductGroup_Level3_Code =databind.ProductGroup_Level3_Code, 
                                    ProductGroup_Level3_Full_Name =databind.ProductGroup_Level3_Full_Name,
                                } 
                            }
                     );
                }
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {

            try
            {
                LogicalZoneConfigModel updateData = new LogicalZoneConfigModel();
                updateData.LogicalZone_Code = this.txtLogicalZoneCode.Text;
                updateData.Supplier_Code = this.cmbSupplier.SelectedItem.Value;
                updateData.ProductGroup_Level3_Code = this.cmbProductGroup3.SelectedItem.Value;

                DataServiceModel dataService = new DataServiceModel();
                dataService.Add<LogicalZoneConfigModel>("LogicalZoneConfigModel", updateData);

                Results data1 = WebServiceHelper.Post<Results>("UpdateLogicalZoneConfig", dataService.GetObject());

                if (data1.result)
                {
                    BindData(data1.text);

                    X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage(data1.message));
                    X.Call("parentAutoLoadControl.close()");
                }
                else
                {
                    MessageBoxExt.ShowError(MyToolkit.CustomMessage(data1.message));
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        private void SetParameterProxy()
        {

            StoreSupplier.AutoCompleteProxy(AutoCompleteService,
                                                     new string[] { "Methode" },
                                                     new string[] { "supplier" });

            StoreProductGroup3.AutoCompleteProxy(AutoCompleteService,
                                                     new string[] { "Methode" },
                                                     new string[] { "product_catagory" });


        }
    }
}