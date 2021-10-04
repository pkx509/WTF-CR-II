using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public class ProductOwnerService : Repository<ProductOwner>, IProductOwnerService
    {
        #region [ Constructor ]

        public ProductOwnerService(IUnitOfWork dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region [ Method ]

        public ProductOwner Get(Guid id)
        {
            try
            {
                ProductOwner _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ProductOwnerID == id)
                                    .Include(x => x.AddressCollection)
                                    .Include(x => x.CustomerOfProductOwnerCollection)
                                    .Include(x => x.SupplierOfProductOwnerCollection)
                                    .Include(x => x.ProductGroupLevel1Collection)
                                    .Include(x => x.ProductGroupLevel2Collection)
                                    .Include(x => x.ProductGroupLevel3Collection)
                                    //.Include(x => x.UserAccountCollection)
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

        public List<ProductOwner> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<ProductOwner> result = Query().Filter(x => x.IsActive && (x.Name.Contains(keyword) || x.Description.Contains(keyword)))
                                    .OrderBy(x => x.OrderBy(s => s.Name)).Get(out totalRecords, pageIndex, pageSize);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }

        }

        #endregion

    }
}
