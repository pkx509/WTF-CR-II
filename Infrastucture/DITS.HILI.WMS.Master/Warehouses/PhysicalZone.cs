using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class PhysicalZone
    {
        public System.Guid Physicalzone_Id { get; set; } // Physicalzone_ID
        public string PhysicalZone_Code { get; set; } // PhysicalZone_Code (Primary key) (length: 10)
        public string Warehouse_Code { get; set; } // Warehouse_Code (length: 10)
        [NotMapped]
        public string Warehouse_Name { get; set; }
        public string ZoneType_Code { get; set; } // ZoneType_Code (length: 10)
        public string PhysicalZone_Name { get; set; } // PhysicalZone_Name (length: 25)
        public string PhysicalZone_Short_Name { get; set; } // PhysicalZone_Short_Name (length: 2)
        public int? PhysicalZone_Status { get; set; } // PhysicalZone_Status
        public string CreateUser { get; set; } // Create_User (length: 100)
        public System.DateTime? CreateDate { get; set; } // Create_Date
        public string UpdateUser { get; set; } // Update_User (length: 100)
        public System.DateTime? UpdateDate { get; set; } // Update_Date
        public bool IsActive { get; set; } // IsActive
        public string Theme { get; set; } // Theme (length: 10)
        public decimal? Width { get; set; } // Width
        public decimal? Height { get; set; } // Height
        public decimal? XAxis { get; set; } // X_Axis
        public decimal? YAxis { get; set; } // Y_Axis
    }
}
