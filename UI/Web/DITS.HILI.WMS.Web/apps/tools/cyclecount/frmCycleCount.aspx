<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCycleCount.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.cyclecount.frmCycleCount" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

     <script type="text/javascript">
         //Ext.Ajax.timeout = 180000; // 1 sec
         //Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.Job_CycleCount_Detail_Status == 22 ||
                record.data.Job_CycleCount_Detail_Status == 77 || 
                record.data.Job_CycleCount_Detail_Status == 99) {
                toolbar.items.getAt(0).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.Job_Adjust_Status == 77) {
                toolbar.items.getAt(1).setDisabled(true);
            }
            //console.log(record.data.Job_Adjust_Status);
        };

     </script>
    <ext:XScript runat="server">
        <script>
            
            var removeItem = function () {
                return;
                Ext.MessageBox.confirm('Delete', 'Are you sure you want to delete?', function(btn){
                    if(btn === 'yes'){
                        var grid = #{grdDataList},
                        sm = grid.getSelectionModel();

                        grid.store.remove(sm.getSelection());
                        if (grid.store.getCount() > 0) {
                            sm.select(0);
                        }
                        grid.editingPlugin.cancelEdit();

                        #{btnSave}.setDisabled(false);

                    }
                    else{
                        //some code
                    }
                });

               
            };

            var customer_select = function(){
                var combobox = this;

                App.direct.LoadProxy();
            };

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

            
            var checkbox_default_change = function (datagrid, index, records, v) {

                var grid = #{GridGetWindows};
             
                grid.store.each(function (record) {
                    if (record.data.Code == records.data.Code) {
                        record.set("IsDefault", true);
                    } else {
                        record.set("IsDefault", false);
                    }
                });

                grid.store.commitChanges();
                //grid.getView().refresh(false);

            };

            
            var before_select_row = function(grid, record, index, eOpts) {
                if(record.data.IsDefault == true)
                    return false;
                else
                    return true;
            }

            var before_default_change = function(grid, index,record) {
                var grid = #{GridGetWindows};

                for(i=0;i<grid.getSelectionModel().selected.items.length;i++)
                {
                    if(grid.getSelectionModel().selected.items[i].index == index)
                        return true;
                }

                return false;
            }

        </script>
    </ext:XScript>

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

        input#cmbCustomer-inputEl {
            background-color: #FFFFCC;
            background-image: none;
        }

        div#ListCmbWarehouse_Name {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListCmbProductGroup_L3 {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListComboSupplier {
            border-top-width: 1 !important;
            width: 300px !important;
        }

        div#ListcmbPhysicalZone {
            border-top-width: 1 !important;
            width: 250px !important;
        }

        div#ListcmbLogicalZone {
            border-top-width: 1 !important;
            width: 250px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Frame="true"
                    Region="North" Layout="FitLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Padding="3">
                            <Items>
                                <ext:FieldSet runat="server" Layout="ColumnLayout" Title="InformationCreateJob" Padding="3" Height="60" DefaultAnchor="90%">
                                    <Items>
                                        <ext:FieldContainer runat="server" FieldLabel="CreateJobNo" Layout="HBoxLayout" LabelWidth="130" Width="280">
                                            <Items>
                                                <ext:Hidden ID="txtCustomerCode" runat="server" />
                                                <ext:TextField ID="txtJobNo" runat="server" TabIndex="1" Text="new"
                                                    MaxLength="50" EnforceMaxLength="true" Flex="1" ReadOnly="true" AllowBlank="false" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:DateField runat="server" ID="dateCreateJob" FieldLabel="DateCreateJob"
                                            LabelWidth="100" Width="230" Format="dd/MM/yyyy" AllowBlank="false" />
                                        <ext:TextField ID="txtRemark" runat="server" FieldLabel="Remark"
                                            MaxLength="200" EnforceMaxLength="true" LabelWidth="80" Width="300" />

                                    </Items>
                                </ext:FieldSet>
                            </Items>
                        </ext:Container>
                    </Items>

                </ext:FormPanel>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center"
                    Frame="true" Flex="1" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAddItem" Icon="Add" Text="Get Product" Hidden="false">
                                    <DirectEvents>
                                        <Click OnEvent="btnAddItem_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">

                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CyclecountDetailID" />
                                        <ext:ModelField Name="CycleCountCode" />
                                        <ext:ModelField Name="Stock_ID" />
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="CountingStockQty" />
                                        <ext:ModelField Name="DiffQty" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="WarehouseID" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="DetailStatus" />
                                        <ext:ModelField Name="IsActive" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <ServerProxy>
                                <ext:PageProxy>
                                    <RequestConfig>
                                        <EventMask ShowMask="true" CustomTarget="App.grdDataList.getView().el" />
                                    </RequestConfig>
                                </ext:PageProxy>
                            </ServerProxy>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20" Locked="true">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete Data" CommandName="Delete" />
                                </Commands>
                                <Listeners>
                                    <Command Handler="Ext.MessageBox.confirm('Delete', 'Are you sure you want to delete?', function(btn){
                                                           if(btn === 'yes'){
                                                               #{grdDataList}.store.removeAt(recordIndex);
                                                               #{btnSave}.setDisabled(true); 
                                                               #{btnPrint}.setDisabled(true);
                                                           }
                                                           else{
                                                              //some code
                                                           }
                                                         });"></Command>
                                </Listeners>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:Column ID="colCyclecountDetailID" runat="server" DataIndex="CyclecountDetailID" Hidden="true" />
                            <ext:Column ID="colCycleCountCode" runat="server" DataIndex="CycleCountCode" Hidden="true" />
                            <ext:Column ID="colProductID" runat="server" DataIndex="ProductID" Hidden="true" />
                            <ext:Column ID="ColWarehouseID" runat="server" DataIndex="WarehouseID" Hidden="true" />
                            <ext:Column ID="ColLocationID" runat="server" DataIndex="LocationID" Hidden="true" />
                            <ext:Column ID="colDetailStatuss" runat="server" DataIndex="DetailStatus" Hidden="true" />

                            <ext:RowNumbererColumn ID="colRowNo" runat="server" Text="No" Width="40" Align="Center" Locked="true" />
                            <ext:Column ID="Column3" runat="server" DataIndex="ProductCode" Text="ProductCode" Width="180" Align="Center" Locked="true" />
                            <ext:Column ID="Column4" runat="server" DataIndex="ProductName" Text="ProductName" Width="200" Align="Left" Locked="true" />
                            <ext:Column ID="Column6" runat="server" DataIndex="Lot" Text="Product Lot" Width="150" Align="Center" />
                            <ext:Column ID="Column9" runat="server" DataIndex="PalletCode" Text="Pallet Code" Width="200" Align="Center" />
                            <ext:NumberColumn ID="colStockBalance" runat="server" DataIndex="RemainQTY" Text="Quantity" Width="100" Align="Center" Format="#,###.##" />
                            <ext:NumberColumn ID="colPackageQty" runat="server" DataIndex="ConversionQty" Text="PackageQuantity" Width="100" Align="Center" Format="#,###.##" />
                            <ext:ComponentColumn ID="colCountingQty" runat="server" DataIndex="CountingStockQty" Text="CountingQty" Width="100" Align="Center"
                                Editor="true" Format="#,###">
                                <Component>
                                    <ext:NumberField runat="server" ID="txtConfirmQty" DataIndex="CountingStockQty"
                                        Text="0" AllowBlank="false" AllowDecimals="true" SelectOnFocus="true">
                                    </ext:NumberField>
                                </Component>
                            </ext:ComponentColumn>
                            <ext:NumberColumn ID="colDiff" runat="server" DataIndex="DiffQty" Text="Diff Qty" Width="100" Align="Center" Format="#,###" />
                            <ext:Column ID="Column7" runat="server" DataIndex="ProductUnitName" Text="Unit" Width="100" Align="Center" />
                            <ext:Column ID="Column8" runat="server" DataIndex="LocationNo" Text="LocationNo" Width="150" Align="Center" />
                            <ext:Column ID="Column10" runat="server" DataIndex="WarehouseName" Text="Warehouse" Width="150" Align="Center" />

                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:CellEditing ClicksToEdit="0">
                        </ext:CellEditing>
                    </Plugins>
                    <KeyMap runat="server">
                        <Binding>
                            <ext:KeyBinding Handler="removeItem" DefaultEventAction="StopEvent">
                                <Keys>
                                    <ext:Key Code="DELETE" />
                                </Keys>
                            </ext:KeyBinding>
                        </Binding>
                    </KeyMap>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="false" DisplayMsg="DisplayingFromTo"
                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                            FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                            BeforePageText="<center>Page</center>">

                            <Items>

                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />

                                <%--<ext:Button ID="btnPrint" runat="server" Icon="Printer" Text="Print" Width="60" Disabled="false" TabIndex="14">
                                    <DirectEvents>
                                        <Click OnEvent="btnPrint_Click"
                                            Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Print ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>--%>

                                <ext:ToolbarSpacer Width="30" />
                                <ext:ToolbarSeparator runat="server" />
                                <ext:ToolbarSpacer Width="30" />
                                <ext:Button ID="btnPrintCycleCount" runat="server" TabIndex="15" Icon="Report" Text='<%$ Resource : PRINTCYCLECOUTLIST %>' >
                                    <DirectEvents>
                                        <Click OnEvent="btnCycleCountlist_Click" Buffer="350">
                                            <EventMask ShowMask="true" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnApprove" runat="server"
                                    Icon="Accept" Text="Approve" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnApprove_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />

                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="60" Disabled="true" TabIndex="15">
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
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnClear_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="18">
                                    <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>

                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>


        <ext:Window ID="WindowDataDetail" runat="server" Icon="FeedGo" Title=""
            Width="900" Height="450"
            Layout="BorderLayout" Resizable="false" Modal="true" Hidden="false">
            <Items>
                <ext:GridPanel ID="GridGetWindows" runat="server" Region="Center" SortableColumns="false" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar runat="server" Padding="5">
                            <Items>
                                <ext:ToolbarFill />
                                <%--    <ext:ComboBox ID="cmbWarehouseName" runat="server" FieldLabel="Warehouse" EmptyText="- Select -"
                                    DisplayField="Warehouse_Code" ValueField="Warehouse_Code" Width="250" LabelWidth="60"
                                    PageSize="20"
                                    MinChars="0"
                                    TypeAhead="false"
                                    TriggerAction="Query"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    TabIndex="3">
                                    <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                {Warehouse_Code} : {Warehouse_Name}
						                </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Warehouse_Code" />
                                                        <ext:ModelField Name="Warehouse_Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>

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
                                    Width="250"
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
                                        <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=WarehouseMk">
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
                                <ext:ComboBox ID="cmbZone" runat="server" Editable="false" FieldLabel="<%$ Resource : ZONE %>"
                                    EmptyText='<%$ Resource : PLEASE_SELECT %>' AllowBlank="false" Width="250" LabelWidth="80" LabelAlign="Right">
                                    <%-- <DirectEvents>
                                                <Select OnEvent="cmbState_Change" />
                                            </DirectEvents>--%>
                                </ext:ComboBox>
                                <%--                <ext:ComboBox ID="cmbZone"
                                    runat="server"
                                    ForceSelection="true"
                                    DisplayField="Name"
                                    ValueField="ZoneID"
                                    LabelAlign="Right"
                                    TriggerAction="All"
                                    FieldLabel="<%$ Resource : ZONE %>"
                                    AllowBlank="false"
                                    Width="250"
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
							                       {Name}
						                        </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                 <Listeners>
                                        <Select Handler="#{txtZoneShortName}.setValue(this.valueModels[0].data.ShortName);" />
                                    </Listeners>
                                    <Store>
                                        <ext:Store ID="StoreZone" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Zone">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model2" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ZoneID" />
                                                        <ext:ModelField Name="Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>

                                <ext:TextField ID="txtSearch" runat="server" TabIndex="1" EmptyText="Search.." MaxLength="50" EnforceMaxLength="true" AllowBlank="true" Width="350" />
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search" Width="60" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreData" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="240" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreWindowsShow" runat="server" PageSize="20" AutoLoad="false">
                            <Proxy>
                                <ext:AjaxProxy Url="">
                                    <ActionMethods Read="GET" />
                                    <Reader>
                                        <ext:JsonReader Root="data" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model8" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ProductID" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="Lot" />
                                        <ext:ModelField Name="PalletCode" />
                                        <ext:ModelField Name="ConversionQty" />
                                        <ext:ModelField Name="RemainQTY" />
                                        <ext:ModelField Name="ProductUnitName" />
                                        <ext:ModelField Name="ProductUnitID" />
                                        <ext:ModelField Name="LocationID" />
                                        <ext:ModelField Name="LocationNo" />
                                        <ext:ModelField Name="ZoneID" />
                                        <ext:ModelField Name="WarehouseName" />
                                        <ext:ModelField Name="ZoneName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column ID="ZoneID" runat="server" DataIndex="ZoneID" Hidden="true" />
                            <ext:Column ID="Column2" runat="server" DataIndex="LocationID" Hidden="true" />
                            <ext:RowNumbererColumn ID="colRowNo1" runat="server" Text="No" Width="40" Align="Center" Locked="true" />
                            <ext:Column ID="Column16" runat="server" DataIndex="ProductCode" Text="ProductCode" Width="90" Align="Center" Locked="true" />
                            <ext:Column ID="Column5" runat="server" DataIndex="ProductName" Text="ProductName" MinWidth="200" Align="Left" Locked="true" />
                            <ext:Column ID="Column18" runat="server" DataIndex="Lot" Text="Product Lot" Width="120" Align="Center" />
                            <ext:Column ID="Column17" runat="server" DataIndex="OrderNo" Text="Order No." MinWidth="80" Align="Left" Locked="true" />
                            <ext:Column ID="Column19" runat="server" DataIndex="PalletCode" Text="Pallet Code" Width="100" Align="Center" />
                            <ext:NumberColumn ID="NumberColumn1" runat="server" DataIndex="RemainQTY" Text="Stock OnHand" Width="80" Align="Center" Format="#,###" />
                            <ext:Column ID="Column21" runat="server" DataIndex="ProductUnitName" Text="Unit" Width="100" Align="Center" />
                            <ext:Column ID="Column1" runat="server" DataIndex="ZoneName" Text="Zone" Width="130" Align="Center" />
                            <ext:NumberColumn ID="NumberColumn2" runat="server" DataIndex="ConversionQty" Text="Package Actual Qty" Width="80" Align="Center" Format="#,###" />
                            <ext:Column ID="Column22" runat="server" DataIndex="LocationNo" Text="LocationNo" Width="130" Align="Center" />
                            <ext:Column ID="Column23" runat="server" DataIndex="WarehouseName" Text="Warehouse" Width="130" Align="Center" />
                        </Columns>
                    </ColumnModel>
                    <Listeners>
                    </Listeners>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" CheckOnly="true" Width="30" Align="Center">
                            <Listeners>
                                <BeforeDeselect Fn="before_select_row" />
                            </Listeners>
                        </ext:CheckboxSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:CellEditing ClicksToEdit="0">
                        </ext:CellEditing>
                    </Plugins>
                    <View>
                        <ext:GridView ID="GridView2" runat="server" LoadMask="true" LoadingText="Loading..." />
                    </View>
                    <BottomBar>
                        <%--   <ext:PagingToolbar runat="server" ID="PagingToolbar2" DisplayInfo="false">
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
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Click" />
                                    </DirectEvents>
                                </ext:ComboBox>

                                <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnConfirm_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreDetail" Mode="Raw" Value="Ext.encode(#{GridGetWindows}.getRowsValues({selectedOnly : false}))" />
                                                <ext:Parameter Name="ParamStoreData" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />

                                            </ExtraParams>
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnWinExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="9">
                                    <Listeners>
                                        <Click Handler="#{WindowDataDetail}.hide();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:PagingToolbar>--%>

                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="false" DisplayMsg="DisplayingFromTo"
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
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Click" />
                                    </DirectEvents>
                                </ext:ComboBox>

                                <ext:ToolbarFill />
                                <ext:Button runat="server" Text="Select" Icon="BasketEdit">
                                    <DirectEvents>
                                        <Click OnEvent="btnConfirm_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreDetail" Mode="Raw" Value="Ext.encode(#{GridGetWindows}.getRowsValues({selectedOnly : false}))" />
                                                <ext:Parameter Name="ParamStoreData" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnWinExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="9">
                                    <Listeners>
                                        <Click Handler="#{WindowDataDetail}.hide();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:PagingToolbar>

                    </BottomBar>
                </ext:GridPanel>

            </Items>
        </ext:Window>
    </form>
</body>
</html>

