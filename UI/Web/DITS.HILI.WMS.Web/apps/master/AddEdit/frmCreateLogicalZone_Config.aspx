<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateLogicalZone_Config.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateLogicalZone_Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                        <ext:Container runat="server" Layout="AnchorLayout">
                            <Items>
                                <ext:Container Layout="ColumnLayout" Flex="1" runat="server" MarginSpec="5 0 5 0">
                                    <Items>
                                        <ext:TextField ID="txtLogicalZoneCode" ReadOnly="true" FieldLabel="Logical Zone Code " Flex="1" LabelWidth="150" LabelAlign="Right" runat="server" Width="300" />
                                    </Items>
                                </ext:Container>

                                <ext:ComboBox ID="cmbSupplier" runat="server" FieldLabel="ผู้ผลิต "
                                    DisplayField="Supplier_NameTH" ValueField="Supplier_Code"
                                    EmptyText="<%$Resources:Langauge, PleaseSelect%>" AllowBlank="false"
                                    PageSize="25" MinChars="0" Width="300" LabelWidth="150" SelectOnFocus="true"
                                    TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                    ForceSelection="true" AllowOnlyWhitespace="false" AutoFocus="true" TabIndex="1">
                                    <ListConfig LoadingText="Searching..." ID="ListComboSupplier">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                               {Supplier_Code}<br> 
                                                           {Supplier_NameTH}
						                                </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreSupplier" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model2" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Supplier_Code" />
                                                        <ext:ModelField Name="Supplier_NameTH" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                </ext:ComboBox>

                                <ext:ComboBox ID="cmbProductGroup3" runat="server" FieldLabel="กลุ่มสินค้าลำดับที่ 3 "
                                    DisplayField="ProductGroup_Level3_Full_Name" ValueField="ProductGroup_Level3_Code"
                                    EmptyText="<%$Resources:Langauge, PleaseSelect%>" AllowBlank="false"
                                    PageSize="25" MinChars="0" Width="300" LabelWidth="150" SelectOnFocus="true"
                                    TypeAhead="false" TriggerAction="Query" QueryMode="Remote" AutoShow="false"
                                    ForceSelection="true" AllowOnlyWhitespace="false" AutoFocus="false" TabIndex="1">
                                    <ListConfig LoadingText="Searching..." ID="ListComboProductGroup3">
                                        <ItemTpl runat="server">
                                            <Html>
                                                <div class="search-item">
							                               {ProductGroup_Level3_Code}<br> 
                                                           {ProductGroup_Level3_Full_Name}
						                                </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                    <Store>
                                        <ext:Store ID="StoreProductGroup3" runat="server" AutoLoad="true">
                                            <Proxy>
                                                <ext:AjaxProxy Url="">
                                                    <ActionMethods Read="POST" />
                                                    <Reader>
                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model1" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ProductGroup_Level3_Code" />
                                                        <ext:ModelField Name="ProductGroup_Level3_Full_Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                </ext:ComboBox>


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
                                    Icon="Disk" Text="<%$Resources:Langauge, Save%>" Width="60" Disabled="true" TabIndex="7">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" 
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$Resources:Langauge, Clear%>" Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$Resources:Langauge, Exit%>" Width="60" TabIndex="16">
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
