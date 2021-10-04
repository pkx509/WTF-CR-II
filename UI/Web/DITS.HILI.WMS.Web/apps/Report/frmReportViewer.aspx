<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportViewer.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.Report.frmReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <div style="margin-top: -20px">

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <rsweb:ReportViewer ID="ReportViewer1" Width="100%" Height="600" runat="server" AsyncRendering="false">
            </rsweb:ReportViewer>
            <asp:HiddenField ID="DatePickers" runat="server" />
        </div>
    </form>
</body>
</html>
