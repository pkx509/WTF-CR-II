using System.ComponentModel;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public enum RegisterTruckEnum
    {
        [Description("Export")]
        Export = 0,
        [Description("Local")]
        Local = 1
    }

    public enum EnumPalletStatus
    {
        DM,
        HOLD,
        INS,
        NM,
        PD,
        REJECT,
        DUMMY,
        GOOD,
        DELIVERY
    }
    public enum ShippingStatusEnum
    {
        [Description("New")]
        New = 10,

        [Description("Assign")]
        Assign = 20,

        [Description("Complete")]
        Complete = 100,

        [Description("Cancel")]
        Cancel = 102
    }
}
