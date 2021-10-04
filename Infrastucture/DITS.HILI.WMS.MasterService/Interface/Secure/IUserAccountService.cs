using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IUserAccountService : IRepository<UserAccounts>
    {
        void ChangePassword(UserAccounts entity, string newPwd);
        bool Login(string username, string password);
        UserAccounts GetUserByName(string username);
        UserAccounts Get(Guid id);
        List<UserAccounts> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void Delete(Guid id);

        List<UserInRole> GetRoles(Guid userid);
        void SaveRoles(List<UserInRole> entity);

        List<UserInGroup> GetGroups(Guid userid);
        void SaveGroups(List<UserInGroup> entity);
    }
}
