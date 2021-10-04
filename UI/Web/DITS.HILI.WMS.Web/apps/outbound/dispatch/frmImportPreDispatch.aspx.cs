using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.WMS.Web.Common;
using Ext.Net;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.outbound.dispatch
{
    public partial class frmImportPreDispatch : BaseUIPage
    {
        public static string ProgramCode = "P-0011";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Disable();
        }


        protected void OnImport(object sender, DirectEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
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

                    MessageBoxExt.ShowError("Invalid file format.");
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

        protected void OnValidate(string path)
        {
            ExcelQueryFactory xPre = new ExcelQueryFactory(path);
            //var worksheetNames = xPre.GetWorksheetNames().FirstOrDefault();
            //var pre = xPre.Worksheet(worksheetNames.ToLower()).ToList();
            List<Row> pre = xPre.Worksheet("PreDispatch").ToList();
            if (pre.Count == 0)
            {
                return;
            }

            List<PreDispatchesImportModel> predispatchModel = new List<PreDispatchesImportModel>();
            PreDispatchesImportModel item;
            double? _Qty = null;
            double? _price = null;



            DateTime? _estDate;
            DateTime? _documentDate = null;
            DateTime? _deliveryDate;

            CultureInfo us = new CultureInfo("en-US");


            pre.ForEach(x =>
            {
                bool isEmply = true;
                x.ForEach(z =>
                {
                    if (!string.IsNullOrEmpty(z.Value.ToString()))
                    {
                        isEmply = false;
                    }
                });
                if (!isEmply)
                {
                    string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy" };
                    //string strFormat = "d/M/yyyy";


                    //if (!string.IsNullOrWhiteSpace(x[13].Value.ToString()))
                    //    _Qty = (double)x[13].Value;
                    //else
                    //    _Qty = null;
                    bool ok = double.TryParse(x[13].Value.ToString(), out double _datadecimal);
                    if (ok)
                    {
                        _Qty = (double)x[13].Value;
                    }
                    else
                    {
                        _Qty = null;
                    }

                    ok = DateTime.TryParse(x[2].Value.ToString(), out DateTime estDate);
                    if (ok)
                    {
                        _estDate = DateTime.Parse(x[2].Value.ToString(), us);
                    }
                    else
                    {
                        _estDate = null;
                    }

                    ok = DateTime.TryParse(x[3].Value.ToString(), out DateTime documentDate);
                    if (ok)
                    {
                        _documentDate = DateTime.Parse(x[3].Value.ToString(), us);
                    }
                    else
                    {
                        _documentDate = null;
                    }

                    ok = DateTime.TryParse(x[5].Value.ToString(), out DateTime deliveryDate);
                    if (ok)
                    {
                        _deliveryDate = DateTime.Parse(x[5].Value.ToString(), us);
                    }
                    else
                    {
                        _deliveryDate = null;
                    }

                    ok = double.TryParse(x[15].Value.ToString(), out _datadecimal);
                    if (ok)
                    {
                        _price = double.Parse(x[15].Value.ToString());
                    }
                    else
                    {
                        _price = null;
                    }

                    item = new PreDispatchesImportModel
                    {
                        DispatchCode = string.Empty,//x[0].Value.ToString(),
                        CustomerCode = x[0].Value.ToString(),
                        Dispatch_Type_Code = x[1].Value.ToString(),
                        EstDispatchDate = _estDate,
                        DocumentDate = _documentDate,
                        ShippingTo = x[4].Value.ToString(),
                        DeliveryDate = _deliveryDate,
                        PONumber = x[5].Value.ToString(),
                        OrderNumber = x[6].Value.ToString(),
                        IsUrgent = (string.IsNullOrEmpty(x[8].Value.ToString()) ? "N" : x[8].Value.ToString()),
                        Remark = x[9].Value.ToString(),
                        IsBackOrder = (string.IsNullOrEmpty(x[10].Value.ToString()) ? "N" : x[10].Value.ToString()),
                        ProductCode = x[11].Value.ToString(),
                        Quantity = _Qty,
                        UOM = x[14].Value.ToString(),
                        Price = _price,
                        UnitPrice = x[16].Value.ToString(),
                        Remark2 = x[17].Value.ToString()
                    };
                    predispatchModel.Add(item);
                }
            });



            predispatchModel.Where(o => o.CustomerCode == "" && o.ProductCode == "" && o.DispatchType == "" && o.UOM == "" && o.PONumber == "").ToList().ForEach(x =>
            {
                predispatchModel.Remove(x);
            });

            var _header = predispatchModel.GroupBy(g => new
            {
                index = g.DispatchCode,
                CustomerCode = g.CustomerCode,
                DispatchTypeCode = g.Dispatch_Type_Code,
                EstDispatchDate = g.EstDispatchDate,
                DocumentDate = g.DocumentDate,
                ShippingTo = g.ShippingTo,
                DeliveryDate = g.DeliveryDate,
                PONumber = g.PONumber,
                OrderNumber = g.OrderNumber,
                IsUrgent = g.IsUrgent,
                IsBackOrder = g.IsBackOrder,
                Remark = g.Remark


            }).Select(n => new
            {
                Index = n.Key.index,
                CustomerCode = n.Key.CustomerCode,
                DispatchTypeCode = n.Key.DispatchTypeCode,
                EstDispatchDate = n.Key.EstDispatchDate,
                DocumentDate = n.Key.DocumentDate,
                ShippingTo = n.Key.ShippingTo,
                DeliveryDate = n.Key.DeliveryDate,
                PONumber = n.Key.PONumber,
                OrderNumber = n.Key.OrderNumber,
                IsUrgent = n.Key.IsUrgent,
                IsBackOrder = n.Key.IsBackOrder,
                Remark = n.Key.Remark
            }).ToList();


            List<string> query = predispatchModel.GroupBy(x => x.PONumber)
              .Select(y => y.Key)
              .ToList();
            if (_header.Count != query.Count)
            {
                FileUploadField1.Reset();
                MessageBoxExt.Warning("PO Number are duplicate.");
                return;
            }
            if (predispatchModel.Count == 0)
            {
                FileUploadField1.Reset();
                MessageBoxExt.Warning("Don't have record to import");
                return;
            }
            //DataServiceModel dataService = new DataServiceModel();

            //dataService.Add<List<PreDispatchesImportModel>>("PreDispatchesImportModel", predispatchModel);
            //List<ObjectPropertyValidatorException> errorData = WebServiceHelper.Post<List<ObjectPropertyValidatorException>>
            //                                        ("ImportPreDispatch", dataService.GetObject());


            // string json = JsonConvert.SerializeObject(predispatchModel);

            int total = 0;
            Core.Domain.ApiResponseMessage apiResp = DispatchClient.ImportDispatch(predispatchModel).Result;
            List<ObjectPropertyValidatorException> errorData = new List<ObjectPropertyValidatorException>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                errorData = apiResp.Get<List<ObjectPropertyValidatorException>>();
            }

            if (errorData == null)
            {
                MessageBoxExt.Show("Warning", "Import data error.");
            }
            List<GridImportDispatchModel> guid = new List<GridImportDispatchModel>();
            GridImportDispatchModel guidItem;

            predispatchModel.ForEach(x =>
            {
                guidItem = new GridImportDispatchModel
                {
                    F1 = x.DispatchCode,
                    F2 = x.CustomerCode,
                    F3 = x.Dispatch_Type_Code,
                    F4 = x.EstDispatchDate,
                    F5 = x.DocumentDate,
                    F6 = x.ShippingTo,
                    F7 = x.DeliveryDate,
                    F8 = x.PONumber,
                    F9 = x.OrderNumber,
                    F10 = x.IsBackOrder,
                    F11 = x.Remark,
                    F12 = x.IsUrgent,
                    F13 = x.ProductCode,
                    F14 = x.Quantity,
                    F15 = x.UOM,
                    F16 = x.Price,
                    F17 = x.UnitPrice,
                    F18 = x.Remark2
                };
                guid.Add(guidItem);
            });

            StoreError.DataSource = errorData.OrderBy(o => o.ColumnIndex).ThenBy(o => o.RowIndex).ToList();
            StoreError.DataBind();

            StoreImport.DataSource = guid;
            StoreImport.DataBind();

            Session["DispatchImport"] = predispatchModel;
            Session["DispatchImportError"] = errorData;

            if (errorData.Count == 0)
            {
                btnSave.Enable();
                btnDownload.Disable();
            }
            else
            {
                btnSave.Disable();
                btnDownload.Enable();
            }

            txtTotal.Text = string.Format("Total : {0} Record / Error : {1} errors.", guid.Count, errorData.Count);
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {

            string importData = e.ExtraParams["ParamStorePages"];
            List<GridImportDispatchModel> _importModel = JSON.Deserialize<List<GridImportDispatchModel>>(importData);

            List<PreDispatchesImportModel> predispatchModel = new List<PreDispatchesImportModel>();
            _importModel.ForEach(x =>
            {
                predispatchModel.Add(new PreDispatchesImportModel
                {
                    DispatchCode = x.F1,
                    CustomerCode = x.F2,
                    Dispatch_Type_Code = x.F3,
                    EstDispatchDate = x.F4,
                    DocumentDate = x.F5,
                    ShippingTo = x.F6,
                    DeliveryDate = x.F7,
                    PONumber = x.F8,
                    OrderNumber = x.F9,
                    IsBackOrder = x.F10,
                    Remark = x.F11,
                    IsUrgent = x.F12,
                    ProductCode = x.F13,
                    Quantity = x.F14,
                    UOM = x.F15,
                    Price = x.F16,
                    UnitPrice = x.F17,
                    Remark2 = x.F18


                });
            });


            bool isSuccess = true;

            // string json = JsonConvert.SerializeObject(predispatchModel);

            Core.Domain.ApiResponseMessage datasave = DispatchClient.SaveImportDispatch(predispatchModel).Result;

            if (datasave.ResponseCode != "0")
            {
                isSuccess = false;
                MessageBoxExt.ShowError(datasave.ResponseMessage);
            }

            if (isSuccess)
            {
                OnReset();
            }
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

        protected void btnDownload_Click(object sender, DirectEventArgs e)
        {
            List<PreDispatchesImportModel> data = (List<PreDispatchesImportModel>)Session["DispatchImport"];
            List<ObjectPropertyValidatorException> error = (List<ObjectPropertyValidatorException>)Session["DispatchImportError"];
            ExcelFile xls = new ExcelFile();
            string file = "disp_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_error.xlsx";
            xls.PredispatchExcelFile(data, error, Server.MapPath("\\Uploads\\" + file));
            X.Redirect("../../../Uploads/" + file);
        }

        private void OnReset()
        {

            FileUploadField1.Reset();
            StoreError.RemoveAll();
            StoreImport.RemoveAll();
            btnSave.Disable();
        }

    }
}