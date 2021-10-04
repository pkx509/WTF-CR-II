using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.DailyPlanService
{
    public class ProductionLineService : Repository<Line>, IProductionLineService
    {
        private readonly IRepository<Warehouse> WarehouseService;

        public ProductionLineService(IUnitOfWork context,
                                      IRepository<Warehouse> _warehose) : base(context)
        {
            WarehouseService = _warehose;
        }
        public Line Get(Guid id)
        {
            try
            {
                Line _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var Result = (from line in Query().Get()
                              join warehouse in WarehouseService.Query().Get() on line.WarehouseID equals warehouse.WarehouseID
                              where line.LineID == id
                              select new { line, warehouse });

                Line _result = Result.Select(n => new Line
                {
                    LineID = n.line.LineID,
                    LineCode = n.line.LineCode,
                    WarehouseID = n.line.WarehouseID,
                    BoiCard = n.line.BoiCard,
                    LineType = n.line.LineType,
                    WarehouseCode = n.warehouse.Code,
                    WarehouseName = n.warehouse.Name,
                }).FirstOrDefault();
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
        public List<Line> GetAll(string keyword, bool Active, LineTypeEnum lineType, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                List<string> lineTypes = new List<string>();
                if (lineType == LineTypeEnum.All)
                {
                    lineTypes.Add(LineTypeEnum.NP.ToString());
                    lineTypes.Add(LineTypeEnum.SP.ToString());
                }
                else
                {
                    lineTypes.Add(lineType.ToString());
                }

                var Result = (from line in Query().Filter(x => (Active == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && lineTypes.Contains(x.LineType)).Get()
                              join warehouse in WarehouseService.Query().Get() on line.WarehouseID equals warehouse.WarehouseID
                              where line.LineCode.Contains(keyword) || (string.IsNullOrEmpty(line.BoiCard) ? false : line.BoiCard.Contains(keyword))
                              select new { line, warehouse });

                totalRecords = Result.Count();
                if (pageIndex != 0 || pageSize != 0)
                {
                    Result = Result.OrderBy(x => x.line.LineCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                IEnumerable<Line> _result = Result.Select(n => new Line
                {
                    LineID = n.line.LineID,
                    LineCode = n.line.LineCode,
                    WarehouseID = n.line.WarehouseID,
                    BoiCard = n.line.BoiCard,
                    LineType = n.line.LineType,
                    WarehouseCode = n.warehouse.Code,
                    WarehouseName = n.warehouse.Name,
                    IsActive = n.line.IsActive
                });

                return _result.ToList();
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
        public override Line Add(Line entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG000015");
                    }

                    bool ok = Query().Get().Any(x => x.LineCode.Replace(" ", string.Empty).ToLower().Equals(entity.LineCode.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }
                    //throw new Exception("LineCode " + entity.LineCode + " is duplicate !");

                    if (entity.LineCode.IndexOf(" ") > -1)
                    {
                        throw new HILIException("MSG00010");
                    }

                    entity.IsActive = entity.IsActive;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;
                    Line result = base.Add(entity);

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
        public override void Modify(Line entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Line _current = Query().Filter(x => x.LineID == entity.LineID).Get().FirstOrDefault();

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
                    Line _current = FindByID(id);

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
    }
}
