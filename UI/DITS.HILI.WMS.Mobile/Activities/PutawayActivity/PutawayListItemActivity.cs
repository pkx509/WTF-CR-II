using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DITS.HILI.WMS.PutAwayModel;
using DITS.HILI.WMS.MobileService.Inbound;
using DITS.HILI.WMS.MasterModel.Core;
using Newtonsoft.Json;
using DITS.HILI.WMS.MobileService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;

namespace DITS.HILI.WMS.Mobile.Activities.PutawayActivity
{
    [Activity(Label = "Putaway list")]
    public class PutawayListItemActivity : Activity, Ihili42
    {
        #region members
        private ListView putawayListView;
        private EditText scanTxt;
        private List<PutAway> putawayList = new List<PutAway>();
        private List<Warehouse> warehouseList = new List<Warehouse>();
        private Button backBtn;
        private Button scanBtn;
        private Button reloadBtn;
        private Spinner spnWareHouses;
        private string currentWareHouseID,currentWareHouseTypeID,currentWareHouseCode,currentWareHouseName;

        #endregion

        public void initControl()
        {
            try
            {
                this.scanTxt = FindViewById<EditText>(Resource.Id.scanTxt);
                this.scanTxt.AfterTextChanged += ScanTxt_AfterTextChanged;

                this.putawayListView = FindViewById<ListView>(Resource.Id.putawayListView);
                this.putawayListView.ItemClick += PutawayListView_ItemClick;

                this.backBtn = FindViewById<Button>(Resource.Id.paBack);
                this.backBtn.Click += BackBtn_Click;

                this.scanBtn = FindViewById<Button>(Resource.Id.scanBtn);
                this.scanBtn.Click += ScanBtn_Click;

                this.reloadBtn = FindViewById<Button>(Resource.Id.reloadBtn);
                this.reloadBtn.Click += ReloadBtn_Click;

                this.spnWareHouses= FindViewById<Spinner>(Resource.Id.spnPAWarehouses);
                this.spnWareHouses.ItemSelected += SpnWareHouses_ItemSelected;

                this.bindWarehouseSelected();
                this.preparePutawayList(true);
            }
            catch { throw; }
        }

        public override void OnBackPressed()
        {

        }


        #region defind method

        private async void preparePutawayList(bool reload)
        {
            try
            {
                var findValue = "";
                if (!string.IsNullOrEmpty(this.scanTxt.Text))
                    findValue = this.scanTxt.Text.Trim().ToUpper();

                if (reload)
                {
                    Ref<int> total = new Ref<int>();
                    Guid? curWareHouse = null;
                    if (!string.IsNullOrEmpty(this.currentWareHouseID))
                        curWareHouse = new Guid(this.currentWareHouseID);

                    this.putawayList = await PutAwayClient.GetPutAway(curWareHouse, total, null, null);
                }

                if (!string.IsNullOrEmpty(findValue))
                {
                    #region filter from key
                    if (this.putawayList.Count > 0)
                    {
                        var filList = this.putawayList.Where(
                                                c => (c.PalletCode != null && c.PalletCode.Contains(findValue)) ||
                                                (c.ProductCode!=null && c.ProductCode.Contains(findValue))
                                                ).ToList();

                        this.putawayListView.Adapter = new PutawayListItemAdapter(this, filList);
                        if (filList.Count > 0)
                        {
                            this.setSelectedToDetail(0);
                        }
                    }

                    #endregion
                }
                else
                {
                    this.putawayListView.Adapter = new PutawayListItemAdapter(this, this.putawayList);
                }
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void setSelectedToDetail(int listPositionData)
        {
            try
            {
                var item = this.putawayList[listPositionData];

                #region pass data by model
                MdlPutawayConfirmDetail selectedValues = new MdlPutawayConfirmDetail();
                selectedValues.putawayID = item.PutAwayID.ToString();
                selectedValues.productCode = item.ProductCode;
                selectedValues.productName = item.ProductName;
                selectedValues.Quantity = item.Quantity;
                selectedValues.remainQuantity = item.RemainQuantity;

                if (item.PutAwayDetailCollection.ToList().FirstOrDefault()!=null)
                    selectedValues.QuantityUnit = item.PutAwayDetailCollection.ToList().FirstOrDefault().ProductUOM.Name;

                selectedValues.lot = item.Lot;
                selectedValues.palletCode = item.PalletCode;
                selectedValues.productImage = item.Product.URLImage;
                selectedValues.locationCode = item.FromLocation.Code;
                selectedValues.suggLocationCode = item.SuggestionLocation.Code;
                selectedValues.confLocationCode = "";
                selectedValues.whereHouseName = this.currentWareHouseName;

                if (item.PutAwayDetailCollection.ToList().Count > 0)
                {
                    selectedValues.stockUnitID = item.PutAwayDetailCollection.ToList()[0].StockUnitID.ToString();
                    selectedValues.baseUnitID = item.PutAwayDetailCollection.ToList()[0].BaseUnitID.ToString();
                    selectedValues.baseQuantity = item.PutAwayDetailCollection.ToList()[0].BaseQuantity;
                    selectedValues.ConversionQty = item.PutAwayDetailCollection.ToList()[0].ConversionQty;
                }

                #endregion

                var objValues = JsonConvert.SerializeObject(selectedValues);

                Intent Page = new Intent(this, typeof(PutawayItemDetailActivity));
                Page.PutExtra("putawayDetail", objValues);
                StartActivity(Page);
            }
            catch { throw; }
        }

        private async void bindWarehouseSelected()
        {
            try
            {
                Ref<int> total = new Ref<int>();
                this.warehouseList = await WarehouseClient.GetWarehouse(null, "", total, null, null);
                List<string> wareHouses = new List<string>();
                foreach (var itm in this.warehouseList)
                    wareHouses.Add(itm.Name);

                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, wareHouses);
                this.spnWareHouses.Adapter = adapter;
            }
            catch(Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        #endregion

        private void SpnWareHouses_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                this.currentWareHouseID = "";
                this.currentWareHouseTypeID = "";
                this.currentWareHouseCode = "";
                this.currentWareHouseName = "";
                if (this.warehouseList.Count > 0)
                {
                    var spinner = (Spinner)sender;
                    this.currentWareHouseID = this.warehouseList[e.Position].WarehouseID.ToString();
                    this.currentWareHouseTypeID = this.warehouseList[e.Position].WarehouseTypeID.ToString();
                    this.currentWareHouseCode = this.warehouseList[e.Position].Code;
                    this.currentWareHouseName = this.warehouseList[e.Position].Name;

                    this.preparePutawayList(true);
                }
            }
            catch { throw; }
        }

        private void ScanTxt_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            try
            {
                //this.ScanBtn_Click(this.scanBtn, null);
            }
            catch { throw; }
        }

        private void ReloadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.scanTxt.Text = "";
                this.preparePutawayList(true);
            }
            catch { throw; }
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.preparePutawayList(false);
            }
            catch { throw; }
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            try
            {
                new LocalLib().mainPage(this);
            }
            catch
            {
                throw;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PutawayListItem);
            this.initControl();
        }

        private void PutawayListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                this.setSelectedToDetail(e.Position);
            }
            catch { throw; }
        }


    }
}