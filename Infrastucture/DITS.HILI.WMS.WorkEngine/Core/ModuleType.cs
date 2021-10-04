using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
    public class ModuleType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ModuleGroup> ModuleGroupCollection { get; set; }
        public ModuleType()
        {
            ModuleGroupCollection = new List<ModuleGroup>();
        }

    }
}
