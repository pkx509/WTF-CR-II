<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmAllDispatchRule.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllDispatchRule" %>

<%--<%@ Register Src="~/Modules/Opt/_Share/ucProductSelect.ascx" TagPrefix="uc1" TagName="ucProductSelect" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
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

            var edit = function (editor, e) {

                if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
                    console.log(e.record.data);
                    App.direct.Edit(e.record.data.RuleId, e.field, e.originalValue, e.value, e.record.data);
                }
            };

 </script>
    </ext:XScript>

    <style>
        div#ListComboSubCustomer {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListcmbShippingTo {
            border-top-width: 1 !important;
            width: 300px !important;
        }
    </style>


</head>
<body>

    <form id="form2" runat="server">
        <ext:ResourceManager ID="ResourceManager2" runat="server">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport2" runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="GridPanel1" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:FieldSet runat="server" Layout="AnchorLayout" AutoScroll="false" Height="60">
                                    <Items>
                                        <ext:FieldContainer runat="server"
                                            Layout="HBoxLayout" MarginSpec="20 0 0 0">
                                            <Items>
                                                <ext:TextField runat="server" Width="200" ID="txtRule_name" FieldLabel="Rule Name" AllowBlank="false"
                                                    LabelAlign="Right" LabelWidth="80" TabIndex="11" SelectOnFocus="true" Height="20">
                                                </ext:TextField>


                                                <ext:NumberField runat="server"
                                                    ID="nbLotAging"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    AllowBlank="false"
                                                    FieldLabel='<%$ Resource : LOT_AGING %>'
                                                    LabelAlign="Right"
                                                    LabelWidth="60"
                                                    TabIndex="14"
                                                    AllowDecimals="true"
                                                    Width="140"
                                                    EnforceMaxLength="true"
                                                    MaxLength="50"
                                                    DecimalPrecision="3"
                                                    IndicatorText="days" Height="20" />

                                                <ext:NumberField runat="server"
                                                    ID="nbLotDuration"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    FieldLabel='<%$ Resource : DURATION_NOT_OVER %>'
                                                    LabelAlign="Right"
                                                    LabelWidth="120"
                                                    TabIndex="14"
                                                    AllowDecimals="true"
                                                    Width="200"
                                                    EnforceMaxLength="true"
                                                    MaxLength="50"
                                                    IndicatorText="days" Height="20" />

                                                <ext:NumberField runat="server"
                                                    ID="nbLotLessThan"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    FieldLabel='<%$ Resource : NO_MORE_THAN %>'
                                                    LabelAlign="Right"
                                                    LabelWidth="100"
                                                    TabIndex="14"
                                                    AllowDecimals="true"
                                                    Width="180"
                                                    EnforceMaxLength="true"
                                                    MaxLength="50"
                                                    IndicatorText="lots" Height="20" />

                                                <ext:Checkbox ID="chkIsActive" runat="server" FieldLabel='<%$ Resource : ACTIVE %>' LabelWidth="50" Name="IsActive" Checked="true" LabelAlign="Right" />
                                                <ext:Checkbox ID="chkIsDefault" runat="server" FieldLabel='<%$ Resource : ISDEFAULT  %>' LabelWidth="50" LabelAlign="Right" />

                                                <ext:Button runat="server"
                                                    ID="btnAddItem"
                                                    Icon="Add"
                                                    Text='<%$ Resource : SAVE %>'
                                                    TabIndex="13"
                                                    MarginSpec="0 0 0 10">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAddSpecialBookingRule_Click">
                                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>

                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldSet>

                                <ext:ToolbarFill />
                                <ext:Checkbox ID="ckbIsActive" runat="server" FieldLabel='<%$ Resource : SHOW_ALL %>' LabelWidth="50" Width="100" Name="IsActive" Checked="true">
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Event" />
                                    </DirectEvents>
                                </ext:Checkbox>
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="100" Height="25">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="Button1" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
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
                        <ext:Store ID="StoreSpecialBookingRule" runat="server" PageSize="20" AutoLoad="false">
                            <Proxy>
                                <ext:AjaxProxy Url="">
                                    <ActionMethods Read="GET" />
                                    <Reader>
                                        <ext:JsonReader Root="data" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model1" runat="server" IDProperty="RuleId">
                                    <Fields>
                                        <ext:ModelField Name="RuleId" />
                                        <ext:ModelField Name="RuleName" />
                                        <ext:ModelField Name="Aging" />
                                        <ext:ModelField Name="UnitAging" />
                                        <ext:ModelField Name="DurationNotOver" />
                                        <ext:ModelField Name="UnitDuration" />
                                        <ext:ModelField Name="LotNo" />
                                        <ext:ModelField Name="NoMoreThanDo" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                        <ext:ModelField Name="IsDefault" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="RuleName" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="CommandColumn3" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.RuleId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="80" Align="Center" />
                            <ext:Column ID="Column2" runat="server" DataIndex="RuleName" Text='<%$ Resource : RULE_NAME %>' Align="Left">
                                <Editor>
                                    <ext:TextField runat="server" Width="150" ID="txtRuleName"
                                        LabelAlign="Right" TabIndex="11" SelectOnFocus="true" AllowBlank="false">
                                    </ext:TextField>
                                </Editor>
                            </ext:Column>
                            <ext:NumberColumn ID="colAging" Format="#,0" runat="server" DataIndex="Aging" Text='<%$ Resource : AGING %>' Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtAging">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="colDurationNotOver" Format="#,0" runat="server" DataIndex="DurationNotOver" Text='<%$ Resource : DURATION %>' Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtDurationNotOver">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="colNoMoreThanDo" Format="#,0" runat="server" DataIndex="NoMoreThanDo" Text='<%$ Resource : NO_MORE_THAN %>' Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtNoMoreThanDo">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>

                            <ext:CheckColumn DataIndex="IsActive" Text='<%$ Resource : ACTIVE %>' runat="server" Align="Center" Width="60" Editable="true" />
                            <ext:CheckColumn DataIndex="IsDefault" Text='<%$ Resource : ISDEFAULT %>' runat="server" Align="Center" Width="60" Editable="true" />

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
                                <ext:Label ID="Label2" runat="server" Text='<%$ Resource : PAGESIZE %>' />
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
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
                                        <Select Handler="#{GridPanel1}.store.pageSize = parseInt(this.getValue(), 10);
                                                        #{PagingToolbar1}.moveFirst();" />
                                    
                                    </Listeners>
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
                        <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="<%Resource : LOADING%>" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>


            </Items>
        </ext:Viewport>
        <%--        <uc1:ucProductSelect runat="server" ID="ucProductSelect" />--%>
    </form>
</body>
</html>
