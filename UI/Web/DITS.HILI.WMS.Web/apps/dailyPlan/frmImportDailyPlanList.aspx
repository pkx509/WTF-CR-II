<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImportDailyPlanList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.dailyPlan.frmImportDailyPlanList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        //Ext.Ajax.timeout = 180000; // 1 sec
        //Ext.net.DirectEvent.timeout = 180000; // 1 sec 
        var template = 'color:{0}; background-color: yellow;';

        var change = function (value, meta, record) {
            var store = Ext.data.StoreManager.get("StoreError").data;
            for (i = 0; i < store.length; i++) {
                if (meta.recordIndex == store.items[i].data.RowIndex) {
                    if (meta.column.dataIndex == store.items[i].data.Field) {
                        meta.style = Ext.String.format(template, "red");
                        meta.tdAttr = 'data-qtip="' + store.items[i].data.Message + '"';
                    }
                }
            }
            return value;
        };


        var changeDate = function (value, meta, record) {
            //var max
            var store = Ext.data.StoreManager.get("StoreError").data;
            for (i = 0; i < store.length; i++) {
                if (meta.columnIndex == store.items[i].data.ColumnIndex - 1 && meta.recordIndex == store.items[i].data.RowIndex) {
                    //meta.style = Ext.String.format(template, "red");
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Store ID="StoreError" runat="server">
            <Model>
                <ext:Model ID="Model2" runat="server">
                    <Fields>
                        <ext:ModelField Name="RowIndex" />
                        <ext:ModelField Name="ColumnIndex" />
                        <ext:ModelField Name="Message" />
                        <ext:ModelField Name="Field" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="grdImport" runat="server" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>

                                <ext:ToolbarFill runat="server" />

                                <ext:FileUploadField
                                    ID="FileUploadField1"
                                    runat="server"
                                    EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                    Icon="Attach"
                                    Width="300"
                                    ButtonText='<%$ Resource : BROWSE %>'>
                                    <DirectEvents>
                                        <Change OnEvent="ImportExcelFile">
                                            <EventMask ShowMask="true" Msg="Loading..." MinDelay="100" />
                                        </Change>
                                    </DirectEvents>
                                </ext:FileUploadField>
                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text='<%$ Resource : SAVE %>' Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdImport}.getRowsValues())" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                            <Confirmation ConfirmRequest="true" Message="Do you want to Save?" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnReset" runat="server" Icon="Reload" Text='<%$ Resource : RESET %>'>
                                    <DirectEvents>
                                        <Click OnEvent="btnReset_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnDownloadTemplete" runat="server" Icon="PageWhitePut" Text='<%$ Resource : TEMPLATE %>'>
                                    <Listeners>
                                        <Click Handler="window.location.href = '../downloads/ProductionDailyPlan.xlsx';" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnDownload" runat="server" Text='<%$ Resource : ERROR %>'>
                                    <DirectEvents>
                                        <Click OnEvent="btnDownload_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreImport" runat="server" PageSize="20" RemoteSort="false">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProductionDate" Type="Date" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="Seq" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductionQty" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="Weight_G" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="OrderType" />
                                        <ext:ModelField Name="Film" />
                                        <ext:ModelField Name="Box" />
                                        <ext:ModelField Name="Powder" />
                                        <ext:ModelField Name="Oil" />
                                        <ext:ModelField Name="FD" />
                                        <ext:ModelField Name="Stamp" />
                                        <ext:ModelField Name="Sticker" />
                                        <ext:ModelField Name="Mark" />
                                        <ext:ModelField Name="DeliveryDate" Type="Date" />
                                        <%--                                        <ext:ModelField Name="CustomerCode" />
                                        <ext:ModelField Name="CustomerName" />--%>
                                        <ext:ModelField Name="WorkingTime" />
                                        <ext:ModelField Name="OilType" />
                                        <ext:ModelField Name="Formula" />
                                    </Fields>
                                </ext:Model>
                            </Model>
<%--                            <Sorters>
                                <ext:DataSorter Property="LineCode" Direction="ASC" />
                            </Sorters>--%>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:DateColumn ID="Column9" runat="server" DataIndex="ProductionDate" Text='<%$ Resource : PRODUCTION_DATE %>' Align="Center" Width="150" Format="dd/MM/yyyy">
                                <Renderer Fn="changeDate" />
                            </ext:DateColumn>
                            <ext:Column ID="col1" runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE %>' Width="80" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" DataIndex="Seq" Text='<%$ Resource : ORDER_SEQ %>' Width="80" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="col2" runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Width="100">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="col3" runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCT_NAME %>' Width="300" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="col4" runat="server" DataIndex="ProductionQty" Text='<%$ Resource : QTY %>' Width="100" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="col5" runat="server" DataIndex="ProductUnitName" Text='<%$ Resource : UNIT %>' Width="120" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" DataIndex="Weight_G" Text='<%$ Resource : WEIGHT %>' Width="120" Align="Center" Hidden="true">
                                <%--<Renderer Fn="change" />--%>
                            </ext:Column>
                            <ext:Column ID="col7" runat="server" DataIndex="OrderNo" Text='<%$ Resource : ORDER_NO %>' Width="200">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="col8" runat="server" DataIndex="OrderType" Text='<%$ Resource : ORDER_TYPE %>' Width="100" Align="Center">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column3" runat="server" DataIndex="Film" Text='<%$ Resource : FILM %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column14" runat="server" DataIndex="Box" Text='<%$ Resource : BOX %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column15" runat="server" DataIndex="Powder" Text='<%$ Resource : POWDER %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column16" runat="server" DataIndex="Oil" Text='<%$ Resource : OIL %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="FD" Text='<%$ Resource : FD %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column17" runat="server" DataIndex="Stamp" Text='<%$ Resource : STAMP %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column5" runat="server" DataIndex="Sticker" Text='<%$ Resource : STICKER %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column6" runat="server" DataIndex="Mark" Text='<%$ Resource : MARK %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:DateColumn ID="colDeliveryDate" runat="server" DataIndex="DeliveryDate" Text='<%$ Resource : DELIVER_DATE %>' Width="100" Align="Left">
                                <Renderer Fn="changeDate" />
                            </ext:DateColumn>
                            <%-- <ext:Column ID="Column8" runat="server" DataIndex="CustomerCode" Text="Customer Code" Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>--%>
                            <%-- <ext:Column ID="Column10" runat="server" DataIndex="CustomerName" Text="Customer Name" Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>--%>
                            <ext:Column ID="Column11" runat="server" DataIndex="WorkingTime" Text='<%$ Resource : WORKING_TIME %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column12" runat="server" DataIndex="OilType" Text='<%$ Resource : FRYING_OIL %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column ID="Column13" runat="server" DataIndex="Formula" Text='<%$ Resource : INGREDIENT %>' Width="100" Align="Left">
                                <Renderer Fn="change" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>

                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill runat="server" />
                                <ext:Label runat="server" ID="txtTotal" />
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>

                </ext:GridPanel>

                <ext:ToolTip ID="ToolTip1"
                    runat="server"
                    Target="={#{grdImport}.getView().el}"
                    Delegate=".x-grid-cell"
                    TrackMouse="true">
                    <Listeners>
                        <BeforeShow Handler="return onBeforeShow(this,#{grdImport});" />
                    </Listeners>
                </ext:ToolTip>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
