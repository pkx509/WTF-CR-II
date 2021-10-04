<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCreateLocation.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateLocation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <style>
        .my-grid .x-grid3-row-last {
            border-bottom: none;
        }

        .my-grid {
            border-right: 1px solid #000000;
        }

        .search-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

            .search-item h3 {
                display: block;
                font: inherit;
                font-weight: bold;
                color: #222;
                margin: 0px;
            }

                .search-item h3 span {
                    float: right;
                    font-weight: normal;
                    margin: 0 0 5px 5px;
                    width: 100px;
                    display: block;
                    clear: none;
                }

        p {
            width: 650px;
        }

        .ext-ie .x-form-text {
            position: static !important;
        }

        div#ListCmbWarehouse_Name {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListCmbPhysicalZone_Name {
            border-top-width: 1 !important;
            width: 200px !important;
        }
    </style>
    <ext:XScript runat="server">
        <script>
            var zone_select = function(){
                var combobox = this;

                var v = combobox.getValue();
                var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
                

                #{txtWHShortName}.setValue(recordValue.data.ShortName);
                #{txtWH}.setValue(recordValue.data.ShortName);
            };
        </script>
    </ext:XScript>
</head>

<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel runat="server" ID="grdVisualgrid" Region="West" Width="460" Visible="false">
                    <Items>

                        <ext:GridPanel ID="GridGraphic" runat="server" Region="Center"
                            SortableColumns="false" AutoScroll="true" Cls="my-grid">
                            <Store>
                                <ext:Store
                                    ID="Store1"
                                    runat="server"
                                    IgnoreExtraFields="false">
                                    <Model>
                                        <ext:Model runat="server" />
                                    </Model>
                                </ext:Store>
                            </Store>

                        </ext:GridPanel>

                    </Items>

                </ext:FormPanel>

                <ext:Container Layout="AnchorLayout" runat="server" Region="Center" Width="300">
                    <Items>

                        <ext:FormPanel runat="server" ID="FormPanelDetail"
                            BodyPadding="5" Layout="AnchorLayout" Height="230" Frame="true">

                            <BottomBar>
                                <ext:Toolbar ID="ToolbarHead" runat="server">

                                    <Items>
                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                        <ext:Button ID="btnEditSave" runat="server" Icon="Disk" Text="<%$ Resource: SAVE %>"
                                            Width="65" TabIndex="15">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click">
                                                    <EventMask ShowMask="true" Msg="<%$ Resource: SAVING %>" MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnEditClear" runat="server" Icon="PageWhite" Text="<%$ Resource: CLEAR %>" Width="65" TabIndex="16">
                                            <DirectEvents>
                                                <Click OnEvent="btnClear_Click">
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnEditExit" runat="server" Icon="Cross" Text="<%$ Resource: EXIT %>" Width="60" TabIndex="16">
                                            <Listeners>
                                                <Click Handler="parentAutoLoadControl.close();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>

                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server">
                                            <Items>
                                                <ext:Container runat="server" DefaultAnchor="90%" Padding="5" Width="450">
                                                    <Items>

                                                        <ext:Hidden ID="txtLocation_No" runat="server" />
                                                        <ext:Hidden ID="txtLocation_Format" runat="server" />
                                                        <ext:Hidden ID="txtWHShortName" runat="server" />
                                                        <ext:Hidden ID="txtZoneID" runat="server" />
                                                        <ext:Hidden ID="txtZoneShortName" runat="server" />
                                                        <ext:Hidden ID="isFloorFlag" runat="server" />
                                                        <ext:Hidden ID="isRowFlag" runat="server" />
                                                        <ext:Hidden ID="isColumnFlag" runat="server" />
                                                        <ext:Hidden ID="isLevelFlag" runat="server" />

                                                        <ext:FieldSet runat="server" Title="<%$ Resource: WAREHOUSE_SETTING %>" Flex="1" Height="70" Layout="ColumnLayout" ID="FLayout">
                                                            <Items>

                                                                <ext:Container Layout="AnchorLayout" Flex="1" runat="server">
                                                                    <Items>
                                                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                                                            <Items>

                                                                                <ext:ComboBox ID="cmbWarehouseName"
                                                                                    FieldLabel="<%$ Resource : WAREHOUSE %>"
                                                                                    Editable="false"
                                                                                    runat="server"
                                                                                    DisplayField="Name"
                                                                                    ValueField="WarehouseID"
                                                                                    TriggerAction="All"
                                                                                    SelectOnFocus="true"
                                                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                                                    MinChars="0"
                                                                                    Width="200"
                                                                                    LabelWidth="80"
                                                                                    LabelAlign="Right"
                                                                                    AllowBlank="false"
                                                                                    TabIndex="2">
                                                                                    <ListConfig LoadingText="Searching..." ID="ListWarehouse_Name">
                                                                                        <ItemTpl runat="server">
                                                                                            <Html>
                                                                                                <div class="search-item">
							                                                                        {ShortName} : {Name}
						                                                                        </div>
                                                                                            </Html>
                                                                                        </ItemTpl>
                                                                                    </ListConfig>
                                                                                    <Store>
                                                                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="false">
                                                                                            <Proxy>
                                                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                                                                    <ActionMethods Read="POST" />
                                                                                                    <Reader>
                                                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                                    </Reader>
                                                                                                </ext:AjaxProxy>
                                                                                            </Proxy>
                                                                                            <Model>
                                                                                                <ext:Model ID="Model4" runat="server">
                                                                                                    <Fields>
                                                                                                        <ext:ModelField Name="Code" />
                                                                                                        <ext:ModelField Name="Name" />
                                                                                                        <ext:ModelField Name="ShortName" />
                                                                                                        <ext:ModelField Name="WarehouseID" />
                                                                                                    </Fields>
                                                                                                </ext:Model>
                                                                                            </Model>
                                                                                        </ext:Store>

                                                                                    </Store>
                                                                                    <Listeners>
                                                                                        <Select Handler="#{txtWHShortName}.setValue(this.valueModels[0].data.ShortName);" />
                                                                                    </Listeners>
                                                                                    <DirectEvents>
                                                                                        <Change OnEvent="cmbWarehouseName_Change">
                                                                                        </Change>
                                                                                    </DirectEvents>
                                                                                </ext:ComboBox>

                                                                                <ext:TextField ID="txtLocFormat" runat="server" FieldLabel="Format" ReadOnly="true" TabIndex="3" LabelWidth="80" LabelAlign="Right" InputWidth="87" />

                                                                                <ext:Button runat="server" Text="..." MarginSpec="0 0 0 3" TabIndex="4" ID="btnLocFormat">
                                                                                    <DirectEvents>
                                                                                        <%--   <Click OnEvent="btnLocFormat_Click" />--%>
                                                                                    </DirectEvents>
                                                                                </ext:Button>
                                                                            </Items>
                                                                        </ext:FieldContainer>

                                                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                                                            <Items>

                                                                                <ext:ComboBox ID="cmbZone"
                                                                                    runat="server"
                                                                                    ForceSelection="true"
                                                                                    DisplayField="Name"
                                                                                    ValueField="ZoneID"
                                                                                    LabelAlign="Right"
                                                                                    TriggerAction="All"
                                                                                    FieldLabel="<%$ Resource : ZONE %>"
                                                                                    AllowBlank="false"
                                                                                    Width="200"
                                                                                    LabelWidth="80"
                                                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                                                    TypeAhead="true"
                                                                                    MinChars="0"
                                                                                    Editable="false"
                                                                                    TabIndex="1">
                                                                                    <ListConfig LoadingText="Searching..." ID="ListCmbPhysicalZone_Name">
                                                                                        <ItemTpl runat="server">
                                                                                            <Html>
                                                                                                <div class="search-item">
							                                                                        {ShortName} : {Name}
						                                                                        </div>
                                                                                            </Html>
                                                                                        </ItemTpl>
                                                                                    </ListConfig>
                                                                                    <Listeners>
                                                                                        <Select Handler="#{txtZoneShortName}.setValue(this.valueModels[0].data.ShortName);" />
                                                                                    </Listeners>
                                                                                    <Store>
                                                                                        <ext:Store ID="StoreZone" runat="server" AutoLoad="false">
                                                                                            <Proxy>
                                                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Zone">
                                                                                                    <ActionMethods Read="POST" />
                                                                                                    <Reader>
                                                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                                                    </Reader>
                                                                                                </ext:AjaxProxy>
                                                                                            </Proxy>
                                                                                            <Model>
                                                                                                <ext:Model ID="Model2" runat="server">
                                                                                                    <Fields>
                                                                                                        <ext:ModelField Name="ZoneID" />
                                                                                                        <ext:ModelField Name="Code" />
                                                                                                        <ext:ModelField Name="Name" />
                                                                                                        <ext:ModelField Name="ShortName" />
                                                                                                    </Fields>
                                                                                                </ext:Model>
                                                                                            </Model>
                                                                                        </ext:Store>
                                                                                    </Store>
                                                                                </ext:ComboBox>

                                                                                <ext:ComboBox ID="cmbLocationType" runat="server" FieldLabel="Location Type" LabelWidth="80" Editable="false" LabelAlign="Right"
                                                                                    EmptyText='<%$ Resource : PLEASE_SELECT %>' AllowBlank="false" Width="200">
                                                                                <%--<DirectEvents>
                                                                                        <Select OnEvent="cmbState_Change" />
                                                                                    </DirectEvents>--%>
                                                                                </ext:ComboBox>
                                                                            </Items>
                                                                        </ext:FieldContainer>
                                                                    </Items>
                                                                </ext:Container>
                                                            </Items>
                                                        </ext:FieldSet>
                                                    </Items>
                                                </ext:Container>

                                                <ext:Container runat="server" Layout="ColumnLayout" DefaultAnchor="90%" Padding="5" MarginSpec="-10 0 0 0">
                                                    <Items>
                                                        <ext:FieldSet runat="server" Title="<%$ Resource : LOCATION_SETTING %>" ID="SLayout" Layout="ColumnLayout" Flex="1" DefaultAnchor="90%" Height="100" Width="440">
                                                            <Items>
                                                                <ext:Container runat="server" Layout="AnchorLayout" DefaultAnchor="90%" Padding="1">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtRow" runat="server" FieldLabel="<%$ Resource : START_ROW %>"
                                                                            TabIndex="5" MinValue="1" MaxLength="2" LabelWidth="60" LabelAlign="Right" Width="120" AllowBlank="false" />
                                                                        <ext:NumberField ID="txtnRow" runat="server" FieldLabel="<%$ Resource : ROW_SIZE %>"
                                                                            TabIndex="6" MinValue="1" MaxLength="2" LabelWidth="60" LabelAlign="Right" Width="120" AllowBlank="false" />
                                                                        <%--   <ext:Label ID="lblnRow" runat="server" Text="PalletCapacity" MarginSpec="0 0 0 0"></ext:Label>--%>
                                                                    </Items>
                                                                </ext:Container>
                                                                <ext:Container runat="server" Layout="AnchorLayout" DefaultAnchor="90%" Padding="1">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtStartCol" runat="server" FieldLabel="<%$ Resource : START_COLUMN %>"
                                                                            TabIndex="7" MinValue="1" MaxLength="2" LabelWidth="85" LabelAlign="Right" Width="170" AllowBlank="false" />
                                                                        <ext:NumberField ID="txtCol" runat="server" FieldLabel="<%$ Resource : COLUMN_SIZE %>"
                                                                            TabIndex="8" MinValue="1" MaxLength="2" LabelWidth="85" LabelAlign="Right" Width="170" AllowBlank="false" />
                                                                        <ext:NumberField ID="txtnPalletCap" runat="server" FieldLabel="<%$ Resource : PALLET_CAPACITY %>"
                                                                            TabIndex="6" MinValue="0" MaxLength="3" LabelWidth="85" LabelAlign="Right" Width="170" AllowBlank="false" />
                                                                    </Items>
                                                                </ext:Container>
                                                                <ext:Container runat="server" Layout="AnchorLayout" DefaultAnchor="90%" Padding="1">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtFloor" runat="server" FieldLabel="<%$ Resource : START_LEVEL %>"
                                                                            TabIndex="9" LabelWidth="60" MinValue="1" MaxLength="1" LabelAlign="Right" Width="120" AllowBlank="false" />
                                                                        <ext:NumberField ID="txtnFloor" runat="server" FieldLabel="<%$ Resource : LEVEL_SIZE %>"
                                                                            TabIndex="10" LabelWidth="60" MinValue="1" MaxLength="2" LabelAlign="Right" Width="120" AllowBlank="false" />
                                                                    </Items>
                                                                </ext:Container>
                                                            </Items>
                                                        </ext:FieldSet>

                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>


                                        <ext:Container runat="server" Layout="ColumnLayout" DefaultAnchor="100%" Width="180" Padding="5">

                                            <Items>
                                                <ext:FieldSet runat="server" Title="Dimension Storage" Layout="ColumnLayout" Flex="1" DefaultAnchor="90%" Height="180">
                                                    <Items>
                                                        <ext:Container runat="server" Layout="AnchorLayout" DefaultAnchor="100%" Padding="5" Width="160">
                                                            <Items>
                                                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtWidth" runat="server" MinValue="0" FieldLabel="<%$ Resource : LOCATION_WIDTH %>" TabIndex="11" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                        <ext:Label runat="server" Text="cm" MarginSpec="0 0 0 5" />
                                                                    </Items>
                                                                </ext:Container>

                                                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtLength" runat="server" MinValue="0" FieldLabel="<%$ Resource : LOCATION_LENGTH %>" TabIndex="12" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                        <ext:Label runat="server" Text="cm" MarginSpec="0 0 0 5" />
                                                                    </Items>
                                                                </ext:Container>

                                                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtHeight" runat="server" MinValue="0" FieldLabel="<%$ Resource : LOCATION_HEIGHT %>" TabIndex="13" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                        <ext:Label runat="server" Text="cm" MarginSpec="0 0 0 5" />
                                                                    </Items>
                                                                </ext:Container>

                                                                <%--<ext:NumberField ID="txtCapacity" runat="server" MinValue="0" FieldLabel="<%$Resources:Langauge, Capacity%>" TabIndex="14" LabelWidth="90" LabelAlign="Right" />--%>

                                                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtWeight" runat="server" MinValue="0" FieldLabel="<%$ Resource : LOCATION_WEIGHT %>" TabIndex="15" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                        <ext:Label runat="server" Text="kg" MarginSpec="0 0 0 5" />
                                                                    </Items>
                                                                </ext:Container>

                                                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" DefaultAnchor="90%" MarginSpec="5 0 0 0">
                                                                    <Items>
                                                                        <ext:NumberField ID="txtCapacity" runat="server" MinValue="0" FieldLabel="<%$ Resource : CAPACITY %>" TabIndex="15" LabelWidth="60" LabelAlign="Right" Width="120" />
                                                                        <%--<ext:Label runat="server" Text="<%$Resources:Langauge, kg%>" MarginSpec="0 0 0 5" />--%>
                                                                    </Items>
                                                                </ext:Container>
                                                            </Items>
                                                        </ext:Container>

                                                    </Items>
                                                </ext:FieldSet>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="ColumnLayout" Width="540" MarginSpec="-10 0 0 470">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Disabled="true" Icon="Add" Text="<%$ Resource : GENERATE_RECORD_LOCATION %>" Border="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click">
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                            <Listeners>
                                <ValidityChange Handler="#{btnAdd}.setDisabled(!valid); " />
                            </Listeners>

                        </ext:FormPanel>

                        <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center"
                            Height="277" SortableColumns="false" Flex="1">
                            <Store>
                                <ext:Store ID="StoreOfDataList" runat="server">
                                    <Model>
                                        <ext:Model ID="Model" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="RowNumbererColumn1" />
                                                <ext:ModelField Name="Code" />
                                                <ext:ModelField Name="WarehouseShortName" />
                                                <ext:ModelField Name="ZoneShortName" />
                                                <ext:ModelField Name="ZoneCode" />
                                                <ext:ModelField Name="RowNo" />
                                                <ext:ModelField Name="ColumnNo" />
                                                <ext:ModelField Name="LevelNo" />
                                                <ext:ModelField Name="Location_Capacity" />
                                                <ext:ModelField Name="LocationType" />
                                                <ext:ModelField Name="Width" />
                                                <ext:ModelField Name="Length" />
                                                <ext:ModelField Name="Height" />
                                                <ext:ModelField Name="Weight" />
                                                <ext:ModelField Name="SizeCapacity" />
                                                <ext:ModelField Name="PalletCapacity" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Sorters>
                                        <ext:DataSorter Property="Code" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModelDriver" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />

                                    <ext:Column ID="Column1" runat="server" DataIndex="Code" Text="<%$ Resource: LOCATION_NO %>" Align="Center" Locked="true" />
                                    <ext:Column ID="Column4" runat="server" DataIndex="WarehouseShortName" Text="<%$ Resource: WAREHOUSE_SHORT_NAME %>" Align="Center" Locked="true" />
                                    <ext:Column ID="Column2" runat="server" DataIndex="ZoneShortName" Text="<%$ Resource: ZONE_SHORT_NAME %>" Align="Center" Locked="true" />
                                    <ext:Column ID="Column5" runat="server" DataIndex="RowNo" Text="<%$ Resource: LOCATION_ROW %>" Align="Center" />
                                    <ext:Column ID="Column6" runat="server" DataIndex="ColumnNo" Text="<%$ Resource: LOCATION_COLUMN %>" Align="Center" />
                                    <ext:Column ID="Column7" runat="server" DataIndex="LevelNo" Text="<%$ Resource: LOCATION_LEVEL %>" Align="Center" />
                                    <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="PalletCapacity" Text="<%$ Resource: LOCATION_PALLET_CAPACITY %>" Align="Center" Format="0" />
                                    <ext:NumberColumn ID="WColumn" runat="server" DataIndex="Width" Text="<%$ Resource: LOCATION_WIDTH %>" Align="Center" Format="00.0" />
                                    <ext:NumberColumn ID="LColumn" runat="server" DataIndex="Length" Text="<%$ Resource: LOCATION_LENGTH %>" Align="Center" Format="00.0" />
                                    <ext:NumberColumn ID="HColumn" runat="server" DataIndex="Height" Text="<%$ Resource: LOCATION_HEIGHT %>" Align="Center" Format="00.0" />
                                    <ext:Column ID="Column8" runat="server" DataIndex="SizeCapacity" Text="<%$ Resource: SIZE_CAPACITY %>" Align="Center" />
                                    <ext:NumberColumn ID="WeColumn" runat="server" DataIndex="Weight" Text="<%$ Resource: LOCATION_WEIGHT %>" Align="Center" Format="00.0" />

                                </Columns>
                            </ColumnModel>

                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                            </View>
                            <BottomBar>
                                <ext:Toolbar ID="ToolbarGrid" runat="server">

                                    <Items>
                                        <ext:ToolbarFill ID="TbarFill" runat="server" />
                                        <ext:Button ID="btnSave" runat="server"
                                            Icon="Disk" Text="<%$ Resource : SAVE %>" Width="65" Disabled="true" TabIndex="15">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click"
                                                    Before="#{btnSave}.setDisabled(true);"
                                                    Complete="#{btnSave}.setDisabled(false);"
                                                    Buffer="350">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <%--<ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="16">
                                            <Listeners>
                                                <Click Handler="#{FormPanelDetail}.reset();" />
                                            </Listeners>
                                        </ext:Button>--%>
                                        <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="8">
                                            <DirectEvents>
                                                <Click OnEvent="btnClear_Click">
                                                    <EventMask ShowMask="true" MinDelay="300" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="16">
                                            <DirectEvents>
                                                <Click OnEvent="btnExit_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>

                        </ext:GridPanel>
                    </Items>
                </ext:Container>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
