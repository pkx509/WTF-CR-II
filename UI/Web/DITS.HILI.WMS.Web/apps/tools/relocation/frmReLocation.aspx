<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReLocation.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.relocation.frmReLocation" %>

<%@ Register Src="~/apps/tools/relocation/_usercontrol/ucProductReLocationSelect.ascx" TagPrefix="uc1" TagName="ucProductReLocationSelect" %>

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
                <ext:FormPanel runat="server" ID="FormPanelDetail" AutoScroll="true"
                    BodyPadding="3" Region="North" Frame="true" Layout="ColumnLayout" Margins="3 3 0 3">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Re-LocationNo" Flex="1" ID="txtReLocationNo" ReadOnly="true" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:DateField ID="txtStartDate" runat="server" FieldLabel="Start Date"
                                            TabIndex="4"
                                            MaxLength="10" EnforceMaxLength="true"
                                            Format="dd/MM/yyyy" EmptyText="dd/MM/yyyy" AllowBlank="false" AllowOnlyWhitespace="false" Flex="1">
                                        </ext:DateField>
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField ID="txtDocumentRef" FieldLabel="Document Ref" runat="server" Text="" TabIndex="2" AllowOnlyWhitespace="false" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="txtCompleteDate" runat="server" FieldLabel="Complete Date"
                                            TabIndex="5"
                                            MaxLength="10" EnforceMaxLength="true"
                                            Format="dd/MM/yyyy" EmptyText="dd/MM/yyyy" AllowBlank="false" AllowOnlyWhitespace="false" Flex="1">
                                        </ext:DateField>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField ID="txtStatus" FieldLabel="Status" runat="server" Text="" TabIndex="9" ReadOnly="true" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="txtReLocationDate" runat="server"
                                            TabIndex="3"
                                            MaxLength="10" EnforceMaxLength="true"
                                            Format="dd/MM/yyyy" EmptyText="dd/MM/yyyy" AllowBlank="false" FieldLabel="Re-Location Date" AllowOnlyWhitespace="false" Flex="1">
                                        </ext:DateField>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField ID="txtRemark" FieldLabel="Remark" runat="server" Text="" TabIndex="6" AllowOnlyWhitespace="false" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:MultiCombo runat="server" FieldLabel="Assign" Width="260" TabIndex="8" Flex="1" AllowOnlyWhitespace="false">
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
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="GridReLocationDetail" runat="server" Margins="0 0 0 3" Region="Center" 
                    Frame="true" SortableColumns="false">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Container runat="server" Layout="HBoxLayout" Flex="1">
                                    <Items>
                                        <ext:ComboBox runat="server" Flex="1" FieldLabel="Warehouse" LabelAlign="Right" LabelWidth="100"   />
                                        <ext:ComboBox runat="server" Flex="1" FieldLabel="Location" MarginSpec="0 0 0 5" LabelAlign="Right" LabelWidth="60"  />
                                        <ext:Button runat="server" MarginSpec="0 0 0 5" Text="Add Product" Icon="Add">
                                            <Listeners>
                                                <Click Handler="getProduct();"></Click>
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill />
                            </Items>
                        </ext:Toolbar>

                    </TopBar>
                    <Store>
                        <ext:Store ID="Store1" runat="server" PageSize="20">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="Quanlity" />
                                        <ext:ModelField Name="ConfirmQuanlity" />
                                        <ext:ModelField Name="StockUnit" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="Sub Status" />
                                        <ext:ModelField Name="Warehouse" />
                                        <ext:ModelField Name="FromLocation" />
                                        <ext:ModelField Name="SuggesLocation" />
                                        <ext:ModelField Name="ConfirmLocation" />
                                        <ext:ModelField Name="Pallet" />
                                        <ext:ModelField Name="Remark" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">

                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="50">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                    <ext:CommandFill></ext:CommandFill>
                                </Commands>

                            </ext:CommandColumn>

                            <ext:Column ID="colProductCode" runat="server"
                                DataIndex="ProductCode" Text="Product Code"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colProduct_Name" runat="server" DataIndex="Product_Name"
                                Text="Product Name" Flex="1" Align="Left">
                            </ext:Column>
                            <ext:Column ID="colLot" runat="server" DataIndex="Lot"
                                Text="LotNo" Flex="1" EnforceMaxLength="true"
                                Width="100" Align="Center">
                            </ext:Column>
                            <ext:NumberColumn ID="colQuantity" runat="server" DataIndex="Quantity"
                                Text="Quantity" Flex="1" Format="#,###.00" Align="Right">
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="colQuantityConfirm" runat="server" DataIndex="QuantityConfirm"
                                Text="Quantity Confirm" Flex="1" Format="#,###.00" Align="Right">
                            </ext:NumberColumn>
                            <ext:Column ID="colStockUnit" runat="server" DataIndex="StockUnit"
                                Text="Stock Unit" Flex="1" Align="Left">
                            </ext:Column>
                            <ext:Column ID="colStatus" runat="server" DataIndex="Status"
                                Text="Status" Flex="1" Align="Left">
                            </ext:Column>
                            <ext:Column ID="colSubStatus" runat="server" DataIndex="Sub Status"
                                Text="SubStatus" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colWarehouse" runat="server" DataIndex="Warehouse"
                                Text="Warehouse" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colFromLocation" runat="server" DataIndex="FromLocation"
                                Text="From Location" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colSuggesLocation" runat="server" DataIndex="SuggesLocation"
                                Text="Sugges Location" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colConfirmLocation" runat="server" DataIndex="ConfirmLocation"
                                Text="Confirm Location" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colPallet" runat="server" DataIndex="Pallet"
                                Text="Pallte" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>
                                                        <ext:Column ID="colRemark" runat="server" DataIndex="Remark"
                                Text="Remark" MaxLength="50" EnforceMaxLength="true"
                                Flex="1" Align="Center">
                            </ext:Column>

                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
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
                                                        <Select Handler="#{GridReLocationDetail}.store.pageSize = parseInt(this.getValue(), 10);
                                                                                 #{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
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
        <uc1:ucProductReLocationSelect runat="server" ID="ucProductReLocationSelect" />
    </form>
</body>
</html>
