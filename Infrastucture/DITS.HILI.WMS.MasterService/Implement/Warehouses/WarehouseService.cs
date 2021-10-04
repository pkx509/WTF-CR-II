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
    public class WarehouseService : Repository<Warehouse>, IWarehouseService
    {
        #region Property 
        private readonly IRepository<WarehouseType> warehoseTypeService;
        #endregion

        #region Constructor

        public WarehouseService(IUnitOfWork dbContext,
                                IRepository<WarehouseType> _warehoseTpye)
            : base(dbContext)
        {
            warehoseTypeService = _warehoseTpye;
        }

        #endregion

        #region Method [Warehose]

        public Warehouse Get(Guid id)
        {
            try
            {
                Warehouse _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.WarehouseID == id)
                                  .Include(x => x.ZoneCollection)
                                  .Include(x => x.WarehouseType)
                                  .Include(x => x.Site)
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
        public List<Warehouse> Get(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Warehouse> result = Query().Include(x => x.WarehouseType)
                                    .Include(x => x.Site)
                                    .Filter(x => x.IsActive == true && (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
                                                 (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
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
        public List<Warehouse> GetMk(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Warehouse> result = Query().Include(x => x.WarehouseType)
                                    .Include(x => x.Site)
                                    .Filter(x => x.IsActive == true && x.ReferenceCode != "412" && (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
                                                 (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
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

        public List<WarehouseModel> GetAll(Guid? warehouseTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Warehouse> result = Query().Include(x => x.WarehouseType)
                                    .Include(x => x.Site)
                                    .Filter(x => (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
                                                 (x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.ShortName.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();
                result = result.Where(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true)).ToList();
                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<WarehouseModel> warehouseTypeResult = result.Select(n => new WarehouseModel
                {
                    SiteID = n.SiteID,
                    SiteName = n.Site.SiteName,
                    WarehouseCode = n.Code,
                    WarehouseID = n.WarehouseID,
                    WarehouseName = n.Name,
                    WarehouseShortName = n.ShortName,
                    WarehouseTypeID = n.WarehouseTypeID,
                    WarehouseTypeName = n.WarehouseType.Name,
                    Description = n.Description,
                    IsActive = n.IsActive
                    //IsActive = n.WarehouseType.IsActive

                }).ToList();

                return warehouseTypeResult;
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
        public List<WarehouseModel> GetWarehouseTypeAll(Guid? warehouseTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<WarehouseType> result = warehoseTypeService.Query()
                                    .Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
                                                 (x.Name.Contains(keyword)
                                                    || x.Remark.Contains(keyword)
                                                    || x.Description.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<WarehouseModel> warehouseTypeResult = result.Select(n => new WarehouseModel
                {
                    WarehouseTypeName = n.Name,
                    WarehouseTypeID = n.WarehouseTypeID,
                    Description = n.Description,
                    IsActive = n.IsActive
                }).ToList();

                return warehouseTypeResult;
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
        public override Warehouse Add(Warehouse entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool WhCode = Query().Get().Any(x => x.Code.Replace(" ", string.Empty).ToLower().Equals(entity.Code.Replace(" ", string.Empty).ToLower()));

                    if (WhCode)
                    {
                        throw new HILIException("MSG00009");
                    }

                    bool WhName = Query().Get().Any(x => x.IsActive && x.Name.ToLower() == entity.Name.ToLower());

                    if (WhName)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    Warehouse result = base.Add(entity);

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
        public override void Modify(Warehouse entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    bool WhCode = Query().Get().Any(x => x.WarehouseID != entity.WarehouseID && x.Code.Replace(" ", string.Empty).ToLower().Equals(entity.Code.Replace(" ", string.Empty).ToLower()));

                    if (WhCode)
                    {
                        throw new HILIException("MSG00009");
                    }

                    bool WhName = Query().Get().Any(x => x.WarehouseID != entity.WarehouseID && x.Name.Replace(" ", string.Empty).ToLower().Equals(entity.Name.Replace(" ", string.Empty).ToLower()));

                    if (WhName)
                    {
                        throw new HILIException("MSG00009");
                    }

                    Warehouse _current = Query().Filter(x => x.WarehouseID == entity.WarehouseID).Get().FirstOrDefault();

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
                    Warehouse _current = FindByID(id);
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

        #endregion Method [Warehose]

        #region Method [WarehoseType]

        public WarehouseType GetWarehouseTypByID(Guid warehouseTypeId)
        {
            try
            {
                WarehouseType result = warehoseTypeService.Query().Filter(x => (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId : true)).Get().FirstOrDefault();

                if (result == null)
                {
                    throw new HILIException("MSG00006");
                }

                return result;
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
        public List<WarehouseType> GetWarehouseType(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<WarehouseType> result = warehoseTypeService.Query().Filter(x => (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
                                                 (x.Name.Contains(keyword)
                                                    || x.Description.Contains(keyword)) && x.IsActive == true).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
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
        public void AddWarehouseType(WarehouseType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = warehoseTypeService.Query().Get().Any(x => x.IsActive && x.Name.ToLower() == entity.Name.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    WarehouseType result = warehoseTypeService.Add(entity);

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
        public void WarehouseTypeModify(WarehouseType entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    WarehouseType _current = warehoseTypeService.Query().Filter(x => x.WarehouseTypeID == entity.WarehouseTypeID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    warehoseTypeService.Modify(entity);
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
        public void WarehouseTypeRemove(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    WarehouseType warehoseType = warehoseTypeService.Query().Filter(x => x.WarehouseTypeID == id && x.IsActive == true).Get().FirstOrDefault();
                    warehoseType.IsActive = false;
                    warehoseType.DateModified = DateTime.Now;
                    warehoseType.UserModified = UserID;
                    warehoseTypeService.Modify(warehoseType);

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

        #endregion Method [WarehoseType]
    }
}
