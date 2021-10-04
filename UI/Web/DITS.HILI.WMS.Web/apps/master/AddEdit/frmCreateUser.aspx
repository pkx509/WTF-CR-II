<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateUser.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmCreateUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <style>
        .search-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

            .search-item h3 {
                display: block;
                font: inherit;
                font-weight: bold;
                color: #222;
                margin: 0px;
            }

                .search-item h3 span {
                    float: right;
                    font-weight: normal;
                    margin: 0 0 5px 5px;
                    width: 100px;
                    display: block;
                    clear: none;
                }

        p {
            width: 650px;
        }

        .ext-ie .x-form-text {
            position: static !important;
        }

        div#ListCmbSiteName {
            border-top-width: 1 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="ColumnLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" ColumnWidth="0.5">
                            <Items>
                                <ext:Hidden ID="hddKey" runat="server" />
                                <ext:TextField ID="txtUserName" runat="server" FieldLabel="<%$ Resource : USER_NAME %>" Name="txtUserName"
                                    AllowBlank="false" MaxLength="50" EnforceMaxLength="true"  >
                                    
                                </ext:TextField>

                                <ext:TextField ID="txtPassword" runat="server" FieldLabel="<%$ Resource : PASSWORD %>" Name="txtPassword"
                                    MaxLength="50" AllowBlank="false"  EnforceMaxLength="true"  InputType="Password">
                                </ext:TextField>
                                <ext:TextField ID="txtConfirmPassword" runat="server" FieldLabel="<%$ Resource : CONFIRM_PASSWORD %>" Name="txtConfirmPassword"
                                    MaxLength="50" AllowBlank="false"  EnforceMaxLength="true"  InputType="Password">
                                    <Validator Handler="return (value === this.previousSibling('[name=txtPassword]').getValue()) ? true : 'Passwords do not match.';" />
                                </ext:TextField> 
                                <ext:Checkbox ID="ChkIsActive" runat="server" FieldLabel="<%$ Resource : ACTIVE %>"
                                    Checked="true" />
                                <%--<ext:TextField ID="txtFName" runat="server" FieldLabel="ชือพนักงาน" Name="txtFName"
                            AnchorHorizontal="100%" AllowBlank="false" MaxLenth="50">
                        </ext:TextField>--%>
 

                                
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" ColumnWidth="0.5">
                            <Items>
                                <ext:Hidden ID="hidEmployeeID" runat="server" />
                                <ext:TextField ID="txtName" runat="server" FieldLabel="<%$ Resource : FIRST_NAME %>"
                                    AllowBlank="false" MaxLength="50" EnforceMaxLength="true" AllowOnlyWhitespace="false">
                                </ext:TextField>
                                <ext:TextField ID="txtSurName" runat="server" FieldLabel="<%$ Resource : LAST_NAME %>"
                                    AllowBlank="false" MaxLength="50" EnforceMaxLength="true" AllowOnlyWhitespace="false">
                                </ext:TextField> 
                               <ext:TextField ID="txtEmail" runat="server" FieldLabel="<%$ Resource : EMAIL %>"
                                    AllowBlank="false" MaxLength="50" EnforceMaxLength="true" AllowOnlyWhitespace="false">
                                </ext:TextField> 
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
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" Disabled="true" TabIndex="15">
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


