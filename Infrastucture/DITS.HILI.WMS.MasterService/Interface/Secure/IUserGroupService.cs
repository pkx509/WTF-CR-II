using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IUserGroupService : IRepository<UserGroups>
    {
        void AddUserGroup(UserGroups entity);
        void DeleteUserGroup(Guid id);
        UserGroups Get(Guid id);
        void ModifyUserGroup(UserGroups entity);
        List<UserGroups> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProgramInGroup> GetProgram(Guid groupID, string langCode);
        void SaveProgram(List<ProgramInGroup> entity);
    }
}
