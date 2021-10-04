<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProductDispatchSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.usercontrol.ucProductDispatchSelect" %>

<script>
    var loadFilter = function (plugin, _product_code) {
        _product_code = _product_code.replace("\"", "");
        plugin.setValue({
            Product_Code: _product_code
        });
        //console.log(_product_code);
    };


    var KeyPressHandler = function (e) {
        // alert(12121212);
        //if (App.ucProductSelect_StoreProductSelect.totalCount > 0) {
        //    var data = App.ucProductSelect_StoreProductSelect.data.items[0].data;
        //    App.direct.ucProductCode_Select(data);
        //}

    }


</script>


<ext:Window runat="server" Title="Product Select"
    Width="700" Height="400" Layout="FitLayout"
    Hidden="true" ID="winProductSelect" Modal="true">
    <Listeners>
    </Listeners>
    <%--    <Items>
        <ext:Checkbox ID="chkUrgent" runat="server" FieldLabel="Urgent" Name="IsUrgent" />
    </Items>--%>
    <Items>

        <ext:GridPanel ID="grdProductSelect" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
            <%--            <Listeners>
                <AfterRender Handler="loadFilter(App.ucProductDispatchSelect_grdProductSelect.filterHeader,'');" />
            </Listeners>--%>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>

                        <ext:Checkbox ID="ckbIsStock" runat="server" Checked="false" FieldLabel="In Stock" LabelWidth="50">

                            <DirectEvents>
                                <Change OnEvent="btnSearch_Event" />
                            </DirectEvents>
                        </ext:Checkbox>
                        <ext:ToolbarFill />
                        <ext:TextField ID="txtSearch" runat="server" FieldLabel="<%$ Resource : PRODUCT %>" LabelAlign="Right">
                  
                            <Listeners>
                                     <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                            </Listeners>
                        </ext:TextField>
                        <ext:Hidden runat="server" ID="hidProduct_Status_Code" />
                        <ext:Hidden runat="server" ID="hidOrderNo" />
                        <ext:Hidden runat="server" ID="hidRefCode" />
                        <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">
                            <DirectEvents>
                                <Click OnEvent="btnSearch_Event" />
                            </DirectEvents>
                            <LoadingState Text="Please Wait..." />
                        </ext:Button>
                    </Items>
                </ext:Toolbar>

            </TopBar>
            <Store>
                <ext:Store ID="StoreProductSelect" runat="server" PageSize="20"
                    RemoteSort="true" RemotePaging="true" AutoLoad="false">
                    <Proxy>
                        <ext:PageProxy DirectFn="App.direct.ProductSelectBindData" />
                    </Proxy>
                    <Model>
                        <ext:Model ID="Model" runat="server">
                            <Fields>
                                <ext:ModelField Name="ProductID" />
                                <ext:ModelField Name="ProductGroupLevel3ID" />
                                <ext:ModelField Name="ProductGroupLevel3Name" />
                                <ext:ModelField Name="ProductCode" />
                                <ext:ModelField Name="ProductName" />
                                <ext:ModelField Name="Description" />
                                <ext:ModelField Name="ProductBrandID" />
                                <ext:ModelField Name="ProductBrandName" />
                                <ext:ModelField Name="ProductShapeName" />
                                <ext:ModelField Name="ProductShapeName" />
                                <ext:ModelField Name="Age" />
                                <ext:ModelField Name="IsActive" />
                                <ext:ModelField Name="ProductCodeModel" />
                                <ext:ModelField Name="ProductUnitID" />
                                <ext:ModelField Name="ProductUnitName" />
                                <ext:ModelField Name="Quantity" />
                                <ext:ModelField Name="BaseUnitId" />
                                <ext:ModelField Name="ProductHeight" />
                                <ext:ModelField Name="ProductLength" />
                                <ext:ModelField Name="ProductWidth" />
                                <ext:ModelField Name="ConversionQty" />
                                <ext:ModelField Name="BaseQuantity" />
                                <ext:ModelField Name="ProductWeight" />
                                <ext:ModelField Name="PackageWeight" />
                                <ext:ModelField Name="PriceUnitId" />
                                <ext:ModelField Name="PriceUnitName" />
                                <ext:ModelField Name="Price" />
                                <ext:ModelField Name="ProductOwnerId" />
                                <ext:ModelField Name="OrderType" />
                                <ext:ModelField Name="OrderNo" />
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
                    <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30">
                        <Commands>
                            <ext:CommandFill />
                            <ext:GridCommand Icon="Accept" ToolTip-Text="Select" CommandName="Select" />
                        </Commands>

                        <DirectEvents>
                            <Command OnEvent="CommandClick">
                                <ExtraParams>
                                    <ext:Parameter Name="oDataKeyId" Value="record.data" Mode="Raw" />

                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                </ExtraParams>

                            </Command>
                        </DirectEvents>

                    </ext:CommandColumn>
                    <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Flex="1" MinWidth="100" />
                    <ext:Column ID="colProductName" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" MinWidth="100" />
                    <ext:Column ID="colOrderType" runat="server" DataIndex="OrderType" Text="<%$ Resource : ORDER_TYPE %>" Align="Left" Flex="1" MinWidth="100" Hidden="true" />
                    <ext:Column ID="colOrderNo" runat="server" DataIndex="OrderNo" Text="<%$ Resource : ORDER_NO %>" Align="Left" Flex="1" MinWidth="100" Hidden="true" />

                    <ext:NumberColumn ID="colQuantity" runat="server" DataIndex="Quantity" Text="<%$ Resource : QUANTITY %>" Align="Right" Width="120" Hidden="false" Format="#,###.00" />
                    <ext:Column ID="colUOM_Name" runat="server" DataIndex="ProductUnitName" Text="<%$ Resource : UNIT %>" Align="Left" Width="120" Hidden="false">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <BottomBar>
                <ext:PagingToolbar ID="PagingProductSelect" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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
                            <Listeners>
                                <Select Handler="#{grdProductSelect}.store.pageSize = parseInt(this.getValue(), 10);
                                                        #{PagingProductSelect}.moveFirst();" />
                            </Listeners>
                        </ext:ComboBox>
                    </Items>
                    <Plugins>
                        <ext:ProgressBarPager runat="server" />
                    </Plugins>
                </ext:PagingToolbar>
            </BottomBar>

            <%--            <DirectEvents>
                <CellDblClick OnEvent="grdDataList_CellDblClick">
                    <ExtraParams>
                        <ext:Parameter Name="oDataKeyId" Value="record.data" Mode="Raw" />
                    </ExtraParams>
                    <EventMask ShowMask="true" />
                </CellDblClick>

            </DirectEvents>--%>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
            </SelectionModel>

            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" />
            </View>
            <%--            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>--%>
            <ViewConfig StripeRows="true" />

            <%--        <CustomConfig>
                <ext:ConfigItem Name="keyMap" Value="{}" Mode="Raw"  />
            </CustomConfig>--%>
            <%--            <KeyMap runat="server">
                <Binding>
                    <ext:KeyBinding Handler="if (App.ucProductSelect_StoreProductSelect.totalCount > 0) {var data = App.ucProductSelect_StoreProductSelect.data.items[0].data;App.direct.ucProductCode_Select(data); }"
                        DefaultEventAction="StopEvent">
                        <Keys>
                            <ext:Key Code="ENTER" />
                        </Keys>
                    </ext:KeyBinding>
                </Binding>
            </KeyMap>--%>

            <%--       <KeyMap runat="server">

                <Binding>
                    <ext:KeyBinding Handler="KeyPressHandler(1111)" DefaultEventAction="None">
                        <Keys>
                            <ext:Key Code="ENTER" />
                        </Keys>
                    </ext:KeyBinding>
                </Binding>
            </KeyMap>--%>
        </ext:GridPanel>
    </Items>

</ext:Window>
