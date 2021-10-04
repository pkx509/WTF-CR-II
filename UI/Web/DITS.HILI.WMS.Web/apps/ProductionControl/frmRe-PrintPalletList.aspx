<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRe-PrintPalletList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.ProductionControl.frmRe_PrintPalletList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="grdPacked" runat="server" Region="Center" >

                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Hidden runat="server" ID="hdLineType" />

                                <ext:ToolbarFill />
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtPlanDate"
                                            FieldLabel="<% $Resource : PLANDATE %>"
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            LabelAlign="Right"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                    <ext:TextField runat="server" ID="txtPallet" FieldLabel="<%$ Resource : PALLETCODE %>"  
                                            LabelAlign="Right"/>
                                    </Items>
                                </ext:FieldContainer>
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
                                            TriggerAction="Query"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            LabelAlign="Right"
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
                                                        <ext:StoreParameter Name="LineType" Value="#{hdLineType}.getValue()" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:Button runat="server" ID="btnSearch" Icon="Magnifier" Text='<%$ Resource : SEARCH %>'>
                                    <Listeners>
                                        <Click Handler="#{PagingToolbar2}.moveFirst();" Buffer="500" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StorePacked" runat="server" PageSize="20" RemoteSort="false" AutoLoad="false">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindPackedData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ControlID" />
                                        <ext:ModelField Name="PackingID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="QTY" />
                                        <ext:ModelField Name="CompleteQTY" />
                                        <ext:ModelField Name="StartTime" />
                                        <ext:ModelField Name="EndTime" />
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="Unit" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="cmbPackedTag" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="TagGreen" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandPackedClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="controlID" Value="record.data.ControlID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn runat="server" Text='<% $Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<% $Resource : PRODUCT_CODE %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<% $Resource : PRODUCT_NAME %>" Width="150" Flex="1" />
                            <ext:Column runat="server" DataIndex="QTY" Text="<% $Resource : QTY %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="CompleteQTY" Text="<% $Resource : COMPLETEQTY %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="StartTime" Text="<% $Resource : STARTTIME %>" />
                            <ext:Column runat="server" DataIndex="EndTime" Text="<% $Resource : ENDTIME %>" />
                            <ext:Column runat="server" DataIndex="LineCode" Text="<% $Resource : LINE %>" />
                            <ext:Column runat="server" DataIndex="OrderNo" Text="<% $Resource : ORDERNO %>" />
                            <ext:Column runat="server" DataIndex="Unit" Text="<% $Resource : UNIT %>" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label runat="server" Text='<%$ Resource : PAGESIZE %>' />
                                <ext:ToolbarSpacer runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList2" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="20" />
                                    </SelectedItems>
                                    <Listeners>
                                        <Select Handler="#{PagingToolbar2}.moveFirst();" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>

                    <DirectEvents>
                        <CellDblClick OnEvent="grdPacked_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="controlID" Value="record.data.ControlID" Mode="Raw" />
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
