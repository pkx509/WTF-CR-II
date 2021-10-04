<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddEditRegisTruck.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.registruck.frmAddEditRegisTruck" %>

<%@ Register Src="~/apps/outbound/registruck/_usercontrol/ucDispatchSelect.ascx" TagPrefix="uc1" TagName="ucDispatchSelect" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
     <%-- <script type="text/javascript">
          Ext.Ajax.timeout = 180000; // 1 sec
          Ext.net.DirectEvent.timeout = 180000; // 1 sec 
      </script>--%>
    <ext:XScript runat="server">
        <script>

            var loadComboDock = function () {
                App.cmbDock.clearValue();
                App.direct.LoadComboDock();
            };

            var popupDispatchSelect = function () {
                App.direct.btnBrowsePO_Click();
            };

         
            var validateSave = function () {
                var plugin = this.editingPlugin;
                if (this.getForm().isValid()) { // local validation    

                    plugin.completeEdit();
                }
            };

            var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
                if (record.data.ShippingStatus == 20 || record.data.ShippingStatus == 100) {
                    toolbar.items.getAt(0).setDisabled(true);               
                }
            };
             
        
            var sumDataDeliveryQty = function(){
                var OrderQty = #{editOrder_Qty}.getValue();
                var DeliveryQty = #{editDelivery_Qty}.getValue();
                var RemainQty = #{txtRemain_Qty}.getValue(e.record.data.Remain_Qty);
               
                if (DeliveryQty <= RemainQty)
                {
                    Ext.MessageBox.show({
                        title:'WARNING',
                        msg: 'Delivery Quantity Over!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(button){
                            if(button=='ok'){
                                Ext.getCmp('txtRemain_Qty').focus('', 10); 
                            }
                        }
                    });
                    return;
                }
                if (DeliveryQty == 0)
                {
                     #{DeliveryQty}.setValue(0);
                }
                else
                {
                    #{editDelivery_Qty}.setValue(RemainQty);
                }
               
            };

        </script>
    </ext:XScript>


