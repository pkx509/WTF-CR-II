namespace DITS.HILI.WMS.DailyPlanModel
{

    public class ValidationImportFileResult
    {
        public string Code { get; set; }
        public string TypeOfFile { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public string Message { get; set; }
        public bool IsWarning { get; set; }
        public string Field { get; set; }
    }
}
