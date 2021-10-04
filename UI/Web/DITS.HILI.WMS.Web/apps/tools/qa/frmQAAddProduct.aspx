<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmQAAddProduct.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmQAAddProduct" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

    <ext:XScript runat="server">
        <script>

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
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Product Code" Flex="1" ID="txtProductCode" ReadOnly="false" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Product Name" Flex="1" ID="txtProductName" ReadOnly="false" TabIndex="3" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Warehouse" Flex="1" ID="cmbWarehouse"
                                            TabIndex="5" DisplayField="Code" ValueField="WarehouseID">
                                            <Store>
                                                <ext:Store ID="WarehouseStore" runat="server">
                                                    <Model>
                                                        <ext:Model ID="Model1" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="WarehouseID" />
                                                                <ext:ModelField Name="Code" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:Container>


                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.5">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Lot" Flex="1" ID="txtLot" ReadOnly="false" TabIndex="2" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ComboBox runat="server" FieldLabel="Location" Flex="1" ID="cmbLoadingIn"
                                            TabIndex="4" DisplayField="Code" ValueField="LocationID">
                                            <Store>
                                                <ext:Store ID="LocationStore" runat="server">
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="LocationID" />
                                                                <ext:ModelField Name="Code" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>

                                    </Items>
                                </ext:FieldContainer>


                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="btnSearch" runat="server" Text="Search" MarginSpec="0 0 0 5" Icon="Magnifier">
                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click"></Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:Container>


                    </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="GridPutawayDetail" runat="server" Margins="0 0 0 3" Region="Center"
                    Frame="true" SortableColumns="false">

                    <Store>
                        <ext:Store ID="PutawayDetailStore" runat="server" PageSize="20">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>

                                        <ext:ModelField Name="BaseQuantity" />
                                        <ext:ModelField Name="BaseUOMName" />
                                        <ext:ModelField Name="BaseUnitID" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="DateCreated" />
                                        <ext:ModelField Name="DateModified" />
                                        <ext:ModelField Name="DocumentType" />
                                        <ext:ModelField Name="DocumentTypeID" />
                                        <ext:ModelField Name="ExpirationDate" />
                                        <ext:ModelField Name="InstanceID" />
                                        <ext:ModelField Name="IsActive" />
                                        <ext:ModelField Name="IsComplete" />
                                        <ext:ModelField Name="LineID" />
                                        <ext:ModelField Name="LocationName" ModelName="Location" ServerMapping="Location.Code" />
                                        <ext:ModelField Name="Location" ModelName="Location" ServerMapping="Location" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="ManufacturingDate" />
                                        <ext:ModelField Name="PackageName" />
                                        <ext:ModelField Name="PackageNextID" />
                                        <ext:ModelField Name="PackagePrevID" />
                                        <ext:ModelField Name="PackageWeight" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="Price" />
                                        <ext:ModelField Name="Product" ModelName="Product" ServerMapping="Product" />
                                        <ext:ModelField Name="ProductBaseUOM" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductCodeCollection" />
                                        <ext:ModelField Name="ProductHeight" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductLength" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductOwner" />
                                        <ext:ModelField Name="ProductOwnerID" />
                                        <ext:ModelField Name="ProductPriceUOM" />
                                        <ext:ModelField Name="ProductStatus" />
                                        <ext:ModelField Name="ProductStatusID" />
                                        <ext:ModelField Name="ProductSubStatus" />
                                        <ext:ModelField Name="ProductSubStatusID" />
                                        <ext:ModelField Name="ProductUOM" />
                                        <ext:ModelField Name="ProductUnitPriceID" />
                                        <ext:ModelField Name="ProductWeight" />
                                        <ext:ModelField Name="ProductWidth" />
                                        <ext:ModelField Name="PutAwayCollection" />
                                        <ext:ModelField Name="PutAwayItemID" />
                                        <ext:ModelField Name="PutAwayJobMapCollection" />
                                        <ext:ModelField Name="Quantity" />
                                        <ext:ModelField Name="ReferenceBaseID" />
                                        <ext:ModelField Name="ReferenceID" />
                                        <ext:ModelField Name="RemainQuantity" />
                                        <ext:ModelField Name="Remark" />
                                        <ext:ModelField Name="Sequence" />
                                        <ext:ModelField Name="Start" />
                                        <ext:ModelField Name="StockUOMNmae" />
                                        <ext:ModelField Name="StockUnitID" />
                                        <ext:ModelField Name="SupplierID" />
                                        <ext:ModelField Name="UserCreated" />
                                        <ext:ModelField Name="UserModified" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">

                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />

                            <ext:Column ID="colProductCode" runat="server"
                                DataIndex="ProductCode" Text="Product Code"
                                Width="100" Align="Center" Flex="1">
                            </ext:Column>
                            <ext:Column ID="colProduct_Name" runat="server" DataIndex="ProductName"
                                Text="Product Name" MinWidth="200" Align="Left" Flex="1">
                            </ext:Column>
                            <ext:Column ID="colLot" runat="server" DataIndex="Lot"
                                Text="LotNo" MaxLength="50" EnforceMaxLength="true"
                                Width="100" Align="left" Flex="1">
                            </ext:Column>
                            <ext:NumberColumn ID="colQuanlity" runat="server" DataIndex="Quantity"
                                Text="Quantity" Width="100" Format="#,###" Align="Right" Flex="1">
                            </ext:NumberColumn>

                            <ext:Column ID="colStockUnit" runat="server" DataIndex="StockUOMNmae"
                                Text="Stock Unit" MinWidth="100" Align="Left" Flex="1">
                            </ext:Column>
                            <ext:Column ID="colFromLocation" runat="server" DataIndex="LocationName"
                                Text="Location" MinWidth="100" Align="Left" Flex="1">
                            </ext:Column>


                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>

                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>

                                        <ext:Button ID="btnGen" runat="server"
                                            Icon="ApplicationAdd" Text="Gen" Width="80" TabIndex="18" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnGen_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Value="Ext.encode(#{GridPutawayDetail}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="generate ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to gen?" />
                                                </Click>
                                            </DirectEvents>

                                        </ext:Button>

                                        <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="80" TabIndex="19" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnClear_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="80" TabIndex="20" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnExit_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                    </SelectionModel>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
