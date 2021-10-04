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
    internal class CustomerOfProductOwnerService : Repository<CustomerOfProductOwner>, ICustomerOfProductOwnerService
    {
        public CustomerOfProductOwnerService(IUnitOfWork dbContext)
            : base(dbContext)
        {

        }

        public List<CustomerOfProductOwner> getByProductOwnerId(Guid id, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<CustomerOfProductOwner> result = Query().Filter(x => x.ProductOwnerID == id).Include(x => x.Contact).Include(x => x.ProductOwner)
                                    .OrderBy(x => x.OrderBy(s => s.CustomerID)).Get(out totalRecords, pageIndex, pageSize);

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

        public List<CustomerOfProductOwner> getByCustomerId(Guid id, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<CustomerOfProductOwner> result = Query().Filter(x => x.CustomerID == id).Include(x => x.Contact).Include(x => x.ProductOwner)
                    .OrderBy(x => x.OrderBy(s => s.CustomerID)).Get(out totalRecords, pageIndex, pageSize);


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

    }
}
