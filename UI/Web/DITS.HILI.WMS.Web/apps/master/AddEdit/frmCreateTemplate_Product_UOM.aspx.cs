using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateTemplate_Product_UOM : BaseUIPage
    {
        private readonly string AutoCompleteService = "~/Common/DataClients/DataOfMaster.ashx";

        public static string ProgramCode = frmAllProductUOMTemplate.ProgramCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void BindData(string oDataKeyId)
        {
            Product_UOM_Template_ID.Text = oDataKeyId;
            txtQtyUnit.Text = "1";
            txtWidth.Text = "1";
            txtLength.Text = "1";
            txtHeight.Text = "1";
            hidRowId.SetValue(-99);
            txtTemplateName.Focus(true);
            chkActive.Checked = true;
            //Get_StdUnit();

            // ID = 0 : Add new
            if (oDataKeyId == "new")
            {
                return;
            }


            Core.Domain.ApiResponseMessage apiResphd = ProductTemplateUOMClient.GetByID(new Guid(oDataKeyId)).Result;
            ProductTemplateUom datahd = new ProductTemplateUom();
            if (apiResphd.ResponseCode == "0")
            {
                datahd = apiResphd.Get<ProductTemplateUom>();

                txtTemplateName.Text = datahd.Product_UOM_Template_Name;

                btnSave.Enable();

                chkActive.Checked = datahd.IsActive;
            }

            Core.Domain.ApiResponseMessage apiResp = ProductTemplateUOMClient.GetProductTemplateUomDetail(new Guid(Request.QueryString["oDataKeyId"])).Result;
            List<ProductTemplateUomDetail> data = new List<ProductTemplateUomDetail>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<ProductTemplateUomDetail>>();

                UOMStore.DataSource = data;
                UOMStore.DataBind();
            }


        }

        [DirectMethod(Timeout=180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStoreUOM"];

            ProductTemplateUom saveModel = new ProductTemplateUom();

            List<ProductTemplateUomDetail> gridData = JSON.Deserialize<List<ProductTemplateUomDetail>>(gridJson);

            #region [ Get Data to Model ]
            //Header Template Name

            string id = Request.QueryString["oDataKeyId"];

            if (id != "new")
            {
                saveModel.Product_UOM_Template_ID = new Guid(id);
            }

            saveModel.Product_UOM_Template_Name = txtTemplateName.Text;
            saveModel.IsActive = chkActive.Checked;

            #region [ Detail Model ]
            saveModel.ProductTemplateUomDetailCollection = new List<ProductTemplateUomDetail>();

            ProductTemplateUomDetail itemModel;

            foreach (ProductTemplateUomDetail item in gridData)
            {
                itemModel = new ProductTemplateUomDetail
                {
                    Product_UOM_Template_ID = saveModel.Product_UOM_Template_ID,
                    Product_UOM_Template_Detail_ID = item.Product_UOM_Template_Detail_ID,
                    Product_UOM_Template_Detail_Name = item.Product_UOM_Template_Detail_Name,
                    Product_UOM_Template_Detail_Short_Name = item.Product_UOM_Template_Detail_Short_Name,
                    Product_UOM_Template_Detail_Quantity = item.Product_UOM_Template_Detail_Quantity,
                    Product_UOM_Template_Detail_Weight = item.Product_UOM_Template_Detail_Weight,
                    Product_UOM_Template_Detail_Package_Weight = item.Product_UOM_Template_Detail_Package_Weight,
                    Product_UOM_Template_Detail_SKU = item.Product_UOM_Template_Detail_SKU,
                    Product_UOM_Template_Detail_Package_Width = item.Product_UOM_Template_Detail_Package_Width,
                    Product_UOM_Template_Detail_Package_Length = item.Product_UOM_Template_Detail_Package_Length,
                    Product_UOM_Template_Detail_Package_Height = item.Product_UOM_Template_Detail_Package_Height,
                    IsActive = true
                };

                saveModel.ProductTemplateUomDetailCollection.Add(itemModel);
            }

            #endregion [ Detail Model ]

            #endregion [ Get Data to Model ]


            bool isSuccess = true;

            if (id == "new")
            {
                Core.Domain.ApiResponseMessage datasave = ClientService.Master.ProductTemplateUOMClient.Add(saveModel).Result;

                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
            }
            else
            {
                Core.Domain.ApiResponseMessage datamodify = ClientService.Master.ProductTemplateUOMClient.Modify(saveModel).Result;
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

        protected void btnClear_Click(object sender, DirectEventArgs e)
        {
            clearUnitForm();

            string Key = Request.QueryString["oDataKeyId"];

            if (Key != "new")
            {
                Core.Domain.ApiResponseMessage apiResp = ProductTemplateUOMClient.GetProductTemplateUomDetail(new Guid(Request.QueryString["oDataKeyId"])).Result;
                List<ProductTemplateUomDetail> data = new List<ProductTemplateUomDetail>();
                if (apiResp.ResponseCode == "0")
                {
                    data = apiResp.Get<List<ProductTemplateUomDetail>>();

                    UOMStore.DataSource = data;
                    UOMStore.DataBind();
                }
            }

        }
        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.AddScript("parent.Ext.WindowMgr.getActive().close();"); ;
        }

        //protected void CommandClick(object sender, DirectEventArgs e)
        //{
        //    string command = e.ExtraParams["command"];
        //    string oDataKeyId = e.ExtraParams["oDataKeyId"];
        //    Guid Id = new Guid(oDataKeyId);

        //    if (command.ToLower() == "delete")
        //    {
        //        var ok = ClientService.Master.ProductTemplateUOMClient.Remove(Id).Result;
        //        if (ok.ResponseCode == "0")
        //        {
        //            //NotificationExt.Show("Delete", "Delete complete");
        //            NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);

        //        }
        //    }
        //}

        protected void btnAddStdUnit_Click(object sender, DirectEventArgs e)
        {
            Icon iconWindows = Icon.ApplicationFormAdd;
            string strTitle = GetResource("UNIT");
            WindowShow.Show(this, strTitle, "CreateUnit", "frmCreateStandard_Unit.aspx?IsPopup=1", iconWindows, 350, 160);

        }

        [DirectMethod(Timeout=180000)]
        public void ReloadUOMMethod(string param)
        {
            Core.Domain.ApiResponseMessage apiResp = UnitsClient.GetUnit("", false, null, null).Result;
            List<Units> data = new List<Units>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<Units>>();
                StdUnitStore.DataSource = data;
                StdUnitStore.DataBind();

                if (data.Count > 0)
                {
                    cmbStdUnit.SetDefaultValue(param, data.FirstOrDefault().ShortName);
                    txtUnitShortName.Text = data.FirstOrDefault().ShortName;
                    txtQtyUnit.Focus(true);
                }

            }
        }



        [DirectMethod(Timeout=180000)]
        public void clearUnitForm()
        {
            cmbStdUnit.Reset();
            txtUnitShortName.Reset();
            txtQtyUnit.Reset();
            txtWidth.Reset();
            txtLength.Reset();
            txtHeight.Reset();
            chkSKU.Reset();
            txtWeightUnit.Reset();
            txtPackWeightUnit.Reset();
            hidStdUnitID.Reset();
            hidStdUnitName.Reset();
            txtQtyUnit.Text = "1";
            txtWidth.Text = "1";
            txtLength.Text = "1";
            txtHeight.Text = "1";

        }

        [DirectMethod(Timeout=180000)]
        public void setStdCombo(string uomName, string uomShortName)
        {
            cmbStdUnit.SetAutoCompleteValue(
                 new List<Units>
                    {
                        new Units
                        {
                            Name =uomName,
                            ShortName =uomShortName,
                        }
                    },
                    uomShortName
              );
            txtUnitShortName.Text = uomShortName;
        }

    }
}