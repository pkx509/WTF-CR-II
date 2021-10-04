<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_ucProductforInternalRec.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.inbound.receive_WTF._usercontrol._ucProductforInternalRec" %>

<ext:Window runat="server"
    ID="winProductSelect"
    Width="850"
    Height="500"
    Layout="FitLayout"
    Hidden="true"
    Modal="true"
    Title="<% $Resource : PRODUCTSELECT %>">

    <Items>

        <ext:Hidden runat="server" ID="hdReferenceDispatchTypeID" />
        <ext:Hidden runat="server" ID="hdFromReprocess" />
        <ext:Hidden runat="server" ID="hdToReprocess" />
        <ext:Hidden runat="server" ID="hdIsNormal" />
        <ext:Hidden runat="server" ID="hdIsCreditNote" />
        <ext:Hidden runat="server" ID="hdIsWithoutGoods" />
        <ext:Hidden runat="server" ID="hdIsItemChange" />
        <ext:Hidden runat="server" ID="hdPONo" />

        <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
            <Store>
                <ext:Store ID="StoreOfDataList"
                    runat="server"
                    PageSize="20"
                    RemoteSort="false"
                    RemotePaging="true"
                    AutoLoad="false">
                    <Proxy>
                        <ext:PageProxy DirectFn="App.direct.ProductSelectBindData" />
                    </Proxy>
                    <Model>
                        <ext:Model ID="Model" runat="server">
                            <Fields>
                                <ext:ModelField Name="ProductID" />
                                <ext:ModelField Name="ProductCode" />
                                <ext:ModelField Name="ProductName" />
                                <ext:ModelField Name="PONo" />
                                <ext:ModelField Name="MFGDate" Type="Date"/>
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>

            <ColumnModel runat="server">
                <Columns>

                    <ext:RowNumbererColumn runat="server" Text="<% $Resource : NUMBER %>" Width="50" Align="Center" Filterable="false" />

                    <ext:Column runat="server" DataIndex="ProductCode" Text="<% $Resource : PRODUCT_CODE %>">
                        <HeaderItems>
                            <ext:TextField runat="server" ID="hfProductCode" />
                        </HeaderItems>
                    </ext:Column>

                    <ext:Column runat="server" DataIndex="ProductName" Text="<% $Resource : PRODUCT_NAME %>" Flex="1" MinWidth="150">
                        <HeaderItems>
                            <ext:TextField runat="server" ID="hfProductName" />
                        </HeaderItems>
                    </ext:Column>

                    <ext:Column runat="server" DataIndex="PONo" Text="<% $Resource : PO_NO %>" Filterable="false" />
                    <ext:DateColumn runat="server" DataIndex="MFGDate" Text="<% $Resource : MFGDATE %>" Filterable="false" Format="dd/MM/yyyy"/>
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
                            <Listeners>
                                <Select Handler="#{PagingToolbar1}.moveFirst();" />
                            </Listeners>
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
                <ext:GridView runat="server" LoadMask="true" LoadingText="<% $Resource : LOADING %>" LoadingUseMsg="true" />
            </View>

            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>

            <ViewConfig StripeRows="true" />

        </ext:GridPanel>
    </Items>
</ext:Window>
