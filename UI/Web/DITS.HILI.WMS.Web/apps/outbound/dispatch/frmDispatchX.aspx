<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDispatchX.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.dispatch.frmDispatchX" %>

 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

 <%--   <script type="text/javascript">
        Ext.Ajax.timeout = 180000; // 1 sec
        Ext.net.DirectEvent.timeout = 180000; // 1 sec
    </script>--%>
    <ext:XScript runat="server">
        <script>
              
        </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Label runat="server" ID="lbMsgError" Text="<%$ Resource : ERROR %>" Hidden="true" /> 
 
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel runat="server" ID="FormPanelDetail"
                    BodyPadding="5" Region="North" Frame="true" MaxHeight="500" AutoScroll="false"
                    Margins="3 3 0 3" Layout="AnchorLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:ToolbarFill />
                                <ext:TextField ID="txtPONo" runat="server"  Name="txtPONo" FieldLabel="PO No./Trans WH"  Width="400">
                                            <Listeners> 
                                            </Listeners>
                                        </ext:TextField>



                                <ext:ComboBox ID="cmbDisType" runat="server" TabIndex="1" 
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>" AutoFocus="true"
                                            PageSize="25" MinChars="0" AllowBlank="true"
                                            TypeAhead="true" AutoShow="false"
                                            ForceSelection="true" Width="120"> 
                                         <Items>
                                            <ext:ListItem Text="Dispatch" Value="0"></ext:ListItem>
                                            <ext:ListItem Text="Transfer WH" Value="1"></ext:ListItem>
                                         </Items>
                                        </ext:ComboBox>


                                <ext:Button runat="server" Text="<%$ Resource : DELETE %>" Icon="Delete" ID="btnDelete" Width="100" >
                                     <DirectEvents>
                                                <Click OnEvent="btnDelete_Click" />
                                            </DirectEvents> 
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items></Items>
                         
                    </ext:FormPanel>
            </Items>
        </ext:Viewport> 
    </form>
</body>
</html>

