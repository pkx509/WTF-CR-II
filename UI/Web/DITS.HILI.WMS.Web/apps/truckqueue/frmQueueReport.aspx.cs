using ClosedXML.Excel;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.IO;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmQueueReport : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest && !IsPostBack)
            {
                dtStartDate.SelectedDate = DateTime.Today.AddDays(-1);
                dtEndDate.SelectedDate = DateTime.Today;

                dtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                dtEndDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                grdDataList.Margin = 0; 
            }
        }
        [Ext.Net.DirectMethod(Timeout = 900000)]  
        public object BindData(string oDataKeyId, Dictionary<string, object> extraParams)
        {
            int total = 0; 
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            QueueStatusEnum queueStatus = QueueStatusEnum.All;
            Guid shipto = Guid.Empty;
            Guid shipFrom = Guid.Empty;
            Guid dockId = Guid.Empty;

            if (cmbQueueStatus.Value != null)
            {
                if (!Enum.TryParse(cmbQueueStatus.Value.ToString(), out queueStatus))
                {
                    queueStatus = QueueStatusEnum.All;
                }
            }
            if (cmbShipTo.Value != null)
            {
                if (!Guid.TryParse(cmbShipTo.Value.ToString(), out shipto))
                {
                    shipto = Guid.Empty;
                }
            }
            if (cmdShipFrom.Value != null)
            {
                if (!Guid.TryParse(cmdShipFrom.Value.ToString(), out shipFrom))
                {
                    shipFrom = Guid.Empty;
                }
            }
            if (cmbSelectDock.Value != null)
            {
                if (!Guid.TryParse(cmbSelectDock.Value.ToString(), out dockId))
                {
                    dockId = Guid.Empty;
                }
            }

            total = 0;
            Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueueReport(dtStartDate.SelectedDate, dtEndDate.SelectedDate, shipFrom, shipto, queueStatus, dockId, txtSearchKeyword.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<QueueReg> data = new List<QueueReg>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<QueueReg>>();
            }
            return new { data, total }; 
        }

        private void QueryReport(out int total, out List<QueueReg> data)
        {
            QueueStatusEnum queueStatus = QueueStatusEnum.All;
            Guid shipto = Guid.Empty;
            Guid shipFrom = Guid.Empty;
            Guid dockId = Guid.Empty;

            if (cmbQueueStatus.Value != null)
            {
                if (!Enum.TryParse(cmbQueueStatus.Value.ToString(), out queueStatus))
                {
                    queueStatus = QueueStatusEnum.All;
                }
            }
            if (cmbShipTo.Value != null)
            {
                if (!Guid.TryParse(cmbShipTo.Value.ToString(), out shipto))
                {
                    shipto = Guid.Empty;
                }
            }
            if (cmdShipFrom.Value != null)
            {
                if (!Guid.TryParse(cmdShipFrom.Value.ToString(), out shipFrom))
                {
                    shipFrom = Guid.Empty;
                }
            }
            if (cmbSelectDock.Value != null)
            {
                if (!Guid.TryParse(cmbSelectDock.Value.ToString(), out dockId))
                {
                    dockId = Guid.Empty;
                }
            }

            total = 0;
            Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueueReport(dtStartDate.SelectedDate, dtEndDate.SelectedDate, shipFrom, shipto, queueStatus, dockId, txtSearchKeyword.Text,null,null).Result;
            data = new List<QueueReg>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<QueueReg>>();
            }
        }

        protected void btnReset_Click(object sender, DirectEventArgs e)
        {
            txtSearchKeyword.Text = "";

            cmdShipFrom.SelectedItem.Value = null;
            cmdShipFrom.UpdateSelectedItems();

            cmbShipTo.SelectedItem.Value = null;
            cmbShipTo.UpdateSelectedItems();

            cmbQueueStatus.SelectedItem.Value = null;
            cmbQueueStatus.UpdateSelectedItems();

            cmbSelectDock.SelectedItem.Value = null;
            cmbSelectDock.UpdateSelectedItems();


            dtStartDate.SelectedDate = DateTime.Today.AddDays(-1);
            dtEndDate.SelectedDate = DateTime.Today;

            dtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
            dtEndDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

            try
            {
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.ToString());
            }
        }
        protected void btnExport_Click(object sender, DirectEventArgs e)
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
        protected void ToExcel(object sender, EventArgs e)
        {
            QueryReport(out int total, out List<QueueReg> data);
            var filePath = Server.MapPath("FoodTruckQueue-export.xlsx");
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Queue");
                worksheet.Range("A1:N1").Row(1).Merge();
                worksheet.Cell("A1").Value = "รายงานระบบบริหารจัดการคิว-วันไทยอุตสาหกรรมอาหาร";
                worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range("A2:N2").Row(1).Merge();
                worksheet.Cell("A2").Value = string.Format("ช่วงวันที่ {0: dd MMMM yyyy} ถึง {0: dd MMMM yyyy}", dtStartDate.SelectedDate, dtEndDate.SelectedDate);
                worksheet.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                var currentRow = 3;
                worksheet.Cell(currentRow, 1).Value = GetResource("NUMBER");
                worksheet.Cell(currentRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 2).Value = GetResource("QUEUENO");
                worksheet.Cell(currentRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 3).Value = GetResource("DRIVINGLICENSE");
                worksheet.Cell(currentRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 4).Value = GetResource("TRUCK");
                worksheet.Cell(currentRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 5).Value = GetResource("QUEUESTATSU");
                worksheet.Cell(currentRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 6).Value = GetResource("QUEUESHIPFROM");
                worksheet.Cell(currentRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 7).Value = GetResource("QUEUESHIPTO");
                worksheet.Cell(currentRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 8).Value = GetResource("PO_NO");
                worksheet.Cell(currentRow, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 9).Value = GetResource("QUEUEDOCK");
                worksheet.Cell(currentRow, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 10).Value = GetResource("QUEUETIMEIN");
                worksheet.Cell(currentRow, 10).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 11).Value = GetResource("QUEUETIMEOUT");
                worksheet.Cell(currentRow, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 12).Value = GetResource("QUEUEUSAEGTIME");//"ประมาณเวลา Loading (นาที)";
                worksheet.Cell(currentRow, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 13).Value = GetResource("CREATEBY");
                worksheet.Cell(currentRow, 13).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 14).Value = GetResource("REMARK");
                worksheet.Cell(currentRow, 14).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(currentRow, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int index = 1;
                foreach (var user in data)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = index;
                    worksheet.Cell(currentRow, 2).Value = user.QueueNo;
                    worksheet.Cell(currentRow, 3).Value = user.TruckRegNo;
                    worksheet.Cell(currentRow, 4).Value = user.TruckType;
                    worksheet.Cell(currentRow, 5).Value = user.QueueStatus;
                    worksheet.Cell(currentRow, 6).Value = user.ShipFrom;
                    worksheet.Cell(currentRow, 7).Value = user.ShippTo;
                    worksheet.Cell(currentRow, 8).Value = user.PONO;
                    worksheet.Cell(currentRow, 9).Value = user.QueueDock;
                    worksheet.Cell(currentRow, 10).Value = string.Format("{0: dd MM yyyy HH:mm}", user.TimeIn);
                    worksheet.Cell(currentRow, 11).Value = string.Format("{0: dd MM yyyy HH:mm}", user.TimeOut); 

                    worksheet.Cell(currentRow, 12).Value = string.Format("{0}",user.UsageTime);  //user.EstimateTime;
                    worksheet.Cell(currentRow, 13).Value = user.CreateByName;
                    worksheet.Cell(currentRow, 14).Value = user.Remark;
                    index++;
                }                
                //worksheet.Columns(1,14).AdjustToContents();
                using(MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=FoodTruckQueue-export.xlsx");
                    byte[] fileByteArray = ms.ToArray(); 
                    Response.BinaryWrite(fileByteArray);
                    Response.Flush();
                    Response.End();
                }
            } 
        } 
    }
}
