<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCycleCountList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.cyclecount.frmCycleCountList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
     <script type="text/javascript">
        // Ext.Ajax.timeout = 180000; // 1 sec
         //Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.CycleCountStatus == 30 || record.data.CycleCountStatus == 100 || record.data.CycleCountStatus == 102) {
                toolbar.items.getAt(0).setDisabled(true);
            }
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.CycleCountStatus == 30 || record.data.CycleCountStatus == 100 || record.data.CycleCountStatus == 102) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };

     </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" Title="Cycle Count List">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="Add New">
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:ToolbarFill />
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DateField runat="server"
                                                    ID="dtStartDate"
                                                    FieldLabel='<%$ Resource : COUNT_START_DATE %>'
                                                    LabelWidth="120"
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DateField runat="server"
                                                    ID="dtEndDate"
                                                    FieldLabel='<%$ Resource : COUNT_END_DATE %>'
                                                    LabelWidth="120"
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    Flex="1" />
                                            </Items>
                                        </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>

                                        <ext:FieldContainer runat="server" FieldLabel="Status" Layout="HBoxLayout" LabelWidth="40">
                                            <Items>
                                                <ext:ComboBox
                                                    ID="cmbStatus"
                                                    runat="server"
                                                    Editable="false"
                                                    DisplayField="Value"
                                                    ValueField="ID"
                                                    EmptyText="PleaseSelect"
                                                    AllowBlank="false"
                                                    PageSize="0"
                                                    MinChars="0"
                                                    Width="130"
                                                    SelectOnFocus="true"
                                                    TypeAhead="false"
                                                    TriggerAction="Query"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    AllowOnlyWhitespace="false" TabIndex="1">
                                                    <ListConfig LoadingText="Searching..." ID="ListComboStatus">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                               {Value}
						                                </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreState" runat="server">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=CycleCountStatus">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model2" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="ID" />
                                                                        <ext:ModelField Name="Value" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <%--<DirectEvents>
                                                        <Select OnEvent="cmbState_Change" />
                                                    </DirectEvents>--%>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:TextField ID="txtSearch" runat="server" EmptyText="SearchWording" Name="DriverFName" LabelWidth="50" Width="200">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">
                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click" CleanRequest="true">
                                                    <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                </Click>

                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>
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
                                        <ext:ModelField Name="Urgent" />
                                        <ext:ModelField Name="CycleCountID" />
                                        <ext:ModelField Name="CycleCountCode" />
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="ZoneName" />
                                        <ext:ModelField Name="WarehouseID" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="CycleCountAssignDate" Type="Date" />
                                        <ext:ModelField Name="CycleCountStartDate" Type="Date" />
                                        <ext:ModelField Name="CycleCountCompleteDate" Type="Date" />
                                        <ext:ModelField Name="CycleCountStatus" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="Remark" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="CycleCountCode" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Job" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.CycleCountCode" Mode="Raw" />
                                            <ext:Parameter Name="CycleCountStatus" Value="record.data.CycleCountStatus" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete?" Title="Delete" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="CheckError" ToolTip-Text="Confirm Cycle Count Job" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.CycleCountCode" Mode="Raw" />
                                            <ext:Parameter Name="CycleCountStatus" Value="record.data.CycleCountStatus" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarConfirm" />
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="CycleCountCode" Text='<%$ Resource : CYCLECOUNT_CODE %>' Align="Center" Width="120" />
                            <ext:Column ID="Column5" runat="server" DataIndex="WarehouseName" Text='<%$ Resource : WAREHOUSE_NAME %>' />
                            <ext:Column ID="Column2" runat="server" DataIndex="Remark" Text='<%$ Resource : REMARK %>' Align="Center" Flex="1" />
                            <ext:DateColumn ID="Column3" runat="server" DataIndex="CycleCountStartDate" Text='<%$ Resource : CYCLECOUNT_START_DATE %>' Align="Center" Format="dd/MM/yyyy" Width="120" />
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="CycleCountCompleteDate" Text='<%$ Resource : CYCLECOUNT_COMPLETE_DATE %>' Align="Center" Width="120" Format="dd/MM/yyyy" />
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="CycleCountAssignDate" Text='<%$ Resource : CYCLECOUNT_ASSIGN_DATE %>' Align="Center" Width="120" Format="dd/MM/yyyy" />
                            <ext:Column ID="Column4" runat="server" DataIndex="Status" Text='<%$ Resource : STATUS %>' Width="130" Align="Center" />
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
                      <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="CycleCountCode" Value="record.data.CycleCountCode" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

