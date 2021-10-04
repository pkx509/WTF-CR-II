<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionDamage.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.frmInspectionDamage" %>


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
                if (e.record.data.InspectionStatus != 'QA_Inspection' ) {
                    App.txtReprocessQty.setDisabled(true);
                    App.txtRejectQty.setDisabled(true); 
                }
                else{
                    App.txtReprocessQty.setDisabled(false);
                    App.txtRejectQty.setDisabled(false); 
                } 
                return true;
            };
            var edit = function (editor, e) {

                App.direct.SaveDamage(e.record.data);
            };
            var cancelEditCheck = function (editor, e, eOpts) {

            };

            var validateSave = function () {
                var plugin = this.editingPlugin;



                var record = App.grdDataList.getSelectionModel().getSelection()[0];


                record.commit();
                if (record.data.InspectionStatus != 'QA_Inspection') {
                    plugin.completeEdit();
                    return;
                }
                // var result = App.direct.SaveDamage(record.data);
                //    alert(result.valid);
                //    if (pass)
                //        plugin.completeEdit();

                App.direct.SaveDamage(record.data, App.txtReprocessQty.getValue(), App.txtRejectQty.getValue(), {
                    success: function (result) {
                        if (!result.valid) {
                            return;
                        }

                        plugin.completeEdit();
                    }
                });

            };
            var prepare = function (grid, toolbar, rowIndex, record) {
                var firstButton = toolbar.items.get(0);
                if (record.data.InspectionStatus != 'QA_Inspection') {
                    firstButton.setDisabled(true);
                    firstButton.setTooltip("Disabled");
                }
            };
            var onColumnCheckChange = function (column, rowIndex, record, checked) {
                //if (record.data.InspectionStatus == 'QA_Inspection')
                //    alert('');
                return record.data.InspectionStatus != 'QA_Inspection';
            };

            var ReprocessChange = function (obj,code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.DamageCode == code)
                {    
                    if (obj.checked) {
                        record.data.IsReprocess = true;
                    } else {
                        record.data.IsReprocess = false;
                    }
                } 
            }


            var RejectChange = function (obj, code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.DamageCode == code) {
                    if (obj.checked) {
                        record.data.IsReject = true;
                    } else {
                        record.data.IsReject = false;
                    }
                }
            }

            var RejectRenderer = function (value, meta, record, index) {  
                if (record.data.InspectionStatus != 'QA_Inspection' && record.data.DispatchRejectStatus == false)
                {
                    return '<input type="checkbox" id="chkIsReject" onchange="RejectChange(this,\'' + record.data.DamageCode + '\')"/>';
                } 
            };
            var ReprocessRenderer = function (value, meta, record, index) {

                if (record.data.InspectionStatus != 'QA_Inspection' && record.data.DispatchReprocessStatus == false) {
                    return '<input type="checkbox" id="chkReprocess" onchange="ReprocessChange(this,\'' + record.data.DamageCode + '\')"/>';
                }
            };
     </script>
    </ext:XScript>


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
                                <ext:FieldSet Margins="0 5 0 5" Title="<%$ Resource : SEARCH_INFO %>" runat="server" Layout="AnchorLayout" AutoScroll="false" Height="55" Flex="1">
                                    <Items>

                                        <ext:FieldContainer runat="server"
                                            Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>
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

                                                <ext:DateField runat="server"
                                                    ID="dtStartDate"
                                                    FieldLabel='<%$ Resource : DAMAGE_START %>'
                                                    MaxLength="10"
                                                    AllowBlank="false"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    Flex="1"
                                                    LabelAlign="Right" />
                                                <ext:DateField runat="server"
                                                    ID="dtEndDate"
                                                    AllowBlank="false"
                                                    FieldLabel='<%$ Resource : DAMAGE_END %>'
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    Format="dd/MM/yyyy"
                                                    Flex="1"
                                                    LabelAlign="Right" />

                                                <ext:ComboBox runat="server"
                                                    ID="cmbLine"
                                                    FieldLabel="<% $Resource : LINE %>"
                                                    Flex="1"
                                                    EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                    DisplayField="LineCode"
                                                    ValueField="LineID"
                                                    TypeAhead="false"
                                                    MinChars="0"
                                                    TriggerAction="Query"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    PageSize="20"
                                                    LabelAlign="Right">
                                                    <ListConfig MinWidth="250"></ListConfig>
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


                                                <ext:TextField runat="server" ID="txtSearch" EmptyText='<%$ Resource : SEARCH_WORDING %>' PaddingSpec="0 5 0 5">
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
                                <ext:Model ID="Model" runat="server" IDProperty="DamageID">
                                    <Fields>
                                        <ext:ModelField Name="DamageID" />
                                        <ext:ModelField Name="DamageCode" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="DamageQty" />
                                        <ext:ModelField Name="ReprocessQty" />
                                        <ext:ModelField Name="RejectQty" />
                                        <ext:ModelField Name="DamageDate" Type="Date" />
                                        <ext:ModelField Name="ApproveDate" Type="Date" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="UnitName" />
                                        <ext:ModelField Name="StatusName" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="InspectionStatus" />
                                        <ext:ModelField Name="IsReject" />
                                        <ext:ModelField Name="IsReprocess" />
                                        <ext:ModelField Name="DispatchRejectStatus" />
                                        <ext:ModelField Name="DispatchReprocessStatus" />
                                        <ext:ModelField Name="ReprocessPalletCode" />
                                        <ext:ModelField Name="RejectPalletCode" />
                                        <ext:ModelField Name="ReasonName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="DamageID" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="80">
                                <Commands>
                                    <ext:GridCommand Icon="Accept" ToolTip-Text="Approve" Text="Approve" CommandName="Approve" />
                                </Commands>
                                <PrepareToolbar Fn="prepare" />
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">  
                                        <Confirmation ConfirmRequest="true" Message="Do you want to Approve?" Title="Save" />
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.DamageID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:Column ID="Column11" runat="server" DataIndex="DamageCode" Text='<%$ Resource : DAMAGECODE %>' Align="Left" Width="100" />
                            <ext:Column ID="colGroup_CODE" runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCTCODE %>' Align="Left" Width="100" />
                            <ext:Column ID="Column2" runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCT_NAME %>' Align="Left" Width="150">
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="PalletCode" Text='<%$ Resource : PALLETNO %>' Align="Left" Width="130" />
                            <ext:NumberColumn ID="colDmgQty" Format="#,###.00" runat="server" DataIndex="DamageQty" Text='<%$ Resource : INS_QTY %>' Align="Right" Width="100" />

                            <ext:NumberColumn ID="colInspectionQty" Format="#,###.00" runat="server" DataIndex="ReprocessQty" Text='<%$ Resource : REPROCESS_QTY %>' Align="Right" Width="100">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtReprocessQty">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                             
                            
                            <ext:Column ID="Column13" runat="server" DataIndex="IsReprocess"  Text="Reprocess"  Width="70" Align="Center" >
                                <Renderer Fn="ReprocessRenderer" />
                            </ext:Column>

                            <ext:NumberColumn ID="NumberColumn1" Format="#,###.00" runat="server" DataIndex="RejectQty" Text='<%$ Resource : REJECT_QTY %>' Align="Right" Width="100">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtRejectQty">
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn> 

                            <ext:Column ID="Column12" runat="server" DataIndex="IsReject"  Text="Reject" Width="70"  Align="Center">
                                <Renderer Fn="RejectRenderer" />
                            </ext:Column>
                             

                            <ext:DateColumn ID="Column3" runat="server" DataIndex="DamageDate" Text='<%$ Resource : DAMAGE_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="100" />
                            <ext:DateColumn ID="Column1" runat="server" DataIndex="ApproveDate" Text='<%$ Resource : APPROVE_DATE %>' Vtype="daterange" Align="Left" Format="dd/MM/yyyy" Width="100" />
                            <ext:Column ID="Column9" runat="server" DataIndex="UnitName" Text='<%$ Resource : UNIT %>' Align="Left" Width="80" />
                            <ext:Column ID="Column5" runat="server" DataIndex="WarehouseName" Text='<%$ Resource : WAREHOUSE %>' Align="Left" Width="150" />
                            <ext:Column ID="Column6" runat="server" DataIndex="LocationNo" Text='<%$ Resource : LOCATION_NO %>' Align="Left" Width="150" />
                            <ext:DateColumn ID="Column7" runat="server" DataIndex="MFGDate" Text='<%$ Resource : MFGDATE %>' Align="Left" Vtype="daterange" Format="dd/MM/yyyy" Width="100" />
                            <ext:Column ID="Column8" runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE %>' Align="Left" Width="100" />
                            <ext:Column ID="Column10" runat="server" DataIndex="StatusName" Text='<%$ Resource : STATUS %>' Align="Left" Width="150"/>
                            <ext:Column ID="Column14" runat="server" DataIndex="ReasonName" Text='<%$ Resource : REASON %>' Align="Left" Width="200"/>
                            
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" SaveHandler="validateSave">
                            <Listeners>

                                <BeforeEdit Fn="beforeEditCheck" />
                                <%--<CancelEdit Fn="cancelEditCheck"></CancelEdit>--%>
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="false" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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

                                <ext:ToolbarFill runat="server" />

                                <ext:Button ID="Button1" runat="server" Icon="Report" Text='เบิกเพื่อทำลาย' MarginSpec="0 0 0 10">
                                    <DirectEvents>
                                        <Click OnEvent="btnDispatchDamage_Click" Buffer="300">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true" Message="Do you want to Dispatch?" Title="Save" />
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>

                                </ext:Button>

                                <ext:Button ID="btnDispatch" runat="server" Icon="Report" Text='เบิกเพื่อส่งซ่อม' MarginSpec="0 0 0 10">
                                    <DirectEvents>
                                        <Click OnEvent="btnDispatchRepair_Click" Buffer="300">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true" Message="Do you want to Dispatch?" Title="Save" />
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>

                                </ext:Button>

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
