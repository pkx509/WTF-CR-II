<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateSiteName.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateSiteName" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <ext:XScript runat="server">
                <script>
                    var Setvalue_PostCode = function (combo,records) {

                        #{Site_PostCode}.setValue(records[0].data.PostCode);
                        
                    }

                    var EnableContactCode = function () {
                        var Site_Code = Ext.getCmp("Site_Code");
                        if (Site_Code.value == 'new') {
                            Site_Code.setReadOnly(false);
                            Site_Code.setValue('');
                            Site_Code.allowBlank = false;
                            Site_Code.validate();
                            Site_Code.focus('', 10);
                        } else {
                            Site_Code.setReadOnly(true);
                            Site_Code.setValue('new');
                        };
                    };

                    </script>
    </ext:XScript>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:Hidden runat="server" ID="hidIsPopup" />
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="FitLayout">
                    <FieldDefaults LabelAlign="Right" LabelWidth="90" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" Flex="1" DefaultAnchor="100%">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtSiteID" FieldLabel='<%$ Resource : SITE_CODE %>' TabIndex="1" Hidden="true" ReadOnly="true" MaxLength="50" EnforceMaxLength="true" />
                                        <ext:Button runat="server" Hidden="true" Icon="NoteEdit" MarginSpec="0 0 0 5" ID="btnSelectList">
                                            <Listeners>
                                                <Click Fn="EnableContactCode" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" Flex="1" DefaultAnchor="100%">
                                    <Items>
                                        <ext:TextField runat="server" ID="SiteName" FieldLabel='<%$ Resource : SITE_NAME %>' MarginSpec="5 0 5 0" TabIndex="2" AllowBlank="false" MaxLength="50" EnforceMaxLength="true" AllowOnlyWhitespace="false" AutoFocus="true" />
                                        <ext:TextField runat="server" ID="SiteAdress" FieldLabel='<%$ Resource : ADDRESS %>' MarginSpec="0 0 5 0" TabIndex="3" MaxLength="500" EnforceMaxLength="true" />
                                        <ext:TextField runat="server" ID="SiteRoad" FieldLabel='<%$ Resource : ROAD %>' MarginSpec="0 0 5 0" TabIndex="4" MaxLength="50" EnforceMaxLength="true" />

                                        <ext:ComboBox
                                            ID="cmbProvince"
                                            runat="server"
                                            LabelWidth="100"
                                            DisplayField="Name"
                                            ValueField="Province_Id"
                                            AllowBlank="false"
                                            AllowOnlyWhitespace="false"
                                            ForceSelection="true"
                                            TriggerAction="All"
                                            PageSize="25"
                                            FieldLabel='<%$ Resource : PROVINCE %>'
                                            LabelAlign="Right"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TypeAhead="true"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreProvince" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=Province">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Province_Id" />
                                                                <ext:ModelField Name="Name" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <DirectEvents>
                                                <Select OnEvent="cmbProvince_Change" />
                                            </DirectEvents>
                                        </ext:ComboBox>

                                        <ext:ComboBox ID="cmbDistrict"
                                            runat="server"
                                            LabelWidth="100"
                                            DisplayField="Name"
                                            ValueField="District_Id"
                                            AllowBlank="false"
                                            AllowOnlyWhitespace="false"
                                            ForceSelection="true"
                                            TriggerAction="All"
                                            FieldLabel='<%$ Resource : DISTRICT %>'
                                            LabelAlign="Right"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TypeAhead="true"
                                            PageSize="25"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreDistrict" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=District">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model1" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="District_Id" />
                                                                <ext:ModelField Name="Name" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <DirectEvents>
                                                <Select OnEvent="cmbDistrict_Change" />
                                            </DirectEvents>
                                        </ext:ComboBox>

                                        <ext:ComboBox ID="cmbSubDistrict"
                                            runat="server"
                                            LabelWidth="100"
                                            TabIndex="12"
                                            DisplayField="Name"
                                            ValueField="SubDistrict_Id"
                                            AllowBlank="false"
                                            AllowOnlyWhitespace="false"
                                            ForceSelection="true"
                                            TriggerAction="All"
                                            FieldLabel='<%$ Resource : SUB_DISTRICT %>'
                                            LabelAlign="Right"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TypeAhead="true"
                                            PageSize="25"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreSubDistrict" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=SubDistrict">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model6" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="SubDistrict_Id" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="PostCode" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Fn="Setvalue_PostCode" />
                                            </Listeners>
                                            <%--   <DirectEvents>
                                                   <Select OnEvent="cmbSubDistrict_Change" />
                                            </DirectEvents>--%>
                                        </ext:ComboBox>

                                        <ext:TextField runat="server" ID="Site_PostCode" FieldLabel='<%$ Resource : ZIPCODE %>' MarginSpec="0 0 5 0" TabIndex="8" MaskRe="/^\d+$/" Regex="/^\d+$/" MaxLength="50" EnforceMaxLength="true" />
                                        <ext:TextField runat="server" ID="SiteTel" FieldLabel='<%$ Resource : TELNO %>' MarginSpec="0 0 5 0" TabIndex="9" MaxLength="50" EnforceMaxLength="true" />
                                        <ext:TextField runat="server" ID="SiteFax" FieldLabel='<%$ Resource : FAXNO %>' MarginSpec="0 0 5 0" TabIndex="10" MaxLength="50" EnforceMaxLength="true" />
                                        <ext:Checkbox ID="txtIsActive" runat="server" FieldLabel='<%$ Resource : ACTIVE %>' MarginSpec="0 0 5 0" Name="IsActive" Checked="true" />
                                    </Items>
                                </ext:Container>
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

