<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmBackOrderList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.backorder.frmBackOrderList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
     <%-- <script type="text/javascript">
          Ext.Ajax.timeout = 180000; // 1 sec
          Ext.net.DirectEvent.timeout = 180000; // 1 sec 
      </script>--%>
    <script type="text/javascript">

        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (!record.data.IsConfirm) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.DispatchQty == record.data.BackOrderQty) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };



        var IsBackOrdered = function (value, metadata, record) {

            if (record.data.IsBackOrder)
                return "<img src='../../../images/bo.png' width='15px'/>";
            else
                return "<img src='../../../images/tick.png' width='15px'/>";

        }
        var IsConfirmed = function (value, metadata, record) {

            if (record.data.IsConfirm)
                return "<img src='../../../images/tick.png' width='15px'/>";
            else
                return "<img src='../../../images/bo.png' width='15px'/>";

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill />
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="DriverFName" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text='<%$ Resource: SEARCH %>'>
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" OnReadData="Store_Refresh"
                            RemoteSort="true" RemotePaging="true" AutoLoad="true" GroupField="Pono">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData">
                                </ext:PageProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DispatchCode" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Pono" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="CustomerName" />
                                        <ext:ModelField Name="DispatchQty" />
                                        <ext:ModelField Name="BackOrderQty" />
                                        <ext:ModelField Name="UnitName" />
                                        <ext:ModelField Name="EstDispatchDate" Type="Date" />
                                        <ext:ModelField Name="IsBackOrder" />
                                        <ext:ModelField Name="IsConfirm" />
                                        <ext:ModelField Name="BookingId" />


                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Pono" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <Features>
                        <ext:GroupingSummary
                            ID="group"
                            ShowSummaryRow="false"
                            runat="server"
                            GroupHeaderTplString="PO No. : {groupValue} ({rows.length} Item{[values.rows.length > 1 ? 's' : '']})"
                            HideGroupedHeader="true"
                            EnableGroupingMenu="true" />
                    </Features>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:CommandColumn runat="server" ID="colReviseNo" Sortable="false" Align="Center" Width="30" Hidden="false">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Revise"  />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyBookingId" Value="record.data.BookingId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='You sure is delete this back order !' Title='Delete' />
                                        
                                        <EventMask ShowMask="true" Msg="Delete..." MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <%--   <PrepareToolbar Fn="prepareToolbarDelete" />--%>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colConfirm" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="ApplicationAdd" ToolTip-Text="Booking" CommandName="Confirm" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.DispatchCode" Mode="Raw" />
                                            <ext:Parameter Name="oDataIsConfirm" Value="record.data.IsConfirm" Mode="Raw" />
                                            <ext:Parameter Name="oDataPono" Value="record.data.Pono" Mode="Raw" />
                                            <ext:Parameter Name="oDataKeyBookingId" Value="record.data.BookingId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message="Do you want to booking?" Title="Booking" />
                                        <EventMask ShowMask="true" Msg="Booking..." MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarConfirm" />
                            </ext:CommandColumn>
                            <ext:Column ID="colBookingId" runat="server" DataIndex="BookingId" Text="BookingId" Align="Left" Flex="1" Hidden="true" />
                            <ext:Column ID="colDispatchCode" runat="server" DataIndex="DispatchCode" Text="<%$ Resource : DISPATCH_CODE %>" Align="Left" Flex="1" />

                            <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Flex="1" />
                            <ext:Column ID="colProductName" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" />
                            <ext:Column ID="colCustomerName" runat="server" DataIndex="CustomerName" Text="<%$ Resource : CUSTOMER %>" Width="140" Align="Left" Flex="1" />
                            <ext:NumberColumn ID="colDispatchQty" runat="server" DataIndex="DispatchQty" Text="<% $Resource : BOOKING_QUANTITY%>" Width="100" Align="Right" Format="#,###.00" Flex="1" />
                            <ext:NumberColumn ID="colBackOrderQty" runat="server" DataIndex="BackOrderQty" Text="<% $Resource : BACKORDER_QUANTITY%>" Width="100" Align="Right" Format="#,###.00" Flex="1" />
                            <ext:Column ID="colUnitName" runat="server" DataIndex="UnitName" Text="<%$ Resource : UNIT %>" Width="140" Align="Left" Flex="1" />
                            <ext:Column ID="colLocationCode" runat="server" DataIndex="LocationCode" Text="<%$ Resource : LOCATION_NO %>" Width="140" Align="Left" Flex="1" Hidden="true" />
                            <ext:DateColumn ID="colOrderDate" runat="server" Groupable="false"
                                DataIndex="EstDispatchDate" Text="<%$ Resource : ESTDISPATCH_DATE %>"
                                Align="Center" Format="dd/MM/yyyy" />
                            <ext:Column ID="colIsBackOrder" runat="server" Text="<%$ Resource : BACKORDER %>" DataIndex="IsBackOrder" Align="Center" Width="80" Hidden="true">
                                <Renderer Fn="IsBackOrdered">
                                </Renderer>
                            </ext:Column>

                            <ext:Column ID="colIsConfirm" runat="server" Text="<%$ Resource : CONFIRM %>" DataIndex="IsConfirm" Align="Center" Width="80">
                                <Renderer Fn="IsConfirmed">
                                </Renderer>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="<%$ Resource : PAGESIZE %>" />
                                <ext:ToolbarSpacer ID="TbarSpacer" runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="20" />
                                    </SelectedItems>
                                    <DirectEvents>
                                        <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <%--                   <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.UnitID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>--%>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

