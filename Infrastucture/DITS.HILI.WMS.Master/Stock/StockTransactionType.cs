namespace DITS.HILI.WMS.MasterModel.Stock
{
    public enum StockTransactionTypeEnum
    {
        Incomming = 10,
        Outgoing = 20,
        Reserve = 30,
        CancelReserve = 40,
        ChangeStatusIn = 50,
        ChangeStatusOut = 51,
        Relocation = 60,
        AdjustIn = 70,
        AdjustOut = 71,
        QAInspectionIn = 80,
        QAInspectionOut = 81,
        Transfer412In = 90,
        Transfer412Out = 91
    }
}
