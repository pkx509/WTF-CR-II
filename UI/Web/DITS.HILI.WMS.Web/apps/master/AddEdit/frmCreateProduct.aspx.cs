using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateProduct : BaseUIPage
    {

        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    BindData(code);
                }
                else
                {
                    txtProductId.Text = code;
                    HideTabs();
                    radioStockCode.Checked = true;
                    groupProductPicture.Hidden = true;
                    txtIsActive.Checked = true;
                    return;
                }
            }

            btnSave.Enable();
        }

        protected void HideTabs()
        {
            tabProductUnit.Hidden = true;
            tabReceiveDispatch.Hidden = true;
        }

        //#region [ Method ]

        private void BindData(string oDataKeyId)
        {
            txtProductId.Text = oDataKeyId;
            hisSysProduct.Text = oDataKeyId;
            groupProductPicture.Hidden = true;
            panelUOMTemplate.Hide();
            //this.btnEditCode.Hide();
            btnSave.Enable();

            #region [ Product Detail ]

            Guid id = new Guid(oDataKeyId);
            ApiResponseMessage data = ClientService.Master.ProductClient.GetAll("", true, id, null, null, null, null, null).Result;
            ProductModel _data = data.Get<List<ProductModel>>().FirstOrDefault();

            if (_data != null)
            {
                txtProductCode.Text = _data.ProductCode;
                hisProductCodeOld.Text = _data.ProductCode;
                txtProductName.Text = _data.ProductName;
                txtProductModel.Text = _data.ProductModelName;
                txtAgeing.Text = _data.Age.ToString();
                txtIsActive.Checked = _data.IsActive;
                txtSafety.Text = _data.SafetyStockQTY.ToString();

                #region [ Load Combo ]

                cmbProductGroup_L3.SetAutoCompleteValue(
                    new List<ProductGroupLevel3>
                    {
                        new ProductGroupLevel3
                        {
                            Name =_data.ProductGroupLevel3Name,
                            ProductGroupLevel3ID =  _data.ProductGroupLevel3ID == null ? Guid.Empty : _data.ProductGroupLevel3ID.Value
                        }
                    }
                );

                cmbProductBrand.SetAutoCompleteValue(
                    new List<ProductBrand>
                    {
                        new ProductBrand
                        {
                            Name =_data.ProductBrandName,
                            ProductBrandID =  _data.ProductBrandID == null ? Guid.Empty : _data.ProductBrandID.Value
                        }
                    }
                );

                cmbProductShape.SetAutoCompleteValue(
                   new List<ProductShape>
                    {
                        new ProductShape
                        {
                            Name =_data.ProductShapeName,
                            ProductShapeID =_data.ProductShapeID == null ? Guid.Empty :_data.ProductShapeID.Value
                        }
                    }
                );

                #endregion [ Load Combo ]

                #region [ Get All Tab Detail ]

                radioGroupProductCode.CheckedItems.Clear();
                Bind_Product_Code(_data.ProductCodeModel);

                #endregion [ Get All Tab Detail ]

            }
            #endregion [ Product Detail ]

        }

        protected List<ProductCodes> AddListProductCode(List<ProductCodes> listProductcode, TextField txtCode, Radio radioCode, string strProductDesc)
        {
            if (!string.IsNullOrWhiteSpace(txtCode.Text))
            {
                System.Guid id = new Guid();
                if (txtProductId.Text != "new")
                {
                    id = new Guid(txtProductId.Text);
                }

                listProductcode.Add(new ProductCodes()
                {
                    Code = txtCode.Text,
                    ProductID = id,
                    IsDefault = radioCode.Checked,
                    CodeType = (ProductCodeTypeEnum)Enum.Parse(typeof(ProductCodeTypeEnum), strProductDesc),
                    Description = strProductDesc
                });
            }

            return listProductcode;
        }

        protected void tabDetail_Click(object sender, EventArgs e)
        {
            btnSave.Enable();
        }

        protected void tabSpec_Click(object sender, EventArgs e)
        {
            //this.btnSave.Disable();
        }

        protected void tabReplace_Click(object sender, EventArgs e)
        {
            //this.btnSave.Disable();
            // Get_Product_Replace(this.txtSysProductCode.Text);
        }

        protected void tabSafetyStock_Click(object sender, EventArgs e)
        {
            //Get_Product_SafetyStock(txtSysProductCode.Text);
        }

        protected void tabStdUnit_Click(object sender, EventArgs e)
        {
            btnSave.Disable();
            Get_Product_Unit(hisSysProduct.Text);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateTabBasic())
            {
                return;
            }

            #region [ Get Data to Model ]
            Product productModel = new Product();

            if (txtProductId.Text != "new")
            {
                productModel.ProductID = new Guid(txtProductId.Text);
            }

            productModel.ProductCode = txtStockCode.Text;
            productModel.ProductCodeOld = hisProductCodeOld.Text;
            productModel.Name = txtProductName.Text;
            productModel.SafetyStockQTY = txtSafety.Text == "" ? 1 : txtSafety.Text.ToInt();
            productModel.ProductModel = txtProductModel.Text;
            productModel.IsActive = txtIsActive.Checked;

            if (cmbProductGroup_L3.SelectedItem.Value != null)
            {
                productModel.ProductGroupLevel3ID = new Guid(cmbProductGroup_L3.SelectedItem.Value);
            }
            else
            {
                productModel.ProductGroupLevel3ID = Guid.Empty;
            }

            if (cmbProductBrand.SelectedItem.Value != null)
            {
                productModel.ProductBrandID = new Guid(cmbProductBrand.SelectedItem.Value);
            }
            else
            {
                productModel.ProductBrandID = Guid.Empty;
            }

            if (cmbProductShape.SelectedItem.Value != null)
            {
                productModel.ProductShapeID = new Guid(cmbProductShape.SelectedItem.Value);
            }
            else
            {
                productModel.ProductShapeID = Guid.Empty;
            }

            productModel.IsActive = txtIsActive.Checked;
            productModel.Age = txtAgeing.Text == "" ? 0 : Convert.ToDouble(txtAgeing.Text);

            if (cmbProductUOMTemplate.SelectedItem.Value != null)
            {
                productModel.ProductUOMTemplateID = new Guid(cmbProductUOMTemplate.SelectedItem.Value);
            }

            #region [ Product Code ]
            List<ProductCodes> listProductcode = new List<ProductCodes>();
            AddListProductCode(listProductcode, txtStockCode, radioStockCode, "Stock");
            AddListProductCode(listProductcode, txtWHCode, radioWHCode, "Warehouse");
            AddListProductCode(listProductcode, txtSupplierCode, radioSupplierCode, "Supplier");
            AddListProductCode(listProductcode, txtManufacturingCode, radioManufacturingCode, "Manufacturing");
            AddListProductCode(listProductcode, txtOEMCode, radioOEMCode, "Commercial");
            #endregion [Product Code]

            productModel.CodeCollection = listProductcode;

            string productId = string.Empty;

            if (hisSysProduct.Text != "new")
            {
                ApiResponseMessage datasave = new ApiResponseMessage();
                bool isSuccess = true;
                if (txtProductId.Text == "new")
                {
                    datasave = ClientService.Master.ProductClient.AddProduct(productModel).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    else
                    {
                        ApiResponseMessage result = datasave.Get<ApiResponseMessage>();
                        productId = result.text;
                    }
                }
                else
                {
                    datasave = ClientService.Master.ProductClient.ModifyProduct(productModel).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    else
                    {
                        ApiResponseMessage result = datasave.Get<ApiResponseMessage>();
                        productId = result.text;
                    }
                }

                if (isSuccess)
                {
                    txtProductCode.ReadOnly = true;
                    //this.btnEditCode.Hide();

                    hisSysProduct.Text = productId;
                    panelUOMTemplate.Hide();
                    X.Call("parent.App.direct.SomeDirectMethod", datasave.ResponseCode);
                    X.Js.Call("showTabs");
                }
            }
        }

        protected void Bind_Product_Code(List<ProductCodes> _pCode)
        {
            bool isCheckRadio = false;
            foreach (ProductCodes item in _pCode)
            {
                if (item.IsDefault == true)
                {
                    isCheckRadio = true;
                }
                else
                {
                    isCheckRadio = false;
                }

                switch (item.CodeType.ToString())
                {
                    case "Stock":
                        txtStockCode.Text = item.Code;
                        txtStockCode.TagString = item.Code;
                        radioStockCode.Checked = isCheckRadio;
                        break;
                    case "Warehouse":
                        txtWHCode.Text = item.Code;
                        txtWHCode.TagString = item.Code;
                        radioWHCode.Checked = isCheckRadio;
                        break;
                    case "Supplier":
                        txtSupplierCode.Text = item.Code;
                        txtSupplierCode.TagString = item.Code;
                        radioSupplierCode.Checked = isCheckRadio;
                        break;
                    case "Manufacturing":
                        txtManufacturingCode.Text = item.Code;
                        txtManufacturingCode.TagString = item.Code;
                        radioManufacturingCode.Checked = isCheckRadio;
                        break;
                    case "Commercial":
                        txtOEMCode.Text = item.Code;
                        txtOEMCode.TagString = item.Code;
                        radioOEMCode.Checked = isCheckRadio;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnShowUOMTemplate_Click(object sender, EventArgs e)
        {

            System.Guid id = new Guid(cmbProductUOMTemplate.SelectedItem.Value);

            if (!string.IsNullOrWhiteSpace(cmbProductUOMTemplate.SelectedItem.Value))
            {
                ApiResponseMessage data = ClientService.Master.ProductTemplateUOMClient.GetProductTemplateUomDetail(id).Result;
                List<ProductTemplateUomDetail> _data = data.Get<List<ProductTemplateUomDetail>>();
                StoreShowUOMTemplate.DataSource = _data;
                StoreShowUOMTemplate.DataBind();
                winUOMTemplate.Show();
            }
            else
            {
                MessageBoxExt.ShowError("Please Select UOM Template");
            }
        }


        #region [ Unit ]

        protected void btnAddStdUnit_Click(object sender, DirectEventArgs e)
        {
            Icon iconWindows = Icon.ApplicationFormAdd;
            string strTitle = GetResource("UNIT");
            WindowShow.Show(this, strTitle, "CreateUnit", "frmCreateStandard_Unit.aspx?IsPopup=1", iconWindows, 350, 160);

        }

        [DirectMethod(Timeout=180000)]
        public void ReloadUOMMethod(string param)
        {
            StdUnitStore.Reload();

            ApiResponseMessage apiResp = UnitsClient.GetUnit("", false, null, null).Result;
            List<Units> data = new List<Units>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<Units>>();
                StdUnitStore.DataSource = data;
                StdUnitStore.DataBind();

                if (data.Count > 0)
                {
                    cmbStdUnit.SetDefaultValue(param, data.FirstOrDefault().ShortName);
                    //txtUnitShortName.Text = data.FirstOrDefault().ShortName;
                    txtQtyUnit.Focus(true);
                }

            }
        }

        protected void grdUnitList_CellDblClick(object sender, DirectEventArgs e)
        {
            if (chkSKU.Checked == true)
            {
                txtQtyUnit.Disabled = true;
            }
            else
            {
                txtQtyUnit.Disabled = false;
            }
            string oDataKeyId = e.ExtraParams["DataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }

            ProductUnit EditModel = JSON.Deserialize<ProductUnit>(oDataKeyId);

            cmbStdUnit.SetAutoCompleteValue(
                 new List<Units>
                    {
                        new Units
                        {
                            Name = EditModel.Name,
                            ShortName =EditModel.Name,
                        }
                    }, EditModel.Name
              );

            //this.txtUnitShortName.Text = EditModel.Product_UOM_Short_Name;
            txtQtyUnit.Text = EditModel.Quantity.ToString();
            hidStdUnitID.Text = EditModel.ProductUnitID.ToString();
            hidStdUnitName.Text = EditModel.Name.ToString();
            txtPalletQty.Text = EditModel.PalletQTY.ToString();
            txtWUnit.Text = EditModel.Width.ToString();
            txtLUnit.Text = EditModel.Length.ToString();
            txtHUnit.Text = EditModel.Height.ToString();

            if (EditModel.IsBaseUOM == true)
            {
                chkSKU.Checked = true;
                txtQtyUnit.Text = "1";
            }
            else
            {
                chkSKU.Checked = false;
            }


            txtWeightUnit.Text = EditModel.ProductWeight.ToString();

            if (EditModel.ProductWeight == 0)
            {
                txtPackWeight.Reset();
            }
            else
            {
                txtPackWeight.Text = EditModel.PackageWeight.ToString();
            }

            txtQtyUnit.Focus(true, 100);

        }

        private void Get_StdUnit()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Unit" }
            };
            StdUnitStore.AutoCompleteProxy(AppDataService, param);
        }

        protected void Get_Product_Unit(string productID)
        {
            int total = 0;
            System.Guid? id = new Guid();
            if (productID != "new")
            {
                id = new Guid(productID);
            }

            ApiResponseMessage apiResp = ProductUnitsClient.GetProductUnits(null, id, "", null, null).Result;
            List<ProductUnit> data = new List<ProductUnit>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductUnit>>();
            }

            //can't edit on received product on first release
            if (data.Count() > 0)
            {
                //this.palletSKU.Text = data.Where(x => x.IsBaseUOM == true).FirstOrDefault().Name;
                //data.FirstOrDefault().IsActive;
                if (data.Where(x => x.IsBaseUOM == false).Count() == 0)
                {
                    chkSKU.Disable();
                    //this.radioSKU.Disable();
                    //this.radioSKUNo.Checked = true;
                }
                else
                {
                    chkSKU.Enable();
                    //this.radioSKU.Enable();
                    //this.radioSKUYes.Checked = true;
                    txtQtyUnit.Disable();
                }
            }

            chkSKU.Checked = true;
            txtPalletQty.Text = "1";
            txtQtyUnit.Text = "1";
            txtWUnit.Text = "1";
            txtLUnit.Text = "1";
            txtHUnit.Text = "1";

            UOMStore.DataSource = data;
            UOMStore.DataBind();

        }

        protected void UnitCommandClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            System.Guid id = new Guid(oDataKeyId);

            string gridJson = e.ExtraParams["ParamStoreUOM"];
            // Array of Dictionaries
            List<ProductUnit> gridData = JSON.Deserialize<List<ProductUnit>>(gridJson);

            foreach (ProductUnit uomdata in gridData)
            {
                if (id == uomdata.ProductUnitID)
                {
                    if (uomdata.IsBaseUOM == true)
                    {
                        MessageBoxExt.ShowError("Can't Delete SKU item.");
                        return;
                    }

                    break;
                }
            }

            ProductUnit uomItem = new ProductUnit
            {
                ProductUnitID = id,
                //uomItem.Product_UOM_Status = 1;
                IsActive = false
            };

            ApiResponseMessage ok = ClientService.Master.ProductUnitsClient.RemoveProductUnits(id).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
            }
            else
            {
                NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
            }

            if (ok.IsSuccess)
            {
                Get_Product_Unit(txtProductId.Text);
                cmbStdUnit.Reset();
                txtQtyUnit.Reset();
                txtWeightUnit.Reset();
                txtPalletQty.Reset();
                txtWUnit.Reset();
                txtLUnit.Reset();
                txtHUnit.Reset();
                txtPackWeight.Reset();
                chkSKU.Reset();
                hidStdUnitID.Reset();
                hidStdUnitName.Reset();
            }
            else
            {
                //MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
            }


        }

        protected void btnAddStdUnitItem_Click(object sender, EventArgs e)
        {
            bool isSuccess = true;

            ProductUnit uomItem = new ProductUnit();
            if (!string.IsNullOrWhiteSpace(hidStdUnitID.Text))
            {
                uomItem.ProductUnitID = new Guid(hidStdUnitID.Text);
            }

            uomItem.ProductID = new Guid(hisSysProduct.Text);
            uomItem.Code = cmbStdUnit.SelectedItem.Text;
            uomItem.Name = cmbStdUnit.SelectedItem.Text;
            uomItem.Quantity = txtQtyUnit.Text == "" ? 1 : Convert.ToDecimal(txtQtyUnit.Text);
            uomItem.Width = Convert.ToDouble(txtWUnit.Text);
            uomItem.Length = Convert.ToDouble(txtLUnit.Text);
            uomItem.Height = Convert.ToDouble(txtHUnit.Text);
            uomItem.ProductWeight = Convert.ToDouble(txtWeightUnit.Text);
            uomItem.PackageWeight = Convert.ToDouble(txtPackWeight.Text);
            uomItem.PalletQTY = txtPalletQty.Text.ToInt();
            uomItem.Description = "";
            uomItem.Cubicmeters = 0;
            uomItem.Barcode = "";
            uomItem.URLImage = "";

            if (chkSKU.Checked)
            {
                uomItem.IsBaseUOM = true;
                uomItem.Quantity = 1;
            }
            else
            {
                uomItem.IsBaseUOM = false;
            }

            uomItem.IsActive = true;

            if (string.IsNullOrEmpty(hidStdUnitID.Text))
            {
                ApiResponseMessage datasave = ClientService.Master.ProductUnitsClient.AddProductUnits(uomItem).Result;

                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
            }
            else
            {
                ApiResponseMessage datamodify = ClientService.Master.ProductUnitsClient.ModifyProductUnits(uomItem).Result;
                if (datamodify.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datamodify.ResponseMessage);
                }
            }

            if (isSuccess)
            {
                Get_Product_Unit(uomItem.ProductID.ToString());
                cmbStdUnit.Reset();
                txtQtyUnit.Reset();
                txtPalletQty.Reset();
                txtWUnit.Reset();
                txtLUnit.Reset();
                txtHUnit.Reset();
                chkSKU.Reset();
                txtWeightUnit.Reset();
                txtPackWeight.Reset();
                hidStdUnitID.Reset();
                hidStdUnitName.Reset();
                txtQtyUnit.Text = "1";
                txtPalletQty.Text = "1";
                txtWUnit.Text = "1";
                txtLUnit.Text = "1";
                txtHUnit.Text = "1";
            }
            else
            {
                // MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
            }

        }

        #endregion [ Unit ]

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.SomeDirectMethod", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            radioGroupProductCode.Reset();
            //this.txtSysProductCode.Reset();
            txtProductCode.Reset();
            radioStockCode.Reset();
            txtSupplierCode.Reset();
            radioSupplierCode.Reset();
            txtOEMCode.Reset();
            radioOEMCode.Reset();
            txtWHCode.Reset();
            radioWHCode.Reset();
            txtManufacturingCode.Reset();
            radioManufacturingCode.Reset();
            txtProductName.Reset();
            //this.txtShortProductName.Reset();
            txtProductModel.Reset();
            txtAgeing.Reset();
            cmbProductGroup_L3.Reset();
            cmbProductBrand.Reset();
            cmbProductShape.Reset();
            cmbStdUnit.Reset();
            hidStdUnitID.Reset();
            hidStdUnitName.Reset();
            //this.txtUnitShortName.Reset();
            txtQtyUnit.Reset();
            txtPalletQty.Reset();
            txtWUnit.Reset();
            txtLUnit.Reset();
            txtHUnit.Reset();
            chkSKU.Reset();
            radioIsQCYes.Reset();
            radioIsQCNo.Reset();
            radioIsPalletYes.Reset();
            radioIsPalletNo.Reset();

        }

        protected bool ValidateTabBasic()
        {
            RadioGroup a = radioGroupProductCode;

            if (radioGroupProductCode.CheckedItems.Count == 0)
            {
                MessageBoxExt.ShowError("กรุณาเลือก Main Code ");
                return false;
            }

            if (radioGroupProductCode.CheckedItems[0].InputValue == "App.radioStockCode" && string.IsNullOrWhiteSpace(txtStockCode.Text))
            {
                MessageBoxExt.ShowError("รหัส Stock Code ไม่ถูกต้อง");
                return false;
            }
            else if (radioGroupProductCode.CheckedItems[0].InputValue == "App.radioWHCode" && string.IsNullOrWhiteSpace(txtWHCode.Text))
            {
                MessageBoxExt.ShowError("รหัส Warehouse Code ไม่ถูกต้อง");
                return false;
            }
            else if (radioGroupProductCode.CheckedItems[0].InputValue == "App.radioSupplierCode" && string.IsNullOrWhiteSpace(txtSupplierCode.Text))
            {
                MessageBoxExt.ShowError("รหัส Supplier Code ไม่ถูกต้อง");
                return false;
            }
            else if (radioGroupProductCode.CheckedItems[0].InputValue == "App.radioManufacturingCode" && string.IsNullOrWhiteSpace(txtManufacturingCode.Text))
            {
                MessageBoxExt.ShowError("รหัส Manufacturing Code ไม่ถูกต้อง");
                return false;
            }
            else if (radioGroupProductCode.CheckedItems[0].InputValue == "App.radioOEMCode" && string.IsNullOrWhiteSpace(txtOEMCode.Text))
            {
                MessageBoxExt.ShowError("รหัส Commercial Code ไม่ถูกต้อง");
                return false;
            }

            if (txtProductId.Text == "new")
            {
                if (string.IsNullOrWhiteSpace(cmbProductUOMTemplate.GetValue()))
                {
                    MessageBoxExt.ShowError("Please Select UOM Template.");
                    return false;
                }
            }

            if (txtProductId.Text == "new")
            {
                if (string.IsNullOrWhiteSpace(cmbProductGroup_L3.GetValue()))
                {
                    MessageBoxExt.ShowError("Please Select Product Categoty.");
                    return false;
                }
            }


            int ageing = txtAgeing.Text == "" ? 0 : Convert.ToInt16(txtAgeing.Text);
            if (ageing == 0)
            {
                MessageBoxExt.ShowError("Please enter Aging.");
                return false;
            }

            return true;
        }

        #endregion [ Validation ]
    }


}