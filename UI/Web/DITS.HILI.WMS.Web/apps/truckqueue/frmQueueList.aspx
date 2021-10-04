<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQueueList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmQueueList" %>
<!DOCTYPE html> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript"> 
        var getRowClass = function (record) { 
            if (record.data.QueueStatusID =="Cancel") {
                return "cancel-row";
            } else if (record.data.QueueStatusID =="Completed") {
                return "completed-row";
            }
        };
        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) { 
            if (!record.data.IsActive || (record.data.QueueStatusID =="Completed")) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };        
         var prepareToolbarUpdate = function (grid, toolbar, rowIndex, record) {
            if (!record.data.IsActive || (record.data.QueueStatusID == "Loading" ||  record.data.QueueStatusID == "Completed"||  record.data.QueueStatusID == "Cancel")) { 
                  toolbar.items.getAt(1).setDisabled(true); 
            }
        };
        var prepareToolbarCallQueue = function (grid, toolbar, rowIndex, record) {
            if (!record.data.IsActive || (
                    record.data.QueueStatusID == "Completed"
                || record.data.QueueStatusID == "Cancel"
                || record.data.QueueStatusID == "Loading"
                || record.data.QueueStatusID == "Register"
                || record.data.QueueStatusID == "WaitingDocument")) {
                toolbar.items.getAt(1).setDisabled(true); 
            }
        };
   </script>
    <style>
        .completed-row .x-grid-cell { 
            background-color: #b6ff00 !important; 
            color: #900; 
        } 

       .cancel-row .x-grid-cell { 
            background-color: #ff6a00 !important; 
            color: #000000;
         }
    </style>
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
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="40">
                                   <Items>
                                        <ext:DateField runat="server"
                                            ID="dtStartDate"
                                            FieldLabel='<%$ Resource : QUEUEREGDATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            AllowBlank="false"
                                            Flex="1" />
                                         <ext:DateField runat="server"
                                            ID="dtEndDate"
                                            FieldLabel='<%$ Resource : QUEUEREGDATETO %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            AllowBlank="false"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : QUEUESTATSU %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="100">
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
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : KEYWORD %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="100">
                                    <Items>
                                        <ext:TextField ID="txtSearchKeyword" runat="server"  Name="txtSearchKeyword"  Width="200">
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
                                        <ext:ModelField Name="QueueId" />
                                        <ext:ModelField Name="QueueNo" />
                                        <ext:ModelField Name="Sequence" /> 
                                        <ext:ModelField Name="EstimateTime" /> 
                                        <ext:ModelField Name="TruckRegNo" /> 
                                       <%-- <ext:ModelField Name="TruckRegProvice" /> --%>
                                        <ext:ModelField Name="PONO" /> 
                                        <ext:ModelField Name="TimeIn" /> 
                                        <ext:ModelField Name="TimeOut" /> 
                                        <ext:ModelField Name="QueueStatus" />
                                        <ext:ModelField Name="QueueStatusID" /> 
                                        <ext:ModelField Name="TruckType" /> 
                                        <ext:ModelField Name="ShipFrom" /> 
                                        <ext:ModelField Name="ShippTo" /> 
                                        <ext:ModelField Name="QueueDock" /> 
                                        <ext:ModelField Name="QueueRegisterType" /> 
                                        <ext:ModelField Name="Remark" /> 
                                        <ext:ModelField Name="IsActive" /> 
                                    </Fields>
                                </ext:Model>
                            </Model> 
                              <Sorters>
                                  <ext:DataSorter Property="TimeIn" Direction="ASC" /> 
                            </Sorters>
                        </ext:Store>
                    </Store> 
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="50">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colUpdate" Text='<%$ Resource : QUEUEUPDATE %>' Sortable="false" Align="Center" Width="75">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" MinWidth="30"  CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                               <PrepareToolbar Fn="prepareToolbarUpdate" />
                            </ext:CommandColumn> 
                           
                            <ext:CommandColumn runat="server" ID="colChangeStatus" Text='<%$ Resource : QUEUECHANGESTS %>' Sortable="false" Align="Center" Width="100">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="ArrowRefresh"  MinWidth="30"  CommandName="ChangeStatus" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn> 

                            <ext:CommandColumn runat="server" ID="colCall" Sortable="false" Align="Center" Text='<%$ Resource : CALLQUEUE %>' Width="75">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="PlayBlue"  MinWidth="30"  CommandName="CALL"  />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.QueueId" Mode="Raw" /> 
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarCallQueue" />
                            </ext:CommandColumn> 

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />  
                            <ext:Column ID="colQueueDockName" Width="150" DataIndex="QueueNo" runat="server" Text="<%$ Resource : QUEUENO %>" Align="Center"  />
                            <ext:Column ID="colQueueDockDesc" Width="150" DataIndex="TruckRegNo" runat="server" Text="<%$ Resource : DRIVINGLICENSE %>" Align="Center" />
                            <ext:Column ID="colTruckType" Width="150" DataIndex="TruckType" runat="server" Text="<%$ Resource : TRUCK %>" Align="Center" />
                            <ext:Column ID="colQueueStatus" Width="150" DataIndex="QueueStatus" runat="server" Text="<%$ Resource : QUEUESTATSU %>" Align="Center" /> 
                            <ext:Column ID="colShipFrom" Width="150" DataIndex="ShipFrom" runat="server" Text="<%$ Resource : QUEUESHIPFROM %>" Align="Center" /> 
                            <ext:Column ID="colShippTo" Width="150" DataIndex="ShippTo" runat="server" Text="<%$ Resource : QUEUESHIPTO %>" Align="Center" /> 
                            <ext:DateColumn ID="colTimeIn" Groupable="false" Align="Center" Format="dd/MM/yyyy HH:mm" Width="120" DataIndex="TimeIn" runat="server" Text="<%$ Resource : QUEUETIMEIN %>"/> 
                            <ext:Column ID="colPONO" Width="150" DataIndex="PONO" runat="server" Text="<%$ Resource : PO_NO %>" Align="Center" /> 
                            <ext:Column ID="colQueueDock" Width="150" DataIndex="QueueDock" runat="server" Text="<%$ Resource : QUEUEDOCK %>" Align="Center" /> 
                            <ext:Column ID="colRemark" Width="150" DataIndex="Remark" runat="server" Text="<%$ Resource : REMARK %>" Align="Center" /> 
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
                                <ext:Parameter Name="oDataKeyId" Value="record.data.QueueId" Mode="Raw" /> 
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" >
                             <GetRowClass Fn="getRowClass" />       
                        </ext:GridView>
                    </View>
                </ext:GridPanel> 
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>