using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public class ISONumberService : Repository<ISONumber>, IISONumberService

    {
        #region Property
        private readonly IUnitOfWork unitofwork;
        #endregion

        #region Constructor

        public ISONumberService(IUnitOfWork dbContext) : base(dbContext)
        {
            unitofwork = dbContext;
        }

        #endregion

        #region Method
        public List<ISONumber> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            throw new NotImplementedException();
        }

        public ISONumber GetIsoByID(Guid IsoId)
        {
            try
            {
                ISONumber result = Query().Filter(x => (IsoId != null ? x.ISO_Id == IsoId : true)).Get().FirstOrDefault();

                if (result == null)
                {
                    throw new HILIException("MSG00006");
                }

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
        public List<ISONumber> GetISONumber(Guid? IsoId, string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                IEnumerable<ISONumber> result = Query().Filter(x => (Active == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (IsoId != null ? x.ISO_Id == IsoId.Value : true) &&
                                                 (x.DocumentName.Contains(keyword)
                                                    || x.ISO_Number.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.DocumentName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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


        public ISONumber GetByDocument(string documentname)
        {
            try
            {
                ISONumber result = Query().Filter(x => x.DocumentName == documentname).Get().SingleOrDefault();
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

        public ISONumber GetById(Guid id)
        {
            throw new NotImplementedException();
        }


        public void AddISONumber(ISONumber entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.IsActive == true &&
                                                    x.DocumentName.ToLower() == entity.DocumentName.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    ISONumber result = base.Add(entity);

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

        public void ModifyISONumber(ISONumber entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    bool ok = Query().Get().Any(x => x.ISO_Id != entity.ISO_Id && x.IsActive &&
                                                    x.DocumentName.ToLower() == entity.DocumentName.ToLower() &&
                                                    x.ISO_Number.ToLower() == entity.ISO_Number.ToLower());
                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    ISONumber _current = Query().Filter(x => x.ISO_Id == entity.ISO_Id).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    base.Modify(entity);
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

        public void RemoveISONumber(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ISONumber _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
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
        #endregion
    }
}
