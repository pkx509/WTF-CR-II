<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImportPreDispatch.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.dispatch.frmImportPreDispatch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script src="../../resources/locale/ext-lang-th.js"></script>
    <script type="text/javascript">
        //Ext.Ajax.timeout = 180000; // 1 sec
        //Ext.net.DirectEvent.timeout = 180000; // 1 sec 
        var template = 'color:{0}; background-color: yellow;';

        var change = function (value, meta, record) {
            //var max
            var store = Ext.data.StoreManager.get("StoreError").data;
            for (i = 0; i < store.length; i++) {
                if (meta.columnIndex == store.items[i].data.ColumnIndex - 1 && meta.recordIndex == store.items[i].data.RowIndex) {
                    meta.style = Ext.String.format(template, "red");
                }
            }


            return value;
        };


        var changeDate = function (value, meta, record) {
            //var max
            var store = Ext.data.StoreManager.get("StoreError").data;
            for (i = 0; i < store.length; i++) {
                if (meta.columnIndex == store.items[i].data.ColumnIndex - 1 && meta.recordIndex == store.items[i].data.RowIndex) {
                    meta.style = Ext.String.format(template, "red");
                }

            }
            return Ext.Date.format(value, 'd/m/Y');
        };


        var onBeforeShow = function (toolTip, grid) {
            var view = grid.getView(),
                store = grid.getStore(),
                record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
                column = view.getHeaderByCell(toolTip.triggerElement),
                data = "";
            var store = Ext.data.StoreManager.get("StoreError").data;
            for (i = 0; i < store.length; i++) {
                if (column.dataIndex == 'F' + (store.items[i].data.ColumnIndex + 1) && record.index == store.items[i].data.RowIndex) {
                    data = store.items[i].data.Message;
                }
            }
            if (data == "") {
                return false;
            }
            else {
                toolTip.update(data);
            }
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>

        <ext:Store ID="StoreError" runat="server">
            <Model>
                <ext:Model ID="Model2" runat="server">
                    <Fields>
                        <ext:ModelField Name="RowIndex" />
                        <ext:ModelField Name="ColumnIndex" />
                        <ext:ModelField Name="Message" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Panel ID="Panel9" runat="server" Layout="Fit" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill />
                                <ext:FileUploadField
                                    ID="FileUploadField1"
                                    runat="server"
                                    EmptyText="Select File"
                                    Icon="Attach"
                                    Width="300">
                                    <DirectEvents>
                                        <Change OnEvent="OnImport" />
                                    </DirectEvents>
                                </ext:FileUploadField>
                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="<%$ Resource : SAVE %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdImport}.getRowsValues())" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true"
                                                Message='<%$ Message :  MSG00025 %>' Title='<%$ MessageTitle :  MSG00025 %>' />
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnReset" runat="server" Icon="Reload" Text="<%$ Resource : RESET %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnReset_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnDownloadTemplete" runat="server" Icon="PageWhitePut" Text="<%$ Resource : TEMPLETE %>">
                                    <Listeners>
                                        <Click Handler="window.location.href =  '../../downloads/PreDispatchTemplete.xlsx';" />
                                    </Listeners>
                                </ext:Button>

                                <ext:Button ID="btnDownload" runat="server" Text="<%$ Resource : ERROR %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnDownload_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="grdImport"
                            runat="server"
                            Frame="true"
                            Layout="FitLayout"
                            Region="Center">
                            <Store>
                                <ext:Store
                                    ID="StoreImport"
                                    runat="server">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="F1" />
                                                <ext:ModelField Name="F2" />
                                                <ext:ModelField Name="F3" />
                                                <ext:ModelField Name="F4" Type="Date" />
                                                <ext:ModelField Name="F5" Type="Date" />
                                                <ext:ModelField Name="F6" />
                                                <ext:ModelField Name="F7" Type="Date" />
                                                <ext:ModelField Name="F8" />
                                                <ext:ModelField Name="F9" />
                                                <ext:ModelField Name="F10" />
                                                <ext:ModelField Name="F11" />
                                                <ext:ModelField Name="F12" />
                                                <ext:ModelField Name="F13" />
                                                <ext:ModelField Name="F14" />
                                                <ext:ModelField Name="F15" />
                                                <ext:ModelField Name="F16" />
                                                <ext:ModelField Name="F17" />
                                                <ext:ModelField Name="F18" />
                                                <ext:ModelField Name="F19" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ID="col1" runat="server" DataIndex="F1" Text="<%$ Resource : DOCUMENT_NO %>" Width="80" Visible="false">
                                    </ext:Column>
                                    <ext:Column ID="col2" runat="server" DataIndex="F2" Text="<%$ Resource : CUSTOMER %>" Width="100">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col3" runat="server" DataIndex="F3" Text="<%$ Resource : DISPATCH_TYPE %>"  Width="85" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:DateColumn ID="col4" runat="server" DataIndex="F4" Text="<%$ Resource : ESTDISPATCH_DATE %>" Width="100" Format="dd/MM/yyyy">
                                        <Renderer Fn="changeDate" />
                                    </ext:DateColumn>
                                    <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="F5" Text="<%$ Resource : DOCUMENT_DATE %>" Width="100" Format="dd/MM/yyyy">
                                        <Renderer Fn="changeDate" />
                                    </ext:DateColumn>
                                    <ext:Column ID="col5" runat="server" DataIndex="F6" Text="<%$ Resource : SHIPPING_TO %>"  Width="85" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="F7" Text="<%$ Resource : DELIVERY_DATE %>" Width="100" Format="dd/MM/yyyy">
                                        <Renderer Fn="changeDate" />
                                    </ext:DateColumn>
                                    <ext:Column ID="col7" runat="server" DataIndex="F8" Text="<%$ Resource : PONO %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="Column8" runat="server" DataIndex="F9" Text="<%$ Resource : ORDER_NO %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="Column2" runat="server" DataIndex="F10" Text="<%$ Resource : URGENT_Y_N %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col9" runat="server" DataIndex="F12" Text="<%$ Resource : BACKORDER_Y_N %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col11" runat="server" DataIndex="F11" Text="<%$ Resource : REMARK %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col12" runat="server" DataIndex="F13" Text="<%$ Resource : PRODUCT_CODE %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col18" runat="server" DataIndex="F14" Text="<%$ Resource : QUANTITY %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="col19" runat="server" DataIndex="F15" Text="<%$ Resource : UNIT %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="Column3" runat="server" DataIndex="F16" Text="<%$ Resource : PRICE %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="Column4" runat="server" DataIndex="F17" Text="<%$ Resource : UNIT_PRICE %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column ID="Column5" runat="server" DataIndex="F18" Text="<%$ Resource : REMARK %>" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <BottomBar>
                                <ext:StatusBar ID="statusbar1" runat="server" StatusAlign="Right">
                                    <Items>
                                        <ext:Label runat="server" ID="txtTotal" Text="Total : 0 Record  / Error : 0 Record" />
                                    </Items>
                                </ext:StatusBar>
                            </BottomBar>
                        </ext:GridPanel>
                        <ext:ToolTip ID="ToolTip1"
                            runat="server"
                            Target="={#{grdImport}.getView().el}"
                            Delegate=".x-grid-cell"
                            TrackMouse="true"  Height="40">
                            <Listeners>
                                <BeforeShow Handler="return onBeforeShow(this,#{grdImport});" />
                            </Listeners>
                        </ext:ToolTip>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <%--<Tasks>
                <ext:Task
                    AutoRun="false"
                    TaskID="servertime"
                    Interval="<%$Resources:Langauge, TimmerRefreshList%>">
                    <Listeners>
                        <Update Handler="#{PagingToolbar1}.moveFirst();" />
                    </Listeners>
                </ext:Task>
            </Tasks>--%>
        </ext:TaskManager>
    </form>
</body>
</html>
