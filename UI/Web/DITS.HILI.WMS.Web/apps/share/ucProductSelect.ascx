<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProductSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.share.ucProductSelect" %>



<script>
    var loadFilter = function (plugin, _product_code) {
        _product_code = _product_code.replace("\"", "");
        plugin.setValue({
            Product_Code: _product_code
        });
    };


    var KeyPressHandler = function (e) {
        alert(12121212);
        //if (App.ucProductSelect_StoreProductSelect.totalCount > 0) {
        //    var data = App.ucProductSelect_StoreProductSelect.data.items[0].data;
        //    App.direct.ucProductCode_Select(data);
        //}

    }


</script>


<ext:Window runat="server" Title="Product Select"
    Width="850" Height="500" Layout="FitLayout"
    Hidden="true" ID="winProductSelect" Modal="true">
    <Listeners>
    </Listeners>
    <%--    <Items>
        <ext:Checkbox ID="chkUrgent" runat="server" FieldLabel="Urgent" Name="IsUrgent" />
    </Items>--%>
    <Items>
        <ext:GridPanel ID="grdProductSelect" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
            <Listeners>
                <AfterRender Handler="loadFilter(App.ucProductSelect_grdProductSelect.filterHeader,'');" />
            </Listeners>
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.25">
                            <Items>

                                <ext:FieldContainer runat="server"
                                    LabelWidth="90"
                                    FieldLabel="" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Hidden runat="server" ID="hdRecevice_Code_Temp"></ext:Hidden>

                                        <ext:Checkbox ID="chkfilter1" runat="server" LabelWidth="110" FieldLabel="Show product all" Name="chkfilter1" Checked="true">
                                            <DirectEvents>
                                                <Change OnEvent="ReloadData" Buffer="350"></Change>
                                            </DirectEvents>

                                        </ext:Checkbox>
                                    </Items>
                                </ext:FieldContainer>



                            </Items>

                        </ext:Container>

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
                                <ext:ModelField Name="Product_System_Code" />
                                <ext:ModelField Name="Product_Code" />
                                <ext:ModelField Name="Supplier_Code" />
                                <ext:ModelField Name="ProductCategory_Full_Name" />
                                <ext:ModelField Name="Product_Name_Short" />
                                <ext:ModelField Name="Product_Name_Full" />
                                <ext:ModelField Name="Product_Model" />
                                <ext:ModelField Name="Product_Brand_Name" />
                                <ext:ModelField Name="ProductGroup_Level3_Full_Name" />
                                <ext:ModelField Name="Product_Lot" />
                                <ext:ModelField Name="Quantity" />
                                <ext:ModelField Name="Product_UOM_ID" />
                                <ext:ModelField Name="Product_UOM_Name" />
                                <ext:ModelField Name="IsActive" Type="Boolean" />
                                <ext:ModelField Name="Product_Status_Code" />
                                <ext:ModelField Name="Product_Sub_Status_Code" />
                                <ext:ModelField Name="Product_UOM_SKU" />
                                <ext:ModelField Name="Product_UOM_Height" />
                                <ext:ModelField Name="Product_UOM_Length" />
                                <ext:ModelField Name="Product_UOM_Width" />
                                <ext:ModelField Name="Product_UOM_Quantity" />
                                <ext:ModelField Name="Product_UOM_Weight" />
                                <ext:ModelField Name="Product_UOM_Package_Weight" />
                                <ext:ModelField Name="ReserveBalanceQty" />

                            </Fields>
                        </ext:Model>
                    </Model>
                    <Sorters>
                        <ext:DataSorter Property="Product_Code" Direction="DESC" />
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModelDriver" runat="server">
                <Columns>
                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$Resources:Langauge, No%>" Width="50" Align="Center" />
                    <ext:Column ID="colProduct_Code" runat="server" DataIndex="Product_Code" Text="<%$Resources:Langauge, ProductCode%>" Align="Center" Width="100">
                        <HeaderItems>
                            <ext:TextField runat="server" ID="headerFilterProduct" />
                        </HeaderItems>

                        <%--          <Items>
                            <ext:TextField runat="server" ID="headerFilterProduct" />
                        </Items>--%>
                    </ext:Column>
                    <ext:Column ID="colProduct_Name_Full" runat="server" DataIndex="Product_Name_Full" Text="<%$Resources:Langauge, ProductName%>" Align="Left" Flex="1" MinWidth="300" />
                    <ext:Column ID="colProduct_Brand_Name" runat="server" DataIndex="Product_Brand_Name" Text="<%$Resources:Langauge, BrandName%>" Align="Left" Width="120" />
                    <ext:Column ID="colProduct_Model" runat="server" DataIndex="Product_Model" Text="<%$Resources:Langauge, ProductModel%>" Align="Left" Width="120" />
                    <ext:Column ID="colProduct_Quantity" runat="server" DataIndex="Quantity" Text="<%$Resources:Langauge, Quantity%>" Align="Right" Width="120" Filterable="false" Hidden="true" />
                    <ext:Column ID="colProduct_Lot" runat="server" DataIndex="Product_Lot" Text="Lot" Align="Right" Width="120" Filterable="false" Hidden="true" />
<%--                    <ext:NumberColumn ID="colBalanceQTY" runat="server" DataIndex="BalanceQty" Hidden="true"
                        Text="<%$Resources:Langauge, Quantity%>" Width="90" Align="Center" Format="#,###.##"></ext:NumberColumn>   --%>
                               <ext:NumberColumn ID="colReserveBalanceQty" runat="server" DataIndex="ReserveBalanceQty" Hidden="true"
                                Text="<%$Resources:Langauge, Quantity%>" Width="90" Align="Center" Format="#,###.##" Filterable="false">
                            </ext:NumberColumn>
                    <ext:Column ID="colUOM_Name" runat="server" DataIndex="Product_UOM_Name" Text="<%$Resources:Langauge, Unit%>" Align="Left" Width="120" Filterable="false" Hidden="true">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <BottomBar>
                <ext:PagingToolbar ID="PagingProductSelect" runat="server" DisplayInfo="true" DisplayMsg="<%$Resources:Langauge, DisplayingFromTo%>"
                    EmptyMsg="<%$Resources:Langauge, NoDataToDisplay%>" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                    FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                    BeforePageText="<center>Page</center>">
                    <Items>
                        <ext:Label ID="Label1" runat="server" Text="Page size:" />
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
            <DirectEvents>
                <CellDblClick OnEvent="grdDataList_CellDblClick">
                    <ExtraParams>
                        <ext:Parameter Name="oDataKeyId" Value="record.data" Mode="Raw" />
                    </ExtraParams>
                    <EventMask ShowMask="true" />
                </CellDblClick>

            </DirectEvents>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
            </SelectionModel>

            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$Resources:Langauge, Loading%>" LoadingUseMsg="true" />
            </View>
            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>
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
