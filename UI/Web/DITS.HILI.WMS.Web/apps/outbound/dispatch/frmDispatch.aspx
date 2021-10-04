<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDispatch.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.dispatch.frmDispatch" %>

<%@ Register Src="~/apps/outbound/dispatch/_usercontrol/ucProductDispatchSelect.ascx" TagPrefix="uc1" TagName="ucProductDispatchSelect" %>



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
            var deleteProduct = function(productcode,unit) {
                var grid = #{GridDispatch}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(  grid.store.data.items[i].data.StockUnitId == unit)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 

            var deleterevise = function(bookingid) {
                var grid = #{GridDispatch}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.BookingId == bookingid)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 
             
            var Combox = {
                getObj: function (combobox) {
                    var v = combobox.getValue();
                    var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                    return recordValue;
                } , aaa: function(aaa){return "";}
            }

            var addProduct = function () {
                 

                var grid = #{GridDispatch};
                var btnAddItem = #{btnAddItem};
                var freeRecord = -1;

                if(#{btnAddItem}.disabled == true)
                    return;

   
                
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
                                    ProductId: App.hidAddProduct_System_Code.getValue(),
                                    ProductCode: App.hidAddProduct_Code.getValue(),
                                    ProductName: App.txtAddProduct_Name_Full.getValue(),
                                    StockUnitId :  App.hidAddUomID.getValue(),
                                    StockUnitName : App.hidAddUomName.getValue(),
                                    Quantity: App.txtAddQty.getValue() == null ? 0 : App.txtAddQty.getValue(),
                                    RuleName : App.hidSpecial_Rul_Name.getValue(),
                                    RuleId : App.hidSpecial_Rul_ID.getValue(),
                                    ProductStatusId : App.hidProduct_Status_Code.getValue(),
                                    ProductStatusName : App.hidProduct_Status_Name.getValue(),
                                    ProductSubStatusId : App.hidProduct_Sub_Status_Code.getValue(),
                                    ProductSubStatusName : App.hidProduct_Sub_Status_Name.getValue(),
                                    DispatchDetailProductHeight : App.hidAddUomHeight.getValue(),
                                    DispatchDetailProductLength : App.hidAddUomLength.getValue(),
                                    DispatchDetailProductWidth : App.hidAddUomWidth.getValue(),
                                    BaseUnitId : App.hidAddUomSKU.getValue(),
                                    BaseQuantity : App.hidAddBaseQty.getValue(),
                                    ConversionQty : App.hidAddUomQty.getValue(),
                                    ProductOwnerId : App.hidAddProductOwnerId.getValue(),
                                    DispatchPriceUnitId : App.hidAddPriceUnitId.getValue(),
                                    DispatchPriceUnitName : App.hidAddPriceUnitName.getValue(),
                                    DispatchPrice : App.hidAddPrice.getValue()
                                });  
                            
                            }

                           


                            //DispatchDetailStatusId :  App.hidProduct_Status_Code.getValue(),
                            //DispatchDetailStatusName :  App.hidProduct_Status_Name.getValue(),

                            
                            sumTotal(); 

                            Ext.getCmp('cmbDispatchType').readOnly = true;
                            Ext.getCmp('chkIsBackorder').readOnly= true;
                            

                            App.hidAddProduct_System_Code.reset();
                            App.hidAddProduct_Code.reset();
                            App.txtProduct_System_Code.reset();
                             
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
                            App.btnBook.setDisabled(1);
                                                     
                        }
                          
                    }
                });
                        
            };
              
            function resetcontrol(){
                App.hidAddProduct_System_Code.reset();
                App.hidAddProduct_Code.reset();
                App.txtProduct_System_Code.reset();
                             
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
            }
            var removeProduct = function () {
                var grid = #{GridDispatch},
                    sm = grid.getSelectionModel();

                grid.store.remove(sm.getSelection());
                if (grid.store.getCount() > 0) {
                    sm.select(0);
                    sumTotal();
                }
                else
                {
                    Ext.getCmp('cmbDispatchType').readOnly = false;
                    Ext.getCmp('chkIsBackorder').readOnly= false;
                }
                grid.editingPlugin.cancelEdit();

                sumTotal();
            };

            var validateSave = function () {  
                var plugin = this.editingPlugin;             
                plugin.completeEdit();
                App.btnBook.setDisabled(1);
                sumTotal();
            };

    
            var beforeEditCheck = function(editor, e, eOpts){
                var btnSave = #{btnSave};        
                var _status = #{txtDispatchStatusId}.getValue();

                console.log('b:' + _status);

                if(_status == ''){
                    _status = 10;
                }
                console.log('a:' + _status);

                if(_status >= 20){
                    e.cancel = true;
                    return;
                }
                App.direct.SetDefault();
           
            };


            var type_change = function(){ 
               

                if(#{hidIsMarketing}.getValue() =="1")
                {
                    Ext.getCmp('cmbWarehouseFrom').allowBlank = false;
                    Ext.getCmp('cmbWarehouseTo').allowBlank = false;
                    Ext.getCmp('cmbWarehouseTo').disabled = true;
                }
                else{
                    Ext.getCmp('cmbWarehouseFrom').allowBlank = true;
                    Ext.getCmp('cmbWarehouseTo').allowBlank = true;
                    Ext.getCmp('cmbWarehouseTo').disabled = false;
                }  
                 
            }; 
             
            var Dispatch_Type_Selected = function(){
                App.direct.DispatchTypeCombo()
            }
            var ShipTo_Selected = function(){
                App.direct.ShipToCombo()
            }
   
           
            var customer_select = function(){
                #{GridDispatch}.getStore().removeAll();
                #{GridDispatch}.getStore().sync();


            }
                    
            var sumData = function(){
                var qty = #{txtDispatchQty}.getValue();
                var NW = #{txtWeight}.getValue();
                var GW = #{txtPackageWeight}.getValue();  
           
                         
                var totleNW = (qty * NW);
                #{txtNetWeight}.setValue(totleNW);
                                    
                var totleGW = (qty * (GW + NW));
                #{txtGrossWeight}.setValue(totleGW);
            };
 

            var ConfirmCloseJob = function(btn, text){
                if(btn=='ok')
                {
                    App.direct.CloseJob(text);
                }
            };

            var sumTotal = function(){

                var grid = #{GridDispatch};
                var gridCount = grid.store.getCount();

                var sumNet = 0; 
                var sumGross = 0;
                var sumQTY = 0;
               
                for(i=0;i<gridCount;i++)
                {                
                    var store_dispatch = grid.store.data.items[i].data; 
                    
                    var product_quantity =  parseFloat(store_dispatch.Quantity);
                    // var product_uom_weight =  parseFloat(store_dispatch.Product_UOM_Weight)
                    // var product_uom_package_weight =  parseFloat(store_dispatch.Product_UOM_Package_Weight)

                    //sumNet += product_quantity * product_uom_weight;
                    //sumGross += (product_uom_weight + product_uom_package_weight) * product_quantity;
                    sumQTY += product_quantity;
 
                }            
 

                 #{txttotalNetWeight}.setValue(parseFloat(sumNet).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                 #{txttotalGrossWeight}.setValue(parseFloat(sumGross).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")); 
                 #{txttotalQTY}.setValue(parseFloat(sumQTY).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

            };
         
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

            var ShippingTo_Selete = function() {
                
                //var recordValue =  Combox.getObj(this);
                
                //#{hidSpecial_Rul_Code}.setValue(recordValue.data.Special_Rul_Code);
                //#{hidSpecial_Rul_Name}.setValue(recordValue.data.Special_Rul_Name);
                //#{hidSpecial_Rul_ID}.setValue(recordValue.data.Special_Rul_Id);
                //console.log('Special_Rul_Code = ' + recordValue.data.Special_Rul_Code);
            }

            var checkisrevice = function(){
                var _check =  #{ckbRevisePoNo}.getValue();
    
                if(_check){
                    App.txtRevisePoNo.focus(true, 100);
                     #{txtRevisePoNo}.show();
                }else
                {
                    App.direct.txtRevisePoNo_Clear();        
                     #{txtRevisePoNo}.hide();
                } 
            }

            var EnterPressHandler = function (field, e) {
             
                App.direct.txtRevisePoNo_Change();
           
            }

            var IsBackOrdered = function (value, metadata, record) {
                if (value == true) {
                    if (record.data.IsBackOrder)
                        return "<img src='../../../images/bo.png' width='15px'/>";
                    else
                        return "<img src='../../../images/tick.png' width='15px'/>";
                }
            }

            var RuleRenderer = function (value, metadata, record) {
                var r = App.StoreComboRule.getById(value);
                if (Ext.isEmpty(r)) {
                    if(record != null){
                        return  record.data.RuleName;
                    }else{
                        return "";
                    }
                   
                }        
                return r.data.RuleName;
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


            var showResultText = function (btn, text) {


                if(btn == "ok"){
                    if(text == ""){
                        //Ext.MessageBox.show({
                        //    title:'เตือน',
                        //    msg:  'กรุณากรอกเหตุผลในการยกเลิก',
                        //    buttons: Ext.MessageBox.OK,
                        //    icon: Ext.MessageBox.WARNING
                        //});
                        App.direct.DoCancelDispatchConfirm();
 
                                
                        return;
                    }

                    App.direct.DoYesCancelDispatch(text);
                }else{
                    App.direct.DoNoCancelDispatch();
                }
            };

        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Label runat="server" ID="lbMsgError" Text="<%$ Resource : ERROR %>" Hidden="true" />
        <ext:Label runat="server" ID="lbProductOwnerCode" Text="" Hidden="true" />





        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel runat="server" ID="FormPanelDetail"
                    BodyPadding="5" Region="North" Frame="true" MaxHeight="500" AutoScroll="false"
                    Margins="3 3 0 3" Layout="AnchorLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" Text="<%$ Resource : DISPATCH %>" Icon="Accept" ID="btnLinkDispatch" Disabled="true" />
                                <ext:Label runat="server" Text=" >> " ID="btnLinkDispatchNext" Hidden="true" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:FieldSet runat="server" Title="<%$ Resource : DISPATCH_INFO %>" Layout="AnchorLayout" AutoScroll="false"
                            Collapsible="true"
                            Collapsed="false">
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" MarginSpec="10 0 0 0">
                                    <Items>
                                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33" AnchorHorizontal="100%">
                                            <Items>
                                                <ext:FieldContainer runat="server" ID="fctRevisePo" FieldLabel="<%$ Resource : REVISE_PO %>"
                                                    Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:Checkbox ID="ckbRevisePoNo" runat="server">
                                                            <%--                      <DirectEvents>
                                                                <Change OnEvent="ckbRevisePoNo_Event"></Change>
                                                            </DirectEvents>--%>
                                                            <Listeners>
                                                                <Change Fn="checkisrevice"></Change>
                                                            </Listeners>
                                                        </ext:Checkbox>
                                                        <ext:TextField ID="txtRevisePoNo" runat="server" TabIndex="1"
                                                            MaxLength="50" EnforceMaxLength="true" LabelAlign="Left" LabelWidth="50" FieldLabel="<%$ Resource : PONO %>" Width="170" Hidden="true">
                                                            <Listeners>
                                                                <SpecialKey Handler="if (e.getKey() === e.ENTER) {
                                                                         EnterPressHandler();
                                                                     }" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:Hidden ID="txtRevisePonoId" runat="server"></ext:Hidden>

                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : DISPATCH_CODE %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1">
                                                    <Items>
                                                        <ext:Hidden ID="txtTemp_System_Product_Code" runat="server" />
                                                        <ext:Hidden ID="txtDispatchStatusId" runat="server" />
                                                        <ext:Hidden ID="txtDispatchStatus" runat="server" />
                                                        <ext:Hidden ID="txtDispatch_Code_Key" runat="server" />
                                                        <ext:Hidden ID="hidDispatchConfig" runat="server" />

                                                        <ext:TextField ID="txtDispatch_Code" runat="server" TabIndex="1" ReadOnly="true"
                                                            MaxLength="50" EnforceMaxLength="true" Flex="1" />

                                                        <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="btnSelectList" Hidden="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnSelectList_Click" />
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : DISPATCH_TYPE %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1">
                                                    <Items>
                                            <ext:ComboBox ID="cmbDispatchTypeAll" runat="server" TabIndex="1" Editable="true"
                                                            DisplayField="FullName" ValueField="DocumentTypeID"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>" AutoFocus="true"
                                                            PageSize="25" MinChars="0" Flex="1" AllowBlank="true"
                                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                                            ForceSelection="true" AllowOnlyWhitespace="true" Hidden="true">
                                                            <ListConfig LoadingText="Searching..." ID="ListComboDispatchTypeAll" MinWidth="250">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
							                                              {FullName} 
						                                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreDispatch_Type_All" runat="server" AutoLoad="true">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=DispatchTypeWithAll">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model3" runat="server">
                                                                            <Fields>
                                                                                 <ext:ModelField Name="Code" />
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="DocumentTypeID" />
                                                                                <ext:ModelField Name="FullName" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <Select Fn="Dispatch_Type_Selected" />
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{cmbSubCust_Code}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="cmbDispatchType" runat="server" TabIndex="1" Editable="true"
                                                            DisplayField="FullName" ValueField="DocumentTypeID"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>" AutoFocus="true"
                                                            PageSize="25" MinChars="0" Flex="1" AllowBlank="false"
                                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                                            ForceSelection="true" AllowOnlyWhitespace="false">
                                                            <ListConfig LoadingText="Searching..." ID="ListComboDispatchType" MinWidth="250">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
							                                              {FullName} 
						                                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreDispatch_Type" runat="server" AutoLoad="true">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=DispatchType">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model1" runat="server">
                                                                            <Fields>
                                                                                 <ext:ModelField Name="Code" />
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="DocumentTypeID" />
                                                                                <ext:ModelField Name="FullName" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <Select Fn="Dispatch_Type_Selected" />
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{cmbSubCust_Code}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>

                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : CUSTOMER %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1">
                                                    <Items>
                                                        <ext:ComboBox ID="cmbSubCust_Code" runat="server" AllowBlank="false"
                                                            DisplayField="FullName"
                                                            ValueField="ContactID"
                                                            TriggerAction="All"
                                                            SelectOnFocus="true"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                            TypeAhead="true"
                                                            PageSize="25"
                                                            MinChars="0"
                                                            TabIndex="2"
                                                            Editable="true"
                                                            MatchFieldWidth="false" AllowOnlyWhitespace="false" Flex="1">
                                                            <ListConfig LoadingText="Searching..." ID="ListComboSubCustomer">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
							                                            {FullName} <br>
                                                          
						                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreSubCust_Code" runat="server" AutoLoad="false">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Customer">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model6" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="Code" />
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="ContactID" />
                                                                                <ext:ModelField Name="FullName" />
                                                                              
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>

                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtDispatch_Remark}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>

                                                        <ext:Button runat="server" ID="btnSubCust_Code" Icon="Add" Margins="0 0 0 5" ToolTip="Add Customer" Hidden="true">
                                                            <DirectEvents>

                                                                <Click OnEvent="btnSubCust_Code_Click" />
                                                            </DirectEvents>
                                                        </ext:Button>

                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : REMARK %>" Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:TextField ID="txtDispatch_Remark" runat="server" TabIndex="9"
                                                            EnforceMaxLength="true" Flex="1" MaxLength="150">
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDispatch_Date_Order}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                    </Items>
                                                </ext:FieldContainer>


                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33" AnchorHorizontal="100%">
                                            <Items>

                                                <ext:FieldContainer runat="server" FieldLabel="<% $Resource: ESTDISPATCH_DATE%>" Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:DateField ID="dtDispatch_Date_Order" runat="server"
                                                            TabIndex="3" Flex="1" Format="dd/MM/yyyy"
                                                            LabelAlign="Right" AllowBlank="false" AllowOnlyWhitespace="false">
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDocumentDate}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<% $Resource : DOCUMENT_DATE%>" Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:DateField ID="dtDocumentDate" runat="server"
                                                            TabIndex="3" Flex="1" Format="dd/MM/yyyy"
                                                            LabelAlign="Right" AllowBlank="false" AllowOnlyWhitespace="false">
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{cmbShippingTo}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<% $Resource : SHIPPING_TO%>" Layout="HBoxLayout" LabelWidth="120" Flex="1">
                                                    <Items>
                                                        <ext:Hidden runat="server" ID="hidSpecial_Rul_Code" />
                                                        <ext:Hidden runat="server" ID="hidSpecial_Rul_Name" />
                                                        <ext:Hidden runat="server" ID="hidSpecial_Rul_ID" />
                                                        <ext:Hidden runat="server" ID="hidIsMarketing" />

                                                        <%--      <ext:ComboBox ID="cmbShippingTo" runat="server" AllowBlank="false"
                                                            DisplayField="Name"
                                                            ValueField="ShiptoID"
                                                            TriggerAction="All"
                                                            SelectOnFocus="true"
                                                            EmptyText="<%$Resource : PLEASE_SELECT%>"
                                                            TypeAhead="true"
                                                            PageSize="25"
                                                            MinChars="0"
                                                            TabIndex="2"
                                                            Editable="true"
                                                            MatchFieldWidth="false" Flex="1">
                                                            <ListConfig LoadingText="Searching..." ID="ListcmbShippingTo">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
                                                                        {Name} 					                                
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store runat="server" ID="StoreShipTo" AutoLoad="false">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="ShiptoID" />
                                                                                <ext:ModelField Name="Name" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                             
                                                        </ext:ComboBox>--%>


                                                        <ext:ComboBox ID="cmbShippingTo" runat="server" TabIndex="1" Editable="true"
                                                            DisplayField="Name" ValueField="ShipToId"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                            PageSize="25" MinChars="0" Flex="1" AllowBlank="false"
                                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                                            ForceSelection="true" AllowOnlyWhitespace="false">
                                                            <ListConfig LoadingText="Searching..." ID="ListcmbShippingTo">
                                                                <ItemTpl runat="server" AllowOnlyWhitespace="false">
                                                                    <Html>
                                                                        <div class="search-item">
							                                               {Name} 
						                                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreShipTo" runat="server" AutoLoad="false">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipTo">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model7" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="ShipToId" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <Select Fn="ShipTo_Selected" />
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDeliveryDate}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : FROM_WARHOUSE %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1" Hidden="true">
                                                    <Items>
                                                        <ext:ComboBox ID="cmbWarehouseFrom" runat="server" TabIndex="1" Editable="true"
                                                            DisplayField="Name" ValueField="WarehouseID"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                            PageSize="0" MinChars="0" Flex="1"
                                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                                            AllowOnlyWhitespace="true">
                                                            <ListConfig LoadingText="Searching..." ID="ListcmbWarehouseFrom">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
							                                               {Name} 
						                                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreWarehouseFrom" runat="server" AutoLoad="true">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model2" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="WarehouseID" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDeliveryDate}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>

                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : URGENT %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1" TabIndex="10">
                                                    <Items>
                                                        <ext:Checkbox ID="chkIsUrgent" runat="server"></ext:Checkbox>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="Dispatch Status" Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="lblDispatchStatus" Flex="1" ReadOnly="true" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33" AnchorHorizontal="100%">
                                            <Items>

                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : DELIVERY_DATE %>" Layout="HBoxLayout" LabelWidth="120">
                                                    <Items>
                                                        <ext:DateField ID="dtDeliveryDate" runat="server"
                                                            TabIndex="3" Flex="1" Format="dd/MM/yyyy"
                                                            LabelAlign="Right" AllowBlank="false" AllowOnlyWhitespace="false">
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtDispatch_Refered_1}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : PONO %>"
                                                    Layout="HBoxLayout" MaxLength="50" EnforceMaxLength="true" LabelWidth="120" Flex="1" TabIndex="6">
                                                    <Items>
                                                        <ext:TextField runat="server" Flex="1" ID="txtDispatch_Refered_1" MaxLength="17" EnforceMaxLength="true" TabIndex="6" AllowBlank="false" AllowOnlyWhitespace="false">

                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtDispatch_Refered_2}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ORDER_NO %>"
                                                    Layout="HBoxLayout" MaxLength="50" EnforceMaxLength="true" LabelWidth="120" Flex="1" TabIndex="7">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtDispatch_Refered_2"
                                                            TabIndex="7" Flex="1" MaxLength="50" EnforceMaxLength="true" LabelAlign="Right">
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtProduct_System_Code}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:TextField>

                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : TO_WARHOUSE %>"
                                                    Layout="HBoxLayout" MaxLength="50" EnforceMaxLength="true" LabelWidth="120" Flex="1" TabIndex="7" Hidden="true">
                                                    <Items>
                                                        <ext:ComboBox ID="cmbWarehouseTo" runat="server" TabIndex="1" Editable="true"
                                                            DisplayField="Name" ValueField="WarehouseID"
                                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                            PageSize="0" MinChars="0" Flex="1" AllowOnlyWhitespace="true"
                                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false">
                                                            <ListConfig LoadingText="Searching..." ID="ListcmbWarehouseFTo">
                                                                <ItemTpl runat="server">
                                                                    <Html>
                                                                        <div class="search-item">
							                                               {Name}
						                                                </div>
                                                                    </Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                            <Store>
                                                                <ext:Store ID="StoreWarehouseTo" runat="server" AutoLoad="true">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model5" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="Name" />
                                                                                <ext:ModelField Name="WarehouseID" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <Listeners>
                                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtProduct_System_Code}.focus(false, 100);}" />
                                                            </Listeners>
                                                        </ext:ComboBox>


                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : BACKORDER %>"
                                                    Layout="HBoxLayout" LabelWidth="120" Flex="1" TabIndex="8">
                                                    <Items>
                                                        <ext:Checkbox ID="chkIsBackorder" runat="server" Disabled="true"></ext:Checkbox>
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>

                                <ext:Hidden runat="server" ID="hidProduct_Status_Code" />
                                <ext:Hidden runat="server" ID="hidProduct_Status_Name" />

                                <ext:Hidden runat="server" ID="hidProduct_Sub_Status_Code" />
                                <ext:Hidden runat="server" ID="hidProduct_Sub_Status_Name" />
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
                                                <SpecialKey Handler="
                                                                            if(e.getKey() == 13){
                                                                                   getProduct();
                                                                            }" />
                                            </Listeners>

                                        </ext:TextField>
                                        <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="btnProductSelect" TabIndex="12">
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

                <ext:GridPanel ID="GridDispatch" runat="server" Margins="3 3 0 3"
                    Region="Center" Frame="true" SortableColumns="true" Disabled="false" TabIndex="15">

                    <Store>
                        <ext:Store ID="StoreDispatch" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DispatchDetailId" />
                                        <ext:ModelField Name="Sequence" />
                                        <ext:ModelField Name="ProductId" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductLot" />
                                        <ext:ModelField Name="LocationCode" />
                                        <ext:ModelField Name="StockUnitId" />
                                        <ext:ModelField Name="StockUnitName" />
                                        <ext:ModelField Name="Quantity" />
                                        <ext:ModelField Name="BaseQuantity" />
                                        <ext:ModelField Name="BaseUnitId" />
                                        <ext:ModelField Name="BaseUnitName" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="ProductOwnerId" />
                                        <ext:ModelField Name="ProductOwnerName" />
                                        <ext:ModelField Name="DispatchDetailProductWidth" />
                                        <ext:ModelField Name="DispatchDetailProductLength" />
                                        <ext:ModelField Name="DispatchDetailProductHeight" />
                                        <ext:ModelField Name="DispatchPriceUnitId" />
                                        <ext:ModelField Name="DispatchPriceUnitName" />
                                        <ext:ModelField Name="DispatchPrice" />
                                        <ext:ModelField Name="DispatchDetailStatusId" />
                                        <ext:ModelField Name="DispatchDetailStatusName" />
                                        <ext:ModelField Name="Remark" />
                                        <ext:ModelField Name="IsActive" />
                                        <ext:ModelField Name="RuleId" />
                                        <ext:ModelField Name="RuleName" />
                                        <ext:ModelField Name="ProductStatusId" />
                                        <ext:ModelField Name="ProductStatusName" />
                                        <ext:ModelField Name="ProductSubStatusId" />
                                        <ext:ModelField Name="ProductSubStatusName" />
                                        <ext:ModelField Name="ProductOwnerId" />
                                        <ext:ModelField Name="IsBackOrder" />
                                        <ext:ModelField Name="PickLocationId" />
                                        <ext:ModelField Name="PickLocationCode" />
                                        <ext:ModelField Name="PickProductLot" />
                                        <ext:ModelField Name="PickQTY" />
                                        <ext:ModelField Name="PickQTYUnitId" />
                                        <ext:ModelField Name="PickQTYUnitName" />
                                        <ext:ModelField Name="PickBaseQTY" />
                                        <ext:ModelField Name="ConsolidateQTY" />
                                        <ext:ModelField Name="ConsolidateBaseQTY" />
                                        <ext:ModelField Name="ConsolidateQTYUnitId" />
                                        <ext:ModelField Name="ConsolidateQTYUnitName" />
                                        <ext:ModelField Name="DeliveryId" />
                                        <ext:ModelField Name="BookingId" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="PickPalletCode" />
                                        <ext:ModelField Name="OrderNo" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColMoDispatch" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Dispatch" CommandName="Delete" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ProductCode" Mode="Raw" />
                                            <ext:Parameter Name="unitId" Value="record.data.StockUnitId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Width="30" Hidden="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Zoom" ToolTip-Text="<%$ Resource : EDIT %>" CommandName="Edit" />
                                </Commands>
                                <%--<DirectEvents>
                                    <Command OnEvent="EditBooking_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="DispatchDetailId" Value="record.data.DispatchDetailId " Mode="Raw" />

                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>--%>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colReviseNo" Sortable="false" Align="Center" Width="30" Hidden="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Revise" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick" >
                                        
                                       <EventMask ShowMask="true" Msg="Delete booking ..." MinDelay="100" />
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.BookingId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='You sure is delete this booking !' Title='Delete' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="60" Align="Center" />
                            <ext:Column ID="colIsBackOrder" runat="server" Text="<%$ Resource : BACKORDER %>" DataIndex="IsBackOrder" Align="Center" Width="70">
                                <Renderer Fn="IsBackOrdered">
                                </Renderer>
                            </ext:Column>
                            <ext:Column ID="Column1" runat="server" DataIndex="BookingId" Text="BookingId" Width="100" Align="Center" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="Column12" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Width="100" Align="Center" Hidden="true">
                            </ext:Column>

                            <ext:Column ID="colProductCode" runat="server" DataIndex="ProductCode"
                                Text="<%$ Resource : PRODUCT_CODE %>" Width="150" Align="Left">
                            </ext:Column>

                            <ext:Column ID="ColProduct_Name_Full" runat="server" DataIndex="ProductName"
                                Text="<%$ Resource : PRODUCT_NAME %>" Width="150" Align="Left">
                            </ext:Column>
                            <ext:Column runat="server" ID="ColRuleEdit" Text="<%$ Resource : SPECIAL_RULE %>" DataIndex="RuleId" Width="150" Hidden="false">
                                <Renderer Fn="RuleRenderer" />
                                <Editor>
                                    <ext:ComboBox
                                        runat="server"
                                        QueryMode="Local"
                                        Editable="false"
                                        StoreID="StoreComboRule"
                                        DisplayField="RuleName"
                                        ValueField="RuleId">
                                        <%--     <Store>
                                            <ext:Store ID="StoreComboRule" runat="server">
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="RuleId">
                                                        <Fields>
                                                            <ext:ModelField Name="RuleId" />
                                                            <ext:ModelField Name="RuleName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>--%>
                                    </ext:ComboBox>
                                </Editor>


                            </ext:Column>
                            <ext:Column ID="Column13" runat="server" DataIndex="RuleId" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="hidSpescial_Rul_Code" runat="server" DataIndex="RuleName" Hidden="true">
                            </ext:Column>

                            <ext:Column ID="ColRuleReadOnly" runat="server" DataIndex="RuleName" Text="<%$ Resource : SPECIAL_RULE %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>

                            <ext:Column ID="colProductLot" runat="server" DataIndex="ProductLot"
                                Text="<%$ Resource : LOTNO %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>

                            <ext:Column ID="colLocationCode" runat="server" DataIndex="LocationCode" Text="<%$ Resource : LOCATION_NO %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="colPalletCode" runat="server" DataIndex="PalletCode" Text="<%$Resource : PALLETCODE%>" Width="100" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="colOrderNo" runat="server" DataIndex="OrderNo" Text="<%$Resource : ORDERNO%>" Width="100" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:NumberColumn ID="colBookingQTY" runat="server" DataIndex="Quantity"
                                Text="<% $Resource : BOOKING_QUANTITY%>" Width="100" Align="Right" Format="#,###.00" Hidden="true">
                            </ext:NumberColumn>

                            <ext:NumberColumn ID="colDispatch_Quantity" runat="server" DataIndex="Quantity"
                                Text="<% $Resource : DISPATCH_QUANTITY%>" Width="100" Align="Right" Format="#,###.00">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true"
                                        ID="txtDispatchQty" AllowDecimals="false" MinValue="1">
                                        <Listeners>

                                            <Blur Fn="sumData" />
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>

                            <ext:Column ID="colUnit" runat="server" DataIndex="StockUnitName" Text="<%$Resource : UNIT%>" Width="100" Align="Left" Hidden="false">
                            </ext:Column>


                            <ext:Column ID="colProductStatus" runat="server" DataIndex="ProductStatusName" Text="<%$Resource : STATUS%>" Width="150" Align="Left" Hidden="false">
                            </ext:Column>

                            <ext:Column ID="colRemark" runat="server" DataIndex="Remark" Text="<%$Resource : REMARK%>" Width="130" Align="Left">
                                <Editor>
                                    <ext:TextArea runat="server" ID="txtAreaRemark" LabelPad="10" Align="Left" />
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="colPickProductLot" runat="server" DataIndex="PickProductLot" Text="<%$ Resource : LOTNO %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="colPickLocationCode" runat="server" DataIndex="PickLocationCode" Text="<%$ Resource : LOCPICK %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:Column ID="colPickPalletCode" runat="server" DataIndex="PickPalletCode" Text="<%$ Resource : PALLETCODE %>" Width="140" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:NumberColumn ID="colPickQTY" runat="server" DataIndex="PickQTY"
                                Text="<% $Resource : PICKQTY%>" Width="100" Align="Right" Format="#,###.00" Hidden="true">
                            </ext:NumberColumn>
                            <ext:Column ID="colPICKUNIT" runat="server" DataIndex="PickQTYUnitName" Text="<%$Resource : PICKUNIT%>" Width="100" Align="Left" Hidden="true">
                            </ext:Column>
                            <ext:NumberColumn ID="colConsolidateQTY" runat="server" DataIndex="ConsolidateQTY"
                                Text="<% $Resource : CONSOLIDATEQTY%>" Width="100" Align="Right" Format="#,###.00" Hidden="true">
                            </ext:NumberColumn>
                            <ext:Column ID="colConsolidateQTYUnitName" runat="server" DataIndex="ConsolidateQTYUnitName" Text="<%$Resource : CONSOLIDATEUNIT%>" Width="100" Align="Left" Hidden="true">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:RowEditing runat="server" ID="pluginEditDispatch" Visible="true" AutoCancel="true" SaveHandler="validateSave" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbar1" Layout="AnchorLayout">
                            <Items>

                                <ext:StatusBar runat="server" ID="toolbar2" PaddingSpec="5 15 5 0">
                                    <Items>
                                        <ext:TextField ID="txttotalQTY" LabelAlign="Right" FieldLabel="<%$Resource : TOTALQTY%>" runat="server" Width="200" ReadOnly="true" FieldStyle="text-align: right" />
                                        <%--<ext:Label runat="server" Text="<%$Resources:Langauge, kg%>" MarginSpec="0 0 0 5" />--%>

                                        <ext:TextField ID="txttotalNetWeight" LabelAlign="Right" FieldLabel="<%$Resource : TOTALNW%>" runat="server" Width="200" ReadOnly="true" FieldStyle="text-align: right" Hidden="true" />
                                        <ext:Label runat="server" Text="<%$Resource : KG%>" MarginSpec="0 0 0 5" Hidden="true" />
                                        <ext:TextField ID="txttotalGrossWeight" LabelAlign="Right" MarginSpec="0 0 0 10" FieldLabel="<%$Resource : TOTALGW%>" runat="server" Width="200" ReadOnly="true" FieldStyle="text-align: right" Hidden="true" />
                                        <ext:Label runat="server" Text="<%$Resource : KG%>" MarginSpec="0 0 0 5" Hidden="true" />


                                    </Items>
                                </ext:StatusBar>

                                <ext:Toolbar runat="server" ID="toolbarControls">
                                    <Items>
                                       <ext:Button runat="server" ID="btnAddPallet" Icon="Add" Text="<%$ Resource : ADD_PALLET %>" Hidden="true">
                                             <DirectEvents>
                                                <Click OnEvent="btnAddPallet_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button runat="server" ID="btnRemove" Icon="Cross" Text="<%$Resource: DELETE%>" TabIndex="16" Hidden="true">
                                            <Listeners>
                                                <Click Fn="removeProduct" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:TextField ID="txtReason" FieldLabel="<%$Resource : REASON%>" runat="server" LabelAlign="Right" Hidden="true" Flex="1" ReadOnly="true" LabelWidth="120" />

                                        <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$Resource : CLEAR%>" Width="60" TabIndex="21">
                                            <DirectEvents>
                                                <Click OnEvent="btnClear_Click" />
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:ToolbarFill ID="TbarFill" runat="server" />
                                        <ext:Button ID="btnPrint" runat="server" Icon="Printer"
                                            Text="<%$Resource : PRINT%>" Width="60" TabIndex="17">
                                            <DirectEvents>
                                                <Click OnEvent="btnPrint_Click">
                                                    <EventMask ShowMask="true" Msg="Print ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:DateField ID="dtApproveDispatch" runat="server"
                                            Width="200" Format="dd/MM/yyyy"
                                            LabelAlign="Right" AllowBlank="false" AllowOnlyWhitespace="false" FieldLabel="<%$Resource : APPROVE_DATE%>" Hidden="true">
                                        </ext:DateField>
                                        <ext:Button ID="btnApproveDispatch" runat="server" Icon="Disk"
                                            Text="<%$Resource : APPROVE_DISPATCH%>" Width="120" TabIndex="17">
                                            <DirectEvents>
                                                <Click OnEvent="btnApproveDispatch_Click">
                                                    <EventMask ShowMask="true" Msg="Approve Dispatch ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Approve Dispatch?" Title="Approve Dispatch" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnApproveDispatchInternal" runat="server" Icon="Disk"
                                            Text="<%$Resource : APPROVE_DISPATCH%>" Width="120" TabIndex="17" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnApproveDispatchInternal_Click">
                                                    <EventMask ShowMask="true" Msg="Approve Dispatch Internal ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Approve Dispatch Internal?" Title="Approve Dispatch Internal" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnApproveDispatchPicking" runat="server" Icon="Disk"
                                            Text="<%$Resource : APPROVE_DISPATCH%>" Width="120" TabIndex="17" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnApproveDispatchPicking_Click">
                                                    <EventMask ShowMask="true" Msg="Approve Dispatch From Pick ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Approve Dispatch From Pick?" Title="Approve Dispatch From Pick" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnApproveDispatchQA" runat="server" Icon="Disk"
                                            Text="<%$Resource : APPROVE_DISPATCH%>" Width="120" TabIndex="17" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnApproveDispatchQA_Click">
                                                    <EventMask ShowMask="true" Msg="Approve Dispatch From QA ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Approve Dispatch From QA?" Title="Approve Dispatch From QA" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnCancelDispatch" runat="server" Icon="Delete"
                                            Text="<%$Resource : CANCEL_DISPATCH%>" Width="120" TabIndex="18" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancelDispatch_Click">
                                                    <EventMask ShowMask="true" Msg="Cancel Dispatch ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Cancel Dispatch?" Title="Approve Dispatch" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnCancelDispatchInternal" runat="server" Icon="Delete"
                                            Text="<%$Resource : CANCEL_DISPATCH%>" Width="120" TabIndex="18" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancelDispatchInternal_Click">
                                                    <EventMask ShowMask="true" Msg="Cancel Dispatch Internal ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Cancel Dispatch Internal?" Title="Approve Dispatch Internal" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnCancelDispatchPicking" runat="server" Icon="Delete"
                                            Text="<%$Resource : CANCEL_DISPATCH%>" Width="120" TabIndex="18" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancelDispatchPicking_Click">
                                                    <EventMask ShowMask="true" Msg="Cancel Dispatch Picking ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Cancel Dispatch Picking?" Title="Approve Dispatch Picking" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnApprove" runat="server" Icon="Disk"
                                            Text="<%$Resource : APPROVE_BOOKING%>" Width="120" TabIndex="17">
                                            <DirectEvents>
                                                <Click OnEvent="btnApprove_Click">
                                                    <EventMask ShowMask="true" Msg="Approve Booking ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Approve Booking?" Title="Approve Booking" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnBook" runat="server" Icon="BookAdd"
                                            Text="<%$Resource : BOOKING%>" Width="80" TabIndex="18">
                                            <DirectEvents>
                                                <Click OnEvent="btnBooking_Click">
                                                    <EventMask ShowMask="true" Msg="Booking ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Booking Dispatch?" Title="Booking" ConfirmRequest="true" />
                                                    <%-- <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePagesBook" Mode="Raw" Value="Ext.encode(#{GridDispatch}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>--%>
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnCancelAll" runat="server" Icon="Delete"
                                            Text="<%$Resource : CANCEL_DISPATCH%>" Width="120" TabIndex="18" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancelAll_Click">
                                                    <EventMask ShowMask="true" Msg="Cancel ..." MinDelay="100" />
                                                    <%--<Confirmation Message="Do you want to Cancel Dispatch?" Title="Cancel Dispatch" ConfirmRequest="true" />--%>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnCancel" runat="server" Icon="Delete"
                                            Text="<%$Resource : CANCEL%>" Width="90" TabIndex="18">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancel_Click">
                                                    <EventMask ShowMask="true" Msg="Booking ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Cancel Booking?" Title="Cancel Booking" ConfirmRequest="true" />
                                                    <%-- <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePagesBook" Mode="Raw" Value="Ext.encode(#{GridDispatch}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>--%>
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:ToolbarSpacer Width="30" />
                                        <ext:ToolbarSeparator runat="server" />
                                        <ext:ToolbarSpacer Width="30" />


                                        <ext:Button ID="btnSave" runat="server"
                                            Icon="Disk" Text="<%$Resource : SAVE%>" Width="60" TabIndex="20">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click"
                                                    Before="#{btnSave}.setDisabled(true);"
                                                    Complete="#{btnSave}.setDisabled(false);#{btnBook}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridDispatch}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true"
                                                        Message='<%$ Message :  MSG00025 %>' Title='<%$ MessageTitle :  MSG00025 %>' />
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                            <Listeners>
                                                <Click Handler="Ext.getCmp('cmbDispatchType').readOnly = false;" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="btnSaveApproveDate" runat="server"
                                            Icon="Disk" Text="<%$Resource : SAVE%>" Width="60" TabIndex="20" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSaveApproveDate_Click">
                                                    <Confirmation ConfirmRequest="true"
                                                        Message='<%$ Message :  MSG00025 %>' Title='<%$ MessageTitle :  MSG00025 %>' />
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                            <Listeners>
                                                <Click Handler="Ext.getCmp('cmbDispatchType').readOnly = false;" />
                                            </Listeners>
                                        </ext:Button>
              
                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$Resource : EXIT%>" Width="60" TabIndex="22">
                                            <%--<Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>--%>
                                            <DirectEvents>
                                                <Click OnEvent="btnClose_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                    </Items>
                                </ext:Toolbar>


                            </Items>
                        </ext:Toolbar>

                    </BottomBar>

                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$Resource : LOADING%>" />
                    </View>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>

        

        <ext:Window ID="WindowDataDetail" runat="server" Icon="FeedGo" Title="" Width="900" Height="450" Layout="BorderLayout" Resizable="false" Modal="true" Hidden="true">
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
                                    Width="200"
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
                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="false">
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

                                <ext:TextField ID="txtProduct" FieldLabel="Product Code" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="150" LabelWidth="80" />
                                <ext:TextField ID="txtPallet" FieldLabel="Pallet Code" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="150" LabelWidth="70" />
                                <ext:TextField ID="txtLot" FieldLabel="Lot" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="100" LabelWidth="30" />
                                <ext:TextField ID="txtOrderNo" FieldLabel="Order No" runat="server" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" Width="150" LabelWidth="60" />
                                <ext:Button ID="btnSearchProduct" runat="server" Icon="Magnifier" TabIndex="0" Text="Search" Width="60">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearchProduct_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreStockBalance" runat="server" PageSize="20" AutoLoad="false">
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
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="LotNo" /> 
                                        <ext:ModelField Name="Quantity" />
                                        <ext:ModelField Name="ExpirationDate" Type="Date" /> 
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="ProductStatusName" />
                                        <ext:ModelField Name="ProductSubStatusName" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="OrderNo" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns> 

                            <ext:RowNumbererColumn ID="colWRowNo" runat="server" Text="No" Width="40" Align="Center" Locked="true" />
                            <ext:Column ID="ColWarehouseName" runat="server" DataIndex="WarehouseName" Text="Warehouse Name" Align="Center" Locked="true" />
                            <ext:Column ID="colWProductCode" runat="server" DataIndex="ProductCode" Text="ProductCode" Align="Center" Locked="true" />
                            <ext:Column ID="colWProductName" runat="server" DataIndex="ProductName" Text="ProductName" MinWidth="200" Align="Left" Locked="true" />
                            <ext:Column ID="colWProductLot" runat="server" DataIndex="LotNo" Text="LotNo" Width="80" Align="Center" />
                            <ext:Column ID="colWPalletCode" runat="server" DataIndex="PalletCode" Text="Pallet Code" Width="150" Align="Center" />
                            <ext:NumberColumn ID="colWQuantity" runat="server" DataIndex="Quantity" Text="Quantity" Format="#.###" Align="Right" />
                            <ext:Column ID="colWUOMName" runat="server" DataIndex="ProductUnitName" Text="Unit" Width="100" Align="Center" />
                            <ext:DateColumn ID="colWDTEMFG" runat="server" DataIndex="MFGDate" Text="MFG" />
                            <ext:DateColumn ID="colWDTEEXP" runat="server" DataIndex="ExpirationDate" Text="EXP" />
                            <ext:Column ID="Column2" runat="server" DataIndex="OrderNo" Text="Order No." Width="70" Align="Center" />
                            <ext:Column ID="colWStatus" runat="server" DataIndex="ProductStatusName" Text="Status" Width="100" Align="Center" />
                            <ext:Column ID="colWSubStatus" runat="server" DataIndex="ProductSubStatusName" Text="Sub Status" Width="100" Align="Center" /> 
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
                                    <%--<DirectEvents>
                                        <Change OnEvent="btnSearchProduct_Click" />
                                    </DirectEvents>--%>
                                </ext:ComboBox>

                                <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnSelectItem_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="StoreStockBalance" Mode="Raw" Value="Ext.encode(#{GridGetWindows}.getRowsValues({selectedOnly : true}))" />
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

        <ext:Store ID="StoreComboRule" runat="server">
            <Model>
                <ext:Model runat="server" IDProperty="RuleId">
                    <Fields>
                        <ext:ModelField Name="RuleId" />
                        <ext:ModelField Name="RuleName" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <uc1:ucProductDispatchSelect runat="server" ID="ucProductDispatchSelect" />
    </form>
</body>
</html>

