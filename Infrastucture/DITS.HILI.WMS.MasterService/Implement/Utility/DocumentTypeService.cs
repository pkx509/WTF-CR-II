using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public class DocumentTypeService : Repository<DocumentType>, IDocumentTypeService
    {
        private readonly IRepository<ItfInterfaceMapping> _ItfInterfaceMappingService;
        private readonly IRepository<ProductStatusMapDocument> _ProductStatusMapDocService;
        private readonly IRepository<ProductStatus> _ProductStatusService;

        #region Constructor

        public DocumentTypeService(IUnitOfWork context)
            : base(context)
        {
            _ItfInterfaceMappingService = context.Repository<ItfInterfaceMapping>();
            _ProductStatusMapDocService = context.Repository<ProductStatusMapDocument>();
            _ProductStatusService = context.Repository<ProductStatus>();
        }

        #endregion

        #region Method

        public DocumentType Get(Guid id)
        {
            try
            {
                DocumentType _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                _current = Query().Filter(x => x.DocumentTypeID == id)
                                  .Include(x => x.ProductStatusMapDocumentCollection)
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

        public List<DocumentType> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<DocumentType> result = Query().Filter(x => x.Code.Contains(keyword)
                                                    || x.Name.Contains(keyword)
                                                    || x.Description.Contains(keyword)).Get();

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

        public List<DocumentType> GetByDocTypeEnum(DocumentTypeEnum docType, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<DocumentType> result = from doctype in Query().Filter(x => x.DocType == docType && (x.Name.Contains(keyword))).Get()
                                                   join docmap in _ItfInterfaceMappingService.Query().Filter(x => x.IsActive && x.IsDisplay == true).Get()
                                                   on doctype.DocumentTypeID equals docmap.DocumentId
                                                   select new DocumentType
                                                   {
                                                       Code = doctype.Code,
                                                       Name = doctype.Name,
                                                       DocumentTypeID = doctype.DocumentTypeID,
                                                       FullName = doctype.Code + ':' + doctype.Name,
                                                   };


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
        public List<DocumentType> GetByDocTypeEnumWithAll(DocumentTypeEnum docType, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {

                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                IEnumerable<DocumentType> result = (from doctype in Query().Filter(x => x.DocType == docType && (x.Name.Contains(keyword))).Get()
                                                    select new DocumentType
                                                    {
                                                        Code = doctype.Code,
                                                        Name = doctype.Name,
                                                        DocumentTypeID = doctype.DocumentTypeID,
                                                        FullName = doctype.Code + ':' + doctype.Name,
                                                    });


                DocumentType _all = new DocumentType { Code = null, Name = null, DocumentTypeID = Guid.Empty, FullName = "ทั้งหมด" };
                // result.ToList().Insert(0, _all);
                //result.Union(_all);

                totalRecords = result.Count();
                if (pageIndex != null && pageSize != null)
                {
                    result = result.OrderBy(x => x.Code).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }


                List<DocumentType> _temp = result.ToList();

                _temp.Insert(0, _all);


                return _temp;
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

        public List<DocumentType> GetReceiveType(DocumentTypeEnum docType, string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword);
                //var result = Query().Filter(x => x.DocType == docType && (x.Name.Contains(keyword))).Get();

                IEnumerable<DocumentType> result = from dt in Query().Filter(x => x.IsActive).Get()
                                                   join itf in _ItfInterfaceMappingService.Query().Filter(x => x.IsActive).Get() on dt.DocumentTypeID equals itf.DocumentId
                                                   where dt.DocType == docType && dt.Name.Contains(keyword) && itf.IsNormal == true
                                                   select dt;


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

        public List<DocumentTypeCustomModel> GetForInternalReceive(string keyword, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;
                IEnumerable<ItfInterfaceMapping> itfMapping = _ItfInterfaceMappingService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ProductStatusMapDocument> statusMapDoc = _ProductStatusMapDocService.Query().Filter(x => x.IsActive).Get();
                IEnumerable<ProductStatus> pStatus = _ProductStatusService.Query().Filter(x => x.IsActive).Get();

                IEnumerable<DocumentTypeCustomModel> result = from dt in Query().Filter(x => x.IsActive).Get()
                                                              join itf in itfMapping on dt.DocumentTypeID equals itf.DocumentId
                                                              where dt.DocType == DocumentTypeEnum.Receive
                                                                       && (keyword != null ? dt.Name.Contains(keyword) : true)
                                                                       && (itf.IsDisplay == true)
                                                                       && (itf.FromReprocess == true
                                                                               || itf.ToReprocess == true
                                                                               || itf.ReferenceDocumentID != null
                                                                               || itf.IsNormal != true)
                                                              select new DocumentTypeCustomModel()
                                                              {
                                                                  DocumentTypeID = dt.DocumentTypeID,
                                                                  Name = dt.Name,
                                                                  DocType = dt.DocType,
                                                                  RefDocumentID = itf.ReferenceDocumentID,
                                                                  IsCreditNote = itf.IsCreditNote ?? false,
                                                                  IsNormal = itf.IsNormal ?? false,
                                                                  FromReprocess = itf.FromReprocess ?? false,
                                                                  ToReprocess = itf.ToReprocess ?? false,
                                                                  IsItemChange = itf.IsItemChange ?? false,
                                                                  IsWithoutGoods = itf.IsWithoutGoods ?? false,
                                                                  ProductStatus = (from smd in statusMapDoc
                                                                                   join ps in pStatus on smd.ProductStatusID equals ps.ProductStatusID into _ps
                                                                                   from ps in _ps.DefaultIfEmpty()
                                                                                   where smd.DocumentTypeID == dt.DocumentTypeID && smd.IsDefault
                                                                                   select ps).FirstOrDefault()
                                                              };

                if (result == null || result.Count() == 0)
                {
                    return new List<DocumentTypeCustomModel>();
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
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }

        public List<DocumentType> GetRefDispatchType(Guid documentID, out int totalRecords, int? pageIndex, int? pageSize)
        {
            try
            {
                totalRecords = 0;

                IEnumerable<DocumentType> result = from dt in Query().Filter(x => x.IsActive).Get()
                                                   where dt.DocType == DocumentTypeEnum.Dispatch
                                                            && dt.DocumentTypeID == documentID
                                                   select dt;

                if (result == null || result.Count() == 0)
                {
                    return new List<DocumentType>();
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
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
        #endregion
    }
}
