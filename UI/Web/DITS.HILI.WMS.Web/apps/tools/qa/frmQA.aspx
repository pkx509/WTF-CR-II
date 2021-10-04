<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="frmQA.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.tools.qa.frmQA1" %>

<%@ Register Src="~/apps/tools/qa/_usercontrol/ucQAConfirm.ascx" TagPrefix="uc1" TagName="ucQAConfirm" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />

    <script type="text/javascript">
        var prepareToolbarEdit = function (grid, toolbar, rowIndex, record) {
            if (record.data.Status != 'Complete' && record.data.Status != 'InProgress') {
                //toolbar.items.getAt(1).setDisabled(true);
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Default" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanelDetail" AutoScroll="true"
                    BodyPadding="3" Region="North" Frame="true" Layout="ColumnLayout" Margins="3 3 0 3">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="QA No." Flex="1" ID="txtQANo" ReadOnly="true" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="txtStartDate" runat="server" FieldLabel="Start Date" Flex="1" TabIndex="8" DataIndex="ActualDate" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Remark" Flex="1" ID="txtRemark" ReadOnly="true" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Document Ref" Flex="1" ID="txtDocumentRef" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="txtCompleteDate" runat="server" FieldLabel="Complete Date" Flex="1" TabIndex="8" DataIndex="CompleteDate" />
                                    </Items>
                                </ext:FieldContainer>

                                <ext:FieldContainer runat="server" Layout="HBoxLayout" LabelWidth="110">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel="Document Status" Flex="1" ID="txtDocumentStatus" TabIndex="1" />
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.33">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField ID="txtQADate" runat="server" FieldLabel="QA Date" Flex="1" TabIndex="8" DataIndex="QADate" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:MultiCombo ID="cmbMutiAssign" runat="server" FieldLabel="Assign" Width="260" TabIndex="13" Flex="1" AllowOnlyWhitespace="false">
                                        </ext:MultiCombo>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:FormPanel>

                <ext:GridPanel ID="GridPutawayDetail" runat="server" Margins="0 0 0 3" Region="Center"
                    Frame="true" SortableColumns="false">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="Add New">
                                            <DirectEvents>
                                                <Click OnEvent="btnAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:FieldContainer>




                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="PutawayDetailStore" runat="server" PageSize="20">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>


                                        <ext:ModelField Name="PutAwayID" />
                                        <ext:ModelField Name="PutAwayJobCode" />
                                        <ext:ModelField Name="MethodName" />
                                        <ext:ModelField Name="PutAwayDate" Type="Date" />
                                        <ext:ModelField Name="FinishDate" Type="Date" />
                                        <ext:ModelField Name="FromLocationID" />
                                        <ext:ModelField Name="SuggestionLocationID" />
                                        <ext:ModelField Name="Status" />
                                        <ext:ModelField Name="FromLocationName" ModelName="FromLocation" ServerMapping="FromLocation.Code" />
                                        <ext:ModelField Name="FromLocation" ModelName="FromLocation" ServerMapping="FromLocation" />
                                        <ext:ModelField Name="SuggestionLocationName" ModelName="SuggestionLocation" ServerMapping="SuggestionLocation.Code" />
                                        <ext:ModelField Name="SuggestionLocation" ModelName="SuggestionLocation" ServerMapping="SuggestionLocation" />
                                        <ext:ModelField Name="SuggestionLocation" />
                                        <ext:ModelField Name="PutAwayJobMatchCollection" />
                                        <ext:ModelField Name="PutAwayConfirmCollection" />

                                        <ext:ModelField Name="AssignJobCollection" />
                                        <ext:ModelField Name="ProductCode" />
                                        <ext:ModelField Name="ProductName" />
                                        <ext:ModelField Name="Product" ModelName="Product" ServerMapping="Product" />
                                        <ext:ModelField Name="Quantity" />
                                        <ext:ModelField Name="ConfirmQuantity" />
                                        <ext:ModelField Name="StockUOMName" ModelName="PutAwayDetailCollection" Mapping="PutAwayDetailCollection.StockUOMName" />
                                        <%--                                       <ext:ModelField Name="PutAwayDetailCollection" ModelName="PutAwayDetailCollection" Mapping="PutAwayDetailCollection" />--%>
                                        <ext:ModelField Name="StockUnitName" />


                                        <ext:ModelField Name="Lot" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModelDriver" runat="server">

                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No" Width="60" Align="Center" />
                            <ext:CommandColumn runat="server" ID="CommandColumn1" Sortable="false" Align="Center" Width="30">
                                <Commands>
                                    <ext:CommandFill />
                                    <ext:GridCommand Icon="Accept" ToolTip-Text="Edit QA" CommandName="edit" />
                                    <ext:CommandFill />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataSelect" Value="record.data" Mode="Raw" />
                                            <ext:Parameter Name="oDataPutAwayID" Value="record.data.PutAwayID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                                <PrepareToolbar Fn="prepareToolbarEdit" />
                            </ext:CommandColumn>

                            <ext:Column ID="Column1" runat="server"
                                DataIndex="Result" Text="Result"
                                Width="70" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colProductCode" runat="server"
                                DataIndex="ProductCode" Text="Product Code"
                                Width="100" Align="Center">
                            </ext:Column>
                            <ext:Column ID="colProduct_Name" runat="server" DataIndex="ProductName"
                                Text="Product Name" MinWidth="200" Align="Left">
                            </ext:Column>
                            <ext:Column ID="colLot" runat="server" DataIndex="Lot"
                                Text="LotNo" MaxLength="50" EnforceMaxLength="true"
                                Width="100" Align="Left">
                            </ext:Column>
                            <ext:DateColumn ID="colMFG" runat="server" DataIndex="PutAwayDate" Text="MFG Date"
                                Align="Center" Format="dd/MM/yyyy">
                            </ext:DateColumn>
                            <ext:DateColumn ID="colEXP" runat="server" DataIndex="FinishDate" Text="EXP Date"
                                Align="Center" Format="dd/MM/yyyy">
                            </ext:DateColumn>
                            <ext:Column ID="colQARule" runat="server" DataIndex="QA Rule"
                                Text="QA Rule(%)" MaxLength="50" EnforceMaxLength="true"
                                Width="100" Align="Left">
                            </ext:Column>
                            <ext:NumberColumn ID="colQuanlity" runat="server" DataIndex="Quantity"
                                Text="Quantity" Width="100" Format="#,###" Align="Right">
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="colQAQuanlity" runat="server" DataIndex="QA Quantity"
                                Text="Quantity" Width="100" Format="#,###" Align="Right">
                            </ext:NumberColumn>
                            <ext:NumberColumn ID="colConfirmQTY" runat="server" DataIndex="ConfirmQuantity"
                                Text="Confirm Quantity" Width="100" Format="#,###" Align="Right">
                            </ext:NumberColumn>
                            <ext:Column ID="colStockUnit" runat="server" DataIndex="StockUnitName"
                                Text="Stock Unit" MinWidth="100" Align="Left">
                            </ext:Column>

                            <ext:NumberColumn ID="colRelate" runat="server" DataIndex="Relate"
                                Text="Relate" Width="100" Format="#,###" Align="Right">
                            </ext:NumberColumn>



                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:Toolbar runat="server" ID="toolbarControls">
                            <Items>
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>
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
                                                    <Listeners>
                                                        <Select Handler="#{GridPutawayDetail}.store.pageSize = parseInt(this.getValue(), 10);
                                                                                 #{PagingToolbar1}.moveFirst();" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Container>
                                <ext:ToolbarFill ID="TbarFill" runat="server" />
                                <ext:Container runat="server" Layout="ColumnLayout" ColumnWidth="0.5">
                                    <Items>
                                                    <ext:Button ID="btnCancel" runat="server" Text="Approve" Icon="Accept" Width="80" TabIndex="16">
                                            <DirectEvents>
                                                <Click OnEvent="btnCancel_Click"
                                                    Before="#{btnCancel}.setDisabled(true);"
                                                    Complete="#{btnCancel}.setDisabled(false);"
                                                    Buffer="350">
                                                    <EventMask ShowMask="true" Msg="Cancel ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to save cancel?" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnApprove" runat="server" Text="Approve" Icon="Accept" Width="80" TabIndex="16">
                                            <DirectEvents>
                                                <Click OnEvent="btnApprove_Click"
                                                    Before="#{btnApprove}.setDisabled(true);"
                                                    Complete="#{btnApprove}.setDisabled(false);"
                                                    Buffer="350">
                                                    <EventMask ShowMask="true" Msg="Approve ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to save approve?" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnPrint" runat="server"
                                            Icon="Printer" Text="Print" Width="80" TabIndex="17" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnPrint_Click">
                                                    <EventMask ShowMask="true" Msg="Save ..." MinDelay="100" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnSave" runat="server"
                                            Icon="Disk" Text="Save" Width="80" TabIndex="18" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click">
                                                    <EventMask ShowMask="true" Msg="Save ..." MinDelay="100" />
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to save?" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="Clear" Width="80" TabIndex="19" MarginSpec="0 0 0 5" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnClear_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="Exit" Width="80" TabIndex="20" MarginSpec="0 0 0 5">
                                            <DirectEvents>
                                                <Click OnEvent="btnExit_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>

                </ext:GridPanel>

            </Items>
        </ext:Viewport>
        <uc1:ucQAConfirm runat="server" ID="ucQAConfirm" />
    </form>
</body>
</html>
