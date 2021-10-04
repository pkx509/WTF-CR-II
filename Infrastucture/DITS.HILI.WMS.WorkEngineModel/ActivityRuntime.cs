using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel
{
    [NotMapped]
    public class ActivityRuntime
    {
        public static List<Activity> ActivityCollection { get; set; }

        public static void OnLoad()
        { 
        }
    }
}
