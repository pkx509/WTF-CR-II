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
using DITS.HILI.WMS.Mobile.Helper;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MobileService.Inbound;

namespace DITS.HILI.WMS.Mobile.Activities.ReceiveActivity
{
    [Activity(Label = "ReceiveActivity")]
    public class ReceiveActivity : Activity
    {
        List<Receive> ReceiveListModel = null;
        ListView listViewReceive = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Receive);

            listViewReceive = FindViewById<ListView>(Resource.Id.listReceive);

            getReceive();
            listViewReceive.ItemClick += ListViewReceive_ItemClick;
        }

        private void ListViewReceive_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        { 
            var item = ReceiveListModel[e.Position];
            goDetail(item.ReceiveCode);
        }

        private async void getReceive()
        {
            try
            {
                Ref<int> total = new Ref<int>();
                ReceiveListModel = await ReceiveClient.GetReceiveList(null, "", total, null, null);
                listViewReceive.Adapter = new ReceiveListAdapter(this, ReceiveListModel);

            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Long);
            }
        }

        private void goDetail(string receiveCode)
        {

            var intent = new Intent(this, typeof(ReceiveItemActivity));
            intent.PutExtra("ReceiveCode", receiveCode);
            StartActivity(intent);
        }

    }
}