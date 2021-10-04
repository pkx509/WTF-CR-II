using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public class ProductStatusService : Repository<ProductStatus>, IProductStatusService
    {
        #region [ Property ]

        private readonly IRepository<ProductStatusMapDocument> productStatusMapDocumentService;

        #endregion

        #region Constructor

        public ProductStatusService(IUnitOfWork context, IRepository<ProductStatusMapDocument> _ProductStatusMapDocument)
            : base(context)
        {
            productStatusMapDocumentService = _ProductStatusMapDocument;
        }
        #endregion

        #region Method

        public ProductStatus Get(Guid id)
        {
            try
            {
                ProductStatus _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.ProductStatusID == id)
                                  .Include(x => x.ProductStatusMapCollection.Select(s => s.ProductSubStatus))
                                  .Include(x => x.ProductStatusMapDocumentCollection.Select(s => s.DocumentType))
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

        public List<ProductStatus> Get(string keyword, Guid? documentTypeID, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                //IQueryable<ProductStatus> result = null;

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<ProductStatus> result = Query().Filter(x => x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.Description.Contains(keyword)).Get();

                if (documentTypeID != null)
                {
                    result = from pStatus in result
                             join mapDoc in productStatusMapDocumentService.Query().Get() on pStatus.ProductStatusID equals mapDoc.ProductStatusID
                             where mapDoc.DocumentTypeID == documentTypeID
                             select pStatus;
                }

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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

        public List<ProductStatus> GetByDocuemtnType(Guid documentTypeId)
        {
            try
            {
                List<ProductStatus> result = Query().Include(x => x.ProductStatusMapCollection)
                                    .Filter(x => x.ProductStatusMapDocumentCollection.Any(s => s.DocumentTypeID == documentTypeId))
                                    .Include(x => x.ProductStatusMapCollection.Select(s => s.ProductSubStatus)).Get().ToList();
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

        #endregion
    }
}
