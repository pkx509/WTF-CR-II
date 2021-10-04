using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PutAwayModel
{
    /// <summary>
    /// Put Away Class
    /// </summary>
    public class PutAway : BaseEntity
    {
        public Guid PutAwayID { get; set; }
        public string PutAwayJobCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public Guid FromLocationID { get; set; }
        public Guid? SuggestionLocationID { get; set; }
        public PutAwayStatusEnum PutAwayStatus { get; set; }
        public Guid? ProductID { get; set; }
        public string Lot { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public double PackageWeight { get; set; }
        public double ProductWeight { get; set; }
        public double ProductWidth { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }
        public string PalletCode { get; set; }
        public Guid ProductOwnerID { get; set; }
        public DateTime PutAwayDate { get; set; }
        public Guid? SupplierID { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductUnitPriceID { get; set; }
        public Guid? ZoneID { get; set; }
        public Guid? WarehouseID { get; set; }
        public Guid? DocumentTypeID { get; set; }
        [NotMapped]
        public virtual ICollection<PutAwayMap> PutAwayMapCollection { get; set; }
        [NotMapped]
        public virtual ICollection<PutAwayDetail> PutAwayDetailCollection { get; set; }
        [NotMapped]
        public Location FromLocation { get; set; }
        public PutAway()
        {
            PutAwayDetailCollection = new List<PutAwayDetail>();
            PutAwayMapCollection = new List<PutAwayMap>();
        }
    }
}
