<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmAdjust.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.adjust.frmAdjust" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

    <script type="text/javascript">
        //Ext.Ajax.timeout = 180000; // 1 sec
        //Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.AdjustStatus == 100) {
                toolbar.items.getAt(0).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.AdjustStatus == 10) {
                toolbar.items.getAt(1).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };

    </script>
    <ext:XScript runat="server">
        <script>
  
            var removeItem = function () {
                return;
                Ext.MessageBox.confirm('Delete', 'Are you sure you want to delete?', function(btn){
                    if(btn === 'yes'){
                        var grid = #{grdDataList},
                        sm = grid.getSelectionModel();

                        grid.store.remove(sm.getSelection());
                        if (grid.store.getCount() > 0) {
                            sm.select(0);
                        }
                        grid.editingPlugin.cancelEdit();

                        #{btnSave}.setDisabled(false);

                    }
                    else{
                        //some code
                    }
                });

               
            };

            var CheckAdjustQty = function(record,xx){
            
                var grid = #{grdDataList},
                    sm = grid.getSelectionModel();
    
                var sel_model =#{grdDataList}.getSelectionModel();
                var record = sm.getSelection()[0];

                var qty = record.raw.Stock_Product_Quantity;
                //  var qty = #{txt}.getValue();
                var adjust_qty = #{txtConfirmQty}.getValue(); 

                if (qty < 0)
                {
                    qty = qty*(-1)
                }
                if (adjust_qty < 0)
                {
                    adjust_qty = adjust_qty*(-1)
                }
                if (qty - adjust_qty < 0)
                {
                    Ext.MessageBox.show({
                        title:'WARNING',
                        msg: 'Adjust Quantity Over!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(button){
                            if(button=='ok'){
                                Ext.getCmp('txtConfirmQty').focus('', 10); 
                            }
                        }
                    });
                    return;
                }
           
          
            };


            var keyUpHandler = function(field, e) {

                var grid = #{grdDataList};

                var adjustType = #{cmbAdjustType}.getValue(); 

                if(adjustType == "AddStock")
                {
                    if(field.record.data.AdjustStockQty < 0)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Add Stock Quantity!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING
                        });                   
                    } 
                }

                if(adjustType == "ReduceStock")
                {
                    if(field.record.data.AdjustStockQty > 0)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Reduce Stock Quantity!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING
                        });                   
                    
                    } 
                }

                if(adjustType == "AddOther")
                {
                    if(field.record.data.AdjustStockQty < 0)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Add Stock Quantity!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING
                        });                   
                    } 
                }

                if(adjustType == "ReduceOther")
                {
                    if(field.record.data.AdjustStockQty > 0)
                    {
                        e.cancel = true;
                        Ext.MessageBox.show({
                            title:'Warning',
                            msg: "Reduce Stock Quantity!",
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING
                        });                   
                    
                    } 
                }

            }
 

        </script>
    </ext:XScript>

    <style>
        .search-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

            .search-item h3 {
                display: block;
                font: inherit;
                font-weight: bold;
                color: #222;
                margin: 0px;
            }

                .search-item h3 span {
                    float: right;
                    font-weight: normal;
                    margin: 0 0 5px 5px;
                    width: 100px;
                    display: block;
                    clear: none;
                }

        p {
            width: 650px;
        }

        .ext-ie .x-form-text {
            position: static !important;
        }

        input#cmbCustomer-inputEl {
            background-color: #FFFFCC;
            background-image: none;
        }

        div#ListCmbWarehouse_Name {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListCmbProductGroup_L3 {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListComboSupplier {
            border-top-width: 1 !important;
            width: 300px !important;
        }

        div#ListcmbPhysicalZone {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListcmbLogicalZone {
            border-top-width: 1 !important;
            width: 250px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Frame="true"
                    Region="North" Layout="FitLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                            <Items>
                                <ext:FieldSet runat="server" Layout="ColumnLayout" Title="InformationCreateJob" Padding="3" DefaultAnchor="90%">
                                    <Items>
                                        <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                                            <Items>
                                                <ext:TextField ID="txtJobNo" runat="server" TabIndex="1" Text="new" FieldLabel="Job No" LabelWidth="100"
                                                    MaxLength="50" EnforceMaxLength="true" ReadOnly="true" AllowBlank="false" />

                                                <ext:ComboBox runat="server" ID="cmbAdjustType"
                                                    FieldLabel="Adjust Type" LabelWidth="100" EmptyText="- Select -" AllowBlank="false">
                                                </ext:ComboBox>

                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="AnchorLayout" Padding="5">
                                            <Items>
                                                <ext:DateField runat="server" ID="dateCreateJob" FieldLabel="DateCreateJob" LabelWidth="100"
                                                    Format="dd/MM/yyyy" AllowBlank="false" Width="320" />

                                                <ext:TextField ID="txtRefered" runat="server" Width="320" LabelWidth="100" FieldLabel="Reference" MaxLength="20" />
                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="AnchorLayout" Padding="5">
                                            <Items>
                                                <ext:TextField ID="txtRemark" runat="server" Width="360" LabelWidth="60" FieldLabel="Remark" MaxLength="250" />
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:FieldSet>
                            </Items>
                        </ext:Container>
                    </Items>
                     <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center"
                    Frame="true" Flex="1" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAddItem" Icon="Add" Text="Get Product" Hidden="false">
                                    <DirectEvents>
                                        <Click OnEvent="btnAddItem_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AdjustDetailID" />
                                        <ext:ModelField Name="AdjustCode" />
                                        <ext:ModelField Name="ReferenceID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductLot" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="MFGDate" />
                                        <ext:ModelField Name="Product_EXP" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="Product_Status_Name" />
                                        <ext:ModelField Name="Product_Sub_Status_Code" />
                                        <ext:ModelField Name="Product_Sub_Status_Name" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="AdjustStockQty" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="AdjustTransactionType" />
                                        <ext:ModelField Name="AdjustStockUnitID" />
                                        <ext:ModelField Name="AdjustBaseUnitID" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="AdjustStatus" />
                                        <ext:ModelField Name="Remark" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelProductr" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20" Locked="true">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Data" CommandName="Delete" />
                                </Commands>
                                <Listeners>
                                    <Command Handler="Ext.MessageBox.confirm('Delete', 'Are you sure you want to delete?', function(btn){
                                                           if(btn === 'yes'){
                                                               #{grdDataList}.store.removeAt(recordIndex);
                                                               #{btnSave}.setDisabled(false); 
                                                               #{btnPrint}.setDisabled(true);
                                                           }
                                                           else{
                                                              //some code
                                                           }
                                                         });"></Command>
                                </Listeners>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:Column ID="Column12" runat="server" DataIndex="ProductStatusID" Hidden="true" />
                            <ext:Column ID="Column13" runat="server" DataIndex="Product_Sub_Status_Code" Hidden="true" />
                            <ext:Column ID="Column14" runat="server" DataIndex="LocationID" Hidden="true" />
                            <ext:Column ID="Column16" runat="server" DataIndex="ProductID" Hidden="true" />
                            <ext:Column ID="Column17" runat="server" DataIndex="ConversionQty" Hidden="true" />
                            <ext:Column ID="Column18" runat="server" DataIndex="Actual_Remain_Qty" Hidden="true" />
                            <ext:Column ID="Column19" runat="server" DataIndex="AdjustStockQty" Hidden="true" />
                            <ext:Column ID="Column20" runat="server" DataIndex="ProductUnitID" Hidden="true" />
                            <ext:Column ID="Column5" runat="server" DataIndex="AdjustStockUnitID" Hidden="true" />
                            <ext:Column ID="Column10" runat="server" DataIndex="AdjustBaseUnitID" Hidden="true" />
                            <ext:Column ID="Column22" runat="server" DataIndex="UnitPriceId" Hidden="true" />
                            <ext:Column ID="colAdjustDetailID" runat="server" DataIndex="AdjustDetailID" Hidden="true" />
                            <ext:Column ID="colAdjustCode" runat="server" DataIndex="AdjustCode" Hidden="true" />
                            <ext:Column ID="ColAdjustStatus" runat="server" DataIndex="AdjustStatus" Hidden="true" />
                            <ext:Column ID="Column21" runat="server" DataIndex="AdjustTransactionType" Hidden="true" />

                            <ext:RowNumbererColumn ID="colRowNo" runat="server" Text="No" Width="40" Align="Center" Locked="true" />
                            <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode" Text="ProductCode" Width="200" Align="Center" Locked="true" />
                            <ext:Column ID="colProductName" runat="server" DataIndex="ProductName" Text="ProductName" MinWidth="200" Align="Left" Locked="true" />
                            <ext:Column ID="colProductLot" runat="server" DataIndex="ProductLot" Text="SerialNo" Width="120" Align="Center" />
                            <ext:Column ID="colPalletCode" runat="server" DataIndex="PalletCode" Text="Pallet Code" MinWidth="150" />
