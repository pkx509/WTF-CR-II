using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Rule
{
    public class RuleService : Repository<SpecialPutawayRule>, IRuleService
    {
        #region Property 
        private readonly IRepository<LogicalZone> logicalZoneService;
        private readonly IRepository<LogicalZoneGroup> logicalZoneGroupService;
        private readonly IRepository<LogicalZoneMapGroup> logicalZoneMapGroupService;
        private readonly IRepository<SpecialBookingRule> SpecialBookingRuleService;

        #endregion

        #region Constructor

        public RuleService(IUnitOfWork dbContext,
                           IRepository<LogicalZoneGroup> _logicalZoneGroup,
                           IRepository<LogicalZone> _logicalZone,
                           IRepository<LogicalZoneMapGroup> _logicalZoneMapGroupZone,
                           IRepository<SpecialBookingRule> _SpecialBookingRule)
            : base(dbContext)
        {
            logicalZoneGroupService = _logicalZoneGroup;
            logicalZoneService = _logicalZone;
            logicalZoneMapGroupService = _logicalZoneMapGroupZone;
            SpecialBookingRuleService = _SpecialBookingRule;
        }

        #endregion

        #region Method [PutawayRule]

        public SpecialPutawayRule Get(Guid id)
        {
            try
            {
                SpecialPutawayRule _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.PutAwayRuleID == id)
                                  .Include(x => x.LogicalZoneMapGroup)
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

        public List<SpecialPutawayRule> Get(Guid? putAwayRuleID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<SpecialPutawayRule> result1 = Query().Filter(x => x.IsActive == true && (putAwayRuleID != null ? x.PutAwayRuleID == putAwayRuleID.Value : true))
                                    .Include(x => x.LogicalZoneMapGroup).Get();


                IEnumerable<SpecialPutawayRule> result = from _putawyRule in Query().Filter(x => x.IsActive == true && (putAwayRuleID != null ? x.PutAwayRuleID == putAwayRuleID.Value : true))
                                    .Include(x => x.LogicalZoneMapGroup).Get()
                                                         select (new SpecialPutawayRule()
                                                         {
                                                             PutAwayRuleID = _putawyRule.PutAwayRuleID,
                                                             LogicalZoneID = _putawyRule.LogicalZoneID,
                                                             LogicalZoneGroupName = (from _logicalZoneMapGroup in _putawyRule.LogicalZoneMapGroup
                                                                                     join _logicalZoneGroup in logicalZoneGroupService.Query().Get() on _logicalZoneMapGroup.LogicalZoneGroupID equals _logicalZoneGroup.LogicalZoneGroupID
                                                                                     where
                                                                                           _logicalZoneMapGroup.PutAwayRuleID == _putawyRule.PutAwayRuleID
                                                                                     select new LogicalZoneGroup()
                                                                                     {
                                                                                         LogicalZoneGroupID = _logicalZoneGroup.LogicalZoneGroupID,
                                                                                         LogicalZoneGroupName = _logicalZoneGroup.LogicalZoneGroupName
                                                                                     }).FirstOrDefault().LogicalZoneGroupName,
                                                             PeriodLot = _putawyRule.PeriodLot,
                                                             Priority = _putawyRule.Priority,
                                                             Condition = _putawyRule.Condition,
                                                             Remark = _putawyRule.Remark
                                                         });
                //.Filter(x => x.IsActive == true && (putAwayRuleID != null ? x.PutAwayRuleID == putAwayRuleID.Value : true) &&
                //             (x.PeriodLot.ToString().Contains(keyword)
                //                || x.Priority.ToString().Contains(keyword)
                //                || x.Remark.Contains(keyword))).Get();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    result = result.Where(x => x.PeriodLot.ToString().Contains(keyword)
                                            || x.Priority.ToString().Contains(keyword)
                                            || x.Remark.Contains(keyword));
                }

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.PeriodLot).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        //public List<WarehouseModel> GetAll(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        //{
        //    try
        //    {
        //        keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

        //        var result = Query().Include(x => x.WarehouseType).Filter(x => x.IsActive == true)
        //                            .Include(x => x.Site)
        //                            .Filter(x => (x.IsActive == true && x.WarehouseType.IsActive == true) && (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
        //                                         (x.Code.Contains(keyword)
        //                                            || x.Name.Contains(keyword)
        //                                            || x.ShortName.Contains(keyword)
        //                                            || x.Description.Contains(keyword))).Get();

        //        totalRecords = result.Count();
        //        if (pageIndex != null && (pageSize != null || pageSize != 0))
        //        {
        //            result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
        //        }

        //        var warehouseTypeResult = result.Select(n => new WarehouseModel
        //        {
        //            SiteID = n.SiteID,
        //            SiteName = n.Site.SiteName,
        //            WarehouseCode = n.Code,
        //            WarehouseID = n.WarehouseID,
        //            WarehouseName = n.Name,
        //            WarehouseShortName = n.ShortName,
        //            WarehouseTypeID = n.WarehouseTypeID,
        //            WarehouseTypeName = n.WarehouseType.Name,
        //            Description = n.Description,
        //            IsActive = n.WarehouseType.IsActive

        //        }).ToList();

        //        return warehouseTypeResult;
        //    }
        //    catch (HILIException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}
        //public List<WarehouseModel> GetWarehouseTypeAll(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        //{
        //    try
        //    {
        //        keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

        //        var result = warehoseTypeService.Query().Filter(x => x.IsActive == true)
        //                            .Filter(x => (x.IsActive == true) && (warehouseTypeId != null ? x.WarehouseTypeID == warehouseTypeId.Value : true) &&
        //                                         (x.Name.Contains(keyword)
        //                                            || x.Remark.Contains(keyword)
        //                                            || x.Description.Contains(keyword))).Get();

        //        totalRecords = result.Count();
        //        if (pageIndex != null && (pageSize != null || pageSize != 0))
        //        {
        //            result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
        //        }

        //        var warehouseTypeResult = result.Select(n => new WarehouseModel
        //        {
        //            WarehouseTypeName = n.Name,
        //            WarehouseTypeID = n.WarehouseTypeID,
        //            Description = n.Description,
        //            IsActive = n.IsActive
        //        }).ToList();

        //        return warehouseTypeResult;
        //    }
        //    catch (HILIException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
        //        throw Framework.ExceptionHelper.ExceptionMessage(ex);
        //    }
        //}

        public override SpecialPutawayRule Add(SpecialPutawayRule entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("REC10001");
                    }

                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    SpecialPutawayRule result = base.Add(entity);

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

        public override void Modify(SpecialPutawayRule entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    SpecialPutawayRule _current = Query().Filter(x => x.PutAwayRuleID == entity.PutAwayRuleID).Get().FirstOrDefault();

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
                    SpecialPutawayRule _current = FindByID(id);
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

        #endregion Method [PutawayRule]

        #region Method [BookingRule]
        public List<SpecialBookingRule> GetBookingRule(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<SpecialBookingRule> result = SpecialBookingRuleService.Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (keyword != "" ? x.RuleName.Contains(keyword) : true)).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.RuleName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public void Add(SpecialBookingRule entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = SpecialBookingRuleService.Query().Get().Any(x => x.IsActive == true && x.RuleName.ToLower() == entity.RuleName.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    SpecialBookingRule result = SpecialBookingRuleService.Add(entity);

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

        public void Modify(SpecialBookingRule entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    SpecialBookingRuleService.Modify(entity);


                    List<SpecialBookingRule> _list = SpecialBookingRuleService.Query().Filter(x => x.RuleId != entity.RuleId).Get().ToList();
                    _list.ForEach(item =>
                    {
                        if (entity.IsDefault.Value)
                        {
                            item.IsDefault = false;
                        }
                        SpecialBookingRuleService.Modify(item);
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

        public void Remove(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    SpecialBookingRule _current = SpecialBookingRuleService.Query().Filter(x => x.RuleId == id).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    SpecialBookingRuleService.Modify(_current);

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
