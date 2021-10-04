using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class Product : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid? ProductGroupLevel3ID { get; set; }
        public Guid? ProductShapeID { get; set; }
        public Guid? ProductBrandID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductModel { get; set; }
        public double Age { get; set; }
        public decimal? SafetyStockQTY { get; set; }
        public string URLImage { get; set; }

        public virtual ProductGroupLevel3 ProductGroupLevel3 { get; set; }
        //public virtual ProductShape ProductShape { get; set; }
        //public virtual ProductBrand ProductBrand { get; set; }

        public virtual ICollection<ProductCodes> CodeCollection { get; set; }
        //public List<ProductCodes> pCode { get; set; }
        public virtual ICollection<ProductColor> ColorCollection { get; set; }
        public virtual ICollection<ProductUnit> UnitCollection { get; set; }
        public virtual ICollection<ProductSafetyStock> SafetyStockCollection { get; set; }
        public virtual ICollection<ProductReplacement> ProductParentReplacementCollection { get; set; }
        public virtual ICollection<ProductReplacement> ProductChildReplacementCollection { get; set; }
        public virtual ICollection<ProductOfProductOwner> ProductOfProductOwnerCollection { get; set; }

        public virtual ICollection<StockInfo> StockInfoCollection { get; set; }
        [NotMapped]
        public string ProductCodeOld { get; set; }
        private string _productCode { get; set; }
        [NotMapped]
        public string ProductCode
        {
            get
            {
                ProductCodes code = ((List<ProductCodes>)CodeCollection).Find(x => x.CodeType == ProductCodeTypeEnum.Stock);
                if (code == null && _productCode != "")
                {
                    return _productCode;
                }
                else if (code != null)
                {
                    code.Product = null;
                    _productCode = $"{code.Code} : {Name}";
                }

                return _productCode;
            }
            set => _productCode = value;
        }
        [NotMapped]
        public string Code
        {
            get
            {
                ProductCodes code = ((List<ProductCodes>)CodeCollection).Find(x => x.CodeType == ProductCodeTypeEnum.Stock);
                if (code == null && _productCode != "")
                {
                    return _productCode;
                }
                else if (code != null)
                {
                    code.Product = null;
                    _productCode = code.Code;
                }

                return _productCode;
            }
            set => _productCode = value;
        }
        private string _unitName { get; set; }
        [NotMapped]
        public string BaseUnitName
        {
            get
            {
                ProductUnit unit = ((List<ProductUnit>)UnitCollection).Find(x => x.IsBaseUOM);
                if (unit == null && _unitName != "")
                {
                    return _unitName;
                }
                else if (unit != null)
                {
                    _unitName = unit.Name;
                }
                return _unitName;
            }
            set => _unitName = value;
        }

        public Product()
        {
            CodeCollection = new List<ProductCodes>();
            ColorCollection = new List<ProductColor>();
            UnitCollection = new List<ProductUnit>();
            SafetyStockCollection = new List<ProductSafetyStock>();
            ProductParentReplacementCollection = new List<ProductReplacement>();
            ProductChildReplacementCollection = new List<ProductReplacement>();
            ProductOfProductOwnerCollection = new List<ProductOfProductOwner>();
            StockInfoCollection = new List<StockInfo>();
        }

        [NotMapped]
        public Guid ProductUOMTemplateID { get; set; }
        [NotMapped]
        public string ProdCode { get; set; }

    }
}
