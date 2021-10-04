using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateZoneType : BaseUIPage
    {

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
                    getDataZone(code);
                }
                else
                {
                    txtZoneType_Code.Text = code;
                    SetButton("Add");
                }

                LoadCombo();
            }
        }

        private void getDataZone(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetZoneType(id, "", true, null, null).Result;
                ZoneType _data = data.Get<List<ZoneType>>().FirstOrDefault();
                if (_data == null)
                {
                    return;
                }

                txtName.Text = _data.Name;
                txtDescription.Text = _data.Description;

                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = Enum.GetName(typeof(SystemKeyEnum), _data.KeyType),
                    Value = _data.KeyType.ToString()
                };
                cmbPlacement_Type.SelectedItem.Value = _data.KeyType.ToString();

                //this.cmbSite.SetAutoCompleteValue(new List<Site>
                //       {
                //               new Site
                //        {
                //            SiteID = _data.Warehouse.SiteID.Value,
                //            SiteName = _data.SiteName,
                //        }
                //       }
                // );

                SetButton("Edit");
                // this.cmbAdjustType.ReadOnly = true;

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
                    btnSave.Disabled = false;
                    break;
                case "Edit":
                    btnSave.Disabled = true;
                    break;
                default: break;
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                ZoneType _zoneType = new ZoneType();
                string id = Request.QueryString["oDataKeyId"];
                if (id != "new")
                {
                    _zoneType.ZoneTypeID = new Guid(id);
                }

                _zoneType.Name = txtName.Text;
                _zoneType.Description = txtDescription.Text;
                _zoneType.KeyType = (SystemKeyEnum)Enum.Parse(typeof(SystemKeyEnum), cmbPlacement_Type.SelectedItem.Text);
                _zoneType.IsActive = txtIsActive.Checked;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtZoneType_Code.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddZoneType(_zoneType).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.ModifyZoneType(_zoneType).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }

                X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
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
            if (txtZoneType_Code.Text == "new")
            {
                txtZoneType_Code.Text = "";
                cmbPlacement_Type.Clear();
                txtName.Clear();
                txtDescription.Clear();
                btnSave.Hide();
                //SetButton(0);
            }
            else
            {
                getDataZone(txtZoneType_Code.Text);
            }
        }

        private void LoadCombo()
        {
            Array values = Enum.GetValues(typeof(SystemKeyEnum));

            List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>(values.Length);

            foreach (object i in values)
            {
                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = Enum.GetName(typeof(SystemKeyEnum), i),
                    Value = i.ToString()
                };
                cmbPlacement_Type.Items.Add(l);
            }

        }
    }
}