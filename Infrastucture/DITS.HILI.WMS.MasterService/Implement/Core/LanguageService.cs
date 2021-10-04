using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.MasterModel.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Core
{
    public class LanguageService : Repository<Language>, ILanguageService
    {
        private const string ALL_RESOURCE_KEY = "";
        private readonly IMessageService messageService;



        public LanguageService(IUnitOfWork context,
                                    IMessageService _messageService) : base(context)
        {
            messageService = _messageService;
            GetProductInfo<LanguageService>(Assembly.GetExecutingAssembly());
        }

        public List<Language> GetAll()
        {
            try
            {
                IEnumerable<Language> result = Query().Get();

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

        public CustomResource GetResource(string resourceKey, string languageCode)
        {
            return messageService.GetResource(resourceKey, languageCode);
        }

        public CustomMessage GetMessage(string propertyKey, string languageCode)
        {

            return messageService.GetMessage(propertyKey, languageCode);
        }
    }
}
