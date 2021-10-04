namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ItfInterfaceMappingDocument
    {
        public string DocumentCode { get; set; } // DocumentCode (Primary key) (length: 10)
        public int GroupTable { get; set; } // TableNo (Primary key)
        public string Description { get; set; } // TableDesc (Primary key) (length: 255)


    }
}
