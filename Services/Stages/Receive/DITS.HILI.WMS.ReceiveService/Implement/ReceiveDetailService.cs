using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Data.Entity.Validation;
using System.Transactions;

namespace DITS.HILI.WMS.ReceiveService
{
    public class ReceiveDetailService : Repository<ReceiveDetail>, IReceiveDetailService
    {
        public ReceiveDetailService(IUnitOfWork context) : base(context)
        {
        }

        public bool Cancel(Guid id)
        {
            try
            {
                ReceiveDetail _current = FindByID(id);
                if (_current == null)
                {
                    throw new HILIException("MSG00006");
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    if (_current.ReceiveDetailStatus != ReceiveDetailStatusEnum.New)
                    {
                        throw new HILIException("MSG00023");
                    }

                    _current.ReceiveDetailStatus = ReceiveDetailStatusEnum.Cancel;
                    _current.IsActive = false;
                    _current.DateModified = DateTime.Now;
                    _current.UserModified = UserID;
                    base.Modify(_current);

                    scope.Complete();
                }

                return true;
            }
            catch (HILIException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                throw Framework.ExceptionHelper.ExceptionMessage(ex);
            }
        }
    }
}
