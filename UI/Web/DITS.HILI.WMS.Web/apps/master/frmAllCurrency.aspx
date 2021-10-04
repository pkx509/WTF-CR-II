<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllCurrency.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllCurrency" %>

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
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="AddNew">
                                  <%--  <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>--%>
                                </ext:Button>

                                <ext:ToolbarFill />
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="SearchWording" Name="txtSearch" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">

                                    <DirectEvents>
                                       <%-- <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>--%>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                       <%-- <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" OnReadData="Store_Refresh"
                            RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData">
                                </ext:PageProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="DriverId">
                                    <Fields>
                                        <ext:ModelField Name="Currency_Code" />
                                        <ext:ModelField Name="Currency_Name_Short" />
                                        <ext:ModelField Name="Currency_Name_Full" />
                                        <ext:ModelField Name="Exchange_Rate" />
                                        <ext:ModelField Name="IsDefault" />
                                        <ext:ModelField Name="IsActive" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Currency_Code" Direction="ASC" />
                            </Sorters>
                        </ext:Store>--%>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                   <%-- <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.Currency_Code" Mode="Raw" />
                                            <ext:Parameter Name="oDefault" Value="record.data.IsDefault" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message="<%$Resources:Langauge, AreYouSureDelete%>" Title="<%$Resources:Langauge, ConfirmDelete%>" />
                                    </Command>--%>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                   <%-- <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.Currency_Code" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>--%>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="Currency_Code" Text="CurrencyCode" Align="Center" Width="100" Hidden="true" />
                            <ext:Column ID="ColumnTrailerCompany" runat="server" DataIndex="Currency_Name_Short" Text="Currency" Align="Center" Width="100" />
                            <ext:Column ID="Column2" runat="server" DataIndex="Currency_Name_Full" Text="CurrencyFullName" Align="Left" Flex="1" />
                            <ext:Column ID="ColumnDriverCode" runat="server" DataIndex="Exchange_Rate" Text="ExchangeRate" Align="Left" Flex="1" />
                            <ext:CheckColumn ID="colIsDefault" runat="server" DataIndex="IsDefault" Text="DefaultCurrency" Align="Center" Width="100"  />
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text="IsActive" runat="server"
                                Align="Center" Width="100" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="Displaying {0} - {1} of {2}"
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
                                    <DirectEvents>
                                      <%--  <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />--%>
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                     <%--   <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data.Currency_Code" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>--%>
                    </DirectEvents>
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
