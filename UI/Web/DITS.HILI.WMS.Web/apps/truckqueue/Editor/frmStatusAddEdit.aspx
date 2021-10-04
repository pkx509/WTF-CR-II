<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmStatusAddEdit.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmStatusAddEdit" %>
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
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUESTATSNAME %>' Layout="HBoxLayout" LabelWidth="200" LabelAlign="Right">
                                    <Items> 
                                        <ext:TextField runat="server" ID="txtQueueStatusName" AllowBlank="false" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUESTATSDESC %>' Layout="HBoxLayout" LabelWidth="200" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtQueueStatusDesc" AllowBlank="true" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>  
                                <ext:FieldContainer ID="fcWeekDaysOptions" runat="server" LabelWidth="190" LabelAlign="Right">
                                         <Items>
                                            <ext:RadioGroup runat="server" ID="rdSelectDefine" ColumnsNumber="1" Vertical="true">
                                                <Items>
                                                    <ext:Radio runat="server" ID="ckIsWaiting" BoxLabel="<%$ Resource : QUEUEISWAITING %>" LabelWidth="100" LabelAlign="Left" />
                                                    <ext:Radio runat="server" ID="ckIsInQueue" BoxLabel="<%$ Resource : QUEUEISINQUEUE %>" LabelWidth="100" LabelAlign="Left" />
                                                    <ext:Radio runat="server" ID="ckIsCompleted" BoxLabel="<%$ Resource : QUEUEISCOMPLETED %>" LabelWidth="100" LabelAlign="Left" />
                                                    <ext:Radio runat="server" ID="ckIsCancel"  BoxLabel="<%$ Resource : QUEUEISCANCEL %>" LabelWidth="100" LabelAlign="Left" /> 
                                                </Items>
                                            </ext:RadioGroup>
                                        </Items>
                                    <%--<ext:RadioGroup Width="150" ID="chkGroupWeekDaysOption" runat="server" ColumnsNumber="1" Vertical="true">
                                            <Items>
                                                 <ext:Radio ID="ckIsWaiting" BoxLabel="<%$ Resource : QUEUEISWAITING %>" runat="server"  LabelWidth="100" LabelAlign="Left" />
                                                 <ext:Radio ID="ckIsInQueue" BoxLabel="<%$ Resource : QUEUEISINQUEUE %>" runat="server"   LabelWidth="100" LabelAlign="Left" />
                                                 <ext:Radio ID="ckIsCompleted" BoxLabel="<%$ Resource : QUEUEISCOMPLETED %>" runat="server" LabelWidth="100" LabelAlign="Left" />
                                                 <ext:Radio ID="ckIsCancel" BoxLabel="<%$ Resource : QUEUEISCANCEL %>" runat="server"  LabelWidth="100" LabelAlign="Left" /> 
                                            </Items>
                                        </ext:RadioGroup>--%>
                                </ext:FieldContainer>   
                                   <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Layout="HBoxLayout" LabelWidth="200" LabelAlign="Right" Hidden="false">
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