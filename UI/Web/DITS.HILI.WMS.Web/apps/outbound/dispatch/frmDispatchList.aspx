<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDispatchList.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.outbound.dispatch.frmDispatchList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <script type="text/javascript">
      //  Ext.Ajax.timeout = 180000; // 1 sec
       // Ext.net.DirectEvent.timeout = 180000; // 1 sec 

        var prepareToolbarDelete = function (grid, toolbar, rowIndex, record) {
            if (record.data.DispatchStatusId > '10') {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };
        var prepareToolbarConfirm = function (grid, toolbar, rowIndex, record) {
            if (record.data.Job_Picking_Code == '') {
                toolbar.items.getAt(1).setDisabled(true);
            }
        };

        var IsBackOrdered = function (value, metadata, record) {
            if (value == true) {
                if (record.data.IsBackOrder)
                    return "<img src='../../../images/bo.png' width='15px'/>";
                else
                    return "<img src='../../../images/tick.png' width='15px'/>";
            }
        };

        var template = '<span style="color:{0};">{1}</span>';

        var renderUrgent = function (value, records) {
            if (value)
                return Ext.String.format(template, "red", "Urgent");
            else
                return "";
        };



    </script>
    <style>
        .x-window-dlg .ext-mb-download {
            background: transparent url(resources/images/download.gif) no-repeat top left;
            height: 46px;
        }

        .x-grid-cell-colTextUrgent .x-grid-cell-inner {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Hidden runat="server" ID="hidIsPopup" />
                <ext:Hidden runat="server" ID="hidByStatus" />
                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <%--    <ext:Label ID="lbTitle" runat="server" Text="Receive List" Icon="Group" />--%>

                                <ext:ToolbarFill />
                                <%--<ext:Button runat="server" ID="btnCheck" Icon="CheckError" Text="ตรวจรับสินค้า">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>--%>
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : DISPATCH_TYPE %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="80">
                                    <Items>
                                        <ext:ComboBox ID="cmbDispatchType" runat="server" TabIndex="1" Editable="true"
                                            DisplayField="FullName" ValueField="DocumentTypeID"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>" AutoFocus="true"
                                            PageSize="25" MinChars="0" AllowBlank="true"
                                            TypeAhead="true" TriggerAction="All" QueryMode="Remote" AutoShow="false"
                                            ForceSelection="true" Width="120">
                                            <ListConfig LoadingText="Searching..." ID="ListComboDispatchType"  MinWidth="250">
                                                <ItemTpl runat="server">
                                                    <Html>
                                                        <div class="search-item">
							                                               {FullName} 
						                                                </div>
                                                    </Html>
                                                </ItemTpl>
                                            </ListConfig>
                                            <Store>
                                                <ext:Store ID="StoreDispatch_Type" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=DispatchTypeWithAll">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model1" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Code" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="DocumentTypeID" />
                                                                <ext:ModelField Name="FullName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDeliveryDate}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" FieldLabel="Delivery Date" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="80">
                                    <Items>
                                        <ext:DateField ID="dtDeliveryDate" runat="server"
                                            TabIndex="3" Format="dd/MM/yyyy"
                                            LabelAlign="Right" AllowBlank="true"  Width="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{dtDispatch_Date_Order}.focus(false, 100);}" />
                                            </Listeners>
                                        </ext:DateField>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : STATUS %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="40">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbStatus"
                                            LabelWidth="40"
                                            LabelAlign="Right"
                                            Width="120"
                                            EmptyText="<%$ Resource : PLEASE_SELECT %>">
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : PONO %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="50">
                                    <Items>
                                        <ext:TextField ID="txtPono" runat="server"  Name="txtPono"  Width="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" FieldLabel="<%$ Resource : ORDER_NO %>" Layout="HBoxLayout" LabelAlign="Right" LabelWidth="50">
                                    <Items>
                                        <ext:TextField ID="txtOrderNo" runat="server"  Name="txtOrderNo"  Width="100">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:FieldContainer>


                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="<%$ Resource : SEARCH %>">
                                    <%--<DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                            <EventMask ShowMask="true" Msg="Searching" MinDelay="300" />
                                        </Click>
                                    </DirectEvents>--%>
                                    <Listeners>
                                        <Click Handler="#{PagingToolbar1}.moveFirst();" Buffer="500" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false" GroupField="Dispatch_Date_Order">
                            <%--GroupField="SubCust_Country"--%>
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DispatchId" />
                                        <ext:ModelField Name="DispatchCode" />
                                        <ext:ModelField Name="IsUrgent" />
                                        <ext:ModelField Name="IsBackOrder" />
                                        <ext:ModelField Name="DocumentId" />
                                        <ext:ModelField Name="DocumentName" />
                                        <ext:ModelField Name="Pono" />
                                        <ext:ModelField Name="OrderNo" />
                                        <ext:ModelField Name="DocumentDate" Type="Date" />
                                        <ext:ModelField Name="DocumentApproveDate" Type="Date" />
                                        <ext:ModelField Name="OrderDate" Type="Date" />
                                        <ext:ModelField Name="TotalDispatchQty" />
                                        <ext:ModelField Name="DispatchStatusId" />
                                        <ext:ModelField Name="DispatchStatusName" />
                                        <ext:ModelField Name="CustomerId" />
                                        <ext:ModelField Name="CustomerName" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="IsUrgent" Direction="DESC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <Features>
                        <ext:GroupingSummary
                            ID="group"
                            ShowSummaryRow="false"
                            runat="server"
                            GroupHeaderTplString="Dispatch Order Date : {groupValue:date('d/m/Y')}  ({rows.length} Item{[values.rows.length > 1 ? 's' : '']})"
                            HideGroupedHeader="true"
                            EnableGroupingMenu="true" />
                    </Features>
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>

                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="Cancel Dispatch" CommandName="Delete" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.DispatchId" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.DispatchStatusId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                        <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>' Title='<%$ MessageTitle :  MSG00003 %>' />
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarDelete" />
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text='<%$ Resource : EDIT %>' CommandName="Edit" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.DispatchId" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.DispatchStatusId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>

                            <ext:CommandColumn runat="server" ID="colConfirm" Sortable="false" Align="Center" Width="30" Hidden="true">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="CheckError" ToolTip-Text="Confirm Dispatch" CommandName="Confirm" />
                                </Commands>

                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.DispatchId" Mode="Raw" />
                                            <ext:Parameter Name="oDataStatus" Value="record.data.DispatchStatusId" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <%--   <PrepareToolbar Fn="prepareToolbarConfirm" />--%>
                            </ext:CommandColumn>

                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server"
                                Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />


                            <ext:Column ID="colTextUrgent" runat="server" DataIndex="IsUrgent"
                                Text="<%$ Resource : URGENT %>" Width="50" Align="Center" Sortable="false">
                                <Renderer Fn="renderUrgent" />
                            </ext:Column>

                            <ext:Column ID="colIsBackOrder" runat="server" Text="<%$ Resource : BACKORDER %>" DataIndex="IsBackOrder" Align="Center" Width="70">
                                <Renderer Fn="IsBackOrdered">
                                </Renderer>
                            </ext:Column>
                            <ext:Column ID="colDispatch_Code" runat="server"
                                DataIndex="DispatchCode" Text="<%$ Resource : DISPATCH_CODE %>" Align="Center">
                            </ext:Column>

                            <ext:Column DataIndex="Pono" runat="server" Text="<%$ Resource : PONO %>" Align="Left" />
                            <ext:Column DataIndex="OrderNo" runat="server" Text="<%$ Resource : ORDER_NO %>" Align="Left" />



                            <ext:Column ID="colDocumentName" runat="server" DataIndex="DocumentName"
                                Text="<%$ Resource : DISPATCH_TYPE %>" Align="Left" Width="150" />

                            <ext:DateColumn ID="colOrderDate" runat="server" Groupable="false"
                                DataIndex="OrderDate" Text="<%$ Resource : DELIVERY_DATE %>"
                                Align="Center" Format="dd/MM/yyyy" Width="120" />




                            <ext:NumberColumn ID="Column4" runat="server" Groupable="false" SummaryType="Sum"
                                DataIndex="TotalDispatchQty" Text="<%$ Resource : TOTALDISPATCHQTY %>" Align="Right" Format="0,000.00"
                                Width="90" Sortable="false" />

                            <ext:Column ID="colDispatchStatusName" runat="server"
                                DataIndex="DispatchStatusName" Text="<%$ Resource : STATUS %>" Align="Left"
                                Width="180" Filterable="false" />

                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg='<%$ Resource : DISPLAYMSG %>'
                            EmptyMsg='<%$ Resource : NODATATODISPLAY %>' PrevText='<%$ Resource : PREV_PAGE %>' NextText='<%$ Resource : NEXT_PAGE %>'
                            FirstText='<%$ Resource : FIRST_PAGE %>' LastText='<%$ Resource : LAST_PAGE %>' RefreshText='<%$ Resource : RELOAD %>'
                            BeforePageText='<%$ Resource : BEFOREPAGE %>'>
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="<%$ Resource : PAGESIZE %>" />
                                <ext:ToolbarSpacer ID="TbarSpacer" runat="server" Width="10" />
                                <ext:ComboBox ID="cmbPageList" runat="server" Width="80" Editable="false">
                                    <Items>
                                        <ext:ListItem Text="10" />
                                        <ext:ListItem Text="20" />
                                        <ext:ListItem Text="50" />
                                        <ext:ListItem Text="100" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Value="10" />
                                    </SelectedItems>
                                    <Listeners>
                                        <Select Handler="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10);
                                                        #{PagingToolbar1}.moveFirst();" /> 
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                    <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="oDataKeyId" Value="record.data.DispatchId" Mode="Raw" />
                                <ext:Parameter Name="oDataStatus" Value="record.data.DispatchStatusId" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" />
                    </View>
                </ext:GridPanel>

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
