<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllRegisTruck.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.registruck.frmAllRegisTruck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  
    <%--<script type="text/javascript">
                         Ext.Ajax.timeout = 180000; // 1 sec
                         Ext.net.DirectEvent.timeout = 180000; // 1 sec 
    </script>--%>
    <script type="text/javascript">

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.ShippingStatus == 20 || record.data.ShippingStatus == 100) {
                toolbar.items.getAt(0).setDisabled(true);
            }
        };
        var prepareToolbarEdit = function (grid, toolbar, rowIndex, record) {
            if (record.data.ShippingStatus == 10 || record.data.ShippingStatus == 20) {
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

                <ext:FormPanel runat="server"
                    BodyPadding="5"
                    Region="North"
                    Frame="true"
                    AutoScroll="false"
                    Margins="3 3 0 3"
                    Layout="AnchorLayout">

                    <FieldDefaults LabelAlign="Right" InputWidth="150" LabelWidth="100" />

                    <Items>
                        <ext:FieldSet runat="server" Title="Search Information" Layout="AnchorLayout" AutoScroll="false">
                            <Items>

                                <ext:FieldContainer runat="server"
                                    Layout="HBoxLayout" MarginSpec="10 0 10 0">
                                    <Items>

                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:TextField runat="server" ID="txtPoNo" FieldLabel="<%$ Resource : PO_NO %>">
                                            
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:TextField runat="server" ID="txtDocNo" FieldLabel="<%$ Resource : DOCUMENT_NO %>">
                                            
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:BoxSplitter runat="server" Flex="1" />

                                        <ext:Button ID="btnSearch" runat="server" Width="90" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click">
                                                    <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:FieldSet>

                    </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server"  PageSize="20" RemoteSort="true" RemotePaging="true" AutoLoad="true" >
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="ShippingCode">
                                    <Fields>
                                        <ext:ModelField Name="ShippingID" />
                                        <ext:ModelField Name="ShippingCode" />
                                        <ext:ModelField Name="PoNo" />
                                        <ext:ModelField Name="DocumentNo" />
                                        <ext:ModelField Name="DocumentDate" Type="Date" />
                                        <ext:ModelField Name="TruckType" />
                                        <ext:ModelField Name="TruckTypeName" />
                                        <ext:ModelField Name="RegisterType" />
                                        <ext:ModelField Name="DockTypeName" />
                                        <ext:ModelField Name="ShippingTruckNo" />
                                        <ext:ModelField Name="ShippingStatus" />
                                        <ext:ModelField Name="CompleteDate" Type="Date" />
                                        <ext:ModelField Name="IsApprove" Type="Boolean" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="ShippingCode" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelTruck" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="<%$ Resource : DELETE %>" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShippingID" Mode="Raw" />
                                            <ext:Parameter Name="isApprove" Value="record.data.IsApprove" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="<%$ Resource : EDIT %>" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ShippingID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column runat="server" DataIndex="ShippingCode" Text="<%$ Resource : SHIPPING_CODE %>" Align="Center" Flex="1" />
                            <ext:Column runat="server" DataIndex="PoNo" Text="<%$ Resource : PO_NO %>" Align="Center" Flex="1" />
                            <ext:Column runat="server" DataIndex="DocumentNo" Text="<%$ Resource : DOCUMENT_NO %>" Align="Center" Flex="1" />
                            <ext:DateColumn runat="server" DataIndex="DocumentDate" Text="<%$ Resource : DOCUMENT_DATE %>" Align="Center" Format="dd/MM/yyyy" />
                            <ext:Column runat="server" DataIndex="DockTypeName" Text="<%$ Resource : DOCK_NAME %>" Align="Center" Flex="1" />
                            <ext:Column runat="server" DataIndex="RegisterType" Text="<%$ Resource : REGISTER_TYPE %>" Align="Center" Flex="1" />
                            <ext:Column runat="server" DataIndex="ShippingTruckNo" Text="<%$ Resource : TRUCK_NO %>" Align="Center" Flex="1" />
                            <ext:Column runat="server" DataIndex="TruckTypeName" Text="<%$ Resource : TRUCK_TYPE_NAME %>" Align="Center" Flex="1" />
                            <ext:DateColumn runat="server" DataIndex="CompleteDate" Text="<%$ Resource : COMPLETE_DATE %>" Align="Center" Format="dd/MM/yyyy" />
                            <ext:CheckColumn runat="server" DataIndex="IsApprove" Text="<%$ Resource : ASSIGN %>" Align="Center" Flex="1" />
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
                                        <%--  #{grdDataList}.store.reload();" />--%>
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.ShippingID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText='<%$ Resource : LOADING %>' LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
