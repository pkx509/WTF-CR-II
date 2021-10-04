using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DITS.HILI.WMS.ReceiveModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.Mobile.Activities.Common;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MobileService.Inbound;
using DITS.HILI.WMS.MobileService.Master;

namespace DITS.HILI.WMS.Mobile.Activities.ReceiveActivity
{
    [Activity(Label = "ReceivingActivity")]
    public class ReceiveItemConfirmActivity : Activity
    {

        Product product = null;
        ReceiveDetail receiveDetail = null;
        List<Location> loadingINLocation = null;
        List<ProductStatus> productStatus = null;
        List<ProductSubStatus> productSubStatus = null;

        ImageView imageProduct = null;
        TextView txtProduct = null;
        TextView txtConvertionQTY = null;
        TextView txtConvertionUnit = null;
        EditText txtStockQTY = null;
        Spinner cmbStockUnit = null;
        Spinner cmbLocation = null;
        TextView txtProductLot = null;
        EditText txtMFGDate = null;
        EditText txtEXPDate = null;
        Spinner cmbStatus = null;
        Spinner cmbSubStatus = null;
        EditText txtRemark = null;
        Button btnConfirm = null;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ReceiveItemConfirm);

            var rcvId = Intent.GetStringExtra("ReceiveDetailID");
            getData(Guid.Parse(rcvId));

            imageProduct = FindViewById<ImageView>(Resource.Id.imageProduct);
            txtProduct = FindViewById<TextView>(Resource.Id.txtProduct);
            txtConvertionQTY = FindViewById<TextView>(Resource.Id.txtConvertionQTY);
            txtConvertionUnit = FindViewById<TextView>(Resource.Id.txtConvertionUnit);
            txtStockQTY = FindViewById<EditText>(Resource.Id.txtStockQTY);
            cmbStockUnit = FindViewById<Spinner>(Resource.Id.cmbStockUnit);
            cmbLocation = FindViewById<Spinner>(Resource.Id.cmbLocation);
            txtProductLot = FindViewById<TextView>(Resource.Id.txtProductLot);
            txtMFGDate = FindViewById<EditText>(Resource.Id.txtMFGDate);
            txtEXPDate = FindViewById<EditText>(Resource.Id.txtEXPDate);
            cmbStatus = FindViewById<Spinner>(Resource.Id.cmbStatus);
            cmbSubStatus = FindViewById<Spinner>(Resource.Id.cmbSubStatus);
            txtRemark = FindViewById<EditText>(Resource.Id.txtRemark);
            btnConfirm = FindViewById<Button>(Resource.Id.btnConfirmReceive);

