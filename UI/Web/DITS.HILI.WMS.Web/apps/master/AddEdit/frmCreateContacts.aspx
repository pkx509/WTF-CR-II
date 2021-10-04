<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateContacts.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateContacts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

    <style>
        div#container-1010 {
            height: 220px !important;
        }

        div#gridCustomer {
            height: 215px !important;
        }

        div#gridCustomer-body {
            height: 215px !important;
        }
    </style>

    <script type="text/javascript">

        var EnableContactCode = function () {
            var txtContact_Code = Ext.getCmp("txtContact_Code");
            if (txtContact_Code.value == 'new') {
                txtContact_Code.setReadOnly(false);
                txtContact_Code.setValue('');
                txtContact_Code.allowBlank = false;
                txtContact_Code.validate();
                txtContact_Code.focus('', 10);
            } else {
                txtContact_Code.setReadOnly(true);
                txtContact_Code.setValue('new');
            };
        };

        var SelectCheckBox = function () {
            var chkSupplier1 = Ext.getCmp("chkIsSupplier").value;
            var chkCustomer1 = Ext.getCmp("chkIsCustomer").value; 
            var cmbRouteChange = Ext.getCmp("cmbRoute");           
            if(chkSupplier1 == true && chkCustomer1 == false){
                cmbRouteChange.setReadOnly(true); 
                cmbRouteChange.setValue("");
            }
            else if(chkSupplier1 == false && chkCustomer1 == true){
                cmbRouteChange.setReadOnly(false);            
            }
            else if(chkSupplier1 == false && chkCustomer1 == false){
                cmbRouteChange.setReadOnly(true);            
            }
            else{
                cmbRouteChange.setReadOnly(false); 
            }
        };

    </script>
    <ext:XScript runat="server">
                <script>
                    var Setvalue_PostCode = function (combo,records) {

                        #{txtContact_Postcode}.setValue(records[0].data.PostCode);
                        
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
                    BodyPadding="10" Flex="1" Layout="AnchorLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <FieldDefaults LabelWidth="100" />
                    <FieldDefaults InputWidth="150" />

                    <Items>
                        <%--<ext:Hidden ID="txtEmployee_ID" runat="server" />--%>
                        <ext:Hidden runat="server" ID="hidIsPopup" />
                        <ext:Hidden runat="server" ID="hitContactsCode" />
                        <ext:Hidden runat="server" ID="hitCustomer_Code" />
                        <ext:Container Layout="ColumnLayout" runat="server">
                            <Items>
                                <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.95">
                                    <Items>

                                        <%-- <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : CODE %>'
                                            Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField ReadOnly="true" runat="server" MaxLength="10"
                                                    ID="txtCode" TabIndex="1" FieldCls="readonly-field" />
                                                <ext:Button runat="server" Icon="NoteEdit" Margins="0 0 0 5" ID="btnSelectList">
                                                    <Listeners>
                                                        <Click Fn="EnableContactCode" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:FieldContainer>--%>

                                        <ext:TextField LabelAlign="Right" runat="server" FieldLabel='<%$ Resource : CODE %>' ID="txtCode" TabIndex="2" AllowBlank="false" AutoFocus="true">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtCode}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:TextField LabelAlign="Right" runat="server" FieldLabel='<%$ Resource : NAME %>' ID="txtName" TabIndex="2" AllowBlank="false">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtName}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:TextField>

                                        <%-- <ext:TextField runat="server" FieldLabel="NameEng" ID="txtContact_NameEN" TabIndex="3" LabelWidth="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtContact_ContractName}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:TextField>--%>

                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : CONTACT_NAME %>' ID="txtContact_ContractName" TabIndex="4" LabelWidth="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtContact_Tel}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : TELNO %>' ID="txtContact_Tel" TabIndex="5">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{txtContact_Fax}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:TextField>

                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : FAXNO %>' ID="txtContact_Fax" TabIndex="6" LabelWidth="100" />
                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : WEB_SITE %>' ID="txtContact_URL" TabIndex="7" />
                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : EMAIL %>' ID="txtContact_Email" TabIndex="8" />
                                        <ext:Checkbox ID="chkISactive" runat="server"
                                                            BoxLabel='<%$ Resource : ACTIVE %>' MarginSpec="0 0 0 105"> 
                                                        </ext:Checkbox>
                                    </Items>
                                </ext:Container>

                                <ext:Container Layout="AnchorLayout" runat="server">

                                    <Items>
                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : ADDRESS %>' ID="txtContact_Address" TabIndex="9" />

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
                                            Width="350"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreProvince" runat="server" AutoLoad="false" PageSize="10">
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

                                        <ext:ComboBox
                                            ID="cmbDistrict"
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
                                            Width="350"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreDistrict" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=District">
                                                            <ActionMethods Read="GET" />
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

                                        <ext:ComboBox
                                            ID="cmbSubDistrict"
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
                                            Width="350"
                                            MinChars="0">
                                            <Store>
                                                <ext:Store ID="StoreSubDistrict" runat="server" AutoLoad="false">
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
                                        </ext:ComboBox>

                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : ZIP_CODE %>' ID="txtContact_Postcode" TabIndex="13" LabelWidth="100" />
                                        <ext:TextField runat="server" FieldLabel='<%$ Resource : COUNTRY %>' ID="txtContact_Country" TabIndex="14" LabelWidth="100" />
                                        <ext:Container Layout="ColumnLayout" runat="server">
                                            <Items>
                                                <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.6" MarginSpec="0 0 5 0">
                                                    <Items>
                                                        <ext:Checkbox ID="chkIsSupplier" runat="server"
                                                            BoxLabel='<%$ Resource : SUPPLIERNAME %>' MarginSpec="0 0 0 105">
                                                            <Listeners>
                                                                <Change Fn="SelectCheckBox" />
                                                            </Listeners>
                                                        </ext:Checkbox>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container Layout="AnchorLayout" runat="server" ColumnWidth="0.4">
                                                    <Items>
                                                        <ext:Checkbox ID="chkIsCustomer" runat="server"
                                                            BoxLabel='<%$ Resource : SUB_COUSTOMER %>' MarginSpec="0 0 0 20">
                                                            <Listeners>
                                                                <Change Fn="SelectCheckBox" />
                                                            </Listeners>
                                                        </ext:Checkbox>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>

                                        <ext:ComboBox
                                            ID="cmbRoute"
                                            runat="server"
                                            LabelWidth="100"
                                            TabIndex="11"
                                            QueryMode="Local"
                                            DisplayField="ValueTH"
                                            ValueField="Code"
                                            EnforceMaxLength="true"
                                            TriggerAction="Query"
                                            FieldLabel="Route"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            TypeAhead="true"
                                            PageSize="25"
                                            MinChars="0">
                                            <ListConfig LoadingText="Searching..." ID="ListComboRoute">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item">
							                               {ValueTH}
						                                </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store ID="StoreRoute" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="">
                                                            <ActionMethods Read="POST" />
                                                            <Reader>
                                                                <%--   <ext:JsonReader Root="plants" TotalProperty="total" />--%>
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model3" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Code" />
                                                                <ext:ModelField Name="ValueTH" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <%--<ext:Checkbox ID="txtIsActive" runat="server" FieldLabel="<%$Resources:Langauge, IsActive%>" Name="IsActive" Checked="true" />--%>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server">
                            <Items>

                                <ext:GridPanel ID="gridCustomer" runat="server" Height="220" AutoScroll="true">
                                    <Store>
                                        <ext:Store ID="StoreCustomer" runat="server">
                                            <Model>
                                                <ext:Model ID="Model4" runat="server" IDProperty="ProductOwnerID">
                                                    <Fields>
                                                        <ext:ModelField Name="ProductOwnerID" />
                                                        <ext:ModelField Name="Name" />
                                                        <ext:ModelField Name="Description" />
                                                        <ext:ModelField Name="IsSelect" DefaultValue="true" Type="Boolean" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel6" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Text='<%$ Resource : NUMBER %>' Width="40" Align="Center" />
                                            <ext:Column ID="Column1" runat="server" DataIndex="Name" Text='<%$ Resource : OWNER_NAME %>' Align="Center" Flex="1" />
                                            <ext:Column ID="Column3" runat="server" DataIndex="Description" Text='<%$ Resource : OWNER_NAME_THAI %>' Align="Center" Flex="1" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" CheckOnly="true" />
                                    </SelectionModel>

                                </ext:GridPanel>

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
                                    Icon="Disk" Text='<%$ Resource : SAVE %>' Width="60" Disabled="true" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStoreDetail" Mode="Raw" Value="Ext.encode(#{gridCustomer}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text='<%$ Resource : CLEAR %>' Width="60" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text='<%$ Resource : EXIT %>' Width="60" TabIndex="16">
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


