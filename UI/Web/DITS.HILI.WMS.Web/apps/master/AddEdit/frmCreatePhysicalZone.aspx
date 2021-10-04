<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCreatePhysicalZone.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreatePhysicalZone" %>

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
        }

        div#ListcmbZoneType_Name {
            border-top-width: 1 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="FitLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                            </Defaults>
                            <Items>
                                <ext:Hidden runat="server" ID="hidIsPopup" />
                                <ext:Hidden ID="txtPhysicalZone_Code_Key" runat="server" />
                                <ext:TextField runat="server" ID="txtPhysicalZone_Code"  FieldLabel='<%$ Resource : ZONE_CODE %>' TabIndex="1" AutoFocus="true" LabelWidth="120" MaxLength="10" EnforceMaxLength="true" AllowBlank="false"
                                    AllowOnlyWhitespace="false" />

                                <%-- <ext:ComboBox ID="cmbWarehouseName" 
                                    runat="server" 
                                    AutoFocus="true"
                                    DisplayField="Name" 
                                    ValueField="WarehouseID"
                                    FieldLabel="WarehoseName" 
                                    PageSize="20"
                                    MinChars="0"
                                    TypeAhead="false"
                                    TriggerAction="Query"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    TabIndex="3">
                                    <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name" MaxHeight="120">
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
                                                <ext:AjaxProxy Url="">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="data" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="WarehouseID" />
                                                        <ext:ModelField Name="Code" />
                                                        <ext:ModelField Name="Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>

                                <ext:ComboBox ID="cmbWarehouseName"
                                    runat="server"
                                    FieldLabel="<%$ Resource : WAREHOUSE_NAME %>"
                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                    DisplayField="Name"
                                    ValueField="WarehouseID"
                                    AllowBlank="false"
                                    AllowOnlyWhitespace="false"
                                    LabelWidth="120"
                                    PageSize="20"
                                    MinChars="0"
                                    TypeAhead="false"
                                    TriggerAction="Query"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    TabIndex="3">
                                    <ListConfig LoadingText="Searching..." ID="ListWarehouse">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                        {Code} : {Name} 
						                        </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store runat="server" ID="StoreWarehouseName" AutoLoad="false">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="WarehouseID" />
                                                        <ext:ModelField Name="Code" />
                                                        <ext:ModelField Name="Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:ComboBox ID="cmbZoneType"
                                    runat="server"
                                    DisplayField="Name"
                                    ValueField="ZoneTypeID"
                                    FieldLabel="<%$ Resource : ZONE_TYPE %>"
                                    LabelWidth="120"
                                    AllowBlank="false"
                                    AllowOnlyWhitespace="false"
                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                    PageSize="20"
                                    MinChars="0"
                                    TypeAhead="false"
                                    TriggerAction="Query"
                                    QueryMode="Remote"
                                    AutoShow="false"
                                    TabIndex="3">
                                    <ListConfig LoadingText="Searching..." ID="ListcmbZoneType_Name" MaxHeight="120">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
                                                    {Name}
                                                </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreZoneType" runat="server" AutoLoad="false" PageSize="10">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ZoneType">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ZoneTypeID" />
                                                        <ext:ModelField Name="Code" />
                                                        <ext:ModelField Name="Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:TextField runat="server" ID="txtName" FieldLabel="<%$ Resource : ZONE_NAME %>" TabIndex="3" LabelWidth="120" MaxLength="25" EnforceMaxLength="true" AllowBlank="false" AllowOnlyWhitespace="false" />
                                <ext:TextField runat="server" ID="txtShortName" FieldLabel="<%$ Resource : ZONE_SHORT_NAME %>" TabIndex="4" LabelWidth="120" MaxLength="2" MinLength="1" FieldStyle="text-transform: uppercase;" EnforceMaxLength="true" AllowBlank="false"   AllowOnlyWhitespace="false" />
                                <ext:Checkbox runat="server" ID="txtIsActive" FieldLabel="<%$ Resource : ACTIVE %>" Name="IsActive" Checked="true" LabelWidth="120" />
                            </Items>
                        </ext:Container>
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />
                    </Listeners>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="60" Disabled="true" TabIndex="6">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnClear_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="8">
                                    <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

