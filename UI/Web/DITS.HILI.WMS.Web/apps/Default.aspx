<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../resources/css/main.css" rel="stylesheet" />
    <link href="../Css/wms.css" rel="stylesheet" />
    <link rel="shortcut icon" href="favicon.ico" />  
    <script src="../resources/js/main.js"></script>
    <script src="../Scripts/ext-locale-th.js"></script>
    <script src="../Scripts/jquery-3.1.1.min.js"></script>
    <script src="../resources/js/wms.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
    <style type="text/css">
        span.menuNew {
            margin-left: 8px;
            padding-left: 25px;
            background: transparent url(resources/images/new.gif) no-repeat 0px 4px;
        }

        .api-title {
            font-size: 16px;
            font-weight: bold;
            padding: 5px 5px;
            color: black;
        } 

        /*div#TbarMenu-innerCt * {
            color: #ffffff !important;
        }

        div#TbarMenu-innerCt a.x-over * {
            color: green !important;
        }

        div#TbarMenu-innerCt {
            background-color: #323439;
        }

        #TbarMenu-targetEl #HILILOGO {
            top: 0px !important;
        }*/

        label#txtCountdown {
            font-size: 60px;
            color: red;
        }

        label#txtSecounds {
            font-size: 20px;
        }

        .x-status-text {
            color: red;
        }
    </style>

  <%--  <script>

        function addTab(tabPanel, id, url, title) {
            if (title == "") {
                return;
            }
            var tab = tabPanel.getComponent(id);
            if (!tab) {
                tab = tabPanel.add({
                    id: id,
                    title: title,
                    closable: true,
                    /*iconCls: icon || 'icon-applicationdouble',*/
                    loader: {
                        url: url,
                        renderer: "frame",
                        loadMask: { showMask: true, msg: "Loading..." }
                    }
                });
            }
            tabPanel.setActiveTab(tab);
        };

    </script>--%>

    <ext:XScript runat="server">
        <script>
            function ChangeLang(lang) {
                App.direct.ChangeLangauge(lang);
            }
            function addTabClient(id, url, title, icon) {
                addTab(#{tabBody}, id, url, title, icon);
            };

            function addTab(tabPanel, id, url, title, icon) {

                var tab = tabPanel.getComponent(id);
                if (!tab) {
                    tab = tabPanel.add({
                        id: id,
                        title: id + ' : ' + title,
                        closable: true,
                        iconCls: icon || 'icon-pagewhiteoffice',
                        loader: {
                            url: url,
                            renderer: "frame",
                            loadMask: { showMask: true, msg: "Loading ..." }
                        }
                    });
                }

                Ext.History.add(title);
                tabPanel.setActiveTab(tab);

            };
        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CompanyX" />

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Panel ID="Panel1" runat="server" Region="North" Header="false" Border="false">
                    <TopBar>
                        <ext:Toolbar ID="TbarMenu" Height="45" runat="server">
                            <Items>
                                <ext:ToolbarFill />

                                <ext:Button ID="MenuButton"  runat="server" Text=""   Icon="StatusOnline">
                                    <Menu>
                                        <ext:Menu runat="server">
                                            <Items>
                                                <ext:MenuItem runat="server" Text="User Profile" Icon="User" /> 
                                                <ext:MenuSeparator />
                                                    <ext:MenuItem runat="server" Text="Logout" Icon="UserGo">
                                                        <DirectEvents>
                                                        <Click OnEvent="btnLogout_Click">
                                             
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:MenuItem>
                                            </Items>
                                        </ext:Menu> 
                                    </Menu>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                  <ext:Panel ID="panelMainMenu" runat="server" Width="220" Region="West" Layout="Accordion" 
                    Split="true" Margins="5 0 5 5" Border="true"
                    Title="Dynamic HILI 4.2"
                    Collapsible="true" Collapsed="false">
                 
                    <Items>
                    </Items>
                </ext:Panel>

                <ext:TabPanel
                    ID="tabBody"
                    runat="server"
                    Region="Center"
                    Margins="0 4 4 0"
                    Cls="tabs"
                    MinTabWidth="50">
                    <Items>
                        <ext:Panel
                            ID="tabHome"
                            runat="server"
                            Title="Task Board"
                            HideMode="Offsets"
                            Icon="House">
                            <Loader runat="server" Mode="Frame" Url="home.aspx">
                                <LoadMask ShowMask="true" Msg="Loading..." />
                            </Loader>
                        </ext:Panel>

                    </Items>
                    <Plugins>
                        <ext:TabCloseMenu runat="server" />
                    </Plugins>
                </ext:TabPanel>

 

                <ext:Panel ID="Panel2" runat="server" Region="South" Header="false">
                    <BottomBar>
                        <ext:StatusBar ID="StatusBarMain" CtCls="word-status" runat="server" DefaultText="Ready">
                            <Items>
                                <ext:ToolbarTextItem Text="Copyright ©2016" /> 
                                <ext:ToolbarTextItem Text="all rights reserved. Terms and Conditions" />
                                <ext:ToolbarFill /> 
                                <ext:Button runat="server" ID="LangButton">
                                    <Menu>
                                        <ext:Menu ID="langMenu" runat="server" TagString="file"> 
                                        </ext:Menu>
                                    </Menu>
                                </ext:Button>
                                 
                            </Items>
                        </ext:StatusBar>
                    </BottomBar>
                </ext:Panel>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
