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

namespace DITS.HILI.WMS.Mobile.Activities.ReceiveActivity
{

    public class ReceiveItemAdapter : BaseAdapter<ReceiveDetail>
    {
        private List<ReceiveDetail> works;
        private Activity context;

        public ReceiveItemAdapter(Activity _context, List<ReceiveDetail> _works)
            : base()
        {
            context = _context;
            works = _works;
        }
        public override ReceiveDetail this[int position]
        {
            get
            {
                return works[position];
            }
        }

        public override int Count
        {
            get
            {
                return works.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.listReceiveItem, null);

            view.FindViewById<TextView>(Resource.Id.txtProduct).Text = works[position].ProductCode + " " + works[position].Product.Name;
            view.FindViewById<TextView>(Resource.Id.txtQty).Text = works[position].Quantity.ToString("#,###.##");
            view.FindViewById<TextView>(Resource.Id.txtUnit).Text = works[position].ProductUOM.Name;
            view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.Shipping6);

            return view;
        }
    }
}