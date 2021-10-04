using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class DispatchModels
    {
        //public Guid Dispatch_Id { get; set; }
        //public string Dispatch_Code { get; set; }
        //public string Customer_Code { get; set; }
        //public string Customer_NameTH { get; set; }
        //public string SubCust_Code { get; set; }
        //public Guid? Dispatch_Type_ID { get; set; }
        //public DateTime? Dispatch_Date_Order { get; set; }
        //public DateTime? Dispatch_Date_Confirm { get; set; }
        //public DateTime? Dispatch_Date_Delivery { get; set; }
        //public string Dispatch_Refered_1 { get; set; }
        //public string Dispatch_Refered_2 { get; set; }

        //public string Dsp_Date { get; set; }

        //public string Dispatch_Status_Name { get; set; }
        //public int? Dispatch_Status_Id { get; set; }

        //public int Dispatch_Status_Dispatch { get; set; }
        //public string Dispatch_Status_TH { get; set; }


        //public int? Dispatch_Status_Job { get; set; }
        //public int? Dispatch_Status_CheckInvoice { get; set; }
        //public int? Dispatch_Status_CompareInvoice { get; set; }
        //public int? Dispatch_Status_ReturnInvoice { get; set; }
        //public string Dispatch_Type_Name { get; set; }

        //public string SubCust_NameTH { get; set; }
        //public string SubCust_NameEN { get; set; }
        //public string SubCust_Name { get; set; }
        //public string SubCust_Name_Short { get; set; }
        //public string SubCust_Country { get; set; }
        //public string Shipping_Routes { get; set; }

        //public string Shipto_Code { get; set; }
        //public string Shipto_Name { get; set; }

        //public string Status { get; set; }

        //public string Create_User { get; set; }
        //public DateTime? Create_Date { get; set; }
        //public string Update_User { get; set; }
        //public DateTime? Update_Date { get; set; }

        //public string Job_Picking_Code { get; set; }

        //public decimal TotalDispatchQty { get; set; }
        //public decimal TotalDispatchWeight { get; set; }
        //public decimal TotalDispatchCBM { get; set; }

        //private bool _isSelect = true;
        //public string Loading_Warehouse_Code { get; set; }
        //public string Loading_Warehouse_Name { get; set; }

        //public bool IsBackOrder { get; set; }
        //public bool IsAssigned { get; set; }
        //public bool IsUrgent { get; set; }

        //public string IsUrgentString
        //{
        //    get
        //    {
        //        return (IsUrgent ? "Yes" : "");
        //    }
        //}
        //public bool IsSelect
        //{
        //    get
        //    {
        //        return _isSelect;
        //    }
        //    set
        //    {
        //        _isSelect = value;
        //    }
        //}

        //public string DocumentNo { get; set; }
        //public string PONO { get; set; }
        //public string OrderNo { get; set; }

        public System.Guid DispatchId { get; set; }
        public string DispatchCode { get; set; }
        public System.Guid? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public System.DateTime? OrderDate { get; set; }
        public System.DateTime? DocumentDate { get; set; }
        public System.DateTime? DeliveryDate { get; set; }
        public System.DateTime? DocumentApproveDate { get; set; }
        public string Pono { get; set; }
        public string OrderNo { get; set; }
        public System.Guid? ShipptoId { get; set; }
        public string ShipptoName { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsBackOrder { get; set; }
        public System.Guid? ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public System.Guid? FromwarehouseId { get; set; }
        public string FromwarehouseName { get; set; }
        public System.Guid? TowarehouseId { get; set; }
        public string TowarehouseName { get; set; }
        public int? DispatchStatusId { get; set; }
        public string DispatchStatusName { get; set; }
        public System.Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public System.Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public bool IsMarketing { get; set; }
        public bool IsAssign { get; set; }
        public decimal TotalDispatchQty { get; set; }
        public int? TypeTotal { get; set; }

        public ICollection<DispatchDetailModels> DispatchDetailModelsCollection { get; set; }
        public DispatchModels()
        {
            DispatchDetailModelsCollection = new List<DispatchDetailModels>();
        }
    }
    public class DispatchApproveModels
    {
        public Guid? BaseUnitID { get; set; }
        public string PalletCode { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal ReserveQty { get; set; }
        public Guid? StockUnitID { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public string DispatchCode { get; set; }
        public string PONo { get; set; }
        public string ProductLot { get; set; }
        public Guid? ProductOwnerID { get; set; }
        public Guid? SupplierID { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double ProductWidth { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }
        public double PackageWeight { get; set; }
        public double ProductWeight { get; set; }
        public Guid? ProductUnitPriceID { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? ProductSubStatusID { get; set; }
        public decimal? Price { get; set; }
        public decimal? ConversionQty { get; set; }
        public string LocationCode { get; set; }
    }
    public class POListModels
    {
        public string PONo { get; set; }
    }
}
