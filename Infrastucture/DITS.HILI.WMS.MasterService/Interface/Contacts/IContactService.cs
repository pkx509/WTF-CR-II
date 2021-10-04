using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public interface IContactService : IRepository<Contact>
    {
        Contact Get(Guid id);
        List<Contact> Get(ContactType contactType, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<Contact> GetAll(ContactType contactType, Guid? contactID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Contact> GetCustomer(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Contact> GetSupplier(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<SubDistrict> GetSubDistrict(Guid? districtId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<District> GetDistrict(Guid? provinceId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Province> GetProvince(Guid? provinceId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
