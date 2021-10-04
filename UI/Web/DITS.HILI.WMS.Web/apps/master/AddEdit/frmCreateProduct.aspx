<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateProduct.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmCreateProduct" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <style>
        .x-panel p {
            margin: 0;
        }

        .tab-cont {
            float: left;
            margin: 0 15px 15px 0;
        }

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



        .ext-ie .x-form-text {
            position: static !important;
        }


        div#ListComboSupplier {
            border-top-width: 1 !important;
            width: 230px !important;
        }

        div#ListCmbProductGroup_L3 {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListcmbProductBrand {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListcmbProductShape {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListcmbProductBOM {
            border-top-width: 1 !important;
            width: 300px !important;
        }

        div#listStdUnit {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListcmbProductUOMTemplate {
            border-top-width: 1 !important;
            width: 200px !important;
        }


        div#ListcmbProductReplace {
            border-top-width: 1 !important;
            width: 300px !important;
        }
        /*input#cmbUnit-inputEl {
            background-color: #FFFFCC;
        }*/
    </style>
    <ext:XScript runat="server">
    <script >

        var sku_change = function(){
            var radSKU = #{chkSKU};
            var qtyUnit = #{txtQtyUnit};
            // var weightUnit = #{txtWeightUnit};

            if(radSKU.getValue())
            {
                qtyUnit.disable();
                qtyUnit.setValue("1");
                //   weightUnit.enable();
                //   weightUnit.setValue("0");
            }
            else
            {
                qtyUnit.enable();
                //  weightUnit.enable();
                //  weightUnit.setValue("0");
            }

        }

        var checkBomCodeBeforeSave = function(){
            
            if( #{cmbBOMProductCode}.getValue() == null)
            {
               
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Please Select Product Code',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbBOMProductCode').focus('', 10); 
                        }
                    }
                });
                
                return false;
            }else{
                return true;
            }
           
        };

        var checkColorBeforeSave = function(){

            var txtbox = #{txtColorName};
            var storeCol = #{ColorStore};
            var countCol = 0;
            for(i=0;i<storeCol.data.items.length;i++)
            {
                if(storeCol.data.items[i].data.Product_Color == txtbox.getValue() && 
                    storeCol.data.items[i].data.Product_Color_ID !=  #{hidColorID}.getValue())
                {
                    countCol++;
                }
            }

            if(countCol > 0)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Color Dupplicate',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('txtColorName').focus('', 10); 
                        }
                    }
                });
                txtbox.reset();
                return false;

            }

            return true;
           
        };

        var checkMinBeforeSave = function(){


            var txtbox = #{txtPriority};
            var storeMin = #{MinStore};
            var countMin = 0;
            for(i=0;i<storeMin.data.items.length;i++)
            {
                if(storeMin.data.items[i].data.Product_SafetyStock_No == txtbox.getValue() && 
                    storeMin.data.items[i].data.Product_SafetyStock_ID !=  #{hidMinID}.getValue())
                {
                    countMin++;
                }
            }

            if(countMin > 0)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Priority Dupplicate',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('txtPriority').focus('', 10); 
                        }
                    }
                });
                txtbox.reset();
                return false;

            }

            return true;
           
        };

        var checkReplaceCodeBeforeSave = function(){
            
            if( #{cmbReplaceProductCode}.getValue() == null)
            {
               
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Please Select Product Code',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbReplaceProductCode').focus('', 10); 
                        }
                    }
                });
                
                return false;
            }else{
                return true;
            }
           
        };
        
        var checkStdUnitBeforeSave = function(){

            if(#{cmbStdUnit}.getValue() == null)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Please Select Standard Unit',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbStdUnit').focus('', 10); 
                        }
                    }
                });
                return false;
            }
            
            
            if(#{txtQtyUnit}.getValue() == null)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Please input Quantity Unit',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('txtQtyUnit').focus('', 10); 
                        }
                    }
                });
                return false;
            }

            if(#{txtPalletQty}.getValue() == null)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Please input Pallet Qty',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('txtPalletQty').focus('', 10); 
                        }
                    }
                });
                return false;
            }

            var storeUOM = #{UOMStore};
            var countUOM = 0;
            for(i=0;i<storeUOM.data.items.length;i++)
            {
                if(storeUOM.data.items[i].data.IsBaseUOM == 1 && 
                    storeUOM.data.items[i].data.ProductUnitID !=  #{hidStdUnitID}.getValue())
                {
                    countUOM++;
                }
            }

            if(countUOM == 0)
            {
                if(#{chkSKU}.getValue()==false)
                {
                    Ext.MessageBox.show({
                        title:'WARNING',
                        msg: 'UOM must set SKU at lease 1 unit.',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(button){
                            if(button=='ok'){
                                Ext.getCmp('chkSKU').focus('', 10); 
                            }
                        }
                    });

                    Ext.getCmp('chkSKU').focus('', 10); 
                    return false;
                }
                
            }
           
            return true;
           
        };

        var StoreProductBOM_Select = function(){
            var combobox = #{cmbBOMProductCode};
            var storeCombo = #{StoreBOM};
            var countProductCode = 0;
            for(i=0;i<storeCombo.data.items.length;i++)
            {
                if(storeCombo.data.items[i].data.SYS_Product_BOM_System_Code == combobox.getValue() && 
                    storeCombo.data.items[i].data.SYS_Product_BOM_System_Code !=  #{hidBomCode}.getValue())
                {
                    countProductCode++;
                }
            }

            if(countProductCode > 0)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Data Dupplicate',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbBOMProductCode').focus('', 10); 
                        }
                    }
                });
                combobox.reset();
                #{txtBOMProductName}.reset();
                #{txtBOMQty}.reset();
                #{hidBomID}.reset();
                #{hidBomCode}.reset();
                Ext.getCmp('cmbBOMProductCode').focus('', 10); 

            }else{
                var v = combobox.getValue();
                var record = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                #{txtBOMProductName}.setValue(record.data.Product_Name_Full);
                Ext.getCmp('txtBOMQty').focus('', 10); 
            }
           
        };

        var StoreStdUnit_Select = function(){
            var combobox = #{cmbStdUnit};
            var storeCombo = #{UOMStore};
            var countStdUnit = 0;
            for(i=0;i<storeCombo.data.items.length;i++)
            {
                if(storeCombo.data.items[i].data.Name == combobox.rawValue
                    && 
                    storeCombo.data.items[i].data.Name !=  #{hidStdUnitName}.getValue())
                {
                    countStdUnit++;
                    break;
                }
            }

            if(countStdUnit > 0)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Data Dupplicate',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbStdUnit').focus('', 10); 
                        }
                    }
                });
                combobox.reset();
                #{txtUnitShortName}.reset();
                Ext.getCmp('cmbStdUnit').focus('', 10); 

            }else{
                var v = combobox.getValue();
                var record = combobox.findRecord(combobox.valueField || combobox.displayField, v);

                if(#{chkSKU}.disable)
                {
                    Ext.getCmp('chkSKU').setValue(false);
                }

                //#{txtUnitShortName}.setValue(record.data.StdUnit_ShortName);
                Ext.getCmp('txtQtyUnit').focus('', 10); 
            }
           
        };

        var StoreProductReplace_Select = function(){
            var combobox = #{cmbReplaceProductCode};
            var storeCombo = #{StoreReplace};
            var countProductCode = 0;
            for(i=0;i<storeCombo.data.items.length;i++)
            {
                if(storeCombo.data.items[i].data.SYS_Product_BOM_System_Code == combobox.getValue() && 
                    storeCombo.data.items[i].data.SYS_Product_BOM_System_Code !=  #{hidReplaceCode}.getValue())
                {
                    countProductCode++;
                }
            }

            if(countProductCode > 0)
            {
                Ext.MessageBox.show({
                    title:'WARNING',
                    msg: 'Data Dupplicate',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(button){
                        if(button=='ok'){
                            Ext.getCmp('cmbReplaceProductCode').focus('', 10); 
                        }
                    }
                });
                combobox.reset();
                #{txtReplaceProductName}.reset();
                #{txtReplaceQty}.reset();
                #{hidReplaceID}.reset();
                #{hidReplaceCode}.reset();
                Ext.getCmp('cmbReplaceProductCode').focus('', 10); 

            }else{
                var v = combobox.getValue();
                var record = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                #{txtReplaceProductName}.setValue(record.data.Product_Name_Full);
                Ext.getCmp('txtReplaceQty').focus('', 10); 
            }
           
        };

        var basicExpand = function () {
            var me = this;
            var productcode = Ext.getCmp("groupProductDetail");
            var productdetail = Ext.getCmp("groupProductCode");
            //var productgroup = Ext.getCmp("groupProductGroup");
            var productpicture = Ext.getCmp("groupProductPicture");

            if (me.id == "groupProductCode" || me.id == "groupProductDetail") {
                if (productcode.collapsed == true) {
                    productcode.expand();
                }

                if (productdetail.collapsed == true) {
                    productdetail.expand();
                }

                //productgroup.collapse();
                productpicture.collapse();

            } else if (me.id == "groupProductGroup") {
                productcode.collapse();
                productdetail.collapse();
                productpicture.collapse();
            } else if (me.id == "groupProductPicture") {
                productcode.collapse();
                productdetail.collapse();
                //productgroup.collapse();
            }

        };

        var showTabs = function () {

            //#{tabBody}.closeTab(#{tabPicture});
            //#{tabBody}.closeTab(#{tabSpec});
            #{tabBody}.closeTab(#{tabProductUnit});            
            //#{tabBody}.closeTab(#{tabReceiveDispatch});
            //#{tabBody}.closeTab(#{tabProductBom});
            //#{tabBody}.closeTab(#{tabProductReplace});
            
            //#{tabBody}.closeTab(#{tabProductColorMin});

            //#{tabBody}.addTab(#{tabPicture},false);
            //#{tabBody}.addTab(#{tabSpec},false);

            #{tabBody}.addTab(#{tabProductUnit},false);
            // #{tabBody}.addTab(#{tabReceiveDispatch},false);
            // #{tabBody}.addTab(#{tabProductBom},false);
            //#{tabBody}.addTab(#{tabProductReplace},false);
            // #{tabBody}.addTab(#{tabProductColorMin},false);

            

            #{groupProductPicture}.show();
            #{radioProductType}.show();

        };

        var ChangeTextSKU = function (value) {
            if(value > 0)
            {
                return "Yes";
            }else{
                return "No";
            }
        };
         
        var EnableSysCode = function () {
            var txtSys_Code = Ext.getCmp("txtSysProductCode");
            if (txtSys_Code.value == 'new') {
                txtSys_Code.setReadOnly(false);
                txtSys_Code.setValue('');
                txtSys_Code.allowBlank = false;
                txtSys_Code.validate();
                txtSys_Code.focus('', 10);
            } else {
                txtSys_Code.setReadOnly(true);
                txtSys_Code.setValue('new');
            };
        }; 

    </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager ID="ResourceManager1" runat="server" />

            <ext:Viewport runat="server" Layout="BorderLayout">
                <Items>

                    <ext:TabPanel
                        ID="tabBody"
                        runat="server"
                        Region="Center"
                        Margins="0 4 4 0"
                        Cls="tabs"
                        MinTabWidth="50">
                        <Items>
                            <ext:FormPanel
                                ID="tabBasic"
                                runat="server"
                                Title='<%$ Resource : BASICINFO %>'
                                Padding="5"
                                AutoScroll="false">

                                <Listeners>
                                    <Activate Handler="#{btnSave}.setDisabled(false);" />
                                </Listeners>
                                <DirectEvents>
                                    <Activate OnEvent="tabDetail_Click" />
                                </DirectEvents>
                                <Items>
                                    <ext:Container Layout="FitLayout" runat="server" Flex="1">
                                        <Items>
                                            <ext:Hidden ID="hidCustomer_Code" runat="server" />
                                            <ext:Hidden ID="hisSysProduct" runat="server" />
                                            <ext:Hidden ID="hisProductCodeOld" runat="server" />
                                            <ext:Hidden ID="txtProductId" runat="server" />
                                            <ext:FieldSet Title='<%$ Resource : PRODUCT_CODE %>' Layout="AnchorLayout" runat="server" ID="groupProductCode" Collapsible="true">
                                                <Items>
                                                    <ext:RadioGroup Layout="ColumnLayout" runat="server" Flex="1" PaddingSpec="0 0 5 0" ID="radioGroupProductCode">
                                                        <Items>
                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>

                                                                    <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : PRODUCT_CODE %>'
                                                                        LabelWidth="100" Layout="HBoxLayout" LabelAlign="Right" Width="235" Hidden="true">
                                                                        <Items>
                                                                            <ext:TextField runat="server" Flex="1" MaxLength="20"
                                                                                EnforceMaxLength="true" LabelAlign="Right" Hidden="true"
                                                                                ID="txtProductCode" />
                                                                            <%--<ext:Button runat="server" Icon="NoteEdit" Margins="0 0 0 5" ID="btnEditCode">
                                                                                <Listeners>
                                                                                    <Click Fn="EnableSysCode" />
                                                                                </Listeners>
                                                                            </ext:Button>--%>
                                                                        </Items>
                                                                    </ext:FieldContainer>

                                                                    <ext:Container Layout="ColumnLayout" runat="server" Flex="1">
                                                                        <Items>
                                                                            <ext:TextField runat="server" FieldLabel='<%$ Resource : STOCK_CODE %>' LabelWidth="100" Width="210"
                                                                                LabelAlign="Right" Flex="1" MaxLength="10" EnforceMaxLength="true"
                                                                                ID="txtStockCode">
                                                                                <%--<RemoteValidation OnValidation="CheckProductCode" Before="#{btnSave}.setDisabled(true);" />--%>
                                                                            </ext:TextField>
                                                                            <ext:Radio runat="server" FieldLabel='<%$ Resource : MAIN_CODE %>'
                                                                                LabelWidth="65" LabelAlign="Right" Width="50"
                                                                                ID="radioStockCode" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                    <ext:Container Layout="ColumnLayout" runat="server" Flex="1" MarginSpec="7 0 0 0">
                                                                        <Items>
                                                                            <ext:TextField runat="server" FieldLabel='<%$ Resource : SUPPLIER_CODE %>' LabelWidth="100" Width="210"
                                                                                LabelAlign="Right" Flex="1" MaxLength="10" EnforceMaxLength="true"
                                                                                ID="txtSupplierCode">
                                                                                <%--<RemoteValidation OnValidation="CheckProductCode" Before="#{btnSave}.setDisabled(true);" />--%>
                                                                            </ext:TextField>
                                                                            <ext:Radio runat="server" FieldLabel='<%$ Resource : MAIN_CODE %>'
                                                                                LabelWidth="65" LabelAlign="Right" Width="50"
                                                                                ID="radioSupplierCode" />
                                                                        </Items>
                                                                    </ext:Container>

                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>
                                                                    <ext:Container Layout="ColumnLayout" runat="server" Flex="1">
                                                                        <Items>
                                                                            <ext:TextField runat="server" FieldLabel='<%$ Resource : COMMERCIAL_CODE %>' LabelWidth="130"
                                                                                Width="250" LabelAlign="Right" Flex="1" MaxLength="10" EnforceMaxLength="true"
                                                                                ID="txtOEMCode">
                                                                                <%--<RemoteValidation OnValidation="CheckProductCode" Before="#{btnSave}.setDisabled(true);" />--%>
                                                                            </ext:TextField>
                                                                            <ext:Radio runat="server" FieldLabel='<%$ Resource : MAIN_CODE %>'
                                                                                LabelWidth="65" LabelAlign="Right" Width="50"
                                                                                ID="radioOEMCode" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                    <ext:Container Layout="ColumnLayout" runat="server" Flex="1" MarginSpec="7 0 0 0">
                                                                        <Items>
                                                                            <ext:TextField runat="server" FieldLabel='<%$ Resource : WAREHOUSE_CODE %>' LabelWidth="130" Width="250"
                                                                                LabelAlign="Right" Flex="1" MaxLength="10" EnforceMaxLength="true"
                                                                                ID="txtWHCode">
                                                                                <%--<RemoteValidation OnValidation="CheckProductCode" Before="#{btnSave}.setDisabled(true);" />--%>
                                                                            </ext:TextField>
                                                                            <ext:Radio runat="server" FieldLabel='<%$ Resource : MAIN_CODE %>'
                                                                                LabelWidth="65" LabelAlign="Right" Width="50"
                                                                                ID="radioWHCode" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                    <ext:Container Layout="ColumnLayout" runat="server" Flex="1" MarginSpec="6 0 0 0">
                                                                        <Items>
                                                                            <ext:TextField runat="server" FieldLabel='<%$ Resource : MANUFACTURING_CODE %>'
                                                                                LabelWidth="130" Width="250" LabelAlign="Right" Flex="1" MaxLength="10" EnforceMaxLength="true"
                                                                                ID="txtManufacturingCode">
                                                                                <%--<RemoteValidation OnValidation="CheckProductCode" Before="#{btnSave}.setDisabled(true);" />--%>
                                                                            </ext:TextField>
                                                                            <ext:Radio runat="server" FieldLabel='<%$ Resource : MAIN_CODE %>'
                                                                                LabelWidth="65" LabelAlign="Right" Width="50"
                                                                                ID="radioManufacturingCode" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:RadioGroup>
                                                </Items>
                                                <Listeners>
                                                    <Expand Fn="basicExpand" />
                                                </Listeners>
                                            </ext:FieldSet>

                                            <ext:FieldSet Title='<%$ Resource : PRODUCT_DETAILS %>' Layout="ColumnLayout" runat="server"
                                                ID="groupProductDetail" Collapsible="true">
                                                <Items>
                                                    <ext:Container Layout="AnchorLayout" runat="server" Flex="1" PaddingSpec="0 0 5 0" ColumnWidth="0.5">
                                                        <Items>
                                                            <%--<ext:TextArea FieldLabel="<%$Resources:Langauge, ProductName%>" Flex="1" MarginSpec="5 0 0 0"
                                                                LabelAlign="Right" LabelWidth="90" Width="300" runat="server" AllowBlank="false"
                                                                MaxLength="50" EnforceMaxLength="true" ID="txtProductName" />--%>

                                                            <ext:TextField FieldLabel='<%$ Resource : PRODUCT_NAME %>' Flex="1" MarginSpec="5 0 0 0"
                                                                LabelAlign="Right" LabelWidth="90" Width="300" runat="server"
                                                                MaxLength="100" EnforceMaxLength="true" ID="txtProductName" AllowBlank="false" />

                                                            <%--<ext:TextField FieldLabel="<%$Resources:Langauge, ShortProductName%>" Flex="1" MarginSpec="5 0 0 0"
                                                                LabelAlign="Right" LabelWidth="90" Width="300" runat="server"
                                                                MaxLength="10" EnforceMaxLength="true" ID="txtShortProductName" />--%>
                                                            <ext:TextField FieldLabel='<%$ Resource : PRODUCT_MODEL %>' Flex="1" MarginSpec="5 0 0 0"
                                                                LabelAlign="Right" LabelWidth="90" Width="300" runat="server"
                                                                MaxLength="50" EnforceMaxLength="true" ID="txtProductModel" />
                                                            <%--  <ext:RadioGroup FieldLabel="<%$Resources:Langauge, ProductType%>" Flex="1" MarginSpec="5 0 0 0"
                                                                LabelAlign="Right" LabelWidth="100" Width="300" runat="server" ID="radioProductType">
                                                                <Items>
                                                                    <ext:Container Layout="ColumnLayout" runat="server">
                                                                        <Items>
                                                                            <ext:Radio BoxLabel="<%$Resources:Langauge, ProductSet%>" BoxLabelAlign="After"
                                                                                LabelWidth="80" LabelAlign="Right" runat="server" Width="100" ID="chkProductSet" />
                                                                            <ext:Radio BoxLabel="<%$Resources:Langauge, ProductUnit%>" BoxLabelAlign="After"
                                                                                LabelWidth="80" LabelAlign="Right" runat="server" ID="chkProductUnit" Checked="true" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                                <Listeners>
                                                                    <Change Handler="
                                                                        if(this.getChecked()[0].id =='chkProductSet'){
                                                                            #{groupProductGroup}.show();
                                                                        }else{
                                                                            #{groupProductGroup}.hide();
                                                                        }" />
                                                                </Listeners>
                                                            </ext:RadioGroup>--%>
                                                            <ext:Container runat="server" Layout="ColumnLayout" MarginSpec="5 0 0 0">
                                                                <Items>
                                                                    <ext:NumberField ID="txtAgeing" runat="server" FieldLabel='<%$ Resource : AGEING %>' LabelAlign="Right" SelectOnFocus="true" LabelWidth="90" Width="145" MinValue="1" AllowBlank="false" />
                                                                    <ext:DisplayField runat="server" Text="day(s)" MarginSpec="0 0 0 5" />
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Checkbox runat="server" ID="txtIsActive" LabelAlign="Right" FieldLabel='<%$ Resource : ACTIVE %>' LabelWidth="90" />
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container Layout="AnchorLayout" runat="server" Flex="1" PaddingSpec="0 0 5 0" ColumnWidth="0.49">
                                                        <Items>
                                                            <ext:Container Layout="ColumnLayout" Flex="1" runat="server" MarginSpec="5 0 0 0" ID="panelUOMTemplate">
                                                                <Items>
                                                                    <ext:ComboBox ID="cmbProductUOMTemplate"
                                                                        runat="server"
                                                                        Width="270"
                                                                        AllowBlank="false"
                                                                        DisplayField="Product_UOM_Template_Name"
                                                                        ValueField="Product_UOM_Template_ID"
                                                                        TriggerAction="Query"
                                                                        FieldLabel='<%$ Resource : UOM_TEMPLATE %>'
                                                                        AutoShow="false"
                                                                        TypeAhead="true"
                                                                        PageSize="25"
                                                                        MinChars="0"
                                                                        LabelWidth="100"
                                                                        LabelAlign="Right"
                                                                        ForceSelection="true">
                                                                        <ListConfig LoadingText="Searching..." ID="ListcmbProductUOMTemplate" MaxHeight="150">
                                                                            <ItemTpl runat="server">
                                                                                <Html>
                                                                                    <div class="search-item">
							                                                           {Product_UOM_Template_Name}
						                                                            </div>
                                                                                </Html>
                                                                            </ItemTpl>
                                                                        </ListConfig>
                                                                        <Store>
                                                                            <ext:Store ID="StoreProductUOMTemplate" runat="server" AutoLoad="false" PageSize="10">
                                                                                <Proxy>
                                                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductTemplateUom">
                                                                                        <ActionMethods Read="POST" />
                                                                                        <Reader>
                                                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                        </Reader>
                                                                                    </ext:AjaxProxy>
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model ID="Model2" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Product_UOM_Template_ID" />
                                                                                            <ext:ModelField Name="Product_UOM_Template_Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Select Handler="#{btnShowUOMTemplate}.setDisabled(0);" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>

                                                                    <ext:Button runat="server" MarginSpec="0 0 0 5" Icon="Magnifier" ID="btnShowUOMTemplate" Disabled="true">
                                                                        <DirectEvents>
                                                                            <Click OnEvent="btnShowUOMTemplate_Click" />
                                                                        </DirectEvents>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container Layout="ColumnLayout" Flex="1" runat="server" MarginSpec="5 0 0 0">
                                                                <Items>
                                                                    <ext:ComboBox ID="cmbProductGroup_L3"
                                                                        runat="server"
                                                                        Width="270"
                                                                        DisplayField="Name"
                                                                        ValueField="ProductGroupLevel3ID"
                                                                        TriggerAction="Query"
                                                                        FieldLabel='<%$ Resource : PRODUCT_CATEGORY %>'
                                                                        AutoShow="false"
                                                                        TypeAhead="true"
                                                                        AllowBlank="false"
                                                                        PageSize="25"
                                                                        MinChars="0"
                                                                        LabelWidth="100"
                                                                        LabelAlign="Right"
                                                                        ForceSelection="true">
                                                                        <ListConfig LoadingText="Searching..." ID="ListCmbProductGroup_L3" MaxHeight="150" PageSize="10">
                                                                            <ItemTpl runat="server">
                                                                                <Html>
                                                                                    <div class="search-item">
							                                                           {Name}
						                                                            </div>
                                                                                </Html>
                                                                            </ItemTpl>
                                                                        </ListConfig>
                                                                        <Store>
                                                                            <ext:Store ID="StoreProductGroup_L3" runat="server" AutoLoad="false">
                                                                                <Proxy>
                                                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductGroupLevel">
                                                                                        <ActionMethods Read="POST" />
                                                                                        <Reader>
                                                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                        </Reader>
                                                                                    </ext:AjaxProxy>
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model ID="Model9" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ProductGroupLevel3ID" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:ComboBox>
                                                                    <%-- <ext:Button Icon="Add" runat="server" MarginSpec="0 0 0 5" />--%>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container Layout="ColumnLayout" Flex="1" runat="server" MarginSpec="5 0 0 0">
                                                                <Items>
                                                                    <ext:ComboBox ID="cmbProductBrand"
                                                                        runat="server"
                                                                        Width="270"
                                                                        DisplayField="Name"
                                                                        ValueField="ProductBrandID"
                                                                        TriggerAction="Query"
                                                                        FieldLabel='<%$ Resource : PRODUCT_BRAND_NAME %>'
                                                                        AutoShow="false"
                                                                        TypeAhead="true"
                                                                        PageSize="25"
                                                                        MinChars="0"
                                                                        LabelWidth="100"
                                                                        LabelAlign="Right"
                                                                        ForceSelection="true">
                                                                        <ListConfig LoadingText="Searching..." ID="ListcmbProductBrand" MaxHeight="150">
                                                                            <ItemTpl runat="server">
                                                                                <Html>
                                                                                    <div class="search-item">
							                                                           {Name}
						                                                            </div>
                                                                                </Html>
                                                                            </ItemTpl>
                                                                        </ListConfig>
                                                                        <Store>
                                                                            <ext:Store ID="StoreProductBrand" runat="server" AutoLoad="false" PageSize="10">
                                                                                <Proxy>
                                                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductBrand">
                                                                                        <ActionMethods Read="POST" />
                                                                                        <Reader>
                                                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                        </Reader>
                                                                                    </ext:AjaxProxy>
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model ID="Model10" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ProductBrandID" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:ComboBox>
                                                                    <%-- <ext:Button Icon="Add" runat="server" MarginSpec="0 0 0 5" />--%>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container Layout="ColumnLayout" Flex="1" runat="server" MarginSpec="5 0 0 0">
                                                                <Items>
                                                                    <ext:ComboBox ID="cmbProductShape"
                                                                        runat="server"
                                                                        Width="270"
                                                                        DisplayField="Name"
                                                                        ValueField="ProductShapeID"
                                                                        TriggerAction="Query"
                                                                        FieldLabel='<%$ Resource : PRODUCT_SHAPE %>'
                                                                        AutoShow="false"
                                                                        TypeAhead="true"
                                                                        PageSize="25"
                                                                        MinChars="0"
                                                                        LabelWidth="100"
                                                                        LabelAlign="Right"
                                                                        ForceSelection="true">
                                                                        <ListConfig LoadingText="Searching..." ID="ListcmbProductShape" MaxHeight="150">
                                                                            <ItemTpl runat="server">
                                                                                <Html>
                                                                                    <div class="search-item">
							                                                           {Name}
						                                                            </div>
                                                                                </Html>
                                                                            </ItemTpl>
                                                                        </ListConfig>
                                                                        <Store>
                                                                            <ext:Store ID="StoreProductShape" runat="server" AutoLoad="false" PageSize="10">
                                                                                <Proxy>
                                                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductShape">
                                                                                        <ActionMethods Read="POST" />
                                                                                        <Reader>
                                                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                        </Reader>
                                                                                    </ext:AjaxProxy>
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model ID="Model11" runat="server">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ProductShapeID" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:ComboBox>
                                                                    <%--  <ext:Button Icon="Add" runat="server" MarginSpec="0 0 0 5" />--%>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container runat="server" Layout="ColumnLayout" Flex="1" MarginSpec="5 0 0 0">
                                                                <Items>
                                                                    <ext:NumberField ID="txtSafety" runat="server" FieldLabel='<%$ Resource : SAFETY_STOCK %>' LabelAlign="Right" SelectOnFocus="true" LabelWidth="100" Width="270" MinValue="1" />
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                                <Listeners>
                                                    <Expand Fn="basicExpand" />
                                                </Listeners>
                                            </ext:FieldSet>

                                            <ext:FieldSet Title='<%$ Resource : PICTURE %>' Layout="ColumnLayout" runat="server"
                                                ID="groupProductPicture" Collapsible="true" Collapsed="true" Padding="10">
                                                <Items>
                                                    <ext:Container StyleSpec="border: 1px solid gray;" Layout="FitLayout" runat="server" ColumnWidth="0.5">
                                                        <Items>
                                                            <ext:Image runat="server" ID="imgProductImage" Width="250" Height="220" />
                                                        </Items>
                                                    </ext:Container>

                                                    <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.5" MarginSpec="0 0 0 10">
                                                        <Items>
                                                            <ext:FileUploadField
                                                                ID="FileUploadPicture"
                                                                runat="server"
                                                                EmptyText="Select an image"
                                                                Icon="ImageAdd" Width="200" />

                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>
                                                                    <%--<ext:Button ID="btnImageAdd" runat="server" Icon="ImageAdd"
                                                                        Text="Add" Width="60">
                                                                        <DirectEvents>
                                                                            <Click OnEvent="UploadImage"
                                                                                Before="Ext.Msg.wait('Uploading...', 'Uploading');"
                                                                                Failure="Ext.Msg.show({
                                                                                            title: 'Uploading Error',
                                                                                            msg: result.errorMessage,
                                                                                            buttons: Ext.Msg.OK,
                                                                                            icon: Ext.MessageBox.ERROR
                                                                                        });">
                                                                            </Click>
                                                                        </DirectEvents>
                                                                    </ext:Button>--%>

                                                                    <%--<ext:Button ID="btnImageDelete" runat="server" Icon="ImageDelete"
                                                                        Text="Delete" MarginSpec="0 0 0 5" Disabled="true">
                                                                        <DirectEvents>
                                                                            <Click OnEvent="DeleteImage"
                                                                                Before="Ext.Msg.wait('Processing...', 'Processing');"
                                                                                Failure="Ext.Msg.show({
                                                                                            title: 'Delete Error',
                                                                                            msg: result.errorMessage,
                                                                                            buttons: Ext.Msg.OK,
                                                                                            icon: Ext.MessageBox.ERROR
                                                                                        });">
                                                                            </Click>
                                                                        </DirectEvents>
                                                                    </ext:Button>--%>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:Container>

                                                </Items>
                                                <Listeners>
                                                    <Expand Fn="basicExpand" />
                                                </Listeners>
                                            </ext:FieldSet>
                                        </Items>
                                    </ext:Container>
                                </Items>
                                <Listeners>
                                    <%--<ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />--%>
                                </Listeners>
                            </ext:FormPanel>

                            <ext:Panel
                                ID="tabProductUnit"
                                runat="server"
                                Title='<%$ Resource : PRODUCT_UNIT %>'
                                HideMode="Offsets"
                                Padding="3"
                                AutoScroll="true"
                                CloseAction="Hide">

                                <DirectEvents>
                                    <Activate OnEvent="tabStdUnit_Click" />
                                </DirectEvents>

                                <Items>

                                    <ext:Container Layout="ColumnLayout" runat="server" Padding="3">
                                        <Items>
                                            <ext:Container Layout="AnchorLayout" runat="server" Padding="3">
                                                <Items>
                                                    <ext:FieldSet Title="UnitDetails" Layout="FitLayout" runat="server" Padding="5">
                                                        <Items>
                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>
                                                                    <ext:Container Layout="ColumnLayout" runat="server">
                                                                        <Items>

                                                                            <ext:ComboBox Editable="false"
                                                                                ID="cmbStdUnit"
                                                                                runat="server"
                                                                                Width="200"
                                                                                MarginSpec="0 5 0 0"
                                                                                DisplayField="Name"
                                                                                ValueField="ShortName"
                                                                                Flex="1"
                                                                                TriggerAction="Query"
                                                                                FieldLabel='<%$ Resource : UNIT %>'
                                                                                AutoShow="false"
                                                                                TypeAhead="true"
                                                                                PageSize="25"
                                                                                MinChars="0"
                                                                                LabelAlign="Right"
                                                                                ForceSelection="true"
                                                                                LabelWidth="110">
                                                                                <ListConfig LoadingText="Searching..." ID="listStdUnit" MaxHeight="150">
                                                                                    <ItemTpl runat="server">
                                                                                        <Html>
                                                                                            <div class="search-item">
							                                                                  {Name} <br>
                                                                                              {ShortName}
						                                                                    </div>
                                                                                        </Html>
                                                                                    </ItemTpl>
                                                                                </ListConfig>
                                                                                <Store>
                                                                                    <ext:Store ID="StdUnitStore" runat="server" AutoLoad="true">
                                                                                        <Proxy>
                                                                                            <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Unit">
                                                                                                <ActionMethods Read="POST" />
                                                                                                <Reader>
                                                                                                    <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                                </Reader>
                                                                                            </ext:AjaxProxy>
                                                                                        </Proxy>
                                                                                        <Model>
                                                                                            <ext:Model ID="Model1" runat="server">
                                                                                                <Fields>

                                                                                                    <ext:ModelField Name="UnitID" />
                                                                                                    <ext:ModelField Name="Name" />
                                                                                                    <ext:ModelField Name="ShortName" />

                                                                                                </Fields>
                                                                                            </ext:Model>
                                                                                        </Model>
                                                                                    </ext:Store>
                                                                                </Store>
                                                                                <Listeners>
                                                                                    <Select Fn="StoreStdUnit_Select" />
                                                                                </Listeners>
                                                                            </ext:ComboBox>

                                                                            <ext:Button runat="server" ID="btnAddStdUnit" Icon="Add">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="btnAddStdUnit_Click" />
                                                                                </DirectEvents>
                                                                            </ext:Button>

                                                                            <ext:Hidden ID="hidStdUnitID" runat="server" Text=""></ext:Hidden>
                                                                            <ext:Hidden ID="hidStdUnitName" runat="server" Text=""></ext:Hidden>
                                                                        </Items>
                                                                    </ext:Container>
                                                                    <%--<ext:TextField FieldLabel="<%$Resources:Langauge, ShortName%>" LabelAlign="Right" LabelWidth="110" Width="150" ID="txtUnitShortName" Flex="1" runat="server" MarginSpec="5 0 0 0" ReadOnly="true" />--%>
                                                                    <ext:NumberField FieldLabel="Counting (SKU)"
                                                                        ID="txtQtyUnit"
                                                                        LabelAlign="Right"
                                                                        LabelWidth="110"
                                                                        Width="180"
                                                                        AllowDecimals="true"
                                                                        MinValue="0.001"
                                                                        AllowBlank="false"
                                                                        Flex="1"
                                                                        runat="server"
                                                                        MarginSpec="5 0 0 0" />
                                                                    <ext:NumberField FieldLabel="Pallet Qty" LabelAlign="Right" LabelWidth="110" Width="180" ID="txtPalletQty" AllowDecimals="false" MinValue="0" AllowBlank="false" Flex="1" runat="server" MarginSpec="5 0 0 0" />
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:FieldSet>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container Layout="AnchorLayout" runat="server" Padding="3">
                                                <Items>
                                                    <ext:FieldSet Title='<%$ Resource : PRODUCT_VOLUME %>' Layout="FitLayout" runat="server" Padding="5">
                                                        <Items>
                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>

                                                                    <ext:Container Layout="ColumnLayout" runat="server" DefaultAnchor="90%">
                                                                        <Items>
                                                                            <ext:NumberField ID="txtWUnit" runat="server" FieldLabel='<%$ Resource : WIDTH %>' AllowBlank="false" AllowDecimals="true" Text="0" Flex="1" DecimalPrecision="3" MinValue="0.001" Format="#,###.###" TabIndex="11" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                            <ext:Label runat="server" Text='<%$ Resource : CM %>' MarginSpec="0 0 0 5" />
                                                                        </Items>
                                                                    </ext:Container>

                                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                        <Items>
                                                                            <ext:NumberField ID="txtLUnit" runat="server" FieldLabel='<%$ Resource : LENGTH %>' AllowDecimals="true" Text="0" Flex="1" DecimalPrecision="3" MinValue="0.001" Format="#,###.###" AllowBlank="false" TabIndex="12" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                            <ext:Label runat="server" Text='<%$ Resource : CM %>' MarginSpec="0 0 0 5" />
                                                                        </Items>
                                                                    </ext:Container>

                                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                        <Items>
                                                                            <ext:NumberField ID="txtHUnit" runat="server" FieldLabel='<%$ Resource : HEIGHT %>' AllowDecimals="true" Text="0" Flex="1" DecimalPrecision="3" MinValue="0.001" Format="#,###.###" AllowBlank="false" TabIndex="13" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                            <ext:Label runat="server" Text='<%$ Resource : CM %>' MarginSpec="0 0 0 5" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                    <%--<ext:NumberField FieldLabel="W x L x H (cm3)" MinValue="1" ID="txtWUnit" AllowBlank="false" LabelAlign="Right" Text="1" Flex="1" Width="160" runat="server" EmptyText="กว้าง" />
                                                                    <ext:NumberField FieldLabel="x" Flex="1" MinValue="1" AllowBlank="false" ID="txtLUnit" LabelWidth="20" Text="1" Width="80" LabelAlign="Right" runat="server" EmptyText="ยาว" />
                                                                    <ext:NumberField FieldLabel="x" Flex="1" MinValue="1" AllowBlank="false" ID="txtHUnit" LabelWidth="20" Text="1" Width="80" LabelAlign="Right" runat="server" EmptyText="สูง" />--%>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:FieldSet>

                                                </Items>
                                            </ext:Container>
                                            <ext:Container Layout="AnchorLayout" runat="server" Padding="3">
                                                <Items>
                                                    <ext:FieldSet Title="Product Weight" Layout="FitLayout" runat="server" Padding="5">
                                                        <Items>
                                                            <ext:Container Layout="AnchorLayout" runat="server">
                                                                <Items>

                                                                    <ext:NumberField FieldLabel="Net Weight (kg)" LabelAlign="Right" Width="180" LabelWidth="110"
                                                                        ID="txtWeightUnit" AllowBlank="false" AllowDecimals="true" Text="0.000" Flex="1" DecimalPrecision="3"
                                                                        MinValue="0.001" Format="#,###.###" runat="server" MarginSpec="0 0 0 0" />

                                                                    <ext:NumberField FieldLabel="Pack. Weight (kg)" LabelAlign="Right" Width="180" LabelWidth="110"
                                                                        ID="txtPackWeight" AllowBlank="false" AllowDecimals="true" Text="0.000" Flex="1" DecimalPrecision="3"
                                                                        MinValue="0" Format="#,###.###" runat="server" MarginSpec="5 0 5 0" />
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:FieldSet>
                                                    <ext:Container Layout="ColumnLayout" runat="server">
                                                        <Items>
                                                            <ext:Checkbox runat="server" FieldLabel="Default Unit" ID="chkSKU" LabelWidth="80" Width="150" LabelAlign="Right">
                                                                <Listeners>
                                                                    <Change Fn="sku_change" />
                                                                </Listeners>
                                                            </ext:Checkbox>
                                                            <ext:Button Text="<%$ Resource : SAVE %>" runat="server" Width="55" ID="Button2">
                                                                <DirectEvents>
                                                                    <Click OnEvent="btnAddStdUnitItem_Click" Before="checkStdUnitBeforeSave">
                                                                        <EventMask ShowMask="true" Msg="Saving" MinDelay="300" />
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>

                                    <ext:GridPanel ID="GridUOM" runat="server" Height="210" Layout="FitLayout">
                                        <Store>
                                            <ext:Store ID="UOMStore" runat="server">
                                                <Model>
                                                    <ext:Model ID="Model6" runat="server">
                                                        <Fields>

                                                            <ext:ModelField Name="ProductUnitID" />
                                                            <ext:ModelField Name="ProductID" />
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Barcode" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Quantity" Type="Float" />
                                                            <ext:ModelField Name="PalletQTY" Type="Float" />
                                                            <ext:ModelField Name="Description" />
                                                            <ext:ModelField Name="Width" />
                                                            <ext:ModelField Name="Height" />
                                                            <ext:ModelField Name="Length" />
                                                            <ext:ModelField Name="Cubicmeters" />
                                                            <ext:ModelField Name="IsBaseUOM" />
                                                            <ext:ModelField Name="ProductWeight" />
                                                            <ext:ModelField Name="PackageWeight" />
                                                            <ext:ModelField Name="URLImage" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Quantity" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="ColumnModel6" runat="server">
                                            <Columns>
                                                <ext:CommandColumn runat="server" Width="30" Locked="true" Resizable="false" Sortable="false">
                                                    <Commands>
                                                        <ext:CommandFill />
                                                        <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Unit" CommandName="Delete" />
                                                    </Commands>
                                                    <DirectEvents>
                                                        <Command OnEvent="UnitCommandClick">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="ParamStoreUOM" Mode="Raw" Value="Ext.encode(#{GridUOM}.getRowsValues({selectedOnly : false}))" />
                                                                <ext:Parameter Name="oDataKeyId" Value="record.data.ProductUnitID" Mode="Raw" />
                                                                <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                                            </ExtraParams>
                                                            <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete?" Title="Delete" />
                                                            <EventMask ShowMask="true" Msg="Processing" />
                                                        </Command>
                                                    </DirectEvents>
                                                </ext:CommandColumn>
                                                <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Text='<%$ Resource : NUMBER %>' Width="40" Align="Center" />
                                                <ext:Column ID="Column26" runat="server" DataIndex="Name" Text='<%$ Resource : UNIT %>' Align="Center" Flex="1" />
                                                <ext:Column ID="Column5" runat="server" DataIndex="Name" Visible="false" />
                                                <ext:Column ID="Column21" runat="server" DataIndex="Code" Visible="false" />

                                                <ext:Column ID="Column1" runat="server" DataIndex="Quantity" Text="Quantity (SKU)" Align="Center" Flex="1" />
                                                <ext:NumberColumn ID="Column3" runat="server" DataIndex="ProductWeight" Text="G.W. (kg)" Format="#,###.###" Align="Center" Flex="1" />
                                                <ext:NumberColumn ID="Column6" runat="server" DataIndex="Width" Text="Width (m)" Format="#,###.###" Align="Center" Flex="1" />
                                                <ext:NumberColumn ID="Column11" runat="server" DataIndex="Length" Text="Length (m)" Format="#,###.###" Align="Center" Flex="1" />
                                                <ext:NumberColumn ID="Column12" runat="server" DataIndex="Height" Text="Height (m)" Format="#,###.###" Align="Center" Flex="1" />
                                                <ext:NumberColumn ID="Column8" runat="server" DataIndex="PalletQTY" Text="Pallet Qty" Format="#,###" Align="Center" Flex="1" />
                                                <ext:Column ID="Column2" runat="server" DataIndex="IsBaseUOM" Text='<%$ Resource : IS_STORAGE_UNIT %>' Align="Center" Flex="1">
                                                    <Renderer Fn="ChangeTextSKU" />
                                                </ext:Column>

                                            </Columns>
                                        </ColumnModel>
                                        <BottomBar>
                                        </BottomBar>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" Mode="Single">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <DirectEvents>
                                            <CellDblClick OnEvent="grdUnitList_CellDblClick">
                                                <ExtraParams>
                                                    <ext:Parameter Name="DataKeyId" Value="record.data" Mode="Raw" />
                                                </ExtraParams>
                                            </CellDblClick>
                                        </DirectEvents>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>

                            <ext:Panel
                                ID="tabReceiveDispatch"
                                runat="server"
                                Title="<%$ Resource : RCV_DISP_CONFIG %>"
                                HideMode="Offsets"
                                Padding="3"
                                AutoScroll="true"
                                CloseAction="Hide" Hidden="true">
                                <Listeners>
                                    <Activate Handler="#{btnSave}.setDisabled(false);if(#{radioFEFO}.checked==true){#{radioDispCon}.setDisabled(1);}" />
                                </Listeners>
                                <Items>
                                    <ext:FieldSet Title="<%$ Resource : RCV_CONFIG %>" Flex="1" Layout="FitLayout" runat="server" Padding="3">
                                        <Items>
                                            <ext:RadioGroup FieldLabel="<%$ Resource : IS_QC %>" LabelWidth="250" ID="radioIsQC"
                                                LabelAlign="Right" Flex="1" runat="server" Width="380">
                                                <Items>

                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server">
                                                        <Items>
                                                            <ext:Radio BoxLabel="<%$ Resource : YES %>" BoxLabelAlign="After"
                                                                runat="server" Width="100" ID="radioIsQCYes" />
                                                            <ext:Radio BoxLabel="<%$ Resource : NOT %>" BoxLabelAlign="After"
                                                                runat="server" ID="radioIsQCNo" />
                                                        </Items>
                                                    </ext:Container>

                                                </Items>
                                            </ext:RadioGroup>

                                            <ext:RadioGroup FieldLabel="<%$ Resource : IS_PALLET %>" ID="radioIsPallet"
                                                LabelWidth="250" LabelAlign="Right" Flex="1" runat="server" Width="380">
                                                <Items>

                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server">
                                                        <Items>
                                                            <ext:Radio BoxLabel="<%$ Resource : YES %>" BoxLabelAlign="After" runat="server" Width="100" ID="radioIsPalletYes">
                                                                <%--<DirectEvents>
                                                                    <Change OnEvent="switchPallet" />
                                                                </DirectEvents>--%>
                                                            </ext:Radio>
                                                            <ext:Radio BoxLabel="<%$ Resource : NOT %>" BoxLabelAlign="After" runat="server" ID="radioIsPalletNo">
                                                                <%-- <DirectEvents>
                                                                    <Activate OnEvent="switchunPallet" />

                                                                </DirectEvents>--%>
                                                            </ext:Radio>
                                                        </Items>
                                                    </ext:Container>

                                                </Items>
                                            </ext:RadioGroup>
                                            <%--<ext:Container Layout="AnchorLayout" Flex="1" runat="server" PaddingSpec="0 0 10 0">
                                                <Items>
                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server">
                                                        <Items>
                                                            <ext:NumberField FieldLabel="<%$Resources:Langauge, Qty1Pallet%>" ID="txtReceive_Pallet_Q"
                                                                LabelWidth="250" Width="350" LabelAlign="Right" MinValue="1" runat="server"
                                                                ReadOnly="true">
                                                                <Listeners>
                                                                    <Blur Handler="if (this.value == null) {  this.setValue(1,true); }" />
                                                                </Listeners>
                                                            </ext:NumberField>

                                                            <ext:Label runat="server" ID="palletSKU" Text="PC." Width="100" MarginSpec="0 0 0 10" />
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:NumberField FieldLabel="<%$Resources:Langauge, Level1Pallet%>" ID="txtReceive_Pallet_QL"
                                                        LabelWidth="250" Width="350" MarginSpec="5 0 0 0 " MinValue="1"
                                                        LabelAlign="Right" runat="server" ReadOnly="true" Visible="false">
                                                        <Listeners>
                                                            <Blur Handler="if (this.value == null) {  this.setValue(1,true); }" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                </Items>
                                            </ext:Container>--%>
                                        </Items>
                                    </ext:FieldSet>

                                    <%--<ext:RadioGroup FieldLabel="<%$Resources:Langauge, IsPickup%>" LabelWidth="250" LabelAlign="Right" Flex="1" runat="server" ID="radioIsPickup">
                                                <Items>
                                                    <ext:Container Layout="ColumnLayout" Flex="1" runat="server">
                                                        <Items>
                                                            <ext:Radio BoxLabel="<%$Resources:Langauge, Yes%>" Flex="1" BoxLabelAlign="After" runat="server" Width="100" ID="radioIsPickupYes" />
                                                            <ext:Radio BoxLabel="<%$Resources:Langauge, Not%>" LabelWidth="130" Flex="1" BoxLabelAlign="After" runat="server"  ID="radioIsPickupNo" />
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                            </ext:RadioGroup>--%>
                                    <%--     </Items>
                                    </ext:FieldSet>--%>
                                </Items>
                            </ext:Panel>

                        </Items>

                        <BottomBar>
                            <ext:Toolbar runat="server" ID="toolbarControls">
                                <Items>
                                    <ext:ToolbarFill ID="TbarFill" runat="server" />
                                    <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save" Width="60" TabIndex="15">
                                        <DirectEvents>
                                            <Click OnEvent="btnSave_Click"
                                                Before="#{btnSave}.setDisabled(true);"
                                                Complete="#{btnSave}.setDisabled(false);"
                                                Buffer="350">
                                                <EventMask ShowMask="true" Msg="Processing" MinDelay="100"></EventMask>
                                                <%--<Confirmation ConfirmRequest="true" Message="<%$Resources:Langauge, DoYouWantToSave%>" />--%>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" TabIndex="16">
                                        <%--<Listeners>
                                            <Click Handler="#{tabBody}.addTab(#{tabPicture});" />
                                        </Listeners>--%>
                                        <DirectEvents>
                                            <Click OnEvent="btnClear_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="16">
                                        <DirectEvents>
                                            <Click OnEvent="btnExit_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:TabPanel>
                </Items>
            </ext:Viewport>

            <ext:Window ID="winUOMTemplate" runat="server" Title="View UOM Template" Height="300" Width="550" Modal="true" Hidden="true" Icon="Magnifier" Resizable="false" Layout="FitLayout" Frame="true">
                <Items>
                    <ext:GridPanel ID="grdShowUOMTemplate" runat="server" Layout="FitLayout">
                        <Store>
                            <ext:Store ID="StoreShowUOMTemplate" runat="server">
                                <Model>
                                    <ext:Model ID="Model3" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Product_UOM_Template_ID" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Name" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Short_Name" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Quantity" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Gross_Weight" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Package_Width" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Package_Length" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_Package_Height" />
                                            <ext:ModelField Name="Product_UOM_Template_Detail_SKU_Text" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn5" runat="server" Text='<%$ Resource : NUMBER %>' Width="40" Align="Center" Locked="true" />
                                <ext:Column ID="Column4" runat="server" DataIndex="Product_UOM_Template_Detail_Name" Text='<%$ Resource : UNIT %>' Align="Center" Width="100" Locked="true" />
                                <ext:Column ID="Column25" runat="server" DataIndex="Product_UOM_Template_Detail_SKU_Text" Text="IsStorageUnit" Align="Center" Width="100">
                                    <Renderer Fn="ChangeTextSKU" />
                                </ext:Column>
                                <ext:Column ID="Column7" runat="server" DataIndex="Product_UOM_Template_Detail_Short_Name" Visible="false" />
                                <ext:Column ID="Column9" runat="server" DataIndex="Product_UOM_Template_Detail_Quantity" Text="Quantity (SKU)" Align="Center" Width="100" />
                                <ext:Column ID="Column10" runat="server" DataIndex="Product_UOM_Template_Detail_Gross_Weight" Text="Weight (kg)" Align="Center" Width="100" />
                                <ext:Column ID="Column14" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Width" Text="Width (m)" Align="Center" Width="100" />
                                <ext:Column ID="Column17" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Length" Text="Length (m)" Align="Center" Width="100" />
                                <ext:Column ID="Column23" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Height" Text="Height (m)" Align="Center" Width="100" />
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                        </BottomBar>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                    </ext:GridPanel>
                </Items>
            </ext:Window>
        </div>
    </form>
</body>
</html>
