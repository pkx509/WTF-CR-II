<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllPhysicalZone.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllPhysicalZone" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
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
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="DriverFName" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" RemoteSort="true" RemotePaging="true" AutoLoad="true" PageSize="10" GroupField="WarehouseName">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="Code" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="ZoneTypeName" />
                                        <ext:ModelField Name="ShortName" />
                                        <ext:ModelField Name="KeyType" />
                                        <ext:ModelField Name="IsActive" />                                       
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

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ZoneID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ZoneID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="Code" Text='<%$ Resource : ZONE_CODE %>' Align="Center" />
                            <ext:Column ID="Column2" runat="server" DataIndex="Name" Text='<%$ Resource : ZONE_NAME %>' Align="Left" Flex="1" />
                        <%--<ext:Column ID="Column3" runat="server" DataIndex="Warehouse_Name" Text='<%$ Resource : WAREHOUSE_NAME %>' Align="Left" Flex="1" />--%>
                            <ext:Column ID="Column4" runat="server" DataIndex="ZoneTypeName" Text='<%$ Resource : ZONE_TYPE_NAME %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="ShortName" Text='<%$ Resource : ZONE_SHORT_NAME %>' Align="Center" />
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text="<%$ Resource : ACTIVE %>" runat="server" Align="Center" Width="100" />

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
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Click" />
                                    </DirectEvents>
                                    <Listeners>
                                        <Select Handler="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10); #{grdDataList}.store.reload();" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                    <%-- <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.PhysicalZone_Code" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>--%>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading..." LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
