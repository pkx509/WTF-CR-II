<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmAdjustList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.adjust.frmAdjustList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Adjust List</title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

     <script type="text/javascript">
         //Ext.Ajax.timeout = 180000; // 1 sec
         //Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.AdjustStatus == 100) {
                toolbar.items.getAt(0).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.AdjustStatus == 77) {
                toolbar.items.getAt(1).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };

     </script>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" Title="Adjust List">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text='<%$ Resource : ADD_NEW %>'>
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:ToolbarFill />

                                <%--                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
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
                                                    <DirectEvents>
                                                        <Select OnEvent="cmbState_Change" />
                                                    </DirectEvents>
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
                                </ext:FieldContainer>--%>

                                <ext:FieldContainer runat="server" FieldLabel="Status" Layout="HBoxLayout" LabelWidth="40">
                                    <Items>
                                        <ext:ComboBox ID="cmbStatus" runat="server" Editable="false"
                                            EmptyText='<%$ Resource : PLEASE_SELECT %>' AllowBlank="false" Width="130">
                                            <DirectEvents>
                                                <Select OnEvent="cmbState_Change" />
                                            </DirectEvents>
                                        </ext:ComboBox>

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
                        <ext:Store ID="StoreOfDataList" runat="server"  PageSize="20" RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AdjustID" />
                                        <ext:ModelField Name="AdjustCode" />
                                        <ext:ModelField Name="ReferenceDoc" />
                                        <ext:ModelField Name="ReferenceDoc" />
                                        <ext:ModelField Name="AdjustTypeName" />
                                        <ext:ModelField Name="AdjustStatusName" />
                                        <ext:ModelField Name="AdjustDate" Type="Date" />
                                        <ext:ModelField Name="AdjustStartDate" Type="Date" />
                                        <ext:ModelField Name="AdjustCompleteDate" Type="Date" />
                                        <ext:ModelField Name="AdjustStatus" />
                                        <ext:ModelField Name="Remark" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="AdjustCode" Direction="ASC" />
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.AdjustCode" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message="Are you sure you want to delete?" Title="Delete" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit Adjsut Job" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.AdjustCode" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarConfirm" />
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="AdjustCode" Text='<%$ Resource : ADJUST_CODE %>' Align="Center" Width="200" />
                            <ext:Column ID="Column2" runat="server" Align="Center" Width="160" DataIndex="AdjustTypeName" Text='<%$ Resource : ADJUST_TYPE %>' />
                            <ext:Column ID="colRefered" runat="server" Align="Center" Width="160" DataIndex="ReferenceDoc" Text='<%$ Resource : REFERENCE_DOC %>' />
                            <ext:Column ID="colRemark" runat="server" Align="Center" Width="200" DataIndex="Remark" Text='<%$ Resource : REMART %>' />
                            <ext:DateColumn ID="Column3" runat="server" DataIndex="AdjustStartDate" Text='<%$ Resource : ADJUST_START_DATE %>' Align="Center" Format="dd/MM/yyyy" Width="250" Flex="1" />
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="AdjustCompleteDate" Text='<%$ Resource : ADJUST_COMPLETE_DATE %>' Align="Center" Width="250" Format="dd/MM/yyyy" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="AdjustStatusName" Text='<%$ Resource : ADJUST_STATUS %>' Align="Center" Width="100" />
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

                    <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.AdjustCode" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

