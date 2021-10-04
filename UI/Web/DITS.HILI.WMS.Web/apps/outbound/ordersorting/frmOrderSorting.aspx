<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmOrderSorting.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.ordersorting.frmOrderSorting" %>

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
            var getProduct = function () {
                App.direct.GetProduct();
            };
        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Default" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
             <ext:FormPanel runat="server" ID="FormPanelDetail"  AutoScroll="true"
                    BodyPadding="3" Region="North" Frame="true" Layout="ColumnLayout" Margins="3 3 0 3">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Dispatch No." Flex="1" ID="txtDispatchNo" ReadOnly="true" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Dispatch Type" Flex="1" ID="cmbDispatchType"
                                            AllowOnlyWhitespace="false" TabIndex="4" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:DateField runat="server" FieldLabel="Est. Date" Flex="1"
                                            AllowOnlyWhitespace="false" TabIndex="7" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Warehouse" Flex="1"
                                            AllowOnlyWhitespace="false" TabIndex="10" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Route" Flex="1" ID="txtRoute"  TabIndex="13" />
                                    </Items>
                                </ext:FieldContainer>


                            </Items>
                        </ext:Container>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Product Owner" Flex="1" AllowOnlyWhitespace="false" TabIndex="2" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="PO No." Flex="1" AllowOnlyWhitespace="false" TabIndex="5" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server" FieldLabel="Dispatch Date" Flex="1" AllowOnlyWhitespace="false" TabIndex="8" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Remark" Flex="1" TabIndex="11" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Dock" Flex="1" TabIndex="14" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Status" Flex="1" TabIndex="16" ReadOnly="true" />
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:Container>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Customer" Flex="1" AllowOnlyWhitespace="false" TabIndex="3" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Invoice No." Flex="1" TabIndex="6"  AllowOnlyWhitespace="false"  />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Container No." Flex="1" TabIndex="9" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Ref." Flex="1" TabIndex="12" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:MultiCombo runat="server" FieldLabel="Assign" Width="260" TabIndex="15" Flex="1" AllowOnlyWhitespace="false">
                                            <Items>
                                                <ext:ListItem Text="Item 1" Value="1" />
                                                <ext:ListItem Text="Item 2" Value="2" />
                                                <ext:ListItem Text="Item 3" Value="3" />
                                                <ext:ListItem Text="Item 4" Value="4" />
                                                <ext:ListItem Text="Item 5" Value="5" />
                                            </Items>

                                            <SelectedItems>
                                                <ext:ListItem Value="2" />
                                                <ext:ListItem Index="4" />
                                            </SelectedItems>
                                        </ext:MultiCombo>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Checkbox runat="server" FieldLabel="Urgent" Flex="1" TabIndex="17" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>

                </ext:FormPanel>

                  <ext:GridPanel ID="GridOrderSorting" runat="server" Margins="0 0 0 3" Region="Center"
                                    Frame="true" SortableColumns="false" Height="280">

                                    <Store>
                                        <ext:Store ID="Store2" runat="server" PageSize="20">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ProductCode" />
                                                        <ext:ModelField Name="ProductName" />
                                                        <ext:ModelField Name="Lot" />
                                                        <ext:ModelField Name="MFGDate" />
                                                        <ext:ModelField Name="EXPDate" />
                                                        <ext:ModelField Name="Price" />
                                                        <ext:ModelField Name="PriceUnit" />
                                                        <ext:ModelField Name="DispatchQTY" />
                                                        <ext:ModelField Name="ConfirmQTY" />
                                                        <ext:ModelField Name="StockUnit" />
                                                        <ext:ModelField Name="Relate" />
                                                        <ext:ModelField Name="Status" />
                                                        <ext:ModelField Name="SubStatus" />
                                                        <ext:ModelField Name="Location" />
                                                        <ext:ModelField Name="Pallet" />
                                                        <ext:ModelField Name="TotalNW" />
                                                        <ext:ModelField Name="TotalCBM" />
                                                        <ext:ModelField Name="Remark" />

                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <ColumnModel ID="ColumnModel1" runat="server">

                                        <Columns>
                                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No." Width="60" Align="Center" />
                                            <ext:CommandColumn runat="server" ID="CommandColumn2" Sortable="false" Align="Center" Width="50" Flex="1">
                                                <Commands>
                                                    <ext:CommandFill />
                                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                                    <ext:CommandFill></ext:CommandFill>
                                                </Commands>

                                            </ext:CommandColumn>

                                            <ext:Column ID="Column11" runat="server" DataIndex="ProductCode" Text="Product Code"
                                                Align="left" Flex="1" />
                                            <ext:Column ID="Column12" runat="server" DataIndex="ProductName" Text="Product Name"
                                                Align="left" Flex="1" />
                                            <ext:Column ID="Column13" runat="server" DataIndex="Lot" Text="Lot"
                                                Align="left" Flex="1" />
                                            <ext:DateColumn ID="DateColumn3" runat="server" DataIndex="MFGDate"
                                                Text="MFG Date" Align="Center" Format="dd/MM/yyyy" Flex="1" />

                                            <ext:DateColumn ID="DateColumn4" runat="server" DataIndex="EXPDate" Text="EXP Date"
                                                Align="Center" Format="dd/MM/yyyy" Flex="1">
                                            </ext:DateColumn>
                                            <ext:NumberColumn ID="NumberColumn6" runat="server" DataIndex="Price"
                                                Text="Price" Format="#,###.00" Align="Right" Flex="1">
                                            </ext:NumberColumn>
                                            <ext:NumberColumn ID="NumberColumn7" runat="server" DataIndex="PriceUnit"
                                                Text="Price Unit" Format="#,###.00" Align="Right" Flex="1">
                                            </ext:NumberColumn>
                                            <ext:NumberColumn ID="NumberColumn8" runat="server" DataIndex="DispatchQTY"
                                                Text="Dispatch QTY" Format="#,###" Align="Right" Flex="1">

                                            </ext:NumberColumn>
                                                                           <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="ConfirmQTY"
                                                Text="Confirm QTY" Format="#,###" Align="Right" Flex="1">

                                            </ext:NumberColumn>
                                            <ext:Column ID="Column14" runat="server" DataIndex="StockUnit"
                                                Text="Stock Unit" Align="Left" Flex="1" />
                                            <ext:Column ID="Column15" runat="server" DataIndex="Remark" Text="Relate"
                                                Align="left" Flex="1" />
                                            <ext:Column ID="Column16" runat="server" DataIndex="Status" Text="Status"
                                                Align="left" Flex="1" />
                                            <ext:Column ID="Column17" runat="server" DataIndex="SubStatus" Text="Sub Status"
                                                Align="left" Flex="1" />
                                            <ext:Column ID="Column18" runat="server" DataIndex="Location" Text="Location"
                                                Align="left" Flex="1" />
                                             <ext:Column ID="Column19" runat="server" DataIndex="Pallet" Text="Pallet"
                                                Align="left" Flex="1" />
                                            <ext:NumberColumn ID="NumberColumn9" runat="server" DataIndex="TotalNW"
                                                Text="Total NW" Format="#,###" Align="Right" Flex="1"/>
                                            <ext:NumberColumn ID="NumberColumn10" runat="server" DataIndex="TotalCBM"
                                                Text="Total CBM" Format="#,###" Align="Right" Flex="1"/>
                                             <ext:Column ID="Column20" runat="server" DataIndex="Remark" Text="Remark"
                                                Align="left" Flex="1" />

                                        </Columns>

                                    </ColumnModel>


                                    <BottomBar>
                                        <ext:Toolbar runat="server" ID="toolbar1">
                                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="DisplayingFromTo"
                                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                                            FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                                            BeforePageText="<center>Page</center>">
                                            <Items>
                                                <ext:Label ID="Label1" runat="server" Text="Page size:" />
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
                                                    <Listeners>
                                                        <Select Handler="#{GridOrderSorting}.store.pageSize = parseInt(this.getValue(), 10);
                                                                                 #{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>


                                        <ext:Button ID="btnApprove" runat="server" Text="Approve" Icon="Accept" Width="120" TabIndex="16">
                                        </ext:Button>
                                        <ext:Button ID="btnprint" runat="server"
                                            Icon="Printer" Text="Print" Width="80" TabIndex="17" MarginSpec="0 0 0 5">
                                        </ext:Button>

                                        <ext:Button ID="btnSave" runat="server"
                                            Icon="Disk" Text="Save" Width="80" TabIndex="18" MarginSpec="0 0 0 5">
                                        </ext:Button>

                                        <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="80" TabIndex="19" MarginSpec="0 0 0 5">
                                        </ext:Button>

                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="80" TabIndex="20" MarginSpec="0 0 0 5">
                                            <Listeners>
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                                            </Items>
                                        </ext:Toolbar>
                                    </BottomBar>
                               </ext:GridPanel>

            </Items>
        </ext:Viewport>

    </form>
</body>
</html>

