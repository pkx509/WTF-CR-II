<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInternalRecList_WTF.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.inbound.receive_WTF.frmInternalRecList_WTF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
   <%-- <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
    </script>--%>
    <ext:XScript runat="server">
        <script>

            var prepareToolbar = function (grid, toolbar, rowIndex, record) {

                toolbar.items.getAt(1).setDisabled(true);

                if (record.data.ReceiveStatus == 'New') {
                    toolbar.items.getAt(1).setDisabled(false);
                }
            };

        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="true"
                    BodyPadding="3"
                    Region="North"
                    Frame="true"
                    Layout="ColumnLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtReceiveDate"
                                            FieldLabel='<%$ Resource : RECEIVEDATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtReceiveNo"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : RECEIVENO %>' />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbReceiveStatus"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : STATUS %>' />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtOrderNo"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : ORDERNO %>' />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtPONo"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : PONO %>' />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbReceiveType"
                                            FieldLabel="<% $Resource : RECEIVETYPE %>"
                                            DisplayField="Name"
                                            ValueField="DocumentTypeID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            Flex="1"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            PageSize="20">
                                            <ListConfig LoadingText="Searching..." MinWidth="350" />
                                            <Store>
                                                <ext:Store ID="StoreReceiveType" runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=InternalReceiveType">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="DocumentTypeID" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="RefDocumentID" />
                                                                <ext:ModelField Name="ProductStatus" />
                                                                <ext:ModelField Name="IsCreditNote" />
                                                                <ext:ModelField Name="IsNormal" />
                                                                <ext:ModelField Name="ToReprocess" />
                                                                <ext:ModelField Name="FromReprocess" />
                                                                <ext:ModelField Name="IsItemChange" />
                                                                <ext:ModelField Name="IsWithoutGoods" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.1">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:BoxSplitter runat="server" Flex="1" />

                                        <ext:Button runat="server" ID="btnSearch" Text='<%$ Resource : SEARCH %>' MarginSpec="0 0 0 5" Icon="Magnifier">
                                            <Listeners>
                                                <Click Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:Container>
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">
                    <TopBar>
                        <ext:Toolbar runat="server">
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
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false" RemotePaging="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ReceiveID" />
                                        <ext:ModelField Name="ReceiveCode" />
                                        <ext:ModelField Name="PONo" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="ReceiveTypeID" />
                                        <ext:ModelField Name="ReceiveType" />
                                        <ext:ModelField Name="ESTReceiveDate" Type="Date" />
                                        <ext:ModelField Name="ReceiveStatus" />
                                    </Fields>
                                </ext:Model>
                            </Model>

                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text='<%$ Resource : DELETE %>' CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="receiveID" Value="record.data.ReceiveID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit' || command=='Confirm' ) return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
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
                            <ext:Column runat="server" DataIndex="PONo" Text="<%$ Resource : PONO %>" />
                            <ext:Column runat="server" DataIndex="OrderNo" Text="<%$ Resource : ORDERNO %>" />
                            <ext:Column runat="server" DataIndex="ReceiveType" Text="<%$ Resource : RECEIVETYPE %>" Flex="1" />
                            <ext:DateColumn runat="server" DataIndex="ESTReceiveDate" Text="<%$ Resource : RECEIVEDATE %>" Format="dd/MM/yyyy" />
                            <ext:Column runat="server" DataIndex="ReceiveStatus" Text="<%$ Resource : STATUS %>" />
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
