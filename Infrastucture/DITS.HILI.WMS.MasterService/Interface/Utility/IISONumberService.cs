using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IISONumberService : IRepository<ISONumber>
    {
        ISONumber GetById(Guid id);
        List<ISONumber> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        ISONumber GetByDocument(string documentname);
        //void ModifyISONumber(ISONumber entity);
        ISONumber GetIsoByID(Guid IsoId);
        List<ISONumber> GetISONumber(Guid? IsoId, string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize);
        void AddISONumber(ISONumber entity);
        void ModifyISONumber(ISONumber entity);
        void RemoveISONumber(Guid id);
    }
}
