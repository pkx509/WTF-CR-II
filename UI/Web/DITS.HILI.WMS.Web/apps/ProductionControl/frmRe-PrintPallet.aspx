<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRe-PrintPallet.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.ProductionControl.frmRe_PrintPallet" %>

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

        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:Hidden runat="server" ID="hdLineID" />

                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">

                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:TextField runat="server" ID="txtWidth" FieldLabel="Width" Hidden="true" />
                                <ext:TextField runat="server" ID="txtHeight" FieldLabel="Height" Hidden="true" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PackingID" />
                                        <ext:ModelField Name="Sequence" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="UnitID" />
                                        <ext:ModelField Name="Unit" />
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="CompleteQTY" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="MFGTime" />
                                    </Fields>
                                </ext:Model>
                            </Model> 
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text='<%$ Resource : DELETE %>' CommandName="Delete" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandCancelPalletClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                            <ext:Parameter Name="palletCode" Value="record.data.PalletCode" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="cmRePrintRemain" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Printer" ToolTip-Text="tag Remain" CommandName="RePrintRemain" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandRePrintRemainClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                            <ext:Parameter Name="printType" Value="2" Mode="Value" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="cmRePrint" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="PrinterGo" ToolTip-Text="re-print tag" CommandName="RePrint" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandRePrintClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                            <ext:Parameter Name="printType" Value="1" Mode="Value" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server"
                                Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text="<%$ Resource : PALLETCODE %>" MinWidth="150" />
                           <%-- <ext:Column runat="server" DataIndex="Sequence" Text="<%$ Resource : SEQUENCE %>" />--%>
                            <ext:Column runat="server" DataIndex="LineCode" Text="<%$ Resource : LINE %>" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCTCODE %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCTNAME %>" MinWidth="150" Flex="1" />
                            <ext:Column runat="server" DataIndex="Unit" Text="<%$ Resource : UNIT %>" />
                            <ext:Column runat="server" DataIndex="CompleteQTY" Text="<%$ Resource : PRODUCTIONQTY %>" Align="Right" />
                            <ext:Column runat="server" DataIndex="RemainQTY" Text="<%$ Resource : REMAINQTY %>" Align="Right" />
                            <ext:Column runat="server" DataIndex="MFGTime" Text="<%$ Resource : MFGTIME %>" />
                        </Columns>
                    </ColumnModel>

                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:Checkbox ID="chkIsProduction" runat="server" FieldLabel="Production Only" LabelWidth="100" Checked="true">
                                    
                                  <Listeners>
                                        <Change Handler="#{StoreOfDataList}.reload();" Buffer="500" >

                                        </Change>
                                    </Listeners>

                                </ext:Checkbox> 
                       <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="">
                           <%-- <DirectEvents>
                                <Click OnEvent="btnSearch_Event" >
                                     <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                </Click>
                            </DirectEvents>--%>
                           <Listeners>
                                        <Click Handler="#{StoreOfDataList}.reload();" Buffer="500" />
                                    </Listeners>
                            <LoadingState Text="Please Wait..." />
                        </ext:Button>
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
                                    AllowBlank="false" Hidden="true">
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

                    <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    
                    <ViewConfig runat="server">
                        <Listeners>
                             
                        </Listeners>
                    </ViewConfig>
                    <Listeners> 
                       <%-- <AfterRender Handler="#{cmbPrinterLocation}.Show();">

                        </AfterRender>--%>
                    </Listeners>
                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" >
 
                        </ext:GridView>
                    </View>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
