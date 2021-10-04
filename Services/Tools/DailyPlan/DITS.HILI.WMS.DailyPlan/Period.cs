using System;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public class Period
    {
        public Guid PeriodID { get; set; }
        public TimeSpan? P_StartTime { get; set; }
        public TimeSpan? P_EndTime { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
        public Guid? UserCreated { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? UserModified { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
