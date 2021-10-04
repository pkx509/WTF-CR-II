<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUpdateLogicalZone_Group.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.AddEdit.frmUpdateLogicalZone_Group" %>


<%@ Register Src="~/apps/master/AddEdit/_usercontrol/ucProductMultiSelect.ascx" TagPrefix="uc2" TagName="ucProductMultiSelect" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

</head>


<ext:XScript runat="server">
        <script>

            var beforeEditCheck = function (editor, e, eOpts) {
                //if (e.record.data.IsReceive == true) {
                //    e.cancel = true;

                //} else {
                //App.direct.LoadCombo();
                //App.direct.LoadOrderType(e.record.data.OrderType);
                e.cancel = false;
                //}

            };

            var edit = function (editor, e) {

                if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
                    console.log(e.record.data);
                    App.direct.Edit(e.record.data.Special_Rul_Code, e.field, e.originalValue, e.value, e.record.data);
                }
            };

 </script>
</ext:XScript>

<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>

        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:FieldSet runat="server" Layout="AnchorLayout" Region="North" AutoScroll="false">
                    <Items>

                        <ext:FieldSet runat="server" ID="FieldSet1"
                            Layout="ColumnLayout" Border="false" Padding="5" Region="North" Width="600">
                            <FieldDefaults LabelWidth="100" LabelAlign="Right" />

                            <Items>
                                <ext:FieldContainer runat="server" Layout="AnchorLayout" Region="North" ColumnWidth="0.5">
                                    <Items>

                                        <ext:TextField runat="server" ID="txtGroup_Code" FieldLabel="<%$ Resource : LOGICALZONE_GROUP_CODE %>" TabIndex="2" ReadOnly="true" Hidden="true" />

                                        <ext:TextField runat="server" ID="txtGroup_Name" FieldLabel="<%$ Resource : LOGICALZONE_GROUP_NAME %>" TabIndex="3" AllowBlank="false" LabelWidth="150" />

                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="AnchorLayout" Region="North" ColumnWidth="0.5">
                                    <Items>


                                        <ext:ComboBox ID="cmbProductGroupLevel3" runat="server" FieldLabel="<%$ Resource : LOGICALZONE_GROUP_LEVEL3 %>" AllowBlank="false"
                                            DisplayField="Name"
                                            ValueField="ProductGroupLevel3ID"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>"
                                            PageSize="0" SelectOnFocus="true"
                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                            ForceSelection="true" TabIndex="3" LabelWidth="150" >
                                            <Store>
                                                <ext:Store ID="StoreLogicalZone" runat="server" AutoLoad="false">
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
                                                                <ext:ModelField Name="ProductGroupLevel3ID" />
                                                                <ext:ModelField Name="Name" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Container runat="server" Layout="hbox">
                                            <Items>
                                                <ext:Checkbox runat="server" ID="chkTransfer_Flag" LabelWidth="70" FieldLabel="<%$ Resource : TRANSFER %>" Name="Transfer" Checked="true" Hidden="true" />
                                                <ext:Checkbox runat="server" ID="ckhPutAway_Flag" LabelWidth="70" FieldLabel="<%$ Resource : PUTAWAY %>" Name="PutAway" Hidden="true" />
                                                <ext:Checkbox ID="txtIsActive" runat="server" LabelWidth="150" FieldLabel="<%$ Resource : ACTIVE %>" Name="IsActive" LabelAlign="Right" Checked="true" >
                                                    <%--<DirectEvents>
                                                        <Change OnEvent="chkIsActive_Change">
                                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                                            <ExtraParams>
                                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                                            </ExtraParams>
                                                        </Change>
                                                    </DirectEvents>--%>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Container>

                                    </Items>
                                </ext:FieldContainer>

                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:FieldSet>


                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW_PRODUCT %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:ToolbarFill />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="LogicalZoneGroupDetailId" />
                                        <ext:ModelField Name="LogicalZoneGroupLevel3Id" />
                                        <ext:ModelField Name="LogicalZoneGroupLevel3Name" />
                                        <ext:ModelField Name="ProductId" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="ProductId" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="<%$ Resource : DELETE %>" CommandName="Delete" />
                                </Commands>
                                <Listeners>
                                    <Command Handler="#{StoreOfDataList}.remove(record);" />
                                </Listeners>
                                <%--                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.LogicalZoneGroupDetailId" Mode="Raw" />
                                            <ext:Parameter Name="oProductId" Value="record.data.ProductId" Mode="Raw" />
                                            <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                        </ExtraParams>
                                                      <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>'  Title='<%$ MessageTitle :  MSG00003 %>'/>
                                    </Command>
                                </DirectEvents>--%>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="80" Align="Center" />

                            <ext:Column ID="Column4" runat="server" DataIndex="ProductCode" Text="<%$ Resource : PRODUCT_CODE %>" Align="Left" Width="200" />
                            <ext:Column ID="Column5" runat="server" DataIndex="ProductName" Text="<%$ Resource : PRODUCT_NAME %>" Align="Left" Flex="1" />
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text="<%$ Resource : ACTIVE %>" runat="server"
                                Align="Center" Width="100" Editable="true" Hidden="false" />

                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:CellEditing runat="server">
                            <Listeners>
                                <BeforeEdit Fn="beforeEditCheck" />
                                <Edit Fn="edit" />
                            </Listeners>
                        </ext:CellEditing>
                    </Plugins>

                    <SelectionModel>
                        <ext:RowSelectionModel Mode="Single">
                            <Listeners>
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="false" />
                    </View>
                    <BottomBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:ToolbarFill />

                                <ext:Button runat="server" Text="Save" Icon="Disk" ID="btnSaveAll">
                                    <DirectEvents>
                                        <Click OnEvent="btnSaveAll_Click" Buffer="300">
                                            <ExtraParams>
                                                <ext:Parameter Name="ParamStorePages" Mode="Raw" Value="Ext.encode(#{grdDataList}.getRowsValues({selectedOnly : false}))" />
                                            </ExtraParams>
                                            <Confirmation ConfirmRequest="true"
                                                Message='<%$ Message :  MSG00025 %>' Title='<%$ MessageTitle :  MSG00025 %>' />
                                            <EventMask ShowMask="true" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="60" TabIndex="9">
                                    <DirectEvents>
                                        <Click OnEvent="btnClose_Click" Buffer="300">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>


            </Items>
        </ext:Viewport>
        <uc2:ucProductMultiSelect runat="server" ID="ucProductMultiSelect" />
    </form>
</body>
</html>
