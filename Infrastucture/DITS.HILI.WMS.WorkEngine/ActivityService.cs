using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngine.Service
{
    public class ActivityService : Repository<Activity>, IActivityService
    {
        public ActivityService(IDbContext context)
            : base(context)
        {
        }

        public override Activity Add(Activity entity)
        {  
            return base.Add(entity);
        }
          
    }
}
