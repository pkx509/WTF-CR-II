<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucQAPalletMultiSelect.ascx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa._usercontrol.ucQAPalletMultiSelect" %>

<script>
    var loadFilter = function (plugin, _product_code) {
        _product_code = _product_code.replace("\"", "");
        plugin.setValue({
            Product_Code: _product_code
        });
        //console.log(_product_code);
    };

    var before_select_row = function (grid, record, index, eOpts) {
        if (record.data.IsDefault == true)
            return false;
        else
            return true;
    }
    var KeyPressHandler = function (e) {

        //if (App.ucProductSelect_StoreProductSelect.totalCount > 0) {
        //    var data = App.ucProductSelect_StoreProductSelect.data.items[0].data;
        //    App.direct.ucProductCode_Select(data);
        //}

    }


</script>


<ext:Window runat="server" Title="Product Select"
    Width="1030" Height="430" Layout="FitLayout"
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
                        <%--<ext:FieldContainer runat="server" Layout="AnchorLayout" LabelAlign="Right">
                            <Items>
                                <ext:TextField ID="txtPalletNo" runat="server" FieldLabel="<%$ Resource : PALLETNO %>" Width="250" LabelWidth="100"></ext:TextField>
                                <ext:TextField ID="txtProductCode" runat="server" FieldLabel="<%$ Resource : PRODUCT_CODE %>" Width="250" LabelWidth="100"></ext:TextField>

                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="AnchorLayout" LabelAlign="Right">
                            <Items>
                                <ext:TextField ID="txtProductName" runat="server" FieldLabel="<%$ Resource : PRODUCT_NAME %>" Width="250" LabelWidth="100"></ext:TextField>

                                <ext:TextField ID="txtLot" runat="server" FieldLabel="<%$ Resource : LOTNO %>" Width="250" LabelWidth="100"></ext:TextField>

                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="AnchorLayout" LabelAlign="Right">
                            <Items>
                                <ext:DateField runat="server" ID="dtMFGDate"
                                    FieldLabel='<%$ Resource : MFGDATE %>'
                                    MaxLength="10"
                                    EnforceMaxLength="true"
                                    Format="dd/MM/yyyy"
                                    Width="250" LabelWidth="100" />
                                <ext:TextField ID="txtLine" runat="server" FieldLabel="<%$ Resource : LINE_NAME %>" Width="250" LabelWidth="100"></ext:TextField>


                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Event" />
                                    </DirectEvents>
                                    <LoadingState Text="Please Wait..." />
                                </ext:Button>
                                <ext:Hidden runat="server" ID="hidProduct_Status_Code" />
                            </Items>
                        </ext:FieldContainer>--%>
                                <ext:Hidden runat="server" ID="hidProduct_Status_Code" />
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
                                <ext:ModelField Name="Location" />
                                <ext:ModelField Name="ProductCode" />
                                <ext:ModelField Name="ProductID" />
                                <ext:ModelField Name="PalletCode" />
                                <ext:ModelField Name="ProductName" />
                                <ext:ModelField Name="Qty" />
                                <ext:ModelField Name="UnitName" />
                                <ext:ModelField Name="LotNo" />
                                <ext:ModelField Name="MFGDate" Type="Date" />
                                <ext:ModelField Name="MFGDate" />
                                <ext:ModelField Name="LineCode" />
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

                    <ext:Column ID="colProductName" runat="server" DataIndex="PalletCode" Text="<%$ Resource : PALLETNO %>" Align="Left" Flex="1" MinWidth="130"  Filterable="true"/>
                    <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Flex="1" MinWidth="100"  Filterable="true"/>
                    <ext:Column ID="Column2" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" MinWidth="200"  Filterable="true"/>
                    <ext:Column ID="Column1" runat="server" DataIndex="LotNo" Text="<%$ Resource : LOTNO %>" Align="Left" Width="100" Hidden="false"  Filterable="true"/>
                    <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>" Flex="100" Format="dd/MM/yyyy"  Filterable="false"/>
                    <ext:Column ID="Column3" runat="server" DataIndex="LineCode" Text="<%$ Resource : LINE_NAME %>" Align="Left" Width="100"  Filterable="true"/>
                    <ext:Column ID="colQuantity" runat="server" DataIndex="Qty" Text="<%$ Resource : QUANTITY %>" Align="Right" Width="100" Filterable="false" />
                    <ext:Column ID="colUOM_Name" runat="server" DataIndex="UnitName" Text="<%$ Resource : UNIT %>" Align="Left" Width="100"  Filterable="false">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi" CheckOnly="true" Width="30" Align="Center">
                    <Listeners>
                        <BeforeDeselect Fn="before_select_row" />
                    </Listeners>
                </ext:CheckboxSelectionModel>
            </SelectionModel>
            <BottomBar>
                <ext:PagingToolbar ID="PagingProductSelect" runat="server" DisplayInfo="false" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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


                        <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnConfirm_Click">
                                            <ExtraParams> 
                                                 <ext:Parameter Name="ParamStoreData" Mode="Raw" Value="Ext.encode(#{grdProductSelect}.getRowsValues({selectedOnly : true}))" />

                                            </ExtraParams>
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnWinExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="9">
                                    <Listeners>
                                        <Click Handler="#{WindowDataDetail}.hide();" />
                                    </Listeners>
                                </ext:Button>

                    </Items>
                    <Plugins>
                        <ext:ProgressBarPager runat="server" />
                    </Plugins>
                </ext:PagingToolbar>
            </BottomBar>
            
            <Plugins>
                <ext:FilterHeader runat="server" Remote="true" />
            </Plugins>
            <View>
                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" />
            </View> 
            <ViewConfig StripeRows="true" />

            
        </ext:GridPanel>
    </Items>

</ext:Window>
