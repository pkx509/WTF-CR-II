using System.ComponentModel;

namespace DITS.HILI.WMS.DailyPlanModel
{
    public enum LineTypeEnum
    {
        [Description("All")]
        All,
        [Description("NP")]
        NP,
        [Description("SP")]
        SP,
    }
    public enum DailyPlanStatusEnum
    {
        [Description("DailyPlan")]
        DailyPlan = 0,
        [Description("Receive")]
        Receive = 1,
    }
    public enum OrderTypeStatusEnum
    {
        [Description("LOCAL")]
        LOCAL,
        [Description("EXPORT")]
        EXPORT,
    }
}
