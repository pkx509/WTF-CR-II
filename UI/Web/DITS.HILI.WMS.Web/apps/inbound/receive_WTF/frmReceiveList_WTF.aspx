<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReceiveList_WTF.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.inbound.receive_WTF.frmReceiveList_WTF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
    </script>
    <ext:XScript runat="server">
        <script>
            var prepareToolbar = function (grid, toolbar, rowIndex, record) {

                toolbar.items.getAt(0).setDisabled(true);
                if (record.data.ReceiveStatus == 'New') {
                    toolbar.items.getAt(0).setDisabled(false);
                }
            };

        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">

            <Items>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">

                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarFill />

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtReceiveDateEST"
                                            FieldLabel='<%$ Resource : RECEIVE_DATE_EST %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1"
                                            LabelWidth="150"
                                            LabelAlign="Right" />

                                        <ext:ComboBox runat="server"
                                            ID="cmbStatus"
                                            FieldLabel="<%$ Resource : STATUS %>"
                                            LabelWidth="80"
                                            LabelAlign="Right"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>">
<%--                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>--%>
                                        </ext:ComboBox>

                                        <ext:ComboBox runat="server"
                                            ID="cmbLine"
                                            LabelWidth="80"
                                            LabelAlign="Right"
                                            FieldLabel="<% $Resource : LINE %>"
                                            Flex="1"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="LineCode"
                                            ValueField="LineID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            PageSize="20">
                                            <Store>
                                                <ext:Store runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Line">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="LineID" />
                                                                <ext:ModelField Name="LineCode" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtSearch" EmptyText='<%$ Resource : SEARCH_WORDING %>' PaddingSpec="0 5 0 5">
                                                    <Listeners>
                                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                                    </Listeners>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:Button runat="server" ID="btnSearch" Icon="Magnifier" Text='<%$ Resource : SEARCH %>'>
                                            <Listeners>
                                                <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false" AutoLoad="false">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ReceiveID" />
                                        <ext:ModelField Name="ReceiveCode" />
                                        <ext:ModelField Name="ReceiveDate" Type="Date" />
                                        <ext:ModelField Name="ReceiveTypeName" />
                                        <ext:ModelField Name="ReceiveStatus" />
                                        <ext:ModelField Name="ReceiveStatusDesc" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="LineType" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="IsProduction" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text='<%$ Resource : DELETE %>' CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="receiveID" Value="record.data.ReceiveID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;"
                                            ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>'
                                            Title='<%$ MessageTitle :  MSG00003 %>' />
                                        <EventMask ShowMask="true" Msg="Delete" MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbar" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="receiveID" Value="record.data.ReceiveID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ReceiveCode" Text="<%$ Resource : RECEIVENO %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCTNAME %>" />
                            <ext:Column runat="server" DataIndex="ReceiveTypeName" Text="<%$ Resource : RECEIVETYPE %>" Flex="1" />
                            <ext:DateColumn runat="server" DataIndex="ReceiveDate" Text="<%$ Resource : RECEIVEDATE %>" Format="dd/MM/yyyy" />
                            <ext:CheckColumn runat="server" DataIndex="IsProduction" Text="<%$ Resource : ISPRODUCTION %>" />
                            <ext:Column runat="server" DataIndex="LineCode" Text="<%$ Resource : LINE %>" />
                            <ext:Column runat="server" DataIndex="LineType" Text="<%$ Resource : TYPE %>" />
                            <ext:Column runat="server" DataIndex="OrderNo" Text="<%$ Resource : ORDERNO %>" />
                            <ext:Column runat="server" DataIndex="ReceiveStatusDesc" Text="<%$ Resource : STATUS %>" />
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
                                <ext:Parameter Name="receiveID" Value="record.data.ReceiveID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
