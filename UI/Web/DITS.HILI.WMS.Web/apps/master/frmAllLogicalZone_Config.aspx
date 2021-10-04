<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAllLogicalZone_Config.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmAllLogicalZone_Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="AddNew">
                                 <%--   <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>--%>
                                </ext:Button>

                                <ext:ToolbarFill />
                                <ext:RadioGroup runat="server">
                                    <Items>
                                        <ext:Radio runat="server" BoxLabelAlign="After" BoxLabel="ไม่ตรวจ QC" Width="100" />
                                        <ext:Radio runat="server" BoxLabelAlign="After" BoxLabel="QC ผ่าน" Width="80" />
                                        <ext:Radio runat="server" BoxLabelAlign="After" BoxLabel="QC ไม่ผ่าน" Width="80" />
                                    </Items>
                                </ext:RadioGroup>
                                <ext:TextField ID="txtSearch" runat="server" EmptyText="SearchWording" LabelWidth="50" Width="200">
                                    <Listeners>
                                        <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="Search">
                                    <%--<DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>--%>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20">
                            <%--RemoteSort="true" RemotePaging="true" AutoLoad="true">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData">
                                </ext:PageProxy>
                            </Proxy>--%>
                            <Model>
                                <ext:Model ID="Model" runat="server" IDProperty="DriverId">
                                    <Fields>
                                        <ext:ModelField Name="RowNumbererColumn1" />
                                        <ext:ModelField Name="Location_No" />
                                        <ext:ModelField Name="PhysicalZone_Code" />
                                        <ext:ModelField Name="LogicalZone_Code" />
                                        <ext:ModelField Name="Location_Zone" />
                                        <ext:ModelField Name="Location_RowNO" />
                                        <ext:ModelField Name="Location_ColNO" />
                                        <ext:ModelField Name="Location_LevNO" />
                                        <ext:ModelField Name="Location_Capacity" />
                                        <ext:ModelField Name="Location_Weight" />


                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Location_No" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />

                            <ext:Column ID="Column1" runat="server" DataIndex="Location_No" Text="รหัส Zone" Align="Center" />
                            <ext:Column ID="Column2" runat="server" DataIndex="Location_Branch" Text="ผู้เช่าคลังสินค้า" Align="Center" />
                            <ext:Column ID="Column3" runat="server" DataIndex="Location_Bld" Text="ผู้ผลิต" Align="Center" />
                            <ext:Column ID="Column4" runat="server" DataIndex="Location_Floor" Text="รหัสของสินค้า" Align="Center" />
                            <ext:Column ID="Column5" runat="server" DataIndex="Location_RowNO" Text="กลุ่มสินค้าลำดับ 3" Align="Center" />
                            <ext:Column ID="Column6" runat="server" DataIndex="Location_ColNO" Text="รูปทรงของสินค้า" Align="Center" />
                            <ext:Column ID="Column7" runat="server" DataIndex="Location_LevNO" Text="ยี่ห้อของสินค้า" Align="Center" />
                            <ext:Column ID="Column8" runat="server" DataIndex="Location_Capacity" Text="สถานะการ QC" Align="Center" />
                            <ext:Column ID="Column9" runat="server" DataIndex="Location_Weight" Text="สถานะการเสียหายของสินค้า" Align="Center" Width="140" />

                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="DisplayingFromTo"
                            EmptyMsg="NoDataToDisplay" PrevText="Prev&nbsp;Page" NextText="Next&nbsp;Page"
                            FirstText="First&nbsp;Page" LastText="Last&nbsp;Page" RefreshText="Reload"
                            BeforePageText="<center>Page</center>">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="Page size:" />
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
                                    <%--<DirectEvents>
                                        <Select Before="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />
                                    </DirectEvents>--%>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
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
