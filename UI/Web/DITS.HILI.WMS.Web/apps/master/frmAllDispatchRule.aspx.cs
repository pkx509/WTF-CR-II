using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllDispatchRule : BaseUIPage
    {
        public static string ProgramCode = "P-0093";
        private readonly string AppDataService = "../../Common/DataClients/MsDataHandler.ashx";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string code = Request.QueryString["oDataKeyId"];
                getSpecialBookingRule();
            }

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

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            getSpecialBookingRule();
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "delete")
            {
                ApiResponseMessage ok = ClientService.Master.WarehouseClient.RemoveBookingRule(Id).Result;

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
        }

        private void getSpecialBookingRule()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "SpecialBookingRule" },
                { "query", txtSearch.Text },
                { "Active", ckbIsActive.Checked }
            };

            StoreSpecialBookingRule.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            StoreSpecialBookingRule.AutoCompleteProxy(AppDataService, param);
            StoreSpecialBookingRule.LoadProxy();
        }

        protected async void btnAddSpecialBookingRule_Click(object sender, DirectEventArgs e)
        {
            try
            {

                ApiResponseMessage apiResp = WarehouseClient.GetBookingRule(txtSearch.Text, false, null, null).Result;
                List<SpecialBookingRule> data = new List<SpecialBookingRule>();
                if (apiResp.IsSuccess)
                {
                    data = apiResp.Get<List<SpecialBookingRule>>();
                }

                bool Ok = data.Any(x => x.IsDefault == true && x.IsActive == true);

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (Ok && chkIsDefault.Checked == true)
                {
                    MessageBoxExt.ShowError("Check IsDefault!");
                    return;

                }
                if (!string.IsNullOrWhiteSpace(txtRule_name.Text))
                {
                    SpecialBookingRule sbr = new SpecialBookingRule
                    {
                        RuleName = txtRule_name.Text,
                        Aging = nbLotAging.Text.ToInt(),
                        UnitAging = "DAYS",
                        DurationNotOver = nbLotDuration.Text.ToInt(),
                        UnitDuration = "DAYS",
                        NoMoreThanDo = nbLotLessThan.Text.ToInt(),
                        IsActive = chkIsActive.Checked,
                        IsDefault = chkIsDefault.Checked
                    };

                    datasave = ClientService.Master.WarehouseClient.AddBookingRule(sbr).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }

                    if (isSuccess)
                    {
                        txtRule_name.Reset();
                        nbLotAging.Reset();
                        nbLotDuration.Reset();
                        nbLotLessThan.Reset();
                        getSpecialBookingRule();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }

        }

        [DirectMethod(Timeout=180000)]
        public void Edit(string id, string field, string oldValue, string newValue, object gridJson)
        {
            List<SpecialBookingRule> specialBookingList = new List<SpecialBookingRule>();
            SpecialBookingRule _search = JSON.Deserialize<SpecialBookingRule>(gridJson.ToString());

            ApiResponseMessage apiResp = WarehouseClient.GetBookingRule(txtSearch.Text, true, null, null).Result;
            List<SpecialBookingRule> data = new List<SpecialBookingRule>();
            if (apiResp.IsSuccess)
            {
                data = apiResp.Get<List<SpecialBookingRule>>();
            }
            //bool _isdefault = false;
            //bool _isdefault_change = false;
            //int  countData = data.Where(x => x.IsDefault == true).ToList().Count();
            //var Data_default = data.Where(x => x.IsDefault == true).SingleOrDefault();
            SpecialBookingRule _data_current = data.Where(x => x.RuleId == _search.RuleId).SingleOrDefault();
            // var _data = data.Where(x => x.RuleId == _search.RuleId).ToList();

            //if (Data_default != null && Data_current == null) {
            //    _isdefault_change = true;
            //}

            //if (Data_current != null)
            //{
            //    if (id == Data_current.RuleId.ToString())
            //    {
            //        _isdefault = true;
            //    }
            //}



            if (id == _data_current.RuleId.ToString())
            {
                switch (field.ToLower())
                {
                    case "isactive":
                        _data_current.IsActive = bool.Parse(newValue);
                        break;
                    case "isdefault":
                        if (!bool.Parse(newValue))
                        {
                            MessageBoxExt.ShowError("Select Default!");
                            return;
                        }
                        _data_current.IsDefault = bool.Parse(newValue);
                        break;
                    case "rulename":
                        _data_current.RuleName = newValue;
                        break;
                    case "aging":
                        _data_current.Aging = int.Parse(newValue);
                        break;
                    case "durationnotover":
                        _data_current.DurationNotOver = int.Parse(newValue);
                        break;
                    case "nomorethando":
                        _data_current.NoMoreThanDo = int.Parse(newValue);
                        break;
                }
            }


            bool isSuccess = true;
            ApiResponseMessage datasave = new ApiResponseMessage();
            datasave = ClientService.Master.WarehouseClient.ModifyBookingRule(_data_current).Result;

            if (datasave.ResponseCode != "0")
            {
                isSuccess = false;
                MessageBoxExt.ShowError(datasave.ResponseMessage);
            }
            if (isSuccess)
            {
                GridPanel1.GetStore().GetById(id).Commit();
                getSpecialBookingRule();
                X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }


        }

        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            NotificationExt.Show("Save", param);
            getSpecialBookingRule();
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }
        }

    }
}