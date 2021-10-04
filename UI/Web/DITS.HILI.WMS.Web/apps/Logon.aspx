<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.Logon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> 
    <link rel="shortcut icon" href="favicon.ico" /> 
    <style>
        body {
            background-color: black;
            background-image: url(../resources/images/hili/WMSonline-Login.jpg);
            background-repeat: no-repeat;
            background-position: center top;
            width: 1024px;
            height: 600px;
        }
    </style>
</head> 
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager runat="server" />

    <ext:Window
            ID="Window1"
            runat="server"
            Closable="false"
            Resizable="false"
            Height="150"
            Icon="Lock"
            Title="<%$ Resource : LOGIN %>"
            Draggable="false"
            Width="340"
            BodyPadding="10"
            Layout="FormLayout">
            <Items>
                <ext:TextField
                    ID="txtUsername" 
                    runat="server"  
                    FieldLabel="<%$ Resource : USER_NAME %>"
                    AllowBlank="false" 
                    AutoFocus="true"
                    Text=""
                    LabelAlign="Right" />
                <ext:TextField
                    ID="txtPassword" 
                    runat="server"
                    Text=""
                    InputType="Password"
                    FieldLabel="<%$ Resource : PASSWORD %>"
                    AllowBlank="false"
                    LabelAlign="Right" >
                    <Listeners>
                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnLogin}.fireEvent('click');}" />
                    </Listeners>
                </ext:TextField> 
            </Items>
            <Buttons>
                <ext:Button ID="btnLogin" runat="server" Text="<%$ Resource : LOGIN %>" Icon="LockStart">
 
                    <DirectEvents>
                        <Click OnEvent="btnLogin_Click"
                            Before="Ext.Msg.wait('Verifying...', 'Authentication');"
                            Failure="Ext.Msg.show({
                            title: 'Login Error',
                            msg: result.errorMessage,
                            buttons: Ext.Msg.OK,
                            icon: Ext.MessageBox.ERROR
                            });">
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="btnCancel" runat="server" Text="<%$ Resource : CANCEL %>" Icon="Decline">
                    <Listeners>
                        <Click Handler="#{txtUsername}.reset();#{txtPassword}.reset();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>
