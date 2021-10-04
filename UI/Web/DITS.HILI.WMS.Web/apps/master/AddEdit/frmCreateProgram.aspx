<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreateProgram.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmCreateProgram" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <ext:XScript runat="server">
        <script>



            var validateSave = function () {
                var plugin = this.editingPlugin;

                plugin.completeEdit();
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
                    <FieldDefaults LabelAlign="Right" LabelWidth="90" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                <ext:TextField runat="server" ID="txtProgram_Code" FieldLabel='<%$ Resource : PROGRAM_CODE %>' TabIndex="1" LabelWidth="130" ReadOnly="true" />
                                <ext:TextField runat="server" ID="txtType" FieldLabel='<%$ Resource : PROGRAM_TYPE %>' TabIndex="1" LabelWidth="130" ReadOnly="true" />
                                <ext:TextField runat="server" ID="txtUrl" FieldLabel='<%$ Resource : URL %>' TabIndex="1" LabelWidth="130" />
                                <ext:SelectBox
                                    ID="cmbModule_Code" runat="server" DisplayField="Description" ValueField="ProgramID" FieldLabel='<%$ Resource : MODULE %>' LabelWidth="130">
                                    <Store>
                                        <ext:Store runat="server" ID="StoreModule">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ProgramID" />
                                                        <ext:ModelField Name="Description" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:SelectBox>


                                <ext:NumberField runat="server" ID="txtSequence" FieldLabel='<%$ Resource : sequence %>' TabIndex="4" LabelWidth="130" />
                                <ext:Container runat="server" Layout="ColumnLayout">
                                    <Items>
                                        <ext:Checkbox runat="server" ID="txtIsActive" FieldLabel='<%$ Resource : ACTIVE %>' Name="IsActive" Checked="true" LabelWidth="130" />

                                    </Items>
                                </ext:Container>



                                <ext:GridPanel ID="grdDataList" runat="server" Region="Center">
                                    <Store>
                                        <ext:Store ID="StoreOfDataList" runat="server" AutoLoad="false">
                                            <Model>
                                                <ext:Model ID="Model" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ProgramValueID" />
                                                        <ext:ModelField Name="ProgramID" />
                                                        <ext:ModelField Name="LanguageCode" />
                                                        <ext:ModelField Name="Value" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <ColumnModel ID="ColumnModelDriver" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="60" />
                                            <ext:Column runat="server" DataIndex="LanguageCode" Text="<% $Resource : LANGUAGE %>" Flex="1" />
                                            <ext:Column runat="server" DataIndex="Value" Text='<%$ Resource : DESCRIPTION %>' Flex="1">
                                                <Editor>
                                                    <ext:TextField runat="server" ID="txtValue" Align="Center" />
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>

                                    <Plugins>
                                        <ext:RowEditing runat="server" ClicksToMoveEditor="1" AutoCancel="false" SaveHandler="validateSave" ErrorSummary="false">
                                        </ext:RowEditing>
                                    </Plugins>

                                    <View>
                                        <ext:GridView runat="server" LoadMask="true" LoadingUseMsg="true" LoadingText="<%$ Resource : LOADING %>" />
                                    </View>
                                </ext:GridPanel>
                            </Items>
                        </ext:Container>


                    </Items>

                    <Listeners>
                        <%-- <ValidityChange Handler="#{btnSave}.setDisabled(!valid); " />--%>
                    </Listeners>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Button ID="btnSave" runat="server"
                                    Icon="Disk" Text="Save" Width="60" TabIndex="15">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click"
                                            Before="#{btnSave}.setDisabled(true);"
                                            Complete="#{btnSave}.setDisabled(false);"
                                            Buffer="350">

                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="Saving ..." MinDelay="100" />
                                        </Click>
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
