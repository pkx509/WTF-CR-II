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
    public class EquipZoneConfigService : Repository<EquipZoneConfig>, IEquipZoneConfigService
    {
        #region Property 
        private readonly IRepository<Zone> ZoneService;
        private readonly IRepository<TruckType> TruckTypeService;
        private readonly IRepository<Warehouse> WarehouseSerrvice;
        #endregion

        #region Constructor

        public EquipZoneConfigService(IUnitOfWork dbContext,
            IRepository<Zone> _zone,
            IRepository<TruckType> _trucktype,
            IRepository<Warehouse> _warehouse)
            : base(dbContext)
        {
            ZoneService = _zone;
            TruckTypeService = _trucktype;
            WarehouseSerrvice = _warehouse;
        }

        #endregion

        #region Method

        public EquipZoneConfig GetById(Guid id)
        {
            try
            {
                EquipZoneConfig _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.EquipID == id).Get().FirstOrDefault();

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

        public List<EquipZoneConfigModel> Get(string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                //var result = Query().Filter(x => (x.EquipName.Contains(keyword) || x.Serialnumber.Contains(keyword)))
                //                    .OrderBy(x => x.OrderBy(s => s.EquipName)).Get(out totalRecords, pageIndex, pageSize);

                var result = (from _equipzone in Query().Filter(x => (Active == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true)).Get()
                              join _zone in ZoneService.Query().Get() on _equipzone.ZoneID equals _zone.ZoneID
                              join _trucktype in TruckTypeService.Query().Get() on _equipzone.TruckTypeID equals _trucktype.TruckTypeID
                              join _warehouse in WarehouseSerrvice.Query().Get() on _zone.WarehouseID equals _warehouse.WarehouseID
                              select new { _equipzone, _zone, _trucktype, _warehouse });


                totalRecords = result.Count();

                List<EquipZoneConfigModel> resultsmodel = result.Select(n => new EquipZoneConfigModel
                {
                    EquipID = n._equipzone.EquipID,
                    EquipName = n._equipzone.EquipName,
                    Barcode = n._equipzone.Barcode,
                    Serialnumber = n._equipzone.Serialnumber,
                    ZoneID = n._equipzone.ZoneID,
                    TruckTypeID = n._equipzone.TruckTypeID,
                    TruckTypeName = n._trucktype.TypeName,
                    WarehouseName = n._warehouse.Name,
                    ZoneName = n._zone.Name,
                    WarehouseId = n._warehouse.WarehouseID,
                    IsActive = n._equipzone.IsActive
                }).ToList();

                List<EquipZoneConfigModel> results = new List<EquipZoneConfigModel>();
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    results = resultsmodel.Where(x => x.EquipName.Contains(keyword)
                                              || x.Serialnumber.Contains(keyword)
                                              || x.WarehouseName.Contains(keyword)
                                              || x.ZoneName.Contains(keyword)).ToList();
                }
                else
                {
                    results = resultsmodel;
                }

                return results;
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

        public void AddEquip(EquipZoneConfig entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.EquipName.Replace(" ", string.Empty).ToLower().Equals(entity.EquipName.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    //if (entity.EquipName.IndexOf(" ") > -1)
                    //    throw new HILIException("MSG00010");

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    EquipZoneConfig result = base.Add(entity);

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

        public void ModifyEquip(EquipZoneConfig entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    EquipZoneConfig _current = Query().Filter(x => x.EquipID == entity.EquipID).Get().FirstOrDefault();

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

        public void RemoveEquip(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    EquipZoneConfig _current = FindByID(id);

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
