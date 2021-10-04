<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmAllLocation.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllLocation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>

                                <ext:ToolbarFill />
                                <ext:Checkbox ID="ckbIsActive" runat="server" FieldLabel='<%$ Resource : SHOW_ALL %>' LabelWidth="50" Width="100" Name="IsActive" Checked="true">
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Event" />
                                    </DirectEvents>
                                </ext:Checkbox>
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>

                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="<%$ Resource: SEARCH_WORDING %>" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" AutoLoad="true" GroupField="WarehouseName">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="ZoneName" />
                                        <ext:ModelField Name="WarehouseID" />
                                        <ext:ModelField Name="WarehouseCode" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="LocationType" />
                                        <ext:ModelField Name="Code" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="RowNo" />
                                        <ext:ModelField Name="ColumnNo" />
                                        <ext:ModelField Name="LevelNo" />
                                        <ext:ModelField Name="Location_Capacity" />
                                        <ext:ModelField Name="Width" />
                                        <ext:ModelField Name="Length" />
                                        <ext:ModelField Name="Height" />
                                        <ext:ModelField Name="Weight" />
                                        <ext:ModelField Name="ReservWeight" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Code" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <Features>
                        <ext:GroupingSummary
                            ID="group"
                            ShowSummaryRow="false"
                            runat="server"
                            GroupHeaderTplString='{name}'
                            HideGroupedHeader="true"
                            EnableGroupingMenu="true" />
                    </Features>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20" Locked="true">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LocationID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Width="30" Locked="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LocationID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column ID="Column2" runat="server" DataIndex="WarehouseName" Width="120" Text="<%$ Resource: WAREHOUSE_NAME %>" Align="Center" Locked="true" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ZoneName" Width="120" Text="<%$ Resource: ZONE_NAME %>" Align="Left" Locked="true" />
                            <ext:Column ID="Column1" runat="server" DataIndex="Code" Text="<%$ Resource: LOCATION_CODE %>" Width="150" Align="Left" Locked="true" />
                            <ext:Column ID="Column5" runat="server" DataIndex="RowNo" Text="<%$ Resource: LOCATION_ROW %>" Align="Center" />
                            <ext:Column ID="Column6" runat="server" DataIndex="ColumnNo" Text="<%$ Resource: LOCATION_COLUMN %>" Align="Center" />
                            <ext:Column ID="Column7" runat="server" DataIndex="LevelNo" Text="<%$ Resource: LOCATION_LEVEL %>" Align="Center" />
                            <ext:Column ID="Column9" runat="server" DataIndex="Weight" Text="<%$ Resource: LOCATION_WEIGHT %>" Align="Center" />
                            <ext:Column ID="Column10" runat="server" DataIndex="Width" Text="<%$ Resource: LOCATION_WIDTH %>" Align="Center" />
                            <ext:Column ID="Column11" runat="server" DataIndex="Length" Text="<%$ Resource: LOCATION_LENGTH %>" Align="Center" />
                            <ext:Column ID="Column12" runat="server" DataIndex="Height" Text="<%$ Resource: LOCATION_HEIGHT %>" Align="Center" />

                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <%--<DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.Location_No" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel>--%>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText='<%$ Resource : LOADING %>' LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
