<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDailyPlan.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.dailyPlan.frmDailyPlan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <title></title>
   <%-- <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
    </script>--%>
    <ext:XScript runat="server">
        <script>

            var getComboObj = function (combobox) {
                var v = combobox.getValue();
                var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                return recordValue;
            }

            var cmbProductUnit_Change = function () {
                var recordValue = getComboObj(this);
                App.nbPalletQty.setValue(recordValue.data.PalletQTY);
            };

            var cmbProduct_Change = function () {
                App.cmbUnit.clearValue();
                App.StoreUnit.load();
                var recordValue = getComboObj(this);
                //App.nbPalletQty.setValue(recordValue.data.PalletQTY);
            };

            var cmbLine_Change = function () {
                var recordValue = getComboObj(this);
                //App.nbPalletQty.setValue(recordValue.data.PalletQTY);
            };

            var cmbOrderType_Change = function () {
                var recordValue = getComboObj(this);
                //debugger;
                if (recordValue.data.field1 == 'EXPORT') {
                    App.txtOrderNo.focus();
                    App.txtOrderNo.allowBlank = 0;

                }
                else {
                    App.txtOrderNo.focus();
                    App.txtOrderNo.allowBlank = 1;
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

                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="true"
                    BodyPadding="3"
                    Region="North"
                    Frame="true"
                    Layout="ColumnLayout"
                    Margins="3 3 0 3">

                    <FieldDefaults LabelAlign="Right" LabelWidth="150" />

                    <Items>

                        <ext:Hidden runat="server" ID="hdSection" />
                        <ext:Hidden runat="server" ID="hdProductionDetailID" />
                        <ext:Hidden runat="server" ID="hdDailyPlanStatus" />

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtProductionDate"
                                            FieldLabel="<% $Resource : PRODUCTION_DATE %>"
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:NumberField runat="server"
                                            ID="nbOrderSeq"
                                            FieldLabel="<% $Resource : ORDER_SEQ %>"
                                            Flex="1"
                                            MinValue="1"
                                            EmptyNumber="0"
                                            AllowDecimals="false"
                                            AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:NumberField runat="server"
                                            ID="nbWeight"
                                            AllowDecimals="false"
                                            EmptyNumber="0"
                                            FieldLabel="<% $Resource : WEIGHT %>"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtOrderNo" FieldLabel="<% $Resource : ORDER_NO %>" Flex="1" Regex="^[A-Za-z0-9_/_ ]+$" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtFilm" FieldLabel="<% $Resource : FILM %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtPowder" FieldLabel="<% $Resource : POWDER %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtFD" FieldLabel="<% $Resource : FD %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtSticker" FieldLabel="Sticker" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtDeliveryDate"
                                            FieldLabel="<% $Resource : DELIVER_DATE %>"
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtIngredients" FieldLabel="<% $Resource : INGREDIENT %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtWorkingTime" FieldLabel="<% $Resource : WORKING_TIME %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbLine"
                                            FieldLabel="<% $Resource : LINE %>"
                                            Flex="1"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="LineCode"
                                            ValueField="LineID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="false"
                                            ForceSelection="true"
                                            AllowBlank="false"
                                            PageSize="20">
                                            <Store>
                                                <ext:Store runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Line">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="LineID" />
                                                                <ext:ModelField Name="LineCode" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="LineType" Value="#{hdSection}.getValue()" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Fn="cmbLine_Change" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbProduct"
                                            FieldLabel="<% $Resource : PRODUCTNAME %>"
                                            Flex="1"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="ProductCode"
                                            ValueField="ProductID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="false"
                                            AllowBlank="false"
                                            ForceSelection="true"
                                            PageSize="20">
                                            <ListConfig LoadingText="Searching..." ID="ListCmbProduct" MinWidth="500">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item"> {ProductCode}
						                        </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=productbystockcode">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model1" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ProductID" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="ProductCode" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Fn="cmbProduct_Change" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:NumberField runat="server"
                                            ID="nbQty"
                                            FieldLabel="<% $Resource : QTY %>"
                                            Flex="1"
                                            MinValue="1"
                                            EmptyNumber="0"
                                            AllowDecimals="false"
                                            AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbUnit"
                                            FieldLabel="<% $Resource : UNIT %>"
                                            Flex="1"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="Name"
                                            ValueField="ProductUnitID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            ForceSelection="true"
                                            AllowOnlyWhitespace="false"
                                            AllowBlank="false"
                                            PageSize="20">
                                            <Store>
                                                <ext:Store ID="StoreUnit" runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductUnit">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model3" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ProductUnitID" />
                                                                <ext:ModelField Name="Code" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="Barcode" />
                                                                <ext:ModelField Name="PalletQTY" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="ProductID" Value="#{cmbProduct}.getValue()" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Fn="cmbProductUnit_Change" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:NumberField runat="server"
                                            ID="nbPalletQty"
                                            FieldLabel="<% $Resource : PALLETQTY %>"
                                            Flex="1"
                                            MinValue="1"
                                            EmptyNumber="0"
                                            AllowDecimals="false"
                                            AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbOrderType"
                                            FieldLabel="<% $Resource : ORDER_TYPE %>"
                                            Flex="1"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            AllowOnlyWhitespace="false"
                                            AllowBlank="false"
                                            ForceSelection="true"
                                            TypeAhead="true">
                                            <Items>
                                                <ext:ListItem Text="LOCAL" Value="LOCAL" />
                                                <ext:ListItem Text="EXPORT" Value="EXPORT" />
                                            </Items>
                                            <Listeners>
                                                <Select Fn="cmbOrderType_Change" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtBox" FieldLabel="<% $Resource : BOX %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtOil" FieldLabel="<% $Resource : OIL %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtStamp" FieldLabel="<% $Resource : STAMP %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtMark" FieldLabel="<% $Resource : MARK %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtRemark" FieldLabel="<% $Resource : REMARK %>" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>

                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill runat="server" />

                                <ext:Button ID="btnMockup" runat="server" Icon="Bug" Text="Mockupdata" Hidden="true" Width="80" MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnMockup_Click" />
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="<% $Resource : SAVE %>" MarginSpec="0 0 0 5" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
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

    </form>
</body>
</html>
