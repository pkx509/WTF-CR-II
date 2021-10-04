<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllSiteName.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllSiteName" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
        var onKeyUp = function () {
            var me = this,
                v = me.getValue(),
                field;

            if (me.startDateField) {
                field = Ext.getCmp(me.startDateField);
                field.setMaxValue(v);
                me.dateRangeMax = v;
            } else if (me.endDateField) {
                field = Ext.getCmp(me.endDateField);
                field.setMinValue(v);
                me.dateRangeMin = v;
            }

            field.validate();
        };

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>

                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill />
                                <ext:Checkbox ID="ckbIsActive" runat="server" FieldLabel='<%$ Resource : SHOW_ALL %>' LabelWidth="50" Width="100" Name="IsActive" Checked="true">
                                    <DirectEvents>
                                        <Change OnEvent="btnSearch_Event" />
                                    </DirectEvents>
                                </ext:Checkbox>
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="<%$ Resource : SEARCH_WORDING %>" Name="txtSearch" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource: SEARCH %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" RemoteSort="true" RemotePaging="true" AutoLoad="true" PageSize="20">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="SiteID">
                                    <Fields>
                                        <ext:ModelField Name="SiteID" />
                                        <ext:ModelField Name="SiteName" />
                                        <ext:ModelField Name="SiteAdress" />
                                        <ext:ModelField Name="SiteRoad" />
                                        <ext:ModelField Name="SiteSubDistrict_Id" />
                                        <ext:ModelField Name="SubDistrictName" />
                                        <ext:ModelField Name="SiteDistrict_Id" />
                                        <ext:ModelField Name="DistrictName" />
                                        <ext:ModelField Name="SiteProvince_Id" />
                                        <ext:ModelField Name="ProvinceName" />
                                        <ext:ModelField Name="SitePostCode" />
                                        <ext:ModelField Name="SiteCountry" />
                                        <ext:ModelField Name="SiteTel" />
                                        <ext:ModelField Name="SiteFax" />
                                        <ext:ModelField Name="SiteEmail" />
                                        <ext:ModelField Name="SiteURL" />
                                        <ext:ModelField Name="IsActive" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <%--                            <Sorters>
                                <ext:DataSorter Property="SiteID" Direction="ASC" />
                            </Sorters>--%>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Delete" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.SiteID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="Edit" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.SiteID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text='<%$ Resource : NUMBER %>' Width="60" Align="Center" />
                            <ext:Column ID="colSite_Name" runat="server" DataIndex="SiteName" Text='<%$ Resource : SITE_NAME %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column1" runat="server" DataIndex="SiteAdress" Text='<%$ Resource : ADDRESS %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column2" runat="server" DataIndex="SiteRoad" Text='<%$ Resource : ROAD %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column3" runat="server" DataIndex="SubDistrictName" Text='<%$ Resource : SUBDISTRICT %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column4" runat="server" DataIndex="DistrictName" Text='<%$ Resource : DISTRICT %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column5" runat="server" DataIndex="ProvinceName" Text='<%$ Resource : PROVINCE %>' Align="Left" Flex="1" />
                            <ext:Column ID="Column6" runat="server" DataIndex="SitePostCode" Text='<%$ Resource : ZIPCODE %>' Align="Left" Flex="1" />
                            <ext:Column ID="colSite_Tel" runat="server" DataIndex="SiteTel" Text='<%$ Resource : TELNO %>' Align="Left" Flex="1" />
                            <ext:Column ID="colSite_Fax" runat="server" DataIndex="SiteFax" Text='<%$ Resource : FAXNO %>' Align="Left" Flex="1" />
                            <ext:CheckColumn ID="colIsActive" DataIndex="IsActive" Text='<%$ Resource : ACTIVE %>' runat="server" Align="Center" Width="100" />
                        </Columns>
                    </ColumnModel>
                    <DirectEvents>
                        <%-- <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data.Site_Code" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>--%>
                    </DirectEvents>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text='<%$ Resource : PAGESIZE %>' />
                                <ext:ToolbarSpacer ID="TbarSpacer" runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="20" />
                                    </SelectedItems>
                                    <Listeners>
                                        <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="Loading" LoadingUseMsg="false" />
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
