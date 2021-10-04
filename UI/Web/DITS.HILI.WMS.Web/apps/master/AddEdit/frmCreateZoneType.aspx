<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateZoneType.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateZoneType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

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
                                <ext:TextField runat="server" ID="txtZoneType_Code" FieldLabel="ZoneTypeCode" TabIndex="1" Hidden="true" LabelWidth="150" />

                                <%-- <ext:ComboBox ID="cmbSite" runat="server" 
                                    LabelWidth="150"
                                    DisplayField="SiteName" 
                                    ValueField="SiteID" 
                                    ReadOnly="true"
                                    TriggerAction="All" 
                                    FieldLabel='<%$ Resource : SITE_NAME %>'
                                    EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                    TypeAhead="true" 
                                    PageSize="0" MinChars="0" 
                                    TabIndex="2" 
                                    AllowBlank="false" 
                                    AllowOnlyWhitespace="false"
                                     ForceSelection="true">
                                    <ListConfig LoadingText="Searching..." ID="ListCmbSiteName" MaxHeight="100">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                        {SiteID} : {SiteName}
						                        </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreSite" runat="server" AutoLoad="true">
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
                                                        <ext:ModelField Name="SiteID" />
                                                        <ext:ModelField Name="SiteName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>--%>

                                <ext:TextField runat="server" ID="txtName" FieldLabel='<%$ Resource : ZONE_TYPE_NAME %>' TabIndex="3" LabelWidth="150" AllowBlank="false" MaxLength="50" EnforceMaxLength="true" AutoFocus="true" />

                                <%--<ext:SelectBox runat="server" ID="cmbPlacement_Type" 
                                    DisplayField="Text" ValueField="Value" 
                                    FieldLabel="PlacementType" EmptyText="PleaseSelect"
                                    TabIndex="4" LabelWidth="150" AllowBlank="false" AllowOnlyWhitespace="false" ForceSelection="true">                                    
                                    <Store>
                                        <ext:Store runat="server" ID="StorePlacement_Type">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Text" />
                                                        <ext:ModelField Name="Value" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:SelectBox>--%>

                                <ext:ComboBox
                                    runat="server"
                                    ID="cmbPlacement_Type"
                                    AllowOnlyWhitespace="false"
                                    ForceSelection="true"
                                    FieldLabel="PlacementType"
                                    LabelWidth="150"
                                    EmptyText="PleaseSelect"
                                    AllowBlank="false"
                                    TriggerAction="All"
                                    TypeAhead="true">
                                </ext:ComboBox>

                                <ext:TextField runat="server" ID="txtDescription" FieldLabel='<%$ Resource : DESCRIPTION %>' TabIndex="5" LabelWidth="150" />
                                <ext:Checkbox ID="txtIsActive" runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Name="IsActive" LabelWidth="150" Checked="true" LabelAlign="Right" />
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
                                    Icon="Disk" Text='<%$ Resource : SAVE %>' Width="60" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text='<%$ Resource : CLEAR %>' Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text='<%$ Resource : EXIT %>' Width="60" TabIndex="16">
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

