using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductUnit : BaseEntity
    {
        public Guid ProductUnitID { get; set; }
        public Guid ProductID { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal PalletQTY { get; set; }
        public bool IsBaseUOM { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Cubicmeters { get; set; }
        public double ProductWeight { get; set; }
        public double PackageWeight { get; set; }
        public string URLImage { get; set; }

        /// <summary>
        /// defalut = 1 : 1 = multiply , 2 = divide
        /// </summary>
        public decimal? ConversionMark { get; set; }

        public virtual Product Product { get; set; }

        public string UnitAndPalletQTY
        {
            get => Name + " (" + PalletQTY.ToString() + ")";
            set { }
        }

        //public virtual ICollection<StockInfo> StockInfoCollection { get; set; }
        //public virtual ICollection<StockInfo> StockInfoBaseUOMCollection { get; set; }
        //public virtual ICollection<StockInfo> StockInfoPriceUOMCollection { get; set; }
        public ProductUnit()
        {
            //StockInfoCollection = new List<StockInfo>();
            //StockInfoBaseUOMCollection = new List<StockInfo>();
            //StockInfoPriceUOMCollection = new List<StockInfo>();
        }
    }
}
