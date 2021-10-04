<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmTransferWarehouseDetail.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.tranfer.frmTransferWarehouseDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
     </script>
    <ext:XScript runat="server">
        <script> 
            var deleteProduct = function(ReclassifiedDetailID) {
                var grid = #{grdDataList}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.ReclassifiedDetailID == ReclassifiedDetailID)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 


            var prepareToolbar = function (grid, toolbar, rowIndex, record) {
                if (record.data.PickStatus == 30 || record.data.PickStatus == "") {
                    toolbar.items.getAt(0).setDisabled(true);
                }
                else
                {
                    toolbar.items.getAt(0).setDisabled(false);
                }
            };

            var getProduct = function () {

                App.direct.GetProduct("");
            };

            var before_select_row = function(grid, record, index, eOpts) {
                if(record.data.IsDefault == true)
                    return false;
                else
                    return true;
            }


            var beforeEditCheck = function(editor, e, eOpts){ 
                
                if(e.record.data.PickQty > e.record.data.PalletQty)
                {
                    e.cancel = true;
                    Ext.MessageBox.show({
                        title:'Warning',
                        msg: "Picking Quantity Over!",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });                   
                }  
            };

            var keyUpHandler = function(field, e) {
                #{btnSave}.setDisabled(false);
                var grid = #{grdDataList};
                var TransferQty = #{txtTotalTransferQty}.getValue();

                if(grid.store.getCount() == 0)
                {
                    if(field.record.data.PickQty > field.record.data.PalletQty)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Picking Quantity Over!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING,
                            fn: function(button){
                                if(button=='ok'){
                                    Ext.getCmp('colPickQty').focus('', 10);
                                    #{btnSave}.setDisabled(true);
                                }
                            }
                        });                   
                    } 
                    else
                    {
                        if(field.record.data.PickQty > TransferQty)
                        {
                            e.cancel = true;
                            Ext.MessageBox.show({
                                title:'Warning',
                                msg: "Transfer Quantity Over!",
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.WARNING,
                                fn: function(button){
                                    if(button=='ok'){
                                        Ext.getCmp('colPickQty').focus('', 10);
                                        #{btnSave}.setDisabled(true);
                                    }
                                }
                            });                   
                        } 
                    }
                }
                else
                {

                    var sumPick = 0; 
                    for(i=0;i<grid.store.getCount();i++)
                    {                    
                        sumPick += grid.store.data.items[i].data.PickQty;  
                    }  

                    if(field.record.data.PickQty > field.record.data.PalletQty)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Picking Quantity Over!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING,
                            fn: function(button){
                                if(button=='ok'){
                                    Ext.getCmp('colPickQty').focus('', 10);
                                    #{btnSave}.setDisabled(true);
                                }
                            }
                             
                        });                   
                    } 

                    if (sumPick > TransferQty)
                    {
                        Ext.MessageBox.show({
                            title:'WARNING',
                            msg: 'TransferQty Quantity Over!',
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING,
                            fn: function(button){
                                if(button=='ok'){
                                    Ext.getCmp('colPickQty').focus('', 10);
                                     #{btnSave}.setDisabled(true);
                                }
                            }
                        });
                        return;
                    }

                }

            }

            var sumChangeOver = function()
            {
                var grid = #{grdDataList};
                var TransferQty = #{txtTotalTransferQty}.getValue();
                var sumPick = 0; 
                var sumTransferQty = 0;

                for(i=0;i<grid.store.getCount();i++)
                {                    
                    sumPick += grid.store.data.items[i].data.PickQty; 
                }            

                if (sumPick >  TransferQty)
                {
                    Ext.MessageBox.show({
                        title:'WARNING',
                        msg: 'Trasfer Quantity Over!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(button){
                            if(button=='ok'){
                                // Ext.getCmp('txtBookingQty').focus('', 10); 
                            }
                        }
                    });
                    return;
                }

            };

    </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server"
                    Region="North"
                    Frame="true"
                    Margins="3 3 0 3"
                    Layout="AnchorLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                    <Items>
                        <ext:FieldSet runat="server" Layout="ColumnLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">
                            <Items>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.4">
                                    <Items>

                                        <ext:TextField runat="server" FieldLabel="<% $Resource : TRANSFER_NO %>"
                                            ID="txtTrmCode"
                                            ReadOnly="true"
                                            AllowBlank="false" />

                                        <ext:Hidden runat="server" ID="hddTrmId"></ext:Hidden>
                                        <ext:Hidden runat="server" ID="hddUnitId"></ext:Hidden>

                                        <ext:TextField runat="server"
                                            ID="txtProductCode"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : PRODUCT_CODE %>" />

                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtTotalTransferQty"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : TRANSFER_QTY %>"
                                            AllowBlank="false" Format="#,###.00" />

                                        <ext:TextField runat="server"
                                            ID="txtProductName"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : PRODUCT_NAME %>" />

                                    </Items>
                                </ext:Container>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                                    <Items>
                                        <ext:Button runat="server" ID="Button1" Icon="Add" Text="<%$ Resource : ADD_PALLET %>" Hidden="false">
                                            <DirectEvents>
                                                <Click OnEvent="btnAddItem_Click">
                                                    <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                    </Items>
                                </ext:Container>

                            </Items>
                        </ext:FieldSet>

                    </Items>

                    <Listeners>
                        <%--  <ValidityChange Handler="#{btnSave}.setDisabled(!valid); #{btnProduce}.setDisabled(!valid);" />--%>
                    </Listeners>

                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center"
                    Frame="true" Flex="1" Layout="FitLayout">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="TRM_Product_Detail_ID" />
                                        <ext:ModelField Name="TRM_CODE" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="TRM_Product_ID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="TRM_ID" />
                                        <ext:ModelField Name="TransferQty" />
                                        <ext:ModelField Name="PalletQty" />
                                        <ext:ModelField Name="PalletUnitID" />
                                        <ext:ModelField Name="PalletBaseUnitID" />
                                        <ext:ModelField Name="PalletBaseQty" />
                                        <ext:ModelField Name="ConfirmPickQty" />
                                        <ext:ModelField Name="PickQty" />
                                        <ext:ModelField Name="LotNo" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="NewPalletCode" />
                                        <ext:ModelField Name="PickStatus" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="Location" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.TRM_Product_ID" Mode="Raw" />
                                            <ext:Parameter Name="oDataKeyDetailId" Value="record.data.TRM_Product_Detail_ID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text='<%$ Resource : PALLETNO %>' MinWidth="120" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' MinWidth="100" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' MinWidth="150" />
                            <ext:Column runat="server" DataIndex="Location" Text='<%$ Resource : LOCATION %>' MinWidth="150" />
                            <ext:NumberColumn runat="server" DataIndex="PalletQty" Text="<% $Resource : PALLET_QTY %>" Format="#,###.00" Align="Right" MinWidth="100" />
                            <ext:ComponentColumn ID="colPickQty" runat="server" DataIndex="PickQty" Text="<% $Resource : PICK_QTY %>" Width="100" Align="Center"
                                Editor="true" Format="#,###">
                                <Component>
                                    <ext:NumberField runat="server" ID="txtPickQty" DataIndex="PickQty"
                                        Text="0" AllowBlank="false" AllowDecimals="true" SelectOnFocus="true">
                                        <Listeners>
                                            <Blur Fn="keyUpHandler" />
                                        </Listeners>
                                    </ext:NumberField>
                                </Component>
                            </ext:ComponentColumn>
                            <ext:Column runat="server" DataIndex="ProductUnitName" Text='<%$ Resource : UNIT %>' MinWidth="150"></ext:Column>
                            <ext:Column runat="server" DataIndex="NewPalletCode" Text='<%$ Resource : TRANSFER_PALLETNO %>' MinWidth="200"></ext:Column>
                        </Columns>
                    </ColumnModel>

                    <%--<Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />

                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>--%>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                    <BottomBar>
                        <ext:Toolbar runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />

                                        <ext:Button ID="btnSave" runat="server" Icon="Disk" Text='<%$ Resource : SAVE %>' Width="60">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Save?" Title="Save" />
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>

                                        </ext:Button>

                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60">
                                            <Listeners>
                                                <Click Handler="parentAutoLoadControl.close();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="WindowDataDetail" runat="server" Icon="FeedGo" Title="" Width="900" Height="450" Layout="BorderLayout" Resizable="false" Modal="true" Hidden="false">
            <Items>
                <ext:GridPanel ID="GridGetWindows" runat="server" Region="Center" SortableColumns="false"
                    RemoteSort="true" RemotePaging="true" Layout="FitLayout">

                    <Store>
                        <ext:Store ID="StoreWindowsShow" runat="server" PageSize="20"
                            RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.ProductSelectBindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="Location" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Quantity" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="ProductLot" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="BaseUnitId" />
                                        <ext:ModelField Name="BaseQuantity" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="MFGDate" />
                                        <ext:ModelField Name="LineCode" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />
                            <ext:Column ID="colProductName" runat="server" DataIndex="PalletCode" Text="<%$ Resource : PALLETNO %>" Align="Left" Flex="1" MinWidth="130" Filterable="true" />
                            <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Flex="1" MinWidth="100" Filterable="true" />
                            <ext:Column ID="Column2" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" MinWidth="200" Filterable="true" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ProductLot" Text="<%$ Resource : LOTNO %>" Align="Left" Width="100" Hidden="false" Filterable="true" />
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>" Flex="100" Format="dd/MM/yyyy" Filterable="false" />
                            <ext:Column ID="Column3" runat="server" DataIndex="LineCode" Text="<%$ Resource : LINE_NAME %>" Align="Left" Width="100" Filterable="true" />
                            <ext:Column ID="colQuantity" runat="server" DataIndex="Quantity" Text="<%$ Resource : QUANTITY %>" Align="Right" Width="100" Filterable="false" />
                            <ext:Column ID="colUOM_Name" runat="server" DataIndex="ProductUnitName" Text="<%$ Resource : UNIT %>" Align="Left" Width="100" Filterable="false">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Listeners>
                    </Listeners>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" CheckOnly="true" Width="30" Align="Center">
                            <Listeners>
                                <BeforeDeselect Fn="before_select_row" />
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:CellEditing ClicksToEdit="0">
                        </ext:CellEditing>
                    </Plugins>
                    <View>
                        <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="Loading..." />
                    </View>
                    <BottomBar>

                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="false" DisplayMsg="DisplayingFromTo"
                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                            FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                            BeforePageText="<center>Page</center>">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="Page size:" />
                                <ext:ToolbarSpacer ID="TbarSpacer" runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList2" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="20" />
                                    </SelectedItems>
                                </ext:ComboBox>

                                <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnConfirm_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreDetail" Mode="Raw" Value="Ext.encode(#{GridGetWindows}.getRowsValues({selectedOnly : false}))" />
                                                <ext:Parameter Name="ParamStoreData" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
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
                        </ext:PagingToolbar>

                    </BottomBar>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="true" />
                    </Plugins>
                </ext:GridPanel>

            </Items>
        </ext:Window>
    </form>
</body>
</html>
