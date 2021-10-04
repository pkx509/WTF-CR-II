using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel
{
    /// <summary>
    /// Data Transfer and receive stage
    /// </summary>
    public class DataTransfer : BaseProductProfile
    {
        public Guid InstanceID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public Guid PackagePrevID { get; set; }
        public Guid PackageNextID { get; set; }
        public bool Start { get; set; }
        public int Sequence { get; set; }


        /// <summary>
        /// Document Number Reference
        /// </summary>
        public string ReferenceCode { get; set; }
        public Guid ReferenceBaseID { get; set; }
        public Guid StockBalanceID { get; set; }
        /// <summary>
        /// Item Line ID Reference
        /// </summary>
        public Guid LineID { get; set; }

        public Guid? SupplierID { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductUnitPriceID { get; set; }

        public decimal Quantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }

        public Guid? LocationID { get; set; }
        public bool IsReserve { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string StockUOMNmae { get; set; }
        [NotMapped]
        public string BaseUOMName { get; set; }
        [NotMapped]
        public string PackageName { get; set; }

        [NotMapped]
        public virtual ProductUnit ProductPriceUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductUOM { get; set; }
        [NotMapped]
        public virtual ProductUnit ProductBaseUOM { get; set; }

        [NotMapped]
        public virtual DocumentType DocumentType { get; set; }
        [NotMapped]
        public virtual Location Location { get; set; }


    }
}
