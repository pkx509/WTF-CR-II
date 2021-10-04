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
    public class UserGroupService : Repository<UserGroups>, IUserGroupService
    {
        #region [ Constructor ]
        private readonly IRepository<Program> programService;
        private readonly IRepository<ProgramInGroup> programInGroupService;
        private readonly IRepository<ProgramValue> programValueService;
        public UserGroupService(IUnitOfWork dbContext,
            IRepository<Program> _programService,
            IRepository<ProgramInGroup> _programInGroupService,
             IRepository<ProgramValue> _programValueService) : base(dbContext)
        {
            programService = _programService;
            programInGroupService = _programInGroupService;
            programValueService = _programValueService;
            GetProductInfo<UserGroupService>(Assembly.GetExecutingAssembly());
        }
        #endregion [ Constructor ]

        public UserGroups Get(Guid id)
        {
            try
            {
                UserGroups _current = FindByID(id);
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

        public List<UserGroups> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<UserGroups> results = Query().Get();

                if (!string.IsNullOrEmpty(keyword))
                {
                    results = results.Where(x => x.GroupName == keyword);
                }

                totalRecords = results == null ? 0 : results.Count();


                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    results = results.OrderBy(x => x.GroupName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
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


        public List<ProgramInGroup> GetProgram(Guid groupID, string langCode)
        {
            try
            {

                var ret = (from p in programService.Query().Filter(x => x.ProgramType == ProgramType.Program && x.IsActive == true).Get()
                           join v in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ProgramID equals v.ProgramID
                           join z in programService.Query().Get() on p.ParentID equals z.ProgramID into parent
                           from gr in parent.DefaultIfEmpty()
                           join x in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ParentID equals x.ProgramID into parent_value
                           from gx in parent_value.DefaultIfEmpty()
                           select new { p, v, gr, gx });


                //var ret = (from p in programService.Query().Filter(x => x.IsActive == true).Get()
                //               join v in programValueService.Query().Get().Where(x => x.LanguageCode == langCode) on p.ProgramID equals v.ProgramID
                //               select new { p, v });

                var results = (from p in ret
                               join n in programInGroupService.Query().Filter(x => x.GroupID == groupID).Get() on p.p.ProgramID equals n.ProgramID into comps
                               from y in comps.DefaultIfEmpty()
                               select new { p, y });


                results = results.OrderBy(x => x.p.p.Sequence);

                List<ProgramInGroup> mResult = results.Select(n => new ProgramInGroup
                {
                    ProgramID = n.p.p.ProgramID,
                    GroupID = groupID,
                    Module = n.p.gr == null ? "" : (n.p.gr.Code + " : " + n.p.gx.Value),
                    IsCheck = n.y == null ? false : true,
                    Description = n.p.p.Code + " : " + n.p.v.Value,
                    Sequence = n.p.p.Sequence
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

        public void AddUserGroup(UserGroups entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.GroupID = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;

                    UserGroups result = base.Add(entity);

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

        public void ModifyUserGroup(UserGroups entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    UserGroups _current = Query().Filter(x => x.GroupID == entity.GroupID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.GroupName = entity.GroupName;
                    _current.GroupDescription = entity.GroupDescription;
                    _current.IsActive = entity.IsActive;
                    _current.Remark = entity.Remark;
                    _current.UserModified = UserID;
                    _current.DateModified = DateTime.Now;

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

        public void DeleteUserGroup(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    UserGroups warehoseType = Query().Filter(x => x.GroupID == id).Get().FirstOrDefault();
                    warehoseType.IsActive = false;
                    warehoseType.DateModified = DateTime.Now;
                    warehoseType.UserModified = UserID;
                    base.Modify(warehoseType);

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



        public void SaveProgram(List<ProgramInGroup> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<Guid> groupIDs = entity.Select(x => x.GroupID).Distinct().ToList();
                    List<ProgramInGroup> programs = programInGroupService.Query().Filter(x => groupIDs.Contains(x.GroupID)).Get().ToList();

                    foreach (ProgramInGroup i in programs)
                    {
                        programInGroupService.Remove(i);
                    }

                    Guid groupId = Guid.Empty;
                    foreach (ProgramInGroup i in entity)
                    {
                        if (i.IsCheck)
                        {
                            ProgramInGroup p = new ProgramInGroup
                            {
                                ProgramID = i.ProgramID,
                                GroupID = i.GroupID,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                UserCreated = UserID
                            };

                            groupId = i.GroupID;
                            programInGroupService.Add(p);
                        }
                    }
                    if (groupId != Guid.Empty)
                    {
                        List<Guid> programIDs = entity.Where(x => x.IsCheck == true).Select(x => x.ProgramID).Distinct().ToList();

                        List<Guid?> parents = programService.Query().Filter(x => programIDs.Contains(x.ProgramID)).Get().Select(x => x.ParentID).Distinct().ToList();

                        if (parents.Count > 0)
                        {
                            foreach (Guid? i in parents)
                            {
                                try
                                {
                                    if (i.HasValue)
                                    {
                                        ProgramInGroup p = new ProgramInGroup
                                        {
                                            ProgramID = i.Value,
                                            GroupID = groupId,
                                            IsActive = true,
                                            DateCreated = DateTime.Now,
                                            DateModified = DateTime.Now,
                                            UserModified = UserID,
                                            UserCreated = UserID
                                        };
                                        programInGroupService.Add(p);
                                    }
                                }
                                catch(Exception ee)
                                {

                                }
                            }
                        }
                    }
                    scope.Complete();
                }
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        } 
    }
}