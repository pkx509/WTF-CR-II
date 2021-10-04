using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DITS.HILI.WMS.Mobile.Activities.ReceiveActivity;
using DITS.HILI.WMS.Mobile.Helper;
using DITS.HILI.WMS.Mobile.Activities.PutawayActivity;
using DITS.HILI.WMS.Mobile.Activities.QAActivity;
using DITS.HILI.WMS.Mobile.Activities;

namespace DITS.HILI.WMS.Mobile
{
    [Activity(Label = "HILI Lego", MainLauncher = true, Icon = "@drawable/fork3")]
    public class MainActivity : Activity, Ihili42
    {
        ImageButton receiveBtn;
        ImageButton putAwayBtn;
        ImageButton qaBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it 
            this.initControl();

            //set base values
            HttpClientService.HttpClientServiceHelper.BaseUrl = Shared.Service;
        }

        public override void OnBackPressed()
        {
            //var disable = (bool)App.Current.Properties["isBackButtonDisabled"];
            //if (disable) return;
            //base.OnBackPressed();
        }

        private void menuActionClick(object sender, EventArgs e)
        {
            try
            {
                Intent Page = null;

                #region click button validate
                if(sender is ImageButton)
                {
                    if (sender == this.receiveBtn)
                        Page = new Intent(this, typeof(ReceiveActivity));
                    else if (sender == this.putAwayBtn)
                        Page = new Intent(this, typeof(PutawayListItemActivity));
                    else if (sender == this.qaBtn)
                    {
                        Page = new Intent(this, typeof(QAJobListActivity));
                        Page.PutExtra("QAJobList", true);
                    }
                }
                #endregion
                
                StartActivity(Page);
            }
            catch { throw; }
        }

        public void initControl()
        {
            try
            {
                this.receiveBtn = FindViewById<ImageButton>(Resource.Id.btnReceive);
                this.receiveBtn.Click += menuActionClick;

                this.putAwayBtn = FindViewById<ImageButton>(Resource.Id.btnPutway);
                this.putAwayBtn.Click += menuActionClick;

                this.qaBtn = FindViewById<ImageButton>(Resource.Id.btnQA);
                this.qaBtn.Click += menuActionClick;
            }
            catch { throw; }
        }
    }
}

