<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllGroup.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllGroup" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <ext:XScript runat="server">
        <script>
           
            var checkbox_default_change = function (datagrid, index, records, v) {

                var grid = #{GridPanelPage};
             
                grid.store.each(function (record) {
                    if (record.data.Code == records.data.Code) {
                        record.set("IsDefault", true);
                    } else {
                        record.set("IsDefault", false);
                    }
                });

                grid.store.commitChanges();
                //grid.getView().refresh(false);

            };

            
            var before_select_row = function(grid, record, index, eOpts) {
                if(record.data.IsCheck == true)
                    return false;
                else
                    return true;
            }

            var before_default_change = function(grid, index,record) {
                var grid = #{GridPanelPage};

                for(i=0;i<grid.store.data.items.length;i++)
                {
                    if(grid.store.data.items[i].raw.IsCheck == true)
                        return false;
                }

                return true;
            }

        </script>
    </ext:XScript>
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
                                        <ext:ModelField Name="GroupID" />
                                        <ext:ModelField Name="GroupName" />
                                        <ext:ModelField Name="GroupDescription" />
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.GroupID" Mode="Raw" />
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.GroupID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <%--<ext:Column TagHiddenName="sys_warehousetype" ID="Column1" runat="server" DataIndex="Code" Text='<%$ Resource : WAREHOUSE_TYPE_CODE %>'   Width="200" Align="Center" />--%>
                            <ext:Column TagHiddenName="GroupName" ID="Column2" runat="server" DataIndex="GroupName" Text='<%$ Resource : NAME %>' Flex="1" Align="Left" />
                            <ext:Column TagHiddenName="GroupDescription" ID="Column1" runat="server" DataIndex="GroupDescription" Text='<%$ Resource : DESCRIPTION %>' Flex="1" Align="Left" />
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
                        <ext:RowSelectionModel ID="SelectModelRules" runat="server" Mode="Single">
                            <DirectEvents>
                                <Select OnEvent="RowRole_Select">
                                    <ExtraParams>
                                        <ext:Parameter Name="GroupID" Value="record.data.GroupID" Mode="Raw" />
                                    </ExtraParams>
                                    <EventMask ShowMask="true" Msg="Loading ..." MinDelay="100" />
                                </Select>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>


                <ext:GridPanel ID="GridPanelPage" runat="server" Margins="0 0 0 0"
                    Region="East" Frame="true" Width="500" DefaultAnchor="100%" AutoScroll="True" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="TBarPage" runat="server">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Text="<%$ Resource : PROGRAM_INFO %>" Icon="Note" />
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
                        <ext:Store ID="StorePages" runat="server" GroupField="Module">
                            <Model>
                                <ext:Model ID="ModelPages" runat="server" IDProperty="PageId">
                                    <Fields>
                                        <ext:ModelField Name="ProgramID" />
                                        <ext:ModelField Name="GroupID" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="Module" />
                                        <ext:ModelField Name="IsCheck" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Sequence" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnPage" runat="server" Region="South">
                        <Columns>
                            <ext:Column ID="ColPgGroup" runat="server" Text="<%$ Resource : PAGE %>" DataIndex="Description" Flex="1" />
                            <ext:CheckColumn Editable="true" runat="server" Align="Center" DataIndex="IsCheck" Width="93" ID="ColPgIsDelete" />
                        </Columns>
                    </ColumnModel>

                    <Features>
                        <ext:GroupingSummary ID="grpPage" runat="server" HideGroupedHeader="true" EnableGroupingMenu="false" />
                    </Features>

                    <View>
                        <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Button ID="btnSelctAll" runat="server" Text="<%$ Resource : SELECT_ALL %>" Icon="Accept" >
                                    <DirectEvents>
                                        <Click OnEvent="btnSelctAll_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridPanelPage}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClearCheckAll" runat="server" Text="<%$ Resource : CLEAR_ALL %>" Icon="PageWhite">
                                    <DirectEvents>
                                        <Click OnEvent="btnClearCheckAll_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridPanelPage}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
