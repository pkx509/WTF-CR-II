using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.ReceiveService
{
    public interface IReceiveService : IRepository<Receive>
    {
        bool Cancel(Guid id);

        ReceiveDetail GetReceiveDetail(Guid receiveDetailID);
        ReceiveDetail GetReceiveDetailByProductCode(Guid receiveID, string productCode);
        ReceiveDetail GetReceiveDetailByPallet(Guid receiveID, string palletCode);

        ReceiveHeaderModel GetReceiveByID(Guid id);
        bool Save(ReceiveHeaderModel receiveHeader);
        bool SaveInternalReceive(ReceiveHeaderModel receiveHeader);
        bool ConfirmInternalReceive(ReceiveHeaderModel receiveHeader);
        bool GenerateDispatch(Guid receiveID);

        Receive GetByID(Guid id);
        Receive GetByReceiveCode(string code);

        List<ReceiveListModel> GetAll(DateTime? estDate, Guid lineID, ReceiveStatusEnum status, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ReceiveHeaderModel> GetAllInternalReceive(DateTime? estDate, ReceiveStatusEnum status, Guid receiveTypeID, string ReceiveCode, string OrderNo, string PONo, out int totalRecords, int? pageIndex, int? pageSize);

        List<Receive> GetAll(Guid? productOwnerId, ReceiveStatusEnum? status, string keyword, DateTime? sdte, DateTime? edte, out int totalRecords, int? pageIndex, int? pageSize);

        bool SendtoProductionControl(List<Guid> receiveIDs);

        #region [ Receiving ]
        void Receiving(Receiving entity);
        void FinishReceiving(Guid id);

        //TODO: Delete Receiving
        void RemoveReceiving(Guid id);
        List<Receiving> GetReceivingNo(Guid receiveid);
        List<Receiving> GetReceivingList(string grnNo);

        //bool ReceivePallet(string palletCode, decimal receiveQty, string suggestLocation);
        //bool ConfirmKeep(string palletCode, decimal receiveQty, string locationCod);
        //PalletTagModel GetReceivingByPalletCode(string palletCode, List<ReceivingStatusEnum> status);
        #endregion
    }
}
