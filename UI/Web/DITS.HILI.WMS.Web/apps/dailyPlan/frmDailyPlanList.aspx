<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDailyPlanList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.dailyPlan.frmDailyPlanList" %>

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

            var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {

                if (record.data.IsReceive == true) {
                    toolbar.items.getAt(1).setDisabled(true);
                }

            };

        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="true"
                    BodyPadding="3"
                    Region="North"
                    Frame="true"
                    Layout="ColumnLayout"
                    Margins="3 3 0 3">

                    <FieldDefaults LabelAlign="Right" LabelWidth="150" />

                    <Items>

                        <ext:Hidden runat="server" ID="hdSection" />

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtStartDate"
                                            FieldLabel='<%$ Resource : START_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbLine"
                                            FieldLabel='<%$ Resource : LINE %>'
                                            Flex="1"
                                            EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                            DisplayField="LineCode"
                                            ValueField="LineID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            PageSize="20">
                                            <Store>
                                                <ext:Store ID="StoreLine" runat="server" AutoLoad="false" PageSize="20">
                                                    <Proxy>
                                                        <ext:PageProxy DirectFn="App.direct.BindLineData" />
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="LineID" />
                                                                <ext:ModelField Name="LineCode" />
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
                    </Items>
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtEndDate"
                                            FieldLabel='<%$ Resource : END_DATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtProduct" FieldLabel='<%$ Resource : PRODUCT_CODE_NAME %>' Flex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Checkbox runat="server" ID="chkIsReceive" FieldLabel='<%$ Resource : RECEIVE %>' />
                                        <ext:BoxSplitter runat="server" Flex="1" />
                                        <ext:Button runat="server" ID="btnSearch" Text='<%$ Resource : SEARCH %>' MarginSpec="0 0 0 5" Icon="Magnifier">
                                            <Listeners>
                                                <Click Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text='<%$ Resource : ADD_NEW %>'>
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false" RemotePaging="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProductionID" />
                                        <ext:ModelField Name="ProductionDetailID" />
                                        <ext:ModelField Name="Seq" type="Int" SortType="AsInt" />
                                        <ext:ModelField Name="ProductionDate" Type="Date" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="LineId" />
                                        <ext:ModelField Name="LineCode" />
                                        <ext:ModelField Name="ProductionQty" />
                                        <ext:ModelField Name="PalletQty" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="OrderType" />
                                        <ext:ModelField Name="IsReceive" />
                                        <ext:ModelField Name="Weight_G" />
                                        <ext:ModelField Name="Film" />
                                        <ext:ModelField Name="Oil" />
                                        <ext:ModelField Name="FD" />
                                        <ext:ModelField Name="Box" />
                                        <ext:ModelField Name="Powder" />
                                        <ext:ModelField Name="Stamp" />
                                        <ext:ModelField Name="Sticker" />
                                        <ext:ModelField Name="Mark" />
                                        <ext:ModelField Name="DeliveryDate" />
                                        <%--                                        <ext:ModelField Name="CustomerCode" />
                                        <ext:ModelField Name="CustomerName" />--%>
                                        <ext:ModelField Name="WorkingTime" />
                                        <ext:ModelField Name="OilType" />
                                        <ext:ModelField Name="Formula" />
                                        <ext:ModelField Name="DailyPlanStatus" />
                                        <ext:ModelField Name="IsActive" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Seq" Direction="ASC" />
                            </Sorters>
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
                                            <ext:Parameter Name="productionDetailID" Value="record.data.ProductionDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit' || command=='Confirm' ) return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                        <EventMask ShowMask="true" Msg="Delete" MinDelay="300" />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="productionDetailID" Value="record.data.ProductionDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <%--<ext:CommandColumn runat="server" ID="colProductInfo" Sortable="false" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Zoom" ToolTip-Text='<%$ Resource : OPEN_PRODUCT_DIALOG %>' CommandName="OpenProduct" />
                                </Commands>

                                   <DirectEvents>
                                    <Command OnEvent="ViewProduct_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="ProductSystemCode" Value="record.data.Product_System_Code" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>--%>

                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                            <ext:Column ID="colProduction_Seq" runat="server" DataIndex="Seq" Text='<%$ Resource : SEQ %>' Width="40" Align="Center" Flex="1" />
                            <ext:DateColumn ID="colProductPlan_Date1" runat="server" DataIndex="ProductionDate" Text='<%$ Resource : DATE %>' Width="130" Align="Right" Vtype="daterange" Format="dd/MM/yyyy" />
                            <%--<ext:Column ID="colProduct_Code" runat="server" DataIndex="ProductCode" Text='<%$ Resource : PRODUCT_CODE %>' Width="130" Align="Center" />--%>
                            <ext:Column ID="colProduct_Name" runat="server" DataIndex="ProductName" Text='<%$ Resource : PRODUCT_NAME %>' Width="200" />
                            <ext:Column ID="colLine" runat="server" DataIndex="LineCode" Text='<%$ Resource : LINE %>' Width="40" />
                            <ext:Column ID="colQty1" runat="server" DataIndex="ProductionQty" Text='<%$ Resource : QTY %>' Width="130" Align="Right" />
                            <ext:Column ID="colPalletQTY" runat="server" DataIndex="PalletQty" Text='<%$ Resource : PALLETQTY %>' Width="130" Align="Right" />
                            <ext:Column ID="colProduct_UOM_Name" runat="server" DataIndex="ProductUnitName" Text='<%$ Resource : UNIT %>' Width="100" />
                            <ext:Column ID="colOrderNo" runat="server" DataIndex="OrderNo" Text='<%$ Resource : ORDER_NO %>' Width="130" />
                            <ext:Column ID="colOrderType" runat="server" DataIndex="OrderType" Text='<%$ Resource : ORDER_TYPE %>' Width="130" />
                            <ext:Column ID="colIsReceive" runat="server" DataIndex="IsReceive" Text='<%$ Resource : RECEIVE %>' Width="130" Align="Right" Hidden="true" />
                            <ext:Column ID="colWeight_G" runat="server" DataIndex="Weight_G" Text='<%$ Resource : WEIGHT %>' Width="130" Align="Right" />
                            <ext:Column ID="colFilm" runat="server" DataIndex="Film" Text='<%$ Resource : FILM %>' Width="130" Align="Left" />
                            <ext:Column ID="colOil" runat="server" DataIndex="Oil" Text='<%$ Resource : OIL %>' Width="130" Align="Left" />
                            <ext:Column ID="colFD" runat="server" DataIndex="FD" Text='<%$ Resource : FD %>' Width="130" Align="Left" />
                            <ext:Column ID="colStamp" runat="server" DataIndex="Stamp" Text='<%$ Resource : STAMP %>' Width="130" Align="Left" />
                            <ext:Column ID="colSticker" runat="server" DataIndex="Sticker" Text='<%$ Resource : STICKER %>' Width="130" Align="Left" />
                            <ext:Column ID="colMark" runat="server" DataIndex="Mark" Text='<%$ Resource : MARK %>' Width="130" Align="Left" />
                            <ext:DateColumn ID="colDeliveryDate" runat="server" DataIndex="DeliveryDate" Text='<%$ Resource : DELIVER_DATE %>' Width="130" Vtype="daterange" Format="dd/MM/yyyy" />
                            <%--<ext:Column ID="colCustomerCode" runat="server" DataIndex="CustomerCode" Text="Customer Code" Width="130" Align="Left" />
                            <ext:Column ID="colCustomerName" runat="server" DataIndex="CustomerName" Text="Customer Name" Width="130" Align="Left" />--%>
                            <ext:Column ID="colWorkingTime" runat="server" DataIndex="WorkingTime" Text='<%$ Resource : WORKING_TIME %>' Width="130" />
                            <ext:Column ID="colOilType" runat="server" DataIndex="OilType" Text='<%$ Resource : FRYING_OIL %>' Width="130" />
                            <ext:Column ID="colFormula" runat="server" DataIndex="Formula" Text='<%$ Resource : INGREDIENT %>' Width="130" />
                            <ext:Column ID="colProduction_Status" runat="server" DataIndex="DailyPlanStatus" Text='<%$ Resource : STATUS %>' Weight="50" />
                            <ext:CheckColumn ID="colIsActive1" DataIndex="IsActive" Text='<%$ Resource : ACTIVE %>' runat="server" Align="Center" Width="100" />
                        </Columns>
                    </ColumnModel>

                    <BottomBar>

                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="false" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
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
                                    <Listeners>
                                        <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:ToolbarFill runat="server" />

                                <ext:Button ID="btnDelete" runat="server" Icon="Delete" Text='<%$ Resource : DELETE_ALL %>' MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnDelete_Click" Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : true}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Sending ..." MinDelay="100" />
                                            <Confirmation ConfirmRequest="true" Message='<%$ Message : MSG00003 %>' />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnSend" runat="server" Icon="Disk" Text='<%$ Resource : SEND_TO_RECEIVE %>' MarginSpec="0 0 0 5">
                                    <DirectEvents>
                                        <Click OnEvent="btnToReceive_Click" Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : true}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Sending ..." MinDelay="100" />
                                            <Confirmation ConfirmRequest="true" Message='<%$ Resource : SEND_TO_RECEIVE %>' />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:PagingToolbar>


                        <%--  <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.7">
                                    <Items>
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
                                                    <Listeners>
                                                        <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>
                                        <ext:Button ID="btnDelete" runat="server" Icon="Delete" Text='<%$ Resource : DELETE_ALL %>' MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnDelete_Click" Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : true}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="Sending ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message='<%$ Message : MSG00003 %>' />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnSend" runat="server" Icon="Disk" Text='<%$ Resource : SEND_TO_RECEIVE %>' MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnToReceive_Click" Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : true}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="Sending ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message='<%$ Resource : SEND_TO_RECEIVE %>' />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Toolbar>--%>
                    </BottomBar>

                    <DirectEvents>
                        <CellDblClick OnEvent="grdDataList_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="productionDetailID" Value="record.data.ProductionDetailID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>

                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" CheckOnly="true" />
                    </SelectionModel>

                </ext:GridPanel>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
