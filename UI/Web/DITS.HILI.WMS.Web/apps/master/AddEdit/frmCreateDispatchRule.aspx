<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateDispatchRule.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateDispatchRule" %>

<%@ Register Src="~/Modules/Opt/_Share/ucProductSelect.ascx" TagPrefix="uc1" TagName="ucProductSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />
    <script src="~/resources/js/JScript.Common.js"></script>


    <ext:XScript runat="server">
        <script>

            var popupProduct = function () {
                App.direct.GetProduct('');
            };

            var validateDefault = function () {

                if (App.chkIsDefault.checked) {

                    Ext.MessageBox.show({
                        title: 'WARNING',
                        msg: 'trying to change default rule?',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (button) {
                            if (button == 'ok') {
                                App.direct.UpdateDispatchRule();
                            }
                        }
                    });
                }
                else {
                    App.direct.UpdateDispatchRule();
                }

            };

            var getProduct = function () {

                var product_code = App.txtProductCode.getValue();

                App.hidAddProduct_System_Code.reset();
                App.txtAddProduct_Name_Full.reset();

                App.direct.GetProduct(product_code);
            };

         </script>
    </ext:XScript>

    <style>
        div#ListcmbProduct {
            border-top-width: 1 !important;
            width: 300px !important;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" Region="Center"
                    BodyPadding="10" Flex="1" Layout="AnchorLayout">

                    <FieldDefaults LabelAlign="Right" LabelWidth="110" InputWidth="150" />

                    <Items>
                        <ext:Container Layout="AnchorLayout" runat="server">
                            <Items>

                                <ext:FieldSet runat="server" Layout="AnchorLayout" AutoScroll="false">
                                    <Items>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>

                                                <ext:TextField ReadOnly="true"
                                                    ID="txtRule_Code"
                                                    runat="server"
                                                    MaxLength="10"
                                                    FieldLabel='<%$ Resource : RULE_CODE %>' />

                                                <ext:TextField runat="server"
                                                    ID="txtRuleName"
                                                    MaxLength="50"
                                                    FieldLabel="Name"
                                                    AllowBlank="false"
                                                    EnforceMaxLength="true" />

                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 10 0">
                                            <Items>
                                                <ext:ComboBox runat="server"
                                                    ID="cmbShippingTo"
                                                    Flex="1"
                                                    MinChars="0"
                                                    AutoShow="false"
                                                    TypeAhead="false"
                                                    QueryMode="Remote"
                                                    FieldLabel="Ship to"
                                                    ValueField='<%$ Resource : SHIP_TO_CODE %>'
                                                    DisplayField="Name_TH"
                                                    TriggerAction="Query"
                                                    AllowOnlyWhitespace="false"
                                                    EmptyText='<%$ Resource : PLEASE_SELECT %>'

                                                    <ListConfig LoadingText="Searching..." ID="ListcmbShippingTo">
                                                        <ItemTpl runat="server">
                                                            <Html>
                                                                <div class="search-item">
                                                                {Shippto_Code} : {Name_TH}						                                
                                                            </Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                    <Store>
                                                        <ext:Store runat="server" ID="StoreShipTo" AutoLoad="true">
                                                            <Proxy>
                                                                <ext:AjaxProxy Url="">
                                                                    <ActionMethods Read="POST" />
                                                                    <Reader>
                                                                        <ext:JsonReader Root="plants" TotalProperty="total" />
                                                                    </Reader>
                                                                </ext:AjaxProxy>
                                                            </Proxy>
                                                            <Model>
                                                                <ext:Model runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="Shippto_Code" />
                                                                        <ext:ModelField Name="Name_TH" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>

                                            </Items>
                                        </ext:FieldContainer>

                                    </Items>
                                </ext:FieldSet>

                                <ext:FieldSet runat="server" Layout="AnchorLayout" AutoScroll="false">
                                    <Items>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtLotNo" FieldLabel="Lot No" MaxLength="20" EnforceMaxLength="true" />
                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 10 0">

                                            <FieldDefaults InputWidth="60" LabelWidth="110" />
                                            <Items>
                                                <ext:NumberField runat="server"
                                                    ID="nbLotAging"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    AllowBlank="false"
                                                    FieldLabel='<%$ Resource : LOT_AGING %>'
                                                    IndicatorText="days" />

                                                <ext:NumberField runat="server"
                                                    ID="nbLotDuration"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    FieldLabel='<%$ Resource : DURATION_NOT_OVER %>'
                                                    IndicatorText="days" />

                                                <ext:NumberField runat="server"
                                                    ID="nbLotLessThan"
                                                    MinValue="1"
                                                    EmptyText="0"
                                                    FieldLabel='<%$ Resource : NO_MORE_THAN %>'
                                                    IndicatorText="lots" />

                                            </Items>
                                        </ext:FieldContainer>

                                    </Items>
                                </ext:FieldSet>


                                <ext:FieldSet runat="server" Layout="AnchorLayout" AutoScroll="false">
                                    <Items>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                            <Items>

                                                <ext:TextField runat="server" ID="txtProductCode" FieldLabel='<%$ Resource : PRODUCT_CODE %>' SelectOnFocus="true">
                                                    <Listeners>
                                                        <SpecialKey Handler=" if(e.getKey() == 13){ getProduct(); }" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Button runat="server" Text="..." Margins="0 0 0 5" ID="btnProductSelect">
                                                    <Listeners>
                                                        <Click Fn="popupProduct" />
                                                    </Listeners>
                                                </ext:Button>

                                                <ext:Hidden runat="server" ID="hidAddProduct_System_Code" />
                                                <ext:Hidden runat="server" ID="hidAddUomID" />

                                            </Items>
                                        </ext:FieldContainer>

                                        <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 10 0">

                                            <FieldDefaults InputWidth="300" />

                                            <Items>
                                                <ext:TextField runat="server" ID="txtAddProduct_Name_Full" FieldLabel='<%$ Resource : PRODUCT_NAME %>' ReadOnly="true" />
                                            </Items>
                                        </ext:FieldContainer>

                                    </Items>
                                </ext:FieldSet>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" MarginSpec="10 0 0 0">
                                    <Items>

                                        <ext:Checkbox ID="chkIsActive" runat="server" FieldLabel='<%$ Resource : ACTIVE %>' Name="IsActive" Checked="true" />
                                        <ext:Checkbox ID="chkIsDefault" runat="server" FieldLabel='<%$ Resource : ISDEFAULT %>' />

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
                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text='<%$ Resource : SAVE %>' Width="60" Disabled="true">
                                    <Listeners>
                                        <Click Fn="validateDefault" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text='<%$ Resource : CLEAR %>' Width="60">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text='<%$ Resource : EXIT %>' Width="60">
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

        <uc1:ucProductSelect runat="server" ID="ucProductSelect" />

    </form>
</body>
</html>