            cmbStockUnit.ItemSelected += CmbStockUnit_ItemSelected;
            cmbStatus.ItemSelected += CmbStatus_ItemSelected;
            btnConfirm.Click += BtnConfirm_Click;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                Confirm();
                Android.Widget.Toast.MakeText(this, "Receiving complete", ToastLength.Long);
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(this, ex.Message, ToastLength.Long);
            }

        }

        private void CmbStatus_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            setProductSubStatus(e.Position);
        }

        private void CmbStockUnit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var item = product.UnitCollection.ToList()[e.Position];
            txtStockQTY.Text = ConvertQty(receiveDetail.BaseQuantity, item).ToString();
        }

        private async void getData(Guid rcvId)
        {
            receiveDetail = await ReceiveClient.GetReceiveDetail(rcvId);
            product = await ProductClient.GetProduct(receiveDetail.ProductID);
            loadingINLocation = await WarehouseClient.GetLoadingINLocation();
            productStatus = await ProductStatusClient.GetByDocumentTypeID(receiveDetail.Receive.ReceiveTypeID);

            cmbStockUnit.Adapter = new ProductUnitListAdapter(this, product.UnitCollection.ToList());
            cmbLocation.Adapter = new LoadingAdapter(this, loadingINLocation);
            cmbStatus.Adapter = new ProductStatusAdapter(this, productStatus);

            var index = productStatus.FindIndex(x => x.ProductStatusID == receiveDetail.ProductStatusID);
            cmbStatus.SetSelection(index);

            index = loadingINLocation.FindIndex(x => x.LocationID == receiveDetail.Receive.LocationID);
            cmbLocation.SetSelection(index);

            index = product.UnitCollection.ToList().FindIndex(x => x.ProductUnitID == receiveDetail.StockUnitID);
            cmbStockUnit.SetSelection(index);

            imageProduct.SetImageResource(Resource.Drawable.Shipping6);
            txtProduct.Text = receiveDetail.Product.Name;
            txtConvertionQTY.Text = receiveDetail.ConversionQty.ToString();
            txtConvertionUnit.Text = receiveDetail.ProductBaseUOM.Name;
            txtStockQTY.Text = receiveDetail.Quantity.ToString();

            txtProductLot.Text = receiveDetail.Lot;
            txtMFGDate.Text = (receiveDetail.ManufacturingDate.HasValue ? receiveDetail.ManufacturingDate.Value.ToShortDateString() : Convert.ToString(receiveDetail.ManufacturingDate));
            txtEXPDate.Text = (receiveDetail.ExpirationDate.HasValue ? receiveDetail.ExpirationDate.Value.ToShortDateString() : Convert.ToString(receiveDetail.ExpirationDate));

            txtRemark.Text = receiveDetail.Remark;


        }

        private decimal ConvertQty(decimal baseQty, ProductUnit productUnit)
        {
            decimal total = baseQty / productUnit.Quantity;
            return (productUnit.IsBaseUOM ? total : Math.Ceiling(total));
        }

        private async void Confirm()
        {
            var pstatus = productStatus[cmbStatus.SelectedItemPosition];
            var pSubStatus = productSubStatus[cmbSubStatus.SelectedItemPosition];
            var pUnit = product.UnitCollection.ToList()[cmbStockUnit.SelectedItemPosition];
            var location = loadingINLocation[cmbLocation.SelectedItemPosition];

            decimal qty = decimal.Parse(txtStockQTY.Text);
            decimal baseQty = qty * pUnit.Quantity;

            Receiving rcv = new Receiving
            {
                Quantity = qty,
                BaseQuantity = baseQty,
                BaseUnitID = product.UnitCollection.FirstOrDefault(x => x.IsBaseUOM).ProductUnitID,
                ConversionQty = pUnit.Quantity,
                LocationID = location.LocationID,
                Lot = receiveDetail.Lot,
                ManufacturingDate = receiveDetail.ManufacturingDate,
                ExpirationDate = receiveDetail.ExpirationDate,
                PackageWeight = receiveDetail.PackageWeight,
                PalletCode = receiveDetail.PalletCode,
                Price = receiveDetail.Price,
                ProductHeight = receiveDetail.ProductHeight,
                ProductLength = receiveDetail.ProductLength,
                ProductID = receiveDetail.ProductID,
                ProductOwnerID = receiveDetail.Receive.ProductOwnerID,
                ProductStatusID = pstatus.ProductStatusID,
                ProductSubStatusID = pSubStatus.ProductSubStatusID,
                ProductUnitPriceID = receiveDetail.ProductUnitPriceID,
                ProductWeight = receiveDetail.ProductWeight,
                ProductWidth = receiveDetail.ProductWidth,
                ReceiveDetailID = receiveDetail.ReceiveDetailID,
                ReceiveID = receiveDetail.ReceiveID,
                StockUnitID = pUnit.ProductUnitID,
                Remark = txtRemark.Text
            };

            await ReceiveClient.ConfirmReceive(rcv);
        }

        private void setProductSubStatus(int position)
        {
            productSubStatus = productStatus[position].ProductStatusMapCollection.Select(n => n.ProductSubStatus).ToList();
            cmbSubStatus.Adapter = new ProductSubStatusAdapter(this, productSubStatus);
            var index = productSubStatus.FindIndex(x => x.ProductSubStatusID == receiveDetail.ProductSubStatusID);
            cmbSubStatus.SetSelection(index);
        }

    }
}