using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllEquipmentZone : BaseUIPage
    {
        public static string ProgramCode = "P-0055";
        private readonly string AutoCompleteService = "~/Common/DataClients/MsDataHandler.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack && DynamicMenu.ToolbarControlOperation(ProgramCode, null, null, null, null, null, null, null, null, null, null))
            //{
            //    this.PagingToolbar1.MoveFirst();
            //    LoadCombo();
            //    BindData();
            //}
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            if (!IsPostBack)
            {
                // LoadCombo();
            }


        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = EquipZoneConfigClient.Get(txtSearch.Text, ckbIsActive.Checked, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<EquipZoneConfigModel> data = new List<EquipZoneConfigModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<EquipZoneConfigModel>>();
            }

            return new { data, total };
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        private void LoadCombo()
        {



            //StoreTruckTypeAdd.AutoCompleteProxy(AutoCompleteService, 
            //                                         new string[] { "Methode" },
            //                                         new string[] { "TruckTypeOnly" });



            //StorePhysicalZoneAdd.AutoCompleteProxy(AutoCompleteService,
            //                                         new string[] { "Methode" },
            //                                         new string[] { "Zone" });

            //StorePhysicalZoneAdd.AutoCompleteProxy(AutoCompleteService,
            //                                         new string[] { "Methode" },
            //                                         new string[] { "combophysicalzone" });
            //StorePhysicalZoneAdd.LoadProxy();

            ////..
            //StoreTruckTypeEdit.AutoCompleteProxy(AutoCompleteService,
            //                                                 new string[] { "Methode" },
            //                                                 new string[] { "comboequipment" }
            //                                                 );
            //StoreTruckTypeEdit.LoadProxy();

            //StoreZoneEdit.AutoCompleteProxy(AutoCompleteService,
            //                                         new string[] { "Methode" },
            //                                         new string[] { "combophysicalzone" });
            //StoreZoneEdit.LoadProxy();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            try
            {
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.ToString());
            }
        }

        [DirectMethod(Timeout=180000)]
        public void Edit(string id, string field, string oldValue, string newValue, object gridJson)
        {

            //DataServiceModel dataService = new DataServiceModel();
            EquipZoneConfigModel _search = JSON.Deserialize<EquipZoneConfigModel>(gridJson.ToString());
            EquipZoneConfig _equip = new EquipZoneConfig
            {
                EquipID = new Guid(id),
                EquipName = _search.EquipName,
                Serialnumber = _search.Serialnumber
            };

            if (field == "TruckTypeName")
            {
                _equip.TruckTypeID = new Guid(newValue);
            }
            else
            {
                _equip.TruckTypeID = _search.TruckTypeID;
            }
            if (field == "ZoneName")
            {
                _equip.ZoneID = new Guid(newValue);
            }
            else
            {
                _equip.ZoneID = _search.ZoneID;
            }
            if (field == "IsActive")
            {
                _equip.IsActive = Convert.ToBoolean(newValue);
            }

            bool isSuccess = true;

            Core.Domain.ApiResponseMessage datasave = ClientService.Master.EquipZoneConfigClient.Modify(_equip).Result;

            if (datasave.ResponseCode != "0")
            {
                isSuccess = false;
                MessageBoxExt.ShowError(datasave.ResponseMessage);
            }

            if (isSuccess)
            {
                PagingToolbar1.MoveFirst();
            }




            //_search.Field_Name = field;

            //dataService.Add<EquipZoneConfig>("EquipmentZoneModel", _search);

            //Results res = WebServiceHelper.Post<Results>("Update_EquipmentZone", dataService.GetObject());

            //if (res.result)
            //{
            //    NotificationExt.Show("Update", "Update Complete");
            //    this.grdDataList.GetStore().GetById(id).Commit();
            //    this.PagingToolbar1.MoveFirst();
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(res.message);
            //}

        }

        private void Clear_Data()
        {
            txtEquipt_NameAdd.Reset();
            txtSerialAdd.Reset();
            cmbTruckTypeAdd.Reset();
            cmbZoneAdd.Reset();
            txtWarehouse_NameAdd.Reset();
        }

        protected void btnAddEqui_Click(object sender, DirectEventArgs e)
        {
            try
            {

                if (Validate_Save() == false)
                {
                    return;
                }

                EquipZoneConfig _equip = new EquipZoneConfig
                {
                    //_equip.EquipID = new Guid();
                    EquipName = txtEquipt_NameAdd.Text,
                    Serialnumber = txtSerialAdd.Text,
                    TruckTypeID = new Guid(cmbTruckTypeAdd.SelectedItem.Value),
                    ZoneID = new Guid(cmbZoneAdd.SelectedItem.Value),
                    IsActive = chkIsActiveAdd.Checked
                };

                bool isSuccess = true;

                Core.Domain.ApiResponseMessage datasave = ClientService.Master.EquipZoneConfigClient.Add(_equip).Result;

                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }

                if (isSuccess)
                {
                    PagingToolbar1.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(GetMessage(ex.Message.ToString()));
            }
        }

        private EquipZoneConfig GetDataEqui()
        {
            EquipZoneConfig itemModel = new EquipZoneConfig
            {
                EquipName = txtEquipt_NameAdd.Text,
                Serialnumber = txtSerialAdd.Text,
                ZoneID = new Guid(cmbZoneAdd.SelectedItem.Value),
                TruckTypeID = new Guid(cmbTruckTypeAdd.SelectedItem.Value),
                IsActive = chkIsActiveAdd.Checked
            };

            return itemModel;
        }

        private bool Validate_Save()
        {
            if (string.IsNullOrWhiteSpace(txtEquipt_NameAdd.Text))
            {
                NotificationExt.Show(GetMessage("MSG00016").MessageTitle, GetMessage("MSG00016").MessageValue);

                // MessageBoxExt.Warning("Please enter Equipt Name.");
                txtEquipt_NameAdd.Focus(true, 300);
                return false;
            }
            else if (txtEquipt_NameAdd.Text.Length > 20)
            {
                NotificationExt.Show(GetMessage("MSG00017").MessageTitle, GetMessage("MSG00017").MessageValue);
                //MessageBoxExt.Warning("The maximum length Equipt Name is 20");
                txtEquipt_NameAdd.Focus(true, 300);
                return false;

            }

            if (string.IsNullOrWhiteSpace(txtSerialAdd.Text))
            {
                NotificationExt.Show(GetMessage("MSG00018").MessageTitle, GetMessage("MSG00018").MessageValue);
                //MessageBoxExt.Warning("Please enter Serial Number.");
                txtSerialAdd.Focus(true, 300);
                return false;
            }
            else if (txtSerialAdd.Text.Length > 20)
            {
                NotificationExt.Show(GetMessage("MSG00019").MessageTitle, GetMessage("MSG00019").MessageValue);
                //MessageBoxExt.Warning("The maximum length Serial Numberis 20");
                txtSerialAdd.Focus(true, 300);
                return false;

            }


            if (string.IsNullOrWhiteSpace(cmbTruckTypeAdd.SelectedItem.Value))
            {
                NotificationExt.Show(GetMessage("MSG00020").MessageTitle, GetMessage("MSG00020").MessageValue);
                // MessageBoxExt.Warning("Please select Truck Type.");
                cmbTruckTypeAdd.Focus(true, 300);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbZoneAdd.SelectedItem.Value))
            {
                NotificationExt.Show(GetMessage("MSG00021").MessageTitle, GetMessage("MSG00021").MessageValue);
                //MessageBoxExt.Warning("Please select Physical Zone.");
                cmbZoneAdd.Focus(true, 300);
                return false;
            }

            return true;
        }
    }
}
