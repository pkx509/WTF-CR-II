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
using Android.Graphics;
using System.Net;

namespace DITS.HILI.WMS.Mobile.Activities
{
    public class UIUtils
    {
        public Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public bool showAlert(Activity parent,string title,string message)
        {
            try
            {
                bool Res = false;
                Android.App.AlertDialog.Builder dlg = new AlertDialog.Builder(parent);
                AlertDialog altDlg = dlg.Create();
                altDlg.SetTitle(title);
                altDlg.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                altDlg.SetMessage(message);
                altDlg.SetButton("OK",(s,ev)=> {
                    Res = true;
                });

                altDlg.Show();

                return Res;
            }
            catch { throw; }
        }
    }
}