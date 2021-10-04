using System.ComponentModel;

namespace DITS.HILI.WMS.PutAwayModel
{
    public enum PutAwayStatusEnum
    {

        [Description("Move")]
        Move = 101,
        [Description("Trans")]
        Trans = 201,

        [Description("ReceiveTrans")]
        ReceiveTrans = 202,

        [Description("PutAway")]
        PutAway = 300
    }


}
