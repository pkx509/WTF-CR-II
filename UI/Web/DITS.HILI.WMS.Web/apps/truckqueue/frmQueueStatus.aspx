<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQueueStatus.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmQueueStatus" %>

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
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : STATUS %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="100">
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
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : QUEUESTATSNAME %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="100">
                                    <Items>
                                        <ext:TextField ID="txtSearchStatus" runat="server"  Name="txtSearchStatus"  Width="150">
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
                                        <ext:ModelField Name="QueueStatusID" />
                                        <ext:ModelField Name="QueueStatusName" />
                                        <ext:ModelField Name="QueueStatusDesc" /> 
                                        <ext:ModelField Name="IsActive" /> 
                                        <ext:ModelField Name="IsWaiting" /> 
                                        <ext:ModelField Name="IsInQueue" /> 
                                        <ext:ModelField Name="IsCompleted" /> 
                                        <ext:ModelField Name="IsCancel" /> 
                                    </Fields>
                                </ext:Model>
                            </Model> 
                             <Sorters>
                                <ext:DataSorter Property="QueueStatusName" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store> 
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Dock" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueStatusID" Mode="Raw" /> 
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueStatusID" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>               
                            <ext:RowNumbererColumn ID="colRowNum" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />  
                            <ext:Column ID="colQueueStatusName" Width="150" runat="server" DataIndex="QueueStatusName" Text="<%$ Resource : QUEUESTATSNAME %>" Align="Center"  />
                            <ext:Column ID="colQueueStatusDesc" Width="150" DataIndex="QueueStatusDesc" runat="server" Text="<%$ Resource : QUEUESTATSDESC %>" Align="Left" />
                            <ext:CheckColumn ID="colIsActive"   Width="150"  DataIndex="IsActive" runat="server" Text="<%$ Resource : QUEUEACTIVE %>" Align="Left" />  
                            <ext:CheckColumn ID="colIsWaiting" Width="150" DataIndex="IsWaiting" runat="server" Text="<%$ Resource : QUEUEISWAITING %>" Align="Left" />  
                            <ext:CheckColumn ID="colIsInQueue" Width="150" DataIndex="IsInQueue" runat="server" Text="<%$ Resource : QUEUEISINQUEUE %>" Align="Left" />  
                            <ext:CheckColumn ID="colIsCompleted" Width="150" DataIndex="IsCompleted" runat="server" Text="<%$ Resource : QUEUEISCOMPLETED %>" Align="Left" />  
                            <ext:CheckColumn ID="colIsCancel" Width="150" DataIndex="IsCancel" runat="server" Text="<%$ Resource : QUEUEISCANCEL %>" Align="Left" />  
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
                                <ext:Parameter Name="oDataKeyId" Value="record.data.QueueStatusID" Mode="Raw" /> 
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