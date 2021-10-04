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
    public class DockService : Repository<DockConfig>, IDockService
    {

        #region Constructor

        public DockService(IUnitOfWork dbContext)
            : base(dbContext)
        {
        }

        #endregion

        #region Method

        public DockConfig Get(Guid id)
        {
            try
            {
                DockConfig _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.DockConfigID == id)
                                  .Include(x => x.Warehouse)
                                  .Include(x => x.TruckType)
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
        public List<DockConfigModel> Get(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<DockConfig> result = Query().Include(x => x.Warehouse)
                                    .Include(x => x.TruckType)
                                    .Filter(x => (x.Barcode.Contains(keyword)
                                                    || x.DockName.Contains(keyword))).Get();

                result = result.Where(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true)).ToList();
                totalRecords = result.Count();

                result = result.OrderBy(x => x.DockName);
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<DockConfigModel> DockConfigResult = result.Select(x => new DockConfigModel
                {
                    DockConfigID = x.DockConfigID,
                    DockName = x.DockName,
                    Barcode = x.Barcode,
                    TruckTypeID = x.TruckTypeID,
                    TypeName = x.TruckType.TypeName,
                    WarehouseID = x.Warehouse.WarehouseID,
                    WarehouseName = x.Warehouse.Name,
                    WarehouseCode = x.Warehouse.Code,
                    WarehouseShortName = x.Warehouse.ShortName,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    IsActive = x.IsActive

                }).ToList();

                return DockConfigResult.ToList();
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
        public List<DockConfigModel> GetAll(Guid? warehouseID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<DockConfig> result = Query().Include(x => x.Warehouse)
                                    .Include(x => x.TruckType)
                                    .Filter(x => x.IsActive && (warehouseID != null ? x.WarehouseID == warehouseID.Value : true) && (x.Barcode.Contains(keyword)
                                                    || x.DockName.Contains(keyword))).Get();

                totalRecords = result.Count();
                result = result.OrderBy(x => x.DockName);
                if (pageIndex != null && pageSize != null)
                {
                    result = result.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<DockConfigModel> DockConfigResult = result.GroupBy(x => new
                {
                    TruckTypeID = x.TruckTypeID,
                    TypeName = x.TruckType.TypeName,
                    WarehouseID = x.Warehouse.WarehouseID,
                    WarehouseName = x.Warehouse.Name,
                    WarehouseCode = x.Warehouse.Code,
                    WarehouseShortName = x.Warehouse.ShortName,
                    DockConfigID = x.DockConfigID,
                    DockName = x.DockName,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    IsActive = x.IsActive
                }).Select(n => new DockConfigModel
                {
                    TruckTypeID = n.Key.TruckTypeID,
                    TypeName = n.Key.TypeName,
                    DockConfigID = n.Key.DockConfigID,
                    DockName = n.Key.DockName,
                    WarehouseID = n.Key.WarehouseID,
                    WarehouseName = n.Key.WarehouseName,
                    WarehouseCode = n.Key.WarehouseCode,
                    WarehouseShortName = n.Key.WarehouseShortName,
                    DateCreated = n.Key.DateCreated,
                    DateModified = n.Key.DateModified,
                    IsActive = n.Key.IsActive
                }).ToList();

                return DockConfigResult.ToList();
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
        public override DockConfig Add(DockConfig entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.DockName.Replace(" ", string.Empty).ToLower().Equals(entity.DockName.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.DockConfigID = new Guid();
                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;

                    DockConfig result = base.Add(entity);

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
        public override void Modify(DockConfig entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DockConfig _current = Query().Filter(x => x.DockConfigID == entity.DockConfigID).Get().FirstOrDefault();

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
                    DockConfig _current = FindByID(id);

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
        #endregion
    }

}
