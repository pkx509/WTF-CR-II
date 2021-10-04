<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMonthEnd.aspx.cs" Inherits="DITS.HILI.WMS.Web.apps.master.frmMonthEnd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/resources/css/WmsOnline.css" rel="stylesheet" />    
</head>
<body>
        <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
          <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">
            <Items>              
               <ext:GridPanel ID="gridData" runat="server" Region="Center" Frame="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="<%$ Resource : ADD_NEW %>">
                                    <DirectEvents>
                                        <Click OnEvent="btnAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill /> 
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="StoreData" runat="server">
                            <Proxy>
                                <ext:PageProxy DirectFn="App.direct.BindData" />
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="ItemNo" />
                                        <ext:ModelField Name="DateCreated" /> 
                                        <ext:ModelField Name="CutOffDate" /> 
                                        <ext:ModelField Name="Month" />
                                        <ext:ModelField Name="Year" />
                                        <ext:ModelField Name="UserCreated" />
                                        <ext:ModelField Name="UserModified" />
                                        <ext:ModelField Name="DateModified" />
                                        <ext:ModelField Name="ID" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                              <Sorters>
                                  <ext:DataSorter Property="CutOffDate" Direction="DESC" />
                              </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel6" runat="server">
                        <Columns>
                            <ext:CommandColumn runat="server" ID="colDel" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="Delete" ToolTip-Text="<%$ Resource : DELETE %>" CommandName="Delete" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                            <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true"
                                            Message='<%$ Message :  MSG00003 %>'  Title='<%$ MessageTitle :  MSG00003 %>'/>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn>
                            <ext:CommandColumn runat="server" ID="colEdit" Sortable="false" Align="Center" Width="20">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" ToolTip-Text="<%$ Resource : EDIT %>" CommandName="Edit" />
                                </Commands>
                                <DirectEvents>
                                    <Command OnEvent="CommandClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="oDataKeyId" Value="record.data.ID" Mode="Raw" />
                                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </DirectEvents>
                            </ext:CommandColumn> 
                            <ext:RowNumbererColumn runat="server" Text='<%$ Resource : NUMBER %>' Align="Center" Width="75" />
                            <ext:DateColumn runat="server" Format="dd/MM/yyyy HH:mm:ss" DataIndex="DateCreated" Text="<%$ Resource : CREATE_DATE %>" Width="200" />
                            <ext:DateColumn  runat="server" Format="dd/MM/yyyy" DataIndex="CutOffDate" Text="<%$ Resource : MONTHENDDATE %>" Width="200" >
                                <Editor>
                                    <ext:DateField runat="server" Format="dd/MM/yyyy"/>
                                </Editor>
                            </ext:DateColumn>
                            <ext:DateColumn  runat="server" Format="HH:mm" DataIndex="CutOffDate" Text="<%$ Resource : MONTHENDTIME %>" Width="200" >
                                 <Editor>
                                    <ext:TimeField runat="server" Format="HH:mm"/>
                                </Editor>
                            </ext:DateColumn>  
                        </Columns>
                    </ColumnModel> 
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
                                    <DirectEvents>
                                         <Select Before="#{gridData}.store.pageSize = parseInt(this.getValue(), 10);" OnEvent="Store_Refresh" />
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar> 
                    </BottomBar>           
                    <DirectEvents>
                        <CellDblClick OnEvent="gvdDataListCenter_CellDblClick">
                            <ExtraParams>
                                <ext:Parameter Name="DataKeyId" Value="record.data.ID" Mode="Raw" />
                            </ExtraParams>
                        </CellDblClick>
                    </DirectEvents>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" LoadMask="true" LoadingText="<%$ Resource : LOADING %>" LoadingUseMsg="false" />
                    </View>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectModel" runat="server" Mode="Single">
                        </ext:RowSelectionModel>
                    </SelectionModel> 
                </ext:GridPanel>
           </Items>
          </ext:Viewport>
    </form>
 </body>
</html>