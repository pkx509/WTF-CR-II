using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateContacts : BaseUIPage
    {
        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";
        private void getProvince()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Province" }
            };
            StoreProvince.AutoCompleteProxy(AppDataService, param);
        }
        private void getDistrict()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "District" }
            };
            StoreDistrict.AutoCompleteProxy(AppDataService, param);
        }
        private void getSubDistrict()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "SubDistrict" }
            };
            StoreSubDistrict.AutoCompleteProxy(AppDataService, param);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsPostBack)
                {
                    return;
                }

                string code = Request.QueryString["oDataKeyId"];

                getDataContacts(code);
                if (Request.QueryString["IsPopup"] != null)
                {
                    hidIsPopup.Text = Request.QueryString["IsPopup"];
                }
            }
        }

        private void getDataContacts(string code)
        {
            try
            {
                getDataProductOwner();

                // ID = 0 : Add new
                if (code == "new")
                {
                    txtCode.Text = "";
                    cmbDistrict.Disable();
                    cmbSubDistrict.Disable();
                    chkIsSupplier.Checked = true;
                    chkIsCustomer.Checked = true;
                    return;
                }

                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.ContactClient.GetByID(id).Result;
                Contact _data = data.Get<Contact>();
                if (_data == null)
                {
                    return;
                }

                txtCode.Text = _data.Code.ToString();
                txtName.Text = _data.Name;
                txtContact_Address.Text = _data.Address;

                txtContact_Postcode.Text = _data.PostCode;
                //this.txtContact_Country.Text = dataItem.Supplier_Country;
                txtContact_Tel.Text = _data.Telephone;
                txtContact_Fax.Text = _data.Fax;
                txtContact_Email.Text = _data.Email;
                txtContact_ContractName.Text = _data.CusContactName;
                txtContact_URL.Text = _data.WebSite;
                chkISactive.Checked = _data.IsActive;
                //this.cmbRoute.SelectedItem.Value = _data.SubCust_Route_Code;
                //this.txtIsActive.Checked = dataItem.IsActive;

                foreach (ContactInType Ischeck in _data.ContactInTypeCollection)
                {
                    if (Ischeck.ContactType == ContactType.Customer)
                    {
                        chkIsCustomer.Checked = true;
                    }
                    else if (Ischeck.ContactType == ContactType.Supplier)
                    {
                        chkIsSupplier.Checked = true;
                    }
                }


                cmbProvince.SetAutoCompleteValue(new List<Province> {
                        new Province
                        {
                            Province_Id = _data.Province_Id,
                            Name = _data.ProvinceName
                        }
                   });

                cmbDistrict.SetAutoCompleteValue(new List<District>
                        {
                           new District
                            {
                                District_Id = _data.District_Id,
                                Name = _data.DistrictName
                            }
                        }
                 );

                cmbSubDistrict.SetAutoCompleteValue(new List<SubDistrict>
                        {
                           new SubDistrict
                            {
                                SubDistrict_Id = _data.SubDistrict_Id,
                                Name = _data.SubDistrictName
                            }
                        }
                );

                #region [ Check box Customer ]

                foreach (ProductOwner dataMap in _data.ProductOwnerCollection.ToList())
                {

                    RowSelectionModel sm = gridCustomer.GetSelectionModel() as RowSelectionModel;
                    sm.SelectedRows.Add(new SelectedRow(dataMap.ProductOwnerID.ToString()));
                }

                #endregion [ Check box Customer ]

                SetButton("Edit");
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        private void getDataProductOwner()
        {
            try
            {
                ApiResponseMessage datacustomer = ClientService.Master.ProductOwnerClient.Get("", 0, 0).Result;
                List<ProductOwner> _datacustomer = datacustomer.Get<List<ProductOwner>>();
                if (_datacustomer == null)
                {
                    return;
                }

                StoreCustomer.DataSource = _datacustomer;
                StoreCustomer.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        private void SetButton(string code)
        {
            switch (code)
            {
                case "Add":
                    btnSave.Disable();
                    break;
                case "Edit":
                    btnSave.Enable();
                    break;
                default: break;
            }
        }

        protected void cmbProvince_Change(object sender, DirectEventArgs e)
        {
            cmbDistrict.Clear();
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "District" },
                { "provinceid", cmbProvince.SelectedItem.Value }
            };
            StoreDistrict.AutoCompleteProxy(AppDataService, param);

            cmbDistrict.Reset();
            cmbSubDistrict.Reset();
            txtContact_Postcode.Reset();

            cmbDistrict.Enable();
            cmbDistrict.Focus();
            cmbSubDistrict.Disable();
        }
        protected void cmbDistrict_Change(object sender, DirectEventArgs e)
        {
            cmbSubDistrict.Clear();
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "SubDistrict" },
                { "districtid", cmbDistrict.SelectedItem.Value }
            };
            StoreSubDistrict.AutoCompleteProxy(AppDataService, param);

            txtContact_Postcode.Reset();
            cmbSubDistrict.Enable();
            cmbSubDistrict.Focus();
        }


        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                Contact _contact = new Contact();
                string id = Request.QueryString["oDataKeyId"];

                if (chkIsCustomer.Checked == false && chkIsSupplier.Checked == false)
                {
                    MessageBoxExt.ShowError("Please some check box of Supplier or Customer.");
                    return;
                }

                RowSelectionModel sm = gridCustomer.GetSelectionModel() as RowSelectionModel;

                string gridJson = e.ExtraParams["ParamStoreDetail"];
                List<ProductOwner> gridData = JSON.Deserialize<List<ProductOwner>>(gridJson);
                List<ProductOwner> _selectLocation = new List<ProductOwner>();
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    Guid Key = new Guid(row.RecordID);
                    ProductOwner objSelect = gridData.Where(x => x.ProductOwnerID == Key).FirstOrDefault();
                    if (objSelect != null)
                    {
                        _selectLocation.Add(objSelect);
                    }
                }

                #region [ Get Data to Model ]
                Contact createModel = new Contact();
                List<ContactTypeModel> _contactTypeModel = new List<ContactTypeModel>();
                //createModel.Customer_Code = string.IsNullOrEmpty(AppsInfo.CustomerCode) ? hitCustomer_Code.Text : AppsInfo.CustomerCode;
                if (id != "new")
                {
                    createModel.ContactID = new Guid(id);
                }

                createModel.ProductOwnerCollection = _selectLocation;
                createModel.Code = txtCode.Text;
                createModel.Name = txtName.Text;
                createModel.Telephone = txtContact_Tel.Text;
                createModel.Fax = txtContact_Fax.Text;
                createModel.Email = txtContact_Email.Text;
                createModel.Address = txtContact_Address.Text;//this.txtContact_Address.Text + "" + this.cmbDistrict.SelectedItem.Text + "" + this.cmbSubDistrict.SelectedItem.Text + "" + this.cmbProvince.SelectedItem.Text + "" + this.txtContact_Postcode.Text;
                createModel.SubDistrict_Id = new Guid(cmbSubDistrict.SelectedItem.Value);
                createModel.District_Id = new Guid(cmbDistrict.SelectedItem.Value);
                createModel.Province_Id = new Guid(cmbProvince.SelectedItem.Value);

                createModel.CusContactName = txtName.Text;
                createModel.CusContactTel = txtContact_Tel.Text;
                createModel.CusContactEmail = txtContact_Email.Text;
                createModel.CusContactMobile = txtContact_Tel.Text;

                createModel.IsCustomer = chkIsCustomer.Checked;
                createModel.IsActive = chkISactive.Checked;
                createModel.IsSupplier = chkIsSupplier.Checked;
                createModel.ContactType = (ContactType)Enum.Parse(typeof(ContactType), chkIsSupplier.Checked == true ? "Supplier" : "3"); //this.chkIsSupplier.Checked == true ? 1 : null;
                createModel.ContactType = (ContactType)Enum.Parse(typeof(ContactType), chkIsCustomer.Checked == true ? "Customer" : "3"); //this.chkIsCustomer.Checked; 


                if (chkIsSupplier.Checked == true)
                {
                    ContactTypeModel _contactType = new ContactTypeModel
                    {
                        Key = 1,
                        Value = "Supplier"
                    };
                    _contactTypeModel.Add(_contactType);
                }
                if (chkIsCustomer.Checked == true)
                {
                    ContactTypeModel _contactType = new ContactTypeModel
                    {
                        Key = 2,
                        Value = "Customer"
                    };
                    _contactTypeModel.Add(_contactType);
                }

                createModel.ContactTypeCollection = _contactTypeModel;

                //createModel.SubCust_Route_Code = this.cmbRoute.SelectedItem.Value;
                #endregion
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (id == "new")
                {
                    datasave = ClientService.Master.ContactClient.Add(createModel).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.ContactClient.Modify(createModel).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "new")
            {
                txtCode.Text = "";
                txtContact_Address.Clear();
                txtContact_ContractName.Clear();
                txtContact_Country.Clear();
                btnSave.Hide();
                txtContact_Email.Clear();
                txtContact_Fax.Clear();
                txtContact_Postcode.Clear();
                txtContact_Tel.Clear();
                txtContact_URL.Clear();
                txtName.Clear();
                //this.StoreOfDataList.RemoveAll();
                cmbProvince.Reset();
                cmbSubDistrict.Reset();
                cmbDistrict.Reset();
                //SetButton(0);
            }
            else
            {
                getDataContacts(txtCode.Text);
            }
        }
    }
}