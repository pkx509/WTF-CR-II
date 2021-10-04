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
    [Activity(Label = "QA Job list")]
    public class QAJobListActivity : Activity,Ihili42
    {
        #region members
        private EditText txtFind;
        private Button btnQAback;
        private Button btnQAFind;
        private Button btnQAFinish;
        private ListView lvQAList;

        private bool isJobQAList;
        #endregion

        public override void OnBackPressed() 
{

        }

        private async void prepareQAJobList()
        {
            try
            {
                this.Title = "QA Job list";
                this.btnQAFinish.Visibility = ViewStates.Invisible;
            }
            catch { throw; }
        }

        private async void prepareQAProductList()
        {
            try
            {
                this.Title = "QA Product list";
                this.isJobQAList = false;
            }
            catch { throw; }
        }

        public void initControl()
        {
            try
            {
                this.txtFind = FindViewById<EditText>(Resource.Id.txtQAFind);
                this.btnQAback = FindViewById<Button>(Resource.Id.btnQABack);
                this.btnQAFind = FindViewById<Button>(Resource.Id.btnQAFind);
                this.btnQAFinish = FindViewById<Button>(Resource.Id.btnQAFinish);
                this.lvQAList = FindViewById<ListView>(Resource.Id.lvQAJoblist);

                this.btnQAback.Click += BtnQAback_Click;
                this.btnQAFind.Click += BtnQAFind_Click;
                this.lvQAList.ItemSelected += LvQAList_ItemSelected;
            }
            catch { throw; }
        }

        private void LvQAList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                if (this.isJobQAList)
                {
                    this.prepareQAProductList();
                }
                else
                {
                    
                }
            }
            catch { throw; }
        }

        private void BtnQAFind_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch { throw; }
        }

        private void BtnQAback_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.isJobQAList)
                    new LocalLib().mainPage(this);
                else
                {
                    this.prepareQAJobList();
                }
            }
            catch { throw; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.QAJobListItem);

            this.isJobQAList = Intent.GetBooleanExtra("QAJobList",true);

            this.initControl();
            this.prepareQAJobList();
        }
    }
}