</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center" AutoScroll="true"
                    BodyPadding="10" Flex="1" Layout="AnchorLayout">
                    <Items>

                        <ext:FieldSet runat="server" Layout="AnchorLayout" MarginSpec="5 5 5 5">

                            <FieldDefaults LabelAlign="Right" LabelWidth="120" InputWidth="200" />

                            <Items>
                                <ext:Container Layout="HBoxLayout" runat="server" MarginSpec="10 10 10 10">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtSearchPO" FieldLabel="Po No.">
                                            <Listeners>
                                                <SpecialKey Handler=" if(e.getKey() == 13){ popupDispatchSelect(); }" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:Button runat="server" ID="btnBrowsePO" Text="..." MarginSpec="0 0 0 10">
                                            <Listeners>
                                                <Click Fn="popupDispatchSelect" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:TextField runat="server" ID="txtSearchShipto" MarginSpec="0 0 0 10" ReadOnly="true" />

                                        <ext:Button runat="server" ID="btnAddPO" Icon="Add" Text="Add Item" MarginSpec="0 0 0 10"></ext:Button>

                                    </Items>
                                </ext:Container>

                            </Items>
                        </ext:FieldSet>

                        <ext:FieldSet runat="server" Layout="AnchorLayout" MarginSpec="5 5 5 5">

                            <FieldDefaults LabelAlign="Right" LabelWidth="120" InputWidth="200" />

                            <Items>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="10 10 10 10">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                 <ext:Hidden ID="txtShippingID" runat="server" />
                                                <ext:TextField runat="server" FieldLabel="Shipping Code" ID="txtShipping_Code" ReadOnly="true" />

                                                <ext:ComboBox
                                                    runat="server"
                                                    ID="cmbRegisterType"
                                                    FieldLabel="Register Type"
                                                    EmptyText="PleaseSelect"
                                                    AllowBlank="false">
                                                    <DirectEvents>
                                                        <Select OnEvent="OrderType_SeleteChange" />
                                                    </DirectEvents>
                                                </ext:ComboBox>

                                                <ext:ComboBox
                                                    ID="cmbTruckType"
                                                    runat="server"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    AllowBlank="false"
                                                    DisplayField="TypeName"
                                                    ValueField="TruckTypeID"
                                                    FieldLabel="Truck Type"
                                                    SelectOnFocus="true"
                                                    Editable="false"
                                                    ForceSelection="true">
                                                    <Store>
                                                        <ext:Store runat="server" ID="StoreTruckType" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckTypeOnly">
                                                                    <ActionMethods Read="POST" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="TruckTypeID" />
                                                                        <ext:ModelField Name="TypeName" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <Listeners>
                                                        <Select Fn="loadComboDock" />
                                                    </Listeners>
                                                </ext:ComboBox>

                                                <ext:ComboBox ID="cmbWarehouseName"
                                                    FieldLabel="Warehouse"
                                                    Editable="false"
                                                    runat="server"
                                                    ReadOnly="true"
                                                    DisplayField="Name"
                                                    ValueField="WarehouseID"
                                                    TriggerAction="All"
                                                    SelectOnFocus="true"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    PageSize="25"
                                                    MinChars="0"
                                                    LabelAlign="Right"
                                                    AllowBlank="false">
                                                    <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                                        {Name}
						                                        </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="true">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model4" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="WarehouseID" />
                                                                        <ext:ModelField Name="Name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <Listeners>
                                                        <Select Fn="loadComboDock" />
                                                    </Listeners>
                                                </ext:ComboBox>

                                                <ext:ComboBox ID="cmbDock"
                                                    runat="server"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    AllowBlank="false"
                                                    DisplayField="DockName"
                                                    ValueField="DockConfigID"
                                                    FieldLabel="Dock"
                                                    TriggerAction="All"
                                                    SelectOnFocus="true"
                                                    ForceSelection="true"
                                                    PageSize="25"
                                                    MinChars="0"
                                                    LabelAlign="Right">
                                                    <Store>
                                                        <ext:Store runat="server" ID="StoreDock" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckType">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="DockConfigID" />
                                                                        <ext:ModelField Name="DockName" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>

                                                            <Parameters>
                                                                <ext:StoreParameter Name="WhKey" Value="#{cmbWarehouseName}.getValue()" Mode="Raw" />
                                                            </Parameters>

                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>

                                                <ext:TextField runat="server" FieldLabel="ทะเบียนรถ" ID="txtTruckNo"  AllowBlank="false" MaxLength="50" />
                                                <ext:TextField runat="server" FieldLabel="Driver Name" ID="txtDriverName" MaxLength="50" />

                                            </Items>
                                        </ext:Container>

                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>

                                                <ext:TextField runat="server" FieldLabel="PO No." ID="txtPONo" MaxLength="30" EnforceMaxLength="true" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Document No." ID="txtDocNo" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Order No." ID="txtOrderNo" MaxLength="30" />
                                                <ext:TextField runat="server" FieldLabel="Container No." ID="txtContainerNo" MaxLength="30" />
                                                <ext:TextField runat="server" FieldLabel="Seal No." ID="txtSeal_No" MaxLength="30" />
                                                <ext:TextField runat="server" FieldLabel="Booking No." ID="txtBookingNo" MaxLength="30" />
                                                <ext:TextField runat="server" FieldLabel="บริษัทขนส่ง" ID="txtCompany" MaxLength="255" />

                                            </Items>
                                        </ext:Container>

                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                <ext:FieldContainer runat="server" FieldLabel="Document Date" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:DateField ID="dtDate" runat="server" MaxLength="10" EnforceMaxLength="true"
                                                            Format="dd/MM/yyyy" EmptyText="dd/MM/yyyy" ReadOnly="true">
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:FieldContainer>

                                                <%-- <ext:ComboBox runat="server"
                                                    ID="cmbShippingTo"
                                                    FieldLabel="Ship to"
                                                    ValueField="ShipToId"
                                                    DisplayField="Name"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    AllowBlank="false"
                                                    AllowOnlyWhitespace="false"
                                                    LabelWidth="120"
                                                    PageSize="20"
                                                    MinChars="0"
                                                    TypeAhead="false"
                                                    TriggerAction="Query"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    TabIndex="3">
                                                    <ListConfig LoadingText="Searching..." ID="ListcmbShippingTo">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
                                                                {Name}						                                
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store runat="server" ID="StoreShipTo" AutoLoad="true">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipTo">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="ShipToId" />
                                                                        <ext:ModelField Name="Name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>--%>

                                                <ext:ComboBox ID="cmbShippingTo"
                                                    runat="server"
                                                    FieldLabel="Ship to"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    DisplayField="Name"
                                                    ValueField="Name"
                                                    AllowBlank="false"
                                                    AllowOnlyWhitespace="false"
                                                    LabelWidth="120"
                                                    PageSize="20"
                                                    MinChars="0"
                                                    TypeAhead="false"
                                                    TriggerAction="All"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    TabIndex="3">
                                                    <ListConfig LoadingText="Searching..." ID="ListcmbShippingTo">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                                        {Name} 
						                                        </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store runat="server" ID="StoreShipTo" AutoLoad="true">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipTo">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="ShipToId" />
                                                                        <ext:ModelField Name="Name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>

                                                <ext:TextField runat="server" ID="txtDispatchCode" FieldLabel="Dispatch Code" MaxLength="20" EnforceMaxLength="true" ReadOnly="true" />
                                                <ext:TextArea runat="server" ID="txtRemark" FieldLabel="Remark" MaxLength="500" EnforceMaxLength="true" />

                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>

                        <ext:Container runat="server" Flex="1">

                            <Items>
                                <ext:GridPanel ID="gridData" runat="server" Region="Center" Frame="true" MinHeight="100">
                                    <Store>
                                        <ext:Store ID="StoreData" runat="server">
                                            <Model>
                                                <ext:Model ID="Model" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ShippingDetailID" />
                                                        <ext:ModelField Name="ShippingID" />
                                                        <ext:ModelField Name="ShippingCode" />
                                                        <ext:ModelField Name="DispatchId" />
                                                        <ext:ModelField Name="DispatchCode" />
                                                        <ext:ModelField Name="ReferenceID" />
                                                        <ext:ModelField Name="ProductID" />
                                                        <ext:ModelField Name="BookingID" />
                                                        <ext:ModelField Name="ProductCode" />
                                                        <ext:ModelField Name="ProductName" />
                                                        <ext:ModelField Name="Order_Qty" />
                                                        <ext:ModelField Name="ShippingQuantity" />
                                                        <ext:ModelField Name="ConfirmQuantity" />
                                                        <ext:ModelField Name="Remain_Qty" />
                                                        <ext:ModelField Name="Delivery_UOM" />
                                                        <ext:ModelField Name="Delivery_UOM_Name" />
                                                        <ext:ModelField Name="Dispatch_Qty" />
                                                        <ext:ModelField Name="ShipptoCode" />
                                                        <ext:ModelField Name="ShippingStatus" />
                                                        <ext:ModelField Name="ShipptoName" />
                                                        <ext:ModelField Name="Shipping_DT" Type="Date" />
                                                        <ext:ModelField Name="Remark" />
                                                        <ext:ModelField Name="DispatchDetail_Status" />
                                                        <ext:ModelField Name="ShippingQuantity" />
                                                        <ext:ModelField Name="ShippingUnitID" />
                                                        <ext:ModelField Name="BasicQuantity" />
                                                        <ext:ModelField Name="BookingQty" />
                                                        <ext:ModelField Name="BasicUnitID" />
                                                        <ext:ModelField Name="BookingStockUnitId" />
                                                        <ext:ModelField Name="TransactionTypeID" />
                                                        <ext:ModelField Name="BookingBaseQty" />
                                                        <ext:ModelField Name="BookingBaseUnitId" />
                                                        <ext:ModelField Name="ProductUnitID" />
                                                        <ext:ModelField Name="ProductUnitName" />
                                                        <ext:ModelField Name="ConversionQty" />
                                                        <ext:ModelField Name="DispatchDetail_Product_Rquantity" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <%--<Sorters>
                                                <ext:DataSorter Property="Shipping_Code" Direction="ASC" />
                                            </Sorters>--%>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel6" runat="server">
                                        <Columns>

                                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                                <Commands>
                                                    <ext:GridCommand Icon="Delete" ToolTip-Text="<%$ Resource : DELETE %>" CommandName="Delete" />
                                                </Commands>
                                                <DirectEvents>
                                                    <Command OnEvent="CommandClick">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="oDataKeyId" Value="record.data.BookingID" Mode="Raw" />
                                                            <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                                        </ExtraParams>
                                                        <Confirmation ConfirmRequest="true" Message="Are you sure to cancel shipping this product?" Title="Delete" />
                                                    </Command>
                                                </DirectEvents>
                                                <PrepareToolbar Fn="prepareToolbarDelete" />
                                            </ext:CommandColumn>
                                            <ext:Column runat="server" DataIndex="BookingBaseQty" Hidden="true" />
                                            <ext:Column runat="server" DataIndex="ConversionQty" Hidden="true" />
                                            <ext:Column runat="server" DataIndex="BasicQuantity" Hidden="true" />
                                            <ext:Column runat="server" DataIndex="BookingBaseQty" Hidden="true" />

                                            <ext:RowNumbererColumn runat="server" Text="<%$ Resource : NUMBER %>" Width="40" Align="Center" />
                                            <ext:Column runat="server" DataIndex="ProductCode" Text="Product Code" Align="Center" Width="150" />
                                            <ext:Column runat="server" DataIndex="ProductName" Text="Product Name" Align="Left" Flex="1" />
                                            <ext:NumberColumn runat="server" DataIndex="BookingQty" Text="Order Qty" Align="Center" Width="80" Format="#,###.000" AllowDecimals="true" DecimalPrecision="3"/>

                                            <ext:NumberColumn ID="DeliveryQty" runat="server" DataIndex="ConfirmQuantity"
                                                Text="Delivery Qty" Width="100" Align="Center" Format="#,###.000">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true"
                                                        ID="editDelivery_Qty" AllowDecimals="true" DecimalPrecision="3" Editable="true" MinValue="0">
                                                    </ext:NumberField>
                                                </Editor>
                                            </ext:NumberColumn>

                                           <%-- <ext:NumberColumn ID="colRemain_Qtyy" runat="server" DataIndex="Remain_Qty"
                                                Text="Remain Qty" Width="100" Align="Center" Format="#,###.000">
                                                <Editor>
                                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true"
                                                        ID="txtRemain_Qty" AllowDecimals="true" DecimalPrecision="3" ReadOnly="true" >
                                                    </ext:NumberField>
                                                </Editor>
                                            </ext:NumberColumn>--%>
                                            <ext:NumberColumn ID="colRemain_Qtyy" runat="server" DataIndex="Remain_Qty" Text="Remain Qty" Width="100" Align="Center" Format="#,###.000" AllowDecimals="true" DecimalPrecision="3" />

                                            <ext:Column runat="server" DataIndex="ProductUnitName" Text="Unit" Align="Center" Width="150" />

                                        </Columns>
                                    </ColumnModel>

                                    <Plugins>
                                        <ext:CellEditing runat="server">
                                            <Listeners>
                                                <BeforeEdit Handler="#{editDelivery_Qty}.setMaxValue(e.record.data.Remain_Qty);" />
                                            </Listeners>
                                        </ext:CellEditing>
                                    </Plugins>

                                    <View>
                                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="false" />
                                    </View>
                                </ext:GridPanel>
                            </Items>
                        </ext:Container>

                    </Items>
                    <%--<Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />
                    </Listeners>--%>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:Button runat="server" ID="btnDeleteAll" Icon="Delete" Text="Delete All" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnDelete_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Delete ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:ToolbarFill ID="TbarFill" runat="server" />

                                <%--<ext:Button ID="btnPickingList" runat="server"
                                    Icon="Report" Text="Picking List" MarginSpec="5,0,0,0">
                                    <DirectEvents>
                                        <Click OnEvent="btnPickingList_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnConsolidateList" runat="server"
                                    Icon="Report" Text="Consolidate Do List" MarginSpec="5,0,0,0">
                                    <DirectEvents>
                                        <Click OnEvent="btnConsolidateList_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>--%>


                                <ext:Button ID="btnAssign" runat="server"
                                    Icon="Disk" Text="Assign Truck" Width="60">
                                    <DirectEvents>
                                        <Click OnEvent="Assign_Click"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="Assign ..." MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="60" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" Hidden="true">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60">
                                    <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                    <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>

        <uc1:ucDispatchSelect runat="server" ID="ucDispatchSelect" />
    </form>
</body>
</html>
