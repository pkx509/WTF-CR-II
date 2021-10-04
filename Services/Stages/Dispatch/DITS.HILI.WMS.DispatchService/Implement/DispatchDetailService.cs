using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Reflection;

namespace DITS.HILI.WMS.DispatchService
{
    public class DispatchDetailService : Repository<DispatchDetail>, IDispatchDetailService
    {
        private readonly IRepository<DispatchBooking> _DispatchBookingService;

        public DispatchDetailService(IUnitOfWork context) : base(context)
        {
            _DispatchBookingService = context.Repository<DispatchBooking>();
        }

        public bool AddList(List<DispatchDetailCustom> dispatchDetails)
        {
            try
            {
                foreach (DispatchDetailCustom item in dispatchDetails)
                {
                    DispatchDetail dispatchDetailModel = new DispatchDetail
                    {
                        DispatchId = item.DispatchID,
                        RuleId = item.RuleID,
                        DispatchDetailStatus = (DispatchDetailStatusEnum)item.DispatchDetailStatus,
                        DispatchDetailProductWidth = 0,
                        DispatchDetailProductLength = 0,
                        DispatchDetailProductHeight = 0,
                        DispatchPrice = 0,
                        IsBackOrder = false,

                        Sequence = item.Sequence,
                        ProductId = item.ProductId,
                        StockUnitId = item.StockUnitId,
                        Quantity = item.Quantity,
                        BaseQuantity = item.BaseQuantity,
                        BaseUnitId = item.BaseUnitId,
                        ConversionQty = item.ConversionQty,
                        ProductOwnerId = item.ProductOwnerId,
                        ProductStatusId = item.ProductStatusId,
                        ProductSubStatusId = item.ProductSubStatusId,

                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        Remark = item.Remark
                    };

                    base.Add(dispatchDetailModel);

                    _DispatchBookingService.Add(new DispatchBooking()
                    {
                        DispatchDetailId = dispatchDetailModel.DispatchDetailId,

                        Sequence = item.Sequence,
                        ProductId = item.ProductId,
                        RequestQty = item.Quantity ?? 0,
                        RequestStockUnitId = item.StockUnitId,
                        RequestBaseQty = item.BaseQuantity,
                        RequestBaseUnitId = item.BaseUnitId,
                        BookingQty = item.Quantity ?? 0,
                        BookingStockUnitId = item.StockUnitId,
                        BookingBaseQty = item.BaseQuantity,
                        BookingBaseUnitId = item.BaseUnitId,

                        IsBackOrder = false,
                        BookingStatus = BookingStatusEnum.InternalReceive,
                        PalletCode = item.PalletCode,
                        LocationId = item.LocationId,
                        ProductLot = item.ProductLot,
                        ConversionQty = item.ConversionQty,
                        Mfgdate = item.Mfgdate,
                        ExpirationDate = item.ExpirationDate,

                        IsActive = true,
                        UserCreated = UserID,
                        DateCreated = DateTime.Now,
                        UserModified = UserID,
                        DateModified = DateTime.Now,
                        Remark = item.Remark
                    });

                }

                return true;
            }
            catch (HILIException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Warning, MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Error, MethodBase.GetCurrentMethod().Name, ex);
                throw ExceptionHelper.ExceptionMessage(ex);
            }
        }
    }
}
