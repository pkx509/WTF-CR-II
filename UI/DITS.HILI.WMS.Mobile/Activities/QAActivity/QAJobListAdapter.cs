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
    class QAJobListAdapter : BaseAdapter
    {

        Activity context;

        public QAJobListAdapter(Activity context)
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
                view = context.LayoutInflater.Inflate(Resource.Layout.tempPutawayProduct, null);

            EditText txtQANoItem = view.FindViewById<EditText>(Resource.Id.txtQANoItem);
            txtQANoItem.Text = "";
            txtQANoItem.Enabled = false;

            EditText txtQADate = view.FindViewById<EditText>(Resource.Id.txtQADate);
            txtQADate.Text = "";
            txtQADate.Enabled = false;
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

    class QAJobListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}