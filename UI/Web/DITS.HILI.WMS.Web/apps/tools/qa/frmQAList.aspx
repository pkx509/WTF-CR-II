<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmQAList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmQAList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />


    <script type="text/javascript">
        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.Status != 'Draft' && record.data.Status != 'New') {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Default" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Title="Putaway List">
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

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox ID="cmbStatus" runat="server" Flex="1" EmptyText="---Please Select---" />

                                        <ext:TextField ID="txtSearch" runat="server" Flex="1" MarginSpec="0 0 0 5" Width="250" EmptyText="Putaway No./Warehouse" />
                                        <ext:Button ID="btnSearch" runat="server" Text="Search" MarginSpec="0 0 0 5" Icon="Magnifier">
                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click"></Click>
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
                                <ext:AjaxProxy Url="">
                                    <ActionMethods Read="POST" />
                                    <Reader>
                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="PutAwayJobCode">
                                    <Fields>
                                        <ext:ModelField Name="PutAwayID" />
                                        <ext:ModelField Name="PutAwayJobCode" />
                                        <ext:ModelField Name="MethodName" />
                                        <ext:ModelField Name="PutAwayDate" Type="Date" />
                                        <ext:ModelField Name="FinishDate" Type="Date" />
                                        <ext:ModelField Name="FromLocationID" />
                                        <ext:ModelField Name="SuggestionLocationID" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="FromLocationName" ModelName="FromLocation" Mapping="FromLocation.Code" />
                                        <ext:ModelField Name="FromLocation" ModelName="FromLocation" ServerMapping="FromLocation" />
                                        <ext:ModelField Name="SuggestionLocationName" ModelName="SuggestionLocation" ServerMapping="SuggestionLocation.Code" />
                                        <ext:ModelField Name="SuggestionLocation" ModelName="SuggestionLocation" ServerMapping="SuggestionLocation" />
                                        <ext:ModelField Name="SuggestionLocation" />
                                        <ext:ModelField Name="PutAwayJobMatchCollection" />
                                        <ext:ModelField Name="PutAwayConfirmCollection" />
                                        <ext:ModelField Name="PutAwayDetailCollection" />
                                        <ext:ModelField Name="AssignJobCollection" />
                                        <ext:ModelField Name="WarehouseName" ModelName="FromLocation" Mapping="FromLocation.Zone.Warehouse.Name" />



                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <%-- <ext:DataSorter Property="op_Putaway.Putaway_Code" Direction="ASC" />--%>
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">

                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No." Width="60" Align="Center" />
                            <ext:CommandColumn runat="server" ID="CommandColumn1" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Cancel Putaway" CommandName="cancel" />
                                    <ext:CommandFill />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataPutAwayJobCode" Value="record.data.PutAwayJobCode" Mode="Raw" />
                                            <ext:Parameter Name="oDataPutawayStatus" Value="record.data.Status" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message="Are you sure you want to cancel?" Title="Cancel" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="CommandColumn2" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="ApplicationEdit" ToolTip-Text="Edit Putaway" CommandName="edit" />
                                    <ext:CommandFill />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataPutAwayJobCode" Value="record.data.PutAwayJobCode" Mode="Raw" />
                                            <ext:Parameter Name="oDataPutawayStatus" Value="record.data.Status" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>

                                    </Command>
                                </DirectEvents>
                                <%-- <PrepareToolbar Fn="prepareToolbarDelete" />--%>
                            </ext:CommandColumn>


                            <ext:Column ID="colPutawayNo" runat="server" DataIndex="PutAwayJobCode" Text="QA No."
                                Align="Center" Width="150" />

                            <ext:Column ID="colWarehouse" runat="server" DataIndex="DocumentRef" Text="Document Ref"
                                Align="left" Flex="1" />

                            <ext:DateColumn ID="colCreateDate" runat="server" DataIndex="CreateDate"
                                Text="Create Date" Align="Center" Width="120" Format="dd/MM/yyyy" />
                            <ext:DateColumn ID="colQADate" runat="server" DataIndex="QADate"
                                Text="QA Date" Align="Center" Width="120" Format="dd/MM/yyyy" />

                            <ext:DateColumn ID="colCompleteDate" runat="server" DataIndex="FinishDate" Text="Complete Date"
                                Align="Center" Width="120" Format="dd/MM/yyyy">
                            </ext:DateColumn>

                            <ext:Column ID="colStatus" runat="server" DataIndex="Status" Text="Status"
                                Align="Center" Width="150" />

                        </Columns>

                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="DisplayingFromTo"
                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                            FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                            BeforePageText="<center>Page</center>">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="Page size:" />
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
                                        <%--   #{grdDataList}.store.reload();" />--%>
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataPutAwayJobCode" Value="record.data.PutAwayJobCode" Mode="Raw" />
                                <ext:Parameter Name="oDataPutawayStatus" Value="record.data.Status" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

