using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public class ReasonService : Repository<Reason>, IReasonService

    {

        #region Constructor

        public ReasonService(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Method

        public List<Reason> GetReasons(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Reason> result = Query().Filter(x => x.ReasonName.Contains(keyword)).Get();

                totalRecords = result.Count();

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.ReasonName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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


        #endregion
    }

}
