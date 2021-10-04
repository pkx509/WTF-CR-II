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
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.HttpClientService;
using DITS.HILI.WMS.MobileService.Inbound;

namespace DITS.HILI.WMS.Mobile.Activities.ReceiveActivity
{
    [Activity(Label = "Receive list")]
    public class ReceiveItemActivity : Activity
    {

        Receive receive = null;
        ListView listViewReceive = null;
        Button btnFinish = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.ReceiveItem);
            listViewReceive = FindViewById<ListView>(Resource.Id.listViewReceive);
            listViewReceive.ItemClick += ListViewReceive_ItemClick;
            btnFinish = FindViewById<Button>(Resource.Id.btnFinish);
            btnFinish.Click += BtnFinish_Click;
            string receiveCode = Intent.GetStringExtra("ReceiveCode");
            getData(receiveCode);
        }

        private async  void BtnFinish_Click(object sender, EventArgs e)
        {
            
            var ok =await  ReceiveClient.Finish(receive.ReceiveID);
            if (ok)
                Android.Widget.Toast.MakeText(this, "Normal complete", ToastLength.Long);
        }

        private void ListViewReceive_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        { 
            var item = receive.ReceiveDetailCollection.ToList()[e.Position];
            goReceiving(item.ReceiveDetailID);
        }

        private async void getData(string receiveCode)
        {
            try
            {
                receive = await ReceiveClient.GetReceive(receiveCode);
                listViewReceive.Adapter = new ReceiveItemAdapter(this, receive.ReceiveDetailCollection.ToList());
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Long);
            }
        }

        private void goReceiving(Guid receiveDetailID)
        {
            var intent = new Intent(this, typeof(ReceiveItemConfirmActivity));
            intent.PutExtra("ReceiveDetailID", receiveDetailID.ToString());

            StartActivity(intent);
        }
    }
}