<%--                            <ext:NumberColumn ID="colAdjustQty" runat="server" DataIndex="AdjustStockQty" Text="Adjust Qty" Width="110" Align="Center" Format="#.###">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" AllowDecimals="true" ID="txtConfirmQty" ReadOnly="false" />
                                </Editor>
                                <Listeners>
                                    <Blur Fn="keyUpHandler" />
                                </Listeners>
                            </ext:NumberColumn>--%>

                              <ext:ComponentColumn ID="colAdjustQty" runat="server" DataIndex="AdjustStockQty" Text="Adjust Qty" Width="110" Align="Center"
                                Editor="true" Format="#,###">
                                <Component>
                                    <ext:NumberField runat="server" ID="txtConfirmQty" DataIndex="AdjustStockQty"
                                        Text="0" AllowBlank="false" AllowDecimals="true" SelectOnFocus="true">
                                        <Listeners>
                                            <Blur Fn="keyUpHandler" />
                                        </Listeners>
                                    </ext:NumberField>
                                </Component>
                            </ext:ComponentColumn>

                            <ext:Column ID="colUOMName" runat="server" DataIndex="ProductUnitName" Text="Unit" Width="100" Align="Center" />
                            <ext:DateColumn ID="colMFG" runat="server" DataIndex="MFGDate" Text="MFG" />
                            <%--<ext:DateColumn ID="colEXP" runat="server" DataIndex="Product_EXP" Text="EXP" />
                            <ext:Column ID="colProductStatus" runat="server" DataIndex="Product_Status_Name" Text="Status" />
                            <ext:Column ID="colProductSubStatus" runat="server" DataIndex="Product_Sub_Status_Name" Text="Sub Status" />
                            <ext:Column ID="colLocation" runat="server" DataIndex="LocationNo" Text="LocationNo" Width="130" Align="Center" />                           
                            <ext:NumberColumn ID="colActualQuantity" runat="server" DataIndex="Package_Actual_Quantity" Text="Package Actual Quantity" />
                            <ext:NumberColumn ID="colPrice" runat="server" DataIndex="Price" Text="Price"  Hidden="true" />
                            <ext:Column ID="colUnitPrice" runat="server" DataIndex="Product_Price_UOM_Name" Text="Unit Price" Width="100" Align="Center" Hidden="true" />
                            <ext:Column ID="colRemark" runat="server" DataIndex="Remark" Text="Remark" Width="120" Align="Center">
                                <Editor>
                                    <ext:TextField ID="txtCRemark" runat="server" DataIndex="Remark" />
                                </Editor>
                            </ext:Column>--%>
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing ClicksToEdit="0">
                        </ext:CellEditing>
                    </Plugins>
                    <KeyMap runat="server">
                        <Binding>
                            <ext:KeyBinding Handler="removeItem" DefaultEventAction="StopEvent">
                                <Keys>
                                    <ext:Key Code="DELETE" />
                                </Keys>
                            </ext:KeyBinding>
                        </Binding>
                    </KeyMap>
                    <BottomBar>
                        <ext:Toolbar runat="server">
                             <Items>
                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                <ext:ToolbarSpacer Width="30" />
                                <ext:ToolbarSeparator runat="server" />
                                <ext:ToolbarSpacer Width="30" />

                                <%--<ext:Button ID="btnPrint" runat="server" Icon="Printer" Text="<%$Resources:Langauge, Print%>" Width="60" Disabled="true" TabIndex="14">
                                </ext:Button>--%>
                                <ext:ToolbarSpacer Width="30" />
                                <ext:ToolbarSeparator runat="server" />
                                <ext:ToolbarSpacer Width="30" />
                                 <ext:Checkbox ID="chkIsHide" runat="server" Hidden="true"
                                                            BoxLabel='Hide Item' MarginSpec="0 0 0 105"> 
                                                        </ext:Checkbox>
                                 <ext:Checkbox ID="chkIsNotSendToInterface" runat="server" Hidden="true"
                                                            BoxLabel='Not Sent Interface' MarginSpec="0 0 0 105"> 
                                                        </ext:Checkbox>
                               <ext:Button ID="btnApprove" runat="server"
                                    Icon="Accept" Text="Approve" Width="70" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnApprove_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Approving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="70" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="70" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnClear_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="18">
                                    <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>

                        </ext:Toolbar>
                       
                    </BottomBar>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="WindowDataDetail" runat="server" Icon="FeedGo" Title="" Width="900" Height="450" Layout="BorderLayout" Resizable="false" Modal="true">
            <Items>
                <ext:GridPanel ID="GridGetWindows" runat="server" Region="Center" SortableColumns="false" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar runat="server" Padding="5">
                            <Items>
                                <ext:ToolbarFill />
                                <ext:ComboBox ID="cmbWarehouseName"
                                    FieldLabel="<%$ Resource : WAREHOUSE %>"
                                    Editable="false"
                                    runat="server"
                                    DisplayField="Name"
                                    ValueField="WarehouseID"
                                    TriggerAction="All"
                                    SelectOnFocus="true"
                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                    MinChars="0"
                                    Width="250"
                                    LabelWidth="80"
                                    LabelAlign="Right"
                                    AllowBlank="false"
                                    TabIndex="2">
                                    <ListConfig LoadingText="Searching..." ID="ListWarehouse_Name">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                      {ShortName} : {Name}
						                        </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=WarehouseMk">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model4" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Code" />
                                                        <ext:ModelField Name="Name" />
                                                        <ext:ModelField Name="ShortName" />
                                                        <ext:ModelField Name="WarehouseID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>

                                    </Store>
                                    <Listeners>
                                        <Select Handler="#{txtWHShortName}.setValue(this.valueModels[0].data.ShortName);" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:TextField ID="txtProduct" FieldLabel="Product Code" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="200" LabelWidth="80" />
                                <ext:TextField ID="txtPallet" FieldLabel="Pallet Code" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="200" LabelWidth="70" />
                                <ext:TextField ID="txtLot" FieldLabel="Lot" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="150" LabelWidth="30" />
                                <ext:Button ID="btnSearchProduct" runat="server" Icon="Magnifier" TabIndex="0" Text="Search" Width="60">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearchProduct_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreStockBalance" runat="server" PageSize="20">
                            <Proxy>
                                <ext:AjaxProxy Url="">
                                    <ActionMethods Read="GET" />
                                    <Reader>
                                        <ext:JsonReader Root="data" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model8" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CyclecountDetailID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="StockQuantity" />
                                        <ext:ModelField Name="MFGTimeStart" Type="Date" />
                                        <ext:ModelField Name="MFGTimeEnd" Type="Date" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="ProductSubStatusID" />
                                        <ext:ModelField Name="ProductStatusName" />
                                        <ext:ModelField Name="ProductSubStatusName" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="Price" />
                                        <ext:ModelField Name="UnitPriceId" />
                                        <ext:ModelField Name="UnitPriceName" />
                                        <ext:ModelField Name="RemainStockUnitID" />
                                        <ext:ModelField Name="RemainBaseUnitID" />
                                        <ext:ModelField Name="AdjustStockUnitID" />
                                        <ext:ModelField Name="AdjustBaseUnitID" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="WarehouseName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column ID="Column1" runat="server" DataIndex="ProductStatusID" Hidden="true" />
                            <ext:Column ID="Column2" runat="server" DataIndex="ProductSubStatusID" Hidden="true" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ProductID" Hidden="true" />
                            <ext:Column ID="Column4" runat="server" DataIndex="CyclecountDetailID" Hidden="true" />
                            <ext:Column ID="Column6" runat="server" DataIndex="ConversionQty" Hidden="true" />
                            <ext:Column ID="Column7" runat="server" DataIndex="LocationID" Hidden="true" />
                            <ext:Column ID="Column8" runat="server" DataIndex="StockQuantity" Hidden="true" />
                            <ext:Column ID="Column9" runat="server" DataIndex="ProductUnitID" Hidden="true" />
                            <ext:Column ID="Column23" runat="server" DataIndex="AdjustStockUnitID" Hidden="true" />
                            <ext:Column ID="Column24" runat="server" DataIndex="AdjustBaseUnitID" Hidden="true" />
                            <ext:Column ID="Column11" runat="server" DataIndex="RemainStockUnitID" Hidden="true" />
                            <ext:Column ID="Column15" runat="server" DataIndex="RemainBaseUnitID" Hidden="true" />

                            <ext:RowNumbererColumn ID="colWRowNo" runat="server" Text="No" Width="40" Align="Center" Locked="true" />
                            <ext:Column ID="ColWarehouseName" runat="server" DataIndex="WarehouseName" Text="Warehouse Name" Align="Center" Locked="true" />
                            <ext:Column ID="colWProductCode" runat="server" DataIndex="ProductCode" Text="ProductCode" Align="Center" Locked="true" />
                            <ext:Column ID="colWProductName" runat="server" DataIndex="ProductName" Text="ProductName" MinWidth="200" Align="Left" Locked="true" />
                            <ext:Column ID="colWProductLot" runat="server" DataIndex="Lot" Text="SerialNo" Width="120" Align="Center" />
                            <ext:Column ID="colWPalletCode" runat="server" DataIndex="PalletCode" Text="Pallet Code" Width="120" Align="Center" />
                            <ext:DateColumn ID="colWDTEMFG" runat="server" DataIndex="MFGDate" Text="MFG" />
                            <ext:DateColumn ID="colWDTEEXP" runat="server" DataIndex="MFGTimeEnd" Text="EXP" />
                            <ext:Column ID="colWUOMName" runat="server" DataIndex="ProductUnitName" Text="Unit" Width="100" Align="Center" />
                            <ext:Column ID="colWStatus" runat="server" DataIndex="ProductStatusName" Text="Status" Width="100" Align="Center" />
                            <ext:Column ID="colWSubStatus" runat="server" DataIndex="ProductSubStatusName" Text="Sub Status" Width="100" Align="Center" />
                            <ext:NumberColumn ID="colWPrice" runat="server" DataIndex="Price" Text="Price" Hidden="true" />
                            <ext:NumberColumn ID="colWQuantity" runat="server" DataIndex="StockQuantity" Text="Quantity" Format="#.###" />
                            <ext:Column ID="colWUnitPrice" runat="server" DataIndex="UnitPriceName" Text="Unit Price" Width="100" Align="Center" Hidden="true" />
                            <ext:Column ID="colWLocation" runat="server" DataIndex="LocationNo" Text="LocationNo" Width="130" Align="Center" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                    </SelectionModel>
                    <Plugins>
                        <ext:CellEditing ClicksToEdit="0">
                        </ext:CellEditing>
                    </Plugins>
                    <View>
                        <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="Loading" />
                    </View>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="false" DisplayMsg="DisplayingFromTo"
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
                                    <DirectEvents>
                                        <Change OnEvent="btnSearchProduct_Click" />
                                    </DirectEvents>
                                </ext:ComboBox>

                                <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnSelectItem_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreWProduct" Mode="Raw" Value="Ext.encode(#{GridGetWindows}.getRowsValues({selectedOnly : false}))" />
                                                <ext:Parameter Name="ParamStoreWData" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
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
                </ext:GridPanel>

            </Items>
        </ext:Window>


    </form>
</body>
</html>

