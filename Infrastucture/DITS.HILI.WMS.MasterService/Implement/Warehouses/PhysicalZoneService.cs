using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public class PhysicalZoneService : Repository<PhysicalZone>, IPhysicalZoneService
    {
        #region Property 
        private readonly IRepository<Warehouse> warehouseService;
        #endregion

        #region Constructor

        public PhysicalZoneService(IUnitOfWork dbContext,
                                IRepository<Warehouse> _warehouse)
            : base(dbContext)
        {
            warehouseService = _warehouse;
        }

        public PhysicalZone Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<PhysicalZone> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            throw new NotImplementedException();
        }

        public List<PhysicalZone> GetPhysicalCombo(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);

                var result = (from _physicalzone in Query().Get().Where(w => w.IsActive == true)
                              join _warehousename in warehouseService.Query().Get() on _physicalzone.Warehouse_Code equals _warehousename.Code
                              select new { _physicalzone, _warehousename });


                totalRecords = result.Count();
                //if (pageIndex != 0 || pageSize != 0)
                //{
                //    result = result.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                //}



                List<PhysicalZone> resultsmodel = result.Select(n => new PhysicalZone
                {
                    PhysicalZone_Code = n._physicalzone.PhysicalZone_Code,
                    PhysicalZone_Name = n._physicalzone.PhysicalZone_Name,
                    Warehouse_Code = n._warehousename.Code,
                    Warehouse_Name = n._warehousename.Name,
                    Physicalzone_Id = n._physicalzone.Physicalzone_Id
                }).ToList();


                List<PhysicalZone> results = new List<PhysicalZone>();
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    results = resultsmodel.Where(x => x.PhysicalZone_Code.Contains(keyword)
                                              || x.PhysicalZone_Name.Contains(keyword)
                                              || x.Warehouse_Code.Contains(keyword)
                                              || x.Warehouse_Name.Contains(keyword)).ToList();
                }
                else
                {
                    results = resultsmodel;
                }

                return results;

            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public void AddPhysicalZone(PhysicalZone entity)
        {
            throw new NotImplementedException();
        }

        public void ModifyPhysicalZone(PhysicalZone entity)
        {
            throw new NotImplementedException();
        }

        public void RemovePhysicalZone(Guid entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Method

        #endregion
    }
}
