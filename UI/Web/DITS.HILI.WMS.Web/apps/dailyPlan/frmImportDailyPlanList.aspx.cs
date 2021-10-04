using DITS.HILI.WMS.ClientService.DailyPlan;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.WMS.Common.Extensions;
using DITS.WMS.Web.Common;
using Ext.Net;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.dailyPlan
{
    public partial class frmImportDailyPlanList : BaseUIPage
    {
        private static List<ProductionPlanCustomModel> _ProductionPlan = new List<ProductionPlanCustomModel>();
        private static List<ValidationImportFileResult> _ValidateImport = new List<ValidationImportFileResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            btnSave.Disable();
            btnDownload.Disable();
            txtTotal.Text = $"{GetResource("TOTAL")} : 0 {GetResource("RECORD")} / {GetResource("ERROR")} : 0 {GetResource("ERROR")}.";
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                OnReset();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string importData = e.ExtraParams["ParamStorePages"];
            List<ProductionPlanCustomModel> productionPlans = JSON.Deserialize<List<ProductionPlanCustomModel>>(importData);

            Core.Domain.ApiResponseMessage apiResp = ImportProductionClient.ImportDailyPlan(productionPlans).Result;

            if (apiResp.IsSuccess)
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            else
            {
                // save incomplete
                MessageBoxExt.ShowError(GetMessage("MSG00004").MessageValue);
            }

            OnReset();
        }

        protected void ImportExcelFile(object sender, DirectEventArgs e)
        {
            try
            {
                string folder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string extensionFile = Path.GetExtension(FileUploadField1.FileName).ToLower();
                if (string.IsNullOrEmpty(extensionFile))
                {
                    return;
                }

                if (!(extensionFile == ".xlsx" || extensionFile == ".xls"))
                {
                    //"Invalid file format."
                    MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                    FileUploadField1.Reset();
                    return;
                }

                string filename = DateTime.Now.ToString("yyyyMMddHHmm") + extensionFile;
                FileUploadField1.PostedFile.SaveAs(folder + filename);
                OnValidate(folder + filename);

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }
        }

        protected void btnDownload_Click(object sender, DirectEventArgs e)
        {
            ExcelFile xls = new ExcelFile();
            string file = "dialy_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_error.xlsx";
            xls.DailyPlanExcelFile(_ProductionPlan, _ValidateImport, Server.MapPath("~/Uploads/" + file));
            X.Redirect("~/Uploads/" + file);
        }

        private void OnReset()
        {
            FileUploadField1.Reset();
            StoreError.RemoveAll();
            StoreImport.RemoveAll();
            btnSave.Disable();
        }

        private void OnValidate(string path)
        {

            string _ProductCode = null;
            string strFormat = "d/M/yyyy";

            DateTime? _DeliveryDate = null;

            ExcelQueryFactory xPre = new ExcelQueryFactory(path);
            string worksheetNames = xPre.GetWorksheetNames().FirstOrDefault();
            List<Row> pre = xPre.Worksheet(worksheetNames.ToLower()).Skip(3).ToList();
            if (pre.Count == 0)
            {
                return;
            }

            List<ProductionPlanCustomModel> productionPlans = new List<ProductionPlanCustomModel>();
            ProductionPlanCustomModel productionPlan;

            List<Row> hdd = xPre.Worksheet(worksheetNames.ToLower()).Take(1).ToList();

            Row hdd_item = hdd.FirstOrDefault();
            if (hdd_item == null)
            {
                //"Warning", "No Item."
                MessageBoxExt.Show(GetMessage("MSG00006").MessageTitle, GetMessage("MSG00006").MessageValue);
                return;
            }

            if (!DateTime.TryParseExact(hdd_item[1].Value.ToString(), strFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _Date))
            {
                //"Warning", "Please Check Date Time."
                MessageBoxExt.Show(GetMessage("MSG00007").MessageTitle, GetMessage("MSG00007").MessageValue);
                return;
            }

            pre.ForEach(x =>
            {
                decimal _Weight = 0;

                if (!string.IsNullOrWhiteSpace(x[0].Value.ToString()))
                {
                    _ProductCode = (string)x[0].Value;
                }

                if (!string.IsNullOrWhiteSpace(x[6].Value.ToString()))
                {
                    _Weight = x[6].Value.ToString().StringToDecimal();
                }

                if (DateTime.TryParseExact(x[17].Value.ToString(), strFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _Delivery))
                {
                    _DeliveryDate = _Delivery;
                }

                productionPlan = new ProductionPlanCustomModel
                {
                    ProductionDate = _Date,
                    LineCode = x[0].Value.ToString().Trim(),
                    Seq = x[1].Value.ToString().ToInt(),
                    ProductCode = x[2].Value.ToString().Trim(),
                    ProductName = x[3].Value.ToString().Trim(),
                    ProductionQty = x[4].Value.ToString().ToInt(),
                    ProductUnitName = x[5].Value.ToString(),
                    Weight_G = _Weight,
                    OrderNo = x[7].Value.ToString().Trim(),
                    OrderType = x[8].Value.ToString(),
                    Film = x[9].Value.ToString(),
                    Box = x[10].Value.ToString(),
                    Powder = x[11].Value.ToString(),
                    Oil = x[12].Value.ToString(),
                    FD = x[13].Value.ToString(),
                    Stamp = x[14].Value.ToString(),
                    Sticker = x[15].Value.ToString(),
                    Mark = x[16].Value.ToString(),
                    DeliveryDate = _DeliveryDate,
                    //CustomerCode = x[18].Value.ToString(),
                    //CustomerName = x[19].Value.ToString(),
                    WorkingTime = x[20].Value.ToString(),
                    OilType = x[21].Value.ToString(),
                    Formula = x[22].Value.ToString(),
                };
                productionPlans.Add(productionPlan);
            });
            productionPlans.Where(o => o.LineCode == "" && o.ProductCode == "" && o.OrderType == "" && o.ProductUnitName == "").ToList().ForEach(x =>
            {
                productionPlans.Remove(x);
            });

            if (productionPlans.Count == 0)
            {
                FileUploadField1.Reset();

                //"Don't have record to import"
                MessageBoxExt.Warning(GetMessage("MSG00008").MessageValue);
                return;
            }

            int irow = 0;
            string dupMessage = "";
            foreach (ProductionPlanCustomModel countRow in productionPlans)
            {
                List<ProductionPlanCustomModel> _dupPlan = productionPlans.Where(w => w.ProductionDate == countRow.ProductionDate
                                                && w.LineCode == countRow.LineCode
                                                && w.Seq == countRow.Seq).ToList();

                if (_dupPlan.Count > 1)
                {
                    dupMessage = $"{GetResource("PRODUCTION_DATE")} : { countRow.ProductionDate.Date } , "
                                + $"{GetResource("PRODUCT_CODE")} : { countRow.ProductCode } , "
                                + $"{GetResource("LINE")} : { countRow.LineCode } , "
                                + $"{GetResource("ORDER_SEQ")} : { countRow.Seq } , "
                                + $"{GetResource("DUPLICATE")} <br />";
                }

                countRow.RowIndex = irow;
                irow++;
            }

            if (!string.IsNullOrWhiteSpace(dupMessage))
            {
                MessageBoxExt.Warning(dupMessage);
                return;
            }

            List<ValidationImportFileResult> inValidData = new List<ValidationImportFileResult>();
            Core.Domain.ApiResponseMessage apiResp = ImportProductionClient.ValidateImport(productionPlans).Result;

            if (apiResp.IsSuccess)
            {
                inValidData = apiResp.Get<List<ValidationImportFileResult>>();
            }

            StoreError.DataSource = inValidData.OrderBy(o => o.ColumnIndex).ThenBy(o => o.RowIndex).ToList();
            StoreError.DataBind();

            StoreImport.DataSource = productionPlans;
            StoreImport.DataBind();

            _ProductionPlan = productionPlans;
            _ValidateImport = inValidData;

            if (inValidData.Count == 0)
            {
                btnSave.Enable();
                btnDownload.Disable();
            }
            else
            {
                btnSave.Disable();
                btnDownload.Enable();
            }

            // use this instead of string.format
            txtTotal.Text = $"{GetResource("TOTAL")} : {productionPlans.Count} {GetResource("RECORD")} / {GetResource("ERROR")} : {inValidData.Count} {GetResource("ERROR")}.";
        }
    }
}