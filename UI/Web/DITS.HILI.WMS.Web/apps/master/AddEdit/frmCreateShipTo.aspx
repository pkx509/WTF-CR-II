<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCreateShipTo.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateShipTo" %>

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
                    BodyPadding="10" Flex="1" Layout="FitLayout" Frame="true">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>

                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : SHIPTO %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:Hidden runat="server" ID="txtShipCode" ></ext:Hidden>
                                        <ext:TextField runat="server" ID="txtShipName" AllowBlank="false" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                                  <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUESHIPSNAME %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items> 
                                        <ext:TextField runat="server" ID="txtShipShortName" AllowBlank="false" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : RULE_NAME %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right" >
                                    <Items>

                                        <ext:ComboBox ID="cmbRule"
                                            Editable="true"
                                            runat="server"
                                            DisplayField="RuleName"
                                            ValueField="RuleId"
                                            TriggerAction="All"
                                            SelectOnFocus="true"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            PageSize="25"
                                            MinChars="0"
                                            Width="250"
                                            LabelAlign="Right"
                                            AllowBlank="false">
                                            <ListConfig LoadingText="Searching..." ID="ListSHIPTO" MaxHeight="150">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item">
							                              {RuleName}
						                                </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store ID="StoreRule" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=BookingRule">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model4" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="RuleId" />
                                                                <ext:ModelField Name="RuleName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right" Hidden="false">
                                    <Items>
                                        <ext:Checkbox ID="ckbIsActive" runat="server" Name="IsActive" LabelWidth="150" Checked="true" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : DEFAULT %>" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right" Hidden="false">
                                    <Items>
                                        <ext:Checkbox ID="ckbIsDefault" runat="server" Name="IsDefault" LabelWidth="150" Checked="true" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:Hidden runat="server" ID="hddDockCode"></ext:Hidden>
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
                                <ext:Button ID="btnSave" runat="server" Icon="Disk"
                                    Text="<%$ Resource : SAVE %>" Width="60" Disabled="true" TabIndex="7">
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
