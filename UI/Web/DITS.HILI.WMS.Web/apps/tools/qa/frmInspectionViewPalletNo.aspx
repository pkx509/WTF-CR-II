<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInspectionViewPalletNo.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmInspectionViewPalletNo" %>

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
                <ext:GridPanel ID="grdDataList" runat="server" Region="Center" Frame="true" MarginSpec="0 5 0 5">
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields> 
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="MFGDate" Type="Date" /> 
                                        <ext:ModelField Name="CompleteQTY" />  
                                        <ext:ModelField Name="LineCode" />  
                                        <ext:ModelField Name="Unit" /> 
                                        <ext:ModelField Name="ProductStatusName" /> 
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column runat="server" DataIndex="PalletCode" Text='<%$ Resource : PALLETNO %>' Width="130" />
                            <ext:Column runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Width="100" />
                            <ext:Column runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCTNAME %>' Width="150" />
                            <ext:Column runat="server" DataIndex="Lot" Text='<%$ Resource : LOT_NO %>' MinWidth="150"></ext:Column>
                            <ext:DateColumn ID="Column6" runat="server" DataIndex="MFGDate" Text="<%$ Resource : MFGDATE %>"  Format="dd/MM/yyyy" Width="100" />
                            <ext:Column runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE_NAME %>' Width="100"></ext:Column>
                            <ext:Column runat="server" DataIndex="ProductStatusName" Text='<%$ Resource : STATUS %>' Width="100"></ext:Column>
                            <ext:NumberColumn runat="server" DataIndex="CompleteQTY" Text="<% $Resource : QUANTITY %>" Format="#,###.00" Align="Right" Width="100" />
                            <ext:Column runat="server" DataIndex="Unit" Text='<%$ Resource : UNIT %>' Width="150"></ext:Column>
                        </Columns>
                    </ColumnModel>

                    <Plugins>
                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" ErrorSummary="false">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />

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
                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="18">
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
