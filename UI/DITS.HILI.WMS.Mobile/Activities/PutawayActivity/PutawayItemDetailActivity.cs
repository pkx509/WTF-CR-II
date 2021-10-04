using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DITS.HILI.WMS.PutAwayModel;
using Newtonsoft.Json;
using DITS.HILI.WMS.MobileService.Inbound;
using System.Collections;

namespace DITS.HILI.WMS.Mobile.Activities.PutawayActivity
{
    [Activity(Label = "Putaway detail")]
    public class PutawayItemDetailActivity : Activity,Ihili42
    {
        #region members
        private MdlPutawayConfirmDetail putawayDetail;
        private string currentReasonID;

        //ui control
        private Button clearBtn;
        private Button paBackToListBtn;
        private Button paConfrimBtn;

        private EditText txtPAPallet;
        private EditText txtPAproductCode;
        private EditText txtPAproductName;
        private EditText txtPAQTY;
        private EditText txtPAUnits;
        private EditText txtPARemainQTY;
        private EditText txtPARemainUnits;
        private EditText txtPALocation;
        private EditText txtPASgLocation;
        private EditText txtPAConfLocation;
        private EditText txtPALot;
        private Spinner spnPAReason;
        private ImageView imgProduct;

        private List<PutAwayReason> reasonItems=new List<PutAwayReason>();
        private EditText txtWH;
        #endregion

        public void initControl()
        {
            try
            {
                var objValues = Intent.GetStringExtra("putawayDetail");
                this.putawayDetail= JsonConvert.DeserializeObject<MdlPutawayConfirmDetail>(objValues);
                //new UIUtils().showAlert(this, "Putaway detail", this.putawayDetail.putawayID);

                #region regis control ui
                //regis button control
                this.paBackToListBtn= FindViewById<Button>(Resource.Id.paBackToListBtn);

                this.paConfrimBtn = FindViewById<Button>(Resource.Id.paConfrimBtn);

                this.clearBtn = FindViewById<Button>(Resource.Id.paClearBtn);
                //regis data control
                this.txtWH = FindViewById<EditText>(Resource.Id.txtWH);

                this.txtPAPallet = FindViewById<EditText>(Resource.Id.txtPAPallet);

                this.txtPAproductCode = FindViewById<EditText>(Resource.Id.txtPAproductCode);
                this.txtPAproductCode.Enabled = false;

                this.txtPAproductName = FindViewById<EditText>(Resource.Id.txtPAproductName);
                this.txtPAproductName.Enabled = false;

                this.txtPAQTY = FindViewById<EditText>(Resource.Id.txtPAQTY);

                this.txtPAUnits= FindViewById<EditText>(Resource.Id.txtPAUnit);
                this.txtPAUnits.Enabled = false;

                this.txtPARemainQTY = FindViewById<EditText>(Resource.Id.txtPARemainQTY);
                this.txtPARemainQTY.Enabled = false;

                this.txtPARemainUnits = FindViewById<EditText>(Resource.Id.txtPARemainUnit);
                this.txtPARemainUnits.Enabled = false;

                this.txtPALocation = FindViewById<EditText>(Resource.Id.txtPALocation);
                this.txtPALocation.Enabled = false;

                this.txtPASgLocation = FindViewById<EditText>(Resource.Id.txtPASgLocation);
                this.txtPASgLocation.Enabled = false;

                this.txtPAConfLocation= FindViewById<EditText>(Resource.Id.txtPAConfLoaction);

                this.txtPALot= FindViewById<EditText>(Resource.Id.txtPALot);

                this.spnPAReason= FindViewById<Spinner>(Resource.Id.spnPAReason);

                this.imgProduct= FindViewById<ImageView>(Resource.Id.imgProduct);
                //sync event handle
                this.paBackToListBtn.Click += PaBackToListBtn_Click;
                this.paConfrimBtn.Click += PaConfrimBtn_Click;
                this.clearBtn.Click += ClearBtn_Click;
                this.txtPAConfLocation.AfterTextChanged += TxtPAConfLocation_AfterTextChanged;
                this.txtPAQTY.AfterTextChanged += TxtPAQTY_AfterTextChanged;

                #endregion

                this.bindData();
            }
            catch { throw; }
        }

        public override void OnBackPressed()
        {

        }

        private void TxtPAQTY_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            try
            {
                string inputTxt = ((EditText)sender).Text.Trim();
                decimal inputVal = 0;
                if(decimal.TryParse(inputTxt,out inputVal))
                {
                    if(inputVal > this.putawayDetail.remainQuantity)
                    {
                        new UIUtils().showAlert(this, "Quantity", "Quantity unit is over");
                        this.txtPAQTY.Text = this.putawayDetail.remainQuantity.ToString("#,##0");
                    }
                }
            }
            catch { throw; }
        }

