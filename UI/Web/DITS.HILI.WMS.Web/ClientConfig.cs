using DITS.HILI.WMS.MasterModel.Core;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace DITS.HILI.WMS.Web
{

    public struct ParameterProxy
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }

    }
    public static class Utility
    {
        public static string AutoCompleteAppsService => "~/AppsHandlerService.ashx";
        public static string AutoCompleteMasterService => "~/MasterHandlerService.ashx";

        public static bool AutoCompleteProxy(this Store StoreAutoComplete, string Url, List<ParameterProxy> ParameterList)
        {
            try
            {
                string oURL = Url + "?";
                foreach (ParameterProxy item in ParameterList)
                {
                    oURL += item.ParameterName + "=" + item.ParameterValue + "&";
                }
                StoreAutoComplete.SetProxyUrl(oURL);
                StoreAutoComplete.LoadProxy();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        public static bool AutoCompleteProxy(this Store StoreAutoComplete, string Url, Dictionary<string, object> param)
        {
            List<ParameterProxy> paramProxy = new List<ParameterProxy>();

            foreach (KeyValuePair<string, object> item in param)
            {
                paramProxy.Add(new ParameterProxy() { ParameterName = item.Key, ParameterValue = Convert.ToString(item.Value) });
            }

            return AutoCompleteProxy(StoreAutoComplete, Url, paramProxy);
        }
    }

    public class WindowShow
    {
        public static void Show(Page page, string title, string id, string url, Icon icon, int width, int height)
        {
            //X.Js.Call("parent.openMasterEditWindow", id, url, title, "icon-" + icon.ToString().ToLower(), width, height);
            //var a = title.Substring(0, 5);

            Window win = new Window
            {   //    ID = id,
                Title = title,
                Width = width,
                Height = height,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Draggable = false,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg = "Please wait...",
                        ShowMask = true
                    }
                }
            };

            win.Render(page.Form);

        }

        public static void ShowNewPage(Page page, string title, string id, string url, Icon icon)
        {
            Window win = new Window
            {   //    ID = id,
                ID = id,
                Title = title,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Maximized = true,
                Minimizable = false,
                Width = 300,
                Height = 300,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg = "Loading...",
                        ShowMask = true
                    }
                },
            };


            win.Render(page.Form);
        }

        public static void ShowNewPage(Page page, int w, int h, string title, string id, string url, Icon icon)
        {
            Window win = new Window
            {   //    ID = id,
                ID = id,
                Title = title,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Maximized = false,
                Minimizable = false,
                X = w,
                Y = h,
                Width = w,
                Height = h,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg = "Loading...",
                        ShowMask = true
                    }
                },
            };


            win.Render(page.Form);
        }

        public static void showOperation(Page page, string title, string id, string url, Icon icon)
        {
            //X.Js.Call("parent.openMasterEditWindow", id, url, title, "icon-" + icon.ToString().ToLower(), width, height);

            Window win = new Window
            {   //    ID = id,
                Title = title,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Maximized = true,
                Minimizable = false,
                Width = 300,
                Height = 300,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg =  "Loading...",
                        ShowMask = true
                    }
                }
            };

            win.Render(page.Form);

        }


    }
    public class WindowsPopup
    {
        public static void show(Page page, string title, string id, string url, Icon icon, int width, int height)
        {
            //X.Js.Call("parent.openMasterEditWindow", id, url, title, "icon-" + icon.ToString().ToLower(), width, height);
            string a = title.Substring(0, 5);

            Window win = new Window
            {   //    ID = id,
                Title = title,
                Width = width,
                Height = height,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Draggable = false,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg =  "Please wait...",
                        ShowMask = true
                    }
                }
            };

            win.Render(page.Form);

        }

        public static void showOperation(Page page, string title, string id, string url, Icon icon)
        {
            //X.Js.Call("parent.openMasterEditWindow", id, url, title, "icon-" + icon.ToString().ToLower(), width, height);

            Window win = new Window
            {   //    ID = id,
                Title = title,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Maximized = true,
                Minimizable = false,
                Width = 300,
                Height = 300,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg =  "Please wait...",
                        ShowMask = true
                    }
                }
            };

            win.Render(page.Form);

        }

        public static void showOperationNew(Page page, string title, string id, string url, Icon icon, string fn = "")
        {
            if (!string.IsNullOrWhiteSpace(fn))
            {
                fn = "function () { " + fn + "}";
            }

            Window win = new Window
            {   //    ID = id,
                Title = title,
                Modal = true,
                Icon = icon,
                Resizable = false,
                Maximized = true,
                Minimizable = false,
                Width = 300,
                Height = 300,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = url,
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        Msg =  "Please wait...",
                        ShowMask = true
                    }
                },
                Listeners = { Destroy = { Fn = fn } }
            };


            win.Render(page.Form);
        }

    }

    public static class Captions
    {
        public static string SavedCaptionText = "Saved";
        public static string CompletedMessageText = "Completed";
        public static string WarningCaptionText = "Warning";
        public static string DeletedCaptionText = "Deleted";

    }

    public class NotificationExt
    {
        public static void Show(string Title = "Save",
                                string Html = "Save success.",
                                int HideDelay = 2000,
                                Ext.Net.Icon icon = Icon.PageSave)
        {
            Notification.Show(new NotificationConfig()
            {
                Title = Title,
                Html = "<br>" + Html,
                Icon = icon,
                HideDelay = HideDelay,
                //Header=false,
                //AlignCfg = new NotificationAlignConfig()
                //{
                //    ElementAnchor = AnchorPoint.BottomRight,
                //    TargetAnchor = AnchorPoint.BottomRight
                //},
                //ShowFx = new FadeIn() { Options = new FxConfig() { Duration = 3 } },
                //HideFx = new FadeOut() { Options = new FadeOutConfig() { Duration = 3, EndOpacity = 0.25F } },

            });
        }

    }

    public class MessageBoxExt
    {
        public static bool Save(
                              string Message = "", string callback = "",
                              Icon HeaderIcon = Icon.DatabaseSave,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {

            string Title = Captions.SavedCaptionText;
            MessageBox.Button buttons = MessageBox.Button.OK;
            if (Message == "") { Message = Captions.CompletedMessageText; }

            MessageBoxConfig msg = new MessageBoxConfig
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                Modal = true
            };

            X.MessageBox.Show(msg);
            return true;
        }

        public static bool Warning(
                              string Message = "Warning", string callback = "",
                              Icon HeaderIcon = Icon.Information,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {

            string Title = Captions.WarningCaptionText;
            MessageBox.Button buttons = MessageBox.Button.OK;

            MessageBoxConfig msg = new MessageBoxConfig
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                // msg.Wait = true;
                // msg.Closable = false;
                // msg.Handler = callback+"()";
                Modal = true
            };
            // msg.AnimEl = "xxx";

            X.MessageBox.Show(msg);
            return true;
        }

        public static bool Delete(
                              string Message = "Delete", string callback = "",
                              Icon HeaderIcon = Icon.Delete,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {

            string Title = Captions.DeletedCaptionText;
            MessageBox.Button buttons = MessageBox.Button.OK;

            MessageBoxConfig msg = new MessageBoxConfig
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                Modal = true
            };

            X.MessageBox.Show(msg);
            return true;
        }

        private static bool Confirm(
                              string Message = "Warning",
                              Icon HeaderIcon = Icon.Information,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {

            string Title = Captions.WarningCaptionText;
            MessageBox.Button buttons = MessageBox.Button.OK;

            MessageBoxConfig msg = new MessageBoxConfig
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                Modal = true,
                AnimEl = "Warning"
            };

            X.MessageBox.Show(msg);
            return true;
        }


        public static void Show(string Title = "Information",
                               string Message = "Save Complete",
                               MessageBox.Button buttons = MessageBox.Button.OK,
                               Icon HeaderIcon = Icon.DatabaseSave,
                               MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {
            if (Title == "")
            {
                Title = "Information";
            }

            X.MessageBox.Show(new MessageBoxConfig()
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon
            }
            );
        }

        public static void ShowError(Exception ex,
                              MessageBox.Button buttons = MessageBox.Button.OK,
                              Icon HeaderIcon = Icon.Error,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.WARNING)
        {

            string msg = "";
            string _msg;
            if (ex.InnerException == null)
            {
                _msg = ex.Message;
            }
            else
            {
                _msg = ex.InnerException.Message;
            }

            if (_msg.Contains("Input string was not in a correct format"))
            {
                msg = "Format file is not correct";
            }
            else if (_msg.Contains("timeout period has expired"))
            {
                msg = "Timeout period has expired";
            }
            else
            {
                msg = _msg;
            }

            X.MessageBox.Show(new MessageBoxConfig()
            {
                Title = Captions.WarningCaptionText,
                Message = msg,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon
            }
            );
        }

        public static void Shows(string Title = "Save",
                                string Message = "Save Success.",
                                MessageBox.Button buttons = MessageBox.Button.OK,
                                Icon HeaderIcon = Icon.DatabaseSave,
                                MessageBox.Icon MsgIcon = MessageBox.Icon.INFO)
        {
            X.MessageBox.Show(new MessageBoxConfig()
            {
                Title = Title,
                Message = Message,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon
            }
            );
        }

        public static void ShowError(string msg,
                              MessageBox.Button buttons = MessageBox.Button.OK,
                              Icon HeaderIcon = Icon.Error,
                              MessageBox.Icon MsgIcon = MessageBox.Icon.WARNING, string fnCallback = "")
        {

            X.MessageBox.Show(new MessageBoxConfig()
            {
                Title = "Warning",
                Message = msg,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                Handler = fnCallback,
            }
           );
        }

        internal static void ShowError(CustomMessage customMessage,
                                MessageBox.Button buttons = MessageBox.Button.OK,
                                Icon HeaderIcon = Icon.Error,
                                MessageBox.Icon MsgIcon = MessageBox.Icon.WARNING, string fnCallback = "")
        {
            X.MessageBox.Show(new MessageBoxConfig()
            {
                Title = "Warning",
                Message = customMessage.MessageValue,
                Buttons = buttons,
                HeaderIcon = HeaderIcon,
                Icon = MsgIcon,
                Handler = fnCallback,
            }
          );
        }
    }

}