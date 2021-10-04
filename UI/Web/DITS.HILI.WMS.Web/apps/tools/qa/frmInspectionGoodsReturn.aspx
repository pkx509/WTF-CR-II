<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionGoodsReturn.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmInspectionGoodsReturn" %>

<%@ Register Src="~/apps/tools/qa/_usercontrol/ucQAPalletMultiSelect.ascx" TagPrefix="uc" TagName="ucQAPalletMultiSelect" %>

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
            var deleteProduct = function(GoodsReturnDetailId) {
                var grid = #{grdDataList}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.GoodsReturnDetailId == GoodsReturnDetailId)
                    { 
                        rec  =i;   
                        break;
                    }
                        
                } 
                grid.store.removeAt(rec);
            } 

            var getProduct = function () {

                App.direct.GetProduct("");
            };

            var validateSave = function () {
                var plugin = this.editingPlugin;



                var record = plugin.context.record.data; 
                if (record.ReceiveQTY < (App.txtReprocessQty.getValue() + App.txtRejectQty.getValue())) {
                    return;
                }
                plugin.completeEdit();
            }

            var ReprocessChange = function (obj, code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.GoodsReturnDetailId == code) {
                    if (obj.checked) {
                        record.data.IsReprocess = true;
                    } else {
                        record.data.IsReprocess = false;
                    }
                }
            } 
            var RejectChange = function (obj, code) {
                var record = App.grdDataList.getSelectionModel().getSelection()[0];
                if (record.data.GoodsReturnDetailId == code) {
                    if (obj.checked) {
                        record.data.IsReject = true;
                    } else {
                        record.data.IsReject = false;
                    }
                }
            }
            var RejectRenderer = function (value, meta, record, index) {  
                
                if (record.data.RejectStatus == false) 
                {
                    return '<input type="checkbox" id="chkIsReject" onchange="RejectChange(this,\'' + record.data.GoodsReturnDetailId + '\')"/>';
 
                } 
            };
            var ReprocessRenderer = function (value, meta, record, index) {

                if (record.data.ReprocessStatus == false) 
                {
                    return '<input type="checkbox" id="chkReprocess" onchange="ReprocessChange(this,\'' + record.data.GoodsReturnDetailId + '\')"/>';
 
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
                        <ext:FieldSet runat="server" Layout="ColumnLayout" MarginSpec="2 2 2 2" PaddingSpec="7 5 5 5">
                            <Items>

                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:TextField runat="server"
                                                    ID="txtReceiveCode"
                                                    ReadOnly="true" FieldLabel="<% $Resource : RECEIVENO %>"
                                                    AllowBlank="false" /> 
                                        
                                                <ext:Hidden runat="server" ID="hddGoodsReturnID"/>

                                         <ext:TextField runat="server"
                                                    ID="txtReceiveDate"
                                                    ReadOnly="true" FieldLabel="<% $Resource : RECEIVE_DATE %>"
                                                    AllowBlank="false" /> 


                                         <ext:TextField runat="server"
                                                    ID="txtPono"
                                                    ReadOnly="true" FieldLabel="<% $Resource : PONO %>"
                                                    AllowBlank="false" />  

                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:TextField runat="server"
                                            ID="txtGoodsReturnStatus"
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
                                        <ext:ModelField Name="GoodsReturnId" />
                                        <ext:ModelField Name="GoodsReturnDetailId" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ReceiveLot" />
                                        <ext:ModelField Name="ReceiveUnitID" />
                                        <ext:ModelField Name="MFGDate" Type="Date" />  
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="UnitName" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="ConversionQty" /> 
                                        <ext:ModelField Name="GoodsReturnStatus" />
                                        <ext:ModelField Name="ReprocessQty" />
                                        <ext:ModelField Name="RejectQty" />
                                        <ext:ModelField Name="ReceiveQTY" />
                                        <ext:ModelField Name="ReceiveDetailID" />
                                        <ext:ModelField Name="IsReprocess" Type="Boolean" DefaultValue="false" />
                                        <ext:ModelField Name="IsReject" Type="Boolean" DefaultValue="false"/>
                                        <ext:ModelField Name="RejectStatus" Type="Boolean" />
                                        <ext:ModelField Name="ReprocessStatus" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns> 
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Width="150" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' Width="200" />

                            <ext:NumberColumn runat="server" DataIndex="ReceiveQTY" Text="<% $Resource : RECEIVE_QTY %>" Format="#,###.00" Align="Right" Width="100" />

                            <ext:NumberColumn runat="server" DataIndex="ReprocessQty" Text="<% $Resource : REPROCESS_QTY %>" Format="#,###.00" Align="Right" Width="100" >
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtReprocessQty">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            
                            <ext:Column ID="colIsReprocess" runat="server" DataIndex="IsReprocess" Text="Reprocess" Width="70" Align="Center">
                                <Renderer Fn="ReprocessRenderer" />
                            </ext:Column> 

                            <ext:NumberColumn runat="server" DataIndex="RejectQty" Text="<% $Resource : REJECT_QTY %>" Format="#,###.00" Align="Right" Width="100">
                                <Editor>
                                    <ext:NumberField runat="server" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="0" MinValue="0"
                                        AllowDecimals="false" Text="1" ID="txtRejectQty">
                                        <Listeners>
                                        </Listeners>
                                    </ext:NumberField>
                                </Editor>
                            </ext:NumberColumn>
                            

                            <ext:Column ID="colIsReject" runat="server" DataIndex="IsReject" Text="Reject" Width="70" Align="Center">
                                <Renderer Fn="RejectRenderer" />
                            </ext:Column>
                            <ext:Column runat="server" DataIndex="UnitName" Text='<%$ Resource : ALTERNATE_UNIT %>' Width="150"></ext:Column>
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>"  Format="dd/MM/yyyy" Width="100" />
                            <ext:Column runat="server" DataIndex="ReceiveLot" Text='<%$ Resource : LOT_NO %>' Width="150"></ext:Column>
                            <ext:Column runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE_NAME %>' Width="100"></ext:Column>

                            <ext:CommandColumn runat="server" ID="colView" Sortable="false" Align="Center" Width="100">
                                <Commands>
                                     <ext:GridCommand Icon="Magnifier" ToolTip-Text="View Pallet" CommandName="View" Text="<%$ Resource : PALLETCODE %>"  />
                                </Commands> 
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ReceiveDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" ErrorSummary="false" SaveHandler="validateSave">
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

                                        <ext:Button ID="btnInspectionDamage" runat="server" Icon="Report" Text='เบิกเพื่อทำลาย' MarginSpec="0 0 0 10">
                                            <DirectEvents>
                                                <Click OnEvent="btnInspectionDamage_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Dispatch?" Title="Save" />
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>

                                        </ext:Button>

                                        <ext:Button ID="btnInspectionRepair" runat="server" Icon="Report" Text='เบิกเพื่อส่งซ่อม' MarginSpec="0 0 0 10">
                                            <DirectEvents>
                                                <Click OnEvent="btnInspectionRepair_Click" Buffer="300">
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
                                        <ext:Button ID="btnSave" runat="server" Icon="Disk" Text='<%$ Resource : SAVE %>' MarginSpec="0 0 0 10">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click" Buffer="300">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <Confirmation ConfirmRequest="true" Message="Do you want to Save?" Title="Save" />
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
        <uc:ucQAPalletMultiSelect runat="server" ID="ucQAPalletMultiSelect" />
    </form>
</body>
</html>
