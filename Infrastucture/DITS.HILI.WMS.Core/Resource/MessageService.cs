using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Caching;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.Core.Resource
{
    public class MessageService : Repository<CustomResource>, IMessageService
    {
        private const string ALL_RESOURCE_KEY = "HILI.RES";
        private const string ALL_MESSAGE_KEY = "HILI.MSG";
        private const string RESOURCES_KEY = "{0}.{1}";
        private readonly IUnitOfWork unitofwork;
        private readonly ICacheManager cacheManager;



        private readonly IRepository<CustomMessage> customremessageService;

        public MessageService(ICacheManager _cacheManager,
                                    IUnitOfWork context,
                                    IRepository<CustomMessage> _customremessage) : base(context)
        {
            unitofwork = context;
            cacheManager = _cacheManager;
            customremessageService = _customremessage;

            GetProductInfo<MessageService>(Assembly.GetExecutingAssembly());
        }

        private List<CustomMessage> GetAllMessages()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<CustomMessage> resource = customremessageService.Query().Get().ToList();
                    return resource;
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

        private List<CustomResource> GetAllProperties()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<CustomResource> resource = Query().Get().ToList();
                    return resource;
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

        private Dictionary<string, CustomResource> GetAllCustomResourceValues()
        {
            return cacheManager.Get(ALL_RESOURCE_KEY, () =>
            {
                List<CustomResource> props = GetAllProperties();

                Dictionary<string, CustomResource> dictionary = new Dictionary<string, CustomResource>();
                foreach (CustomResource locale in props)
                {
                    string resourceKey = string.Format(RESOURCES_KEY, locale.LanguageCode.ToLowerInvariant(), locale.ResourceKey.ToLowerInvariant());

                    if (!dictionary.ContainsKey(resourceKey))
                    {
                        dictionary.Add(resourceKey, locale);
                    }
                }
                return dictionary;
            });
        }

        private Dictionary<string, CustomMessage> GetAllMessageValues()
        {
            return cacheManager.Get(ALL_MESSAGE_KEY, () =>
            {

                List<CustomMessage> msgs = GetAllMessages();

                Dictionary<string, CustomMessage> dictionary = new Dictionary<string, CustomMessage>();
                foreach (CustomMessage locale in msgs)
                {
                    string resourceKey = string.Format(RESOURCES_KEY, locale.LanguageCode.ToLowerInvariant(), locale.MessageCode.ToLowerInvariant());

                    if (!dictionary.ContainsKey(resourceKey))
                    {
                        dictionary.Add(resourceKey, locale);
                    }
                }
                return dictionary;
            });
        }

        public CustomResource GetResource(string key, string languageCode)
        {
            string resourceKey = string.Format("{0}.{1}", languageCode.ToLowerInvariant(), key.ToLowerInvariant());
            CustomResource resource = null;

            Dictionary<string, CustomResource> resources = GetAllCustomResourceValues();
            if (resources.ContainsKey(resourceKey))
            {
                resource = resources[resourceKey];
            }


            bool.TryParse(ConfigurationManager.AppSettings["AutoInsertResource"].ToString(), out bool isAutoAdd);
            if (resource == null && isAutoAdd)
            {
                bool isSucc = AddCustomResource(key, "");
                if (isSucc)
                {
                    cacheManager.Remove(ALL_RESOURCE_KEY);
                    resources = GetAllCustomResourceValues();
                    if (resources.ContainsKey(resourceKey))
                    {
                        resource = resources[resourceKey];
                    }
                }
            }
            return resource;
        }

        public CustomMessage GetMessage(string key, string languageCode)
        {
            string propertyKey = string.Format("{0}.{1}", languageCode.ToLowerInvariant(), key.ToLowerInvariant());
            CustomMessage prop = null;

            Dictionary<string, CustomMessage> props = GetAllMessageValues();
            if (props.ContainsKey(propertyKey))
            {
                prop = props[propertyKey];
            }
            bool.TryParse(ConfigurationManager.AppSettings["AutoInsertResource"].ToString(), out bool isAutoAdd);
            if (prop == null && isAutoAdd)
            {
                bool isSucc = AddCustomMessage(key, "");
                if (isSucc)
                {
                    cacheManager.Remove(ALL_MESSAGE_KEY);
                    props = GetAllMessageValues();
                    if (props.ContainsKey(propertyKey))
                    {
                        prop = props[propertyKey];
                    }
                }
            }
            return prop;
        }

        public bool AddCustomResource(string key, string val)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    int iCount = Query().Get().Where(x => x.ResourceKey == key).Count();

                    if (iCount > 0)
                    {
                        return true;
                    }

                    List<CustomResource> cus_lst = new List<CustomResource>();
                    List<Language> langList = unitofwork.Repository<Language>().Query().Get().ToList();
                    langList.ForEach(l =>
                    {
                        CustomResource obj = new CustomResource
                        {
                            LanguageCode = l.LanguageCode,
                            ResourceId = Guid.NewGuid(),
                            ResourceKey = key.ToUpperInvariant(),
                            ResourceValue = string.IsNullOrWhiteSpace(val) ? key.ToLowerInvariant() : val
                        };
                        obj.LanguageCode = l.LanguageCode;
                        obj.DateModified = DateTime.Now;
                        cus_lst.Add(obj);

                    });
                    IEnumerable<CustomResource> result = base.AddRange(cus_lst);

                    scope.Complete();

                    return true;
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


        public bool AddCustomMessage(string key, string val, string org_val = "")
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    int iCount = customremessageService.Query().Get().Where(x => x.MessageCode == key).Count();

                    if (iCount > 0)
                    {
                        return true;
                    }

                    List<CustomMessage> cus_lst = new List<CustomMessage>();
                    List<Language> langList = unitofwork.Repository<Language>().Query().Get().ToList();
                    langList.ForEach(l =>
                    {
                        CustomMessage obj = new CustomMessage
                        {
                            LanguageCode = l.LanguageCode,
                            MessageId = Guid.NewGuid(),
                            MessageCode = key.ToUpperInvariant(),
                            MessageValue = string.IsNullOrWhiteSpace(val) ? key.ToLowerInvariant() : val,
                            MessageOrgValue = string.IsNullOrWhiteSpace(org_val) ? key.ToLowerInvariant() : org_val
                        };
                        obj.LanguageCode = l.LanguageCode;
                        obj.DateModified = DateTime.Now;
                        cus_lst.Add(obj);

                    });
                    IEnumerable<CustomMessage> result = customremessageService.AddRange(cus_lst);

                    scope.Complete();

                    return true;
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
    }
}
