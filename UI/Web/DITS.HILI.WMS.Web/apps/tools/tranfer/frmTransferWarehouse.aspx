<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmTransferWarehouse.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.tranfer.frmTransferWarehouse" %>

<%@ Register Src="~/apps/tools/tranfer/_usercontrol/ucTransferProductSelect.ascx" TagPrefix="uc" TagName="ucTransferProductSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.PickStatus == 20 || record.data.PickStatus == 30 || record.data.PickStatus == 100 || record.data.PickStatus == 102) {
                toolbar.items.getAt(0).setDisabled(true);
            }
            else
            {
                toolbar.items.getAt(0).setDisabled(false);
            }
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.PickStatus == 30 || record.data.PickStatus == 102 ||  record.data.PickStatus == "") {
                toolbar.items.getAt(0).setDisabled(true);
            }
            else
            {
                toolbar.items.getAt(0).setDisabled(false);
            }
        };

        function popitup(url, windowName) {

            var browser = navigator.appName;
            if (browser == 'Microsoft Internet Explorer') {
                window.opener = self;

            }
            newwindow = window.open(url, windowName, 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=900,height=600');
            window.moveTo(0, 0);
            self.close();

            if (window.focus) { newwindow.focus() }
            return false;
        }

    </script>
    <ext:XScript runat="server">
        <script>
            var deleteProduct = function(TRM_Product_ID) {
                var grid = #{grdDataList}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.TRM_Product_ID == TRM_Product_ID)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 
            var deleteProduct2 = function(TRM_Product_ID,TransferUnitID) {
                var grid = #{grdDataList}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.TRM_Product_ID == TRM_Product_ID && grid.store.data.items[i].data.TransferUnitID == TransferUnitID)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 
            var getProduct = function () {

                var product_code = App.txtProduct_System_Code.getValue();


                App.hidAddProduct_System_Code.reset();
                App.txtAddProduct_Name_Full.reset();
                App.hidAddUomID.reset();
                App.hidAddUomName.reset();

                App.direct.GetProduct(product_code);
            };

            var popupProduct = function () {
                if (App.cmbDispatchType.getValue() == null) {
                    MessageBox.Warning("Please Select Dispatch type",
                        function () { App.cmbDispatchType.focus(true, 100); });
                    return;
                }
                App.direct.GetProduct('');
            };

            var addProduct = function () {
                 

                var grid = #{grdDataList};
                var btnAddItem = #{btnAddItem};
                var freeRecord = -1;

                if(#{btnAddItem}.disabled == true)
                    return;

                var ProductId = App.hidAddProduct_System_Code.getValue();
                var UnitId = App.hidAddUomID.getValue();

                var CheckProduct = grid.store.data.items.filter(function (val) {
                    return val.data['Product_ID'] == ProductId && val.data['TransferUnitID'] == UnitId;
                });

                if(CheckProduct.length > 0){
                    Ext.MessageBox.show({
                        title:'Warning',
                        msg:  "กรุณาตรวจสอบรายการสินค้า",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    return;
                }
   
                
                App.direct.ValidateProdcut(App.txtProduct_System_Code.getValue(),App.hidAddProduct_System_Code.getValue() , {
                    success : function (result) {
                        if (!result.valid) {
                            Ext.MessageBox.show({
                                title:'Warning',
                                msg:  result.msg,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.WARNING
                            });
                            return;
                        }else{ 

   


                            var records= grid.store.getRange();


                            for (var i = 0; i < records.length; i++) {
                                var rec = records[i];
                                
                                //if(rec.data.RuleId != App.hidSpecial_Rul_ID.getValue()){

                                //    Ext.getCmp('txtProduct_System_Code').focus('', 20);
                                //    Ext.getCmp('cmbDispatchType').readOnly = true;
                                //    Ext.getCmp('chkIsBackorder').readOnly= true;
                             
                                //    resetcontrol();

                                //    Ext.MessageBox.show({
                                //        title:'Warning',
                                //        msg:  'Can not channge shipping To as rule not the same after add product ? <br> Please select shipto as before or delete all product already add new product!',
                                //        buttons: Ext.MessageBox.OK,
                                //        icon: Ext.MessageBox.WARNING
                                //    });
                                
                                //    return;
                                //}

                                if (rec.data.ProductCode == App.txtProduct_System_Code.getValue() && rec.data.StockUnitId == App.hidAddUomID.getValue()) {
        

                                    Ext.getCmp('txtProduct_System_Code').focus('', 20);
                                    Ext.getCmp('cmbDispatchType').readOnly = true;
                                    Ext.getCmp('chkIsBackorder').readOnly= true;
                             
                                    resetcontrol();
                                                
                                    Ext.MessageBox.show({
                                        title:'Warning',
                                        msg:  'Product are duplicate',
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.WARNING
                                    });
                                                                    
                                }                 
                            }

                            if(App.hidAddProduct_Code.getValue() != ""){
                            
                                record = grid.store.add({
                                    Product_ID: App.hidAddProduct_System_Code.getValue(),
                                    ProductStatusID: App.hidAddProductStatusId.getValue(),
                                    ProductCode: App.hidAddProduct_Code.getValue(),
                                    ProductName: App.txtAddProduct_Name_Full.getValue(),
                                    TransferUnitID :  App.hidAddUomID.getValue(),
                                    ProductUnitName : App.hidAddUomName.getValue(),
                                    TransferQty: App.txtAddQty.getValue() == null ? 0 : App.txtAddQty.getValue(),
                                    PickQty: 0 ,
                                    ConfirmQty: 0 ,
                                    ProductHeight : App.hidAddUomHeight.getValue(),
                                    ProductLength : App.hidAddUomLength.getValue(),
                                    ProductWidth : App.hidAddUomWidth.getValue(),
                                    TransferBaseUnitID : App.hidAddUomSKU.getValue(),
                                    TransferBaseQty : App.hidAddBaseQty.getValue(),
                                    ConversionQty : App.hidAddUomQty.getValue(),
                                    ProductOwnerId : App.hidAddProductOwnerId.getValue()
                                });  
                            
                            }                          

                            
                            sumTotal(); 
                            

                            App.hidAddProduct_System_Code.reset();
                            App.hidAddProduct_Code.reset();
                            App.txtProduct_System_Code.reset();
                            App.hidAddProductStatusId.reset();
                            App.txtAddProduct_Name_Full.reset();
                            App.hidAddUomID.reset();
                            App.hidAddUomName.reset();
                            App.hidAddPriceUnitId.reset();
                            App.hidAddPriceUnitName.reset();
                            App.hidAddPrice.reset();
                            App.hidAddUomQty.reset();
                            App.hidAddBaseQty.reset();
                            App.hidAddProductOwnerId.reset();
                            App.hidAddUomWidth.reset();
                            App.hidAddUomLength.reset();
                            App.hidAddUomHeight.reset();
                            App.hidAddUomSKU.reset();
                            App.txtUOM.reset();
                            App.txtAddQty.reset();
                            App.txtProduct_System_Code.focus(true, 100);
                            App.btnSave.setDisabled(0);
                        }
                          
                    }
                });
                        
            };

            var sumTotal = function(){

                var grid = #{grdDataList};
                var gridCount = grid.store.getCount();

                var sumNet = 0; 
                var sumGross = 0;
                var sumQTY = 0;
               
                for(i=0;i<gridCount;i++)
                {                
                    var store_transfer = grid.store.data.items[i].data;                    
                    var product_quantity =  parseFloat(store_transfer.TransferQty);
                    sumQTY += product_quantity; 
                }            

                //#{txttotalNetWeight}.setValue(parseFloat(sumNet).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //#{txttotalGrossWeight}.setValue(parseFloat(sumGross).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")); 
                //#{txttotalQTY}.setValue(parseFloat(sumQTY).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            };

            var beforeEditCheck = function(editor, e, eOpts){ 
                
                if(e.record.data.PickQty > e.record.data.PalletQty)
                {
                    e.cancel = true;
                    Ext.MessageBox.show({
                        title:'Warning',
                        msg: "Can't edit item.",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                   
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
                                        <ext:FieldContainer runat="server" FieldLabel="<% $Resource : TRANSFER_NO %>" Layout="HBoxLayout" LabelWidth="150">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtTrmCode"
                                                    ReadOnly="true"
                                                    AllowBlank="false" Width="150" />

                                                <ext:Hidden runat="server" ID="hddTrmId"></ext:Hidden>

                                                <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="btnProductSelect" TabIndex="12">
                                                    <Listeners>
                                                        <Click Fn="getProduct" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:DateField runat="server"
                                            ID="TransferDate"
                                            FieldLabel='<%$ Resource : TRANSFER_DATE %>'
                                            LabelWidth="150"
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1"
                                            LabelAlign="Right" />

                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtTransferStatus"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : TRANSFER_STATUS %>" />

                                        <ext:DateField runat="server"
                                            ID="dtApproveDate"
                                            FieldLabel='<%$ Resource : APPROVE_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1"
                                            LabelAlign="Right" />

                                    </Items>
                                </ext:Container>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                                    <Items>

                                        <ext:TextArea runat="server"
                                            ID="txtDesc"
                                            Flex="1" Rows="1"
                                            FieldLabel="<% $Resource : DESCRIPTION %>" />

                                    </Items>
                                </ext:Container>

                                <ext:Hidden runat="server" ID="hidProduct_Status_Code" />
                            </Items>
                        </ext:FieldSet>

                        <ext:FieldSet runat="server" Layout="ColumnLayout" AutoScroll="false" Height="40" ID="fdAddProduct">
                            <Items>
                                <ext:FieldContainer runat="server"
                                    LabelWidth="120"
                                    FieldLabel="<%$ Resource : PRODUCT_CODE %>"
                                    Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                    <Items>
                                        <ext:TextField runat="server"
                                            Width="150"
                                            ID="txtProduct_System_Code"
                                            TabIndex="11"
                                            SelectOnFocus="true" AllowOnlyWhitespace="false" AllowBlank="false">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13) 
                                                                       { 
                                                                         getProduct(); 
                                                                       }" />
                                            </Listeners>

                                        </ext:TextField>
                                        <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="Button1" TabIndex="12">
                                            <Listeners>
                                                <Click Fn="getProduct" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:TextField runat="server"
                                            MarginSpec="0 0 0 10"
                                            Width="250"
                                            ID="txtAddProduct_Name_Full"
                                            TabIndex="13"
                                            ReadOnly="true" Hidden="false" AllowOnlyWhitespace="false" AllowBlank="false">
                                        </ext:TextField>
                                        <ext:TextField runat="server"
                                            MarginSpec="0 0 0 10"
                                            Width="100"
                                            ID="txtUOM"
                                            TabIndex="13"
                                            ReadOnly="true" Hidden="false" AllowOnlyWhitespace="false" AllowBlank="false">
                                        </ext:TextField>
                                        <ext:Hidden runat="server" ID="hidTrmID" />
                                        <ext:Hidden runat="server" ID="hidAddProduct_System_Code" />
                                        <ext:Hidden runat="server" ID="hidAddProduct_Code" />
                                        <ext:Hidden runat="server" ID="hidAddUomID" />
                                        <ext:Hidden runat="server" ID="hidAddUomName" />
                                        <ext:Hidden runat="server" ID="hidAddUomSKU" />
                                        <ext:Hidden runat="server" ID="hidAddUomHeight" />
                                        <ext:Hidden runat="server" ID="hidAddUomLength" />
                                        <ext:Hidden runat="server" ID="hidAddUomWidth" />
                                        <ext:Hidden runat="server" ID="hidAddUomQty" />
                                        <ext:Hidden runat="server" ID="hidAddBaseQty" />
                                        <ext:Hidden runat="server" ID="hidAddWeight" />
                                        <ext:Hidden runat="server" ID="hidAddPackWeight" />
                                        <ext:Hidden runat="server" ID="hidAddProductOwnerId" />
                                        <ext:Hidden runat="server" ID="hidAddProductStatusId" />
                                        <ext:Hidden runat="server" ID="hidAddPriceUnitId" />
                                        <ext:Hidden runat="server" ID="hidAddPriceUnitName" />
                                        <ext:Hidden runat="server" ID="hidAddPrice" />
                                        <ext:NumberField ID="txtAddQty" runat="server"
                                            FieldLabel="Qty."
                                            LabelWidth="50"
                                            TabIndex="14"
                                            AllowDecimals="true"
                                            MinValue="1"
                                            Width="160"
                                            EnforceMaxLength="true" MaxLength="50" DecimalPrecision="3" AllowOnlyWhitespace="false" AllowBlank="false">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ App.btnAddItem.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:NumberField>
                                        <ext:NumberField ID="txtAddQtyMax" runat="server"
                                            AllowDecimals="true"
                                            MinValue="1"
                                            Width="160"
                                            EnforceMaxLength="true" MaxLength="50" DecimalPrecision="3" Hidden="true">
                                        </ext:NumberField>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server"
                                    LabelWidth="70"
                                    FieldLabel="<%$ Resource : LOTNO %>" Layout="HBoxLayout" MarginSpec="10 0 0 0" Hidden="true">
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server"
                                    LabelWidth="70"
                                    FieldLabel="Status" Layout="HBoxLayout" MarginSpec="10 0 0 0" Hidden="true">
                                </ext:FieldContainer>

                                <ext:Button runat="server"
                                    ID="btnAddItem"
                                    Icon="Add"
                                    Text="<%$ Resource : ADD_NEW %>"
                                    TabIndex="13"
                                    MarginSpec="10 0 0 10" Disabled="true">
                                    <Listeners>
                                        <Click Fn="addProduct" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:FieldSet>
                    </Items>

                    <Listeners>
                        <ValidityChange Handler="#{btnAddItem}.setDisabled(!valid);" />
                    </Listeners>

                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center"
                    Frame="true" Flex="1" Layout="FitLayout">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="TRM_CODE" />
                                        <ext:ModelField Name="Product_ID" />
                                        <ext:ModelField Name="TRM_Product_ID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="TRM_ID" />
                                        <ext:ModelField Name="TransferQty" />
                                        <ext:ModelField Name="PickQty" />
                                        <ext:ModelField Name="ConfirmQty" />
                                        <ext:ModelField Name="PickStatus" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="Location" />
                                        <ext:ModelField Name="TransferUnitID" />
                                        <ext:ModelField Name="TransferBaseUnitID" />
                                        <ext:ModelField Name="TransferBaseQty" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="ProductStatusName" />
                                        <ext:ModelField Name="ProductSubStatusId" />
                                        <ext:ModelField Name="ProductSubStatusName" />
                                        <ext:ModelField Name="ProductOwnerId" />
                                        <ext:ModelField Name="ProductOwnerName" />
                                        <ext:ModelField Name="ProductWidth" />
                                        <ext:ModelField Name="ProductLength" />
                                        <ext:ModelField Name="ProductHeight" />
                                        <ext:ModelField Name="PriceUnitId" />
                                        <ext:ModelField Name="PriceUnitName" />
                                        <ext:ModelField Name="Price" />
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
                                            <ext:Parameter Name="uId" Value="record.data.TransferUnitID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="<%$ Resource : EDIT %>" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.TRM_Product_ID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarConfirm" />
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' MinWidth="100" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' MinWidth="250" />
                            <ext:NumberColumn runat="server" DataIndex="TransferQty" Text="<% $Resource : TRANSFER_QTY %>" Format="#,###.00" Align="Right" MinWidth="150" />
                            <ext:NumberColumn runat="server" DataIndex="PickQty" Text="<% $Resource : PICK_QTY %>" Format="#,###.00" Align="Right" MinWidth="100" />
                            <ext:NumberColumn runat="server" DataIndex="ConfirmQty" Text="<% $Resource : CONFIRM_PICK_QTY %>" Format="#,###.00" Align="Right" MinWidth="150" />
                            <ext:Column runat="server" DataIndex="ProductUnitName" Text='<%$ Resource : UNIT %>' MinWidth="150"></ext:Column>

                        </Columns>
                    </ColumnModel>

                    <%--                    <Plugins>
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

                                        <ext:Button ID="btnPrintCycleCount" runat="server" TabIndex="15" Icon="Report" Text='<%$ Resource : PRINTTRANSFERLIST %>'>
                                            <DirectEvents>
                                                <Click OnEvent="btnTransferlist_Click" Buffer="350">
                                                    <EventMask ShowMask="true" MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnApprove" runat="server" Icon="Accept" Text='<%$ Resource : APPROVING %>' Width="60">
                                            <DirectEvents>
                                                <Click OnEvent="btnApprove_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Approve?" Title="Save" />
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnAssign" runat="server" Icon="Accept" Text='<%$ Resource : ASSIGN %>' Width="60">
                                            <DirectEvents>
                                                <Click OnEvent="btnAssign_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Approve?" Title="Save" />
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

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

                                        <ext:Button ID="btnWinExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="9">
                                            <DirectEvents>
                                                <Click OnEvent="btnExit_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <uc:ucTransferProductSelect runat="server" ID="ucTransferProductSelect" />
    </form>
</body>
</html>
