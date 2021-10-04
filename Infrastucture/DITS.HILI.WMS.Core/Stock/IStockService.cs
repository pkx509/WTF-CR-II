using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Stock;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.Stock
{
    public interface IStockService : IRepository<StockInfo>
    {
        bool Incomming(List<StockInOutModel> stockIn, StockTransactionTypeEnum transType = StockTransactionTypeEnum.Incomming);
        bool Incomming2(List<StockInOutModel> stockIn, IUnitOfWork uow);
        bool Outgoing(List<StockInOutModel> stockOut, StockTransactionTypeEnum transType = StockTransactionTypeEnum.Outgoing);
        bool Outgoing2(List<StockInOutModel> stockOut, IUnitOfWork uow);
        bool StockReserve(List<StockInOutModel> stockRestore, IUnitOfWork uow);
        bool RestoreReserve(List<StockInOutModel> stockRestore, IUnitOfWork uow);
        bool ReceivetoReprocess(StockInternalRecModel stock, IUnitOfWork uow);
        bool OutgoingAndIncomming(List<StockInOutModel> stockInOut);
        bool ChangeStatus(List<StockInOutModel> stockOut, List<StockInOutModel> stockChange);
        bool InspectionReclassify(List<StockInOutModel> stockOut, List<StockInOutModel> stockChange);

        bool AdjustReserve(StockSearch stock, StockReserveTypeEnum reserveType);
        bool AdjustReserve_(List<StockSearch> stock, StockReserveTypeEnum reserveType);
        bool AdjustStockTrans(StockInOutModel stock);
        //List<DataTransfer> Incomming(List<DataTransfer> dataTrans);
        //StockBalance Outgoing(List<DataTransfer> dataTrans);
        //void Reserve(List<DataTransfer> dataTrans);
        //void CancelReserve(List<DataTransfer> dataTrans);
        //StockBalance GetStockBalance(Guid balanceId);
        //List<StockBalance> GetStockBalanceList(Guid stockInfoID);
        bool AdjustIncomming(List<StockInOutModel> stockIn);
        bool AdjustOutgoing(List<StockInOutModel> stockOut);
        bool AdjustOthergoing(List<StockInOutModel> stockOut);
        bool UpdateLocationOutgoingAndIncomming(List<StockInOutModel> stockInOut);
        bool TransferOutgoingAndIncomming(List<StockInOutModel> stockInOut);
    }
}
