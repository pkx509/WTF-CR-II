<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllUser.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllUser" %>


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
        <ext:Viewport ID="Viewport1" runat="server" Layout="ColumnLayout"  >
            <Items> 
                <ext:Container runat="server" Region="Center" Layout="FitLayout" Margins="0 0 0 0"  Padding="5"  ColumnWidth="0.7" Flex="1">
                    <Items>


                        <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Frame="true" >
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:ToolbarFill />
                                        <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="200">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
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
                                                <ext:ModelField Name="UserID" />
                                                <ext:ModelField Name="UserName" />
                                                <ext:ModelField Name="FirstName" />
                                                <ext:ModelField Name="LastName" />
                                                <ext:ModelField Name="Email" />
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
                                    <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                        <Commands>
                                            <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="CommandClick">
                                                <ExtraParams>
                                                    <ext:Parameter Name="oDataKeyId" Value="record.data.UserID" Mode="Raw" />
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
                                                    <ext:Parameter Name="oDataKeyId" Value="record.data.UserID" Mode="Raw" />
                                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                                </ExtraParams>
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>

                                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                                    <%--<ext:Column TagHiddenName="sys_warehousetype" ID="Column1" runat="server" DataIndex="Code" Text='<%$ Resource : WAREHOUSE_TYPE_CODE %>'   Width="200" Align="Center" />--%>
                                    <ext:Column TagHiddenName="UserName" ID="Column2" runat="server" DataIndex="UserName" Text='<%$ Resource : USER_NAME %>' Flex="1" Align="Left" />
                                    <ext:Column TagHiddenName="FirstName" ID="Column1" runat="server" DataIndex="FirstName" Text='<%$ Resource : FIRST_NAME %>' Flex="1" Align="Left" />
                                    <ext:Column TagHiddenName="LastName" ID="Column3" runat="server" DataIndex="LastName" Text='<%$ Resource : LAST_NAME %>' Flex="1" Align="Left" />
                                    <ext:Column TagHiddenName="Email" ID="Column4" runat="server" DataIndex="E_mail" Text='<%$ Resource : EMAIL %>' Flex="1" Align="Left" />
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
                             <SelectionModel>
                                <ext:RowSelectionModel ID="SelectModelRules" runat="server" Mode="Single">
                                    <DirectEvents>
                                        <Select OnEvent="RowRole_Select">
                                            <ExtraParams>
                                                <ext:Parameter Name="UserID" Value="record.data.UserID" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Loading ..." MinDelay="100" />
                                        </Select>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false"   />
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Region="East" Layout="FitLayout"  Flex="1" Margins="0 0 0 0" ColumnWidth="0.3">
                    <Items>

                        <ext:GridPanel ID="GridPanelPage" runat="server" Margins="10 0 0 0" ColumnWidth="1" AutoScroll="True">
                            <TopBar>
                                <ext:Toolbar ID="TBarPage" runat="server">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" Text="<%$ Resource : ROLE %>" Icon="Note" />
                                        <ext:ToolbarFill runat="server" />
                                        <ext:Button ID="btnSavePage" runat="server" Text="<%$ Resource : SAVE %>" Icon="Disk" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSavePage_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridPanelPage}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnClearPage" runat="server" Text="<%$ Resource : CLEAR %>" Icon="PageWhite">
                                            <DirectEvents>
                                                <Click OnEvent="btnClearPage_Click">
                                                    <EventMask ShowMask="true" Msg="Loading ..." MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StorePages" runat="server">
                                    <Model>
                                        <ext:Model ID="ModelPages" runat="server" IDProperty="PageId">
                                            <Fields>
                                                <ext:ModelField Name="UserID" />
                                                <ext:ModelField Name="RoleID" />
                                                <ext:ModelField Name="RoleName" />
                                                <ext:ModelField Name="IsRole" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Sorters>
                                        <ext:DataSorter Property="RoleName" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnPage" runat="server" Region="South">
                                <Columns>
                                    <ext:Column ID="ColPgGroup" runat="server" Text="<%$ Resource : NAME %>" DataIndex="RoleName" Flex="1" />
                                    <ext:CheckColumn Editable="true" runat="server" Align="Center" DataIndex="IsRole" Width="93" ID="ColPgIsDelete" />
                                </Columns>
                            </ColumnModel>
                            <Features>
                                <ext:GroupingSummary ID="grpPage" runat="server" HideGroupedHeader="true" EnableGroupingMenu="false" />
                            </Features>
                            <View>
                                <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                            </View>
                        </ext:GridPanel>

                        <ext:GridPanel ID="GridGroup" runat="server" Margins="10 0 0 0" ColumnWidth="1" AutoScroll="True">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Label ID="Label3" runat="server" Text="<%$ Resource : USER_GROUP %>" Icon="Note" />
                                        <ext:ToolbarFill runat="server" />
                                        <ext:Button ID="btnSaveGroup" runat="server" Text="<%$ Resource : SAVE %>" Icon="Disk" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSaveGroup_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridGroup}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button2" runat="server" Text="<%$ Resource : CLEAR %>" Icon="PageWhite">
                                            <DirectEvents>
                                                <Click OnEvent="btnClearPage_Click">
                                                    <EventMask ShowMask="true" Msg="Loading ..." MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="Store1" runat="server">
                                    <Model>
                                        <ext:Model ID="Model1" runat="server" IDProperty="PageId">
                                            <Fields>
                                                <ext:ModelField Name="UserID" />
                                                <ext:ModelField Name="GroupID" />
                                                <ext:ModelField Name="GroupName" />
                                                <ext:ModelField Name="IsGroup" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Sorters>
                                        <ext:DataSorter Property="GroupName" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server" Region="South">
                                <Columns>
                                    <ext:Column ID="Column5" runat="server" Text="<%$ Resource : NAME %>" DataIndex="GroupName" Flex="1" />
                                    <ext:CheckColumn Editable="true" runat="server" Align="Center" DataIndex="IsGroup" Width="93" ID="CheckColumn1" />
                                </Columns>
                            </ColumnModel>
                            <Features>
                                <ext:GroupingSummary ID="GroupingSummary1" runat="server" HideGroupedHeader="true" EnableGroupingMenu="false" />
                            </Features>
                            <View>
                                <ext:GridView ID="GridView3" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:Container>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
