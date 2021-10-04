using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.Common.Util
{
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
    }

    public class GridPanelExt
    {
        public static void ClearSelection(GridPanel grid)
        {
            RowSelectionModel sm = grid.GetSelectionModel() as RowSelectionModel;
            if (sm.SelectedRow != null)
            {
                sm.SelectedRows.Remove(sm.SelectedRow);
            }
            sm.UpdateSelection();
        }
    }
}