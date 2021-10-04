<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPrintPalletSP.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.ProductionControl.frmPrintPalletSP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

     <ext:XScript runat="server">
        <script>

            function popitup(url, windowName) {

                var browser = navigator.appName;
                if (browser == 'Microsoft Internet Explorer') {
                    window.opener = self;

                }
                newwindow = window.open(url, windowName, 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=600,height=400');
                window.moveTo(0, 0);
                self.close();

                if (window.focus) { newwindow.focus() }
                return false;
            }

            var refreshGrid = function () {
                parent.App.direct.RefreshGrid();
                parentAutoLoadControl.close();
            };

        </script>
    </ext:XScript>

    <style>
        .text-size {
            font-size: 180%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server"
                    AutoScroll="true"
                    BodyPadding="3"
                    Region="Center"
                    Frame="true"
                    Layout="ColumnLayout"
                    ButtonAlign="Center"
                    Margins="3 3 0 3">

                    <FieldDefaults LabelAlign="Right" LabelWidth="200" LabelCls="text-size" />

                    <Items>

                        <ext:Hidden runat="server" ID="hdShiftsNo" />
                        <ext:Hidden runat="server" ID="hdControlID" />
                        <ext:Hidden runat="server" ID="hdLineID" />
                        <ext:Hidden runat="server" ID="hdProductID" />

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtProductCode"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : PRODUCT_CODE %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtPalletQTY"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : PALLETQTY %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtCurrentPallet"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : CURRENTPALLET %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtMFGDate"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : MFGDATE %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TimeField runat="server"
                                            ID="tmMFGTime"
                                            Flex="1"
                                            Format="H:mm"
                                            FieldCls="text-size"
                                            Height="40"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : MFGTIME %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtBatchNo"
                                            Flex="1"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : BATCHNO %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtLine"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : LINE %>" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtProductName"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtTotalQTY"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : TOTALQTY %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbUnit"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            FieldLabel="<% $Resource : UNIT %>"
                                            DisplayField="Code"
                                            ValueField="ProductUnitID"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="Query"
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
                                                        <ext:StoreParameter Name="productID" Value="#{hdProductID}.getValue()" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtOrderType"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : EXPORT_LOCAL %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtOrderNo"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : ORDERNO %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtQTYperPallet"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldCls="text-size"
                                            Height="40"
                                            FieldLabel="<% $Resource : TOTALQTYPERPALLET %>" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:NumberField runat="server"
                                            ID="nbQTYperPallet"
                                            Flex="1"
                                            FieldCls="text-size"
                                            Height="40"
                                            MinValue="0"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : QTYPERPALLET %>" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>

                    <Buttons>
                        <ext:ImageButton ID="btnSave"
                            runat="server"
                            ImageUrl="~/resources/images/printer.png"
                            ToolTip="Print Tag"
                            Width="100"
                            Height="100">
                            <DirectEvents>
                                <Click OnEvent="btnSave_Click" Buffer="350">
                                    <EventMask ShowMask="true" Msg="Printing ..." MinDelay="100" />
                                </Click>
                            </DirectEvents>
                        </ext:ImageButton>

                        <ext:ImageButton ID="btnExit"
                            runat="server"
                            Margins="0 0 50 100"
                            ToolTip="Exit"
                            ImageUrl="~/resources/images/no.png"
                            Width="100"
                            Height="100">
                            <Listeners>
                                <Click fn="refreshGrid" />
                            </Listeners>
                        </ext:ImageButton>
                    </Buttons>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill />

                                <ext:ComboBox runat="server"
                                    ID="cmbPrinterLocation"
                                    EmptyText="<% $Resource : PLEASE_SELECT %>"
                                    FieldLabel="<% $Resource : PRINTER_LOCATION %>"
                                    DisplayField="PrinterName"
                                    ValueField="PrinterId"
                                    TypeAhead="true"
                                    MinChars="0"
                                    TriggerAction="All"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    AllowOnlyWhitespace="false"
                                    AllowBlank="false">
                                    <Store>
                                        <ext:Store ID="StorePStatus" runat="server" AutoLoad="false">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Printer">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="PrinterId" />
                                                        <ext:ModelField Name="PrinterName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Parameters>
                                                <%--<ext:StoreParameter Name="lineID" Value="3CCA9B6F-6E66-E711-80BF-0A94EF0B2DDF" Mode="Value" />--%>
                                                <ext:StoreParameter Name="lineID" Value="#{hdLineID}.getValue()" Mode="Raw" />
                                            </Parameters>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:Checkbox ID="chkPDF" runat="server" FieldLabel="PDF" LabelWidth="50" />
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
