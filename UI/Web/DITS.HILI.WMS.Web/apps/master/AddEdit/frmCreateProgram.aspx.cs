using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateProgram : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsPostBack)
                {
                    return;
                }

                string code = Request.QueryString["oDataKeyId"];

                LoadCombo();
                getProgramInfo(code);
            }
        }

        private void LoadCombo()
        {
            ApiResponseMessage apiResp = ClientService.Master.ProgramClient.GetAll(ProgramType.Module, 0, 100).Result;
            List<Program> data = new List<Program>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<Program>>();
            }
            StoreModule.DataSource = data;

        }

        private void getProgramInfo(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.ProgramClient.GetById(id).Result;
                Program _data = data.Get<Program>();
                if (_data == null)
                {
                    return;
                }

                txtUrl.Text = _data.Url;
                txtProgram_Code.Text = _data.Code;
                txtSequence.Text = _data.Sequence.ToString();
                txtIsActive.Value = _data.IsActive;
                txtType.Text = _data.ProgramType.ToString();
                cmbModule_Code.SelectedItem.Value = _data.ParentID.ToString();
                if (_data.ProgramType == ProgramType.Module)
                {
                    cmbModule_Code.Hide();
                    txtUrl.Hide();
                }
                StoreOfDataList.Data = _data.ProgramValueCollection.ToList();
            }
            catch
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string gridJson = e.ExtraParams["ParamStorePages"];
                ICollection<ProgramValue> gridData = JSON.Deserialize<ICollection<ProgramValue>>(gridJson);

                Program _program = new Program();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _program.ProgramID = new Guid(id);
                }

                _program.Sequence = Convert.ToInt32(txtSequence.Value);
                if (!string.IsNullOrEmpty(cmbModule_Code.SelectedItem.Value))
                {
                    _program.ParentID = new Guid(cmbModule_Code.SelectedItem.Value);
                    _program.Url = txtUrl.Text;
                }
                _program.IsActive = txtIsActive.Checked;
                _program.ProgramValueCollection = gridData;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                datasave = ClientService.Master.ProgramClient.ModifyProgram(_program).Result;
                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
            //var receieve = GetToModel(gridData);
        }
    }
}