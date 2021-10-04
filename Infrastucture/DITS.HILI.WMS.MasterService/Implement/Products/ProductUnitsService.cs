using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Products
{
    public class ProductUnitsService : Repository<ProductUnit>, IProductUnitsService
    {
        #region [ Property ]

        #endregion

        #region [ Constructor ]

        public ProductUnitsService(IUnitOfWork dbContext)
            : base(dbContext)
        {
        }

        #endregion

        #region [ Method ]

        public ProductUnit Get(Guid id)
        {
            try
            {
                ProductUnit _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ProductUnitID == id).Get().FirstOrDefault();

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


        public List<ProductUnit> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ProductUnit> result = Query().Filter(x => (x.Name.Contains(keyword) || x.Description.Contains(keyword)))
                                    .OrderBy(x => x.OrderBy(s => s.Name)).Get(out totalRecords, pageIndex, pageSize);

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


        public List<ProductUnit> GetProductUnitAll(Guid? productUnitId, Guid? productId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                List<ProductUnit> result = Where(x => x.IsActive == true &&
                                            (productUnitId != null ? x.ProductUnitID == productUnitId : true) &&
                                            (productId != null ? x.ProductID == productId : true) &&
                                            (x.Name.Contains(keyword) || x.Barcode.Contains(keyword)))
                                    .OrderBy(x => x.Name).ToList();//.Get(out totalRecords, pageIndex, pageSize);

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != 0 || pageSize != 0))
                {
                    result = result.OrderBy(x => x.PalletQTY).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
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


        public void AddUnit(ProductUnit entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    ProductUnit result = base.Add(entity);

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
        public void ModifyUnit(ProductUnit entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<ProductUnit> _current = Query().Filter(x => x.ProductUnitID == entity.ProductUnitID).Get().ToList();

                    if (_current.Count() == 0)
                    {
                        throw new HILIException("MSG00006");
                    }

                    if (entity.IsBaseUOM)
                    {
                        List<ProductUnit> _flageList = Query().Filter(x => x.ProductID == entity.ProductID).Get().ToList();
                        foreach (ProductUnit update in _flageList)
                        {
                            bool yes = _current.Any(x => x.ProductUnitID == update.ProductUnitID);
                            if (yes)
                            {
                                entity.UserModified = UserID;
                                entity.DateModified = DateTime.Now;
                                base.Modify(entity);
                            }
                            else
                            {
                                ProductUnit model = new ProductUnit
                                {
                                    ProductID = update.ProductID,
                                    ProductUnitID = update.ProductUnitID,
                                    Code = update.Code,
                                    Barcode = update.Barcode,
                                    Cubicmeters = update.Cubicmeters,
                                    Description = update.Description,
                                    Height = update.Height,
                                    Length = update.Length,
                                    Name = update.Name,
                                    PackageWeight = update.PackageWeight,
                                    PalletQTY = update.PalletQTY
                                };
                                model.ProductUnitID = update.ProductUnitID;
                                model.ProductWeight = update.ProductWeight;
                                model.Quantity = update.Quantity;
                                model.Remark = update.Remark;
                                model.Width = update.Width;
                                model.URLImage = update.URLImage;
                                model.IsBaseUOM = false;
                                model.IsActive = update.IsActive;
                                model.ConversionMark = update.ConversionMark;
                                model.UserModified = UserID;
                                model.DateModified = DateTime.Now;
                                base.Modify(model);
                            }

                        }
                    }
                    else
                    {
                        entity.UserModified = UserID;
                        entity.DateModified = DateTime.Now;
                        base.Modify(entity);
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
        public void RemoveUnit(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductUnit _current = FindByID(id);

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
