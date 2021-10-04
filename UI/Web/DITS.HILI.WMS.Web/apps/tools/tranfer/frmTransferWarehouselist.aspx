<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmTransferWarehouselist.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.frmTransferWarehouselist" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.TransferStatus == 100 || record.data.TransferStatus == 102) {
                toolbar.items.getAt(0).setDisabled(true);
            }
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.TransferStatus == 30 || record.data.TransferStatus == 100 || record.data.TransferStatus == 102) {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };

    </script>

    <ext:XScript runat="server">
        <script>

            var beforeEditCheck = function (editor, e, eOpts) {
                //if (e.record.data.IsReceive == true) {
                //    e.cancel = true;

                //} else {
                //App.direct.LoadCombo();
                //App.direct.LoadOrderType(e.record.data.OrderType);
                e.cancel = false;
                //}

            };

            var getProduct = function () {

                var product_code = App.txtProductCode.getValue();


                App.hidAddProduct_System_Code.reset();
                App.txtAddProduct_Name_Full.reset();

                App.direct.GetProduct(product_code);
            };


            var popupProduct = function () {
                App.direct.GetProduct('');
            };

            var edit = function (editor, e) {



                App.direct.UpdateSuggestLocation(e.record.data.DamageID, e.record.data.ChangeStatusQty, e.value);

                //plugin.completeEdit();
            };
 </script>
    </ext:XScript>

    <style>
        div#ListComboSubCustomer {
            border-top-width: 1 !important;
            width: 250px !important;
        }
    </style>


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
                                <ext:FieldSet Margins="0 5 0 5" Title="<%$ Resource : SEARCH_INFO %>" runat="server" Layout="AnchorLayout" AutoScroll="false" Height="60" Flex="1">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:FieldContainer runat="server"
                                            Layout="HBoxLayout" MarginSpec="5 0 0 0">
                                            <Items>

                                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text='<%$ Resource : ADD_NEW %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>

                                                <ext:TextField runat="server" FieldLabel="<% $Resource : TRANSFER_NO %>"
                                                    ID="txtTrmCode"
                                                    AllowBlank="true" 
                                                    Width="300"
                                                    LabelWidth="150"
                                                    LabelAlign="Right" />

                                                <ext:DateField runat="server"
                                                    ID="dtStartDate"
                                                    FieldLabel='<%$ Resource : TRANSFER_DATE %>'
                                                    MaxLength="10"
                                                    AllowBlank="false"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    Width="350"
                                                    LabelWidth="150"
                                                    LabelAlign="Right" />

                                                <ext:ComboBox ID="cmbStatus"
                                                    runat="server"
                                                    FieldLabel='<%$ Resource : STATUS %>'
                                                    LabelAlign="Right"
                                                    DisplayField="LogicalZoneGroupName"
                                                    ValueField="LogicalZoneGroupID"
                                                    EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                                    PageSize="0"
                                                    MinChars="0"
                                                    Width="300"
                                                    AllowBlank="false"
                                                    TypeAhead="false"
                                                    TriggerAction="Query"
                                                    QueryMode="Remote"
                                                    SelectOnFocus="true"
                                                    ForceSelection="true"
                                                    AllowOnlyWhitespace="false"
                                                    TabIndex="2">
                                                </ext:ComboBox>

                                                <ext:Button ID="btnSearch" Margins="0 15 0 15" Width="100" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">

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
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" AutoDataBind="false" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="TRM_CODE">
                                    <Fields>
                                        <ext:ModelField Name="TRM_ID" />
                                        <ext:ModelField Name="TRM_CODE" />
                                        <ext:ModelField Name="TransferStatus" />
                                        <ext:ModelField Name="TransferStatusName" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="IsApprove" Type="Boolean" />
                                        <ext:ModelField Name="TransferDate" Type="Date" />
                                        <ext:ModelField Name="ApproveDate" Type="Date" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="TransferDate" Direction="DESC"/>
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
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.TRM_ID" Mode="Raw" />
                                            <ext:Parameter Name="TransferStatus" Value="record.data.TransferStatus" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete?" Title="Delete" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.TRM_ID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column ID="colGroup_CODE" runat="server" DataIndex="TRM_CODE" Text='<%$ Resource : TRANSFER_NO %>' Align="Left" Width="130" />
                            <ext:DateColumn ID="DateColumn2" runat="server" DataIndex="TransferDate" Text='<%$ Resource : TRANSFER_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="150" />
                            <ext:CheckColumn ID="Column11" runat="server" DataIndex="IsApprove" Text='<%$ Resource : APPROVE_STATUS %>' Align="Center" Width="150" />
                            <ext:DateColumn ID="Column1" runat="server" DataIndex="ApproveDate" Text='<%$ Resource : APPROVE_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="150" />
                            <ext:Column ID="Column10" runat="server" DataIndex="TransferStatusName" Text='<%$ Resource : STATUS %>' Align="Left" Width="150" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing runat="server">
                            <Listeners>
                                <Edit Fn="edit" />
                            </Listeners>
                        </ext:CellEditing>
                    </Plugins>
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
                                    <DirectEvents>
                                        <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />
                                    </DirectEvents>
                                </ext:ComboBox>

                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%Resource : LOADING%>" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
