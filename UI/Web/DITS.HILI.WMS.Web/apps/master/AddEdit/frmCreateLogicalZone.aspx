<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateLogicalZone.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateLogicalZone" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

    <style>
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

        div#ListComboProductOwner {
            border-top-width: 1 !important;
            width: 225px !important;
        }

        div#ListComboSupplier {
            border-top-width: 1 !important;
            width: 225px !important;
        }

        div#ListComboProductCode {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListComboProductGroup1 {
            border-top-width: 1 !important;
            width: 180px !important;
        }

        div#ListComboProductGroup2 {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListComboProductGroup3 {
            border-top-width: 1 !important;
            width: 210px !important;
        }

        div#ListComboProductStatus {
            border-top-width: 1 !important;
            width: 200px !important;
        }

        div#ListComboProductSubStatus {
            border-top-width: 1 !important;
            width: 200px !important;
        }
    </style>

    <ext:XScript runat="server">
        <script>
            function startEdit(record) {
                var grid = #{gridConfig};
                var freeRecord = grid.store.data.length-1;
                grid.editingPlugin.startEdit(-1,2);

            }
            var edit = function (editor, e) {
                App.direct.SaveConfig(e.record.data);
            };

            var beforeEditCheck = function(editor, e, eOpts){
                // App.direct.LoadCombo();
                var grid = #{gridConfig};
                var sm = grid.getSelectionModel();

                //var combobox = #{cmbConfig};
                //console.log(combobox);

                //var v = combobox.getValue();  
                var id = sm.getSelection()[0].data.LogicalConfigID;    
                var code = sm.getSelection()[0].data.ConfigID;   
                
              
                if(id != '00000000-0000-0000-0000-000000000000'){
                    App.direct.LoadComboValue(id,code);
                }
                
            };

            var cancelEditCheck = function(editor, e, eOpts){
                // App.direct.CancelConfig(e.record.data);
            };

            var cmbConfig_Change = function(){  
                var grid = #{gridConfig};
                var sm = grid.getSelectionModel();


                var combobox = this;
                var v = combobox.getValue();  
                var id = sm.getSelection()[0].data.LogicalConfigID;
         
                //var recordValue = combobox.findRecord(combobox.valueField || combobox.displayField, v);
       
                App.direct.LoadComboValue(id,v);
            };

            var validateSave = function () {
                var plugin = this.editingPlugin; 
                App.direct.ValidateSave(this.getValues(false, false, false, true),plugin.context.record.data);
            };
        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />

        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel1" Region="North" BodyPadding="5" Layout="FitLayout">
                    <Items>
                        <ext:Container Layout="AnchorLayout" runat="server">
                            <Items>
                                <ext:Container Layout="ColumnLayout" runat="server">
                                    <Items>
                                        <ext:Container Layout="AnchorLayout" runat="server" Hidden="true">
                                            <Items>
                                                <ext:Container Layout="ColumnLayout" runat="server">
                                                    <Items>
                                                        <ext:Hidden ID="hddVariable" runat="server" />
                                                        <ext:Hidden ID="txtLogicalZone_Code_Key" runat="server" />
                                                        <ext:TextField ID="txtLogicalZone_Code" FieldLabel="<%$ Resource : LOGZONECODE %>" ReadOnly="true"
                                                            Text="new" TabIndex="1" LabelAlign="Right" LabelWidth="115" Width="300" runat="server" />
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>

                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:TextField ID="txtLogicalZone_Name"
                                                    FieldLabel="<%$ Resource : LOGZONENAME %>"
                                                    AllowBlank="false"
                                                    AllowOnlyWhitespace="false"
                                                    TabIndex="1"
                                                    LabelAlign="Right"
                                                    LabelWidth="115"
                                                    Width="300"
                                                    runat="server" />
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="5 0 0 0">
                                    <Items>
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:Hidden ID="hddwharehouse" runat="server"></ext:Hidden>
                                                <ext:ComboBox ID="cmbWarehouseName"
                                                    runat="server"
                                                    AllowBlank="false"
                                                    DisplayField="Name"
                                                    ValueField="WarehouseID"
                                                    TriggerAction="Query"
                                                    SelectOnFocus="true"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    TypeAhead="true"
                                                    PageSize="25"
                                                    MinChars="0"
                                                    LabelWidth="115"
                                                    Width="300"
                                                    FieldLabel="<%$ Resource : WAREHOUSE %>"
                                                    LabelAlign="Right"
                                                    TabIndex="2"
                                                    Editable="true"
                                                    MatchFieldWidth="false">
                                                    <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name" Width="250" Height="300">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                                       {Code} : {Name}
						                                        </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                                    <ActionMethods Read="GET" />
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
                                                                        <ext:ModelField Name="WarehouseID" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <DirectEvents>
                                                        <Change OnEvent="cmbWarehouseName_Change" />
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:Hidden ID="hddphysicalzone" runat="server"></ext:Hidden>
                                                <ext:ComboBox ID="cmbPhysicalZone" runat="server"
                                                    AllowBlank="false"
                                                    AllowOnlyWhitespace="false"
                                                    Editable="true"
                                                    DisplayField="Name"
                                                    ValueField="ZoneID"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    FieldLabel="<%$ Resource : ZONE %>"
                                                    TriggerAction="Query"
                                                    TypeAhead="true"
                                                    PageSize="10"
                                                    MinChars="0"
                                                    LabelAlign="Right"
                                                    MatchFieldWidth="false"
                                                    LabelWidth="115"
                                                    Width="300"
                                                    SelectOnFocus="true">
                                                    <ListConfig LoadingText="Searching..." ID="ListcmbPhysicalZone" Width="250" Height="300">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                                       {Name}
						                                        </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StorePhysicalZone" runat="server" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=LocationByZoneWarehouse">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model1" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="ZoneID" />
                                                                        <ext:ModelField Name="Name" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <%--                        <Parameters>
                                                                <ext:StoreParameter Name="WhKey" Value="#{cmbWarehouseName}.getValue()" Mode="Raw" />
                                                            </Parameters>--%>
                                                        </ext:Store>
                                                    </Store>
                                                    <DirectEvents>
                                                        <Change OnEvent="cmbPhysicalZone_Change" />
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="0 0 0 40">
                                            <Items>
                                                <ext:Checkbox runat="server" ID="chkIsPallet" MarginSpec="0 0 0 0" FieldLabel="Pallet Zone" LabelAlign="Right" Checked="true" ReadOnly="true">
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="5 0 0 0">
                                    <Items>
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>

                                                <ext:ComboBox ID="cmbLocationNo" runat="server"
                                                    QueryMode="Remote"
                                                    AutoShow="false"
                                                    AllowOnlyWhitespace="false"
                                                    AllowBlank="false"
                                                    Editable="true"
                                                    DisplayField="Code"
                                                    ValueField="LocationID"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    FieldLabel="<%$ Resource : LOCATION_FROM %>"
                                                    TypeAhead="true"
                                                    MinChars="0"
                                                    TriggerAction="Query"
                                                    PageSize="25"
                                                    LabelAlign="Right"
                                                    MatchFieldWidth="false"
                                                    LabelWidth="115"
                                                    Width="300"
                                                    SelectOnFocus="true">
                                                    <ListConfig LoadingText="Searching..." ID="ListComboLocation" Width="250" Height="300">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                               {Code} 
						                                </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreLocation" runat="server" PageSize="25" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=LocationByZoneWarehouse">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model5" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="LocationID" />
                                                                        <ext:ModelField Name="Code" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                            <%--<Sorters>
                                                                <ext:DataSorter Property="Code" Direction="ASC" />
                                                            </Sorters>--%>
                                                            <%--<Parameters>
                                                                <ext:StoreParameter Name="warehouseId" Value="#{cmbWarehouseName}.getValue()" Mode="Raw" />
                                                                <ext:StoreParameter Name="zoneId" Value="#{cmbPhysicalZone}.getValue()" Mode="Raw" />
                                                            </Parameters>--%>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:ComboBox ID="cmbLocationNoTo" runat="server"
                                                    Editable="true"
                                                    DisplayField="Code"
                                                    ValueField="LocationID"
                                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                                    FieldLabel="<%$ Resource : LOCATION_TO %>"
                                                    TriggerAction="Query"
                                                    TypeAhead="true"
                                                    PageSize="25"
                                                    MinChars="0"
                                                    LabelAlign="Right"
                                                    MatchFieldWidth="false"
                                                    LabelWidth="115"
                                                    AllowBlank="false"
                                                    AllowOnlyWhitespace="false"
                                                    SelectOnFocus="true"
                                                    Width="300">
                                                    <ListConfig LoadingText="Searching..." ID="ListComboLocationFrom" Width="250" Height="300">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
							                               {Code} 
						                                </div>
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store ID="StoreLocationTo" runat="server" PageSize="25" AutoLoad="false">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=LocationByZoneWarehouse">
                                                                    <ActionMethods Read="GET" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model ID="Model2" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="LocationID" />
                                                                        <ext:ModelField Name="Code" />
                                                                    </Fields>
                                                                </ext:Model>

                                                            </Model>
                                                            <Sorters>
                                                                <ext:DataSorter Property="Code" Direction="ASC" />
                                                            </Sorters>
                                                            <%--                            <Parameters>
                                                                <ext:StoreParameter Name="warehouseId" Value="#{cmbWarehouseName}.getValue()" Mode="Raw" />
                                                                <ext:StoreParameter Name="zoneId" Value="#{cmbPhysicalZone}.getValue()" Mode="Raw" />
                                                            </Parameters>--%>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>

                                        </ext:Container>
                                        <ext:Container Layout="ColumnLayout" runat="server" MarginSpec="0 0 0 80">
                                            <Items>
                                                <ext:Button ID="btnAdd" runat="server" Icon="Add" Text="<%$ Resource : ADD_NEW %>" Width="80" TabIndex="16">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </Items>

        <%--            <Listeners>
                        <ValidityChange Handler="#{btnAdd}.setDisabled(!valid); " />
                    </Listeners>--%>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel2" runat="server" Region="West" Split="true" MarginSpec="0 0 0 0" BodyPadding="2" Title="LogicalZone Config" Icon="Package"
                    AutoScroll="True" Width="300" Layout="FitLayout">
                    <FieldDefaults LabelWidth="150" LabelAlign="Right" InputWidth="110" />
                    <Items>
                        <ext:GridPanel ID="gridConfig" runat="server" Margins="0 0 0 0" Region="Center" Frame="true" TabIndex="5" SortableColumns="false">
                            <Store>
                                <ext:Store ID="StoreConfig" runat="server" PageSize="20">

                                    <Model>
                                        <ext:Model ID="Model3" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="LogicalConfigID" />
                                                <ext:ModelField Name="LogicalZoneCode" />
                                                <ext:ModelField Name="ConfigID" />
                                                <ext:ModelField Name="ConfigCode" />
                                                <ext:ModelField Name="ConfigName" />
                                                <ext:ModelField Name="ConfigVariable" />
                                                <ext:ModelField Name="ConfigValue" />
                                                <ext:ModelField Name="ConfigValueId" />
                                                <ext:ModelField Name="ConfigSeq" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:CommandColumn runat="server" ID="CommandColumn3" Sortable="false" Align="Center" Width="25">
                                        <Commands>
                                            <ext:GridCommand Icon="Delete" ToolTip-Text="<%$ Resource : DELETE %>" CommandName="Delete" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="CommandConfigClick">
                                                <ExtraParams>
                                                    <ext:Parameter Name="oDataKeyId" Value="record.data.LogicalConfigID" Mode="Raw" />
                                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                                </ExtraParams>
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:Column runat="server" ID="Column5" Text="<%$ Resource : NUMBER %>" Align="Center" DataIndex="ConfigSeq" Width="40" />
                                    <ext:Column runat="server" ID="Column6" Text="<%$ Resource : CONFIG_NAME %>" DataIndex="ConfigName" Width="110">
                                        <Editor>
                                            <%--                          <ext:ComboBox Editable="false" ID="cmbConfig" runat="server"
                                                DisplayField="ConfigName" ValueField="ConfigCode" InputWidth="110"
                                                MinChars="0" Width="110" AllowBlank="false" MinWidth="110"
                                                TypeAhead="false" QueryMode="Remote" AutoShow="false">--%>
                                            <ext:ComboBox ID="cmbConfig" runat="server"
                                                Editable="false"
                                                DisplayField="ConfigName" ValueField="ConfigID" InputWidth="110"
                                                MinChars="0" Width="110" AllowBlank="false" MinWidth="110"
                                                TypeAhead="false" QueryMode="Remote" AutoShow="false">
                                                <ListConfig LoadingText="Searching..." ID="ListComboConfig">
                                                    <ItemTpl runat="server">
                                                        <Html>
                                                            <div class="search-item">
							                               {ConfigName} 
						                                </div>
                                                        </Html>
                                                    </ItemTpl>
                                                </ListConfig>
                                                <Store>
                                                    <ext:Store ID="StoreConConfig" runat="server" AutoLoad="false">
                                                        <Proxy>
                                                            <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Conditionconfig">
                                                                <ActionMethods Read="GET" />
                                                                <Reader>
                                                                    <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                </Reader>
                                                            </ext:AjaxProxy>
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model ID="Model6" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="ConfigName" />
                                                                    <ext:ModelField Name="ConfigVariable" />
                                                                    <ext:ModelField Name="ConfigID" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>

                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Fn="cmbConfig_Change" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column runat="server" ID="Column7" Text="<%$ Resource : CONFIG_VALUE %>" DataIndex="ConfigValue" Width="110">
                                        <Editor>
                                            <ext:ComboBox Editable="false" ID="cmbValue" runat="server"
                                                DisplayField="Key" ValueField="Value" InputWidth="110"
                                                MinChars="0" Width="110" AllowBlank="false" MaxWidth="110"
                                                TypeAhead="false" QueryMode="Remote" AutoShow="false">
                                                <ListConfig LoadingText="Searching..." ID="ListComboKey">
                                                    <ItemTpl runat="server">
                                                        <Html>
                                                            <div class="search-item">
							                               {Key} 
						                                </div>
                                                        </Html>
                                                    </ItemTpl>
                                                </ListConfig>
                                                <Store>
                                                    <ext:Store ID="StoreValue" runat="server" AutoLoad="false">

                                                        <Model>
                                                            <ext:Model ID="Model7" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="Key" />
                                                                    <ext:ModelField Name="Value" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                        </Editor>
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" ErrorSummary="false">
                                    <Listeners>
                                        <BeforeEdit Fn="beforeEditCheck"></BeforeEdit>
                                        <Edit Fn="edit" />
                                        <CancelEdit Fn="cancelEditCheck"></CancelEdit>
                                    </Listeners>
                                </ext:RowEditing>
                            </Plugins>
                            <View>
                                <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" />
                            </View>
                        </ext:GridPanel>
                    </Items>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbar1">
                            <Items>

                                <ext:Button ID="btnAddItem" runat="server"
                                    Icon="Add" Text="<%$ Resource : ADD_NEW %>" Width="80" TabIndex="6">
                                    <DirectEvents>
                                        <Click OnEvent="btnAddItem_Click">
                                            <EventMask ShowMask="true" Msg="Add Item ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:FormPanel>
                <ext:GridPanel ID="GridLogicalDetail" runat="server" Margins="3 3 0 3" Region="Center" Frame="true" TabIndex="5" SortableColumns="false">
                    <Store>
                        <ext:Store ID="StoreLogicalDetail" runat="server" PageSize="20">

                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="Product_Code">
                                    <Fields>
                                        <ext:ModelField Name="LogicalZoneDetailID" />
                                        <ext:ModelField Name="Seq" />
                                        <ext:ModelField Name="ZoneName" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="LocationCapacity" />
                                        <ext:ModelField Name="LocationId" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LogicalZoneDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="CommandColumn1" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="ArrowUp" ToolTip-Text="Up" CommandName="Up" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LogicalZoneDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="CommandColumn2" Sortable="false" Align="Center" Width="25">
                                <Commands>
                                    <ext:GridCommand Icon="ArrowDown" ToolTip-Text="Down" CommandName="Down" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LogicalZoneDetailID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:Column runat="server" ID="Column3" Text="Sequent" Align="Center" DataIndex="Seq" Width="80" />
                            <ext:Column runat="server" ID="Column2" Text="<%$ Resource : LOCATION_NO %>" DataIndex="LocationNo" Flex="1" />
                            <ext:Column runat="server" ID="Column1" Text="<%$ Resource : ZONE %>" DataIndex="ZoneName" Flex="1" />
                            <ext:Column runat="server" ID="Column4" Text="<%$ Resource : LOCATION_CAPACITY %>" DataIndex="LocationCapacity" Align="Right" Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />

                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" TabIndex="6">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter  Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{GridLogicalDetail}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="8">
                                    <DirectEvents>
                                        <Click OnEvent="btnClear_Click">
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="9">
                                    <DirectEvents>
                                        <Click OnEvent="btnExit_Click"></Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>

                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" />
                    </View>
                </ext:GridPanel>
            </Items>

        </ext:Viewport>

    </form>
</body>
</html>
