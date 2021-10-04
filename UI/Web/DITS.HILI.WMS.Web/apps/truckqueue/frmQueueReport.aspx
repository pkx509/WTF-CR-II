<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQueueReport.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmQueueReport" %>
<!DOCTYPE html> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="~/Scripts/WmsOnline.css" rel="stylesheet" /> 
   <script type="text/javascript">
        var saveData = function () {
           // GridData.setValue(Ext.encode(grdDataList.getRowsValues({selectedOnly : false})));
        };
    </script>
</head>
<body>
 <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1"  runat="server">
        </ext:ResourceManager>
        <ext:Hidden ID="GridData" runat="server" />
         <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>           
                <ext:FormPanel runat="server"
                    ID="FormPanelDetail"
                    AutoScroll="true"
                    BodyPadding="3"
                    Region="North"
                    Frame="true"
                    Layout="ColumnLayout">
                    <FieldDefaults LabelAlign="Right" LabelWidth="120" />
                    <Items>                        
                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.4">
                            <Items>
                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:DateField runat="server"
                                            ID="dtStartDate"
                                            FieldLabel='<%$ Resource : QUEUEREGDATE %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy"
                                            AllowBlank="false"
                                            Flex="1" />
                                         <ext:DateField runat="server"
                                            ID="dtEndDate"
                                            FieldLabel='<%$ Resource : QUEUEREGDATETO %>'
                                            MaxLength="10"
                                            EnforceMaxLength="true"
                                            AllowBlank="false"
                                            Format="dd/MM/yyyy"
                                            Flex="1" />
                                    </Items>
                                </ext:FieldContainer>                               
                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                          <ext:ComboBox runat="server"
                                            ID="cmbQueueStatus"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            FieldLabel='<%$ Resource : QUEUESTATSU %>'
                                            DisplayField="QueueStatusName"
                                            ValueField="QueueStatusID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Flex="1"> 
                                            <Store>
                                                <ext:Store ID ="storeQueueStatus" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=QueueStatus">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="Model1" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="QueueStatusID" />
                                                                <ext:ModelField Name="QueueStatusName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmdShipFrom"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                             FieldLabel='<%$ Resource : QUEUESHIPFROM %>'
                                            DisplayField="Name"
                                            ValueField="ShipFromId"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Flex="1"
                                            PageSize="20">  
                                            <Store>
                                                <ext:Store ID="StoreShipFrom" runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipFrom">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelShipFrom" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ShipFromId" />
                                                                <ext:ModelField Name="ShortName" />
                                                                <ext:ModelField Name="Name" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>
                                 <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:BoxSplitter runat="server" Flex="1" />
                                        <ext:Button runat="server" ID="btnReset" Text='<%$ Resource : CLEAR %>' Width="100" MarginSpec="0 0 0 5" Icon="Reload">
                                             <DirectEvents>
                                                    <Click OnEvent="btnReset_Click"  Buffer="350" />
                                            </DirectEvents> 
                                        </ext:Button>
                                         <ext:Button runat="server" ID="Button2" Text='<%$ Resource : SEARCH %>' Width="100" MarginSpec="0 0 0 5" Icon="Magnifier">
                                            <Listeners>
                                                <Click Handler="#{PagingToolbar1}.moveFirst();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button runat="server" Text='<%$ Resource : EXPORTEXCEL %>' Width="100" MarginSpec="0 0 0 5" AutoPostBack="true" OnClick="ToExcel" Icon="PageExcel">
                                            
                                        </ext:Button>
                                        <%-- <ext:Button runat="server" ID="btnExport" Text='<%$ Resource : EXPORTEXCEL %>' Width="100" MarginSpec="0 0 0 5" Icon="PageExcel"> 
                                           <DirectEvents>
                                                   <Click OnEvent="btnExport_Click" AutoDataBind="true" 
                                                          FormID="form1" Buffer="300" 
                                                          IsUpload="true" Method="GET"
                                                          Success="Ext.net.DirectMethods.Download();">
                                                        <EventMask ShowMask="true" />
                                                    </Click>
                                            </DirectEvents> 
                                        </ext:Button>--%>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:Container>

                        <ext:Container runat="server" Layout="AnchorLayout" ColumnWidth="0.4">
                            <Items>  
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbShipTo"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            FieldLabel='<%$ Resource : QUEUESHIPTO %>'
                                            DisplayField="Name"
                                            ValueField="ShipToId"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Flex="1"
                                            PageSize="20">  
                                              <Store>
                                                <ext:Store  ID="StoreShipTo"  runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipTo">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelShipTo" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="ShipToId" />
                                                                <ext:ModelField Name="Name" />
                                                                <ext:ModelField Name="ShortName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" Layout="HBoxLayout">
                                    <Items>
                                        <ext:ComboBox runat="server"
                                            ID="cmbSelectDock"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            FieldLabel='<%$ Resource : QUEUEDOCK %>'
                                            DisplayField="QueueDockName"
                                            ValueField="QueueDockID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Flex="1"> 
                                            <Store>
                                                <ext:Store  ID="StoreQueueDock"   runat="server" AutoLoad="false">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=QueueDock">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelQUEUEDOCK" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="QueueDockID" />
                                                                <ext:ModelField Name="QueueDockName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer>

                                <ext:TextField ID="txtSearchKeyword" runat="server"  FieldLabel='<%$ Resource : KEYWORD %>' Name="txtSearchKeyword"  Width="600">
                                            <Listeners>
                                                <SpecialKey Handler="if(e.getKey() == 13){ #{btnSearch}.fireEvent('click');}" />
                                            </Listeners>
                                        </ext:TextField> 
                            </Items>
                        </ext:Container>     
                    </Items>
                </ext:FormPanel>
                <ext:GridPanel ID="grdDataList" runat="server" Margins="0 0 0 0" Region="Center" StripeRows="true" Frame="true">
                         <Store>
                        <ext:Store ID="StoreOfDataList" runat="server" PageSize="20" RemoteSort="false"> 
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields> 
                                        <ext:ModelField Name="QueueId" />
                                        <ext:ModelField Name="QueueNo" />
                                        <ext:ModelField Name="Sequence" /> 
                                        <ext:ModelField Name="EstimateTime" /> 
                                        <ext:ModelField Name="TruckRegNo" /> 
                                        <ext:ModelField Name="TruckRegProvice" /> 
                                        <ext:ModelField Name="PONO" /> 
                                        <ext:ModelField Name="TimeIn" /> 
                                        <ext:ModelField Name="TimeOut" /> 
                                        <ext:ModelField Name="QueueStatus" />
                                        <ext:ModelField Name="QueueStatusID" /> 
                                        <ext:ModelField Name="TruckType" /> 
                                        <ext:ModelField Name="ShipFrom" /> 
                                        <ext:ModelField Name="ShippTo" /> 
                                        <ext:ModelField Name="QueueDock" /> 
                                        <ext:ModelField Name="QueueRegisterType" /> 
                                        <ext:ModelField Name="Remark" /> 
                                        <ext:ModelField Name="IsActive" /> 
                                        <ext:ModelField Name="UsageTime" /> 
                                        <ext:ModelField Name="CreateByName" /> 
                                    </Fields>
                                </ext:Model>
                            </Model> 
                              <Sorters>
                                  <ext:DataSorter Property="TimeIn" Direction="ASC" /> 
                            </Sorters>
                        </ext:Store>
                    </Store> 
                    <ColumnModel ID="ColumnModelDriver" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="<%$ Resource : NUMBER %>" Width="50" Align="Center" />  
                            <ext:Column ID="colQueueDockName" Width="150" DataIndex="QueueNo" runat="server" Text="<%$ Resource : QUEUENO %>" Align="Center"  />
                            <ext:Column ID="colQueueDockDesc" Width="150" DataIndex="TruckRegNo" runat="server" Text="<%$ Resource : DRIVINGLICENSE %>" Align="Center" />
                            <ext:Column ID="colTruckType" Width="150" DataIndex="TruckType" runat="server" Text="<%$ Resource : TRUCK %>" Align="Center" />
                            <ext:Column ID="colQueueStatus" Width="150" DataIndex="QueueStatus" runat="server" Text="<%$ Resource : QUEUESTATSU %>" Align="Center" /> 
                            <ext:Column ID="colShipFrom" Width="150" DataIndex="ShipFrom" runat="server" Text="<%$ Resource : QUEUESHIPFROM %>" Align="Center" /> 
                            <ext:Column ID="colShippTo" Width="150" DataIndex="ShippTo" runat="server" Text="<%$ Resource : QUEUESHIPTO %>" Align="Center" /> 
                            <ext:Column ID="colPONO" Width="150" DataIndex="PONO" runat="server" Text="<%$ Resource : PO_NO %>" Align="Center" /> 
                            <ext:Column ID="colQueueDock" Width="150" DataIndex="QueueDock" runat="server" Text="<%$ Resource : QUEUEDOCK %>" Align="Center" /> 
                            <ext:DateColumn ID="colTimeIn" Groupable="false" Align="Center" Format="dd/MM/yyyy HH:mm" Width="120" DataIndex="TimeIn" runat="server" Text="<%$ Resource : QUEUETIMEIN %>"/> 
                            <ext:DateColumn ID="colTimeOut" Groupable="false" Align="Center" Format="dd/MM/yyyy HH:mm" Width="120" DataIndex="TimeOut" runat="server" Text="<%$ Resource : QUEUETIMEOUT %>"/> 
                            <ext:Column ID="colEstimatTime" Width="150" DataIndex="UsageTime" runat="server" Text="<%$ Resource : QUEUEUSAEGTIME %>" Align="Center" /> 
                            <ext:Column ID="colCreateByName" Width="150" DataIndex="CreateByName" runat="server" Text="<%$ Resource : CREATEBY %>" Align="Center" /> 
                            <ext:Column ID="colRemark" Width="150" DataIndex="Remark" runat="server" Text="<%$ Resource : REMARK %>" Align="Center" /> 
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
                                        <Select Handler="#{grdDataList}.store.pageSize = parseInt(this.getValue(), 10); #{PagingToolbar1}.moveFirst();" /> 
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar> 
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="true" >
                        </ext:GridView>
                    </View>
                </ext:GridPanel> 
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>