<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAssignJobList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.picking.frmAssignJobList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <%--  <script type="text/javascript">
          Ext.Ajax.timeout = 180000; // 1 sec
          Ext.net.DirectEvent.timeout = 180000; // 1 sec 
      </script>--%>
    <ext:XScript runat="server">
        <script>

            var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {

                if (record.data.PickingStatusEnums != 'WaitingPick') {
                    toolbar.items.getAt(1).setDisabled(true);
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
                                        <ext:TextField runat="server"
                                            ID="txtPONO"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : PONO %>' >
                                            
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{Button1}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtStartDate"
                                            FieldLabel='<%$ Resource : START_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtDocNo"
                                            Flex="1"
                                            FieldLabel='<%$ Resource : DOCUMENT_NO %>' >
                                            
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{Button1}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtEndDate"
                                            FieldLabel='<%$ Resource : END_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbPickingStatus"
                                            FieldLabel="<%$ Resource : PICKINGSTATUS %>"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            Flex="1">
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
                                        <ext:Button runat="server" ID="Button1" Text='<%$ Resource : SEARCH %>' MarginSpec="0 0 0 5" Icon="Magnifier">
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
                                        <ext:ModelField Name="PickingID" />
                                        <ext:ModelField Name="PONo" />
                                        <ext:ModelField Name="DocNo" />
                                        <ext:ModelField Name="PickingDate" Type="Date" />
                                        <ext:ModelField Name="PickingStatus" />
                                        <ext:ModelField Name="PickingStatusEnums" />
                                        <ext:ModelField Name="ShippingCode" />
                                        <ext:ModelField Name="ShippingTruckNo" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="PONo" Direction="DESC" />
                            </Sorters>
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
                                            <ext:Parameter Name="pickingID" Value="record.data.PickingID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit' || command=='Confirm' ) return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                        <EventMask ShowMask="true" Msg="Delete" MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="pickingID" Value="record.data.PickingID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="PONo" Text="<%$ Resource : PONO %>" Flex="1" />
                            <ext:Column runat="server" DataIndex="DocNo" Text="<%$ Resource : DOCUMENT_NO %>" />
                            <ext:DateColumn runat="server" DataIndex="PickingDate" Text="<%$ Resource : PICKINGDATE %>" Format="dd/MM/yyyy" />
                            <ext:Column runat="server" DataIndex="PickingStatus" Text="<%$ Resource : PICKINGSTATUS %>" />
                            <ext:Column runat="server" DataIndex="ShippingCode" Text="<%$ Resource : SHIPPINGCODE %>" />
                            <ext:Column runat="server" DataIndex="ShippingTruckNo" Text="<%$ Resource : DRIVINGLICENSE %>" />
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
                                <ext:Parameter Name="pickingID" Value="record.data.PickingID" Mode="Raw" />
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
