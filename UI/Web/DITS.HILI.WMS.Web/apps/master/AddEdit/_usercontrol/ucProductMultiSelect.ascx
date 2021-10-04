<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProductMultiSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit._usercontrol.ucProductMultiSelect" %>

<ext:Window runat="server" Title="Product Select"
    Width="700" Height="400" Layout="FitLayout"
    Hidden="true" ID="winProductMultiSelect" Modal="true">

    <Items>
        <ext:GridPanel ID="grdProductMultiSelect" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                            <DirectEvents>
                                <Click OnEvent="btnAdd_Click">
                                    <ExtraParams>
                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdProductMultiSelect}.getRowsValues({selectedOnly : true}))" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarFill />
                        <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="200">
                            <Listeners>
                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                            </Listeners>
                        </ext:TextField>
                        <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource : SEARCH %>">
                            <Listeners>
                                <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>

            <Store>
                <ext:Store ID="StoreProductMultiSelect" runat="server" PageSize="100"
                    RemoteSort="true" RemotePaging="true" AutoLoad="false">

                    <Proxy>
                        <ext:PageProxy DirectFn="App.direct.ProductMultiSelectBindData" />
                    </Proxy>

                    <Model>
                        <ext:Model ID="Model" runat="server">
                            <Fields>
                                <ext:ModelField Name="ProductID" />
                                <ext:ModelField Name="ProductCode" />
                                <ext:ModelField Name="ProductName" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Sorters>
                        <ext:DataSorter Property="ProductCode" Direction="DESC" />
                    </Sorters>
                </ext:Store>
            </Store>

            <ColumnModel ID="ColumnModelDriver" runat="server">
                <Columns>
                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />
                    <ext:Column ID="Column1" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Flex="1" MinWidth="100" />
                    <ext:Column ID="Column2" runat="server" DataIndex="Product_Name_Short" Text="<%$ Resource : PRODUCT_SHORT_NAME %>" Align="Left" Flex="1" MinWidth="200" Hidden="true"/>
                    <ext:Column ID="colProduct_Name_Full" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" MinWidth="300" />

                </Columns>
            </ColumnModel>

            <BottomBar>
<%--                                 <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                                            BeforePageText='<%$ Resource : BEFOREPAGE %>' >
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
                                        <Select Before="#{grdProductMultiSelect}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>--%>
              <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                                            BeforePageText='<%$ Resource : BEFOREPAGE %>' >
                </ext:PagingToolbar>
            </BottomBar>


<%--            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Multi" />
            </SelectionModel>--%>

            
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" ShowHeaderCheckbox="false" CheckOnly="true" />
            </SelectionModel>


            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>"  LoadingUseMsg="false" />
            </View>

        </ext:GridPanel>

    </Items>

</ext:Window>
