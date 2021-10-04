<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllPutAwayRule.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllPutAwayRule" %>

<%--<%@ Register Src="~/apps/share/ucProductSelect.ascx" TagPrefix="uc1" TagName="ucProductSelect" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <script src="~/resources/js/JScript.Common.js"></script>


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

                if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
                    console.log(e.record.data);
                    App.direct.Edit(e.record.data.Special_Rul_Code, e.field, e.originalValue, e.value, e.record.data);
                }
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
                                <ext:FieldSet runat="server" Layout="AnchorLayout" AutoScroll="false" Height="55">
                                    <Items>

                                        <ext:FieldContainer runat="server"
                                            Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>
                                                <ext:ComboBox ID="cmbLogicalzone"
                                                    runat="server"
                                                    FieldLabel='<%$ Resource : GROUP_PRODUCT %>'
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
                                                    <ListConfig LoadingText="Searching..." ID="ListLogicalzone">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                                              {LogicalZoneGroupName}
						                                                </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreLogicalzone" runat="server">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="../../../Common/DataClients/MsDataHandler.ashx?Method=LogicalZone">
                                                                    <ActionMethods Read="POST" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model11" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="LogicalZoneGroupID" />
                                                                        <ext:ModelField Name="LogicalZoneGroupName" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <Listeners>
                                                        <SpecialKey Handler="if(e.getKey() == 13){ #{txtDocRef}.focus(false, 100);}" />
                                                    </Listeners>
                                                    <DirectEvents>
                                                        <%--<Select OnEvent="cmbDispatch_Code_ChangeClick" />--%>
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                                <ext:TextField runat="server" Width="150" ID="txtOrderNo" FieldLabel='<%$ Resource : ORDER_NO %>'
                                                    LabelAlign="Right" LabelWidth="50" TabIndex="11" SelectOnFocus="true">
                                                </ext:TextField>
                                                <ext:TextField runat="server" Width="170" ID="txtProductCode" FieldLabel='<%$ Resource : PRODUCT_CODE %>' Hidden="true"
                                                    LabelAlign="Right" LabelWidth="80" TabIndex="11" SelectOnFocus="true">
                                                    <Listeners>
                                                        <SpecialKey Handler=" if(e.getKey() == 13){ getProduct(); }" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="btnProductSelect" TabIndex="12" Hidden="true">
                                                    <Listeners>
                                                        <Click Fn="popupProduct" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:TextField runat="server" MarginSpec="0 0 0 10" Width="150" ID="txtAddProduct_Name_Full" TabIndex="13" ReadOnly="true" Hidden="true" />
                                                <ext:Hidden runat="server" ID="hidAddProduct_System_Code" />
                                                <ext:Hidden runat="server" ID="hidAddUomID" />

                                                <ext:NumberField ID="txtPeriodLot" runat="server"
                                                    FieldLabel='<%$ Resource : PERIOD_LOT %>' LabelAlign="Right" LabelWidth="60"
                                                    TabIndex="14" AllowDecimals="true" MinValue="1" Width="140"
                                                    EnforceMaxLength="true" MaxLength="50" DecimalPrecision="3">
                                                    <Listeners>
                                                        <SpecialKey Handler="if(e.getKey() == 13){ App.btnAddItem.fireEvent('click');}" />
                                                    </Listeners>
                                                </ext:NumberField>
                                                <ext:SelectBox 
                                                    Editable="false" 
                                                    ID="ddlUnitLot" 
                                                    runat="server" 
                                                    Width="50"
                                                     AllowBlank="false">
                                                    <Items>
                                                        <ext:ListItem Value="days" Index="0" Text="days" />
                                                    </Items>
                                                </ext:SelectBox>

                                                <ext:SelectBox
                                                    Editable="false"
                                                    ID="cmbConditionAdd"
                                                    runat="server"
                                                    Width="130"
                                                    LabelAlign="Right"
                                                    DisplayField="Condition"
                                                    ValueField="Condition"
                                                    FieldLabel='<%$ Resource : CONDITION %>'
                                                    LabelWidth="70"
                                                    AllowBlank="false">
                                                    <Items>
                                                        <ext:ListItem Value=">" Text=">" />
                                                        <ext:ListItem Value=">=" Text=">=" />
                                                        <ext:ListItem Value="=" Text="=" />
                                                        <ext:ListItem Value="<" Text="<" />
                                                        <ext:ListItem Value="<=" Text="<=" />
                                                    </Items>
                                                </ext:SelectBox>

                                                <ext:Button runat="server"
                                                    ID="btnAddItem"
                                                    Icon="Add"
                                                    Text='<%$ Resource : SAVE %>'
                                                    TabIndex="13"
                                                    MarginSpec="0 0 0 10">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAddPutAwayRule_Click">
                                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                        </Click>
                                                    </DirectEvents>
                                                    <Listeners>
                                                        <%--<Click Fn="addProduct" />--%>
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:FieldContainer>

                                    </Items>
                                </ext:FieldSet>

                                <ext:ToolbarFill />
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="100" Height="25">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">

                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="PutAwayRuleID">
                                    <Fields>
                                        <ext:ModelField Name="PutAwayRuleID" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="Priority" />
                                        <ext:ModelField Name="Product_Code" />
                                        <ext:ModelField Name="Product_Name" />
                                        <ext:ModelField Name="PeriodLot" />
                                        <ext:ModelField Name="Unit_Lot" />
                                        <ext:ModelField Name="Condition" />
                                        <ext:ModelField Name="Group_CODE" />
                                        <ext:ModelField Name="LogicalZoneGroupName" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="PutAwayRuleID" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="CommandColumn3" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.PutAwayRuleID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="80" Align="Center" />
                            <%--                            <ext:Column ID="Column1" runat="server" DataIndex="Special_Rul_Code" Text='<%$ Resource : RULE_CODE %>' Align="Center" />--%>
                            <ext:Column ID="colGroup_CODE" runat="server" DataIndex="LogicalZoneGroupName" Text='<%$ Resource : GROUP_PRODUCT %>' Align="Center" Width="100" />
                            <ext:Column ID="Column2" runat="server" DataIndex="OrderNo" Text='<%$ Resource : ORDER_NO %>' Align="Left">
                                <Editor>
                                    <ext:TextField runat="server" Width="150" ID="TextField1"
                                        LabelAlign="Right" TabIndex="11" SelectOnFocus="true" AllowBlank="false">
                                    </ext:TextField>
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="Product_Code" Text='<%$ Resource : PRODUCT_CODE %>' Align="Left" Hidden="true" />
                            <ext:Column ID="Column5" runat="server" DataIndex="Product_Name" Text='<%$ Resource : PRODUCT_NAME %>' Align="Left" Flex="1" Hidden="true" />
                            <ext:NumberColumn ID="colAging" Format="#,0" runat="server" DataIndex="PeriodLot" Text='<%$ Resource : PERIOD_LOT %>' Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtAging">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            <ext:Column ID="Column3" runat="server" DataIndex="Unit_Lot" Text='<%$ Resource : UNIT_LOT %>' Align="Left" />
                            <ext:Column ID="Column7" runat="server" DataIndex="Condition" Text="Condition" Align="Center">
                                <Editor>
                                    <ext:ComboBox ID="cmbConditionEdit" runat="server" Editable="false"
                                        DisplayField="Code" ValueField="ValueTH" AutoSelect="true"
                                        MinChars="0" Width="160" AllowBlank="false" ForceSelection="true"
                                        TypeAhead="false" QueryMode="Remote" AutoShow="false">
                                        <Items>
                                            <ext:ListItem Value=">" Text=">" />
                                            <ext:ListItem Value=">=" Text=">=" />
                                            <ext:ListItem Value="=" Text="=" />
                                            <ext:ListItem Value="<" Text="<" />
                                            <ext:ListItem Value="<=" Text="<=" />
                                        </Items>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:Column>
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text='<%$ Resource : ACTIVE %>' runat="server"
                                Align="Center" Width="100" Editable="true" />

                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing runat="server">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
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
                                        <%-- <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />--%>
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel Mode="Single">
                            <Listeners>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%Resource : LOADING%>" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>


            </Items>
        </ext:Viewport>
        <%--        <uc1:ucProductSelect runat="server" ID="ucProductSelect" />--%>
    </form>
</body>
</html>
