using DITS.HILI.WMS.DispatchModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace DITS.HILI.WMS.Web.Common.Util
{
    public static class MyToolkit
    {

        #region [ Session ]

        public static string LocalSession(this Page page, string Prefix)
        {
            string subfix = "_SESSIONNAME_";
            try
            {
                string oSessionName = page.Session.SessionID + subfix + Prefix;
                return oSessionName;
            }
            catch (Exception)
            {
                return subfix + Prefix;
            }
        }

        public static string LocalSession(this UserControl usercontrol, string Prefix)
        {
            string subfix = "_SESSIONNAME_";
            try
            {
                string oSessionName = usercontrol.Session.SessionID + subfix + Prefix;
                return oSessionName;
            }
            catch (Exception)
            {
                return subfix + Prefix;
            }
        }

        //public static bool IsNull(this object instance)
        //{
        //    if (instance == null)
        //        return true;
        //    return false;
        //}

        #endregion


    }

    public static class MyCombobox
    {
        public static bool AutoCompleteProxy(this Store StoreAutoComplete, string Url, Dictionary<string, object> param)
        {
            List<ParameterProxy> _param = new List<ParameterProxy>();

            foreach (KeyValuePair<string, object> item in param)
            {
                _param.Add(new ParameterProxy { ParameterName = item.Key, ParameterValue = Convert.ToString(item.Value) });
            }
            return AutoCompleteProxy(StoreAutoComplete, Url, _param);
        }

        public static bool AutoCompleteProxy(this Store StoreAutoComplete, string Url, string[] arrFiled, string[] arrSerarch)
        {
            List<ParameterProxy> ParaEmployee = new List<ParameterProxy>();

            for (int i = 0; i < arrFiled.Length; i++)
            {
                ParaEmployee.Add(new ParameterProxy() { ParameterName = arrFiled[i], ParameterValue = arrSerarch[i] });
            }
            return AutoCompleteProxy(StoreAutoComplete, Url, ParaEmployee);
        }

        public static bool AutoCompleteProxy(this Store StoreAutoComplete, string Url, List<ParameterProxy> ParameterList)
        {
            try
            {
                string oURL = Url + "?";
                foreach (ParameterProxy item in ParameterList)
                {
                    oURL += item.ParameterName + "=" + item.ParameterValue + "&";
                }
                oURL += "guid=" + DateTime.Now.ToString();

                StoreAutoComplete.SetProxyUrl(oURL);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetValue(this ComboBox Ctl)
        {
            try
            {
                return Ctl.SelectedItem.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetText(this ComboBox Ctl)
        {
            try
            {
                string ret = Ctl.SelectedItem.Text;
                if (ret == null) { ret = Ctl.Text; }
                return ret;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static bool SetDefaultValue(this ComboBox Ctl, object text, object value)
        {
            try
            {

                //ถ้ามีค่าอยู่แล้ว ไม่ต้อง add ใหม่
                Ctl.SetValue(value.ToString());
                string strValue = Ctl.GetValue() != null ? Ctl.GetValue() : Ctl.GetText(); // บางที GetValue ไม่มีค่า
                if (strValue != value.ToString() || Ctl.GetValue() == Ctl.GetText() || Ctl.GetValue() == null)
                {
                    Ctl.AddItem(text.ToString(), value.ToString());
                    Ctl.SetValue(value.ToString());
                }
                else
                {
                    Ctl.SelectedItem.Value = value.ToString();
                }

                return true;
            }
            catch (Exception)
            {
                // Ctl.SetValue(value);
                return false;
            }
        }

        public static bool SetAutoCompleteValue(this ComboBox Ctl, object storeObj)
        {
            try
            {
                Store store = Ctl.GetStore();
                store.Data = storeObj;
                store.DataBind();

                Ctl.SelectedItem.Index = 0;
                Ctl.UpdateSelectedItems();


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SetAutoCompleteValue(this ComboBox Ctl, object storeObj, string value)
        {
            try
            {
                Ctl.GetStore().RemoveAll();
                Ctl.Items.Clear();
                Ctl.GetStore().Add(storeObj);
                Ctl.SelectedItem.Value = value;
                Ctl.UpdateSelectedItems();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static void FindDefault(this ComboBox Ctl, string value = "0", string text = "")
        {
            try
            {
                //  ComboBox Ctl = (ComboBox)Ctls;
                if (text == "")
                {
                    //Ctl.EmptyText = Captions.p;
                    Ctl.EmptyText = "ALL";
                }
                else
                {
                    Ctl.EmptyText = text;
                }

                Ctl.SelectedItem.Value = value;

            }
            catch (Exception)
            {

            }
        }

    }

    public struct ParameterProxy
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }

    }

    public struct ComboboxDataSource
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
    }

   
}