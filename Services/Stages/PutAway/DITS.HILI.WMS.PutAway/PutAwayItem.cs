using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayItem : BaseEntity
    {
        public Guid PutAwayItemID { get; set; }
        public decimal RemainQuantity { get; set; }
        public Guid ProductID { get; set; }
        public string Lot { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public double PackageWeight { get; set; }
        public double ProductWeight { get; set; }
        public double ProductWidth { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }
        public string PalletCode { get; set; }


        public decimal? Price { get; set; }
        public bool IsComplete { get; set; }
        public Guid? InstanceID { get; set; }
        public Guid DocumentTypeID { get; set; }
        //public Guid PackagePrevID { get; set; }
        //public Guid? PackageNextID { get; set; }
        public bool? Start { get; set; }
        public Guid ReferenceBaseID { get; set; }
        public Guid LineID { get; set; }
        public Guid? SupplierID { get; set; }
        public Guid? ProductUnitPriceID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public bool? IsReserve { get; set; }
        public int? Sequence { get; set; }
        public string ReferenceCode { get; set; }
        public Guid? StockBalanceID { get; set; }
        public Guid FromLocationID { get; set; }
        public Guid? SuggestionLocationID { get; set; }

        [NotMapped]
        public virtual ICollection<PutAway> PutAwayCollection { get; set; }

        [NotMapped]
        public Location Location { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public PutAwayItem()
        {
            PutAwayCollection = new List<PutAway>();
        }
    }
}
