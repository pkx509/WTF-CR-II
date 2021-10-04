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
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;

namespace DITS.HILI.WMS.Mobile.Activities.Common
{

    public class LoadingAdapter : BaseAdapter<Location>
    {

        private List<Location> works;
        private LayoutInflater context;

        public LoadingAdapter(Activity _context, List<Location> _works)
            : base()
        {
            context = LayoutInflater.FromContext(_context);
            works = _works;
        }

        public override Location this[int position]
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
                view = context.Inflate(Android.Resource.Layout.SimpleDropDownItem1Line, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = works[position].Code;

            return view;
        }
    }
}