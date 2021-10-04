using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateShipTo : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    getDataShip(code);
                }
                else
                {
                    txtShipCode.Text = code;
                }
            }
        }

        private void getDataShip(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetShipToByID(id, null, null).Result;
                ShippingTo _data = data.Get<ShippingTo>();
                if (_data == null)
                {
                    return;
                }

                txtShipName.Text = _data.Name;
                txtShipShortName.Text = _data.ShortName;
                ckbIsActive.Checked = _data.IsActive;
                ckbIsDefault.Checked = (_data.IsDefault != null ? _data.IsDefault.Value : false);
                SpecialBookingRule rule_edit = new SpecialBookingRule
                {
                    RuleId = _data.SpecialBookingRule.RuleId,
                    RuleName = _data.SpecialBookingRule.RuleName
                };
                StoreRule.Add(rule_edit);
                cmbRule.SelectedItem.Value = _data.SpecialBookingRule.RuleId.ToString();
                cmbRule.UpdateSelectedItems();
                //this.cmbRule.SetAutoCompleteValue(new List<SpecialBookingRule>
                //       {
                //           new SpecialBookingRule
                //         {
                //            RuleId = _data.SpecialBookingRule.RuleId,
                //            RuleName = _data.SpecialBookingRule.RuleName
                //         }
                //       }, _data.SpecialBookingRule.RuleId.ToString()
                // );
                SetButton("Edit");
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

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                ShippingTo _shipto = new ShippingTo();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _shipto.ShipToId = new Guid(id);
                }

                _shipto.Name = txtShipName.Text;
                _shipto.RuleId = new Guid(cmbRule.SelectedItem.Value);
                _shipto.IsActive = ckbIsActive.Checked;
                _shipto.IsDefault = ckbIsDefault.Checked;
                _shipto.ShortName = txtShipShortName.Text;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (id == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddShipTo(_shipto).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.ModifyShipTo(_shipto).Result;
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
            if (txtShipCode.Text == "new")
            {
                txtShipCode.Text = "new";
                txtShipCode.Clear();
                txtShipName.Clear();
                btnSave.Hide();
                cmbRule.Reset();

            }
            else
            {
                getDataShip(txtShipCode.Text);
            }
        }
    }
}