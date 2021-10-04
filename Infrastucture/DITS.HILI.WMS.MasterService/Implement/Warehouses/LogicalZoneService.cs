using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public class LogicalZoneService : Repository<LogicalZone>, ILogicalZoneService
    {
        #region Property 
        private readonly IRepository<Zone> ZoneService;
        private readonly IRepository<LogicalZoneGroup> LogicalZoneGroupService;
        private readonly IRepository<Warehouse> WarehouseSerrvice;
        private readonly IRepository<ConditionConfig> ConditionConfigService;
        private readonly IRepository<LogicalZoneDetail> LogicalZoneDetailService;
        private readonly IRepository<Location> LocationSerrvice;
        private readonly IRepository<LogicalZoneConfig> LogicalZoneConfigService;
        private readonly IRepository<ProductGroupLevel3> ProductGroupLevelService;

        #endregion

        #region Constructor

        public LogicalZoneService(IUnitOfWork dbContext,
             IRepository<Zone> _zone,
             IRepository<LogicalZoneGroup> _LogicalZoneGroup,
             IRepository<Warehouse> _warehouse,
             IRepository<ConditionConfig> _conditionconfig,
             IRepository<LogicalZoneDetail> _logicalzonedetail,
             IRepository<Location> _Location,
             IRepository<LogicalZoneConfig> _Logicalzoneconfig,
             IRepository<ProductGroupLevel3> _ProductGroupLevel3
             )
            : base(dbContext)
        {
            ZoneService = _zone;
            LogicalZoneGroupService = _LogicalZoneGroup;
            WarehouseSerrvice = _warehouse;
            ConditionConfigService = _conditionconfig;
            LogicalZoneDetailService = _logicalzonedetail;
            LocationSerrvice = _Location;
            LogicalZoneConfigService = _Logicalzoneconfig;
            ProductGroupLevelService = _ProductGroupLevel3;
        }

        #endregion

        #region Method

        public LogicalZoneModel GetById(Guid id)
        {
            try
            {
                LogicalZone _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var result = (from _logicalzone in Query().Filter(x => x.LogicalZoneID == id).Get()
                              join _zone in ZoneService.Query().Get() on _logicalzone.ZoneID equals _zone.ZoneID
                              join _warehouse in WarehouseSerrvice.Query().Get() on _zone.WarehouseID equals _warehouse.WarehouseID
                              select new { _logicalzone, _zone, _warehouse });

                var resultdetail = (from _logicaldetail in LogicalZoneDetailService.Query().Filter(x => x.LogicalZoneID == id && x.IsActive).Get()
                                    join _location in LocationSerrvice.Query().Get() on _logicaldetail.LocationID equals _location.LocationID
                                    join _zone in ZoneService.Query().Get() on _location.ZoneID equals _zone.ZoneID
                                    select new { _logicaldetail, _location, _zone });

                var resultconfig = (from _logicalconfig in LogicalZoneConfigService.Query().Filter(x => x.LogicalZoneID == id && x.IsActive).Get()
                                    join _conditionconfig in ConditionConfigService.Query().Get() on _logicalconfig.ConfigID equals _conditionconfig.ConfigId
                                    select new { _logicalconfig, _conditionconfig });

                List<LogicalZoneConfigModel> resultsconfigmodel = resultconfig.Select(n => new LogicalZoneConfigModel
                {
                    LogicalConfigID = n._logicalconfig.LogicalConfigID,
                    ConfigSeq = n._logicalconfig.PrioritySeq,
                    ConfigValue = n._logicalconfig.ConfigValue,
                    ConfigValueId = n._logicalconfig.ConfigValueID,
                    ConfigName = n._conditionconfig.ConfigName,
                    ConfigID = n._conditionconfig.ConfigId
                }).OrderBy(o => o.ConfigSeq).ToList();

                List<LogicalZoneDetailModel> resultsdetailmodel = resultdetail.Select(n => new LogicalZoneDetailModel
                {
                    LogicalZoneDetailID = n._logicaldetail.LogicalZoneDetailID,
                    Seq = n._logicaldetail.Seq,
                    ZoneName = n._zone.Name,
                    LocationNo = n._location.Code,
                    LocationCapacity = n._location.PalletCapacity,
                    LocationId = n._location.LocationID
                }).OrderBy(o => o.Seq).ToList();

                LogicalZoneModel resultmodel = result.Select(n => new LogicalZoneModel
                {
                    LogicalZoneID = n._logicalzone.LogicalZoneID,
                    LogicalZoneName = n._logicalzone.LogicalZoneName,
                    WarehouseID = n._warehouse.WarehouseID,
                    WarehouseName = n._warehouse.Name,
                    ZoneID = n._zone.ZoneID,
                    ZoneName = n._zone.Name,
                    IsPallet = n._logicalzone.IsPallet,
                    LogicalZoneDetailModelCollection = resultsdetailmodel,
                    LogicalZoneConfigModelCollection = resultsconfigmodel
                }).FirstOrDefault();

                return resultmodel;
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

        public List<LogicalZoneModel> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword.ToLower());

                var result = (from _logicalzone in Query().Filter(x => x.IsActive).Get()
                              join _zone in ZoneService.Query().Get() on _logicalzone.ZoneID equals _zone.ZoneID
                              join _warehouse in WarehouseSerrvice.Query().Get() on _zone.WarehouseID equals _warehouse.WarehouseID
                              select new { _logicalzone, _zone, _warehouse });


                totalRecords = result.Count();

                List<LogicalZoneModel> resultsmodel = result.Select(n => new LogicalZoneModel
                {
                    LogicalZoneID = n._logicalzone.LogicalZoneID,
                    LogicalZoneName = n._logicalzone.LogicalZoneName,
                    WarehouseID = n._warehouse.WarehouseID,
                    WarehouseName = n._warehouse.Name,
                    ZoneID = n._zone.ZoneID,
                    ZoneName = n._zone.Name,
                    IsPallet = n._logicalzone.IsPallet
                }).ToList();

                List<LogicalZoneModel> results = new List<LogicalZoneModel>();
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    results = resultsmodel.Where(x => x.LogicalZoneName.ToLower().Contains(keyword)
                                              || x.WarehouseName.ToLower().Contains(keyword)).ToList();
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

        public List<ConditionConfigModel> GetConditionConfig(string modulename, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                modulename = (string.IsNullOrEmpty(modulename) ? "" : modulename);

                IEnumerable<ConditionConfig> result = ConditionConfigService.Query().Filter(x => x.ModuleName == modulename || x.ConfigName.Contains(keyword)).Get();

                totalRecords = result.Count();

                List<ConditionConfigModel> resultsmodel = result.Select(n => new ConditionConfigModel
                {
                    ConfigID = n.ConfigId,
                    ConfigName = n.ConfigName,
                    ModuleName = n.ModuleName,
                    ConfigScript = n.ConfigScript,
                    ConfigVariable = n.ConfigVariable,
                    isComboBox = n.IsComboBox
                }).ToList();

                return resultsmodel;
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

        public List<DataKeyValueString> GetConditionConfigBy_Configvaliable(string configvaliable, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                configvaliable = (string.IsNullOrEmpty(configvaliable) ? "" : configvaliable);

                string condition_code = ConditionConfigService.Query().Filter(x => x.ConfigId == new Guid(configvaliable)).Get().FirstOrDefault().ConfigVariable;

                if (condition_code == "@LogicalZone")
                {
                    IEnumerable<LogicalZoneGroup> result = LogicalZoneGroupService.Query().Filter(x => x.IsActive).Get();

                    totalRecords = result.Count();

                    List<DataKeyValueString> resultsmodel = result.Select(n => new DataKeyValueString
                    {
                        Key = n.LogicalZoneGroupName,
                        Value = n.LogicalZoneGroupID.ToString()
                    }).ToList();

                    return resultsmodel;
                }
                else if (condition_code == "@ProductGroup")
                {
                    IEnumerable<ProductGroupLevel3> resultp = ProductGroupLevelService.Query().Filter(x => x.IsActive).Get();

                    totalRecords = resultp.Count();

                    List<DataKeyValueString> resultsmodel = resultp.Select(n => new DataKeyValueString
                    {
                        Key = n.Name,
                        Value = n.ProductGroupLevel3ID.ToString()
                    }).ToList();

                    return resultsmodel;
                }
                else if (condition_code == "@Order")
                {
                    List<DataKeyValueString> _listdata = new List<DataKeyValueString>();
                    DataKeyValueString _item = new DataKeyValueString
                    {
                        Key = "Order",
                        Value = "1"
                    };
                    _listdata.Add(_item);

                    DataKeyValueString _item2 = new DataKeyValueString
                    {
                        Key = "No Order",
                        Value = "0"
                    };
                    _listdata.Add(_item2);

                    totalRecords = 2;
                    return _listdata;
                }
                else if (condition_code == "@Local")
                {
                    List<DataKeyValueString> _listdata = new List<DataKeyValueString>();
                    DataKeyValueString _item = new DataKeyValueString
                    {
                        Key = "LOCAL",
                        Value = "LOCAL"
                    };
                    _listdata.Add(_item);

                    DataKeyValueString _item2 = new DataKeyValueString
                    {
                        Key = "EXPORT",
                        Value = "EXPORT"
                    };
                    _listdata.Add(_item2);

                    totalRecords = 2;
                    return _listdata;
                }
                else if (condition_code == "@Lot")
                {
                    List<DataKeyValueString> _listdata = new List<DataKeyValueString>();
                    for (int i = 1; i <= 10; i++)
                    {
                        DataKeyValueString _item = new DataKeyValueString
                        {
                            Key = i.ToString(),
                            Value = i.ToString()
                        };
                        _listdata.Add(_item);
                    }
                    totalRecords = 2;
                    return _listdata;
                }
                else
                {
                    totalRecords = 0;
                    return new List<DataKeyValueString>();
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

        public void AddLogicalZone(LogicalZone entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.LogicalZoneName.Replace(" ", string.Empty).ToLower().Equals(entity.LogicalZoneName.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    //if (entity.EquipName.IndexOf(" ") > -1)
                    //    throw new HILIException("MSG00010");

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    LogicalZone result = base.Add(entity);

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

        public void ModifyLogicalZone(LogicalZone entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    LogicalZone _current = Query().Filter(x => x.LogicalZoneID == entity.LogicalZoneID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    //base.Modify(entity);
                    // scope.Complete();

                    //Detail
                    List<LogicalZoneDetail> _currentdetail = LogicalZoneDetailService.Query().Filter(x => x.LogicalZoneID == entity.LogicalZoneID && x.IsActive).Get().ToList();
                    //Remove 
                    _currentdetail
                      .ForEach(item =>
                      {
                          bool exist = entity.LogicalZoneDetail.Any(x => x.LogicalZoneDetailID == item.LogicalZoneDetailID);
                          if (!exist)
                          {
                              item.IsActive = false;
                              item.DateModified = DateTime.Now;
                              item.UserModified = UserID;
                              LogicalZoneDetailService.Modify(item);
                          }
                      });
                    //Add Edit Item 
                    entity.LogicalZoneDetail.ToList()
                       .ForEach(item =>
                       {

                           if (item.LogicalZoneDetailID == null || Utilities.IsZeroGuid(item.LogicalZoneDetailID))
                           {
                               item.LogicalZoneDetailID = Guid.NewGuid();
                               item.DateCreated = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               LogicalZoneDetailService.Add(item);
                           }
                           else
                           {
                               LogicalZoneDetail itemCurrent = LogicalZoneDetailService.Query().Filter(x => x.LogicalZoneDetailID == item.LogicalZoneDetailID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   item.LogicalZoneDetailID = Guid.NewGuid();
                                   item.DateCreated = DateTime.Now;
                                   item.UserCreated = entity.UserModified;
                                   LogicalZoneDetailService.Add(item);
                               }
                               else
                               {
                                   item.DateModified = DateTime.Now;
                                   item.UserModified = entity.UserModified;
                                   LogicalZoneDetailService.Modify(item);
                               }
                               //    throw new HILIException("REC10001");



                           }
                       });


                    //Config
                    List<LogicalZoneConfig> _currentconfig = LogicalZoneConfigService.Query().Filter(x => x.LogicalZoneID == entity.LogicalZoneID && x.IsActive).Get().ToList();
                    //Remove 
                    _currentconfig
                      .ForEach(item =>
                      {
                          bool exist = entity.LogicalZoneConfig.Any(x => x.LogicalConfigID == item.LogicalConfigID);
                          if (!exist)
                          {
                              item.IsActive = false;
                              item.DateModified = DateTime.Now;
                              item.UserModified = UserID;
                              LogicalZoneConfigService.Modify(item);
                          }
                      });
                    //Add Edit Item 
                    entity.LogicalZoneConfig.ToList()
                       .ForEach(item =>
                       {

                           if (item.LogicalConfigID == null || Utilities.IsZeroGuid(item.LogicalConfigID))
                           {
                               item.LogicalConfigID = Guid.NewGuid();
                               item.DateCreated = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               LogicalZoneConfigService.Add(item);
                           }
                           else
                           {
                               LogicalZoneConfig itemCurrent = LogicalZoneConfigService.Query().Filter(x => x.LogicalConfigID == item.LogicalConfigID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   item.LogicalConfigID = Guid.NewGuid();
                                   item.DateCreated = DateTime.Now;
                                   item.UserCreated = entity.UserModified;
                                   LogicalZoneConfigService.Add(item);
                               }
                               else
                               {
                                   item.DateModified = DateTime.Now;
                                   item.UserModified = entity.UserModified;
                                   LogicalZoneConfigService.Modify(item);
                               }
                               //throw new HILIException("REC10001");



                           }
                       });

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

        public void RemoveLogicalZone(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    LogicalZone _current = FindByID(id);

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

        public List<LogicalZoneGroup> GetLogicalZoneGroup(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<LogicalZoneGroup> result = LogicalZoneGroupService.Query().Filter(x => x.IsActive == true)
                                    .Filter(x => (x.IsActive == true) && (x.LogicalZoneGroupName.Contains(keyword)
                                                    || x.Remark.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.LogicalZoneGroupName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        #endregion
    }
}
