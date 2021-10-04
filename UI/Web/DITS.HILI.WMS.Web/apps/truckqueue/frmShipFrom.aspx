<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShipFrom.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmShipFrom" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/Scripts/WmsOnline.css" rel="stylesheet" /> 
    <script type="text/javascript"> 
        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (!record.data.IsActive) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };
   </script>
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
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : STATUS %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="40">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="txtSearchDockStatus"
                                            LabelWidth="40"
                                            LabelAlign="Right"
                                            Width="120"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>">
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items> 
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : QUEUESHIPNAME %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="150">
                                    <Items>
                                        <ext:TextField ID="txtCompanyName" runat="server"  Name="txtCompanyName"  Width="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                   <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : QUEUESHIPSNAME %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="150">
                                    <Items>
                                        <ext:TextField ID="txtCompanyShortName" runat="server"  Name="txtCompanyShortName"  Width="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource : SEARCH %>"> 
                                    <Listeners>
                                        <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false"> 
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields> 
                                        <ext:ModelField Name="ShipFromId" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="ShortName" /> 
                                        <ext:ModelField Name="Description" /> 
                                         <ext:ModelField Name="Address" /> 
                                        <ext:ModelField Name="IsActive" /> 
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
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShipFromId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true" Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShipFromId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn> 
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />  
                            <ext:Column ID="colName" Width="150" DataIndex="Name" runat="server" Text="<%$ Resource : QUEUESHIPNAME %>" Align="Center"  />
                            <ext:Column ID="colShortName" Width="150" DataIndex="ShortName" runat="server" Text="<%$ Resource : QUEUESHIPSNAME %>" Align="Left" />
                            <ext:Column ID="colAddress" Width="150" DataIndex="Address" runat="server" Text="<%$ Resource : QUEUESHIPADDR %>" Align="Left" />
                            <ext:CheckColumn ID="colIsActive" Width="150" DataIndex="IsActive" runat="server" Text="<%$ Resource : QUEUEACTIVE %>" Align="Left" />  
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="<%$ Resource : PAGESIZE %>" />
                                <ext:ToolbarSpacer ID="TbarSpacer" runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="10" />
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="10" />
                                    </SelectedItems>
                                    <Listeners>
                                        <Select Handler="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10); #{PagingToolbar1}.moveFirst();" /> 
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.ShipFromId" Mode="Raw" /> 
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" />
                    </View>
                </ext:GridPanel> 
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>