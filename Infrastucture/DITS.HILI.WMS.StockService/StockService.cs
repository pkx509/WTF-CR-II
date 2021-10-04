using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Stock;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DITS.HILI.WMS.StockService
{
    public class StockService : Repository<StockInfo>, IStockService
    {

        private readonly IRepository<StockBalance> stockBalanceService;
        private readonly IRepository<StockTransaction> stockTransService;
        public StockService(IUnitOfWork context, IRepository<StockBalance> _stockBalance,
                                                 IRepository<StockTransaction> _stockTrans)
            : base(context)
        {
            stockBalanceService = _stockBalance;
            stockTransService = _stockTrans;
        }

        public void Incomming(List<DataTransfer> dataTrans)
        {
            try
            {

                var stock = buildStockInfo(dataTrans);

                stock.ForEach(stockInfo =>
                {
                    var balance = buildStockBalance(stockInfo, dataTrans);

                    var _stockInfo = getStockInfo(stockInfo);
                    if (_stockInfo == null) //New Stock
                    {
                        balance.ForEach(item =>
                        {
                            StockTransaction trans = new StockTransaction
                            {
                                StockBalanceID = item.StockBalanceID,
                                StockTransactionType = StockTransactionTypeEnum.Incomming,
                                LocationID = item.LocationID,
                                DocumentID = dataTrans.FirstOrDefault().ReferenceBaseID,
                                PackageID = dataTrans.FirstOrDefault().InstanceID,
                                PalletCode = item.PalletCode,
                                ConversionQty = item.ConversionQty,
                                BaseQuantity = item.BaseQuantity,
                                UserCreated = this.UserID,
                                UserModified = this.UserID,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                IsActive = true
                            };
                            item.StockTransactionCollection.Add(trans);
                            stockInfo.StockBalanceCollection.Add(item);
                        });
                        
                        base.Add(stockInfo);
                    }
                    else // Update balance
                    {
                        balance.ForEach(item =>
                        {
                            var _balance = _stockInfo.StockBalanceCollection.FirstOrDefault(x => x.StockInfoID == item.StockInfoID && x.PalletCode == item.PalletCode && x.LocationID == item.LocationID && x.ConversionQty == item.ConversionQty);
                            if (_balance != null)
                            {
                                _balance.BaseQuantity += item.BaseQuantity;
                                StockTransaction trans = new StockTransaction
                                {
                                    StockBalanceID = _balance.StockBalanceID,
                                    StockTransactionType = StockTransactionTypeEnum.Incomming,
                                    LocationID = item.LocationID,
                                    DocumentID = dataTrans.FirstOrDefault().ReferenceBaseID,
                                    PackageID = dataTrans.FirstOrDefault().InstanceID,
                                    PalletCode = item.PalletCode,
                                    ConversionQty = item.ConversionQty,
                                    BaseQuantity = item.BaseQuantity,
                                    UserCreated = this.UserID,
                                    UserModified = this.UserID
                                };
                                stockBalanceService.Modify(_balance);
                                stockTransService.Add(trans);
                            }
                            else
                            {
                                _balance.StockInfoID = _stockInfo.StockInfoID;
                                stockBalanceService.Add(_balance);
                                StockTransaction trans = new StockTransaction
                                {
                                    StockBalanceID = _balance.StockBalanceID,
                                    StockTransactionType = StockTransactionTypeEnum.Incomming,
                                    LocationID = item.LocationID,
                                    DocumentID = dataTrans.FirstOrDefault().ReferenceBaseID,
                                    PackageID = dataTrans.FirstOrDefault().InstanceID,
                                    PalletCode = item.PalletCode,
                                    ConversionQty = item.ConversionQty,
                                    BaseQuantity = item.BaseQuantity,
                                    UserCreated = this.UserID,
                                    UserModified = this.UserID
                                };
                                stockTransService.Add(trans);
                            }
                        });
                    }
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Outgoing(List<DataTransfer> dataTrans)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var stock = buildStockInfo(dataTrans);
                    stock.ForEach(stockInfo =>
                    {
                        var balance = buildStockBalance(stockInfo, dataTrans);
                        var _stockInfo = getStockInfo(stockInfo);
                        if (_stockInfo == null)
                            throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.PB10001, "Stock Information"));

                        balance.ForEach(item =>
                        {
                            var _balance = _stockInfo.StockBalanceCollection.FirstOrDefault(x => x.StockInfoID == item.StockInfoID && x.PalletCode == item.PalletCode && x.LocationID == item.LocationID && x.ConversionQty == item.ConversionQty);
                            if (_balance != null)
                            {

                                decimal availableQty = (_balance.BaseQuantity - _balance.ReserveQuantity);

                                if (item.BaseQuantity > availableQty)
                                    throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.STK10001, "Stock Reserve"));

                                if ((item.BaseQuantity - availableQty) <= 0)
                                    throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.STK10001, "Stock Reserve"));

                                _balance.BaseQuantity -= item.BaseQuantity;
                                _balance.ReserveQuantity -= item.BaseQuantity;
                                stockBalanceService.Modify(_balance);

                                StockTransaction trans = new StockTransaction
                                {
                                    StockBalanceID = _balance.StockBalanceID,
                                    StockTransactionType = StockTransactionTypeEnum.Outgoing,
                                    LocationID = item.LocationID,
                                    DocumentID = dataTrans.FirstOrDefault().ReferenceBaseID,
                                    PackageID = dataTrans.FirstOrDefault().InstanceID,
                                    PalletCode = item.PalletCode,
                                    ConversionQty = item.ConversionQty,
                                    BaseQuantity = item.BaseQuantity,
                                    UserCreated = this.UserID,
                                    UserModified = this.UserID
                                };
                                stockTransService.Add(trans);
                            }
                        });
                    });

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Reserve(List<DataTransfer> dataTrans)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var stock = buildStockInfo(dataTrans);
                    stock.ForEach(stockInfo =>
                    {
                        var balance = buildStockBalance(stockInfo, dataTrans);
                        var _stockInfo = getStockInfo(stockInfo);
                        if (_stockInfo == null)
                            throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.PB10001, "Stock Information"));

                        balance.ForEach(item =>
                        {
                            var _balance = _stockInfo.StockBalanceCollection.FirstOrDefault(x => x.StockInfoID == item.StockInfoID && x.PalletCode == item.PalletCode && x.LocationID == item.LocationID && x.ConversionQty == item.ConversionQty);
                            if (_balance != null)
                            {

                                decimal availableQty = (_balance.BaseQuantity - _balance.ReserveQuantity);

                                if (item.BaseQuantity > availableQty)
                                    throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.STK10001, "Stock Reserve"));

                                if ((item.BaseQuantity - availableQty) <= 0)
                                    throw new Exception(Message.MessageManger.GetMessage(Message.GlobalLanguage.STK10001, "Stock Reserve"));

                                _balance.ReserveQuantity += item.BaseQuantity;
                                stockBalanceService.Modify(_balance);

                                StockTransaction trans = new StockTransaction
                                {
                                    StockBalanceID = _balance.StockBalanceID,
                                    StockTransactionType = StockTransactionTypeEnum.Reserver,
                                    LocationID = item.LocationID,
                                    DocumentID = dataTrans.FirstOrDefault().ReferenceBaseID,
                                    PackageID = dataTrans.FirstOrDefault().InstanceID,
                                    PalletCode = item.PalletCode,
                                    ConversionQty = item.ConversionQty,
                                    BaseQuantity = item.BaseQuantity,
                                    UserCreated = this.UserID,
                                    UserModified = this.UserID
                                };
                                stockTransService.Add(trans);
                            }
                        });
                    });

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private StockInfo getStockInfo(StockInfo stockInfo)
        {
            try
            {

                var stock = Query().Filter(x => x.IsActive &&
                                                x.ExpirationDate == stockInfo.ExpirationDate &&
                                                x.ManufacturingDate == stockInfo.ManufacturingDate &&
                                                x.ProductID == stockInfo.ProductID &&
                                                x.Lot == stockInfo.Lot &&
                                                x.ProductWidth == stockInfo.ProductWidth &&
                                                x.ProductLength == stockInfo.ProductLength &&
                                                x.ProductHeight == stockInfo.ProductHeight &&
                                                x.ProductWeight == stockInfo.ProductWeight &&
                                                x.PackageWeight == stockInfo.PackageWeight &&
                                                x.Price == stockInfo.Price &&
                                                x.UnitPriceID == stockInfo.UnitPriceID &&
                                                x.ProductOwnerID == stockInfo.ProductOwnerID &&
                                                x.StockUnitID == stockInfo.StockUnitID &&
                                                x.BaseUnitID == stockInfo.BaseUnitID &&
                                                x.ConversionQty == stockInfo.ConversionQty &&
                                                x.ProductStatusID == stockInfo.ProductStatusID &&
                                                x.ProductSubStatusID == stockInfo.ProductSubStatusID &&
                                                x.SupplierID == stockInfo.SupplierID)
                                    .Include(x => x.StockBalanceCollection)
                                    .Get().FirstOrDefault();
                return stock;
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

        private List<StockInfo> buildStockInfo(List<DataTransfer> dataTrans)
        {
            var stock = dataTrans.GroupBy(g => new
            {
                ExpirationDate = g.ExpirationDate,
                ManufacturingDate = g.ManufacturingDate,
                ProductID = g.ProductID,
                Lot = g.Lot,
                ProductWidth = g.ProductWidth,
                ProductLength = g.ProductLength,
                ProductHeight = g.ProductHeight,
                ProductWeight = g.ProductWeight,
                PackageWeight = g.PackageWeight,
                Price = g.Price,
                UnitPriceID = g.ProductUnitPriceID,
                ProductOwnerID = g.ProductOwnerID.Value,
                StockUnitID = g.StockUnitID,
                BaseUnitID = g.BaseUnitID,
                ConversionQty = g.ConversionQty,
                ProductStatusID = g.ProductStatusID,
                ProductSubStatusID = g.ProductSubStatusID,
                SupplierID = g.SupplierID.Value
            }).Select(n => new StockInfo
            {
                ExpirationDate = n.Key.ExpirationDate,
                ManufacturingDate = n.Key.ManufacturingDate,
                ProductID = n.Key.ProductID,
                Lot = n.Key.Lot,
                ProductWidth = n.Key.ProductWidth,
                ProductLength = n.Key.ProductLength,
                ProductHeight = n.Key.ProductHeight,
                ProductWeight = n.Key.ProductWeight,
                PackageWeight = n.Key.PackageWeight,
                Price = n.Key.Price,
                UnitPriceID = n.Key.UnitPriceID,
                ProductOwnerID = n.Key.ProductOwnerID,
                StockUnitID = n.Key.StockUnitID,
                BaseUnitID = n.Key.BaseUnitID,
                ConversionQty = n.Key.ConversionQty,
                ProductStatusID = n.Key.ProductStatusID,
                ProductSubStatusID = n.Key.ProductSubStatusID,
                SupplierID = n.Key.SupplierID,
                UserCreated = this.UserID,
                UserModified = this.UserID
            }).ToList();

            return stock;
        }

        private List<StockBalance> buildStockBalance(StockInfo stockInfo, List<DataTransfer> dataTrans)
        {
            var balance = dataTrans.Where(x => x.ExpirationDate == stockInfo.ExpirationDate &&
                                                     x.ManufacturingDate == stockInfo.ManufacturingDate &&
                                                     x.ProductID == stockInfo.ProductID &&
                                                     x.Lot == stockInfo.Lot &&
                                                     x.ProductWidth == stockInfo.ProductWidth &&
                                                     x.ProductLength == stockInfo.ProductLength &&
                                                     x.ProductHeight == stockInfo.ProductHeight &&
                                                     x.ProductWeight == stockInfo.ProductWeight &&
                                                     x.PackageWeight == stockInfo.PackageWeight &&
                                                     x.ProductUnitPriceID == stockInfo.UnitPriceID &&
                                                     x.Price == stockInfo.Price &&
                                                     x.ProductOwnerID == stockInfo.ProductOwnerID &&
                                                     x.StockUnitID == stockInfo.StockUnitID &&
                                                     x.BaseUnitID == stockInfo.BaseUnitID &&
                                                     x.ConversionQty == stockInfo.ConversionQty &&
                                                     x.ProductStatusID == stockInfo.ProductStatusID &&
                                                     x.ProductSubStatusID == stockInfo.ProductSubStatusID &&
                                                     x.SupplierID == stockInfo.SupplierID)
                                                    .GroupBy(group => new
                                                    {
                                                        group.ProductID,
                                                        group.Lot,
                                                        group.PalletCode,
                                                        group.LocationID,
                                                        group.ConversionQty,
                                                        group.BaseUnitID,
                                                        group.StockUnitID
                                                    }).Select(s => new StockBalance
                                                    {
                                                        PalletCode = s.Key.PalletCode,
                                                        LocationID = s.Key.LocationID.Value,
                                                        ConversionQty = s.Key.ConversionQty,
                                                        ReserveQuantity = 0,
                                                        BaseQuantity = s.Sum(x => x.BaseQuantity)
                                                    }).ToList();
            return balance;
        }

    }
}
