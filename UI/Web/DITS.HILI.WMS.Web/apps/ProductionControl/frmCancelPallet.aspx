<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCancelPallet.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.ProductionControl.frmCancelPallet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <title></title>
    <ext:XScript runat="server">
        <script></script>
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
                        <ext:Hidden runat="server" ID="hdPackingDetailID" />

                        <ext:Container runat="server" Layout="AnchorLayout">
                            <Items>

<%--                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Label runat="server" ID="lbPalletDetail" Flex="1" />
                                    </Items>
                                </ext:FieldContainer>--%>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtPalletCode" FieldLabel="<% $Resource : PALLET_CODE %>" Flex="1" ReadOnly="true" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtUsername" FieldLabel="<% $Resource : USER_NAME %>" Flex="1" AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextField
                                            ID="txtPassword"
                                            runat="server"
                                            InputType="Password"
                                            FieldLabel="<%$ Resource : PASSWORD %>"
                                            AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:TextArea runat="server" ID="txtRemark" FieldLabel="<% $Resource : REMARK %>" Flex="1" AllowOnlyWhitespace="false" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>

                    </Items>

                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill runat="server" />

                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="<% $Resource : SAVE %>" MarginSpec="0 0 0 5" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="<% $Resource : SAVING %>" MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </BottomBar>

                    <Listeners>
                        <ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />
                    </Listeners>

                </ext:FormPanel>

            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
