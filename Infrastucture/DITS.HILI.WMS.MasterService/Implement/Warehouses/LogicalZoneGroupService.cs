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
    public class LogicalZoneGroupService : Repository<LogicalZoneGroup>, ILogicalZoneGroupService
    {
        #region Property 
        private readonly IRepository<LogicalZoneGroupDetail> LogicalZoneGroupDetailService;
        private readonly IRepository<ProductGroupLevel3> ProductGroupLevel3Service;
        private readonly IRepository<Product> ProductService;
        private readonly IRepository<ProductCodes> ProductCodeService;
        #endregion

        #region Constructor

        public LogicalZoneGroupService(IUnitOfWork dbContext,
             IRepository<LogicalZoneGroupDetail> _LogicalZoneGroupDetail,
             IRepository<ProductGroupLevel3> _ProductGroupLevel3,
             IRepository<Product> _Product,
             IRepository<ProductCodes> _ProductCodes)
            : base(dbContext)
        {
            LogicalZoneGroupDetailService = _LogicalZoneGroupDetail;
            ProductGroupLevel3Service = _ProductGroupLevel3;
            ProductService = _Product;
            ProductCodeService = _ProductCodes;
        }

        #endregion

        #region Method

        public LogicalZoneGroupModel GetById(Guid id)
        {
            try
            {
                LogicalZoneGroup _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                LogicalZoneGroup result = Query().Filter(x => x.LogicalZoneGroupID == id).Get().FirstOrDefault();



                var rerultdetail = (from _logicalgroupdetail in LogicalZoneGroupDetailService.Query().Filter(x => x.LogicalZoneGroupID == id).Get()
                                    join _product in ProductService.Query().Get() on _logicalgroupdetail.ProductID equals _product.ProductID
                                    join _productcode in ProductCodeService.Query().Filter(x => x.IsActive && x.CodeType == ProductCodeTypeEnum.Stock).Get() on _product.ProductID equals _productcode.ProductID
                                    select new { _logicalgroupdetail, _product, _productcode });

                LogicalZoneGroupModel hd = new LogicalZoneGroupModel
                {
                    LogicalZoneGroupId = id,
                    LogicalZoneGroupName = result.LogicalZoneGroupName,
                    IsActive = result.IsActive
                };

                List<LogicalZoneGroupDetailModel> resultdetailmodel = rerultdetail.Select(n => new LogicalZoneGroupDetailModel
                {
                    LogicalZoneGroupDetailId = n._logicalgroupdetail.LogicalGroupDetailID,
                    LogicalZoneGroupLevel3Id = n._logicalgroupdetail.ProductGroupLevel3ID,
                    ProductCode = n._productcode.Code,
                    ProductId = n._logicalgroupdetail.ProductID.Value,
                    ProductName = n._product.Name,
                    IsActive = n._logicalgroupdetail.IsActive,
                }).ToList();

                hd.LogicalZoneGroupDetailCollection = resultdetailmodel;

                return hd;
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

        public List<LogicalZoneGroupModel> Get(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword.ToLower());

                var result = (from _group in Query().Filter(x => x.LogicalZoneGroupName.ToLower().Contains(keyword)).Get()
                              join _groupdt in LogicalZoneGroupDetailService.Query().Get() on _group.LogicalZoneGroupID equals _groupdt.LogicalZoneGroupID
                              join _group3 in ProductGroupLevel3Service.Query().Get() on _groupdt.ProductGroupLevel3ID equals _group3.ProductGroupLevel3ID into temp
                              from j in temp.DefaultIfEmpty()
                              where IsActive == true ? (_group.IsActive == true || _group.IsActive == false) : _group.IsActive == true
                              select new
                              {
                                  LogicalZoneGroupID = _group.LogicalZoneGroupID,
                                  LogicalZoneGroupName = _group.LogicalZoneGroupName,
                                  LogicalZoneGroupLevel3Name = j == null ? "" : j.Name
                              } into g
                              group g by new
                              {
                                  g.LogicalZoneGroupID,
                                  g.LogicalZoneGroupName,
                                  g.LogicalZoneGroupLevel3Name
                              } into s
                              select new
                              {
                                  LogicalZoneGroupID = s.Key.LogicalZoneGroupID,
                                  LogicalZoneGroupName = s.Key.LogicalZoneGroupName,
                                  LogicalZoneGroupLevel3Name = s.Key.LogicalZoneGroupLevel3Name
                              }
                              );


                totalRecords = result.Count();
                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.LogicalZoneGroupName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }


                List<LogicalZoneGroupModel> resultsmodel = result.Select(n => new LogicalZoneGroupModel
                {
                    LogicalZoneGroupId = n.LogicalZoneGroupID,
                    LogicalZoneGroupName = n.LogicalZoneGroupName,
                    LogicalZoneGroupLevel3Name = n.LogicalZoneGroupLevel3Name,
                    IsActive = true
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

        public void AddLogicalZoneGroup(LogicalZoneGroup entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.LogicalZoneGroupName.Replace(" ", string.Empty).ToLower().Equals(entity.LogicalZoneGroupName.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    LogicalZoneGroup result = base.Add(entity);

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

        public void ModifyLogicalZoneGroup(LogicalZoneGroup entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    LogicalZoneGroup _current = Query().Filter(x => x.LogicalZoneGroupID == entity.LogicalZoneGroupID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.LogicalZoneGroupID != entity.LogicalZoneGroupID && x.LogicalZoneGroupName.Replace(" ", string.Empty).ToLower().Equals(entity.LogicalZoneGroupName.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    //base.Modify(entity);
                    // scope.Complete();

                    //Detail
                    List<LogicalZoneGroupDetail> _currentdetail = LogicalZoneGroupDetailService.Query().Filter(x => x.LogicalZoneGroupID == entity.LogicalZoneGroupID).Get().ToList();
                    //Remove 
                    _currentdetail
                      .ForEach(item =>
                      {
                          bool exist = entity.LogicalZoneGroupDetail.Any(x => x.LogicalGroupDetailID == item.LogicalGroupDetailID);
                          if (!exist)
                          {
                              item.IsActive = false;
                              item.DateModified = DateTime.Now;
                              item.UserModified = UserID;
                              LogicalZoneGroupDetailService.Remove(item);
                          }
                      });
                    //Add Edit Item 
                    entity.LogicalZoneGroupDetail.ToList()
                       .ForEach(item =>
                       {

                           if (item.LogicalGroupDetailID == null || Utilities.IsZeroGuid(item.LogicalGroupDetailID))
                           {
                               item.LogicalGroupDetailID = Guid.NewGuid();
                               item.DateCreated = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               LogicalZoneGroupDetailService.Add(item);
                           }
                           else
                           {
                               LogicalZoneGroupDetail itemCurrent = LogicalZoneGroupDetailService.Query().Filter(x => x.LogicalGroupDetailID == item.LogicalGroupDetailID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   item.LogicalGroupDetailID = Guid.NewGuid();
                                   item.DateCreated = DateTime.Now;
                                   item.UserCreated = entity.UserModified;
                                   LogicalZoneGroupDetailService.Add(item);
                               }
                               else
                               {
                                   item.DateModified = DateTime.Now;
                                   item.UserModified = entity.UserModified;
                                   LogicalZoneGroupDetailService.Modify(item);
                               }
                               //    throw new HILIException("REC10001");

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

        public void RemoveLogicalZoneGroup(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    LogicalZoneGroup _current = FindByID(id);

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
