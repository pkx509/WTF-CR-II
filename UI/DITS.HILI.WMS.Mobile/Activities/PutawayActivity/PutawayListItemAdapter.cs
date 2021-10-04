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

namespace DITS.HILI.WMS.Mobile.Activities.PutawayActivity
{
    class PutawayListItemAdapter : BaseAdapter<PutAway>
    {
        private List<PutAway> works;
        private Activity context;

        public PutawayListItemAdapter(Activity _context, List<PutAway> _works) : base()
        {
            context = _context;
            works = _works;
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

            PutAway viewValues = works[position];

            view.FindViewById<ImageView>(Resource.Id.imgProductTmp).SetImageResource(Resource.Drawable.Shipping6);
            if (!string.IsNullOrEmpty(viewValues.Product.URLImage))
            {
                try
                {
                    var img = new UIUtils().GetImageBitmapFromUrl(viewValues.Product.URLImage);
                    view.FindViewById<ImageView>(Resource.Id.imgProductTmp).SetImageBitmap(img);
                }
                catch
                {

                }
            }

            view.FindViewById<TextView>(Resource.Id.txvProductTmp).Text = viewValues.ProductCode+"/"+ viewValues.ProductName;
            view.FindViewById<TextView>(Resource.Id.txvLocationFromTmp).Text = viewValues.FromLocation.Code;
            view.FindViewById<TextView>(Resource.Id.txvPalletTmp).Text = viewValues.PalletCode;

            decimal remQTYNet = viewValues.Quantity - viewValues.RemainQuantity;
            view.FindViewById<TextView>(Resource.Id.txvUnitRemTmp).Text = remQTYNet.ToString("#,##0");
            view.FindViewById<TextView>(Resource.Id.txvUnitTmp).Text = "/" + viewValues.Quantity.ToString("#,##0");

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return works.Count;
            }
        }

        public override PutAway this[int position]
        {
            get
            {
                return works[position];
            }
        }
    }

    class Adapter1ViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}