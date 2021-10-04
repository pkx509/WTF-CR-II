<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAssignJob.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.picking.frmAssignJob" %>

<%@ Register Src="~/apps/outbound/picking/_usercontrol/ucDispatchforAssignJob.ascx" TagPrefix="uc1" TagName="ucDispatchforAssignJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
     <%-- <script type="text/javascript">
          Ext.Ajax.timeout = 180000; // 1 sec
          Ext.net.DirectEvent.timeout = 180000; // 1 sec 
      </script>--%>
    <ext:XScript runat="server">
        <script>

            var popupDispatchSelect = function () {
                App.direct.btnBrowsePO_Click();
            };

            var validateSave = function () {

                //Ext.MessageBox.show({
                //    title: 'STATUS',
                //    msg: 'update row....',
                //    buttons: Ext.MessageBox.OK,
                //});

                var plugin = this.editingPlugin;

                var record = App.grdDataList.getSelectionModel().getSelection()[0];

                var grid = #{grdDataList};
               



                var palletInfo = new Object();
                palletInfo.palletCode = App.txtPalletNoEdit.getValue();
                palletInfo.oldProductID = record.data.ProductID;
                palletInfo.orderQTY = record.data.OrderQTY;
                palletInfo.oldPalletCode = record.data.OldPalletNo;

                if(  palletInfo.oldPalletCode == palletInfo.oldPalletCode)
                { 
                    
                    plugin.completeEdit();
                    return;
                }
                var rec =-1;
                for(i=0;i<grid.store.getCount();i++)
                {  
                    if(  grid.store.data.items[i].data.PalletNo == palletInfo.palletCode && grid.store.data.items[i].data.PalletNo != palletInfo.oldPalletCode)
                    { 
                        rec  =i;   
                        break;
                    } 
                }  
                if(rec != -1)
                {
                    App.direct.ShowErrorX("MSG00090");
                    return;
                }

                var ID, Value;
                App.direct.UpdateSuggestLocation(palletInfo, {
                    success: function (result) { 
                        if (result.Lot) {
                            record.data.PickingLot = result.Lot;
                            record.data.SGLocationID = result.LocationID;
                            record.data.SGLocation = result.Location;
                            record.data.PickingQTY = 0;
                            record.data.PalletQTY = result.RemainStockQTY;
                            record.data.PalletUnitID = result.RemainStockUnitID;
                            record.data.PalletUnit = result.RemainStockUnit;
                            record.commit();
                            plugin.completeEdit();
                        }
                        else
                        { 
                            return;
                        }
                    }

                });

            }

            function popitup(url, windowName) {

                var browser = navigator.appName;
                if (browser == 'Microsoft Internet Explorer') {
                    window.opener = self;

                }
                newwindow = window.open(url, windowName, 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=900,height=600');
                window.moveTo(0, 0);
                self.close();

                if (window.focus) { newwindow.focus() }
                return false;
            }

        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server"
                    Region="North"
                    Frame="true"
                    Margins="3 3 0 3"
                    Layout="AnchorLayout">
                    <Items>
                        <ext:FieldSet runat="server" Layout="AnchorLayout" MarginSpec="5 5 5 5">

                            <FieldDefaults LabelAlign="Right" LabelWidth="120" InputWidth="200" />

                            <Items>

                                <ext:Hidden runat="server" ID="hdPickingID" />
                                <ext:Hidden runat="server" ID="hdDispatchCode" />

                                <ext:Container Layout="HBoxLayout" runat="server" MarginSpec="10 10 10 10">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtSearchPO" FieldLabel="<% $Resource : PONO %>">
                                            <Listeners>
                                                <SpecialKey Handler=" if(e.getKey() == 13){ popupDispatchSelect(); }" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:Button runat="server" ID="btnBrowsePO" Text="..." MarginSpec="0 0 0 10">
                                            <Listeners>
                                                <Click Fn="popupDispatchSelect" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:TextField runat="server" ID="txtSearchShipto" MarginSpec="0 0 0 10" ReadOnly="true" />

                                        <ext:Button runat="server" ID="btnAddPO" Icon="Add" Text="<% $Resource : ADDITEM %>" MarginSpec="0 0 0 10"></ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>

                        <ext:FieldSet runat="server" Layout="AnchorLayout" MarginSpec="5 5 5 5">

                            <FieldDefaults LabelAlign="Right" LabelWidth="120" />

                            <Items>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="10 10 0 10">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtPickingCode" FieldLabel="<% $Resource : PICKINGCODE %>" ReadOnly="true" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:ComboBox runat="server"
                                                            ID="cmbEmployee"
                                                            FieldLabel="<% $Resource : EMPLOYEE %>"
                                                            Flex="1"
                                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                                            DisplayField="GroupName"
                                                            ValueField="GroupID"
                                                            TypeAhead="false"
                                                            MinChars="0"
                                                            TriggerAction="All"
                                                            QueryMode="Remote"
                                                            AutoShow="false"
                                                            PageSize="20">
                                                            <Store>
                                                                <ext:Store ID="StoreEmployee" runat="server" AutoLoad="false" PageSize="20">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=EmployeeAssign">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model2" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="GroupID" />
                                                                                <ext:ModelField Name="GroupName" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtShippingCode" FieldLabel="<% $Resource : SHIPPINGCODE %>" ReadOnly="true" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtPONo" FieldLabel="<% $Resource : PONO %>" ReadOnly="true" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtDocNo" FieldLabel="<% $Resource : DOCUMENT_NO %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtOrderNo" FieldLabel="<% $Resource : ORDERNO %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.33">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:DateField runat="server"
                                                            ID="dtEntryDate"
                                                            FieldLabel="<% $Resource : ENTRYDATE %>"
                                                            MaxLength="10"
                                                            EnforceMaxLength="true"
                                                            Format="dd/MM/yyyy"
                                                            Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtShippingTo" FieldLabel="<% $Resource : SHIPPING_TO %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextField runat="server" ID="txtPickingStatus" FieldLabel="<% $Resource : PICKINGSTATUS %>" ReadOnly="true" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="0 10 10 10">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.66">
                                            <Items>
                                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                                    <Items>
                                                        <ext:TextArea runat="server" ID="txtRemark" FieldLabel="<% $Resource : REMARK %>" Flex="1" />
                                                    </Items>
                                                </ext:FieldContainer>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" MarginSpec="0 5 0 5">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="AssignID" />
                                        <ext:ModelField Name="BookingID" />
                                        <ext:ModelField Name="OrderPick" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="OldSGLocationID" />
                                        <ext:ModelField Name="SGLocationID" />
                                        <ext:ModelField Name="SGLocation" />
                                        <ext:ModelField Name="PickLocationID" />
                                        <ext:ModelField Name="PickLocation" />
                                        <ext:ModelField Name="OrderQTY" />
                                        <ext:ModelField Name="OrderUnitID" />
                                        <ext:ModelField Name="OrderUnit" />
                                        <ext:ModelField Name="OrderBaseQTY" />
                                        <ext:ModelField Name="OrderBaseUnitID" />
                                        <ext:ModelField Name="PickQTY" />
                                        <ext:ModelField Name="PickUnitID" />
                                        <ext:ModelField Name="PickUnit" />
                                        <ext:ModelField Name="OldPalletNo" />
                                        <ext:ModelField Name="PalletNo" />
                                        <ext:ModelField Name="PickingLot" />
                                        <ext:ModelField Name="PalletQTY" />
                                        <ext:ModelField Name="PalletUnitID" />
                                        <ext:ModelField Name="PalletUnit" />
                                        <ext:ModelField Name="Dock" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />

                          <%--  <ext:Column runat="server" DataIndex="OrderPick" Text="<%$ Resource : ORDERPICK %>">
                                <Editor>
                                    <ext:NumberField ID="nbOrderPickEdit" MinValue="0" runat="server" />
                                </Editor>
                            </ext:Column>--%>
                            <ext:Column runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCTCODE %>" />
                            <ext:Column runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCTNAME %>" MinWidth="150" Flex="1" />
                            <ext:Column runat="server" DataIndex="SGLocation" Text="<%$ Resource : LOCSUGGEST %>" MinWidth="150" />
                            <ext:Column runat="server" DataIndex="PickLocation" Text="<%$ Resource : LOCPICK %>" />
                            <ext:NumberColumn runat="server" DataIndex="OrderQTY" Text="<% $Resource : ORDERQTY %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="OrderUnit" Text="<%$ Resource : ORDERUNIT %>" />
                            <ext:NumberColumn runat="server" DataIndex="PickQTY" Text="<% $Resource : PICKQTY %>" Format="#,###.00" Align="Right">
                                <%--          <Editor>
                                    <ext:NumberField ID="nbPickingQTY" MinValue="0" runat="server" />
                                </Editor>--%>
                            </ext:NumberColumn>
                            <ext:Column runat="server" DataIndex="PickUnit" Text="<%$ Resource : PICKUNIT %>" />
                            <ext:Column runat="server" DataIndex="PalletNo" Text="<%$ Resource : PALLETNO %>" MinWidth="150">
                                <Editor>
                                    <ext:TextField ID="txtPalletNoEdit" runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:NumberColumn runat="server" DataIndex="PalletQTY" Text="<% $Resource : PALLETQTY %>" Format="#,###.00" Align="Right" />
                            <ext:Column runat="server" DataIndex="PalletUnit" Text="<%$ Resource : PALLETUNIT %>" />
                            <ext:Column runat="server" DataIndex="Dock" Text="<%$ Resource : DOCK %>" />
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" SaveHandler="validateSave" EnableViewState="true" AutoCancel="false" ErrorSummary="false">

                            <%-- <Listeners>
                                <BeforeEdit Handler="#{nbPickingQTY}.setMaxValue(e.record.data.PalletQTY);" />
                            </Listeners>--%>
                        </ext:RowEditing>
                    </Plugins>

                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>

                    <View>
                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>

                    <BottomBar>
                        <ext:Toolbar runat="server">
                            <Items>
                            <%--    <ext:Button ID="btnDelete" runat="server" Icon="Delete" Text='<%$ Resource : DELETE_ALL %>' MarginSpec="0 0 0 5">
                                        <DirectEvents>
                                        <Click OnEvent="btnDelete_Click" Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : true}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Sending ..." MinDelay="100" />
                                            <Confirmation ConfirmRequest="true" Message='<%$ Message : MSG00003 %>' />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>--%>

                                <ext:ToolbarFill runat="server" />

                                 <ext:Button ID="btnAssign" runat="server" Icon="Report" Text='<%$ Resource : CONSODOLIST %>' MarginSpec="0 0 0 5" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnAssign_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnPicking" runat="server" Icon="Report" Text='<%$ Resource : PICKINGLIST %>' MarginSpec="0 0 0 5" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnPicking_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnApprove" runat="server" Icon="Disk" Text='<%$ Resource : APPROVE %>' MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnApprove_Click" Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="<% $Resource : APPROVING %>" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text='<%$ Resource : SAVE %>' MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <uc1:ucDispatchforAssignJob runat="server" ID="ucDispatchforAssignJob" />

    </form>
</body>
</html>
