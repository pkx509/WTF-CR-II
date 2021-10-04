using System.ComponentModel;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public enum ConsolidateStatusEnum
    {
        [Description("ทั้งหมด")]
        All = 0,
        [Description("ใหม่")]
        LoadingOut = 10,
        [Description("รอยืนยัน")]
        WaitingConfirm = 20,
        [Description("สำเร็จ")]
        Complete = 100,
        [Description("ยกเลิก")]
        Cancel = 102
    }
}
