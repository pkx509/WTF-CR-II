using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Products
{
    public class ProductGroupLevelService : Repository<ProductGroupLevel3>, IProductGroupLevelService
    {
        #region [ Property ]

        private readonly IRepository<Product> productService;
        private readonly IRepository<ProductGroupLevel2> ProductGroupLevel2Service;
        private readonly IRepository<ProductOwner> ProductOwnerService;

        #endregion

        #region [ Constructor ]

        public ProductGroupLevelService(IUnitOfWork dbContext,
                IRepository<Product> _product,
                IRepository<ProductGroupLevel2> _productGroupLevel2,
                IRepository<ProductOwner> _ProductOwner)
            : base(dbContext)
        {
            productService = _product;
            ProductGroupLevel2Service = _productGroupLevel2;
            ProductOwnerService = _ProductOwner;
        }

        #endregion

        #region [ Method ]

        public ProductGroupLevel3 Get(Guid id)
        {
            try
            {
                ProductGroupLevel3 _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ProductGroupLevel3ID == id)
                                  .Include(x => x.ProductCollection)
                                  .Include(x => x.ProductGroupLevel2)
                                  .Include(x => x.ProductGroupLevel2.ProductGroupLevel1)
                                  .Include(x => x.ProductGroupLevel2.ProductGroupLevel1.ProductOwner)
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

        public List<ProductGroupLevel3> Get(Guid? groupLV2Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<ProductGroupLevel3> result = Query().Include(x => x.ProductGroupLevel2)
                                    .Filter(x => x.IsActive && (groupLV2Id != null ? x.ProductGroupLevel2ID == groupLV2Id : true)
                                        && (x.Name.Contains(keyword) || x.Description.Contains(keyword)))
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
        public List<ProductGroupLevel3> GetAll(Guid? groupLV2Id, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<ProductGroupLevel3> result = Query().Include(x => x.ProductGroupLevel2)
                                    .Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (groupLV2Id != null ? x.ProductGroupLevel3ID == groupLV2Id : true)
                                        && (x.Name.Contains(keyword) || x.Description.Contains(keyword)))
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
        public List<ProductGroupLevel2> GetProductGroupLevel2(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<ProductGroupLevel2> result = ProductGroupLevel2Service.Query()
                                    .Filter(x => (x.Name.Contains(keyword) || x.Description.Contains(keyword)))
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
        public override ProductGroupLevel3 Add(ProductGroupLevel3 entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    ProductOwner _productOwner = ProductOwnerService.Query().Get().FirstOrDefault();

                    bool Name = Query().Get().Any(x => x.IsActive && x.Name.ToLower() == entity.Name.ToLower());

                    if (Name)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.ProductOwnerID = _productOwner.ProductOwnerID;
                    entity.ProductGroupLevel3ID = Guid.NewGuid();
                    entity.IsActive = entity.IsActive;
                    entity.UserCreated = UserID;
                    entity.UserModified = UserID;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    ProductGroupLevel3 result = base.Add(entity);

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


        public override void Modify(ProductGroupLevel3 entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    bool Name = Query().Get().Any(x => x.ProductGroupLevel3ID != entity.ProductGroupLevel3ID && x.Name.ToLower() == entity.Name.ToLower());

                    if (Name)
                    {
                        throw new HILIException("MSG00009");
                    }

                    ProductGroupLevel3 _current = Query().Filter(x => x.ProductGroupLevel3ID == entity.ProductGroupLevel3ID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.ProductOwnerID = _current.ProductOwnerID;
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
                    ProductGroupLevel3 _current = FindByID(id);
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
