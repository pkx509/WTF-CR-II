using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.TruckQueueModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.TruckQueueService
{
    public class QueueService : Repository<QueueReg>, IQueueService
    {
        private readonly IRepository<QueueDock> QueueDockService;
        private readonly IRepository<QueueRegisterType> QueueRegisterTypeService;
        private readonly IRepository<QueueConfiguration> QueueSetupService;
        private readonly IRepository<QueueStatus> QueueStatusService;
        private readonly IRepository<ShippingFrom> ShippingFromService;
        private readonly IRepository<ShippingTo> ShippingToService;
        private readonly IRepository<QueueRunning> QueueRunningService;
        private readonly IRepository<Province> ProvinceService;
        private readonly IRepository<TruckType> TruckTypeService;
        private readonly IRepository<Employee> EmployeeService;
        private readonly IRepository<UserAccounts> UserAccountsService;
        public QueueService(IUnitOfWork context) : base(context)
        {
            QueueDockService = context.Repository<QueueDock>();
            QueueRegisterTypeService = context.Repository<QueueRegisterType>();
            QueueSetupService = context.Repository<QueueConfiguration>();
            QueueStatusService = context.Repository<QueueStatus>();
            ShippingFromService = context.Repository<ShippingFrom>();
            ShippingToService = context.Repository<ShippingTo>();
            QueueRunningService = context.Repository<QueueRunning>();
            ProvinceService = context.Repository<Province>();
            TruckTypeService = context.Repository<TruckType>();
            EmployeeService = context.Repository<Employee>();
            UserAccountsService = context.Repository<UserAccounts>();
        }

        #region --QueueDock
        public List<QueueDock> GetDockAll(out int totalRecord, int? status, string dockname)
        {
            var list = QueueDockService.Query().Get();
            if (status.HasValue && status == 1)
            {
                list = list.Where(e => e.IsActive);
            }
            else if (status.HasValue && status == 0)
            {
                list = list.Where(e => !e.IsActive);
            }
            if (!string.IsNullOrEmpty(dockname))
            {
                list = list.Where(e => e.QueueDockName.Contains(dockname));
            }
            totalRecord = list.Count();
            return list.ToList();
        }
        public QueueDock GetDock(Guid id)
        {
            return QueueDockService.FirstOrDefault(e => e.QueueDockID == id);
        }
        public ApiResponseMessage Add(QueueDock entity)
        {
            try
            {
                bool ok = QueueDockService.Any(x => x.QueueDockName == entity.QueueDockName && x.IsActive == true);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.UserCreated = UserID;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.QueueDockID = Guid.NewGuid();
                    QueueDockService.Add(entity);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.QueueDockID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public ApiResponseMessage Modify(QueueDock entity)
        {
            try
            {
                bool ok = QueueDockService.Any(x => x.QueueDockName == entity.QueueDockName && x.IsActive == true && x.QueueDockID != entity.QueueDockID);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueDock _current = QueueDockService.FirstOrDefault(x => x.QueueDockID == entity.QueueDockID);
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    _current.QueueDockName = entity.QueueDockName;
                    _current.QueueDockDesc = entity.QueueDockDesc;
                    _current.Remark = entity.Remark;
                    QueueDockService.Modify(_current);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.QueueDockID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void Remove(QueueDock entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueDock _current = QueueDockService.FirstOrDefault(x => x.QueueDockID == entity.QueueDockID);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    QueueDockService.Modify(_current);
                    scope.Complete();
                }

            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        #endregion

        #region --- QueueStatus
        public List<QueueStatus> GetStatusAll(out int totalRecord, int? status, string statusname)
        {
            var list = QueueStatusService.Query().Get();
            if (status.HasValue && status == 1)
            {
                list = list.Where(e => e.IsActive);
            }
            else if (status.HasValue && status == 0)
            {
                list = list.Where(e => !e.IsActive);
            }
            if (!string.IsNullOrEmpty(statusname))
            {
                list = list.Where(e => e.QueueStatusName.Contains(statusname));
            }
            totalRecord = list.Count();
            return list.ToList();
        }

        public QueueStatus GetStatus(QueueStatusEnum id)
        {
            return QueueStatusService.FirstOrDefault(e => e.QueueStatusID == (int)id);
        }

        //public ApiResponseMessage Add(QueueStatus entity)
        //{
        //    try
        //    {
        //        bool ok = QueueStatusService.Any(x => x.QueueStatusName == entity.QueueStatusName && x.IsActive == true);
        //        if (ok)
        //        {
        //            throw new HILIException("MSG00009");
        //        }
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            entity.IsActive = true;
        //            entity.DateCreated = DateTime.Now;
        //            entity.UserCreated = UserID;
        //            entity.DateModified = DateTime.Now;
        //            entity.UserModified = UserID;
        //            entity.QueueStatusID = (QueueStatusService.Any(e => e.IsActive) ? QueueStatusService.Where(e => e.IsActive).Max(e => e.QueueStatusID) + 1 : 1); 
        //            QueueStatusService.Add(entity);
        //            ApiResponseMessage _result = new ApiResponseMessage
        //            {
        //                text = entity.QueueStatusID.ToString()
        //            };
        //            scope.Complete();
        //            return _result;
        //        }
        //    }
        //    catch (HILIException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}
        //public ApiResponseMessage Modify(QueueStatus entity)
        //{
        //    try
        //    {
        //        bool ok = QueueStatusService.Any(x => x.QueueStatusName == entity.QueueStatusName && x.IsActive == true && x.QueueStatusID != entity.QueueStatusID);
        //        if (ok)
        //        {
        //            throw new HILIException("MSG00009");
        //        }
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            QueueStatus _current = QueueStatusService.FirstOrDefault(x => x.QueueStatusID == entity.QueueStatusID);
        //            _current.IsActive = true;
        //            _current.DateModified = DateTime.Now;
        //            _current.UserModified = UserID;
        //            _current.QueueStatusName = entity.QueueStatusName;
        //            _current.QueueStatusDesc = entity.QueueStatusDesc;
        //            _current.Remark = entity.Remark;
        //            //_current.IsCancel = entity.IsCancel;
        //            //_current.IsCompleted = entity.IsCompleted;
        //            //_current.IsInQueue = entity.IsInQueue;
        //            //_current.IsWaiting = entity.IsWaiting; 
        //            QueueStatusService.Modify(_current);
        //            ApiResponseMessage _result = new ApiResponseMessage
        //            {
        //                text = entity.QueueStatusID.ToString()
        //            };
        //            scope.Complete();
        //            return _result;
        //        }
        //    }
        //    catch (HILIException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}
        //public void Remove(QueueStatus entity)
        //{
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            QueueStatus _current = QueueStatusService.FirstOrDefault(x => x.QueueStatusID == entity.QueueStatusID); 
        //            _current.IsActive = false;
        //            _current.DateModified = DateTime.Now;
        //            _current.UserModified = UserID;
        //            QueueStatusService.Modify(_current);
        //            scope.Complete();
        //        }
        //    }
        //    catch (HILIException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}
        #endregion

        #region --- ShippingFrom  
        public List<ShippingFrom> GetShippingFromAll(out int totalRecord, int? status, string comname, string comshortname)
        {
            var list = ShippingFromService.Query().Get();
            if (status.HasValue && status == 1)
            {
                list = list.Where(e => e.IsActive);
            }
            else if (status.HasValue && status == 0)
            {
                list = list.Where(e => !e.IsActive);
            }
            if (!string.IsNullOrEmpty(comname))
            {
                list = list.Where(e => e.Name.Contains(comname));
            }
            if (!string.IsNullOrEmpty(comshortname))
            {
                list = list.Where(e => e.ShortName.Contains(comshortname));
            }
            totalRecord = list.Count();
            return list.ToList();
        }
        public ShippingFrom GetShippingFrom(Guid id)
        {
            return ShippingFromService.FirstOrDefault(e => e.ShipFromId == id);
        }
        public ApiResponseMessage Add(ShippingFrom entity)
        {
            try
            {
                bool ok = ShippingFromService.Any(x => x.Name == entity.Name && x.IsActive == true);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.UserCreated = UserID;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.ShipFromId = Guid.NewGuid();
                    ShippingFromService.Add(entity);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ShipFromId.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public ApiResponseMessage Modify(ShippingFrom entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ShippingFrom _current = ShippingFromService.FirstOrDefault(x => x.ShipFromId == entity.ShipFromId && x.IsActive == true);
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    _current.Remark = entity.Remark;
                    _current.Address = entity.Address;
                    _current.Description = entity.Description;
                    _current.Name = entity.Name;
                    _current.Remark = entity.Remark;
                    _current.ShortName = entity.ShortName;
                    ShippingFromService.Modify(_current);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ShipFromId.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void Remove(ShippingFrom entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ShippingFrom _current = ShippingFromService.FirstOrDefault(x => x.ShipFromId == entity.ShipFromId);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    ShippingFromService.Modify(_current);
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        #endregion

        #region --QueueRegisterType
        public List<QueueRegisterType> GetRegisterTypeAll(out int totalRecord, int? status, string typename)
        {
            var list = QueueRegisterTypeService.Query().Get();
            if (status.HasValue && status == 1)
            {
                list = list.Where(e => e.IsActive);
            }
            else if (status.HasValue && status == 0)
            {
                list = list.Where(e => !e.IsActive);
            }
            if (!string.IsNullOrEmpty(typename))
            {
                list = list.Where(e => e.QueueRegisterTypeName.Contains(typename));
            }
            totalRecord = list.Count();
            return list.ToList();
        }
        public QueueRegisterType GetRegisterType(Guid id)
        {
            return QueueRegisterTypeService.FirstOrDefault(e => e.QueueRegisterTypeID == id);
        }
        public ApiResponseMessage Add(QueueRegisterType entity)
        {
            try
            {
                bool ok = QueueRegisterTypeService.Any(x => x.QueueRegisterTypeName == entity.QueueRegisterTypeName && x.IsActive == true);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.UserCreated = UserID;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.QueueRegisterTypeID = Guid.NewGuid();
                    QueueRegisterTypeService.Add(entity);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.QueueRegisterTypeID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public ApiResponseMessage Modify(QueueRegisterType entity)
        {
            try
            {
                bool ok = QueueRegisterTypeService.Any(x => x.QueueRegisterTypeName == entity.QueueRegisterTypeName && x.IsActive == true && x.QueueRegisterTypeID != entity.QueueRegisterTypeID);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueRegisterType _current = QueueRegisterTypeService.FirstOrDefault(x => x.QueueRegisterTypeID == entity.QueueRegisterTypeID);
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    _current.QueueRegisterTypeName = entity.QueueRegisterTypeName;
                    _current.QueueRegisterTypeDesc = entity.QueueRegisterTypeDesc;
                    _current.Remark = entity.Remark;
                    QueueRegisterTypeService.Modify(_current);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.QueueRegisterTypeID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void Remove(QueueRegisterType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueRegisterType _current = QueueRegisterTypeService.FirstOrDefault(x => x.QueueRegisterTypeID == entity.QueueRegisterTypeID);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    QueueRegisterTypeService.Modify(_current);
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

        #endregion

        #region --- QueueConfiguration
        public QueueConfiguration GetQueueConfigurationActive(out int totalRecord)
        {
            totalRecord = 1;
            return QueueSetupService.FirstOrDefault(e => e.IsActive);
        }

        public List<QueueConfiguration> GetConfigurationAll(out int totalRecord, int? status)
        {
            var list = QueueSetupService.Query().Get();
            if (status.HasValue && status == 1)
            {
                list = list.Where(e => e.IsActive);
            }
            else if (status.HasValue && status == 0)
            {
                list = list.Where(e => !e.IsActive);
            }
            totalRecord = list.Count();
            return list.ToList();
        }
        public QueueConfiguration GetConfiguration(Guid id)
        {
            return QueueSetupService.FirstOrDefault(e => e.ConfigurationID == id);
        }
        public ApiResponseMessage Add(QueueConfiguration entity)
        {
            try
            {
                bool ok = QueueSetupService.Any(x => x.IsActive == true);
                if (ok)
                {
                    throw new HILIException("MSG00009");
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.UserCreated = UserID;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.ConfigurationID = Guid.NewGuid();
                    QueueSetupService.Add(entity);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ConfigurationID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public ApiResponseMessage Modify(QueueConfiguration entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueConfiguration _current = QueueSetupService.FirstOrDefault(x => x.ConfigurationID == entity.ConfigurationID && x.IsActive == true);
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    _current.EnableMessage = entity.EnableMessage;
                    _current.Message = entity.Message;
                    _current.StartHour = entity.StartHour;
                    _current.StartMinute = _current.StartMinute;
                    _current.Remark = entity.Remark;
                    QueueSetupService.Modify(_current);
                    ApiResponseMessage _result = new ApiResponseMessage
                    {
                        text = entity.ConfigurationID.ToString()
                    };
                    scope.Complete();
                    return _result;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void Remove(QueueConfiguration entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueConfiguration _current = QueueSetupService.FirstOrDefault(x => x.ConfigurationID == entity.ConfigurationID);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    QueueSetupService.Modify(_current);
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        #endregion

        public List<QueueReg> GetSearchQueue(out int totalRecord, DateTime startDate, DateTime endDate, QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize)
        {
            //var config = QueueSetupService.FirstOrDefault(e => e.IsActive);
            //if (config == null)
            //{
            //    config = new QueueConfiguration()
            //    {
            //        StartHour = 7,
            //        StartMinute = 0
            //    };
            //}
            var beDate = new DateTime(startDate.Year, startDate.Month, (startDate.Day), 0,0, 0);
            // endDate = endDate.AddDays(1);
            var finDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0);// ;.AddSeconds(-1);


            var list = (from q in Where(e => e.IsActive)

                        join st in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals st.QueueStatusID into GSt
                        from stat in GSt.DefaultIfEmpty()

                        join r in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals r.QueueRegisterTypeID into greg
                        from reg in greg.DefaultIfEmpty()

                        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
                        from dock in gd.DefaultIfEmpty()

                        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
                        from shipForm in gsf.DefaultIfEmpty()

                        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
                        from shipTo in gst.DefaultIfEmpty()

                        //join p in ProvinceService.Where(e => e.IsActive) on q.TruckRegProviceId equals p.Province_Id into gp
                        //from prov in gp.DefaultIfEmpty()

                        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
                        from truck in gtr.DefaultIfEmpty() 
                        where q.QueueDate>= beDate && q.QueueDate<= finDate
                        /*q.QueueDate == currentDate || (q.TimeOut.HasValue && EntityFunctions.TruncateTime(q.TimeOut) == currentDate)
                        || (q.QueueStatusID == (int)QueueStatusEnum.Loading
                            || q.QueueStatusID == (int)QueueStatusEnum.Register
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitingCall
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitingDocument
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitinLoad)*/
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo,/* Province = prov,*/ truck }).ToList();
            var results = (from t in list
                           where (status == QueueStatusEnum.All ? true : t.Queue.QueueStatusID == (int)status)
                           && (string.IsNullOrEmpty(keyword) ? true : (t.Queue.QueueNo.Contains(keyword) || t.Queue.PONO.Contains(keyword) || t.Queue.TruckRegNo.Contains(keyword)
                            || (t.Dock != null && t.Dock.QueueDockName.Contains(keyword))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))))
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.Name : "",
                               ShippTo = t.shipTo != null ? t.shipTo.Name : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo,
                              // TruckRegProvice = t.Province != null ? t.Province.Name : "",
                               //TruckRegProviceId = t.Queue.TruckRegProviceId,
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified
                           }).ToList();
            totalRecord = results.Count();
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int skip = (pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault();
                int take = pageSize.GetValueOrDefault();
                if (totalRecord - skip < take)
                {
                    take = totalRecord - skip;
                }
                results = results.OrderByDescending(x => x.QueueNo).Skip(skip).Take(take).ToList();
            }

            return results.ToList();
        }

        public List<QueueReg> GetQueueAll(out int totalRecord, QueueStatusEnum status, string keyword, int? pageIndex, int? pageSize)
        {
            var config = QueueSetupService.FirstOrDefault(e => e.IsActive);
            if (config == null)
            {
                config = new QueueConfiguration()
                {
                    StartHour = 7,
                    StartMinute = 0
                };
            } 
            var currentDate = QueueRunningService.Any(e => e.IsActive) ? QueueRunningService.Where(e => e.IsActive).Max(e => e.QueuDate) : DateTime.Today;
            //var startDate = currentDate.AddHours(config.StartHour);
            //startDate = startDate.AddMinutes(config.StartMinute);
            //var endate = startDate.AddDays(1).AddSeconds(-1); 

            var list = (from q in Where(e => e.IsActive)
                        join stat in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals stat.QueueStatusID
                        join r in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals r.QueueRegisterTypeID into greg
                        from reg in greg.DefaultIfEmpty()
                        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
                        from dock in gd.DefaultIfEmpty()

                        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
                        from shipForm in gsf.DefaultIfEmpty()

                        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
                        from shipTo in gst.DefaultIfEmpty()

                        //join p in ProvinceService.Where(e => e.IsActive) on q.TruckRegProviceId equals p.Province_Id into gp
                        //from prov in gp.DefaultIfEmpty()
                        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
                        from truck in gtr.DefaultIfEmpty()


                        where q.QueueDate == currentDate || (q.TimeOut.HasValue && EntityFunctions.TruncateTime(q.TimeOut) == currentDate)
                        || (q.QueueStatusID == (int)QueueStatusEnum.Loading
                            || q.QueueStatusID == (int)QueueStatusEnum.Register
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitingCall
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitingDocument
                            || q.QueueStatusID == (int)QueueStatusEnum.WaitinLoad)
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo,/* Province = prov, */truck }).ToList();
            var results = (from t in list
                           where (status == QueueStatusEnum.All ? true : t.Queue.QueueStatusID == (int)status)
                           && (string.IsNullOrEmpty(keyword) ? true : (t.Queue.QueueNo.Contains(keyword) || t.Queue.PONO.Contains(keyword) || t.Queue.TruckRegNo.Contains(keyword)
                            || (t.Dock != null && t.Dock.QueueDockName.Contains(keyword))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))))
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.Name : "",
                               ShippTo = t.shipTo != null ? t.shipTo.Name : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo,
                               //TruckRegProvice = t.Province != null ? t.Province.Name : "",
                               //TruckRegProviceId = t.Queue.TruckRegProviceId,
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified
                           }).ToList();
            totalRecord = results.Count();
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int skip = (pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault();
                int take = pageSize.GetValueOrDefault();
                if(totalRecord- skip < take)
                {
                    take = totalRecord - skip;
                }
                results = results.OrderByDescending(x => x.QueueNo).Skip(skip).Take(take).ToList();
            } 
          
            return results.ToList();
        }

        public List<QueueReg> GetInQueue(out int totalRecord, QueueStatusEnum status, string keyword)
        { 
            var nowDate = DateTime.Now.AddSeconds(-10);
            var list = (from q in Where(e => e.IsActive)
                        join stat in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals stat.QueueStatusID
                        join reg in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals reg.QueueRegisterTypeID
                        join dock in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals dock.QueueDockID
                        join shipForm in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals shipForm.ShipFromId
                        join shipTo in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals shipTo.ShipToId 
                        join truck in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals truck.TruckTypeID
                        where (q.QueueStatusID == (int)QueueStatusEnum.Completed && q.TimeOut.Value >= nowDate)
                        || (q.QueueStatusID == (int)QueueStatusEnum.Loading)
                        || (q.QueueStatusID == (int)QueueStatusEnum.WaitinLoad)
                        orderby new { dock.QueueDockName, q.TimeIn }
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo, truck }).ToList();
            var results = (from t in list
                           where (status == (int)QueueStatusEnum.All ? true : t.Queue.QueueStatusID == (int)status)
                           && (string.IsNullOrEmpty(keyword) ? true : (t.Queue.QueueNo.Contains(keyword) || t.Queue.PONO.Contains(keyword) || t.Queue.TruckRegNo.Contains(keyword)
                || (t.Dock != null && t.Dock.QueueDockName.Contains(keyword))
                || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))
                || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))))
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.ShortName : "",
                               ShippTo = t.shipTo != null ? t.shipTo.ShortName : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo, 
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified
                           }).ToList();
            foreach (var item in results)
            {
                var timeIn = item.TimeIn;
                var estimateTimeout = timeIn.AddMinutes(item.EstimateTime.GetValueOrDefault());
                var remainTime = estimateTimeout.Subtract(DateTime.Now).TotalMinutes;
                if (remainTime <= 0)
                {
                    remainTime = 0;
                }
                item.RemainingTime = (int)Math.Ceiling(remainTime);
                item.TimeInString = string.Format("{0:HH:mm}", item.TimeIn); 
            }
            totalRecord = results.Count();
            return results.ToList();
        }
        public List<QueueReg> GetInprogress(out int totalRecord, QueueStatusEnum status, string keyword)
        { 
            int[] statusList = new int[]
            {
               (int)  QueueStatusEnum.Register,
               (int)  QueueStatusEnum.WaitingCall,
               (int)  QueueStatusEnum.WaitingDocument,
               (int)  QueueStatusEnum.WaitDocJob,
                (int)  QueueStatusEnum.WaitRegNo
            };

            var list = (from q in Where(e => e.IsActive)

                        join st in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals st.QueueStatusID into gsta
                        from stat in gsta.DefaultIfEmpty()

                        join r in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals r.QueueRegisterTypeID into greg
                        from reg in greg.DefaultIfEmpty()

                        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
                        from dock in gd.DefaultIfEmpty()

                        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
                        from shipForm in gsf.DefaultIfEmpty()

                        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
                        from shipTo in gst.DefaultIfEmpty()
                        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
                        from truck in gtr.DefaultIfEmpty()
                        where (statusList.Contains(q.QueueStatusID))
                        orderby new { dock.QueueDockName, q.TimeIn }
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo, truck }).ToList();
            var results = (from t in list 
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.ShortName : "",
                               ShippTo = t.shipTo != null ? t.shipTo.ShortName : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo, 
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified
                           }).ToList();
            foreach (var item in results)
            {
                var timeIn = item.TimeIn;
                var estimateTimeout = timeIn.AddMinutes(item.EstimateTime.GetValueOrDefault());
                var remainTime = estimateTimeout.Subtract(DateTime.Now).TotalMinutes;
                if (remainTime <= 0)
                {
                    remainTime = 0;
                }
                item.RemainingTime = (int)Math.Ceiling(remainTime); 
                item.TimeInString = string.Format("{0:HH:mm}", item.TimeIn); 
            }
            totalRecord = results.Count();
            return results.ToList();
        }
        public List<QueueReg> GetQueueReport(out int totalRecord, DateTime startDate, DateTime endDate, Guid? shippingfrom, Guid? shippingTo, QueueStatusEnum status, Guid? dockId, string keyword, int? pageIndex, int? pageSize)
        { 
            var beDate = new DateTime(startDate.Year, startDate.Month, (startDate.Day), 0, 0, 0); 
            var finDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0); 
            var list = (from q in Where(e => e.IsActive)
                        join stat in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals stat.QueueStatusID
                        join r in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals r.QueueRegisterTypeID into greg
                        from reg in greg.DefaultIfEmpty()
                        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
                        from dock in gd.DefaultIfEmpty()
                        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
                        from shipForm in gsf.DefaultIfEmpty()
                        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
                        from shipTo in gst.DefaultIfEmpty() 
                        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
                        from truck in gtr.DefaultIfEmpty()
                        join usr in UserAccountsService.Where(e => e.IsActive) on q.UserCreated equals usr.UserID into gu
                        from user in gu.DefaultIfEmpty()
                        join em in EmployeeService.Where(e => e.IsActive) on user.EmployeeID equals em.EmployeeID into gE
                        from emp in gE.DefaultIfEmpty()

                        where q.QueueDate >= beDate && q.QueueDate <= finDate
                        && (shippingfrom.HasValue && shippingfrom.Value != Guid.Empty ? q.ShipFromId == shippingfrom.Value : true)
                        && (shippingTo.HasValue && shippingTo.Value != Guid.Empty ? q.ShipToId == shippingTo.Value : true)
                        && (status == QueueStatusEnum.All ? true : q.QueueStatusID == (int)status)
                        && (dockId.HasValue && dockId.Value != Guid.Empty ? q.QueueDockID == dockId.Value : true)
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo,/* Province = prov,*/ truck, user, emp }).ToList();

            var results = (from t in list
                           where (status == QueueStatusEnum.All ? true : t.Queue.QueueStatusID == (int)status)
                           && (string.IsNullOrEmpty(keyword) ? true : (t.Queue.QueueNo.Contains(keyword) || t.Queue.PONO.Contains(keyword) || t.Queue.TruckRegNo.Contains(keyword)
                            || (t.Dock != null && t.Dock.QueueDockName.Contains(keyword))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))
                            || (t.shipForm != null && (t.shipForm.ShortName.Contains(keyword) || t.shipForm.Name.Contains(keyword)))))
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.Name : "",
                               ShippTo = t.shipTo != null ? t.shipTo.Name : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo, 
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified,
                               CreateByName = t.emp != null ? t.emp.FirstName + " " + t.emp.LastName : ""
                           }).ToList();
            totalRecord = results.Count();
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int skip = (pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault();
                int take = pageSize.GetValueOrDefault();
                if (totalRecord - skip < take)
                {
                    take = totalRecord - skip;
                }
                results = results.OrderByDescending(x => x.QueueNo).Skip(skip).Take(take).ToList();
            } 
            return results.ToList();
        }

        public QueueReg GetQueue(Guid id)
        {
            var list = (from q in Where(e => e.IsActive)

                        join st in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals st.QueueStatusID into GSt
                        from stat in GSt.DefaultIfEmpty()

                        join r in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals r.QueueRegisterTypeID into greg
                        from reg in greg.DefaultIfEmpty()

                        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
                        from dock in gd.DefaultIfEmpty()

                        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
                        from shipForm in gsf.DefaultIfEmpty()

                        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
                        from shipTo in gst.DefaultIfEmpty()

                      //  join p in ProvinceService.Where(e => e.IsActive) on q.TruckRegProviceId equals p.Province_Id into gp
                      //  from prov in gp.DefaultIfEmpty()

                        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
                        from truck in gtr.DefaultIfEmpty()
                        where q.QueueId == id
                        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo,/* Province = prov,*/ truck }).ToList();
            //(from q in Where(e => e.IsActive)
            //        join reg in QueueRegisterTypeService.Where(e => e.IsActive) on q.QueueRegisterTypeID equals reg.QueueRegisterTypeID
            //        join stat in QueueStatusService.Where(e => e.IsActive) on q.QueueStatusID equals stat.QueueStatusID
            //        join d in QueueDockService.Where(e => e.IsActive) on q.QueueDockID equals d.QueueDockID into gd
            //        from dock in gd.DefaultIfEmpty()
            //        join sf in ShippingFromService.Where(e => e.IsActive) on q.ShipFromId equals sf.ShipFromId into gsf
            //        from shipForm in gsf.DefaultIfEmpty()
            //        join st in ShippingToService.Where(e => e.IsActive) on q.ShipToId equals st.ShipToId into gst
            //        from shipTo in gst.DefaultIfEmpty()
            //        join p in ProvinceService.Where(e => e.IsActive) on q.TruckRegProviceId equals p.Province_Id into gp
            //        from prov in gp.DefaultIfEmpty()
            //        join tr in TruckTypeService.Where(e => e.IsActive) on q.TruckTypeID equals tr.TruckTypeID into gtr
            //        from truck in gtr.DefaultIfEmpty()
            //        where q.QueueId == id
            //        select new { Queue = q, RegisterType = reg, Status = stat, Dock = dock, shipForm, shipTo, Province = prov, truck }).ToList();


            var results = (from t in list
                           select new QueueReg()
                           {
                               DateCreated = t.Queue.DateCreated,
                               DateModified = t.Queue.DateModified,
                               EstimateTime = t.Queue.EstimateTime,
                               IsActive = t.Queue.IsActive,
                               PONO = t.Queue.PONO,
                               QueueDock = t.Dock != null ? t.Dock.QueueDockName : "",
                               QueueDockID = t.Queue.QueueDockID,
                               QueueId = t.Queue.QueueId,
                               QueueNo = t.Queue.QueueNo,
                               QueueRegisterType = t.RegisterType != null ? t.RegisterType.QueueRegisterTypeName : "",
                               QueueRegisterTypeID = t.Queue.QueueRegisterTypeID,
                               QueueStatus = t.Status != null ? t.Status.QueueStatusName : "",
                               QueueStatusID = t.Queue.QueueStatusID,
                               Remark = t.Queue.Remark,
                               Sequence = t.Queue.Sequence,
                               ShipFrom = t.shipForm != null ? t.shipForm.Name : "",
                               ShippTo = t.shipTo != null ? t.shipTo.Name : "",
                               ShipFromId = t.shipForm != null ? t.shipForm.ShipFromId : Guid.Empty,
                               ShipToId = t.shipTo != null ? t.shipTo.ShipToId : Guid.Empty,
                               TimeIn = t.Queue.TimeIn,
                               TimeOut = t.Queue.TimeOut,
                               TruckRegNo = t.Queue.TruckRegNo,
                             //  TruckRegProvice = t.Province != null ? t.Province.Name : "",
                              // TruckRegProviceId = t.Queue.TruckRegProviceId,
                               TruckType = t.truck != null ? t.truck.TypeName : "",
                               TruckTypeID = t.Queue.TruckTypeID,
                               UserCreated = t.Queue.UserCreated,
                               UserModified = t.Queue.UserModified
                           }).ToList();
            return results.FirstOrDefault();
        }
        public override void Remove(QueueReg entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueReg _current = FirstOrDefault(x => x.QueueId == entity.QueueId);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public QueueReg ChangeQueueStatus(Guid id, QueueStatusEnum status)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueReg _current = FirstOrDefault(x => x.QueueId == id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    if (status == QueueStatusEnum.WaitinLoad || status == QueueStatusEnum.Loading)
                    {
                        var list = Where(e => e.QueueDockID == _current.QueueDockID && (e.QueueStatusID == (int)QueueStatusEnum.Loading || e.QueueStatusID == (int)QueueStatusEnum.WaitinLoad) && e.IsActive).ToList();
                        if (list.Any() && !list.Any(e=>e.QueueId== _current.QueueId))
                        {
                            throw new HILIException("QUEUE00003");
                        }
                    }

                    if (status == QueueStatusEnum.Completed || status == QueueStatusEnum.Loading || status == QueueStatusEnum.WaitingCall || status == QueueStatusEnum.WaitinLoad)
                    {
                        if (!(!string.IsNullOrEmpty(_current.PONO)
                            && !string.IsNullOrEmpty(_current.TruckRegNo) 
                            && (_current.TruckTypeID != null && _current.TruckTypeID != Guid.Empty)
                            && (_current.ShipFromId != null && _current.ShipFromId != Guid.Empty)
                            && (_current.ShipToId != null && _current.ShipToId != Guid.Empty)
                            && (_current.QueueDockID != null && _current.QueueDockID != Guid.Empty)))
                        {
                            throw new HILIException("QUEUE00001");
                        }
                    }  
                    if (status == QueueStatusEnum.Completed || status == QueueStatusEnum.Cancel)
                    {
                        _current.TimeOut = DateTime.Now;
                    }
                    _current.QueueStatusID = (int)status;
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);
                    scope.Complete();
                    return _current;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public QueueReg AddQueue(QueueReg entity)
        {
            try
            {
                var config = QueueSetupService.FirstOrDefault(e => e.IsActive);
                if (config == null)
                {
                    config = new QueueConfiguration()
                    {
                        StartHour = 7,
                        StartMinute = 0
                    };
                }
                var getRefDate = DateTime.Now;
                var currentDate = DateTime.Today;
                var startDate = currentDate.AddHours(config.StartHour);
                startDate = currentDate.AddMinutes(config.StartMinute);
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueRunning currentQ;
                    if (currentDate >= startDate)
                    {
                        currentQ = QueueRunningService.FirstOrDefault(e => e.QueuDate == currentDate && e.IsActive);
                        if (currentQ == null)
                        {
                            currentQ = new QueueRunning()
                            {
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                QueuDate = currentDate,
                                QueueRunId = Guid.NewGuid(),
                                QueueRun = 1,
                                UserCreated = UserID
                            };
                            QueueRunningService.Add(currentQ);
                        }
                        else
                        {
                            currentQ.QueueRun = currentQ.QueueRun + 1;
                            currentQ.DateModified = DateTime.Now;
                            currentQ.UserModified = UserID;
                            QueueRunningService.Modify(currentQ);
                        }
                    }
                    else
                    {
                        currentQ = new QueueRunning()
                        {
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            QueuDate = currentDate.AddDays(1),
                            QueueRunId = Guid.NewGuid(),
                            QueueRun = 1,
                            UserCreated = UserID
                        };
                        QueueRunningService.Add(currentQ);
                    }
                    entity.IsActive = true;
                    entity.QueueId = Guid.NewGuid();
                    entity.QueueDate = currentQ.QueuDate;
                    entity.Sequence = currentQ.QueueRun;
                    entity.QueueNo = string.Format("{0:000}", currentQ.QueueRun);
                    entity.DateCreated = DateTime.Now;
                    entity.UserCreated = UserID;
                    if ((entity.TruckTypeID != null && entity.TruckTypeID != Guid.Empty))
                    {
                        var trckT = TruckTypeService.FindByID(entity.TruckTypeID);
                        if (trckT != null)
                        {
                            entity.EstimateTime = trckT.EsitmateTime;
                        }
                        else
                        {
                            entity.EstimateTime = 0;
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.PONO)
                        && !string.IsNullOrEmpty(entity.TruckRegNo)
                     //   && (entity.TruckRegProviceId != null && entity.TruckRegProviceId != Guid.Empty)
                        && (entity.TruckTypeID != null && entity.TruckTypeID != Guid.Empty)
                        && (entity.ShipFromId != null && entity.ShipFromId != Guid.Empty)
                        && (entity.ShipToId != null && entity.ShipToId != Guid.Empty)
                        && (entity.QueueDockID != null && entity.QueueDockID != Guid.Empty))
                    {
                        entity.QueueStatusID = (int)QueueStatusEnum.WaitingCall;
                    }
                    else
                    {
                        entity.QueueStatusID = (int)QueueStatusEnum.Register;
                    }
                    var qreg = base.Add(entity);
                    scope.Complete();
                    return qreg;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public QueueReg ModifyQueue(QueueReg entity)
        {
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    QueueReg _current = FirstOrDefault(x => x.QueueId == entity.QueueId);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    _current.IsActive = true;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    _current.PONO = entity.PONO;
                    _current.QueueDockID = entity.QueueDockID;
                    _current.QueueRegisterTypeID = entity.QueueRegisterTypeID;
                    _current.ShipFromId = entity.ShipFromId;
                    _current.ShipToId = entity.ShipToId;
                    _current.TruckRegNo = entity.TruckRegNo;
                   // _current.TruckRegProviceId = entity.TruckRegProviceId;
                    _current.TruckTypeID = entity.TruckTypeID;
                    if (_current.EstimateTime <= 0 && (entity.TruckTypeID != null && entity.TruckTypeID != Guid.Empty))
                    {
                        var trckT = TruckTypeService.FindByID(entity.TruckTypeID);
                        if (trckT != null)
                        {
                            _current.EstimateTime = trckT.EsitmateTime;
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.PONO)
                        && !string.IsNullOrEmpty(entity.TruckRegNo)
                       // && (entity.TruckRegProviceId != null && entity.TruckRegProviceId != Guid.Empty)
                        && (entity.TruckTypeID != null && entity.TruckTypeID != Guid.Empty)
                        && (entity.ShipFromId != null && entity.ShipFromId != Guid.Empty)
                        && (entity.ShipToId != null && entity.ShipToId != Guid.Empty)
                        && (entity.QueueDockID != null && entity.QueueDockID != Guid.Empty))
                    {
                        _current.QueueStatusID = (int)QueueStatusEnum.WaitingCall;
                    }

                    base.Modify(_current);
                    scope.Complete();
                    return _current;
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public void RemoveQueue(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueReg _current = FirstOrDefault(x => x.QueueId == id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        public QueueReg CallQueue(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QueueReg _current = FirstOrDefault(x => x.QueueId == id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }
                    var list = Where(e => e.QueueDockID == _current.QueueDockID && (e.QueueStatusID == (int)QueueStatusEnum.Loading || e.QueueStatusID == (int)QueueStatusEnum.WaitinLoad) && e.IsActive).ToList();
                    if (list.Any() && !list.Any(e => e.QueueId == _current.QueueId))
                    {
                        throw new HILIException("QUEUE00003");
                    }

                    //if (Any(e => e.QueueDockID == _current.QueueDockID && e.QueueId != _current.QueueId && (e.QueueStatusID == (int)QueueStatusEnum.Loading || e.QueueStatusID == (int)QueueStatusEnum.WaitinLoad)))
                    //{
                    //    throw new HILIException("QUEUE00003");
                    //}
                    else
                    {
                        if (!(!string.IsNullOrEmpty(_current.PONO)
                                  && !string.IsNullOrEmpty(_current.TruckRegNo)
                                //  && (_current.TruckRegProviceId != null && _current.TruckRegProviceId != Guid.Empty)
                                  && (_current.TruckTypeID != null && _current.TruckTypeID != Guid.Empty)
                                  && (_current.ShipFromId != null && _current.ShipFromId != Guid.Empty)
                                  && (_current.ShipToId != null && _current.ShipToId != Guid.Empty)
                                  && (_current.QueueDockID != null && _current.QueueDockID != Guid.Empty)))
                        {
                            throw new HILIException("QUEUE00001");
                        }
                        if (_current.QueueStatusID != (int)QueueStatusEnum.Loading && _current.QueueStatusID != (int)QueueStatusEnum.Completed && _current.QueueStatusID != (int)QueueStatusEnum.Cancel)
                        {
                            _current.QueueStatusID = (int)QueueStatusEnum.WaitinLoad;
                            _current.TimeIn = DateTime.Now;
                        }
                        _current.IsActive = true;
                        _current.DateModified = DateTime.Now;
                        _current.UserModified = UserID;
                        base.Modify(_current);
                        scope.Complete();
                        return _current;
                    }
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
    }
}