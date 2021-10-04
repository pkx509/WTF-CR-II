using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public class MonthEndService : Repository<Monthend>, IMonthEndService
    {
        public MonthEndService(IUnitOfWork context) : base(context)
        {
        }
        public bool CanUsed(DateTime dateRef)
        {
            return !Any(e => e.Month == dateRef.Month && e.Year == dateRef.Year && e.IsActive);// && e.CutOffDate < dateRef);
        }

        public bool CheckCutoffDate(DateTime dateref)
        {
            if (dateref >= DateTime.Now) return true;
            Monthend lastCutoff = GetLastMonthend();            
            if (lastCutoff == null || lastCutoff.CutOffDate == DateTime.MinValue)
            {
                /*  lastCutoff = new Monthend()
                  {
                      CutOffDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), 23, 59, 59),
                       Month = DateTime.Today.Month,
                        Year = DateTime.Today.Year
                  };*/

                if (lastCutoff == null)
                {
                    var minCutoff = new Monthend()
                    {
                        CutOffDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), 23, 59, 59),
                        Month = DateTime.Today.Month,
                        Year = DateTime.Today.Year
                    };
                    if (minCutoff.CutOffDate.Date == new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)))
                    {
                        return true;
                    }
                    else
                    {
                        if (dateref.Date >= DateTime.Today)
                        {
                            return true;
                        }
                        else
                        {
                            throw new HILIException("MEND004");
                        }
                    }
                }
            }
            if (lastCutoff == null || dateref > lastCutoff.CutOffDate)
            {
                return true;
            }
            else
            {
                throw new HILIException("MEND004");
            }
            //if (lastCutoff == null)
            //{
            //    var minCutoff = Where(e => e.IsActive && e.CutOffDate < DateTime.Now).OrderByDescending(e => e.CutOffDate).FirstOrDefault();
            //    if (minCutoff == null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        if (dateref.Date == DateTime.Today)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            throw new HILIException("MEND004");
            //        }
            //    }
            //}

            //if (lastCutoff.CutOffDate >= dateref)// && lastCutoff.CutOffDate >= DateTime.Now)
            //{
            //    return true;
            //}
            //if(dateref >= DateTime.Now)
            //{
            //    return true;
            //}
            //throw new HILIException("MEND004");
            /*

            if (lastCutoff == null || lastCutoff.CutOffDate == DateTime.MinValue)
            {
                if (dateref < DateTime.Now)
                {
                    throw new HILIException("MEND004");
                }
            }
            if (lastCutoff == null || dateref > lastCutoff.CutOffDate)
            {
                return true;
            }
            else
            {
                throw new HILIException("MEND004");
            }
            //return true;
            Monthend lastCutoff = GetLastMonthend(dateref);
            if (lastCutoff == null || lastCutoff.CutOffDate == DateTime.MinValue)
            {
                if (dateref < DateTime.Now)
                {
                    throw new HILIException("MEND004");
                }
            }

            if (lastCutoff==null || dateref > lastCutoff.CutOffDate)
            {
                return true;
            }
            else
            {
                throw new HILIException("MEND004");
            }*/
            //
        }
        public Monthend CreateOrUpdate(Monthend entity)
        {
            DateTime currentDate = DateTime.Now;
            //if (entity.CutOffDate < currentDate)
            //{
            //    throw new HILIException("MEND001");
            //}

            Monthend dateMonthend = FirstOrDefault(e => e.Month == entity.CutOffDate.Month && e.Year == entity.CutOffDate.Year && e.IsActive);
            if (dateMonthend != null && entity.CutOffDate < currentDate)
            {
                throw new HILIException("MEND002");
            }

            if (dateMonthend == null)
            {
                dateMonthend = new Monthend()
                {
                    DateModified = currentDate,
                    DateCreated = currentDate,
                    CutOffDate = entity.CutOffDate,
                    IsActive = true,
                    Month = entity.CutOffDate.Month,
                    Remark = "",
                    UserCreated = UserID,
                    UserModified = UserID,
                    Year = entity.CutOffDate.Year
                };
                Add(dateMonthend);
            }
            else
            {
                dateMonthend.DateModified = currentDate;
                dateMonthend.UserModified = UserID;
                dateMonthend.CutOffDate = entity.CutOffDate;
                Modify(dateMonthend);
            }
            return dateMonthend;
        }
        public List<Monthend> GetAll(out int totalRecord)
        {
            return GetAll(out totalRecord, null, null);
        }
        public List<Monthend> GetAll(Guid monthendId)
        {
            return Where(e => e.IsActive && e.ID == monthendId).ToList();
        }
        public List<Monthend> GetAll(out int totalRecord, int? pageIndex, int? pageSize)
        {
            IQueryable<Monthend> results = Where(e => e.IsActive);
            totalRecord = results.Count();
            if (pageIndex != null && pageSize != null)
            {
                results = results.OrderBy(x => x.CutOffDate).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return results.ToList();
        }
        private Monthend GetLastMonthend()//(DateTime dateRef)
        {
            DateTime currentDate = DateTime.Now;
            //var monthends = Where(e => e.IsActive && e.Month == currentDate.Month && e.Year == currentDate.Year).OrderByDescending(e => e.CutOffDate).ToList();
            //return monthends.FirstOrDefault();
            List<Monthend> dateMonthend = Where(e => (e.CutOffDate <= currentDate) && e.IsActive)
                                .OrderByDescending(e => e.CutOffDate).ToList();
            return dateMonthend.FirstOrDefault();
        }
               
        public bool Active(Monthend entity)
        {           
            Monthend dateMonthend = FirstOrDefault(e => e.ID == entity.ID);
            if (dateMonthend == null)
            {
                throw new HILIException("MEND002");
            }

            if (entity.CutOffDate < DateTime.Now)
            {
                throw new HILIException("MEND002");
            }

            dateMonthend.IsActive = true;
            dateMonthend.DateModified = DateTime.Now;
            dateMonthend.UserModified = UserID;
            Modify(dateMonthend);
            return true;

        }

        public bool InActive(Monthend entity)
        {
            Monthend dateMonthend = FirstOrDefault(e => e.ID == entity.ID);
            if (dateMonthend == null)
            {
                throw new HILIException("MEND002");
            }

            if (entity.CutOffDate < DateTime.Now)
            {
                throw new HILIException("MEND002");
            }

            dateMonthend.IsActive = false;
            dateMonthend.DateModified = DateTime.Now;
            dateMonthend.UserModified = UserID;
            Modify(dateMonthend);
            return true;
        }

        public bool Remove(Monthend entity)
        {
            Monthend dateMonthend = FirstOrDefault(e => e.ID == entity.ID && e.IsActive);
            if (dateMonthend == null)
            {
                throw new HILIException("MEND002");
            }

          /*  if (entity.CutOffDate < DateTime.Now)
            {
                throw new HILIException("MEND002");
            }
            */
            dateMonthend.IsActive = false;
            dateMonthend.DateModified = DateTime.Now;
            dateMonthend.UserModified = UserID;
            Modify(dateMonthend);
            return true;
        }
    }
}

