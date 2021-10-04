using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.PutAwayModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmQA1 : BaseUIPage
    {
        string AutoCompleteService = "~/Common/DataClients/OptDataHandler.ashx";
        public string _putawayjobcode
        {
            get
            {

                if (!string.IsNullOrEmpty(Request.QueryString["oDataPutAwayJobCode"]))
                {
                    return Request.QueryString["oDataPutAwayJobCode"].ToString();
                }
                else
                {
                    return "";
                }

            }
        }
        #region Initail
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            populateData();
        }

        private void populateData()
        {
            getAssing();
            getPutawayDetailList();
        }
        private async void getAssing()
        {
            try
            {
                cmbMutiAssign.Items.Clear();

                List<Employee> emp = new List<Employee>();

                var apiResp = ClientService.Master.EmployeeClient.Get("", null, null).Result;
                int total = 0;
                if (apiResp.IsSuccess)
                {
                    emp = apiResp.Get<List<Employee>>();
                    total = apiResp.Totals;
                }


                //Ref<int> total = new Ref<int>();
                //var emp = await ClientService.Master.EmployeeClient.Get("", total, null, null);
                emp.ForEach(item =>
                {
                    cmbMutiAssign.Items.Add(new Ext.Net.ListItem
                    {
                        Text = item.FirstName + "" + item.LastName,
                        Value = item.EmployeeID.ToString()
                    });

                    //cmbMutiAssign.SelectedItems.Add(new Ext.Net.ListItem
                    //{
                    //    Text = item.FirstName  + "" + item.LastName ,
                    //    Value = item.ID.ToString()
                    //});

                });
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBoxExt.ShowError("Load Assign Fail : " + ex.Message.ToString());
            }

        }

        private void AutoCompleteProxy(string putawayjobcode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Method", "putawaydetaillist");
            param.Add("putawayjobcode", putawayjobcode);
            this.PutawayDetailStore.AutoCompleteProxy(AutoCompleteService, param);
            this.PutawayDetailStore.LoadProxy();
        }

        private async void getPutawayDetailList()
        {
            try
            {

                var putawaydetaillist = await ClientService.Inbound.PutAwayClient.GetJobPutaway(_putawayjobcode);
                var putawaydetail = putawaydetaillist.FirstOrDefault();
                if (putawaydetail == null)
                    return;

                // var ddd = putawaydetaillist.FirstOrDefault().PutAwayConfirmCollection.FirstOrDefault().UserModified;
                this.txtQANo.Text = putawaydetail.PutAwayJobCode;

                var scount = putawaydetaillist.Where(x => x.Status == PutAwayStatusEnum.Complete).Count();

                if (putawaydetail.Status != PutAwayStatusEnum.Draft)
                {
                    this.btnSave.Hidden = true;
                }
                if (putawaydetail.Status == PutAwayStatusEnum.New || putawaydetail.Status == PutAwayStatusEnum.Draft || scount > 0)
                {
                    this.btnCancel.Hidden = true;
                }
                this.cmbMutiAssign.Items.Clear();
                this.cmbMutiAssign.SelectedItems.Clear();
                this.cmbMutiAssign.UpdateSelectedItems();


                //Ref<int> total = new Ref<int>();
                //var emp = await ClientService.Master.EmployeeClient.Get("", total, null, null);

                List<Employee> emp = new List<Employee>();

                var apiResp = ClientService.Master.EmployeeClient.Get("", null, null).Result;
                int total = 0;
                if (apiResp.IsSuccess)
                {
                    emp = apiResp.Get<List<Employee>>();
                    total = apiResp.Totals;
                }

                emp.ForEach(item =>
                {
                    cmbMutiAssign.Items.Add(new Ext.Net.ListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.EmployeeID.ToString()
                    });
                    var dataassign = putawaydetail.AssignJobCollection.Where(rsg => rsg.EmployeeID == item.EmployeeID).FirstOrDefault();
                    if (dataassign != null)
                    {
                        cmbMutiAssign.SelectedItems.Add(new Ext.Net.ListItem
                        {
                            Text = item.FirstName + " " + item.LastName,
                            Value = item.EmployeeID.ToString()
                        });

                    }
                });
                this.cmbMutiAssign.UpdateSelectedItems();

                foreach (var item in putawaydetaillist)
                {
                    item.StockUnitName = item.PutAwayDetailCollection.FirstOrDefault().StockUOMName;
                }

                this.PutawayDetailStore.DataSource = putawaydetaillist;
                this.PutawayDetailStore.DataBind();
                this.GridPutawayDetail.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Load Grid Fail : " + ex.Message.ToString());

            }
        }
        #endregion

        #region Event Handle
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.GetAddForm();
        }
        private void GetEditForm(string oDataPutAwayJobCode, PutAwayStatusEnum oDataPutawayStatus)
        {

            string strTitle = "Edit QA Product";


            WindowShow.ShowNewPage(this, strTitle, "PutawayOrderPage",
                                        "frmQAAddProduct.aspx?oDataPutAwayJobCode=" + oDataPutAwayJobCode +
                                        "&oDataPutawayStatus=" + oDataPutawayStatus, Icon.Newspaper);
        }
        private void GetAddForm()
        {

            string strTitle = "Add QA Product";


            WindowShow.ShowNewPage(this, strTitle, "PutawayOrderPage",
                                        "frmQAAddProduct.aspx", Icon.Newspaper);
        }
        protected async void btnCancel_Click(object sender, DirectEventArgs e)
        {
            try
            {
                var cancel = await ClientService.Inbound.PutAwayClient.Cancel(_putawayjobcode);

                NotificationExt.Show("Cancek", "Cancel complete");

                X.Call("parent.App.direct.Reload");
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Cancel Fail : " + ex.Message.ToString());
            }
        }

        protected async void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {

                var _itemassign = cmbMutiAssign.SelectedItems.ToList();
                if (_itemassign.Count == 0)
                {
                    MessageBoxExt.ShowError("Please select assign!");
                    return;
                }


                JobPutAway data = new JobPutAway();
                data.JobPutAwayNo = this.txtQANo.Text;
                data.PutAwayDate = DateTime.Now;

                List<Guid> _AssignModel = new List<Guid>();
                foreach (var item in _itemassign)
                {
                    _AssignModel.Add(new Guid(item.Value));
                }
                data.EmployeeID = _AssignModel;

                var save = await ClientService.Inbound.PutAwayClient.Modify(data);

                NotificationExt.Show("Save", "Save complete");

                X.Call("parent.App.direct.Reload");
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Cancel Fail : " + ex.Message.ToString());
            }
        }

        protected async void btnClear_Click(object sender, DirectEventArgs e)
        {
            //this.ucApproveViewConfirmPutaway.Show("approve", "Approve Putaway", null,720,380);
            getPutawayDetailList();
        }

        protected async void btnPrint_Click(object sender, DirectEventArgs e)
        {
            // this.ucApproveViewConfirmPutaway.Show("view", "View Putaway", null, 720, 380);
            getPutawayDetailList();
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            try
            {
                string command = e.ExtraParams["command"];
                Guid oDataPutAwayID = new Guid(e.ExtraParams["oDataPutAwayID"]);
                string gridJson = e.ExtraParams["oDataSelect"];

                var _putawaydetail = Newtonsoft.Json.JsonConvert.DeserializeObject<PutAway>(gridJson);

                if (command.ToLower() == "edit")
                {
                    this.ucQAConfirm.LoadData();
                    this.ucQAConfirm.Show_Approve(oDataPutAwayID, "view", "View Putaway");
                }

            }
            catch (Exception ex)
            {
                //  throw ex;
                MessageBoxExt.ShowError("Action Fail : " + ex.Message.ToString());
            }


        }
        #endregion

        #region DirectMethod
        [DirectMethod]
        public void Reload()
        {
            getPutawayDetailList();
        }
        #endregion

    }
}