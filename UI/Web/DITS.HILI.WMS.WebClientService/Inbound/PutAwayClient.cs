namespace DITS.HILI.WMS.ClientService.Inbound
{
    public class PutAwayClient
    {
        private static readonly string prefix = "putaway/";

        //public async static Task<List<PutAwayItem>> GetInitialItem(string productCode, string productName, string lot, Guid? locationId, Guid? warehouseId)
        //{
        //    return await HttpService.Get<List<PutAwayItem>>(prefix + "getinitialitem?productCode=" + productCode + "&productName=" + productName + "&lot=" + lot + "&locationId=" + locationId + "&warehouseId=" + warehouseId, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<List<PutAway>> GetJobPutawayList(PutAwayStatusEnum? status, string keyword, Ref<int> total, int? pageIndex, int? pageSize)
        //{
        //    return await HttpService.Get<List<PutAway>>(prefix + "getjobputawaylist?status=" + status + "&keyword=" + keyword, total, pageIndex, pageSize, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<List<CustomEnumerable>> GetPutawayStatus()
        //{
        //    return await HttpService.Get<List<CustomEnumerable>>(prefix + "getputawaystatus", Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<List<PutAwayReason>> GetPutawayReason()
        //{
        //    return await HttpService.Get<List<PutAwayReason>>(prefix + "getputawayreason", Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<List<PutAway>> GetJobPutaway(string putawayNo)
        //{
        //    return await HttpService.Get<List<PutAway>>(prefix + "getjobputaway?putAwayNo=" + putawayNo, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<bool> CreateJobPutaway(List<PutAwayItem> putawayItem)
        //{
        //    return await HttpService.Put(prefix + "createjobputaway", putawayItem, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<bool> Modify(JobPutAway jobPutaway)
        //{
        //    return await HttpService.Put(prefix + "modify", jobPutaway, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public async static Task<bool> ConfirmPutAway(PutAwayConfirm putawayConfirm)
        //{
        //    return await HttpService.Put(prefix + "confirmputaway", putawayConfirm, Common.User.UserID, Common.Language, Common.AccessToken);
        //}

        //public static async Task<bool> Cancel(string jobputAwayCode)
        //{
        //    var result = await HttpService.Put(prefix + "cancel?jobputAwayCode=" + jobputAwayCode, new object(), Common.User.UserID, Common.Language, Common.AccessToken);
        //    return result;
        //}
        //public async static Task<PutAway> GetJobPutawayByID(Guid PutAwayID)
        //{
        //    return await HttpService.Get<PutAway>(prefix + "getjobputawaybyid?putawayId=" + PutAwayID, Common.User.UserID, Common.Language, Common.AccessToken);
        //}
    }
}
