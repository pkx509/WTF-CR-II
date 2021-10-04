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
    public class ItfInterfaceMappingService : Repository<ItfInterfaceMapping>, IItfInterfaceMappingService
    {
        #region Constructor

        public ItfInterfaceMappingService(IUnitOfWork context)
            : base(context)
        {

        }

        #endregion

        #region Method

        public List<ItfInterfaceMapping> GetByDocument(Guid id)
        {
            try
            {
                List<ItfInterfaceMapping> result = Query().Filter(x => x.DocumentId == id)
                                  .Get().ToList();

                return result;
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
