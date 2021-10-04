namespace DITS.HILI.WMS.PutAwaySuggestMethod
{
    //public class ProductSuggestion : ISuggestionMethod
    //{

    //    //public List<PutAwayJob> Suggestion(List<PutAwayItem> Items)
    //    //{
    //    //    try
    //    //    {
    //    //        List<PutAwayJob> jobItem = new List<PutAwayJob>();

    //    //        Items.GroupBy(g => new
    //    //        {
    //    //            g.Location.Zone.WarehouseID,
    //    //            g.Location.ZoneID,
    //    //            g.ProductID
    //    //        }).Select(n => new
    //    //        {
    //    //            n.Key.WarehouseID,
    //    //            n.Key.ZoneID,
    //    //            n.Key.ProductID
    //    //        }).ToList()

    //    //        .ForEach(item =>
    //    //        {
    //    //            PutAwayJob newJob = new PutAwayJob();
    //    //            newJob.GroupID = Guid.NewGuid();
    //    //            newJob.Warehouse = item.WarehouseID;
    //    //            newJob.Zone = item.ZoneID;
    //    //            newJob.Product = item.ProductID;
    //    //            newJob.Item = Items.Where(x => x.Location.Zone.WarehouseID == item.WarehouseID && x.Location.ZoneID == item.ZoneID && x.ProductID == item.ProductID)
    //    //            .Select(n => new PutAwayItem
    //    //            {
    //    //                PutAwayItemID = n.PutAwayItemID,
    //    //                ProductOwnerID = n.ProductOwnerID,
    //    //                SupplierID = n.SupplierID,
    //    //                StockBalanceID = n.StockBalanceID, 
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
