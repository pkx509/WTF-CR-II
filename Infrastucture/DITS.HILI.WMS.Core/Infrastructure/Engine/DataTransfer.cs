using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine
{
    /// <summary>
    /// Data Transfer and receive stage
    /// </summary>
    public class DataTransfer
    {
        public Guid InstanceID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public Guid PackagePrevID { get; set; }
        public Guid PackageNextID { get; set; }
        public bool Start { get; set; }
        public int Sequence { get; set; }

        /// <summary>
        /// ID Refernece
        /// </summary>
        public Guid BaseID { get; set; }
        /// <summary>
        /// Document Number Reference
        /// </summary>
        public string ReferenceID { get; set; } //Activity ID
        /// <summary>
        /// Item Line ID Reference
        /// </summary>
        public Guid LineID { get; set; }
        public Guid? ProductOwnerID { get; set; }
        public Guid? SupplierID { get; set; }
        public Guid ProductID { get; set; }
        public string Lot { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double? ProductWeight { get; set; }
        public double? ProductPackageWeight { get; set; }
        public double? ProductPackageWidth { get; set; }
        public double? ProductPackageLength { get; set; }
        public double? ProductPackageHeight { get; set; }
        public decimal? Price { get; set; }
        public Guid? PriceUnitID { get; set; }
        public string Remark { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public double? Quantity { get; set; }
        public double? ConversionQty { get; set; }
        public Guid? StockUnitID { get; set; }
        public Guid? BaseUnitID { get; set; }
        public string PalletCode { get; set; }
        public Guid? LocationID { get; set; }
    }
}
