<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProductAdjustSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.adjust._usercontrol.ucProductAdjustSelect" %>

<script>
    var loadFilter = function (plugin, _product_code) {
        _product_code = _product_code.replace("\"", "");
        plugin.setValue({
            Product_Code: _product_code
        });
    };


    var KeyPressHandler = function (e) {

        if (App.ucProductAdjustSelect_StoreProductSelect.totalCount > 0) {
            var data = App.ucProductAdjustSelect_StoreProductSelect.data.items[0].data;
            App.direct.ucProductCode_Select(data);
        }

    }


</script>

<ext:Window runat="server" Title="Product Select"
    Width="950" Height="500" Layout="FitLayout"
    Hidden="true" ID="winProductSelect" Modal="true">
    <Listeners>
    </Listeners>
    <%--    <Items>
        <ext:Checkbox ID="chkUrgent" runat="server" FieldLabel="Urgent" Name="IsUrgent" />
    </Items>--%>
    <Items>
        <ext:GridPanel ID="grdProductSelect" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
            <Listeners>
                <AfterRender Handler="loadFilter(App.ucProductAdjustSelect_grdProductSelect.filterHeader,'');" />
            </Listeners>
       
            <Store>
                <ext:Store ID="StoreProductSelect" runat="server" PageSize="20"
                    RemoteSort="true" RemotePaging="true" AutoLoad="false">
                    <%--              <Proxy>
                        <ext:PageProxy DirectFn="App.direct.ProductSelectBindData" />
                    </Proxy>--%>
                    <Model>
                        <ext:Model ID="Model" runat="server">
                            <Fields>
                                <ext:ModelField Name="ProductCode" />
                                <ext:ModelField Name="ProductName" />                   
                                <ext:ModelField Name="Quantity" />
                                <ext:ModelField Name="Stock_Unit" />
                                <ext:ModelField Name="Warehouse" />
                                <ext:ModelField Name="Location" />
                                <ext:ModelField Name="Status" />
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
                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="50" Align="Center" />
                    <ext:Column ID="colProduct_Code" runat="server" DataIndex="Product_Code" Text="ProductCode" Align="Center" Width="100">
                        <Items>
                            <ext:TextField runat="server" ID="headerFilterProduct" />
                        </Items>
                    </ext:Column>
                    <ext:Column ID="colProduct_Name_Full" runat="server" DataIndex="Product_Name_Full" Text="ProductName" Align="Left" Flex="1" MinWidth="300" />
                    <ext:Column ID="colProduct_Quantity" runat="server" DataIndex="Quantity" Text="Quantity" Align="Right" Width="120" Filterable="false" />
                    <ext:Column ID="colStock_Unit" runat="server" DataIndex="Stock_Unit" Text="Stock Unit" Align="Left" Width="120" />
                    <ext:Column ID="colStatus" runat="server" DataIndex="Status" Text="Status" Align="Left" Width="120" />

                    <ext:Column ID="colWarehouse" runat="server" DataIndex="Warehouse" Text="Warehouse" Align="Right" Width="120" />


                    <ext:Column ID="colULocation" runat="server" DataIndex="Location" Text="Location" Align="Left" Width="120">
                    </ext:Column>
                </Columns>
            </ColumnModel>

            <%--            <DirectEvents>
                <CellDblClick OnEvent="grdDataList_CellDblClick">
                    <ExtraParams>
                        <ext:Parameter Name="oDataKeyId" Value="record.data" Mode="Raw" />
                    </ExtraParams>
                    <EventMask ShowMask="true" />
                </CellDblClick>

            </DirectEvents>--%>
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
            </SelectionModel>
            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="true" />
            </View>
            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>
            <ViewConfig StripeRows="true" />
            <KeyMap runat="server">
                <Binding>
                    <ext:KeyBinding Handler="if (App.ucProductAdjustSelect_StoreProductSelect.totalCount > 0) {var data = App.ucProductAdjustSelect_StoreProductSelect.data.items[0].data;App.direct.ucProductCode_Select(data); }"
                        DefaultEventAction="StopEvent">
                        <Keys>
                            <ext:Key Code="ENTER" />
                        </Keys>
                    </ext:KeyBinding>
                </Binding>
            </KeyMap>

            <BottomBar>
                <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="DisplayingFromTo"
                                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
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
                                                                                 #{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>

                        <ext:Button ID="Button1" runat="server"
                            Icon="ShapesManySelect" Text="Select" Width="80" TabIndex="15" MarginSpec="0 0 0 5" >
                        </ext:Button>

                        <ext:Button ID="Button2" runat="server" Icon="PageWhite" Text="Clear" Width="80" TabIndex="16"  MarginSpec="0 0 0 5" >
                        </ext:Button>

                        <ext:Button ID="Button3" runat="server" Icon="Cross" Text="Exit" Width="80" TabIndex="17"  MarginSpec="0 0 0 5" >
                            <Listeners>
                            </Listeners>
                        </ext:Button>

                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Toolbar>

            </BottomBar>
        </ext:GridPanel>

    </Items>

</ext:Window>