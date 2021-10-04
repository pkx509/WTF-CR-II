<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateWarehouseType.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmCreateWarehouseType" %>

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

        div#ListCmbSiteName {
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
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                <ext:Hidden ID="WarehouseTypeKey" runat="server" />
                                <%--                                <ext:TextField runat="server" ID="txtCode"
                                    FieldLabel="WHTypeCode"
                                    TabIndex="1"
                                    ReadOnly="true"
                                    LabelWidth="140" MaxLength="11" EnforceMaxLength="true" AllowBlank="false" />--%>

                                <%--   <ext:ComboBox runat="server"
                                    ID="cmbSiteName"
                                    LabelWidth="140"
                                    DisplayField="SiteName"
                                    ValueField="SiteID"
                                    TriggerAction="All"
                                    FieldLabel="SiteName"
                                    AllowBlank="false"
                                    EmptyText="PleaseSelect"
                                    AllowOnlyWhitespace="false"
                                    ForceSelection="true"
                                    TypeAhead="true"
                                    ReadOnly="true"
                                    PageSize="0" MinChars="0">

                                    <ListConfig LoadingText="Searching..." ID="ListCmbSiteName" MaxHeight="100">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                         {SiteName}
						                        </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreSiteName" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Site">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model6" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="SiteName" />
                                                        <ext:ModelField Name="SiteID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>

                                <ext:TextField runat="server" ID="txtName" FieldLabel='<%$ Resource : WAREHOUSE_TYPE_NAME %>' TabIndex="3" LabelWidth="140" MaxLength="50" EnforceMaxLength="true" AllowBlank="false" AllowOnlyWhitespace="false" AutoFocus="true" />
                                <ext:TextField runat="server" ID="txtDescription" FieldLabel="<%$ Resource : DESCRIPTION %>" TabIndex="4" LabelWidth="140" MaxLength="50" EnforceMaxLength="true" />
                                <ext:Checkbox runat="server" ID="txtIsActive" FieldLabel="<%$ Resource : ACTIVE %>" Name="IsActive" Checked="true" LabelWidth="140" />
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
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnExit_Click" />
                                    </DirectEvents>
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


