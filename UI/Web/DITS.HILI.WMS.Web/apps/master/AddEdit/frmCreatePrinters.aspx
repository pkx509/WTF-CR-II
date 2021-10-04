<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreatePrinters.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreatePrinters" %>

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
        .my-combo  {
            height: 150px !important;
        }

        /*div#ListCmbPrintersLocation {
            border-top-width: 1 !important;
        }*/
    </style>
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
                                <ext:Hidden ID="txtPrinterID" runat="server" />
                                <ext:TextField runat="server" ID="txtPrintersLocation"
                                    FieldLabel="Printers Location Code" TabIndex="1" ReadOnly="true" LabelWidth="170" Hidden="true" />
                                <ext:ComboBox ID="cmbPrinterName" runat="server" Width="100"
                                    DisplayField="PrinterName" ValueField="PrinterName" TabIndex="2"
                                    FieldLabel='<%$ Resource : PRINTER_MACHINE %>' EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                    PageSize="25" MinChars="0" SelectOnFocus="true" AllowBlank="false"
                                    TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                    ForceSelection="true" AllowOnlyWhitespace="false" LabelWidth="170">
                                    <ListConfig LoadingText="Searching..." ID="ListcmbPrinterName" Height="150">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
                                                {PrinterName}
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StorePrinterName" runat="server" AutoLoad="false">
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=PrinterMachine">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="PrinterId" />
                                                        <ext:ModelField Name="PrinterName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:ComboBox ID="cmbPrintersLocation" 
                                    runat="server" 
                                    Width="100"
                                    DisplayField="LineCode" 
                                    ValueField="LineID" 
                                    TabIndex="2"
                                    FieldLabel='<%$ Resource : PRINTER_LOCATION %>' 
                                    EmptyText='<%$ Resource : PLEASE_SELECT %>'
                                    PageSize="25" MinChars="0" 
                                    SelectOnFocus="true" 
                                    AllowBlank="false"
                                    TypeAhead="true" 
                                    TriggerAction="Query"
                                    QueryMode="Remote" 
                                    AutoShow="false"
                                    ForceSelection="true" 
                                    AllowOnlyWhitespace="false" 
                                    LabelWidth="170">
                                    <ListConfig LoadingText="Searching..." ID="ListCmbPrintersLocation"  Cls="my-combo" >
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
                                                {LineCode}
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StorePrintersLocation" runat="server" AutoLoad="false" >
                                            <Proxy>
                                                <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=PrinterLocation">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model2" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="LineID" />
                                                        <ext:ModelField Name="LineCode" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>

                                <ext:TextField runat="server" ID="txtDescription"
                                    FieldLabel="Description" TabIndex="4" LabelWidth="170"
                                    MaxLength="50" EnforceMaxLength="true" />

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
                                    Icon="Disk" Text="<%$ Resource : SAVE %>" Width="60" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Single="true" Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
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
