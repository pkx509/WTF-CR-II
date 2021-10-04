<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateStandard_Unit.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateStandard_Unit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />

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
                                <ext:Hidden ID="hidIsPopup" runat="server" />
                                <ext:Hidden ID="txtStdUnit_ID" runat="server" />
                                <ext:TextField runat="server" FieldLabel="<%$ Resource : UNIT_NAME %>" ID="txtStdUnit_Name"
                                    TabIndex="1" LabelWidth="100" AllowBlank="false" AllowOnlyWhitespace="false"
                                    AutoFocus="true">

                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{txtStdUnit_ShortName}.focus(false, 100);}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField runat="server" FieldLabel="<%$ Resource : UNIT_SHORT_NAME %>" ID="txtStdUnit_ShortName" TabIndex="2" LabelWidth="100">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSave}.focus(false, 100);}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Checkbox ID="txtIsActive" runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Name="IsActive" LabelWidth="100" Checked="true" />
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
                                    Icon="Disk" Text="Save" Width="65" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350" />

                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="65" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset(#{txtStdUnit_Name}.focus()) ;" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="16">
                                    <DirectEvents>
                                        <Click OnEvent="btnExit_Click"></Click>
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
