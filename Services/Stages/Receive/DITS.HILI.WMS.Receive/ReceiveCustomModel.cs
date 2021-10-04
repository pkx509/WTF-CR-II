using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.ReceiveModel
{
    /// <summary>
    /// Receive Custom Model
    /// </summary>
    public class ReceiveCustomModel
    {

    }

    /// <summary>
    /// Recieve List Custom Model
    /// </summary>
    public class ReceiveListModel
    {
        public Guid ReceiveID { get; set; }
        public string ReceiveCode { get; set; }
        public string ReceiveTypeName { get; set; }
        public string OrderNo { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReceiveStatus { get; set; }
        public string ReceiveStatusDesc { get; set; }
        public string LineCode { get; set; }
        public string LineType { get; set; }
        public string ProductName { get; set; }
        public bool? IsProduction { get; set; }
    }

    /// <summary>
    /// Add/Edit Receive Model
    /// </summary>
    public class ReceiveHeaderModel
    {
        public Guid ReceiveID { get; set; }
        public string ReceiveCode { get; set; }
        public Guid? ReceiveTypeID { get; set; }
        public Guid? DispatchTypeID { get; set; }
        public string ReceiveType { get; set; }
        public string DispatchType { get; set; }
        public Guid? LineID { get; set; }
        public string LineCode { get; set; }
        public Guid? LocationID { get; set; }
        public string Location { get; set; }
        public Guid? ProductOwnerID { get; set; }
        public string ProductOwner { get; set; }
        public string SupplierName { get; set; }
        public DateTime? ESTReceiveDate { get; set; }
        public ReceiveStatusEnum? ReceiveStatus { get; set; }
        public string InvoiceNo { get; set; }
        public string ContainerNo { get; set; }
        public string PONo { get; set; }
        public string OrderNo { get; set; }
        public string OrderType { get; set; }
        public string Remark { get; set; }
        public bool? IsUrgent { get; set; }

        public bool? IsNormal { get; set; }
        public bool? IsCreditNote { get; set; }
        public bool? FromReprocess { get; set; }
        public bool? ToReprocess { get; set; }
        public bool? IsItemChange { get; set; }
        public bool? IsWithoutGoods { get; set; }
        public ProductStatus ProductStatus { get; set; }

        public IEnumerable<ReceiveDetailModel> ReceiveDetails { get; set; }
        public object DbFunctions { get; set; }
    }

    public class ReceiveDetailModel
    {
        public Guid ReceiveDetailID { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? EXPDate { get; set; }
        public decimal? QTY { get; set; }
        public decimal? RemainQTY { get; set; }
        public decimal? PackageQTY { get; set; }
        public decimal? ConfirmQTY { get; set; }
        public decimal? ConversionQTY { get; set; }
        public Guid? UnitID { get; set; }
        public string Unit { get; set; }
        public Guid? StatusID { get; set; }
        public string Status { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Height { get; set; }
        public string Remark { get; set; }
    }
}
