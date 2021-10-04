<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllProgram.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllProgram" %>

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

                                <ext:ToolbarFill />   
                                <ext:ComboBox runat="server"
                                            ID="cmbPromgramType"
                                            FieldLabel="<%$ Resource : PROGRAM_TYPE %>"
                                            LabelWidth="100"
                                            LabelAlign="Right"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>">
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" RemoteSort="false" RemotePaging="true" AutoLoad="true" PageSize="20">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProgramID" />
                                        <ext:ModelField Name="Code" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="ParentDescription" />
                                        <ext:ModelField Name="Sequence" />
                                        <ext:ModelField Name="Url" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Name" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ProgramID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <%--<ext:Column TagHiddenName="sys_warehousetype" ID="Column1" runat="server" DataIndex="Code" Text='<%$ Resource : WAREHOUSE_TYPE_CODE %>'   Width="200" Align="Center" />--%>
                            <ext:Column TagHiddenName="Code" ID="Column2" runat="server" DataIndex="Code" Text='<%$ Resource : PROGRAM_CODE %>' Width="100" Align="Left" />
                            <ext:Column TagHiddenName="Description" ID="Column1" runat="server" DataIndex="Description" Text='<%$ Resource : DESCRIPTION %>' Flex="1" Align="Left" />
                            <ext:Column TagHiddenName="Url" ID="Url" runat="server" DataIndex="Url" Text="<%$ Resource : URL %>" Align="Left" Flex="1" />
                            <ext:Column TagHiddenName="Sequence" ID="Column3" runat="server" DataIndex="Sequence" Text='<%$ Resource : SEQUENCE %>' Width="80" Align="Right" />
                            <ext:Column TagHiddenName="ParentDescription" ID="Column4" runat="server" DataIndex="ParentDescription" Text='<%$ Resource : MODULE %>' Flex="1" Align="Left" />
                            <ext:CheckColumn TagHiddenName="IsActive" ID="colIsActive" DataIndex="IsActive" Text="<%$ Resource : ACTIVE %>" runat="server"
                                Align="Center" Width="100" />
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
                    <%-- <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data.WarehouseType_Code" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>--%>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
