using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
 public    class ModuleGroup : BaseEntity
    {
        public Guid ModuleTypeID { get; set; }
        public string  Name { get; set; }
        public string  Description { get; set; }

        public virtual ModuleType ModuleType { get; set; }

        public virtual ICollection<Module> ModuleCollection { get; set; }
        public  ModuleGroup()
        {
            ModuleCollection = new List<Module>();
        }
    }
}
