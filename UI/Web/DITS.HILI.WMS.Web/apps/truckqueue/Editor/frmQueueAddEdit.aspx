<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQueueAddEdit.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.truckqueue.frmQueueAddEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  
         <link href="~/Scripts/WmsOnline.css" rel="stylesheet" />
    <style type="text/css">
    .my-list .x-combo-list-item {
        overflow-x: scroll;
        text-overflow: normal;
    }
</style>
</head> 
<body>
    <form id="form1" runat="server"> 
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" Layout="AutoLayout">
            <Items>
               <ext:FormPanel runat="server" ID="FormPanelDetail" AutoScroll="false" BodyPadding="3" Region="Center" Frame="true"  Layout="AutoLayout">
                    <FieldDefaults LabelAlign="Right" />
                    <Items>
                        <ext:Container runat="server" Layout="AnchorLayout" Flex="1" DefaultAnchor="100%">
                            <Defaults>
                                <ext:Parameter Name="HideEmptyLabel" Value="false" Mode="Raw" />
                            </Defaults>
                            <Items>
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : DATE %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items> 
                                          <ext:DateField runat="server"
                                            ID="dtRegisDate" 
                                            MaxLength="10"
                                            Width="250"
                                            ReadOnly="true"
                                            EnforceMaxLength="true"
                                            Format="dd/MM/yyyy" />
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUENO %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtQueuNo" AllowBlank="true" Width="250" ReadOnly="true" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUECONTACT %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmdRegisterType"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="QueueRegisterTypeName"
                                            ValueField="QueueRegisterTypeID"
                                            TypeAhead="false"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="false"
                                            ForceSelection="true"
                                            AllowBlank="false"
                                            ListClass="my-list"
                                             LazyInit="false"
                                            Width="250">  
                                             <Store>
                                                <ext:Store ID="StoreRegisterType"  runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=RegisterType">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelRegisterType" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="QueueRegisterTypeID" />
                                                                <ext:ModelField Name="QueueRegisterTypeName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : DRIVINGLICENSE %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server"  ID="txtTruckRegno"  AllowBlank="false" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer> 
                                <%--  <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : PROVINCE %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbProvince"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="Name"
                                            ValueField="Province_Id"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false" 
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            ListClass="my-list"
                                            LazyInit="false"
                                            Width="250"> 
                                             <Store>
                                                <ext:Store ID="StoreProvince" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ProvinceWithoutpage">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelProvince" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Province_Id" />
                                                                <ext:ModelField Name="Name" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer> --%>
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : TRUCK %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbTruckType"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="TypeName"
                                            ValueField="TruckTypeID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                              ListClass="my-list"
                                             LazyInit="false"
                                            Width="250">  
                                            <Store>
                                                <ext:Store ID="StoreTruckType" runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=TruckTypeOnly">
                                                            <ActionMethods Read="GET" />
                                                            <Reader>
                                                                <ext:JsonReader Root="plants" TotalProperty="total" />
                                                            </Reader>
                                                        </ext:AjaxProxy>
                                                    </Proxy>
                                                    <Model>
                                                        <ext:Model ID="ModelTruckType" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="TruckTypeID" />
                                                                <ext:ModelField Name="TypeName" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model> 
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:FieldContainer> 
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUESHIPFROM %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmdShipFrom"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="ShortName"
                                            ValueField="ShipFromId"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Width="250"
                                             LazyInit="false"
                                            ListClass="my-list">  
                                            <Store>
                                                <ext:Store ID="StoreShipFrom" runat="server" AutoLoad="true">
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
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUESHIPTO %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbShipTo"  
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="ShortName"
                                            ValueField="ShipToId"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            Width="250"
                                             LazyInit="false"
                                            ListClass="my-list">  
                                              <Store>
                                                <ext:Store  ID="StoreShipTo"  runat="server" AutoLoad="true">
                                                    <Proxy>
                                                        <ext:AjaxProxy Url="~/Common/DataClients/MsDataHandler.ashx?Method=ShipToWithOutPage">
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
                                 <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : PO_NO %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtPoNo" AllowBlank="true" Width="250" AutoFocus="true"></ext:TextField>
                                    </Items>
                                </ext:FieldContainer>
                               <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : QUEUEDOCK %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbSelectDock"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="QueueDockName"
                                            ValueField="QueueDockID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            ListClass="my-list"
                                            LazyInit="false"
                                            Width="250"> 
                                            <Store>
                                                <ext:Store  ID="StoreQueueDock"   runat="server" AutoLoad="true">
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
                                 <ext:FieldContainer runat="server" ID ="flQueueStatus" FieldLabel='<%$ Resource : QUEUESTATSU %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                         <ext:ComboBox runat="server"
                                            ID="cmbQueueStatus"
                                            EmptyText="<% $Resource : PLEASE_SELECT %>"
                                            DisplayField="QueueStatusName"
                                            ValueField="QueueStatusID"
                                            TypeAhead="true"
                                            MinChars="0"
                                            TriggerAction="All"
                                            QueryMode="Remote"
                                            AutoShow="false"
                                            AllowOnlyWhitespace="true"
                                            AllowBlank="true"
                                            ListClass="my-list"
                                            Width="250"> 
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
                                <ext:FieldContainer runat="server" FieldLabel='<%$ Resource : REMARK %>' Layout="HBoxLayout" LabelWidth="150" LabelAlign="Right">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtRemark" AllowBlank="true" Width="250" AutoFocus="true"></ext:TextField>
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
                                <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="<%$ Resource : SAVE %>" Width="75" Disabled="true" TabIndex="7">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="#{btnSave}.setDisabled(true);" Complete="#{btnSave}.setDisabled(false);" Buffer="350" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnClear" runat="server" Icon="PageWhite" Text="<%$ Resource : CLEAR %>" Width="75" TabIndex="16">
                                    <Listeners>
                                        <Click Handler="#{FormPanelDetail}.reset();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnExit" runat="server" Icon="Cross" Text="<%$ Resource : EXIT %>" Width="75" TabIndex="16">
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