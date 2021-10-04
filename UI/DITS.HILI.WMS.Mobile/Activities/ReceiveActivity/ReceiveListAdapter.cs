using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.ReceiveModel;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DITS.HILI.WMS.Mobile.Activities.ReceiveActivity
{
    public class ReceiveListAdapter : BaseAdapter<Receive>
    {
        private List<Receive> works;
        private Activity context;

        public ReceiveListAdapter(Activity _context, List<Receive> _works)
            : base()
        {
            context = _context;
            works = _works;
        }

        public override Receive this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.listReceive, null);


            view.FindViewById<TextView>(Resource.Id.txtRecNo).Text = works[position].ReceiveCode;
            view.FindViewById<TextView>(Resource.Id.txtSup).Text = works[position].Supplier.Name;
            view.FindViewById<TextView>(Resource.Id.txtRecDate).Text = works[position].EstimateDate.ToString("dd/MM/yyyy");
            view.FindViewById<TextView>(Resource.Id.txtLocation).Text = works[position].Location.Code;
             
            return view;
        }

    }
}