        private void TxtPAConfLocation_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            try
            {
                this.spnPAReason.Enabled = false;
                if (!string.IsNullOrEmpty(this.txtPAConfLocation.Text))
                {
                    string confLocationValue = this.txtPAConfLocation.Text.Trim();
                    if(confLocationValue!=this.txtPASgLocation.Text)
                    {
                        this.spnPAReason.Enabled = true;
                    }
                }
            }
            catch { throw; }
        }

        private void bindData()
        {
            try
            {
                this.txtWH.Text = "";
                this.txtPAPallet.Text = this.putawayDetail.palletCode;
                this.txtPAproductCode.Text = this.putawayDetail.productCode;
                this.txtPAproductName.Text = this.putawayDetail.productName;
                this.txtPAQTY.Text = this.putawayDetail.Quantity.ToString("#,##0");
                this.txtPAUnits.Text = this.putawayDetail.QuantityUnit;
                this.txtPARemainQTY.Text=this.putawayDetail.remainQuantity.ToString("#,##0");
                this.txtPARemainUnits.Text= this.putawayDetail.QuantityUnit;
                this.txtPALocation.Text = this.putawayDetail.locationCode;
                this.txtPASgLocation.Text = this.putawayDetail.suggLocationCode;
                this.txtPALot.Text = this.putawayDetail.lot;
                this.txtPAConfLocation.Text = this.putawayDetail.confLocationCode;
                

                this.loadReasonItems();
                this.spnPAReason.Enabled = false;

                if (!string.IsNullOrEmpty(this.putawayDetail.palletCode))
                    this.txtPAQTY.Enabled = false;

                this.imgProduct.SetImageResource(Resource.Drawable.Shipping6);
                if (!string.IsNullOrEmpty(this.putawayDetail.productImage))
                {
                    try
                    {
                        var img = new UIUtils().GetImageBitmapFromUrl(this.putawayDetail.productImage);
                        this.imgProduct.SetImageBitmap(img);
                    }
                    catch
                    {

                    }
                }
            }
            catch { throw; }
        }

        private async void loadReasonItems()
        {
            try
            {
                this.reasonItems = await PutAwayClient.GetPutawayReason();

                List<string> reasonList = new List<string>();
                foreach (var val in this.reasonItems)
                    reasonList.Add(val.Description);

                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, reasonList);
                this.spnPAReason.Adapter = adapter;

                this.spnPAReason.ItemSelected += SpnPAReason_ItemSelected;
            }
            catch { throw; }
        }

        private void SpnPAReason_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                this.currentReasonID = "";
                if (this.reasonItems.Count > 0)
                {
                    var spinner = (Spinner)sender;
                    //string SelectedCategory = string.Format("{0}", spinner.GetItemAtPosition(e.Position));

                    this.currentReasonID = this.reasonItems[e.Position].PutAwayReasonID.ToString();
                    //new UIUtils().showAlert(this, "Putaway detail", rsCode);
                }
            }
            catch { throw; }
        }

        private async void PaConfrimBtn_Click(object sender, EventArgs e)
        {
            try
            {
                #region gen model confirm
                PutAwayConfirm confirmValue = new PutAwayConfirm();
                confirmValue.PutAwayID = new Guid(this.putawayDetail.putawayID);
                //confirmValue.PutAway.PalletCode = this.txtPAPallet.Text.Trim();
                confirmValue.Quantity = Convert.ToDecimal(this.txtPAQTY.Text.Trim());
                confirmValue.ConfirmLocation.Code = this.txtPAConfLocation.Text.Trim();
                //confirmValue.PutAway.Lot = this.txtPALot.Text.Trim();
                if(!string.IsNullOrEmpty(this.putawayDetail.baseUnitID))
                    confirmValue.BaseUnitID= new Guid(this.putawayDetail.baseUnitID);

                if (!string.IsNullOrEmpty(this.putawayDetail.stockUnitID))
                    confirmValue.StockUnitID= new Guid(this.putawayDetail.stockUnitID);

                confirmValue.BaseQuantity = this.putawayDetail.baseQuantity;
                confirmValue.ConversionQty = this.putawayDetail.ConversionQty;

                if (!string.IsNullOrEmpty(this.currentReasonID))
                {
                    if(this.spnPAReason.Enabled)
                        confirmValue.PutAwayReasonID = new Guid(this.currentReasonID);
                }

                #endregion

                bool result = await PutAwayClient.ConfirmPutAway(confirmValue);
                if(result)
                {
                    new UIUtils().showAlert(this, "Putaway confirm", "Putaway job is confirm");
                    this.PaBackToListBtn_Click(this.paBackToListBtn, null);
                }
                else
                {
                    new UIUtils().showAlert(this, "Putaway confirm", "Confirm is false!");
                }
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void PaBackToListBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Intent Page = new Intent(this, typeof(PutawayListItemActivity));
                StartActivity(Page);
            }
            catch 
{ throw; }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtPALot.Text = "";
            }
            catch { throw; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PutawayItemDetail);
            this.initControl();
        }
    }
}