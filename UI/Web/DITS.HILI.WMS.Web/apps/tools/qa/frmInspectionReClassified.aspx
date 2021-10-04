<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionReClassified.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmInspectionReClassified" %>

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
            var deleteProduct = function(ReclassifiedDetailID) {
                var grid = #{grdDataList}; 
                var rec =0;
                for(i=0;i<grid.store.getCount();i++)
                {
                   
                    if(grid.store.data.items[i].data.ReclassifiedDetailID == ReclassifiedDetailID)
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

            var beforeEditCheck = function(editor, e, eOpts){ 
                
                if(e.record.data.ReclassifiedQty > e.record.data.PalletQty)
                {
                    e.cancel = true;
                    Ext.MessageBox.show({
                        title:'Warning',
                        msg: "Can't edit item.",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                   
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
                                        <ext:FieldContainer runat="server" FieldLabel="<% $Resource : RECLASS_NO %>" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="txtReclassCode"
                                                    ReadOnly="true"
                                                    AllowBlank="false" />
                                                <ext:Hidden runat="server" ID="hddReclassId" >
                                                </ext:Hidden>
                                                <ext:Button runat="server" Icon="Add" Text="Add Product" Margins="0 0 0 5" ID="btnProductSelect" TabIndex="12">
                                                    <Listeners>
                                                        <Click Fn="getProduct" />
                                                    </Listeners>
                                                </ext:Button>
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
                                        <ext:ModelField Name="ReclassifiedDetailID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ReclassifiedDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Width="100" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' Width="200"  />
                            <ext:Column runat="server" DataIndex="PalletCode" Text='<%$ Resource : PALLETNO %>' Width="150" />

                            <ext:NumberColumn runat="server" DataIndex="ReclassifiedQty" Text="<% $Resource : INS_QTY %>" Format="#,###.00" Align="Right" Width="100" /> 
 
                            <ext:Column runat="server" DataIndex="UnitName" Text='<%$ Resource : UNIT %>' Width="100"></ext:Column>
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>"  Format="dd/MM/yyyy" Width="100" />
                            <ext:Column runat="server" DataIndex="Location" Text='<%$ Resource : LOCATION %>' Width="150"></ext:Column>
                            <ext:Column runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE_NAME %>' Width="100"></ext:Column>
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        
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
