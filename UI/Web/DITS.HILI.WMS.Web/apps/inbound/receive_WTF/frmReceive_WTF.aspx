<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReceive_WTF.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.inbound.receive_WTF.frmReceive_WTF" %>

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

            var sumTotal = function () {

                var sumQTY = 0;
                var grid = App.grdDataList;

                for (i = 0; i < grid.store.getCount() ; i++) {
                    sumQTY += grid.store.data.items[i].data.QTY;
                }

                App.txtTotalQTY.setValue(parseFloat(sumQTY).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            };


            var getComboObj = function (combobox) {
                var v = combobox.getValue();
                var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                return recordValue;
            }

            var validateSave = function () {

                var plugin = this.editingPlugin;

                var cmbUnitData = getComboObj(App.cmbProductUnitEdit);
                if (cmbUnitData != false) {
                    App.cmbProductUnitEdit.setRawValue(cmbUnitData.data.Code + ' ');
                    plugin.context.record.set(cmbUnitData.data);
                }

                var cmbStatusData = getComboObj(App.cmbProductStatusEdit);
                if (cmbStatusData != false) {
                    App.cmbProductStatusEdit.setRawValue(cmbStatusData.data.Description + ' ');
                    plugin.context.record.set(cmbStatusData.data);
                }

                sumTotal();
                plugin.completeEdit();
            }

            var beforeEditCheck = function (editor, e) {
                App.direct.LoadEditCombo(e.record.data.UnitID, e.record.data.Unit, e.record.data.StatusID, e.record.data.Status);
            };

            var productUnitChange = function () {
                var recordValue = getComboObj(this);
                App.txtUnitID.setValue(recordValue.data.ProductUnitID);
            };

            var productStatusChange = function () {
                var recordValue = getComboObj(this);
                App.txtStatusID.setValue(recordValue.data.ProductStatusID);
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

                        <ext:Hidden runat="server" ID="hdReceiveID" />
                        <ext:Hidden runat="server" ID="hdLineID" />

                        <ext:FieldSet runat="server" Layout="ColumnLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">
                            <Items>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.25">
                                    <Items>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtReceiveCode"
                                                    Flex="1"
                                                    ReadOnly="true"
                                                    AllowBlank="false"
                                                    FieldLabel="<% $Resource : RECEIVENO %>" />

<%--                                                <ext:Button runat="server" ID="btnSelectReceiveCode" Text="..." Margins="0 0 0 5">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnSelectReceiveCode_Click" />
                                                    </DirectEvents>
                                                </ext:Button>--%>
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="cmbReceiveType"
                                                    FieldLabel="<% $Resource : RECEIVETYPE %>"
                                                    Flex="1"
                                                    EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                    DisplayField="Name"
                                                    ValueField="DocumentTypeID"
                                                    TypeAhead="false"
                                                    MinChars="0"
                                                    TriggerAction="All"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    AllowOnlyWhitespace="false"
                                                    AllowBlank="false"
                                                    PageSize="20">
                                                    <ListConfig MinWidth="300"></ListConfig>

                                                    <Store>
                                                        <ext:Store ID="StoreReceiveType" runat="server" AutoLoad="false" PageSize="20">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ReceiveType">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model2" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="DocumentTypeID" />
                                                                        <ext:ModelField Name="Name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>

                                                    <ToolTips>
                                                        <ext:ToolTip runat="server">
                                                            <Listeners>
                                                                <BeforeShow Fn="
                                                                    function updateTipBody(tip) 
                                                                    {
                                                                            tip.update(#{cmbReceiveType}.getDisplayValue());
                                                                    }">
                                                                </BeforeShow>
                                                            </Listeners>
                                                        </ext:ToolTip>
                                                    </ToolTips>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="cmbLocation"
                                                    FieldLabel="<% $Resource : LOADLOCATION %>"
                                                    Flex="1"
                                                    EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                    DisplayField="Code"
                                                    ValueField="LocationID"
                                                    TypeAhead="false"
                                                    MinChars="0"
                                                    TriggerAction="All"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    AllowOnlyWhitespace="false"
                                                    AllowBlank="false"
                                                    PageSize="20">
                                                    <Store>
                                                        <ext:Store ID="StoreLocation" runat="server" AutoLoad="false" PageSize="20">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=LocationByLine">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model1" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="LocationID" />
                                                                        <ext:ModelField Name="Code" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <Parameters>
                                                                <ext:StoreParameter Name="lineID" Value="#{hdLineID}.getValue()" Mode="Raw" />
                                                            </Parameters>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.25">
                                    <Items>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="cmbProductOwner"
                                                    FieldLabel="<% $Resource : PRODUCTOWNER %>"
                                                    Flex="1"
                                                    EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                    DisplayField="Description"
                                                    ValueField="ProductOwnerID"
                                                    TypeAhead="false"
                                                    MinChars="0"
                                                    TriggerAction="All"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    AllowOnlyWhitespace="false"
                                                    AllowBlank="false"
                                                    PageSize="20">
                                                    <Store>
                                                        <ext:Store ID="StoreProductOwner" runat="server" AutoLoad="false" PageSize="20">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductOwner">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model3" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="ProductOwnerID" />
                                                                        <ext:ModelField Name="Description" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>

                                                    <ToolTips>
                                                        <ext:ToolTip runat="server">
                                                            <Listeners>
                                                                <BeforeShow Fn="
                                                            function updateTipBody(tip) 
                                                            {
                                                                    tip.update(#{cmbProductOwner}.getDisplayValue());
                                                            }">
                                                                </BeforeShow>
                                                            </Listeners>
                                                        </ext:ToolTip>
                                                    </ToolTips>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtSupplier" FieldLabel="<% $Resource : SUPPLIERNAME %>" ReadOnly="true" Flex="1">
                                                    <ToolTips>
                                                        <ext:ToolTip runat="server">
                                                            <Listeners>
                                                                <BeforeShow Fn="
                                                            function updateTipBody(tip) 
                                                            {
                                                                    tip.update(#{txtSupplier}.getValue());
                                                            }">
                                                                </BeforeShow>
                                                            </Listeners>
                                                        </ext:ToolTip>
                                                    </ToolTips>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DateField runat="server"
                                                    ID="dtEstReceiveDate"
                                                    FieldLabel="<% $Resource : RECEIVE_DATE_EST %>"
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    AllowOnlyWhitespace="false"
                                                    Flex="1" >
                                                    <%-- <Listeners>
                                                        <Change Handler="Ext.net.DirectMethods.ValidateEstReceiveDate(this.value);" />
                                                    </Listeners>--%>
                                                </ext:DateField>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.25">
                                    <Items>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtInvoiceNo" FieldLabel="<% $Resource : INVOICENO %>" Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtContainerNo" FieldLabel="<% $Resource : CONTAINERNO %>" Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtPONo" FieldLabel="<% $Resource : PONO %>" Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.25">
                                    <Items>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtOrderNo" FieldLabel="<% $Resource : ORDER_NO %>" Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtRemark" FieldLabel="<% $Resource : REMARK %>" Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:Checkbox runat="server" ID="chkUrgent" FieldLabel="<% $Resource : URGENT %>" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                    </Items>

                    <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); #{btnProduce}.setDisabled(!valid);" />
                    </Listeners>

                </ext:FormPanel>

                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" MarginSpec="0 5 0 5">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ReceiveDetailID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="LotNo" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="EXPDate" Type="Date" />
                                        <ext:ModelField Name="QTY" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="PackageQTY" />
                                        <ext:ModelField Name="ConfirmQTY" />
                                        <ext:ModelField Name="ConversionQTY" />
                                        <ext:ModelField Name="UnitID" />
                                        <ext:ModelField Name="Unit" />
                                        <ext:ModelField Name="StatusID" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="Width" />
                                        <ext:ModelField Name="Length" />
                                        <ext:ModelField Name="Height" />
                                        <ext:ModelField Name="Remark" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <%--                            <ext:CommandColumn runat="server" ID="colProductInfo" Sortable="false" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Zoom" ToolTip-Text='<%$ Resource : OPEN_PRODUCT_DIALOG %>' CommandName="OpenProduct" />
                                </Commands>
                                   <DirectEvents>
                                    <Command OnEvent="ViewProduct_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="ProductSystemCode" Value="record.data.Product_System_Code" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>--%>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' MinWidth="300" Flex="1" />
                            <ext:Column runat="server" DataIndex="LotNo" Text='<%$ Resource : LOTNO %>' />
                            <ext:DateColumn runat="server" DataIndex="MFGDate" Text='<%$ Resource : MFGDATE %>' Format="dd/MM/yyyy">
                                <Editor>
                                    <ext:DateField runat="server" ID="nbMFGDateEdit" Vtype="daterange" Format="dd/MM/yyyy" EnableKeyEvents="true" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn runat="server" DataIndex="EXPDate" Text='<%$ Resource : EXPDATE %>' Format="dd/MM/yyyy">
                                <Editor>
                                    <ext:DateField runat="server" ID="nbEXPDateEdit" Vtype="daterange" Format="dd/MM/yyyy" EnableKeyEvents="true" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:NumberColumn runat="server" DataIndex="QTY" Text="<% $Resource : QTY %>" Format="#,###.00" Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server"
                                        ID="nbQTYEdit"
                                        AllowBlank="false"
                                        SelectOnFocus="true"
                                        DecimalPrecision="3"
                                        AllowDecimals="true"
                                        MinValue="0" />
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn runat="server" DataIndex="RemainQTY" Text="<% $Resource : REMAINQTY %>" Format="#,###.00" Align="Right" />
                            <ext:NumberColumn runat="server" DataIndex="ConfirmQTY" Text="<% $Resource : CONFIRMQTY %>" Format="#,###.00" Align="Right" />

                            <ext:Column runat="server" DataIndex="UnitID" Hidden="true">
                                <Editor>
                                    <ext:TextField ID="txtUnitID" runat="server" />
                                </Editor>
                            </ext:Column>

                            <ext:Column runat="server" DataIndex="Unit" Text='<%$ Resource : UNIT %>'>
                                <Editor>
                                    <ext:ComboBox runat="server"
                                        ID="cmbProductUnitEdit"
                                        EmptyText="<% $Resource : PLEASE_SELECT %>"
                                        DisplayField="Code"
                                        ValueField="ProductUnitID"
                                        TypeAhead="true"
                                        MinChars="0"
                                        TriggerAction="All"
                                        QueryMode="Remote"
                                        AutoShow="false"
                                        AllowOnlyWhitespace="false"
                                        AllowBlank="false">
                                        <Store>
                                            <ext:Store ID="Store1" runat="server" AutoLoad="false">
                                                <Proxy>
                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductUnit">
                                                        <ActionMethods Read="GET" />
                                                        <Reader>
                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                        </Reader>
                                                    </ext:AjaxProxy>
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="ProductUnitID" />
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Barcode" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Parameters>
                                                    <ext:StoreParameter Name="ProductID" Value="App.grdDataList.getSelectionModel().getSelection()[0].data.ProductID" Mode="Raw" />
                                                </Parameters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="productUnitChange" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:Column>

                            <ext:Column runat="server" DataIndex="StatusID" Hidden="true">
                                <Editor>
                                    <ext:TextField ID="txtStatusID" runat="server" />
                                </Editor>
                            </ext:Column>

                            <ext:Column runat="server" DataIndex="Status" Text='<%$ Resource : STATUS %>'>
                                <Editor>
                                    <ext:ComboBox runat="server"
                                        ID="cmbProductStatusEdit"
                                        EmptyText="<% $Resource : PLEASE_SELECT %>"
                                        DisplayField="Description"
                                        ValueField="ProductStatusID"
                                        TypeAhead="true"
                                        MinChars="0"
                                        TriggerAction="All"
                                        QueryMode="Remote"
                                        AutoShow="false"
                                        Editable="false"
                                        AllowOnlyWhitespace="false"
                                        AllowBlank="false">
                                        <Store>
                                            <ext:Store ID="StorePStatus" runat="server" AutoLoad="false">
                                                <Proxy>
                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductStatusByDocType">
                                                        <ActionMethods Read="GET" />
                                                        <Reader>
                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                        </Reader>
                                                    </ext:AjaxProxy>
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="ProductStatusID" />
                                                            <ext:ModelField Name="Description" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Parameters>
                                                    <ext:StoreParameter Name="DocumentTypeID" Value="#{cmbReceiveType}.getValue()" Mode="Raw" />
                                                </Parameters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="productStatusChange" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:Column>
                            <ext:NumberColumn runat="server" DataIndex="PackageQTY" Text="<% $Resource : PACKAGEQTY %>" Format="#,###.00" Align="Right" />
                            <ext:NumberColumn runat="server" DataIndex="Width" Text="<% $Resource : WIDTH %>" Format="#,###.00" Align="Right" />
                            <ext:NumberColumn runat="server" DataIndex="Length" Text="<% $Resource : LENGTH %>" Format="#,###.00" Align="Right" />
                            <ext:NumberColumn runat="server" DataIndex="Height" Text="<% $Resource : HEIGHT %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="Remark" Text='<%$ Resource : REMARK %>'>
                                <Editor>
                                    <ext:TextField runat="server" ID="txtAreaRemark" Align="Center" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" SaveHandler="validateSave" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                    <BottomBar>
                        <ext:Toolbar runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:StatusBar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:TextField runat="server"
                                            ID="txtTotalQTY"
                                            FieldLabel="<% $Resource : TOTALQTY %>"
                                            ReadOnly="true"
                                            LabelAlign="Right"
                                            EmptyText="0.00"
                                            FieldStyle="text-align: right" />
                                    </Items>
                                </ext:StatusBar>

                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />

                                        <%--                                        <ext:Button runat="server" ID="btnPrint" Icon="Printer" Text="<% $Resource : PRINT %>" MarginSpec="0 0 0 5">
                                        </ext:Button>--%>

                                        <ext:Button runat="server" ID="btnProduce" Icon="Accept" Text="<% $Resource : PRODUCE %>" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnSendToProductionControl_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                                    <Confirmation Message="<% $Message : MSG00026 %>" Title="<% $MessageTitle : MSG00026 %>" ConfirmRequest="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button runat="server" ID="btnSave" Icon="Disk" Text="<% $Resource : SAVE %>" MarginSpec="0 0 0 5" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click"
                                                    Before="#{btnSave}.setDisabled(true);"
                                                    Complete="#{btnSave}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                                </Click>
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
    </form>
</body>
</html>
