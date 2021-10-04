<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmConsolitdateList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.consolidate.frmConsolitdateList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        //Ext.Ajax.timeout = 180000; // 1 sec
        //Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.ConsolidateStatusId > '10') {
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
                <ext:Hidden runat="server" ID="hidIsPopup" />
                <ext:Hidden runat="server" ID="hidByStatus" />
                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="false"
                    BodyPadding="3"
                    Region="North"
                    Frame="true"
                    Layout="ColumnLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                    <Items>

                        <ext:FieldSet runat="server" Title="Search Infomation" Layout="ColumnLayout" AutoScroll="false"
                            Collapsible="false"
                            Collapsed="false">
                            <Items>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.3">
                                    <Items>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtPono"
                                                    Flex="1"
                                                    FieldLabel='<%$ Resource : PONO %>' />
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
                                                    FieldLabel='<%$ Resource : DOCUMENT_NO %>' />
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
                                                    ID="cmbStatus"
                                                    FieldLabel="<%$ Resource : CONSOLIDATESTATUS %>"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    Flex="1">
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtLicensePlate"
                                                    Flex="1"
                                                    FieldLabel='<%$ Resource : LICENSEPLATE %>' />
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
                                                    <%--                                              <Listeners>
                                                        <Click Handler="#{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>--%>
                                                    <DirectEvents>
                                                        <Click OnEvent="btnSearch_Click"></Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false" GroupField="Dispatch_Date_Order">
                            <%--GroupField="SubCust_Country"--%>
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DeliveryId" />
                                        <ext:ModelField Name="DateCreated" Type="Date" />
                                        <ext:ModelField Name="PoNo" />
                                        <ext:ModelField Name="DocumentNo" />
                                        <ext:ModelField Name="ConsolidateStatusId" />
                                        <ext:ModelField Name="ConsolidateStatusName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="DeliveryId" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30" Hidden="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Cancel Dispatch" CommandName="Delete" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.PoNo" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.ConsolidateStatusId" Mode="Raw" />
                                            <ext:Parameter Name="oDataDocumentNo" Value="record.data.DocumentNo" Mode="Raw" />

                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.PoNo" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.ConsolidateStatusId" Mode="Raw" />
                                            <ext:Parameter Name="oDataDocumentNo" Value="record.data.DocumentNo" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>



                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server"
                                Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />

                            <ext:Column DataIndex="PoNo" runat="server" Text="<%$ Resource : PONO %>" Align="Left" Flex="1" />
                            <ext:Column runat="server" DataIndex="DocumentNo" Text="<%$ Resource : DOCUMENT_NO %>" Flex="1" />
                            <ext:DateColumn runat="server" DataIndex="DateCreated" Text="<%$ Resource : CONSOLIDATEDATE %>" Format="dd/MM/yyyy" Flex="1" />
                            <ext:Column runat="server" DataIndex="ConsolidateStatusName" Text="<%$ Resource : CONSOLIDATESTATUS %>" Flex="1" />


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
                                                                           <ext:Parameter Name="oDataKeyId" Value="record.data.PoNo" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.ConsolidateStatusId" Mode="Raw" />
                                            <ext:Parameter Name="oDataDocumentNo" Value="record.data.DocumentNo" Mode="Raw" />

                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" />
                    </View>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
