<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="frmCreateTruckNo.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateTruckNo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
</head>
<ext:XScript runat="server">
        <script>
            var getProduct = function () {


            };

            var popupProduct = function () {

            };


        </script>

</ext:XScript>

<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="FitLayout" Frame="true">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                 <ext:Hidden ID="txtTruck" runat="server" />
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : TRUCK_NO %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Left"  >
                                    <Items>
                                        <ext:TextField runat="server" ID="txtTruckNo" AllowBlank="false" ></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>

                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : TRUCK_TYPE_NAME %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Left">
                                    <Items>

                                        <ext:ComboBox ID="cmbTruckType"
                                            runat="server"
                                            DisplayField="TypeName"
                                            ValueField="TruckTypeID"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TriggerAction="All"
                                            PageSize="0"
                                            MinChars="0"
                                            LabelAlign="Left"
                                            Width="143"
                                            AllowBlank="false">
                                            <ListConfig LoadingText="Searching..." ID="ListcmbTruckType" MaxHeight="150">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item">
							                                       {TypeName}
						                                 </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store ID="StoreTruckType" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckTypeOnly">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model3" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="TruckTypeID" />
                                                                <ext:ModelField Name="TypeName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>

                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ACTIVE %>" Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:Checkbox ID="txtIsActive" runat="server" Name="IsDefault" LabelWidth="150" Checked="true" LabelAlign="Right" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:Hidden runat="server" ID="truckTypeID"></ext:Hidden>
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
                                    Text="<%$ Resource : SAVE %>" Width="60" Disabled="true" TabIndex="7">
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

