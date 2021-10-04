<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionDispatch.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmInspectionDispatch" %>

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
            var ReprocessChange = function (obj, code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.ReclassifiedDetailID == code) {
                    if (obj.checked) {
                        record.data.IsReprocess = true;
                    } else {
                        record.data.IsReprocess = false;
                    }
                }
            } 
            var validateSave = function () {
                var plugin = this.editingPlugin;



                var record = plugin.context.record.data; 
                if (record.ReclassifiedQty < (App.txtReprocessQty.getValue() + App.txtRejectQty.getValue())) {
                    return;
                }
                plugin.completeEdit();
            }
            var edit = function (editor, e) {

                //App.direct.SaveDamage(e.record.data);
            };
            var RejectChange = function (obj, code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.ReclassifiedDetailID == code) {
                    if (obj.checked) {
                        record.data.IsReject = true;
                    } else {
                        record.data.IsReject = false;
                    }
                }
            }
            var RejectRenderer = function (value, meta, record, index) { 
                    return '<input type="checkbox" id="chkIsReject" onchange="RejectChange(this,\'' + record.data.ReclassifiedDetailID + '\')"/>';
 
            };
            var ReprocessRenderer = function (value, meta, record, index) {
                 
                    return '<input type="checkbox" id="chkReprocess" onchange="ReprocessChange(this,\'' + record.data.ReclassifiedDetailID + '\')"/>';
 
            };
        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server"
                    Region="North"
                    Frame="true"
                    Margins="3 3 0 3"
                    Layout="AnchorLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                    <Items>
                        <ext:FieldSet runat="server" Layout="ColumnLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">
                            <Items>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:FieldContainer runat="server" FieldLabel="<% $Resource : RECLASS_NO %>" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtReclassCode"
                                                    ReadOnly="true" Width="150"
                                                    AllowBlank="false" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:ComboBox runat="server"
                                            FieldLabel="<% $Resource : FROM_STATUS %>"
                                            ID="cmbProductStatusEdit"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="Description"
                                            ValueField="ProductStatusID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            Editable="false"
                                            AllowOnlyWhitespace="false"
                                            AllowBlank="false">
                                            <Store>
                                                <ext:Store ID="StorePStatus" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductStatusByDocType">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ProductStatusID" />
                                                                <ext:ModelField Name="Description" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="IsInspectionReclassify" Value="true" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:ComboBox runat="server"
                                            FieldLabel="<% $Resource : TO_STATUS %>"
                                            ID="cmbProductStatusTo"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="Description"
                                            ValueField="ProductStatusID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            Editable="false"
                                            AllowOnlyWhitespace="false"
                                            AllowBlank="false">
                                            <Store>
                                                <ext:Store ID="Store1" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductStatusByDocType">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ProductStatusID" />
                                                                <ext:ModelField Name="Description" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                    <Parameters>
                                                        <ext:StoreParameter Name="IsInspectionReclassify" Value="true" Mode="Raw" />
                                                    </Parameters>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                            </Listeners>
                                        </ext:ComboBox>


                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtReclassStatus"
                                            Flex="1"
                                            ReadOnly="true"
                                            FieldLabel="<% $Resource : RECLASSIFIED_STATUS %>" />

                                        <ext:DateField runat="server"
                                            ID="dtApproveDate"
                                            FieldLabel='<%$ Resource : APPROVE_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1"
                                            LabelAlign="Right" />

                                        <ext:TextArea runat="server"
                                            ID="txtDesc"
                                            Flex="1" Rows="1"
                                            FieldLabel="<% $Resource : DESCRIPTION %>" />
                                        
                                                <ext:Hidden runat="server" ID="hddReclassId">
                                                </ext:Hidden>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>

                    </Items>

                    <Listeners>
                        <%--  <ValidityChange Handler="#{btnSave}.setDisabled(!valid); #{btnProduce}.setDisabled(!valid);" />--%>
                    </Listeners>

                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" MarginSpec="0 5 0 5">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ItemID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="EXPDate" Type="Date" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="PalletQty" />
                                        <ext:ModelField Name="ReclassifiedQty" />
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="UnitName" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="ReclassifiedBaseUnitID" />
                                        <ext:ModelField Name="ReclassifiedBaseQty" />
                                        <ext:ModelField Name="ReclassifiedUnitID" />
                                        <ext:ModelField Name="Location" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ReclassifiedID" />
                                        <ext:ModelField Name="ReclassifiedDetailID" />
                                        <ext:ModelField Name="ReclassifiedCode" />
                                        <ext:ModelField Name="IsReject" DefaultValue="false" Type="Boolean" />
                                        <ext:ModelField Name="IsReprocess" DefaultValue="false" Type="Boolean" />
                                        <ext:ModelField Name="RejectQty" />
                                        <ext:ModelField Name="ReprocessQty" />
                                        <ext:ModelField Name="ReclassStatus" />
                                        <ext:ModelField Name="TotalRejectQty" />
                                        <ext:ModelField Name="TotalReprocessQty" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' MinWidth="100" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' MinWidth="200" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text='<%$ Resource : PALLETNO %>' MinWidth="150" />

                            <ext:NumberColumn runat="server" DataIndex="ReclassifiedQty" Text="<% $Resource : INS_QTY %>" Format="#,###.00" Align="Right" MinWidth="100" />
                             

                            <ext:NumberColumn runat="server" DataIndex="ReprocessQty" Text="<% $Resource : REPROCESS_QTY %>" Format="#,###.00" Align="Right" MinWidth="100" >
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtReprocessQty">
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>


                            <ext:Column ID="colIsReprocess" runat="server" DataIndex="IsReprocess" Text="Reprocess" Width="70" Align="Center">
                                <Renderer Fn="ReprocessRenderer" />
                            </ext:Column> 
                            <ext:NumberColumn ID="NumberColumn1" Format="#,###.00" runat="server" DataIndex="RejectQty" Text='<%$ Resource : REJECT_QTY %>' Align="Right" Width="100">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtRejectQty">
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>

                            <ext:Column ID="colIsReject" runat="server" DataIndex="IsReject" Text="Reject" Width="70" Align="Center">
                                <Renderer Fn="RejectRenderer" />
                            </ext:Column>

                            <ext:Column runat="server" DataIndex="UnitName" Text='<%$ Resource : UNIT %>' MinWidth="100"></ext:Column>
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>" Format="dd/MM/yyyy" MinWidth="100" />
                            <ext:Column runat="server" DataIndex="Location" Text='<%$ Resource : LOCATION %>' MinWidth="150"></ext:Column>
                            <ext:Column runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE_NAME %>' MinWidth="100"></ext:Column>
                        </Columns>
                    </ColumnModel>

                    <Plugins>

                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false"   SaveHandler="validateSave" >
                            <Listeners>  
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                    <BottomBar>
                        <ext:Toolbar runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />

                                        <ext:Button ID="btnDispatchDamage" runat="server" Icon="Report" Text='เบิกเพื่อทำลาย' MarginSpec="0 0 0 10">
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

                                        <ext:Button ID="btnApprove" runat="server" Icon="Accept" Text='<%$ Resource : APPROVING %>' MarginSpec="0 0 0 10">
                                            <DirectEvents>
                                                <Click OnEvent="btnApprove_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Approve?" Title="Save" />
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                         <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$Resource : EXIT%>" Width="60" TabIndex="22">
                                            <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners> 
                                        </ext:Button>

                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

