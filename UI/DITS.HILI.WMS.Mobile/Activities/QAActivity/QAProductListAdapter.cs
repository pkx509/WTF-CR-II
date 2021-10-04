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

namespace DITS.HILI.WMS.Mobile.Activities.QAActivity
{
    class QAProductListAdapter : BaseAdapter
    {

        Activity context;

        public QAProductListAdapter(Activity context)
        {
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.tempQAProductList, null);

            #region sync ui control
            EditText txtQAProduct = view.FindViewById<EditText>(Resource.Id.txtQAProduct);
            txtQAProduct.Text = "";
            txtQAProduct.Enabled = false;

            EditText txtQAConfirmQTY1 = view.FindViewById<EditText>(Resource.Id.txtQAConfirmQTY1);
            txtQAConfirmQTY1.Text = "";
            txtQAConfirmQTY1.Enabled = false;

            EditText txtQAConfirmQTY2 = view.FindViewById<EditText>(Resource.Id.txtQAConfirmQTY2);
            txtQAConfirmQTY2.Text = "";
            txtQAConfirmQTY2.Enabled = false;

            EditText txtQALocation = view.FindViewById<EditText>(Resource.Id.txtQALocation);
            txtQALocation.Text = "";
            txtQALocation.Enabled = false;

            EditText txtQAStatus = view.FindViewById<EditText>(Resource.Id.txtQAStatus);
            txtQAStatus.Text = "";
            txtQAStatus.Enabled = false;

            #endregion

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return 0;
            }
        }

    }

    class QAProductListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}