<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RePrintPalletOutboundList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.RePrintPalletOutboundList" %>

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

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill />

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtMFGDate"
                                            FieldLabel='<%$ Resource : MFGDATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1"
                                            LabelWidth="100"
                                            LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtProductName" FieldLabel="<%$ Resource : PRODUCTNAME %>" LabelWidth="100" LabelAlign="Right">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:ComboBox runat="server"
                                    ID="cmbPONo"
                                    FieldLabel="<% $Resource : PONO %>"
                                    LabelWidth="100"
                                    LabelAlign="Right"
                                    Flex="1"
                                    DisplayField="PONo"
                                    ValueField="PONo"
                                    TypeAhead="false"
                                    MinChars="0"
                                    TriggerAction="All"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    PageSize="20">
                                    <ListConfig LoadingText="Searching..." MinWidth="200" />
                                    <Store>
                                        <ext:Store runat="server" AutoLoad="false" PageSize="20">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=POList">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model4" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="PONo" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>


                                <ext:Button runat="server" ID="btnSearch" Icon="Magnifier" Text='<%$ Resource : SEARCH %>'>
                                    <Listeners>
                                        <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false" AutoLoad="false">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="PackingID" />
                                        <ext:ModelField Name="PickingDetailID" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="UnitID" />
                                        <ext:ModelField Name="Unit" />
                                        <ext:ModelField Name="DOQty" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LineCode" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="cmRePrintRemain" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Printer" ToolTip-Text="tag Remain" CommandName="RePrintRemain" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandRePrintRemainClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                            <ext:Parameter Name="pickingDetailID" Value="record.data.PickingDetailID" Mode="Raw" />
                                            <ext:Parameter Name="printType" Value="2" Mode="Value" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>


                            <ext:CommandColumn runat="server" ID="cmRePrintDO" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="PrinterGo" ToolTip-Text="tag DO" CommandName="RePrintDO" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandRePrintDOClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                            <ext:Parameter Name="pickingDetailID" Value="record.data.PickingDetailID" Mode="Raw" />
                                            <ext:Parameter Name="printType" Value="3" Mode="Value" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn runat="server" Text='<% $Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text="<% $Resource : PALLETTAG %>" MinWidth="200" Flex="1" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<% $Resource : PRODUCT_CODE %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<% $Resource : PRODUCT_NAME %>" Width="200" Flex="1" />
                            <ext:Column runat="server" DataIndex="RemainQTY" Text="<% $Resource : PALLETQTY %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="DOQty" Text="<% $Resource : DOQTY %>" Format="#,###.00" Align="Right" />
                            <ext:DateColumn runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>" Format="dd/MM/yyyy" />
                            <ext:Column runat="server" DataIndex="LineCode" Text="<% $Resource : LINE %>" />
                            <ext:Column runat="server" DataIndex="Unit" Text="<% $Resource : UNIT %>" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="false" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text='<%$ Resource : PAGESIZE %>' />
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
                                    <Listeners>
                                        <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                    </Listeners>
                                </ext:ComboBox>

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
                                                        <ext:ModelField Name="PrinterName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Parameters>
                                                <ext:StoreParameter Name="isGroupName" Value="true" Mode="Value" />
                                            </Parameters>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:Checkbox ID="chkPDF" runat="server" FieldLabel="PDF" LabelWidth="50" />
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>

                    <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="packingID" Value="record.data.PackingID" Mode="Raw" />
                                <ext:Parameter Name="pickingDetailID" Value="record.data.PickingDetailID" Mode="Raw" />
                                <ext:Parameter Name="printType" Value="3" Mode="Value" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
