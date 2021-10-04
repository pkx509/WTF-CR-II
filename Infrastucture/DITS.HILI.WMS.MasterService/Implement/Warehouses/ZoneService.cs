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
    public class ZoneService : Repository<Zone>, IZoneService
    {
        #region Property 
        private readonly IRepository<ZoneType> zoneTypeService;
        #endregion

        #region Constructor

        public ZoneService(IUnitOfWork dbContext, IRepository<ZoneType> _zoneTpye)
            : base(dbContext)
        {
            zoneTypeService = _zoneTpye;
        }

        #endregion

        #region Method


        public Zone Get(Guid id)
        {
            try
            {
                Zone _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ZoneID == id)
                                  .Include(x => x.Warehouse)
                                  .Include(x => x.ZoneType)
                                  .Include(x => x.LocationCollection)
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
        public List<Zone> Get(Guid warehouseId, Guid? zoneTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Zone> result = Query().Include(x => x.ZoneType)
                                    .Filter(x => x.WarehouseID == warehouseId && (zoneTypeId != null ? x.ZoneTypeID == zoneTypeId.Value : true) &&
                                                (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<Zone> GetAll(Guid? warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Zone> result = Query().Include(x => x.ZoneType)
                                    .Filter(x => x.IsActive == true && (warehouseId != null ? x.WarehouseID == warehouseId : true) && (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != 0 || pageSize != 0)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<ZoneModel> Getlist(Guid? warehouseId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Zone> result = Query().Include(x => x.ZoneType)
                                    .Include(x => x.Warehouse)
                                    .Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (warehouseId != null ? x.WarehouseID == warehouseId : true) && (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != 0 || pageSize != 0)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<ZoneModel> ZoneResult = result.Select(n => new ZoneModel
                {
                    ZoneID = n.ZoneID,
                    Name = n.Name,
                    ShortName = n.ShortName,
                    ZoneTypeName = n.ZoneType.Name,
                    Code = n.Code,
                    WarehouseID = n.WarehouseID,
                    WarehouseName = n.Warehouse.Name,
                    Description = n.Description,
                    IsActive = n.IsActive

                }).ToList();

                return ZoneResult.ToList();
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

        public List<ZoneModel> GetZoneCombo(Guid? warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Zone> result = Query().Include(x => x.ZoneType)
                                    .Include(x => x.Warehouse)
                                    .Filter(x => x.IsActive == true && (warehouseId != null ? x.WarehouseID == warehouseId : true) && (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                //if (pageIndex != 0 || pageSize != 0)
                //{
                //    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                //}

                List<ZoneModel> ZoneResult = result.Select(n => new ZoneModel
                {
                    ZoneID = n.ZoneID,
                    Name = n.Name,
                    ShortName = n.ShortName,
                    ZoneTypeName = n.ZoneType.Name,
                    Code = n.Code,
                    WarehouseID = n.WarehouseID,
                    WarehouseName = n.Warehouse.Name,
                    Description = n.Description,
                    IsActive = n.IsActive

                }).ToList();

                return ZoneResult.ToList();
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

        public override Zone Add(Zone entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool duplicateCode = Query().Get().Any(x => x.Code.Replace(" ", string.Empty).ToLower().Equals(entity.Code.Replace(" ", string.Empty).ToLower()));

                    if (duplicateCode)
                    {
                        throw new HILIException("MSG00009");
                    }

                    bool duplicateName = Query().Get().Any(x => x.Name.ToLower() == entity.Name.ToLower());

                    if (duplicateName)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;
                    Zone result = base.Add(entity);

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
        public override void Modify(Zone entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Zone _current = Query().Filter(x => x.ZoneID == entity.ZoneID).Include(x => x.ZoneType).Get().FirstOrDefault();

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
                    Zone _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Remove(_current);

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

        public List<ZoneType> GetZoneType(Guid? zoneTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ZoneType> result = zoneTypeService.Query()
                                    .Filter(x => IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true && (zoneTypeId != null ? x.ZoneTypeID == zoneTypeId : true) &&
                                                      (x.Description.Contains(keyword)
                                                    || x.Name.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != 0 || pageSize != 0)
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
        public void AddZoneType(ZoneType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool duplicateName = zoneTypeService.Query().Get().Any(x => x.Name.ToLower() == entity.Name.ToLower());

                    if (duplicateName)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    ZoneType result = zoneTypeService.Add(entity);

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
        public void ModifyZoneType(ZoneType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ZoneType _current = zoneTypeService.Query().Filter(x => x.ZoneTypeID == entity.ZoneTypeID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    zoneTypeService.Modify(entity);
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
        public void RemoveZoneType(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ZoneType _current = zoneTypeService.Query().Filter(x => x.ZoneTypeID == id).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    zoneTypeService.Modify(_current);

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
