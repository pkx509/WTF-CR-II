using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Companies
{
    public class SiteService : Repository<Site>, ISiteService
    {
        #region Property

        private readonly IRepository<SubDistrict> SubDistrictService;
        private readonly IRepository<District> DistrictService;
        private readonly IRepository<Province> ProvinceService;
        #endregion

        #region Constructor

        public SiteService(IUnitOfWork context
            ,
                                IRepository<SubDistrict> _SubDistrict,
                                IRepository<District> _District,
                                IRepository<Province> _Province)
            : base(context)
        {
            SubDistrictService = _SubDistrict;
            DistrictService = _District;
            ProvinceService = _Province;
        }


        #endregion

        #region Method

        public Site Get(Guid id)
        {
            try
            {
                Site _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.SiteID == id)
                                  .Include(x => x.Company)
                                  .Include(x => x.WarehouseCollection)
                                  .Get().FirstOrDefault();

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
        public List<Site> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Site> result = Query().Filter(x => x.IsActive && x.SiteName.Contains(keyword)).Get();

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.SiteID).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
        public List<SiteModel> GetAll(Guid? siteid, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from site in Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true)).Get()
                              join province in ProvinceService.Query().Filter(x => x.IsActive == true).Get() on site.SiteProvince_Id equals province.Province_Id into tempprovince
                              from pv in tempprovince.DefaultIfEmpty()
                              join district in DistrictService.Query().Filter(x => x.IsActive == true).Get() on site.SiteDistrict_Id equals district.District_Id into tempdistrict
                              from dt in tempdistrict.DefaultIfEmpty()
                              join subdistrict in SubDistrictService.Query().Filter(x => x.IsActive == true).Get().DefaultIfEmpty() on site.SiteSubDistrict_Id equals subdistrict.SubDistrict_Id into tempsubdistrict
                              from sdt in tempsubdistrict.DefaultIfEmpty()

                              where ((site.SiteName.Contains(keyword)) &&
                                    (siteid != null ? site.SiteID == siteid.Value : true))
                              group new { site, pv, dt, sdt } by new
                              {
                                  SiteID = site.SiteID,
                                  SiteName = site.SiteName,
                                  SiteAdress = site.SiteAdress,
                                  SiteRoad = site.SiteRoad,
                                  SubDistrict_Id = site?.SiteSubDistrict_Id,
                                  SubDistrictName = sdt?.Name,
                                  District_Id = site?.SiteDistrict_Id,
                                  DistrictName = dt?.Name,
                                  Province_Id = site?.SiteProvince_Id,
                                  ProvinceName = pv?.Name,
                                  SitePostCode = site.SitePostCode,
                                  SiteCountry = site.SiteCountry,
                                  SiteTel = site.SiteTel,
                                  SiteFax = site.SiteFax,
                                  SiteEmail = site.SiteEmail,
                                  SiteURL = site.SiteURL,
                                  CompanyID = site.CompanyID,
                                  IsActive = site.IsActive
                              }
                              into s
                              select new
                              {
                                  SiteID = s.Key.SiteID,
                                  SiteName = s.Key.SiteName,
                                  SiteAdress = s.Key.SiteAdress,
                                  SiteRoad = s.Key.SiteRoad,
                                  SubDistrict_Id = s.Key.SubDistrict_Id,
                                  SubDistrictName = s.Key.SubDistrictName,
                                  District_Id = s.Key.District_Id,
                                  DistrictName = s.Key.DistrictName,
                                  Province_Id = s.Key.Province_Id,
                                  ProvinceName = s.Key.ProvinceName,
                                  SitePostCode = s.Key.SitePostCode,
                                  SiteCountry = s.Key.SiteCountry,
                                  SiteTel = s.Key.SiteTel,
                                  SiteFax = s.Key.SiteFax,
                                  SiteEmail = s.Key.SiteEmail,
                                  SiteURL = s.Key.SiteURL,
                                  CompanyID = s.Key.CompanyID,
                                  IsActive = s.Key.IsActive
                              });

                totalRecords = result.Count();

                pageIndex = pageIndex == 0 ? null : pageIndex;
                pageSize = pageSize == 0 ? null : pageSize;
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.SiteName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<SiteModel> siteResult = result.Select(n => new SiteModel
                {
                    SiteID = n.SiteID,
                    SiteName = n.SiteName,
                    SiteAdress = n.SiteAdress,
                    SiteRoad = n.SiteRoad,
                    SubDistrict_Id = n.SubDistrict_Id,
                    SubDistrictName = n.SubDistrictName,
                    District_Id = n.District_Id,
                    DistrictName = n.DistrictName,
                    Province_Id = n.Province_Id,
                    ProvinceName = n.ProvinceName,
                    SitePostCode = n.SitePostCode,
                    SiteCountry = n.SiteCountry,
                    SiteTel = n.SiteTel,
                    SiteFax = n.SiteFax,
                    SiteEmail = n.SiteEmail,
                    SiteURL = n.SiteURL,
                    CompanyID = n.CompanyID,
                    IsActive = n.IsActive

                }).ToList();

                return siteResult;
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
        public override Site Add(Site entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.SiteName.ToLower() == entity.SiteName.ToLower());

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    //if (entity.SiteName.IndexOf(" ") > -1)
                    //    throw new HILIException("MSG00010");

                    entity.SiteID = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.UserCreated = UserID;
                    entity.DateCreated = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;

                    Site result = base.Add(entity);

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
        public override void Modify(Site entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Site _current = Query().Filter(x => x.SiteID == entity.SiteID).Include(x => x.Company).Get().FirstOrDefault();

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
        public override void Remove(object id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Site _current = FindByID(id);

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
