<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmOrderSortingList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.ordersorting.frmOrderSortingList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Sorting List</title>
        <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <%--  <script type="text/javascript">
         Ext.Ajax.timeout = 180000; // 1 sec
         Ext.net.DirectEvent.timeout = 180000; // 1 sec 
      </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Default" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" Title="Order Sorting List">
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
                                        <ext:ComboBox runat="server" Flex="1" EmptyText="---Please Select---" />
                                        <ext:TextField runat="server" Flex="1" MarginSpec="0 0 0 5" />
                                        <ext:Button runat="server" Text="Search" MarginSpec="0 0 0 5" Icon="Magnifier" />


                                    </Items>
                                </ext:FieldContainer>


                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false">
                            <Proxy>
                                <%-- <ext:PageProxy DirectFn="App.direct.BindData" />--%>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>

          
                                        <ext:ModelField Name="Urgent" />
                                        <ext:ModelField Name="DispatchNo" />
                                        <ext:ModelField Name="Route" />
                                        <ext:ModelField Name="InvoiceNo" />
                                        <ext:ModelField Name="PONo" />
                                        <ext:ModelField Name="Customer" />
                                         <ext:ModelField Name="Dispatch Type" />
                                        <ext:ModelField Name="EstDate" Type="Date" />
                                        <ext:ModelField Name="DispatchDate" Type="Date" />
                                        <ext:ModelField Name="Status" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <%-- <ext:DataSorter Property="op_receive.Receive_Code" Direction="ASC" />--%>
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">

                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No." Width="60" Align="Center" />
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="50">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                    <ext:CommandFill></ext:CommandFill>
                                </Commands>
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="ApplicationEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                    <ext:CommandFill></ext:CommandFill>
                                </Commands>
                            </ext:CommandColumn>



                            <ext:CheckColumn ID="colUrgent" runat="server" DataIndex="Urgent"
                                Text="Urgent" Align="Center" Width="120" />

                            <ext:Column ID="colDispatchNo" runat="server" DataIndex="DispatchNo" Text="Dispatch No."
                                Align="Center" Width="150" />
                            <ext:Column ID="colDocumentRef" runat="server" DataIndex="DocumentRef" Text="Document Ref"
                                Align="left" Flex="1" />
                            <ext:Column ID="colRoute" runat="server" DataIndex="Route" Text="Route"
                                Align="left" Flex="1" />
                            <ext:Column ID="colInvoiceNo" runat="server" DataIndex="InvoiceNo" Text="Invoice No."
                                Align="left" Flex="1" />

                            <ext:Column ID="colPONo" runat="server" DataIndex="PONo" Text="PO No."
                                Align="left" Flex="1" />
                            <ext:Column ID="colCustomer" runat="server" DataIndex="Customer" Text="Customer"
                                Align="left" Flex="1" />
                            <ext:Column ID="colDispatchType" runat="server" DataIndex="DispatchType" Text="Dispatch Type"
                                Align="left" Flex="1" />
                            <ext:DateColumn ID="colEstDate" runat="server" DataIndex="EstDate"
                                Text="PickingDate" Align="Center" Width="120" Format="dd/MM/yyyy" />

                            <ext:DateColumn ID="colDispatchDate" runat="server" DataIndex="DispatchDate" Text="Dispatch Date"
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

                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

