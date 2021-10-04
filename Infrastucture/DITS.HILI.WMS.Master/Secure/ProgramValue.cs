using System;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class ProgramValue
    {
        //public Guid ProgramValeuID { get; set; }
        public Guid ProgramValueID { get; set; }
        //public string Code { get; set; }
        public string Value { get; set; }
        public string LanguageCode { get; set; }
        public Guid? ProgramID { get; set; }
        //public string Name { get; set; } 
        public virtual Program Program { get; set; }
    }
}
