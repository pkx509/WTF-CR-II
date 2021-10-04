using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class Application : BaseEntity
    {
        public Guid AppID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Program> ProgramCollection { get; set; }

        public Application()
        {
            ProgramCollection = new List<Program>();
        }

    }
}
