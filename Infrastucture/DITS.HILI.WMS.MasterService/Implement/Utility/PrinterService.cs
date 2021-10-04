using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Transactions;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public class PrinterService : Repository<Printer>, IPrinterService

    {
        #region Property 
        private readonly IRepository<Line> LineService;
        #endregion

        #region Constructor

        public PrinterService(IUnitOfWork dbContext,
            IRepository<Line> _lineplan)
            : base(dbContext)
        {
            LineService = _lineplan;
        }

        #endregion

        #region Method

        public Printer GetById(Guid id)
        {
            try
            {
                Printer _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.PrinterId == id).Get().FirstOrDefault();

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

        public List<PrinterModel> Get(Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                List<BasePrinter> _basePrinter = getBasePrinter();
                List<PrinterModel> _printer = new List<PrinterModel>();
                PrinterModel printer = null;

                Query().Filter(x => x.IsActive
                                 && (lineID != null ? x.PrinterLocationId == lineID : true)).Get().ToList().ForEach(n =>
                    {
                        printer = new PrinterModel
                        {
                            PrinterId = n.PrinterId,
                            PrinterLocation = n.PrinterLocation,

                            Location_Name = (LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().ToList().Count == 0 ? null :
                                                LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().FirstOrDefault().LineCode),
                            Location_Loading = (LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().ToList().Count == 0 ? null :
                                                LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().FirstOrDefault().LineCode),
                            PrinterName = n.PrinterName,
                            Description = n.Description,
                            IsDefault = n.IsDefault,
                            IsDriverInstall = true
                        };
                        printer.IsDriverInstall = _basePrinter.Any(x => x.PrinterName.ToLower() == n.PrinterName.ToLower());
                        printer.IsOnLine = ((_basePrinter.FirstOrDefault(x => x.PrinterName == n.PrinterName) == null ? false :
                                        _basePrinter.FirstOrDefault(x => x.PrinterName == n.PrinterName).OnLine));
                        _printer.Add(printer);
                    });

                totalRecords = _printer.Count();

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                if (pageIndex != null && pageSize != null)
                {
                    _printer = _printer.Where(x => x.PrinterName.Contains(keyword) || x.PrinterLocation.Contains(keyword) || x.Location_Loading.Contains(keyword) || x.Location_Name.Contains(keyword) || x.Description.Contains(keyword)).OrderBy(x => x.PrinterName).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
                }

                return _printer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<BasePrinter> GetPrinterMachine(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                List<BasePrinter> _printer = new List<BasePrinter>();
                getBasePrinter().Where(x => x.PrinterName.ToLower().Contains(keyword.ToLower())).ToList().ForEach(item =>
                  {
                      PrinterModel _p = GetPrinter(item.PrinterName);
                      //if (_p == null)
                      _printer.Add(item);
                  });

                totalRecords = _printer.Count();


                return _printer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<Line> GetPrinterLocation(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        //{

        //}
        //public List<Line> GetPrinterLocation(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        //{
        //    keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
        //    var result = LineService.Query().Filter(x => (x.LineCode.Contains(keyword)))
        //                        .OrderBy(x => x.OrderBy(s => s.LineCode)).Get(out totalRecords, pageIndex, pageSize);

        //    totalRecords = result.Count();

        //    if (pageIndex != null && pageSize != null)
        //    {
        //        result = result.OrderBy(x => x.LineCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
        //    }
        //    return result.ToList();
        //}

        private List<BasePrinter> getBasePrinter()
        {
            List<BasePrinter> _printer = new List<BasePrinter>();
            ManagementObjectSearcher printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

            foreach (ManagementBaseObject printer in printerQuery.Get())
            {
                _printer.Add(new BasePrinter
                {
                    PrinterName = printer.GetPropertyValue("Name").ToString(),
                    OnLine = (int.Parse(printer.GetPropertyValue("ExtendedPrinterStatus").ToString()) == 7 ? false : true),
                    IsDefault = Convert.ToBoolean(printer.GetPropertyValue("Default"))
                });
            }

            return _printer;
        }

        public PrinterModel GetPrinter(string printerName)
        {
            try
            {
                return getPrinters(printerName, true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PrinterModel> getPrinters(string PrinterName, bool IsActive)
        {
            try
            {
                List<BasePrinter> _basePrinter = getBasePrinter();
                List<PrinterModel> _printer = new List<PrinterModel>();
                PrinterModel printer = null;

                Query().Filter(x => x.IsActive && x.PrinterName == PrinterName).Get().ToList().ForEach(n =>
                     {
                         printer = new PrinterModel
                         {
                             PrinterId = n.PrinterId,
                             PrinterLocation = n.PrinterLocation,

                             Location_Name = (LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().ToList().Count == 0 ? null :
                                                 LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().FirstOrDefault().LineCode),
                             Location_Loading = (LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().ToList().Count == 0 ? null :
                                                 LineService.Query().Filter(x => x.LineCode == n.PrinterLocation).Get().FirstOrDefault().LineCode),
                             PrinterName = n.PrinterName,
                             Description = n.Description,
                             IsDefault = n.IsDefault,
                             IsDriverInstall = true
                         };
                         printer.IsDriverInstall = _basePrinter.Any(x => x.PrinterName.ToLower() == n.PrinterName.ToLower());
                         printer.IsOnLine = ((_basePrinter.FirstOrDefault(x => x.PrinterName == n.PrinterName) == null ? false :
                                         _basePrinter.FirstOrDefault(x => x.PrinterName == n.PrinterName).OnLine));
                         _printer.Add(printer);
                     });
                return _printer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddPrinter(Printer entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity == null)
                    {
                        throw new HILIException("MSG00006");
                    }

                    bool ok = Query().Get().Any(x => x.IsActive && x.PrinterName.Replace(" ", string.Empty).ToLower().Equals(entity.PrinterName.Replace(" ", string.Empty).ToLower()) && x.PrinterLocation.Replace(" ", string.Empty).ToLower().Equals(entity.PrinterLocation.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    //if (entity.EquipName.IndexOf(" ") > -1)
                    //    throw new HILIException("MSG00010");

                    entity.DateCreated = DateTime.Now;
                    entity.DateModified = DateTime.Now;

                    Printer result = base.Add(entity);

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

        public void ModifyPrinter(Printer entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    bool ok = Query().Get().Any(x => x.PrinterId != entity.PrinterId && x.PrinterName.Replace(" ", string.Empty).ToLower().Equals(entity.PrinterName.Replace(" ", string.Empty).ToLower()) && x.PrinterLocation.Replace(" ", string.Empty).ToLower().Equals(entity.PrinterLocation.Replace(" ", string.Empty).ToLower()));

                    if (ok)
                    {
                        throw new HILIException("MSG00009");
                    }

                    Printer _current = Query().Filter(x => x.PrinterId == entity.PrinterId).Get().FirstOrDefault();

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

        public void RemovePrinter(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Printer _current = FindByID(id);

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

        public List<Line> GetPrinterLocation(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                IEnumerable<Line> result = LineService.Query().Filter(x => keyword != null ? (x.LineCode.ToLower().Contains(keyword.ToLower())) : true)
                                    .OrderBy(x => x.OrderBy(s => s.LineCode)).Get(out totalRecords, pageIndex, pageSize);

                totalRecords = result.Count();

                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.LineCode).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                return result.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
