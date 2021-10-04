using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.PasswordCrypt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public class UserAccountService : Repository<UserAccounts>, IUserAccountService
    {

        #region [ Property ]
        private readonly string _sharedSecret = ConfigurationManager.AppSettings["SharedSecret"].ToString();
        private readonly string _baseSecret = ConfigurationManager.AppSettings["BaseSecret"].ToString();

        // private readonly IRepository<Application> appService;
        private readonly IRepository<Program> programService;
        private readonly IRepository<Roles> roleService;
        private readonly IRepository<Employee> employeeService;
        private readonly IRepository<UserInRole> userInRoleService;
        private readonly IRepository<UserGroups> userGroupService;
        private readonly IRepository<UserInGroup> userInGroupService;
        // private readonly IRepository<Roles> roleService;
        //private readonly IRepository<ProgramInUserGroups> pgmRoleService;
        //private readonly IRepository<UsersInRoles> uInRoleService;
        #endregion

        #region [ Constructor ]
        public UserAccountService(IUnitOfWork dbContext,
                                        //IRepository<Application> _app,
                                        IRepository<Program> _program,
                                        IRepository<Roles> _role,
                                        IRepository<Employee> _employeeService,
                                        IRepository<UserInRole> _userInRoleService,
                                        IRepository<UserGroups> _userGroupService,
                                        IRepository<UserInGroup> _userInGroupService)
            : base(dbContext)
        {

            userInGroupService = _userInGroupService;
            programService = _program;
            roleService = _role;
            employeeService = _employeeService;
            userInRoleService = _userInRoleService;
            userGroupService = _userGroupService;
        }

        #endregion

        #region [ Method ]

        public override UserAccounts Add(UserAccounts entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new ArgumentNullException("UserAccount");
                    }

                    bool ok = Query().Filter(x => x.UserName.ToLower() == entity.UserName.ToLower()).Get().Count() == 0;
                    if (!ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    if (entity.UserName.IndexOf(" ") > -1)
                    {
                        throw new HILIException("MSG00010");
                    }

                    string _pwd = PasswordCryptography.Encrypt(entity.Password, _sharedSecret);
                    string salt = PasswordCryptography.GenRandomPwd(10);
                    string pass = PasswordCryptography.GenerateHashPassword(_pwd, salt);



                    Employee emp = new Employee
                    {
                        EmployeeID = Guid.NewGuid(),
                        FirstName = entity.Employee.FirstName,
                        LastName = entity.Employee.LastName,
                        Email = entity.Employee.Email,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        UserCreated = UserID,
                        IsActive = entity.IsActive
                    };


                    employeeService.Add(emp);

                    UserAccounts usr = new UserAccounts
                    {
                        UserID = Guid.NewGuid(),
                        Password = pass,
                        PasswordSalt = salt,
                        IsActive = entity.IsActive,
                        EmployeeID = emp.EmployeeID,
                        UserName = entity.UserName,

                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        UserCreated = UserID
                    };
                    //entity.Employee = emp;
                    UserAccounts result = base.Add(usr);

                    scope.Complete();
                    return result;
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

        public override void Modify(UserAccounts entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string _pwd = PasswordCryptography.Encrypt(entity.Password, _sharedSecret);
                    string salt = PasswordCryptography.GenRandomPwd(10);
                    string pass = PasswordCryptography.GenerateHashPassword(_pwd, salt);

                    UserAccounts _user = Query().Filter(x => x.UserID == entity.UserID).Include(x => x.Employee).Get().FirstOrDefault();
                    _user.IsActive = entity.IsActive;
                    _user.DateModified = DateTime.Now;
                    _user.UserModified = UserID;

                    _user.Password = pass;
                    _user.PasswordSalt = salt;

                    _user.Employee.FirstName = entity.Employee.FirstName;
                    _user.Employee.LastName = entity.Employee.LastName;
                    _user.Employee.Email = entity.Employee.Email;
                    _user.Employee.DateModified = DateTime.Now;
                    _user.Employee.UserModified = UserID;


                    employeeService.Modify(_user.Employee);


                    base.Modify(_user);

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
                    UserAccounts _user = Query().Filter(x => x.UserID == id).Get().FirstOrDefault();
                    _user.IsActive = false;
                    _user.DateModified = DateTime.Now;
                    _user.UserModified = UserID;
                    base.Modify(_user);

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

        public void ChangePassword(UserAccounts entity, string newPwd)
        {
            try
            {
                UserAccounts _current = FindByID(entity.UserID);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                string _oldpwd = PasswordCryptography.Decrypt(entity.Password, _sharedSecret);
                string _pass = _current.Password;
                string _salt = _current.PasswordSalt;
                string _pwd = PasswordCryptography.GenerateHashPassword(_oldpwd, _salt);
                if (_pass.CompareTo(_pwd) < 0)
                {
                    throw new HILIException("MSG00011");
                }

                if (!_current.Password.Equals(_pwd))
                {
                    throw new HILIException("MSG00012");
                }

                string pwd = PasswordCryptography.Decrypt(newPwd, _sharedSecret);
                string salt = PasswordCryptography.GenRandomPwd(10);
                string pass = PasswordCryptography.GenerateHashPassword(pwd, salt);
                _current.Password = pass;
                _current.PasswordSalt = salt;
                base.Modify(_current);

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

        public bool Login(string username, string password)
        {
            try
            {
                if (username == null)
                {
                    throw new HILIException("MSG00011");
                }

                string _pwd = PasswordCryptography.Encrypt(password, _sharedSecret);
                UserAccounts _user = Query().Filter(x => x.UserName == username).Get().FirstOrDefault();
                if (_user == null)
                {
                    throw new HILIException("MSG00011");
                }

                string pass = _user.Password;
                string salt = _user.PasswordSalt;
                string pwd = PasswordCryptography.GenerateHashPassword(_pwd, salt);
                if (pass.CompareTo(pwd) < 0)
                {
                    throw new HILIException("MSG00011");
                }

                UserAccounts user = Query().Filter(x => x.UserName == username && x.Password == pwd)
                                      //.Include(x => x.UserPermissionCollection.Select(s => s.Program.Application))
                                      //.Include(x => x.UserInRoleCollection.Select(s => s.Role))
                                      .Get().FirstOrDefault();

                if (user == null)
                {
                    throw new HILIException("MSG00011");
                }

                if (!user.IsActive)
                {
                    throw new HILIException("MSG00013");
                }

                //var upm = getUserPermission(user);

                //List<ProgramAuthorize> apps = new List<ProgramAuthorize>();
                //appService.Query().Get().ToList()
                //    .ForEach(item =>
                //    {
                //        apps.AddRange(appService.Query().Filter(x => x.ParentID == item.NodeID).Get()
                //           .Select(n => new ProgramAuthorize
                //           {
                //               AppID = n.AppID,
                //               Name = n.Name,
                //               Description = n.Description,
                //               NodeID = n.NodeID,
                //               ParentID = n.ParentID,
                //               Sequence1 = n.Sequence1,
                //               Sequence2 = n.Sequence2,
                //               NodeType = n.NodeType
                //           }).ToList());

                //        apps.AddRange(upm.Where(x => x.Program.AppID == item.AppID)
                //           .Select(n => new ProgramAuthorize
                //           {
                //               AppID = n.Program.ProgramID,
                //               Name = n.Program.Name,
                //               Description = n.Program.Description,
                //               ParentID = item.NodeID,
                //               ProgramID = n.ProgramID,
                //               URL = n.Program.Url,
                //               Icon = n.Program.Icon,
                //               Sequence1 = n.Program.Sequence
                //           }).ToList());
                //    });


                //user.UserPermissionCollection = upm;
                //user.ProgramAuthorizeCollection = apps;
                //user.Password = string.Empty;
                return true;
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

        public UserAccounts GetUserByName(string username)
        {
            try
            {
                if (username == null)
                {
                    throw new ArgumentNullException("UserAccount");
                }

                UserAccounts user = Query().Filter(x => x.UserName == username).Get().FirstOrDefault();


                return user;
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

        public UserAccounts Get(Guid id)
        {
            try
            {


                UserAccounts _current = Query().Filter(x => x.UserID == id)
                                   //.Include(x => x.UserPermissionCollection.Select(s => s.Program.Application))
                                   .Include(x => x.Employee)
                                   .Get().FirstOrDefault();
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }
                //var upm = getUserPermission(_current);
                // _current.UserPermissionCollection = upm;
                _current.Password = string.Empty;
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

        public List<UserAccounts> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<UserAccounts> result = Query().Include(x => x.Employee)
                            .Filter(x => (x.UserName.Contains(keyword) || x.Employee.FirstName.Contains(keyword) || x.Employee.LastName.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Employee.FirstName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<UserAccounts> ret = result.ToList();

                foreach (UserAccounts i in ret) { i.FirstName = i.Employee.FirstName; i.LastName = i.Employee.LastName; i.Email = i.Employee.Email; }

                return ret;
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




        public List<UserInRole> GetRoles(Guid userid)
        {
            try
            {

                var results = (from p in roleService.Query().Filter(x => x.IsActive == true).Get()
                               join n in userInRoleService.Query().Filter(x => x.UserID == userid).Get() on p.RoleID equals n.RoleID into comps
                               from y in comps.DefaultIfEmpty()
                               select new { p, y });


                results = results.OrderBy(x => x.p.Name);
                List<UserInRole> mResult = results.Select(n => new UserInRole
                {
                    RoleID = n.p.RoleID,
                    UserID = userid,
                    IsRole = n.y == null ? false : true,
                    RoleName = n.p.Name
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


        public void SaveRoles(List<UserInRole> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<Guid> userIDs = entity.Select(x => x.UserID).Distinct().ToList();
                    List<UserInRole> roles = userInRoleService.Query().Filter(x => userIDs.Contains(x.UserID)).Get().ToList();

                    foreach (UserInRole i in roles)
                    {
                        userInRoleService.Remove(i);
                    }

                    foreach (UserInRole i in entity)
                    {
                        if (i.IsRole)
                        {
                            UserInRole p = new UserInRole
                            {
                                UserID = i.UserID,
                                RoleID = i.RoleID,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                UserCreated = UserID
                            };

                            userInRoleService.Add(p);
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





        public List<UserInGroup> GetGroups(Guid userid)
        {
            try
            {

                var results = (from p in userGroupService.Query().Filter(x => x.IsActive == true).Get()
                               join n in userInGroupService.Query().Filter(x => x.UserID == userid).Get() on p.GroupID equals n.GroupID into comps
                               from y in comps.DefaultIfEmpty()
                               select new { p, y });


                results = results.OrderBy(x => x.p.GroupName);
                List<UserInGroup> mResult = results.Select(n => new UserInGroup
                {
                    GroupID = n.p.GroupID,
                    UserID = userid,
                    IsGroup = n.y == null ? false : true,
                    GroupName = n.p.GroupName
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


        public void SaveGroups(List<UserInGroup> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<Guid> userIDs = entity.Select(x => x.UserID).Distinct().ToList();
                    List<UserInGroup> groups = userInGroupService.Query().Filter(x => userIDs.Contains(x.UserID)).Get().ToList();

                    foreach (UserInGroup i in groups)
                    {
                        userInGroupService.Remove(i);
                    }

                    foreach (UserInGroup i in entity)
                    {
                        if (i.IsGroup)
                        {
                            UserInGroup p = new UserInGroup
                            {
                                UserID = i.UserID,
                                GroupID = i.GroupID,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                UserModified = UserID,
                                UserCreated = UserID
                            };

                            userInGroupService.Add(p);
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

        #endregion

        #region [ Private ]


        //private List<UserPermission> getUserPermission(UserAccounts user)
        //{
        //    Role role = null;
        //    foreach (var item in user.UserInRoleCollection)
        //    {
        //        role = item.Role;
        //        if (role != null)
        //            break;
        //    }

        //    List<UserPermission> up = new List<UserPermission>();
        //    var pgmRoles = roleService.Query().Filter(x => x.Level <= role.Level)
        //                              //.Include(x => x.ProgramInRoleCollection.Select(p => p.Program.Application))
        //                              .Get().ToList();


        //    Query().Filter(x => x.UserID == user.UserID)
        //        .Include(x => x.UserPermissionCollection).Get().ToList()
        //        .ForEach(item =>
        //        {
        //            item.UserPermissionCollection.ToList().ForEach(p =>
        //            {
        //                if (!p.Allow)
        //                {
        //                    pgmRoles.ForEach(r =>
        //                    {
        //                        var pr = r.ProgramInRoleCollection.FirstOrDefault(x => x.ProgramID == p.ProgramID);
        //                        r.ProgramInRoleCollection.Remove(pr);
        //                    });
        //                }
        //                else
        //                {
        //                    var pro = programService.Query().Filter(x => x.ProgramID == p.ProgramID).Get().FirstOrDefault();

        //                    up.Add(new UserPermission
        //                    {
        //                        Allow = true,
        //                        Program = pro,
        //                        ProgramID = p.ProgramID,
        //                        UserAccounts = user,
        //                        UserID = user.UserID
        //                    });
        //                }
        //            });
        //        });


        //    pgmRoles.ForEach(item =>
        //    {
        //        item.ProgramInRoleCollection.ToList().ForEach(p =>
        //        {
        //            up.Add(new UserPermission
        //            {
        //                Allow = true,
        //                Program = p.Program,
        //                ProgramID = p.ProgramID,
        //                UserAccounts = user,
        //                UserID = user.UserID
        //            });
        //        });

        //    });

        //    return up;
        //}

        #endregion
    }
}
