<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateTemplate_Product_UOM.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateTemplate_Product_UOM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <style>
        div#listStdUnit {
            border-top-width: 1 !important;
            width: 200px !important;
        }
    </style>
    <ext:XScript runat="server">
        <script>

            var sku_change = function(){
                var radSKU = #{chkSKU};
                var qtyUnit = #{txtQtyUnit};
                var weightUnit = #{txtWeightUnit};

                if(radSKU.getValue())
                {
                    qtyUnit.disable();
                    qtyUnit.setValue("1");
                    //weightUnit.enable();
                    //weightUnit.setValue("0");
                }
                else
                {
                    qtyUnit.enable();
                    //weightUnit.enable();
                    //weightUnit.setValue("0");
                }

            };

            var StoreStdUnit_Select = function(){
                var combobox = #{cmbStdUnit};
                var storeCombo = #{UOMStore};
                var countStdUnit = 0;
            
                for(i=0;i<storeCombo.data.items.length;i++)
                {
                   // console.log(storeCombo.data.items[i].data.Name );
                    
                    if(storeCombo.data.items[i].data.Product_UOM_Template_Detail_Name == combobox.rawValue
                        && 
                        storeCombo.data.items[i].data.Product_UOM_Template_Detail_Name !=  #{hidStdUnitName}.getValue())
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
                    console.log(record.data.ShortName);
                    #{txtUnitShortName}.setValue(record.data.ShortName);
                    Ext.getCmp('txtQtyUnit').focus('', 10); 
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

                var storeUOM = #{UOMStore};
                var countUOM = 0;
                for(i=0;i<storeUOM.data.items.length;i++)
                {
                    if(storeUOM.data.items[i].data.Product_UOM_Template_Detail_SKU == 1)
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
                else
                {
                    if(#{chkSKU}.getValue())
                    {
                        for(i=0;i<storeUOM.data.items.length;i++)
                        {
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_SKU =0;
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_SKU_Text ='No';
                        }

                         #{GridUOM}.getView().refresh();
                    }
                }
           

                return true;
           
            };

            var btnAddStdUnitItem_Click = function(){

                if(!checkStdUnitBeforeSave()) return;

                var storeUOM = #{UOMStore};

                if(#{hidRowId}.getValue() != -99)
                {
                    var isSKU = 0;
                    var isSKUText = 'No';

                    if(#{chkSKU}.getValue()) {isSKU = 1;isSKUText = 'Yes';}

                    for(i=0;i<storeUOM.data.items.length;i++)
                    {
                        if(#{hidRowId}.getValue() == i)
                        {
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_ID = #{Product_UOM_Detail_ID}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Name = #{cmbStdUnit}.rawValue;
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Short_Name = #{cmbStdUnit}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Quantity = #{txtQtyUnit}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_SKU = isSKU;
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_SKU_Text = isSKUText;
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Status = 0;
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Weight = #{txtWeightUnit}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Package_Weight = #{txtPackWeightUnit}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Gross_Weight = #{txtWeightUnit}.getValue() + #{txtPackWeightUnit}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Package_Width = #{txtWidth}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Package_Length = #{txtLength}.getValue();
                            storeUOM.data.items[i].data.Product_UOM_Template_Detail_Package_Height = #{txtHeight}.getValue();
                            break;
                        }
                    }

                }
                else
                {

                    var grid = #{GridUOM},record;

                    var freeRecord = -1;
              
                    var isSKU = 0;
                    var isSKUText = 'No';

                    if(#{chkSKU}.getValue()) {isSKU = 1;isSKUText = 'Yes';}


                    if(freeRecord == -1)
                    {
                        record = grid.store.add({
                            Product_UOM_Template_Detail_ID : '00000000-0000-0000-0000-000000000000',
                            Product_UOM_Template_Detail_Name : #{cmbStdUnit}.rawValue,
                            Product_UOM_Template_Detail_Short_Name : #{cmbStdUnit}.getValue(),
                            Product_UOM_Template_Detail_Quantity : #{txtQtyUnit}.getValue(),
                            Product_UOM_Template_Detail_SKU : isSKU,
                            Product_UOM_Template_Detail_SKU_Text : isSKUText,
                            Product_UOM_Template_Detail_Status : 0,
                            Product_UOM_Template_Detail_Weight : #{txtWeightUnit}.getValue(),
                            Product_UOM_Template_Detail_Package_Weight : #{txtPackWeightUnit}.getValue(),
                            Product_UOM_Template_Detail_Gross_Weight : #{txtWeightUnit}.getValue() + #{txtPackWeightUnit}.getValue(),
                            Product_UOM_Template_Detail_Package_Width : #{txtWidth}.getValue(),
                            Product_UOM_Template_Detail_Package_Length : #{txtLength}.getValue(),
                            Product_UOM_Template_Detail_Package_Height : #{txtHeight}.getValue()
                        });
                        freeRecord = grid.store.data.length-1;
                    }

                }

                  #{GridUOM}.getView().refresh();
                  #{hidRowId}.setValue(-99);
                  #{Product_UOM_Detail_ID}.setValue(0);
                App.direct.clearUnitForm();
            };

            var getSelectedRowIndex =  function(grid){
                var r = grid.getSelectionModel().getSelection();
                var s = grid.getStore();
                return s.indexOf(r[0]);
            }

            var grdUnitList_CellDblClick = function() {

                var rowIndex = getSelectedRowIndex(#{GridUOM});
                var rowSelect = #{GridUOM}.getSelectionModel().getSelection();

                //console.log(rowSelect[0].data.Product_UOM_Template_Detail_SKU);

                #{hidRowId}.setValue(rowIndex);
                #{Product_UOM_Detail_ID}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_ID);
                App.direct.setStdCombo(rowSelect[0].data.Product_UOM_Template_Detail_Name, rowSelect[0].data.Product_UOM_Template_Detail_Short_Name);
                #{txtQtyUnit}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Quantity);
                #{txtWeightUnit}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Weight);
                #{txtPackWeightUnit}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Package_Weight);
                #{txtWidth}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Package_Width);
                #{txtLength}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Package_Length);
                #{txtHeight}.setValue(rowSelect[0].data.Product_UOM_Template_Detail_Package_Height);
                if(rowSelect[0].data.Product_UOM_Template_Detail_SKU == 1){
                    #{chkSKU}.setValue(true);
                }else{
                   #{chkSKU}.setValue(false);
                }
                
            };

            var uomDelete = function(delRecord) {

                /*var id = delRecord.data.Product_UOM_Detail_ID;

                if(id > 0)
                {
                  #{DelGrid}.store.add({
                      Product_UOM_Detail_ID : id
                  });

                  #{DelGrid}.getView().refresh();
                }*/
            }

        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:Container Layout="ColumnLayout" runat="server" Padding="3" Region="North" Height="155">
                    <Items>
                        <ext:TextField FieldLabel="<%$ Resource : PRODUCTUOMTEMPLATE_NAME %>" LabelAlign="Right" Width="230" ID="txtTemplateName" LabelWidth="120"
                            runat="server" MarginSpec="5 0 0 0" AllowBlank="false">
                            <Listeners>
                                <Blur Handler="if(this.value =='' || this.value==null){#{btnSave}.setDisabled(true);}else{#{btnSave}.setDisabled(false);};" />
                            </Listeners>
                        </ext:TextField>
                        <ext:Checkbox runat="server" ID="chkActive" FieldLabel="<%$ Resource : ACTIVE %>" LabelAlign="Right" MarginSpec="5 0 0 0" />
                        <ext:Container Layout="ColumnLayout" runat="server" Hidden="false">
                            <Items>
                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server" Padding="3">
                                    <Items>
                                        <ext:Hidden runat="server" ID="Product_UOM_Template_ID" />
                                        <ext:Hidden runat="server" ID="Product_UOM_Detail_ID" />
                                        <ext:Hidden runat="server" ID="hidRowId" />
                                        <ext:FieldSet Title="<%$ Resource : UNIT_DETAIL %>" Flex="1" Layout="FitLayout" runat="server" Padding="5">
                                            <Items>
                                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server">
                                                    <Items>
                                                        <ext:Container Layout="ColumnLayout" Flex="1" runat="server">
                                                            <Items>

                                                             
                                                         <ext:ComboBox Editable="false" ID="cmbStdUnit" runat="server" Width="200" MarginSpec="0 5 0 0"
                                                                    DisplayField="Name" ValueField="ShortName" Flex="1"
                                                                    TriggerAction="Query" FieldLabel="<%$ Resource : UNIT_NAME %>"  AutoShow="false"
                                                                    TypeAhead="true" PageSize="25" MinChars="0" LabelAlign="Right" LabelWidth="110">
                                                                    <ListConfig LoadingText="Searching..." ID="listStdUnit" MaxHeight="150"  >
                                                                        <ItemTpl runat="server">
                                                                            <Html>
                                                                                <div class="search-item">
							                                                {Name}: {ShortName}
						                                                </div>
                                                                            </Html>
                                                                        </ItemTpl>
                                                                    </ListConfig>
                                                                    <Store>
                                                                        <ext:Store ID="StdUnitStore" runat="server" AutoLoad="false">
                                                                            <Proxy>
                                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=UnitCombo">
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
                                                        <ext:TextField FieldLabel="<%$ Resource : UNIT_SHORT_NAME %>" LabelAlign="Right" LabelWidth="110" Width="150" ID="txtUnitShortName" Flex="1" runat="server" MarginSpec="5 0 0 0" ReadOnly="true" />
                                                        <ext:NumberField FieldLabel="<%$ Resource : COUNTING_SKU %>" LabelAlign="Right" Width="180" LabelWidth="110" ID="txtQtyUnit" AllowDecimals="false" MinValue="1" AllowBlank="false" Flex="1" runat="server" MarginSpec="5 0 0 0" />
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:FieldSet>
                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server" Padding="3">
                                    <Items>
                                        <ext:FieldSet runat="server" Title="<%$ Resource : PRODUCT_VALUE %>" Layout="ColumnLayout" Flex="1" DefaultAnchor="90%">
                                            <Items>
                                                <ext:Container runat="server" Layout="AnchorLayout" DefaultAnchor="100%" Padding="5" Width="160">
                                                    <Items>
                                                        <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%">
                                                            <Items>
                                                                <ext:NumberField ID="txtWidth" runat="server" MinValue="1" FieldLabel="<%$ Resource : WIDTH %>" AllowBlank="false" TabIndex="11" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                <ext:Label runat="server" Text="<%$ Resource : CM %>" MarginSpec="0 0 0 5" />
                                                            </Items>
                                                        </ext:Container>

                                                        <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                            <Items>
                                                                <ext:NumberField ID="txtLength" runat="server" MinValue="1" FieldLabel="<%$ Resource : LENGHT %>" AllowBlank="false" TabIndex="12" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                <ext:Label runat="server" Text="<%$ Resource : CM %>" MarginSpec="0 0 0 5" />
                                                            </Items>
                                                        </ext:Container>

                                                        <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                            <Items>
                                                                <ext:NumberField ID="txtHeight" runat="server" MinValue="1" FieldLabel="<%$ Resource : HEIGHT %>" AllowBlank="false" TabIndex="13" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                <ext:Label runat="server" Text="<%$ Resource : CM %>" MarginSpec="0 0 0 5" />
                                                            </Items>
                                                        </ext:Container>

                                                    </Items>
                                                </ext:Container>

                                            </Items>
                                        </ext:FieldSet>

                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server" Padding="3">
                                    <Items>
                                        <ext:FieldSet  Title="<%$ Resource : PRODUCT_WEIGHT %>"  Flex="1" Layout="FitLayout" runat="server" Padding="5">
                                            <Items>
                                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server">
                                                    <Items>

                                                        <ext:NumberField FieldLabel="<%$ Resource : NET_WEIGHT_KG %>" LabelAlign="Right" Width="180" LabelWidth="110" ID="txtWeightUnit" AllowBlank="false" Text="0" Flex="1" MinValue="0" runat="server" MarginSpec="0 0 0 0" />
                                                        <ext:NumberField FieldLabel="<%$ Resource : PACK_WEIGHT_KG %>" LabelAlign="Right" Width="180" LabelWidth="110" ID="txtPackWeightUnit" AllowBlank="false" Text="0" Flex="1" MinValue="0" runat="server" MarginSpec="5 0 5 0" />
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:FieldSet>
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:Checkbox runat="server" FieldLabel="<%$ Resource : DEFAULTUNIT %>" ID="chkSKU" LabelWidth="80" Width="150" LabelAlign="Right">
                                                    <Listeners>
                                                        <Change Fn="sku_change" />
                                                    </Listeners>
                                                </ext:Checkbox>
                                                <ext:Button Text="<%$ Resource : ADD %>" runat="server" Width="50" ID="Button2">
                                                    <Listeners>
                                                        <Click Fn="btnAddStdUnitItem_Click" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:Container>

                <ext:GridPanel ID="GridUOM" runat="server" Layout="FitLayout" Region="Center">
                    <Store>
                        <ext:Store ID="UOMStore" runat="server">
                            <Model>
                                <ext:Model ID="Model6" runat="server">
                                    <Fields>
                              
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Name" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Short_Name" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Quantity" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_SKU" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_SKU_Text" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Status" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Weight" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Package_Weight" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Gross_Weight" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Package_Width" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Package_Length" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_Package_Height" />
                                        <ext:ModelField Name="Product_UOM_Template_Detail_ID" />
                                  
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel6" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" Width="30" Sortable="false">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Unit" CommandName="Delete" />
                                </Commands>
                                <Listeners>
                                    <Command Handler="uomDelete(record);#{UOMStore}.remove(record);" />
                                </Listeners>
                                <%--<DirectEvents>
                                    <Command OnEvent="UnitCommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="id" Value="rowIndex" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete?" Title="Delete" />
                                        <EventMask ShowMask="true" Msg="Processing" />
                                    </Command>
                                </DirectEvents>--%>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Text="<%$ Resource : NUMBER %>" Width="40" Align="Center" />
                            <ext:Column ID="Column26" runat="server" DataIndex="Product_UOM_Template_Detail_Name" Text="<%$ Resource : UNIT_NAME %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="Product_UOM_Template_Detail_Short_Name" Visible="false" />
                            <ext:Column ID="Column21" runat="server" DataIndex="Product_UOM_Template_Detail_Detail_ID" Visible="false" />
                            <ext:Column ID="Column1" runat="server" DataIndex="Product_UOM_Template_Detail_Quantity" Text="<%$ Resource : QTY_SKU_NAME %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="Product_UOM_Template_Detail_Gross_Weight" Text="<%$ Resource : GW_NAME_KG %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Width" Text="<%$ Resource : WIDTH_CM %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column11" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Length" Text="<%$ Resource : LENGTH_CM %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column12" runat="server" DataIndex="Product_UOM_Template_Detail_Package_Height" Text="<%$ Resource : HEIGHT_CM %>" Align="Center" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="Product_UOM_Template_Detail_SKU_Text" Text="<%$ Resource : ISSTORAGEUNIT %>" Align="Center" Flex="1" />

                        </Columns>
                    </ColumnModel>

                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" Mode="Single" />
                    </SelectionModel>

                    <Listeners>
                        <CellDblClick Fn="grdUnitList_CellDblClick" />
                    </Listeners>

                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreUOM" Mode="Raw" Value="Ext.encode(#{GridUOM}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnClear_Click">
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="16">
                                           <DirectEvents>
                                        <Click OnEvent="btnExit_Click">
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>

            </Items>

        </ext:Viewport>
    </form>
</body>
</html>
