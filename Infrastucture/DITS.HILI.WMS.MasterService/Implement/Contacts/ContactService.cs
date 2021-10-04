using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public class ContactService : Repository<Contact>, IContactService
    {
        #region [ Property ] 

        private readonly IRepository<ContactAddress> cAddressService;
        private readonly IRepository<ContactInType> cInTypeService;
        private readonly IRepository<CustomerOfProductOwner> customerOfOwnerService;
        private readonly IRepository<ProductOwner> productOwnerService;
        private readonly IRepository<ProductGroupLevel1> pGroupLevel1Service;
        private readonly IRepository<ProductGroupLevel2> pGroupLevel2Service;
        private readonly IRepository<ProductGroupLevel3> pGroupLevel3Service;

        private readonly IRepository<SubDistrict> SubDistrictService;
        private readonly IRepository<District> DistrictService;
        private readonly IRepository<Province> ProvinceService;

        #endregion

        #region [ Constructor ]

        public ContactService(IUnitOfWork dbContext,
                                IRepository<ContactAddress> _cAddress,
                                IRepository<ContactInType> _cInType,
                                IRepository<CustomerOfProductOwner> _cOfPOwner,
                                IRepository<ProductOwner> _productOwner,
                                IRepository<ProductGroupLevel1> _pGroupLevel1,
                                IRepository<ProductGroupLevel2> _pGroupLevel2,
                                IRepository<ProductGroupLevel3> _pGroupLevel3,
                                IRepository<SubDistrict> _SubDistrict,
                                IRepository<District> _District,
                                IRepository<Province> _Province) : base(dbContext)
        {
            cAddressService = _cAddress;
            cInTypeService = _cInType;
            customerOfOwnerService = _cOfPOwner;
            productOwnerService = _productOwner;
            pGroupLevel1Service = _pGroupLevel1;
            pGroupLevel2Service = _pGroupLevel2;
            pGroupLevel3Service = _pGroupLevel3;
            SubDistrictService = _SubDistrict;
            DistrictService = _District;
            ProvinceService = _Province;
        }

        #endregion

        #region [ Method ]

        public Contact Get(Guid id)
        {
            try
            {
                Contact _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                var result = (from _contact in Query().Filter(x => x.ContactID == id).Include(x => x.ContactInTypeCollection).Get().DefaultIfEmpty()
                              join _cInTypes in cInTypeService.Query().Get()
                                on _contact.ContactID equals _cInTypes.ContactID into f
                              from t in f.DefaultIfEmpty()

                              join _address in cAddressService.Query().Get().DefaultIfEmpty()
                                on _contact.ContactID equals _address.ContactID into a
                              from x in a.DefaultIfEmpty()
                              join province in ProvinceService.Query().Get().DefaultIfEmpty()
                                on x?.Province_Id equals province.Province_Id into b
                              from y in b.DefaultIfEmpty()
                              join district in DistrictService.Query().Get().DefaultIfEmpty()
                                on x?.District_Id equals district.District_Id into c
                              from z in c.DefaultIfEmpty()
                              join subdistrict in SubDistrictService.Query().Get().DefaultIfEmpty()
                                on x?.SubDistrict_Id equals subdistrict.SubDistrict_Id into d
                              from w in d.DefaultIfEmpty()
                              select new
                              {

                                  ContactID = _contact.ContactID,
                                  Code = _contact.Code,
                                  Name = _contact.Name,
                                  ContactInTypeCollection = _contact?.ContactInTypeCollection,
                                  Address = _contact.Address,
                                  SubDistrict_Id = x == null ? Guid.Empty : x.SubDistrict_Id,
                                  SubDistrictName = w == null ? string.Empty : w.Name,
                                  District_Id = x == null ? Guid.Empty : x.District_Id,
                                  DistrictName = z == null ? string.Empty : z.Name,
                                  Province_Id = x == null ? Guid.Empty : x.Province_Id,
                                  ProvinceName = y == null ? string.Empty : y.Name,
                                  PostCode = _contact.PostCode,
                                  Telephone = _contact.Telephone,
                                  Fax = _contact.Fax,
                                  Email = _contact.Email,
                                  WebSite = _contact.WebSite,
                                  IsActive = _contact.IsActive
                              });

                _current = result.Select(n => new Contact
                {
                    ContactID = n.ContactID,
                    Code = n.Code,
                    Name = n.Name,
                    ProductOwnerCollection = (from _cusOfPowner in customerOfOwnerService.Query().Get().DefaultIfEmpty()
                                              join _productOwner in productOwnerService.Query().Get().DefaultIfEmpty()
                                                on _cusOfPowner.ProductOwnerID equals _productOwner.ProductOwnerID into d
                                              from w in d.DefaultIfEmpty()
                                              where _cusOfPowner.CustomerID == n.ContactID
                                              select new ProductOwner()
                                              {
                                                  ProductOwnerID = w.ProductOwnerID,
                                                  BranchID = w.BranchID,
                                                  Name = w.Name,
                                                  Description = w.Description,
                                                  DateCreated = w.DateCreated,
                                                  DateModified = w.DateModified,
                                                  UserCreated = w.UserCreated,
                                                  UserModified = w.UserModified,
                                              }).ToList(),
                    ContactInTypeCollection = n?.ContactInTypeCollection,
                    Address = n.Address,
                    SubDistrict_Id = n.SubDistrict_Id,
                    SubDistrictName = n.SubDistrictName,
                    District_Id = n.District_Id,
                    DistrictName = n.DistrictName,
                    Province_Id = n.Province_Id,
                    ProvinceName = n.ProvinceName,
                    PostCode = n.PostCode,
                    Telephone = n.Telephone,
                    Fax = n.Fax,
                    Email = n.Email,
                    WebSite = n.WebSite,
                    IsActive = n.IsActive

                }).FirstOrDefault();

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
        public List<Contact> Get(ContactType contactType, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Contact> result = Query().Filter(x => (IsActive == true ? (x.IsActive == true || x.IsActive == false) : x.IsActive == true) && (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.Code.Contains(keyword)))
                                    .Include(x => x.ContactInTypeCollection)
                                    .Include(x => x.AddressCollection).Get();

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<Contact> contactResult = result.Select(n => new Contact
                {
                    ContactID = n.ContactID,
                    ProductOwnerCollection = n.ProductOwnerCollection,
                    IsCustomer = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Customer),
                    IsSupplier = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Supplier),
                    Code = n.Code,
                    Name = n.Name,
                    Address = n.Address,
                    PostCode = n.PostCode,
                    Telephone = n.Telephone,
                    Fax = n.Fax,
                    Email = n.Email,
                    WebSite = n.WebSite,
                    IsActive = n.IsActive

                }).ToList();

                return contactResult.ToList();
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
        public List<Contact> GetAll(ContactType contactType, Guid? ContactID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                var result = (from _contact in Query().Filter(x => (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.Code.Contains(keyword)))
                                    .Include(x => x.ContactInTypeCollection).Get()
                              join _cusOfPowner in customerOfOwnerService.Query().Include(x => x.ProductOwner).Get() on _contact.ContactID equals _cusOfPowner.CustomerID
                              join _address in cAddressService.Query().Get() on _contact.ContactID equals _address.ContactID
                              join province in ProvinceService.Query().Get() on _contact.Province_Id equals province.Province_Id
                              join district in DistrictService.Query().Get() on _contact.District_Id equals district.District_Id
                              join subdistrict in SubDistrictService.Query().Get() on _contact.SubDistrict_Id equals subdistrict.SubDistrict_Id

                              where ((_contact.Name.Contains(keyword)) &&
                                    (ContactID != null ? _contact.ContactID == ContactID.Value : true))
                              select new { _contact, province, district, subdistrict });

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x._contact.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<Contact> contactResult = result.Select(n => new Contact
                {
                    ContactID = n._contact.ContactID,
                    ProductOwnerCollection = n._contact.ProductOwnerCollection,
                    Code = n._contact.Code,
                    Name = n._contact.Name,
                    Address = n._contact.Address,
                    SubDistrict_Id = n._contact.SubDistrict_Id,
                    SubDistrictName = n.subdistrict.Name,
                    District_Id = n._contact.District_Id,
                    DistrictName = n.district.Name,
                    Province_Id = n._contact.Province_Id,
                    ProvinceName = n.province.Name,
                    PostCode = n._contact.PostCode,
                    Telephone = n._contact.Telephone,
                    Fax = n._contact.Fax,
                    Email = n._contact.Email,
                    WebSite = n._contact.WebSite,
                    IsActive = n._contact.IsActive

                }).ToList();

                return contactResult.ToList();
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
        public List<Contact> GetCustomer(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Contact> result = Query().Filter(x => (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.Code.Contains(keyword)))
                                    .Include(x => x.ContactInTypeCollection)
                                    .Include(x => x.AddressCollection).Get()
                .Where(x => x.ContactInTypeCollection.All(s => s.ContactType == ContactType.Customer) && x.IsActive);

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<Contact> contactResult = result.Select(n => new Contact
                {
                    ContactID = n.ContactID,
                    ProductOwnerCollection = n.ProductOwnerCollection,
                    IsCustomer = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Customer),
                    IsSupplier = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Supplier),
                    Code = n.Code,
                    Name = n.Name,
                    FullName = n.Code + ':' + n.Name,
                    Address = n.Address,
                    PostCode = n.PostCode,
                    Telephone = n.Telephone,
                    Fax = n.Fax,
                    Email = n.Email,
                    WebSite = n.WebSite,
                    IsActive = n.IsActive

                }).ToList();

                return contactResult.ToList();
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
        public List<Contact> GetSupplier(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Contact> result = Query().Filter(x => (x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.Code.Contains(keyword)))
                                    .Include(x => x.ContactInTypeCollection)
                                    .Include(x => x.AddressCollection).Get()
                .Where(x => x.ContactInTypeCollection.All(s => s.ContactType == ContactType.Supplier));

                totalRecords = result.Count();


                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderByDescending(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                List<Contact> contactResult = result.Select(n => new Contact
                {
                    ContactID = n.ContactID,
                    ProductOwnerCollection = n.ProductOwnerCollection,
                    IsCustomer = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Customer),
                    IsSupplier = n.ContactInTypeCollection.Any(x => x.ContactID == n.ContactID && x.ContactType == ContactType.Supplier),
                    Code = n.Code,
                    Name = n.Name,
                    Address = n.Address,
                    PostCode = n.PostCode,
                    Telephone = n.Telephone,
                    Fax = n.Fax,
                    Email = n.Email,
                    WebSite = n.WebSite,
                    IsActive = n.IsActive

                }).ToList();

                return contactResult.ToList();
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
        public override Contact Add(Contact entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ContactCode = Query().Get().Any(x => x.IsActive && x.Code.ToLower() == entity.Code.ToLower());

                    if (ContactCode)
                    {
                        throw new HILIException("MSG00009");
                    }

                    bool ContactName = Query().Get().Any(x => x.IsActive && x.Name.ToLower() == entity.Name.ToLower());

                    if (ContactName)
                    {
                        throw new HILIException("MSG00009");
                    }

                    entity.ContactID = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = UserID;
                    entity.UserCreated = UserID;
                    Contact result = base.Add(entity);

                    entity.ProductOwnerCollection.ToList().ForEach(item =>
                    {
                        CustomerOfProductOwner _customerOfProductOwner = new CustomerOfProductOwner
                        {
                            ProductOwnerID = item.ProductOwnerID,
                            CustomerID = entity.ContactID
                        };
                        customerOfOwnerService.Add(_customerOfProductOwner);
                    });

                    ContactAddress _contactAddress = new ContactAddress
                    {
                        ContactID = entity.ContactID,
                        Address = entity.Address,
                        SubDistrict_Id = entity.SubDistrict_Id,
                        District_Id = entity.District_Id,
                        Province_Id = entity.Province_Id,
                        PostCode = entity.PostCode,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        UserModified = UserID,
                        UserCreated = UserID
                    };
                    cAddressService.Add(_contactAddress);

                    entity.ContactTypeCollection.ToList().ForEach(item =>
                    {
                        ContactInType _contactInType = new ContactInType
                        {
                            ContactID = entity.ContactID,
                            CusContactName = entity.CusContactName,
                            CusContactMobile = entity.CusContactMobile,
                            CusContactEmail = entity.CusContactEmail,
                            ContactType = (ContactType)Enum.Parse(typeof(ContactType), item.Value)
                        };
                        cInTypeService.Add(_contactInType);
                    });

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
        public override void Modify(Contact entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Contact _current = Query().Filter(x => x.ContactID == entity.ContactID).Get().FirstOrDefault();

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    entity.UserModified = UserID;
                    entity.DateModified = DateTime.Now;
                    base.Modify(entity);

                    ContactAddress _currentaddress = cAddressService.Query().Filter(x => x.ContactID == entity.ContactID).Get().FirstOrDefault();

                    if (_currentaddress == null)
                    {
                        ContactAddress _caddress = new ContactAddress
                        {
                            ContactID = entity.ContactID,
                            Address = entity.Address,
                            Province_Id = entity.Province_Id,
                            District_Id = entity.District_Id,
                            SubDistrict_Id = entity.SubDistrict_Id,
                            PostCode = entity.PostCode,
                            Telephone = entity.Telephone,
                            Email = entity.Email,
                            Fax = entity.Fax,
                            WebSite = entity.WebSite,
                            IsActive = true,
                            UserCreated = UserID,
                            DateCreated = DateTime.Now,
                            UserModified = UserID,
                            DateModified = DateTime.Now,

                        };

                        cAddressService.Add(_caddress);
                    }
                    else
                    {
                        _currentaddress.Address = entity.Address;
                        _currentaddress.Province_Id = entity.Province_Id;
                        _currentaddress.District_Id = entity.District_Id;
                        _currentaddress.SubDistrict_Id = entity.SubDistrict_Id;
                        _currentaddress.PostCode = entity.PostCode;
                        _currentaddress.Telephone = entity.Telephone;
                        _currentaddress.Email = entity.Email;
                        _currentaddress.Fax = entity.Fax;
                        _currentaddress.WebSite = entity.WebSite;
                        _currentaddress.UserModified = UserID;
                        _currentaddress.DateModified = DateTime.Now;
                        cAddressService.Modify(_currentaddress);
                    }

                    if (entity.IsSupplier)
                    {
                        ContactInType _currentcInTypeSupplier = cInTypeService.Query().Filter(x => x.ContactID == entity.ContactID && x.ContactType == ContactType.Supplier).Get().FirstOrDefault();

                        ContactInType _contactInType = new ContactInType
                        {
                            ContactID = entity.ContactID,
                            CusContactName = entity.CusContactName,
                            CusContactMobile = entity.CusContactMobile,
                            CusContactEmail = entity.CusContactEmail,
                            ContactType = ContactType.Supplier
                        };

                        if (_currentcInTypeSupplier == null)
                        {
                            cInTypeService.Add(_contactInType);
                        }
                        else
                        {
                            cInTypeService.Modify(_contactInType);
                        }
                    }
                    else
                    {
                        ContactInType _cInType = cInTypeService.Query().Filter(x => x.ContactID == entity.ContactID && x.ContactType == ContactType.Supplier).Get().FirstOrDefault();
                        if (_cInType != null)
                        {
                            cInTypeService.Remove(_cInType);
                        }
                    }

                    if (entity.IsCustomer)
                    {
                        ContactInType _currentcInTypeCustomer = cInTypeService.Query().Filter(x => x.ContactID == entity.ContactID && x.ContactType == ContactType.Customer).Get().FirstOrDefault();

                        ContactInType _contactInType = new ContactInType
                        {
                            ContactID = entity.ContactID,
                            CusContactName = entity.CusContactName,
                            CusContactMobile = entity.CusContactMobile,
                            CusContactEmail = entity.CusContactEmail,
                            ContactType = ContactType.Customer
                        };

                        if (_currentcInTypeCustomer == null)
                        {
                            cInTypeService.Add(_contactInType);
                        }
                        else
                        {
                            cInTypeService.Modify(_contactInType);
                        }
                    }
                    else
                    {
                        ContactInType _cInType = cInTypeService.Query().Filter(x => x.ContactID == entity.ContactID && x.ContactType == ContactType.Customer).Get().FirstOrDefault();
                        if (_cInType != null)
                        {
                            cInTypeService.Remove(_cInType);
                        }
                    }

                    List<CustomerOfProductOwner> _cusOfOwner = customerOfOwnerService.Query().Filter(x => x.CustomerID == entity.ContactID).Get().ToList();
                    _cusOfOwner.ToList().ForEach(item =>
                    {
                        customerOfOwnerService.Remove(item);
                    });

                    entity.ProductOwnerCollection.ToList().ForEach(item =>
                    {
                        CustomerOfProductOwner _customerOfProductOwner = new CustomerOfProductOwner
                        {
                            ProductOwnerID = item.ProductOwnerID,
                            CustomerID = entity.ContactID
                        };
                        customerOfOwnerService.Add(_customerOfProductOwner);
                    });

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
                    Contact _current = FindByID(id);

                    if (_current == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    List<ContactInType> _conInType = cInTypeService.Query().Filter(x => x.ContactID == _current.ContactID).Get().ToList();
                    _conInType.ToList().ForEach(item =>
                    {
                        cInTypeService.Modify(item);
                    });

                    List<ContactAddress> _conAddress = cAddressService.Query().Filter(x => x.ContactID == _current.ContactID).Get().ToList();
                    _conAddress.ToList().ForEach(item =>
                    {
                        item.IsActive = false;
                        item.DateModified = DateTime.Now;
                        item.UserModified = UserID;
                        cAddressService.Modify(item);
                    });

                    List<CustomerOfProductOwner> _cusOfOwn = customerOfOwnerService.Query().Filter(x => x.CustomerID == _current.ContactID).Get().ToList();
                    _cusOfOwn.ToList().ForEach(item =>
                    {
                        customerOfOwnerService.Modify(item);
                    });

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
        public List<SubDistrict> GetSubDistrict(Guid? districtId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<SubDistrict> result = SubDistrictService.Query()
                                                .Filter(x => (districtId != null ? x.District_Id == districtId.Value : true) && (x.Name.Contains(keyword)
                                                || x.NameEN.Contains(keyword)
                                                || x.PostCode.Contains(keyword))).Get();

                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
        public List<District> GetDistrict(Guid? provinceId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<District> result = DistrictService.Query()
                                            .Filter(x => (provinceId != null ? x.Province_Id == provinceId.Value : true) && (x.Name.Contains(keyword)
                                             || x.NameEN.Contains(keyword)
                                             || x.PostCode.Contains(keyword))).Get();


                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
        public List<Province> GetProvince(Guid? provinceId, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<Province> result = ProvinceService.Query()
                                  .Filter(x => (provinceId != null ? x.Province_Id == provinceId.Value : true) && (x.Name.Contains(keyword)
                                          || x.NameEN.Contains(keyword)
                                          || x.Province_Code.Contains(keyword))).Get();


                totalRecords = result.Count();
                if (pageIndex != null && (pageSize != null || pageSize != 0))
                {
                    result = result.OrderBy(x => x.Name).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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
