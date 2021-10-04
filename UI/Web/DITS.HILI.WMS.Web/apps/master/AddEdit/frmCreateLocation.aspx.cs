using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateLocation : BaseUIPage
    {
        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";
        private bool isFloor = true;
        private bool isRow = true;
        private bool isColumn = true;
        private bool isLevel = true;

        //private void getWarehouse()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("Method", "Warehouse");
        //    StoreWarehouseName.AutoCompleteProxy(AppDataService, param);
        //}
        //private void getZone()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("Method", "Zone");
        //    StoreZone.AutoCompleteProxy(AppDataService, param);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                //if (IsPostBack)
                //    return;
                btnLocFormat.Visible = true;
                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    getDataLocation(code);
                }
                else
                {
                    getLocationType();
                    txtLocation_Format.Text = "W-Z-C-F-L";
                    txtLocFormat.Text = "W-Z-C-F-L";
                    ToolbarHead.Visible = false;
                    EnableLocationSetting("W-Z-C-F-L");
                    return;
                }

                btnAdd.Visible = false;
                //this.grdVisualgrid.Visible = false;

                ToolbarHead.Visible = true;
                btnEditSave.Visible = true;
                btnEditClear.Visible = true;
                btnEditExit.Visible = true;

                FormPanelDetail.Height = 230;
                txtLocFormat.Visible = false;
                btnLocFormat.Visible = false;

                cmbZone.ReadOnly = true;
                txtRow.ReadOnly = true;
                txtFloor.ReadOnly = true;
                txtnRow.ReadOnly = true;
                txtnFloor.ReadOnly = true;
                txtCol.ReadOnly = true;
                txtStartCol.ReadOnly = true;

                grdDataList.Visible = false;
            }
        }
        private async void getDataLocation(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetLocationByID(id).Result;
                LocationModel _data = data.Get<LocationModel>();
                if (_data == null)
                {
                    return;
                }

                txtZoneID.Text = _data.ZoneID.ToString();

                txtLocation_No.Text = _data.Code.ToString();
                txtRow.Text = _data.RowNo.ToString();
                txtFloor.Text = _data.LevelNo.ToString();
                txtStartCol.Text = _data.ColumnNo.ToString();
                txtCol.Text = "1";
                txtnRow.Text = "1";
                txtnFloor.Text = "1";
                txtnPalletCap.Text = _data.PalletCapacity.ToString();

                btnAdd.Visible = false;
                grdVisualgrid.Visible = false;

                ToolbarHead.Visible = true;
                btnEditSave.Visible = true;
                btnEditClear.Visible = true;
                btnEditExit.Visible = true;

                FormPanelDetail.Height = 230;
                txtLocFormat.Visible = false;
                btnLocFormat.Visible = false;

                cmbWarehouseName.ReadOnly = true;

                cmbZone.ReadOnly = true;
                txtRow.ReadOnly = true;
                txtFloor.ReadOnly = true;
                txtnRow.ReadOnly = true;
                //this.txtnPalletCap.ReadOnly = true;
                txtnFloor.ReadOnly = true;
                txtCol.ReadOnly = true;
                txtStartCol.ReadOnly = true;

                grdDataList.Visible = false;

                cmbWarehouseName.SetAutoCompleteValue(new List<Warehouse>
                       {
                               new Warehouse
                        {
                            WarehouseID = _data.WarehouseID,
                            Code = _data.WarehouseCode,
                            Name = _data.WarehouseName
                        }
                       }
                 );

                cmbZone.SetAutoCompleteValue(new List<Zone>
                        {
                           new Zone
                            {
                                ZoneTypeID = _data.ZoneID,
                                Code = _data.ZoneCode,
                                Name = _data.ZoneName,
                                ShortName = _data.ZoneShortName
                            }
                        }
                 );

                cmbLocationType.SetValue(_data.LocationType);

                txtWHShortName.Text = _data.WarehouseShortName;
                txtZoneShortName.Text = _data.ZoneShortName;
                txtCapacity.Text = _data.SizeCapacity.ToString();
                txtHeight.Text = _data.Height.ToString();
                txtLength.Text = _data.Length.ToString();
                txtWidth.Text = _data.Width.ToString();
                txtWeight.Text = _data.Weight.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        private void EnableLocationSetting(string format)
        {

            isFloor = false;
            txtRow.ReadOnly = true;
            txtnRow.ReadOnly = true;
            txtnPalletCap.ReadOnly = true;
            txtRow.Text = "1";
            txtnRow.Text = "1";
            txtnPalletCap.Text = "1";


            isRow = false;
            txtRow.ReadOnly = true;
            txtnRow.ReadOnly = true;
            txtnPalletCap.ReadOnly = true;
            txtRow.Text = "1";
            txtnRow.Text = "1";
            txtnPalletCap.Text = "1";

            isColumn = false;
            txtStartCol.ReadOnly = true;
            txtCol.ReadOnly = true;
            txtStartCol.Text = "1";
            txtCol.Text = "1";

            isLevel = false;
            txtFloor.ReadOnly = true;
            txtnFloor.ReadOnly = true;
            txtFloor.Text = "1";
            txtnFloor.Text = "1";

            foreach (char txt in format)
            {
                switch (txt)
                {
                    case 'F':
                        isFloor = true;
                        isRow = true;
                        txtRow.ReadOnly = false;
                        txtnRow.ReadOnly = false;
                        txtRow.MaxLength = 1;
                        txtnRow.MaxLength = 2;
                        txtnPalletCap.ReadOnly = false;
                        break;
                    case 'R':
                        isRow = true;
                        txtnPalletCap.ReadOnly = false;
                        txtRow.ReadOnly = false;
                        txtnRow.ReadOnly = false;
                        txtRow.MaxLength = 2;
                        txtnRow.MaxLength = 2;
                        break;
                    case 'L':
                        isLevel = true;
                        txtnPalletCap.ReadOnly = false;
                        txtFloor.ReadOnly = false;
                        txtnFloor.ReadOnly = false;
                        break;
                    case 'C':
                        isColumn = true;
                        txtnPalletCap.ReadOnly = false;
                        txtStartCol.ReadOnly = false;
                        txtCol.ReadOnly = false;
                        break;
                }
            }

            isFloorFlag.Text = isFloor ? "1" : "0";
            isRowFlag.Text = isRow ? "1" : "0";
            isColumnFlag.Text = isColumn ? "1" : "0";
            isLevelFlag.Text = isLevel ? "1" : "0";

        }
        protected string Generate_Location_No(int row, int col, int lev)
        {
            string LocationFormat = txtLocFormat.Text;

            string frontString = "";

            int index = 0;

            foreach (char txt in LocationFormat)
            {
                switch (txt)
                {
                    case 'Z':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        LocationFormat = frontString + txtZoneShortName.Text + LocationFormat;
                        if (txtZoneShortName.Text.Length > 1)
                        {
                            index++;
                        }

                        break;
                    case 'W':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        LocationFormat = frontString + txtWHShortName.Text + LocationFormat;
                        if (txtWHShortName.Text.Length > 1)
                        {
                            index++;
                        }

                        break;
                    case 'R':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        //LocationFormat = frontString + Convert.ToInt32(this.txtRow.Text).ToString("00") + LocationFormat;
                        LocationFormat = frontString + row.ToString("00") + LocationFormat;
                        row++;
                        index++;
                        break;
                    case 'F':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        //LocationFormat = frontString + Convert.ToInt32(this.txtFloor.Text).ToString("00") + LocationFormat;
                        //LocationFormat = frontString + "F" + row.ToString("0") + LocationFormat;
                        LocationFormat = frontString + row.ToString("00") + LocationFormat;
                        row++;
                        index++;
                        break;
                    case 'L':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        //LocationFormat = frontString + Convert.ToInt32(this.txtFloor.Text).ToString("00") + LocationFormat;
                        LocationFormat = frontString + "L" + lev.ToString("0") + LocationFormat;
                        lev++;
                        index++;
                        break;
                    case 'C':
                        frontString = LocationFormat.Substring(0, index);
                        LocationFormat = LocationFormat.Substring(index + 1, LocationFormat.Length - (index + 1));
                        LocationFormat = frontString + col.ToString("00") + LocationFormat;
                        col++;
                        index++;
                        break;
                }
                index++;
            }

            return LocationFormat;
        }

        protected void cmbWarehouseName_Change(object sender, DirectEventArgs e)
        {
            if (Request.QueryString["oDataKeyId"] == "new")
            {
                cmbZone.Clear();
                Dictionary<string, object> param = new Dictionary<string, object>
                {
                    { "Method", "Zone" },
                    { "WhKey", cmbWarehouseName.SelectedItem.Value }
                };
                StoreZone.AutoCompleteProxy(AppDataService, param);
            }
        }

        protected async void getLocationType()
        {
            try
            {
                cmbLocationType.Items.Clear();
                Array values = Enum.GetValues(typeof(LocationTypeEnum));

                List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>(values.Length);

                foreach (object i in values)
                {
                    Ext.Net.ListItem l = new Ext.Net.ListItem
                    {
                        Text = Enum.GetName(typeof(LocationTypeEnum), i),
                        Value = i.ToString()
                    };
                    cmbLocationType.Items.Add(l);
                }

                //this.cmbLocationType.Items.Add(new Ext.Net.ListItem { Value = "0", Text = "- Select All -" });
                cmbLocationType.SelectedItem.Index = 0;
                //this.PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {

            bool res = Validate_Add();

            if (!res)
            {
                return;
            }

            #region [ Generate Location No ]
            List<LocationModel> listItem = new List<LocationModel>();
            string Location_Chk = "";

            int sRow = Convert.ToInt32(txtRow.Text);
            int sCol = Convert.ToInt32(txtStartCol.Text);
            int sLev = Convert.ToInt32(txtFloor.Text);

            int nRow = sRow + Convert.ToInt32(txtnRow.Text);
            int nCol = sCol + Convert.ToInt32(txtCol.Text);
            int nLev = sLev + Convert.ToInt32(txtnFloor.Text);

            if (((nRow - sRow) * (nCol - sCol) * (nLev - sLev)) > 2000)
            {
                MessageBoxExt.ShowError("Can't Generate Location Over 2,000/Click.");
                return;
            }

            for (int i = sRow; i < nRow; i++)
            {

                for (int j = sCol; j < nCol; j++)
                {

                    for (int k = sLev; k < nLev; k++)
                    {

                        LocationModel itemL = new LocationModel
                        {
                            Code = Generate_Location_No(i, j, k)
                        };

                        if (!string.IsNullOrWhiteSpace(Location_Chk))
                        {
                            Location_Chk += ",'" + itemL.Code + "'";
                        }
                        else
                        {
                            Location_Chk += "'" + itemL.Code + "'";
                        }

                        itemL.ZoneID = new Guid(cmbZone.SelectedItem.Value);
                        itemL.ZoneShortName = txtZoneShortName.Text;
                        itemL.WarehouseShortName = txtWHShortName.Text;
                        LocationTypeEnum LT = (LocationTypeEnum)Enum.Parse(typeof(LocationTypeEnum), cmbLocationType.SelectedItem.Text);
                        itemL.LocationType = LT;
                        itemL.RowNo = i;
                        itemL.LevelNo = k;
                        itemL.ColumnNo = j;
                        itemL.PalletCapacity = txtnPalletCap.Text.ToInt();
                        itemL.SizeCapacity = txtCapacity.Text.ToInt();
                        itemL.Weight = txtWeight.Text == "" ? 0 : Convert.ToDecimal(txtWeight.Text);
                        itemL.Width = txtWidth.Text == "" ? 0 : Convert.ToDecimal(txtWidth.Text);
                        itemL.Length = txtLength.Text == "" ? 0 : Convert.ToDecimal(txtLength.Text);
                        itemL.Height = txtHeight.Text == "" ? 0 : Convert.ToDecimal(txtHeight.Text);
                        listItem.Add(itemL);
                    }
                }
            }

            #endregion [ Generate Location No ]

            #region [ Validate on Service ]

            ApiResponseMessage CheckLocation = ClientService.Master.WarehouseClient.CheckLocation(listItem).Result;
            ApiResponseMessage _data = CheckLocation.Get<ApiResponseMessage>();

            if (_data.ResponseCode == "0")
            {
                if (listItem.Count() > 0)
                {
                    btnSave.Enable();
                }
                else
                {
                    btnSave.Disable();
                }
                StoreOfDataList.DataSource = listItem;
                StoreOfDataList.DataBind();
            }
            else
            {
                MessageBoxExt.ShowError(GetMessage(_data.ResponseCode).MessageValue + " " + "[" + _data.text + "]");
            }

            #endregion [ Validate on Service ]
        }

        protected void btnLocFormat_Click(object sender, DirectEventArgs e)
        {
            WindowShow.Show(this, "Edit Location Format", "",
                                "frmCreateLocationFormat.aspx?IsPopup=1", Icon.CheckError, 200, 300);
        }
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                Location createModel = null;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (!string.IsNullOrWhiteSpace(txtLocation_Format.Text))
                {

                    string gridJson = e.ExtraParams["ParamStorePages"];
                    // Array of Dictionaries
                    List<Location> gridData = JSON.Deserialize<List<Location>>(gridJson);

                    foreach (Location iData in gridData)
                    {
                        iData.ZoneID = new Guid(cmbZone.SelectedItem.Value);
                        iData.LocationType = (LocationTypeEnum)LocationType(cmbLocationType.SelectedItem.Text);
                    }

                    datasave = ClientService.Master.WarehouseClient.AddLocation(gridData).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    createModel = new Location();
                    string id = Request.QueryString["oDataKeyId"];
                    Guid KeyValue = new Guid(id);

                    createModel.LocationID = KeyValue;
                    createModel.Code = txtLocation_No.Text;
                    createModel.ZoneID = new Guid(txtZoneID.Text);
                    createModel.RowNo = txtRow.Text.ToInt();
                    createModel.ColumnNo = txtStartCol.Text.ToInt();
                    createModel.LevelNo = txtFloor.Text.ToInt();
                    createModel.PalletCapacity = txtnPalletCap.Text == "" ? 0 : Convert.ToInt32(txtnPalletCap.Text);
                    createModel.Weight = Convert.ToDecimal(txtWeight.Text);
                    createModel.Width = Convert.ToDecimal(txtWidth.Text);
                    createModel.Length = Convert.ToDecimal(txtLength.Text);
                    createModel.Height = Convert.ToDecimal(txtHeight.Text);
                    createModel.SizeCapacity = txtCapacity.Text == "" ? 0 : Convert.ToInt32(txtCapacity.Text);
                    createModel.LocationType = (LocationTypeEnum)LocationType(cmbLocationType.SelectedItem.Value);
                    datasave = ClientService.Master.WarehouseClient.ModifyLocation(createModel).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

                X.Call("parent.App.direct.Reload");
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["oDataKeyId"] == "new")
            {
                //this.txtJobNo.Text = "new";
                txtRow.Clear();
                txtStartCol.Clear();
                txtnRow.Clear();
                txtCol.Clear();
                txtnPalletCap.Clear();
                txtFloor.Clear();
                txtnFloor.Clear();
                txtWidth.Clear();
                txtLength.Clear();
                txtHeight.Clear();
                txtWeight.Clear();
                txtCapacity.Clear();
                StoreOfDataList.RemoveAll();
                cmbWarehouseName.Reset();
                cmbZone.Reset();
                btnEditSave.Disable();
                btnSave.Disable();
            }
            else
            {
                getDataLocation(Request.QueryString["oDataKeyId"]);
                txtWeight.Reset();
                txtWidth.Reset();
                txtLength.Reset();
                txtHeight.Reset();
                txtCapacity.Clear();

                btnSave.Disable();
                btnEditSave.Disable();
            }
        }

        protected bool Validate_Add()
        {
            bool res = true;

            if (isFloorFlag.Text == "1")
            {
                if (cmbZone.SelectedItem == null)
                {
                    MessageBoxExt.ShowError("Please Select Zone.");
                    res = false;
                }

            }

            if (isRowFlag.Text == "1")
            {
                if (string.IsNullOrWhiteSpace(txtnRow.Text))
                {
                    MessageBoxExt.ShowError("Please Input Row Size.");
                    res = false;
                }

                if (string.IsNullOrWhiteSpace(txtRow.Text))
                {
                    MessageBoxExt.ShowError("Please Input Start Row.");
                    res = false;
                }

                if ((((Convert.ToDecimal(txtnRow.Text) + Convert.ToDecimal(txtRow.Text)) - 1) > 9) && (isFloorFlag.Text == "1"))
                {
                    MessageBoxExt.ShowError("Row can't more than 9 on Floor Location.");
                    res = false;
                }
            }

            if (isLevelFlag.Text == "1")
            {
                if (string.IsNullOrWhiteSpace(txtnFloor.Text))
                {
                    MessageBoxExt.ShowError("Please Input Level Size.");
                    res = false;
                }

                if (string.IsNullOrWhiteSpace(txtFloor.Text))
                {
                    MessageBoxExt.ShowError("Please Input Start Level.");
                    res = false;
                }

                if (((Convert.ToDecimal(txtnFloor.Text) + Convert.ToDecimal(txtFloor.Text)) - 1) > 9)
                {
                    MessageBoxExt.ShowError("Level can't more than 9.");
                    res = false;
                }
            }

            if (isColumnFlag.Text == "1")
            {
                if (string.IsNullOrWhiteSpace(txtCol.Text))
                {
                    MessageBoxExt.ShowError("Please Input Column Size.");
                    res = false;
                }

                if (string.IsNullOrWhiteSpace(txtStartCol.Text))
                {
                    MessageBoxExt.ShowError("Please Input Start Column.");
                    res = false;
                }
            }

            return res;
        }

        protected int LocationType(string Type)
        {
            int a = 0;
            if (Type == LocationTypeEnum.LoadingIN.ToString())
            {
                a = 0;
            }

            if (Type == LocationTypeEnum.Storage.ToString())
            {
                a = 1;
            }

            if (Type == LocationTypeEnum.CrossDock.ToString())
            {
                a = 2;
            }

            if (Type == LocationTypeEnum.Pickface.ToString())
            {
                a = 3;
            }

            if (Type == LocationTypeEnum.UnPack.ToString())
            {
                a = 4;
            }

            if (Type == LocationTypeEnum.Packing.ToString())
            {
                a = 5;
            }

            if (Type == LocationTypeEnum.LoadingOut.ToString())
            {
                a = 6;
            }

            if (Type == LocationTypeEnum.Dummy.ToString())
            {
                a = 7;
            }

            if (Type == LocationTypeEnum.Damage.ToString())
            {
                a = 8;
            }

            if (Type == LocationTypeEnum.Inspection.ToString())
            {
                a = 9;
            }

            return a;
        }
    }

}