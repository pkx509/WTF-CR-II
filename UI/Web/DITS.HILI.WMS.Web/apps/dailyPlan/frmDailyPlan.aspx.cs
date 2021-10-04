using DITS.HILI.WMS.ClientService.DailyPlan;
using DITS.HILI.WMS.DailyPlanModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.dailyPlan
{
    public partial class frmDailyPlan : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData(Request.QueryString["productionDetailID"], Request.QueryString["section"]);
        }

        private void BindData(string productionDetailID, string section)
        {

            try
            {
                hdSection.Value = section;
                hdProductionDetailID.Value = null;

                if (productionDetailID == "new")
                {
                    dtProductionDate.SelectedDate = DateTime.Now;
                    return;
                }

                if (!Guid.TryParse(productionDetailID, out Guid searchID))
                {
                    MessageBoxExt.Warning("Invalid searchID format");
                    return;
                }

                if (section != "NP" && section != "SP")
                {
                    MessageBoxExt.ShowError("Section (NP/SP) not found");
                    return;
                }

                Core.Domain.ApiResponseMessage apiResp = ImportProductionClient.GetByID(searchID).Result;

                if (apiResp.IsSuccess)
                {
                    ProductionPlanCustomModel data = apiResp.Get<ProductionPlanCustomModel>();

                    if (data == null)
                    {
                        MessageBoxExt.Warning("Data not found");
                        return;
                    }

                    BindtoControl(data);
                }
                else
                {
                    MessageBoxExt.ShowError(string.Join(", ", apiResp.ResponseMessage));
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }
        }

        private void BindtoControl(ProductionPlanCustomModel data)
        {
            hdProductionDetailID.SetValue(data.ProductionDetailID);
            hdDailyPlanStatus.SetValue(data.DailyPlanStatus);

            dtProductionDate.SelectedDate = data.ProductionDate;
            dtDeliveryDate.SelectedDate = data.DeliveryDate ?? DateTime.Now;

            txtOrderNo.Text = data.OrderNo;
            txtFilm.Text = data.Film;
            txtPowder.Text = data.Powder;
            txtBox.Text = data.Box;
            txtFD.Text = data.FD;
            txtOil.Text = data.Oil;
            txtSticker.Text = data.Sticker;
            txtStamp.Text = data.Stamp;
            txtMark.Text = data.Mark;
            //txtCustomerCode.Text = 
            //txtCustomerName.Text = 
            txtWorkingTime.Text = data.WorkingTime;
            txtIngredients.Text = data.Formula;
            txtRemark.Text = data.Remark;

            nbOrderSeq.SetValue(data.Seq ?? 0);
            nbQty.SetValue(data.ProductionQty);
            nbWeight.SetValue(data.Weight_G ?? 0);
            nbPalletQty.SetValue(data.PalletQty ?? 0);

            #region bindCombobox

            if (data.LineId != null)
            {
                cmbLine.InsertItem(0, data.LineCode, data.LineId.Value);
                cmbLine.Select(data.LineId.Value);
            }

            if (data.ProductID != null)
            {
                cmbProduct.InsertItem(0, data.ProductCode + " : " + data.ProductName, data.ProductID);
                cmbProduct.Select(data.ProductID);
            }

            if (data.ProductUnitID != null)
            {
                cmbUnit.InsertItem(0, data.ProductUnitName, data.ProductUnitID.Value);
                cmbUnit.Select(data.ProductUnitID.Value);
            }

            if (!string.IsNullOrWhiteSpace(data.OrderType))
            {
                cmbOrderType.Select(data.OrderType);
            }

            #endregion

            btnSave.Hidden = data.DailyPlanStatus == 1;
        }

        /// <summary>
        /// For Testing Only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMockup_Click(object sender, DirectEventArgs e)
        {
            int count = int.Parse(nbOrderSeq.Value?.ToString() ?? "0");

            BindtoControl(new ProductionPlanCustomModel()
            {
                ProductionDate = DateTime.Now,
                DeliveryDate = DateTime.Now,

                ProductName = "ProductName " + count.ToString(),
                OrderNo = "OrderNo " + count.ToString(),
                Film = "Film " + count.ToString(),
                Powder = "Powder " + count.ToString(),
                Box = "Box " + count.ToString(),
                FD = "FD " + count.ToString(),
                Oil = "Oil " + count.ToString(),
                Sticker = "Sticker " + count.ToString(),
                Stamp = "Stamp " + count.ToString(),
                Mark = "Mark " + count.ToString(),
                WorkingTime = "WorkingTime " + count.ToString(),
                Formula = "Formula " + count.ToString(),

                Seq = count,
                ProductionQty = count,
                Weight_G = count,
                PalletQty = count,

                //LineId = Guid.Parse("749F57C8-1415-E711-9413-0050569135DB"),
                //LineCode = "L01",
                //ProductID = Guid.Parse("E255CC05-A74A-40EA-A913-143046800CF5"),
                //ProductCode = "MockData1",
                //ProductUnitID = Guid.Parse("AE06ED11-72EF-E611-93FF-0050569135DB"),
                //ProductUnitName = "MockData1",
                OrderType = "LOCAL"
            });

        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            ProductionPlanCustomModel productionPlan = GetToModel();

            Core.Domain.ApiResponseMessage apiResp = ImportProductionClient.SavePlan(productionPlan).Result;

            if (apiResp.IsSuccess)
            {

                X.Call("parent.App.direct.Reload", apiResp.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }
        }

        private ProductionPlanCustomModel GetToModel()
        {

            ProductionPlanCustomModel model = new ProductionPlanCustomModel()
            {
                ProductionDetailID = Guid.TryParse(hdProductionDetailID.Text, out Guid productionDetailID) ? productionDetailID : (Guid?)null,
                DailyPlanStatus = int.TryParse(hdDailyPlanStatus.Text, out int status) ? status : (int?)null,

                ProductionDate = dtProductionDate.SelectedDate,
                DeliveryDate = dtDeliveryDate.SelectedDate,

                OrderNo = txtOrderNo.Text.Trim(' '),
                Film = txtFilm.Text,
                Powder = txtPowder.Text,
                Box = txtBox.Text,
                FD = txtFD.Text,
                Oil = txtOil.Text,
                Sticker = txtSticker.Text,
                Stamp = txtStamp.Text,
                Mark = txtMark.Text,
                Remark = txtRemark.Text,
                WorkingTime = txtWorkingTime.Text,
                Formula = txtIngredients.Text,

                Seq = (decimal?)nbOrderSeq.Number,
                ProductionQty = (int)nbQty.Number,
                PalletQty = (int)nbPalletQty.Number,
                Weight_G = (decimal?)nbWeight.Number,

                LineId = Guid.TryParse(cmbLine.SelectedItem.Value, out Guid lineID) ? lineID : (Guid?)null,
                LineCode = cmbLine.SelectedItem.Text,
                ProductID = Guid.TryParse(cmbProduct.SelectedItem.Value, out Guid productID) ? productID : Guid.Empty,
                ProductCode = cmbProduct.SelectedItem.Text,
                ProductUnitID = Guid.TryParse(cmbUnit.SelectedItem.Value, out Guid productUnitID) ? productUnitID : (Guid?)null,
                ProductUnitName = cmbUnit.SelectedItem.Text,
                OrderType = cmbOrderType.SelectedItem.Text
            };

            return model;
        }
    }
}