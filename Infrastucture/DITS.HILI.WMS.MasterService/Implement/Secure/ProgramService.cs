using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public class ProgramService : Repository<Program>, IProgramService
    {
        private readonly IRepository<ProgramValue> programValueService;

        private readonly IRepository<ProgramInGroup> programInGroupService;

        private readonly IRepository<UserInGroup> userInGroupsService;

        public ProgramService(IUnitOfWork context,
                            IRepository<ProgramValue> _programValueService,
                            IRepository<ProgramInGroup> _programInGroupService,
                            IRepository<UserInGroup> _userInGroupsService) : base(context)
        {
            programValueService = _programValueService;
            programInGroupService = _programInGroupService;
            userInGroupsService = _userInGroupsService;

            GetProductInfo<ProgramService>(Assembly.GetExecutingAssembly());
        }

        public override void Modify(Program entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Program _current = Query().Filter(x => x.ProgramID == entity.ProgramID).Include(x => x.ProgramValueCollection).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.Url = string.IsNullOrEmpty(entity.Url) ? "" : entity.Url;
                    _current.ParentID = entity.ParentID;
                    _current.Sequence = entity.Sequence;
                    _current.IsActive = entity.IsActive;
                    _current.UserModified = UserID;
                    _current.DateModified = DateTime.Now;

                    foreach (ProgramValue i in _current.ProgramValueCollection)
                    {
                        ProgramValue v = entity.ProgramValueCollection.Where(x => x.ProgramValueID == i.ProgramValueID).SingleOrDefault();
                        i.Value = v.Value;
                        programValueService.Modify(i);
                    }


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

        public List<Program> GetAll(Guid appId, Guid userId, string langCode)
        {
            try
            {
                List<Guid> programIds = (from p in programInGroupService.Query().Get()
                                         join u in userInGroupsService.Query().Filter(x => x.UserID == userId).Get() on p.GroupID equals u.GroupID
                                         select p.ProgramID).ToList();


                //var results = (from p in Query().Filter(x => x.IsActive == true && programIds.Contains(x.ProgramID) && x.AppID == appId).Get()
                //              join v in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ProgramID equals v.ProgramID
                //               select new { p, v });

                var results = (from p in Query().Filter(x => x.IsActive == true && programIds.Contains(x.ProgramID) && x.AppID == appId).Get()
                               join v in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ProgramID equals v.ProgramID
                               join z in Query().Get() on p.ParentID equals z.ProgramID into parent
                               from gr in parent.DefaultIfEmpty()
                               join x in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ParentID equals x.ProgramID into parent_value
                               from gx in parent_value.DefaultIfEmpty()
                               select new { p, v, gr, gx });


                results = results.OrderBy(x => x.p.ProgramType).OrderBy(x => x.p.Sequence);
                List<Program> mResult = results.Select(n => new Program
                {
                    DateCreated = n.p.DateCreated,
                    DateModified = n.p.DateModified,
                    Description = n.v.Value,
                    Icon = n.p.Icon,
                    IsActive = n.p.IsActive,
                    Code = n.p.Code,
                    ParentID = n.p.ParentID,
                    ProgramID = n.p.ProgramID,
                    ParenCode = n.gr != null ? n.gr.Code : "",
                    ParentDescription = n.gx == null ? "" : n.gx.Value,
                    ProgramType = n.p.ProgramType,
                    Remark = n.p.Remark,
                    Sequence = n.p.Sequence,
                    Url = n.p.Url,
                    UserCreated = n.p.UserCreated,
                    UserModified = n.p.UserModified,
                    //UserPermissionCollection = n.p.UserPermissionCollection, 
                }).ToList();

                return mResult;

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

        public Program Get(Guid id)
        {
            try
            {
                IEnumerable<Program> _current = Query().Filter(x => x.ProgramID == id).Include(x => x.ProgramValueCollection).Get();
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                return _current.SingleOrDefault();
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

        public List<Program> GetAll(ProgramType programType, string langCode, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                var results = (from p in Query().Filter(x => x.ProgramType == programType).Get()
                               join v in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ProgramID equals v.ProgramID
                               join z in Query().Get() on p.ParentID equals z.ProgramID into parent
                               from gr in parent.DefaultIfEmpty()
                               join x in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ParentID equals x.ProgramID into parent_value
                               from gx in parent_value.DefaultIfEmpty()
                               select new { p, v, gr, gx });


                totalRecords = results == null ? 0 : results.Count();

                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    results = results.OrderBy(x => x.p.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                else
                {
                    results = results.OrderBy(x => x.p.Code);
                }

                List<Program> mResult = results.Select(n => new Program
                {
                    DateCreated = n.p.DateCreated,
                    DateModified = n.p.DateModified,
                    Description = n.v.Value,
                    ParentDescription = n.gx == null ? "" : n.gx.Value,
                    Icon = n.p.Icon,
                    IsActive = n.p.IsActive,
                    Code = n.p.Code,
                    ParentID = n.p.ParentID,
                    ProgramID = n.p.ProgramID,
                    ProgramType = n.p.ProgramType,
                    Remark = n.p.Remark,
                    Sequence = n.p.Sequence,
                    Url = n.p.Url,
                    UserCreated = n.p.UserCreated,
                    UserModified = n.p.UserModified,
                }).ToList();

                return mResult;

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
