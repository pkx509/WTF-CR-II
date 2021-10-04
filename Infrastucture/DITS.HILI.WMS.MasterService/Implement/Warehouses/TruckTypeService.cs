using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public class TruckTypeService : Repository<TruckType>, ITruckTypeService
    {
        #region Property 
        private readonly IRepository<Truck> TruckNoService;
        #endregion

        #region Constructor

        public TruckTypeService(IUnitOfWork dbContext, IRepository<Truck> _truckNo)

            : base(dbContext)
        {
            TruckNoService = _truckNo;
        }

        #endregion

        #region Method

        public TruckType Get(Guid id)
        {
            try
            {
                TruckType _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.TruckTypeID == id)
                                  .Get().FirstOrDefault();

                return _current;
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
        public TruckNoModel GetTruckNobyid(Guid id)
        {
            try
            {
                TruckNoModel _current = (from _truck in TruckNoService.Query().Filter(x => x.TruckID == id).Get()
                                         join _truckType in Query().Get()
                                            on _truck.TruckTypeID equals _truckType.TruckTypeID
                                         group new { _truck, _truckType }
                                         by new
                                         {
                                             TruckTypeID = _truck.TruckTypeID,
                                             TruckNo = _truck.TruckNo,
                                             TypeName = _truckType.TypeName,
                                             IsActive = _truck.IsActive
                                         }
                                             into s
                                         select new TruckNoModel()
                                         {
                                             TruckTypeID = s.Key.TruckTypeID,
                                             TruckNo = s.Key.TruckNo,
                                             TypeName = s.Key.TypeName,
                                             IsActive = s.Key.IsActive
                                         }).SingleOrDefault();
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                return _current;
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
        public List<TruckType> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize, bool IsActive = true)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<TruckType> result = Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (x.TypeName.Contains(keyword))).Get();
                result = result.OrderBy(x => x.TypeName);
                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.ToList();
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
        public List<TruckNoModel> GetTruckNolist(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = from _truckType in Query().Filter(x => x.IsActive == true).Get()
                             join _truck in TruckNoService.Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true)).Get()
                                on _truckType.TruckTypeID equals _truck.TruckTypeID
                             where (_truck.TruckNo.Contains(keyword)) ||
                                   (_truckType.TypeName.Contains(keyword))
                             group new { _truck, _truckType }
                             by new
                             {
                                 TruckID = _truck.TruckID,
                                 TruckTypeID = _truck.TruckTypeID,
                                 TruckNo = _truck.TruckNo,
                                 TypeName = _truckType.TypeName,
                                 IsActive = _truck.IsActive
                             }
                              into s
                             select new
                             {
                                 TruckID = s.Key.TruckID,
                                 TruckTypeID = s.Key.TruckTypeID,
                                 TruckNo = s.Key.TruckNo,
                                 TypeName = s.Key.TypeName,
                                 IsActive = s.Key.IsActive
                             };

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.TypeName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<TruckNoModel> TruckNo = result.Select(x => new TruckNoModel
                {
                    TruckID = x.TruckID,
                    TruckTypeID = x.TruckTypeID,
                    TruckNo = x.TruckNo,
                    TypeName = x.TypeName,
                    IsActive = x.IsActive

                }).ToList();


                return TruckNo;
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
        public override TruckType Add(TruckType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.TypeName.ToLower() == entity.TypeName.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;
                    TruckType result = base.Add(entity);

                    scope.Complete();
                    return result;
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
        public override void Modify(TruckType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TruckType _current = Query().Filter(x => x.TruckTypeID == entity.TruckTypeID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    base.Modify(entity);
                    scope.Complete();
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
        public override void Remove(object id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TruckType _current = FindByID(id);

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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public bool AddTruckNo(Truck entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = TruckNoService.Query().Get().Any(x => x.TruckNo.ToLower() == entity.TruckNo.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.TruckID = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;
                    Truck result = TruckNoService.Add(entity);

                    scope.Complete();
                    return true;
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
        public bool ModifyTruckNo(Truck entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Truck _current = TruckNoService.Query().Filter(x => x.TruckID == entity.TruckID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    TruckNoService.Modify(entity);
                    scope.Complete();
                    return true;
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
        public bool RemoveTruckNo(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Truck _current = TruckNoService.Query().Filter(x => x.TruckID == id).Get().SingleOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    TruckNoService.Modify(_current);

                    scope.Complete();
                    return true;
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

        #endregion
    }

}
