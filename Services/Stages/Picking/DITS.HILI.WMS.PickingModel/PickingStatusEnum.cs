using System.ComponentModel;

namespace DITS.HILI.WMS.PickingModel
{
    public enum PickingStatusEnum
    {
        [Description("ทั้งหมด")]
        All = 0,

        [Description("รอหยิบ")]
        WaitingPick = 10,

        [Description("หยิบ")]
        Pick = 20,

        [Description("Pick Partial")]
        PickPartial = 25,

        [Description("เตรียมขนส่ง")]
        LoadingOut = 30,

        [Description("สำเร็จ")]
        Complete = 100,

        [Description("ยกเลิก")]
        Cancel = 101,
    }

    public enum EnumPickingStatus
    {
        PUT,
        COMPLETE,
        TRANS,
        MOVE,
        DELIVERY
    }
}
