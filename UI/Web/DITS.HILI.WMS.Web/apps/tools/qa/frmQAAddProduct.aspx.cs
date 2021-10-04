using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.PutAwayModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmQAAddProduct : BaseUIPage
    {
        string AutoCompleteService = "~/Common/DataClients/OptDataHandler.ashx";

        #region Initail
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            populateData();
        }

        private void populateData() {
            getLoadingIN();
            getWarehouse();
            //getPutaway();
        }

        private async void getLoadingIN()
        {
            try
            {
                //var location = await ClientService.Master.WarehouseClient.GetLoadingINLocation();
                List<Location> location = new List<Location>();

                var apiResp = ClientService.Master.WarehouseClient.GetLoadingINLocation().Result;
                int total = 0;
                if (apiResp.IsSuccess)
                {
                    location = apiResp.Get<List<Location>>();
                    total = apiResp.Totals;
                }

                location.Insert(0, new Location()
                {
                    LocationID = new Guid( "00000000-0000-0000-0000-000000000000"),
                    Code = "All"

                });

                this.LocationStore.DataSource = location;
                this.LocationStore.DataBind();

                this.cmbLoadingIn.SelectedItem.Index = 0;
                this.cmbLoadingIn.UpdateSelectedItems();

                //cmbLoadingIn.Items.Clear();
                //cmbLoadingIn.Items.Insert(0, new Ext.Net.ListItem
                //{
                //    Text = "All",
                //    Value = ""
                //});
                //var location = await ClientService.Master.WarehouseClient.GetLoadingINLocation();

                //location.ForEach(Item =>
                //{
                //    cmbLoadingIn.Items.Add(new Ext.Net.ListItem
                //    {
                //        Text = Item.Zone.Warehouse.Code + " " + Item.Code,
                //        Value = Item.LocationID.ToString()
                //    });
                //});


                //this.cmbLoadingIn.SelectedItem.Index = 0;
                //this.cmbLoadingIn.UpdateSelectedItems();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private async void getWarehouse()
        {
            try
            {
                List<Warehouse> warehouse = new List<Warehouse>();

                var apiResp = ClientService.Master.WarehouseClient.GetWarehouse(null, "", null, null).Result;
                int total = 0;
                if (apiResp.IsSuccess)
                {
                    warehouse = apiResp.Get<List<Warehouse>>();
                    total = apiResp.Totals;
                }

                
                warehouse.Insert(0, new Warehouse()
                {
                    WarehouseID = new Guid("00000000-0000-0000-0000-000000000000"),
                    Code = "All"

                });

                foreach (var item in warehouse) {
                    item.Code = item.Code + " " + item.Name;
                }
                this.WarehouseStore.DataSource = warehouse;
                this.WarehouseStore.DataBind();
                this.cmbWarehouse.SelectedItem.Index = 0;
                this.cmbWarehouse.UpdateSelectedItems();

                //cmbWarehouse.Items.Clear();
                //cmbLoadingIn.Items.Insert(0, new Ext.Net.ListItem
                //{
                //    Text = "All",
                //    Value = ""
                //});
                //Ref<int> total = new Ref<int>();
                //var warehouse = await ClientService.Master.WarehouseClient.GetWarehouse(null,"", total,null,null);
                //warehouse.ForEach(Item =>
                //{
                //    cmbWarehouse.Items.Add(new Ext.Net.ListItem
                //    {
                //        Text = Item.Code + " " + Item.Name,
                //        Value = Item.WarehouseID.ToString()
                //    });
                //});

                //this.cmbWarehouse.SelectedItem.Index = 0;
                //this.cmbWarehouse.UpdateSelectedItems();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Load Warehouse Fail : " + ex.Message.ToString());
            }

        }

        private async void getPutawayItemList()
        {
            try
            {
                Guid? locationid = null;
                Guid? warehouseid = null;
                if (this.cmbLoadingIn.SelectedItem.Text != "All") {
                    locationid = new Guid(this.cmbLoadingIn.SelectedItem.Value);
                }
                //if (this.cmbWarehouse.SelectedItem.Text != "All")
                //{
                //    warehouseid = new Guid(this.cmbWarehouse.SelectedItem.Value);
                //}
                var putawaydetaillist = await ClientService.Inbound.PutAwayClient.GetInitialItem(this.txtProductCode.Text, this.txtProductName.Text,this.txtLot.Text , locationid, warehouseid);

                this.PutawayDetailStore.DataSource = putawaydetaillist.ToList();
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getPutawayItemList();
        }
        protected async void btnGen_Click(object sender, DirectEventArgs e)
        {
            try {

            string gridJson = e.ExtraParams["ParamStorePages"];
            var _putawaydetaillist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PutAwayItem>>(gridJson);

            var createpitawy = await ClientService.Inbound.PutAwayClient.CreateJobPutaway(_putawaydetaillist);
            NotificationExt.Show("Create Putawy", "Create putawy complete");

             X.AddScript("parent.Ext.WindowMgr.getActive().close();");
             X.Call("parent.App.direct.Reload");

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Create Putawy Fail : " + ex.Message.ToString());
                //throw ex;
            }
        }
        protected  void btnClear_Click(object sender, DirectEventArgs e)
        {
            Cleardata();
        }
        protected  void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }
        private void Cleardata() {
            this.txtProductCode.Clear();
            this.txtProductName.Clear();
            this.txtLot.Clear();
            //  populateData();
            var defaultvalue = new Ext.Net.ListItem { Value = "0" };
            this.cmbLoadingIn.SelectedItems.Clear();
            this.cmbLoadingIn.SelectedItems.Add(defaultvalue);
            this.cmbLoadingIn.UpdateSelectedItems();
            this.cmbWarehouse.SelectedItems.Clear();
            this.cmbWarehouse.SelectedItems.Add(defaultvalue);
            this.cmbWarehouse.UpdateSelectedItems();
            this.PutawayDetailStore.RemoveAll();
            this.PutawayDetailStore.DataBind();
        }
        #endregion

        #region DirectMethod
        #endregion

    }
}