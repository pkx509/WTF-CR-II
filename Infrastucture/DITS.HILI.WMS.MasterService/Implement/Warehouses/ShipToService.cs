using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public class ShipToService : Repository<ShippingTo>, IShipToService
    {
        #region Property 
        private readonly IRepository<SpecialBookingRule> SpecialBookingRuleService;
        #endregion

        #region Constructor

        public ShipToService(IUnitOfWork dbContext,
            IRepository<SpecialBookingRule> _SpecialBookingRule)
            : base(dbContext)
        {
            SpecialBookingRuleService = _SpecialBookingRule;
        }

        #endregion

        #region Method

        public ShippingTo GetById(Guid id)
        {
            try
            {
                ShippingTo _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Include(x => x.SpecialBookingRule)
                                    .Filter(x => x.ShipToId == id)
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

        public List<ShippingTo> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ShippingTo> result = Query().Filter(x => x.IsActive && x.Name.Contains(keyword)).Get();
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
        public List<ShipToModel> GetShipToRule(string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ShipToModel> result = (from _shipto in Query().Filter(x => (Active == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && x.Name.Contains(keyword)).Get()
                                                   join _rule in SpecialBookingRuleService.Query().Get() on _shipto.RuleId equals _rule.RuleId
                                                   select new ShipToModel
                                                   {
                                                       ShipToId = _shipto.ShipToId,
                                                       Name = _shipto.Name,
                                                       Description = _shipto.Description,
                                                       IsDefault = _shipto.IsDefault,
                                                       IsActive = _shipto.IsActive,
                                                       BusinessGroup = _shipto.BusinessGroup,
                                                       RuleId = _shipto.RuleId,
                                                       RuleName = _rule.RuleName,
                                                       ShortName = _shipto.ShortName
                                                   });

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

        public void Add(ShippingTo entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.Name.Replace(" ", string.Empty).ToLower().Equals(entity.Name.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    ShippingTo result = base.Add(entity);

                    if (entity.IsDefault.Value)
                    {
                        List<ShippingTo> _all = Query().Filter(x => x.IsActive && x.ShipToId != result.ShipToId).Get().ToList();
                        foreach (ShippingTo item in _all)
                        {
                            item.IsDefault = false;
                            base.Modify(item);
                        }
                    }

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

        public void Modify(ShippingTo entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ShippingTo _current = Query().Filter(x => x.ShipToId == entity.ShipToId).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    base.Modify(entity);

                    if (entity.IsDefault.Value)
                    {
                        List<ShippingTo> _all = Query().Filter(x => x.IsActive && x.ShipToId != entity.ShipToId).Get().ToList();
                        foreach (ShippingTo item in _all)
                        {
                            item.IsDefault = false;
                            base.Modify(item);
                        }
                    }

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
                    ShippingTo _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
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
