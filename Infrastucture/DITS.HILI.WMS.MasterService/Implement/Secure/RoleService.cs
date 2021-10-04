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
    public class RoleService : Repository<Roles>, IRoleService
    {
        #region [ Constructor PermissionInRole
        private readonly IRepository<PermissionInRole> permissionInroleService;
        private readonly IRepository<Permission> permissionService;


        public RoleService(IUnitOfWork dbContext, IRepository<Permission> _permissionService, IRepository<PermissionInRole> _permissionInroleService) : base(dbContext)
        {

            permissionService = _permissionService;
            permissionInroleService = _permissionInroleService;
            GetProductInfo<RoleService>(Assembly.GetExecutingAssembly());
        }
        #endregion [ Constructor ]


        public Roles Get(Guid id)
        {
            try
            {
                Roles _current = FindByID(id);
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

        public List<Roles> GetAll(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                IEnumerable<Roles> results = Query().Get();

                if (!string.IsNullOrEmpty(keyword))
                {
                    results = results.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
                }

                totalRecords = results == null ? 0 : results.Count();


                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    results = results.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<PermissionInRole> GetPermission(Guid roleID)
        {
            try
            {
                var results = (from p in permissionService.Query().Filter(x => x.IsActive == true).Get()
                               join n in permissionInroleService.Query().Filter(x => x.RoleID == roleID).Get() on p.PermissionID equals n.PermissionID into comps
                               from y in comps.DefaultIfEmpty()
                               select new { p, y });


                results = results.OrderBy(x => x.p.Sequent);
                List<PermissionInRole> mResult = results.Select(n => new PermissionInRole
                {
                    PermissionID = n.p.PermissionID,
                    RoleID = roleID,
                    IsPermission = n.y == null ? false : true,
                    PermissionName = n.p.Action
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

        public void SavePermission(List<PermissionInRole> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<Guid> RoleIDs = entity.Select(x => x.RoleID).Distinct().ToList();
                    List<PermissionInRole> permissions = permissionInroleService.Query().Filter(x => RoleIDs.Contains(x.RoleID)).Get().ToList();

                    foreach (PermissionInRole i in permissions)
                    {
                        permissionInroleService.Remove(i);
                    }

                    foreach (PermissionInRole i in entity)
                    {
                        if (i.IsPermission)
                        {
                            PermissionInRole p = new PermissionInRole
                            {
                                PermissionID = i.PermissionID,
                                RoleID = i.RoleID,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                UserCreated = UserID
                            };

                            permissionInroleService.Add(p);
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
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Framework.Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public void Add(Roles entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.RoleID = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;

                    Roles result = base.Add(entity);

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

        public void Modify(Roles entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Roles _current = Query().Filter(x => x.RoleID == entity.RoleID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.Description = entity.Description;
                    _current.Name = entity.Name;
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

        public void Delete(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Roles warehoseType = Query().Filter(x => x.RoleID == id).Get().FirstOrDefault();
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
    }
}
