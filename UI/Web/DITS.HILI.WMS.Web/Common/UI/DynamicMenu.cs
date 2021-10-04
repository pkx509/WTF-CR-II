using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DITS.HILI.WMS.Web.Common.UI
{

    public class MenuModel
    {
        public string Menu_Code { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Parent { get; set; }
        public string Menu_Url { get; set; }
        public string Menu_Icon { get; set; }
        public string Menu_Description { get; set; }
        public int Menu_Level { get; set; }
        public int Menu_Sequnce { get; set; }
        public bool IsNew { get; set; }
    }

    public static class DynamicMenu
    {

        public static async void BulidingTree(Panel onTree)
        {
            try
            {
                List<MenuModel> menuList = new List<MenuModel>();

                List<Program> data = new List<Program>();

                Guid AppID = new Guid(ConfigurationManager.AppSettings["AppID"].ToString());

                Core.Domain.ApiResponseMessage apiResp = ClientService.Master.ProgramClient.Get(AppID).Result;

                if (apiResp.IsSuccess)
                {
                    data = apiResp.Get<List<Program>>();
                }



                string JsHandler = " if (record.data.hrefTarget) {addTab(#{tabBody}, record.data.id, record.data.hrefTarget, record.data.text, record.data.iconCls);};";

                foreach (Program item in data.OrderBy(c => c.Sequence).Where(w => w.ProgramType == ProgramType.Module))
                {
                    List<Program> pagelist = data.OrderBy(o => o.Sequence).Where(w => w.ParentID == item.ProgramID).ToList();

                    if (pagelist.Count() > 0)
                    {
                        TreePanel t1 = new TreePanel();
                        NodeCollection group = new NodeCollection();

                        t1.ID = "root_" + item.Code;
                        t1.Title = item.Description;
                        Icon onIcon = (string.IsNullOrEmpty(item.Icon) ? Icon.Brick : (Icon)Enum.Parse(typeof(Icon), item.Icon));
                        t1.Listeners.ItemClick.Handler = JsHandler;
                        t1.Icon = onIcon;
                        t1.RootVisible = false;

                        Node root = new Node
                        {
                            Text = item.Description,
                            NodeID = "root_" + item.Code,
                            Leaf = false,
                            Expanded = true
                        };

                        t1.Root.Add(root);
                        int tempChildren = 0;

                        foreach (Program menu in pagelist)
                        {

                            Node n1 = new Node
                            {
                                Text = menu.Url == string.Empty ? menu.Description + "*" : menu.Description,
                                NodeID = menu.Code,
                                Leaf = true,
                                HrefTarget = menu.Url
                            };
                            Icon onIcon2 = (string.IsNullOrEmpty(menu.Icon) ? Icon.Brick : (Icon)Enum.Parse(typeof(Icon), menu.Icon));
                            n1.Icon = onIcon2;

                            root.Children.Add(n1);
                            tempChildren++;
                        }
                        if (tempChildren > 0)
                        {
                            onTree.Items.Add(t1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
    }
}