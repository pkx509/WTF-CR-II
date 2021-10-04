<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateProductionLine.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateProductionLine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">

        var SelectCheckBox = function () {
            var chkBOI = Ext.getCmp("chkBOI").value;
            var cmbTranReaChange = Ext.getCmp("cmbTranRea");

            if (chkBOI) {
                cmbTranReaChange.setDisabled(false);
            }
            else {
                cmbTranReaChange.setDisabled(true);
                cmbTranReaChange.setValue("");
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>

                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="FitLayout" Frame="true">

                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container Layout="AnchorLayout" runat="server" Flex="1" DefaultAnchor="100%">
                            <Items>
                                <ext:Hidden ID="txtCode" runat="server" />
                                <ext:Hidden runat="server" ID="add_edit_Status"></ext:Hidden>
                                <ext:FieldContainer runat="server" FieldLabel="Line Code" Layout="HBoxLayout" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtLine_Code" AllowBlank="false" EnforceMaxLength="true" MaxLength="10"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>

                                <%--                                        <ext:TextField runat="server" FieldLabel="Line Name" ID="txtLine_Name" Disabled="false" AllowBlank="false" EnforceMaxLength="true" MaxLength="50"></ext:TextField>--%>

                                <%--                                        <ext:FieldContainer runat="server" FieldLabel="BOI" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                            <Items>
                                                <ext:Checkbox ID="chkBOI" runat="server" Name="BOI" LabelWidth="150" LabelAlign="Right">
                                                    <Listeners>
                                                        <Change Fn="SelectCheckBox" />
                                                    </Listeners>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:FieldContainer>--%>


                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : BOICARD %>" Layout="HBoxLayout" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtBoiCard" MaxLength="3" AllowBlank="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="Line Type" Layout="HBoxLayout" LabelAlign="Right">
                                    <Items>
                                        <ext:ComboBox ID="cmbLineType" runat="server"
                                            Editable="false"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TriggerAction="All"
                                            MinChars="0"
                                            Width="150"
                                            LabelAlign="Right"
                                            AllowBlank="false">
                                            <Items>
                                                <ext:ListItem Text="SP" />
                                                <ext:ListItem Text="NP" />
                                            </Items>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : WAREHOUSE %>" Layout="HBoxLayout" LabelAlign="Right">
                                    <Items>
                                        <ext:ComboBox ID="cmbWarehouseName"
                                            Editable="false"
                                            runat="server"
                                            DisplayField="Name"
                                            ValueField="WarehouseID"
                                            TriggerAction="All"
                                            SelectOnFocus="true"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            PageSize="25"
                                            MinChars="0"
                                            Width="150"
                                            LabelAlign="Right"
                                            AllowBlank="false"
                                            TabIndex="2">
                                            <ListConfig LoadingText="Searching..." ID="ListCmbWarehouse_Name" MaxHeight="150">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item">
							                              {ShortName} : {Name}
						                                </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store ID="StoreWarehouseName" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Warehouse">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model4" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Code" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="ShortName" />
                                                                <ext:ModelField Name="WarehouseID" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server"  FieldLabel="<%$ Resource : ACTIVE %>" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:Checkbox ID="chkIsActive" runat="server" LabelWidth="150" Checked="true" LabelAlign="Right" />
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
                                <ext:Button ID="btnSave" runat="server" Icon="Disk"
                                    Text="Save" Width="60" Disabled="true" TabIndex="7">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="60" TabIndex="16">
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
