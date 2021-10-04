<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateCutoffDate.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateCutoffDate" %>

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
                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="false"
                    BodyPadding="3"
                    Region="Center"
                    Frame="true" 
                    Layout="AutoLayout"> 
                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                    <ext:Hidden ID="txtCutoffID" runat="server" /> 
                                     <ext:DateField runat="server"
                                                    ID="dtCutoffDate"
                                                    FieldLabel='<%$ Resource : MONTHENDDATE %>'
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    TabIndex="1"
                                                    Format="dd/MM/yyyy"
                                                    Width="100px" />
                                     <ext:TimeField runat="server"
                                                    ID="dtCutoffTime"
                                                    FieldLabel='<%$ Resource : MONTHENDTIME %>'
                                                    MaxLength="10"
                                                    EnforceMaxLength="true"
                                                    TabIndex="2"
                                                    Format="HH:mm"
                                                    Width="100px" />
                                 </Items>
                                
                            </ext:Container>
                    </Items>
                       <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Button ID="Button1" runat="server"
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Single="true"
                                            Buffer="350" />

                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="60" TabIndex="16">
                                              <DirectEvents>
                                        <Click OnEvent="btnClear_Click"></Click>
                                    </DirectEvents>
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
