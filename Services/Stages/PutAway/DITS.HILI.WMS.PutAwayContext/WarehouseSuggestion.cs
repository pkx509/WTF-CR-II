namespace DITS.HILI.WMS.PutAwaySuggestMethod
{
    //public class WarehouseSuggestion : ISuggestionMethod
    //{
    //    //public List<PutAwayJob> Suggestion(List<PutAwayItem> Items)
    //    //{
    //    //    try
    //    //    {
    //    //        List<PutAwayJob> jobItem = new List<PutAwayJob>();

    //    //        Items.GroupBy(g => new
    //    //        {
    //    //            g.Location.Zone.WarehouseID
    //    //        }).Select(n => new
    //    //        {
    //    //            n.Key.WarehouseID
    //    //        }).ToList()
    //    //        .ForEach(item =>
    //    //        {
    //    //            PutAwayJob newJob = new PutAwayJob();
    //    //            newJob.GroupID = Guid.NewGuid();
    //    //            newJob.Warehouse = item.WarehouseID;
    //    //            newJob.Item = Items.Where(x => x.Location.Zone.WarehouseID == item.WarehouseID)
    //    //            .Select(n => new PutAwayItem
    //    //            {
    //    //                PutAwayItemID = n.PutAwayItemID,
    //    //                StockBalanceID = n.StockBalanceID, 
    //    //                ProductOwnerID = n.ProductOwnerID,
    //    //                SupplierID = n.SupplierID,
    //    //                ProductID = n.ProductID,
    //    //                Lot = n.Lot,
    //    //                PalletCode = n.PalletCode,
    //    //                ManufacturingDate = n.ManufacturingDate,
    //    //                ExpirationDate = n.ExpirationDate,
    //    //                ProductHeight = n.ProductHeight,
    //    //                ProductLength = n.ProductLength,
    //    //                ProductWidth = n.ProductWidth,
    //    //                ProductWeight = n.ProductWeight,
    //    //                PackageWeight = n.PackageWeight,
    //    //                ProductStatusID = n.ProductStatusID,
    //    //                ProductSubStatusID = n.ProductSubStatusID,
    //    //                Price = n.Price,
    //    //                ProductUnitPriceID = n.ProductUnitPriceID,                       
    //    //                LocationID = n.LocationID,
    //    //                BaseUnitID = n.BaseUnitID,
    //    //                StockUnitID = n.StockUnitID,
    //    //                Location = n.Location,
    //    //                Product = n.Product,
    //    //                ConversionQty = n.ConversionQty,
    //    //                BaseQuantity = n.BaseQuantity,
    //    //                Quantity = n.Quantity 
    //    //            }).ToList();
    //    //            jobItem.Add(newJob);
    //    //        });

    //    //        return jobItem;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        throw ex;
    //    //    }
    //    //}
    //}
}
