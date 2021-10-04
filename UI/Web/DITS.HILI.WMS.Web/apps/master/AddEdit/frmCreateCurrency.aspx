<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateCurrency.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateCurrency" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
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
    </style>
    <ext:XScript runat="server">
        <script>
            var default_change = function(){
                var chkdefault = this;

                var v = chkdefault.getValue();

                if(v)
                {
                    #{txtExchange_Rate}.setValue(1);
                    #{txtExchange_Rate}.setDisabled(1);
                }
                else
                {
                    #{txtExchange_Rate}.setDisabled(0);
                }

            }
        </script>
    </ext:XScript>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="FitLayout">
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%" Padding="3">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>

                                <ext:TextField runat="server" ID="txtCurrency_Code" FieldLabel="CurrencyCode" TabIndex="1" LabelWidth="120" ReadOnly="true" LabelAlign="Right" />
                                <ext:TextField runat="server" ID="txtCurrency_Short_Name" FieldLabel="Currency" LabelAlign="Right" MinLength="3" MaxLength="3" TabIndex="2" FieldStyle="text-transform: uppercase;" LabelWidth="120" AllowBlank="false" Regex="/^[a-zA-Z]*$/" AllowOnlyWhitespace="false" EnforceMaxLength="true" AutoFocus="true" />
                                <ext:TextField runat="server" ID="txtCurrency_Full_Name" FieldLabel="CurrencyFullName" TabIndex="3" LabelWidth="120" LabelAlign="Right" />
                                <ext:NumberField runat="server" ID="txtExchange_Rate" Format="#,###.00" FieldLabel="ExchangeRate" LabelAlign="Right" TabIndex="4" LabelWidth="120" AllowBlank="false" MinValue="0.01" />
                                <ext:Checkbox runat="server" ID="txtIsDefault" FieldLabel="DefaultCurrency" Name="IsDefault" LabelAlign="Right" Checked="true" LabelWidth="120">
                                    <Listeners>
                                        <Change Fn="default_change" />
                                    </Listeners>
                                </ext:Checkbox>
                                <ext:Checkbox runat="server" ID="txtIsActive" FieldLabel="IsActive" Name="IsActive" LabelAlign="Right" Checked="true" LabelWidth="120" />

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
                                    Icon="Disk" Text="Save" Width="60" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                       <%-- <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);" 
                                            Buffer="350"/>--%>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="parentAutoLoadControl.close();" />
                                    </Listeners>
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
