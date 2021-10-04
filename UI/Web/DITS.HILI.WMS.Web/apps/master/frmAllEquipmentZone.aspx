<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllEquipmentZone.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllEquipmentZone" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <script src="~/resources/js/JScript.Common.js"></script>


    <ext:XScript runat="server">
        <script>

            var beforeEditCheck = function (editor, e, eOpts) {
                //if (e.record.data.IsReceive == true) {
                //    e.cancel = true;

                //} else {
                //App.direct.LoadCombo();
                //App.direct.LoadOrderType(e.record.data.OrderType);
                e.cancel = false;
                //}

            };

            var getProduct = function () {

                var product_code = App.txtProductCode.getValue();


                //App.hidAddProduct_System_Code.reset();
                //App.txtAddProduct_Name_Full.reset();

                //App.direct.GetProduct(product_code);
            };


            var popupProduct = function () {
                //App.direct.GetProduct('');
            };

            var edit = function (editor, e) {

                if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
                    console.log(e.record.data);
                    App.direct.Edit(e.record.data.EquipID, e.field, e.originalValue, e.value, e.record.data);
                }
            };

            var zone_select = function(){
                var combobox = this;
                var v = combobox.getValue();
                var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                console.log(recordValue);
                #{txtWarehouse_NameAdd}.setValue(recordValue.data.WarehouseName);
                                 
            };



 </script>
    </ext:XScript>

    <style>
        div#ListComboTruckType {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListComboPhysicalZone {
            border-top-width: 1 !important;
            width: 250px !important;
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

                                <ext:FieldSet Margins="0 5 0 5" Title="<%$ Resource : SEARCH_INFO %>" runat="server" Layout="AnchorLayout" AutoScroll="false" Height="85" Flex="1">
                                    <Items>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>
                                                <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                                                    <Items>
                                                        <ext:TextField runat="server" Width="175" ID="txtEquipt_NameAdd" FieldLabel="<%$ Resource : EQUIP_NAME %>" AllowBlank="false"
                                                            LabelAlign="Right" LabelWidth="70" TabIndex="11" SelectOnFocus="true" MaxLength="20">
                                                            <Listeners>
                                                                <SpecialKey Handler=" if(e.getKey() == 13){ #{txtSerialAdd}.focus('', 10); }" />
                                                            </Listeners>
                                                        </ext:TextField>

                                                        <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : TRUCK_TYPE_NAME %>'
                                                            Layout="HBoxLayout" LabelWidth="70" LabelAlign="Right">
                                                            <Items>
                                                                <ext:ComboBox ID="cmbTruckTypeAdd" runat="server" Width="100"
                                                                    DisplayField="TypeName" ValueField="TruckTypeID" TabIndex="2"
                                                                    FieldLabel="" EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                                                    PageSize="25" MinChars="0" SelectOnFocus="true" AllowBlank="false"
                                                                    TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                                                    ForceSelection="true" AllowOnlyWhitespace="false">
                                                                    <ListConfig LoadingText="Searching..." ID="ListComboTruckType">
                                                                        <ItemTpl runat="server">
                                                                            <Html>
                                                                                <div class="search-item">
							                                              {TypeName}
						                                                </div>
                                                                            </Html>
                                                                        </ItemTpl>
                                                                    </ListConfig>
                                                                    <Store>
                                                                        <ext:Store ID="StoreTruckTypeAdd" runat="server" AutoLoad="false">
                                                                            <Proxy>
                                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckTypeOnly">
                                                                                    <ActionMethods Read="GET" />
                                                                                    <Reader>
                                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                    </Reader>
                                                                                </ext:AjaxProxy>
                                                                            </Proxy>
                                                                            <Model>
                                                                                <ext:Model ID="Model6" runat="server">
                                                                                    <Fields>
                                                                                        <ext:ModelField Name="TruckTypeID" />
                                                                                        <ext:ModelField Name="TypeName" />
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
                                                <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                                                    <Items>
                                                        <ext:TextField runat="server" Width="185" ID="txtSerialAdd" FieldLabel="<%$ Resource : SERIAL_NUMBER %>" AllowBlank="false"
                                                            LabelAlign="Right" LabelWidth="80" TabIndex="11" SelectOnFocus="true" MaxLength="20">
                                                            <Listeners>
                                                                <SpecialKey Handler=" if(e.getKey() == 13){ #{cmbTruckTypeAdd}.focus('', 10); }" />
                                                            </Listeners>
                                                        </ext:TextField>

                                                        <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : ZONE_NAME %>'
                                                            Layout="HBoxLayout" LabelWidth="80" LabelAlign="Right">
                                                            <Items>
                                                                <ext:ComboBox ID="cmbZoneAdd" runat="server" Width="100"
                                                                    DisplayField="Name" ValueField="ZoneID" TabIndex="2"
                                                                    FieldLabel="" EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                                                    PageSize="25" MinChars="0" SelectOnFocus="true" AllowBlank="false"
                                                                    TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                                                    ForceSelection="true" AllowOnlyWhitespace="false">
                                                                    <ListConfig LoadingText="Searching..." ID="ListComboPhysicalZone">
                                                                        <ItemTpl runat="server">
                                                                            <Html>
                                                                                <div class="search-item">
							                                               {Code} : {Name} 
						                                                </div>
                                                                            </Html>
                                                                        </ItemTpl>
                                                                    </ListConfig>
                                                                    <Store>
                                                                        <ext:Store ID="StorePhysicalZoneAdd" runat="server" AutoLoad="false">
                                                                            <Proxy>
                                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ZoneCombo">
                                                                                    <ActionMethods Read="GET" />
                                                                                    <Reader>
                                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                    </Reader>
                                                                                </ext:AjaxProxy>
                                                                            </Proxy>
                                                                            <Model>
                                                                                <ext:Model ID="Model1" runat="server">
                                                                                    <Fields>

                                                                                        <ext:ModelField Name="Code" />
                                                                                        <ext:ModelField Name="Name" />
                                                                                        <ext:ModelField Name="ShortName" />
                                                                                        <ext:ModelField Name="SiteName" />
                                                                                        <ext:ModelField Name="Code" />
                                                                                        <ext:ModelField Name="WarehouseID" />
                                                                                        <ext:ModelField Name="WarehouseName" />
                                                                                        <ext:ModelField Name="WarehouseTypeID" />
                                                                                        <ext:ModelField Name="ZoneID" />
                                                                                        <ext:ModelField Name="ZoneTypeCode" />
                                                                                        <ext:ModelField Name="ZoneTypeID" />
                                                                                        <ext:ModelField Name="ZoneTypeName" />
                                                                                    </Fields>
                                                                                </ext:Model>
                                                                            </Model>

                                                                        </ext:Store>
                                                                    </Store>
                                                                    <Listeners>
                                                                        <Select Fn="zone_select" />
                                                                    </Listeners>
                                                                </ext:ComboBox>

                                                            </Items>
                                                        </ext:FieldContainer>
                                                    </Items>
                                                </ext:Container>




                                                <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                                                    <Items>
                                                       
                                                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ACTIVE %>"
                                                                    Layout="HBoxLayout" LabelWidth="40" LabelAlign="Left">
                                                                    <Items>
                                                                        <ext:Checkbox ID="chkIsActiveAdd" runat="server" Checked="true" LabelAlign="Left">
                                                                        </ext:Checkbox>

                                                                        <ext:Button runat="server"
                                                                            ID="btnAddItem"
                                                                            Icon="Add"
                                                                            Text="<%$ Resource : SAVE %>"
                                                                            TabIndex="13"
                                                                            MarginSpec="0 0 0 10">
                                                                            <DirectEvents>
                                                                                <Click OnEvent="btnAddEqui_Click">
                                                                                    <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                                                </Click>
                                                                            </DirectEvents>
                                                                            <Listeners>
                                                                            </Listeners>
                                                                        </ext:Button>


                                                                        <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : AVAILABLE %>"
                                                                            Layout="HBoxLayout" LabelWidth="60" LabelAlign="Right" Flex="1" Hidden="true">
                                                                            <Items>
                                                                                <ext:Checkbox ID="chkAvaliableAdd" runat="server" Checked="true">
                                                                                </ext:Checkbox>
                                                                            </Items>
                                                                        </ext:FieldContainer>
                                                                 



                                                            </Items>
                                                        </ext:FieldContainer>


                                                        <ext:TextField runat="server" Width="120" ID="txtWarehouse_NameAdd" TabIndex="13" ReadOnly="true" />
                                                    </Items>
                                                </ext:Container>




                                                <%--<ext:ToolbarFill />--%>
                                                <ext:Checkbox ID="ckbIsActive" runat="server" FieldLabel='<%$ Resource : SHOW_ALL %>' LabelWidth="60" Width="100" Name="IsActive" Checked="true">
                                                    <DirectEvents>
                                                        <Change OnEvent="btnSearch_Event" />
                                                    </DirectEvents>
                                                </ext:Checkbox>
                                                <ext:TextField ID="txtSearch" runat="server" EmptyText='<%$ Resource: SEARCH_WORDING %>' Name="txtSearch" LabelWidth="50" Width="120">
                                                    <Listeners>
                                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text='<%$ Resource: SEARCH %>'>

                                                    <DirectEvents>
                                                        <Click OnEvent="btnSearch_Click">
                                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>

                                            </Items>

                                        </ext:FieldContainer>

                                    </Items>
                                </ext:FieldSet>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>

                                <ext:PageProxy DirectFn="App.direct.BindData">
                                </ext:PageProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="EquipID">
                                    <Fields>
                                        <ext:ModelField Name="EquipID" />
                                        <ext:ModelField Name="EquipName" />
                                        <ext:ModelField Name="Serialnumber" />
                                        <ext:ModelField Name="TruckTypeID" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="TruckTypeName" />
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="ZoneName" />
                                        <ext:ModelField Name="Barcode" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="EquipID" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="80" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="EquipName" Text='<%$ Resource : EQUIP_NAME %>' Align="Left" Width="200">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" SelectOnFocus="true" ID="txtEquiptName" MaxLength="20">
                                        <Listeners>
                                        </Listeners>
                                    </ext:TextField>
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column2" runat="server" DataIndex="Serialnumber" Text='<%$ Resource : SERIAL_NUMBER %>' Align="Left" Width="150">
                                <Editor>
                                    <ext:TextField runat="server" AllowBlank="false" SelectOnFocus="true" ID="txtSerialNo" MaxLength="20">
                                        <Listeners>
                                        </Listeners>
                                    </ext:TextField>
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column6" runat="server" DataIndex="TruckTypeID" Text='<%$ Resource : TRUCK_TYPE_CODE %>' Align="Left" Hidden="true" />
                            <ext:Column ID="Column3" runat="server" DataIndex="TruckTypeName" Text='<%$ Resource : TRUCK_TYPE_NAME %>' Align="Left" Flex="1">
                                <Editor>
                                    <ext:ComboBox ID="cmbTruckTypeEdit" runat="server"
                                        DisplayField="TypeName" ValueField="TruckTypeID" TabIndex="2"
                                        FieldLabel="" EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                        PageSize="25" MinChars="0" Flex="1" SelectOnFocus="true" AllowBlank="false"
                                        TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                        ForceSelection="true" AllowOnlyWhitespace="false">
                                        <ListConfig LoadingText="Searching..." ID="ListComboTruckTypeEdit">
                                            <ItemTpl runat="server">
                                                <Html>
                                                    <div class="search-item">
							                            {TypeName}
						                            </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                        <Store>
                                            <ext:Store ID="StoreTruckTypeEdit" runat="server" AutoLoad="false">
                                                <Proxy>
                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckTypeOnly">
                                                        <ActionMethods Read="GET" />
                                                        <Reader>
                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                        </Reader>
                                                    </ext:AjaxProxy>
                                                </Proxy>
                                                <Model>
                                                    <ext:Model ID="Model2" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="TruckTypeID" />
                                                            <ext:ModelField Name="TypeName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>

                                    </ext:ComboBox>
                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column4" runat="server" DataIndex="ZoneID" Text='<%$ Resource : ZONE_CODE %>' Align="Left" Hidden="true" />
                            <ext:Column ID="Column7" runat="server" DataIndex="ZoneName" Text='<%$ Resource : ZONE_NAME %>' Align="Left" Flex="1">
                                <Editor>
                                    <ext:ComboBox ID="cmbZoneEdit" runat="server"
                                        DisplayField="Name" ValueField="ZoneID" TabIndex="2"
                                        FieldLabel="" EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                        PageSize="25" MinChars="0" Flex="1" SelectOnFocus="true" AllowBlank="false"
                                        TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                        ForceSelection="true" AllowOnlyWhitespace="false">
                                        <ListConfig LoadingText="Searching..." ID="ListComboZoneEdit">
                                            <ItemTpl runat="server">
                                                <Html>
                                                    <div class="search-item">
							                            {Code} : {Name}
						                            </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                        <Store>
                                            <ext:Store ID="StoreZoneEdit" runat="server" AutoLoad="false">
                                                <Proxy>
                                                    <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ZoneCombo">
                                                        <ActionMethods Read="GET" />
                                                        <Reader>
                                                            <ext:JsonReader Root="plants" TotalProperty="total" />
                                                        </Reader>
                                                    </ext:AjaxProxy>
                                                </Proxy>
                                                <Model>
                                                    <ext:Model ID="Model3" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="ShortName" />
                                                            <ext:ModelField Name="SiteName" />
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="WarehouseID" />
                                                            <ext:ModelField Name="WarehouseName" />
                                                            <ext:ModelField Name="WarehouseTypeID" />
                                                            <ext:ModelField Name="ZoneID" />
                                                            <ext:ModelField Name="ZoneTypeCode" />
                                                            <ext:ModelField Name="ZoneTypeID" />
                                                            <ext:ModelField Name="ZoneTypeName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>

                                </Editor>
                            </ext:Column>
                            <ext:Column ID="Column5" runat="server" DataIndex="WarehouseName" Text="<%$ Resource : WAREHOUSE_NAME %>" Align="Left" Flex="1" />

                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text="<%$ Resource : ACTIVE %>" runat="server"
                                Align="Center" Width="100" Editable="true" />
                            <ext:CheckColumn ID="colIsAvailable" DataIndex="IsAvailable" Text="<%$ Resource : AVAILABLE %>" runat="server"
                                Align="Center" Width="100" Editable="true" Hidden="true" />

                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing runat="server">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
                                <Edit Fn="edit" />
                            </Listeners>
                        </ext:CellEditing>
                    </Plugins>
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
                    <SelectionModel>
                        <ext:RowSelectionModel Mode="Single">
                            <Listeners>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>


            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
