using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public class PermissionService : Repository<Permission>, IPermissionService
    {
        #region [ Constructor ]
        public PermissionService(IUnitOfWork dbContext) : base(dbContext)
        {

            GetProductInfo<PermissionService>(Assembly.GetExecutingAssembly());
        }
        #endregion [ Constructor ]
        public Permission Get(Guid id)
        {
            try
            {
                Permission _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

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

        public List<Permission> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<Permission> results = Query().Filter(x => x.IsActive == true).Get();

                totalRecords = results == null ? 0 : results.Count();

                results = results.OrderBy(x => x.Sequent);


                return results.ToList();

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
