<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllProduct.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllProduct" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script src="~/resources/js/JScript.Common.js"></script>
    <style>
        input#cmbCustomer-inputEl {
            background-color: #FFFFCC;
        }
    </style>
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
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill />

                                <%-- <ext:SelectBox ID="cmbCustomer" runat="server" LabelWidth="80"
                                    DisplayField="Customer_NameTH" ValueField="Customer_Code"
                                    TriggerAction="All" EmptyText="<%$Resources:Langauge, PleaseSelect%>"
                                    FieldLabel="<%$Resources:Langauge, CustomerName%>"
                                    TypeAhead="true" PageSize="0" MinChars="0" AllowBlank="false"
                                    ForceSelection="true" AllowOnlyWhitespace="false" Width="250">
                                    <Store>
                                        <ext:Store ID="StoreCustomer" runat="server">
                                            <Model>
                                                <ext:Model ID="Model6" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Customer_Code" />
                                                        <ext:ModelField Name="Customer_NameTH" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Listeners>
                                        <Select Handler=" #{btnSearch}.fireEvent('click');#{btnAdd}.setDisabled(false);" />
                                    </Listeners>
                                </ext:SelectBox>--%>
                                <%--<ext:RadioGroup runat="server">
                                    <Items>
                                        <ext:Radio runat="server" BoxLabelAlign="After" BoxLabel="<%$Resources:Langauge, ProductSet%>" Width="120" />
                                        <ext:Radio runat="server" BoxLabelAlign="After" BoxLabel="<%$Resources:Langauge, ProductUnit%>" Width="120" />
                                    </Items>
                                </ext:RadioGroup>--%>
                                <ext:Checkbox ID="ckbIsActive" runat="server" FieldLabel='<%$ Resource : SHOW_ALL %>' LabelWidth="50" Width="100" Name="IsActive" Checked="true">
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Event" />
                                    </DirectEvents>
                                </ext:Checkbox>

                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="200">
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
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20"
                            RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="ProductCode">
                                    <Fields>
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductShapeName" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="ProductBrandName" />
                                        <ext:ModelField Name="ProductBrandID" />
                                        <ext:ModelField Name="ProductModelName" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="ProductCode" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20" Locked="true">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ProductID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20" Locked="true">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ProductID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="50" Align="Center" Locked="true" />
                            <ext:Column ID="colProduct_Code" runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Align="Left" Width="150" Locked="true" />
                            <ext:Column ID="colProduct_Name_Full" runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCT_NAME %>' Align="Left" Flex="1" MinWidth="300" />
                            <ext:Column ID="colProduct_Brand_Name" runat="server" DataIndex="ProductBrandName" Text='<%$ Resource : PRODUCT_BRAND_NAME %>' Align="Left" Width="100" />
                            <ext:Column ID="colProduct_Model" runat="server" DataIndex="ProductModelName" Text='<%$ Resource : PRODUCT_MODEL %>' Align="Left" Width="100" />
                            <ext:CheckColumn ID="txtIsActive" runat="server" DataIndex="IsActive" Text='<%$ Resource : ACTIVE %>' Align="Center" Width="60" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                        <CellClick OnEvent="gvdDataListCenter_CellClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data" Mode="Raw" />
                            </ExtraParams>
                        </CellClick>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data.ProductID" Mode="Raw" />
                            </ExtraParams>
                            <EventMask ShowMask="true" />
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="true" />
                    </View>
                </ext:GridPanel>

                <ext:FormPanel
                    ID="FormPanel1"
                    runat="server"
                    Region="East"
                    Split="true"
                    MarginSpec="5 5 5 0"
                    BodyPadding="2"
                    Title='<%$ Resource : PRODUCT_DETAILS %>'
                    Width="300"
                    Icon="Package"
                    AutoScroll="True">

                    <FieldDefaults LabelWidth="110" LabelAlign="Right" />
                    <Items>
                        <ext:FieldSet
                            runat="server"
                            ColumnWidth="0.4"
                            Title='<%$ Resource : PRODUCT_DETAILS %>'
                            MarginSpec="0 0 0 10"
                            ButtonAlign="Right">
                            <Items>
                                <%--<ext:Image runat="server" ID="txtProductImage" Width="250" Height="220" Layout="FitLayout" MarginSpec="10 10 20 10">
                        </ext:Image>--%>
                                <ext:TextField runat="server" ID="txtProductCode" FieldLabel='<%$ Resource : PRODUCT_CODE %>' />
                                <ext:TextField runat="server" ID="txtProductName" FieldLabel='<%$ Resource : PRODUCT_NAME %>' />
                                <ext:TextField runat="server" ID="txtBrandName" FieldLabel='<%$ Resource : PRODUCT_BRAND_NAME %>' />
                                <ext:TextField runat="server" ID="txtProductModel" FieldLabel='<%$ Resource : PRODUCT_MODEL %>' />
                                <ext:TextField runat="server" ID="txtProGroupLevel3" FieldLabel='<%$ Resource : PRODUCT_CATEGORY %>' />
                                <ext:TextField runat="server" ID="txtProductSharp" FieldLabel='<%$ Resource : PRODUCT_SHAPE %>' />
                                <%--<ext:NumberField runat="server" ID="txtStockBalance" FieldLabel='<%$ Resource : QUANTITY %>'
                            Format="#,###.##" ReadOnly="true" Width="250" />--%>
                            </Items>
                        </ext:FieldSet>
                    </Items>

                </ext:FormPanel>

            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
