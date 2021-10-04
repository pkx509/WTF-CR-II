<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionReClassifiedList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.frmInspectionReClassifiedList" %>

<%--<%@ Register Src="~/apps/share/ucProductSelect.ascx" TagPrefix="uc1" TagName="ucProductSelect" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <script src="~/resources/js/JScript.Common.js"></script>
      <script type="text/javascript">
          Ext.Ajax.timeout = 180000; // 1 sec
          Ext.net.DirectEvent.timeout = 180000; // 1 sec 
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
                                <ext:FieldContainer runat="server"
                                    Layout="HBoxLayout" MarginSpec="5 0 0 0">
                                    <Items>

                                        <ext:DateField runat="server"
                                            ID="dtStartDate"
                                            FieldLabel='<%$ Resource : START_DATE %>'
                                            MaxLength="10"
                                            AllowBlank="false"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                                    Width="200"
                                            LabelAlign="Right" />
                                        <ext:DateField runat="server"
                                            ID="dtEndDate"
                                            AllowBlank="false"
                                            FieldLabel='<%$ Resource : END_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                                    Width="200"
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
                                            Flex="1"
                                            AllowBlank="false"
                                            TypeAhead="false"
                                            TriggerAction="Query"
                                            QueryMode="Remote"
                                            SelectOnFocus="true"
                                            ForceSelection="true"
                                            AllowOnlyWhitespace="false"
                                            TabIndex="2">
                                        </ext:ComboBox>

                                                <ext:TextField runat="server" ID="txtSearch" EmptyText='<%$ Resource : SEARCH_WORDING %>' PaddingSpec="0 5 0 5">
                                                    <Listeners>
                                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                                    </Listeners>
                                                </ext:TextField>
                                        <ext:Button ID="btnSearch" Margins="0 10 0 10" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">

                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click">
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
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" AutoDataBind="false" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="ReclassifiedID">
                                    <Fields>
                                        <ext:ModelField Name="ReclassifiedID" />
                                        <ext:ModelField Name="ReclassifiedCode" />
                                        <ext:ModelField Name="ReclassFromLot" />
                                        <ext:ModelField Name="ReclassToLot" />
                                        <ext:ModelField Name="ReclassStatusDesc" />
                                        <ext:ModelField Name="ApproveDate" Type="Date" />
                                        <ext:ModelField Name="IsApprove" Type="Boolean" />
                                        <ext:ModelField Name="DateCreated" Type="Date" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="EXPDate" Type="Date" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ReclassStatusName" />
                                        <ext:ModelField Name="Description" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="ProductStatusName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="ReclassifiedCode" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ReclassifiedID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit' || command=='Confirm' ) return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                        <EventMask ShowMask="true" Msg="Delete" MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column ID="colGroup_CODE" runat="server" DataIndex="ReclassifiedCode" Text='<%$ Resource : QA_CODE %>' Align="Left" Width="120" />
                            <ext:Column ID="Column2" runat="server" DataIndex="ReclassToLot" Text='<%$ Resource : LOTNO %>' Align="Left" Width="150"> </ext:Column>
                            <ext:Column ID="Column3" runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCTCODE %>' Align="Left" Width="150"> </ext:Column>
                            <ext:Column ID="Column5" runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCT_NAME %>' Align="Left" Width="150"> </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="Description" Text='<%$ Resource : DESCRIPTION %>' Align="Left" Width="150" />
                            <ext:Column ID="Column10" runat="server" DataIndex="ReclassStatusName" Text='<%$ Resource : STATUS %>' Align="Left" Width="150" />
                            <ext:DateColumn ID="Column1" runat="server" DataIndex="ApproveDate" Text='<%$ Resource : APPROVE_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="100" />
                            <ext:CheckColumn ID="Column11" runat="server" DataIndex="IsApprove" Text='<%$ Resource : APPROVE %>' Align="Center" Width="100" />
                            <ext:DateColumn ID="DateColumn1" runat="server" DataIndex="DateCreated" Text='<%$ Resource : CREATE_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="100" />

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
