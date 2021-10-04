<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmContactAddEdit.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmContactAddEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  
         <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
</head> 
<body>
    <form id="form1" runat="server"> 
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="AutoLayout">
            <Items>
               <ext:FormPanel runat="server" ID="FormPanelDetail" AutoScroll="false" BodyPadding="3" Region="Center" Frame="true"  Layout="AutoLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUECONTACTNAME %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items> 
                                        <ext:TextField runat="server" ID="txtDockName" AllowBlank="false" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUECONTACTDESC %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtDockDesc" AllowBlank="true" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right" Hidden="false">
                                    <Items>
                                        <ext:Checkbox ID="ckbIsActive" runat="server" Name="IsActive" LabelWidth="150" Checked="true" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer> 
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
                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="<%$ Resource : SAVE %>" Width="75" Disabled="true" TabIndex="7">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="#{btnSave}.setDisabled(true);" Complete="#{btnSave}.setDisabled(false);" Buffer="350" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="75" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="75" TabIndex="16">
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