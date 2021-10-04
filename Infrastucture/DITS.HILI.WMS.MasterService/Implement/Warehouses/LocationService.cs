using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public class LocationService : Repository<Location>, ILocationService
    {
        #region Property 
        private readonly IRepository<Warehouse> warehouseService;
        private readonly IRepository<Zone> zoneService;
        #endregion

        #region Constructor

        public LocationService(IUnitOfWork context,
            IRepository<Warehouse> _warehouse,
            IRepository<Zone> _zone)
            : base(context)
        {
            warehouseService = _warehouse;
            zoneService = _zone;
        }

        #endregion

        #region Method

        public LocationModel Get(Guid id)
        {
            try
            {
                Location _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                //_current = Query().Filter(x => x.Code == id)
                //                  .Include(x => x.Zone)
                //                  .Include(x => x.LocationType)
                //                  .Get().FirstOrDefault();
                _current = Query().Filter(x => x.LocationID == id)
                       .Include(x => x.Zone)
                       .Include(x => x.Zone.Warehouse)
                       .Get().FirstOrDefault();

                LocationModel LocationModelResult = new LocationModel
                {
                    LocationID = _current.LocationID,
                    LocationType = _current.LocationType,
                    Code = _current.Code,
                    Description = _current.Description,
                    RowNo = _current.RowNo,
                    ColumnNo = _current.ColumnNo,
                    LevelNo = _current.LevelNo,
                    Width = _current.Width,
                    Length = _current.Length,
                    Height = _current.Height,
                    Weight = _current.Weight,
                    ReserveWeight = _current.ReserveWeight,
                    PalletCapacity = _current.PalletCapacity,
                    SizeCapacity = _current.SizeCapacity,
                    IsBlock = _current.IsBlock,
                    ZoneID = _current.ZoneID,
                    ZoneName = _current.Zone?.Name,
                    ZoneCode = _current.Zone?.Code,
                    ZoneShortName = _current.Zone?.ShortName,
                    WarehouseID = _current.Zone.Warehouse.WarehouseID,
                    WarehouseName = _current.Zone?.Warehouse?.Name,
                    WarehouseCode = _current.Zone?.Warehouse?.Code,
                    WarehouseShortName = _current.Zone?.Warehouse?.ShortName

                };

                return LocationModelResult;
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

        public List<Location> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Location> result = Query().Filter(x => x.Code.Contains(keyword)).Get();

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

        public List<Location> Get(Guid zoneId, LocationTypeEnum? locationType, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Location> result = Query().Filter(x => x.ZoneID == zoneId && (locationType != null ? x.LocationType == locationType.Value : true) &&
                                                 (x.Code.Contains(keyword) || x.Remark.Contains(keyword))).Get();

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

        public List<LocationModel> Get(Guid zoneId, Guid warehouseId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Location> result = Query().Filter(x => x.IsActive && x.Zone.ZoneID == zoneId && x.Zone.WarehouseID == warehouseId && x.Code.Contains(keyword))
                                  .Include(x => x.Zone)
                                  .Include(x => x.Zone.Warehouse)
                                  .Get();

                totalRecords = result.Count();

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<LocationModel> LocationModelResult = result.Select(n => new LocationModel
                {
                    LocationID = n.LocationID,
                    LocationType = n.LocationType,
                    Code = n.Code,
                    Description = n.Description,
                    RowNo = n.RowNo,
                    ColumnNo = n.ColumnNo,
                    LevelNo = n.LevelNo,
                    Width = n.Width,
                    Length = n.Length,
                    Height = n.Height,
                    Weight = n.Weight,
                    ReserveWeight = n.ReserveWeight,
                    IsBlock = n.IsBlock,
                    ZoneID = n.ZoneID,
                    ZoneName = n.Zone?.Name,
                    ZoneCode = n.Zone?.Code,
                    ZoneShortName = n.Zone?.ShortName,
                    WarehouseID = n.Zone.Warehouse.WarehouseID,
                    WarehouseName = n.Zone?.Warehouse?.Name,
                    WarehouseCode = n.Zone?.Warehouse?.Code,
                    WarehouseShortName = n.Zone?.Warehouse?.ShortName

                }).ToList();

                return LocationModelResult;
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

        public List<Location> GetLoading()
        {
            try
            {
                List<Location> result = Query().Filter(x => x.LocationType == LocationTypeEnum.LoadingIN)
                                    .Include(x => x.Zone.Warehouse).Get().ToList();
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

        public List<Location> GetLoadingOut()
        {
            try
            {
                List<Location> result = Query().Filter(x => x.LocationType == LocationTypeEnum.LoadingOut)
                                    .Include(x => x.Zone.Warehouse).Get().ToList();
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

        public List<LocationModel> GetAll(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<Location> result = Query().Filter(x => x.IsActive && (x.Code.Contains(keyword)
                                              || x.Zone.Name.Contains(keyword)
                                              || x.Zone.ShortName.Contains(keyword)
                                              || x.Zone.Description.Contains(keyword)
                                              || x.Zone.Warehouse.Code.Contains(keyword)
                                              || x.Zone.Warehouse.Name.Contains(keyword)
                                              || x.Zone.Warehouse.ShortName.Contains(keyword)))
                                  .Include(x => x.Zone)
                                  .Include(x => x.Zone.Warehouse)
                                  .Get();

                result = result.Where(x => (IsActive == true ? (x.IsActive == false || x.IsActive == true) : x.IsActive == true)).ToList();
                totalRecords = result.Count();

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<LocationModel> LocationModelResult = result.Select(n => new LocationModel
                {
                    LocationID = n.LocationID,
                    LocationType = n.LocationType,
                    Code = n.Code,
                    Description = n.Description,
                    RowNo = n.RowNo,
                    ColumnNo = n.ColumnNo,
                    LevelNo = n.LevelNo,
                    Width = n.Width,
                    Length = n.Length,
                    Height = n.Height,
                    Weight = n.Weight,
                    ReserveWeight = n.ReserveWeight,
                    IsBlock = n.IsBlock,
                    ZoneID = n.ZoneID,
                    ZoneName = n.Zone.Name,
                    ZoneCode = n.Zone.Code,
                    ZoneShortName = n.Zone.ShortName,
                    WarehouseID = n.Zone.Warehouse.WarehouseID,
                    WarehouseName = n.Zone.Warehouse.Name,
                    WarehouseCode = n.Zone.Warehouse.Code,
                    WarehouseShortName = n.Zone.Warehouse.ShortName

                }).ToList();

                return LocationModelResult;
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

        public List<LogicalZoneDetailModel> GetLocationBetweenList(string location_from, string location_to, Guid zoneid)
        {
            try
            {
                location_from = (string.IsNullOrEmpty(location_from) ? "" : location_from);
                location_to = (string.IsNullOrEmpty(location_to) ? "" : location_to);

                var result = (from _location in Query().Filter(x => x.ZoneID == zoneid).Get()
                              join _zone in zoneService.Query().Get() on _location.ZoneID equals _zone.ZoneID
                              select new { _location, _zone });

                List<LogicalZoneDetailModel> resultsmodel = result.Select(n => new LogicalZoneDetailModel
                {
                    LocationId = n._location.LocationID,
                    LocationNo = n._location.Code,
                    ZoneName = n._zone.Name,
                    LocationCapacity = n._location.PalletCapacity
                }).ToList();

                List<LogicalZoneDetailModel> results = new List<LogicalZoneDetailModel>();
                if (!string.IsNullOrWhiteSpace(location_from) && !string.IsNullOrWhiteSpace(location_to))
                {
                    results = resultsmodel.Where(x => x.LocationNo.CompareTo(location_from) >= 0 && x.LocationNo.CompareTo(location_to) <= 0).ToList();
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

        public ApiResponseMessage CheckLocation(List<LocationModel> modellist)
        {
            try
            {
                ApiResponseMessage _result = new ApiResponseMessage
                {
                    ResponseCode = "0"
                };
                if (modellist.Count() > 0)
                {
                    foreach (LocationModel l in modellist)
                    {
                        bool ok = Query().Get().Any(x => x.IsActive == true &&
                                                        x.ZoneID == l.ZoneID &&
                                                        x.RowNo == l.RowNo &&
                                                        x.ColumnNo == l.ColumnNo &&
                                                        x.LevelNo == l.LevelNo);
                        if (ok)
                        {
                            _result.text = l.Code;
                            _result.ResponseMessage = new HILIException("MSG00009").Message;
                            _result.ResponseCode = new HILIException("MSG00009").ErrorCode;
                        }
                    }
                }

                return _result;
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

        public void AddLocation(List<Location> entities)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entities.Count() == 0)
                    {
                        throw new HILIException("REC10001");
                    }

                    entities.ToList().ForEach(item =>
                    {
                        item.LocationReserveQty = 0;
                        item.UserCreated = UserID;
                        item.DateCreated = DateTime.Now;
                        item.UserModified = UserID;
                        item.DateModified = DateTime.Now;
                        base.Add(item);
                    });

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

        public void ModifyLocation(Location entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Location _current = Query().Filter(x => x.LocationID == entity.LocationID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.LocationReserveQty = _current.LocationReserveQty;
                    entity.UserCreated = _current.UserCreated;
                    entity.DateCreated = _current.DateCreated;
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

        public void RemoveLocation(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Location _current = Query().Filter(x => x.LocationID == id).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    if (_current.LocationReserveQty > 0)
                    {
                        throw new HILIException("MSG00091");
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
