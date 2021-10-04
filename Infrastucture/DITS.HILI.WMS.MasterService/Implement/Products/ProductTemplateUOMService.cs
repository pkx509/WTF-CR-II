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
    public class ProductTemplateUOMService : Repository<ProductTemplateUom>, IProductTemplateUOMService
    {
        #region [ Property ] 
        private readonly IRepository<ProductTemplateUomDetail> ProductTemplateUomDetailService;
        #endregion

        #region [ Constructor ]

        public ProductTemplateUOMService(IUnitOfWork context,
            IRepository<ProductTemplateUomDetail> _producttemplateuom
            )
            : base(context)
        {
            ProductTemplateUomDetailService = _producttemplateuom;
        }

        #endregion

        #region [ Method ]

        public ProductTemplateUom GetById(Guid id)
        {
            try
            {
                ProductTemplateUom _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.Product_UOM_Template_ID == id).Get().FirstOrDefault();

                return _current;
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<ProductTemplateUom> GetProductTemplateUom(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ProductTemplateUom> result = Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && x.Product_UOM_Template_Name.Contains(keyword)).Get();

                totalRecords = result.Count();


                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Product_UOM_Template_Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            };
        }

        public List<ProductTemplateUomDetail> GetProductTemplateUomDetail(Guid productuomtemplateid)
        {
            try
            {
                List<ProductTemplateUomDetail> result = ProductTemplateUomDetailService.Query().Filter(x => x.Product_UOM_Template_ID == productuomtemplateid && x.IsActive).Get().ToList();

                foreach (ProductTemplateUomDetail item in result)
                {
                    item.Product_UOM_Template_Detail_Gross_Weight = item.Product_UOM_Template_Detail_Package_Weight + item.Product_UOM_Template_Detail_Weight;
                    if (item.Product_UOM_Template_Detail_SKU == 1)
                    {
                        item.Product_UOM_Template_Detail_SKU_Text = "Yes";
                    }
                    else
                    {
                        item.Product_UOM_Template_Detail_SKU_Text = "No";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            };
        }

        public void AddProductTemplateUom(ProductTemplateUom entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.Product_UOM_Template_Name.Replace(" ", string.Empty).ToLower().Equals(entity.Product_UOM_Template_Name.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    //if (entity.EquipName.IndexOf(" ") > -1)
                    //    throw new HILIException("MSG00010");

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    foreach (ProductTemplateUomDetail item in entity.ProductTemplateUomDetailCollection)
                    {
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                    }
                    ProductTemplateUom result = base.Add(entity);

                    scope.Complete();
                }

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

        public void ModifyProductTemplateUom(ProductTemplateUom entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductTemplateUom _current = Query().Filter(x => x.Product_UOM_Template_ID == entity.Product_UOM_Template_ID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;
                    base.Modify(entity);

                    List<ProductTemplateUomDetail> _currentdetail = ProductTemplateUomDetailService.Query().Filter(x => x.Product_UOM_Template_ID == entity.Product_UOM_Template_ID && x.IsActive).Get().ToList();
                    //Remove 
                    _currentdetail
                      .ForEach(item =>
                      {
                          bool exist = entity.ProductTemplateUomDetailCollection.Any(x => x.Product_UOM_Template_Detail_ID == item.Product_UOM_Template_Detail_ID);
                          if (!exist)
                          {
                              item.IsActive = false;
                              item.DateModified = DateTime.Now;
                              item.UserModified = UserID;
                              ProductTemplateUomDetailService.Modify(item);
                          }
                      });
                    //Add Edit Item 
                    entity.ProductTemplateUomDetailCollection.ToList()
                       .ForEach(item =>
                       {

                           if (item.Product_UOM_Template_Detail_ID == null || Utilities.IsZeroGuid(item.Product_UOM_Template_Detail_ID))
                           {
                               item.DateCreated = DateTime.Now;
                               item.UserCreated = entity.UserModified;
                               ProductTemplateUomDetailService.Add(item);
                           }
                           else
                           {
                               ProductTemplateUomDetail itemCurrent = ProductTemplateUomDetailService.Query().Filter(x => x.Product_UOM_Template_Detail_ID == item.Product_UOM_Template_Detail_ID).Get().FirstOrDefault();
                               if (itemCurrent == null)
                               {
                                   throw new HILIException("REC10001");
                               }

                               item.DateModified = DateTime.Now;
                               item.UserModified = entity.UserModified;
                               ProductTemplateUomDetailService.Modify(item);

                           }
                       });

                    scope.Complete();
                }

            }
            catch (DbEntityValidationException ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public void RemoveProductTemplateUom(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductTemplateUom _current = FindByID(id);

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
