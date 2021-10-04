<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDispatchSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.registruck._usercontrol.ucDispatchSelect" %>

<ext:Window runat="server" Title="<% $Resource : DISPATCHSELECT %>"
    Width="850" Height="500" Layout="FitLayout"
    Hidden="true" ID="winDispatchSelect" Modal="true">

    <Items>

        <ext:GridPanel ID="grdDispatchSelect" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">

            <Store>
                <ext:Store ID="StoreDispatchSelect" runat="server" PageSize="20"
                    RemoteSort="false" RemotePaging="true" AutoLoad="false">
                    <Proxy>
                        <ext:PageProxy DirectFn="App.direct.DispatchSelectBindData" />
                    </Proxy>
                    <Model>
                        <ext:Model ID="Model" runat="server">
                            <Fields>
                                <ext:ModelField Name="PoNo" />
                                <ext:ModelField Name="CustomerName" />
                                <ext:ModelField Name="WarehouseName" />
                                <ext:ModelField Name="WarehouseID" />
                                <ext:ModelField Name="OrderNo" />
                                <ext:ModelField Name="DeliveryDate" Type="Date" />
                                <ext:ModelField Name="ShipToId" />
                                <ext:ModelField Name="ShiptoName" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Sorters>
                        <ext:DataSorter Property="PoNo" Direction="DESC" />
                    </Sorters>
                </ext:Store>
            </Store>

            <ColumnModel ID="ColumnModelDriver" runat="server">
                <Columns>

                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<% $Resource : NUMBER %>" Width="50" Align="Center" Filterable="false" />
                    <ext:Column runat="server" DataIndex="PoNo" Text="<% $Resource : PONO %>" Align="Left" MinWidth="120">
                        <HeaderItems>
                            <ext:TextField runat="server" ID="HeaderFilterDispatch" />
                        </HeaderItems>
                    </ext:Column>
                    <ext:Column runat="server" DataIndex="CustomerName" Text="<% $Resource : CUSTOMER %>" Align="Left" Flex="1" Filterable="false" />
                    <ext:Column runat="server" DataIndex="WarehouseName" Text="<% $Resource : WAREHOUSE_NAME %>" Align="Left" Filterable="false" />
                    <ext:Column runat="server" DataIndex="OrderNo" Text="<% $Resource : ORDERNO %>" Align="Left" MinWidth="120" Filterable="false" />
                    <ext:DateColumn runat="server" DataIndex="DeliveryDate" Text="<% $Resource : DELIVER_DATE %>" Align="Center" MinWidth="100" Format="dd/MM/yyyy" Filterable="false" />
                    <ext:Column runat="server" DataIndex="ShiptoName" Text="<% $Resource : SHIPTO %>" Align="Left" MinWidth="100" Filterable="false" />

                </Columns>
            </ColumnModel>

            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                    EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                    FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                    BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                    <Items>
                        <ext:Label ID="Label1" runat="server" Text='<%$ Resource : PAGESIZE %>' />
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
                                <%-- <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />--%>
                            </DirectEvents>
                        </ext:ComboBox>
                    </Items>
                </ext:PagingToolbar>
            </BottomBar>

            <DirectEvents>
                <CellDblClick OnEvent="grdDataList_CellDblClick">
                    <ExtraParams>
                        <ext:Parameter Name="DataKey" Value="record.data" Mode="Raw" />
                    </ExtraParams>
                    <EventMask ShowMask="true" />
                </CellDblClick>
            </DirectEvents>

            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
            </SelectionModel>

            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<% $Resource : LOADING %>" LoadingUseMsg="true" />
            </View>

            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>

            <ViewConfig StripeRows="true" />

        </ext:GridPanel>

    </Items>
</ext:Window>
