using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateSiteName : BaseUIPage
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

                if (code != "new")
                {
                    getDataSite(code);
                }
                else
                {
                    txtSiteID.Text = code;
                }
            }
        }

        private void getDataSite(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.SiteClient.GetSite(id, "", true, null, null).Result;
                SiteModel _data = data.Get<List<SiteModel>>().FirstOrDefault();
                if (_data == null)
                {
                    return;
                }

                txtSiteID.Text = _data.SiteID.ToString();
                SiteName.Text = _data.SiteName;
                SiteAdress.Text = _data.SiteAdress;
                SiteRoad.Text = _data.SiteRoad;
                Site_PostCode.Text = _data.SitePostCode;
                SiteTel.Text = _data.SiteTel;
                SiteFax.Text = _data.SiteFax;

                if (!string.IsNullOrWhiteSpace(_data.Province_Id.ToString()))
                {
                    cmbProvince.SetAutoCompleteValue(new List<Province> {
                        new Province
                        {
                            Province_Id = _data.Province_Id.Value,
                            Name = _data.ProvinceName
                        }
                   });
                }
                if (!string.IsNullOrWhiteSpace(_data.Province_Id.ToString()))
                {
                    cmbDistrict.SetAutoCompleteValue(new List<District>
                        {
                           new District
                            {
                                District_Id = _data.District_Id.Value,
                                Name = _data.DistrictName
                            }
                        }
                 );
                }
                if (!string.IsNullOrWhiteSpace(_data.Province_Id.ToString()))
                {
                    cmbSubDistrict.SetAutoCompleteValue(new List<SubDistrict>
                        {
                           new SubDistrict
                            {
                                SubDistrict_Id = _data.SubDistrict_Id.Value,
                                Name = _data.SubDistrictName
                            }
                        }
                  );
                }
                SetButton("Edit");
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
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

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            ApiResponseMessage _site = ClientService.Master.WarehouseClient.GetZone(Guid.Empty, prms.Query, prms.Page, prms.Limit).Result;
            List<Site> data = new List<Site>();
            if (_site.IsSuccess)
            {
                total = _site.Totals;
                data = _site.Get<List<Site>>();
            }

            return new { data, total };
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
            Site_PostCode.Reset();

            cmbDistrict.Enable();
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

            Site_PostCode.Reset();
            cmbSubDistrict.Enable();
        }
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                Site _site = new Site();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _site.SiteID = new Guid(id);
                }

                _site.SiteName = SiteName.Text;
                _site.SiteAdress = SiteAdress.Text;
                _site.SiteRoad = SiteRoad.Text;
                _site.SitePostCode = Site_PostCode.Text;
                _site.SiteTel = SiteTel.Text;
                _site.SiteFax = SiteFax.Text;
                _site.IsActive = txtIsActive.Checked;

                if (!string.IsNullOrWhiteSpace(cmbProvince.SelectedItem.Value))
                {
                    _site.SiteProvince_Id = new Guid(cmbProvince.SelectedItem.Value);
                }

                if (!string.IsNullOrWhiteSpace(cmbDistrict.SelectedItem.Value))
                {
                    _site.SiteDistrict_Id = new Guid(cmbDistrict.SelectedItem.Value);
                }

                if (!string.IsNullOrWhiteSpace(cmbSubDistrict.SelectedItem.Value))
                {
                    _site.SiteSubDistrict_Id = new Guid(cmbSubDistrict.SelectedItem.Value);
                }

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtSiteID.Text == "new")
                {
                    datasave = ClientService.Master.SiteClient.Add(_site).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.SiteClient.Modify(_site).Result;
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
            if (txtSiteID.Text == "new")
            {
                txtSiteID.Text = "new";
                SiteName.Clear();
                SiteAdress.Clear();
                SiteRoad.Clear();
                btnSave.Hide();
                SiteRoad.Clear();
                Site_PostCode.Clear();
                SiteTel.Clear();
                SiteFax.Clear();
                cmbProvince.Reset();
                cmbSubDistrict.Reset();
                cmbDistrict.Reset();
                //SetButton(0);
            }
            else
            {
                getDataSite(txtSiteID.Text);
            }
        }
    }
}