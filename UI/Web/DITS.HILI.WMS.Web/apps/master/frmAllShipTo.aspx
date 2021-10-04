<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllShipTo.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllShipTo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
            <Listeners>
                <DocumentReady Handler="#{btnSearch}.fireEvent('click');" />
            </Listeners>
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true">
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
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
                                    <Listeners>
                                        <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20"
                            RemoteSort="true" RemotePaging="true" AutoLoad="false">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="Name">
                                    <Fields>
                                        <ext:ModelField Name="ShipToId" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="ShortName" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="IsDefault" />
                                        <ext:ModelField Name="IsActive" />
                                        <ext:ModelField Name="BusinessGroup" />
                                        <ext:ModelField Name="RuleId" />
                                        <ext:ModelField Name="RuleName" /> 
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Name" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShipToId" Mode="Raw" />
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShipToId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column ID="colName" runat="server" DataIndex="Name" Text='<%$ Resource : SHIPTO %>' Align="Left" Width="200" TagHiddenName="d" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ShortName" Text='<%$ Resource : QUEUESHIPSNAME %>' Align="Left" Width="200" TagHiddenName="d" />
                            <ext:Column ID="colRuleName" runat="server" DataIndex="RuleName" Text='<%$ Resource : RULE_NAME %>' Align="Left" Width="200" TagHiddenName="w" />
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text="<%$ Resource : ACTIVE %>" runat="server"
                                Align="Center" Width="100" Hidden="true" />
                            <ext:CheckColumn ID="colIsDefault" DataIndex="IsDefault" Text="<%$ Resource : DEFAULT %>" runat="server" Align="Center" Width="100" />

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
                                        <Select Handler="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);
                                                        #{PagingToolbar1}.moveFirst();" />

                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
