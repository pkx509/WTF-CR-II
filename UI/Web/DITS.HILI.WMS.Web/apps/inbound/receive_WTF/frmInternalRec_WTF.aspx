<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInternalRec_WTF.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.inbound.receive_WTF.frmInternalRec_WTF" %>

<%@ Register Src="~/apps/inbound/receive_WTF/_usercontrol/_ucProductforInternalRec.ascx" TagPrefix="uc1" TagName="ucProductforInternalRec" %>

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

            var receiveID;

            var sumTotal = function () {

                var sumQTY = 0;
                var grid = App.grdDataList;

                for (i = 0; i < grid.store.getCount() ; i++) {
                    sumQTY += grid.store.data.items[i].data.QTY;
                }

                App.txtTotalQTY.setValue(parseFloat(sumQTY).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            };


            var getComboObj = function (combobox) {
                var v = combobox.getValue();
                var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                return recordValue;
            }

            var ReceiveTypeSelect = function () {
                App.cmbDispatchType.clearValue();
                var recordValue = getComboObj(this);
                var refDocID = recordValue.data.RefDocumentID;
                var pStatus = recordValue.data.ProductStatus;
                var isNormal = recordValue.data.IsNormal;
                var fromReprocess = recordValue.data.FromReprocess;
                var toReprocess = recordValue.data.ToReprocess;
                var isCredit = recordValue.data.IsCreditNote;
                var isItemChange = recordValue.data.IsItemChange;
                var isWithoutGoods = recordValue.data.IsWithoutGoods;

                App.hdProductStatus.setValue(pStatus.Code);
                App.hdProductStatusID.setValue(pStatus.ProductStatusID);
                App.hdIsNormal.setValue(isNormal);
                App.hdIsCreditNote.setValue(isCredit);
                App.hdFromReprocess.setValue(fromReprocess);
                App.hdToReprocess.setValue(toReprocess);
                App.hdIsItemChange.setValue(isItemChange);
                App.hdIsWithoutGoods.setValue(isWithoutGoods);
                App.hdReferenceDispatchTypeID.setValue(recordValue.data.RefDocumentID);

                App.direct.LoadDispatchType(refDocID);

                App.cmbLine.clearValue()
                App.cmbPONo.clearValue();
                App.nbQTY.setValue(0);
                App.txtSearchProductName.setValue();
                App.txtSearchProductCode.setValue();
                App.StoreOfDataList.removeAll();
                App.cmbProductUnitEdit.clearValue();
                sumTotal();
            }

            var PopupProductSelect = function () {
                App.direct.btnBrowseProduct_Click();
            }

            var beforeEdit = function () {
                App.cmbProductUnitEdit.clearValue();
                App.Store2.load();
            }

            var validateSave = function () {

                var plugin = this.editingPlugin;

                var cmbUnitData = getComboObj(App.cmbProductUnitEdit);

                if (cmbUnitData != false) {
                    App.cmbProductUnitEdit.setRawValue(cmbUnitData.data.UnitAndPalletQTY + ' ');
                    plugin.context.record.set(cmbUnitData.data);
                }

                var date = App.dtMFGDateEdit.getValue();
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                record.data.LotNo = date.getFullYear() + ("0" + (date.getMonth() + 1)).slice(-2) + ("0" + date.getDate()).slice(-2);
                record.commit();

                App.direct.ValidateProduct(Ext.encode(App.grdDataList.getRowsValues({ selectedOnly: false }))
                      , cmbUnitData.data.ProductUnitID
                      , record.data.ProductID
                      , record.data.LotNo , {
                          success: function (result) {

                              if (result == false) {
                                  record.data.UnitID = App.hdUnitID.getValue();
                                  record.data.Unit = App.hdUnit.getValue();
                                  record.commit();
                                  return;
                              }
                          }
                      });

                sumTotal();
                plugin.completeEdit();
            }

            var productUnitChange = function () {
                var recordValue = getComboObj(this);
                var record = App.grdDataList.getSelectionModel().getSelection()[0];

                App.hdUnitID.setValue(record.data.UnitID);
                App.hdUnit.setValue(record.data.Unit);
                App.txtUnitID.setValue(recordValue.data.ProductUnitID);
            };

            var prepareToolbar = function (grid, toolbar, rowIndex, record) {

                //toolbar.items.getAt(1).setDisabled(true);

                //if (record.data.ReceiveStatus == 'New') {
                //    toolbar.items.getAt(1).setDisabled(false);
                //}
            };

            var cmbOrderType_Change = function () {
                var recordValue = getComboObj(this);
                if (recordValue.data.field1 == 'EXPORT') {
                    App.txtOrderNo.focus();
                    App.txtOrderNo.allowBlank = 0;

                }
                else {
                    App.txtOrderNo.focus();
                    App.txtOrderNo.allowBlank = 1;
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
                <ext:FormPanel runat="server"
                    Region="North"
                    Frame="true"
                    Margins="3 3 0 3"
                    Layout="AnchorLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                    <Items>

                        <ext:Hidden runat="server" ID="hdReceiveID" />
                        <ext:Hidden runat="server" ID="hdReceiveStatus" />
                        <ext:Hidden runat="server" ID="hdUnitID" />
                        <ext:Hidden runat="server" ID="hdUnit" />
                        <ext:Hidden runat="server" ID="hdProductStatusID" />
                        <ext:Hidden runat="server" ID="hdProductStatus" />
                        <ext:Hidden runat="server" ID="hdProductID" />

                        <ext:Hidden runat="server" ID="hdReferenceDispatchTypeID" />
                        <ext:Hidden runat="server" ID="hdMFGDate" />
                        <ext:Hidden runat="server" ID="hdFromReprocess" />
                        <ext:Hidden runat="server" ID="hdToReprocess" />
                        <ext:Hidden runat="server" ID="hdIsNormal" />
                        <ext:Hidden runat="server" ID="hdIsCreditNote" />
                        <ext:Hidden runat="server" ID="hdIsItemChange" />
                        <ext:Hidden runat="server" ID="hdIsWithoutGoods" />

                        <ext:FieldSet runat="server" ID="fsHeaderSection" Layout="AutoLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">
                            <Items>
                                <ext:Container Layout="ColumnLayout" runat="server">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server"
                                                            ID="txtReceiveCode"
                                                            Flex="1"
                                                            ReadOnly="true"
                                                            AllowOnlyWhitespace="false"
                                                            FieldLabel="<% $Resource : RECEIVENO %>" />

                                                        <%--                                                        <ext:Button runat="server" ID="btnSelectReceiveCode" Text="..." Margins="0 0 0 5">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnSelectReceiveCode_Click" />
                                                            </DirectEvents>
                                                        </ext:Button>--%>
                                                    </Items>
                                                </ext:FieldContainer>

                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtOrderNo" FieldLabel="<% $Resource : ORDERNO %>" Flex="1"  Regex="^[A-Za-z0-9_/_ ]+$"/>
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <%--<ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtInvoiceNo" FieldLabel="<% $Resource : INVOICENO %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>--%>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbOrderType"
                                                            FieldLabel="<% $Resource : ORDER_TYPE %>"
                                                            Flex="1"
                                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                            AllowOnlyWhitespace="false"
                                                            AllowBlank="false"
                                                            ForceSelection="true"
                                                            TypeAhead="true">
                                                            <Items>
                                                                <ext:ListItem Text="LOCAL" Value="LOCAL" />
                                                                <ext:ListItem Text="EXPORT" Value="EXPORT" />
                                                            </Items>
                                                            <Listeners>
                                                                <Select Fn="cmbOrderType_Change" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>

                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtReceiveStatus" FieldLabel="<% $Resource : RECEIVESTATUS %>" ReadOnly="true" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="ColumnLayout" runat="server">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.5">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbReceiveType"
                                                            FieldLabel="<% $Resource : RECEIVETYPE %>"
                                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                            DisplayField="Name"
                                                            ValueField="DocumentTypeID"
                                                            TypeAhead="false"
                                                            MinChars="0"
                                                            Flex="1"
                                                            TriggerAction="All"
                                                            QueryMode="Remote"
                                                            AutoShow="false"
                                                            AllowOnlyWhitespace="false"
                                                            AllowBlank="false"
                                                            PageSize="20">
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
                                                            <Listeners>
                                                                <Select Fn="ReceiveTypeSelect" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip runat="server">
                                                                    <Listeners>
                                                                        <BeforeShow Fn="
                                                                            function updateTipBody(tip) 
                                                                            {
                                                                                    tip.update(#{cmbReceiveType}.getDisplayValue());
                                                                            }">
                                                                        </BeforeShow>
                                                                    </Listeners>
                                                                </ext:ToolTip>
                                                            </ToolTips>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbDispatchType"
                                                            FieldLabel="<% $Resource : DISPATCHTYPE %>"
                                                            Flex="1"
                                                            DisplayField="Name"
                                                            ValueField="DocumentTypeID"
                                                            TypeAhead="false"
                                                            MinChars="0"
                                                            TriggerAction="All"
                                                            QueryMode="Remote"
                                                            AutoShow="false"
                                                            ReadOnly="true"
                                                            PageSize="20">
                                                            <Store>
                                                                <ext:Store ID="Store1" runat="server" AutoLoad="false" PageSize="20">
                                                                    <Model>
                                                                        <ext:Model ID="Model1" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="DocumentTypeID" />
                                                                                <ext:ModelField Name="Name" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <ToolTips>
                                                                <ext:ToolTip runat="server">
                                                                    <Listeners>
                                                                        <BeforeShow Fn="
                                                                            function updateTipBody(tip) 
                                                                            {
                                                                                    tip.update(#{cmbDispatchType}.getDisplayValue());
                                                                            }">
                                                                        </BeforeShow>
                                                                    </Listeners>
                                                                </ext:ToolTip>
                                                            </ToolTips>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:DateField runat="server"
                                                            ID="dtEstReceiveDate"
                                                            FieldLabel="<% $Resource : RECEIVE_DATE_EST %>"
                                                            MaxLength="10"
                                                            EnforceMaxLength="true"
                                                            Format="dd/MM/yyyy"
                                                            AllowOnlyWhitespace="false"
                                                            Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbPONo"
                                                            FieldLabel="<% $Resource : PONO %>"
                                                            Flex="1"
                                                            DisplayField="PONo"
                                                            ValueField="PONo"
                                                            TypeAhead="false"
                                                            MinChars="0"
                                                            TriggerAction="Query"
                                                            QueryMode="Remote"
                                                            AutoShow="false"
                                                            PageSize="20">
                                                            <ListConfig LoadingText="Searching..." MinWidth="200" />
                                                            <Store>
                                                                <ext:Store runat="server" AutoLoad="false" PageSize="20">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=POList">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model4" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="PONo" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Parameters>
                                                                        <ext:StoreParameter Name="documentTypeID" Value="#{cmbReceiveType}.getValue()" Mode="Raw" />
                                                                        <ext:StoreParameter Name="dispatchStatus" Value="100" Mode="Value" />
                                                                    </Parameters>
                                                                </ext:Store>
                                                            </Store>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.25">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbLine"
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
                                                            AllowOnlyWhitespace="false"
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
                                                                        <ext:Model ID="Model3" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="LineID" />
                                                                                <ext:ModelField Name="LineCode" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Parameters>
                                                                       <%-- <ext:StoreParameter Name="LineType" Value="NP" Mode="Value" />--%>
                                                                    </Parameters>
                                                                </ext:Store>
                                                            </Store>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtRemark" FieldLabel="<% $Resource : REMARK %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                        <ext:FieldSet runat="server" ID="fsProductSection" Layout="ColumnLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">

                            <FieldDefaults LabelAlign="Right" LabelWidth="100" />

                            <Items>
                                <ext:Container Layout="HBoxLayout" runat="server">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtSearchProductCode" FieldLabel="<% $Resource : PRODUCTCODE %>" Flex="1" AllowBlank="false">
                                            <Listeners>
                                                <SpecialKey Handler=" if(e.getKey() == 13){ PopupProductSelect(); }" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:Button runat="server" ID="btnProductCode" Text="..." MarginSpec="0 0 0 10">
                                            <Listeners>
                                                <Click Fn="PopupProductSelect" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:TextField runat="server" ID="txtSearchProductName" MarginSpec="0 0 0 10" ReadOnly="true" InputWidth="300" />
                                        <ext:NumberField runat="server" ID="nbQTY" FieldLabel="<% $Resource : QTY %>" EmptyText="0" EmptyNumber="0" MinValue="0" />
                                        <ext:Button runat="server" ID="btnAddProduct" Icon="Add" Text="<% $Resource : ADDITEM %>" MarginSpec="0 0 0 10" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnAddProduct_Click"
                                                    Before="#{btnAddProduct}.setDisabled(true);"
                                                    Complete="#{btnAddProduct}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="<% $Resource : ADDPRODUCT %>" MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                    </Items>

                    <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); #{btnAddProduct}.setDisabled(!valid);" />
                    </Listeners>

                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" MarginSpec="0 5 0 5">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false" RemotePaging="true">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ReceiveDetailID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="LotNo" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />
                                        <ext:ModelField Name="EXPDate" Type="Date" />
                                        <ext:ModelField Name="QTY" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="PackageQTY" />
                                        <ext:ModelField Name="ConfirmQTY" />
                                        <ext:ModelField Name="ConversionQTY" />
                                        <ext:ModelField Name="UnitID" />
                                        <ext:ModelField Name="Unit" />
                                        <ext:ModelField Name="StatusID" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="Width" />
                                        <ext:ModelField Name="Length" />
                                        <ext:ModelField Name="Height" />
                                        <ext:ModelField Name="Remark" />
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
                                            <ext:Parameter Name="receiveDetailID" Value="record.data.ReceiveDetailID" Mode="Raw" />
                                            <ext:Parameter Name="productID" Value="record.data.ProductID" Mode="Raw" />
                                            <ext:Parameter Name="unitID" Value="record.data.UnitID" Mode="Raw" />
                                            <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit' || command=='Confirm' ) return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                        <EventMask ShowMask="true" Msg="Delete" MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbar" />
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCTCODE %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCTNAME %>" Width="150" Flex="1" />
                            <ext:Column runat="server" DataIndex="LotNo" Text="<%$ Resource : LOTNO %>" />
                            <ext:DateColumn runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>" Format="dd/MM/yyyy">
                                <Editor>
                                    <ext:DateField ID="dtMFGDateEdit" runat="server" AllowOnlyWhitespace="false" />
                                </Editor>
                            </ext:DateColumn>
                            <ext:NumberColumn runat="server" DataIndex="QTY" Text="<% $Resource : QUANTITY %>" Format="#,###.00" Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" ID="nbQTYEdit" MinText="0" MinValue="0" />
                                </Editor>
                            </ext:NumberColumn>
                            <%--<ext:NumberColumn runat="server" DataIndex="QTY" Text="<% $Resource : CONFIRMQTY %>" Format="#,###.00" Align="Right">
                                <Editor>
                                    <ext:NumberField runat="server" ID="nbConfirmQTYEdit" MinText="0" MinValue="0" />
                                </Editor>
                            </ext:NumberColumn>--%>
                            <ext:Column runat="server" DataIndex="UnitID" Hidden="true">
                                <Editor>
                                    <ext:TextField ID="txtUnitID" runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="Unit" Text="<%$ Resource : UNIT %>">
                                <Editor>
                                    <ext:ComboBox runat="server"
                                        ID="cmbProductUnitEdit"
                                        EmptyText="<% $Resource : PLEASE_SELECT %>"
                                        DisplayField="UnitAndPalletQTY"
                                        ValueField="ProductUnitID"
                                        TypeAhead="true"
                                        MinChars="0"
                                        TriggerAction="All"
                                        QueryMode="Remote"
                                        AutoShow="false"
                                        AllowOnlyWhitespace="false"
                                        AllowBlank="false">
                                        <Store>
                                            <ext:Store ID="Store2" runat="server" AutoLoad="false">
                                                <Proxy>
                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProductUnit">
                                                        <ActionMethods Read="GET" />
                                                        <Reader>
                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                        </Reader>
                                                    </ext:AjaxProxy>
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="ProductUnitID" />
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="UnitAndPalletQTY" />
                                                            <ext:ModelField Name="Barcode" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Parameters>
                                                    <ext:StoreParameter Name="ProductID" Value="App.grdDataList.getSelectionModel().getSelection()[0].data.ProductID" Mode="Raw" />
                                                    <ext:StoreParameter Name="isQuery" Value="true" Mode="Value" />
                                                </Parameters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="productUnitChange" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="Status" Text="<%$ Resource : STATUS %>" />
                            <ext:Column runat="server" DataIndex="Remark" Text="<%$ Resource : REMARK %>">
                                <Editor>
                                    <ext:TextArea ID="txtRemarkEdit" runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" SaveHandler="validateSave" AutoCancel="false" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit fn="beforeEdit" />
                                <%--<BeforeEdit Handler="#{nbConfirmQTYEdit}.setMaxValue(e.record.data.QTY);" />--%>
                            </Listeners>
                        </ext:RowEditing>
                    </Plugins>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                    <BottomBar>
                        <ext:Toolbar runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:StatusBar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:TextField runat="server"
                                            ID="txtTotalQTY"
                                            FieldLabel="<% $Resource : TOTALQTY %>"
                                            ReadOnly="true"
                                            LabelAlign="Right"
                                            EmptyText="0.00"
                                            FieldStyle="text-align: right" />
                                    </Items>
                                </ext:StatusBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill runat="server" />

                                        <ext:Button runat="server" ID="btnGenDispatch" Icon="Disk" Text="<% $Resource : GENDISPATCH %>" MarginSpec="0 0 0 5" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnGenDispatch_Click"
                                                    Before="#{btnGenDispatch}.setDisabled(true);"
                                                    Complete="#{btnGenDispatch}.setDisabled(false);"
                                                    Buffer="350">
                                                    <%-- <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>--%>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button runat="server" ID="btnConfirm" Icon="Disk" Text="<% $Resource : CONFIRM %>" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnConfirm_Click"
                                                    Before="#{btnConfirm}.setDisabled(true);"
                                                    Complete="#{btnConfirm}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button runat="server" ID="btnSave" Icon="Disk" Text="<% $Resource : SAVE %>" MarginSpec="0 0 0 5" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click"
                                                    Before="#{btnSave}.setDisabled(true);"
                                                    Complete="#{btnSave}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <uc1:ucProductforInternalRec runat="server" ID="ucProductforInternalRec" />

    </form>
</body>
</html>
