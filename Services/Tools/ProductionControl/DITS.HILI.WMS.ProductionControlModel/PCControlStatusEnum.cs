namespace DITS.HILI.WMS.ProductionControlModel
{
    public enum PCControlStatusEnum
    {
        New = 10,
        InProgress = 20,
        Complete = 100
    }

    public enum PackingStatusEnum
    {
        Waiting_Receive = 10,
        Loading_In = 11,
        In_Progress = 20,
        Transfer = 30,
        PutAway = 100,
        Cancel = 102,
        Delivery = 200,
        Damage = 300,
        QAInspection = 301
    }
}
