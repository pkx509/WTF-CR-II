using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.TruckQueueService
{
    public interface IQueueService : IRepository<QueueReg>
    {
        #region --dock---
        List<QueueDock> GetDockAll(out int totalRecord, int? status, string dockname);
        QueueDock GetDock(Guid id);
        ApiResponseMessage Add(QueueDock entity);
        ApiResponseMessage Modify(QueueDock entity);
        void Remove(QueueDock entity);
        #endregion

        #region --Ship From---
        List<ShippingFrom> GetShippingFromAll(out int totalRecord, int? status, string comname,string comshortname);
        ShippingFrom GetShippingFrom(Guid id);
        ApiResponseMessage Add(ShippingFrom entity);
        ApiResponseMessage Modify(ShippingFrom entity);
        void Remove(ShippingFrom entity);
        #endregion

        #region --Status---
        List<QueueStatus> GetStatusAll(out int totalRecord, int? status,string statusname);
        QueueStatus GetStatus(QueueStatusEnum id); 
        #endregion

        #region --RegisterType---
        List<QueueRegisterType> GetRegisterTypeAll(out int totalRecord, int? status, string typename);
        QueueRegisterType GetRegisterType(Guid id);
        ApiResponseMessage Add(QueueRegisterType entity);
        ApiResponseMessage Modify(QueueRegisterType entity);
        void Remove(QueueRegisterType entity);
        #endregion

        #region --Setup---
        List<QueueConfiguration> GetConfigurationAll(out int totalRecord, int? status);
        QueueConfiguration GetConfiguration(Guid id);
        ApiResponseMessage Add(QueueConfiguration entity);
        ApiResponseMessage Modify(QueueConfiguration entity);
        void Remove(QueueConfiguration entity);
        #endregion

        #region --Registeration---
        List<QueueReg> GetSearchQueue(out int totalRecord, DateTime startDate, DateTime endDate, QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize);
        List<QueueReg> GetQueueAll(out int totalRecord, QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize);
        List<QueueReg> GetInQueue(out int totalRecord, QueueStatusEnum status, string keyword);
        List<QueueReg> GetInprogress(out int totalRecord, QueueStatusEnum status, string keyword);
        List<QueueReg> GetQueueReport(out int totalRecord, DateTime startDate,DateTime endDate,Guid? shipfrom,Guid? shipTo, QueueStatusEnum status,Guid? dock, string keyword, int? pageIndex, int? pageSize);
        QueueReg GetQueue(Guid id);
        //QueueReg GetInQueue(string queueno, string dockno);
        QueueReg ChangeQueueStatus(Guid id, QueueStatusEnum status);
        QueueReg AddQueue(QueueReg queueReg);
        QueueReg ModifyQueue(QueueReg queueReg);
        void RemoveQueue(Guid id);
        QueueReg CallQueue(Guid id);
        #endregion

        QueueConfiguration GetQueueConfigurationActive(out int totalRecord);

       
    }
}
