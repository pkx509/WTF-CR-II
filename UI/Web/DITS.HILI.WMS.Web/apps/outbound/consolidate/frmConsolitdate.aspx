<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmConsolitdate.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.consolidate.frmConsolitdate" %>

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

                App.direct.LoadComboDock();
            };

            var popupDispatchSelect = function () {
                App.direct.btnBrowsePO_Click();
            };

         
            //var validateSave = function () {
            //    var plugin = this.editingPlugin;
            //    if (this.getForm().isValid()) { // local validation    

            //        plugin.completeEdit();
            //    }
            //};
             
        
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


            var validateSave = function () {  
                var plugin = this.editingPlugin;             
                plugin.completeEdit();               
                App.btnApprove.setDisabled(1);
                sumTotal();
            };

    
            var beforeEditCheck = function(editor, e, eOpts){
                var btnSave = #{btnSave};
                var status = #{txtConsolidateStatusId}.getValue();
                // console.log('status:' + status);
                if(status != 20){
                    e.cancel = true;
                    return;
                }


                // App.direct.SetDefault();

           
            };
            
            function popitup(url, windowName) {

                debugger;

                var browser = navigator.appName;
                if (browser == 'Microsoft Internet Explorer') {
                    window.opener = self;

                }

                newwindow = window.open(url, windowName, 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=900,height=600');
                window.moveTo(0, 0);
                self.close();

                if (window.focus) { newwindow.focus() }
                return false;
            };

        </script>
    </ext:XScript>


</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" BodyPadding="5" Region="North" Frame="true" MaxHeight="500" AutoScroll="false"
                    Margins="3 3 0 3" Layout="AnchorLayout">
                    <Items>
                        <ext:FieldSet runat="server" Layout="AnchorLayout" MarginSpec="5 5 5 5" Title="Register Truck Information" Collapsible="true" Collapsed="false">

                            <FieldDefaults LabelAlign="Right" LabelWidth="120" InputWidth="200" />

                            <Items>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="10 10 10 10">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                <ext:Hidden runat="server" ID="txtConsolidateStatusId"></ext:Hidden>
                                                <ext:TextField runat="server" FieldLabel="Shipping Code" ID="txtShipping_Code" ReadOnly="true" />


                                                <ext:ComboBox
                                                    runat="server"
                                                    ID="cmbRegisterType"
                                                    FieldLabel="Register Type"
                                                    EmptyText="PleaseSelect"
                                                    AllowBlank="false" ReadOnly="true">
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
                                                    ForceSelection="true" ReadOnly="true">
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
                                                    DisplayField="Name"
                                                    ValueField="WarehouseID"
                                                    TriggerAction="All"
                                                    SelectOnFocus="true"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    PageSize="25"
                                                    MinChars="0"
                                                    LabelAlign="Right"
                                                    AllowBlank="false" ReadOnly="true">
                                                    <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name" MaxHeight="150">
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
                                                                    <ActionMethods Read="POST" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model4" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="WarehouseID" />
                                                                        <ext:ModelField Name="Code" />
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
                                                    Editable="false"
                                                    ForceSelection="true" ReadOnly="true">

                                                    <Store>

                                                        <ext:Store runat="server" ID="StoreDock">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckType">
                                                                    <ActionMethods Read="POST" />
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
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>

                                                <ext:TextField runat="server" FieldLabel="ทะเบียนรถ" ID="txtTruckNo" MaxLength="50" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Driver Name" ID="txtDriverName" MaxLength="50" ReadOnly="true" />

                                            </Items>
                                        </ext:Container>

                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>

                                                <ext:TextField runat="server" FieldLabel="PO No." ID="txtPONo" MaxLength="30" EnforceMaxLength="true" ReadOnly="true" />

                                                <ext:TextField runat="server" FieldLabel="Document No." ID="txtDocNo" MaxLength="50" EnforceMaxLength="true" ReadOnly="true" />

                                                <ext:TextField runat="server" FieldLabel="Order No." ID="txtOrderNo" MaxLength="30" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Container No." ID="txtContainerNo" MaxLength="30" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Seal No." ID="txtSeal_No" MaxLength="30" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="Booking No." ID="txtBookingNo" MaxLength="30" ReadOnly="true" />
                                                <ext:TextField runat="server" FieldLabel="บริษัทขนส่ง" ID="txtCompany" MaxLength="255" ReadOnly="true" />

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

                                                <ext:ComboBox runat="server"
                                                    ID="cmbShippingTo"
                                                    MinChars="0"
                                                    TypeAhead="false"
                                                    QueryMode="Remote"
                                                    FieldLabel="Ship to"
                                                    ValueField="ShipToId"
                                                    DisplayField="Name"
                                                    TriggerAction="All"
                                                    SelectOnFocus="true"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    PageSize="25"
                                                    LabelAlign="Right"
                                                    AllowBlank="false" ReadOnly="true">

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
                                                                    <ActionMethods Read="POST" />
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
                                                <%--       AutoShow="false"--%>

                                                <ext:TextField runat="server" ID="txtDispatchCode" FieldLabel="Dispatch Code" MaxLength="12" EnforceMaxLength="true" ReadOnly="true" />

                                                <ext:TextArea runat="server" ID="txtRemark" FieldLabel="Remark" MaxLength="500" EnforceMaxLength="true" ReadOnly="true" />

                                            </Items>
                                        </ext:Container>


                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="gridData" runat="server" Region="Center" Frame="true">
                    <Store>
                        <ext:Store ID="StoreData" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="OrderPick" />
                                        <ext:ModelField Name="ProductId" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="LocationId" />
                                        <ext:ModelField Name="LocationCode" />
                                        <ext:ModelField Name="LocationSuggestId" />
                                        <ext:ModelField Name="LocationSuggestCode" />
                                        <ext:ModelField Name="PickStockUnitId" />
                                        <ext:ModelField Name="PickStockUnitName" />
                                        <ext:ModelField Name="PickStockQty" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="PalletUnitId" />
                                        <ext:ModelField Name="PalletUnitName" />
                                        <ext:ModelField Name="PalletQty" />
                                        <ext:ModelField Name="PickBaseQty" />
                                        <ext:ModelField Name="DeliveryID" />
                                        <ext:ModelField Name="ConsolidateQty" />
                                        <ext:ModelField Name="ConsolidateUnitId" />
                                        <ext:ModelField Name="ConsolidateUnitName" />
                                        <ext:ModelField Name="DispatchCode" />
                                        <ext:ModelField Name="AssignID" />
                                        <ext:ModelField Name="Pono" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                            <%--  <Sorters>
                                                <ext:DataSorter Property="Shipping_Code" Direction="ASC" />
                                            </Sorters>--%>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel6" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25" Hidden="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text='<%$ Resource : DELETE %>' CommandName="Delete" />
                                </Commands>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />

                            <ext:Column runat="server" DataIndex="OrderPick" Text="<%$ Resource : ORDERPICK %>" Width="60">
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCTCODE %>" Width="120" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCTNAME %>" Width="150" />
                            <ext:Column runat="server" DataIndex="LocationSuggestCode" Text="<%$ Resource : LOCSUGGEST %>" Width="100" />
                            <ext:Column runat="server" DataIndex="LocationCode" Text="<%$ Resource : LOCPICK %>" Width="100" />
                            <ext:NumberColumn runat="server" DataIndex="PickStockQty" Text="<% $Resource : PICKQTY %>" Format="#,###.00" Align="Right" Width="100" />
                            <ext:Column runat="server" DataIndex="PickStockUnitName" Text="<%$ Resource : PICKUNIT %>" Width="100" />
                            <ext:NumberColumn runat="server" DataIndex="ConsolidateQty" Text="<% $Resource : CONSOLIDATEQTY %>" Format="#,###.00" Align="Right" Width="100">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true"
                                        ID="txtConsolidateQtyEdit" AllowDecimals="false" MinValue="0">
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            <ext:Column runat="server" DataIndex="ConsolidateUnitName" Text="<%$ Resource : CONSOLIDATEUNIT %>" Width="100" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text="<%$ Resource : PALLETNO %>" Width="100">
                                <%--          <Editor>
                                    <ext:TextField ID="txtPalletNoEdit" runat="server" />
                                </Editor>--%>
                            </ext:Column>
                            <ext:NumberColumn runat="server" DataIndex="PalletQty" Text="<% $Resource : PALLETQTY %>" Format="#,###.00" Align="Right" Width="100" />
                            <ext:Column runat="server" DataIndex="PalletUnit" Text="<%$ Resource : PALLETUNIT %>" Width="100" />
                            <ext:Column runat="server" DataIndex="Dock" Text="<%$ Resource : DOCK %>" Hidden="true" />
                        </Columns>
                    </ColumnModel>

                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="false" />
                    </View>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:RowEditing runat="server" ID="pluginEditConsolidate" Visible="true" AutoCancel="true" SaveHandler="validateSave" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill></ext:ToolbarFill>


                                <ext:Button ID="btnConsolidateDo" runat="server" Icon="Report" Text='<%$ Resource : CONSODOLIST %>' MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnConsolidateDo_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnApprove" runat="server" Icon="Disk"
                                    Text="<%$Resource : APPROVE_CONSOLIDATE%>" Width="150" TabIndex="17">
                                    <DirectEvents>
                                        <Click OnEvent="btnApprove_Click">
                                            <EventMask ShowMask="true" Msg="Approve Consolidate ..." MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <Confirmation Message="Do you want to Approve Consolidate?" Title="Approve Consolidate" ConfirmRequest="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnCancel" runat="server" Icon="Delete" Hidden="true" Text="<%$Resource : CANCEL%>" Width="90" TabIndex="18">
                                    <%--<DirectEvents>
                                                <Click OnEvent="btnCancel_Click">
                                                    <EventMask ShowMask="true" Msg="Booking ..." MinDelay="100" />
                                                    <Confirmation Message="Do you want to Cancel Booking?" Title="Cancel Booking" ConfirmRequest="true" />
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>--%>
                                </ext:Button>

                                <ext:ToolbarSpacer Width="30" />
                                <ext:ToolbarSeparator runat="server" />
                                <ext:ToolbarSpacer Width="30" />

                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="60" Disabled="true" Hidden="false">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{gridData}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true"
                                                Message='<%$ Message :  MSG00025 %>' Title='<%$ MessageTitle :  MSG00025 %>' />
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
                    </BottomBar>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
